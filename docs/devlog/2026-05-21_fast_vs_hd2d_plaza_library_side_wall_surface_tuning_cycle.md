# 2026-05-21 Fast VS HD2D Plaza Library Side Wall Surface Tuning Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Scene target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- Screenshot output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_side_wall_surface_tuning_20260521`

This cycle follows the deep exterior volume pass by reducing the newly readable side wall's flat/black-band feel with small surface courses, panel chips, and roof-under-edge cues. It keeps the library footprint, transition, and gameplay contracts unchanged.

No Meshy/API token, paid asset purchase, external download, or new third-party asset was used.

## Worker Cycle

- Worker: `gpt-5.4-mini` session `019e492a-d336-72f3-ba7f-04f81afff44e`.
- Worker instruction: add only visual, non-colliding, current/past-matched plaza library side-wall surface cues; do not touch story, Time Window, movement, route lights, map transitions, UI, font, or input.
- Parent review: checked current/past oblique screenshots and accepted the pass as a restrained cleanup that does not create new large panels.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Wired `CreateCentralPlazaLibrarySideWallSurfaceTuningPolish(...)` into `CreateCentralPlaza(...)` after `CreateCentralPlazaLibraryDeepExteriorVolumePolish(...)`.
- Added current-side non-colliding visual objects:
  - `Current_CentralPlaza_LibrarySideWallSurfaceTuning_WestLongStoneCourseA`
  - `Current_CentralPlaza_LibrarySideWallSurfaceTuning_WestLongStoneCourseB`
  - `Current_CentralPlaza_LibrarySideWallSurfaceTuning_EastReturnStoneCourseA`
  - `Current_CentralPlaza_LibrarySideWallSurfaceTuning_EastReturnStoneCourseB`
  - `Current_CentralPlaza_LibrarySideWallSurfaceTuning_WestPanelBreakDustA`
  - `Current_CentralPlaza_LibrarySideWallSurfaceTuning_EastPanelBreakDustA`
  - `Current_CentralPlaza_LibrarySideWallSurfaceTuning_WestRoofSideUnderShadowA`
  - `Current_CentralPlaza_LibrarySideWallSurfaceTuning_EastRoofSideUnderShadowA`
- Added past-side equivalents under `Past_CentralPlaza_LibrarySideWallSurfaceTuning_*`.
- Added `ValidateFastVsHd2dNinetyFifthCyclePlazaLibrarySideWallSurfaceTuning()`.
- Added `ValidateCentralPlazaLibrarySideWallSurfaceTuningObject(...)`.
- Added `CaptureHd2dNinetyFifthCycleScreenshotsBatch()` and `CaptureHd2dNinetyFifthCycleScreenshotsToDirectory(...)`.

The Unity scene was regenerated so the new visual-only objects are present in the checked-in scene.

## Validation

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle95_plaza_library_side_wall_surface_tuning_parent_validate_retry2_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle95_plaza_library_side_wall_surface_tuning_parent_capture_20260521.log`
- Result: passed with `Fast VS ninety-fifth-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_side_wall_surface_tuning_20260521`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle95_plaza_library_side_wall_surface_tuning_parent_build_retry_20260521.log`
- Result: passed with `Build Finished, Result: Success.` and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle95_plaza_library_side_wall_surface_tuning_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_side_wall_surface_tuning_20260521\01_current_plaza_library_side_wall_surface_tuning_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_side_wall_surface_tuning_20260521\02_past_plaza_library_side_wall_surface_tuning_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_side_wall_surface_tuning_20260521\03_current_plaza_library_side_wall_surface_tuning_oblique.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_side_wall_surface_tuning_20260521\04_past_plaza_library_side_wall_surface_tuning_oblique.png`

## Notes

- The pass is intentionally restrained. It adds side-wall surface breakup without changing the silhouette, route glow, transition trigger, story, Time Window logic, or collision.
- Unity batchmode again produced transient Addressables/ProjectSettings/importer changes during validation and build; these were excluded from the commit so only the intended script, regenerated scene, devlog, and screenshots remain.
