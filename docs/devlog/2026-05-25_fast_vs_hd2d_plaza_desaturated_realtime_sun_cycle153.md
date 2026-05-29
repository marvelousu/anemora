# feat(hd2d): desaturate plaza realtime sun grade

## Intent

The realtime cookie ribbon was removed in Cycle 152, but the central plaza still read as too yellow compared with the reference images' warm yet faded HD-2D sunlight. Cycle 153 keeps realtime shadows and light tracking while reducing the orange-yellow surface grade.

## Scope

- Keep the realtime Directional Light, cookie, live shadowmap receivers, sprite light response, visible prop casters, VS camera anchor, and blue-gray sky.
- Shift the central-plaza Directional Light color from orange-yellow toward warm-neutral sunlight.
- Reduce `SurfaceLit` central-plaza top/floor/side grade saturation so the floor does not become a flat yellow wash.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 153 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaDesaturatedRealtimeSunCycle153Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaDesaturatedRealtimeSunCycle153ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_desaturated_realtime_sun_cycle153.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle153_plaza_desaturated_realtime_sun_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_follow_start.png`
- `parent_review_02_current_central_plaza_follow_forward.png`
- `parent_review_03_current_central_plaza_realtime_shadow_floor.png`
- `parent_review_04_current_central_plaza_realtime_shadow_facade.png`
- `parent_review_05_current_library_guard.png`

## Visual Gate

- Plaza sunlight should still be warm, but the floor should no longer read as a saturated yellow overlay.
- Realtime shadow/light stack from cycles 147-152 must remain active and validated.
