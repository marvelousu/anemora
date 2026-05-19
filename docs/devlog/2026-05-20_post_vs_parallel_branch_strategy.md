# 2026-05-20 Post-VS Parallel Branch Strategy

## Summary

After the public Fast VS baseline, development is split into two focused sessions:

- VS-range HD-2D visual polish.
- Chapter 1 continuation beyond the current VS clear point.

`main` remains the canonical public VS snapshot and must not be edited from either session. `work/post-vs-public-20260518` is the integration branch. Actual work should happen on short-lived work branches and separate Unity worktrees, then be merged back into `work/post-vs-public-20260518` after validation.

## Branch Policy

- Public baseline:
  - Branch: `main`
  - Current public commit: `e9d61c290edfccef75ead8a0fa9942436fdbf3ef`
  - Rule: do not commit to or push from active development sessions.
- Integration branch:
  - Branch: `work/post-vs-public-20260518`
  - Implementation baseline before this planning note: `26a93b0a9409e2c87ea709887e033193936becd4`
  - If this strategy note is already committed, branch HEAD may be one docs-only commit ahead of that baseline.
  - Rule: merge validated feature branches here; avoid long-running direct edits.
- HD-2D branch:
  - Proposed branch: `work/fast-vs-hd2d-polish-20260520`
  - Proposed worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
  - Scope: existing Fast VS maps and presentation only.
- Chapter 1 continuation branch:
  - Proposed branch: `work/chapter1-continuation-20260520`
  - Proposed worktree: `C:\Users\maro6\Documents\Unity\Anemora-chapter1-continuation-work`
  - Scope: story, map, and event implementation after the current Reto VS clear point.

## Work Boundary

HD-2D visual polish owns:

- Existing VS map presentation for Niro house interior, house exterior, plaza, library exterior, and library interior.
- Lighting, shadows, material feel, texture replacements, set dressing, camera-safe visual depth, and review screenshots.
- No story progression, event trigger, save/flag, Time Window behavior, or route-contract changes unless they are direct visual validation fixes.

Chapter 1 continuation owns:

- Events and gameplay after the current Reto clear point.
- New story beats, dialogue, route gating, progress flags, next maps, and future validation.
- No broad restyling of the existing VS graphics. If a placeholder is needed, keep it obviously temporary and document it.

Shared files, especially `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`, are high-conflict. Each session must keep commits small and record which section it touched in devlog.

## Validation Before Merge

Each branch must pass, at minimum:

- Unity batch validation:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath <worktree> -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Windows player build:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath <worktree> -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildHouseSlicePlayer`
- Player smoke:
  - Launch the generated `Anemora_FastVS_HouseSlice.exe` briefly.
  - Check `C:\Users\maro6\AppData\LocalLow\DefaultCompany\Anemora\Player.log` for runtime errors.

HD-2D work should also produce review screenshots. Chapter 1 continuation work should add story-flow validation for any new progress states.

## Other Session Prompt

Target session ID provided by user:

- `019e411a-0d8d-7082-9d15-1c2bab8af942`

Prompt to send:

```text
Anemora の post-VS 開発を 2 セッションに分けます。あなたの担当は「Chapter 1 continuation」、つまり現在の Fast VS のレトイベント終了後から先の 1章続きを実装するセッションです。

重要な前提:
- `main` は VS 公開版の正です。絶対に commit / push / reset しないでください。
- 統合元は `work/post-vs-public-20260518` です。実装ベースラインは `26a93b0a9409e2c87ea709887e033193936becd4` です。作業開始時は `git rev-parse work/post-vs-public-20260518` の結果を正とし、この方針メモの docs-only commit が上に載っている場合はそれを含めて branch を切って構いません。
- 作業 branch は `work/chapter1-continuation-20260520`、作業 worktree は `C:\Users\maro6\Documents\Unity\Anemora-chapter1-continuation-work` を使ってください。
- 既存 VS 範囲の HD-2D 見た目改善は別セッションの担当です。あなたは既存 VS グラフィックの大規模変更をしないでください。必要な仮配置は最小限に留め、devlog に明記してください。
- ユーザー指定の開発サイクルを守ってください: まず詳細プランを立てる → 必要に応じて gpt-5.4-mini worker に細かい指示を出す → worker 報告をレビュー → 実装/検証 → devlog 作成。
- 報告ではすべての実ファイルパスをフルパスで示してください。

最初にやること:
1. `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample-hd2d-work` で `git status --short --branch` と `git rev-parse work/post-vs-public-20260518` を確認してください。
2. `work/post-vs-public-20260518` から `work/chapter1-continuation-20260520` を切り、`C:\Users\maro6\Documents\Unity\Anemora-chapter1-continuation-work` の worktree を作成してください。すでに存在する場合は状態を確認して、勝手に削除や reset はしないでください。
3. 以下の方針記録を読んでから進めてください:
   - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample-hd2d-work\docs\devlog\2026-05-20_post_vs_parallel_branch_strategy.md`
   - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample-hd2d-work\docs\devlog\2026-05-19_fast_vs_guidance_mainbase_validation.md`
   - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample-hd2d-work\docs\devlog\2026-05-19_fast_vs_hd2d_local_shape_pass.md`
   - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample-hd2d-work\docs\HD2D_IMPLEMENTATION_PROPOSAL.md` は参考だけ。Chapter 1 continuation 側では HD-2D 実装を主目的にしないでください。
4. story source / canon を再確認してください。少なくとも以下を探して読み、最新版を判断してください:
   - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample-hd2d-work\docs\STORY_BIBLE_v1.md`
   - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample-hd2d-work\docs\VS_SCOPE.md`
   - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample-hd2d-work\docs\devlog\2026-05-18_fast_vs_story_canon_v4_source_check.md`
   - 必要なら `C:\Users\maro6\Documents\Unity\Anemora-stage4-chapter1-impl\docs\devlog\2026-05-09_chapter1_scene1_v3_final.md`
   - 必要なら `C:\Users\maro6\Documents\Unity\Anemora-stage4-chapter1-impl\docs\devlog\2026-05-12_chapter1_vs_story_canon_inventory.md`
   - 必要なら `C:\Users\maro6\Documents\Unity\Anemora-stage4-chapter1-impl\docs\chapter1_vs_story_timewindow_recovery_instruction_20260514.md`

実装方針:
- 現在の VS clear は「レトの話を聞いた。」までです。その後から先の 1章 continuation を設計してください。
- 既存の Fast VS route / Time Window V24 same-coordinate behavior / レトイベント / HUD guidance regression fixes は壊さないでください。
- まずは大規模実装に入らず、Chapter 1 continuation の最小縦切りを設計してください。具体的には、次の目的地、必要な会話、進行フラグ、マップ遷移、仮アセット、検証項目を決めるところから始めてください。
- 実装時は既存生成コードの構造に従い、勝手な新アーキテクチャを作らないでください。ただし、既存 VS 用の巨大生成ファイルにさらに詰め込むと衝突が大きい場合は、分割案をプランとして先に出してください。
- devlog は必ず `C:\Users\maro6\Documents\Unity\Anemora-chapter1-continuation-work\docs\devlog\` に追加し、INDEX も更新してください。

検証方針:
- 実装前に validation の追加方針を決めてください。
- 実装後は最低限、Unity batch validation と Windows build を通してください。
- 新しい進行状態を追加した場合は、既存 VS clear までの validation を壊していないことに加え、新規 continuation の到達条件も検証してください。

最初の返答:
- いきなり実装せず、まず現在状態、参照した canon、作業 branch/worktree、Chapter 1 continuation の最小実装案、worker に割り振る場合の指示案をまとめてください。
- 実装判断が必要な箇所は、ユーザーに確認する前に repo/docs から判定できるか探索してください。
```
