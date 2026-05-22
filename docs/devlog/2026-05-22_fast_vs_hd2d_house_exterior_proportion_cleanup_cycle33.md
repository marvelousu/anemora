# 2026-05-22 Fast VS HD2D House Exterior Proportion Cleanup Cycle 33

File: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_house_exterior_proportion_cleanup_cycle33.md`

## Scope
- Tightened the house exterior silhouette so the facade reads as a closed 3D house instead of oversized flat roof and wall boards.
- Kept gameplay, time-window flow, story logic, UI, and interaction routing unchanged.
- Preserved the current/past paired layout and naming style.

## Intent
- Reduce roof dominance from the current snapshot camera.
- Seat the chimney into the roof so it does not read as a floating block.
- Break the front wall into clearer facade planes without opening new holes or black voids.

## Files Changed
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_house_exterior_proportion_cleanup_cycle33.md`

## Implementation
- Added `CreateHouseExteriorProportionCleanupPolish(...)` and called it from `CreateExterior(...)` immediately after `CreateHouseExteriorFacadeBackdropReadabilityPolish(...)`.
- Reduced the size and adjusted the placement of `Current_` / `Past_` `HouseExterior_RoofWidePixelPlane` and `HouseExterior_RoofFrontEave`.
- Reduced the chimney and chimney cap proportions, then added chimney flashing and contact-shadow cleanup pieces.
- Added subtle facade cleanup pieces for the roof lip, mid-wall band, base line, and window frame bands.
- Added `ValidateFastVsHd2dThirtyThirdCycleHouseExteriorProportionCleanup()` and a dedicated helper that validates object name, parent, material token, local position range, local scale range, landmark kind, arrival state, and collider/shadow expectations where applicable.

## Validation
- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_proportion_cleanup_cycle33_validate_worker_20260522.log`
- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_proportion_cleanup_cycle33_capture_worker_20260522.log`
- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_proportion_cleanup_cycle33_validate_parent_20260522.log`
- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_proportion_cleanup_cycle33_capture_parent_20260522.log`

## Result
- `Fast VS house slice validation passed.` in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_proportion_cleanup_cycle33_validate_worker_20260522.log`
- `Fast VS HD2D visual snapshot audit passed: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522` in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_proportion_cleanup_cycle33_capture_worker_20260522.log`
- Parent validation log `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_proportion_cleanup_cycle33_validate_parent_20260522.log` also included `Fast VS house slice validation passed.`
- Parent visual snapshot log `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_proportion_cleanup_cycle33_capture_parent_20260522.log` also included `Fast VS HD2D visual snapshot audit passed: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`.
- Both Unity batch logs included `Licensing::Module` warnings about an unavailable access token, but the runs still completed successfully.
- No new texture or material assets were added in this cycle.
- Parent visual review of `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\02_current_house_exterior_visual_snapshot.png` found that the roof/chimney scale improved slightly, but the house exterior is still not at the target "first glance impresses" quality bar.

## Residual Risk
- The house now reads more like a closed facade, but the final visual balance still depends on the exact review camera and lighting composition.
- The exterior can still feel simple if the wider outdoor sky and backdrop treatment is not aligned with the house silhouette.
- A follow-up cycle should more aggressively rework house exterior silhouette, wall material read, and facade depth rather than only adding cleanup trim.
