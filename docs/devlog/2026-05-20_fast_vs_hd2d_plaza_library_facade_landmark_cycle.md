# 2026-05-20 Fast VS HD2D Plaza Library Facade Landmark Cycle

## Purpose

Improve the central plaza library exterior so it reads more clearly as the plaza landmark from normal gameplay camera distance. This cycle stays deterministic and uses geometry/material polish only. It does not change story, dialogue, font, map-transition logic, routes, triggers, or gameplay flow.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

## Implementation Summary

- Added `CreateCentralPlazaLibraryFacadeLandmarkPolish(...)` and called it from `CreateCentralPlaza(...)` immediately after the existing facade architecture polish pass.
- Added small non-colliding cube details for the current and past plaza library facade.
- Current-side details added:
  - `Current_CentralPlaza_LibraryFacadeLandmark_DoorNailLeftA`
  - `Current_CentralPlaza_LibraryFacadeLandmark_DoorNailRightA`
  - `Current_CentralPlaza_LibraryFacadeLandmark_LeftWindowCrackA`
  - `Current_CentralPlaza_LibraryFacadeLandmark_RightWindowCrackA`
  - `Current_CentralPlaza_LibraryFacadeLandmark_BaseDustScatterA`
  - `Current_CentralPlaza_LibraryFacadeLandmark_WestWingShadowBandA`
- Past-side details added:
  - `Past_CentralPlaza_LibraryFacadeLandmark_DoorNailLeftA`
  - `Past_CentralPlaza_LibraryFacadeLandmark_DoorNailRightA`
  - `Past_CentralPlaza_LibraryFacadeLandmark_LeftWindowGlintA`
  - `Past_CentralPlaza_LibraryFacadeLandmark_RightWindowGlintA`
  - `Past_CentralPlaza_LibraryFacadeLandmark_BaseTileHighlightA`
  - `Past_CentralPlaza_LibraryFacadeLandmark_EastWingTrimBandA`
- Added `ValidateFastVsHd2dThirtyNinthCyclePlazaLibraryFacadeLandmark()` and wired it into `ValidateHouseSliceBatch()`.
- Added `CaptureHd2dThirtyNinthCycleScreenshotsBatch()` plus a dedicated capture helper for the new landmark screenshots.
- Kept all new objects parented under the plaza map roots, collider-free, and tagged with `TimeWindowPairedSpaceLandmarkKind.PropOrFeature`.

## Validation Commands Ran

- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle39_worker_validate_20260520.log`
- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dThirtyNinthCycleScreenshotsBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle39_worker_capture_20260520.log`
- Result: both commands passed.

## Screenshot Full Paths

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_facade_landmark_20260520\01_current_plaza_library_facade_landmark.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_facade_landmark_20260520\02_past_plaza_library_facade_landmark.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_facade_landmark_20260520\03_current_plaza_library_door_window_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_facade_landmark_20260520\04_past_plaza_library_door_window_close.png`

## Notes

- No external, Meshy, or paid assets were used in this cycle.
- The new pieces are intentionally thin and low-profile so they do not block movement or cover the plaza route glow pads.
