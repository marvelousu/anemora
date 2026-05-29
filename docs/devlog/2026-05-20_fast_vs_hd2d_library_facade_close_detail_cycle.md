# 2026-05-20 Fast VS HD2D Library Facade Close Detail Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_facade_close_detail_20260520`

This cycle adds small non-colliding close-view details to the central-plaza library exterior. It preserves story, dialogue, font, UI, Time Window behavior, route triggers, door/area transitions, camera runtime logic, player/character animation, map extents, and movement glow behavior.

Meshy, API generation, external assets, and paid assets were not used. This pass was a low-risk local geometry/detail pass on existing facade parts.

## Implementation

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateCentralPlazaLibraryFacadeCloseDetails(...)`.
- Added current-side worn details around the library entrance: kick plate, dust lines, threshold crack, and stone chip.
- Added past-side maintained details around the same facade: kick plate, small door studs, warm window trim, and threshold tiles.
- Added `ValidateFastVsHd2dEighteenthCycleLibraryFacadeCloseDetails()` and wired it into `ValidateHouseSliceBatch()`.
- Added `CaptureHd2dEighteenthCycleScreenshotsBatch()` and a matching output directory helper.
- Parent removed an overly dark current-side scratch detail after screenshot review because it read as an accidental black bar.

Representative added objects:

- `Current_CentralPlaza_LibraryFacadeCloseDetail_DoorKickPlate`
- `Current_CentralPlaza_LibraryFacadeCloseDetail_LeftWindowDustLine`
- `Current_CentralPlaza_LibraryFacadeCloseDetail_RightWindowDustLine`
- `Current_CentralPlaza_LibraryFacadeCloseDetail_ThresholdCrackA`
- `Current_CentralPlaza_LibraryFacadeCloseDetail_ThresholdStoneChipA`
- `Past_CentralPlaza_LibraryFacadeCloseDetail_DoorKickPlate`
- `Past_CentralPlaza_LibraryFacadeCloseDetail_DoorStudLeft`
- `Past_CentralPlaza_LibraryFacadeCloseDetail_DoorStudRight`
- `Past_CentralPlaza_LibraryFacadeCloseDetail_LeftWindowWarmTrim`
- `Past_CentralPlaza_LibraryFacadeCloseDetail_RightWindowWarmTrim`
- `Past_CentralPlaza_LibraryFacadeCloseDetail_ThresholdTileA`
- `Past_CentralPlaza_LibraryFacadeCloseDetail_ThresholdTileB`

## Verification Plan

- Validate with Unity batch mode and write the worker log to `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle18_worker_validate_20260520.log`.
- Capture screenshots with Unity batch mode and write the worker log to `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle18_worker_capture_20260520.log`.
- Re-run parent validation after review fixes and write the final validation log to `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle18_parent_validate3_20260520.log`.
- Re-run parent screenshot capture after review fixes and write the final capture log to `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle18_parent_capture3_20260520.log`.
- Confirm required facade close-detail objects exist, remain collider-free, keep `TimeWindowPairedSpaceLandmarkKind.PropOrFeature`, and are parented under the correct central plaza map root.
- Confirm the current library door does not regress to the black doorway material and that library route glow pads remain present.

## Verification Results

- Worker validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle18_worker_validate_20260520.log`
- Worker validation result: passed.
- Worker screenshot capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle18_worker_capture_20260520.log`
- Worker screenshot capture result: passed.
- Parent validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle18_parent_validate3_20260520.log`
- Parent validation result: passed.
- Parent screenshot capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle18_parent_capture3_20260520.log`
- Parent screenshot capture result: passed.
- Parent build log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle18_build_20260520.log`
- Parent build result: passed, with `Build Finished, Result: Success.`
- Player smoke log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle18_player_smoke_20260520.log`
- Player smoke result: passed. The process was stopped after 20 seconds as planned and produced `match_count=0` for the runtime error scan.

Final screenshot folder:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_facade_close_detail_20260520`

Final screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_facade_close_detail_20260520\01_current_library_facade_door_close_detail.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_facade_close_detail_20260520\02_current_library_facade_window_close_detail.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_facade_close_detail_20260520\03_past_library_facade_door_close_detail.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_facade_close_detail_20260520\04_past_library_facade_window_close_detail.png`

## Parent Review

- Current door close shot is intentionally restrained: the door remains readable and no black panel/bar regression is present.
- Past door close shot keeps the warm maintained read without introducing oversized glowing marks.
- Current/past window close shots retain the existing window textures and sill geometry while adding small local detail.
- Unity batchmode still emits the licensing access-token warning and `LogAssemblyErrors` timing lines in this environment; these are known benign log noise for this worktree.
