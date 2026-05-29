# 2026-05-22 Fast VS HD2D Facade Edge Softening Cycle 42

## Scope
- Softened the strongest Cycle 41 facade edge/contact accents on the house exterior and central plaza library facade.
- Kept the improved dimensional read, but reduced the line-like feel by narrowing the harsh contact strips and adding a few tiny material-consistent breakup pieces.
- Left gameplay colliders, triggers, story flow, UI, and time-window logic untouched.

## Intent
- Keep the eave, edge, and base contacts visible without making them read like black placeholder bands.
- Add a small amount of wall/stone breakup so the facade surfaces feel more integrated into the material language already used by the scene.
- Stay within the existing HD-2D material vocabulary rather than introducing new flat-black accents.

## Files Changed
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_facade_edge_softening_cycle42.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

## Implementation
- Tuned the existing house-exterior surface-depth contact strips so the strongest bands are thinner and less visually heavy.
- Tuned the existing central-plaza library surface-depth contact strips the same way, especially the eave line, inner vertical edge shades, and entry-step contact strip.
- Parent review found the worker pass still too black-line-like on the house eave/right return and plaza-library eave/inner edges, so those B-pass strips were switched from the hard shadow material to the softer `hd2d_outdoor_occlusion_gradient` material.
- Added small non-arrival, non-collider breakup pieces for both current and past variants:
  - a subtle eave trim highlight on the house exterior
  - a small base weather chip on the house exterior
  - a subtle eave trim highlight on the central plaza library facade
  - a small lower stone chip on the central plaza library facade
- Reused the current/past exterior wall and stone materials so the new pieces stay material-consistent instead of reading like extra shadow bars.
- Added a new validation method for the softening pass and wired it into the house-slice batch validation sequence.

## Validation
- Unity batch house validation:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_facade_edge_softening_cycle42_validate_worker_20260522.log`
  - Result: passed, with `Fast VS house slice validation passed.`
- Parent Unity batch house validation:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_facade_edge_softening_cycle42_validate_parent_20260522.log`
  - Result: passed, with `Fast VS house slice validation passed.`
- Unity batch visual snapshot audit:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_facade_edge_softening_cycle42_capture_worker_20260522.log`
  - Result: passed, with `Fast VS HD2D visual snapshot audit passed.`
- Parent Unity batch visual snapshot audit:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_facade_edge_softening_cycle42_capture_parent_20260522.log`
  - Result: passed, with `Fast VS HD2D visual snapshot audit passed.`

## Output Evidence
- Current audit screenshot directory:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`
- Cycle 42 copy:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle42_facade_edge_softening_worker_20260522_01`
- Parent review copy:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle42_facade_edge_softening_parent_review_20260522_01`
- PNGs in both folders:
  - `01_current_house_interior_visual_snapshot.png`
  - `02_current_house_exterior_visual_snapshot.png`
  - `03_current_central_plaza_visual_snapshot.png`
  - `04_current_library_visual_snapshot.png`
  - `visual_snapshot_metrics_cycle10_20260522.md`

## Review Notes
- The house exterior still reads dimensional, but the eave and right-side contact accents are less line-like than the first Cycle 41 pass.
- The central plaza library facade also still reads dimensional, with the softened contact lines staying integrated into the wall and stone materials instead of standing out as dark bands.
- Remaining risk: the library exterior still needs a larger dedicated height/mass pass so it matches the tall interior. This cycle only softened existing facade accents.
