# 2026-05-21 Fast VS HD2D Outdoor Far-Edge Transition Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_far_edge_transition_20260521`

This cycle adds a small authored transition pass at the far/north edge of the house exterior and central plaza outdoor maps. The intent is to soften the read from playable ground into the outdoor void/sky treatment by adding low-profile berm, grass, paving, stone, and dust shapes at the map edge.

This is not a sky/background replacement. It does not add clouds, camera clear-color changes, large sky boards, new downloaded assets, API assets, story/UI/input changes, route changes, or colliders.

No external or paid asset source was used. The pass relies on existing scene materials and code-generated non-arrival landmark cubes only.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateOutdoorFarEdgeTransitionPolish(...)`.
- Wired it into `CreateExterior(...)` and `CreateCentralPlaza(...)` immediately after `CreateOutdoorSkyWashTreatment(...)`.
- Added the following visual-only objects:
  - `Current_HouseExterior_FarEdgeTransition_BermShadowA`
  - `Current_HouseExterior_FarEdgeTransition_GrassLipA`
  - `Current_HouseExterior_FarEdgeTransition_StoneChipA`
  - `Past_HouseExterior_FarEdgeTransition_BermShadowA`
  - `Past_HouseExterior_FarEdgeTransition_GrassLipA`
  - `Past_HouseExterior_FarEdgeTransition_StoneChipA`
  - `Current_CentralPlaza_FarEdgeTransition_BermShadowA`
  - `Current_CentralPlaza_FarEdgeTransition_PavingLipA`
  - `Current_CentralPlaza_FarEdgeTransition_SideChipA`
  - `Past_CentralPlaza_FarEdgeTransition_BermShadowA`
  - `Past_CentralPlaza_FarEdgeTransition_PavingLipA`
  - `Past_CentralPlaza_FarEdgeTransition_SideChipA`
- Added `ValidateFastVsHd2dSeventyThirdCycleOutdoorFarEdgeTransition()` and `ValidateOutdoorFarEdgeTransitionObject(...)`.
- Added `CaptureHd2dSeventyThirdCycleScreenshotsBatch()` and `CaptureHd2dSeventyThirdCycleScreenshotsToDirectory(...)`.

## Validation

- Worker validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle73_outdoor_far_edge_transition_worker_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`
- Worker screenshot capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle73_outdoor_far_edge_transition_worker_capture_20260521.log`
- Result: passed with `Fast VS seventy-third-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_far_edge_transition_20260521`
- Parent validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle73_outdoor_far_edge_transition_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`
- Parent screenshot capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle73_outdoor_far_edge_transition_parent_capture_20260521.log`
- Result: passed with `Fast VS seventy-third-cycle screenshots captured`.
- Parent build log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle73_outdoor_far_edge_transition_parent_build_20260521.log`
- Result: passed with `Build Finished, Result: Success.`
- Parent smoke log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle73_outdoor_far_edge_transition_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_far_edge_transition_20260521\01_current_house_exterior_far_edge_transition.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_far_edge_transition_20260521\02_past_house_exterior_far_edge_transition.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_far_edge_transition_20260521\03_current_central_plaza_far_edge_transition.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_far_edge_transition_20260521\04_past_central_plaza_far_edge_transition.png`
