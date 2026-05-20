# 2026-05-20 Fast VS HD2D Library Floor Decay Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Main implementation file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

This cycle raises the HD-2D density of the library interior floor read. Current-side library space now reads as a damaged but still legible archive floor with thin dust bands, paper bundles, and low wood fragments. Past-side library space stays orderly and only gains a few tidy floor bundles. Story state, dialogue, Time Window behavior, entry/exit flow, desk collisions, movement glow, font/UI, characters, camera, and existing collider/trigger setup were left intact.

## Implementation Summary

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateLibraryFloorDecayDetails(...)` and called it from `CreateLibrary(...)`.
- Kept all added objects collider-free by building them with `CreateLandmarkCube(...)`.
- Added 11 current-side floor detail objects:
  - dust/scuff bands near entry, west shelf, back shelf, east shelf, and center path
  - paper bundles near shelf edges
  - short wood fragments near shelf and back-wall edges
- Added 4 past-side floor detail objects:
  - tidy book/paper bundles near shelves and reading-table edges
- Added `ValidateFastVsHd2dThirtyThirdCycleLibraryFloorDecayDetails()` and wired it into `ValidateHouseSliceBatch()`.
- Added `ValidateLibraryFloorDecayDetailObject(...)` for MeshRenderer, collider, parent, landmark kind, and thin-scale checks.
- Added `CaptureHd2dThirtyThirdCycleScreenshotsBatch()` and `CaptureHd2dThirtyThirdCycleScreenshotsToDirectory(...)`.

## Changed Object Names

Current side:

- `Current_Library_FloorDecay_ScuffBandEntry`
- `Current_Library_FloorDecay_DustBandWest`
- `Current_Library_FloorDecay_ScuffBandBack`
- `Current_Library_FloorDecay_DustBandEast`
- `Current_Library_FloorDecay_ScuffBandCenter`
- `Current_Library_FloorDecay_PageBundleWest`
- `Current_Library_FloorDecay_PageBundleBack`
- `Current_Library_FloorDecay_PageBundleEast`
- `Current_Library_FloorDecay_WoodShardWest`
- `Current_Library_FloorDecay_WoodShardBack`
- `Current_Library_FloorDecay_WoodShardEast`

Past side:

- `Past_Library_OrderedFloorDetail_BookBundleA`
- `Past_Library_OrderedFloorDetail_PaperBundleB`
- `Past_Library_OrderedFloorDetail_BookBundleC`
- `Past_Library_OrderedFloorDetail_PaperBundleD`

Validation preserved existing scene objects, including:

- `Current_Library_PixelFloor`
- `Past_Library_PixelFloor`
- `FastVS_Reto_WritingAtDesk`
- `Past_Library_AriaIdleAtTable`
- `Current_Library_ToCentralPlaza_MapMoveGlowPad`
- `Past_Library_ToCentralPlaza_MapMoveGlowPad`
- `Current_Library_ReturnedBookOnDesk`
- `Past_Library_TargetBook_ForPickup`

## Verification Plan / Results

1. Validation command:

   `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle33_worker_validate_20260520.log'`

   Result: passed. The log contains `Fast VS house slice validation passed.`

2. Screenshot capture command:

   `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dThirtyThirdCycleScreenshotsBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle33_worker_capture_20260520.log'`

   Result: passed. The log contains `Fast VS thirty-third-cycle screenshots captured:` and batchmode exit success.

3. Screenshot evidence files:

   - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_floor_decay_20260520\01_current_library_floor_decay_near_entry.png`
   - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_floor_decay_20260520\02_current_library_floor_decay_reto_side.png`
   - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_floor_decay_20260520\03_current_library_floor_decay_shelf_side.png`
   - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_floor_decay_20260520\04_past_library_ordered_floor_details.png`

4. Parent validation rerun:

   `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle33_parent_validate_20260520.log`

   Result: passed. The log contains `Fast VS house slice validation passed.`

5. Parent screenshot rerun:

   `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle33_parent_capture_20260520.log`

   Result: passed. The log contains `Fast VS thirty-third-cycle screenshots captured:`.

6. Parent player build:

   `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle33_build_20260520.log`

   Result: passed. The log contains `Build Finished, Result: Success.` and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`.

7. Parent player smoke:

   `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle33_player_smoke_20260520.log`

   Result: passed. The player was run for 20 seconds in `-batchmode -nographics`; expected manual stop was `stopped=True`, and the error-pattern scan returned `match_count=0`.

## Meshy / API / External Assets

- Meshy/API/paid external assets not used.

## Notes

- The canonical existing target-book scene object remains `Past_Library_TargetBook_ForPickup`; the validation checks that actual object name.
- No known functional regressions were introduced in the library story setup, movement triggers, or collider layout.
