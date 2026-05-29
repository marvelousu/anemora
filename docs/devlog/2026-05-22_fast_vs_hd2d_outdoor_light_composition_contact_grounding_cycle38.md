# 2026-05-22 Fast VS HD2D Outdoor Light Composition Contact Grounding Cycle 38

## Scope

Cycle 38 adds a restrained outdoor light-composition pass for the Fast VS HD-2D house slice. The focus is on visible directional staging and contact grounding at the house exterior and central plaza so the shots read richer without becoming darker. This cycle does not change gameplay collision, map transitions, story, dialogue, UI, player controller, time-window behavior, or the main branch.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Intent

The goal of this cycle was to make the exterior shots feel more intentionally lit: soft under-eave shadowing, facade base contact, doorway recess grounding, and small directional highlights that suggest daylight bounce. The pass is meant to increase HD-2D staging quality, not to introduce a broad darkening treatment.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_outdoor_light_composition_contact_grounding_cycle38.md`

## What Changed

- Added `CreateOutdoorLightCompositionContactGrounding(...)` and called it from both `CreateExterior(...)` and `CreateCentralPlaza(...)` after the outdoor horizon scenic depth pass.
- Added a new house-exterior light-composition helper with thin non-arrival pieces for under-eave shadow breaks, facade base lift, door threshold glow, porch step grounding, tree base grounding, road-edge dust, and a subtle roof-side highlight.
- Added a new central-plaza light-composition helper with segmented library eave shadowing, facade base grounding, door recess glow, approach bounce, and small sign/fountain contact pieces.
- Added `EnsureHd2dCoolLightPoolMaterial()` and `EnsureHd2dCoolLightPoolTexture()` to support the cooler directional-light accents.
- Added `ValidateFastVsHd2dOneHundredEleventhCycleOutdoorLightCompositionContactGrounding()` and wired it into `ValidateHouseSliceBatch()`.
- Updated `docs/devlog/INDEX.md` to index the new cycle and refresh the 2026-05-22 coverage counts.

## Validation Performed

- Unity batch house validation:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_light_composition_contact_grounding_cycle38_validate_worker_20260522.log`
  - Result: `Fast VS house slice validation passed.`
- Unity batch visual snapshot audit:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_light_composition_contact_grounding_cycle38_capture_worker_20260522.log`
  - Result: `Fast VS HD2D visual snapshot audit passed: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`

## Output Evidence

- Validation log:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_light_composition_contact_grounding_cycle38_validate_worker_20260522.log`
- Snapshot audit log:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_light_composition_contact_grounding_cycle38_capture_worker_20260522.log`
- Snapshot output directory:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`
- Parent review snapshot directory:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle38_outdoor_light_composition_contact_grounding_parent_review_20260522_01`
- Representative snapshot files:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\02_current_house_exterior_visual_snapshot.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\03_current_central_plaza_visual_snapshot.png`

## Parent Review Notes

- The parent review copied the refreshed audit screenshots into the cycle-specific parent review directory above.
- The new contact and glow pieces did not introduce obvious rectangular artifacts, black bands, or gameplay-obstructing geometry.
- The house exterior and plaza shots are accepted as a stable contact-grounding pass, but the visual change is still fairly restrained from the audit camera distance.
- The next lighting pass should increase visible composition by using clearer light planes, occlusion gradients, or camera-framed highlights rather than simply adding more small contact pieces.

## Residual Risk

- The new contact-grounding pieces are deliberately restrained, so the improvement depends on the existing backdrop stack and camera framing to carry most of the scene depth.
- A future composition or camera change could shift the balance between the added highlights, the eave shadows, and the readable sky.
- Unity batchmode touched generated scene, material, texture, and ProjectSettings side effects during validation and snapshot capture; those side effects are left in place for the parent session to clean or restage.
