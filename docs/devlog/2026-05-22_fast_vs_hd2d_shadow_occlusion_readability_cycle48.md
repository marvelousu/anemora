# Fast VS HD2D Shadow/Occlusion Readability Cycle 48

Date: 2026-05-22

## Summary

This cycle sharpened the HD-2D shadow read without turning the scene into a global darkness pass. The changes separate contact shadows, eave and gallery occlusion, facade grounding, and texture alpha tuning so the house, plaza library, interior galleries, tables, and character feet read with clearer depth.

## Changed Files

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

## Notable Implementation Details

- Retuned the transparent shadow textures and material tints for the contact, directional, static, surface, and outdoor occlusion helpers.
- Added dedicated cycle 48 occlusion readability objects for the house exterior, central plaza library facade, and library interior galleries/table bases.
- Widened the character footprint overlays slightly so Niro, Reto, and Aria keep a readable shadow at the feet without becoming square patches.
- Added a dedicated validation method for the new cycle 48 shadow and occlusion objects plus alpha-range checks for the retuned textures.
- Parent review corrected the worker's too-solid central plaza library door occlusion from a full dark panel into a thin top recess band.
- Parent review also closed the house exterior door-side visual leaks with current/past exterior-wall fill panels so a closed exterior door no longer exposes the interior shell from the outside.

## Validation

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_shadow_occlusion_readability_cycle48_validate_worker_20260522.log`
  - Result: passed
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_shadow_occlusion_readability_cycle48_capture_worker_20260522.log`
  - Result: passed
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_shadow_occlusion_readability_cycle48_validate_parent_after_gap_fix_20260522.log`
  - Result: passed
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_shadow_occlusion_readability_cycle48_capture_parent_after_gap_fix_20260522.log`
  - Result: passed

## Screenshot Evidence

- Source audit folder: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`
- Parent review copy folder: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle48_shadow_occlusion_readability_parent_review_20260522_01`

## Notes

- The read is intentionally soft and localized: under eaves, door recesses, porch posts, library facade grounding, gallery undersides, and table bases.
- The current pass keeps the floor walkable and does not add collision changes or gameplay behavior changes.
- Parent visual review outcome: the door-side exterior leak is reduced enough to keep this cycle, but the shadow pass still reads as a baseline rather than a finished HD-2D lighting look. Next work should move from local alpha overlays to a more decisive area-light / contact-occlusion composition.
