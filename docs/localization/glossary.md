# Anemora 用語集 (Glossary, ja-JP / en-US)

> ADR-0008 §4.2 で約束した世界観固有名詞 + UI 用語 + ゾーン名 + NPC 名の日英対応集。
> DeepL Pro の機械訳より優先される一貫性ソース。Stage 4 入口前に英訳を確定し、それ以降は変更しない (`tableEntryReference` 安定性のため)。

> **Status (2026-05-04)**: v0 起草。世界観コア用語 + UI 系を確定、固有名詞は Stage 3 進行中の確定タイミングで追記。

---

## 1. 用語集の運用

### 1.1 各列の定義

| 列 | 内容 |
|---|---|
| **ja** | 日本語表記 (Stage 3 確定済の文言、世界観 / UI で使う) |
| **en** | 英語訳 (本書で確定、機械訳より優先) |
| **status** | `confirmed` (確定) / `provisional` (仮、変更余地あり) / `tbd` (Stage 3-4 で確定予定) |
| **scope** | `world` (世界観・固有名詞) / `ui` (UI 部品) / `system` (システム用語) / `place` (場所名) / `character` (キャラ名) |
| **stage** | 確定 Stage (`s3` / `s4` / `s5`) |
| **note** | 補足、避けるべき訳語、判断根拠 |

### 1.2 翻訳判断の指針

- **AI 主体個人開発の Pitch 軸**: 機械訳ベタ訳ではなく、Anemora の世界観を尊重した coined translation を優先 (例: 時の筆 = "Timewriter" は造語、辞書訳の "Time Brush" は不採用)
- **シンプル優先**: Stage 5 で他言語追加時に再翻訳しやすい平易な英語に寄せる
- **音の質感を尊重**: Chrono Trigger / Undertale 系の "なんかかっこいい" 感を意識 (`feedback_anemora_no_premature_lockin.md` のネーミング軸と整合)
- **欧米寄り Latin/Greek の overuse 回避**: "Chronoscript" のような重い造語は避ける

---

## 2. 世界観コア用語 (確定)

| ja | en | status | scope | stage | note |
|---|---|---|---|---|---|
| Anemora (作品名) | Anemora | confirmed | world | s1 | そのまま固有名詞、ローマ字綴り維持 |
| 時の筆 | Timewriter | confirmed | world | s3 | "Time Brush" "Chronoscript" は不採用。Time + writer の造語、書く動作を含意。発音 [ˈtaɪmˌraɪtər] |
| 時の窓 | Time Frame | confirmed | world | s3 | フレーム = 枠を強調、ポータル機構の名称。"Time Window" は普通すぎ、不採用 |
| ベール剥離 | Veil Peeling | confirmed | world | s3 | 5 層構造、各層をベールで覆い、剥がしていく構造。"Layer Reveal" は不採用 (構造を明示しすぎる) |
| 真層 | True Layer | provisional | world | s4 | 第 6 層相当の最終層。確定は Stage 4 (収束パターン確定後) |
| 観測者 | Observer | provisional | world | s4 | 世界の観測主体 (詳細は Stage 4-5 で確定)、複数案維持 |
| 異物 | Anomaly | provisional | world | s4 | 主人公の正体表現。"Outsider" "Foreigner" は人間的すぎ、"Anomaly" は中立 |
| 痕跡 | Trace | confirmed | world | s3 | 過去の能動行動が現在に残す形跡。"Imprint" "Mark" より動的 |
| 時間侵食 | Temporal Erosion | confirmed | world | s3 | 時の窓の異常状態 (VS では発動させない)。直訳寄りだが意味が明確 |
| ループ | Loop | confirmed | world | s4 | 世界がループしている設定。直訳で OK |
| 衰退 | Decline | confirmed | world | s3 | 街の現状を表す形容、"Fading" "Decay" より中立 |

---

## 3. ゾーン名 (Stage 3 進行中、暫定)

| ja | en | status | scope | stage | note |
|---|---|---|---|---|---|
| 第 1 ゾーン (仮称: 街) | Zone 1 (working title: Town) | tbd | place | s3 | 正式名は Stage 3 試作と同期して確定 (`STAGE3_TBD_RESOLUTION.md` §7.1) |
| 中央広場 | Central Plaza | provisional | place | s3 | 広場の名称、一般語 |
| 図書館跡 | Old Library / Library Ruin | provisional | place | s3 | "Old Library" 推奨 (シンプル)、Library Ruin は予備 |
| 主人公の家 | Player's House | confirmed | place | s3 | システム的呼称、固有名はつけない (主人公の名前未確定のため) |
| 集会所 (廃) | Abandoned Hall | provisional | place | s4 | Stage 4 で詳細実装、仮称 |
| 別の家 | Neighboring House | provisional | place | s4 | NPC_2 の家、Stage 4 で詳細 |

---

## 4. キャラクター名 (Stage 3 進行中、暫定)

| ja | en | status | scope | stage | note |
|---|---|---|---|---|---|
| 主人公 | Protagonist | tbd | character | s3 | 名前は Stage 3 中盤の `/spec` で確定予定 (`STAGE3_TBD_RESOLUTION.md` §1.1)、確定後本書を更新 |
| Resident_A (中央広場の通行人) | Resident A | provisional | character | s3 | 個別名は Stage 3 中盤で `/spec` 確定。Resident A は内部 ID |
| Resident_B (ベンチに座る人) | Resident B | provisional | character | s3 | 同上 |

---

## 5. システム / メカニクス用語 (確定)

| ja | en | status | scope | stage | note |
|---|---|---|---|---|---|
| 過去 (時の窓) | Past | confirmed | system | s3 | 赤シンボルが指す時代 |
| 現在 (時の窓) | Present | confirmed | system | s3 | 通常状態の時代 |
| 未来 (時の窓) | Future | confirmed | system | s4 | 青シンボル、Stage 4 で実装 |
| 赤シンボル | Red Symbol | confirmed | system | s3 | UI 表示時は単に "Past" として表示、内部 ID で Red |
| 白シンボル | White Symbol | confirmed | system | s4 | 内部 ID、UI は "Present" |
| 青シンボル | Blue Symbol | confirmed | system | s4 | 内部 ID、UI は "Future" |
| 持ち帰る | Take | confirmed | system | s3 | 過去で物を取る能動行動。"Bring back" は冗長、シンプルに Take |
| 告げる | Tell | confirmed | system | s4 | NPC への能動行動 (Stage 4) |
| 押す/動かす | Move | confirmed | system | s4 | 物理オブジェクトへの能動行動 (Stage 4) |
| 踏込み | Crossing | confirmed | system | s3 | ポータル平面を越える動作。"Stepping In" より中立 |
| 帰還 | Return | confirmed | system | s3 | 過去から現在へ戻る動作 |
| ポータル | Portal | confirmed | system | s3 | 時の窓の描画機構名称 |
| シンボル選択 | Symbol Selection | confirmed | ui | s3 | UI 部品の名称 |
| ActionRecord | ActionRecord | confirmed | system | s3 | 内部用語、ローカライズ対象外 (コード上の概念) |

---

## 6. UI 用語 (確定)

### 6.1 メニュー

| ja | en | status | scope | stage | note |
|---|---|---|---|---|---|
| タイトル | Title | confirmed | ui | s3 | タイトル画面 |
| はじめる | Start | confirmed | ui | s3 | "Begin" "New Game" 不採用、シンプルに Start |
| 続きから | Continue | confirmed | ui | s3 | セーブから再開、定型訳 |
| 設定 | Settings | confirmed | ui | s3 | "Options" は古い、Settings 推奨 |
| タイトルへ戻る | Return to Title | confirmed | ui | s3 | ESC メニュー項目 |
| 終了 | Quit | confirmed | ui | s3 | "Exit" よりシンプル |

### 6.2 設定 (アクセシビリティ)

| ja | en | status | scope | stage | note |
|---|---|---|---|---|---|
| UI 拡大率 | UI Scale | confirmed | ui | s3 | "UI Magnification" は冗長 |
| 字幕サイズ | Subtitle Size | confirmed | ui | s3 | 定型訳 |
| コントラスト | Contrast | confirmed | ui | s3 | 定型訳 |
| 高コントラスト | High Contrast | confirmed | ui | s3 | アクセシビリティモード名 |

### 6.3 設定 (音量 / 表示)

| ja | en | status | scope | stage | note |
|---|---|---|---|---|---|
| マスター音量 | Master Volume | confirmed | ui | s3 | 定型訳 |
| 音楽 | Music | confirmed | ui | s3 | "BGM" よりシンプル、英語ユーザー向け |
| 効果音 | Sound Effects | confirmed | ui | s3 | "SFX" は内部用語、UI は full word |
| 環境音 | Ambient | confirmed | ui | s3 | 定型訳 |
| 解像度 | Resolution | confirmed | ui | s3 | 定型訳 |
| フルスクリーン | Fullscreen | confirmed | ui | s3 | 定型訳 |
| 言語 | Language | confirmed | ui | s3 | 定型訳 |

### 6.4 セーブ / ロード

| ja | en | status | scope | stage | note |
|---|---|---|---|---|---|
| オートセーブ | Auto Save | confirmed | ui | s3 | スペース区切り |
| 手動セーブ | Manual Save | confirmed | ui | s4 | Stage 4 で実装 |
| スロット | Slot | confirmed | ui | s4 | "Save Slot" の短縮形可 |
| 上書き保存 | Overwrite | confirmed | ui | s4 | Stage 4 |
| 保存しました | Saved | confirmed | ui | s3 | 完了通知 |
| 保存に失敗しました | Save Failed | confirmed | ui | s3 | エラー通知、ADR-0006 §8 |

### 6.5 対話 UI

| ja | en | status | scope | stage | note |
|---|---|---|---|---|---|
| 続ける | Continue | confirmed | ui | s3 | テキスト送り。`ui.menu` の Continue (続きから) と区別するため key は別 (例: `ui.dialog.continue`) |
| 確定 | Confirm | confirmed | ui | s3 | 選択確定 |
| キャンセル | Cancel | confirmed | ui | s3 | 定型訳 |
| 戻る | Back | confirmed | ui | s3 | 定型訳 |

---

## 7. 含み演出のテキスト (`docs/draft/g1_opening_text.md` 整合)

オープニング (D-3 / D-7 / D-6 弱版 / ドア前 / 外出) のテキスト英訳。最終的に String Table (`Anemora_Strings.asset` ja-JP / en-US sub-Asset) に投入。

### 7.1 D-3 (夢を見ていたような、夢を見ていなかったような)

| ja (確定 A 案) | en | status | note |
|---|---|---|---|
| 夢を見ていたような、夢を見ていなかったような。 | Like having had a dream — or having not. | provisional | 二項並列の構造を保つ訳。"Maybe a dream, maybe not." も候補 |

### 7.2 D-6 弱版 (なんとなく、重い、推奨)

| ja (確定 C 案) | en | status | note |
|---|---|---|---|
| (なんとなく、重い) | (Somehow, heavy.) | provisional | 直訳寄り、抽象度を保つ |

D-6 弱版は削除可能フラグあり (`gameSettings.tutorial.show_d6_weak`)。

### 7.3 D-7 (テキストなし、視覚演出のみ)

テキストはなし、Z 案採用 (推奨)。文字列なし。

### 7.4 ドア前で時の筆を取り出す演出 (B 案推奨)

| ja (確定 B 案) | en | status | note |
|---|---|---|---|
| (ポケットに、何か入っている) | (Something in my pocket.) | provisional | "Something is in my pocket." だと冗長、簡潔に。括弧書きは内面表現 |

### 7.5 外に出た瞬間の演出

テキストなし、空気で語る (BGM + SFX のみ)。文字列なし。

---

## 8. NPC 対話 (`docs/draft/g3_npc_dialogue.md` 整合、初版)

NPC のセリフ英訳。確定 → String Table 投入。

### 8.1 Resident_A 初対面 (ActionRecord 反映前)

| ja | en | status | note |
|---|---|---|---|
| おはよう。今日も静かだね。 | Morning. Quiet again today. | provisional | "Good morning" は formal すぎ、Morning. が口語自然 |
| この街は、もうずっとこんな感じでね。前はもう少し、人がいたんだけれど。 | The town's been like this for a while now. There used to be more people. | provisional | Past contrast を保つ |
| …いや、年寄りの繰り言だ。気をつけて。 | ...Just an old one rambling. Take care. | provisional | "繰り言" を "rambling" で軽くする |

### 8.2 Resident_A 反映後 (本持ち帰り後)

| ja | en | status | note |
|---|---|---|---|
| …おかえり。 | ...Welcome back. | provisional | 短く |
| あの図書館、随分前から閉まってるはずなのに。なぜか今日は、本の匂いがする気がしてね。 | That library's been closed for ages. But somehow today, I keep thinking I smell books. | provisional | "本の匂いがする気がしてね" を "I keep thinking I smell books" で内面の不確かさを保つ |
| …気のせいかね。気をつけて。 | ...Must be imagining it. Take care. | provisional | 軽く流す質感を維持 |

### 8.3 Resident_B (3.1 案、最小寡黙版)

| ja | en | status | note |
|---|---|---|---|
| …。 | ... | provisional | 沈黙の表現、英語でも … |
| …空が、低いね。 | ...The sky is low. | provisional | 直訳、抽象的描写を保つ |
| …。(視線だけ向ける) | ... (just a glance) | provisional | ト書き |
| …いいよ、何も言わなくて。 | ...It's okay. You don't have to say anything. | provisional | 主人公が話そうとしてやめた時の応答 |

---

## 9. 改訂運用

### 9.1 ステータス遷移

- `tbd` → `provisional` → `confirmed` の流れ
- `confirmed` 後の英訳変更は **大幅 refactor** (String Table key を全更新する必要あり) になるため避ける
- `provisional` のままビルドはできる、ただし Stage 5 リリース判断時に全 `confirmed` 化を目指す

### 9.2 新規追加時のフロー

1. ADR / 計画書 / draft で日本語が確定したら、本書 §2-§8 の該当 scope に追記
2. 英訳を起草 (本書 §1.2 指針に従う)
3. ユーザーレビュー → `provisional` 決定
4. Stage 4 入口で全 `provisional` を再点検し、`confirmed` 化を判断

### 9.3 機械訳ツールへの参照

DeepL Pro / Claude / Codex で翻訳補助を使う場合、本書を **必ず参照** させる。指示プロンプト例:

```
以下の日本語テキストを英訳してください。
ただし、以下の用語は本プロジェクトの公式英訳に従ってください:

時の筆 = Timewriter
時の窓 = Time Frame
ベール剥離 = Veil Peeling
痕跡 = Trace
踏込み = Crossing
帰還 = Return
持ち帰る = Take

[原文]
...
```

DialogueAsset の `LocalizedString` も同じ用語に従う (ADR-0008 §5)。

---

## 10. 関連文書

- `docs/adr/0008-localization.md` §4.2 (本書の運用方針)
- `docs/draft/g1_opening_text.md` (含み演出テキスト原文、§7 で英訳)
- `docs/draft/g3_npc_dialogue.md` (NPC 対話原文、§8 で英訳)
- `docs/STAGE3_TBD_RESOLUTION.md` (主人公 / 時の筆 / オープニング確定事項)
- `SPEC.md` §3 / §4 / §5 (世界観 / 主人公 / メカニクス)

---

## 11. 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v0 | 2026-05-04 | 初版起草。世界観コア用語 (confirmed 11 / provisional 3) + ゾーン名 + キャラ名 + システム用語 + UI 用語 + 含み演出テキスト + NPC 対話の英訳を整理。Stage 3 確定済日本語に対する暫定英訳を投入 |
