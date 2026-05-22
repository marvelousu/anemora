# 2026-05-22 Fast VS HD2D Outdoor World Envelope / Horizon Grounding Cycle 36

## Scope

Cycle 36 is a focused outdoor-envelope pass for the Fast VS HD-2D house slice. It keeps the cycle 32-35 sky, backdrop, and ground-continuation work intact while making the HouseExterior and CentralPlaza edges feel embedded in a larger world instead of sitting on isolated plates. It does not change gameplay collision, map transitions, story, dialogue, player controller, UI, time-window behavior, or the main branch.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Intent

The goal of this cycle was to add a non-playable world envelope around the outdoor areas using low, material-matched terrain shelves and horizon ridges. The intent was to ground the exterior house view and the central plaza library view without reintroducing black masks, opaque walls, or any change to movement/arrival logic.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_outdoor_world_envelope_foundation_cycle36.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

## What Changed

- Added `CreateOutdoorWorldEnvelopeFoundation(...)` and called it from both `CreateExterior(...)` and `CreateCentralPlaza(...)` immediately after `CreateOutdoorWorldGroundingFoundation(...)`.
- Added non-colliding, non-arrival world-envelope pieces for HouseExterior using low ground, grass, path, stone, and exterior-wall materials to form left/right shelves, a rear shelf, and a staggered rear horizon ridge.
- Added non-colliding, non-arrival world-envelope pieces for CentralPlaza using low perimeter shoulders, a rear plinth shelf behind the library, and a low rear ridge so the library facade no longer sits on a visually cut-out plane.
- Added `ValidateFastVsHd2dOneHundredNinthCycleOutdoorWorldEnvelopeFoundation()` and wired it into `ValidateHouseSliceBatch()`.
- Updated `docs/devlog/INDEX.md` to index this cycle and bump the 2026-05-22 coverage counts.

## Validation Performed

- Unity batch house validation:
  - `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_world_envelope_foundation_cycle36_validate_worker_20260522.log'`
  - Result: `Fast VS house slice validation passed.`
- Unity batch visual snapshot audit:
  - `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_world_envelope_foundation_cycle36_capture_worker_20260522.log'`
  - Result: `Fast VS HD2D visual snapshot audit passed: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`

## Output Evidence

- Validation log:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_world_envelope_foundation_cycle36_validate_worker_20260522.log`
- Snapshot audit log:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_world_envelope_foundation_cycle36_capture_worker_20260522.log`
- Snapshot output directory:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`
- Parent review snapshot directory:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle36_outdoor_world_envelope_parent_review_20260522_01`
- Representative snapshot files:
  - `02_current_house_exterior_visual_snapshot.png`
  - `03_current_central_plaza_visual_snapshot.png`

## Parent Review Notes

- The parent review copied the refreshed audit screenshots into the cycle-specific parent review directory above so this cycle has stable progress evidence.
- The house exterior screenshot now has additional low ground and ridge layers beyond the immediate playable plate, most visibly around the right and rear edge.
- The central plaza screenshot now has side and rear grounding shelves around the library frontage, reducing the previous cut-out/floating-map read.
- This cycle is accepted as a world-grounding foundation rather than final scenic background art.

## Residual Risk

- The world envelope is intentionally subtle and non-playable; if a later camera framing change moves the review window, the balance between edge grounding and backdrop depth may need one more pass.
- The house exterior envelope is most visible on the right/rear side in the current review shot; the left/front anchoring is improved but still comparatively light.
- Unity batchmode touched generated scene and asset side effects during validation, and those side effects are left in place for the parent session to clean or restage.
