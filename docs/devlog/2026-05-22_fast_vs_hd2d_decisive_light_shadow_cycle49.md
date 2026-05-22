# Fast VS HD2D Decisive Light / Shadow Contrast Cycle 49

Date: 2026-05-22

## Summary

The user pointed out that the image read still had weak shadows and wanted a more immediately convincing HD-2D lighting look. Cycle49 is the first step toward that direction: it pushes the exterior, central plaza, and library profiles darker and sharper, then adds localized occlusion pieces so the lighting contrast is visible in the scene without turning doors, windows, or entrances into black panels.

## Changed Files

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsHouseLightingDirector.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

## What Changed

- Lowered ambient light and raised main-light shadow strength for the exterior and central plaza profiles so the scene reads with more decisive contrast.
- Kept the library and interior darker, but within readable bounds, so desks, shelves, and characters remain legible.
- Matched `CreateLighting()` and the area lighting profile review values to the updated profiles.
- Added cycle49 current/past occlusion objects for house eaves and porch bases, central plaza library grounding, and library desk/table underside contact.
- Parent review removed the initial full-width library gallery underside strip and back-shelf contact strip because both read as straight black bands across the room instead of localized HD-2D contact shadow.
- Parent review widened the house exterior closed-door panel and added validation for the current/past door width so the closed door does not leave a visible side gap into the interior.
- Updated the relevant audits so the new profile values and sky/background clear colors validate against the actual rendered scene.

## Validation

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_decisive_light_shadow_cycle49_validate_parent_after_door_gap_fix_20260522.log`
  - Result: passed
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_decisive_light_shadow_cycle49_capture_parent_after_door_gap_fix_retry_20260522.log`
  - Result: passed
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_decisive_light_shadow_cycle49_close_review_after_door_gap_fix_20260522.log`
  - Result: passed

## Screenshot Evidence

- Source audit folder: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`
- Accepted Cycle49 copy folder: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle49_decisive_light_shadow_parent_review_20260522_03`
- Door close-up evidence: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle49_decisive_light_shadow_parent_review_20260522_03\05_house_exterior_door_close_after_gap_fix.png`

## Self Review

Conditionally OK after parent correction.

The first worker screenshot made the library darker but included horizontal bands that looked too much like black boards. Parent correction removed those full-width strips and kept the desk/table contact shadows and lighting profile changes. The accepted re-run screenshots show no black library band regression and no plaza entrance blackout.

The shadow read is still not strong enough to call the HD-2D foundation complete. Cycle49 is a restrained lighting/occlusion step, not the final look; the next cycle should move from overlay strips toward a stronger, more systematic light/material response.
