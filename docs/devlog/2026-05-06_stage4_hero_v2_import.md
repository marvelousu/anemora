# Stage 4 Character v2 Import

Status: v0.1 implementation record (2026-05-06)

This devlog records the Stage 4 Niro / Hero v2 redraw import and Resident_A / Resident_B v2 adoption after user review approval.

## 1. Source

| Item | Path / ID |
|---|---|
| Hero v2 concept source | `art/_intermediate/hero_v2_full_redraw/niro_v2_imagegen_concept.png` |
| Hero v2 review sheet | `art/_intermediate/hero_v2_full_redraw/niro_v2_review_sheet.png` |
| Pixel granularity review | `art/_intermediate/stage4_character_pixel_pass_review/stage4_pixel_granularity_alignment_review_clean.png` |
| Resident_B diagonal review | `art/_intermediate/stage4_character_pixel_pass_review/resident_b_longhair_diagonal_review.png` |
| Final runtime contact sheet | `art/_intermediate/stage4_character_pixel_pass_review/stage4_v2_runtime_sprite_contact_sheet.png` |
| OpenAI generated source copy | `.codex/generated_images/.../ig_02229e5a51303c6b0169fa60d3bf3c8191b9fe4374c1e51e7e.png` |
| Asset brief | `docs/asset_prompts/hero_v2_full_redraw.md` |

User approved the Hero v2 candidate after visual review. The accepted direction reads as a full redraw rather than a small v1 edit: broad travel-worn hat, muted earth palette, gender-neutral teen silhouette, stronger pixel pass, and consistent front/back/side identity.

Resident_A was accepted with a slightly higher-resolution box downscale so its youth / witness read remains clear while staying within the same 32x48 pixel granularity. Resident_B was accepted as a long-haired, dark, seated 3/4 diagonal sprite for the current/future-side resident mood.

## 2. Imported Assets

New runtime assets:

- `Assets/Art/Sprites/Hero/v2/hero_stand.png`
- `Assets/Art/Sprites/Hero/v2/hero_idle.png`
- `Assets/Art/Sprites/Hero/v2/hero_walk_front.png`
- `Assets/Art/Sprites/Hero/v2/hero_walk_back.png`
- `Assets/Art/Sprites/Hero/v2/hero_walk_left.png`
- `Assets/Art/Sprites/Hero/v2/hero_walk_right.png`
- `Assets/Art/Sprites/Hero/v2/hero_hands_d7.png`
- `Assets/Art/Sprites/NPC/Resident_A/v2/resident_a_idle.png`
- `Assets/Art/Sprites/NPC/Resident_A/v2/resident_a_walk_front.png`
- `Assets/Art/Sprites/NPC/Resident_A/v2/resident_a_walk_back.png`
- `Assets/Art/Sprites/NPC/Resident_A/v2/resident_a_walk_left.png`
- `Assets/Art/Sprites/NPC/Resident_A/v2/resident_a_walk_right.png`
- `Assets/Art/Sprites/NPC/Resident_B/v2/resident_b_idle.png`

Import settings mirror the Stage 3 v1 sprite settings: Sprite texture type, PPU 32, Point filter, mipmaps disabled, alpha transparency, Clamp wrap, Uncompressed default and Standalone texture settings. The v1 assets remain untouched for comparison.

## 3. Runtime Wiring

Updated runtime references:

- `Assets/Prefabs/Characters/Hero.prefab`
- `Assets/Prefabs/Characters/Resident_A.prefab`
- `Assets/Prefabs/Characters/Resident_B.prefab`
- `Assets/Animators/Clips/Hero_Idle.anim`
- `Assets/Animators/Clips/Hero_Walk.anim`
- `Assets/Animators/Clips/Resident_A_Idle.anim`
- `Assets/Animators/Clips/Resident_A_Walk.anim`
- `Assets/Animators/Clips/Resident_B_Idle.anim`

`HeroAnimatorBinder` remains unchanged. The Hero prefab now resolves its idle/front/back/side sprite arrays from `Assets/Art/Sprites/Hero/v2/`. Resident prefabs and their animation clips now reference `Assets/Art/Sprites/NPC/**/v2/`. The scene file `Assets/Scenes/Anemora_Main.unity` was not edited.

## 4. NPC Adoption Notes

Resident_A and Resident_B went through an explicit review gate before adoption:

- `art/_intermediate/npc_residents_v2_review/resident_ab_v2_imagegen_concept.png`
- `art/_intermediate/npc_residents_v2_review/resident_ab_v2_review_sheet.png`
- `art/_intermediate/stage4_character_pixel_pass_review/resident_b_longhair_diagonal_review.png`
- `art/_intermediate/stage4_character_pixel_pass_review/stage4_pixel_granularity_alignment_review_clean.png`

Resident_A uses the approved slightly higher-resolution box downscale. Resident_B uses the approved long-hair dark 3/4 seated direction. Additional diagonal Resident_B views are retained only as ignored intermediate review material for future scene-specific orientation work.

## 5. Verification

Completed in this task:

- Targeted EditMode: `Anemora.Tests.EditMode.CharacterPrefabStructureTests` `6/6` passed.
- Targeted PlayMode: `Anemora.Tests.PlayMode.HeroAnimatorBinderTests` `2/2` passed.
- Full EditMode: `35/35` passed.
- Full PlayMode: `29/29` passed in graphics-enabled batchmode.
- Windows Standalone build: success at `Builds/Stage4CharacterV2/Anemora_Stage4_CharacterV2.exe`.

The first full PlayMode attempt with `-nographics` reported `27/29` because graphics-dependent tests could not initialize render targets in that mode. The final graphics-enabled batchmode run is the recorded result for this asset import.

## 6. Follow-Up

- If `Anemora_Main` later needs Resident_B to face the exact scene diagonal, use the retained 8-view intermediate sheet to create a scene-specific v2 idle variant instead of editing the current runtime sheet blindly.
- Any further character polish should keep Hero, Resident_A, and Resident_B pixel granularity aligned.
