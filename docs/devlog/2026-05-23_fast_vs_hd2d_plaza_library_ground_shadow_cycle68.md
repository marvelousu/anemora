# 2026-05-23 Fast VS HD-2D Plaza Library Ground Shadow Cycle68

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-shading-foundation-20260522`
- Primary file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

## Direction

This cycle continues the HD-2D shadow foundation pass. The current target was the central plaza library exterior, which still read too much like a vertical facade sitting on a flat floor. The user clarified that shadows may be slightly exaggerated, so the goal was readable building weight rather than physically exact cast length.

## Implementation

- Added `CreateCentralPlazaLibraryExaggeratedGroundShadowCycle68(...)`.
- Added `ValidateFastVsHd2dShadowFoundationCycle68PlazaLibraryGroundShadow()`.
- Connected Cycle68 creation after the existing Cycle59 library roof underside shadow pass.
- Connected Cycle68 validation into `ValidateHouseSliceBatch()`.
- Added current and past variants for:
  - broad library ground cast
  - entry canopy/door cast
  - west return ground shadow
  - east return ground shadow
- Used `hd2d_outdoor_occlusion_gradient` for broad and side ground shadows.
- Used `hd2d_depth_shadow` for the tighter entry cast.

## Parent Refinement

The worker's first broad ground shadow was structurally safe but visually too close to the facade. After parent screenshot review, the broad ground cast was shifted slightly toward the plaza floor and widened in depth:

- Current broad cast Z: `8.00` to `7.68`
- Current broad cast depth: `0.94` to `1.08`
- Past broad cast Z: `8.02` to `7.72`
- Past broad cast depth: `1.00` to `1.16`

This keeps the result slightly exaggerated without creating a black slab.

## Worker Cycle

- Detailed instruction was sent to gpt-5.4-mini worker `019e5298-054f-7c40-8bed-76ae92f43f5a`.
- The worker implemented the helper, validator, object set, and material choices.
- Parent performed the visual placement refinement after screenshot review.

## Validation

Unity validation passed:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_shadow_foundation_cycle68_plaza_library_ground_shadow_forward_validate_parent_20260523.log`

Screenshot capture/audit completed:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_shadow_foundation_cycle68_plaza_library_ground_shadow_forward_visual_snapshot_parent_20260523.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_shadow_foundation_cycle68_plaza_library_ground_shadow_forward_close_review_parent_20260523.log`

Retained evidence:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_shadow_foundation_cycle68_plaza_library_ground_shadow_parent_review_20260523_01`

## Review Notes

- The added shadows are safe, non-arrival, and do not interfere with movement.
- The effect is intentionally controlled. It improves grounding without obscuring the entrance glow or plaza floor readability.
- A later cycle should add a larger directional building cast or stronger exterior edge falloff if the library still lacks first-glance impact.
