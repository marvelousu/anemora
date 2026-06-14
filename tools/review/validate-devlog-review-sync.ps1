#requires -Version 5.1
<#
.SYNOPSIS
    Guard Anemora devlog/review-image discipline for local hooks and CI.

.DESCRIPTION
    This script catches the project-global failure modes that are easy to miss
    during long visual cycles:

    - implementation/workflow commits without a matching docs/devlog entry
    - new devlog files that were not added to docs/devlog/INDEX.md
    - recent local docs/review/<cycle>/ folders without devlog.txt or images
    - recent local docs/review/<cycle>/ folders missing from the public R2 manifest

    CI mode validates only tracked git history. Local hook mode also validates
    recent ignored review-image cycle directories and their public R2 propagation.
#>
[CmdletBinding()]
param(
    [string]$BaseRef = '',
    [switch]$Ci,
    [int]$RecentReviewDays = 1,
    [string]$R2PublicBase = 'https://pub-d14764d639a647339a6b0d81de923abf.r2.dev',
    [switch]$SkipR2ManifestCheck
)

# PS 5.1 can wrap native stderr as NativeCommandError even when we inspect
# $LASTEXITCODE ourselves. Keep native command handling manual.
$ErrorActionPreference = 'Continue'

function Invoke-Git {
    param([Parameter(ValueFromRemainingArguments = $true)][string[]]$Args)
    $out = & git @Args 2>$null
    if ($LASTEXITCODE -ne 0) {
        throw "git $($Args -join ' ') failed"
    }
    return $out
}

function Normalize-Path {
    param([string]$Path)
    return ($Path -replace '\\', '/').Trim()
}

function Get-FirstDevlogLine {
    param([string]$Path)
    if (-not (Test-Path -LiteralPath $Path)) { return $null }
    return Get-Content -LiteralPath $Path |
        ForEach-Object { $_.Trim() } |
        Where-Object { $_ -and -not $_.StartsWith('#') } |
        Select-Object -First 1
}

function Add-Failure {
    param([string]$Message)
    $script:Failures += $Message
}

function Convert-BranchToR2Slug {
    param([string]$Branch)
    return (($Branch -replace '^work/', '') -replace '[^a-zA-Z0-9._-]+', '-').Trim('-')
}

function Get-R2ManifestPathSet {
    param(
        [string]$Slug,
        [string]$BaseUrl
    )
    $base = $BaseUrl.TrimEnd('/')
    $uri = "$base/manifests/$Slug.json?guard=review-sync"
    try {
        $response = Invoke-WebRequest -Uri $uri -UseBasicParsing -TimeoutSec 20
        if ($response.StatusCode -lt 200 -or $response.StatusCode -gt 299) {
            Add-Failure "R2 manifest fetch failed for $Slug (HTTP $($response.StatusCode)): $uri"
            return $null
        }
        $parsed = $response.Content | ConvertFrom-Json
        $set = @{}
        foreach ($entry in @($parsed)) {
            if ($entry -is [string] -and $entry.StartsWith('docs/')) {
                $set[$entry] = $true
            }
        }
        return $set
    } catch {
        Add-Failure "R2 manifest fetch/parse failed for ${Slug}: $($_.Exception.Message)"
        return $null
    }
}

$RepoRoot = (Invoke-Git rev-parse --show-toplevel | Select-Object -First 1).Trim()
Set-Location $RepoRoot
$Failures = @()

if (-not $BaseRef) {
    $upstream = & git rev-parse --abbrev-ref --symbolic-full-name '@{upstream}' 2>$null
    if ($LASTEXITCODE -eq 0 -and $upstream) {
        $BaseRef = $upstream.Trim()
    } else {
        $currentBranch = (& git branch --show-current 2>$null | Select-Object -First 1).Trim()
        $originBranch = ''
        if ($currentBranch) { $originBranch = "origin/$currentBranch" }
        if ($originBranch) {
            & git rev-parse --verify $originBranch 2>$null | Out-Null
            if ($LASTEXITCODE -eq 0) {
                $BaseRef = $originBranch
            }
        }
        if (-not $BaseRef) { $BaseRef = 'origin/main' }
    }
}

$zero = '0000000000000000000000000000000000000000'
if ($BaseRef -eq $zero) { $BaseRef = 'HEAD~1' }

$MergeBase = & git merge-base HEAD $BaseRef 2>$null
if ($LASTEXITCODE -ne 0 -or -not $MergeBase) {
    Write-Warning "[ReviewSync] Could not resolve merge-base with $BaseRef; falling back to HEAD~1"
    $MergeBase = & git rev-parse HEAD~1 2>$null
}

if ($MergeBase) {
    $nameStatus = @(git diff --name-status --diff-filter=AMRT "$MergeBase..HEAD")
} else {
    $nameStatus = @()
}

$changed = @()
foreach ($line in $nameStatus) {
    if (-not $line.Trim()) { continue }
    $parts = $line -split "`t"
    if ($parts.Count -lt 2) { continue }
    $status = $parts[0]
    $path = Normalize-Path ($parts[-1])
    $changed += [pscustomobject]@{ Status = $status; Path = $path }
}

$changedPaths = @($changed | ForEach-Object { $_.Path })
$requiresDevlog = @($changedPaths | Where-Object {
    $_ -match '^(Assets/|Packages/|ProjectSettings/|art/|tools/|\.github/|AGENTS\.md|CLAUDE\.md|docs/(STATUS\.md|MAP\.md|canon/|handoff/))'
})
$devlogChanges = @($changedPaths | Where-Object { $_ -match '^docs/devlog/[^/]+\.md$' })

if ($requiresDevlog.Count -gt 0 -and $devlogChanges.Count -eq 0) {
    Add-Failure ("Tracked implementation/workflow changes need a matching docs/devlog/*.md entry. Examples: " +
        (($requiresDevlog | Select-Object -First 6) -join ', '))
}

$newDevlogs = @($changed | Where-Object {
    $_.Status -match '^A' -and $_.Path -match '^docs/devlog/[^/]+\.md$' -and $_.Path -ne 'docs/devlog/INDEX.md'
})
if ($newDevlogs.Count -gt 0) {
    if ($changedPaths -notcontains 'docs/devlog/INDEX.md') {
        Add-Failure "New devlog file(s) were added but docs/devlog/INDEX.md was not changed."
    }
    $indexPath = Join-Path $RepoRoot 'docs/devlog/INDEX.md'
    $indexText = ''
    if (Test-Path -LiteralPath $indexPath) {
        $indexText = Get-Content -LiteralPath $indexPath -Raw
    }
    foreach ($entry in $newDevlogs) {
        $leaf = Split-Path $entry.Path -Leaf
        if ($indexText -notmatch [regex]::Escape($leaf)) {
            Add-Failure "New devlog is missing from docs/devlog/INDEX.md: $($entry.Path)"
        }
    }
}

if (-not $Ci) {
    $reviewRoot = Join-Path $RepoRoot 'docs/review'
    if (Test-Path -LiteralPath $reviewRoot) {
        $cutoffDate = (Get-Date).Date.AddDays(-1 * ([Math]::Max($RecentReviewDays, 1) - 1))
        $cycleDirs = @(Get-ChildItem -LiteralPath $reviewRoot -Directory -ErrorAction SilentlyContinue |
            Where-Object {
                $include = $false
                if ($_.Name -match '^\d{4}-\d{2}-\d{2}T') {
                    $cycleDate = [datetime]::MinValue
                    if ([datetime]::TryParseExact($_.Name.Substring(0, 10), 'yyyy-MM-dd',
                        [Globalization.CultureInfo]::InvariantCulture,
                        [Globalization.DateTimeStyles]::None, [ref]$cycleDate)) {
                        $include = ($cycleDate -ge $cutoffDate)
                    }
                }
                $include
            })
        $r2Manifest = $null
        $r2Slug = $null
        $runR2Check = (-not $SkipR2ManifestCheck) -and ($env:ANEMORA_SKIP_R2_MANIFEST_CHECK -ne '1')
        if ($runR2Check -and $cycleDirs.Count -gt 0) {
            $currentBranch = (& git branch --show-current 2>$null | Select-Object -First 1).Trim()
            if ($currentBranch) {
                $r2Slug = Convert-BranchToR2Slug -Branch $currentBranch
                $r2Manifest = Get-R2ManifestPathSet -Slug $r2Slug -BaseUrl $R2PublicBase
            } else {
                Add-Failure "Cannot resolve current branch for R2 review manifest validation."
            }
        }

        foreach ($dir in $cycleDirs) {
            if ($dir.Name -notmatch '^\d{4}-\d{2}-\d{2}T\d{2}-\d{2}([_-][A-Za-z0-9._-]+)?$') {
                Add-Failure "Review cycle dir must be ISO-like and URL-safe: docs/review/$($dir.Name)"
                continue
            }

            $devlogTxt = Join-Path $dir.FullName 'devlog.txt'
            $devlogRel = Get-FirstDevlogLine -Path $devlogTxt
            if (-not $devlogRel) {
                Add-Failure "Review cycle is missing devlog.txt with a devlog path: docs/review/$($dir.Name)"
            } elseif ($devlogRel -notmatch '^docs/devlog/[^/]+\.md$') {
                Add-Failure "Review cycle devlog.txt must point at docs/devlog/*.md: docs/review/$($dir.Name)"
            } else {
                $devlogLocal = Join-Path $RepoRoot ($devlogRel -replace '/', [IO.Path]::DirectorySeparatorChar)
                if (-not (Test-Path -LiteralPath $devlogLocal)) {
                    Add-Failure "Review cycle points at missing devlog: docs/review/$($dir.Name) -> $devlogRel"
                }
            }

            $images = @(Get-ChildItem -LiteralPath $dir.FullName -File -ErrorAction SilentlyContinue |
                Where-Object { $_.Extension.ToLowerInvariant() -in @('.png', '.jpg', '.jpeg', '.webp') })
            if ($images.Count -eq 0) {
                Add-Failure "Review cycle has no review image files: docs/review/$($dir.Name)"
            }

            if ($runR2Check -and $r2Manifest) {
                $cycleFiles = @(Get-ChildItem -LiteralPath $dir.FullName -File -ErrorAction SilentlyContinue)
                foreach ($file in $cycleFiles) {
                    $rel = "docs/review/$($dir.Name)/$($file.Name)"
                    if (-not $r2Manifest.ContainsKey($rel)) {
                        Add-Failure "Review cycle file is missing from R2 manifest ${r2Slug}: $rel"
                    }
                }
                if ($devlogRel -and $devlogRel -match '^docs/devlog/[^/]+\.md$' -and -not $r2Manifest.ContainsKey($devlogRel)) {
                    Add-Failure "Review cycle devlog is missing from R2 manifest ${r2Slug}: docs/review/$($dir.Name) -> $devlogRel"
                }
            }
        }
    }
}

if ($Failures.Count -gt 0) {
    Write-Error ("[ReviewSync] FAILED`n - " + ($Failures -join "`n - "))
    exit 1
}

Write-Host "[ReviewSync] OK (base=$BaseRef, ci=$Ci)"
exit 0
