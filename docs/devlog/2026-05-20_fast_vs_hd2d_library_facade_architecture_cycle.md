# 2026-05-20 Fast VS HD2D Library Facade Architecture Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_facade_architecture_20260520`

This cycle raises the HD-2D density of the central-plaza library facade only. The change is limited to small doorway, window, wall, and facade-decoration geometry. It does not change map size, routes, movement glow pads, time-window window behavior, characters, dialogue, fonts, UI, camera logic, colliders, or triggers.

Meshy/API/external/paid assets were not used. This pass was handled with local geometry only.

## Implementation Summary

- Added `CreateCentralPlazaLibraryFacadeArchitecturePolish(...)` and called it from `CreateCentralPlaza(...)` immediately after the existing facade close-detail pass.
- Added small non-colliding current-side facade polish pieces that read as worn architecture: hinge strips, threshold wear, a threshold chip, dust lines, and wall patch slabs.
- Added small non-colliding past-side facade polish pieces that read as maintained architecture: hinge strips, tiled threshold pieces, window highlights, and tidy wall bands.
- Added `ValidateFastVsHd2dThirtySecondCycleLibraryFacadeArchitecturePolish()` and wired it into `ValidateHouseSliceBatch()`.
- Added `CaptureHd2dThirtySecondCycleScreenshotsBatch()` with a dedicated capture helper for the architecture pass.
- Parent review moved the past-side window highlights into the lit panes and reduced their size so they no longer read as a long bright bar on the window sill.

## Changed Object Names

Current facade polish objects:

- `Current_CentralPlaza_LibraryFacadeArchitecture_DoorHingeLeft`
- `Current_CentralPlaza_LibraryFacadeArchitecture_DoorHingeRight`
- `Current_CentralPlaza_LibraryFacadeArchitecture_ThresholdWearStrip`
- `Current_CentralPlaza_LibraryFacadeArchitecture_ThresholdChipA`
- `Current_CentralPlaza_LibraryFacadeArchitecture_LeftWindowDustLine`
- `Current_CentralPlaza_LibraryFacadeArchitecture_RightWindowDustLine`
- `Current_CentralPlaza_LibraryFacadeArchitecture_LeftWallPatch`
- `Current_CentralPlaza_LibraryFacadeArchitecture_RightWallPatch`

Past facade polish objects:

- `Past_CentralPlaza_LibraryFacadeArchitecture_DoorHingeLeft`
- `Past_CentralPlaza_LibraryFacadeArchitecture_DoorHingeRight`
- `Past_CentralPlaza_LibraryFacadeArchitecture_ThresholdTileA`
- `Past_CentralPlaza_LibraryFacadeArchitecture_ThresholdTileB`
- `Past_CentralPlaza_LibraryFacadeArchitecture_LeftWindowHighlight`
- `Past_CentralPlaza_LibraryFacadeArchitecture_RightWindowHighlight`
- `Past_CentralPlaza_LibraryFacadeArchitecture_LeftWallBand`
- `Past_CentralPlaza_LibraryFacadeArchitecture_RightWallBand`

## Verification Plan / Results

- Validation command:
  `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle32_worker_validate_20260520.log`
- Validation result: passed.
- Validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle32_worker_validate_20260520.log`
- Screenshot command:
  `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dThirtySecondCycleScreenshotsBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle32_worker_capture_20260520.log`
- Screenshot capture result: passed.
- Screenshot capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle32_worker_capture_20260520.log`
- Parent validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle32_parent_validate_20260520.log`
- Parent validation result: passed.
- Parent screenshot capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle32_parent_capture_20260520.log`
- Parent screenshot capture result: passed after reducing and repositioning the past-side pane highlights.
- Parent build log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle32_build_20260520.log`
- Parent build result: `Build Finished, Result: Success.`
- Parent build output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Parent player smoke log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle32_player_smoke_20260520.log`
- Parent player smoke result: launched for 20 seconds and was intentionally stopped; checked error-pattern match count was 0.
- Confirmed in validation that the current and past facade polish pieces exist, stay collider-free, keep `TimeWindowPairedSpaceLandmarkKind.PropOrFeature`, and remain parented directly under `Current_CentralPlazaMap_SeparateSpace` / `Past_CentralPlazaMap_SeparateSpace`.
- Confirmed retained objects in validation: `Current_CentralPlaza_LibraryNorthFacade`, `Past_CentralPlaza_LibraryNorthFacade`, `Current_CentralPlaza_ToLibrary_MapMoveGlowPad`, `Past_CentralPlaza_ToLibrary_MapMoveGlowPad`, `Current_CentralPlaza_LibraryDoorPanelsLeft`, `Past_CentralPlaza_LibraryDoorPanelsLeft`, `Current_CentralPlaza_LibraryWindowLeftPaneUpperLeft`, `Past_CentralPlaza_LibraryWindowLeftPaneUpperLeft`.

## Screenshot Full Paths

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_facade_architecture_20260520\01_current_library_facade_door_architecture.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_facade_architecture_20260520\02_current_library_facade_window_architecture.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_facade_architecture_20260520\03_past_library_facade_door_architecture.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_facade_architecture_20260520\04_past_library_facade_window_architecture.png`

## Meshy / API / External Assets

- Meshy not used.
- API not used.
- External paid assets not used.
- No source, license, or import-path record was needed for this cycle.
