# Devlog Index

> Status: v1.2 Stage 4 entry index (2026-05-06). This index is navigation only; detailed implementation history remains in each devlog file.

## 1. 概要

This file indexes the current `docs/devlog/` Markdown files by stage, milestone category, and visual evidence.

- **Purpose**: provide navigation across devlogs, connect detailed devlogs to milestone-level release notes, and keep Stage 3 Day 1 records discoverable.
- **Update policy**: when adding a new root-level devlog under `docs/devlog/`, add one entry here. Keep the time-based devlog structure; do not reorganize devlog files like `docs/asset_prompts/`.
- **CHANGELOG relation**: [CHANGELOG.md](../../CHANGELOG.md) is the release-bullet summary. Devlogs are the detailed record. This index cross-references the relevant CHANGELOG section without duplicating the release notes.
- **Related navigation**: public-facing repo overview stays in [README.md](../../README.md), while design decisions stay under [docs/adr/](../adr/README.md).

## 2. Stage 別 Devlog 一覧

Current root-level Markdown coverage: 54 files under `docs/devlog/`, including this index, the Stage 3 closeout record, and the Stage 4 Phase 0 triage records.

### 2.0.1 Stage 4 Entry (2026-05-06)

| 日付 | ファイル | topic | 関連 commit | 関連 milestone | CHANGELOG |
|---|---|---|---|---|---|
| 2026-05-06 | [2026-05-06_stage4_phase0_triage.md](2026-05-06_stage4_phase0_triage.md) | Stage 4 Phase 0 triage and backlog extraction | This task | Stage 4 Phase 0 | [Unreleased](../../CHANGELOG.md#unreleased) Stage 4 entry |
| 2026-05-06 | [2026-05-06_urp_renderobjects_pass_migration.md](2026-05-06_urp_renderobjects_pass_migration.md) | PortalStencilFeature migration from internal DrawObjectsPass to public RenderObjectsPass | This task | Stage 4 Phase 0 / URP cleanup | [Unreleased](../../CHANGELOG.md#unreleased) fixed |
| 2026-05-06 | [2026-05-06_stage4_niro_full_redraw_scope.md](2026-05-06_stage4_niro_full_redraw_scope.md) | Niro / Hero v2 full-redraw scope and asset brief handoff | This task | Stage 4 Phase 0 / character art | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-06 | [2026-05-06_stage4_brush_tutorial_hint.md](2026-05-06_stage4_brush_tutorial_hint.md) | Runtime brush tutorial hint for create / release / close affordance | This task | Stage 4 Phase 0 / brush UX | [Unreleased](../../CHANGELOG.md#unreleased) added |
| 2026-05-06 | [2026-05-06_stage4_hero_v2_import.md](2026-05-06_stage4_hero_v2_import.md) | User-approved Hero / Resident_A / Resident_B v2 import | This task | Stage 4 Phase 0 / character art | [Unreleased](../../CHANGELOG.md#unreleased) added / changed |
| 2026-05-06 | [2026-05-06_stage4_audio_polish_inventory.md](2026-05-06_stage4_audio_polish_inventory.md) | Stage 4 audio polish inventory and listening-dispatch report | This task | Stage 4 Phase 0 / audio polish | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-06 | [2026-05-06_stage4_audio_listening_checklist.md](2026-05-06_stage4_audio_listening_checklist.md) | User-guided Stage 4 audio listening and replacement checklist | This task | Stage 4 Phase 0 / audio polish | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-06 | [2026-05-06_stage4_test_count_reconciliation.md](2026-05-06_stage4_test_count_reconciliation.md) | Stage 4 EditMode runner/source test-count reconciliation | This task | Stage 4 Phase 0 / verification | [Unreleased](../../CHANGELOG.md#unreleased) verification |
| 2026-05-06 | [2026-05-06_stage4_tmp_palette_readability_review.md](2026-05-06_stage4_tmp_palette_readability_review.md) | Stage 4 TMP font and palette readability review | This task | Stage 4 Phase 0 / UI readability | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-06 | [2026-05-06_stage4_dialogue_tmp_capture_investigation.md](2026-05-06_stage4_dialogue_tmp_capture_investigation.md) | Stage 4 DialoguePanel TMP screenshot capture investigation | This task | Stage 4 Phase 0 / UI readability | [Unreleased](../../CHANGELOG.md#unreleased) documentation |

### 2.0 Stage 3 Closeout (2026-05-06)

| 日付 | ファイル | topic | 関連 commit | 関連 milestone | CHANGELOG |
|---|---|---|---|---|---|
| 2026-05-06 | [2026-05-06_stage3_closeout.md](2026-05-06_stage3_closeout.md) | Stage 3 closeout, G5 manual confirmation, and Stage 4 entry docs | `a0bd50b` input / closeout docs commit | Stage 3 closeout | [Unreleased](../../CHANGELOG.md#unreleased) verification |

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
| 2026-05-05 | [2026-05-05_g3_partial_npc_dialog_scaffold.md](2026-05-05_g3_partial_npc_dialog_scaffold.md) | G3 partial NPC placement and dialogue scaffold | `4029cc0` | G3 partial | [Unreleased](../../CHANGELOG.md#unreleased) follow-up |
| 2026-05-05 | [2026-05-05_adr_review_pass.md](2026-05-05_adr_review_pass.md) | ADR 0001-0009 review pass | `73d4808` | ADR consistency | [Unreleased](../../CHANGELOG.md#unreleased) doc hygiene |
| 2026-05-05 | [2026-05-05_test_count_reconcile.md](2026-05-05_test_count_reconcile.md) | EditMode 31 vs 32 test count reconcile | `1c7ac12` | Verification docs | [Unreleased](../../CHANGELOG.md#unreleased) verification |
| 2026-05-05 | [2026-05-05_performance_baseline.md](2026-05-05_performance_baseline.md) | Stage 3 Day 1 performance baseline | `2e3569f` | G5 / performance | [Unreleased](../../CHANGELOG.md#unreleased) verification |
| 2026-05-05 | [2026-05-05_doc_cross_link_audit.md](2026-05-05_doc_cross_link_audit.md) | Docs cross-link integrity audit | `c72a79c` | Documentation QA | [Unreleased](../../CHANGELOG.md#unreleased) doc hygiene |
| 2026-05-05 | [2026-05-05_asset_ledger_sfx_consolidation_review.md](2026-05-05_asset_ledger_sfx_consolidation_review.md) | asset_ledger SFX 30-entry consolidation review | `50ab8c0` | A5 / audio ledger | [Unreleased](../../CHANGELOG.md#unreleased) asset ledger |
| 2026-05-05 | [2026-05-05_g5_automated_run.md](2026-05-05_g5_automated_run.md) | G5 automated preflight, tests, build, and matrix results | `c17d62f` | G5 automated | [Unreleased](../../CHANGELOG.md#unreleased) verification |
| 2026-05-05 | [2026-05-05_g5_audio_rebuild.md](2026-05-05_g5_audio_rebuild.md) | G5 audio-enabled Windows rebuild and metrics refresh | `e6e3c61` | G5 audio rebuild | [Unreleased](../../CHANGELOG.md#unreleased) verification |

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
- [2026-05-05_g3_partial_npc_dialog_scaffold.md](2026-05-05_g3_partial_npc_dialog_scaffold.md) — G3 partial NPC placement and dialogue UI scaffold.

### Assets (F1-F4 + A3 buildings)

- [2026-05-05_f1_pixellab_draft.md](2026-05-05_f1_pixellab_draft.md) — F1 PixelLab Hero draft.
- [2026-05-05_g3_npc_pixellab_draft.md](2026-05-05_g3_npc_pixellab_draft.md) — G3 NPC PixelLab draft.
- [2026-05-05_f2_aseprite_hero.md](2026-05-05_f2_aseprite_hero.md) — F2 Hero Aseprite finish draft.
- [2026-05-06_stage4_hero_v2_import.md](2026-05-06_stage4_hero_v2_import.md) — Stage 4 Hero / Resident_A / Resident_B v2 import.
- [2026-05-05_g3_aseprite_residents.md](2026-05-05_g3_aseprite_residents.md) — G3 Resident A/B Aseprite finish draft.
- [2026-05-05_f4_hero_npc_prefab_animator.md](2026-05-05_f4_hero_npc_prefab_animator.md) — F4 Hero/NPC prefab animator setup.
- A3 buildings: no root devlog file exists as of v0.1; see CHANGELOG entry for commit `a547e96`.

### Localization / UI (A1 + UI foundation + A5)

- [2026-05-05_palette_v0.md](2026-05-05_palette_v0.md) — Anemora Palette v0 draft.
- [2026-05-05_tmp_jp_atlas_v0.md](2026-05-05_tmp_jp_atlas_v0.md) — TMP Japanese Atlas v0 draft.
- [2026-05-05_tmp_jp_atlas_v0_1_missing_chars_review.md](2026-05-05_tmp_jp_atlas_v0_1_missing_chars_review.md) — TMP JP Atlas missing 70 chars review.
- [2026-05-05_tmp_en_atlas_v0.md](2026-05-05_tmp_en_atlas_v0.md) — TMP English Atlas v0 draft.
- [2026-05-06_stage4_tmp_palette_readability_review.md](2026-05-06_stage4_tmp_palette_readability_review.md) — Stage 4 TMP / palette readability review.
- [2026-05-06_stage4_dialogue_tmp_capture_investigation.md](2026-05-06_stage4_dialogue_tmp_capture_investigation.md) — Stage 4 DialoguePanel TMP screenshot capture investigation.
- A1 DialogueAsset: no root devlog file exists as of v0.1; see CHANGELOG entry for commit `523c048`.

### Audio (A4 BGM + SFX)

- [2026-05-05_audio_prompts_integration_check.md](2026-05-05_audio_prompts_integration_check.md) — Audio prompt / ADR / ledger integration check after A4 BGM export.
- [2026-05-05_asset_ledger_sfx_consolidation_review.md](2026-05-05_asset_ledger_sfx_consolidation_review.md) — SFX 30-entry ledger consolidation review.
- [2026-05-05_g5_audio_rebuild.md](2026-05-05_g5_audio_rebuild.md) — Audio-enabled G5 Windows rebuild and runtime metrics refresh.
- [2026-05-06_stage4_audio_polish_inventory.md](2026-05-06_stage4_audio_polish_inventory.md) — Stage 4 audio inventory, ledger status, tests, and user listening checkpoints.
- [2026-05-06_stage4_audio_listening_checklist.md](2026-05-06_stage4_audio_listening_checklist.md) — User-guided Stage 4 audio listening and replacement checklist.
- Prompt details live in `docs/asset_prompts/`; see `sfx_zone1.md` v1.0 draft and `bgm_zone1_ambient.md`.

### G5 / Verification / Performance

- [2026-05-06_stage4_phase0_triage.md](2026-05-06_stage4_phase0_triage.md) — Phase 0 immediate-fix / backlog / no-action extraction for Stage 4 dispatch.
- [2026-05-06_urp_renderobjects_pass_migration.md](2026-05-06_urp_renderobjects_pass_migration.md) — PortalStencilFeature migration to public RenderObjectsPass and player-log warning count verification.
- [2026-05-06_stage3_closeout.md](2026-05-06_stage3_closeout.md) — Stage 3 closeout, latest demo brush repair confirmation, final test/build summary, and Stage 4 carry-forward.
- [2026-05-05_test_count_reconcile.md](2026-05-05_test_count_reconcile.md) — EditMode 31 vs 32 baseline reconcile.
- [2026-05-05_performance_baseline.md](2026-05-05_performance_baseline.md) — G5 performance baseline.
- [2026-05-05_g5_automated_run.md](2026-05-05_g5_automated_run.md) — G5 automated preflight, tests, build, and matrix results.
- [2026-05-05_g5_audio_rebuild.md](2026-05-05_g5_audio_rebuild.md) — G5 audio-enabled rebuild for manual §H verification.

### Documentation / ADR (0001-0009)

- [README.md](README.md) — Devlog operating rules.
- [2026-05-04_stage1_concept_dialogue.md](2026-05-04_stage1_concept_dialogue.md) — Stage 1 concept dialogue log.
- [2026-05-04_stage2_pitch_spec.md](2026-05-04_stage2_pitch_spec.md) — Stage 2 PITCH / SPEC drafting log.
- [2026-05-04_stage3_day0.md](2026-05-04_stage3_day0.md) — Stage 3 Day 0 planning and setup log.
- [2026-05-05_adr_review_pass.md](2026-05-05_adr_review_pass.md) — ADR 0001-0009 consistency review pass.
- [2026-05-05_doc_cross_link_audit.md](2026-05-05_doc_cross_link_audit.md) — docs-wide cross-link integrity audit.

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
| v0.2 | 2026-05-05 | 最新 devlog 8 件を追加。root-level Markdown coverage を 28 件へ更新し、G5 / verification / audio ledger / documentation QA の cross-index と CHANGELOG cross-ref を追加 |
| v0.3 | 2026-05-06 | Stage 3 closeout devlog を追加し、root-level Markdown coverage を 44 件へ更新 |
| v0.4 | 2026-05-06 | Stage 4 Phase 0 triage devlog を追加し、root-level Markdown coverage を 45 件へ更新 |
| v0.5 | 2026-05-06 | URP RenderObjectsPass migration devlog を追加し、root-level Markdown coverage を 46 件へ更新 |
| v0.6 | 2026-05-06 | Niro full-redraw scope devlog を追加し、root-level Markdown coverage を 47 件へ更新 |
| v0.7 | 2026-05-06 | Brush tutorial hint devlog を追加し、root-level Markdown coverage を 48 件へ更新 |
| v0.8 | 2026-05-06 | Hero v2 import devlog を追加し、root-level Markdown coverage を 49 件へ更新 |
| v0.9 | 2026-05-06 | Stage 4 audio polish inventory and test-count reconciliation devlogs added; root-level Markdown coverage updated to 51 files. |
| v1.0 | 2026-05-06 | TMP / palette readability review devlog を追加し、root-level Markdown coverage を 52 件へ更新 |
| v1.1 | 2026-05-06 | Stage 4 audio listening checklist devlog added; root-level Markdown coverage updated to 53 files. |
| v1.2 | 2026-05-06 | DialoguePanel TMP screenshot capture investigation devlog を追加し、root-level Markdown coverage を 54 件へ更新 |
