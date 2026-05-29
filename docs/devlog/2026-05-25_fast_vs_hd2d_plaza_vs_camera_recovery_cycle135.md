# feat(hd2d): recover plaza VS camera framing

## Intent

Cycle 134 restored realtime shadow casting, but the follow shot exposed too much empty clear color because the plaza camera was pulled back like the exterior area. Cycle 135 keeps the realtime shadow setup and moves only the central-plaza follow profile back into a map-safe VS framing.

## Scope

- Keep Cycle 134 realtime lights, ShadowOnly occluders, and painted-overlay suppression intact.
- Change only the central-plaza follow camera profile to frame the plaza floor and facade instead of the dark map edge.
- Add Cycle 135 validation, parent-review screenshots, and `docs/review/` output.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 135 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaVsCameraRecoveryCycle135Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaVsCameraRecoveryCycle135ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_vs_camera_recovery_cycle135.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle135_plaza_vs_camera_recovery_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_vs_camera_recovery_follow.png`
- `parent_review_02_current_central_plaza_vs_camera_recovery_floor.png`
- `parent_review_03_current_central_plaza_vs_camera_recovery_facade.png`
- `parent_review_04_current_library_vs_camera_recovery_guard.png`

## Visual Gate

- Central plaza follow shot should show real floor, facade, and cast shadows, not mostly empty sky/clear color.
- Shadow/light should still come from realtime casters and the directional light.
- Library guard should remain visually unchanged.
