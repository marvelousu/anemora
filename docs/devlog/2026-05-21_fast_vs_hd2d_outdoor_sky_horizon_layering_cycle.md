# 2026-05-21 Fast VS HD2D Outdoor Sky Horizon Layering Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Scene target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- Screenshot output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_horizon_layering_20260521`

This cycle continues the user's added sky/background task. The implementation deliberately stays conservative: it avoids large visible backdrop panels and only keeps a low-alpha horizon layer near the existing outdoor far edge.

No Meshy/API token, paid asset purchase, external download, or new third-party asset was used.

## Worker Cycle

- Worker: `gpt-5.4-mini` session `019e492a-d336-72f3-ba7f-04f81afff44e`.
- Worker instruction: add a small sky/horizon layering pass using the existing sky wash infrastructure; do not touch story, Time Window, movement, route coordinates, map transitions, or colliders.
- Parent review/fix:
  - Removed new upper-air wisp objects because they could read as rectangular sky panels.
  - Added lower-alpha detail/horizon material wrappers using the existing sky wash textures.
  - Lowered generated sky wash texture alpha so existing outdoor sky wash/back-panel elements blend into the clear color instead of reading as crude rectangles.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Wired `CreateOutdoorSkyHorizonLayeringPolish(...)` after the existing `CreateOutdoorSkyDetailPolish(...)` pass for:
  - House exterior current/past maps.
  - Central plaza current/past maps.
- Added low-alpha horizon ridge objects:
  - `Current_HouseExterior_OutdoorSkyHorizonLayering_HorizonSoftRidgeA`
  - `Past_HouseExterior_OutdoorSkyHorizonLayering_HorizonSoftRidgeA`
  - `Current_CentralPlaza_OutdoorSkyHorizonLayering_HorizonSoftRidgeA`
  - `Past_CentralPlaza_OutdoorSkyHorizonLayering_HorizonSoftRidgeA`
- Added `EnsureHd2dOutdoorSkyHorizonLayerMaterial(...)`.
- Reused existing sky wash textures through separate detail/horizon layer materials.
- Tuned `EnsureHd2dOutdoorSkyWashTexture(...)` alpha output downward for safer background blending.
- Added `ValidateFastVsHd2dNinetyThirdCycleOutdoorSkyHorizonLayering()`.
- Added `ValidateHd2dOutdoorSkyHorizonLayeringObject(...)`.
- Added `CaptureHd2dNinetyThirdCycleScreenshotsBatch()` and `CaptureHd2dNinetyThirdCycleScreenshotsToDirectory(...)`.

The Unity scene was regenerated so the new visual-only horizon objects are present in the checked-in scene. Generated material assets for the new detail/horizon wrappers are included because the scene references them.

## Validation

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle93_outdoor_sky_horizon_layering_parent_validate_alpha_texture_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle93_outdoor_sky_horizon_layering_parent_validate_tuned_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle93_outdoor_sky_horizon_layering_parent_capture_alpha_texture_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle93_outdoor_sky_horizon_layering_parent_capture_tuned_20260521.log`
- Result: passed with `Fast VS ninety-third-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_horizon_layering_20260521`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle93_outdoor_sky_horizon_layering_parent_build_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`, `Build Finished, Result: Success.`, and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle93_outdoor_sky_horizon_layering_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_horizon_layering_20260521\01_current_house_exterior_sky_horizon_layering.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_horizon_layering_20260521\02_past_house_exterior_sky_horizon_layering.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_horizon_layering_20260521\03_current_central_plaza_sky_horizon_layering.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_horizon_layering_20260521\04_past_central_plaza_sky_horizon_layering.png`

## Notes

- This pass intentionally favors safety over visibility. The sky remains mostly the camera clear color with subtle far-edge toning, not a painted backdrop.
- All new horizon objects are visual-only, collider-free, and non-arrival landmarks.
