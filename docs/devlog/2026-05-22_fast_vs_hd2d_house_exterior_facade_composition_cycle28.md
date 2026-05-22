# 2026-05-22 Fast VS HD2D House Exterior Facade Composition Cycle 28

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-shading-foundation-20260522`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Audit writer: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dHouseExteriorFacadeCompositionAudit.cs`
- Report path: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_facade_composition_cycle28_20260522\house_exterior_facade_composition_cycle28_20260522.md`

This cycle tightens the Niro house exterior so it reads as a complete outside-facing building instead of a shell with visible behind-the-scenes gaps. The pass keeps the gameplay route, dialogue, Time Window flow, movement, input, fonts, and existing door/glow interactions unchanged.

No external or paid assets were used. The work uses the existing Fast VS materials plus small non-colliding landmark cubes.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Wired `CreateHouseExteriorFacadeCompositionPolish(...)` into `CreateExterior(...)` after `CreateHouseExteriorOcclusionShell(...)`.
- Added current/past facade composition pieces:
  - `Current_HouseExterior_FacadeComposition_DoorLeftReturnWallA`
  - `Current_HouseExterior_FacadeComposition_DoorRightReturnWallA`
  - `Current_HouseExterior_FacadeComposition_RightWallVerticalTrimA`
  - `Current_HouseExterior_FacadeComposition_RightWallBaseTrimA`
  - `Current_HouseExterior_FacadeComposition_PorchPostBackTrimA`
  - `Current_HouseExterior_FacadeComposition_RightWallMiddleBreakLineA`
  - `Current_HouseExterior_FacadeComposition_RightWallUpperBreakLineA`
  - `Current_HouseExterior_FacadeComposition_UnderRoofDepthShadowA`
  - `Current_HouseExterior_FacadeComposition_RoofSideRakeLineA`
  - `Current_HouseExterior_FacadeComposition_BackdropSideMaskLeftA`
  - `Current_HouseExterior_FacadeComposition_BackdropSideMaskRightA`
  - `Past_HouseExterior_FacadeComposition_DoorLeftReturnWallA`
  - `Past_HouseExterior_FacadeComposition_DoorRightReturnWallA`
  - `Past_HouseExterior_FacadeComposition_RightWallVerticalTrimA`
  - `Past_HouseExterior_FacadeComposition_RightWallBaseTrimA`
  - `Past_HouseExterior_FacadeComposition_PorchPostBackTrimA`
  - `Past_HouseExterior_FacadeComposition_RightWallMiddleBreakLineA`
  - `Past_HouseExterior_FacadeComposition_RightWallUpperBreakLineA`
  - `Past_HouseExterior_FacadeComposition_UnderRoofDepthShadowA`
  - `Past_HouseExterior_FacadeComposition_RoofSideRakeLineA`
  - `Past_HouseExterior_FacadeComposition_BackdropSideMaskLeftA`
  - `Past_HouseExterior_FacadeComposition_BackdropSideMaskRightA`
- Kept the new pieces non-colliding, non-arrival, and shadow-safe by using `CreateNonArrivalLandmarkCubeShadowSafe(...)`.
- Used current/past wall, fence, stone, shadow, and dust materials only; no new external or paid assets were introduced in this corrective facade pass.
- Added `ValidateFastVsHd2dTwentyEighthCycleHouseExteriorFacadeComposition()` and `ValidateHouseExteriorFacadeCompositionObject(...)`.
- Kept `Current_HouseExterior_DoorClosedPanel`, `Past_HouseExterior_DoorClosedPanel`, `Current_HouseExterior_DoorEntrySmallGlow`, and `Past_HouseExterior_DoorEntrySmallGlow` present in validation.
- Added the new cycle validation call to `ValidateHouseSliceBatch()`.

Parent review correction:

- Rejected the first worker screenshot because `Current_HouseExterior_FacadeComposition_PorchPostBackShadowA` appeared as a heavy black horizontal bar across the door.
- Replaced that piece with narrow current/past fence-trim objects named `..._PorchPostBackTrimA`.
- Added `..._RightWallMiddleBreakLineA` and `..._RightWallUpperBreakLineA` to reduce the remaining large flat right-wall read without covering the door or windows.

Added `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dHouseExteriorFacadeCompositionAudit.cs`:

- Added `WriteHouseExteriorFacadeCompositionCycle28ReportBatch()`.
- The report writer calls `CreateHouseSliceScene()` and `ValidateHouseSliceBatch()`.
- The generated report records the branch, worktree, full report path, additional object count, object names, and PASS/FAIL status.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dHouseExteriorFacadeCompositionAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_house_exterior_facade_composition_cycle28.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_facade_composition_cycle28_20260522\house_exterior_facade_composition_cycle28_20260522.md`

## Validation

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Assets/Editor/AnemoraFastVsHd2dHouseExteriorFacadeCompositionAudit.cs`
  - PASS
- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dHouseExteriorFacadeCompositionAudit.WriteHouseExteriorFacadeCompositionCycle28ReportBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_facade_composition_cycle28_report_worker_20260522.log`
  - PASS: `HD2D house exterior facade composition cycle 28 report written: ...\house_exterior_facade_composition_cycle28_20260522.md`
- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_facade_composition_cycle28_validate_worker_20260522.log`
  - PASS: `Fast VS house slice validation passed.`
- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_facade_composition_cycle28_capture_worker_20260522.log`
  - PASS: `Fast VS HD2D visual snapshot audit passed: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`

## Notes

- The exterior close view is improved after parent correction: the doorway no longer has the worker-introduced black bar, the house reads less like an open shell, and the right-side wall plane now has more breakup.
- The existing visual snapshot audit directory was refreshed in place for the house exterior shot.
- Unity batchmode still touched unrelated imported assets and generated scene/import metadata while validating and capturing. Those are expected side effects and should be cleaned by the parent if needed.

## Residual Risk

- The exterior right wall still relies on simple procedural pieces rather than a authored facade texture/model, so it is structurally improved but not final-quality.
- The far-background mask is intentionally conservative, so it solves the visibility problem without turning the frame into a heavy blackout.
