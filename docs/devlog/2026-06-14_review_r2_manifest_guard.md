# Review R2 manifest guard

Area: Repo workflow / review publishing
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-14

## Investigation

- The distant-valley cycle proved that local `docs/review/<cycle>/`, R2 upload, and `anemora-viewer` rebuild are all separate gates. A cycle can pass Unity validation while the public review surface still shows stale image and devlog content.
- Existing `tools/review/validate-devlog-review-sync.ps1` already blocked missing devlog entries, missing `docs/devlog/INDEX.md` updates, and malformed recent review folders. It did not prove that recent local review folders had been uploaded to the public R2 manifest.
- The live viewer flow is build-time: `anemora-viewer` fetches the Anemora branch, reads the R2 manifest for the branch slug, fetches the listed review/devlog files, then generates the static review/gallery/devlog pages. Therefore the cheapest project-global guard is to fail local pre-push when a recent local review cycle is absent from the manifest.

## Change

- Extended `tools/review/validate-devlog-review-sync.ps1` local mode with an R2 manifest check. For recent `docs/review/<cycle>/` directories, it now verifies that every file in the cycle plus the linked `docs/devlog/*.md` are present in the public manifest for the current branch slug.
- Kept CI mode history-only so CI remains independent of local ignored review folders.
- Added `-SkipR2ManifestCheck` and `ANEMORA_SKIP_R2_MANIFEST_CHECK=1` as explicit emergency/offline bypasses.
- Updated the review and R2 docs to state the full publication contract: local review directory, devlog/index, R2 upload, Anemora or viewer rebuild trigger, and public route verification.
- Updated `tools/r2/r2-upload-review.ps1` completion text so future cycles do not stop at upload-only success.

## Verification

- PowerShell parser checks passed for `tools/review/validate-devlog-review-sync.ps1` and `tools/r2/r2-upload-review.ps1`.
- Local review guard passed: `powershell -ExecutionPolicy Bypass -File tools/review/validate-devlog-review-sync.ps1` returned `[ReviewSync] OK` with R2 manifest validation enabled.
- CI-mode review guard passed: `powershell -ExecutionPolicy Bypass -File tools/review/validate-devlog-review-sync.ps1 -Ci -BaseRef origin/wip/hd2d-point15-recovery-20260612` returned `[ReviewSync] OK`.
- Public viewer propagation from the previous visual cycle was rechecked after the viewer marker rebuild: review route showed `7 cycles` / `98 images`, the distant-valley devlog route included `R2 review upload`, `DistantVista_ValleyThread`, and `112 paths`, the gallery route included `00_contact_sheet.png`, `13_scene6_sideview_auto.png`, and `14 images`, and the older bridge-support devlog route now includes the previously missing `c9303a0`, `R2 retry`, and `Local viewer proof` text.
