# 2026-05-22 Fast VS HD2D Outdoor Scenic Backdrop Foundation Cycle 29

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-shading-foundation-20260522`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Audit writer: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dOutdoorScenicBackdropFoundationAudit.cs`
- Report path: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_scenic_backdrop_foundation_cycle29_20260522\outdoor_scenic_backdrop_foundation_cycle29_20260522.md`

This cycle addresses the outdoor map edge problem directly. The goal was to stop the house exterior and central plaza from feeling like they open into empty backstage space, and to replace the remaining gap read with readable sky, haze, treeline, and distant roofline layers.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateOutdoorScenicBackdropFoundation(...)` and wired it into both `CreateExterior(...)` and `CreateCentralPlaza(...)`.
- Added current/past backdrop objects for both areas:
  - House exterior: sky curtain, low haze band, distant tree line, left sky wrap, right sky wrap.
  - Central plaza: sky curtain, low haze band, distant roofline, left sky wrap, right sky wrap.
- Kept the new pieces non-colliding, non-arrival, and `TimeWindowPairedSpaceLandmarkKind.PropOrFeature`.
- Avoided `materials.Shadow` for the backdrop itself so the result reads as scenery rather than a blackout slab.
- Added `EnsureHd2dOutdoorScenicBackdropMaterial(...)` and `EnsureHd2dOutdoorScenicBackdropTexture(...)` for the new procedural backdrop layer.
- Added `ValidateFastVsHd2dTwentyNinthCycleOutdoorScenicBackdropFoundation()` and a dedicated scenic backdrop object validator to `ValidateHouseSliceBatch()`.
- Parent review correction: the first screenshot still read as dark void in the outdoor corners, so the runtime outdoor camera background colors in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsHouseLightingDirector.cs` were raised to muted sky blue-gray for HouseExterior and CentralPlaza only. The interior dark clear color was left unchanged.

Added `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dOutdoorScenicBackdropFoundationAudit.cs`:

- Added `WriteOutdoorScenicBackdropFoundationCycle29ReportBatch()`.
- The report writer calls `CreateHouseSliceScene()` and `ValidateHouseSliceBatch()`.
- The generated report records the branch, worktree, full report path, object count, current/past HouseExterior names, current/past CentralPlaza names, and PASS/FAIL status.

## Validation

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Assets/Editor/AnemoraFastVsHd2dOutdoorScenicBackdropFoundationAudit.cs docs/devlog/2026-05-22_fast_vs_hd2d_outdoor_scenic_backdrop_foundation_cycle29.md docs/devlog/INDEX.md`
- Unity report batch:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dOutdoorScenicBackdropFoundationAudit.WriteOutdoorScenicBackdropFoundationCycle29ReportBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_scenic_backdrop_foundation_cycle29_report_worker_20260522.log`
- Unity validation batch:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_scenic_backdrop_foundation_cycle29_validate_worker_20260522.log`
- Visual snapshot batch:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_scenic_backdrop_foundation_cycle29_capture_worker_20260522.log`

## Result

- The outdoor edges now read as actual atmosphere and distant structure instead of a hollow hole behind the map.
- Current and past house exterior / central plaza both carry a readable scenic backdrop layer.
- Outdoor clear color now supports the scenic panels instead of fighting them with a near-black background.
- Residual risk remains that this is still a procedural backdrop foundation rather than an authored horizon set, so the far edge is improved but not final-art quality.
