# 2026-05-21 Fast VS HD2D Outdoor Sky And Plaza Library Volume Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_and_plaza_library_volume_20260521`

This cycle implements the user-added HD-2D outdoor tasks:

- Create/improve outdoor sky/background.
- Make the central plaza library read as a three-dimensional building by extending it backward within the current map bounds.

No paid asset, no API token, and no external download was used.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Wired a new outdoor sky-detail pass after `CreateOutdoorSkyWashTreatment(...)` for both house exterior and central plaza.
- Wired a new central plaza library rear-volume pass after `CreateCentralPlazaLibraryExteriorDepthPolish(...)` and before `CreateCentralPlazaLibraryApproachHd2dPolish(...)`.
- Added `CreateOutdoorSkyDetailPolish(...)`.
- Added `CreateCentralPlazaLibraryRearVolumePolish(...)`.
- Added `ValidateFastVsHd2dSeventySeventhCycleOutdoorSkyAndLibraryVolume()`.
- Added `ValidateOutdoorSkyDetailObject(...)`.
- Added `ValidateCentralPlazaLibraryRearVolumeObject(...)`.
- Added `CaptureHd2dSeventySeventhCycleScreenshotsBatch()`.
- Added `CaptureHd2dSeventySeventhCycleScreenshotsToDirectory(...)`.

New visual-only objects:

- `Current_HouseExterior_OutdoorSkyDetail_CloudWispA`
- `Current_HouseExterior_OutdoorSkyDetail_FarRidgeA`
- `Past_HouseExterior_OutdoorSkyDetail_CloudWispA`
- `Past_HouseExterior_OutdoorSkyDetail_FarRidgeA`
- `Current_CentralPlaza_OutdoorSkyDetail_CloudRakeA`
- `Current_CentralPlaza_OutdoorSkyDetail_CloudRakeB`
- `Current_CentralPlaza_OutdoorSkyDetail_DistantRooflineA`
- `Past_CentralPlaza_OutdoorSkyDetail_CloudRakeA`
- `Past_CentralPlaza_OutdoorSkyDetail_CloudRakeB`
- `Past_CentralPlaza_OutdoorSkyDetail_DistantRooflineA`
- `Current_CentralPlaza_LibraryRearVolume_BackWallMassA`
- `Current_CentralPlaza_LibraryRearVolume_BackRoofCapA`
- `Current_CentralPlaza_LibraryRearVolume_BackEaveShadowA`
- `Current_CentralPlaza_LibraryRearVolume_WestDepthFaceA`
- `Current_CentralPlaza_LibraryRearVolume_EastDepthFaceA`
- `Current_CentralPlaza_LibraryRearVolume_RearGroundShadowA`
- `Current_CentralPlaza_LibraryRearVolume_RearDustBreakA`
- `Past_CentralPlaza_LibraryRearVolume_BackWallMassA`
- `Past_CentralPlaza_LibraryRearVolume_BackRoofCapA`
- `Past_CentralPlaza_LibraryRearVolume_BackEaveShadowA`
- `Past_CentralPlaza_LibraryRearVolume_WestDepthFaceA`
- `Past_CentralPlaza_LibraryRearVolume_EastDepthFaceA`
- `Past_CentralPlaza_LibraryRearVolume_RearGroundShadowA`
- `Past_CentralPlaza_LibraryRearVolume_WarmBackWindowHintA`

## Validation

Worker validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle77_outdoor_sky_and_plaza_library_volume_worker_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Worker screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle77_outdoor_sky_and_plaza_library_volume_worker_capture_20260521.log`
- Result: passed with `Fast VS seventy-seventh-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_and_plaza_library_volume_20260521`

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle77_outdoor_sky_and_plaza_library_volume_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle77_outdoor_sky_and_plaza_library_volume_parent_capture_20260521.log`
- Result: passed with `Fast VS seventy-seventh-cycle screenshots captured`.

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle77_outdoor_sky_and_plaza_library_volume_parent_build_20260521.log`
- Result: passed with `Build Finished, Result: Success.`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle77_outdoor_sky_and_plaza_library_volume_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_and_plaza_library_volume_20260521\01_current_central_plaza_library_volume_sky_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_and_plaza_library_volume_20260521\02_past_central_plaza_library_volume_sky_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_and_plaza_library_volume_20260521\03_current_house_exterior_sky_detail_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_and_plaza_library_volume_20260521\04_past_house_exterior_sky_detail_overview.png`

## Notes

- The cycle stayed in deterministic geometry/material polish only.
- No gameplay scripts, dialogue, Time Window behavior, UI/font, map transition triggers, colliders, player/character animation, or stable generated asset/material files were intentionally edited. The new sky details reuse the existing sky-wash material path.
