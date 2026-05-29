# 2026-05-20 Fast VS HD2D Current Library Wall Shelf Depth Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Main implementation file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

This cycle adds current-side library wall and shelf depth polish only. The goal was to break up the large flat wall and shelf planes with non-blocking shadow and dust bands plus broken shelf lips, while keeping story, dialogue, controls, Time Window behavior, and map transition behavior unchanged.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateCurrentLibraryWallShelfDepthPolish(...)` and called it only from the current-library branch of `CreateLibrary(...)`.
- Added eight current-library wall/shelf depth objects under `Current_LibraryMap_SeparateSpace`:
  - `Current_Library_WallShelfDepth_BackUpperShadowBand`
  - `Current_Library_WallShelfDepth_BackLowerDustBand`
  - `Current_Library_WallShelfDepth_LeftWallDustStrip`
  - `Current_Library_WallShelfDepth_RightWallDustStrip`
  - `Current_Library_WallShelfDepth_BackShelfBrokenLipLeftA`
  - `Current_Library_WallShelfDepth_BackShelfBrokenLipRightA`
  - `Current_Library_WallShelfDepth_LeftSideShelfFloorShadowA`
  - `Current_Library_WallShelfDepth_RightSideShelfFloorShadowA`
- Added `ValidateFastVsHd2dFortyThirdCycleCurrentLibraryWallShelfDepth()` and wired it into `ValidateHouseSliceBatch()`.
- Added `CaptureHd2dFortyThirdCycleScreenshotsBatch()` and `CaptureHd2dFortyThirdCycleScreenshotsToDirectory(...)`.

The current-side story objects were kept in place, including:

- `Current_Library_TimeWindowOpenCue_Book`
- `Current_Library_TimeWindowOpenCue_Aria`
- `Past_Library_TargetBook_ForPickup`
- `Past_Library_AriaIdleAtTable`
- `Current_Library_RetoDeskBook_Initial`
- `Current_Library_ReturnedBookOnDesk`
- `FastVS_Reto_WritingAtDesk`
- `Current_Library_ToCentralPlaza_MapMoveGlowPad`
- `Past_Library_ToCentralPlaza_MapMoveGlowPad`

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-20_fast_vs_hd2d_current_library_wall_shelf_depth_cycle.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_wall_shelf_depth_20260520\01_current_library_wall_shelf_depth_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_wall_shelf_depth_20260520\02_current_library_back_wall_depth_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_wall_shelf_depth_20260520\03_current_library_left_shelf_shadow_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_wall_shelf_depth_20260520\04_current_library_right_shelf_shadow_close.png`

## Validation

Worker validation:

1. Unity validation command:

   `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle43_worker_validate_20260520.log'`

   Result: failed during worker handoff review. The log reports a validator-coordinate mismatch for `Current_Library_WallShelfDepth_BackUpperShadowBand`; the object was generated at `LibraryVsCenter + offset`, while the validator compared against the raw offset.

2. Screenshot capture command:

   `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dFortyThirdCycleScreenshotsBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle43_worker_capture_20260520.log'`

   Result: passed. The command exited 0 and the log ended with `Fast VS forty-third-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_wall_shelf_depth_20260520`

3. Validation log:

   `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle43_worker_validate_20260520.log`

4. Screenshot capture log:

   `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle43_worker_capture_20260520.log`

Parent review validation:

1. Unity validation command:

   `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle43_parent_validate_20260520.log'`

   Result: passed. The log contains `Fast VS house slice validation passed.`

2. Screenshot capture command:

   `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dFortyThirdCycleScreenshotsBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle43_parent_capture_20260520.log'`

   Result: passed. The log contains `Fast VS forty-third-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_wall_shelf_depth_20260520`.

3. Build command:

   `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle43_parent_build_20260520.log'`

   Result: passed. The log contains `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`.

4. Player smoke command:

   `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe -batchmode -nographics -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle43_parent_smoke_20260520.log`

   Result: passed. The process was stopped after 20 seconds and the runtime log scan returned `match_count=0` for error/exception/missing-reference patterns.

## External Assets

No external assets, Meshy assets, or paid assets were used. The pass uses existing procedural primitives and the local material set already in the project.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_wall_shelf_depth_20260520\01_current_library_wall_shelf_depth_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_wall_shelf_depth_20260520\02_current_library_back_wall_depth_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_wall_shelf_depth_20260520\03_current_library_left_shelf_shadow_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_wall_shelf_depth_20260520\04_current_library_right_shelf_shadow_close.png`

## Risks / Next Checks

- Parent review corrected the validator to compare against `LibraryVsCenter + offset`, then reran validation, screenshots, build, and player smoke before commit.
- The new wall and shelf depth pieces are intentionally thin and low; the next review should confirm they improve depth without crowding the shelf silhouettes.
- The current-side-only scope was preserved. Past-library layout and story-critical objects were left unchanged.
