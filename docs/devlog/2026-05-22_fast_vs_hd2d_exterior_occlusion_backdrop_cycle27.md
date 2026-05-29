# 2026-05-22 Fast VS HD2D Exterior Occlusion Backdrop Cycle 27

Cycle 27 treated the user report as an exterior-structure problem, not a shading problem.
The goal was to stop houses and the plaza library from exposing interior/behind-the-scenes space when viewed from outside, and to add a minimal but visible backdrop foundation for sky, horizon, and outer map edges.

## What changed

- Added house exterior occlusion shells for current and past variants.
- After parent screenshot review, narrowed the broad house backplate into an upper backing band and added door-jamb fill strips so the door remains visible while the side gaps are masked.
- Added plaza library occlusion shells for current and past variants.
- Added a backdrop foundation layer for the house exterior and central plaza in both time states.
- Kept story, transition, portal, map movement, and time-window behavior unchanged.

## Result

- The exterior shell objects now provide back/side/roof depth masking instead of relying on darker shading alone.
- The plaza library now has rear volume and window backing so the facade reads as a real structure from outside.
- The backdrop foundation gives the scene a low-key sky and horizon presence so the map edge no longer feels empty.

## Validation

- `Anemora.EditorTools.AnemoraFastVsHd2dExteriorOcclusionBackdropAudit.WriteExteriorOcclusionBackdropCycle27ReportBatch`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- `Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch`

All three batch runs passed.

## Report

- [Exterior occlusion backdrop cycle 27 report](screenshots/fast_vs_hd2d_exterior_occlusion_backdrop_cycle27_20260522/exterior_occlusion_backdrop_cycle27_20260522.md)
