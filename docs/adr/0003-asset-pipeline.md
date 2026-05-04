# ADR-0003: アセットパイプライン (AI 主体 + 人手仕上げ)

## Status

Accepted

## Date

2026-05-04 (Stage 3 Day 0)

## Context

Anemora は **AI 主体個人開発** を「売り」にする 1 ヶ月集中プロジェクト。VS 制作 (Stage 3) から Stage 5 公開まで、複数のアセット種別 (ドット絵 / 3D / BGM / SFX / ストアビジュアル / VFX) を AI ツールと人手仕上げの組合せで生成する必要がある。

### 制約

- **個人開発** (AI 主体、Claude × Codex × Blender + ユーザー最終判断)
- **1 ヶ月集中スケジュール** (Day 0-28)、Vertical Slice は Day 0-10
- **月次予算 ¥30,000-40,000** (Claude / Codex サブスクは別枠、PITCH §8 / §11 確定)
- **HD-2D Tier 2 簡素版** (動的影 + 単一方向光、Tier 3-4 不採用)
- **Silent protagonist** (ボイスなし、テキスト + 環境音 + 余白で語る)
- **三段階公開** (GitHub Public Day 0 → itch.io VS 完成後 → Steam Stage 5 オプション)
- **法的整合**: Steam AI 開示 2026-01-17 改訂を前提に、Tier 1 (player-consumed AI 生成物) の開示内容を台帳ベースで準備する。Tier 2 (Claude / Codex 等の開発支援) は **原則として開示対象外の見込み** だが、提出時点の Steamworks フォームとガイドラインを再確認して確定する
- **アセット権利の証跡**: `docs/legal/asset_ledger.md` で AI 生成と権利関係を逐次記録 (Stage 3 Day 0-1 でテンプレート先行起草)

### 美術方向性 (Stage 1-2 で確定)

- **絵柄参照**: HD-2D 系スクエニ作品 (Octopath Traveler / Triangle Strategy / Sea of Stars 序盤 / DQ3HD2D)
- **空気感参照**: メイドインアビス / 少女終末旅行 / けものフレンズ (テーマ・物語的方向性のみ、絵柄は参照しない)
- **構造参照**: Outer Wilds / NieR:Automata (ベール剥離アーキテクチャ)

### 音響方向性 (Stage 1-2 で確定)

- ボイスは採用しない (Silent protagonist)
- BGM はステム / MIDI 編集前提 (ユーザーが Reaper で最終調整可能)
- 環境音重視、静謐・衰退の表現

---

## Decision

### アセット種別とツール採用

| 種別 | 主軸ツール | 補助ツール | 仕上げ |
|---|---|---|---|
| **ドット絵 (キャラクター + 重要オブジェクト)** | **PixelLab** + **Retro Diffusion** | (種別特化生成) | **Aseprite** で手仕上げ |
| **3D 背景 (建物 / 環境装飾)** | **Meshy v6 (LowPoly Mode)** | **Claude MCP** (Blender 連携) | **Blender 4.5 LTS** で手仕上げ |
| **シェーダ / VFX** | **Unity URP** (HLSL カスタム) | (なし) | **Unity Editor** で調整 |
| **BGM (骨格)** | **AIVA Pro** | (なし) | **Reaper (DAW)** でステム編集・マスタリング |
| **BGM (ムード参照)** | **Suno v5.5** | (なし) | Reaper で部分採用 |
| **BGM (inpainting / 部分差替)** | **Stable Audio 2.5** | (なし) | Reaper で統合 |
| **環境音 / SFX** | **ElevenLabs SFX v2** | (なし) | Reaper で調整 |
| **UI 2D (HUD / 対話 / メニュー)** | **Claude + Unity UI** | 必要に応じて Firefly / PixelLab 補助 | **Unity Editor** で手仕上げ |
| **タイポグラフィ / アイコン** | **Adobe Firefly** / 手描き | (なし) | Photoshop / Aseprite で仕上げ |
| **ローカライズ素材 (テキスト)** | **Claude** (下訳) | **DeepL** (補助) | 人手校正 |
| **ストアビジュアル (Steam ページ等)** | **Adobe Firefly** | (なし) | Photoshop 等で仕上げ |

#### ツール採用理由 (なぜそのツールか)

- **PixelLab** は 4/8 方向スプライト量産に向くため採用。**Retro Diffusion** は静止画補助として併用
- **Meshy v6 LowPoly** は低ポリ背景の初速を優先できるため採用、**Blender 4.5 LTS** で破綻補正
- **AIVA Pro** は骨格生成に、**Suno v5.5** はムード探索に、**Stable Audio 2.5** は部分差替に役割分担
- **ElevenLabs** は Voice ではなく **SFX のみに限定**、Silent protagonist と矛盾させない
- **Adobe Firefly** は商用利用条項が明確で法的安全層、Midjourney は法的安全性懸念で除外

### 人手仕上げ前提の運用

AI 生成のみで完結せず、すべてのアセットは **人手で最終調整** する。理由:

- AI 生成のばらつきを統一感ある作品にまとめる
- 法的整合の観点で「AI 生成 + 人手仕上げ」は権利主張が明確
- VS_SCOPE §7 の「FIX エリア」は人手仕上げまで到達した品質、「VS 時点暫定完成」は AI 生成段階
- ユーザーが音楽を扱える前提 (Reaper) を活かす

#### 人手仕上げの完了条件

「人手仕上げ完了」を判定する基準:

- 色調 / 明度 / 彩度の統一 (作品全体のパレット整合)
- 画面内スケール / 比率 / シルエットの調整 (Anemora の絵柄に合致)
- 破綻部位 (手足 / UV / ノイズ / 継ぎ目 / 輪郭) の修正
- テキスト / アイコン / UI の最終校正
- 音声系は最終ミックス / ループ点 / 音量差の調整

### Silent protagonist 方針との整合

- **ボイス AI は採用しない** (ElevenLabs Voice 等)
- ElevenLabs は **SFX v2 のみ採用**、テキスト + 環境音で語る
- 対話 UI は NPC 一方的話 + 主人公感情/反応の選択肢 (SPEC §9.1)

### 法的整合 (Steam AI 開示 + 商用利用)

- 各 AI ツールの **商用利用可能性を逐次確認**、`docs/legal/asset_ledger.md` に記録
- **Steam AI 開示 2026-01-17 改訂前提**:
  - Tier 1 (player-consumed AI 生成物) は開示が必須 → アセット台帳ベースで申告内容を準備
  - Tier 2 (engineering AI tools = Claude / Codex の開発支援) は **原則として開示対象外の見込み**。ただし提出時点で Steamworks のフォーム文言を再確認し、必要なら修正する
- **Adobe Firefly** はストアビジュアルで採用 (法的安全層) — Midjourney は法的安全性懸念で除外
- **AI 生成アセットの中間ファイルは公開対象外** (`.gitignore` で `art/_intermediate/` 等を除外、ADR-0001 関連)

#### ツールごとの権利条件 (台帳で分けて記録)

- **Suno**: paid plan で生成した曲のみを商用利用対象にする。free plan 出力は採用しない
- **AIVA**: Pro plan 前提で生成し、生成時のプラン証跡を `asset_ledger.md` に残す
- **ElevenLabs**: Sound Effects Terms を **別条項として扱い、Voice 系条項とは分離して記録**
- **Stable Audio**: 商用利用条件を生成時点で確認し、プラン / API / on-prem の別を台帳に残す (2.5 の具体契約形態は要確認)
- **Adobe Firefly**: beta ラベルなし機能は商用利用可、beta でも明示禁止がなければ商用利用可 (公式 FAQ 確認)
- **PixelLab / Retro Diffusion / Meshy**: 商用利用可能性をプラン単位で確認、台帳に記録

### 月次予算配分 (PITCH §11.4 と整合)

| 項目 | 月額 |
|---|---|
| AI 画像 (PixelLab / Retro Diffusion) | ¥4,000-6,000 |
| AI 音楽 (AIVA Pro / Suno v5.5 / Stable Audio 2.5) | ¥8,000-12,000 |
| AI 3D (Meshy v6) | ¥4,000-6,000 |
| ElevenLabs SFX v2 | ¥3,000 |
| Adobe Firefly (ストアビジュアル) | ¥3,000-4,000 |
| その他 (バッファ) | ¥4,000-6,000 |
| **合計** | **¥30,000-37,000** |

Claude / Codex のサブスクは別枠 (現行枠内)。`PITCH.md` §11.4 とこのレンジで同期する。

---

## Consequences

### 利点

- **AI 主体個人開発の証跡として強い** — 制作プロセス自体が `docs/devlog/` で公開され、Zenn 技術記事として転用可能
- **1 ヶ月集中スコープに合致** — 各種別で AI 生成 → Aseprite/Blender/Reaper で仕上げる工程が個人で回せる
- **法的安全性** — Adobe Firefly + 商用利用可能 AI ツールに限定、Midjourney 等の法的グレーゾーンを排除
- **月次予算 ¥30,000-40,000 に収まる** — Claude / Codex のサブスクを除いた範囲で全アセット生成可能
- **ユーザーの音楽スキルを活用** — ステム / MIDI 編集前提の AI ツール選定 (AIVA + Reaper) でユーザー裁量が効く
- **HD-2D Tier 2 と整合** — 重い表現 (volumetric / 高ポリ) を要求しないため、AI 生成 LowPoly でも品質に達する

### 欠点 / 注意点

- **ツール数が多い** — 学習コストとワークフロー切替コストがある (8 種類のツール、Aseprite + Blender + Reaper + Unity)
- **AI 生成の一貫性** — 同一プロジェクト内でビジュアル・音響の統一感を保つにはプロンプトテンプレート確立が必須 (Stage 3 早期に整備)
- **AI ツールのサブスク管理** — 月額契約の重複・解約タイミング・無料枠活用の管理が必要
- **AI ツールの利用規約変更リスク** — 各サービスの規約は半年〜1年単位で変わる可能性、代替パスを並行確保
- **法的整合の継続監視** — Steam AI 開示・商用利用条件は変わりうる、Stage 3-5 中も継続監視必要
- **AI 中間ファイルの管理** — `art/_intermediate/` 等の生成過程ファイルは Git 除外、必要に応じて別途バックアップ

### 後続への影響

- **`docs/legal/asset_ledger.md` 起草** — **Stage 3 Day 0-1 で最低限のテンプレートを先行起草**、各アセットの生成日 / ツール / プラン / 入力素材 / 商用利用可否 / 手修正有無 / 公開可否 / Steam 開示区分を記録
- **`docs/legal/steam_ai_disclosure.md` 起草** — **Stage 3 Day 0-1 で開示区分整理表と文面草案を置く**、Stage 5 (Steam リリース判断時) に提出文面を確定
- **Aseprite + Blender + Reaper のセットアップ** — Windows 側 (デスクトップ UJPVOG2 + ノート PC TOM) で別途準備、ADR-0004 (プロジェクトディレクトリ構造) と整合
- **プロンプトテンプレート** — `docs/asset_prompts/` に各種別の生成プロンプトを保管 (Stage 3 中に整備)
- **VS_SCOPE.md §4 アセット規模** — 本 ADR の予算配分と照らして、新規作成上限と再利用前提を維持

### 後続文書との責務分担

| 文書 | 責務 |
|---|---|
| `docs/legal/asset_ledger.md` | 権利・出典・開示の **記録責務** (アセット生成のたびに追記) |
| `ADR-0004` (プロジェクトディレクトリ構造) | 生成物の **保存先 / 命名規則 / ディレクトリ責務** |
| `docs/legal/steam_ai_disclosure.md` | Steam **提出文面の責務** (Stage 5 で確定) |
| 本 ADR (0003) | パイプライン **方針の責務** (ツール採用 / 役割分担 / 法的整合方針) |

---

## Alternatives

### 候補 B: 全 AI 生成、人手仕上げなし

**実装:** AI ツールの出力をそのままゲームに組み込む

**判定:** **不採用**。
- 統一感が出ない、品質が安定しない
- 法的整合の観点で「AI のみ」は権利主張が弱い
- VS_SCOPE §7 FIX エリアの品質に達しない

### 候補 C: 全外注 (人手のみ、AI 不採用)

**実装:** ドット絵師 / 3D モデラー / 作曲家にすべて外注

**判定:** **不採用**。
- 月次予算 ¥30,000-40,000 では成立しない (個人作家への適正報酬と乖離)
- 1 ヶ月集中スケジュールと両立しない (依頼〜納品サイクル)
- 「AI 主体個人開発」という Anemora の Pitch 軸を毀損

### 候補 D: Midjourney 採用

**実装:** ストアビジュアル / コンセプトアートに Midjourney v7 採用

**判定:** **不採用**。
- 法的安全性懸念 (学習データの著作権論争、商用利用条項のグレーゾーン) — Codex レビュー (PITCH §8) で指摘済
- 代替に Adobe Firefly (法的安全性が比較的高い、商用利用条項が明確) を採用

### 候補 E: ElevenLabs Voice 採用

**実装:** 主要 NPC の音声を ElevenLabs v3 で生成

**判定:** **不採用**。
- ユーザー方針: 一発出しで自然品質が出ない、当初からボイスなし想定 (Stage 2 で確定)
- Silent protagonist との整合
- ElevenLabs は SFX v2 のみ採用

### 候補 F: Suno v5 (旧バージョン)

**実装:** Suno v5 を BGM 主軸に

**判定:** **不採用**。
- v5 は EOL、v5.5 へ世代遷移済 (Codex レビュー指摘)
- 本 ADR では v5.5 を採用

### 候補 G: 全部 Stable Audio (Suno / AIVA 不採用)

**実装:** BGM をすべて Stable Audio 2.5 で生成

**判定:** **不採用**。
- Stable Audio 単独では骨格構成 (オーケストラ + アンビエント) の制御が弱い
- AIVA Pro の方が骨格生成に強い
- 役割分担: AIVA = 骨格 / Suno = ムード / Stable Audio = inpainting で組み合わせる方が品質高い

---

## 検証ポイント (Stage 3 制作中に確認)

1. **プロンプトテンプレートの確立** — 各種別で「Anemora らしい」生成が安定するプロンプトを Stage 3 早期に確立
2. **PixelLab + Retro Diffusion の使い分け** — どちらが Anemora の絵柄により近いかを実機サンプル比較
3. **Meshy v6 LowPoly + Blender ワークフロー** — Meshy 出力を Blender で何手間入れれば HD-2D Tier 2 に乗るかの工程確認
4. **AIVA + Suno + Stable Audio の役割境界** — 実曲制作で各ツールの強み境界を確認
5. **Aseprite 仕上げの工数** — AI 生成 → 手仕上げで 1 アセットあたり何分かかるか
6. **アセット台帳の記録運用** — 生成直後 5 分以内に、生成物・プロンプト・入力素材・プラン・商用可否・開示区分を記録できるか (運用負荷の現実性確認)
7. **法的整合の継続確認** — Steam AI 開示 / 各 AI ツール規約の変更を Stage 3-5 中に定期チェック
8. **Stage 3 での確定項目** — UI / ローカライズ素材 / 台帳運用 / 音楽プラン / 3D 商用条項 / Steam 開示区分を、実制作に入る前に確定する
9. **GitHub Public への公開可否判定** — AI 中間ファイルを公開対象から外す .gitignore 運用が機能するか、商用利用条項曖昧なツールを採用する場合の代替切替基準が機能するか

検証で破綻が出たらアセット種別ごとに本 ADR を改訂、または別 ADR (Superseded) で記録する。

---

## References

### 公式 / ツール

- **PixelLab**: https://pixellab.ai
- **Retro Diffusion**: https://retrodiffusion.ai
- **Aseprite**: https://www.aseprite.org
- **Meshy v6**: https://www.meshy.ai
- **Blender 4.5 LTS**: https://www.blender.org
- **Claude MCP (Blender 連携)**: Anthropic 公式 + コミュニティ実装
- **AIVA Pro**: https://www.aiva.ai
- **Suno v5.5**: https://www.suno.com
- **Stable Audio 2.5**: https://www.stableaudio.com
- **ElevenLabs SFX v2**: https://elevenlabs.io
- **Reaper (DAW)**: https://www.reaper.fm (Linux 版あり)
- **Adobe Firefly**: https://www.adobe.com/firefly

### 法的整合

- **Steam AI 開示 (2026-01-17 改訂)**: Steamworks ドキュメント
- 各 AI ツールの商用利用条項 (`docs/legal/asset_ledger.md` で個別確認・記録)

### Anemora 内部文書

- `ADR-0001` (エンジン Unity 6.3 LTS 採用)
- `ADR-0002` (Time Frame ポータル — VFX パイプラインの一部)
- `PITCH.md` §6 (Visual Direction & Mood References)
- `PITCH.md` §8 (AI-Driven Solo Production Pipeline) ← 本 ADR の出発点
- `PITCH.md` §11 (月次予算配分)
- `SPEC.md` §7 (Art) / §8 (Sound) / §10 (Technology)
- `VS_SCOPE.md` §4 (アセット規模) / §5 (音響規模) / §7 (FIX / 暫定完成 / プレースホルダ可)
- `STAGE3_PLAN.md` §10 (開発環境の使い分け、Aseprite / Blender / Reaper の機材配置)

### 関連 ADR

- `ADR-0001`: エンジン Unity 6.3 LTS 採用 (本 ADR の前提)
- `ADR-0002`: Time Frame ポータル — VFX パイプラインの一部
- `ADR-0004` (Windows Codex 起草中): プロジェクトディレクトリ構造 — `Assets/Art/Sprites/`, `Assets/Art/Models/`, `Assets/Audio/BGM/` 等のアセット配置と本 ADR が整合
