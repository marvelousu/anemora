# Chapter 1 VS Story / TimeWindow Recovery — Baseline (Phase 0)

Date: 2026-05-14
Driver: `docs/chapter1_vs_story_timewindow_recovery_instruction_20260514.md`
Branch: `codex/stage4-chapter1-implementation-20260510`
Repo: `Anemora-stage4-chapter1-impl`

## Recent commits (top 10)

```
d8f19dd Realign chapter 1 graphic handover with Windows session reality
8b3da1d Add chapter 1 graphic session handover
43ebc43 Lay out Scene 5 and redo the chapter-1 path layout
1d1ae4c Lay out Scene 4 v1 with auto-trigger window for the T4 reveal
57080ef Lift Layer 1 observation-only rule and design Scene 3 around it
eaa9aeb Finalize Chapter 1 Scene 2 dialogue and align sister map doc
12229a6 Finalize Chapter 1 Scene 1 v3 to 8-section structure
160e4d4 Add Zenn draft on AI agent orchestration patterns
5641304 Add Chapter 1 design corpus, map handover, and session recovery doc
8761a22 Add Chapter 1 production handover (broader scope)
```

## Source-of-truth canon docs (read-only inputs)

- `docs/devlog/2026-05-09_chapter1_scene1_v3_final.md` — 8セクション最終版（[1.F]=本は出ない）
- `docs/devlog/2026-05-12_chapter1_vs_story_canon_inventory.md` — 既存 asset と canon の不整合棚卸
- `docs/chapter1_vs_story_timewindow_recovery_instruction_20260514.md` — 本回収作業の元指示
- `docs/devlog/2026-05-11_chapter1_vs_story_dialogue_canon_audit.md`
- `docs/devlog/2026-05-11_chapter1_playable_first_route_pass.md`

## 現状の実装位置（要再構築箇所）

- `Assets/Scripts/Chapter/Chapter1PlayableFlowController.cs` — 進行フラグ・objective 切替・feedback 表示。VsLibraryEventCompleteFlag 既存。
- `Assets/Scripts/Chapter/Chapter1MilestoneInteractable.cs` — interact + `requirePlayerInsideTimeWindow` + `TryShowCompletedDialogueAsset` 既存。
- `Assets/Scripts/Chapter/Chapter1UiText.cs` — `LibraryEndpointCompleted = "(...からっぽ)"` / `RetoLibraryCompleted = "もし手があるなら..."` 既に v3 final 整合。
- `Assets/Editor/AnemoraChapter1RouteGraphicsPackageIntegrator.cs:1645-1667` — `ApplyRetoDialogueSequence` が B / C / G のみ wire（D / F 未配線。E は仕様上 asset 不要）。
- `Assets/ScriptableObjects/Dialogues/Resident_B_Scene1_F_BookAppears.asset` — v3 final と内容不整合（本が出る前提の 4 turns）。
- `Assets/ScriptableObjects/Dialogues/Resident_B_Scene1_G_MiaHint.asset` — line_3 の ja-JP が `あなたなら、力になれるかもしれません`（v3 final は `もし手があるなら、少し、助けてやってください`）。
- `TimeWindowPairedSpacePortalController` — HouseExterior と Library の 2 個。HouseInterior / CentralPlaza 未整備。

## 既知の dirty files（事前作業由来。今回スコープ外で除外しない）

- Localization tables (`Anemora_Strings*.asset`) — 過去の翻訳作業
- TimeWindow / Portal / Player / Dialogue 関連 runtime — 既設定
- 多数の untracked `Assets/Editor/AnemoraChapter1*Builder.cs` / Assets/Art / Models — V24 ビルド成果物

これらは本回収作業の差分計算で除外し、純粋に本作業で増分した箇所のみを Phase 5 で集計する。

## 既存スクショ

`docs/devlog/screenshots/chapter1_house_proof/` 配下は空（Phase 5 で beat 別新規取得）。

## ベースラインを記録した上で次へ

Phase 0 終了。Phase 1（canon lock 文書化）に進む。
