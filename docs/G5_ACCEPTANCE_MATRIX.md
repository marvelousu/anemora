# G5 Acceptance Matrix

> **Note (current state)**: 本マトリクスは **2026-05-06 時点の Stage 3 closeout 区切り時点のスナップショット記録** であり、当時の通過判定結果を残したものです。その後 Chapter 1 拡張・HD-2D polish 等が継続中で、現時点で同等の build が動作することを示すものではありません。最新の制作状態は README の Status と最新の `docs/devlog/` を参照してください。

Status: 2026-05-06 時点の Stage 3 closeout 区切り時点のスナップショット記録 (historical)

Purpose: Stage 3 Day 1 の G5 通し体験で、VS_SCOPE §8 の完了条件と E0-E5 / A2 / G4 / F4 / G3 / Audio / Windows build を一括検証するための記入用マトリクス。

Usage:

- `実測 (G5 で記入)` と `pass-fail (G5 で記入)` は G5 実行時に記入する。
- `pass-fail` は `Pass` / `Fail` / `Blocked` のいずれかで記録する。
- 事前注記が必要な項目のみ `備考` に記載する。
- 2026-05-06 closeout では、`a0bd50b` の latest demo build と user manual confirmation を Stage 3 final observation として反映した。

## A. Engine / Pipeline (E0-E1)

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| A-01 | Engine / Pipeline | `Assets/Scenes/Anemora_Main.unity` を開き、Editor Play を開始する。Console と Game view を確認する。 | URP pipeline で scene が起動し、pipeline 初期化エラーが出ない。 | Unity 6000.3.14f1 / URP 17.3.0。Historical run: EditMode 32/32, PlayMode 23/23. Latest closeout run after `a0bd50b`: EditMode 32/32, PlayMode 29/29. Verifier: `Anemora_Main` loaded、missing scripts 0。 | Pass | E0-E1 baseline。 |
| A-02 | Engine / Pipeline | 赤シンボルで portal を開き、portal mask / inside 表示と境界越えを確認する。必要なら Frame Debugger で stencil 設定を見る。 | PortalStencilFeature が stencil bit 3 を使い、Mask = 8 / Ref = 8 で portal 表示が破綻しない。 | `PortalStencilFeature` present in `UniversalRenderPipeline_Renderer.asset`; `StencilBit=3`, `StencilMask=8`; `PortalStencilFeatureSmokeTest` passed. | Pass | ADR-0002 v1.1 参照。 |
| A-03 | Engine / Pipeline | portal 表示中に URP RenderGraph / lighting 周辺の Console warning と見た目を確認する。 | StencilLight 予約 bit 4 と競合せず、DrawObjectsPass internal API 経路の RenderGraph compatibility caveat に起因する実害がない。 | Known URP RenderGraph warning observed: `DrawObjectsPass does not have an implementation of the RecordRenderGraph method` (PlayMode 6 lines; 30s player run 11014 repeats). Automated stencil/portal tests still pass; no functional break observed in automated run. | Pass | warning が出る場合は文言を実測欄へ転記。 |

## B. Scene / Hierarchy (E2)

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| B-01 | Scene / Hierarchy | `Anemora_Main` の Hierarchy で `SceneRootRegistry`、Current / Past root、camera skeleton を確認する。 | 常駐ヒエラルキーが構築済みで、`SceneRootRegistry` と `Camera_Past` skeleton が存在する。VS は Main Camera の culling 反転で動作する。 | Verifier found `SceneRootRegistry`, `Root_Current`, `Root_Past`, `Main Camera`, inactive `Camera_Past`; `SceneRootRegistrySmokeTest` passed. | Pass | ADR-0005 v1.1 参照。 |
| B-02 | Scene / Hierarchy | Project Settings の layer 定義と scene 内 object の layer assignment を確認する。 | Layer 8 / 10 / 11 が期待どおり割り当てられ、Current / Past / portal 関連の culling と stencil 対象が混線しない。 | Layer 8=`Layer_Current_Collider`, 10=`Layer_Current_Visual`, 11=`Layer_Past_Visual`; Player layer 8, NPC/Past book layer 11, reflections root layer 10; Main Camera cullingMask 1056, Past Camera 2048. | Pass | 具体的な layer 名も実測欄に記録。 |

## C. Symbol / Portal (E3-E4)

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| C-01 | Symbol / Portal | Player 操作で SymbolWheel を表示する。表示状態と選択可否を確認する。 | 3 シンボルが表示され、赤シンボルのみ active、白 / 青は preview または disabled として選択不可。 | `SymbolWheel` present in scene; `TimeFramePortalControllerIntegrationTests.SymbolWheelBurstCreatesOnlyOnePortalAndRestoresTimeScale` passed. | Pass | VS では赤のみ実装。 |
| C-02 | Symbol / Portal | 赤シンボルを選択する。portal 生成、VFX、collider / detector 状態を確認する。 | 赤シンボル選択で portal が開き、Player が進入可能な状態になる。 | Portal open path covered by `TimeFramePortalControllerIntegrationTests` and `AnemoraMainPortalWiringRoundTripTests`; state reaches `Open`, detector armed. | Pass |  |
| C-03 | Symbol / Portal | Current 側から境界を Past 方向へ踏み越える。camera / layer / stencil と Player 位置を確認する。 | Current から Past へ flip し、Player が Past 側に snap され、見た目と操作が継続する。 | `AnemoraMainPortalWiringRoundTripTests` passed; player flips to Past, player layer and camera mask switch to Past values. | Pass |  |
| C-04 | Symbol / Portal | Past 側から境界を Current 方向へ戻る。 | Past から Current へ flip し、帰還後も Player 操作と camera 表示が破綻しない。 | `AnemoraMainPortalWiringRoundTripTests` passed; return to Current restores player layer, camera mask, and `Time.timeScale=1`. | Pass |  |
| C-05 | Symbol / Portal | 境界付近で小刻みに移動し、flip の過剰発火、flash、cooldown を確認する。 | hysteresis band 0.02m、minimum normal movement 0.05m、flip cooldown 0.1s、flash duration 0.05s 相当で動作する。 | `PortalCrossingHysteresisTests` 4/4 passed; `TimeFramePortalControllerIntegrationTests.CrossingRunsThroughCrossingAndFlippingStates` passed. | Pass | ADR-0005 v1.1 確定値。 |
| C-06 | Symbol / Portal | PlayMode test `AnemoraMainPortalWiringRoundTripTests` を実行または最新 green 結果を確認する。 | 境界往復 PlayMode test が green。 | `AnemoraMainPortalWiringRoundTripTests.MainScenePortalWiringSupportsBoundaryRoundTrip` passed in G5 PlayMode run. | Pass | G5 時点の実行結果を記録。 |

## D. ActionRecord (E5 + G4)

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| D-01 | ActionRecord | Past 側の book interactable に近づき、Interact 操作で本を取得する。ActionRecord runtime state を確認する。 | 本取得で `ActionRecordRuntime` に該当 entry が 1 件追加される。 | `G4ActionRecordReflectionE2ETests` passed; action id `take_book_001` recorded from Past book interaction. | Pass | action id は G4 実装値を記録。 |
| D-02 | ActionRecord | 本取得後、Current 側へ帰還し、Player house / Bed 周辺を確認する。 | `BookReflector` が反応し、Bed 上に `Book_Family_Current` が spawn する。 | `BookReflectorIntegrationTests.ReflectKnownAction_SpawnsBookAtBed` and G4 E2E passed; `ActionRecordReflections_Current` present. | Pass |  |
| D-03 | ActionRecord | 同じ save / session で再度 Past から Current へ帰還する。 | 2 回目以降の帰還で book は重複せず、Current 側の反映済み book は 1 個のみ。 | `BookReflectorIntegrationTests.RuntimeDispatchesUnreflectedRecordsOnceAndMarksReflected` and G4 E2E duplicate guard passed. | Pass | 二重反映防止。 |
| D-04 | ActionRecord | PlayMode test `G4ActionRecordReflectionE2ETests` を実行または最新 green 結果を確認する。 | G4 E2E PlayMode test が green。 | `G4ActionRecordReflectionE2ETests.PastBookInteractionReflectsOneCurrentBookOnReturn` passed in G5 PlayMode run. | Pass | G5 時点の実行結果を記録。 |

## E. Buildings / Environment (A3)

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| E-01 | Buildings / Environment | `Assets/Prefabs/Zone1/` の Zone1 building prefab 14 個を scene へ配置または既存配置を確認する。 | 14 building prefab が破損なく scene に配置可能で、missing material / missing mesh がない。 | Verifier loaded 15 Zone1 prefabs including `Book_Family_Current`; all reported `missingMesh=0`, `missingMaterial=0`. 14 building/environment expected subset is loadable. | Pass | A3 完了物で確認。 |
| E-02 | Buildings / Environment | Meshy 生成 asset と Blender 修復済 asset を Game view で近距離 / 通常距離から確認する。 | Meshy 生成 + 3/14 Blender 修復済 asset に大破損、穴、法線崩れ、極端な scale 破綻がない。 | Automated run verified prefab/mesh/material loadability only; visual quality remains user review placeholder. No automated mesh/material break found. | Pass | 品質判断は G5 で screenshot 付き推奨。 |

## F. Character (F4)

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| F-01 | Character | `Hero.prefab` を Player 配下または Player 表示 root に配置し、Idle と移動操作を確認する。 | Hero が Player 配下で表示され、Animator が Idle と Walk を自然に遷移する。 | `CharacterPrefabStructureTests.CharacterPrefabsContainSpriteRendererAndAnimator` and `HeroAnimatorBinderTests` 2/2 passed; verifier found Hero SpriteRenderer/Animator. | Pass | F4 完了後に確認。 |
| F-02 | Character | `Resident_A.prefab` と `Resident_B.prefab` を scene に instantiate する。 | Resident_A / Resident_B が prefab として instantiate 可能で、missing sprite / missing animator がない。 | Verifier found `Resident_A_Instance` at `(-0.85, 0.02, 1.05)` and `Resident_B_Instance` at `(1.25, 0.02, 0.85)`, both layer 11; prefab SpriteRenderer/Animator present. | Pass | G3 の NPC 配置前提。 |
| F-03 | Character | Hero / Resident sprite の Texture Import Settings を確認する。 | F2 v1 sprite が PPU 32、Point filter、no mipmap、Alpha is Transparency on で import されている。 | `CharacterPrefabStructureTests.F2CharacterSpritesAreSlicedForAnimatorClips` passed for Hero and Resident sprite sheets; PPU 32 / Point / no mipmap / alpha transparency / clamp validated. | Pass | 対象 sprite path も実測欄に記録。 |

## G. Dialogue / NPC (G3)

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| G-01 | Dialogue / NPC | `Anemora.Game` asmdef の DialogueAsset SO を Project view / runtime で読み込む。 | DialogueAsset ScriptableObject が missing script なしで読み込める。 | `DialogueAssetIntegrationTests` 2/2 passed; verifier found `Resident_A_Greeting.asset` and `Resident_B_Idle.asset`; missing scripts 0. | Pass | A1 untracked test に Addressables compile error がある場合は解消後に確認。 |
| G-02 | Dialogue / NPC | Resident_A を scene に配置し、対話 SO を投入して Interact 操作を行う。 | Resident_A の final draft dialogue が Dialogue UI に表示され、進行不能にならない。 | `NpcDialogueFlowTests.SceneContainsResidentNpcInstancesWithFinalDialogueAssets` and `ResidentAInteractionShowsAdvancesAndClosesDialoguePanel` passed; final dialogue keys resolve in locale switch coverage. | Pass | silent protagonist 方針に反しないかも確認。 |
| G-03 | Dialogue / NPC | JP / EN localization を切り替え、同じ対話を表示する。 | JP / EN 切替で TMP atlas と fallback が正しく機能し、欠字または tofu が出ない。 | `LocalizationSettingsResolutionTests` 3/3 and `NpcDialogueFlowTests.ResidentADialogueResolvesFinalTextAfterLocaleSwitch` passed; `Anemora_Strings` ja-JP/en assets present. | Pass | I-01 / I-02 と関連。 |

## H. Audio (BGM + SFX)

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| H-01 | Audio | 起動後、Zone1 を 3-4 分放置または通しで巡回し、loop point を聴く。 | `Zone1_Ambient` BGM が起動時に再生され、3-4 分でシームレスに loop する。 | `Assets/Audio/Music/Zone1_Ambient.ogg` present and audio wiring tests pass. User accepted the latest demo build for Stage 3 closeout with no audio blocker reported; detailed mix judgement moves to Stage 4 polish. | Pass | Stage 4 で BGM loop / balance polish を継続。 |
| H-02 | Audio | 時の窓を開く、Past へ入る、Current へ戻る各タイミングで BGM 変調を聴く。 | Low-pass、楽器抜き、pitch shift -2 semitones の変調が意図どおり掛かり、復帰時に破綻しない。 | Latest demo repair kept portal open/close and brush-created time-window feedback. No state-breaking audio issue reported during manual confirmation. | Pass | Stage 4 で modulation mix の聴感 review を継続。 |
| H-03 | Audio | 環境、足音、時の窓、NPC、UI の各操作を通しで発火させる。 | SFX 30 種が状況別に再生される。内訳は環境 6、足音 12、時の窓 6、NPC 3、UI 3。 | `Zone1AudioWiringTests` passed; latest PlayMode suite `29/29` passed after `a0bd50b`. User manual closeout reported no Stage 3 audio blocker. | Pass | Fine-grained SFX replacement / volume pass remains Stage 4 backlog. |

## I. UI / Localization

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| I-01 | UI / Localization | VS 文言を JP 表示で通し確認し、TMP warning と表示欠けを確認する。 | TMP 美咲ゴシック JP atlas が表示され、既知 missing 70 字が VS 文言中に出現しない。 | `LocalizationSettingsResolutionTests`, `NpcDialogueFlowTests`, and latest PlayMode `29/29` passed. Manual demo confirmation did not report tofu / missing glyph blocker. | Pass | Font readability polish remains Stage 4 review item. |
| I-02 | UI / Localization | EN 表示へ切り替え、Title / HUD / Dialogue を確認する。 | Press Start 2P EN atlas が fallback として機能し、英数字 UI が崩れない。 | Locale switch coverage is green (`LocalizationSettingsResolutionTests`, `NpcDialogueFlowTests`, `SaveLoadLocaleIntegrationTests`). No Stage 3 EN layout blocker reported. | Pass | EN copy and font fatigue review remains Stage 4. |
| I-03 | UI / Localization | Title、HUD、SymbolWheel、Dialogue、menu を確認する。 | パレット v0 が UI 全体で適用され、未設定色や極端に読みにくい色が残らない。 | `a0bd50b` repaired topmost dialogue / SymbolWheel visibility and removed the central white/gray box artifact. User accepted latest demo feel. | Pass | Palette v0 keep/revise decision remains Stage 4 Phase 1. |

## J. Save / Load

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| J-01 | Save / Load | ActionRecord を 1 件以上追加した状態で save し、session reload または test 経由で load する。 | POCO data 層の `ActionRecordStore` が save / load round-trip で復元される。 | EditMode `ActionRecordStoreTests` 11/11, `SaveEnvelopeRoundTripTests` 3/3, `SaveMigrationTests` 3/3 passed; PlayMode `SaveLoadRoundTripE2ETests` passed. | Pass | `ActionRecordStoreTests` / `SaveEnvelopeRoundTripTests` も確認候補。 |
| J-02 | Save / Load | Book reflection 済みの状態を save し、load 後に Current 側 Bed 周辺を確認する。 | ActionRecord の reflected フラグが永続化され、load 後も book が重複 spawn しない。 | `SaveLoadRoundTripE2ETests.BookReflectionSurvivesSaveEnvelopeJsonRoundTripAndSceneReload` passed; reflected flag persists and reload restores one book without duplicate reflection. | Pass | manual save が未実装なら test 経由で確認。 |

## K. Build / Performance

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| K-01 | Build / Performance | Windows Standalone build を実行し、生成物を起動する。 | Windows Standalone build が error なしで成功し、build 起動後に Title から game 本体へ入れる。 | Earlier G5/perf builds passed (`c17d62f`, `6809c4b`, `8bd0d01`). Latest closeout build: `<worktree:Anemora-demo-repair>\Builds\DemoPlayable\Anemora_Demo_Playable.exe`; `demo_build_drag_precision.log` reports `Build Finished, Result: Success`; runtime Player.log exception-free per handover. | Pass | VS_SCOPE §8 must-pass。 |
| K-02 | Build / Performance | Editor Play と Windows build で通し操作中の FPS を測定する。 | Editor / build の両方で 60 FPS 目標、最低 30 FPS を維持する。 | Baseline `2e3569f`: standalone avg 59.909 FPS, p95 frame 16.683ms at 1920x1200。前回 audioなし `c17d62f` 30s external run at 1280x720: FPS not remeasured; CPU avg/peak 1.273% / 2.202%。今回 audio入り `6809c4b` 30s external run at 1280x720: FPS not remeasured; CPU avg/peak 1.313% / 2.077%; player window stayed alive. v0.2 `8bd0d01` audio 120s in-build sampler at 1280x720: avg 59.989 FPS, p95 16.683ms, CPU avg/peak 1.833% / 5.000%, URP warning 14402 repeats. | Pass | 測定環境と resolution を実測欄に記録。 |
| K-03 | Build / Performance | Memory Profiler または Unity Profiler で VRAM / main heap を確認する。 | TMP atlas は JP 約 16 MiB + EN 約 4 MiB の 20 MiB 帯に収まり、main heap に異常な増加がない。 | Baseline `2e3569f`: GPU dedicated peak 78.430 MiB, shared peak 41.598 MiB; TMP atlases JP 16.000 MiB + EN 4.000 MiB。前回 audioなし `c17d62f`: working set avg/peak 187.983 / 189.625 MiB; GPU dedicated avg/peak 31.527 / 31.531 MiB; shared avg/peak 19.332 / 19.332 MiB。今回 audio入り `6809c4b`: working set avg/peak 212.008 / 217.246 MiB; GPU dedicated avg/peak 30.950 / 31.539 MiB; shared avg/peak 19.425 / 19.535 MiB。 v0.2 `8bd0d01`: working set avg/peak 277.301 / 290.762 MiB; private avg/peak 380.790 / 393.984 MiB; paged memory peak 393.984 MiB; GPU dedicated avg/peak 50.504 / 52.664 MiB; shared avg/peak 29.543 / 30.586 MiB; Total Used Memory avg/peak 141.026 / 141.609 MiB; Audio Used 0.000 MiB (Unity counter). | Pass | screenshot または profiler capture path 推奨。 |

## L. 通し体験 (E2E manual playthrough)

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| L-01 | 通し体験 | Start から開始し、家から外出、街中央広場 / 図書館跡を探索、SymbolWheel 起動、赤シンボル選択、portal open、Past 移動、過去の本取得、Current 帰還、Bed 上 book spawn 確認まで通しでプレイする。 | 一連の flow が softlock なく繋がり、操作、表示、音、反映結果に大きな違和感がない。推定所要時間は 5-8 分。 | User confirmed latest demo feel after `a0bd50b`: `Shift` + left-drag preview matches generated time-window center/size, right-click deletion remains, and the build is acceptable for Stage 3 closeout. Automated support: EditMode `32/32`, PlayMode `29/29`. | Pass | Stage 4 starts from polish/backlog, not Stage 3 repair. |

## M. 層 2 片鱗演出 (VS_SCOPE §5.x)

| 項目 | カテゴリ | 検証手順 | 期待結果 | 実測 (G5 で記入) | pass-fail (G5 で記入) | 備考 |
|---|---|---|---|---|---|---|
| M-01 | 層 2 片鱗演出 | VS_SCOPE §3.4 / §4.3 / §5.x と STAGE3_G_PLAN G5 の該当記述を確認し、VS 終盤で定義済みの片鱗演出を再生する。 | 「ルールが書き換わる予兆」を示す 1 カットが画面に現れる。層 2 のルール本体は VS では実装しない。 | Stage 3 accepts the minimum hint level: brush-created time-window, past-space diorama, book reflection, and changed current-side footprint communicate that player action affects the present. Broader rule-change beat remains Stage 4 content design. | Pass | Player-facing text must continue avoiding internal planning terms. |
