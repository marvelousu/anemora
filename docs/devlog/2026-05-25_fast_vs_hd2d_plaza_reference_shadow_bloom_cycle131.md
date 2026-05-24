# feat(hd2d): add plaza reference shadow bloom

## Intent

Cycle 130 improved validation and separated world-space sun/shadow layers, but the parent-review frames still read too flat because the world quads were subtle against the plaza wall and floor. Cycle 131 adds a camera-space HD-2D paint pass so the reference-like black shade, bright sun bloom, and diagonal shafts are visible immediately in the review view.

## Scope

- Add central-plaza-only runtime camera paint plates in `FastVsRealtimeLightShadowRig`.
- Paint a late-rendered shadow plate with broad diagonal shade, contact darkness, side vignette, and dapple breakup.
- Paint a late-rendered sun plate with pale sun patches and stronger diagonal shafts.
- Keep the plates inactive outside the central plaza by using the existing camera-grade root activation.
- Add Cycle 131 validation and parent-review screenshots, including `docs/review/` output.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 131 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaReferenceShadowBloomCycle131Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaReferenceShadowBloomCycle131ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_reference_shadow_bloom_cycle131.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle131_plaza_reference_shadow_bloom_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_reference_shadow_bloom_follow.png`
- `parent_review_02_current_central_plaza_reference_shadow_bloom_floor.png`
- `parent_review_03_current_central_plaza_reference_shadow_bloom_facade.png`
- `parent_review_04_current_library_reference_shadow_bloom_guard.png`

## Visual Gate

- Central plaza should show obvious black shade and pale sun bloom in the camera view.
- The effect should be immediately visible compared with Cycle 130, not only a small material tweak.
- Library guard should remain visually unchanged.
