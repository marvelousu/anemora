# feat(hd2d): restore plaza sky reveal camera

## Intent

The central plaza had drifted away from the VS framing and the sky was reduced to a solid clear color while lighting work focused on map-space haze and overlays. Cycle 156 changes the runtime basis instead: the plaza follow camera is farther, higher, and slightly wider, and the plaza sky is supplied by a runtime skybox while the realtime Directional Light, cookie, and shadow casters remain active.

## Scope

- Move only the central-plaza follow profile to a sky-revealing VS framing: higher/farther camera, higher look target, and FOV 39.
- Replace the central-plaza solid sky clear with `FastVS_CentralPlazaRuntimeSkyboxCycle156` from the realtime light/shadow rig.
- Update validations so the current target is realtime sky, realtime Directional Light shadows, and disabled painted air/sun plates rather than another haze overlay.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 156 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSkyRevealCameraCycle156Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaSkyRevealCameraCycle156ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_sky_reveal_camera_cycle156.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle156_plaza_sky_reveal_camera_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_follow_start.png`
- `parent_review_02_current_central_plaza_follow_forward.png`
- `parent_review_03_current_central_plaza_realtime_shadow_floor.png`
- `parent_review_04_current_central_plaza_realtime_shadow_facade.png`
- `parent_review_05_current_library_guard.png`

## Visual Gate

- The first plaza screenshots should show more roof/upper facade/sky, closer to the old VS composition.
- The sky should come from the runtime skybox, not map-space sky/haze plates.
- Realtime Directional Light shadows, cookie, visible casters, and trimmed `ShadowsOnly` casters must remain active.
