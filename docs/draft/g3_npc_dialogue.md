# G3 NPC 対話文言ドラフト (draft)

> `STAGE3_G_PLAN.md` §3 Phase G3 で配置する Resident_A / Resident_B の対話文言案。
> Linux Claude 起草、ユーザー判断で確定 → `Assets/ScriptableObjects/Dialogues/` に組込み。

> **Status**: v0 ドラフト。silent protagonist + 異物原則 + 過剰説明回避を守りつつ、各 NPC に 2-3 案ずつ並列。

---

## 1. 共通方針

### 1.1 silent protagonist 仕様

- NPC のセリフ → 主人公の **感情 / 反応の選択肢 (テキストアイコン)** → 続く NPC のセリフ → クローズ
- 主人公はしゃべらない (口元アニメも控える)
- 選択肢は 2-3 個、選択により会話のニュアンスが変わるが分岐は最小
- 「すべての選択肢が物語上等価」(誤った選択肢を作らない、プレイヤー責任を負わせない)

### 1.2 異物原則の遵守

NPC は普通の住人。以下を **避ける**:

- 主人公の正体に関する示唆 (「あなたには何かを感じる」「特別な力を持っているのね」)
- 時の筆や時の窓に関する直接言及 (主人公が街で初めて使うまで NPC は知らない)
- 過剰な歓迎 / 警戒 (異物として認識している演出)
- アーキタイプ的役割 (語り部 / 守り人 / 前任者)

### 1.3 過剰説明の回避

- 衰退の原因を語らない (本人もわからない)
- 「層」「観測者」「真層」などの世界観用語は使わない
- 単なる日常の世間話、その中に違和感の片鱗が静かに紛れている

### 1.4 ActionRecord 反映の見える形

VS_SCOPE.md §8 推奨: NPC の **少なくとも 1 人に「現在反映」が見える** こと。本書では:

- **Resident_A**: 反映あり (本持ち帰り後にセリフが変化)
- **Resident_B**: 反映なし (制御群、変化しない NPC を 1 人置くことで対比を作る)

---

## 2. Resident_A (中央広場の西側、立ち姿、時々歩く)

**設定**: 街に住む普通の人。年齢は中年〜初老くらい (中性表現の主人公とのコントラスト)。性別は中性的でも明確でも可、ユーザー判断。

### 2.1 初対面 (主人公が初めて Resident_A に話しかけた時、ActionRecord 未反映)

| ターン | 話者 | テキスト |
|---|---|---|
| 1 | Resident_A | おはよう。今日も静かだね。 |
| 2 | (主人公の選択肢) | A. 静かに頷く / B. 周りを見回す / C. 小さく頭を下げる |
| 3 | Resident_A (どの選択でも同じ続き) | この街は、もうずっとこんな感じでね。前はもう少し、人がいたんだけれど。 |
| 4 | (主人公の選択肢) | A. もう一度頷く / B. 何か言いたそうにする / C. 視線を落とす |
| 5 | Resident_A (どの選択でも同じ続き) | …いや、年寄りの繰り言だ。気をつけて。 |
| END | (会話終了) | |

#### 案 1.1 別案 (より静謐、より曖昧)

| ターン | 話者 | テキスト |
|---|---|---|
| 1 | Resident_A | …ああ、おはよう。 |
| 2 | (選択肢) | A. 頷く / B. 立ち止まる |
| 3 | Resident_A | 気をつけて行きなさい。風が、少し冷たい。 |
| END | (会話終了) | |

短く、世間話の入り口で閉じる案。違和感を「この距離感そのもの」が運ぶ。

### 2.2 ActionRecord 反映後 (本持ち帰り後、`take_book_family_001` フラグが立った後)

| ターン | 話者 | テキスト |
|---|---|---|
| 1 | Resident_A | …おかえり。 |
| 2 | (主人公の選択肢) | A. 頭を下げる / B. 戸惑う / C. 周りを見回す |
| 3 | Resident_A | あの図書館、随分前から閉まってるはずなのに。なぜか今日は、本の匂いがする気がしてね。 |
| 4 | (主人公の選択肢) | A. 黙って聞く / B. 少し驚く / C. 困ったように頷く |
| 5 | Resident_A | …気のせいかね。気をつけて。 |
| END | (会話終了) | |

#### 解説

- **「おかえり」**: Resident_A は主人公をどこかから戻ってきた人として認識する (= 本を持ち帰った行為が世界に痕跡を残した)
- **「本の匂いがする気がしてね」**: 過去で持ち帰った本が現在に存在することを、Resident_A は認識できないが「匂い」という間接情報で察知している。考察余地を残す
- **「気のせいかね」**: 痕跡を確定的に語らない、Resident_A 自身も状況を把握できていない

### 2.3 Stage 4 持ち越し候補

- 街の歴史 / 家族の話 / 衰退の予兆についての追加対話 (ActionRecord 多段階反映)
- 別の違和感 (サイド違和感) を Resident_A 経由で示唆する案

---

## 3. Resident_B (中央広場の東側、ベンチに座る、Idle のみ)

**設定**: 静かに座っている人。話しかけても多くは語らない。年齢 / 性別はユーザー判断、Resident_A との対比で逆寄り (Resident_A が中年なら B は若者、または逆)。

### 3.1 初対面 (どの段階でも同じセリフ、ActionRecord 反映なし)

| ターン | 話者 | テキスト |
|---|---|---|
| 1 | Resident_B | …。 |
| 2 | (主人公の選択肢) | A. 隣に立つ / B. 少し離れる / C. 何か言おうとする |
| 3 | Resident_B (A の場合) | …空が、低いね。 |
| 3 | Resident_B (B の場合) | …。(視線だけ向ける) |
| 3 | Resident_B (C の場合) | …いいよ、何も言わなくて。 |
| END | (会話終了) | |

#### 解説

- Resident_B は対比群: ActionRecord に反映されず、主人公の能動行動が「すべての NPC に効くわけではない」境界を示す
- 寡黙な存在感、「空が低い」という景色の描写だけで世界の質感を運ぶ
- 主人公が何を言おうとしても受け流す姿勢で、silent protagonist と矛盾しない

### 3.2 別案 (もう少し言葉数が増える版)

| ターン | 話者 | テキスト |
|---|---|---|
| 1 | Resident_B | …。 |
| 2 | (選択肢) | A. 頷く / B. 隣に座る (ベンチ空きあれば) |
| 3 | Resident_B | あなたも、ここに座る? それとも、行く? |
| 4 | (選択肢) | A. 座る / B. 行く |
| 5 | Resident_B (A の場合) | じゃあ、しばらく一緒に。何も話さなくていいよ。 |
| 5 | Resident_B (B の場合) | そう。気をつけて。 |
| END | (会話終了) | |

「一緒にいる」という選択肢が成立する案。ただしゲーム機構として「座って待つ」を実装するかは Stage 4 判断 (時間経過演出が必要)。VS では複雑化を避けて 3.1 を採用推奨。

---

## 4. 対話 UI 構造の前提 (ADR-0007 整合)

ADR-0007 §UI 要素別の実装方針 に従い:

- **Canvas**: Screen Space - Overlay
- **テキスト送り**: 既定で即時表示、補助オプションで一文字送り (アクセシビリティ)
- **選択肢表示**: NPC セリフ下に 2-3 個のテキストアイコン (主人公感情 / 反応)
- **会話ログ**: VS では実装しない (Stage 4 候補)

---

## 5. ScriptableObject 構造の提案

`Assets/ScriptableObjects/Dialogues/Resident_A_Dialogue.asset` を以下の形で持つ:

```csharp
[CreateAssetMenu(menuName = "Anemora/Dialogue")]
public class DialogueAsset : ScriptableObject
{
    public string npcId;
    public List<DialogueVariant> variants;
}

public class DialogueVariant
{
    public string variantId;             // "initial" / "post_take_book_family_001" など
    public List<DialogueTurn> turns;
    public List<string> requiredFlags;   // ActionRecord フラグの ID リスト
    public List<string> excludedFlags;   // これらのフラグが立っていたら使わない
}

public class DialogueTurn
{
    public string speakerId;             // "npc" / "player" / "narration"
    public string text;
    public List<DialogueChoice> choices; // speaker = "player" の時のみ
}

public class DialogueChoice
{
    public string emotion;               // "nod" / "lookAround" / "lowerGaze" 等
    public string nextTurnId;            // 分岐先 turn (省略可、線形なら次の turn へ)
}
```

VS では分岐を最小化、線形対話 + flag 別 variant 切替で十分。

---

## 6. 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v0 | 2026-05-04 | 初版起草。Resident_A (反映あり) + Resident_B (反映なし) のドラフト + ScriptableObject 構造案 |
