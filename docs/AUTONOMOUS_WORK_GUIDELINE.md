# Anemora 干渉なし作業ガイドライン

> **作成**: 2026-05-07
> **目的**: ユーザーが干渉できない時間帯 (睡眠・出勤・外出等) に Codex が独自判断で進められる作業を明示する。
> **位置付け**: 物語/世界観の核心判断は本書ではなく `STORY_BIBLE_v1.md` に従う。本書は **グラフィック / シェーダ / 技術改善 / 整備系** の "干渉不要で進められる範囲" を扱う。

---

## 1. 進められる作業 (干渉不要、独自判断 OK)

### 1.1 グラフィック polish (SPEC §7.1 Tier 2 範囲内)

- HD-2D Tier 2 シェーダの polish (動的影、単一方向光、カラーグレーディング)
- 既存スプライトの polish (PixelLab 再生成 or Aseprite 仕上げ)
- 既存ゾーン背景の polish (Meshy v6 → Blender 修正)
- VFX (時の窓ポータル、痕跡可視化、層 2 片鱗演出) の調整
- パーティクル品質改善 (環境装飾、Stage 4 範囲)

### 1.2 技術改善

- Unity URP Renderer Feature 最適化
- ステンシルバッファ実装の磨き (PortalStencilFeature、ADR-0002)
- パフォーマンス改善 (Profile 確認、ボトルネック解消)
- アクセシビリティ実装 (字幕、UI 拡大、コントラスト切替)
- セーブ/ロード仕様の磨き (オートセーブ条件追加、復元境界整理)
- ローカライズ基盤の polish (Unity Localization、StringTable)

### 1.3 バグ修正・テスト

- PlayMode test の追加 / 修正 (既存 16/16 green の維持)
- EditMode test の充実
- Stage 3 G5 検証マトリクス (`docs/G5_ACCEPTANCE_MATRIX.md`) の項目追加・実行
- 既知バグの修正 (Codex の `_handover/` ログを参照)

### 1.4 ドキュメント整備

- ADR (Architecture Decision Record) の補完 (`docs/adr/`)
- API ドキュメント (`docs/api/`) の整備
- アセット台帳 (`docs/legal/asset_ledger.md`) の更新
- OSS 台帳 (`docs/legal/oss_ledger.md`) の整備
- Steam AI 開示 (`docs/legal/steam_ai_disclosure.md`) の準備

### 1.5 既存 Story Bible の構造に沿った clarification 作業

- `STORY_BIBLE_v1.md` §8.1 確定事項の **実装側からの整合性検証**
- 矛盾点 / 抜け漏れの発見 → コメントとして残す (= `STORY_BIBLE_v1.md` 末尾に追記、または別 issue/devlog)
- ただし **判断は変更しない、レポートのみ**。判断変更はユーザー判断保留 (§2)

---

## 2. 進められない作業 (要ユーザー判断、独自進行禁止)

以下は Codex が独自判断で進めず、ユーザー判断を待つ:

### 2.1 物語 / 世界観の核心判断

- `STORY_BIBLE_v1.md` §8.1 確定事項の変更
- 各 NPC の名前 final 化 (provisional → final はユーザー判断、§2 推奨方法を後で確認)
- ゾーン B/C/D のテーマ確定 (現状は仮称)
- ゲート条件の具体内容
- player-facing 章名 (「層」用語を出さない別表現)
- 真層第 4 の壁演出の具体テキスト
- 3 エンドの具体演出 (放棄エンドのセーブ削除是非、継承エンドの暗示、終焉エンドの空白)

### 2.2 メカ追加 / 変更

- `STORY_BIBLE_v1.md` §6.2 「部分空間ギミック P1-P7 各層配分」は **暫定**、Codex から提案 OK
- ただし P1-P7 の **新規ギミック追加** (P8 以降の発明) はユーザー判断
- 時の窓基本機構の変更はユーザー判断
- 観測者輪廻 / 異物原則 / 階層的開示の構造変更はユーザー判断

### 2.3 美術スタイルの大幅変更

- HD-2D Tier 2 → Tier 3-4 への移行はユーザー判断 (SPEC §7.1 で明示却下、ただしユーザーから "本格的なシェード" の意向あり、後で具体内容確認)
- カメラ設計の変更 (固定アイソメ → 自由視点) はユーザー判断 (SPEC §7.4 で確定)
- カラーパレットの大幅変更はユーザー判断

### 2.4 公開 / リリース判断

- Steam Early Access 提出
- itch.io / GitHub Public へのビルド公開
- ライセンス決定 (現状 All Rights Reserved default)
- AI 生成アセット開示の最終形

### 2.5 commit / push の自動化

- `STORY_BIBLE_v1.md` 本体の commit / push はユーザー確認推奨
- 自動化された routine 作業 (lint / format / test 結果反映) は OK
- 物語 doc の編集を含む commit はユーザー確認

---

## 3. 進められる作業の境界判断ガイド

判断に迷ったら以下を確認:

1. **物語 / 世界観に影響するか?** → Yes ならユーザー判断保留
2. **`STORY_BIBLE_v1.md` §8.1 確定事項を変更するか?** → Yes ならユーザー判断保留
3. **新規メカ / 美術スタイルの大幅変更か?** → Yes ならユーザー判断保留
4. **既存仕様の polish / 最適化 / バグ修正か?** → Yes なら独自判断 OK
5. **現状動作の整合性検証 / レポートか?** → Yes なら独自判断 OK (レポートのみ、判断変更しない)

---

## 4. ユーザーから後で確認すべき事項

Phase 5 / Phase 6 進行時に確認したい事項:

- **本格的なシェードの導入の具体内容** (SPEC §7.1 Tier 2 範囲を超える可能性、ユーザー意向あり 2026-05-07)
- **NPC 生成のやり方の推奨** (ユーザーから推奨方法あり、2026-05-07 言及)
- **NPC 名 final 化の方法** (provisional → final)
- **ゾーン B/C/D テーマの具体化方法** (Claude/Codex どちらで詰めるか)
- **player-facing 章名の方針** (Claude/Codex どちらで詰めるか)
- **3 エンドの具体演出** (詳細はユーザーが直接決めたいか、提案を出して選ぶか)

---

## 5. 進捗報告フォーマット

干渉なしで進めた作業は以下の形式で報告:

```
## YYYY-MM-DD <作業内容> 完了

### 進めた範囲
- [§1 のどの category か明示]

### 変更内容
- [具体的なファイル / 機能]

### 確認したい点 (もしあれば)
- [ユーザー判断保留に該当するか迷った点]

### 次のステップ
- [次に進められる作業 / 待ちが発生した点]
```

報告は `~/notes/_handover/anemora-windows-handover-YYYY-MM-DD-<topic>-complete.md` に既存パターンで残す。

---

## 6. 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v1.0 | 2026-05-07 | 初版起草 (干渉なし作業の境界明示) |

---

## 7. 関連 doc

- `STORY_BIBLE_v1.md` (Phase 1-4 確定の物語骨格、本書の上位 doc)
- `SPEC.md` (Stage 2 GDD)
- `VS_SCOPE.md` (Stage 3 VS 完成定義)
- `STAGE4_ROADMAP.md` (Stage 4 計画)
