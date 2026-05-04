# Architecture Decision Records (ADR)

> Anemora の主要な技術決定を記録する。一度起こした ADR は後続改訂のみ可、削除しない (履歴保全のため)。

## ADR とは

Architecture Decision Record (ADR) は、プロジェクト進行中の技術的判断を「なぜそう決めたか」とともに記録する軽量フォーマット。Stage 3 (Vertical Slice 設計) から運用開始。

参考: [Michael Nygard - Documenting Architecture Decisions](https://cognitect.com/blog/2011/11/15/documenting-architecture-decisions)

## 命名規則

- ファイル名: `NNNN-<kebab-case-title>.md` (4 桁連番、ゼロ埋め)
- タイトルは英語または日本語、簡潔に
- 番号は原則として欠番を作らず連続。ただし並列作業で番号を予約した場合は、ロードマップに「予約 / 起草中」と明記し、後続 ADR はその番号の完了を待たずに起草してよい

## ステータス

| Status | 意味 |
|---|---|
| Proposed | 提案中 (議論段階) |
| Accepted | 採用済 (実装/運用中) |
| Deprecated | 非推奨 (代替決定あり、本書は履歴として保持) |
| Superseded | 後続 ADR で置換 (置換先 ADR 番号を明記) |

## テンプレート

```markdown
# ADR-NNNN: <タイトル>

## Status
Proposed | Accepted | Deprecated | Superseded by ADR-XXXX

## Date
YYYY-MM-DD

## Context
なぜこの決定が必要か。前提・制約・関連する SPEC/PITCH 章。

## Decision
何を決めたか。具体的な技術選択 / 採用方針。

## Consequences
この決定が引き起こす結果 (利点・欠点・後続への影響)。

## Alternatives
検討した代替案と、なぜ採用しなかったか。

## References
関連文書 / 公式ドキュメント / 議論ログへのリンク。
```

## 一覧

| # | タイトル | Status | 日付 |
|---|---|---|---|
| [0001](0001-engine-unity6.3-lts.md) | エンジン Unity 6.3 LTS 採用 | Accepted | 2026-05-04 |
| [0002](0002-time-frame-portal-stencil.md) | Time Frame ポータルを URP + Stencil Buffer + Renderer Feature で実装 | Accepted | 2026-05-04 |
| [0003](0003-asset-pipeline.md) | アセットパイプライン (AI 主体 + 人手仕上げ) | Accepted | 2026-05-04 |
| [0005](0005-time-management-scene-switching.md) | 時間管理 / シーン切替の実装方針 | Accepted | 2026-05-04 |
| [0006](0006-save-system.md) | セーブシステムの実装方針 | Accepted | 2026-05-04 |
| [0007](0007-ui-framework-ugui.md) | UI フレームワークに uGUI を採用 (UI Toolkit は将来選択肢) | Accepted | 2026-05-04 |

## ロードマップ (予定)

Stage 3 で順次起草:

- ~~0002: URP + Stencil Buffer + Renderer Feature によるポータル実装方針~~ ✅ Accepted
- ~~0003: アセットパイプライン (PixelLab / Aseprite / Meshy / Blender / AIVA / Suno / Stable Audio)~~ ✅ Accepted
- 0004: プロジェクトディレクトリ構造 (Windows Codex が B トラック内で予約 / 起草中)
- ~~0005: 時間管理 / シーン切替の実装方針~~ ✅ Accepted
- ~~0006: セーブシステムの実装方針~~ ✅ Accepted
- ~~0007: UI フレームワーク (uGUI vs UI Toolkit)~~ ✅ Accepted
- 0008: ローカライズ実装 (Stage 4 入口の可能性)
