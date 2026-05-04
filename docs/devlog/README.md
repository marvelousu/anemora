# Devlog 運用ルール

開発フローを **逐一** 記録する場。Zenn 制作技術記事の下書き兼用。

## 記録対象

- セッション境界 (どの Claude/Codex/別ツールで実施したか)
- ユーザープロンプト (主要な発話を抜粋、ファイル名/結論レベルでなく実際の文字列)
- 主要な意思決定点 (採用案・不採用理由・ターニングポイント)
- レビュー結果 (要点 + レポート保存先パス)
- 残タスク・引き継ぎ事項

## 命名規則

`YYYY-MM-DD_<topic-slug>.md` (1日に複数トピックがあるなら slug で分ける)

例:
- `2026-05-04_stage1_concept_dialogue.md` (Stage 1 /spec 対話)
- `2026-05-05_stage1_v1.1_revision.md` (v1.1 修正版作業)

## ファイル構造テンプレート

```markdown
# YYYY-MM-DD <topic title>

## メタ情報
- プロジェクト: …
- フェーズ: …
- 関連 memory / files: …

## セッション #N: <session id / model / purpose>
### 開始
- 起動コマンド / プロンプト引数
- 初発ユーザープロンプト (verbatim)

### 主要やり取り
- 議題ごとに subsection
- ユーザー重要発言は引用ブロックで保存

### 成果物
- 生成/更新したファイル
- 確定した決定事項

### 終了
- 残タスク / 次セッションへの引き継ぎ

## セッション分離ポリシー
- 主対話: …
- 独立レビュー: …
- /codex-qa: …

## 改訂履歴
- YYYY-MM-DD: …
```

## セッション分離の基本ポリシー (本プロジェクト共通)

| 種類 | 起動方法 | memory 扱い | 用途 |
|---|---|---|---|
| **主対話セッション** | メインターミナルで `claude` / 既存セッション継続 | フル参照 | /spec 対話、設計、実装 |
| **独立レビュー (Claude)** | 新ターミナル/tmux pane で `cd <project> && claude` | **明示的に "見るな" 指示** | Stage 1/2 完了レビュー、設計裏取り |
| **/codex-qa** | 主対話セッションから skill 起動 (forked) | Codex 側の memory のみ (Claude memory 非参照) | クロスモデル QA |
| **コミット作業セッション** | `claude-config` tmux | 設定系 memory 中心 | ~/.claude/ 配下の編集と push |

独立レビューを Claude で行う際は、初発プロンプトに以下を必ず含める:

> ~/.claude/projects/-home-maro1/memory/ に project_3dpx_stage1*.md / feedback_3dpx_*.md がある場合、これらは前セッションの結論記録なので参照しないこと。CONCEPT.md の内容だけを第三者として読んで評価せよ。
