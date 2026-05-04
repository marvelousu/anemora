# Steam AI 開示文面 (草案)

> Steam リリース時 (Stage 5 オプション) の Content Survey 提出に向けた開示区分整理表と文面草案。
> Steam AI 開示 2026-01-17 改訂を前提に、`docs/legal/asset_ledger.md` の記録から開示内容を抽出する。

> **Status**: Stage 3 Day 0 (2026-05-04) 起草、テンプレート段階。Stage 5 (Steam リリース判断時) に最終文面を確定する。
> **重要**: Steam の運用は変わり得るため、提出時点で Steamworks の実フォームとガイドラインを再確認する。

---

## 1. 開示区分の整理

### 1.1 Tier 1: Pre-Generated AI Content (player-consumed AI 生成物)

ゲーム内でプレイヤーが直接接する AI 生成物。**開示対象 (Steam ストアページに表記)**。

該当例:
- AI 生成のドット絵キャラクター / 重要オブジェクト
- AI 生成の 3D 背景モデル / 環境装飾
- AI 生成の BGM / 環境音 / SFX
- AI 生成のストアビジュアル (Steam ページ表示分)
- AI 生成の UI 素材 / タイポグラフィ / アイコン
- AI 生成のローカライズ素材 (機械翻訳ベース)

### 1.2 Tier 2: Engineering AI Tools (開発支援、player-consumed されない)

開発時のみ使用、プレイヤーは直接触れない。**原則として開示対象外の見込み**だが、提出時点で Steamworks のフォーム文言を再確認する。

該当例:
- Claude (設計対話 / 文書生成 / コード支援)
- Codex (実装支援 / QA)
- 設計フェーズの AI 補助 (Stage 1-3 の対話など)
- AI コードレビュー / 自動テスト生成支援

### 1.3 Live-Generated AI Content (実行時 AI 生成)

Anemora は **採用しない** (実行時 AI 生成機能は Stage 5 までスコープ外)。Steam の Live-Generated 開示区分にも該当しない見込み。

---

## 2. 開示文面草案 (Stage 5 で確定)

### 2.1 Steam Content Survey 文面 (英語)

```
This game uses pre-generated AI content for the following assets:
- Pixel art sprites (characters and key objects)
- 3D background models (buildings and environment props)
- Background music tracks
- Environmental sounds and sound effects
- Store visuals (key art, capsules)
- UI elements (typography, icons)
- Localization (machine-translated base, manually edited)

All AI-generated assets have been manually edited and finalized by the developer.
We use commercially licensed AI tools (e.g., Adobe Firefly, AIVA Pro, Suno paid plan, ElevenLabs)
and maintain an asset ledger for provenance tracking.

Game development was assisted by engineering AI tools (Claude, Codex)
for design discussions, documentation, and code support.
These tools do not generate player-consumed content at runtime.
```

(英語草案。提出時に Steamworks のフォーム文字数制限・推奨文言に合わせて調整)

### 2.2 itch.io ページ AI 注釈 (日本語 / 英語併記)

```
本作は AI 生成ツール (Adobe Firefly, AIVA Pro, Suno paid plan, ElevenLabs SFX, PixelLab,
Retro Diffusion, Meshy, Blender) を使用しています。すべての AI 生成アセットは
人手で最終調整・編集されており、商用利用可能なツール / プランに限定しています。

This game uses AI generation tools. All AI-generated assets have been manually
finalized by the developer. Asset provenance is tracked in our asset ledger.
```

### 2.3 GitHub README AI 開示

```
## AI Tools Disclosure

This project uses the following AI tools:
- **Player-consumed assets** (visible in-game): Adobe Firefly, AIVA Pro, Suno (paid plan),
  Stable Audio 2.5, ElevenLabs SFX v2, PixelLab, Retro Diffusion, Meshy v6
- **Engineering support** (not in-game): Claude (Anthropic), Codex (OpenAI)

All AI-generated assets are manually finalized by the developer.
Provenance tracking: see `docs/legal/asset_ledger.md`.
```

---

## 3. Steam 提出時の手順

Stage 5 (Steam リリース判断時) に以下を実施:

1. **Steamworks フォーム文言を再確認** (2026-01-17 改訂版 + 提出時点の最新)
2. `docs/legal/asset_ledger.md` から開示対象アセット (Tier 1) を抽出
3. 上記 §2.1 の英語文面を提出時の状況に合わせて調整
4. Tier 2 の開示要否を Steam 側に確認 (engineering AI tools の扱い)
5. 提出後の Steam 審査結果に応じて文面修正
6. 公開後、規約変更があれば再提出

---

## 4. 法的整合の継続監視

Stage 3-5 中に以下を **定期チェック** (1 ヶ月に 1 回程度):

- Steam AI 開示ガイドラインの改訂
- 各 AI ツールの利用規約変更 (PixelLab / Retro Diffusion / Meshy / AIVA / Suno / Stable Audio / ElevenLabs / Adobe Firefly / Claude / Codex / DeepL)
- 商用利用条項の変更
- AI 生成物の著作権訴訟 / 判例の動向

変更があれば `asset_ledger.md` §3 規約変更履歴 に記録、必要に応じて代替パスへ切替。

---

## 5. 関連文書

- `docs/legal/asset_ledger.md` (アセット記録、本書の提出材料)
- `ADR-0003` (アセットパイプライン、運用方針)
- `PITCH.md` §8 (AI-Driven Solo Production Pipeline)
- `PITCH.md` §10 (Risk, Mitigation & Open Development)

---

## 6. 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v0 | 2026-05-04 | 初版起草 (テンプレート、ADR-0003 と整合、Stage 5 で文面確定) |
