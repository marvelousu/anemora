# 2026-05-22 Fast VS HD2D Outdoor Ground Continuation / Library Verticality Cycle 35

## Scope

Cycle 35 is a focused visual grounding pass for the Fast VS HD-2D house slice. It addresses two review notes only: the outdoor playable areas feeling like they float into empty sky at the edges, and the plaza library exterior still reading too short/flat relative to the taller interior volume. It does not touch story, dialogue, time-window behavior, transitions, player controller, UI, or the main branch.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Intent

The goal of this cycle was to keep the cycle 34 sky/backdrop fixes intact while making the outdoor maps read as continuous ground rather than isolated floor plates. At the same time, the central plaza library needed a stronger exterior vertical profile so the facade no longer feels shorter than the interior volume suggests.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_outdoor_ground_continuation_library_verticality_cycle35.md`

## What Changed

- Added `CreateOutdoorWorldGroundingFoundation(...)` and called it from both `CreateExterior(...)` and `CreateCentralPlaza(...)`.
- Added non-colliding, non-arrival ground continuation pieces for the exterior and central plaza, using existing ground/path/grass/wall materials instead of transparent or black filler.
- Added `CreatePlazaLibraryVerticalityFoundation(...)` and called it from the central plaza generation flow.
- Added upper facade mass, clerestory/high-window band, side returns, roof step cap, and upper trim band pieces to make the plaza library read taller and deeper.
- Extended the non-arrival landmark cube helpers so ground continuation can use `PathOrFloor` while staying non-colliding and counts-for-arrival disabled.
- Added `ValidateFastVsHd2dOneHundredEighthCycleOutdoorGroundContinuationAndLibraryVerticality()` and a generic non-arrival landmark validator, then called the new validation from `ValidateHouseSliceBatch()`.
- Updated `docs/devlog/INDEX.md` to index this cycle and bump the 2026-05-22 coverage counts.

## Validation Performed

- Unity batch house validation:
  - `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_ground_continuation_library_verticality_cycle35_validate_worker_20260522.log'`
  - Result: `Fast VS house slice validation passed.`
- Unity batch visual snapshot audit:
  - `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_ground_continuation_library_verticality_cycle35_capture_worker_20260522.log'`
  - Result: `Fast VS HD2D visual snapshot audit passed: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`

## Output Evidence

- Validation log:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_ground_continuation_library_verticality_cycle35_validate_worker_20260522.log`
- Parent validation log after clerestory split review fix:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_ground_continuation_library_verticality_cycle35_validate_parent_window_split_20260522.log`
- Snapshot audit log:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_ground_continuation_library_verticality_cycle35_capture_worker_20260522.log`
- Parent snapshot audit log after clerestory split review fix:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_ground_continuation_library_verticality_cycle35_capture_parent_window_split_20260522.log`
- Snapshot output directory:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`
- Parent review screenshot directory before the clerestory split fix:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle35_outdoor_ground_continuation_library_verticality_parent_review_20260522_01`
- Parent review screenshot directory after the clerestory split fix:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle35_outdoor_ground_continuation_library_verticality_parent_review_20260522_02_window_split`

## Parent Review Follow-Up

- The first parent review screenshot showed that the ground continuation worked, and the exterior library now read taller, but the upper facade had one large black horizontal band that looked like a hole rather than high windows.
- The parent fix replaced that single `DoorwayDark` clerestory strip with four separated clerestory window panels, a wall backing, a sill/header pair, and thin mullions. Current-side windows now use the existing empty-window material; past-side windows use the existing warm window-light material.
- The second parent review screenshot confirmed the black-hole read was removed while keeping the taller library mass and the broader outdoor ground continuation.

## Residual Risk

- The new ground continuation pieces are intentionally non-colliding and visually oriented only; if a later camera change shifts the review framing, the balance between ground continuity and backdrop depth may need another pass.
- The plaza library verticality pieces are layered on top of the existing facade and occlusion shell work, so future facade edits should keep an eye on overlap and silhouette density.
- Unity validation touched generated assets and ProjectSettings during the batch runs; those side effects are left in place and are listed in the working tree status.
