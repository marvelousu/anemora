# 2026-05-21 Fast VS HD2D Outdoor Sky Atmosphere Depth Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Scene target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- Screenshot output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_atmosphere_depth_20260521`

This cycle adds a restrained outdoor sky atmosphere depth pass for Niro's house exterior and central plaza, then tones the existing outdoor void background slabs so the added sky treatment does not read as rectangular panels.

No Meshy/API token, paid asset purchase, external download, or new third-party asset was used.

## Worker Cycle

- Worker: `gpt-5.4-mini` session `019e492a-d336-72f3-ba7f-04f81afff44e`.
- Worker instruction: add subtle outdoor sky/background depth for Niro's house exterior and the central plaza without replacing the current environment, touching gameplay, story, Time Window behavior, route transitions, route lights, UI, font, input, characters, collisions, or map coordinates.
- Parent review: the first pass created a visible rectangular sky plate in the past house exterior review screenshot. Parent changed the pass from flat white overlay material to a low-alpha generated sky-wash material, then further tinted sky/horizon and existing outdoor void background materials toward the dark background color. The final screenshot no longer shows the obvious rectangle.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateOutdoorSkyAtmosphereDepthPolish(...)` and called it after `CreateOutdoorSkyHorizonLayeringPolish(...)` for:
  - `Current_HouseExteriorMap_SeparateSpace`
  - `Past_HouseExteriorMap_SeparateSpace`
  - `Current_CentralPlazaMap_SeparateSpace`
  - `Past_CentralPlazaMap_SeparateSpace`
- Added low-collision, non-arrival sky atmosphere landmarks:
  - `Current_HouseExterior_OutdoorSkyAtmosphereDepth_MidSkyWashA`
  - `Current_HouseExterior_OutdoorSkyAtmosphereDepth_HorizonHazeBandA`
  - `Past_HouseExterior_OutdoorSkyAtmosphereDepth_MidSkyWashA`
  - `Past_HouseExterior_OutdoorSkyAtmosphereDepth_HorizonHazeBandA`
  - `Current_CentralPlaza_OutdoorSkyAtmosphereDepth_MidSkyWashA`
  - `Current_CentralPlaza_OutdoorSkyAtmosphereDepth_HorizonHazeBandA`
  - `Past_CentralPlaza_OutdoorSkyAtmosphereDepth_MidSkyWashA`
  - `Past_CentralPlaza_OutdoorSkyAtmosphereDepth_HorizonHazeBandA`
- Added `EnsureHd2dOutdoorSkyAtmosphereDepthMaterial(...)`, which reuses the existing generated outdoor sky-wash texture source with very low alpha and background-colored tint.
- Reduced the existing `EnsureHd2dOutdoorSkyHorizonLayerMaterial(...)` tint so horizon layer plates do not read as bright rectangles.
- Toned the existing outdoor void background material colors for house exterior and central plaza so far-background slabs blend into the sky/void background.
- Added `ValidateFastVsHd2dNinetyNinthCycleOutdoorSkyAtmosphereDepth()`.
- Added `ValidateOutdoorSkyAtmosphereDepthObject(...)`, which verifies parent area, non-collision, non-shadow, landmark id prefix, material token, placement range, and scale range.
- Added `CaptureHd2dNinetyNinthCycleScreenshotsBatch()` and `CaptureHd2dNinetyNinthCycleScreenshotsToDirectory(...)`.

Updated/generated material assets under:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice`

The Unity scene was regenerated so the atmosphere objects and toned materials are present in the checked-in scene.

## Validation

Parent validation logs:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle99_outdoor_sky_atmosphere_depth_parent_validate_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle99_outdoor_sky_atmosphere_depth_parent_validate_fix_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle99_outdoor_sky_atmosphere_depth_parent_validate_fix2_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle99_outdoor_sky_atmosphere_depth_parent_validate_fix3_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle99_outdoor_sky_atmosphere_depth_parent_validate_fix4_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle99_outdoor_sky_atmosphere_depth_parent_validate_fix5_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle99_outdoor_sky_atmosphere_depth_parent_validate_fix6_20260521.log`
- Final result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture logs:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle99_outdoor_sky_atmosphere_depth_parent_capture_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle99_outdoor_sky_atmosphere_depth_parent_capture_fix_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle99_outdoor_sky_atmosphere_depth_parent_capture_fix2_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle99_outdoor_sky_atmosphere_depth_parent_capture_fix3_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle99_outdoor_sky_atmosphere_depth_parent_capture_fix4_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle99_outdoor_sky_atmosphere_depth_parent_capture_fix5_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle99_outdoor_sky_atmosphere_depth_parent_capture_fix6_20260521.log`
- Final result: passed with `Fast VS ninety-ninth-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_atmosphere_depth_20260521`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle99_outdoor_sky_atmosphere_depth_parent_build_20260521.log`
- Result: passed with `Build Finished, Result: Success.` and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle99_outdoor_sky_atmosphere_depth_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_atmosphere_depth_20260521\01_current_house_exterior_sky_atmosphere_depth.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_atmosphere_depth_20260521\02_past_house_exterior_sky_atmosphere_depth.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_atmosphere_depth_20260521\03_current_central_plaza_sky_atmosphere_depth.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_atmosphere_depth_20260521\04_past_central_plaza_sky_atmosphere_depth.png`

## Notes

- The user previously rejected a rough outdoor sky/background pass, so this pass intentionally keeps the sky treatment subtle and treats visible rectangular panels as a failure condition.
- The central plaza screenshots are dominated by the library exterior, so this cycle mainly prevents the outdoor void from looking raw when the camera exposes it near the map edges.
- Unity batchmode produced transient Addressables/ProjectSettings/importer changes during validation and build; these were excluded from the commit so only the intended script, regenerated scene, intended materials, devlog, and screenshots remain.
