# feat(hd2d): texture plaza realtime shadows

## Intent

Cycle 141 kept shadows realtime and reduced the worst rectangular caster silhouettes, but the remaining realtime shadow read was still too hard and flat. Cycle 142 changes the receiving shader response instead of painting haze onto the map: central-plaza receivers use a realtime shadow-attentuation texture term to lift and mottle the darkest shadow areas.

## Scope

- Add `_ShadowTextureStrength` to `Anemora/FastVS/SurfaceRampLit`.
- Keep the shader driven by `GetMainLight(shadowCoord)` and realtime directional-light shadow attenuation.
- Apply the new property only through central-plaza runtime material property blocks.
- Keep non-plaza materials at default zero so this does not repaint the whole game.
- Preserve Cycle 137 camera/sky and Cycle 140/141 realtime caster geometry.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 142 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeShadowTextureCycle142Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaRealtimeShadowTextureCycle142ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_realtime_shadow_texture_cycle142.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle142_plaza_realtime_shadow_texture_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_follow_start.png`
- `parent_review_02_current_central_plaza_follow_forward.png`
- `parent_review_03_current_central_plaza_realtime_shadow_floor.png`
- `parent_review_04_current_central_plaza_realtime_shadow_facade.png`
- `parent_review_05_current_library_guard.png`

## Visual Gate

- Shadow response should remain tied to realtime caster geometry and main-light shadow attenuation.
- Deep shadow should be less flat-black and more HD-2D/paper-textured.
- No fog, haze, painted floor shadow plates, or screen-space sun patches should return.
