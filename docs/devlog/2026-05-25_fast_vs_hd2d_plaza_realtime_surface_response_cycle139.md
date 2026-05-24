# feat(hd2d): boost plaza realtime surface response

## Intent

Cycle 138 added realtime dapple casters, but the sunlit side still read flatter than the reference because the receiving materials were conservative. Cycle 139 keeps the realtime setup and strengthens the central-plaza surface response through renderer property blocks instead of map-painted haze or camera plates.

## Scope

- Keep the Cycle 137 follow framing and Cycle 138 realtime caster set.
- Let current central-plaza wall, door, and roof surfaces receive realtime shadows in addition to floors/grounds.
- Apply central-plaza-only material property blocks for warmer sunlit ramps and darker shadow receiving.
- Do not mutate shared material assets or re-enable painted shadow/fog overlays.
- Capture the same follow/floor/facade parent-review set.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 139 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeSurfaceResponseCycle139Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaRealtimeSurfaceResponseCycle139ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_realtime_surface_response_cycle139.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle139_plaza_realtime_surface_response_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_follow_start.png`
- `parent_review_02_current_central_plaza_follow_forward.png`
- `parent_review_03_current_central_plaza_realtime_shadow_floor.png`
- `parent_review_04_current_central_plaza_realtime_shadow_facade.png`
- `parent_review_05_current_library_guard.png`

## Visual Gate

- Sunlit ground/facade should read warmer and brighter without flattening the shadows.
- Wall and door shadows should be realtime receivers, not static facade tint.
- No camera-space haze, fog, or painted shadow plates should return.
