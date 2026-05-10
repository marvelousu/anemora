# VS Playable Failure — Orchestration Postmortem

Date: 2026-05-05

## 1. 発見

user が以下の Windows Standalone build を起動した結果、VS が actually playable な体になっていないことが発覚:

`<worktree:Anemora-g5-audio-build>\Builds\G5Audio\Anemora_G5_Audio.exe` (commit `e6e3c61`、A4 audio 入り)

症状:

- 箱が 2 つ浮いているだけ
- 音なし
- グラフィックなし (Hero / NPC / buildings / palette が見えない)
- 操作不可

= VS が catastrophic failure。orchestrator (Linux Claude) が「Stage 3 = 技術的完了」「Stage 4 Phase 0 進入」を宣言した直後、user が試作起動して発見。

## 2. Orchestration 過誤の整理

orchestrator (Linux Claude) が「VS 完成」と誤認した根拠:

| 根拠 | 実態 |
| --- | --- |
| PlayMode 27/27 pass | unit-level state assertion のみ、visual / audio / input pipeline 未検証 |
| EditMode 31/31 pass | compile + 単体 assertion |
| Windows Standalone build success | compile + asset bundle 生成のみ、actual render / play 未確認 |
| A3 G5 automated `c17d62f` "Go" 判定 | automated checklist 通過、実プレイは not in scope |
| Player ready 7.934s | 起動時間のみ、起動後の playable 状態未検証 |
| 30s / 120s runtime sample (working set / GPU peak) | idle 計測のみ、画面 / 音 / 操作の actual verification なし |
| `Anemora.EditorTools.Zone1AudioSceneSetup.VerifyMainScene` pass | editor tool で配線存在の確認、runtime render / audio playback とは別 |
| BuildReport で audio asset 含有確認 | build に詰め込まれている事実のみ、scene 起動時に load / play されるかは別 |

要するに、automated portion (compile + test assertion + 数値計測) を「VS 完成判定」と読み替えていた。 **actual render / audio / input / gameplay の user 起動確認** という最も重要な verification step がスコープから抜けていた。

A3 G5 automated run の handover でも `§H Audio / §I UI / §L 通し体験 / §M 層 2 片鱗` は「user 検証用として未実施」と明記されていたにも関わらず、orchestrator はこれを「user が時間あるとき実施すれば良い」と判断、Stage 4 dispatch に進んでしまった。

## 3. 撤回する Claims

本 incident に伴い以下の orchestrator 判断 / 宣言 を撤回:

| Claim | 撤回理由 |
| --- | --- |
| 「Stage 3 = 技術的完了」 | actual playability 未検証 |
| 「Stage 3 完成判定の前提揃った」 | 同上 |
| 「Stage 4 Phase 0 進入」 | Stage 3 が未完成 |
| Stage 4 dispatch (A1 asmdef 2 層化 / A2 SFX normalize / A3 URP migration / A4 compat SFX 削除 / A5 F2 v2 redraw) | Stage 4 進入前提が未成立 |
| 「user manual G5 は user 時間あるとき実施で OK」 | manual G5 = VS 完成判定の核心、postpone 不可 |
| 「saturation 限界に達したので追加 dispatch なし」 | そもそも VS が動いていなかったので saturation 議論以前 |

## 4. 直近対応

1. Stage 4 dispatch 5 件 完全撤回 (実行されないこと user 確認済)
2. 他 task 全 freeze (修復まで)
3. A3 セッションへ diagnostic dispatch (調査のみ、実装修復は別 task)
   - スコープ: BuildReport / Player.log 解析 / Anemora_Main scene hierarchy 確認 / Editor Play vs Build 比較 / 「箱 2 つ」の正体特定 / 原因分類 / 修復方針提示
   - 出力: `docs/devlog/2026-05-05_vs_playable_failure_diagnostic.md` (調査結果)
4. orchestrator (Linux Claude) は本 devlog で orchestration 過誤を整理

## 5. 学び

| 学び | 内容 |
| --- | --- |
| test pass ≠ playable | PlayMode / EditMode test は state 変化 assertion、visual / audio / input / gameplay は別 |
| build success ≠ playable | compile + bundle 生成のみ、起動後 actually renders / plays の verification ではない |
| automated "Go" ≠ playable | A3 G5 automated 判定は automated checklist の意味、実プレイ観点は含まない |
| runtime sample ≠ playable | 30s / 120s working set / GPU peak は idle 計測、 scene 中身が壊れていても数値は出る |
| editor tool verifier ≠ runtime | `VerifyMainScene` は配線存在のみ、scene 起動後の動作は別 |
| user 主観 review = VS 完成判定の核心 | manual G5 §H / §I / §L / §M を user が実施しないと VS 完成判定不可、postpone 禁止 |
| orchestrator 自走禁止 | Stage 完成 / 次 Stage 進入は user 明示承認後 |

## 6. Memory 更新

orchestrator memory に以下を保存 (2026-05-05):

- `feedback_anemora_user_review_required.md` — user 体験確認なしで Stage 完了宣言しない
- `feedback_anemora_test_pass_vs_playable.md` — test pass / build success ≠ playable

両 entry とも `MEMORY.md` index に追加済。

## 7. 次の流れ

1. A3 diagnostic 完了待ち (`2026-05-05_vs_playable_failure_diagnostic.md`)
2. A3 が 「箱 2 つ」の正体 + 原因分類 (A: sprite ref 切れ / B: camera-lighting / C: URP culling / D: build inclusion / E: audio + spawn 連鎖 / F: その他) を特定
3. 修復方針に基づき修復 task dispatch (実装、light / medium / heavy 規模に応じて 1-2 セッション)
4. 修復 commit + 新 build 再生成
5. user 再起動 verify (Editor Play / Build 両方で playable 確認)
6. user による §H / §I / §L / §M 評価 → Stage 3 完成判定
7. user 明示承認後に Stage 4 進入相談

これより前に「Stage 3 完成」「Stage 4 進入」を orchestrator が宣言しない (`feedback_anemora_user_review_required.md` 遵守)。

## 8. 影響を受けた既存 doc / handover (参考)

本 incident で「Stage 3 = 完成」と読める記述がある doc / handover (修復後に必要なら更新):

- `docs/STAGE3_RETROSPECTIVE.md` v0.1 (`34585e3`) — preliminary draft、user manual G5 結果反映後 v1.0 で修復
- `docs/STAGE4_ROADMAP.md` v0.1 (`a49ee52`) — preliminary draft、Stage 3 完成判定後に再評価
- `CHANGELOG.md` `[Unreleased]` Stage 3 Day 1 entry (`bd5abc7`) — preliminary release notes、内容自体は変更不要 (commit 履歴は事実)
- `docs/G5_ACCEPTANCE_MATRIX.md` (`c17d62f` + `e6e3c61` + `df19870`) — automated 部記入済、manual 部は user 評価で記入予定
- `~/notes/_handover/anemora-windows-handover-2026-05-05-*.md` 系 — 各 handover は task 単位の事実記録、Stage 3 完成宣言は含まないため修正不要

これらは文書として「事実 (commit / 数値 / 記述)」を残しているので削除不要。「Stage 3 完成宣言」は orchestrator が口頭で行ったもので、文書には含まれない。
