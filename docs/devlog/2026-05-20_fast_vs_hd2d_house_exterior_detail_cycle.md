# 2026-05-20 Fast VS HD2D House Exterior Detail Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_detail_20260520`

This cycle adds a small HD-2D polish pass to the house exterior only. The scope is limited to the porch, yard, and the small non-colliding ornament line near the north-east road shoulder. It does not change story, dialogue, font, UI, Time Window behavior, route triggers, door/area transitions, camera runtime logic, character animation, or collider behavior.

## Implementation

- Added six thin non-colliding exterior prop details for current/past house exteriors.
- Kept the new objects as `TimeWindowPairedSpaceLandmarkKind.PropOrFeature` and used existing materials only.
- Reused the current scene builder patterns and kept the door, glow pads, route triggers, and road layout untouched.
- Added a sixteenth-cycle validation pass in `ValidateHouseSliceBatch()`.
- Added a sixteenth-cycle screenshot batch for current/past porch and road review images.

Representative added objects:

- `Current_HouseExterior_PropDetail_PorchPebbleA`
- `Current_HouseExterior_PropDetail_DoorstepDustA`
- `Current_HouseExterior_PropDetail_NorthEastRoadLeafA`
- `Past_HouseExterior_PropDetail_PorchFlowerA`
- `Past_HouseExterior_PropDetail_DoorstepPetalA`
- `Past_HouseExterior_PropDetail_NorthEastRoadLeafA`

## Verification Plan

- Validate with Unity batch mode and write the log to `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle16_worker_validate_20260520.log`.
- Capture screenshots with Unity batch mode and write the log to `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle16_worker_capture_20260520.log`.
- Confirm the new objects exist, keep renderers/materials, remain collider-free, stay tagged as prop/feature landmarks, and keep very low Y thickness.
- Confirm the existing house exterior map-move glow pads, door-entry small glow, to-plaza glow pads, and door-closed panels remain present.

## Notes

- Meshy was not used.
- No API or paid external assets were used.
- The new details are intended as thin visual evidence only, not gameplay surfaces.

## Verification

- Validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle16_worker_validate_20260520.log`
- Result: passed.
- Screenshot capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle16_worker_capture_20260520.log`
- Result: passed.
- Screenshot folder: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_detail_20260520`
- Captured screenshots:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_detail_20260520\01_current_house_exterior_porch_detail.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_detail_20260520\02_current_house_exterior_road_detail.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_detail_20260520\03_past_house_exterior_porch_detail.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_detail_20260520\04_past_house_exterior_road_detail.png`

## Parent Review

- Reviewed the four generated screenshots and accepted the pass as a small, non-blocking exterior detail layer.
- Confirmed the porch, door area, north-east road, and map-move glows remain readable.
- Parent build log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle16_build_20260520.log`
- Parent build result: passed, with `Build Finished, Result: Success.`
- Player smoke log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle16_player_smoke_20260520.log`
- Player smoke result: passed. The process was stopped after 20 seconds as planned and produced `match_count=0` for the runtime error scan.
- Known benign log noise: Unity batchmode still emits the licensing access-token warning and `LogAssemblyErrors` timing lines in this environment.
