# Stage 3 G トラック実装計画書 — 第 1 ゾーン (家 + 街)

> VS_SCOPE.md §2 (10-15 分体験) / §3.2 (第 1 ゾーン) / §8 (完了条件) を成立させる第 1 ゾーンの最小実装計画。E トラック (Time Frame プロトタイプ) と F トラック (主人公スプライト) を統合し、**ニューゲームから VS 終端まで 1 セッションで通しプレイ可能** な状態を作る。

> **Status (2026-05-04 = Day 1 起草)**: v0.1。Phase G0-G5 + 含み演出統合 + 検証マトリクスを確定。E0-E1 / F0-F2 進行中の段階で先行起草。

---

## 0. 目的とスコープ

### 0.1 G トラックで成立させるもの

VS_SCOPE.md §8 「必須 (VS 達成判定の死守ライン)」を満たすゾーン構築:

1. **主人公の家の内部** (ベッドからスタート、ドア前で時の筆発見)
2. **中央広場 + 周辺 2-3 棟** (歩いて 1-2 分で回れる物理規模、新規 2-3 棟 + 再利用)
3. **過去版の同エリア** (Root_Past、ADR-0005 常駐ヒエラルキー)
4. **NPC 1-2 人** (普通の住人、異物原則遵守)
5. **主要違和感 1 個** (失われた家族の記憶 = 本 / 手紙 / 歌譜のいずれか)
6. **ActionRecord トリガー** (過去で本を取る → 帰還後ベッドに痕跡反映、E5 と接続)
7. **含み演出 D-3 / D-7 / D-6 弱版** (オープニング、Stage 3 A トラック確定事項)
8. **層 2 片鱗演出 1 カット** (VS フィナーレ)
9. **街アンビエント BGM 1 曲** + 環境音 + 時の窓 SFX (VS_SCOPE §5)

### 0.2 G トラックで作らないもの

- メトロイドヴァニア要素 (探索的バリア、VS_SCOPE §3.2)
- 戦闘 / HP / インベントリ実装 (VS_SCOPE §6)
- ゾーン 2-5 (Stage 4 マイルストン)
- 高品質遠景 / スカイボックス (プレースホルダ可、VS_SCOPE §7)
- 完成品質 BGM / SFX (Stage 4 で固有曲差替)
- パーティクル全般 (VS_SCOPE §4.3 範囲外)

### 0.3 完了条件

VS_SCOPE §8 必須項目すべて pass。特に:

- ニューゲームから VS 終端まで 1 セッションで破綻なく通しプレイ可能 (10-15 分)
- コアループが破綻なく動作 (E トラック実装と統合)
- 主要違和感 1 個が機能、痕跡が確認できる
- 層 2 への片鱗演出が 1 カット入る
- Windows ビルドが起動 → タイトル → ゲーム本体まで動作
- 詰みが起きない

---

## 1. 前提資料

| 文書 | 関係 |
|---|---|
| `docs/VS_SCOPE.md` §2, §3.2, §4, §5, §6, §8 | スコープ定義、完了条件 |
| `docs/STAGE3_TBD_RESOLUTION.md` §4, §5 | オープニング、VS 主要違和感 |
| `docs/adr/0005-time-management-scene-switching.md` | 常駐ヒエラルキー / ActionRecord |
| `docs/adr/0007-ui-framework-ugui.md` | 対話 UI、HUD |
| `docs/STAGE3_E_PLAN.md` | E トラックとの接続点 (E2 / E5) |
| `docs/STAGE3_F_PLAN.md` | F トラックとの接続点 (F4 Hero Prefab) |
| `SPEC.md` §3.5, §5.2, §7.4 | オープン要件、痕跡可視化、固定アイソメ視点 |

---

## 2. 第 1 ゾーンの空間構造

### 2.1 物理規模 (歩いて 1-2 分)

```
                                    [ 家族の家 (主人公の家) ]
                                    ├─ ベッド (start point)
                                    ├─ 本棚 (空、家族の本があった場所)
                                    ├─ テーブル + 椅子 2 つ
                                    └─ ドア (出ると時の筆発見)
                                            │
                                            ↓ 出る
                                    [ 家の前の小道 ]
                                            │
                                            ↓
                                    [ 中央広場 ]
                                    ├─ ベンチ or 噴水跡 (中央)
                                    ├─ NPC 1: 通行人 (西側)
                                    └─ NPC 2: 静かに座る人 (東側)
                                            │
                          ┌─────────────────┼─────────────────┐
                          ↓                 ↓                 ↓
                    [ 図書館跡 ]    [ 集会所 (廃) ]    [ 別の家 (近所) ]
                    主要違和感        サイド違和感         (NPC 2 の家)
                    の場所            の場所候補
```

- **新規作成は 3 棟程度**: 家族の家 / 中央広場の建造物 (ベンチ・噴水跡) / 図書館跡
- **再利用**: 集会所・別の家・図書館跡は同一モジュールの向き / テクスチャ違いで構成
- **VS_SCOPE §3.2 「歩いて 1-2 分で回れる」を遵守**

### 2.2 過去側 (Root_Past) の同エリア

過去版は **現在版と同じ物理レイアウト** を保ちつつ、以下の差分:

- 家族の家: 本棚に **家族の本が並んでいる** (主要違和感の出所)
- 集会所: 人の気配 (NPC 配置はせず、家具配置のみで「使われていた」感)
- 別の家: 灯りがともる雰囲気 (現在は消えている)
- 視覚区別: マテリアルの色温度をやや暖かめにシフト (ADR-0005 §視覚区別 整合)

VS では **過去のジオラマは家族の家 + 図書館跡のみ実装**、集会所 / 別の家の過去側は Stage 4 へ。

---

## 3. Phase 分解

### Phase G0: ゾーン Blockout

**目的**: 物理レイアウトを Unity 内で grey-box 構築。すべて Cube / Plane で配置、見た目はあとで。

**成果物**:
- `Assets/Scenes/Anemora_Main.unity` (E2 で常駐ヒエラルキー scaffold 済み)
- `Anemora_Main` シーン内に `Root_Current/Zone1/` 配下:
  - `House_Player/` (主人公の家、Blockout)
  - `Plaza/` (中央広場、Blockout)
  - `Library_Ruin/` (図書館跡、Blockout)
  - `House_Near/` (別の家、Blockout、Stage 4 candidate)
  - `MeetingHall_Ruin/` (集会所、Blockout、Stage 4 candidate)
- `Root_Past/Zone1/` 配下に **House_Player + Library_Ruin の過去版のみ** Blockout
- `docs/devlog/2026-05-XX_g0_blockout.md` (画面キャプチャ + 規模確認)

**手順**:
1. Cube / Plane で家・広場・建物のサイズ感を作る (Unity の Probuilder か単純 Cube で可)
2. 主人公スプライト (F1 段階のドラフトでも可) を仮置きしてカメラ画角・スケール確認
3. **歩いて 1-2 分で回れる** を守る (家 → 広場 = 30 秒、広場 → 図書館跡 = 30 秒、戻り計 1-2 分)
4. 床マテリアルを 2-3 種で仮塗り (中央広場は石畳風、家の床は木板風)

**重要な判断点**:
- カメラの orthographic size を G0 で確定する (Pixel Per Unit + Camera size + Sprite 32x48 の整合)
- 家の中 / 街 で **カメラ距離を変えるか同一にするか** を決める (VS では同一推奨、視点切替の演出コスト削減)

**担当**: Windows Codex (Unity Scene 編集)
**Linux 関与**: 規模 / カメラ画角 のレビュー

---

### Phase G1: 主人公の家 (内装)

**目的**: オープニング (0:00-1:00) が成立する家の内装を整える。

**成果物**:
- `Assets/Prefabs/Zone1/House_Player.prefab` (内装 prefab、Root_Current 配下)
- `Assets/Prefabs/Zone1/House_Player_Past.prefab` (過去版、Root_Past 配下)
- `Assets/Prefabs/Zone1/Bed_Player.prefab` (ベッド、ニューゲーム start point)
- `Assets/Prefabs/Zone1/Bookshelf_Empty.prefab` (現在版、空)
- `Assets/Prefabs/Zone1/Bookshelf_FamilyBooks.prefab` (過去版、本が並ぶ)
- `Assets/Prefabs/Zone1/Door_House.prefab` (ドア、時の筆発見トリガー付き)
- `Assets/Scripts/TimeManagement/Reflectors/BookshelfReflector.cs` (ActionRecord で本がベッドに現れる反映スクリプト、E5 で定義したものと統合)

**手順**:
1. ベッド / テーブル / 本棚 / ドアの 3D モデルを Meshy v6 LowPoly で生成 (ADR-0003)、Blender で破綻補正
2. House_Player prefab に配置、Bed_Player を **NewGame の起点** として `SpawnPoint_Newgame` タグを付与
3. Door_House に Trigger collider を設置、入退室判定
4. Door_House の **入口 (家から外への向き) に近づくとイベント発火**: 「ポケットに何かを感じる → 時の筆を取り出す」演出 (UI / Animator / SFX)
5. 過去版 House_Player_Past を作成、Bookshelf を `Bookshelf_FamilyBooks` に差替

**含み演出 D-3 / D-7 / D-6 弱版の組込み**:
- ニューゲーム時、Bed_Player から起き上がる Animator State を **2 状態に分離**:
  - State A: D-3 (夢を見ていたような…) — テキスト + Fade-in 1 秒
  - State B: D-7 (手を見る) — F4 のハンドアセットを Animator でフラッシュ表示 0.5 秒、その後通常立ち姿
  - (任意) State C: D-6 弱版 (体がだるい) — テキスト 1 行のみ、`gameSettings.tutorial.show_d6_weak = false` で削除可能
- 状態遷移は scripted、自動進行 (プレイヤー入力で待機しない、空気感優先)

**担当**: Windows Codex (Meshy / Blender / Unity 配置), ユーザー (家具レイアウトの最終判断)
**Linux 関与**: 含み演出スクリプト構造のレビュー、テキスト文言の校正

---

### Phase G2: 中央広場 + 周辺建物

**目的**: 街の中心 + 主要違和感の場所 (図書館跡) を整える。

**成果物**:
- `Assets/Prefabs/Zone1/Plaza_Center.prefab` (中央広場、ベンチ or 噴水跡)
- `Assets/Prefabs/Zone1/Library_Ruin.prefab` (図書館跡、現在版)
- `Assets/Prefabs/Zone1/Library_Ruin_Past.prefab` (過去版、家族の本がある場所)
- `Assets/Prefabs/Zone1/StreetLamp.prefab` × 2-3 (再利用)
- `Assets/Prefabs/Zone1/Tree_Decay.prefab` × 2-3 (再利用、衰退表現)
- 床タイル: `Assets/Art/Materials/Floor_Stone.mat` (石畳), `Assets/Art/Materials/Floor_Wood.mat` (木板)

**手順**:
1. Plaza_Center に「失われた何か」のモニュメント風オブジェクト (ベンチ / 噴水跡 / 廃墟風像) を配置
2. Library_Ruin は **小さな建物** (大きすぎず、入口だけ見えれば十分)
3. Library_Ruin_Past は同じ位置に「使われていた図書館」、家族の本 (Book_Family.prefab、E5 で取れるもの) を 1 個配置
4. 中央広場 → 図書館跡 = 30 秒以内の動線
5. 街灯 / 樹を再利用配置で街並みを構成

**重要な配慮**:
- **異物原則**: 街の見た目に「異界アイテム」を入れない (浮遊する石、機械、紋様などは置かない)
- **HD-2D Tier 2**: 動的影 + 単一方向光が乗りやすいシルエット (枠線がはっきりした構造)
- **静謐 / 衰退**: 派手な装飾を避け、塀の崩れ / 樹の落葉 / 灯りの消えた窓などで表現

**担当**: Windows Codex
**Linux 関与**: 異物原則違反がないかのレビュー

---

### Phase G3: NPC 配置

**目的**: 普通の住人 1-2 人を配置、対話と痕跡反映の検証。

**成果物**:
- `Assets/Art/Sprites/NPC/Resident_A/v1/` (NPC 1 スプライト、4 方向 + Idle、F トラック後段で生成)
- `Assets/Art/Sprites/NPC/Resident_B/v1/` (NPC 2 スプライト、同上)
- `Assets/Prefabs/NPC_Resident_A.prefab`
- `Assets/Prefabs/NPC_Resident_B.prefab`
- `Assets/ScriptableObjects/Dialogues/Resident_A_Dialogue.asset` (対話データ)
- `Assets/ScriptableObjects/Dialogues/Resident_B_Dialogue.asset`

**手順**:
1. F トラック完了後、NPC スプライトを F1-F4 と同手順で生成 (PixelLab + Aseprite)
2. NPC 1: 中央広場の西側、立ち姿 (時々歩く程度のごく軽いアニメ)
3. NPC 2: 中央広場の東側、ベンチに座っている (椅子座 Idle)
4. 対話データ: ScriptableObject で台詞 + 痕跡反映後の差分を保持
   - **ActionRecord を反映していない場合の台詞**: 普通の世間話 (天気、街の様子)
   - **本持ち帰り後の台詞 (NPC 1)**: 「あの図書館、随分前から閉まってるはずなのに…」
   - **本持ち帰り後の台詞 (NPC 2)**: 「不思議だね、何か変わった気がする」
5. NPC との対話は ADR-0007 §対話 UI に従って実装

**重要な配慮**:
- **異物原則**: NPC は普通の住人。語り部 / 守り人 / 前任者などの特殊役割を持たせない
- **silent protagonist**: 対話は NPC 一方的話 + 主人公感情/反応の選択肢のみ (主人公はしゃべらない)
- **痕跡反映の見える形**: NPC 1 か NPC 2 のどちらか **少なくとも 1 人に「現在反映」が見える** (VS_SCOPE §8 推奨)

**担当**: Windows Codex (生成 + Unity 実装), Linux Claude (対話文言の起草 + レビュー), ユーザー (NPC キャラクター方向性の最終判断)

---

### Phase G4: ActionRecord トリガー設置

**目的**: E5 の ActionRecord 機構と G ゾーンを統合。「過去で本を取る → 帰還後ベッドに本が現れる」の主要違和感を成立させる。

**成果物**:
- `Assets/Prefabs/Zone1/Book_Family_Past.prefab` (過去版図書館跡に置かれた本、Interactable)
- `Assets/Prefabs/Zone1/Book_Family_Current.prefab` (現在版、ベッドに現れる本のプレースホルダ)
- `Assets/ScriptableObjects/ActionRecords/ActionRecordCatalog.asset` に以下のエントリ追加:
  - `actionId = "take_book_family_001"`
  - `type = TakeItem`
  - `currentSideEffect = SpawnBookOnBed_Player`
- `Assets/Scripts/TimeManagement/Reflectors/BookshelfReflector.cs` の SpawnBookOnBed_Player ロジック実装 (E5 で書いた BookReflector を本ゾーンに特化)

**手順**:
1. 過去版 Library_Ruin_Past に Book_Family_Past を配置 (Interactable component)
2. プレイヤーが Book_Family_Past に近づき E キー → `ActionRecordStore.Add(...)` で記録、本を非表示
3. PortalCrossingDetector の Past → Current 反転イベントで ActionRecordStore をスキャン
4. SpawnBookOnBed_Player を実行: Bed_Player の上に Book_Family_Current.prefab を Instantiate
5. 帰還して家に戻ると、ベッドの上に本がある = **主要違和感の痕跡**

**検証**:
- 過去で本を取らずに帰還 → ベッドに本は現れない
- 過去で本を取って帰還 → ベッドに本が現れる
- 同じエントリが二重反映されない

**担当**: Windows Codex (Unity 実装、E5 と統合)
**Linux 関与**: ActionRecord エントリ設計レビュー

---

### Phase G5: 通し体験 + 含み演出統合 + 層 2 片鱗 + BGM/SFX

**目的**: 10-15 分の通し体験を成立させ、VS_SCOPE §8 必須項目をすべて pass する。

**成果物**:
- 通しプレイ動画 (Editor + Windows ビルド版の両方)
- `docs/devlog/2026-05-XX_g_track_walkthrough.md` (体験記録 + 検証マトリクス §7)
- 街アンビエント BGM 1 曲 (`Assets/Audio/Music/Zone1_Ambient.ogg`、AIVA Pro + Reaper、ADR-0003)
- 環境音 5-8 種 (`Assets/Audio/SFX/env/*.ogg`、ElevenLabs SFX v2)
- 時の窓 SFX 5 種 (`Assets/Audio/SFX/timeframe/*.ogg`)
- 層 2 片鱗演出 1 カット (`Assets/Prefabs/Zone1/Layer2_Hint.prefab`、VS フィナーレで発火)
- Windows ビルド (`Builds/Windows/Anemora_VS.exe` 等)

**手順**:
1. ニューゲーム → オープニング (D-3/D-7/D-6) → ドア前で時の筆 → 外へ → 中央広場 → 図書館跡 → 過去踏込み → 本取得 → 帰還 → ベッドに本確認 → NPC 対話で痕跡確認 → 層 2 片鱗演出 → VS 終端
2. 各段階で **何が見えるか / 何が聞こえるか / 何が動くか** を devlog に記録
3. 層 2 片鱗 1 カット: VS 終端付近で「過去で持ち帰った本が、未来時代を覗くと別の場所にある」演出 (VS_SCOPE §3.4)
4. BGM / SFX を統合、音量バランス調整
5. Windows ビルドで通し再現
6. 検証マトリクス §7 を埋める

**担当**: Windows Codex (実装 + 計測), Linux Claude (検証マトリクス整理 + ADR 改訂判断)

---

## 4. 含み演出の実装ノート (D-3 / D-7 / D-6 弱版)

### 4.1 D-3: 夢を見ていたような、夢を見ていなかったような

- **発火タイミング**: ニューゲーム → 黒画面 → Fade-in 開始の最初 1 秒
- **実装**:
  - 黒画面でテキスト UI に「夢を見ていたような、夢を見ていなかったような…」を 1 行表示
  - 1 秒後に消え、ベッドの主人公に Fade-in
  - SFX: 静寂 + 微かな風音 (`Assets/Audio/SFX/env/wind_subtle.ogg`)
- **テキスト変更可能**: `gameSettings.tutorial.d3_text` で文言調整可能 (Stage 3 試作で確定)

### 4.2 D-7: 手を見る一瞬の動作

- **発火タイミング**: 主人公がベッドから起き上がる Animator State の中、Frame 5-10
- **実装**:
  - F4 の `hero_hands_d7.png` を Animator で短時間 (0.3-0.5 秒) overlay 表示
  - カメラはほんのわずかにズーム (orthographic size を 0.05 だけ縮小、すぐ戻す)
  - SFX: 環境音のみ (派手な効果音は入れない)
- 終了後、通常の立ち姿 Animator State に遷移

### 4.3 D-6 弱版: 体がだるい (削除可能)

- **発火タイミング**: D-7 の直後、立ち姿になって最初の 1 歩を踏み出すまで
- **実装**:
  - テキスト UI に「(今日は体がだるい)」を 1 行、2 秒表示
  - `gameSettings.tutorial.show_d6_weak = false` で削除可能 (`STAGE3_TBD_RESOLUTION.md` §4.3 削除可能フラグ)
- **削除判定**: Stage 3 試作で実体験して、家族の偽記憶と重なって過剰に感じたら削除する

---

## 5. ディレクトリ配置 (ADR-0004 準拠)

```
Assets/
├── Prefabs/
│   ├── Hero.prefab                            (F4)
│   ├── Zone1/
│   │   ├── House_Player.prefab                (G1, Current)
│   │   ├── House_Player_Past.prefab           (G1, Past)
│   │   ├── Bed_Player.prefab
│   │   ├── Bookshelf_Empty.prefab
│   │   ├── Bookshelf_FamilyBooks.prefab
│   │   ├── Door_House.prefab
│   │   ├── Plaza_Center.prefab                (G2)
│   │   ├── Library_Ruin.prefab                (G2)
│   │   ├── Library_Ruin_Past.prefab           (G2)
│   │   ├── House_Near.prefab                  (G2, Stage 4 候補)
│   │   ├── MeetingHall_Ruin.prefab            (G2, Stage 4 候補)
│   │   ├── StreetLamp.prefab                  (G2)
│   │   ├── Tree_Decay.prefab                  (G2)
│   │   ├── Book_Family_Past.prefab            (G4)
│   │   ├── Book_Family_Current.prefab         (G4)
│   │   └── Layer2_Hint.prefab                 (G5)
│   ├── NPC_Resident_A.prefab                  (G3)
│   └── NPC_Resident_B.prefab                  (G3)
├── ScriptableObjects/
│   ├── ActionRecords/
│   │   └── ActionRecordCatalog.asset          (E5 + G4)
│   └── Dialogues/
│       ├── Resident_A_Dialogue.asset          (G3)
│       └── Resident_B_Dialogue.asset          (G3)
├── Scenes/
│   └── Anemora_Main.unity                     (G0-G5、E2 で常駐ヒエラルキー scaffold)
├── Audio/
│   ├── Music/
│   │   └── Zone1_Ambient.ogg                  (G5)
│   └── SFX/
│       ├── env/                               (G5)
│       │   ├── wind_subtle.ogg
│       │   ├── footstep_stone_*.ogg
│       │   └── footstep_wood_*.ogg
│       └── timeframe/                         (G5)
│           ├── portal_open.ogg
│           ├── symbol_select.ogg
│           ├── crossing.ogg
│           ├── take_item.ogg
│           └── return.ogg
└── Art/
    └── Sprites/
        ├── Hero/v1/                           (F4)
        └── NPC/
            ├── Resident_A/v1/                 (G3)
            └── Resident_B/v1/                 (G3)
```

---

## 6. 依存関係 / 並列性

```
F4 ─┐
    ├─ G3 (NPC sprite が必要)
    │
E0-E2 ─┐
       ├─ G0 (Scene scaffold は E2 で用意される)
       │
E3-E4 ─┤
       ├─ G1 + G2 (家 + 街 Blockout) — E と並列着手可
       │
E5 ────┤
       └─ G4 (ActionRecord 統合) — E5 が前提
                                                 │
                                                 ↓
                                                 G5 (通し体験)
```

- **G0 は E2 完了後**に着手 (Anemora_Main scene が E2 で作られる)
- **G1 / G2 は E2 完了後** + Blockout 段階なら並列着手可
- **G3 は F4 完了後** (NPC sprite がないと配置できない)
- **G4 は E5 完了後** (ActionRecord 機構統合)
- **G5 は G1-G4 + E6 + F5 すべて完了後** (通し体験)

---

## 7. 検証マトリクス

| ID | 項目 | 必須 | 確認方法 |
|---|---|---|---|
| V1 | 歩いて 1-2 分で家 → 中央広場 → 図書館跡 が回れる | ✓ | G0 / G2 |
| V2 | 主人公の家のオープニングが破綻なく動く (D-3 / D-7 / D-6) | ✓ | G1 単体 + 通し |
| V3 | ドア前で時の筆発見演出が機能 | ✓ | G1 単体 |
| V4 | 中央広場のレイアウトが「静謐 / 衰退」を表現 | ✓ | G2 ユーザー判断 |
| V5 | 異物原則違反がない (奇抜な装飾なし) | ✓ | G1-G4 全段階 |
| V6 | NPC 1-2 人と対話可能 | ✓ | G3 |
| V7 | NPC のいずれかに「現在反映」が見える | ✓ | G4 + G5 |
| V8 | 過去で本を取って帰還、ベッドに本が現れる | ✓ | G4 (E5 統合) |
| V9 | 過去で本を取らないと痕跡なし | ✓ | G4 |
| V10 | 同一エントリの二重反映なし | ✓ | G4 |
| V11 | 層 2 片鱗演出 1 カットが入る | ✓ | G5 |
| V12 | 街アンビエント BGM 1 曲が流れる | ✓ | G5 |
| V13 | 時の窓 SFX 5 種が鳴る | ✓ | G5 |
| V14 | 環境音 5-8 種が機能 | ✓ | G5 |
| V15 | ニューゲームから VS 終端まで 10-15 分で通しプレイ | ✓ | G5 |
| V16 | Windows ビルドで通し再現 | ✓ | G5 |
| V17 | 詰みが起きない (時の窓再描画でリトライ可) | ✓ | G5 |
| V18 | HD-2D Tier 2 (動的影 + 単一方向光) で破綻しない | ✓ | G2 / G5 |
| V19 | カメラの orthographic size が 32x48 sprite と整合 | ✓ | G0 |
| V20 | Pixel Perfect で整数解像度が崩れない | △ | G5 |

`✓` = G トラック完了に必須、`△` = 観察項目。

---

## 8. リスクと早期検知

| リスク | 兆候 | 対応 |
|---|---|---|
| 街が広すぎ / 狭すぎ | G0 で歩行時間が 30 秒未満 or 3 分超 | Blockout を Cube 単位で再配置、目安 1-2 分を死守 |
| 異物原則違反 (Meshy 出力に奇抜要素) | G2 段階で建物に異界感 | Blender で破綻補正時に派手要素を削除、再生成は ADR-0003 §人手仕上げ で吸収 |
| HD-2D Tier 2 で動的影が乗らない | G2 / G5 で影が消える / 線が潰れる | E トラック側で Forward Renderer 設定見直し、必要なら ADR-0002 改訂 |
| カメラ orthographic size 不整合 | G0 で sprite が小さすぎ / 大きすぎ | Pixel Per Unit / orthographic size を再計算、F_PLAN §4.1 と同期 |
| NPC の「普通の住人」が没個性 | G3 で NPC スプライトが背景化 | 服装の色 / シルエットで区別、ただし異物原則は維持 |
| 主要違和感が伝わらない | G5 通し試遊で「何が起きたか分からない」 | 痕跡反映を NPC セリフ + 視覚 + SFX の 3 重で見せる、説明テキスト追加も可 |
| 層 2 片鱗が直接的すぎ | G5 通し試遊でユーザーが「ああこの仕組みね」と感じる | 演出を弱める、台詞を抽象化、考察余地を残す (`feedback_anemora_no_premature_lockin` 整合) |
| 通し時間が 15 分超過 | G5 で 18-20 分かかる | サイド違和感削除、NPC 対話短縮、VS_SCOPE §9 削減トリガー発火 |
| Windows ビルドで Editor 差異 | G5 ビルド時に挙動変化 | `Library/` 削除して再 import、ADR-0006 atomic write の挙動確認 |

---

## 9. 担当割り

| Phase | Linux Claude | Windows Codex | ユーザー |
|---|---|---|---|
| G0 Blockout | 規模 / カメラ画角レビュー | Unity Scene 編集 | 規模感最終判断 |
| G1 主人公の家 | 含み演出スクリプト構造レビュー、テキスト校正 | Meshy / Blender / Unity 配置、Animator | 家具レイアウト判断 |
| G2 中央広場 + 周辺 | 異物原則違反レビュー | 3D 生成、Blender 補正、Unity 配置 | 静謐 / 衰退の表現方向最終判断 |
| G3 NPC 配置 | 対話文言起草 + レビュー | NPC スプライト生成 (F の手順を流用)、Unity 配置 | NPC キャラクター方向性 |
| G4 ActionRecord | エントリ設計レビュー | Unity 実装、E5 と統合 | (確認のみ) |
| G5 通し体験 + 演出 | 検証マトリクス整理、ADR 改訂判断、層 2 片鱗の演出強度レビュー | BGM / SFX 統合、Windows ビルド、通し試遊 | 通し体験の最終判断 (10-15 分が成立しているか) |

---

## 10. Stage 4 への持ち越し候補

VS で実装しないが、Stage 4 で着手する候補:

- House_Near (NPC 2 の家)、MeetingHall_Ruin (集会所) の本実装
- ゾーン 2-5 (B/C/D/E) の構築
- メトロイドヴァニア要素 (探索的バリア)
- インベントリ / 進行ログ UI
- 白 / 青シンボルの本実装
- 戦闘がないままの「告げる / 押す/動かす」能動行動

---

## 11. 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v0.1 | 2026-05-04 | Stage 3 Day 1 起草、Phase G0-G5 + 含み演出統合 + 検証マトリクス + 担当割り定義 |
