# 2026-05-09 Stage 4 DQ3R Character Readability Push

## Summary

Created a non-Unity review package for evaluating whether Chapter 1 character art should remain on the current `32x48 / PPU32` runtime contract or move toward higher-density HD-2D production sprites.

Unity was not opened. No scene, prefab, Animator, AnimationClip, or `.meta` file was intentionally changed.

## Review Roots

- `docs/review_gallery/imports/stage4_dq3r_character_sources_2026-05-09/`
- `docs/review_gallery/imports/stage4_dq3r_character_runtime_candidates_2026-05-09/`
- `docs/review_gallery/imports/stage4_dq3r_character_review_2026-05-09/`
- `docs/review_gallery/imports/stage4_dq3r_master_sheets_2026-05-09/`
- `docs/review_gallery/imports/stage4_animation_framecount_review_2026-05-09/`
- `docs/review_gallery/imports/stage4_dq3r_scene_mocks_2026-05-09/`
- `docs/review_gallery/imports/stage4_dq3r_expression_pack_review_2026-05-09/`

## Optional Stretch Sources

Reference-only source sheets were added for dialogue and role-pose exploration:

- `chapter1_priority_portrait_bust_review_source_01.png`
- `chapter1_priority_role_pose_review_source_01.png`

These are useful for evaluating expression, object handling, and role readability, but they should not be used as runtime replacement sources without user review. They intentionally sit outside the current import-ready sprite set.

## Screenshots

- `docs/devlog/screenshots/stage4_dq3r_character_cast_lineup_1x.png`
- `docs/devlog/screenshots/stage4_dq3r_character_cast_lineup_6x.png`
- `docs/devlog/screenshots/stage4_dq3r_character_animation_stability.png`
- `docs/devlog/screenshots/stage4_dq3r_character_lighting_compat.png`
- `docs/devlog/screenshots/stage4_dq3r_character_scene_mocks_s1_s3.png`
- `docs/devlog/screenshots/stage4_dq3r_character_scene_mocks_s4_s5.png`
- `docs/devlog/screenshots/stage4_dq3r_crowd_density_review.png`
- `docs/devlog/screenshots/stage4_dq3r_expression_pack_review.png`

## Key Findings

Prototype import can proceed for the Chapter 1 priority six if the goal is VS playability:

- Mia / Resident_F
- Kaia / Resident_C
- Dario / Resident_D
- Karla / Resident_J
- Kairo / Resident_K
- Luna / Resident_L

Production `64x96` review is strongly recommended before locking:

- Dario / Resident_D
- Kairo / Resident_K
- Luna / Resident_L
- Karla / Resident_J

Frame-count upgrade candidates:

- Dario / Resident_D
- Luna / Resident_L
- Kairo / Resident_K

Static jitter review flags:

- `Assets/Art/Sprites/NPC/Resident_J/v1/resident_j_walk_back.png`
- `Assets/Art/Sprites/NPC/Resident_K/v1/resident_k_walk_right.png`
- `Assets/Art/Sprites/NPC/Resident_L/v1/resident_l_walk_front.png`

## Manifests

- `docs/asset_manifests/stage4_character_runtime_manifest_2026-05-09.json`
- `docs/asset_manifests/stage4_character_dq3r_review_manifest_2026-05-09.json`
- `docs/review_gallery/imports/stage4_dq3r_character_review_2026-05-09/runtime_png_static_verification.json`

## Handoff

Graphics foundation handoff:

- `docs/review_gallery/imports/stage4_dq3r_character_review_2026-05-09/graphics_foundation_handoff.md`
- `docs/review_gallery/imports/stage4_dq3r_character_review_2026-05-09/acceptance_bar_review.md`

Notes handoff:

- `<notes>\_handover\anemora-character-generation-session-dq3r-progress-2026-05-09.md`
