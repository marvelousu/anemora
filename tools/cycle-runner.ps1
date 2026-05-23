#requires -Version 5.1
<#
.SYNOPSIS
    Cycle runner: orchestrate one parent-driven implementation cycle to commit.

.DESCRIPTION
    One cycle = one commit. Invoked by the parent (orchestrator) session after the cycle-worker sub-agent
    finishes editing and returns its WORKER_RESULT with the Validate/Capture method names.

    Phases:
      1. Validate -- batch tool invokes $ValidateMethod
      2. Capture  -- batch tool invokes $CaptureMethod (produces review artifacts: PNG/metrics/logs)
      3. Build    -- batch tool invokes $BuildMethod (produces a runnable artifact, e.g. a player .exe)
      4. Smoke    -- runs the built artifact for $SmokeSeconds, then scans the log for failure patterns

    On any phase failure: git reset --hard HEAD, append the failure log tail to the cycle devlog, exit non-zero.
    On all phases pass: derive commit subject from the cycle devlog title line, git add + commit + push.

    Defaults target Unity Editor + Windows player builds (the reference implementation). The script is
    parameterized so non-Unity projects can swap in their own batch tool via -BatchTool / -BatchArgsTemplate.
    Pass -SkipBuild (or omit -BuildMethod) when the project has no compile-and-run smoke phase.

.PARAMETER CycleNumber
    Cycle ordinal (e.g. 1, 25). Used for log filenames and the commit body.

.PARAMETER ValidateMethod
    Fully-qualified callable for the validate batch. Returned by the worker in WORKER_RESULT.

.PARAMETER CaptureMethod
    Fully-qualified callable for the capture batch (produces review PNGs, metrics, or other observables).
    Returned by the worker in WORKER_RESULT.

.PARAMETER DevlogPath
    Absolute or repo-relative path to the cycle devlog .md. First H1 line becomes the commit subject.

.PARAMETER BuildMethod
    Fully-qualified callable for the build batch (produces a runnable artifact). Optional --
    leave empty to skip the build and smoke phases.

.PARAMETER Audience
    Capture-output prefix routing. Pass 'worker' to surface review-ready artifacts for the worker session,
    or 'parent_review' (default) for the parent's own review pass.
    Exported as env CYCLE_AUDIENCE so capture methods can honor it; the runner also renames untagged
    artifacts in the capture output dir to add the prefix if the method did not.

.PARAMETER CaptureOutputDir
    Directory the runner scans for newly-produced capture artifacts (PNG by default) so it can apply
    the audience prefix. Defaults to <ProjectPath>/docs/devlog/screenshots.

.PARAMETER CaptureFilter
    File glob the runner renames within CaptureOutputDir. Defaults to '*.png'.

.PARAMETER BuildExe
    Optional explicit path to the built runnable. If omitted, the runner parses the build log for
    $BuildArtifactPattern and falls back to the most recently modified file under <ProjectPath>/Builds/.

.PARAMETER BuildArtifactPattern
    Regex applied to the build log to extract the path of the produced runnable. The first capture group
    is the path. Default matches Unity's "player built: <path>.exe" log line.

.PARAMETER ProjectPath
    Project root. Defaults to the repo root resolved from this script's location ($PSScriptRoot\..).

.PARAMETER BatchTool
    Path to the batch executable. Default is Unity.exe (override via env CYCLE_BATCH_TOOL or -BatchTool).

.PARAMETER BatchArgsTemplate
    Argument template for the batch tool. Supports placeholders {projectPath}, {method}, {logFile}.
    Default matches Unity Editor's -executeMethod invocation.

.PARAMETER SmokeArgsTemplate
    Argument template for running the built artifact during smoke. Supports placeholders {logFile}.
    Default matches a Unity player's -batchmode -nographics -logFile invocation.

.PARAMETER SmokeSeconds
    Smoke runtime in seconds. Default 20.

.PARAMETER SmokePatterns
    Regex (pipe-separated alternation) of failure patterns to grep the smoke log for. Match count must be 0.

.PARAMETER SkipBuild
    Skip the build and smoke phases entirely. Use when the project has no compile step worth smoking.

.PARAMETER SkipPush
    Commit locally but do not push.

.PARAMETER CommitPath
    Optional explicit path allowlist to stage before commit. When omitted, the legacy behavior
    is `git add -A`. Paths may be repo-relative or absolute. Use this for Unity projects where
    batch validation dirties scenes or ProjectSettings that are not authored side effects.

.PARAMETER NoRollback
    Do not run git reset --hard HEAD when a phase fails. The failure tail is still appended
    to the devlog and the script exits non-zero. Use this when the caller must preserve a
    dirty worktree for manual inspection.

.PARAMETER DryRun
    Print the resolved plan and exit. No batch invocation, no git mutation.

.EXAMPLE
    # Unity Editor reference cycle (drop-in for Unity projects)
    pwsh -File tools/cycle-runner.ps1 `
        -CycleNumber 2 `
        -ValidateMethod Project.EditorTools.SetupClass.ValidateTopicBatch `
        -CaptureMethod  Project.EditorTools.SetupClass.CaptureTopicCycle02ScreenshotsBatch `
        -BuildMethod    Project.EditorTools.SetupClass.BuildAndValidateBatch `
        -DevlogPath     docs/devlog/2026-05-23_topic_cycle02.md `
        -Audience       parent_review

.EXAMPLE
    # Non-Unity project (e.g. a Node.js codegen pipeline)
    pwsh -File tools/cycle-runner.ps1 `
        -CycleNumber 7 `
        -ValidateMethod scripts/validate.js `
        -CaptureMethod  scripts/capture.js `
        -DevlogPath     docs/devlog/cycle07.md `
        -BatchTool      node `
        -BatchArgsTemplate '{method} --project "{projectPath}" --log "{logFile}"' `
        -SkipBuild
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)][int]$CycleNumber,
    [Parameter(Mandatory = $true)][string]$ValidateMethod,
    [Parameter(Mandatory = $true)][string]$CaptureMethod,
    [Parameter(Mandatory = $true)][string]$DevlogPath,
    [string]$BuildMethod = '',
    [ValidateSet('worker', 'parent_review')][string]$Audience = 'parent_review',
    [string]$CaptureOutputDir = '',
    [string]$CaptureFilter = '*.png',
    [string]$BuildExe = '',
    [string]$BuildArtifactPattern = 'player built:\s*(.+\.exe)',
    [string]$ProjectPath = '',
    [string]$BatchTool = '',
    [string]$BatchArgsTemplate = '-batchmode -quit -projectPath "{projectPath}" -executeMethod {method} -logFile "{logFile}"',
    [string]$SmokeArgsTemplate = '-batchmode -nographics -logFile "{logFile}"',
    [int]$SmokeSeconds = 20,
    [string]$SmokePatterns = 'Error|Exception|Assert|NullReference|Font Atlas Texture|DrawObjectsPass|RenderGraph',
    [string[]]$CommitPath = @(),
    [switch]$SkipBuild,
    [switch]$SkipPush,
    [switch]$NoRollback,
    [switch]$DryRun
)

# PS 5.1 native exe stderr workaround: leave $ErrorActionPreference at Continue so that
# NativeCommandError-wrapped stderr does not trip a Stop terminator.
$ErrorActionPreference = 'Continue'
$PSDefaultParameterValues['Out-File:Encoding'] = 'utf8'

# ---------------------------------------------------------------------------
# Path resolution
# ---------------------------------------------------------------------------

if (-not $ProjectPath) {
    $ProjectPath = Resolve-Path (Join-Path $PSScriptRoot '..') | Select-Object -ExpandProperty Path
}
if (-not (Test-Path $ProjectPath)) {
    throw "ProjectPath not found: $ProjectPath"
}

if (-not $BatchTool) {
    if ($env:CYCLE_BATCH_TOOL) { $BatchTool = $env:CYCLE_BATCH_TOOL }
    else { $BatchTool = 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' }
}
if (-not (Test-Path $BatchTool)) {
    throw "BatchTool not found at: $BatchTool (override with -BatchTool or `$env:CYCLE_BATCH_TOOL)"
}

$DevlogResolved = $DevlogPath
if (-not [System.IO.Path]::IsPathRooted($DevlogResolved)) {
    $DevlogResolved = Join-Path $ProjectPath $DevlogPath
}
if (-not (Test-Path $DevlogResolved)) {
    throw "Devlog not found: $DevlogResolved"
}

if (-not $CaptureOutputDir) {
    $CaptureOutputDir = Join-Path $ProjectPath 'docs/devlog/screenshots'
}

$ToolsDir = Join-Path $ProjectPath 'tools'
$LogsDir = Join-Path $ToolsDir 'logs'
$BatchLogsDir = Join-Path $ProjectPath 'Logs'
if (-not (Test-Path $LogsDir)) { New-Item -ItemType Directory -Path $LogsDir | Out-Null }
if (-not (Test-Path $BatchLogsDir)) { New-Item -ItemType Directory -Path $BatchLogsDir | Out-Null }

$Stamp = (Get-Date).ToString('yyyyMMdd-HHmmss')
$CycleTag = "cycle-{0:D2}-{1}" -f $CycleNumber, $Stamp
$RunLog = Join-Path $LogsDir "$CycleTag.log"

$RunBuildSmoke = (-not $SkipBuild) -and ($BuildMethod -ne '')

# ---------------------------------------------------------------------------
# Logging helpers
# ---------------------------------------------------------------------------

function Write-Run {
    param([string]$Message)
    $line = "[{0}] {1}" -f (Get-Date).ToString('HH:mm:ss'), $Message
    Write-Host $line
    Add-Content -Path $RunLog -Value $line -Encoding utf8
}

function Append-File {
    param([string]$SrcPath, [string]$Header)
    Add-Content -Path $RunLog -Value "`n===== $Header =====" -Encoding utf8
    if (Test-Path $SrcPath) {
        Get-Content -Path $SrcPath -Encoding utf8 | Add-Content -Path $RunLog -Encoding utf8
    } else {
        Add-Content -Path $RunLog -Value "(file not found: $SrcPath)" -Encoding utf8
    }
}

function Expand-Template {
    param([string]$Template, [hashtable]$Vars)
    $out = $Template
    foreach ($k in $Vars.Keys) {
        $out = $out.Replace('{' + $k + '}', $Vars[$k])
    }
    return $out
}

# ---------------------------------------------------------------------------
# Plan summary
# ---------------------------------------------------------------------------

Write-Run "Cycle runner starting"
Write-Run "  CycleNumber    : $CycleNumber"
Write-Run "  ProjectPath    : $ProjectPath"
Write-Run "  BatchTool      : $BatchTool"
Write-Run "  ValidateMethod : $ValidateMethod"
Write-Run "  CaptureMethod  : $CaptureMethod"
if ($RunBuildSmoke) {
    Write-Run "  BuildMethod    : $BuildMethod"
} else {
    Write-Run "  BuildMethod    : (skipped)"
}
Write-Run "  Audience       : $Audience"
Write-Run "  CaptureOutDir  : $CaptureOutputDir"
Write-Run "  DevlogPath     : $DevlogResolved"
Write-Run "  SmokeSeconds   : $SmokeSeconds"
Write-Run "  SmokePatterns  : $SmokePatterns"
Write-Run "  CommitPath     : $($CommitPath -join '; ')"
Write-Run "  NoRollback     : $NoRollback"
Write-Run "  RunLog         : $RunLog"

if ($DryRun) {
    Write-Run "DryRun set; exiting without invoking batch tool or git."
    exit 0
}

# ---------------------------------------------------------------------------
# Phase invocation helpers
# ---------------------------------------------------------------------------

$env:CYCLE_AUDIENCE = $Audience
$env:CYCLE_NUMBER = "$CycleNumber"

function Invoke-Batch {
    param(
        [string]$PhaseName,
        [string]$Method,
        [string]$LogFile
    )
    Write-Run "Phase '$PhaseName' begin: $Method"
    $argString = Expand-Template -Template $BatchArgsTemplate -Vars @{
        projectPath = $ProjectPath
        method      = $Method
        logFile     = $LogFile
    }
    # Split expanded args while preserving double-quoted segments. The template is an
    # argument string, not a command line with an executable prefix, so do not drop
    # the first token.
    $argv = [regex]::Matches($argString, '("[^"]*"|\S+)') |
        ForEach-Object { $_.Value.Trim('"') }
    $proc = Start-Process -FilePath $BatchTool -ArgumentList $argv -PassThru -Wait -WindowStyle Hidden
    $exit = $proc.ExitCode
    Append-File -SrcPath $LogFile -Header "$PhaseName batch log ($LogFile)"
    if ($exit -ne 0) {
        Write-Run "Phase '$PhaseName' FAILED with exit $exit"
        return $false
    }
    Write-Run "Phase '$PhaseName' passed"
    return $true
}

function Rollback-AndReport {
    param([string]$PhaseName)
    if ($NoRollback) {
        Write-Run "NoRollback set; preserving worktree after $PhaseName failure"
    } else {
        Write-Run "Rolling back via 'git reset --hard HEAD' due to $PhaseName failure"
        Push-Location $ProjectPath
        try {
            git reset --hard HEAD | Out-Null
        } finally {
            Pop-Location
        }
    }
    $tail = ""
    if (Test-Path $RunLog) {
        $tail = (Get-Content $RunLog -Tail 80) -join "`n"
    }
    $failureText = @(
        "",
        "## Cycle $CycleNumber failure ($PhaseName) -- $Stamp",
        "",
        '```',
        $tail,
        '```'
    ) -join "`n"
    Add-Content -Path $DevlogResolved -Value $failureText -Encoding utf8
    Write-Run "Failure tail appended to devlog: $DevlogResolved"
}

# ---------------------------------------------------------------------------
# Phase 1: validate
# ---------------------------------------------------------------------------

$ValidateLog = Join-Path $BatchLogsDir "$CycleTag-validate.log"
if (-not (Invoke-Batch -PhaseName 'validate' -Method $ValidateMethod -LogFile $ValidateLog)) {
    Rollback-AndReport -PhaseName 'validate'
    exit 1
}

# ---------------------------------------------------------------------------
# Phase 2: capture
# ---------------------------------------------------------------------------

$CaptureLog = Join-Path $BatchLogsDir "$CycleTag-capture.log"
if (-not (Invoke-Batch -PhaseName 'capture' -Method $CaptureMethod -LogFile $CaptureLog)) {
    Rollback-AndReport -PhaseName 'capture'
    exit 1
}

# Audience prefix enforcement: if the capture method ignored CYCLE_AUDIENCE,
# prepend the prefix here to keep worker_/parent_review_ partitioning consistent.
if (Test-Path $CaptureOutputDir) {
    Get-ChildItem -Path $CaptureOutputDir -Recurse -Filter $CaptureFilter -File `
        | Where-Object { $_.LastWriteTime -gt (Get-Date).AddMinutes(-5) } `
        | Where-Object { $_.Name -notmatch '^(worker_|parent_review_)' } `
        | ForEach-Object {
            $newName = "{0}_{1}" -f $Audience, $_.Name
            $newPath = Join-Path $_.DirectoryName $newName
            if (Test-Path $newPath) { Remove-Item $newPath -Force }
            Move-Item -Path $_.FullName -Destination $newPath
            Write-Run "Renamed capture artifact for audience '$Audience': $($_.Name) -> $newName"
        }
}

# ---------------------------------------------------------------------------
# Phase 3: build
# ---------------------------------------------------------------------------

if ($RunBuildSmoke) {
    $BuildLog = Join-Path $BatchLogsDir "$CycleTag-build.log"
    if (-not (Invoke-Batch -PhaseName 'build' -Method $BuildMethod -LogFile $BuildLog)) {
        Rollback-AndReport -PhaseName 'build'
        exit 1
    }

    # Resolve the built artifact path.
    if (-not $BuildExe) {
        $match = Select-String -Path $BuildLog -Pattern $BuildArtifactPattern -AllMatches `
            | Select-Object -Last 1
        if ($match) {
            $BuildExe = $match.Matches[0].Groups[1].Value.Trim()
        }
    }
    if (-not $BuildExe -or -not (Test-Path $BuildExe)) {
        $BuildsDir = Join-Path $ProjectPath 'Builds'
        if (Test-Path $BuildsDir) {
            $candidate = Get-ChildItem -Path $BuildsDir -Recurse -Filter '*.exe' -File `
                | Sort-Object LastWriteTime -Descending `
                | Select-Object -First 1
            if ($candidate) { $BuildExe = $candidate.FullName }
        }
    }
    if (-not $BuildExe -or -not (Test-Path $BuildExe)) {
        Write-Run "Could not resolve built artifact (build log pattern + Builds/ scan both failed)"
        Rollback-AndReport -PhaseName 'build-artifact-resolve'
        exit 1
    }
    Write-Run "Resolved built artifact: $BuildExe"

    # ---------------------------------------------------------------------------
    # Phase 4: smoke
    # ---------------------------------------------------------------------------

    $SmokeLog = Join-Path $BatchLogsDir "$CycleTag-smoke.log"
    Write-Run "Phase 'smoke' begin: $BuildExe (run for $SmokeSeconds s)"

    $smokeArgString = Expand-Template -Template $SmokeArgsTemplate -Vars @{
        logFile = $SmokeLog
    }
    $smokeArgv = $smokeArgString -split ' '

    $proc = Start-Process -FilePath $BuildExe `
        -ArgumentList $smokeArgv `
        -PassThru -WindowStyle Hidden

    Start-Sleep -Seconds $SmokeSeconds

    if (-not $proc.HasExited) {
        try { Stop-Process -Id $proc.Id -Force } catch {}
    }

    Append-File -SrcPath $SmokeLog -Header "smoke log ($SmokeLog)"

    # Note: do not name this $matches -- that collides with the $Matches automatic variable.
    $smokeHits = @()
    if (Test-Path $SmokeLog) {
        $smokeHits = @(Select-String -Path $SmokeLog -Pattern $SmokePatterns)
    }
    if ($smokeHits.Count -gt 0) {
        Write-Run "Phase 'smoke' FAILED: $($smokeHits.Count) pattern hits"
        foreach ($m in ($smokeHits | Select-Object -First 20)) {
            Write-Run "  smoke hit: $($m.Line.Trim())"
        }
        Rollback-AndReport -PhaseName 'smoke'
        exit 1
    }
    Write-Run "Phase 'smoke' passed: 0 pattern hits"
} else {
    Write-Run "Build + smoke phases skipped (-SkipBuild set or no -BuildMethod)"
}

# ---------------------------------------------------------------------------
# Commit + push
# ---------------------------------------------------------------------------

$titleLine = (Get-Content $DevlogResolved -TotalCount 5 -Encoding utf8 `
    | Where-Object { $_ -match '^# ' } `
    | Select-Object -First 1)
if (-not $titleLine) {
    Write-Run "Could not find an H1 title line in devlog; aborting commit"
    exit 1
}
$commitSubject = ($titleLine -replace '^#\s+', '').Trim()

Push-Location $ProjectPath
try {
    $devlogRel = (Resolve-Path -Relative $DevlogResolved -ErrorAction SilentlyContinue)
    if (-not $devlogRel) { $devlogRel = $DevlogResolved }
    $commitBody = "Cycle $CycleNumber. Devlog: $devlogRel"

    # Write the commit message to a temp file and use -F to avoid PS native-arg quoting pitfalls.
    $msgFile = New-TemporaryFile
    Set-Content -Path $msgFile -Value "$commitSubject" -Encoding utf8
    Add-Content -Path $msgFile -Value "" -Encoding utf8
    Add-Content -Path $msgFile -Value "$commitBody" -Encoding utf8

    Write-Run "Committing: $commitSubject"
    if ($CommitPath -and $CommitPath.Count -gt 0) {
        foreach ($path in $CommitPath) {
            $stagePath = $path
            if ([System.IO.Path]::IsPathRooted($stagePath)) {
                $resolvedStagePath = Resolve-Path -LiteralPath $stagePath -ErrorAction SilentlyContinue
                if ($resolvedStagePath) {
                    $stagePath = Resolve-Path -Relative $resolvedStagePath.Path
                }
            }
            Write-Run "  staging path: $stagePath"
            git add -- "$stagePath"
            if ($LASTEXITCODE -ne 0) {
                Write-Run "git add failed for $stagePath (exit $LASTEXITCODE)"
                exit 1
            }
        }
    } else {
        git add -A
    }
    git commit -F $msgFile.FullName
    $commitExit = $LASTEXITCODE
    Remove-Item $msgFile -ErrorAction SilentlyContinue
    if ($commitExit -ne 0) {
        Write-Run "git commit failed (exit $commitExit)"
        exit 1
    }
    if (-not $SkipPush) {
        Write-Run "Pushing to origin"
        git push
        if ($LASTEXITCODE -ne 0) {
            Write-Run "git push failed (exit $LASTEXITCODE) -- commit is local; resolve and retry manually"
            exit 1
        }
    } else {
        Write-Run "SkipPush set; commit stayed local"
    }
} finally {
    Pop-Location
}

Write-Run "Cycle $CycleNumber completed cleanly"
exit 0
