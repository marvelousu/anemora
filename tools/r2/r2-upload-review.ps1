# Upload ONE review cycle's images to Cloudflare R2 + a never-pruned local archive,
# using the same layout as .github/workflows/r2-mirror-review.yml:
#   tree/<slug>/docs/review/<ts>/<file>   and   manifests/<slug>.json (path array).
# The AI review loop calls this AFTER writing a cycle dir (e.g. docs/review/<ts>/).
# Does NOT git-add anything (the pre-commit guard hard-blocks those paths).
# Requires: wrangler (logged in / CLOUDFLARE_API_TOKEN). ASCII only (PS5.1 safe).
#
# Usage:
#   tools\r2\r2-upload-review.ps1 -CycleDir docs/review/2026-05-30T10-00 -Branch work/<branch>
#   tools\r2\r2-upload-review.ps1 -CycleDir docs/review/2026-06-14T11-09 -Branch wip/hd2d-point15-recovery-20260612
param(
  [Parameter(Mandatory = $true)][string]$CycleDir,
  [Parameter(Mandatory = $true)][string]$Branch,
  [int]$TtlDays = 45,                                  # informational; TTL is the bucket lifecycle rule
  [string]$Bucket = 'anemora-review',
  [string]$ArchiveRoot = 'C:\Users\maro6\Anemora-archive'
)
$ErrorActionPreference = 'Continue'

# Durable unattended auth: if CLOUDFLARE_API_TOKEN is not already exported, load it
# from ~/.cf_token (plaintext Account "Workers R2 Storage: Edit" token, by design).
# Lets the AI review loop upload without an interactive `wrangler login`.
if (-not $env:CLOUDFLARE_API_TOKEN) {
  $CfTokenFile = Join-Path $env:USERPROFILE '.cf_token'
  if (Test-Path $CfTokenFile) {
    $env:CLOUDFLARE_API_TOKEN = (Get-Content $CfTokenFile -Raw).Trim()
  }
}
if (-not $env:CLOUDFLARE_API_TOKEN) {
  Write-Warning "No CLOUDFLARE_API_TOKEN and no ~/.cf_token; wrangler put will fail. See tools/r2/README.md."
}

if (-not (Test-Path $CycleDir)) { Write-Error "CycleDir not found: $CycleDir"; exit 1 }

$Slug = ($Branch -replace '^work/', '' -replace '[^a-zA-Z0-9._-]+', '-').Trim('-')
$Ts   = Split-Path $CycleDir -Leaf            # 2026-05-30T10-00
# git-relative prefix this cycle lives under (review cycles -> docs/review/<ts>)
$RelPrefix = "docs/review/$Ts"

# (a) never-pruned local archive (outside the repo working tree)
$Archive = Join-Path $ArchiveRoot "tree\$Slug\docs\review\$Ts"
New-Item -ItemType Directory -Force $Archive | Out-Null
Copy-Item (Join-Path $CycleDir '*') $Archive -Recurse -Force

# (b) upload each file to tree/<slug>/<rel>, collecting the rel paths that succeeded
$rels = @()
foreach ($f in Get-ChildItem $CycleDir -File) {
  $rel = "$RelPrefix/$($f.Name)"
  wrangler r2 object put "$Bucket/tree/$Slug/$rel" --file $f.FullName --remote | Out-Null
  if ($LASTEXITCODE -ne 0) { Write-Warning "upload FAILED (kept in local archive): $rel"; continue }
  $rels += $rel
}

# (b.5) If devlog.txt points at a repo-root devlog markdown file, mirror that too.
# Review images stay out of git, so this lets anemora-viewer open the matching
# devlog without requiring an Anemora push.
$DevlogTxt = Join-Path $CycleDir 'devlog.txt'
if (Test-Path $DevlogTxt) {
  $DevlogRel = Get-Content $DevlogTxt |
    ForEach-Object { $_.Trim() } |
    Where-Object { $_ -and -not $_.StartsWith('#') } |
    Select-Object -First 1
  if ($DevlogRel -and ($DevlogRel -match '^docs/devlog/[^/]+\.md$')) {
    $DevlogLocal = Join-Path (Get-Location) ($DevlogRel -replace '/', [IO.Path]::DirectorySeparatorChar)
    if (Test-Path $DevlogLocal) {
      $ArchiveDevlog = Join-Path $ArchiveRoot "tree\$Slug\$($DevlogRel -replace '/', '\')"
      New-Item -ItemType Directory -Force (Split-Path $ArchiveDevlog -Parent) | Out-Null
      Copy-Item $DevlogLocal $ArchiveDevlog -Force
      wrangler r2 object put "$Bucket/tree/$Slug/$DevlogRel" --file $DevlogLocal --remote | Out-Null
      if ($LASTEXITCODE -ne 0) { Write-Warning "devlog upload FAILED (kept in local archive): $DevlogRel" }
      else { $rels += $DevlogRel }
    } else {
      Write-Warning "devlog ref not found locally; viewer devlog link may 404: $DevlogRel"
    }
  }
}

# (c) Rebuild manifests/<slug>.json from the never-pruned local archive (authoritative;
#     it holds every uploaded cycle). Replaces a fragile download-union round-trip whose
#     ConvertTo-Json/ConvertFrom-Json compounding corrupted the manifest into nested
#     objects + space-joined strings. Build the JSON array by hand so PS cannot wrap it.
$mTmp = Join-Path $env:TEMP "manifest-$Slug.json"
$ArchiveSlugRoot = Join-Path $ArchiveRoot "tree\$Slug"
$allRels = @()
if (Test-Path $ArchiveSlugRoot) {
  $allRels = Get-ChildItem $ArchiveSlugRoot -Recurse -File |
    ForEach-Object { ($_.FullName.Substring($ArchiveSlugRoot.Length + 1) -replace '\\', '/') } |
    Where-Object { $_ -like 'docs/review/*' -or $_ -like 'docs/devlog/screenshots/*' -or $_ -match '^docs/devlog/[^/]+\.md$' } |
    Sort-Object -Unique
}
$items = $allRels | ForEach-Object { '"' + ($_ -replace '\\', '\\' -replace '"', '\"') + '"' }
$manifestJson = '[' + ($items -join ',') + ']'
[System.IO.File]::WriteAllText($mTmp, $manifestJson, [System.Text.Encoding]::ASCII)
wrangler r2 object put "$Bucket/manifests/$Slug.json" --file $mTmp --content-type application/json --remote | Out-Null
if ($LASTEXITCODE -ne 0) { Write-Warning "manifest upload FAILED for $Slug (viewer may miss this cycle until re-run)" }

Write-Host "uploaded $($rels.Count) files for $Slug/$Ts (bucket TTL ${TtlDays}d); manifest now lists $($allRels.Count) paths"
$PublicR2Base = 'https://pub-d14764d639a647339a6b0d81de923abf.r2.dev'
$ViewerBase = 'https://anemora-viewer.pages.dev'
$ManifestUrl = "$PublicR2Base/manifests/$Slug.json"
$ViewerUrl = "$ViewerBase/$Slug/review"
try {
  Invoke-WebRequest -Method Head -Uri $ManifestUrl -UseBasicParsing -TimeoutSec 20 | Out-Null
  Write-Host "manifest HEAD OK: $ManifestUrl"
} catch {
  Write-Warning "manifest HEAD check failed; verify before closing the cycle: $ManifestUrl"
}
Write-Host "viewer review URL: $ViewerUrl"
Write-Host "After upload, push the Anemora branch or refresh anemora-viewer/public/deploy-refresh.txt, then verify review, gallery, and linked devlog routes."
