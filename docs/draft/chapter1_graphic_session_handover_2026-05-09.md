# Chapter 1 グラフィック / アセット製作セッション 引継ぎ書 (2026-05-09)

> **位置付け**: 第 1 章「忘れられた街」全シーン動線骨格完成 (2026-05-09) を受け、Windows 側グラフィック foundation orchestrator へ整合性確認 + 指示書作成依頼を渡す書類
> **宛先**: Windows 側 graphics foundation orchestrator (Tom)。そこから character generation session / implementation session に振り分け想定
> **次のステップ**: orchestrator が本書を読み、(1) 既存制作物との整合性確認、(2) Production session (VS = Vertical Slice 第 1 章実装) 向けの優先順序付き指示書を作成
> **二部構成**: 本書は **graphic / asset 特化**。**narrative / map 詳細** は sister docs:
>   - `docs/draft/chapter1_s1_s2_handover_2026-05-08.md` v1.5 (broader、物語 + メカ + 制約)
>   - `docs/draft/chapter1_map_handover_2026-05-08.md` v1.3 (map / level design 特化)
>
> **重要**: パスはすべて **repo 相対** で記載。Anemora repo の Windows 側実体は `C:\Users\maro6\Documents\Unity\Anemora-stage4-hero-v2`、notes repo の Windows 側実体は `C:\Users\maro6\notes`。
>
> **承知事項**: Windows 側で character generation session が **既に v10 proportion lock を進行中** (Mia v10 完了 / Aria v10 front 完了 / 他は source pending)。本書は新規キャラ制作依頼ではなく、**narrative / spec / map 側の最新状態と整合性確認** が主目的。

---

## 0. 直近の主要変更点 (2026-05-08 → 2026-05-09)

前回 graphic 側に渡した状態 (commit `5641304` 付近) からの変更を整理:

| # | 変更項目 | 詳細 | 影響 |
|---|---|---|---|
| 1 | **動線再設計** | シーン 1-5 がすべて南進 → 東 / 北東 展開に変更 | マップ全体配置の見直し必要 |
| 2 | **時の窓モード v3.2** | 内部範囲指定で統一、Niro が **窓内に入って歩ける** | TimeFramePortalSystem の再実装または拡張 |
| 3 | **Layer 1 仕様改訂** | 観察ルール撤廃、Layer 1 から **個人レベル干渉 + 痕跡** 可 | シーン 1 [1.E][1.F] 本出現演出復活、シーン 4 T4 が「空間的代償の初体験」に再定義 |
| 4 | **シンボル段階開示** | 1 周目は **赤 (過去) のみ**、白 (現在) は撤回、青 (未来) は 2 周目 | チュートリアル UI 簡略化、Symbol Wheel 1 周目は赤のみ表示 |
| 5 | **シーン 1 v4** | Niro 窓内移動 + 本に触れる + レト「...本物だ」復活 | 図書館シーンの絵的体験変更 |
| 6 | **シーン 3 v3** | アリア家廃墟も入れる、商売教え + エリュトリア連動 | アリア家内外、商家家具、商売道具アセット必要 |
| 7 | **シーン 4 v1** | T4 連鎖 9 セクション、過去のカイア畑 + 先祖 + ダリオ訪問 | カイア畑過去版、ダリオ香料屋台アセット必要 |
| 8 | **シーン 5 v1** | 廃墟予兆 + 鍵建物 + 入れる 1 個 + BF1 起点 (小石蹴る) | 廃墟エリアアセット、鍵演出、章切り替えアニメ |
| 9 | **キャラクター設定確定** | ミア = 縫物職人 / カイア = ナッツ農家 + 一人暮らし | Mia / Kaia prefab 仕様確定 |
| 10 | **モブ NPC 使いまわし** | Mob_Resident_A/B prefab 提案 | 共通モブ prefab を最初に作る方針 |
| 11 | **Niro 家伏線** | 1 周目に内部時の窓 = 過去 (別の家族の気配) のみ、未来 = 廃墟は 2 周目 | Niro 家過去版アセット |
| 12 | **章終了演出** | フェード + 章名「忘れられた街」表示 + 中間セーブ + BF1 起点 | 章切り替え演出システム |
| 13 | **用語整理** | 「Z 第 X 段階」→「クライマックス気付き第 X 段階」 | (ドキュメント整理のみ、実装影響なし) |

---

## 1. 第 1 章「忘れられた街」全シーン構成 (2026-05-09 確定)

| シーン | 場 | セクション数 | 所要時間 | 主要 NPC | 状態 |
|---|---|---|---|---|---|
| 序章 | Niro 家 + 中央広場 + 図書館跡入口 | [O.A]-[O.E] = 5 | 約 3 分 | (老人 1) | v1 final |
| シーン 1 | 図書館跡 | [1.A]-[1.H] = 8 | 約 8 分 | レト + アリア (過去遠景) | v4 |
| シーン 2 | ミア家 | [2.A]-[2.F] = 6 | 約 5 分 | ミア | v1 final |
| シーン 3 | 街角 + アリア家 | [3.A]-[3.F] = 6 | 約 6-7 分 | (過去) ダリオ / カイロ / ルナ / アリア / カーラ | v3 動線骨格 |
| シーン 4 | カイア畑 | [4.A]-[4.I] = 9 | 約 8-10 分 | カイア + (過去) ダリオ + 先祖 (シルエット) | v1 動線骨格 |
| シーン 5 | 廃墟予兆 + 森入口 | [5.A]-[5.E] = 5 | 約 4-5 分 | なし (環境演出主体) | v1 動線骨格 |

合計 39 セクション、約 35-40 分 (第 1 章全体)。

---

## 2. Antela マップ (新マップ、東 / 北東 展開)

### 2.1 全体配置

```
                       [図書館跡] (シーン 1、北)
                       アーチ入口/ろうそく/レト机
                              ↑ 石畳 約 5m
                              │
                       [中央広場] (序章末)
                       井戸/ベンチ/老人 NPC 1
                              │
            ┌─────────────────┼─────────────────┐
            │                                    │
      [Niro家南西]                       [ミア家南東] (シーン 2)
      (起床地点)                         煙突から煙
                                                │
                                                │ 東
                                                ▼
                                    [街角 + アリア家] (シーン 3)
                                    路地、現代廃のアリア家、時の窓越し中世
                                                │
                                                │ 北東
                                                ▼
                                    [カイア畑] (シーン 4)
                                    ナッツ畑 + 異変、街と森境 (森は東側)
                                                │
                                                │ さらに東
                                                ▼
                                    [廃墟予兆] (シーン 5、層 2 布石)
                                    荒れ地、抜け殻、森への入口は東
```

### 2.2 動線

中央広場 → 北 (シーン 1 図書館跡) → 中央広場戻り → 南東 (シーン 2 ミア家) → 東 (シーン 3 街角) → 北東 (シーン 4 カイア畑) → さらに東 (シーン 5 廃墟予兆) → 第 2 章「揺れる森」(東の森) へ

### 2.3 規模感
- 第一マップ blockout 約 **26m × 24m** の町区画 + 拡張 (シーン 3-5 でさらに東 / 北東に伸びる)
- 家 ≈ 4m、図書館 ≈ 6m、ミア家内部 6m × 5m、アリア家内部 (商家、要詳細)
- キャラクタースロット 32×48 px、PPU 32、prefab scale 1、nominal **1.5 Unity units**
- 詳細寸法: `chapter1_map_handover_2026-05-08.md` v1.3 §2.1 移動制限テーブル参照

---

## 3. 時の窓モード仕様 v3.2 (visual / interaction 影響大)

### 3.1 モード分類

| 場所 | 時の窓モード | Niro の挙動 |
|---|---|---|
| 屋外 (街角、カイア畑) | 範囲指定 | 窓内に入って歩ける |
| 建物 (現在も入れる、Niro 家・ミア家) | 内部に入って範囲指定 | 窓内に入って歩ける |
| 建物 (廃墟、アリア家・廃墟予兆) | **入れる仕様にする** (戸が朽ちて半開き)、内部で範囲指定 | 窓内に入って歩ける |
| 大きな建物 (図書館跡) | 内部で範囲指定 (建物全体には窓届かない) | 窓内に入って歩ける |
| 特殊ギミック (例外) | ドアに時の窓 → 過去/未来マップ全体 | (Chapter 1 では未使用) |

### 3.2 Niro が窓内でできること (Layer 1 で許可)
- 歩く (移動)
- 触れる (オブジェクト操作、本を取る等)
- 観察する
- 過去/未来 NPC の独り言 / 会話を聴く (Niro が話しかけても反応しない、異物原則)

### 3.3 シンボル段階開示
- **1 周目**: 赤 (過去) のみ、シーン 1 [1.D] チュートリアル UI も「赤シンボル (過去) を選択」のみ表示
- 2 周目: 青 (未来) 追加、Niro 家伏線などで使う
- 白 (現在): 1 周目では使わない、第 2 章以降のギミック検討時に追加検討

### 3.4 オート発動例外 (ストーリー上の見せ場)
- デフォルトはプレイヤー発動 (場所・サイズ・タイミングすべてプレイヤー)
- 例外: シーン 4 [4.D] T4 連鎖発動、[4.G] 代償確認 → **オート発動** (固定演出を確実に見せる)
- プレイヤー認識: 時の筆の反応 (淡い赤光) + Niro 心情「(...筆が、反応している)」

### 3.5 視覚演出要件 (新規)
- **時の窓の境界**: 過去 = 赤、現在 = 白 (PROTOTYPE_TIME_FRAME_v0.md §3.2 整合)
- **窓内移動時の Niro 視覚**: 過去側に立つ Niro の見え方 (色味、影)
- **過去 NPC の Niro への無反応演出**: NPC は通り過ぎる、目が合わない
- **痕跡可視化**: シーン 1 [1.F] 本出現、シーン 4 [4.F] カイア畑の変化 (枯れ拡大、ナッツ落下、土変色)
- **鍵演出**: シーン 5 [5.B] 鍵かかった建物 (ドアに鍵マーク? 動かないドア? 等の表現)

---

## 4. キャラクター prefab 一覧 (Chapter 1 必要分)

### 4.1 既存 + Windows 側で v10 proportion lock 進行中 (2026-05-09 時点)

Windows 側 character generation session の最新状態 (`_handover/anemora-character-generation-claude-*-v10-*-2026-05-09.md` 参照):

| コード | 名前 | Windows 側状態 | Chapter 1 での使用 | narrative 側からの整合確認依頼 |
|---|---|---|---|---|
| Hero | Niro | F2 v1 既存、Stage 4 v2 redraw 進行中 | 序章〜シーン 5 全シーン (主人公) | 新仕様 v3.2「Niro が窓内に入って歩ける」と整合する pose / animation セット必要 |
| Resident_A | アリア | Stage 3 prefab 既存、**v10 front 完了**、back/left/right は source pending | シーン 1 (過去遠景)、シーン 3 (過去アリア家、商売教え演出) | シーン 3 [3.D] で商家娘 (12-15) として母 J と並ぶ姿の整合確認 |
| Resident_B | レト | Stage 3 prefab 既存、**v8 front pending source** | シーン 1 (現在、図書館跡) | シーン 1 v4 dialogue (本出現 + 「あなたのような方が」) と既存 Stage 3 dialogue の差し替え整合 |
| **Resident_F** | **ミア** | **v10 proportion lock 完了**、front pending source | シーン 2 | 設定確定 = 30-40、**元・縫物 / 織物職人、一人暮らし**、面倒見の良さ。proportion lock が確定設定に整合か確認 |
| **Resident_C** | **カイア** | **v10 front pending source** | シーン 4 | 設定確定 = 25-30、**街と森境のナッツ農家、一人暮らし**、観察力ある寡黙。proportion lock 設計時に反映 |
| **Resident_D** | **ダリオ** | **v10 front pending source** | シーン 3 (過去街角)、シーン 4 (過去カイア畑) | 旅商人 / 香料商 (エリュトリア由来)、複数シーンで香料瓶を扱う動作 |
| **Resident_J** | **カーラ** | **v10 front pending source** | シーン 3 (過去アリア家) | 商家女主人 (アリアの母)、商売教えの動作 |
| **Resident_K** | **カイロ** | **v10 front pending source** | シーン 3 (過去街角) | 楽器奏者・詩人、楽器演奏動作 |
| **Resident_L** | **ルナ** | **v10 front pending source** | シーン 3 (過去街角) | 5-10 子供、遊び動作 |

→ **キャラ生成は順調**。本 handover からの追加依頼は **設定確定情報の反映 + 確認** のみ:
- Mia: 「縫物 / 織物職人」「一人暮らし」「面倒見の良さ」が proportion lock 反映されているか確認
- Kaia: 「ナッツ農家」「一人暮らし」「観察力寡黙」を proportion lock 設計時に反映
- Dario: 香料瓶を持つ / 渡す / 並べる 動作セット必要 (T4 連鎖の核)
- Karla: 商売道具 (茶葉 / 香料 / 布) を扱う動作セット
- Reto v8: シーン 1 v4 dialogue 差し替えに伴う表情 / 動作追加可否

### 4.2 新規必要 (使いまわし用 Mob、優先度中)

| コード | 用途 |
|---|---|
| **Mob_Resident_A** | 汎用住人 prefab (男性想定)、性別 / 年代バリエーション |
| **Mob_Resident_B** | 汎用住人 prefab (女性想定)、性別 / 年代バリエーション |

#### 流用先
- 序章 [O.E] 中央広場の老人 1
- シーン 3 [3.C] 過去街角の名無し住人多数 (商談 / 子供の遊び / 労働の活気演出)
- シーン 3 [3.D] 過去アリア家のカーラ取引相手 (シルエット)
- シーン 4 [4.D] 過去カイア畑の **カイアの先祖** (シルエット、固有名なし)
- シーン 5 [5.B] 廃墟予兆 入れる建物の過去住人 (シルエット、固有名なし)

### 4.4 シルエット表現用 (バリエーション)

過去 NPC のうち以下はシルエット表現 (背景的に動く):
- 過去図書館員 (シーン 1 [1.E]、1-2 名)
- 過去街角の名無し住人多数 (シーン 3 [3.C])
- 過去アリア家のカーラ取引相手 (シーン 3 [3.D])
- カイアの先祖 (シーン 4 [4.D])
- 廃墟予兆過去住人 (シーン 5 [5.B])

→ Mob_Resident_A/B のシルエット / 暗色マテリアルバリエーションで対応可能か

---

## 5. 環境 / シーン geometry 一覧

### 5.1 既存 (Stage 3 構築済)
- **14 building prefab**: `Assets/Prefabs/Buildings/` 系列、loadable 確認済 (現状 Anemora_Main 未配置、本タスクで配置)
- **Portal_Frame.prefab**: 時の窓本体
- **Book_Family_Past_Model / Book_Family_Current.prefab**: 図書館跡シーン用
- **DialoguePanel.prefab** + **SymbolWheel.prefab**

### 5.2 新規必要 (Chapter 1 用)

| 場所 | 内容 | 優先度 |
|---|---|---|
| **Niro 家 内部** | ベッド / 扉 / 家具数点 / 棚 / 窓 (序章) + 過去伏線版 (家具配置の違い、編み物道具、ベッド別位置、食器、住人気配シルエット) | 高 |
| **中央広場** | 12m × 12m 想定、井戸 / ベンチ / 老人 NPC 1 (序章 + シーン 5 末カメラパン用) | 高 |
| **図書館跡 (現在)** | アーチ入口 / 室内 6m × 8m / 空棚多数 / ろうそく光 / レト机 + 帳面 (Stage 3 一部既存、要 Anemora_Main 配置) | 高 |
| **図書館跡 (過去)** | 満架の本棚 / 温かいろうそく光 / 本の匂い (テキスト) / 過去図書館員シルエット 1-2 名 / アリア (奥で本を読む) | 高 |
| **ミア家 (内部)** | 6m × 5m、テーブル / 椅子 / **作業場 (はさみ・布・機械・糸巻き)** / 棚 (種袋・保存食) / 暖炉 / 玄関戸 | 高 |
| **ミア家 (外観)** | 玄関 / 煙突 (煙) / 入口前職人道具 (はさみ・布の山・糸巻き) | 高 |
| **街角 (現在)** | 路地、寂れた木造建物 3-4 棟、空の屋台 (市場跡) / 朽ちた看板 / 苔・ひび | 高 |
| **アリア家 (現在 = 廃墟)** | 戸が朽ちて半開き、入れる、空洞、朽ちた家具 | 高 |
| **アリア家 (過去 = 商家)** | 暖色の灯り、整った家具、**商家らしい棚 (商品サンプル: 茶葉、香料、布)**、本棚 (家計簿 / 在庫帳) | 高 |
| **カイア畑 (現在)** | 緩やかな斜面 (北 = 街方向 / 南 = 森方向)、ナッツの木 7-10 本、中央作業小屋、井戸、異変ポイント (枯れた木 / 落ちたナッツ / 変色した土) | 高 |
| **カイア畑 (過去)** | 賑やかな畑 (緑豊か、健康な木、整った土) + 先祖 (シルエット) + ダリオ訪問 (香料を渡す動作) | 高 |
| **廃墟予兆エリア** | 荒れ地、抜け殻の建物 3-4 棟 (ほとんど鍵かかり、1 個入れる) | 中 |
| **廃墟予兆 入れる建物 (現在)** | 内部空洞、朽ちた家具、苔と砂塵 | 中 |
| **廃墟予兆 入れる建物 (過去)** | 普通の家、住人シルエット背景、家族の生活感 | 中 |
| **森入口 (遠景)** | 廃墟予兆の東端、森の暗さ、葉のざわめき、奥が見えない | 中 |

---

## 6. VFX / UI 一覧

### 6.1 既存
- 時の窓ポータル (Portal_Frame.prefab)
- DialoguePanel + SymbolWheel
- Zone1 audio (BGM 1 + SFX 30 種)

### 6.2 新規必要

| 要素 | 内容 | 優先度 |
|---|---|---|
| **時の筆 反応光 (淡い赤光)** | シーン 1 [1.D] / シーン 3 [3.B] / シーン 4 [4.D] で時の筆が特定箇所に反応 | 高 |
| **チュートリアル UI 拡張** | シーン 1 [1.D]「窓内に入って歩ける」「触れる」操作説明 | 高 |
| **Niro 窓内移動演出** | 過去側に立つ Niro の見え方 (色味、影、世界の境界感) | 高 |
| **痕跡可視化 (シーン 1 [1.F])** | 本が現在の机に出現、レト「...本物だ」反応 | 高 |
| **痕跡可視化 (シーン 4 [4.F])** | カイア畑の変化 (枯れ拡大、ナッツ大量落下、土の変色) | 高 |
| **過去 NPC の独り言 UI** | ダリオ「!?」「あの香料が…」、シーン 4 [4.E][4.G] | 中 |
| **鍵演出** | シーン 5 [5.B] 鍵かかった建物のドア (鍵マーク? 動かないドア?) | 中 |
| **章切り替えアニメ + BF1 起点** | シーン 5 [5.E] Niro が小石を自動的に蹴る → フェード → 章名「忘れられた街」表示 | 高 |
| **カメラパン (シーン 5 [5.D])** | 廃墟予兆 → カイア畑 → 街角 → 中央広場 → 図書館跡 (Antela 全体を見渡す)、isometric strict との整合要検証 | 中 |
| **視界解放 (シーン 2 [2.F])** | 時間経過 (陽の角度) + 音の誘導 + 道の光誘導 | 中 |
| **種の包みインベントリ UI** | Chapter 1 唯一の所持品、布袋に紐、暖色 | 高 |
| **過去のカイア畑の絵** | 健康な畑 + 先祖シルエット + ダリオ + 香料瓶 (色とりどり、エリュトリア由来) | 高 |

---

## 7. Audio 一覧

### 7.1 既存 (Zone1)
- **BGM**: `Zone1_Ambient.ogg`
- **SFX**: env 6 + footstep 12 + time 6 + npc 3 + ui 3 = 30 種
- **Zone1AudioController** (scene-local instance)

### 7.2 新規検討必要

| 要素 | 内容 | 優先度 |
|---|---|---|
| **シーン 5 廃墟予兆エリア音** | 砂塵、風、廃墟特有の音 (Zone2 として別実装か Zone1 拡張か要検討) | 中 |
| **シーン 4 カイア畑 異変音** | 枯れ木が立てる軋み、ナッツが落ちる音、土の変色時の SFX | 中 |
| **過去 NPC dialogue 音** | ダリオ独り言、カーラ商売教え、カイロ歌、ルナ笑い声 (要録音 or TTS) | 中 |
| **シーン 5 [5.E] 章切り替え音** | 小石を蹴る SFX、フェード時の環境音、章名表示時の効果音 | 中 |
| **シーン 4 [4.D] 時の筆オート反応音** | 時の筆が淡く赤光する瞬間の SFX | 中 |

---

## 8. 既存制作物との整合性確認 (グラフィックセッションでの判断)

### 8.1 既存制作物 (Windows 側 Stage 3 / Stage 4 進行中、2026-05-09 時点)

#### キャラクター系 (character generation session)
- Hero F2 v1 既存 / v2 redraw 進行中
- Aria v10 front 完了 (back/left/right source pending)
- Mia v10 proportion lock 完了 (front source pending)
- Reto v8 / Dario v10 / Karla v10 / Kairo v10 / Luna v10 / Kaia v10 = front pending source

#### foundation 系 (graphics foundation / implementation session)
- Resident_A / B prefab + Stage 3 lore-aware dialogue
- 14 building prefab (Anemora_Main 未配置)
- Portal_Frame、DialoguePanel、SymbolWheel
- Zone1 audio (BGM 1 + SFX 30 種、Zone1AudioController)
- TMP fonts/atlas
- TimeFramePortalSystem (旧仕様、v3.2 拡張未対応)

### 8.2 整合性確認項目

| # | 確認項目 | 担当 | 判断 |
|---|---|---|---|
| 1 | Hero v2 redraw の方向性 (Niro silent protagonist + 窓内移動) と新仕様 v3.2 の整合 | character generation | proportion lock 設計時に「窓内移動 pose」要否確認 |
| 2 | Mia v10 proportion lock が「縫物 / 織物職人 + 一人暮らし + 面倒見」設定と整合 | character generation | 既設計を確認、必要なら追加動作 (布を扱う / 縫う動作) |
| 3 | Kaia v10 proportion lock 設計時に「ナッツ農家 + 一人暮らし + 観察力寡黙」を反映 | character generation | 設計開始前に本 handover §4.1 参照 |
| 4 | Dario v10 proportion lock 設計時に「香料瓶を持つ / 渡す / 並べる」動作セット必要 (T4 連鎖の核) | character generation | front pending source 段階で確認 |
| 5 | Karla v10 で「商売道具 (茶葉 / 香料 / 布) を扱う」動作セット | character generation | 同上 |
| 6 | Resident_A (アリア) Stage 3 dialogue ↔ 新シーン 1 [1.E] / シーン 3 [3.D] dialogue (商売教え + エリュトリア連動) 整合 | implementation | dialogue 差し替え必要、新 dialogue は handover §9.1 [3.D] 参照 |
| 7 | Resident_B (レト) Stage 3 dialogue ↔ 新シーン 1 v4 dialogue (本出現復活、「...本物だ」「...あなたのような方が」復活) 整合 | implementation | dialogue 差し替え必要、Reto v8 の表情追加可能か確認 |
| 8 | 14 building prefab の配置可能性 (新マップ 東/北東展開、handover §2.1) | implementation | Anemora_Main 配置時に再検証 |
| 9 | Anemora_Main.unity を Chapter 1 として発展させるか、新規 Anemora_Chapter1.unity を作るか | implementation orchestrator | アセット配置 + 動線テスト容易性で判断 |
| 10 | TimeFramePortalSystem の新仕様 v3.2 (Niro 窓内移動 + 触れる + 痕跡) 対応 | implementation | 既存実装の拡張 vs 再実装、要設計判断 (大きな変更、ユーザー確認推奨) |
| 11 | Zone1 audio をシーン 5 廃墟予兆まで使うか、Zone2 を新設するか | implementation | シーン 5 廃墟特有の音 (砂塵 / 風) 必要、別 zone 推奨 |
| 12 | Symbol Wheel の 1 周目「赤のみ」表示対応 | implementation | 表示制御変更、シンボル段階開示 (handover §3.3) 反映 |
| 13 | シーン 1 [1.F] 本出現演出復活 (痕跡可視化) の実装 | implementation | 旧設計に戻すが、Niro 能動行動の結果として整合する形 |
| 14 | シーン 4 [4.D] [4.G] オート発動例外の実装 | implementation | ストーリー上の見せ場として確実に再生する仕組み |
| 15 | シーン 5 [5.E] 章切り替えアニメ + BF1 起点 (小石蹴る) の実装 | implementation | 章切り替え演出システム新規 + 小石オブジェクト |

### 8.3 graphics foundation orchestrator への依頼

1. **既存制作物との整合性確認** (§8.2 の 15 項目) を行う
2. **担当別の振り分け**:
   - character generation session 向け: §8.2 #1-#5 (proportion lock 設計時の設定反映 + 動作セット)
   - implementation session 向け: §8.2 #6-#15 (dialogue 差し替え + 新仕様 v3.2 実装 + Symbol Wheel + 章切り替え演出 + シーン構成)
3. 各セッション向けの **優先順序付き指示書** を作成
4. アセット制作の **進捗管理 + 工数見積**
5. **新仕様 v3.2 (時の窓モード)** の実装方針判断 (大きな変更のためユーザー確認推奨、§10.3 自走境界参照)
6. 不明点があれば本書 + sister docs を参照、それでも不明なら orchestration index に質問を残す

---

## 9. 出力期待 (グラフィックセッションが製作セッションに渡すもの)

グラフィックセッションが本書を読んだ後、以下を製作セッションに渡す想定:

1. **優先順序付きアセット製作リスト** (例: Mia prefab → Kaia prefab → Dario prefab → Aria 家 → カイア畑、等)
2. **新仕様 v3.2 実装指示** (TimeFramePortalSystem 拡張方針、UI 修正、痕跡可視化の実装)
3. **シーン構築指示** (Anemora_Main を Chapter 1 として発展 or 新規シーン作成)
4. **Stage 3 既存制作物の修正指示** (アリア / レト dialogue 差し替え等)
5. **アセット工数見積** (製作セッションが計画立案できる粒度)

---

## 10. 制約 (グラフィックセッションが守るべき)

### 10.1 物語/世界観
- **異物原則**: 異物は Niro のみ。NPC 全員「普通の住人」(語り部 / 前任者 / 守り人 / 予言者 NG)
- **「層」「ベール剥離」用語**: 設計用便宜語、player-facing で出さない
- **固有名詞 player-facing 抑制**: 章名は OK、「Antela」「エリュトリア」等は dialogue / UI で前面化しない (ただし dialogue 内で「エリュトリアの新茶葉」のように出すのは OK、UI ラベルとしては避ける)
- **ネタバレ語彙 metadata 禁止**: commit / PR / branch / ファイル名で Echo / 観測者輪廻 / 真層 / 第 4 の壁 等を使わない

### 10.2 美術/技術
- HD-2D Tier 2 範囲内 (SPEC §7.1)
- isometric strict カメラ (SPEC §7.4)
- 動的影 + 単一方向光 + カラーグレーディング
- Tier 3-4 不採用 (※「本格的なシェード導入」のユーザー意向あり、後で確認予定)
- 黄昏色彩を世界全体のトーンとして敷く
- 静謐・衰退 を基調 (劇的でなく、ゆっくり、一歩ずつ)

### 10.3 自走境界 (`AUTONOMOUS_WORK_GUIDELINE.md`)

| 種別 | 自走可否 |
|---|---|
| 既存アセット polish / 配置 | ✅ |
| 新規キャラクター prefab 作成 (Mia / Dario / Karla / Kairo / Luna / Kaia 等) | ✅ (PixelLab / Aseprite / Meshy パイプ準拠) |
| 既存 Tier 2 範囲のシェーダ・VFX 調整 | ✅ |
| 動線設計詳細化 (5 軸) の **本書内追記** | ✅ |
| TimeFramePortalSystem の新仕様 v3.2 実装 | ⚠️ ユーザー確認推奨 (大きな実装変更) |
| 物語 / 世界観の核心判断変更 | ❌ ユーザー判断 |
| 章名 / NPC 名 final 化変更 | ❌ ユーザー判断 |
| 第 1 章クライマックス気付き 第 2 段階の具体演出 確定 | ❌ ユーザー判断 (Phase 5) |
| Story Bible §3.2 の更新 commit | ⚠️ ユーザー確認推奨 |
| 物語 doc を含む commit / push | ⚠️ ユーザー確認推奨 |

### 10.4 ユーザー体験必須
- test pass / build success ≠ playable
- 完成宣言は user 主観 review

---

## 11. 関連ドキュメント (Anemora repo 相対パス、Windows 側実体は `C:\Users\maro6\Documents\Unity\Anemora-stage4-hero-v2\`)

| Doc | 役割 | 現状 |
|---|---|---|
| `docs/draft/chapter1_s1_s2_handover_2026-05-08.md` | broader (物語/演出/メカ/制約/アセット) | v1.5 |
| `docs/draft/chapter1_map_handover_2026-05-08.md` | map / level design 特化 | v1.3 |
| **本書 (`docs/draft/chapter1_graphic_session_handover_2026-05-09.md`)** | graphic / asset 特化、整合性確認 + 指示書作成依頼 | v1.1 |
| `docs/STORY_BIBLE_v1.md` | 物語骨格 (本章は §3.2、登場人物 §4) | v1.7 (案 D 5 シーン構成は本 handover 側が最新) |
| `docs/DESIGN_RATIONALE.md` | 命名 / クライマックス由来 | §1.5 章名「忘れられた街」由来 |
| `docs/PROTOTYPE_TIME_FRAME_v0.md` | 操作感プロト仕様 (precursor) | v0.1 |
| `docs/AUTONOMOUS_WORK_GUIDELINE.md` | 独走可否境界 | v1.0 |
| `docs/scene_tour_anemora_main.md` | 現状 scene 構造 (再利用前提) | v0.2 |
| `docs/VS_SCOPE.md` | Stage 3 完成定義 | v0.4 |
| `docs/STAGE4_ROADMAP.md` | Stage 4 計画 | preliminary |
| `docs/ASSET_STRUCTURE.md` | アセット配置規約 | v0.2 |

### 11.1 関連 devlog (Anemora repo 相対、2026-05-09)
- `docs/devlog/2026-05-09_chapter1_scene1_v3_final.md` (シーン 1 v3 final、後の v4 で本出現復活)
- `docs/devlog/2026-05-09_chapter1_scene2_v1_final.md` (シーン 2 + 視界解放 + sister doc 整合)
- `docs/devlog/2026-05-09_chapter1_layer1_revision_and_scene3_design.md` (Layer 1 仕様改訂 + シーン 1 v4 + シーン 3 v3)
- `docs/devlog/2026-05-09_chapter1_scene4_v1.md` (シーン 4 + エリュトリア + カイア)
- `docs/devlog/2026-05-09_chapter1_scene5_v1_and_map_redesign.md` (シーン 5 + 動線再設計 + Z 用語整理 + 白窓 1 周目なし)

### 11.2 Windows 側 character generation 進捗 (notes repo 相対、Windows 側実体は `C:\Users\maro6\notes\`)
- `_handover/anemora-character-generation-claude-aria-v10-front-complete-2026-05-09.md`
- `_handover/anemora-character-generation-claude-mia-v10-proportion-lock-complete-2026-05-09.md`
- `_handover/anemora-character-generation-claude-{dario,karla,kairo,luna,kaia,reto v8}-v10-front-pending-source-2026-05-09.md`

### 11.1 関連 devlog (2026-05-09)
- `2026-05-09_chapter1_scene1_v3_final.md` (シーン 1 v3 final、後の v4 で本出現復活)
- `2026-05-09_chapter1_scene2_v1_final.md` (シーン 2 + 視界解放 + sister doc 整合)
- `2026-05-09_chapter1_layer1_revision_and_scene3_design.md` (Layer 1 仕様改訂 + シーン 1 v4 + シーン 3 v3)
- `2026-05-09_chapter1_scene4_v1.md` (シーン 4 + エリュトリア + カイア)
- `2026-05-09_chapter1_scene5_v1_and_map_redesign.md` (シーン 5 + 動線再設計 + Z 用語整理 + 白窓 1 周目なし)

---

## 12. 報告フォーマット (グラフィックセッションが orchestration に戻すもの)

作業完了時:
- Anemora repo: `docs/devlog/2026-05-DD_chapter1_graphic_session_review.md` (確認結果 + 製作セッション向け指示書)
- notes repo (orchestration): `_handover/anemora-chapter1-graphic-session-review-2026-05-DD.md` (Windows: `C:\Users\maro6\notes\_handover\...`)

`AUTONOMOUS_WORK_GUIDELINE.md` §5 報告フォーマット準拠。

---

## 13. 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v1.0 | 2026-05-09 | 初版作成 (第 1 章「忘れられた街」全シーン動線骨格完成 commit `43ebc43` を受け、グラフィックセッション向け整合性確認 + 指示書作成依頼書を作成) |
| v1.1 | 2026-05-09 | パスを **repo 相対** に統一 (Linux パス `~/learning/games/anemora/...` は使わない、Windows 側との互換性確保)。Windows 側 character generation session の **進捗を反映** (Mia v10 proportion lock 完了 / Aria v10 front 完了 / 他 v10 front pending source)、本 handover からの新規キャラ制作依頼ではなく **設定整合確認 + 動作セット要望 + dialogue 差し替え** が主目的に修正。§4.1 を「進行中」状態に書き直し、§8 整合性確認を 15 項目に拡大 (担当別: character generation #1-#5 / implementation #6-#15)、§11 関連ドキュメントを Anemora repo 相対 + Windows 実体パス併記。 |
