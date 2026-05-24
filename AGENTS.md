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

## screenshot / review ワークフロー (Codex 主担当)
- レビュー対象画像は `docs/review/<YYYY-MM-DDTHH-MM>/` (JST、ISO 8601 + URL safe で `:` を `-`) に置く。1 セッション = 1 ディレクトリ。画像枚数の上限なし。
- 同ディレクトリに **`devlog.txt` 必須**。最初の非空・非コメント行 (`#` で始まらない行) に、そのサイクルの対応 devlog markdown のリポ相対パスを 1 行で書く。
- `docs/devlog/screenshots/` は引き続き Codex の作業ログ用 (生ログ・revision 含む)。`docs/review/` はレビュー出し用にキュレーションした画像群。役割分離で並存。
- PR には `.github/workflows/review-check.yml` が走り、ディレクトリ名・`devlog.txt` 存在・参照先 .md の実在・画像 1 枚以上を validate。違反は CI fail。
- viewer (`https://anemora-viewer.pages.dev/`) の Review タブが `docs/review/*` を自動表示。
- 詳細: `docs/review/README.md`

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
