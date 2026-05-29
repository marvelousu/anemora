# 2026-05-22 Fast VS HD2D Outdoor Horizon Scenic Depth Cycle 37

## Scope

Cycle 37 adds a scenic-depth layer around the outdoor map edges for the Fast VS HD-2D house slice. It is a follow-up to the cycle 36 world envelope pass and keeps the outdoor areas from reading as quick blocks against empty sky. This cycle does not change gameplay collision, map transitions, story, dialogue, UI, player controller, time-window behavior, or the main branch.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Intent

The goal of this cycle was to add a subtle, material-matched horizon layer beyond the cycle 36 envelope. The additions are meant to read as distant terrain, town parapets, and light background continuation, not as a new sky panel, opaque wall, or playable boundary.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_outdoor_horizon_scenic_depth_cycle37.md`

## What Changed

- Added `CreateOutdoorHorizonScenicDepthFoundation(...)` and called it from both `CreateExterior(...)` and `CreateCentralPlaza(...)` after the cycle 36 world-envelope work and before the existing outdoor backdrop layers.
- Added 3-5 staggered horizon strips per map using existing current/past ground, grass, path, stone, roof, and exterior-wall materials so the HouseExterior and CentralPlaza views read as continuing land and town mass instead of ending at a hard cut.
- Added two small non-colliding detail clusters per map using the new shadow-safe helper so the horizon does not read as one flat band.
- Kept all new pieces non-arrival and non-colliding by using the existing shadow-safe non-arrival landmark helper.
- Added `ValidateFastVsHd2dOneHundredTenthCycleOutdoorHorizonScenicDepth()` and wired it into `ValidateHouseSliceBatch()`.
- Updated `docs/devlog/INDEX.md` to index this cycle and bump the 2026-05-22 coverage counts.

## Validation Performed

- Unity batch house validation:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_horizon_scenic_depth_cycle37_validate_worker_20260522.log`
  - Result: `Fast VS house slice validation passed.`
- Unity batch visual snapshot audit:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_horizon_scenic_depth_cycle37_capture_worker_20260522.log`
  - Result: `Fast VS HD2D visual snapshot audit passed: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`

## Output Evidence

- Validation log:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_horizon_scenic_depth_cycle37_validate_worker_20260522.log`
- Snapshot audit log:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_horizon_scenic_depth_cycle37_capture_worker_20260522.log`
- Snapshot output directory:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`
- Parent review snapshot directory:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle37_outdoor_horizon_scenic_depth_parent_review_20260522_01`
- Representative snapshot files:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\02_current_house_exterior_visual_snapshot.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\03_current_central_plaza_visual_snapshot.png`

## Parent Review Notes

- The parent review copied the refreshed audit screenshots into the cycle-specific parent review directory above.
- The house exterior shot now has additional low horizon/city-depth strips visible around the right and rear route area, which further reduces the hard floating-plate read.
- The central plaza shot now has extra low side and rear town/terrain forms around the library, keeping the facade from sitting directly in empty sky.
- The change is intentionally restrained; it is accepted as scenic depth groundwork, not the final outdoor background pass.

## Residual Risk

- The scenic layer is intentionally restrained, so the central plaza shot still relies on the existing backdrop stack for most of its depth read.
- If later camera framing changes move the review window, the balance between subtle horizon pieces and readable sky may need another pass.
- Unity batchmode touched generated assets and scene side effects during validation, and those side effects are left in place for the parent session to clean or restage.
