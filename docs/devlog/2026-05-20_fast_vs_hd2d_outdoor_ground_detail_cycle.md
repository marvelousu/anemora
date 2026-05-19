# 2026-05-20 Fast VS HD2D Outdoor Ground Detail Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Public baseline not touched: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample`
- Main implementation file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

This cycle adds low-risk outdoor ground detail to reduce empty-looking exterior space around the house and central plaza. It stays on the ground plane, avoids new background layers, and keeps route geometry, Time Window behavior, story flow, dialogue, UI, and collider logic unchanged.

## Implementation

Updated the house slice editor builder in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added small non-colliding outdoor ground detail clusters for the house exterior and central plaza.
- Added `CreateOutdoorGroundDetailCluster(...)` so the added detail stays shallow, deterministic, and easy to validate by name.
- Added current/past detail placements near the house front, northeast road shoulder, yard edge, plaza stone-square edge, fountain perimeter, notice board, and library approach.
- Added `ValidateFastVsHd2dEleventhCycleOutdoorGroundDetails()` and wired it into `ValidateHouseSliceBatch()`.
- Added `CaptureHd2dEleventhCycleScreenshotsBatch()` for review evidence.
- Kept route glow pads present and checked them in validation.

Representative added objects:

- `Current_HouseExterior_GroundDetail_FrontYardPebble`
- `Current_HouseExterior_GroundDetail_NorthEastRoadShoulder`
- `Current_HouseExterior_GroundDetail_GardenEdgeLeaf`
- `Past_HouseExterior_GroundDetail_SideYardBloom`
- `Current_CentralPlaza_GroundDetail_FountainSideDust`
- `Past_CentralPlaza_GroundDetail_FountainSideLeaf`
- `Current_CentralPlaza_GroundDetail_LibraryApproachChip`
- `Past_CentralPlaza_GroundDetail_LibraryApproachChip`

## Screenshot Evidence

Output directory:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_ground_detail_20260520`

Captured files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_ground_detail_20260520\01_interior_niro_shadow.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_ground_detail_20260520\02_exterior_niro_shadow.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_ground_detail_20260520\03_library_reto_desk.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_ground_detail_20260520\04_library_reto_talk_loop.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_ground_detail_20260520\05_library_past_no_temp_people.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_ground_detail_20260520\06_library_dialogue_tmp_font.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_ground_detail_20260520\07_plaza_library_facade_current.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_ground_detail_20260520\08_plaza_library_facade_past.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_ground_detail_20260520\09_library_timewriter_pocket_glow.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_ground_detail_20260520\10_library_current_yellow_timewindow_cues.png`

## Verification

Worker validation:

- Worker: `019e41ad-3060-77d3-8f24-60861b993611` (`Boole`, gpt-5.4-mini).
- Validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle11_worker_validate_20260520.log`
- Result: passed.
- Screenshot capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle11_worker_capture_20260520.log`
- Result: passed.

Parent review and verification:

- Parent rejected/removed unrelated generated material side effects and an out-of-scope grass sampler change, keeping this commit focused on outdoor detail placement.
- Parent validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle11_validate_parent_20260520.log`
- Result: passed.
- Parent screenshot capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle11_capture_parent_20260520.log`
- Result: passed.
- Build log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle11_build_20260520.log`
- Result: success.
- Build output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Player smoke log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle11_player_smoke_20260520.log`
- Result: launched for 20 seconds and was intentionally stopped; checked error-pattern match count was 0.

## Notes

- Meshy/API and paid external assets were not used.
- No paid asset looked necessary for this small cycle. Paid/external assets should be reserved for a larger replacement pass where one consistent pack can cover multiple props or surfaces without style mismatch.
- Unity batch execution left normal scene/ProjectSettings/Addressables side effects during validation and build; those side effects were restored or removed before commit selection.
