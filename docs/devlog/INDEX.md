# Devlog Index

> Status: v0.1 draft (2026-05-05). This index is navigation only; detailed implementation history remains in each devlog file.

## 1. 概要

This file indexes the current `docs/devlog/` Markdown files by stage, milestone category, and visual evidence.

- **Purpose**: provide navigation across devlogs, connect detailed devlogs to milestone-level release notes, and keep Stage 3 Day 1 records discoverable.
- **Update policy**: when adding a new root-level devlog under `docs/devlog/`, add one entry here. Keep the time-based devlog structure; do not reorganize devlog files like `docs/asset_prompts/`.
- **CHANGELOG relation**: [CHANGELOG.md](../../CHANGELOG.md) is the release-bullet summary. Devlogs are the detailed record. This index cross-references the relevant CHANGELOG section without duplicating the release notes.
- **Related navigation**: public-facing repo overview stays in [README.md](../../README.md), while design decisions stay under [docs/adr/](../adr/README.md).

## 2. Stage 別 Devlog 一覧

Current root-level Markdown coverage: 19 files under `docs/devlog/`, including this index.

### 2.1 Stage 3 Day 1 (2026-05-05)

| 日付 | ファイル | topic | 関連 commit | 関連 milestone | CHANGELOG |
|---|---|---|---|---|---|
| 2026-05-05 | [2026-05-05_urp_setup_check.md](2026-05-05_urp_setup_check.md) | URP setup check and package resolution | `4fa56d9` | Stage 3E setup | [0.1.0-alpha.1](../../CHANGELOG.md#010-alpha1---2026-05-05) §Engine/Pipeline |
| 2026-05-05 | [2026-05-05_e0_urp_pipeline_asset.md](2026-05-05_e0_urp_pipeline_asset.md) | E0 URP Pipeline Asset | `f854466` | E0 | [0.1.0-alpha.1](../../CHANGELOG.md#010-alpha1---2026-05-05) §Engine/Pipeline |
| 2026-05-05 | [2026-05-05_e1_stencil_minimum.md](2026-05-05_e1_stencil_minimum.md) | E1 stencil minimum and E2/E3 skeleton | `773d35f` | E1 / E2 / E3 | [0.1.0-alpha.1](../../CHANGELOG.md#010-alpha1---2026-05-05) §Engine/Pipeline |
| 2026-05-05 | [2026-05-05_a2_anemora_main_wiring.md](2026-05-05_a2_anemora_main_wiring.md) | A2 Anemora_Main wiring and boundary round-trip | `cb2b6ed` | A2 | [0.1.0-alpha.1](../../CHANGELOG.md#010-alpha1---2026-05-05) §Scene/Wiring |
| 2026-05-05 | [2026-05-05_g4_actionrecord_trigger.md](2026-05-05_g4_actionrecord_trigger.md) | G4 ActionRecord trigger wiring | `0644822` | G4 | [0.1.0-alpha.1](../../CHANGELOG.md#010-alpha1---2026-05-05) §Scene/Wiring |
| 2026-05-05 | [2026-05-05_f1_pixellab_draft.md](2026-05-05_f1_pixellab_draft.md) | F1 PixelLab Hero draft | `4a420a5` | F1 | [0.1.0-alpha.1](../../CHANGELOG.md#010-alpha1---2026-05-05) §Assets |
| 2026-05-05 | [2026-05-05_g3_npc_pixellab_draft.md](2026-05-05_g3_npc_pixellab_draft.md) | G3 NPC PixelLab draft | `4a420a5` | G3 draft | [0.1.0-alpha.1](../../CHANGELOG.md#010-alpha1---2026-05-05) §Assets |
| 2026-05-05 | [2026-05-05_f2_aseprite_hero.md](2026-05-05_f2_aseprite_hero.md) | F2 Hero Aseprite finish draft | `4d2092a` | F2 | [0.1.0-alpha.1](../../CHANGELOG.md#010-alpha1---2026-05-05) §Assets |
| 2026-05-05 | [2026-05-05_g3_aseprite_residents.md](2026-05-05_g3_aseprite_residents.md) | G3 Resident A/B Aseprite finish draft | `4d2092a` | G3 finish | [0.1.0-alpha.1](../../CHANGELOG.md#010-alpha1---2026-05-05) §Assets |
| 2026-05-05 | [2026-05-05_f4_hero_npc_prefab_animator.md](2026-05-05_f4_hero_npc_prefab_animator.md) | F4 Hero/NPC prefab animator setup | `d2c95c2` | F4 | [0.1.0-alpha.1](../../CHANGELOG.md#010-alpha1---2026-05-05) §Assets |
| 2026-05-05 | [2026-05-05_palette_v0.md](2026-05-05_palette_v0.md) | Anemora Palette v0 draft | `a8f2710` | UI foundation v0 | [0.1.0-alpha.1](../../CHANGELOG.md#010-alpha1---2026-05-05) §Localization/UI |
| 2026-05-05 | [2026-05-05_tmp_jp_atlas_v0.md](2026-05-05_tmp_jp_atlas_v0.md) | TMP Japanese Atlas v0 draft | `a8f2710` | UI foundation v0 | [0.1.0-alpha.1](../../CHANGELOG.md#010-alpha1---2026-05-05) §Localization/UI |
| 2026-05-05 | [2026-05-05_tmp_jp_atlas_v0_1_missing_chars_review.md](2026-05-05_tmp_jp_atlas_v0_1_missing_chars_review.md) | TMP JP Atlas missing 70 chars review | `3a29757` | UI foundation follow-up | [0.1.0-alpha.1](../../CHANGELOG.md#010-alpha1---2026-05-05) §Localization/UI |
| 2026-05-05 | [2026-05-05_tmp_en_atlas_v0.md](2026-05-05_tmp_en_atlas_v0.md) | TMP English Atlas v0 draft | `f5b4685` | A5 TMP EN atlas | [0.1.0-alpha.1](../../CHANGELOG.md#010-alpha1---2026-05-05) §Localization/UI |

### 2.2 Stage 3 Day 0 (2026-05-04)

| 日付 | ファイル | topic | 関連 commit | 関連 milestone | CHANGELOG |
|---|---|---|---|---|---|
| 2026-05-04 | [2026-05-04_stage3_day0.md](2026-05-04_stage3_day0.md) | Stage 3 Day 0 planning and setup log | `f2e40eb` | Stage 3 Day 0 | Pre-0.1.0-alpha.1 foundation |

### 2.3 Stage 1 / Stage 2 (2026-05-04)

| 日付 | ファイル | topic | 関連 commit | 関連 milestone | CHANGELOG |
|---|---|---|---|---|---|
| 2026-05-04 | [2026-05-04_stage1_concept_dialogue.md](2026-05-04_stage1_concept_dialogue.md) | Stage 1 concept dialogue log | `9d4edbb` | Stage 1 | Pre-0.1.0-alpha.1 foundation |
| 2026-05-04 | [2026-05-04_stage2_pitch_spec.md](2026-05-04_stage2_pitch_spec.md) | Stage 2 PITCH / SPEC drafting log | `9d4edbb` | Stage 2 | Pre-0.1.0-alpha.1 foundation |

### 2.4 Devlog 運用

| 日付 | ファイル | topic | 関連 commit | 関連 milestone | CHANGELOG |
|---|---|---|---|---|---|
| 2026-05-04 | [README.md](README.md) | Devlog operating rules | `9d4edbb` | Devlog operations | Not release-scoped |
| 2026-05-05 | [INDEX.md](INDEX.md) | Devlog navigation index | This task | Devlog navigation | Not release-scoped |

## 3. カテゴリ別 Cross-Index

### Engine / Pipeline (E0-E5)

- [2026-05-05_urp_setup_check.md](2026-05-05_urp_setup_check.md) — URP setup check and package resolution.
- [2026-05-05_e0_urp_pipeline_asset.md](2026-05-05_e0_urp_pipeline_asset.md) — E0 URP Pipeline Asset.
- [2026-05-05_e1_stencil_minimum.md](2026-05-05_e1_stencil_minimum.md) — E1 stencil minimum and E2/E3 skeleton.

### Scene / Wiring (A2 + G4)

- [2026-05-05_a2_anemora_main_wiring.md](2026-05-05_a2_anemora_main_wiring.md) — A2 Anemora_Main wiring and boundary round-trip.
- [2026-05-05_g4_actionrecord_trigger.md](2026-05-05_g4_actionrecord_trigger.md) — G4 ActionRecord trigger wiring.

### Assets (F1-F4 + A3 buildings)

- [2026-05-05_f1_pixellab_draft.md](2026-05-05_f1_pixellab_draft.md) — F1 PixelLab Hero draft.
- [2026-05-05_g3_npc_pixellab_draft.md](2026-05-05_g3_npc_pixellab_draft.md) — G3 NPC PixelLab draft.
- [2026-05-05_f2_aseprite_hero.md](2026-05-05_f2_aseprite_hero.md) — F2 Hero Aseprite finish draft.
- [2026-05-05_g3_aseprite_residents.md](2026-05-05_g3_aseprite_residents.md) — G3 Resident A/B Aseprite finish draft.
- [2026-05-05_f4_hero_npc_prefab_animator.md](2026-05-05_f4_hero_npc_prefab_animator.md) — F4 Hero/NPC prefab animator setup.
- A3 buildings: no root devlog file exists as of v0.1; see CHANGELOG entry for commit `a547e96`.

### Localization / UI (A1 + UI foundation + A5)

- [2026-05-05_palette_v0.md](2026-05-05_palette_v0.md) — Anemora Palette v0 draft.
- [2026-05-05_tmp_jp_atlas_v0.md](2026-05-05_tmp_jp_atlas_v0.md) — TMP Japanese Atlas v0 draft.
- [2026-05-05_tmp_jp_atlas_v0_1_missing_chars_review.md](2026-05-05_tmp_jp_atlas_v0_1_missing_chars_review.md) — TMP JP Atlas missing 70 chars review.
- [2026-05-05_tmp_en_atlas_v0.md](2026-05-05_tmp_en_atlas_v0.md) — TMP English Atlas v0 draft.
- A1 DialogueAsset: no root devlog file exists as of v0.1; see CHANGELOG entry for commit `523c048`.

### Audio (A4 BGM + SFX)

- No root devlog file exists as of v0.1.
- Prompt details live in `docs/asset_prompts/`; see `sfx_zone1.md` v1.0 draft and `bgm_zone1_ambient.md`.

### Documentation / ADR (0001-0009)

- [README.md](README.md) — Devlog operating rules.
- [2026-05-04_stage1_concept_dialogue.md](2026-05-04_stage1_concept_dialogue.md) — Stage 1 concept dialogue log.
- [2026-05-04_stage2_pitch_spec.md](2026-05-04_stage2_pitch_spec.md) — Stage 2 PITCH / SPEC drafting log.
- [2026-05-04_stage3_day0.md](2026-05-04_stage3_day0.md) — Stage 3 Day 0 planning and setup log.

## 4. Screenshots 一覧

| Screenshot | Linked devlog | Purpose |
|---|---|---|
| [a2_main_current_open.png](screenshots/a2_main_current_open.png) | [2026-05-05_a2_anemora_main_wiring.md](2026-05-05_a2_anemora_main_wiring.md) | Current-side Anemora_Main view with portal open |
| [a2_main_past_after_cross.png](screenshots/a2_main_past_after_cross.png) | [2026-05-05_a2_anemora_main_wiring.md](2026-05-05_a2_anemora_main_wiring.md) | Past-side Anemora_Main view after crossing |
| [a2_main_current_after_return.png](screenshots/a2_main_current_after_return.png) | [2026-05-05_a2_anemora_main_wiring.md](2026-05-05_a2_anemora_main_wiring.md) | Current-side Anemora_Main view after return |
| [e1_portal_front.png](screenshots/e1_portal_front.png) | [2026-05-05_e1_stencil_minimum.md](2026-05-05_e1_stencil_minimum.md) | E1 portal front visual evidence |
| [e1_portal_side.png](screenshots/e1_portal_side.png) | [2026-05-05_e1_stencil_minimum.md](2026-05-05_e1_stencil_minimum.md) | E1 portal side visual evidence |
| [e1_portal_back.png](screenshots/e1_portal_back.png) | [2026-05-05_e1_stencil_minimum.md](2026-05-05_e1_stencil_minimum.md) | E1 portal back visual evidence |

## 5. 更新履歴

| Version | Date | Change |
|---|---|---|
| v0.1 | 2026-05-05 | 初版起草。既存 root devlog 18 件、この index、screenshots 6 件を index 化 |
