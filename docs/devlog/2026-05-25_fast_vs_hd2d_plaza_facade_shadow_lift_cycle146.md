# feat(hd2d): lift plaza facade realtime shadows

## Intent

Cycle 145 moved the reference atmosphere into postprocess, but the central facade still carried a heavy horizontal realtime shadow band. Cycle 146 keeps floor shadows dense and realtime while giving wall/door/roof receivers a stronger realtime shadow texture lift.

## Scope

- Detect current central-plaza facade receivers in the runtime shadow rig.
- Raise `_ShadowTextureStrength` to `0.48` only for wall, door, roof, and named facade receivers.
- Keep ground/floor receivers at the Cycle 143/144 `0.36` value so floor shadows remain strong.
- Preserve realtime mesh casters, camera grade, and no-fog/no-painted-overlay constraints.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 146 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaFacadeShadowLiftCycle146Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaFacadeShadowLiftCycle146ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_facade_shadow_lift_cycle146.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle146_plaza_facade_shadow_lift_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_follow_start.png`
- `parent_review_02_current_central_plaza_follow_forward.png`
- `parent_review_03_current_central_plaza_realtime_shadow_floor.png`
- `parent_review_04_current_central_plaza_realtime_shadow_facade.png`
- `parent_review_05_current_library_guard.png`

## Visual Gate

- The facade band should remain a realtime shadow but read less like a flat black stripe.
- Floor shadows should retain their directional, high-contrast realtime silhouette.
- Camera grade and sky/camera framing should remain stable.
