# Stage 3 Review Aids

> Status: v0.2 resolved aid (2026-05-05). This document remains a review record only. It does not choose, recommend, or predict future art revisions.

## 1. 概要

This document supports user visual review for high-impact or art-direction-sensitive TBD items tracked in [STAGE3_TBD_RESOLUTION.md](STAGE3_TBD_RESOLUTION.md). It is separate from the tracking sheet: `STAGE3_TBD_RESOLUTION.md` records status and final resolution, while this file lists review viewpoints, verified reference files, comparison targets, and where a decision should be reflected.

Covered TBDs and Stage 3 /spec resolution results:

| TBD ID | Review target | Resolution |
|---|---|---|
| ~~ART-01~~ | F2 Hero / NPC v1 sprite review | provisional 採用。Reflected by `c0cb631`; Stage 4 revision |
| ~~ART-03~~ | Anemora palette v0 final adoption review | provisional 採用。Reflected by `c0cb631`; Stage 4 再評価 |
| ~~ART-04~~ | TMP Misaki Gothic JP atlas final adoption review | provisional 採用。Reflected by `c0cb631`; Stage 4 再評価 |
| ~~ART-05~~ | TMP Press Start 2P EN atlas final adoption review | provisional 採用。Reflected by `c0cb631`; Stage 4 再評価 |

How to use after v0.2:

1. Treat the v0.2 entries as the Stage 3 resolved baseline.
2. If Stage 4 reopens a visual decision, open the relevant section below and compare against the listed reference files.
3. Record any Stage 4 revision as a new TBD or revision task; do not overwrite the Stage 3 resolution history.

Holding or reopening a future art question remains valid. This file records the Stage 3 result without ranking later alternatives.

## 2. F2 Hero / NPC Sprite Review

> Resolution (2026-05-05, `c0cb631`): F2 Hero / NPC v1 sprite set is provisionally adopted for Stage 3. Niro is gender-neutral, 15-19, and keeps the Snufkin-like hat direction. Stage 4 revision remains planned.

### 2.1 観点

- **中性表現**: Hero が gender-neutral に見えるか。声に依らず外見だけで複数の読みが成立するか。
- **同一性**: Hero front / side / back が同一人物に見えるか。髪型、服装、体型、色の特徴が 3 方向で一貫しているか。
- **年齢対比**: Hero と Resident_A / Resident_B の age 表現に差があるか。差の有無を user が見て判断する。
- **Palette 統一**: Anemora palette v0 が consistent に適用されているか。外れ色がないか。
- **Resident_B 暗め**: Resident_B が Resident_A と比べて暗めのトーン、座位、沈降した雰囲気を持つか。

### 2.2 参照ファイル

Final F2 / G3 sprite outputs:

- `Assets/Art/Sprites/Hero/v1/hero_stand.png`
- `Assets/Art/Sprites/Hero/v1/hero_idle.png`
- `Assets/Art/Sprites/Hero/v1/hero_walk_front.png`
- `Assets/Art/Sprites/Hero/v1/hero_walk_back.png`
- `Assets/Art/Sprites/Hero/v1/hero_walk_left.png`
- `Assets/Art/Sprites/Hero/v1/hero_walk_right.png`
- `Assets/Art/Sprites/Hero/v1/hero_hands_d7.png`
- `Assets/Art/Sprites/NPC/Resident_A/v1/resident_a_idle.png`
- `Assets/Art/Sprites/NPC/Resident_A/v1/resident_a_walk_front.png`
- `Assets/Art/Sprites/NPC/Resident_A/v1/resident_a_walk_back.png`
- `Assets/Art/Sprites/NPC/Resident_A/v1/resident_a_walk_left.png`
- `Assets/Art/Sprites/NPC/Resident_A/v1/resident_a_walk_right.png`
- `Assets/Art/Sprites/NPC/Resident_B/v1/resident_b_idle.png`

Committed F1 / G3 draft comparison files:

- `Assets/Art/Sprites/Hero/v1/_draft/front_v1.png`
- `Assets/Art/Sprites/Hero/v1/_draft/side_v1.png`
- `Assets/Art/Sprites/Hero/v1/_draft/back_v1.png`
- `Assets/Art/Sprites/NPC/Resident_A/v1/_draft/front_v1.png`
- `Assets/Art/Sprites/NPC/Resident_A/v1/_draft/back_v1.png`
- `Assets/Art/Sprites/NPC/Resident_A/v1/_draft/left_v1.png`
- `Assets/Art/Sprites/NPC/Resident_B/v1/_draft/seated_v1.png`

In-motion / integration reference files:

- `Assets/Prefabs/Characters/Hero.prefab`
- `Assets/Prefabs/Characters/Resident_A.prefab`
- `Assets/Prefabs/Characters/Resident_B.prefab`

Related devlogs:

- `docs/devlog/2026-05-05_f1_pixellab_draft.md`
- `docs/devlog/2026-05-05_g3_npc_pixellab_draft.md`
- `docs/devlog/2026-05-05_f2_aseprite_hero.md`
- `docs/devlog/2026-05-05_g3_aseprite_residents.md`
- `docs/devlog/2026-05-05_f4_hero_npc_prefab_animator.md`

`art/_intermediate/` source files are gitignored and are not listed as review references here.

### 2.3 比較対象

- F1 / G3 draft PNGs vs F2 / G3 palette-finished outputs.
- Static sprite sheets vs F4 prefab / animator appearance in Unity.
- Existing comparison language in devlogs, such as gender-neutral read, directional identity, age contrast, palette v0 containment, and Resident_B darkness.
- Current state has one F2/G3 v1 candidate set. If user wants alternate candidates, that becomes a separate generation or redraw task.

### 2.4 判断後の反映先

- **Resolved Stage 3 baseline**: `docs/STAGE3_TBD_RESOLUTION.md` row `ART-01` and `docs/legal/asset_ledger.md` rows `hero_v1_f2_sprite_set`, `resident_a_v1_g3_sprite_set`, and `resident_b_v1_g3_sprite_set` were updated by `c0cb631`.
- **Minor revision**: keep F4 prefab references stable when possible; revise only the affected PNGs under `Assets/Art/Sprites/Hero/v1/` or `Assets/Art/Sprites/NPC/`.
- **Alternate candidate comparison**: add new candidate assets and devlog notes in a separate task, then compare against the paths above.
- **Full replacement**: decide whether an F3-style supplemental art task is needed, then update `ART-01` and the related sprite / prefab references after the replacement lands.

## 3. Anemora パレット v0 採用最終確定

> Resolution (2026-05-05, `c0cb631`): Anemora palette v0 is provisionally adopted for Stage 3 and remains subject to Stage 4 reevaluation.

### 3.1 観点

- **トーンの一貫性**: 16-32 muted earth color range が「衰退する街」の atmosphere を支えられるか。
- **VS 中で運用可能**: sprite、building、UI、particle を同じ palette family で扱えるか。
- **拡張余地**: Stage 4 以降の別ゾーンで色変奏を加えられるか。

### 3.2 参照ファイル

- `Assets/Art/anemora_palette_v0.png`
- `Assets/Art/anemora_palette_v0.gpl`
- `Assets/Art/anemora_palette_v0.aseprite-palette`
- `docs/devlog/2026-05-05_palette_v0.md`

### 3.3 比較対象

- Current comparison target is v0 only.
- If user wants another palette, create a v0.x or alternate palette task and compare it against the v0 files above.
- Hold is valid if Stage 3 can continue with v0 as draft while Stage 4 palette direction remains open.

### 3.4 判断後の反映先

- **Resolved Stage 3 baseline**: `docs/STAGE3_TBD_RESOLUTION.md` row `ART-03` and `docs/legal/asset_ledger.md` row `anemora_palette_v0` were updated by `c0cb631`.
- **Minor revision**: create v0.1 palette assets in a separate commit and update affected sprite / UI references after review.
- **Full replacement**: create alternate palette assets, then review sprite, building, and UI imports against the replacement.

## 4. TMP 美咲ゴシック JP Atlas 採用最終確定

> Resolution (2026-05-05, `c0cb631`): TMP 美咲ゴシック JP atlas is provisionally adopted for Stage 3 and remains subject to Stage 4 reevaluation.

### 4.1 観点

- **VS 文言で missing 字なし**: Missing 70 characters from the v0 bake should be checked against actual Stage 3 String Tables or display text.
- **読みやすさ**: 16x16 pixel font behavior in TMP SDF output should be reviewed in panel and menu contexts.
- **VRAM 容量**: 4096 x 4096 Alpha8 runtime estimate is 16.0 MiB, about 0.78% of the laptop's 2 GiB VRAM.
- **License**: Misaki font license state should remain aligned with GitHub Public, itch.io, and Steam commercial use.

### 4.2 参照ファイル

- `Assets/UI/Localization/Fonts/Anemora_JP.asset`
- `Assets/UI/Localization/Fonts/Anemora_JP_Atlas.asset`
- `Assets/UI/Localization/Fonts/ThirdParty/misaki_gothic.ttf`
- `docs/devlog/2026-05-05_tmp_jp_atlas_v0.md`
- `docs/devlog/2026-05-05_tmp_jp_atlas_v0_1_missing_chars_review.md`
- `docs/legal/asset_ledger.md`

### 4.3 比較対象

- Current comparison target is Misaki Gothic JP atlas v0 only.
- If user wants another JP font, create a separate atlas generation task and compare readability, coverage, license, and VRAM against the files above.
- Hold is valid if Stage 3 uses the current atlas draft and Stage 4 revisits font selection.

### 4.4 判断後の反映先

- **Resolved Stage 3 baseline**: `docs/STAGE3_TBD_RESOLUTION.md` row `ART-04` and `docs/legal/asset_ledger.md` row `anemora_jp_tmp_font_v0` were updated by `c0cb631`.
- **Missing character work needed**: add fallback font, extend atlas coverage, or adjust actual VS text in a separate task.
- **Alternate JP font comparison**: generate a new TMP atlas and devlog, then compare against this section's reference files.

## 5. TMP Press Start 2P EN Atlas 採用最終確定

> Resolution (2026-05-05, `c0cb631`): TMP Press Start 2P EN atlas is provisionally adopted for Stage 3 and remains subject to Stage 4 reevaluation.

### 5.1 観点

- **読みやすさ**: 8x8 pixel-based font behavior in TMP SDF output should be checked against expected English VS text.
- **HD-2D 雰囲気との整合**: Press Start 2P should be viewed beside Anemora palette v0 and the JP atlas output.
- **License**: SIL Open Font License 1.1 should remain aligned with GitHub Public, itch.io, and Steam commercial use.
- **代替候補**: Pixel Square, Pixelmix, and VCR OSD Mono can remain parallel candidates for Stage 4 review.

### 5.2 参照ファイル

- `Assets/UI/Localization/Fonts/Anemora_EN.asset`
- `Assets/UI/Localization/Fonts/Anemora_EN_Atlas.asset`
- `Assets/UI/Localization/Fonts/ThirdParty/PressStart2P-Regular.ttf`
- `Assets/UI/Localization/Fonts/ThirdParty/PressStart2P_LICENSE.txt`
- `docs/devlog/2026-05-05_tmp_en_atlas_v0.md`

### 5.3 比較対象

- Current generated candidate: Press Start 2P.
- Non-generated candidate names for possible future comparison: Pixel Square, Pixelmix, VCR OSD Mono.
- Hold is valid if the current EN atlas remains a draft until Stage 4 localization review.

### 5.4 判断後の反映先

- **Resolved Stage 3 baseline**: `docs/STAGE3_TBD_RESOLUTION.md` row `ART-05` and `docs/legal/asset_ledger.md` row `anemora_en_tmp_font_v0` were updated by `c0cb631`.
- **Alternate EN font comparison**: generate one or more parallel TMP atlases and compare readability, license, and VRAM in a separate task.
- **Fallback chain update**: if the accepted EN font changes, update JP / EN fallback links and record the change in a devlog.

## 6. Review 判断後の運用

For the Stage 3 rows covered by this document, the TBDs are resolved by `c0cb631` and closed in `docs/STAGE3_TBD_RESOLUTION.md`.

If Stage 4 reopens one of these areas:

1. Create a new row in `docs/STAGE3_TBD_RESOLUTION.md` or the Stage 4 equivalent, instead of editing away the Stage 3 resolution.
2. Update `docs/legal/asset_ledger.md` only for the newly adopted or revised asset rows.
3. Keep ADR-0009 status as Accepted unless a separate ADR revision changes the pipeline decision.

Other valid outcomes:

- **Hold**: keep multiple possibilities open and revisit at Stage 4 entry.
- **Minor revision**: create a narrow revision commit for the affected asset only.
- **Full replacement**: start a separate asset generation / redraw / atlas generation task.

This review aid records the closed Stage 3 rows and remains available as a comparison checklist for later revisions.

## 7. 更新履歴

| Version | Date | Change |
|---|---|---|
| v0.1 | 2026-05-05 | 初版起草。ART-01 / ART-03 / ART-04 / ART-05 の目視レビュー aid を追加 |
| v0.2 | 2026-05-05 | Stage 3 /spec resolution interview の結果を反映。ART-01 / ART-03 / ART-04 / ART-05 を provisional 採用として close し、反映 commit `c0cb631` を記録 |
