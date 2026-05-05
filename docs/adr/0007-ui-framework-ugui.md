# ADR-0007: UI フレームワークに uGUI を採用 (UI Toolkit は将来選択肢)

## Status

Accepted

## Date

2026-05-04 (Stage 3 Day 0)

## Context

Anemora の VS 制作 (Stage 3) と Stage 4-5 の本実装で、UI フレームワークを Unity 6.3 LTS の選択肢から決定する必要がある。Unity の主要選択肢は:

- **uGUI (UnityEngine.UI)** — Unity 標準、Canvas / RectTransform / Button / Image ベース、ランタイム UI に特化
- **UI Toolkit (UI Builder + UXML + USS)** — 2021 LTS 以降の推奨方向、Web 風スタイル、Editor + ランタイム両対応

### Anemora 固有の UI 要件

| 要素 | 内容 | VS 必要度 |
|---|---|---|
| **タイトル画面** | 最小限、プレースホルダ可 (Stage 4 で完成) | 低 |
| **HUD (最小限ヒント表示)** | テキストのみ、HP/MP/ミニマップなし | 中 (VS 時点暫定完成) |
| **時の窓シンボル選択 UI** | ホイール状の 3 シンボル (赤=選択可 / 白・青=グレーアウト) | **高 (コア体験)** |
| **対話 UI (Silent protagonist 対応)** | NPC 一方的話 + 主人公感情/反応の選択肢 | 高 (VS 物語入口) |
| **インベントリ / 進行ログ** | プレースホルダ可、Stage 4 で本実装 | 低 |
| **ESC メニュー** | 設定 / セーブ / タイトルへ戻る (最小機能) | 中 |
| **ローカライズ** | 日本語 (VS) → 英語 (Stage 4-5) | 中 |
| **アクセシビリティ** | UI 拡大 / 字幕サイズ / コントラスト切替 | 中 |

### 制約

- Unity 6.3 LTS + URP (ADR-0001)
- HD-2D Tier 2 簡素版 (動的影 + 単一方向光、レトロ調 UI と整合)
- Silent protagonist (Undertale 型、ボイスなし)
- 1 ヶ月集中開発 (AI 開発支援前提)
- VS_SCOPE.md §6 で UI 完成度は「VS 時点暫定完成」(Stage 4 で小修正許容)
- ローカライズ対応 (Stage 4-5 で英語追加)

### 美術方向性

- HD-2D 系スクエニ作品 (Octopath / Triangle Strategy / Sea of Stars 序盤 / DQ3HD2D) の **テクスチャベース UI**
- ピクセル絵 + 軽量装飾、現代風スマートフォン UI ではなく古典的ゲーム UI 寄り

---

## Decision

### 採用方針: **uGUI を本線として採用、UI Toolkit は将来選択肢として保持**

#### 理由

1. **HD-2D Tier 2 のテクスチャベース UI と相性が良い** — uGUI は Canvas / Image / RawImage で 9-slice / Sprite を直接扱え、ピクセル絵 + 装飾の UI に直感的
2. **AI 開発支援 (Claude / Codex) の知識ベースが圧倒的に厚い** — uGUI は Unity の長年の標準、コード生成・トラブルシュート支援が安定
3. **コミュニティ事例 / アセット / チュートリアルが豊富** — HD-2D 風 UI / Undertale 型対話 UI / シンボル選択ホイールなど、uGUI ベースの実装例が多数
4. **TextMeshPro が標準採用** — 日本語表示が高品質、Stage 4-5 ローカライズ対応で英語フォントも安定
5. **Unity Animator で UI アニメ可能** — シンボル選択ホイールの回転、対話ボックスのフェードイン等が容易
6. **Unity Localization Package と uGUI の連携が安定** — Stage 4-5 の多言語化で実績ある組合せ
7. **Pixel Perfect 対応** — Canvas Scaler + Pixel Perfect Camera で HD-2D の整数解像度を保てる

#### UI Toolkit の位置づけ (2026 年時点)

- UI Toolkit は **Editor 拡張と一部 runtime UI で有力** だが、Anemora の VS 期間に必要な runtime UI (対話 / シンボル選択 / HUD / メニュー) では uGUI の実績と事例密度が依然優位
- Stage 4 以降に runtime UI 要件が増えた場合 (リスト UI / データバインディング中心の画面) のみ再評価
- **本ADRでの不採用範囲を明示**:
  - **Runtime UI は VS / Stage 4-5 を通じて uGUI を本線**
  - **UI Toolkit は原則 Editor 拡張用途に限定**
  - Runtime UI への採用は別 ADR で再評価する (本 ADR の Superseded として記録)

### UI 要素別の実装方針

| UI 要素 | 採用 | Canvas モード | 詳細 |
|---|---|---|---|
| タイトル画面 | uGUI | Screen Space - Overlay | Image + Button、Stage 4 で完成 |
| HUD (ヒント) | uGUI | Screen Space - Overlay | TextMeshPro、軽量 |
| **時の窓シンボル選択** | uGUI | **Screen Space - Camera (固定)** | ホイール状の 3 シンボル、カメラ追従前提、シェーダで選択可/不可を視覚的に区別 (赤=活性、白/青=グレーアウト) |
| **対話 UI** | uGUI | **Screen Space - Overlay (固定)** | TextMeshPro、NPC セリフ + 主人公感情アイコン + 反応選択肢、テキスト送りは **既定で即時表示**、必要時のみ一文字送りを補助オプション |
| **入力ナビゲーション** | uGUI + EventSystem | — | キーボード / ゲームパッドでのフォーカス遷移、決定、キャンセルを共通化 (VS で必須) |
| インベントリ | uGUI | Screen Space - Overlay | プレースホルダ、Stage 4 で本実装 |
| 進行ログ | uGUI | Screen Space - Overlay | プレースホルダ、Stage 4 で本実装 |
| ESC メニュー | uGUI | Screen Space - Overlay | Button、最小機能 (設定 / セーブ / タイトル) |
| ローカライズ | Unity Localization Package + uGUI | — | 日本語 (VS) → 英語 (Stage 4-5)、TextMeshPro Asset でフォント切替 |
| **アクセシビリティ** | uGUI 設定画面 + Canvas Scaler + 設定データ | — | UI 拡大 / 字幕サイズ / コントラスト切替を **設定項目として保持**、**起動時に適用**、**永続化方針は ADR-0006 (セーブシステム) で詳細化** |

### 命名規則 / ディレクトリ構造

ADR-0004 (プロジェクトディレクトリ構造) で確定するが、本 ADR の前提として:

- `Assets/UI/Prefabs/` — UI Prefab (HUD / Dialog / SymbolWheel / Menu)
- `Assets/UI/Sprites/` — UI Sprite アセット (9-slice / アイコン)
- `Assets/UI/Scripts/` — UI 制御スクリプト (DialogController / SymbolSelector / etc.)
- `Assets/UI/Localization/` — Unity Localization 用 String Tables

### 共通 UI スタイル方針

HD-2D Tier 2 と整合する UI スタイル:

- **9-slice ベースの装飾枠** (テクスチャ繰り返しでサイズ変動に対応)
- **TextMeshPro + ピクセルフォント** (Stage 3 でフォント選定済みとして運用)
- **カラーパレット** は Anemora 全体パレットに整合 (ADR-0003 アセットパイプラインと連動)
- **アニメーション** は控えめ (Tween 系を最小限、フェードイン/アウト程度)
- **レイアウト** は中央寄せ + 余白を多めに (空気感優先、密度を上げない)、**最小可読サイズを下回らない範囲で UI 拡大率の上限を定義** する

### TextMeshPro フォント戦略

Stage 3 では以下を運用方針として扱う。フォント資産の最終採用は `docs/STAGE3_TBD_RESOLUTION.md` で tracking する:

- **日本語**: **事前生成 Atlas を基本** とする (Dynamic Font は使わない)
  - JIS 第 1 水準 + 第 2 水準で 6,000-7,000 字、Atlas 容量を計測
  - 日本語ピクセルフォントは **商用利用可** のものを選定 (フリーフォント / 商用ライセンスフォント)
  - VS 時点では **固定フォント資産** として運用
- **英語** (Stage 4-5 ローカライズ): **別 Atlas を用意** するか、**必要最小限の fallback を併用**
- **フォント候補と商用可否は `docs/legal/asset_ledger.md` に記録**

---

## Consequences

### 利点

- **VS 制作工数を最小化** — uGUI の知識ベース + AI 支援で対話 UI / シンボル選択 UI / HUD を Day 5-8 で実装可能
- **HD-2D Tier 2 と整合** — テクスチャベース UI の自然な実装、レトロ調を出しやすい
- **TextMeshPro で日本語表示が安定** — 字間 / 行間 / カーニングが高品質
- **Unity Localization で多言語化が容易** — Stage 4-5 で英語追加、フォント切替も標準対応
- **Animator で UI アニメ実装** — Time Frame シンボル選択の回転や対話ボックスのフェードを Animator Controller で記述
- **Pixel Perfect 対応** — Canvas Scaler + Pixel Perfect Camera で整数解像度を保つ
- **将来的な拡張余地** — UI Toolkit を Editor 拡張で採用、ランタイムは Stage 4 以降に部分採用可能

### 欠点 / 注意点

- **uGUI は Canvas Hierarchy の管理が煩雑になりがち** — 大量の UI を配置する際にレンダリングコスト増、対策として Canvas を分割 (Static / Dynamic で別 Canvas)
- **UI Toolkit は Editor 拡張と一部 runtime UI で有力だが、Anemora の VS 期間に必要な runtime UI では uGUI の実績と事例密度が依然優位** — Stage 4 以降に runtime UI 要件が増えた場合のみ再評価する。再評価結果は別 ADR で記録
- **Pixel Perfect 設定の細かい調整必要** — HD-2D の整数解像度を保つには Canvas Scaler / Pixel Perfect Camera / Sprite Atlas の設定を整合させる
- **TextMeshPro 日本語フォント生成のコスト** — JIS 第 1 水準 + 第 2 水準で 6,000-7,000 文字、**事前生成 Atlas を採用** (Dynamic Font は不採用)、商用利用可フォントを選定済として運用
- **Animator の過剰使用は避ける** — UI アニメーションを多用すると Animator Controller が複雑化、軽い Tween は DOTween / プログラマティック制御も検討

### 後続への影響

- **VS_SCOPE.md §6 UI 規模** — uGUI 前提で構築、対話 UI / シンボル選択 UI は完成品質方針で詰める
- **ADR-0004 (プロジェクトディレクトリ構造)** — `Assets/UI/` の標準サブディレクトリと整合
- **ADR-0008 (ローカライズ実装)** — Unity Localization Package + TextMeshPro + uGUI の連携方針を詳細化。特に **String Table の命名規則、Locale 切替時の TMP Font Asset 切替、翻訳差分の運用責務** を定義する
- **`docs/legal/asset_ledger.md`** — UI 2D アセット (アイコン / 装飾枠) も AI 生成の場合は記録対象

---

## Alternatives

### 候補 B: UI Toolkit を本線採用

**実装:** UXML + USS でランタイム UI 全部を構築

**利点:**
- Unity の長期推奨方向と整合、将来の保守コスト低
- Web 風スタイルで CSS 知識を流用可能
- データバインディング機能 (MVVM 系) が組込み

**欠点:**
- ランタイム UI の事例 / アセット / コミュニティ事例が uGUI より少ない
- HD-2D 風のテクスチャベース UI の実装事例が乏しい
- AI 開発支援 (Claude / Codex) の知識ベースが uGUI より薄い
- Pixel Perfect 対応 (整数解像度 + ピクセル絵) で工夫が必要
- 1 ヶ月集中スケジュールの初学コストが高い

**判定:** **不採用 (将来選択肢として保持)**。長期的には UI Toolkit に寄せる選択肢があるが、VS では uGUI で完結する判断。

### 候補 C: ハイブリッド (uGUI + UI Toolkit 併用)

**実装:** ランタイム UI は uGUI、Editor 拡張 UI は UI Toolkit

**利点:**
- 各フレームワークの強みを活用
- Editor ツール作成時に UI Toolkit の生産性を享受

**判定:** **将来採用候補**。VS では uGUI 単独で十分。Stage 4 以降に Editor 拡張ツール (アセット管理 / レベルデザイナー等) を作る際に部分採用検討。

### 候補 D: IMGUI (即時モード GUI)

**実装:** OnGUI() でデバッグ UI 主体

**判定:** **不採用**。製品 UI には不向き、Anemora の VS には合わない (デバッグ用途のみ)。

### 候補 E: サードパーティ UI フレームワーク (NoesisGUI / Coherent UI 等)

**判定:** **不採用**。コスト + ライセンス + 学習コストが個人開発に過剰、Unity 標準で十分。

---

## 検証ポイント (Stage 3 制作中に確認)

1. **TextMeshPro 日本語フォント運用** — 日本語ピクセルフォント候補の **商用可否、fallback 構成、Atlas 容量** を `asset_ledger.md` に記録、選定済資産で実装
2. **時の窓シンボル選択 UI のホイール実装** — Canvas (Screen Space - Camera) でホイール表示、シェーダで選択可/不可の視覚区別、Animator で回転アニメ
3. **対話 UI の Undertale 型実装** — NPC セリフ + 主人公感情アイコン + 反応選択肢、即時表示既定 + 一文字送り補助オプション、**会話ログ保持の有無とテキスト送り速度の UX**
4. **Pixel Perfect 設定** — Canvas Scaler + Pixel Perfect Camera + Sprite Atlas の整合確認、整数解像度を保つ
5. **アクセシビリティ実装** — UI 拡大 / 字幕サイズ / コントラスト切替が VS 時点で機能するか、**設定データの永続化方針も含めて確認** (実装可否を Stage 3 で確定、VS_SCOPE §6)
6. **入力ナビゲーション (ゲームパッド対応)** — uGUI + EventSystem でフォーカス遷移、決定、キャンセル動作が **キーボード / ゲームパッド** で破綻なく動作
7. **UI スケーリング基準** — **解像度別の見え方、最小文字サイズ、UI 拡大率上限** を実機で検証
8. **Unity Localization Package との連携** — Stage 4-5 ローカライズに向けて、VS 段階から String Table 設計を準備
9. **Canvas 分割の最適化** — 大量 UI 配置時のレンダリングコスト計測、Static / Dynamic Canvas 分割

検証で破綻が出たら本 ADR を改訂、または別 ADR (Superseded) で記録する。

---

## 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v1.0 | 2026-05-04 | 初版。uGUI を本線採用し、UI Toolkit を将来選択肢として保持 |
| v1.1 | 2026-05-05 | ADR review pass: ADR-0004 / ADR-0008 cross-reference と TMP font finality 表現を現状に合わせて更新 |

---

## References

### 公式

- [Unity uGUI 公式 (UnityEngine.UI)](https://docs.unity3d.com/Manual/com.unity.ugui.html)
- [TextMeshPro 公式](https://docs.unity3d.com/Packages/com.unity.textmeshpro@latest)
- [Unity UI Toolkit 公式](https://docs.unity3d.com/Manual/UIElements.html) (将来選択肢として参照)
- [Unity Localization Package 公式](https://docs.unity3d.com/Packages/com.unity.localization@latest)
- [Unity Pixel Perfect Camera (URP)](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest/manual/2d-pixelperfect.html)

### コミュニティ事例 (実装着手時に参照)

- Undertale 型対話 UI 実装事例 (Code Monkey / Brackeys 系)
- HD-2D 風 UI のピクセルフォント選定例 (Octopath Traveler の UI 分析記事)
- 時の窓ポータル UI のシンボルホイール実装事例
- TextMeshPro 日本語フォント生成チュートリアル

### Anemora 内部文書

- `ADR-0001` (エンジン Unity 6.3 LTS 採用)
- `ADR-0002` (Time Frame ポータル — シンボル選択 UI と連携)
- `ADR-0003` (アセットパイプライン — UI 2D / タイポグラフィ / アイコン)
- `SPEC.md` §9 (UI/UX、対話 UI に Silent protagonist 反映済)
- `VS_SCOPE.md` §6 (UI 規模、VS 時点暫定完成方針)
- `docs/STAGE3_TBD_RESOLUTION.md` §1.1 (Silent protagonist 確定事項)

### 関連 ADR

- `ADR-0001`: エンジン Unity 6.3 LTS 採用 (本 ADR の前提)
- `ADR-0002`: Time Frame ポータル — 本 ADR のシンボル選択 UI が連携
- `ADR-0003`: アセットパイプライン — UI 素材 (アイコン / 装飾枠) の生成方針
- `ADR-0004`: プロジェクトディレクトリ構造 — `Assets/UI/` 配置と本 ADR が整合
- `ADR-0005`: 時間管理 / シーン切替 — 時の窓シンボル選択 UI の状態遷移と連携
- `ADR-0008`: ローカライズ実装 — Unity Localization + TextMeshPro + uGUI の連携詳細
