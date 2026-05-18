# Chapter 1 Graphic Session Review (2026-05-10)

> **位置付け**: graphics foundation orchestrator (Windows / `codex/stage4-graphics-foundation-20260508` branch) が、handover v1.1 (commit `d8f19dd` "Realign chapter 1 graphic handover with Windows session reality") を受領し、§8.2 の 15 整合性確認項目を調査、character generation session (`codex/stage4-hero-v2-20260506` branch) / implementation session (新規予定、未着手) 向けの担当別指示書を作成した記録。
>
> **入力**: 3 handover (graphic v1.1 / s1_s2 v1.5 / map v1.3) + 5 devlog (scene1-5) + char session v10 progress handovers (Mia complete / Aria front complete / Dario pending source / Pass2-3 priority f-n) + Anemora repo 既存制作物状態 (Hero/Resident_A/Resident_B prefabs, TimeFramePortalSystem, Symbol Wheel, Zone1 audio, 15 Zone1 building prefabs)。
>
> **出力**: 本 devlog (Anemora repo) + orchestration index (notes repo `_handover/anemora-chapter1-graphic-session-review-2026-05-10.md`)。

---

## 0. summary

| 項目 | 結果 |
|---|---|
| 整合性確認 15 項目 | 全件調査完了、担当別振り分け済 |
| char session 向け指示 (#1-#5) | settings parity 確認 + 動作セット要望 (Mia / Kaia / Dario / Karla / Hero v2 + Reto v8 補足) |
| implementation session 向け指示 (#6-#15) | dialogue 差し替え + 新仕様 v3.2 実装 + Symbol Wheel 1 周目赤のみ + 章切り替え演出 + シーン構成 + 痕跡可視化 |
| ボトルネック | image-gen pass = Tom-blocked (Aria back/left/right + Reto v8 + Dario / Karla / Kairo / Luna / Kaia front 全 source PNG)。char session は周辺工具整備 (Pass 3 priority f-n) で並行進行中 |
| ユーザー確認推奨 | (a) TimeFramePortalSystem v3.2 = 拡張 vs 再実装、(b) シーン構成 = Anemora_Main 発展 vs 新規 Anemora_Chapter1、(c) Zone1 audio をシーン 5 廃墟予兆まで使うか Zone2 新設か、(d) image-gen pass 進行手段 |
| handover §5.1 / §13.1 の誤情報訂正 | 「14 building prefab in `Assets/Prefabs/Buildings/`」→ 実体は `Assets/Prefabs/Zone1/` 配下に **15 prefabs** (Bed_Player, Book_Family_*, Bookshelf_*, Door_House, Floor_Stone/Wood, House_Player, Library_Ruin, Plaza_Fountain_Dry_Broken, StreetLamp, Table_SmallChair_Wooden, Tree_Decay) |

---

## 1. character generation session 向け指示書 (#1-#5)

> **宛先**: `codex/stage4-hero-v2-20260506` branch、Windows side。autonomous (Tom 介入なし) で動作中、Mia v10 proportion lock 完了 / Aria v10 front 完了 / Dario builder ready / 他は source pending。
>
> **依頼の性格**: 新規キャラ制作ではなく **設定整合確認 + 動作セット要望**。proportion lock 設計時 (= bible 作成時) に narrative 側の確定設定を反映、source PNG 配置後に動作セットを v10 builder の `SOURCES` に追加する形。

### #1 Hero v2 redraw vs 新仕様 v3.2 (Niro 窓内移動 pose)

- **状態**: F2 v1 既存 + Stage 4 v2 redraw 進行中 (Aria 同等の v10 pipeline へ移行候補)
- **整合性結論**: 新仕様 v3.2 で Niro が時の窓内に入って歩く演出が追加されるが、窓内移動のアニメ自体は通常歩行 (4 方向 walk) と同じ。色味/影は shader / カラーグレーディング側で過去側補正をかける形 (handover §6.2 設定 A + §3.6 仕様 v3.2 整合) なので、Hero スプライト側に追加 pose 不要
- **char session 要望**: なし。既存 v10 redraw 計画で十分
- **implementation session への確認事項**: Niro 窓内移動時の visual treatment (色味 / outline / 透過) を shader 側でどう実装するかは implementation session の §10 (TimeFramePortalSystem v3.2 実装) で扱う

### #2 Mia v10 proportion lock 整合性確認

- **状態**: Mia v10 proportion lock **完了** (target_height=72, height_ratio_vs_niro 1.014, status `lock_complete`)
- **設定確定** (handover §10.2 + §11.4 + scene2 v1 final devlog):
  - 30-40 / 元・縫物 / 織物職人 / **一人暮らし** (未亡人 / 独身) / 面倒見の良い、世話好き、職人らしい忙しさ
  - 道具: 針 / 梭 / 紡 / 機械 / はさみ / 糸巻き (handover §6.2 + scene2 [2.C] 整合)
- **bible 反映確認依頼**: `mia_proportion_bible_v10.md` で上記設定が記載されているか確認、漏れがあれば追記
- **front pending source generation 時の動作セット要望** (シーン 2 [2.A]-[2.F] v1 final 整合):
  - [2.B] **「立ち話もなんだから、寄っていって」面倒見動作** = 玄関で布を片手に持ちつつ Niro を促す pose
  - [2.C] **作業場と棚 (種袋・保存食) の中間に立つ pose** = ミア家内部での自然な立ち位置
  - [2.D] **縫いかけの布をテーブル横に置く動作** = 片手で布を持ちつつテーブルに置く中間 pose
  - [2.E] **棚から種袋を取り出す動作** = 背伸び or 棚に手を伸ばす pose
  - [2.F] **玄関で手を振って見送る動作** = 軽く手を振る pose
- **優先度**: Mia は v10 proportion lock 完了済なので、front 4 方向 source 配置時に上記動作セットを `SOURCES` に追加する形で漸増可

### #3 Kaia v10 設定反映依頼 (proportion lock 設計時に bible 反映)

- **状態**: front pending source、proportion lock **設計開始前**
- **設定確定** (handover §10.2 + scene4 v1 devlog):
  - 25-30 / 街と森境のナッツ農家 / **一人暮らし** (※ 衰退の象徴の一部、表現は出さない) / 観察力ある、寡黙寄り、畑に集中、真面目
  - 街との繋がりは仕事限定 (種 / 道具を取りに行く程度、特に親しい人なし)
  - ミアとの差別化: 面倒見の良さ (ミア) vs 畑中心 (カイア)、明確に分ける
- **char session への要望** (Mia パターン同様):
  - 1. **proportion lock bible** に「ナッツ農家、一人暮らし、観察力寡黙、街との繋がりは仕事限定、ミアとの差別化 = 面倒見良さ vs 畑中心」を明記して開始
  - 2. **target_height=72** (Mia / Niro adult anchor uniform、handover §10.2 で年齢 25-30 = adult)
  - 3. **動作セット要望** (シーン 4 [4.A]-[4.I] v1 動線骨格整合):
    - [4.B] **「枝を見ている」観察動作** = 枝の先を指でなぞる / 枝を持ち上げる pose (寡黙・観察力ある描写)
    - [4.B] **種袋を受け取る動作** = 両手で受け取る丁寧な所作 (handover §10.2「真面目」整合)
    - [4.B] **種袋を軽く中を確認する動作** = 種袋を開いて中を覗く pose
    - [4.F] **「畑を駆け回って異変を確認する」動作** = 走る中間 pose + しゃがんで地面を見る pose (寡黙だが驚きの観察)
    - [4.H] **「あなた…大丈夫?」動作** = Niro を見る pose (異物原則維持、深追いしない)

### #4 Dario v10 動作セット要望 (T4 連鎖の核)

- **状態**: builder + prompt brief + ディレクトリ structure **完了** (`tools/build_stage4_dq3r_dario_locomotion_pilot_v10.py`、target_height=72、broader shoulders by outfit)、4 source PNG 待ち
- **設定確定** (handover §10.1 + scene3 v3 devlog + scene4 v1 devlog):
  - 40 代 / 旅商人 / 香料商
  - 香料 = **エリュトリア由来** (失われた交易相手の街、シーン 3 [3.C]・シーン 4 [4.D] 連動キーワード)
  - シーン 3 [3.C] 屋台で香料を並べる + 客と話す
  - シーン 4 T4 連鎖の **過去側触媒** = カイア畑への過去訪問者
- **char session への要望** (Dario v10 source PNG 配置時に builder の `SOURCES` に追加する形):
  - 5. **シーン 3 [3.C] 屋台で香料を並べる動作** = 屋台 (固定背景) + 香料瓶 (色とりどり) を片手で並べる pose
  - 6. **シーン 3 [3.C] 客と話す動作** = 香料瓶を客に渡す中間 pose、独り言 (「今日のヤツは特に良い」「香りが立つ」) UI 整合
  - 7. **シーン 4 [4.D] 過去カイア畑訪問、香料瓶を渡す動作** = カイアの先祖 (シルエット) に香料瓶を渡す中間 pose
  - 8. **シーン 4 [4.E] 「!?」と独り言で驚く動作** = 香料瓶が「ひとりでに」動いた瞬間に頭を振り向く pose (Niro 異物干渉時)
  - 9. **シーン 4 [4.G] 香料を失って頭を抱える動作** = 「なに? あの香料が…」「これは…」発話時の俯き pose
- **broader shoulders 注意**: Dario handover §"verification" の「width pressure (>62 → needs_width_redraw) リスク高い」を踏まえ、side variant が `needs_width_redraw` 出たら image-gen 側で stride / shoulder を更に絞る (= Dario handover §"Carry-forward" 既記載)

### #5 Karla v10 動作セット要望 (商売教え)

- **状態**: front pending source、proportion lock 設計開始前
- **設定確定** (handover §10.1 + scene3 v3 devlog):
  - 30-40 / アリアの母 / 商家女主人
  - シーン 3 [3.D] アリア家内部で **アリア (12-15) に商売を教える** (C-4 商売教え)
  - 商品サンプル: 茶葉、香料、布
- **char session への要望**:
  - 10. **target_height=72** (Mia / Dario 同 adult uniform)
  - 11. **bible** に「アリアの母、商家女主人、商売教え動作 = アリアと並んで商品を見せる」を明記
  - 12. **動作セット要望** (シーン 3 [3.D] 整合):
    - **茶葉を見せる pose** = 両手で茶葉を持ち、アリアに見せる中間 pose
    - **色の違いを指差す pose** = 「沖の岸からの茶葉、こちらは山のお茶」「よく見て、色が違うでしょう」発話時の指差し
    - **香料を扱う pose** = 香料瓶 (Dario と同種、エリュトリア連動) を手に取る pose
    - **布を扱う pose** = 布を広げる / 折る pose (商品サンプル整合)
  - **連動キーワード**: カーラ「お父さんが言うには、**エリュトリア**では…」発話時のアリアとの距離感 (隣接 / 並ぶ) は dialogue 側 implementation session で wiring

### #B (補足) Reto v8 既知問題 + dialogue 差し替えへの対応

- **状態**: Reto v7c stand は **座位ポーズ** (sitting on chair)、bbox_height=67、Mia v10 lineup で anchor として misleading (Mia handover §4 既記載の既知問題)
- **char session への carry-forward** (Mia handover §4 と整合):
  - **Reto v8 stand redraw を立位で再生成** (Dario / Kaia / Karla / Kairo の proportion lock anchor として正しく使えるように)
  - 13. **シーン 1 v4 dialogue 差し替えに伴う表情 / 動作追加** (※ implementation session #7 と連動):
    - [1.F] **「...本物だ」発話時の本を手に取る + めくる + 撫でる動作 3 連** (本出現復活、handover §7.2 [1.F] 整合)
    - [1.F] **「...あなたのような方が、来てくれるとは」発話時の顔を上げる pose** (L-γ 諦観に芯が灯る瞬間)
    - [1.G] **帳面 + 本を抱えて何かを書き始める pose** (handover §7.2 [1.G] 整合)
- **front pending source 段階で動作セット要望に組み込み**

### #C (補足) その他 char session への carry-forward

- **#14 Kairo / Luna v10**: front pending source。Kairo = 楽器奏者 (シーン 3 [3.C] 楽器演奏 / 歌)、Luna = 5-10 子供 (シーン 3 [3.C] 子供たちと遊ぶ、target_height=46 toddler-read = Mia handover §6 既記載)。bible 反映 + 動作セット要望は Mia パターン同様
- **#15 Mob_Resident_A/B 提案** (handover §4.3 / §13.6 = Mob NPC 使いまわし):
  - char session が proportion lock を全 named NPC 完了した後に、Mob_Resident_A (汎用男性) / Mob_Resident_B (汎用女性) prefab の作成を carry-forward
  - 流用先: 序章 [O.E] 老人 1、シーン 3 [3.C] 過去街角名無し住人多数、シーン 3 [3.D] カーラ取引相手シルエット、シーン 4 [4.D] カイアの先祖シルエット、シーン 5 [5.B] 廃墟予兆過去住人シルエット
  - 性別 / 年代バリエーション (4-6 体程度)、シルエット / 暗色マテリアルバリエーションあり

---

## 2. implementation session 向け指示書 (#6-#15)

> **宛先**: 新規 implementation session (まだ立ち上げ無し)。Tom が production-session を立ち上げる時の起動プロンプト雛形を本書 + orchestration index に記載。
>
> **依頼の性格**: dialogue 差し替え + 新仕様 v3.2 実装 + Symbol Wheel + 章切り替え演出 + シーン構成 + 痕跡可視化。**新仕様 v3.2 実装 + シーン構成方針** は ⚠️ ユーザー確認推奨 (大きな実装変更、自走境界 §10.3)。

### #6 Resident_A (アリア) Stage 3 dialogue ↔ 新シーン 1 [1.E] / シーン 3 [3.D] dialogue 整合

- **状態**: Stage 3 lore-aware dialogue 投入済 (`Assets/ScriptableObjects/Dialogues/Resident_A_Greeting.asset`、`dialogue.encounter.past_resident_a.*` keys)
- **新シーン 1 [1.E]** (handover §7.2 v4): アリアは過去図書館で **本を読んでいる遠景**、Niro が窓内に入って近くで見られる、**Niro 側に気付かない** (異物原則維持)、Niro 心情 "(...あの子)"
- **新シーン 3 [3.D]** (handover §9.1 [3.D]): アリアはカーラに **商売を教わっている**、商品サンプル (茶葉) を見ながらカーラと dialogue (「うん、こっちのほうが濃い」)、エリュトリア連動キーワードあり
- **implementation session への要望**:
  - Stage 3 の `Resident_A_Greeting.asset` を **削除** (Niro 同時代の出会い演出が新仕様で消えた、シーン 1 過去遠景は dialogue 不要)
  - 新規 DialogueAsset 作成: `Resident_A_Past_Library_Glimpse.asset` (シーン 1 [1.E] アリア遠景、独り言なし、Niro が見るだけ)
  - 新規 DialogueAsset 作成: `Resident_A_Past_AriaHouse_Lesson.asset` (シーン 3 [3.D] 商売教え、カーラと並ぶ)
  - StringTable key migration: 旧 `dialogue.encounter.past_resident_a.*` → 新 `dialogue.scene1.past_aria.*` + `dialogue.scene3.aria_house.*`

### #7 Resident_B (レト) Stage 3 dialogue ↔ 新シーン 1 v4 dialogue 整合

- **状態**: Stage 3 lore-aware dialogue 投入済 (`Resident_B_Idle.asset`、`dialogue.encounter.present_resident_b.*` keys)
- **新シーン 1 v4** (handover §7.2 v4): 8 セクション [1.A]-[1.H]、本出現復活、L-γ クライマックスは [1.C] (諦観表現) + [1.F] (芯が灯る瞬間) の二層
- **implementation session への要望**:
  - Stage 3 の `Resident_B_Idle.asset` を **新シーン 1 v4 dialogue 全文に差し替え**
  - セクション別に DialogueAsset 分割推奨: `Resident_B_Scene1_B_Initial.asset` ([1.B] 初対面)、`Resident_B_Scene1_C_LibraryHistory.asset` ([1.C] 図書館の歴史 + 誘い)、`Resident_B_Scene1_D_BrushReaction.asset` ([1.D] 時の筆発動 = レト「...?」のみ)、`Resident_B_Scene1_F_BookAppears.asset` ([1.F] 本出現 + 「...本物だ」「...あなたのような方が、来てくれるとは」)、`Resident_B_Scene1_G_MiaHint.asset` ([1.G] ミア家ヒント)
  - StringTable key migration: 旧 `dialogue.encounter.present_resident_b.*` → 新 `dialogue.scene1.reto.*` (handover §7.2 v4 dialogue 全文 transcribe)
  - Reto v8 表情 / 動作 (char session #B carry-forward) と wire-up: [1.F] 本を撫でる、L-γ 顔を上げる pose との連動

### #8 14 (実は 15) Zone1 building prefab の配置可能性 (新マップ 東/北東展開)

- **状態**: `Assets/Prefabs/Zone1/` に 15 prefabs 実在 (handover §5.1 / §13.1 の「14 building prefab in `Assets/Prefabs/Buildings/`」記述は誤り、実体は `Zone1/` 配下、本 devlog §0 で訂正済)
- **既存**: Bed_Player / Book_Family_Past / Book_Family_Current / Bookshelf_Empty / Bookshelf_FamilyBooks / Bookshelf_Library_Past / Door_House / Floor_Stone / Floor_Wood / House_Player / Library_Ruin / Plaza_Fountain_Dry_Broken / StreetLamp / Table_SmallChair_Wooden / Tree_Decay
- **新マップ (handover §2)** に対する配置適合性:
  - Niro 家 (南西): `House_Player` + `Bed_Player` + `Door_House` + `Table_SmallChair_Wooden` 流用可
  - 中央広場 (中央): `Plaza_Fountain_Dry_Broken` + `StreetLamp` 流用可
  - 図書館跡 (北): `Library_Ruin` + `Bookshelf_Empty` + `Bookshelf_Library_Past` (時の窓越しの過去用) + `Book_Family_Past` (時の窓内アイテム) + `Book_Family_Current` (痕跡可視化、シーン 1 [1.F]) 流用可
  - ミア家 (南東): **新規必要** = ミア家外観 (煙突 + 玄関戸 + 職人道具) + 内部 (テーブル + 作業場 + 棚 + 暖炉)、`House_Player` を bake する形でも可だが屋根に煙突追加が必要
  - 街角 + アリア家 (東、シーン 3): **新規必要** = 寂れた木造建物 3-4 棟 + 空の屋台 + アリア家 (戸が朽ちて半開き、入れる)
  - カイア畑 (北東、シーン 4): **新規必要** = 緩斜面 + ナッツの木 7-10 本 + 中央作業小屋 + 井戸 (`Plaza_Fountain_Dry_Broken` を流用 or 新規)
  - 廃墟予兆 (東端、シーン 5): **新規必要** = 荒れ地 + 抜け殻の建物 3-4 棟 (鍵かかった建物) + 1 個入れる建物 + 森入口 (遠景)
- **implementation session への要望**:
  - 既存 15 prefab の流用配置 + 新規 building prefab の優先順序付き作成リスト
  - 新規 prefab は graphics-foundation branch の `Assets/Art/Models/Zone1/` (Meshy + Blender pipeline、handover §13.4) で生成、proportion / palette は既存 atlas (`Anemora_Zone1_Atlas_512.png`) と整合

### #9 シーン構成: Anemora_Main 発展 vs 新規 Anemora_Chapter1 ⚠️ ユーザー確認推奨

- **状態**: `Assets/Scenes/Anemora_Main.unity` (Stage 3 VS 用、placeholder 配置済) + `Sandbox_E1_Stencil.unity` (E1 portal stencil sandbox) のみ
- **両論併記**:
  - **A. Anemora_Main 発展案**: 既存 wiring (PrototypePlayerController / 境界往復 PlayMode test / 14 building placement / Resident_A/B prefab placement) を活かしつつ、新マップ (東/北東展開) + 新シーン構成 (5 シーン) に差し替え。利点: 既存テスト群 (PlayMode 29/29) との互換性、Stage 3 → Stage 4 連続性。欠点: Anemora_Main は Stage 3 VS の placeholder で、シーン規模 (26m × 24m + 拡張) が違うため大改修必要、Stage 3 用構成を残しつつ Chapter 1 に発展させると hybrid scene が複雑化。
  - **B. 新規 Anemora_Chapter1.unity 作成案**: Anemora_Main は Stage 3 VS reference として残し、Chapter 1 production scene は新規ファイル。利点: 関心分離、Stage 3 既存 PlayMode test (`AnemoraMainPortalWiringRoundTripTests` 等) との conflict 回避、Chapter 1 設計に最適化された scene 構成。欠点: 新規 PlayMode test 群が必要、Stage 3 wiring を再利用するための setup helper が必要。
- **orchestrator 推奨**: **B. 新規 Anemora_Chapter1.unity** (関心分離 + Chapter 1 設計に最適化)。但し ⚠️ ユーザー確認推奨 (大きな構成変更)
- **carry-forward**: ユーザー確定後、implementation session が新規 scene を作成、Stage 3 wiring の helper 抽出 + Chapter 1 各シーン (序章 + S1-S5) のセクション境界 trigger / camera rig / lighting tone を配置

### #10 TimeFramePortalSystem 新仕様 v3.2 実装 ⚠️ ユーザー確認推奨

- **状態** (handover §13.3 + Anemora repo `Assets/Scripts/TimeManagement/` 実装): TimeFramePortalController + PortalCrossingDetector + TimeWindowDiorama + ActionRecordRuntime + PortalVisualSwitcher + PortalFlashPlayer + NiroMonologueController + URP PortalStencilFeature
- **新仕様 v3.2** (handover §3.6 + scene3 design devlog) で必要な変更:
  - **(1) Niro 窓内移動** (内部範囲指定モード、屋外 / 屋内 / 廃墟も入れる仕様) — Niro が時の窓を踏み越えて過去側に入ったまま歩ける、PortalCrossingDetector の境界判定が「入ったら過去 layer 11 に切り替え + 維持」になる
  - **(2) 過去 NPC は Niro に気付かない** (異物原則維持、設定 A 延長) — 過去 NPC の AI / interaction が「Layer 9 player」を認識しない
  - **(3) Niro が窓内で能動行動 (touch / take / push)** — シーン 1 [1.E] 本を取る (PastBookInteractable 流用)、シーン 4 [4.E] 香料に触れる (新規 SpiceJarInteractable 等)
  - **(4) 痕跡可視化** — シーン 1 [1.F] 本出現 (既存 BookReflector + Book_Family_Current.prefab 流用)、シーン 4 [4.F] カイア畑変化 (新規 reflector、複数 GameObject の visible state を切り替える)
  - **(5) オート発動例外** (handover §3.6 例外) — シーン 4 [4.D] [4.G] のオート時の窓発動、ストーリー演出 trigger で controller に「auto-trigger」モード送信
  - **(6) シンボル段階開示** (handover §3.6 シンボル段階開示) — Symbol Wheel で 1 周目は赤のみ active、白は撤回、青は 2 周目で activate (= #12 と連動)
  - **(7) 鍵演出** (シーン 5 [5.B]) — 廃墟予兆建物のうち「鍵かかった建物」(3-4 棟) と「入れる建物」(1 個) の差別化、入れる建物の戸が朽ちて半開きの visual
- **両論併記**:
  - **A. 拡張案**: 既存 TimeFramePortalSystem に新 mode (内部範囲指定 + Niro 窓内 + 能動行動) を追加。利点: 既存 32+29 test 維持、Stage 3 backward compat。欠点: 既存 PortalCrossingDetector の 6 状態 state machine (handover §3.5 ADR-0005 v1.1) を二重化する必要、test の追加負荷
  - **B. 再実装案**: TimeFramePortalSystem v2 を新規 namespace で書き直し、既存 system は Stage 3 VS legacy として残す。利点: 仕様 v3.2 に最適化された clean 実装、テストカバレッジ再構築。欠点: Stage 3 wiring との互換 layer が必要、開発工数大
- **orchestrator 推奨**: **A. 拡張案** (Stage 3 backward compat + 既存 ADR の連続性)。但し ⚠️ ユーザー確認推奨
- **carry-forward**: ユーザー確定後、implementation session が新仕様 v3.2 を実装、ADR-0010 (時の窓モード v3.2) を新設 + ADR-0002 v1.1 / ADR-0005 v1.1 を更新

### #11 Zone1 audio をシーン 5 廃墟予兆まで使うか、Zone2 を新設するか ⚠️ ユーザー確認推奨

- **状態** (handover §13.2): Zone1_Ambient.ogg (BGM) + SFX 30 種 (env 6 + footstep 12 + time 6 + npc 3 + ui 3) + Zone1AudioController
- **シーン 5 廃墟予兆 audio 要件** (handover §6.2):
  - 砂塵、風、廃墟特有の音 (新規必要)
  - シーン 4 [4.D] 時の筆オート反応音 + シーン 4 [4.F] カイア畑異変音 (枯れ木軋み / ナッツ落下)
  - シーン 5 [5.E] 章切り替え音 (小石蹴る SFX、フェード環境音、章名表示効果音)
- **両論併記**:
  - **A. Zone1 拡張案**: SFX に env 廃墟音 / 章切り替え音を追加、BGM はシーン 5 で Zone1_Ambient.ogg を low-pass filter 適用 (handover §13.2 「演出曲 = 街アンビエントの変調」と同パターン)
  - **B. Zone2 新設案**: 廃墟予兆 audio profile を Zone2 として独立、Zone2_Ambient.ogg を新規 Suno 生成 + SFX zone2_ruin_dust / zone2_chapter_close 等を追加
- **orchestrator 推奨**: **A. Zone1 拡張案** (VS 範囲外の Zone2 新設は工数 vs 体験価値で trade-off 良くない、handover §6.1 / §13.2 「街アンビエントの変調で代用」整合)。但し ⚠️ ユーザー確認推奨

### #12 Symbol Wheel の 1 周目「赤のみ」表示対応

- **状態** (Anemora repo `Assets/UI/Prefabs/SymbolWheel.prefab`): 3 シンボル表示、赤のみ活性、白 / 青は preview / disabled (handover §13.3 / VS_SCOPE §3.1 「シンボル UI 表示方針」既記載)
- **新仕様** (handover §3.6 シンボル段階開示):
  - 1 周目 = **赤 (過去) のみ active**、白 (現在) は **撤回** (旧設計の「別ループ世界線が見える」前提が消えた)、青 (未来) は **2 周目で覚醒** (Story Bible memory「2 周ループ構造」整合)
- **implementation session への要望**:
  - 既存 SymbolWheel.prefab で 3 シンボル UI のうち **白を完全に削除 (or 永続的に hidden)**、青は 1 周目では preview / disabled、2 周目フラグ立つと active
  - シーン 1 [1.D] チュートリアル UI を「赤シンボル (過去) を選択」「ドラッグで窓を描く」「窓内に入って歩ける」「過去のオブジェクトに触れられる」(handover §7.2 v4 [1.D] 4 行) に更新
  - 2 周目フラグ (= 第 1 章末で覚醒) は新規 PlayerProgressionFlag に追加、SaveEnvelope で永続化

### #13 シーン 1 [1.F] 本出現演出復活 (痕跡可視化) の実装

- **状態**: Stage 3 で旧設計 (本出現あり、L-γ クライマックス) を一度実装 (G4 ActionRecord + BookReflector + Book_Family_Current.prefab) → 2026-05-09 早朝 v3 final で削除 (Layer 1 観察ルール整合) → 2026-05-09 v4 で復活 (Layer 1 仕様改訂で「個人レベル干渉 + 痕跡可」)
- **実装パターン** (handover §7.2 v4 [1.E][1.F]):
  - シーン 1 [1.E] Niro が窓内で過去図書館の本「家族の記録」(B-γ) に近づく → 手に取る (PastBookInteractable click → ActionRecord `take_book_001` 記録)
  - シーン 1 [1.F] 窓を閉じる → 現在の机に Book_Family_Current.prefab 出現 (BookReflector が ActionRecord を見て activate)
  - レト「...本物だ」「...あなたのような方が、来てくれるとは」発話 (Resident_B_Scene1_F_BookAppears DialogueAsset、#7 と連動)
- **implementation session への要望**: 旧 G4 実装を流用、新シーン構成 (Anemora_Chapter1) に wire-up、StringTable key migration

### #14 シーン 4 [4.D] [4.G] オート発動例外の実装

- **状態** (handover §3.6 例外 + scene4 v1 devlog):
  - シーン 4 [4.D] = T4 連鎖の発動、過去のカイア畑 (健康な畑 + 先祖 + ダリオ訪問) を確実に見せる
  - シーン 4 [4.G] = ダリオ代償の確認、過去側に戻ってダリオが香料を失って困っている演出
- **implementation session への要望**:
  - TimeFramePortalSystem に `AutoTriggerMode` 追加 (#10 内、新仕様 v3.2 実装の一部)
  - シーン 4 [4.D] / [4.G] の story trigger (player position + state flag) で controller に auto-trigger 送信
  - プレイヤー認識: 時の筆の反応 (淡い赤光 = 既存 PortalFlashPlayer 流用)、Niro 心情 "(...筆が、反応している)" (NiroMonologueController 流用)

### #15 シーン 5 [5.E] 章切り替えアニメ + BF1 起点 (小石蹴る) の実装

- **状態** (handover §6.2 + scene5 v1 devlog [5.E]):
  - Niro が振り返って西街方向に歩き出す
  - **自動アニメ: Niro が小石を蹴る** ← BF1 起点 (明示なし、プレイヤーが気付かない自然な動作)
  - フェードアウト + 章名表示「**忘れられた街**」(N18、player-facing 章名) + 中間セーブ + 第 2 章「揺れる森」へ
- **implementation session への要望**:
  - 新規 ChapterTransitionController (state machine: Niro auto-walk → stone kick anim → fadeout → chapter title text → save → next chapter)
  - 小石 prefab (1 回限り、データドリブン、第 2 章シーン 4 で「あの小石が川の流路を変えて」として回収) + Niro 蹴り animation clip 追加
  - 章名表示 UI (TMP、フェード in/out、フォントは美咲ゴシック JP / Press Start 2P EN provisional)
  - 中間セーブ: 既存 SaveEnvelope で chapter1_complete フラグ書き込み

---

## 3. 進捗管理 + ボトルネック整理

### 3.1 char session 進捗 (2026-05-09 時点、Mia handover §6 + Aria handover §6 + Dario handover §"Pending")

| キャラ | 状態 | 次工程 |
|---|---|---|
| Niro Hero v2 | F2 v1 既存、v2 redraw 進行中 (Aria 同 pipeline 移行候補) | proportion lock 開始 |
| Aria | v10a front 完了 (target_height=65 child-read)、graceful skip 実装済 | back/left/right source PNG 待ち (Tom-blocked、image-gen pass) |
| Mia | v10 proportion lock 完了 (target_height=72) | front 4 方向 source PNG 待ち、bible 反映確認 + 動作セット要望 (本書 #2) |
| Dario | builder + prompt brief + ディレクトリ完了 (target_height=72、broader shoulders) | 4 source PNG 待ち + 動作セット要望 (本書 #4) |
| Reto v8 | front pending source、v7c stand 座位ポーズ既知問題 | 立位 redraw + シーン 1 v4 dialogue 動作 (本書 #B) |
| Kaia | front pending source、proportion lock 設計開始前 | bible 反映 + proportion lock + 動作セット要望 (本書 #3) |
| Karla | front pending source | bible 反映 + proportion lock + 動作セット要望 (本書 #5) |
| Kairo | front pending source | (#C) |
| Luna | front pending source、target_height=46 (toddler-read 5-10) | (#C) |

### 3.2 ボトルネック: image-gen pass (Tom-blocked)

- **症状**: Mia / Aria / Dario handover の「Carry-forward (Tom + next pass)」で「Tom が image-gen で N PNG を `source/` に配置」と各キャラ毎に明記。char session は image-gen 後の builder 1 発で alpha 化 / strip / metrics / review/manifest/assessment refresh が可能だが、source PNG 自体は char session の責任範囲外 (= Codex `imagegen` skill 想定 / Tom 手動 image-gen pass)。
- **2D pipeline 整備状況** (anemora README + ADR-0009 + ASSET_STRUCTURE §1): **PixelLab + Aseprite** が正規 pipeline。char session は alpha 抽出 / chroma key / proportion lock 等の **後工程** を担当、初期生成 (PixelLab API 直接呼び出し / Aseprite 仕上げ) は Tom が orchestrate
- **char session の並行進行**: Pass 3 priority O (v10 sibling lineup builder polish) / Pass 3 全完了の総合検証 + 統合報告プロンプト / `tools/_data_lib.py` 抽出 (Linux/macOS 移植時 / Pillow < 11 環境必要時の carry-forward) など、image-gen 待ちの間に builder 群と周辺工具を整備できる
- **orchestrator から Tom への提言**: image-gen pass の進行手段を 1 つに絞らず、PixelLab API 直叩き (char session 自走可能) / Aseprite 手動 (Tom 手作業) / Codex `imagegen` skill (limit 解除待ち) のうち **どれを正規ルートにするか確認**。VS 製作の進捗 critical path がここに依存する

### 3.3 implementation session の前提条件

- **新規 implementation session 立ち上げ前に必要な大方針判断** (本書 §5 ⚠️ ユーザー確認推奨):
  - (a) シーン構成 = Anemora_Main 発展 vs 新規 Anemora_Chapter1
  - (b) TimeFramePortalSystem v3.2 = 拡張 vs 再実装
  - (c) Zone1 audio をシーン 5 まで使うか Zone2 新設か
- これらが Tom 確定すれば、implementation session 起動プロンプト (orchestration index §3) で具体的な方針を出せる

---

## 4. 工数見積 (effort band: S / M / L / XL、STAGE4_ROADMAP §2 整合)

> 個人開発 + AI 主体 scale。calendar date は含めない。S = focused session、M = 複数 session、L = まとまった implementation block、XL = 複数 block にまたがる作業。

### 4.1 char session 側

| 項目 | 工数 | 前提 |
|---|---|---|
| Mia bible 反映確認 + 動作セット source 配置 (front 4 方向) | M | image-gen pass 完了が前提 |
| Kaia / Karla / Kairo / Luna proportion lock + bible 反映 + 動作セット source 配置 | L | 各キャラ M、image-gen pass 完了が前提 |
| Dario proportion lock + 動作セット source 配置 | M | broader shoulders width pressure リスク有、image-gen 側調整が必要かも |
| Reto v8 stand 立位 redraw + シーン 1 v4 dialogue 動作 source 配置 | M | image-gen pass 完了が前提 |
| Hero v2 redraw 完了 (Aria 同 pipeline) | M-L | 既存 v2 redraw を v10 pipeline に移行する場合 |
| Mob_Resident_A/B prefab 作成 (Mob 流用、シルエットバリエーション) | M | named NPC 全員 v10 完了後 |
| **char session 全完了** | **L-XL** | image-gen pass のスループット次第 |

### 4.2 implementation session 側

| 項目 | 工数 | 前提 |
|---|---|---|
| Resident_A/B dialogue 差し替え + 新シーン用 DialogueAsset 作成 + StringTable migration | M | scene 構成 (#9) 確定後 |
| 14 (15) Zone1 building prefab の新マップ配置 + 新規 building prefab 作成 (ミア家 / 街角 / アリア家 / カイア畑 / 廃墟予兆) | L-XL | scene 構成 (#9) + 3D pipeline 稼働 |
| TimeFramePortalSystem 新仕様 v3.2 実装 (拡張案 A) | XL | ユーザー確定 (#10) 後 |
| Symbol Wheel 1 周目赤のみ + チュートリアル UI 更新 + 2 周目フラグ | M | 新仕様 v3.2 実装 (#10) と連動 |
| シーン 1 [1.F] 本出現演出復活 (旧 G4 流用) | S-M | dialogue 差し替え (#7) 完了後 |
| シーン 4 [4.D] [4.G] オート発動例外実装 | M | TimeFramePortalSystem 拡張 (#10) と連動 |
| シーン 5 [5.E] 章切り替えアニメ + BF1 起点 (小石蹴る) + 章名表示 + 中間セーブ | L | ChapterTransitionController + アニメ + UI 新規 |
| Zone1 audio 拡張 (シーン 4 [4.D] 時の筆反応 / [4.F] 異変音 / シーン 5 廃墟音 / [5.E] 章切り替え音) | M | Zone1 拡張案 (案 A) なら音源変調 + SFX 数本追加 |
| Niro 家伏線 (1 周目過去のみ別の家族の気配 / 2 周目未来 = 廃墟) | M | 1 周目はシーン 0 [O.B] 棚調査の延長で「過去の編み物道具 / 食器」を加える |
| **implementation session 全完了** | **XL** | char session 完了 + 大方針判断 (a)(b)(c) 確定後 |

### 4.3 critical path

```
[image-gen pass (Tom)] → [char session per-character source PNG drop]
       │                          │
       ▼                          ▼
[PixelLab/Aseprite/Codex pipe]  [char session builder refresh + proportion lock]
       │
       ▼ (大方針判断)
[ユーザー判断: シーン構成 + TimeFramePortalSystem v3.2 方針]
       │
       ▼
[implementation session: dialogue / wiring / new building / TimeFramePortal v3.2 / Symbol Wheel / 章切り替え]
       │
       ▼
[VS 第 1 章 通し体験 + 1 セッション完走可能]
```

---

## 5. ⚠️ ユーザー確認推奨項目 (Tom 判断待ち)

handover §10.3 「自走境界」整合。本 review では結論を出さず、両論併記 + orchestrator 推奨を記載。

| # | 確認項目 | 両論 | orchestrator 推奨 |
|---|---|---|---|
| (a) | シーン構成 = Anemora_Main 発展 vs 新規 Anemora_Chapter1 | A 既存 wiring 流用 / B 関心分離 + Stage 3 残す | **B 新規 Anemora_Chapter1** (#9) |
| (b) | TimeFramePortalSystem v3.2 = 拡張 vs 再実装 | A 既存 system に mode 追加 / B v2 を新規 namespace で書き直し | **A 拡張** (#10、Stage 3 backward compat) |
| (c) | Zone1 audio をシーン 5 まで使うか Zone2 新設か | A Zone1 SFX 追加 + BGM 変調 / B Zone2 新設 (Suno + 新 SFX) | **A Zone1 拡張** (#11、VS 範囲外のため) |
| (d) | image-gen pass 進行手段 | PixelLab API 直叩き / Aseprite 手動 / Codex imagegen (limit 中) | **未確定**: Tom 判断 (§3.2 ボトルネック整理) |
| (e) | 物語 doc を含む commit / push (本 devlog + orchestration index) | git add + commit / 作業ツリー保留 | Tom 確認 (handover §10.3) |

---

## 6. 関連 commit / 文書

### 6.1 Anemora repo (main branch)

- `d8f19dd` (2026-05-09): Realign chapter 1 graphic handover with Windows session reality (handover v1.1)
- `8b3da1d` (2026-05-09): Add chapter 1 graphic session handover (handover v1.0)
- `43ebc43` (2026-05-09): Lay out Scene 5 and redo the chapter-1 path layout
- `1d1ae4c` (2026-05-09): Lay out Scene 4 v1 with auto-trigger window for the T4 reveal
- `57080ef` (2026-05-09): Lift Layer 1 observation-only rule and design Scene 3 around it
- `eaa9aeb` (2026-05-09): Finalize Chapter 1 Scene 2 dialogue and align sister map doc
- `12229a6` (2026-05-09): Finalize Chapter 1 Scene 1 v3 to 8-section structure
- `5641304` (2026-05-08): Add Chapter 1 design corpus, map handover, and session recovery doc

### 6.2 notes repo

- `fed5a21` (2026-05-09): Add Anemora Chapter 1 graphic session orchestration handover (orchestration index v1.0)

### 6.3 主要 source 文書 (本 review の参照元)

- `docs/draft/chapter1_graphic_session_handover_2026-05-09.md` v1.1 (Anemora repo)
- `docs/draft/chapter1_s1_s2_handover_2026-05-08.md` v1.5 (Anemora repo)
- `docs/draft/chapter1_map_handover_2026-05-08.md` v1.3 (Anemora repo)
- `docs/devlog/2026-05-09_chapter1_scene1_v3_final.md` (Anemora repo)
- `docs/devlog/2026-05-09_chapter1_scene2_v1_final.md` (Anemora repo)
- `docs/devlog/2026-05-09_chapter1_layer1_revision_and_scene3_design.md` (Anemora repo)
- `docs/devlog/2026-05-09_chapter1_scene4_v1.md` (Anemora repo)
- `docs/devlog/2026-05-09_chapter1_scene5_v1_and_map_redesign.md` (Anemora repo)
- `docs/SPEC.md` v0.3 / `docs/VS_SCOPE.md` v1.0 / `docs/STAGE4_ROADMAP.md` v1.1 / `docs/ASSET_STRUCTURE.md` v0.3 (Anemora repo)
- `_handover/anemora-character-generation-claude-mia-v10-proportion-lock-complete-2026-05-09.md` (notes repo)
- `_handover/anemora-character-generation-claude-aria-v10-front-complete-2026-05-09.md` (notes repo)
- `_handover/anemora-character-generation-claude-dario-v10-front-pending-source-2026-05-09.md` (notes repo)

---

## 7. 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v1.0 | 2026-05-10 | 初版作成 (graphics foundation orchestrator が handover v1.1 §8.2 15 項目を調査、char session / implementation session 向けの担当別指示書を作成) |
