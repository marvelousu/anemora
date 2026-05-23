# 2026-05-23 Fast VS HD-2D House Exterior Ground Shadow Cycle69

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-shading-foundation-20260522`
- Primary file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

## Direction

This cycle continues the HD-2D shading foundation work after the plaza library grounding pass. The focus was the house exterior, where the building still needed clearer contact with the ground and the surrounding yard/road. The user clarified that shadows should not be fully realistic in length; a slightly exaggerated, readable shadow shape is preferred if it makes the scene feel more staged and grounded.

## Implementation

- Added `CreateHouseExteriorExaggeratedGroundShadowCycle69(...)`.
- Added `ValidateFastVsHd2dShadowFoundationCycle69HouseExteriorGroundShadow()`.
- Connected Cycle69 creation immediately after the existing Cycle49 house exterior decisive light/shadow pass.
- Connected Cycle69 validation into `ValidateHouseSliceBatch()`.
- Added current and past variants for:
  - broad house ground cast
  - porch entry contact cast
  - north-east road edge falloff
  - front yard falloff
- Used `hd2d_outdoor_occlusion_gradient` for broad, soft grounding.
- Used `hd2d_depth_shadow` for the tighter porch entry contact shadow.

## Parent Refinement

The first worker implementation was structurally safe, but the full visual snapshot made the ground shadow read too subtly at normal camera distance. Parent review widened and shifted the broad cast and falloff pieces to better match the requested "slightly exaggerated" direction:

- House broad cast moved from local Z `-0.76` to `-0.34`.
- House broad cast scale increased from `6.72 x 2.42` to `7.05 x 3.05`.
- Porch entry cast scale increased from `1.54 x 0.30` to `1.78 x 0.44`.
- North-east road and yard falloff pieces were widened so the house no longer sits on an isolated flat patch.

The result remains intentionally controlled. It avoids black-slab shadows while making the facade, porch, and nearby ground feel more connected.

## Worker Cycle

- Detailed instruction was sent to gpt-5.4-mini worker `019e52bc-6181-7922-96a6-371fdf46d2a8`.
- The worker implemented the helper, validator, current/past objects, and material choices.
- Parent performed the visual-density refinement after screenshot review.

## Validation

Unity validation passed:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_shadow_foundation_cycle69_house_exterior_ground_shadow_validate_parent_r2_20260523.log`

Screenshot capture/audit completed:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_shadow_foundation_cycle69_house_exterior_ground_shadow_close_review_parent_r3_20260523.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_shadow_foundation_cycle69_house_exterior_ground_shadow_visual_snapshot_parent_r2_20260523.log`

Retained evidence:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_shadow_foundation_cycle69_house_exterior_ground_shadow_parent_review_20260523_01`

## Review Notes

- The added objects are non-arrival landmarks and have no colliders.
- Renderers are shadow-safe: `shadowCastingMode = Off` and `receiveShadows = false`.
- The house exterior close review keeps the accepted darker eave/porch shading direction from `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_close_review_20260520`.
- The next shading cycle should expand this approach from individual building contact shadows into larger outdoor composition shadows and perimeter falloff so the maps feel less like isolated tiles.
