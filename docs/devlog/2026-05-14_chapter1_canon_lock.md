# Chapter 1 Scene 1 Canon Lock for VS Recovery (2026-05-14)

Driver: `docs/chapter1_vs_story_timewindow_recovery_instruction_20260514.md`
Authoritative source: `docs/devlog/2026-05-09_chapter1_scene1_v3_final.md`
Scope: Reto library event [1.B]-[1.G] beats + Niro 内心 + TimeWindow first-use + VS end flag.

これは Phase 1 成果物。Phase 2 以降の Reto event runtime / TimeWindow 全エリア対応 / 演出層 / 検証は本文書のロックに基づいて実装される。

---

## D1. v3 final と v4 (本が出る/出ない) の差分 — canon lock

**採用**: v3 final（[1.F] で本は出現しない）

根拠:
- `2026-05-09_chapter1_scene1_v3_final.md` §[1.F] と §設計上の含意で明示
- STORY_BIBLE §3.2 Layer 1 = 観察のみ ルール厳守
- T4 (シーン 4) を能動行動・因果連鎖の初体験として保つ構造的役割
- L-γ クライマックスは [1.C] に集約済み、[1.F] は Reto 弱化に整合

棄却: v4 系統 (本が出る + Reto 強反応)

---

## D2. ビート → dialogue asset / localization key 写像

| Beat | 主体 | dialogue asset | localization key prefix | 既存行 | v3 final 行数 | 変更要否 |
|------|------|---------------|-------------------------|--------|-------------|---------|
| [1.B] レト初対面 | Reto | `Resident_B_Scene1_B_Initial.asset` | `dialogue.scene1.reto.b_initial.line_{1..3}` | 3 | 3 | 変更なし |
| [1.C] 図書館の歴史 + 誘い | Reto | `Resident_B_Scene1_C_LibraryHistory.asset` | `dialogue.scene1.reto.c_library_history.line_{1..6}` | 6 | 6 | 変更なし |
| [1.D] 時の筆発動 | Reto | `Resident_B_Scene1_D_BrushReaction.asset` | `dialogue.scene1.reto.d_brush_reaction.line_1` | 1 (`...?`) | 1 (`...?`) | 変更なし |
| [1.E] 過去図書館 | (Reto なし) | — | — | — | — | — |
| [1.F] 現在に戻る | Reto | `Resident_B_Scene1_F_BookAppears.asset` | `dialogue.scene1.reto.f_book_appears.line_{1..4}` | 4 | 2 | **要修正** |
| [1.G] ミア家ヒント | Reto | `Resident_B_Scene1_G_MiaHint.asset` | `dialogue.scene1.reto.g_mia_hint.line_{1..3}` | 3 | 3 | 変更なし |

### D2.1 [1.F] localization 修正詳細

ja-JP テーブル現状値 → v3 final canon:

| key | m_Id | 現状 ja-JP | v3 final | アクション |
|-----|------|-----------|----------|-----------|
| `f_book_appears.line_1` | 1538252836 | `...?` | `...どうかしましたか?` | 値を上書き |
| `f_book_appears.line_2` | 1538252837 | `...本物だ` | (削除) | turn から外す。key/m_Id 自体は残す（参照途絶を避けるため一旦温存） |
| `f_book_appears.line_3` | 1538252838 | `...そうですか` | `...そうですか` | 変更なし |
| `f_book_appears.line_4` | 1538252839 | `...あなたのような方が、来てくれるとは` | (削除) | turn から外す。key/m_Id 残す |

asset の `turns` リストは `line_1` と `line_3` の 2 件のみに削減。

ファイル名・variantId・key prefix は `f_book_appears` のまま温存（既存 wiring の整合維持。命名が古いことは canon lock 文書として残す）。

### D2.2 [1.G] localization 確認

ja-JP テーブル現状値:

| key | m_Id | 現状 ja-JP |
|-----|------|-----------|
| `g_mia_hint.line_1` | 1538252840 | `...そういえば` |
| `g_mia_hint.line_2` | 1538252841 | `中央集落のミアさんが、今朝、困っていました` |
| `g_mia_hint.line_3` | 1538252842 | `もし手があるなら、少し、助けてやってください` |

v3 final 3 行と完全一致。**変更不要**。
（2026-05-12 canon inventory での「line_3 が旧表現」報告は当時のスナップショット由来。現状は既に canon。）

### D2.3 EN 側

`Anemora_Strings_en.asset` の同 m_Id 値も Phase 2 で v3 final EN 訳に合わせる。EN 詞は別途確定するため、Phase 1 では「修正 key 一覧」のみロック:

- `f_book_appears.line_1` → "...Is something the matter?"
- `f_book_appears.line_3` → (既存維持 / 流用 — Phase 2 で確認)
- 他 5 key (B/C/D/G) は EN 既存値の整合確認のみで content lock 不要

---

## D3. Niro 内心 (inner monologue) ロック

Reto dialogue asset とは独立して、`Chapter1PlayableFlowController.ShowStoryFeedback` または `NiroMonologueController` 経由で表示。**Reto の Resident_B asset には混ぜない**。

| Beat | 位置（前後 anchor） | 内心テキスト | 表示モード |
|------|--------------------|------------|-----------|
| [1.C] | line_3 終了後 → line_4 開始前 | `(...誰も)` | story_feedback bottom prompt, 短時間 (auto dismiss) |
| [1.C] | line_5 終了後 → line_6 開始前 | `(...からっぽ)` | story_feedback (視線が空棚へ移る瞬間) |
| [1.D] | brush UI 出現直前 | `(...筆)` | story_feedback |
| [1.D] | 赤光発光後 | `(...?)` | story_feedback |
| [1.E] | 過去窓内、最初の数秒 | `(...ここに、本が)` | story_feedback |
| [1.E] | 過去窓内、過去図書館員シルエット視認時 | `(...あの子)` | story_feedback |
| [1.F] | 窓を閉じた直後、レト発話前 | `(...あった)` | story_feedback |
| [1.H] | 図書館跡を出て煙視認時 | `(...煙)` | (Phase 1 範囲外、参考) |

新規 localization key 必要。Phase 2 で `dialogue.niro.scene1.thought.{beat}.{slot}` の体系で追加:

- `dialogue.niro.scene1.thought.c_history.no_one`
- `dialogue.niro.scene1.thought.c_history.empty`
- `dialogue.niro.scene1.thought.d_brush.brush`
- `dialogue.niro.scene1.thought.d_brush.question`
- `dialogue.niro.scene1.thought.e_past.books_here`
- `dialogue.niro.scene1.thought.e_past.the_child`
- `dialogue.niro.scene1.thought.f_return.was_there`

---

## D4. ビート毎の完了条件と進行フラグ

新規 progression flag (Phase 2 で `Chapter1PlayableFlowController` に定数追加):

| Beat | 完了条件 | 設定する flag |
|------|---------|--------------|
| [1.B] | Reto との dialogue B 全 turn を読了 | `progression.chapter1.vs.reto.beat_b_done` |
| [1.C] | dialogue C 全 turn 読了 + 「誘い」line_6 表示完了 | `progression.chapter1.vs.reto.beat_c_done` |
| [1.D] | Library 範囲内で TimeWindow 初回オープン（drag complete + portal Open） | `progression.chapter1.vs.reto.beat_d_done` |
| [1.E] | 過去側に滞在 N 秒 (推奨 3.0s) または過去側 landmark 1 体以上に近接 | `progression.chapter1.vs.reto.beat_e_done` |
| [1.F] | TimeWindow を閉じる + dialogue F 全 turn (2 turn) 読了 | `progression.chapter1.vs.reto.beat_f_done` |
| [1.G] | dialogue G 全 turn 読了 | `progression.chapter1.vs.reto.beat_g_done` |

既存 flag との関係:

- `VsLibraryEndpointInspectedFlag` (`progression.chapter1.vs.library_endpoint_inspected`) は **`beat_b_done` と同義** に再定義（Reto と初対面 = 図書館エンドポイント inspected）。後方互換のため両 flag を同時に set する。
- `VsLibraryEventCompleteFlag` (`progression.chapter1.vs.library_event_complete`) は **`beat_g_done` 達成時のみ** set。中間 beat では set しない。

### D4.1 ビート遷移ガード

beat_X_done が未 set のとき、その先の beat は発火しない。例:

- [1.D] の TimeWindow drag UI は `beat_c_done` が set されてからでないと開かない（Library 範囲内かつ Niro 動作後にのみ）
- [1.G] dialogue は `beat_f_done` set 後に Reto が自発的に切り出す（player input 不要 = オート）
- [1.F] の Reto dialogue は `beat_e_done` set + 「TimeWindow を閉じた」イベントを両方満たしてから開始

---

## D5. TimeWindow 全エリア対応スコープ

`docs/chapter1_vs_story_timewindow_recovery_instruction_20260514.md` 仕様 B に従い:

| エリア | 既存 controller | Phase 3 アクション |
|--------|---------------|------------------|
| HouseInterior (Mia 家) | なし | 新規追加 (controller + paired space root) |
| HouseExterior | あり | 維持（既存 V24 視覚差分を保つ） |
| CentralPlaza | なし | 新規追加。**ポータル/門/輪 と読まれない控えめ視覚** |
| Library | あり | 維持 + 初回使用を [1.D] に強制ゲート |

「全エリア開ける」= 任意箇所で drag 可能。「特定箇所で深く感じる」= Library [1.D] / HouseExterior (既設 V24) は landmark / 過去視覚差分が濃い、HouseInterior / CentralPlaza は landmark 数控えめ + 過去側はトーン差のみで深さは浅め。

---

## D6. VS end (Chapter1 VS milestone) ロック

`VsLibraryEventCompleteFlag` を set するタイミング = **[1.G] dialogue G の last turn が読了した瞬間のみ**。

Phase 2 で `ApplyRetoDialogueSequence` を以下に置き換える:

- 既存: milestone の `completedDialogueAssets` に B/C/G を投げ込み、interact で順次再生
- 新方式: `Chapter1RetoLibraryEventController` (Phase 2 新設) が [1.B]-[1.G] state machine を回し、各 beat の dialogue / 内心 / TimeWindow 操作 / flag 設定を管理。最後の [1.G] 完了時のみ `VsLibraryEventCompleteFlag` を set。

中間 beat 完了で「VS end 達成」と誤判定する経路は廃止。

---

## D7. アセット改修一覧（Phase 2 着手分）

1. **`Anemora_Strings_ja-JP.asset`**: m_Id 1538252836 の m_Localized を `...?` → `...どうかしましたか？` に上書き
2. **`Resident_B_Scene1_F_BookAppears.asset`**: turns を 4→2 件に削減 (line_1 と line_3 のみ残す)
3. **`Anemora_Strings_ja-JP.asset` + Shared Data**: Niro 内心 7 key (D3 参照) を追加
4. **`Anemora_Strings_en.asset`**: 上記同 m_Id 群の EN 訳を追加・修正 (Phase 2 後半 or 別 task)
5. **`Chapter1PlayableFlowController.cs`**: 新規 7 flag 定数 (`beat_b_done`...`beat_g_done`) 追加
6. **新規 `Chapter1RetoLibraryEventController.cs`**: state machine 本体
7. **`AnemoraChapter1RouteGraphicsPackageIntegrator.cs`** の `ApplyRetoDialogueSequence`: Reto milestone wiring を新 controller 起動に置換

---

## D8. Library TimeWindow 必須使用設計

[1.D] でプレイヤーが TimeWindow を初回使用するまで、Reto の以降 dialogue が進まない。

具体的:

- `beat_c_done` set 後 → Reto 発話停止 (waiting for player action)
- player が brush UI を起動 + Library 内で drag 完了 + portal Open → `beat_d_done` set + 即 [1.E] 過去観察フェーズ
- TimeWindow 未起動のまま 一定時間 (推奨 30s) 経過 → ヒント「`Shift+ドラッグで時の窓を開く`」を `TimeWindowLibraryFirstUseHint` で再表示

Library 範囲外で player が brush を発火しても [1.D] は完了しない。Library 範囲判定は既存の `IsPlayerInsideOpenTimeWindow` ロジックを参考に `TimeWindowPairedSpacePortalController` (Library) の `window.ContainsWorldPosition` を流用。

---

## D9. 凍結境界

- `docs/chapter1_vs_story_timewindow_recovery_instruction_20260514.md` Phase 3 により **runtime 側 TimeWindow 本体への変更は許容** (旧 freeze 方針はこの回収作業では解除)
- ただし `TimeWindowPairedSpacePortalController` の座標契約 (`CoordinateBasisEquivalentForReview` 系) は壊さない
- V24 視覚差分 (`PastVisualWallTint` 等) は HouseExterior と Library で維持。HouseInterior / CentralPlaza は新規導入分のみ控えめ色で適用

---

## D10. 自走判断（Tom 介入待ちなし）

`docs/chapter1_vs_story_timewindow_recovery_instruction_20260514.md` §3 unresolved 3 件への自走決定:

1. **[1.F] = v3 final（本なし）** — §D1 で確定
2. **TimeWindow 全エリア open + key 地点で深く** — §D5 で確定
3. **VS end = [1.G] 完了後のみ** — §D6 で確定

commit / push / PR は引き続き Tom 明示指示待ち（自走 memory 準拠）。

---

## 完了

Phase 1 (Chapter1 canon lock) 終了。続いて Phase 2 (Reto event runtime 実装) と Phase 3 (TimeWindow 全エリア対応) を並走で進めることが可能。
