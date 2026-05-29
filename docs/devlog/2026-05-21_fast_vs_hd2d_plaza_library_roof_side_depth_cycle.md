# 2026-05-21 Fast VS HD2D Plaza Library Roof Side Depth Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Scene target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- Screenshot output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_roof_side_depth_20260521`

This cycle continues the user's added task to make the central plaza library exterior less like a flat backdrop. Cycle87 added a rear/backward volume layer. Cycle90 adds a second set of visual-only side/roof cues so the roof and side walls read more like a building mass extending backward within the current map bounds.

No Meshy/API token, paid asset purchase, external download, or new third-party asset was used.

## Worker Cycle

- Worker: `gpt-5.4-mini` session `019e492a-d336-72f3-ba7f-04f81afff44e`.
- Worker instruction: add only visual, non-arrival, non-colliding roof/side depth cues near the existing plaza library; do not touch story, Time Window, movement, map transitions, UI, or route coordinates.
- Parent fix: added screenshot existence checks for the two oblique Cycle90 screenshots before validation.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Wired `CreateCentralPlazaLibraryRoofSideDepthPolish(...)` after the existing plaza library rear/readability/backward-volume passes.
- Added current-side non-colliding visual objects:
  - `Current_CentralPlaza_LibraryRoofSideDepth_WestRoofSideBevelStripA`
  - `Current_CentralPlaza_LibraryRoofSideDepth_EastRoofSideBevelStripA`
  - `Current_CentralPlaza_LibraryRoofSideDepth_RearWallBandA`
  - `Current_CentralPlaza_LibraryRoofSideDepth_RearGroundContactStripA`
  - `Current_CentralPlaza_LibraryRoofSideDepth_WestSideReturnSeamA`
  - `Current_CentralPlaza_LibraryRoofSideDepth_EastSideReturnSeamA`
- Added past-side equivalents under `Past_CentralPlaza_LibraryRoofSideDepth_*`.
- Added `ValidateFastVsHd2dNinetiethCyclePlazaLibraryRoofSideDepth()`.
- Added `ValidateCentralPlazaLibraryRoofSideDepthObject(...)`.
- Added `CaptureHd2dNinetiethCycleScreenshotsBatch()` and `CaptureHd2dNinetiethCycleScreenshotsToDirectory(...)`.

The Unity scene was regenerated so the new visual objects are present in the checked-in scene. Generated horizon-depth cleanup material assets are included because the regenerated scene references them.

## Validation

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle90_plaza_library_roof_side_depth_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle90_plaza_library_roof_side_depth_parent_capture_20260521.log`
- Result: passed with `Fast VS ninetieth-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_roof_side_depth_20260521`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle90_plaza_library_roof_side_depth_parent_build_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`, `Build Finished, Result: Success.`, and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Note: Unity emitted `move_path failed: No error`, but the build completed successfully.

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle90_plaza_library_roof_side_depth_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_roof_side_depth_20260521\01_current_plaza_library_roof_side_depth_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_roof_side_depth_20260521\02_past_plaza_library_roof_side_depth_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_roof_side_depth_20260521\03_current_plaza_library_roof_side_depth_oblique.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_roof_side_depth_20260521\04_past_plaza_library_roof_side_depth_oblique.png`

## Notes

- This pass intentionally keeps the plaza footprint and map bounds unchanged.
- The added objects are small, visual-only depth cues. They do not add route blockers, arrival landmarks, story flags, transition pads, or Time Window behavior.
