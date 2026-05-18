# Stage 4 Character Runtime Restyle

Date: 2026-05-08

## Summary

The user-approved `radical01_watercolor_pixel_mix_03` character direction was converted into runtime-ready sprite replacements for the first VS / Chapter 1 character set: Niro / Hero, Resident_A, and Resident_B.

This pass preserves the existing Unity import contract:

- `32 x 48` sprite cells
- PPU 32
- point filtering / no mipmaps via unchanged `.meta`
- transparent background
- existing sprite GUIDs, slice rects, pivots, prefab references, AnimatorController references, AnimationClip references, and scene references

No scene, prefab, AnimatorController, AnimationClip, or `.meta` file was intentionally changed.

## Runtime Asset Changes

- `Assets/Art/Sprites/Hero/v2/hero_stand.png`
- `Assets/Art/Sprites/Hero/v2/hero_idle.png`
- `Assets/Art/Sprites/Hero/v2/hero_walk_front.png`
- `Assets/Art/Sprites/Hero/v2/hero_walk_back.png`
- `Assets/Art/Sprites/Hero/v2/hero_walk_left.png`
- `Assets/Art/Sprites/Hero/v2/hero_walk_right.png`
- `Assets/Art/Sprites/NPC/Resident_A/v2/resident_a_idle.png`
- `Assets/Art/Sprites/NPC/Resident_A/v2/resident_a_walk_front.png`
- `Assets/Art/Sprites/NPC/Resident_A/v2/resident_a_walk_back.png`
- `Assets/Art/Sprites/NPC/Resident_A/v2/resident_a_walk_left.png`
- `Assets/Art/Sprites/NPC/Resident_A/v2/resident_a_walk_right.png`
- `Assets/Art/Sprites/NPC/Resident_B/v2/resident_b_idle.png`

## Processing Notes

- Approved style reference: `docs/review_gallery/imports/stage4_character_style_direction_accepted_2026-05-08/character_style_direction_radical01_watercolor_pixel_mix_03_approved.png`
- Source production sheets are kept under ignored intermediate storage: `art/_intermediate/stage4_character_v3_runtime_sources_2026-05-08/`
- Review contact sheet: `docs/review_gallery/imports/stage4_character_runtime_restyle_2026-05-08/stage4_character_v2_restyle_runtime_contact_6x.png`
- Extraction used row-wise connected-component detection instead of fixed 4-column slicing because the generated production sheets did not align to exact grid columns.
- Magenta background and anti-aliased magenta fringe were removed, then each frame was downscaled into the existing `32 x 48` runtime cell.
- Runtime PNG alpha is hard `0 / 255`; no magenta-like opaque pixels remain in the checked output.

## Verification

- EditMode: `39/39 passed`
  - Results: `stage4_character_restyle_editmode.xml`
- PlayMode: `31 passed / 32 total`
  - The skipped test is the existing `[Explicit]` manual TMP screenshot capture harness.
  - Results: `stage4_character_restyle_playmode.xml`
- Windows Standalone build smoke: success
  - Output: `Builds/Stage4Smoke/2026-05-08-character-restyle/Anemora_Stage4_CharacterRestyle_Smoke.exe`
  - Build log marker: `Build Finished, Result: Success.`
- Player smoke: 30 seconds at `1280 x 720`
  - Player log: `stage4_character_restyle_player.log`
  - Checked patterns: `Error`, `Exception`, `Assert`, `DrawObjectsPass`, `RenderGraph`, `NullReference`, `MissingReference`, `Failed`
  - Result: no matches.
- Stage 4 scale-lineup captures were regenerated with the new runtime sprites:
  - `docs/devlog/screenshots/stage4_scale_lineup_current_demo.png`
  - `docs/devlog/screenshots/stage4_scale_lineup_target_metrics.png`

## Follow-Up

- Manual in-game review should still check character scale, especially whether Resident_A / Resident_B now read too large relative to Niro in the real camera context.
- The current `Anemora_Main` remains a Stage 3 VS / wiring nucleus, not the final first-map layout.
- The next high-impact visual work should remain environment density, material / lighting polish, and first-map authored asset scale rather than increasing character sprite resolution.
