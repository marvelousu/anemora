# feat(hd2d): force plaza reference shadow contrast

## Intent

Cycle 131's extra camera plates validated but were still too visually subtle in parent review. Cycle 132 rewrites the already-visible central-plaza camera grade texture itself so the review frame must carry obvious black shade bands, contact darkness, pale sun patches, and diagonal shafts.

## Scope

- Repaint the runtime Cycle 128 central-plaza grade texture with strong HD-2D shadow/light shapes.
- Keep Cycle 131 camera paint plates active as an additional late pass.
- Add Cycle 132 validation and parent-review screenshots, including `docs/review/` output.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 132 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaReferenceShadowContrastCycle132Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaReferenceShadowContrastCycle132ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_reference_shadow_contrast_cycle132.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle132_plaza_reference_shadow_contrast_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_reference_shadow_contrast_follow.png`
- `parent_review_02_current_central_plaza_reference_shadow_contrast_floor.png`
- `parent_review_03_current_central_plaza_reference_shadow_contrast_facade.png`
- `parent_review_04_current_library_reference_shadow_contrast_guard.png`

## Visual Gate

- Cycle 132 should be visibly different from Cycle 130/131 in a single glance.
- Central plaza should show deliberate black shade and pale sun patches even if world-space floor overlays stay subtle.
- Library guard should remain visually unchanged.
