# One-time backfill: capture the review/devlog images checked in at each branch
# tip into the never-pruned local archive, and (unless -LocalOnly) upload them to
# Cloudflare R2 keyed by each file's last-commit date, writing per-branch
# manifests. Run BEFORE the Phase D purge so nothing the viewer shows is lost.
# Idempotent. ASCII only (PS5.1 safe). Binary extraction is byte-exact (git
# archive -> tar file, never a PowerShell text pipe).
#
# Usage (local preservation only, no Cloudflare needed):
#   tools\r2\r2-backfill-review.ps1 -LocalOnly -Branches @('main','work/...')
# Usage (after `wrangler login`):
#   tools\r2\r2-backfill-review.ps1 -Branches @('main','work/...')
param(
  [string[]]$Branches = @('main'),
  [switch]$LocalOnly,
  [string]$Bucket = 'anemora-review',
  [string]$ArchiveRoot = 'C:\Users\maro6\Anemora-archive',
  [string]$RepoRoot = 'C:\Users\maro6\Documents\Unity\Anemora'
)
$ErrorActionPreference = 'Continue'
Push-Location $RepoRoot

function Get-ContentType($name) {
  switch ([IO.Path]::GetExtension($name).ToLower()) {
    '.png'  { 'image/png' }
    '.jpg'  { 'image/jpeg' }
    '.jpeg' { 'image/jpeg' }
    '.webp' { 'image/webp' }
    default { 'text/plain' }
  }
}

foreach ($Branch in $Branches) {
  $Slug = ($Branch -replace '^work/', '' -replace '[^a-zA-Z0-9._-]+', '-').Trim('-')
  Write-Host "=== backfill $Branch (slug=$Slug)$(if($LocalOnly){' [local-only]'}) ==="

  $tmp = Join-Path $env:TEMP "anemora-bf-$Slug"
  $tar = Join-Path $env:TEMP "anemora-bf-$Slug.tar"
  if (Test-Path $tmp) { Remove-Item -Recurse -Force $tmp }
  New-Item -ItemType Directory -Force $tmp | Out-Null

  # Byte-exact extraction: git writes the tar itself (no PS pipe), tar unpacks it.
  git archive --format=tar -o $tar $Branch -- docs/review docs/devlog/screenshots
  if ($LASTEXITCODE -ne 0) { Write-Host "  (no review imagery at tip / archive skipped)"; continue }
  tar -xf $tar -C $tmp
  if ($LASTEXITCODE -ne 0) { Write-Warning "  tar extract failed"; continue }

  $files = Get-ChildItem -Recurse -File $tmp
  if (-not $files) { Write-Host "  (no files extracted)"; Remove-Item -Force $tar -ErrorAction SilentlyContinue; continue }

  $byTs = @{}
  foreach ($f in $files) {
    $rel = ($f.FullName.Substring($tmp.Length).TrimStart('\', '/')) -replace '\\', '/'
    if ($rel -match '^docs/review/([^/]+)/(.+)$')                { $ts = $Matches[1];                 $fname = Split-Path $rel -Leaf }
    elseif ($rel -match '^docs/devlog/screenshots/(.+)/([^/]+)$'){ $ts = ($Matches[1] -replace '/', '__'); $fname = $Matches[2] }
    else                                                         { $ts = 'misc';                       $fname = Split-Path $rel -Leaf }

    $date = (git log -1 --format=%cs $Branch -- $rel)
    if (-not $date) { $date = (Get-Date -Format 'yyyy-MM-dd') }

    # (a) local archive — always, even in LocalOnly mode (this is the never-lost copy)
    $localDir = Join-Path $ArchiveRoot "review\$Slug\$ts"
    New-Item -ItemType Directory -Force $localDir | Out-Null
    Copy-Item $f.FullName (Join-Path $localDir $fname) -Force

    # (b) upload — only on success do we record it in the manifest
    $ok = $true
    if (-not $LocalOnly) {
      $key = "review/$Slug/$date/$ts/$fname"
      wrangler r2 object put "$Bucket/$key" --file $f.FullName --content-type (Get-ContentType $fname) --remote
      if ($LASTEXITCODE -ne 0) { Write-Warning "  upload FAILED (kept locally, omitted from manifest): $key"; $ok = $false }
    }
    if ($ok) {
      if (-not $byTs.ContainsKey($ts)) { $byTs[$ts] = [pscustomobject]@{ ts = $ts; date = $date; files = @() } }
      $byTs[$ts].files += $fname
    }
  }

  $m = [pscustomobject]@{ branch = $Branch; slug = $Slug; cycles = @($byTs.Values | Sort-Object ts) }
  $mLocal = Join-Path $ArchiveRoot "manifests\$Slug.json"
  New-Item -ItemType Directory -Force (Split-Path $mLocal) | Out-Null
  ($m | ConvertTo-Json -Depth 8) | Out-File $mLocal -Encoding utf8
  if (-not $LocalOnly) {
    wrangler r2 object put "$Bucket/manifests/$Slug.json" --file $mLocal --content-type application/json --remote
    if ($LASTEXITCODE -ne 0) { Write-Warning "  manifest upload FAILED for $Slug" }
  }

  Remove-Item -Recurse -Force $tmp -ErrorAction SilentlyContinue
  Remove-Item -Force $tar -ErrorAction SilentlyContinue
  Write-Host "  archived $($files.Count) files across $($m.cycles.Count) cycles"
}
Pop-Location
