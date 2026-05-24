# feat(hd2d): restore plaza VS sky camera

## Intent

The realtime shadow work was still being reviewed from a wall-heavy camera angle, and the central plaza sky had drifted into a brown clear-color wash. Cycle 151 restores the central plaza follow camera to a VS-style plaza-wide anchor while keeping realtime light/shadow tracking, and switches the plaza clear color back to a blue-gray sky instead of a painted haze/backdrop substitute.

## Scope

- Clamp the central-plaza follow camera anchor depth to the VS plaza framing when the player is near the library facade.
- Keep horizontal follow tracking so the camera still follows player movement.
- Replace the central-plaza brown clear color in the visibility controller, lighting director, and realtime rig with a blue-gray sky color.
- Update the realtime follow screenshot path to use the runtime camera anchor resolver instead of manually anchoring on the player's close facade position.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 151 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaVsSkyCameraCycle151Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaVsSkyCameraCycle151ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_vs_sky_camera_cycle151.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle151_plaza_vs_sky_camera_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_follow_start.png`
- `parent_review_02_current_central_plaza_follow_forward.png`
- `parent_review_03_current_central_plaza_realtime_shadow_floor.png`
- `parent_review_04_current_central_plaza_realtime_shadow_facade.png`
- `parent_review_05_current_library_guard.png`

## Visual Gate

- The follow screenshots should read as a central plaza view, not a close crop of the library facade.
- The sky should come from a stable blue-gray camera clear color, not from map haze plates or brown wash.
- Realtime Directional Light, cookie, shadow receivers, sprite tracking, and visible prop casters from cycles 147-150 must remain active.
