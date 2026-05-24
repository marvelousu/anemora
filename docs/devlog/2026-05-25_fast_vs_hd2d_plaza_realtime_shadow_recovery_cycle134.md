# feat(hd2d): recover plaza realtime shadow read

## Intent

Cycle 133 still read as fog and camera tint over a flat plaza. Cycle 134 removes that premise: central plaza lighting now depends on realtime ShadowOnly casters, realtime shadow receivers, the directional light, and the same pulled-back VS follow camera used by the exterior.

## Scope

- Keep Cycle 127/128 central-plaza realtime shadow casters active through later painted-shadow setup passes.
- Add eight Cycle 134 ShadowOnly occluders over the plaza so the stronger shadow read comes from Unity realtime shadows, not texture plates.
- Disable Cycle 129/130 painted recovery quads and Cycle 128/131 camera-grade plates so realtime cast shadows drive the plaza read.
- Remove central-plaza runtime fog and return the clear color to the existing VS central-plaza sky value.
- Pull the central-plaza follow review camera back to the higher exterior VS profile for broader shadow readability.
- Add Cycle 134 validation, parent-review screenshots, and `docs/review/` output.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 134 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeShadowRecoveryCycle134Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaRealtimeShadowRecoveryCycle134ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_realtime_shadow_recovery_cycle134.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle134_plaza_realtime_shadow_recovery_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_realtime_shadow_recovery_follow.png`
- `parent_review_02_current_central_plaza_realtime_shadow_recovery_floor.png`
- `parent_review_03_current_central_plaza_realtime_shadow_recovery_facade.png`
- `parent_review_04_current_library_realtime_shadow_recovery_guard.png`

## Visual Gate

- Central plaza follow shot should show the plaza/facade shadow field, not a blank wall.
- Realtime cast shadows should be visible without the old giant painted sun patch.
- Floor/facade screenshots should regain structural dark shapes without full-screen haze or camera-space shadow paint.
- Library guard should remain visually unchanged.
