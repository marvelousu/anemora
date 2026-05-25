# feat(hd2d): neutralize plaza realtime shader

## Intent

Cycle 158 tightened the realtime light and receiver values, but the surface shader still had hardcoded yellow sun and brown shadow tint constants. Cycle 159 updates the realtime `SurfaceRampLit` light calculation itself so realtime shadows and sun response are more neutral, closer to the desaturated reference direction, without adding painted haze or map overlays.

## Scope

- Reduce realtime cookie sun grade from the previous bright yellow response.
- Change the realtime sun tint base from yellow to neutral warm stone.
- Change textured realtime shadow tint from brown to gray-green/stone.
- Preserve realtime shadowmap sampling, cookie support, camera/skybox fixes, visible casters, and trimmed `ShadowsOnly` casters.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 159 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaNeutralRealtimeShaderCycle159Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaNeutralRealtimeShaderCycle159ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_neutral_realtime_shader_cycle159.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle159_plaza_neutral_realtime_shader_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_follow_start.png`
- `parent_review_02_current_central_plaza_follow_forward.png`
- `parent_review_03_current_central_plaza_realtime_shadow_floor.png`
- `parent_review_04_current_central_plaza_realtime_shadow_facade.png`
- `parent_review_05_current_library_guard.png`

## Visual Gate

- Sunlit floor and facade should lose the previous beige/yellow cast.
- Realtime shadows should read as darker neutral stone shadows rather than brown painted bands.
- The improvement must come from realtime shader/light evaluation, not enabled haze or sun plates.
