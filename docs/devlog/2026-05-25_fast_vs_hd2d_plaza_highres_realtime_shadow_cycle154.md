# feat(hd2d): raise plaza realtime shadow quality

## Intent

Cycle 153 reduced the saturated sun grade, but the wall and floor shadows still showed blocky realtime shadowmap edges. Cycle 154 raises the central-plaza Directional Light shadow resolution and tightens the receiver-side realtime filter so the shadow quality improves without returning to painted haze or map overlays.

## Scope

- Keep cycles 147-153 realtime lighting, VS camera, blue-gray sky, desaturated sun grade, live cookie, sprite tracking, and visible prop casters.
- Force the central-plaza Directional Light to `LightShadowResolution.VeryHigh`.
- Replace the 5-tap receiver shadowmap feather with a tighter 9-tap filter.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 154 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaHighResRealtimeShadowCycle154Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaHighResRealtimeShadowCycle154ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_highres_realtime_shadow_cycle154.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle154_plaza_highres_realtime_shadow_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_follow_start.png`
- `parent_review_02_current_central_plaza_follow_forward.png`
- `parent_review_03_current_central_plaza_realtime_shadow_floor.png`
- `parent_review_04_current_central_plaza_realtime_shadow_facade.png`
- `parent_review_05_current_library_guard.png`

## Visual Gate

- Realtime shadow edges should be less blocky on the facade and floor.
- The fix must remain realtime: no re-enabled haze, painted light plates, or map-baked shadow ribbons.
