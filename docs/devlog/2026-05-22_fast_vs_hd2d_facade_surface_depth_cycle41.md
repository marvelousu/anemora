# 2026-05-22 Fast VS HD2D Facade Surface Depth Cycle 41

## Scope
- Added a small surface-depth pass for the house exterior and the central plaza library facade.
- Focused on layered shading, eave occlusion, side-return volume, and low contact grounding without changing layout or arrivals.
- Kept the pass non-collider and non-arrival so the new pieces stay visual only.

## Intent
- Make the exterior walls and roof masses read as deliberately lit surfaces instead of flat slabs.
- Preserve door and window readability while pushing a little more dimensionality into the screenshot views.
- Reuse the current outdoor occlusion, cool light, warm light, shadow, wall, trim, and stone material roles so the pass stays consistent with the rest of the house slice.

## Files Changed
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_facade_surface_depth_cycle41.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\02_current_house_exterior_visual_snapshot.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\03_current_central_plaza_visual_snapshot.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\visual_snapshot_metrics_cycle10_20260522.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle41_facade_surface_depth_worker_20260522_01\`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle41_facade_surface_depth_parent_review_20260522_01\`

## Implementation
- Added house-exterior depth pieces for the current and past variants:
  - a subtle under-eave occlusion band
  - a side-return light/shadow plane on the right wall mass
  - a small porch and door contact shadow that leaves the door readable
  - a low ambient stone band near the base
  - a small roof-side catch to keep the upper mass from reading as a single flat cap
- Added plaza library depth pieces for the current and past variants:
  - roof/eave underside shading
  - left and right tower return shading
  - a faint lower facade base shadow
  - a small entry-height light catch to lift the central read
- Reused the existing outdoor occlusion gradient, cool light pool, warm stage light, shadow, wall, trim, roof, and stone roles so the added surfaces stay material-consistent.
- Parent review added a second contact-line pass for the house eave, right wall edge, door lintel, library eave, library inner vertical edges, and library entry step because the first worker pass was visually too restrained in the screenshots.
- Added validation for the new house and plaza facade depth objects and wired it into the house-slice batch validation sequence.

## Validation
- Unity batch house validation:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_facade_surface_depth_cycle41_validate_worker_20260522.log`
  - Result: passed, with `Fast VS house slice validation passed.`
- Unity batch visual snapshot audit:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_facade_surface_depth_cycle41_capture_worker_20260522.log`
  - Result: passed, with `Fast VS HD2D visual snapshot audit passed.`
- Parent Unity batch house validation:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_facade_surface_depth_cycle41_validate_parent_20260522.log`
  - Result: passed, with `Fast VS house slice validation passed.`
- Parent Unity batch visual snapshot audit:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_facade_surface_depth_cycle41_capture_parent_20260522.log`
  - Result: passed, with `Fast VS HD2D visual snapshot audit passed.`

## Output Evidence
- Screenshot directory:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`
- Worker copy:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle41_facade_surface_depth_worker_20260522_01`
- Parent review copy:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle41_facade_surface_depth_parent_review_20260522_01`
- PNGs:
  - `01_current_house_interior_visual_snapshot.png`
  - `02_current_house_exterior_visual_snapshot.png`
  - `03_current_central_plaza_visual_snapshot.png`
  - `04_current_library_visual_snapshot.png`
- Metrics:
  - `visual_snapshot_metrics_cycle10_20260522.md`

## Review Notes
- The house exterior now reads more dimensional in the snapshot, especially around the eave line, right-side mass, door lintel, and porch threshold.
- The plaza library facade also reads more dimensional, with the tower returns, inner edge lines, eave contact, and base shadow helping the mass stop reading as a single flat wall.
- The library vertical edge lines are now intentionally more visible than the worker pass; the next cycle can soften their color if they read too black in live play.
