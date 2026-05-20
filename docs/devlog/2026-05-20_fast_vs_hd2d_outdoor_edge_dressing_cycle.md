# 2026-05-20 Fast VS HD2D Outdoor Edge Dressing Cycle

## Scope
- Added low-wall, hedge, and boundary dressing to the outdoor edge of the house exterior and central plaza.
- Kept the existing route flow, time-window interactions, camera behavior, player setup, dialogue, and collider layout unchanged.
- Did not add any sky background, backdrop plane, gradient, or large sky texture.
- Used only the existing in-repo materials and existing capture helpers.

## Implementation
- Added `CreateHouseExteriorEdgeDressing(...)` and `CreateCentralPlazaEdgeDressing(...)` in `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`.
- Added the required non-colliding props/features:
  - `Current_HouseExterior_EdgeDressing_NorthHedgeA`
  - `Current_HouseExterior_EdgeDressing_NorthHedgeB`
  - `Current_HouseExterior_EdgeDressing_WestFenceShadow`
  - `Current_HouseExterior_EdgeDressing_RoadEdgeLowWall`
  - `Past_HouseExterior_EdgeDressing_NorthHedgeA`
  - `Past_HouseExterior_EdgeDressing_NorthHedgeB`
  - `Past_HouseExterior_EdgeDressing_WestFenceShadow`
  - `Past_HouseExterior_EdgeDressing_RoadEdgeLowWall`
  - `Current_CentralPlaza_EdgeDressing_WestLowWall`
  - `Current_CentralPlaza_EdgeDressing_EastLowWall`
  - `Current_CentralPlaza_EdgeDressing_NorthTreeLineA`
  - `Current_CentralPlaza_EdgeDressing_NorthTreeLineB`
  - `Past_CentralPlaza_EdgeDressing_WestLowWall`
  - `Past_CentralPlaza_EdgeDressing_EastLowWall`
  - `Past_CentralPlaza_EdgeDressing_NorthTreeLineA`
  - `Past_CentralPlaza_EdgeDressing_NorthTreeLineB`
- Added `ValidateFastVsHd2dTwentyThirdCycleOutdoorEdgeDressing()` and `ValidateOutdoorEdgeDressingObject(...)`.
- Added `CaptureHd2dTwentyThirdCycleScreenshotsBatch()` and `CaptureHd2dTwentyThirdCycleScreenshotsToDirectory(...)`.
- Parent review moved the central-plaza tree-line blocks to the visible side gaps instead of behind the library mass, and changed the house-exterior screenshots away from the over-close roof/ground angle used in the worker pass.

## Verification
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle23_worker_validate_20260520.log` - passed with `Fast VS house slice validation passed.`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle23_worker_capture_20260520.log` - passed and wrote the 4 screenshot files under `docs/devlog/screenshots/fast_vs_hd2d_outdoor_edge_dressing_20260520/`.
- Parent validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle23_parent_validate_20260520.log`
- Parent validation result: passed with `Fast VS house slice validation passed.`
- Parent capture logs:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle23_parent_capture_20260520.log`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle23_parent_capture_retry_20260520.log`
- Parent capture result: passed and regenerated the 4 screenshot files under `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_edge_dressing_20260520`.
- Build log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle23_build_20260520.log`
- Build result: `Build Finished, Result: Success.`
- Player smoke log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle23_player_smoke_20260520.log`
- Player smoke result: 20 second headless run, stopped intentionally, `match_count=0`.

## Notes
- The new edge dressing objects are `TimeWindowPairedSpaceLandmarkKind.PropOrFeature` and have no colliders.
- The existing route glow pads were preserved and validated.
- The generated scene file was restored after Unity verification if it changed during batch capture.
- No external free or paid assets were used in this cycle.
