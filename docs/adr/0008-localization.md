# ADR-0008: ローカライズ実装方針

## Status

Accepted (Stage 4 入口で実装着手、本 ADR は方針の先行確定)

## Date

2026-05-04 (Stage 3 Day 1 起草)

## Context

ADR-0007 (UI フレームワーク) で **Unity Localization Package + TextMeshPro + uGUI** の組合せを採用済。本 ADR は、その組合せにおける具体的な **String Table 設計 / Atlas 切替 / 翻訳ワークフロー / バージョニング** を確定する。

### 段階別ローカライズ計画 (`VS_SCOPE.md` §6, `PITCH.md` §11 と整合)

| 段階 | 言語 | 範囲 |
|---|---|---|
| **VS (Stage 3)** | 日本語のみ | UI / 対話 / オープニングテキスト |
| **Stage 4** | 日本語 + **英語** | 全 UI / 対話 / 設定 / メニュー |
| **Stage 5 (Steam リリース判断時)** | 日本語 + 英語 + 任意で 1-2 言語 | 同上、追加言語は需要見て判断 |

VS 着手段階では Unity Localization の API を呼ぶが、実翻訳データは日本語のみ。Stage 4 で英語データ投入、Stage 5 で必要に応じて追加。

### 関連 ADR / 文書

- ADR-0007 (UI フレームワーク): Unity Localization + TMP + uGUI 採用
- ADR-0003 (アセットパイプライン): DeepL Pro を下訳ツールとして採用
- ADR-0006 (セーブシステム): `SettingsEnvelope.display` でロケール選好を保持できるが、本 ADR で具体化
- `docs/draft/g3_npc_dialogue.md`: DialogueAsset の ScriptableObject 構造案
- `STAGE3_F_PLAN.md` §4.2: 日本語ピクセルフォントの選定

### 技術前提

- Unity 6000.3.14f1 + URP (ADR-0001)
- Unity Localization Package: 最新安定版 (Unity 6.3 系で `com.unity.localization@1.5.x` 想定)
- TextMeshPro: `com.unity.textmeshpro` (URP 同梱)
- DeepL Pro: 翻訳 API として商用契約 (ADR-0003)
- Aseprite: 日本語ピクセルフォント Atlas 生成 (ピクセルフォントの場合)

---

## Decision

### 1. Locale 構成

#### 1.1 採用 Locale

| Locale | Code | Stage | フォント Atlas |
|---|---|---|---|
| **日本語** | `ja-JP` | VS / Stage 4 / Stage 5 | 専用 Atlas (JIS 第 1 + 第 2 水準、6,000-7,000 字) |
| **英語** | `en-US` | Stage 4 / Stage 5 | 専用 Atlas (US-ASCII + 一部拡張、約 200 字) |
| (追加言語) | TBD | Stage 5 任意 | 言語ごとに別 Atlas |

#### 1.2 デフォルト Locale 解決順序 (起動時)

1. `settings.json` の `display.localeCode` (ADR-0006、Stage 4 で `DisplaySettingsSaveData` に追加予定)
2. ユーザー OS の preferred language (`Application.systemLanguage` を Locale code にマップ)
3. **Fallback: `ja-JP`** (Anemora の主言語、Stage 5 までこの方針)

#### 1.3 ランタイム切替

- ESC メニュー → 設定 → 言語選択で切替可能 (Stage 4 で実装)
- 切替時に `LocalizationSettings.SelectedLocale` 更新 + TMP Font Atlas 切替
- 進行中のテキスト表示は次のセリフ送りから新 Locale 反映 (再起動不要)

---

### 2. String Table 設計

#### 2.1 命名規則

**Asset 形式**: `Assets/UI/Localization/Anemora_Strings.asset` (シングル StringTableCollection、Locale ごとに sub-Asset)

**Key 命名**: `domain.subdomain.purpose_id`

| ドメイン | 用途 | 例 |
|---|---|---|
| `ui.menu.*` | タイトル / ESC メニュー / 設定 | `ui.menu.title.start`, `ui.menu.settings.locale_label` |
| `ui.hud.*` | HUD / ヒント | `ui.hud.hint.use_timewriter` |
| `ui.dialog.*` | 対話 UI フレームワーク (NPC セリフは別、ここは UI 部品) | `ui.dialog.choice.confirm`, `ui.dialog.system.next_page` |
| `ui.symbol.*` | 時の窓シンボル選択 UI | `ui.symbol.red.label`, `ui.symbol.white.disabled_tooltip` |
| `dialogue.npc.<id>.*` | NPC 対話、各 NPC ごと | `dialogue.npc.resident_a.initial_01`, `dialogue.npc.resident_a.post_take_book_01` |
| `narration.opening.*` | オープニング (D-3 / D-7 / D-6 / ドア前 など) | `narration.opening.d3_text`, `narration.opening.timewriter_discover` |
| `system.error.*` | エラー / 警告 / セーブ失敗通知 | `system.error.save_failed`, `system.error.load_corrupted` |
| `system.tutorial.*` | チュートリアル | `system.tutorial.first_portal` |

#### 2.2 Key 制約

- 全小文字、`_` で区切る (camelCase / PascalCase 不採用)
- ドメイン階層は `.` で区切る
- 数値 suffix `_NN` は 2 桁ゼロ埋め (`_01`, `_02`)
- ドメイン名は **不変** (Stage 4-5 で改名すると翻訳メタデータが壊れる)

#### 2.3 Plural / Gender / Conditional

VS / Stage 4 では使わない (英語と日本語で plural は意識せず単純訳)。Stage 5 で必要言語が出たら Unity Localization の Smart Format / SmartString で対応。

---

### 3. TextMeshPro Font Atlas 戦略

#### 3.1 日本語

ADR-0007 §TextMeshPro フォント戦略 を引き継ぐ:

- **事前生成 Atlas を基本**、Dynamic Font は使わない
- JIS 第 1 + 第 2 水準で 6,000-7,000 字
- ピクセルフォント (商用利用可、Aseprite 互換) を選定 → `Assets/UI/Localization/Fonts/Anemora_JP.asset` (TMP Font Asset)
- Atlas 容量: 4096x4096 SDF1 想定、サイズ計測は Stage 3 中に確定

**フォント候補** (商用可性を `asset_ledger.md` §1.2 で確認):

- 美咲フォント (8x8 / 8x12、商用可、レトロ系)
- M+ FONTS (商用可、汎用)
- 源真ゴシック / 源ノ角ゴシック (商用可、オールラウンド)

ピクセル絵 UI と合うよう **8x8 〜 12x12 程度のビットマップ系** を優先、ただし HD-2D 絵柄に合えば SDF も可。

#### 3.2 英語 (Stage 4)

- **別 Atlas**: `Assets/UI/Localization/Fonts/Anemora_EN.asset`
- 日本語 Atlas と分離 (容量効率 + フォント性質の違い)
- 候補: Pixel Square (Pixelmix 系) / Press Start 2P / VCR OSD Mono (商用可性確認)

#### 3.3 Atlas 切替

- Locale 変更時に `TMP_Settings` の Default Font を Locale ごとに差し替え
- または `LocalizedAssetTable` で TMP Font Asset を Locale 別に配信 (Unity Localization 標準機能)
- 切替遅延を避けるため、起動時に **両 Atlas を preload** (Stage 4 で英語追加時から)

#### 3.4 Fallback

- 日本語 Atlas に英字を含めず、英字は英語 Atlas にフォールバック (`Fallback Font Asset` に `Anemora_EN` を設定)
- 同様に英語 Atlas には日本語を含めず、フォールバック設定で対称的に解決

---

### 4. 翻訳ワークフロー (DeepL Pro + 人手校正)

#### 4.1 入力 → 出力フロー

```
[日本語原文 (Stage 3 で確定)]
   ↓
[DeepL Pro API or 手動コピペで下訳 → 英語]
   ↓
[人手校正 (ユーザー or 言語確認できる人)]
   ↓
[TMP / Localization 専用語彙に整形 (例: "時の筆" = "Timewriter")]
   ↓
[String Table の en-US sub-Asset に投入]
   ↓
[実機確認: 画面でレイアウト破綻なし、フォント Atlas で表示可能]
```

#### 4.2 専用語彙集 (用語集)

`docs/legal/asset_ledger.md` とは別に `docs/localization/glossary.md` を作成 (Stage 4 入口で起草):

- 主人公固有名詞 (確定後)
- 時の筆 = Timewriter (仮)
- 時の窓 = Time Frame
- 観測者 / ベール / 真層 などの世界観用語
- ゾーン名 (Stage 3 で確定後)
- NPC 名 (Stage 3 で確定後)

専用語彙は **DeepL の自動翻訳より優先**。プロジェクトの一貫性を確保。

#### 4.3 翻訳粒度

- **行単位**: String Table の 1 key = 1 行 (1 段落程度) を翻訳
- **改行・改ページ位置**: 言語ごとに別 (英語は日本語より長くなりがち、UI レイアウトに影響)
- **コンテキスト注釈**: Unity Localization の `Comment` フィールドに「このセリフを話す NPC / シチュエーション」を記述、翻訳者の判断材料

#### 4.4 バージョニング

- 日本語原文を変更した場合、対応する翻訳に「要再校正」フラグを立てる
- Unity Localization の `Translation Status` を `Up to Date` / `Out of Date` / `Missing` で管理
- リリース前に全 key が `Up to Date` (主言語以外、未翻訳許容なら `Missing` でビルド可) であることを確認

---

### 5. ScriptableObject Dialogues との接続

#### 5.1 asmdef 境界 (重要、Codex E1 review 2026-05-05 反映)

`Anemora.Data` (asmdef、`Assets/Scripts/Data/`) は **engine-free POCO 専用** に保つ (ADR-0004 + ADR-0006 で確定済)。Unity Localization の `LocalizedString` は engine-dependent な型 (`UnityEngine.Localization` 必須) なので、`Anemora.Data` には載せない。

代わりに **2 層構造** を採る:

| 層 | asmdef | 役割 |
|---|---|---|
| Pure data layer | `Anemora.Data` | POCO のみ。Dialogue の text は **string key** で持つ (例: `"dialogue.npc.resident_a.initial_01"`) |
| Runtime / UI 層 | 別 asmdef (例: `Anemora.UI` or 新設 `Anemora.Game`) | `LocalizedString` で key を解決、TMP で描画 |

#### 5.2 構造接続案 (改訂)

```csharp
// === Anemora.Data (engine-free, POCO のみ) ===
namespace Anemora.Data
{
    public sealed class DialogueAssetData  // POCO version、ScriptableObject ではない
    {
        public string npcId;
        public List<DialogueVariantData> variants;
    }

    public sealed class DialogueVariantData
    {
        public string variantId;
        public List<DialogueTurnData> turns;
        public List<string> requiredFlags;
        public List<string> excludedFlags;
    }

    public sealed class DialogueTurnData
    {
        public string speakerId;
        public string textKey;          // ← String Table の key (例: "dialogue.npc.resident_a.initial_01")
        public List<DialogueChoiceData> choices;
    }

    public sealed class DialogueChoiceData
    {
        public string emotion;
        public string labelKey;         // ← 選択肢ラベル key
        public string nextTurnId;
    }
}

// === Anemora.UI or Anemora.Game (Unity-dependent) ===
namespace Anemora.Game.Dialogue
{
    [CreateAssetMenu(menuName = "Anemora/Dialogue")]
    public class DialogueAsset : ScriptableObject
    {
        public string npcId;
        public List<DialogueVariantSO> variants;
    }

    [Serializable]
    public class DialogueVariantSO
    {
        public string variantId;
        public List<DialogueTurnSO> turns;
        public List<string> requiredFlags;
        public List<string> excludedFlags;
    }

    [Serializable]
    public class DialogueTurnSO
    {
        public string speakerId;
        public LocalizedString text;            // ← Unity Localization で解決
        public List<DialogueChoiceSO> choices;
    }

    [Serializable]
    public class DialogueChoiceSO
    {
        public string emotion;
        public LocalizedString label;           // ← 同上
        public string nextTurnId;
    }
}
```

#### 5.3 使い分け

- **編集 / Inspector**: `DialogueAsset` (ScriptableObject、`LocalizedString` で string key を Inspector で選択)
- **永続化 (セーブデータ等)**: `DialogueAssetData` 経由で string key のみを記録 (`Anemora.Data` で扱う)
- **ランタイム描画**: `DialogueAsset` の `LocalizedString.GetLocalizedString()` を呼んで現在 Locale に応じたテキストを取得

DialogueAsset と DialogueAssetData は **同じ string key を共有** することで両者間で参照可能。`LocalizedString` の `tableReference` + `tableEntryReference` が `string key` 1 つに圧縮される。

#### 5.4 命名整合 (変更なし)

- DialogueAsset の `variantId` (例: "initial", "post_take_book_family_001") は String Table key suffix と一致させる:
  - String Table key: `dialogue.npc.resident_a.initial_01`, `..._02` (Turn 番号 suffix)
  - String Table key: `dialogue.npc.resident_a.post_take_book_family_001_01`, `..._02`

#### 5.2 命名整合

- DialogueAsset の `variantId` (例: "initial", "post_take_book_family_001") は String Table key suffix と一致させる:
  - DialogueAsset key: `dialogue.npc.resident_a.initial_01`, `..._02` (Turn 番号 suffix)
  - DialogueAsset key: `dialogue.npc.resident_a.post_take_book_family_001_01`, `..._02`

#### 5.3 開発フロー

1. **Stage 3 中**: 日本語原文を `docs/draft/g3_npc_dialogue.md` で起草
2. **Stage 3 完了時**: 確定文言を String Table (`Anemora_Strings.asset` の ja-JP sub-Asset) に投入
3. **Stage 4 入口**: DeepL Pro で英語下訳 → 人手校正 → en-US sub-Asset に投入
4. **継続**: 新 NPC / 新セリフ追加時に同フロー

---

### 6. Editor Workflow (asset 管理)

#### 6.1 Localization Editor 拡張 (任意)

Unity Localization 標準の Editor で十分だが、量が増えたら以下の補助を `Assets/Editor/Localization/` に追加検討:

- **CSV import / export**: 外部 (DeepL の Web UI や翻訳者) との橋渡し
- **未翻訳 key スキャン**: ビルド前に全 key の翻訳状態を一覧
- **Atlas 容量チェック**: 全 String Table の文字を集計して TMP Atlas の必要文字を抽出

これらは ADR-0008 の Decision には含めず、Stage 4 で必要に応じて追加。

#### 6.2 ロケール追加時の手順

Stage 5 で言語を 1 つ追加する場合:

1. `LocalizationSettings` で新 Locale を追加 (`Assets/UI/Localization/Locales/`)
2. String Table に該当 sub-Asset を生成
3. TMP Font Atlas を新言語用に作成 (`Assets/UI/Localization/Fonts/Anemora_<lang>.asset`)
4. 既存の全 key を新 Locale に翻訳投入 (DeepL Pro + 人手校正)
5. Locale 切替 UI に新言語を追加

---

### 7. 検証ポイント (Stage 4 入口で実機確認)

| ID | 項目 | 必須 |
|---|---|---|
| L1 | 日本語 Atlas で全 key が表示される (Stage 3 段階で先行確認) | ✓ |
| L2 | 英語 Atlas で全 key が表示される (Stage 4) | ✓ |
| L3 | 起動時の Locale 解決順序が機能する (`settings.json` → OS → fallback ja-JP) | ✓ |
| L4 | ESC メニューから Locale を切替えると、表示中のテキストが次回更新時に切り替わる | ✓ |
| L5 | 日本語 → 英語切替時に UI レイアウトが破綻しない (英語の方が長くなる対応) | ✓ |
| L6 | TMP Atlas Fallback で日本語に英字が混在しても表示される | ✓ |
| L7 | DeepL Pro + 人手校正フローで 1 NPC の対話を 1 時間以内に完成できる (運用負荷確認) | △ |
| L8 | DialogueAsset の `LocalizedString` 解決が runtime で動く | ✓ |
| L9 | バージョニング (原文変更 → 翻訳 Out of Date 検出) が機能 | △ |

`✓` = Stage 4 入口で必須、`△` = 観察項目。

---

## Consequences

### 利点

- **Unity 標準で完結** — Localization Package + TMP の組合せは Unity 公式サポート、コミュニティ事例も豊富
- **Stage 4-5 の言語追加が容易** — String Table + Atlas 追加でスケール
- **DeepL Pro で初速確保** — 個人開発で英語翻訳を成立させる現実解
- **DialogueAsset と整合** — ScriptableObject ベースのセリフ管理がローカライズと両立
- **VS では日本語のみで簡素** — Stage 3 中に Localization API を準備しつつ、翻訳負荷は Stage 4 まで延期

### 欠点 / 注意点

- **TMP Atlas 容量**: 日本語 6,000-7,000 字で 4096x4096 SDF1 想定、メモリ占有が大きい (ノート PC TOM の VRAM 2GB で動作確認必須)
- **Fallback 設計**: 日本語 / 英語 Atlas のフォールバック関係を双方向で設定する必要、設定漏れで「□」表示が出るリスク
- **DeepL の機械訳と用語集の整合**: 自動翻訳が用語集を尊重しないため、人手校正の負荷
- **専用語彙の確定タイミング**: 「時の筆 = Timewriter」など世界観固有名詞は Stage 4 入口前に英訳を確定させないと、後から全 String Table を更新する負荷
- **UI レイアウトの言語別調整**: 英語 / 日本語で文字長が違うため、UI Prefab で動的レイアウト (Content Size Fitter / Layout Group) が必須

### 後続への影響

- **ADR-0007 (UI フレームワーク)**: 本 ADR で TMP Atlas 戦略を具体化、ADR-0007 §TextMeshPro フォント戦略を補強
- **ADR-0006 (セーブシステム)**: `DisplaySettingsSaveData` に `localeCode` フィールドを Stage 4 で追加 (現在の SettingsEnvelope は未含有)
- **ADR-0003 (アセットパイプライン)**: DeepL Pro の利用方針を本 ADR で具体化、`asset_ledger.md` §1.2 行も更新
- **`docs/draft/g3_npc_dialogue.md`**: §5 ScriptableObject 構造案を本 ADR §5 で `LocalizedString` ベースに進化
- **Stage 5 Steam 公開**: 提供言語数が Steam ストアページ表示に影響、開示文 (`steam_ai_disclosure.md` §2 ローカライズ素材行) と整合

---

## Alternatives

### 候補 B: I2 Localization (アセットストア有償プラグイン)

**実装:** I2 Localization パッケージで全ローカライズ管理

**利点:**
- 多機能 (CSV / Google Sheets 連携 / Term Source 管理 / etc.)
- 日本語コミュニティ事例多数

**欠点:**
- **有料** (asset store ¥4,000-5,000)、本 ADR 予算 (`PITCH.md` §11) の範囲外
- **依存度が高い**、Stage 5 以降のメンテで I2 仕様に縛られる
- Unity 公式サポート外

**判定:** **不採用**。Unity Localization で十分、無料 + 公式サポート。

### 候補 C: 自作ローカライズ (CSV + Dictionary 直接管理)

**実装:** CSV ファイルを Resources に置き、起動時に Dictionary に load

**利点:**
- 完全に自由
- 依存ゼロ

**欠点:**
- **TMP Font Atlas 切替を自作必須** (Unity Localization の standard 機能を再実装)
- **Editor 統合が弱い** (LocalizedString の Inspector 統合がない)
- **メンテ負荷が個人開発スコープを超える**

**判定:** **不採用**。Unity 標準採用が正解。

### 候補 D: Crowdin / Lokalise などのクラウド SaaS 連携

**実装:** Unity Localization と Crowdin の連携で翻訳をクラウド管理

**利点:**
- 翻訳者が複数いる場合に強力
- バージョン管理 / 翻訳メモリが組込み

**欠点:**
- **個人開発で翻訳者は実質ユーザー 1 人**、SaaS 利用は過剰
- 月額契約が予算 (`PITCH.md` §11.4) を圧迫
- Stage 5 で言語数が 3+ になった場合のみ再評価候補

**判定:** **不採用 (将来候補)**。Stage 5 で多言語要求が出たら再考。

### 候補 E: 機械訳のみ、人手校正なし

**実装:** DeepL Pro の自動翻訳をそのまま投入

**利点:**
- 工数最小
- 言語追加が DeepL 側でカバー可能な範囲なら一瞬

**欠点:**
- **専用語彙が崩れる** (時の筆 / 観測者などの世界観用語が定型訳されない)
- **AI 主体個人開発という Pitch 軸との整合**: 人手校正が「AI 生成 + 人手仕上げ」の主張を裏打ちする
- **Steam AI 開示**: 機械訳のみだと Steam Tier 1 (player-consumed) 開示の「人手仕上げ」要件が弱まる

**判定:** **不採用**。下訳は DeepL、最終投入は人手校正必須。

---

## References

### 公式

- [Unity Localization Package 公式](https://docs.unity3d.com/Packages/com.unity.localization@latest)
- [Unity TextMeshPro 公式](https://docs.unity3d.com/Packages/com.unity.textmeshpro@latest)
- [Unity LocalizedString API](https://docs.unity3d.com/Packages/com.unity.localization@latest/api/UnityEngine.Localization.LocalizedString.html)
- [DeepL API 公式](https://www.deepl.com/docs-api)

### コミュニティ事例 (実装着手時に参照)

- Unity Localization + TMP 日本語 Atlas 連携チュートリアル
- DeepL API + Unity Editor 拡張事例
- HD-2D / RPG ローカライズ事例 (Octopath Traveler の多言語対応分析記事があれば)

### Anemora 内部文書

- `ADR-0001` (Unity 6.3 LTS + URP)
- `ADR-0003` (アセットパイプライン、DeepL Pro 採用根拠)
- `ADR-0006` (セーブシステム、`DisplaySettingsSaveData` 連携)
- `ADR-0007` (UI フレームワーク、Unity Localization + TMP 採用根拠)
- `STAGE3_F_PLAN.md` §4.2 (日本語ピクセルフォント選定)
- `docs/draft/g3_npc_dialogue.md` §5 (DialogueAsset 構造案)
- `VS_SCOPE.md` §6 (UI 規模)
- `PITCH.md` §11 (月次予算)

### 関連 ADR

- `ADR-0003`: アセットパイプライン — DeepL Pro 採用、本 ADR の翻訳ツール選定根拠
- `ADR-0006`: セーブシステム — `DisplaySettingsSaveData.localeCode` の Stage 4 追加 (本 ADR §1.2)
- `ADR-0007`: UI フレームワーク — Unity Localization + TMP + uGUI 採用、本 ADR で具体化

---

## 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v0.1 | 2026-05-04 | Stage 3 Day 1 起草。Locale 構成 / String Table 命名規則 / TMP Atlas 戦略 / DeepL ワークフロー / DialogueAsset 接続 / 検証ポイント / Alternatives 5 件 |
| v0.2 | 2026-05-05 | Codex E1 review 反映: §5 asmdef 境界を明確化。`Anemora.Data` は POCO + string key のみ、`LocalizedString` は別 asmdef (`Anemora.Game` or `Anemora.UI`) の DialogueAsset SO に分離。2 層構造で engine-free 性を維持 |
