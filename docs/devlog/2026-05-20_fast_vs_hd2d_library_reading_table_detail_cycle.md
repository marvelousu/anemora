# 2026-05-20 Fast VS HD2D Library Reading Table Detail Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Public baseline not touched: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample`
- Main implementation file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

This cycle increases the HD-2D density around the library reading tables. The goal is for the table area itself to read as a lived-in library reading space, without changing story, dialogue, Time Window behavior, player coordinates, or the existing book pickup/return flags.

## Implementation

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateLibraryReadingSeatPair(...)` for low bench/seat pairs that track each reading table with table-relative offsets.
- Added table-top detail children inside `CreateLibraryReadingTableAssembly(...)`:
  - `*_TabletopPlankSeamA`
  - `*_TabletopPlankSeamB`
  - `*_TabletopPlankSeamC`
  - `*_LeftEdgeHighlight`
  - `*_RightEdgeHighlight`
  - `*_FrontThicknessRim`
  - `*_RearThicknessRim`
- Wired the seat pair helper into the current library tables:
  - `Current_Library_ReadingTableLong`
  - `Current_Library_ReadingTableSideA`
  - `Current_Library_ReadingTableSideB`
- Wired the seat pair helper into the past clean reading tables through `CreatePastLibraryCleanReadingTable(...)`.
- Added `ValidateFastVsHd2dTwentyNinthCycleLibraryReadingTableDetails()` and called it from `ValidateHouseSliceBatch()`.
- Added `ValidateLibraryReadingTableDetailObject(...)` and `ValidateLibraryReadingSeatVisualObject(...)` to verify collider-free visual detail placement.
- Added `CaptureHd2dTwentyNinthCycleScreenshotsBatch()` and `CaptureHd2dTwentyNinthCycleScreenshotsToDirectory(...)`.

Representative added object names:

- `Current_Library_ReadingTableLong_TabletopPlankSeamA`
- `Current_Library_ReadingTableLong_LeftEdgeHighlight`
- `Current_Library_ReadingTableLong_FrontBenchSeat`
- `Current_Library_ReadingTableSideA_FrontBenchSeat`
- `Current_Library_ReadingTableSideB_RearBenchShadow`
- `Past_Library_ReadingTableClean_LeftFront_TabletopPlankSeamA`
- `Past_Library_ReadingTableClean_LeftFront_FrontBenchSeat`
- `Past_Library_ReadingTableClean_CenterFront_RearBenchSeat`
- `Past_Library_ReadingTableClean_RightRear_LeftEdgeHighlight`

## Screenshot Evidence

Output directory:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_reading_tables_20260520`

Captured files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_reading_tables_20260520\01_current_library_reto_table_details.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_reading_tables_20260520\02_past_library_clean_reading_tables_details.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_reading_tables_20260520\03_current_library_side_table_details.png`

## Verification

- `git -C 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' status --short --branch`
- `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle29_worker_validate_20260520.log'`
  - Result: passed
- `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dTwentyNinthCycleScreenshotsBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle29_worker_capture_20260520.log'`
  - Result: passed
- Parent validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle29_parent_validate_20260520.log`
  - Result: passed.
- Parent screenshot capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle29_parent_capture_20260520.log`
  - Result: passed.
- Build log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle29_build_20260520.log`
  - Result: success. The log contains `Build Finished, Result: Success.`
- Build output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Player smoke log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle29_player_smoke_20260520.log`
  - Result: launched for 20 seconds and was intentionally stopped; checked error-pattern match count was 0.

## Notes

- Meshy/API/paid external assets were not used.
- Existing current-side Reto desk books, past-side pickup books, and NoStep colliders remained in place.
- The new seat pair objects are collider-free and do not alter player movement.
