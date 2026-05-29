# 2026-05-20 Fast VS HD2D Current Library Ruin Floor Detail Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Main implementation file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

This cycle adds current-side library floor clutter and ruin detail only. The goal was to make the current library feel less empty and more decayed while keeping every new object low, non-blocking, and outside the existing story flow, Time Window flow, controls, font selection, and map transition behavior.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateCurrentLibraryRuinFloorDetailPolish(...)` and called it from the current-library branch of `CreateLibrary(...)`.
- Added six current-side floor detail objects under `Current_LibraryMap_SeparateSpace`:
  - `Current_Library_RuinFloorDetail_DustMatCenterA`
  - `Current_Library_RuinFloorDetail_PaperFanNearRetoA`
  - `Current_Library_RuinFloorDetail_BrokenPlankNearEntryA`
  - `Current_Library_RuinFloorDetail_StoneChipsEastA`
  - `Current_Library_RuinFloorDetail_BookPageTrailWestA`
  - `Current_Library_RuinFloorDetail_LowShadowUnderDebrisA`
- Kept the additions non-colliding by building them with the existing landmark cube helper and disabling colliders.
- Added `ValidateFastVsHd2dFortySecondCycleCurrentLibraryRuinFloorDetail()` and wired it into `ValidateHouseSliceBatch()`.
- Added `CaptureHd2dFortySecondCycleScreenshotsBatch()` and `CaptureHd2dFortySecondCycleScreenshotsToDirectory(...)`.

Validation kept the existing story/map objects in place, including:

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
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-20_fast_vs_hd2d_current_library_ruin_floor_detail_cycle.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

## Validation

Worker validation:

1. Unity validation command:

   `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle42_worker_validate_20260520.log'`

   Result: passed. The log contains `Fast VS house slice validation passed.`

2. Screenshot capture command:

   `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dFortySecondCycleScreenshotsBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle42_worker_capture_20260520.log'`

   Result: passed. The log contains `Fast VS forty-second-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_ruin_floor_detail_20260520`

3. Validation log:

   `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle42_worker_validate_20260520.log`

4. Screenshot capture log:

   `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle42_worker_capture_20260520.log`

Parent review validation:

1. Unity validation command:

   `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle42_parent_validate_20260520.log'`

   Result: passed. The log contains `Fast VS house slice validation passed.`

2. Screenshot capture command:

   `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dFortySecondCycleScreenshotsBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle42_parent_capture_20260520.log'`

   Result: passed. The log contains `Fast VS forty-second-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_ruin_floor_detail_20260520`.

3. Build command:

   `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle42_parent_build_20260520.log'`

   Result: passed. The log contains `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`.

4. Player smoke command:

   `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe -batchmode -nographics -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle42_parent_smoke_20260520.log`

   Result: passed. The process was stopped after 20 seconds and the runtime log scan returned `match_count=0` for error/exception/missing-reference patterns.

## External Assets

No external assets, Meshy assets, or paid assets were used. The pass uses existing generated/procedural Unity primitives and the local material set already in the project.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_ruin_floor_detail_20260520\01_current_library_ruin_floor_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_ruin_floor_detail_20260520\02_current_library_ruin_floor_entry_detail.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_ruin_floor_detail_20260520\03_current_library_ruin_floor_reto_detail.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_ruin_floor_detail_20260520\04_current_library_ruin_floor_west_detail.png`

## Risks / Next Checks

- The new floor clutter is intentionally thin and low, but it should still be checked in the editor against player pathing and the Reto desk sightline.
- If the next polish pass needs stronger readability, the best follow-up is tiny placement tuning rather than adding more geometry.
- The current-side-only scope was preserved; the past library was left unchanged except for the shared validation checks.
