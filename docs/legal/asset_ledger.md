# アセット法務台帳 (Asset Ledger)

> Anemora で使用するすべての AI 生成アセットの権利・出典・公開可否を記録する。
> Steam AI 開示 (Stage 5 リリース時) の申告材料として運用。
> ADR-0003 (アセットパイプライン) で運用方針を確定、本ファイルは記録責務を持つ。

> **Status**: Stage 3 Day 0 (2026-05-04) 起草、テンプレート段階。実アセット生成時に逐次追記する。
> **更新タイミング**: アセット生成直後 5 分以内 (運用負荷を分散させるため)

---

## 1. 運用方針

### 1.1 何を記録するか

各 AI 生成アセットについて、以下を記録:

- **ID** (アセット固有 ID、ファイル名と紐付け)
- **種別** (ドット絵 / 3D / BGM / SFX / UI / ストアビジュアル / ローカライズ)
- **アセット名 / ファイルパス**
- **生成日**
- **使用ツール** (PixelLab / Retro Diffusion / Meshy / AIVA / Suno / Stable Audio / ElevenLabs SFX / Adobe Firefly / Claude / 他)
- **プラン** (Free / Paid / Pro / API / on-prem 等、商用利用可否の根拠)
- **入力素材** (プロンプト / 入力画像 / 参照素材 など)
- **手修正の有無 / 内容** (Aseprite / Blender / Reaper / Photoshop / 等で何をしたか)
- **商用利用可否** (各ツールの条項に基づく判定)
- **公開可否** (GitHub Public / itch.io / Steam の段階別)
- **Steam 開示区分** (Tier 1 player-consumed / Tier 2 engineering / 開示不要)
- **備考**

### 1.2 ツールごとの権利条件 (記録時の参照)

各ツールの商用利用条項の **基準時点** (台帳記録時に再確認):

| ツール | 商用利用条件 (記録時) | 注意点 |
|---|---|---|
| PixelLab | 公開 ToS 上は生成物の商用/非商用利用可。生成時のアカウント/プラン状態は別途記録必須 | 2026-05-05 Codex 確認: ToS §1.3/§3.3。API 利用は公式 API のみ、生成物で他モデルを訓練する用途は禁止/要許諾。PixelLab UI の paid 加入状態は batchmode から未確認 |
| Retro Diffusion (Scenario 経由) | Scenario paid plan なら商用利用可。Free plan 出力は採用しない | 2026-05-05 Codex 確認: Scenario pricing/terms。Retro Diffusion/RD Plus は Scenario 上のモデルとして確認。Aseprite 拡張版/standalone で使う場合は別途規約確認 |
| Aseprite | 自分の創作物に限り商用可 | 2026-05-05 Codex 確認: 公式 FAQ。会社利用は developer ごとに license 必要、Aseprite 本体の再配布は禁止 |
| Meshy v6 | 要確認 | LowPoly Mode の出力ライセンス確認 |
| Blender | 商用利用可 (GPL) | 出力物に GPL は伝播しない |
| AIVA Pro | Pro plan でフル商用権 | プラン証跡を残す、Free は不可 |
| Suno v5.5 | **paid plan のみ commercial use rights** | Free plan 出力は採用しない |
| Stable Audio 2.5 | 要確認 (プラン / API / on-prem 別) | 2.5 の具体契約形態を生成時点で再確認 |
| ElevenLabs SFX v2 | Sound Effects Terms に従う | Voice 系条項とは別、Voice は不採用 |
| Adobe Firefly | beta なし機能は商用可、Adobe IP 補償あり | Creative Cloud 加入で利用、beta 機能は明示禁止確認 |
| Claude (Anthropic) | 出力物の利用権はユーザー帰属 (Acceptable Use Policy 遵守) | 開発支援は engineering AI tools (Tier 2) 想定 |
| Codex (OpenAI) | 出力物の利用権はユーザー帰属 (Codex 利用規約遵守) | 開発支援は engineering AI tools (Tier 2) 想定 |
| DeepL Pro | 商用利用可 (Pro plan) | ローカライズ用 |

2026-05-05 参照 URL:
PixelLab ToS `https://www.pixellab.ai/termsofservice` / PixelLab API `https://www.pixellab.ai/pixellab-api` / Scenario terms `https://www.scenario.com/terms-and-conditions` / Scenario pricing `https://www.scenario.com/pricing` / Scenario Retro Diffusion essentials `https://help.scenario.com/articles/4202673551-retro-diffusion-models-the-essentials` / Aseprite FAQ `https://www.aseprite.org/faq`

### 1.3 中間ファイルの扱い

- AI 生成の中間ファイル (失敗作 / プロンプト試作 / バリエーション) は `art/_intermediate/` 等に保管
- `.gitignore` で除外し **GitHub Public には公開しない**
- 必要に応じて別途バックアップ (Stage 3-5 中)

### 1.4 公開可否の判断軸

| 段階 | 判断基準 |
|---|---|
| GitHub Public (Day 0 〜) | ソースコード + docs + 確定アセット、AI 中間ファイルは除外 |
| itch.io (VS 完成後) | ビルド + ストア素材、AI 開示文を README で明示 |
| Steam (Stage 5 オプション) | ビルド + ストア素材 + Steam Content Survey 申告 |

---

## 2. 台帳本体 (アセット記録)

### 2.1 ドット絵 (キャラクター + 重要オブジェクト)

| ID | アセット名 | 生成日 | ツール | プラン | 入力素材 | 手修正 | 商用可否 | 公開可否 | Steam 開示区分 | 備考 |
|---|---|---|---|---|---|---|---|---|---|---|
| (Stage 3 着手時に追記) | | | | | | | | | | |

### 2.2 3D 背景 (建物 / 環境装飾)

| ID | アセット名 | 生成日 | ツール | プラン | 入力素材 | 手修正 | 商用可否 | 公開可否 | Steam 開示区分 | 備考 |
|---|---|---|---|---|---|---|---|---|---|---|
| (Stage 3 着手時に追記) | | | | | | | | | | |

### 2.3 BGM

| ID | アセット名 | 生成日 | ツール | プラン | 入力素材 | 手修正 | 商用可否 | 公開可否 | Steam 開示区分 | 備考 |
|---|---|---|---|---|---|---|---|---|---|---|
| (Stage 3 着手時に追記) | | | | | | | | | | |

### 2.4 環境音 / SFX

| ID | アセット名 | 生成日 | ツール | プラン | 入力素材 | 手修正 | 商用可否 | 公開可否 | Steam 開示区分 | 備考 |
|---|---|---|---|---|---|---|---|---|---|---|
| (Stage 3 着手時に追記) | | | | | | | | | | |

### 2.5 UI 2D / タイポグラフィ / アイコン

| ID | アセット名 | 生成日 | ツール | プラン | 入力素材 | 手修正 | 商用可否 | 公開可否 | Steam 開示区分 | 備考 |
|---|---|---|---|---|---|---|---|---|---|---|
| (Stage 3 着手時に追記) | | | | | | | | | | |

### 2.6 ローカライズ素材 (テキスト)

| ID | アセット名 | 生成日 | ツール | プラン | 入力素材 | 手修正 | 商用可否 | 公開可否 | Steam 開示区分 | 備考 |
|---|---|---|---|---|---|---|---|---|---|---|
| (Stage 3 着手時に追記、Stage 4-5 で本格運用) | | | | | | | | | | |

### 2.7 ストアビジュアル (Steam ページ等)

| ID | アセット名 | 生成日 | ツール | プラン | 入力素材 | 手修正 | 商用可否 | 公開可否 | Steam 開示区分 | 備考 |
|---|---|---|---|---|---|---|---|---|---|---|
| (Stage 3-5 リリース判断時に追記) | | | | | | | | | | |

---

## 3. 規約変更履歴 (重要)

各 AI ツールの規約変更を Stage 3-5 中に定期チェックし、変更があれば本セクションに記録:

| 日付 | ツール | 変更内容 | 対応 |
|---|---|---|---|
| (規約変更時に追記) | | | |

---

## 4. 関連文書

- `ADR-0003` (アセットパイプライン、本台帳の運用方針)
- `docs/legal/steam_ai_disclosure.md` (Steam AI 開示用文面、本台帳から提出材料を抽出)
- `PITCH.md` §8 (AI-Driven Solo Production Pipeline)
- `SPEC.md` §7 / §8 (Art / Sound)

---

## 5. 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v0 | 2026-05-04 | 初版起草 (テンプレート、ADR-0003 と整合) |
| v0.1 | 2026-05-05 | PixelLab / Scenario-hosted Retro Diffusion / Aseprite の公開規約確認結果を §1.2 に追記。PixelLab paid 状態は未確認として分離 |
