# Devlog Index

> Status: v2.4 Chapter 1 Scene 5 v1 + map redesign + Z terminology cleanup (2026-05-09). This index is navigation only; detailed implementation history remains in each devlog file.

## 1. 概要

This file indexes the current `docs/devlog/` Markdown files by stage, milestone category, and visual evidence.

- **Purpose**: provide navigation across devlogs, connect detailed devlogs to milestone-level release notes, and keep Stage 3 Day 1 records discoverable.
- **Update policy**: when adding a new root-level devlog under `docs/devlog/`, add one entry here. Keep the time-based devlog structure; do not reorganize devlog files like `docs/asset_prompts/`.
- **CHANGELOG relation**: [CHANGELOG.md](../../CHANGELOG.md) is the release-bullet summary. Devlogs are the detailed record. This index cross-references the relevant CHANGELOG section without duplicating the release notes.
- **Related navigation**: public-facing repo overview stays in [README.md](../../README.md), while design decisions stay under [docs/adr/](../adr/README.md).

## 2. Stage 別 Devlog 一覧

Current root-level Markdown coverage: 111 files under `docs/devlog/`, including this index, the Stage 3 closeout record, the Stage 4 Phase 0 triage records, the Chapter 1 narrative design records, and Fast VS implementation records.

### 2.0.0a Fast VS V24 Sample (2026-05-18)

| 日付 | ファイル | topic | 関連 commit | 関連 milestone | CHANGELOG |
|---|---|---|---|---|---|
| 2026-05-18 | [2026-05-18_fast_vs_story_canon_v4_source_check.md](2026-05-18_fast_vs_story_canon_v4_source_check.md) | Fast VS story canon source check: Scene 1 v4 adoption, parent/child material assigned to Scene 3 Aria/Karla, Niro house separate hidden foreshadowing | This task | Fast VS / story canon | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-18 | [2026-05-18_fast_vs_story_v4_review_fix.md](2026-05-18_fast_vs_story_v4_review_fix.md) | Fast VS story v4 review fix: Reto/Aria wording cleanup, book pickup/returned-book gating, past book marker replacement, HUD positioning, validation evidence | This task | Fast VS / story implementation | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-18 | [2026-05-18_fast_vs_review_gap_fix_plan_and_result.md](2026-05-18_fast_vs_review_gap_fix_plan_and_result.md) | Fast VS review gap fix: worker-cycle record, persistent guide HUD, house-exit brush trigger, Reto desk book overlap, past library clean tables, book pickup/return gating, validation evidence | This task | Fast VS / review fix | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-18 | [2026-05-18_fast_vs_pre_exit_bookshelf_tabletop_fix.md](2026-05-18_fast_vs_pre_exit_bookshelf_tabletop_fix.md) | Fast VS pre-exit event, side bookshelf parity, and tabletop book-height fix with worker-cycle record and validation evidence | This task | Fast VS / review fix | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-18 | [2026-05-18_fast_vs_ui_cue_library_home_polish.md](2026-05-18_fast_vs_ui_cue_library_home_polish.md) | Fast VS UI cue, current-side red floor glow, library ruin / bookshelf texture, past door-window texture, and Niro house bed/book polish with worker-cycle record and validation evidence | This task | Fast VS / review fix | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-18 | [2026-05-18_fast_vs_plaza_library_side_shelf_fix.md](2026-05-18_fast_vs_plaza_library_side_shelf_fix.md) | Fast VS plaza/library side-shelf, facade, entrance-slab, red cue, and typewriter review fix with worker-cycle record and validation evidence | This task | Fast VS / review fix | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-18 | [2026-05-18_fast_vs_facade_redcue_text_fix.md](2026-05-18_fast_vs_facade_redcue_text_fix.md) | Fast VS plaza facade regression, red cue/marker material, dry fountain debris, and story text fix with worker-cycle record and validation evidence | This task | Fast VS / review fix | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-18 | [2026-05-18_fast_vs_marker_glow_bookshelf_revert.md](2026-05-18_fast_vs_marker_glow_bookshelf_revert.md) | Fast VS red marker flattening, stable red floor cue, back-wall book-row texture panels, Reto-event text fix, and plaza-window revert with worker-cycle record and validation evidence | This task | Fast VS / review fix | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-18 | [2026-05-18_fast_vs_red_marker_aria_bookshelf_review_fix.md](2026-05-18_fast_vs_red_marker_aria_bookshelf_review_fix.md) | Fast VS house-exit parentheses, red floor cue visibility, moving framed red markers for book/Aria, and horizontal bookshelf texture panels with worker-cycle record and validation evidence | This task | Fast VS / review fix | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-18 | [2026-05-18_fast_vs_bookshelf_front_texture_red_edge_fix.md](2026-05-18_fast_vs_bookshelf_front_texture_red_edge_fix.md) | Fast VS moving red floor cue, 12-edge thin red marker frame, and front-facing bookshelf texture panels applied to back/side shelves with worker-cycle record and validation evidence | This task | Fast VS / review fix | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-18 | [2026-05-18_fast_vs_external_bookshelf_story_motion.md](2026-05-18_fast_vs_external_bookshelf_story_motion.md) | Fast VS external CC0 bookshelf texture adoption, Aria return-record line removal, and Reto look-up motion beat after `...本物だ`, with worker-cycle record and validation evidence | This task | Fast VS / review fix | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-18 | [2026-05-18_fast_vs_pocket_glow_yellow_cues.md](2026-05-18_fast_vs_pocket_glow_yellow_cues.md) | Fast VS Reto resolve line, long Timewriter pause, Niro pocket glow, yellow Time Window cues, and front-side `!` marker glyphs with worker-cycle record and validation evidence | This task | Fast VS / review fix | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-18 | [2026-05-18_fast_vs_door_brush_reto_pause_refine.md](2026-05-18_fast_vs_door_brush_reto_pause_refine.md) | Fast VS door-brush pre-transition trigger, question/brush order, revised pocket/Reto wording, yellow guide text, and Reto down/up pause beat with worker-cycle record and validation evidence | This task | Fast VS / review fix | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-18 | [2026-05-18_fast_vs_door_brush_center_icon_skip_guard.md](2026-05-18_fast_vs_door_brush_center_icon_skip_guard.md) | Fast VS door-brush center icon, actual door-trigger skip guard, visible Reto lowering/raising motion, and library ruin overlap cleanup with worker-cycle record and validation evidence | This task | Fast VS / review fix | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-18 | [2026-05-18_fast_vs_question_pause_portal_transition_close.md](2026-05-18_fast_vs_question_pause_portal_transition_close.md) | Fast VS head-close question marker, restored Reto silent pause before `いえ。今のは、ただの独り言です。`, and current-time Time Window close on map transition with worker-cycle record and validation evidence | This task | Fast VS / review fix | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-18 | [2026-05-18_fast_vs_library_shelf_window_objective_cleanup.md](2026-05-18_fast_vs_library_shelf_window_objective_cleanup.md) | Fast VS library side-shelf height, left overlap-box removal, post-past-flags objective text, and library/plaza window pane cleanup with worker-cycle record and validation evidence | This task | Fast VS / review fix | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-18 | [2026-05-18_fast_vs_outdoor_sky_background_revert.md](2026-05-18_fast_vs_outdoor_sky_background_revert.md) | Fast VS rejected outdoor sky/background pass reverted to no-added-backdrop state with worker-cycle record and validation evidence | This task | Fast VS / visual background | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-18 | [2026-05-18_fast_vs_left_bottom_timewindow_hint.md](2026-05-18_fast_vs_left_bottom_timewindow_hint.md) | Fast VS lower-left HUD narrowed to a brief Time Window creation hint only, with pre-unlock/after-unlock validation and worker-cycle record | This task | Fast VS / UI guidance | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-18 | [2026-05-18_fast_vs_skip_opening_wake_line.md](2026-05-18_fast_vs_skip_opening_wake_line.md) | Fast VS branch-only removal of the initial wake dialogue line, with playable-start validation and worker-cycle record | This task | Fast VS / story pacing | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-18 | [2026-05-18_fast_vs_public_repo_promotion.md](2026-05-18_fast_vs_public_repo_promotion.md) | Fast VS public baseline promotion plan: stable main, archive tags, continuation branch, and preservation of devlogs | This task | Fast VS / repository management | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-18 | [2026-05-18_fast_vs_public_readme_playability.md](2026-05-18_fast_vs_public_readme_playability.md) | Fast VS public README playability pass: play-first Windows build instructions, controls, technical entry points, and release zip note | This task | Fast VS / public onboarding | [Unreleased](../../CHANGELOG.md#unreleased) documentation |

### 2.0.0 Chapter 1 Narrative Design (2026-05-09)

| 日付 | ファイル | topic | 関連 commit | 関連 milestone | CHANGELOG |
|---|---|---|---|---|---|
| 2026-05-09 | [2026-05-09_chapter1_scene1_v3_final.md](2026-05-09_chapter1_scene1_v3_final.md) | Chapter 1 Scene 1 v3 final 確定 (8 セクション [1.A]-[1.H])、pending 3 点 + 後出し 2 点すべて解決 (※ 後の v4 改訂で本出現復活、レト dialogue 復活、L-γ 二層化、layer1_revision devlog 参照) | This task | Stage 4 / Chapter 1 narrative | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-09 | [2026-05-09_chapter1_scene2_v1_final.md](2026-05-09_chapter1_scene2_v1_final.md) | Chapter 1 Scene 2 v1 final 確定 (6 セクション [2.A]-[2.F]、ミア設定確定 = 元縫物職人 / 一人暮らし)、視界解放メカニズム確定 (1+5: 陽の角度 + 音の誘導、土砂封鎖を視界誘導に統一)、sister doc map 整合修正 (シーン 5 を南端へ、街角を南方面へ) | This task | Stage 4 / Chapter 1 narrative | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-09 | [2026-05-09_chapter1_layer1_revision_and_scene3_design.md](2026-05-09_chapter1_layer1_revision_and_scene3_design.md) | Chapter 1 Layer 1 仕様改訂 (観察ルール撤廃、Layer 1 から個人レベル干渉 + 痕跡可)、時の窓モード v3.2 統合 (内部範囲指定で統一、ドアモードはギミック扱い)、シーン 1 v4 (本出現復活 + レト dialogue 復活)、シーン 3 v3 動線骨格 6 セクション [3.A]-[3.F] (廃墟も入れる、商売教え + エリュトリア連動)、シーン 4 T4 再定義 (空間的・遠地での因果連鎖の初体験)、Niro 家の隠し伏線追加 (第 1 章サービス、過去 = 別の家族の気配 / 未来 = 廃墟) | This task | Stage 4 / Chapter 1 narrative + spec | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-09 | [2026-05-09_chapter1_scene4_v1.md](2026-05-09_chapter1_scene4_v1.md) | Chapter 1 Scene 4 v1 動線骨格 9 セクション [4.A]-[4.I] (カイア出会い → 異変発見 → 時の窓発動オート → ダリオ香料に干渉プレイヤー → 痕跡可視化 → 代償発見 → Z 第 1 段階気付き → シーン 5 動線)、エリュトリア確定 (失われた交易相手の街 / EC-2 香料連動)、カイア設定確定 (一人暮らし / 観察力寡黙 / ナッツ農家)、過去のカイア畑の所有者 = カイアの先祖 (設計のみストーリー非言及)、仕様 v3.2 例外追記 (ストーリー上の見せ場でのオート発動) | This task | Stage 4 / Chapter 1 narrative + spec | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-09 | [2026-05-09_chapter1_scene5_v1_and_map_redesign.md](2026-05-09_chapter1_scene5_v1_and_map_redesign.md) | Chapter 1 Scene 5 v1 動線骨格 5 セクション [5.A]-[5.E] (廃墟予兆探索 + 鍵かかった建物 + 入れる 1 個 + 森入口 + クライマックス気付き 第 2 段階「自分の動きが、世界を」+ 章終了 + BF1 起点 = 章切り替えアニメで小石蹴る) + **動線再設計** (シーン 1-5 を東/北東展開、Niro 家南西/ミア家南東/街角ミア家東/カイア畑街角北東/廃墟予兆カイア畑東) + Z 用語整理 (→ 「クライマックス気付き第 X 段階」) + 白 (現在) 窓 1 周目なし + Niro 家伏線「未来 = 廃墟」を 2 周目に修正 + モブ NPC 使いまわし追記 | This task | Stage 4 / Chapter 1 narrative + spec | [Unreleased](../../CHANGELOG.md#unreleased) documentation |

### 2.0.1 Stage 4 Entry (2026-05-06)

| 日付 | ファイル | topic | 関連 commit | 関連 milestone | CHANGELOG |
|---|---|---|---|---|---|
| 2026-05-06 | [2026-05-06_stage4_phase0_triage.md](2026-05-06_stage4_phase0_triage.md) | Stage 4 Phase 0 triage and backlog extraction | This task | Stage 4 Phase 0 | [Unreleased](../../CHANGELOG.md#unreleased) Stage 4 entry |
| 2026-05-06 | [2026-05-06_urp_renderobjects_pass_migration.md](2026-05-06_urp_renderobjects_pass_migration.md) | PortalStencilFeature migration from internal DrawObjectsPass to public RenderObjectsPass | This task | Stage 4 Phase 0 / URP cleanup | [Unreleased](../../CHANGELOG.md#unreleased) fixed |
| 2026-05-06 | [2026-05-06_stage4_niro_full_redraw_scope.md](2026-05-06_stage4_niro_full_redraw_scope.md) | Niro / Hero v2 full-redraw scope and asset brief handoff | This task | Stage 4 Phase 0 / character art | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
| 2026-05-06 | [2026-05-06_stage4_brush_tutorial_hint.md](2026-05-06_stage4_brush_tutorial_hint.md) | Runtime brush tutorial hint for create / release / close affordance | This task | Stage 4 Phase 0 / brush UX | [Unreleased](../../CHANGELOG.md#unreleased) added |
| 2026-05-06 | [2026-05-06_stage4_hero_v2_import.md](2026-05-06_stage4_hero_v2_import.md) | User-approved Hero / Resident_A / Resident_B v2 import | This task | Stage 4 Phase 0 / character art | [Unreleased](../../CHANGELOG.md#unreleased) added / changed |
| 2026-05-06 | [2026-05-06_stage4_resident_a_followup_review.md](2026-05-06_stage4_resident_a_followup_review.md) | Resident_A follow-up review sheet after runtime scale / pixel-granularity feedback | This task | Stage 4 Phase 0 / character art | [Unreleased](../../CHANGELOG.md#unreleased) documentation |
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
- [2026-05-06_stage4_resident_a_followup_review.md](2026-05-06_stage4_resident_a_followup_review.md) — Resident_A follow-up review sheet after runtime scale / pixel-granularity feedback.
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
| [stage4_resident_a_followup_review_sheet_abc.png](screenshots/stage4_resident_a_followup_review_sheet_abc.png) | [2026-05-06_stage4_resident_a_followup_review.md](2026-05-06_stage4_resident_a_followup_review.md) | Resident_A A/B/C follow-up review sheet |
| [stage4_resident_a_candidate_c_size_compare.png](screenshots/stage4_resident_a_candidate_c_size_compare.png) | [2026-05-06_stage4_resident_a_followup_review.md](2026-05-06_stage4_resident_a_followup_review.md) | Resident_A Candidate C fitted-size comparison |
| [stage4_resident_a_followup_review_sheet_c2_c3.png](screenshots/stage4_resident_a_followup_review_sheet_c2_c3.png) | [2026-05-06_stage4_resident_a_followup_review.md](2026-05-06_stage4_resident_a_followup_review.md) | Resident_A C2/C3 smaller-head review sheet |
| [stage4_resident_a_candidate_c2_c3_size_compare.png](screenshots/stage4_resident_a_candidate_c2_c3_size_compare.png) | [2026-05-06_stage4_resident_a_followup_review.md](2026-05-06_stage4_resident_a_followup_review.md) | Resident_A C2/C3 fitted-size comparison |
| [stage4_resident_a_c3_32x48_headfix_variants.png](screenshots/stage4_resident_a_c3_32x48_headfix_variants.png) | [2026-05-06_stage4_resident_a_followup_review.md](2026-05-06_stage4_resident_a_followup_review.md) | Resident_A C3 32x48 head / hair reduction prototype comparison |
| [stage4_resident_a_followup_review_sheet_d_e_hero_ratio.png](screenshots/stage4_resident_a_followup_review_sheet_d_e_hero_ratio.png) | [2026-05-06_stage4_resident_a_followup_review.md](2026-05-06_stage4_resident_a_followup_review.md) | Resident_A D/E stricter Hero-ratio regeneration sheet |
| [stage4_resident_a_hero_ratio_regen_compare.png](screenshots/stage4_resident_a_hero_ratio_regen_compare.png) | [2026-05-06_stage4_resident_a_followup_review.md](2026-05-06_stage4_resident_a_followup_review.md) | Resident_A D/E fitted-size comparison against Hero |
| [stage4_resident_a_fgh_connected_nearest_compare.png](screenshots/stage4_resident_a_fgh_connected_nearest_compare.png) | [2026-05-06_stage4_resident_a_followup_review.md](2026-05-06_stage4_resident_a_followup_review.md) | Resident_A F/G/H connected-candidate nearest-neighbor comparison |
| [stage4_resident_a_f_based_f2_f3_f4_nearest_compare.png](screenshots/stage4_resident_a_f_based_f2_f3_f4_nearest_compare.png) | [2026-05-06_stage4_resident_a_followup_review.md](2026-05-06_stage4_resident_a_followup_review.md) | Resident_A F-based F2/F3/F4 nearest-neighbor comparison |

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
| v1.3 | 2026-05-06 | Resident_A follow-up review devlog and screenshot added; root-level Markdown coverage updated to 55 files. |
| v1.4 | 2026-05-06 | Resident_A Candidate C / C2 / C3 size comparison screenshots indexed. |
| v1.5 | 2026-05-06 | Resident_A C3 32x48 headfix and D/E Hero-ratio regeneration comparison screenshots indexed. |
| v1.6 | 2026-05-07 | Resident_A C3 fix C user review correction recorded: head/body connection and preview blur remain blockers. |
| v1.7 | 2026-05-07 | Resident_A F/G/H connected-candidate nearest-neighbor comparison indexed. |
| v1.8 | 2026-05-07 | Resident_A F-based F2/F3/F4 nearest-neighbor comparison indexed. |
| v1.9 | 2026-05-07 | Resident_A F2 rejection and F/F4 role-framed review state recorded. |
| v2.0 | 2026-05-09 | Chapter 1 Scene 1 v3 final 確定 devlog (8 セクション [1.A]-[1.H]) を追加し、root-level Markdown coverage を 56 件へ更新。 |
| v2.1 | 2026-05-09 | Chapter 1 Scene 2 v1 final 確定 devlog (6 セクション [2.A]-[2.F]、ミア設定確定、視界解放 1+5 統一、sister doc 整合修正) を追加し、root-level Markdown coverage を 57 件へ更新。 |
| v2.2 | 2026-05-09 | Chapter 1 Layer 1 仕様改訂 + シーン 3 v3 設計 devlog (Layer 1 観察ルール撤廃、時の窓モード v3.2 統合、シーン 1 v4、シーン 3 v3 6 セクション、シーン 4 T4 再定義、Niro 家伏線追加) を追加し、root-level Markdown coverage を 58 件へ更新。 |
| v2.3 | 2026-05-09 | Chapter 1 Scene 4 v1 動線骨格 devlog (9 セクション [4.A]-[4.I]、エリュトリア確定、カイア設定確定、仕様 v3.2 例外追記) を追加し、root-level Markdown coverage を 59 件へ更新。 |
| v2.4 | 2026-05-09 | Chapter 1 Scene 5 v1 + 動線再設計 + Z 用語整理 + 白窓 1 周目なし + Niro 家伏線 2 周目修正 + モブ NPC 使いまわし devlog を追加し、root-level Markdown coverage を 60 件へ更新。第 1 章「忘れられた街」全シーン動線骨格完成。 |
| v2.5 | 2026-05-18 | Fast VS rejected outdoor sky/background pass reverted; root-level Markdown coverage remains 107 files. |
