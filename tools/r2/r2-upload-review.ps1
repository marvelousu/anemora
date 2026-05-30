# Upload ONE review cycle's images to Cloudflare R2 + a never-pruned local archive,
# using the same layout as .github/workflows/r2-mirror-review.yml:
#   tree/<slug>/docs/review/<ts>/<file>   and   manifests/<slug>.json (path array).
# The AI review loop calls this AFTER writing a cycle dir (e.g. docs/review/<ts>/).
# Does NOT git-add anything (the pre-commit guard hard-blocks those paths).
# Requires: wrangler (logged in / CLOUDFLARE_API_TOKEN). ASCII only (PS5.1 safe).
#
# Usage:
#   tools\r2\r2-upload-review.ps1 -CycleDir docs/review/2026-05-30T10-00 -Branch work/<branch>
param(
  [Parameter(Mandatory = $true)][string]$CycleDir,
  [Parameter(Mandatory = $true)][string]$Branch,
  [int]$TtlDays = 45,                                  # informational; TTL is the bucket lifecycle rule
  [string]$Bucket = 'anemora-review',
  [string]$ArchiveRoot = 'C:\Users\maro6\Anemora-archive'
)
$ErrorActionPreference = 'Continue'

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

# (c) merge rel paths into manifests/<slug>.json (download-union-upload)
$mTmp = Join-Path $env:TEMP "manifest-$Slug.json"
Remove-Item $mTmp -ErrorAction SilentlyContinue
wrangler r2 object get "$Bucket/manifests/$Slug.json" --file $mTmp --remote 2>$null | Out-Null
$existing = @()
if (Test-Path $mTmp) { try { $existing = @(Get-Content $mTmp -Raw | ConvertFrom-Json) } catch { $existing = @() } }
$union = @($existing + $rels | Where-Object { $_ } | Select-Object -Unique | Sort-Object)
($union | ConvertTo-Json) | Out-File $mTmp -Encoding ascii
wrangler r2 object put "$Bucket/manifests/$Slug.json" --file $mTmp --content-type application/json --remote | Out-Null
if ($LASTEXITCODE -ne 0) { Write-Warning "manifest upload FAILED for $Slug (viewer may miss this cycle until re-run)" }

Write-Host "uploaded $($rels.Count) files for $Slug/$Ts (bucket TTL ${TtlDays}d); manifest now lists $($union.Count) paths"
