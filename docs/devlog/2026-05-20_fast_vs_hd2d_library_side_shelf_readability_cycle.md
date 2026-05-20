# 2026-05-20 Fast VS HD2D Library Side Shelf Readability Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Main implementation file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Devlog output directory: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_side_shelf_readability_20260520\`

This cycle keeps the current/past library side shelf roots and colliders intact, but improves how the shelf faces read from the side review angles. Current-side shelves now read as dark, decayed wood with narrow recesses, dust, broken boards, residual books, and paper slips instead of large gray slab placeholders. Past-side shelves now read as organized bookshelves with clearer vertical spine rhythm and stronger row separation.

## Implementation Summary

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Tightened `CreateCurrentLibraryEmptySideBookshelf(...)` detail geometry so the current-side missing-gap markers are narrow dark recesses instead of flat blocky slabs.
- Kept the current-side shelf root positions/rotations unchanged:
  - `Current_Library_LeftSideBookshelf`
  - `Current_Library_RightSideBookshelf`
  - `Past_Library_LeftSideBookshelf`
  - `Past_Library_RightSideBookshelf`
- Reworked `SampleCurrentEmptyBookshelfFrontHd2dPixel(...)` to push the current empty shelf texture toward darker warm wood, clearer horizontal ledges, deeper recesses, and small dust/paper/chip accents.
- Adjusted `SampleBookShelfTexturePixel(...)` and `GetBookSpineWidth(...)` so `bookshelf_front_painted_hd2d` reads with narrower, more legible book spines and cleaner row seams.
- Added `ValidateFastVsHd2dThirtyFourthCycleLibrarySideShelfReadability()` and wired it into `ValidateHouseSliceBatch()`.
- Added `ValidateCurrentLibrarySideShelfGapDetailBounds(...)` and `ValidateCurrentShelfGapDetail(...)` so the current-side missing-gap markers stay small and dark.
- Added `CaptureHd2dThirtyFourthCycleScreenshotsBatch()` and `CaptureHd2dThirtyFourthCycleScreenshotsToDirectory(...)` for the new review set.

## Changed Textures

- `current_empty_bookshelf_front_hd2d` at `256x128`
- `bookshelf_front_painted_hd2d` at `256x128`

## Changed Scene Objects

Current side shelf detail objects adjusted in `CreateCurrentLibraryEmptySideBookshelf(...)`:

- `Current_Library_LeftSideBookshelf_MissingBookGapA`
- `Current_Library_LeftSideBookshelf_MissingBookGapB`
- `Current_Library_RightSideBookshelf_MissingBookGapA`
- `Current_Library_RightSideBookshelf_MissingBookGapB`
- `Current_Library_LeftSideBookshelf_BrokenBoardA`
- `Current_Library_LeftSideBookshelf_BrokenBoardB`
- `Current_Library_LeftSideBookshelf_BrokenBoardC`
- `Current_Library_RightSideBookshelf_BrokenBoardA`
- `Current_Library_RightSideBookshelf_BrokenBoardB`
- `Current_Library_RightSideBookshelf_BrokenBoardC`
- `Current_Library_LeftSideBookshelf_ResidualBook_0`
- `Current_Library_LeftSideBookshelf_ResidualBook_1`
- `Current_Library_RightSideBookshelf_ResidualBook_0`
- `Current_Library_RightSideBookshelf_ResidualBook_1`
- `Current_Library_LeftSideBookshelf_PaperSlip_0`
- `Current_Library_LeftSideBookshelf_PaperSlip_1`
- `Current_Library_RightSideBookshelf_PaperSlip_0`
- `Current_Library_RightSideBookshelf_PaperSlip_1`

## Verification

1. Validation command:

   `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle34_worker_validate_20260520.log'`

   Result: first worker pass failed on an over-strict `bookshelf front shelf band contrast` sample. The final retry validation is recorded at `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle34_retry_worker_validate_20260520.log` and passed with `Fast VS house slice validation passed.`

2. Screenshot capture command:

   `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dThirtyFourthCycleScreenshotsBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle34_worker_capture_20260520.log'`

   Result: passed. The log contains `Fast VS thirty-fourth-cycle screenshots captured:`.

3. Screenshot evidence files:

   - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_side_shelf_readability_20260520\01_current_left_side_shelf_readability.png`
   - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_side_shelf_readability_20260520\02_current_right_side_shelf_readability.png`
   - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_side_shelf_readability_20260520\03_past_left_side_shelf_readability.png`
   - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_side_shelf_readability_20260520\04_past_right_side_shelf_readability.png`

4. Parent validation rerun:

   `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle34_parent_validate_20260520.log`

   Result: passed. The log contains `Fast VS house slice validation passed.`

5. Parent screenshot rerun:

   `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle34_parent_capture_20260520.log`

   Result: passed. The log contains `Fast VS thirty-fourth-cycle screenshots captured:`.

6. Parent player build:

   `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle34_build_20260520.log`

   Result: passed. The log contains `Build Finished, Result: Success.` and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`.

7. Parent player smoke:

   `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle34_player_smoke_20260520.log`

   Result: passed. The player was run for 20 seconds in `-batchmode -nographics`; expected manual stop was `stopped=True`, and the error-pattern scan returned `match_count=0`.

## Meshy / API / External Assets

- Meshy/API/paid external assets not used.

## Notes

- The first validation pass caught an existing width guard on bookshelf-front spine widths; I adjusted the front distribution back into the validator's 5-14 px range and reran validation.
- The final current-right screenshot was nudged deeper behind the shelf so the review frame would stay on the shelf face instead of a player/debug cube.
- No character/story/dialogue/Time Window/portal/movement code was changed.
