# G5 Acceptance Matrix Draft

Status: Draft for G5 execution

Purpose: Stage 3 Day 1 の G5 通し体験で、VS_SCOPE §8 の完了条件と E0-E5 / A2 / G4 / F4 / G3 / Audio / Windows build を一括検証するための記入用マトリクス。

Usage:

- `実測 (G5 で記入)` と `pass-fail (G5 で記入)` は G5 実行時に記入する。
- `pass-fail` は `Pass` / `Fail` / `Blocked` のいずれかで記録する。
- 事前注記が必要な項目のみ `備考` に記載する。
- 文書作成時点では実測検証を行わない。

## A. Engine / Pipeline (E0-E1)

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| A-01 | Engine / Pipeline | `Assets/Scenes/Anemora_Main.unity` を開き、Editor Play を開始する。Console と Game view を確認する。 | URP pipeline で scene が起動し、pipeline 初期化エラーが出ない。 |  |  | E0-E1 baseline。 |
| A-02 | Engine / Pipeline | 赤シンボルで portal を開き、portal mask / inside 表示と境界越えを確認する。必要なら Frame Debugger で stencil 設定を見る。 | PortalStencilFeature が stencil bit 3 を使い、Mask = 8 / Ref = 8 で portal 表示が破綻しない。 |  |  | ADR-0002 v1.1 参照。 |
| A-03 | Engine / Pipeline | portal 表示中に URP RenderGraph / lighting 周辺の Console warning と見た目を確認する。 | StencilLight 予約 bit 4 と競合せず、DrawObjectsPass internal API 経路の RenderGraph compatibility caveat に起因する実害がない。 |  |  | warning が出る場合は文言を実測欄へ転記。 |

## B. Scene / Hierarchy (E2)

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| B-01 | Scene / Hierarchy | `Anemora_Main` の Hierarchy で `SceneRootRegistry`、Current / Past root、camera skeleton を確認する。 | 常駐ヒエラルキーが構築済みで、`SceneRootRegistry` と `Camera_Past` skeleton が存在する。VS は Main Camera の culling 反転で動作する。 |  |  | ADR-0005 v1.1 参照。 |
| B-02 | Scene / Hierarchy | Project Settings の layer 定義と scene 内 object の layer assignment を確認する。 | Layer 8 / 10 / 11 が期待どおり割り当てられ、Current / Past / portal 関連の culling と stencil 対象が混線しない。 |  |  | 具体的な layer 名も実測欄に記録。 |

## C. Symbol / Portal (E3-E4)

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| C-01 | Symbol / Portal | Player 操作で SymbolWheel を表示する。表示状態と選択可否を確認する。 | 3 シンボルが表示され、赤シンボルのみ active、白 / 青は preview または disabled として選択不可。 |  |  | VS では赤のみ実装。 |
| C-02 | Symbol / Portal | 赤シンボルを選択する。portal 生成、VFX、collider / detector 状態を確認する。 | 赤シンボル選択で portal が開き、Player が進入可能な状態になる。 |  |  |  |
| C-03 | Symbol / Portal | Current 側から境界を Past 方向へ踏み越える。camera / layer / stencil と Player 位置を確認する。 | Current から Past へ flip し、Player が Past 側に snap され、見た目と操作が継続する。 |  |  |  |
| C-04 | Symbol / Portal | Past 側から境界を Current 方向へ戻る。 | Past から Current へ flip し、帰還後も Player 操作と camera 表示が破綻しない。 |  |  |  |
| C-05 | Symbol / Portal | 境界付近で小刻みに移動し、flip の過剰発火、flash、cooldown を確認する。 | hysteresis band 0.02m、minimum normal movement 0.05m、flip cooldown 0.1s、flash duration 0.05s 相当で動作する。 |  |  | ADR-0005 v1.1 確定値。 |
| C-06 | Symbol / Portal | PlayMode test `AnemoraMainPortalWiringRoundTripTests` を実行または最新 green 結果を確認する。 | 境界往復 PlayMode test が green。 |  |  | G5 時点の実行結果を記録。 |

## D. ActionRecord (E5 + G4)

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| D-01 | ActionRecord | Past 側の book interactable に近づき、Interact 操作で本を取得する。ActionRecord runtime state を確認する。 | 本取得で `ActionRecordRuntime` に該当 entry が 1 件追加される。 |  |  | action id は G4 実装値を記録。 |
| D-02 | ActionRecord | 本取得後、Current 側へ帰還し、Player house / Bed 周辺を確認する。 | `BookReflector` が反応し、Bed 上に `Book_Family_Current` が spawn する。 |  |  |  |
| D-03 | ActionRecord | 同じ save / session で再度 Past から Current へ帰還する。 | 2 回目以降の帰還で book は重複せず、Current 側の反映済み book は 1 個のみ。 |  |  | 二重反映防止。 |
| D-04 | ActionRecord | PlayMode test `G4ActionRecordReflectionE2ETests` を実行または最新 green 結果を確認する。 | G4 E2E PlayMode test が green。 |  |  | G5 時点の実行結果を記録。 |

## E. Buildings / Environment (A3)

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| E-01 | Buildings / Environment | `Assets/Prefabs/Zone1/` の Zone1 building prefab 14 個を scene へ配置または既存配置を確認する。 | 14 building prefab が破損なく scene に配置可能で、missing material / missing mesh がない。 |  |  | A3 完了物で確認。 |
| E-02 | Buildings / Environment | Meshy 生成 asset と Blender 修復済 asset を Game view で近距離 / 通常距離から確認する。 | Meshy 生成 + 3/14 Blender 修復済 asset に大破損、穴、法線崩れ、極端な scale 破綻がない。 |  |  | 品質判断は G5 で screenshot 付き推奨。 |

## F. Character (F4)

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| F-01 | Character | `Hero.prefab` を Player 配下または Player 表示 root に配置し、Idle と移動操作を確認する。 | Hero が Player 配下で表示され、Animator が Idle と Walk を自然に遷移する。 |  |  | F4 完了後に確認。 |
| F-02 | Character | `Resident_A.prefab` と `Resident_B.prefab` を scene に instantiate する。 | Resident_A / Resident_B が prefab として instantiate 可能で、missing sprite / missing animator がない。 |  |  | G3 の NPC 配置前提。 |
| F-03 | Character | Hero / Resident sprite の Texture Import Settings を確認する。 | F2 v1 sprite が PPU 32、Point filter、no mipmap、Alpha is Transparency on で import されている。 |  |  | 対象 sprite path も実測欄に記録。 |

## G. Dialogue / NPC (G3)

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| G-01 | Dialogue / NPC | `Anemora.Game` asmdef の DialogueAsset SO を Project view / runtime で読み込む。 | DialogueAsset ScriptableObject が missing script なしで読み込める。 |  |  | A1 untracked test に Addressables compile error がある場合は解消後に確認。 |
| G-02 | Dialogue / NPC | Resident_A を scene に配置し、対話 SO を投入して Interact 操作を行う。 | Resident_A の台詞が Dialogue UI に表示され、進行不能にならない。 |  |  | silent protagonist 方針に反しないかも確認。 |
| G-03 | Dialogue / NPC | JP / EN localization を切り替え、同じ対話を表示する。 | JP / EN 切替で TMP atlas と fallback が正しく機能し、欠字または tofu が出ない。 |  |  | I-01 / I-02 と関連。 |

## H. Audio (BGM + SFX)

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| H-01 | Audio | 起動後、Zone1 を 3-4 分放置または通しで巡回し、loop point を聴く。 | `Zone1_Ambient` BGM が起動時に再生され、3-4 分でシームレスに loop する。 |  |  | OGG Vorbis quality 6 import 前提。 |
| H-02 | Audio | 時の窓を開く、Past へ入る、Current へ戻る各タイミングで BGM 変調を聴く。 | Low-pass、楽器抜き、pitch shift -2 semitones の変調が意図どおり掛かり、復帰時に破綻しない。 |  |  | 固有 BGM でなく modulation / reuse 可。 |
| H-03 | Audio | 環境、足音、時の窓、NPC、UI の各操作を通しで発火させる。 | SFX 30 種が状況別に再生される。内訳は環境 6、足音 12、時の窓 6、NPC 3、UI 3。 |  |  | 足音は床種別の鳴り分けも確認。 |

## I. UI / Localization

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| I-01 | UI / Localization | VS 文言を JP 表示で通し確認し、TMP warning と表示欠けを確認する。 | TMP 美咲ゴシック JP atlas が表示され、既知 missing 70 字が VS 文言中に出現しない。 |  |  | `2026-05-05_tmp_jp_atlas_v0_1_missing_chars_review.md` 参照。 |
| I-02 | UI / Localization | EN 表示へ切り替え、Title / HUD / Dialogue を確認する。 | Press Start 2P EN atlas が fallback として機能し、英数字 UI が崩れない。 |  |  |  |
| I-03 | UI / Localization | Title、HUD、SymbolWheel、Dialogue、menu を確認する。 | パレット v0 が UI 全体で適用され、未設定色や極端に読みにくい色が残らない。 |  |  | screenshot 推奨。 |

## J. Save / Load

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| J-01 | Save / Load | ActionRecord を 1 件以上追加した状態で save し、session reload または test 経由で load する。 | POCO data 層の `ActionRecordStore` が save / load round-trip で復元される。 |  |  | `ActionRecordStoreTests` / `SaveEnvelopeRoundTripTests` も確認候補。 |
| J-02 | Save / Load | Book reflection 済みの状態を save し、load 後に Current 側 Bed 周辺を確認する。 | ActionRecord の reflected フラグが永続化され、load 後も book が重複 spawn しない。 |  |  | manual save が未実装なら test 経由で確認。 |

## K. Build / Performance

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| K-01 | Build / Performance | Windows Standalone build を実行し、生成物を起動する。 | Windows Standalone build が error なしで成功し、build 起動後に Title から game 本体へ入れる。 |  |  | VS_SCOPE §8 must-pass。 |
| K-02 | Build / Performance | Editor Play と Windows build で通し操作中の FPS を測定する。 | Editor / build の両方で 60 FPS 目標、最低 30 FPS を維持する。 |  |  | 測定環境と resolution を実測欄に記録。 |
| K-03 | Build / Performance | Memory Profiler または Unity Profiler で VRAM / main heap を確認する。 | TMP atlas は JP 約 16 MiB + EN 約 4 MiB の 20 MiB 帯に収まり、main heap に異常な増加がない。 |  |  | screenshot または profiler capture path 推奨。 |

## L. 通し体験 (E2E manual playthrough)

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| L-01 | 通し体験 | Start から開始し、家から外出、街中央広場 / 図書館跡を探索、SymbolWheel 起動、赤シンボル選択、portal open、Past 移動、過去の本取得、Current 帰還、Bed 上 book spawn 確認まで通しでプレイする。 | 一連の flow が softlock なく繋がり、操作、表示、音、反映結果に大きな違和感がない。推定所要時間は 5-8 分。 |  |  | VS 全体目標は 10-15 分。録画推奨。 |

## M. 層 2 片鱗演出 (VS_SCOPE §5.x)

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| M-01 | 層 2 片鱗演出 | VS_SCOPE §3.4 / §4.3 / §5.x と STAGE3_G_PLAN G5 の該当記述を確認し、VS 終盤で定義済みの片鱗演出を再生する。 | 「ルールが書き換わる予兆」を示す 1 カットが画面に現れる。層 2 のルール本体は VS では実装しない。 |  |  | 詳細仕様は VS_SCOPE 参照。本表では項目のみ列挙。 |
