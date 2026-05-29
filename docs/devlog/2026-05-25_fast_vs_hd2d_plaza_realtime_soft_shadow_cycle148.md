# feat(hd2d): soften plaza realtime shadowmap

## Intent

Cycle 147 moved the light variation into the Directional Light and restored the VS camera, but the plaza still read as hard-edged caster geometry. Cycle 148 softens the realtime shadow receiver path by sampling the live main-light shadowmap around each receiver fragment.

## Scope

- Keep the Cycle 147 Directional Light cookie and VS follow camera.
- In `FastVS_SurfaceRampLit`, sample `MainLightRealtimeShadow` at offset world positions around the receiver fragment.
- Blend those live shadowmap samples with the existing receiver response using `_ShadowTextureStrength`.
- Validate that the shader uses realtime shadowmap taps instead of map-painted haze or camera plates.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 148 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeSoftShadowCycle148Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaRealtimeSoftShadowCycle148ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_realtime_soft_shadow_cycle148.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle148_plaza_realtime_soft_shadow_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_follow_start.png`
- `parent_review_02_current_central_plaza_follow_forward.png`
- `parent_review_03_current_central_plaza_realtime_shadow_floor.png`
- `parent_review_04_current_central_plaza_realtime_shadow_facade.png`
- `parent_review_05_current_library_guard.png`

## Visual Gate

- Realtime shadow edges should be less brutally hard than Cycle 147 while staying directional and caster-driven.
- The VS camera and no-fog/no-fake-sky contract from Cycle 147 must remain intact.
