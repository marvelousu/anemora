# feat(hd2d): keep plaza follow camera on map

## Intent

Cycle 135 still left the follow shot dominated by the central-plaza clear color. Cycle 136 fixes the review and runtime follow framing by using a lower, closer VS camera and moving the parent-review follow anchor onto the actual plaza floor.

## Scope

- Keep Cycle 134 realtime shadow/light setup intact.
- Tune the central-plaza follow camera to a lower, closer map-safe profile.
- Capture the follow review from an in-plaza floor position instead of a map-edge debris position.
- Add Cycle 136 validation, parent-review screenshots, and `docs/review/` output.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 136 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaMapSafeCameraCycle136Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaMapSafeCameraCycle136ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_map_safe_camera_cycle136.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle136_plaza_map_safe_camera_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_map_safe_camera_follow.png`
- `parent_review_02_current_central_plaza_map_safe_camera_floor.png`
- `parent_review_03_current_central_plaza_map_safe_camera_facade.png`
- `parent_review_04_current_library_map_safe_camera_guard.png`

## Visual Gate

- Follow shot must be on the plaza map, with floor/facade/shadows visible.
- Dark clear color should no longer dominate the normal central-plaza view.
- Realtime cast shadows should still read without camera-space haze or painted overlays.
