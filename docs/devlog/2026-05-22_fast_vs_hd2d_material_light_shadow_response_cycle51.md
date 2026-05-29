# Fast VS HD2D Material Light / Shadow Response Cycle 51

Date: 2026-05-22

## Summary

Cycle51 is a material-response foundation pass, not a final shadow pass. The goal here was to make the existing HD-2D shader/material stack react more clearly to world light and shadow without adding new black strips or dark placeholder geometry. The house exterior, ground, roof, and sprite cards now lean harder on the same SurfaceRampLit and SpriteCardRampUnlit foundation so the light direction and grounded shading read more clearly in the snapshot set.

## Changed Files

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_*.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

## What Changed

- Raised the generation constants in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` to strengthen material response:
  - `SurfaceRampDirectionalLightStrength = 0.18f`
  - `SurfaceRampShadowReceiveStrength = 0.26f`
  - `SpriteCardWorldLightStrength = 0.10f`
  - `SpriteCardWorldShadowReceiveStrength = 0.07f`
- Added `ValidateFastVsHd2dCycle51MaterialLightShadowResponse()` and wired it into `ValidateHouseSliceBatch()` so the new response range is checked on representative materials rather than assumed.
- Kept the change inside the existing material regeneration flow (`CreateHouseSliceScene()` / `ValidateHouseSliceBatch()`), so the `.mat` assets are regenerated from code instead of hand-edited.
- Parent review accepted the stronger shadow direction, then corrected a remaining house-exterior structural issue: the closed door still had side views into the house from an angled gameplay-like camera. The fix adds non-colliding current/past door-side return walls and outer return walls so the closed facade reads as a closed building rather than a slice with visible interior space.

## Validation

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_material_light_shadow_response_cycle51_validate_worker_20260522.log`
  - Result: passed
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_material_light_shadow_response_cycle51_material_audit_worker_20260522.log`
  - Result: passed
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_material_light_shadow_response_cycle51_sprite_audit_worker_20260522.log`
  - Result: passed
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_material_light_shadow_response_cycle51_visual_snapshot_worker_20260522.log`
  - Result: passed
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_material_light_shadow_response_cycle51_validate_parent_after_outer_shell_20260522.log`
  - Result: passed
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_material_light_shadow_response_cycle51_close_review_parent_after_door_shell_20260522.log`
  - Result: passed
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_material_light_shadow_response_cycle51_visual_snapshot_parent_after_door_shell_20260522.log`
  - Result: passed
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_material_light_shadow_response_cycle51_material_audit_parent_after_shell_20260522.log`
  - Result: passed
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_material_light_shadow_response_cycle51_sprite_audit_parent_after_shell_20260522.log`
  - Result: passed

## Screenshot Evidence

- Visual snapshot source folder: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`
- Cycle51 parent-review copy folder: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle51_material_light_shadow_response_parent_review_20260522_01`
- Copied evidence:
  - `01_current_house_interior_visual_snapshot.png`
  - `02_current_house_exterior_visual_snapshot.png`
  - `03_current_central_plaza_visual_snapshot.png`
  - `04_current_library_visual_snapshot.png`
  - `05_house_exterior_door_close_after_side_shell.png`
  - `visual_snapshot_metrics_cycle10_20260522.md`

## Self Review

Conditionally OK.

The snapshot set is now more readable in the light/shadow direction than the previous cycle, and the material stack reacts more strongly without introducing new fake dark geometry. The user specifically noted that the shadow direction is now good. Parent review also closed the remaining visible side gap around the house door with actual facade return geometry rather than a black mask.

The remaining gap is that this is still a foundation pass: shadow shape, contact density, background composition, and character-ground integration can still be pushed further before the result should be treated as final HD-2D shading.
