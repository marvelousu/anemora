# 2026-05-21 Fast VS HD2D Library Wall Plane Dressing Cycle

## Scope

- Cycle55: library wall plane dressing
- Goal: break up the large flat wall surfaces, shelf contact zones, and window recesses in the library without touching dialogue, Time Window behavior, map movement, fonts, characters, or story flags.
- No external or paid assets were adopted. The pass uses existing materials plus small cube/slab dressing only.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateLibraryWallPlaneDressing(...)` and called it from `CreateLibrary(...)` after the side bookshelves are created.
- Added eight current/past wall-plane dressing objects under `Current_LibraryMap_SeparateSpace` and `Past_LibraryMap_SeparateSpace`:
  - `Current_Library_WallPlane_BackUpperDepthBandA`
  - `Current_Library_WallPlane_BackShelfContactShadowA`
  - `Current_Library_WallPlane_BackPilasterLeftA`
  - `Current_Library_WallPlane_BackPilasterRightA`
  - `Current_Library_WallPlane_LeftWallBaseShadowA`
  - `Current_Library_WallPlane_RightWallBaseShadowA`
  - `Current_Library_WallPlane_LeftWindowRecessA`
  - `Current_Library_WallPlane_RightWindowRecessA`
  - and the matching `Past_` versions of the same eight objects
- Added `ValidateFastVsHd2dFiftyFifthCycleLibraryWallPlaneDressing()` and `ValidateLibraryWallPlaneDressingObject(...)`, and wired the validation into `ValidateHouseSliceBatch()`.
- Added `CaptureHd2dFiftyFifthCycleScreenshotsBatch()` and `CaptureHd2dFiftyFifthCycleScreenshotsToDirectory(...)`.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-21_fast_vs_hd2d_library_wall_plane_dressing_cycle.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_wall_plane_dressing_20260521\`

## Validation

Worker validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle55_library_wall_plane_worker_validate_20260521.log`

Worker capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle55_library_wall_plane_worker_capture_20260521.log`

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle55_library_wall_plane_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent capture logs:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle55_library_wall_plane_parent_capture_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle55_library_wall_plane_parent_capture2_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle55_library_wall_plane_parent_capture3_20260521.log`
- Note: the first side-wall close capture placed the camera under the gallery, so the parent session changed the side-wall evidence frames to normal review-camera screenshots and regenerated the final screenshot set with `parent_capture3`.

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle55_library_wall_plane_parent_build_20260521.log`
- Result: validation passed and `Build Finished, Result: Success.`

Parent player smoke:

- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle55_library_wall_plane_parent_smoke_20260521.log`
- Result: stopped after 20 seconds, `match_count=0`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_wall_plane_dressing_20260521\01_current_library_back_wall_plane_dressing.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_wall_plane_dressing_20260521\02_past_library_back_wall_plane_dressing.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_wall_plane_dressing_20260521\03_current_library_side_wall_plane_dressing.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_wall_plane_dressing_20260521\04_past_library_side_wall_plane_dressing.png`

## External Assets

No external or paid assets were used.

## Residual Risk

- The new dressing is intentionally thin and should stay non-blocking, but the final visual balance still depends on the screenshot/camera composition and the library wall depth already present in the scene.
