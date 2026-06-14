# R2 Mirror Review README Trigger Guard

Date: 2026-06-14
Branch: `wip/hd2d-point15-recovery-20260612`
Scope: R2/viewer propagation safeguard.

## Change

Updating `docs/review/README.md` triggered `r2-mirror-review` because the
workflow watched all `docs/review/**` paths. That legacy mirror job rebuilds
`manifests/<slug>.json` from git contents, so it can overwrite direct R2-upload
manifest entries for ignored review-image cycles.

The workflow push trigger now watches only review image extensions,
`docs/review/**/devlog.txt`, and `docs/devlog/screenshots/**`. Documentation
changes under `docs/review/` no longer run the legacy mirror.

## Verification

- `https://pub-d14764d639a647339a6b0d81de923abf.r2.dev/tree/wip-hd2d-point15-recovery-20260612/docs/review/2026-06-14T11-09_environment_uplift_phase1_4_apv_rebake/00_contact_sheet.png`:
  HEAD 200, confirming the review object still exists.
- Before restoration, the R2 manifest did not include
  `2026-06-14T11-09_environment_uplift_phase1_4_apv_rebake` after the README
  triggered mirror run.
- Re-ran `tools/r2/r2-upload-review.ps1 -CycleDir docs/review/2026-06-14T11-09_environment_uplift_phase1_4_apv_rebake -Branch wip/hd2d-point15-recovery-20260612`:
  uploaded 16 files, manifest HEAD OK.
- Rechecked `manifests/wip-hd2d-point15-recovery-20260612.json`: the
  environment uplift cycle is present again. Push this fix after the restoration
  so the viewer rebuild reads the restored manifest.
