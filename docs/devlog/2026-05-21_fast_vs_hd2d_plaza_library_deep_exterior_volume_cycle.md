# 2026-05-21 Fast VS HD2D Plaza Library Deep Exterior Volume Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Scene target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- Screenshot output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_deep_exterior_volume_20260521`

This cycle addresses the added task to make the central plaza library exterior read less like a flat facade and more like a building that extends backward within the current plaza map range. The sky/background side of the same user request was handled in the immediately preceding outdoor sky horizon layering cycle.

No Meshy/API token, paid asset purchase, external download, or new third-party asset was used.

## Worker Cycle

- Worker: `gpt-5.4-mini` session `019e492a-d336-72f3-ba7f-04f81afff44e`.
- Worker instruction: add only visual, non-colliding, current/past-matched plaza library exterior volume cues; do not touch story, Time Window, movement, route lights, map transitions, or UI.
- Parent review: checked current/past overview and current/past oblique screenshots, then adjusted only the Cycle94 review camera so the side-depth evidence is readable instead of a roof close-up.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Wired `CreateCentralPlazaLibraryDeepExteriorVolumePolish(...)` into `CreateCentralPlaza(...)` after the front-depth readability polish.
- Added current-side non-colliding visual objects:
  - `Current_CentralPlaza_LibraryDeepExteriorVolume_RearHallWallA`
  - `Current_CentralPlaza_LibraryDeepExteriorVolume_WestLongSideWallA`
  - `Current_CentralPlaza_LibraryDeepExteriorVolume_EastLongSideWallA`
  - `Current_CentralPlaza_LibraryDeepExteriorVolume_RearRoofSlabA`
  - `Current_CentralPlaza_LibraryDeepExteriorVolume_WestRoofRunA`
  - `Current_CentralPlaza_LibraryDeepExteriorVolume_EastRoofRunA`
  - `Current_CentralPlaza_LibraryDeepExteriorVolume_RearBaseContactShadowA`
  - `Current_CentralPlaza_LibraryDeepExteriorVolume_WestSidePilasterA`
  - `Current_CentralPlaza_LibraryDeepExteriorVolume_EastSidePilasterA`
  - `Current_CentralPlaza_LibraryDeepExteriorVolume_RearWallCrackDustA`
- Added past-side equivalents under `Past_CentralPlaza_LibraryDeepExteriorVolume_*`, including `Past_CentralPlaza_LibraryDeepExteriorVolume_RearWindowWarmHintA`.
- Added `ValidateFastVsHd2dNinetyFourthCyclePlazaLibraryDeepExteriorVolume()`.
- Added `ValidateCentralPlazaLibraryDeepExteriorVolumeObject(...)`.
- Added `CaptureHd2dNinetyFourthCycleScreenshotsBatch()` and `CaptureHd2dNinetyFourthCycleScreenshotsToDirectory(...)`.

The Unity scene was regenerated so the new visual-only objects are present in the checked-in scene.

## Validation

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle94_plaza_library_deep_exterior_volume_parent_validate_retry_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle94_plaza_library_deep_exterior_volume_parent_capture_final_retry2_20260521.log`
- Result: passed with `Fast VS ninety-fourth-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_deep_exterior_volume_20260521`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle94_plaza_library_deep_exterior_volume_parent_build_20260521.log`
- Result: passed with `Build Finished, Result: Success.` and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle94_plaza_library_deep_exterior_volume_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_deep_exterior_volume_20260521\01_current_plaza_library_deep_exterior_volume_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_deep_exterior_volume_20260521\02_past_plaza_library_deep_exterior_volume_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_deep_exterior_volume_20260521\03_current_plaza_library_deep_exterior_volume_oblique.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_deep_exterior_volume_20260521\04_past_plaza_library_deep_exterior_volume_oblique.png`

## Notes

- The new volume cues are visual-only and non-colliding.
- The route glow, library entrance, map transition, story flags, Time Window logic, and player placement were not changed.
- Unity batchmode produced transient Addressables/ProjectSettings/importer changes during validation and build; these were excluded from the commit so only the intended script, regenerated scene, devlog, and screenshots remain.
