# feat(hd2d): fix plaza skybox horizon

## Intent

Cycle 156 restored a runtime skybox and moved the central-plaza camera toward the VS composition, but the procedural skybox ground band rendered too dark at the horizon. Cycle 157 keeps the realtime sky path and fixes the black horizon by tuning the procedural skybox ground/tint/exposure, while moving the plaza camera slightly farther and wider so the roof/upper facade are less clipped.

## Scope

- Retain the realtime skybox path instead of map-space haze or sky plates.
- Raise the skybox ground band to blue-gray and increase exposure so the horizon no longer reads as a black strip.
- Move the central-plaza follow profile to `(0, 3.55, -6.50)` / look `(0, 1.18, 1.35)` / FOV `40`.
- Preserve realtime Directional Light shadows, cookie, visible caster shadows, and trimmed `ShadowsOnly` casters.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 157 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSkyboxHorizonCycle157Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaSkyboxHorizonCycle157ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_skybox_horizon_cycle157.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle157_plaza_skybox_horizon_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_follow_start.png`
- `parent_review_02_current_central_plaza_follow_forward.png`
- `parent_review_03_current_central_plaza_realtime_shadow_floor.png`
- `parent_review_04_current_central_plaza_realtime_shadow_facade.png`
- `parent_review_05_current_library_guard.png`

## Visual Gate

- The sky horizon should no longer collapse into a black strip.
- The first plaza screenshots should show more roof/upper facade/sky than Cycle155.
- Realtime shadow and light response must remain active and should still visibly follow the live Directional Light/caster setup.
