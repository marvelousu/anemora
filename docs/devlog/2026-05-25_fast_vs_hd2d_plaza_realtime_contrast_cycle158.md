# feat(hd2d): tighten plaza realtime contrast

## Intent

After the camera and runtime sky fixes, the central plaza still read too yellow and flat. Cycle 158 tightens the realtime shading path directly instead of adding more painted haze: lower ambient, stronger realtime Directional Light shadow strength, neutral warm sun color, darker receiver shade ramps, and stronger receiver shadow property blocks.

## Scope

- Increase central-plaza realtime `shadowStrength` to near full while keeping soft realtime shadows and the existing cookie.
- Lower central-plaza ambient light so live shadows remain visible without painted fog.
- Neutralize the surface top-light/floor-shade ramp so the floor no longer washes into a beige/yellow sheet.
- Raise realtime receiver shadow strength/texture property blocks on central-plaza surfaces.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 158 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeContrastCycle158Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaRealtimeContrastCycle158ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_realtime_contrast_cycle158.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle158_plaza_realtime_contrast_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_follow_start.png`
- `parent_review_02_current_central_plaza_follow_forward.png`
- `parent_review_03_current_central_plaza_realtime_shadow_floor.png`
- `parent_review_04_current_central_plaza_realtime_shadow_facade.png`
- `parent_review_05_current_library_guard.png`

## Visual Gate

- Shadows should be darker and less washed out, with light/shadow contrast coming from realtime light/shadow receiver settings.
- The floor should be less yellow and less uniformly bright.
- Runtime skybox, VS camera framing, realtime cookie, visible casters, and trimmed `ShadowsOnly` casters must remain active.
