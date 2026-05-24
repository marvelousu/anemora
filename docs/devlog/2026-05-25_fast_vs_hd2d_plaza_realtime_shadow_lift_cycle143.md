# feat(hd2d): lift plaza realtime shadow texture

## Intent

Cycle 142 added a realtime shader texture term, but the visual pass was still conservative and the darkest shadow bands remained too flat. Cycle 143 strengthens the same realtime receiver response and removes the material-asset churn caused by setting the zero default on every generated material.

## Scope

- Increase central-plaza `_ShadowTextureStrength` runtime property blocks from `0.28` to `0.36`.
- Strengthen the realtime shader shadow lift and mottle term while still using main-light shadow attenuation.
- Stop writing `_ShadowTextureStrength = 0` into every generated surface material asset; the shader default remains zero.
- Preserve Cycle 137 camera/sky and all Cycle 140/141 realtime caster geometry.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 143 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeShadowLiftCycle143Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaRealtimeShadowLiftCycle143ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_realtime_shadow_lift_cycle143.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle143_plaza_realtime_shadow_lift_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_follow_start.png`
- `parent_review_02_current_central_plaza_follow_forward.png`
- `parent_review_03_current_central_plaza_realtime_shadow_floor.png`
- `parent_review_04_current_central_plaza_realtime_shadow_facade.png`
- `parent_review_05_current_library_guard.png`

## Visual Gate

- Dark shadow areas should keep the realtime geometry silhouette but stop reading as a flat black overlay.
- Shared material assets should not stay dirty after capture/build.
- No painted fog, haze, or screen-space light/shadow plates should return.
