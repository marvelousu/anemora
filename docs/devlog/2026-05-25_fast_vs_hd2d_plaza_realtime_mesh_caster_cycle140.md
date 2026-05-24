# feat(hd2d): add plaza realtime mesh casters

## Intent

Cycle 139 made more central-plaza surfaces receive realtime light and shadow, but the shadow shapes still read too rectangular because most casters were cube slabs. Cycle 140 replaces that failure mode with actual irregular mesh casters that are invisible in the camera but cast realtime shadows from the directional light.

## Scope

- Add 10 current-central-plaza `ShadowsOnly` mesh casters with ragged canopy, broken slat, and branch-fork silhouettes.
- Keep the Cycle 137 VS follow camera framing, Cycle 138 realtime dapple set, and Cycle 139 receiver/material response.
- Keep painted fog, haze, sun patch, and floor shadow overlays disabled.
- Validate that the new cycle objects are mesh-backed, collider-free, enabled, and non-receiving realtime shadow casters.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 140 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeMeshCasterCycle140Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaRealtimeMeshCasterCycle140ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_realtime_mesh_caster_cycle140.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle140_plaza_realtime_mesh_caster_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_follow_start.png`
- `parent_review_02_current_central_plaza_follow_forward.png`
- `parent_review_03_current_central_plaza_realtime_shadow_floor.png`
- `parent_review_04_current_central_plaza_realtime_shadow_facade.png`
- `parent_review_05_current_library_guard.png`

## Visual Gate

- New dark shapes must come from realtime caster geometry, not map-painted haze or screen plates.
- Floor/facade shadows should have less slab-like rectangular edges.
- Camera framing and sky should remain at the Cycle 137 corrected VS follow view.
