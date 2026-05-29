# 2026-05-20 Fast VS HD2D House Interior Life Props Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_interior_life_props_20260520`

This cycle increases the small-prop density around Niro's house interior by adding non-colliding life props near the bed and the small table. It keeps story, dialogue, UI, Time Window behavior, player setup, route markers, camera behavior, and collider layout unchanged.

## Implementation

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CaptureHd2dTwentySecondCycleScreenshotsBatch()`.
- Added `CaptureHd2dTwentySecondCycleScreenshotsToDirectory(...)`.
- Added `ValidateFastVsHd2dTwentySecondCycleHouseInteriorLifeProps()` and `ValidateHouseInteriorLifePropObject(...)`.
- Added five current-side and five past-side life props under the existing interior map roots.
- Kept every new object non-colliding and tagged as `TimeWindowPairedSpaceLandmarkKind.PropOrFeature`.
- Reused existing helper construction paths for thin prop detail slabs and landmark cubes.
- Parent review adjusted the current-side bedside rug slightly forward and changed its material to `current_bed` so it does not disappear into the floor and bed shadow in close review.

Representative added objects:

- `Current_HouseInterior_LifeProp_BedsideRug`
- `Current_HouseInterior_LifeProp_TableInkCup`
- `Current_HouseInterior_LifeProp_TableBrush`
- `Current_HouseInterior_LifeProp_BookPageMarker`
- `Current_HouseInterior_LifeProp_PillowCreaseB`
- `Past_HouseInterior_LifeProp_BedsideRug`
- `Past_HouseInterior_LifeProp_TableInkCup`
- `Past_HouseInterior_LifeProp_TableBrush`
- `Past_HouseInterior_LifeProp_BookPageMarker`
- `Past_HouseInterior_LifeProp_PillowCreaseB`

State split used in-scene:

- Current side is a little more dusty and slightly more askew.
- Past side is cleaner and more orderly.

## Verification

- Validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle22_worker_validate_20260520.log`
- Result: passed.
- Screenshot capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle22_worker_capture_20260520.log`
- Result: passed.
- Screenshot output directory: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_interior_life_props_20260520`
- Captured screenshots:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_interior_life_props_20260520\01_current_house_bedside_life_props.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_interior_life_props_20260520\02_current_house_table_life_props.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_interior_life_props_20260520\03_past_house_bedside_life_props.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_interior_life_props_20260520\04_past_house_table_life_props.png`
- Parent validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle22_parent_validate_20260520.log`
- Parent validation result: passed.
- Parent screenshot capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle22_parent_capture_20260520.log`
- Build log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle22_build_20260520.log`
- Build result: `Build Finished, Result: Success.`
- Player smoke log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle22_player_smoke_20260520.log`
- Player smoke result: 20 second headless run, stopped intentionally, `match_count=0`.

## Notes

- Meshy was not used.
- No external paid assets were used.
- The new interior details rely on existing generated materials and thin prop composition, which kept the change fast and low-risk.
- Unity batch startup logged the usual licensing access-token warning, but validation and screenshot capture still completed successfully.
