# Stage 4 Character Animation Baseline Review

Date: 2026-05-08

## Scope

Added a Unity-side review capture for already-imported runtime character animation strips.

This pass does not import or approve the new Batch A full-cast animation source sheets. Those remain review-source images in the upstream handoff until the user approves extraction / runtime conversion.

## Implementation

- Added `Assets/Editor/Stage4CharacterAnimationReviewCapture.cs`.
- Added Unity menu item:
  - `Anemora/Review/Capture Stage4 Character Animation Baseline`
- The capture reads existing runtime PNG strips directly from disk and composes a nearest-neighbor 1920 x 1080 review sheet.
- Included baseline characters:
  - Hero / Niro `v2`
  - Resident_A `v2`
  - Resident_B `v2` seated idle
- Added `GraphicsFoundationAssetTests.Stage4CharacterAnimationBaselineReviewScreenshotExists`.

## Screenshot Artifact

- `docs/devlog/screenshots/stage4_character_animation_baseline_review.png`
  - SHA-256: `2F263DED08EF7115BB050EFB2F0EAD45EB2B72D081691E2BA3A4D33294BF60C5`

## Verification

- `Anemora.EditorTools.Stage4CharacterAnimationReviewCapture.Capture`
  - Exit code: `0`
  - Shader error / shader warning / Exception / Assert matches: `0`
- `GraphicsFoundationAssetTests`
  - `20/20` passed
  - Result XML: `%TEMP%/AnemoraCodexLogs/20260508_gfx_foundation_targeted/graphics_foundation_tests_with_animation_review.xml`

## Notes

- The review sheet is a visual baseline for runtime-scale animation readability, not a final full-cast animation import.
- Batch A source sheets should remain outside Unity until approved, then converted to the same `32 x 48`, 4-frame strip contract.

## Full-Cast Animation Source Handoff

The upstream non-Unity animation source batch is available for review but is not imported by this graphics foundation pass.

- Source directory:
  - `<worktree>\docs\review_gallery\imports\stage4_full_cast_animation_sources_2026-05-08`
- Batch A source sheets:
  - `resident_c_animation_source_01.png`
  - `resident_d_animation_source_01.png`
  - `resident_j_animation_source_01.png`
  - `resident_k_animation_source_01.png`
  - `resident_l_animation_source_01.png`
  - `robot_x_animation_source_01.png`
- Also present in the same upstream source directory:
  - `resident_e_animation_source_01.png`
  - `resident_f_animation_source_01.png`
  - `resident_g_animation_source_01.png`
  - `resident_h_animation_source_01.png`
  - `resident_i_animation_source_01.png`
  - `resident_r_animation_source_01.png`
  - `b1_robot_variants_source_01.png`
  - `past_crowd_extras_source_01.png`
  - `present_crowd_extras_source_01.png`
  - `future_ruin_silhouettes_source_01.png`

Safe graphics-foundation work before user approval:

- Keep using the already-imported Hero / Niro, Resident_A, and Resident_B runtime sprites as the scale/readability reference.
- Prepare extraction/import requirements and review notes.
- Do not convert, import, animate, or place unapproved Batch A/B source sheets in Unity.

After user approval, the next safe runtime conversion step is chroma-key cleanup, `32 x 48` frame extraction, idle / walk strip assembly, 1x / 6x contact sheets, and only then Unity import under `Assets/Art/Sprites/NPC/<Character>/v1/`.
