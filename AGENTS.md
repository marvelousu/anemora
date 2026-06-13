# Anemora プロジェクト指示 (project-scoped)

> Claude Code および Codex 双方が読む project-level 規律。ファイル名は `AGENTS.md` で統一。
> 本ファイルは Anemora 固有の **critical gotcha + pointer のみ**。コード/git/devlog を読めば分かることは書かない。肥大化させない (目安 50 行)。

## まず読む
- セッション開始は `docs/STATUS.md` → 本ファイル → `docs/canon/` (確定 canon の現在地) → `docs/MAP.md` (Antela 配置+方位) → `docs/devlog/INDEX.md` 最新日付。
- git archaeology や recovery file 漁りの **前** に STATUS を読む。
- 方位・マップ配置を扱う作業は MAP.md が正。古い devlog のマップ文言は drift していることが多い。

## canon と devlog の運用 (immutable rule、厳守)
- **canon の "living state" = `docs/canon/` のみ**。物語に関する確定情報の更新はここに反映。他セッション引き渡しはここを参照させる。
- **devlog (`docs/devlog/`) は immutable**: 過去 entry を編集しない。訂正・更新は **新 devlog ファイルを別日付で作成**、旧 entry は触らない。
- devlog の意味 = 「その日に何を決めたか」の歴史。歴史を書き換えない。改訂履歴表を本文内に作って正当化しない。
- canon と古い devlog が食い違う場合 = **canon が正**、devlog は当時の記録として保持。
- 詳細: `docs/canon/README.md`

## 役割分担 (Codex メイン試行中)
- **物語 / canon / 演出 / handover = Linux Claude**。実装 / Unity build / validate / screenshot = **Codex Windows**。
- Linux 環境では Unity build 不可。実装検証を要する作業は Codex 引継ぎプロンプトで渡す。

## レビュー画像ワークフロー (R2 移行済 2026-05-30、Codex 主担当)
- レビュー画像は従来どおり `docs/review/<YYYY-MM-DDTHH-MM>/` (JST、ISO 8601、`:`→`-`) に **ローカル生成**。1 サイクル = 1 ディレクトリ、`devlog.txt` 必須 (最初の非空・非コメント行 = 対応 devlog .md のリポ相対パス)。
- ただし **git にはコミットしない**。`bloat-guard` (pre-commit/pre-push + CI) が `docs/review/`・`docs/devlog/screenshots/` を拒否する。代わりに R2 へ:
  `tools/r2/r2-upload-review.ps1 -CycleDir docs/review/<ts> -Branch work/<branch>` (画像は `git add` しない)。
- `work/*` を origin へ push すると Action `r2-mirror-review` が R2 を同期。viewer は R2 から取得して Review タブに表示。詳細 `tools/r2/README.md`。
- 生成シーン `Assets/Scenes/Anemora_FastVS_HouseSlice.unity` と APV ベイク `Assets/Settings/*.Cell*.bytes` は gitignore (再生成/再ベイク)。**Git LFS 禁止** (viewer の git archive を壊す)。
- 新 clone/worktree は `tools/githooks/install.ps1` (= `git config core.hooksPath tools/githooks`) でガード有効化。詳細 `../anemora-repo-hygiene-cicd-plan.md`。

## branch / 公開規律
- `main` = **public 安定 VS baseline**。コード変更は continuation branch 経由が原則、`main` 直編集はしない。
- devlog/doc-only commit は `main` で可 (established practice)。物語 doc commit は user 確認推奨。
- `work/post-vs-public-20260518` は main と分岐 + DotGothic16 atlas 欠落。branch 整合判断は Codex 領域。

## stale context 事故防止
- `_recovery_*.md` / compact summary は **当時の状態**。現在ではない。STATUS の最終更新日と今日を必ず照合。
- devlog 履歴は 2026-05-07〜05-18 が sweep→再構築済。`docs/devlog/INDEX.md` を一次ナビに。

## 並列セッション規律
- pathspec stage 厳守 (`git add -A` 禁止)。他セッション由来の untracked/dirty は触らない。
- push 前に `git fetch` + `HEAD..origin/main` 確認、必要なら rebase。

## ネタバレ語彙禁止 (外部見えメタデータ)
- commit message / PR / branch 名 / handover ファイル名 / 公開 doc にネタバレ語彙を出さない。
- player-facing で「層」「ベール剥離」用語禁止 (設計用便宜語)。固有名詞 (Antela 等) も UI/dialogue で前面化しない。

## 調査は委譲
- frontier 特定 / 横断調査は Explore subagent に委譲しメイン context を守る (global 方針、本プロジェクトで特に重要)。

## 確定 canon の優先
- `docs/canon/` 配下が物語の正 (living state)。実装ビルドの provisional 台詞より canon doc を優先。
- `docs/STORY_BIBLE_v1.md` は legacy (全章骨格)、`docs/canon/` 未整備の章は参照可だが矛盾時は `docs/canon/` 優先。

## 運用ガード (2026-06-13 環境監査で導入)
- **承認アセットは即保全**: レビューで承認が出たアセットはその場で git 管理パスへ移して commit+push してから次へ (v57/v58 紛失の再発防止)。worktree/レビュー置き場に承認版を残置しない。
- **微調整の止め時**: 同一対象への微調整サイクルが10連続で閾値未満の差分しか生まないなら停止して方針を見直す (cycle-start skill の Project gates にも明記)。
- **視覚レビューは3段**: shotdiff (機械、変化なし→自動パス) → Claude 一次レビュー (変化分のみ所見付き) → 人間最終。全数目視をしない。
- **STATUS 鮮度**: pre-push hook が docs/STATUS.md の7日超漂流を fail させる (緊急時のみ ANEMORA_SKIP_STATUS_CHECK=1)。
- **バックアップ**: ローカル定期タスク AnemoraNightlyBackup が毎晩 bundle+dirty.patch+untracked を退避する (コミットの代替ではない)。
