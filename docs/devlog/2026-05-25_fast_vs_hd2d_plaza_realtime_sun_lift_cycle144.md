# feat(hd2d): lift plaza realtime sun response

## Intent

The realtime shadows now track correctly, but the lit side still needed a stronger sun read. Cycle 144 raises the central-plaza realtime receiver sun response without returning to painted light patches, and commits the material default serialization required by the new shadow texture shader property.

## Scope

- Increase central-plaza runtime `_DirectionalLightStrength` property blocks to `0.70`.
- Raise central-plaza runtime `_SurfaceRampStrength` and warm top-light color.
- Expand the shader UI range for `_DirectionalLightStrength` so the runtime value is within the material contract.
- Persist `_ShadowTextureStrength: 0` on generated material assets so Unity no longer leaves material defaults dirty after batch runs.
- Preserve realtime caster geometry, corrected camera framing, and no-fog/no-painted-overlay constraints.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 144 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeSunLiftCycle144Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaRealtimeSunLiftCycle144ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_realtime_sun_lift_cycle144.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle144_plaza_realtime_sun_lift_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_follow_start.png`
- `parent_review_02_current_central_plaza_follow_forward.png`
- `parent_review_03_current_central_plaza_realtime_shadow_floor.png`
- `parent_review_04_current_central_plaza_realtime_shadow_facade.png`
- `parent_review_05_current_library_guard.png`

## Visual Gate

- Sunlit plaza surfaces should read warmer and brighter against the realtime shadows.
- Shadow/light should still be driven by the main directional light and receiver shader response.
- Batch run should not leave material assets dirty after the material default serialization is committed.
