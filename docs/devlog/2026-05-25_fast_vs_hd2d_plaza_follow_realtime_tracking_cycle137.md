# feat(hd2d): recover plaza realtime follow framing

## Intent

Cycle 136 proved the realtime shadow/light setup in the floor and facade review shots, but the normal follow shot was still mostly clear color. Cycle 137 moves the actual follow camera profile to the same playable-floor composition that showed the realtime shadows, and restores the central-plaza clear color to the warm VS backdrop used by the lighting director.

## Scope

- Keep Cycle 134 realtime shadow casters/receivers and fog suppression.
- Replace the central-plaza follow camera with a player-relative look-ahead profile derived from the working floor review frame.
- Restore central-plaza sky/clear color from dark blue-black to the warm VS outdoor backdrop.
- Capture two follow positions so the camera tracking can be checked as the player moves through the plaza.
- Add Cycle 137 validation, parent-review screenshots, and `docs/review/` output.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 137 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaFollowRealtimeTrackingCycle137Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaFollowRealtimeTrackingCycle137ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_follow_realtime_tracking_cycle137.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle137_plaza_follow_realtime_tracking_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_follow_start.png`
- `parent_review_02_current_central_plaza_follow_forward.png`
- `parent_review_03_current_central_plaza_realtime_shadow_floor.png`
- `parent_review_04_current_central_plaza_realtime_shadow_facade.png`
- `parent_review_05_current_library_guard.png`

## Visual Gate

- The two follow shots must show the plaza floor/facade and realtime shadows, not mostly clear color.
- Clear color must read as a warm VS backdrop if it appears, not an unexplained dark void.
- Realtime cast shadows must continue to track from scene light/casters without camera haze, painted fog, or camera-space shadow plates.
