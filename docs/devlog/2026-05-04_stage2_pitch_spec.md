# 2026-05-04 Stage 2 PITCH / SPEC 起草 開発ログ

## メタ情報

- **プロジェクト**: Anemora — HD-2D 探索アクションアドベンチャー
- **フェーズ**: Stage 2 (GDD v0 起こし + 公開ポートフォリオ用ピッチ作成)
- **日付**: 2026-05-04 (Stage 1 完了と同日に Stage 2 着手・1 ヶ月集中開発の Day 0)
- **前段ログ**: `2026-05-04_stage1_concept_dialogue.md` (Stage 1 コンセプト対話)

## 成果物

| ファイル | 内容 | 行数 |
|---|---|---|
| `PITCH.md` | 公開ポートフォリオ用企画書 (10 章、外部ステークホルダー向け) | 867 |
| `SPEC.md` | Stage 2 GDD v0 (breadth-first 13 章、実装観点で再展開) | 763 |

## 関与モデル / セッション一覧

| # | 種類 | モデル | 用途 |
|---|---|---|---|
| 1 | 主対話 (PITCH 起草) | Claude Opus 4.7 (Claude Code) | `/spec` 対話で章 8 → 章 1-7・9-10 を順次起こす |
| 1-sub | 並列研究 | Claude general-purpose Agent × 3 並列 | エンジン/開発環境、AI 画像、AI 音響/3D/ローカライズの最新調査 |
| 1-sub | クロスモデルレビュー (PITCH §8 章立て) | Codex (fast tier) | §8 章立て妥当性検証 |
| 1-sub | クロスモデルレビュー (PITCH §8 技術スタック) | Codex (fast tier) | 技術スタック追加調査 + 事実誤認補正 |
| 2 | 主対話 (SPEC 起草) | Claude Opus 4.7 (Claude Code) | breadth-first 13 章を一括起草 (対話なし、PITCH/CONCEPT を素材に) |
| 2-sub | クロスモデルレビュー (SPEC v0) | Codex (fast tier) | A/B/C/D/E 5 軸レビュー → 12 件差分案 |

## Phase A: PITCH.md 起草 (10 章)

### 章立て確定 (Codex レビュー反映後)

1. Cover / Logline
2. Executive Summary
3. Signature Moment (30 秒トレイラー版 + 3 分体験版)
4. Gameplay & Core Loop
5. World, Tone & Narrative Promise
6. Visual Direction & Mood References
7. Audience, Comparables & Positioning
8. **AI-Driven Solo Production Pipeline** ★ (最初に確定)
9. Scope, Milestones & Feasibility
10. Risk, Mitigation & Open Development

### 進行方針

- ユーザー指定: 「8 から、後は順番で」 — §8 を起点に確定 (技術スタック・役割分担・公開戦略・リスク全領域を最初に固める)、続いて §1-7・§9-10
- **AI 主体個人開発を「売り」として打ち出す方針** をユーザー指示で確定
- §8 の AI 活用層分離: **Claude (設計/対話/文書/最終判断補助) + Codex (実装/QA/反復) + Blender (3D アセット) + ユーザー (最終判断)**

### 並列研究 (Agent × 3)

§8 の技術スタック確定のため、`general-purpose` Agent を 3 系統並列で起動:

- **エンジン / 開発環境** — Unity 6.3 LTS / Godot 4.6 / VS Code / GitHub Actions 等の最新動向
- **AI 画像** — PixelLab / Aseprite / Retro Diffusion / Adobe Firefly / 各社の商用利用条項
- **AI 音響 / 3D / ローカライズ** — AIVA Pro / Suno v5.5 / Stable Audio 2.5 / Meshy v6 / Reaper / 多言語化選択肢

### Codex レビュー (fast tier) の反映

| 検出項目 | 反映 |
|---|---|
| Suno v5 EOL → v5.5 (世代遷移) | §8 BGM スタックを Suno v5.5 に更新 |
| AIVA Pro 価格更新 | §8 月額予算に反映 |
| Blender 4.5 LTS (4.4 → 4.5 移行) | §8 3D パイプラインに反映 |
| Midjourney 法的安全性懸念 | §8 から除外、Adobe Firefly を法的安全層として採用 |
| Steam AI 開示 2026-01-17 改訂 (Tier 1 player-consumed disclosure / engineering AI tools 免除) | §10 / §8 公開戦略に反映 |
| Codex の VS スコープ縮減提案 (Hook + 1 compact zone) | **不採用** (1 ヶ月集中ペースで全本編可能とユーザー判断) |

### グラフィック方針の混同訂正

研究 Agent が空気感参照 3 作 (段階開示型残酷ファンタジー / 静謐終末紀行 / 無垢視点アニメ) を絵柄参照として扱っていた。ユーザー指摘で訂正:

- **絵柄参照**: HD-2D 系既存作 (大手パブリッシャ HD-2D ライン)
- **空気感参照**: 上記 3 作品 (テーマ・物語的方向性のみ、絵柄は参照しない)

### ボイス採用見送り

ユーザー指摘 (一発出しの自然なクオリティが現状 AI で困難 + 当初からボイスなし想定) で **ElevenLabs v3 を技術スタックから除外**。テキスト + 環境音 + 余白で語る方針を §6 / §8 に明記。

### HD-2D Tier 議論

動的ライティングの工数増加リスクをユーザーが懸念 → Tier 0-4 フレームワーク提示 → **Tier 2 (動的影 + 単一方向光) を採用**、Tier 3-4 (volumetric / sprite normal map / multiple lights) は不採用。HD-2D 系既存作を参照してプロトタイプで検証。

### 三段階公開モデル

- **GitHub Public** (Day 0) — 最低保証、ソース + docs + devlog
- **itch.io** ($0) — Vertical Slice 完成後 (Stage 3 後半)
- **Steam** ($100 サンクコスト判断後) — 完成度しきい値超過後 (Stage 5)

2026-05-04 確定済の三段階モデルを公開ドックに反映。私的メモの内容自体は公開せず、結論のみ転載。

### Obsidian / workflow 更新

- ローカル Obsidian vault に PITCH/CONCEPT/README をコピー
- Working Copy (iOS) で表示されない問題 → vault は git repo のため commit + push が必要と判明
- 共通 workflow ルールに追加: 「vault 配下にファイルを書込み・コピーした場合は commit + push まで一連で行う」

### 工数見積もりバイアスの矯正

ユーザーから複数回指摘:
- 「AI は工数見積もりが甘すぎます」「2,3 時間しか取れないと勘違いしているのでは」
- 「Stage 2 を 2-3 日と書いた時点でずれている、本日中にも終わる量」
- 「9,10 章を 15-30 分と書いていたが 10 分もかかっていない」

→ **見積もりバイアスを認識し、Stage 2 では時間数値の事前提示を最小化**。本書の数値も保守的に見えるが、実際は前倒し可能。

## Phase B: SPEC.md 起草 (breadth-first 13 章)

### 章立て (大手 GDD 準拠)

1. Overview / 2. Gameplay / 3. Story / 4. Characters / 5. Systems / 6. Levels / 7. Art / 8. Sound / 9. UI/UX / 10. Technology / 11. Production / 12. Risk / 13. Appendices

### 進行方針

- **breadth-first** で 1 周し、後続 Stage で深掘り (depth-first 改訂)
- **重複は避ける** (PITCH.md / CONCEPT.md と二層構造、SPEC は実装観点で再展開)
- **TBD を明示** (Stage 3 で確定すべき項目を Stage 2 で勝手に決めない、`feedback_anemora_no_premature_lockin` 遵守)

### 起草フローの障害切り分け

複数回「着手します」と宣言しながら同レスポンス内でファイル書き込みが行われず、会話が停止する事象が発生。原因は応答パターン (宣言文で終了し書き込みに進まない) 側にあると判定、書き込み機能自体は正常動作。修正後、宣言なしで直接書き込みを発行して 758 行の v0 を作成。

### Codex (fast) v0 レビュー → 差分案 → P0+P1 適用

レビュー軸: A. クリティカル / B. 重要改善 / C. 抜け落ち候補 / D. TBD 妥当性 / E. 良い点

差分案 12 件 (P0×2 / P1×5 / P2×5)。ユーザー判断: **P0+P1 のみ適用、P2 は見送り** (メトロイドヴァニア明文化など早期確定回避)。

#### P0 (適用済)
- §3.3 真層: 「Stage 1 で並列維持」→「Stage 2 で並列維持」(時制ズレ訂正)
- §10.1: Unity 本線確定、Godot は撤退候補に格下げ (PITCH §8 と整合)

#### P1 (適用済)
- §5.2: 未来側能動行動「Stage 3 では過去側先行実装、未来は最小実装」注記
- §2.4: エンディング「5-8 種」削除、「真エンディング」を「複数エンドの最終選択」に統一
- §6.2: ゾーン候補 A〜D を「仮称」化、配置方針のみ Stage 2 固定
- §7.4: カメラ角度数値 (30°-45°) 削除、「Stage 3 でプロトタイプ確定」

#### P2 (見送り)
- 進行ログ項目定義 / アクセシビリティ確定 / 手動セーブ採用 + スロット 3 / シンボル 3 個固定 / メトロイドヴァニア採用範囲限定 — いずれも Stage 3 での確定に委ねる

### 結果

- v0 (758 行) → v0.1 (763 行)、改訂履歴は §13.4 に記載

## Stage 2 → Stage 3 引継ぎ

### 確定事項 (Stage 2 で固定、変更しない)

- ジャンル: HD-2D 探索アクションアドベンチャー (戦闘なし)
- 視点: 固定アイソメ (自由視点排除)
- ボイス: 採用しない (テキスト + 環境音)
- HD-2D: Tier 2 簡素版
- エンジン本線: Unity 6.3 LTS + URP (Godot 4.6 は撤退候補)
- 開発スタイル: 1 ヶ月集中、AI 主体個人開発
- 公開: 三段階 (GitHub Public Day 0 → itch.io VS 完成後 → Steam オプション)

### Stage 3 (Vertical Slice) で確定すべき主項目

- 主人公の名前 / 性別 / 年齢 / ヒーロービジュアル
- 衰退原因の具体描写 (比喩か実体か)
- 時の筆の起源 / 主人公が選ばれた理由
- 真層の収束パターン (複数案 → 1 つに絞る)
- 時の窓の細部仕様 (サイズ / 持続時間 / 停止 vs 減速)
- ゾーン構成の最終確定 + ゲート条件
- カメラ角度 (実機プロトタイプで決定)
- セーブ仕様 / 進行ログ情報設計 / アクセシビリティ詳細
- メトロイドヴァニア採用範囲の運用定義

### Vertical Slice 対象

- 第 1 ゾーン (仮称: 街)
- 時の窓プロトタイプ (Unity URP + Renderer Feature + Stencil Buffer)
- 主人公ヒーロービジュアル
- 第 1 層のコアループ完走

## 関連 memory

- `project_anemora_stage1_complete.md` (Stage 1 完了)
- `feedback_anemora_no_premature_lockin.md` (世界観・コンセプト軸の早期確定回避)

## 反省点 / 次回への申送り

1. **工数見積もりバイアス** — Claude の時間見積もりは保守的に出やすい。Stage 3 以降は数値を出さないか、ユーザーに「枠」として提示するに留める
2. **起草宣言だけで停止する問題** — 「着手します」で応答終了せず、同レスポンス内で書き込みまで進める
3. **クロスモデルレビューの定常化** — Stage 1 で確立した 3 ラウンドレビューを Stage 2 でも実施 (Codex × 3 回)、Stage 3 でも継続予定
4. **早期確定回避の境界** — 世界観・コンセプト軸 = 確定回避 / システム機構 = Stage 2 で固定可、の判断軸が明確化。P2 群の扱いはユーザー判断に委ねる方針が機能
