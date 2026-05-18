# Chapter 1 graphics asset integration (2026-05-10)

## 目的

graphics-foundation worktree の Chapter 1 共有グラフィック資産を、Chapter 1 implementation base へ移植した。今回の目的は `Anemora_Chapter1.unity` の本番組み立てではなく、実装本体セッションが地形・建物配置に使える prefab / model / material / texture / shader / editor helper / validation helper を揃えること。

Dialogue / TimeManagement / Save / UI / production scene 本体は触っていない。`Anemora_Main` も変更していない。

## 移植対象

- Chapter 1 map prefab roots:
  - `Assets/Prefabs/Zone1/Chapter1Map/`
  - `Assets/Prefabs/Zone1/Chapter1MapProduction/`
- Chapter 1 detail kit prefab roots:
  - `Assets/Prefabs/Zone1/Chapter1DetailKit/`
  - `Assets/Prefabs/Zone1/Chapter1DetailKitProduction/`
- Zone1 直下の Chapter 1 補助 prefab:
  - `Assets/Prefabs/Zone1/Antela_Rowhouse_Corner.prefab`
  - `Assets/Prefabs/Zone1/Library_Active_Past.prefab`
  - `Assets/Prefabs/Zone1/Merchant_Stall_Closed_Current.prefab`
  - `Assets/Prefabs/Zone1/Merchant_Stall_Spice_Past.prefab`
  - `Assets/Prefabs/Zone1/ResidentA_MerchantHouse_Current.prefab`
  - `Assets/Prefabs/Zone1/ResidentA_MerchantHouse_Past.prefab`
- Chapter 1 models:
  - `Assets/Art/Models/Zone1/Chapter1/`
  - `Assets/Art/Models/Zone1/Chapter1Map/`
  - `Assets/Art/Models/Zone1/Chapter1DetailKit/`
- Chapter 1 materials / textures / shaders:
  - `Assets/Art/Materials/Zone1/Chapter1/`
  - `Assets/Art/Materials/Zone1/Chapter1Map/`
  - `Assets/Art/Textures/Zone1/Chapter1/`
  - `Assets/Art/Shaders/`
- Editor import/postprocess helpers:
  - `Assets/Editor/AnemoraChapter1MapAssetSetup.cs`
  - `Assets/Editor/AnemoraChapter1AtlasPostprocessor.cs`
  - `Assets/Editor/AnemoraChapter1BuildingAssetSetup.cs`
- validation / capture 関連:
  - `Assets/Tests/EditMode/Chapter1MapAssetTests.cs`
  - `Assets/Tests/EditMode/Chapter1BuildingAssetTests.cs`
  - `tools/generate_chapter1_buildings_blender.py`
  - `tools/generate_chapter1_map_assets_blender.py`
  - `tools/render_dq3r_review_sheets.py`
  - `tools/verify_chapter1_next5_static.py`
  - `tools/verify_chapter1_next6_static.py`
  - `tools/verify_chapter1_next7_static.py`
  - `tools/verify_chapter1_next8_static.py`
- graphics reference docs:
  - `docs/Chapter1AntelaSurfaceShader.md`
  - `docs/dq3r_lighting_tone_matrix.md`
  - `docs/dq3r_visual_rubric.md`

Unity asset は `.meta` を維持してコピーし、graphics-foundation 側の GUID を保持した。

## ローカル修正

`Assets/Art/Shaders/Chapter1AntelaSurfaceAtlas.shader` は Unity 6000.3 / URP import 時に shader compile issue が出たため、移植先で次を修正した。

- `[Header(Sub-Tile Detail)]` を `[Header(Sub Tile Detail)]` に変更。Unity shader property attribute が hyphen 付き header token を parse できなかった。
- `LitInput.hlsl` が既に宣言する URP Lit 標準 property を custom CBUFFER から除去。Chapter 1 atlas 固有の uniform だけを custom CBUFFER に残した。

## 検証結果

- Unity batchmode import/compile smoke:
  - target: `<worktree>`
  - Unity: `6000.3.14f1`
  - result: pass
  - log: `<temp>\anemora_ch1_graphics_import_compile_after_shaderfix2.log`
  - final log に `Shader error` / `error CS` / compiler error / batchmode abort は残っていない。licensing token noise は出たが run は成功。
- missing script / missing material scan:
  - 対象: `Assets/Prefabs/Zone1/Chapter1MapProduction/`, `Assets/Prefabs/Zone1/Chapter1DetailKitProduction/`
  - result: zero GUID script reference なし、`m_Material: {fileID: 0}` hit なし。
- GUID dependency scan:
  - result: unresolved external GUID は expected URP package reference のみ。
- `Chapter1BuildingAssetTests`:
  - result: pass, `12` total / `7` passed / `0` failed / `5` skipped
  - log/xml: `<temp>\anemora_ch1_graphics_building_tests.log`, `<temp>\anemora_ch1_graphics_building_tests.xml`
- `Chapter1MapAssetTests`:
  - result: partial pass, `71` total / `49` passed / `4` failed / `18` skipped
  - log/xml: `<temp>\anemora_ch1_graphics_map_tests2.log`, `<temp>\anemora_ch1_graphics_map_tests2.xml`
  - 4 failures は missing script/material ではなく、review/capture artifact 未生成によるもの:
    - `docs/devlog/screenshots/stage4_chapter1_map_unity_close_s3_currentstreet.png`
    - `docs/devlog/screenshots/stage4_chapter1_map_dq3r_s3_street_current_close.png`
    - `docs/devlog/screenshots/stage4_chapter1_map_unity_placement_review.png`
    - `docs/devlog/screenshots/stage4_chapter1_map_prefab_review.png`

## 実装本体セッション向け path

`Anemora_Chapter1.unity` の初回組み立てでは、まず以下を placement source として使う。

- `Assets/Prefabs/Zone1/Chapter1MapProduction/`
- `Assets/Prefabs/Zone1/Chapter1DetailKitProduction/`
- `Assets/Art/Materials/Zone1/Chapter1Map/`
- `Assets/Art/Materials/Zone1/Chapter1/`
- `Assets/Art/Textures/Zone1/Chapter1/`
- `Assets/Art/Shaders/Chapter1AntelaSurfaceAtlas.shader`
- `Assets/Art/Shaders/Chapter1AntelaAnimatedCloth.shader`

scene assembly には production roots を優先する。non-production の `Chapter1Map/` と `Chapter1DetailKit/` も、tests / editor helper / review pipeline が参照しているため移植した。

## 残リスク

- review screenshot artifacts は移植・再生成していない。`Anemora_Chapter1.unity` 作成後の capture pass まで map tests の capture 系 failure は残る。
- 新 worktree へ copy/import した影響で、一部 map test が「prefab が generated FBX より古い」と報告している。実装本体側で blocker になる場合は `AnemoraChapter1MapAssetSetup` の import menu を rerun する。そうでなければ、今回移植した production prefabs を assembly input として扱う。
- character final sprites は別キャラセッション待ち。現時点では既存 sprite / placeholder 前提。
- final visual polish / lighting polish / scene-specific shot composition は `Anemora_Chapter1.unity` 作成後の別 pass。

## 2026-05-10 追記: implementation scaffold 向け integration contract

実装本体側で `Anemora_Chapter1.unity` scaffold が進んだため、graphics package 側で scene に置く root を明示した。

### 追加した contract / aggregate

- `Assets/Editor/AnemoraChapter1GraphicsIntegrationContract.cs`
  - menu: `Anemora/Assets/Build Chapter1 Graphics Integration Aggregates`
  - `Chapter1MapProduction/` と `Chapter1DetailKitProduction/` を source として Current/Past aggregate を再生成する。
- `Assets/Prefabs/Zone1/Chapter1GraphicsIntegration/Ch1_GraphicsRoute_Current.prefab`
  - scene では `Chapter1_GraphicsIntegration_Current` に rename して instantiate する想定。
- `Assets/Prefabs/Zone1/Chapter1GraphicsIntegration/Ch1_GraphicsRoute_Past.prefab`
  - scene では `Chapter1_GraphicsIntegration_Past` に rename して instantiate する想定。
- `Assets/Prefabs/Zone1/Chapter1GraphicsIntegration/chapter1_graphics_integration_contract.json`
  - production roots / scene instance names / shader dependencies / atlas dependencies / menu helpers / copy-import checklist を machine-readable に記録。
- `Assets/Tests/EditMode/Chapter1GraphicsIntegrationContractTests.cs`
  - aggregate prefab existence、root scale、renderer bounds、missing script/material、contract JSON existence、production prefab root scale/bounds/missing references を検証。

### Aggregate 内容

- source production prefab count: `93`
- Current aggregate: `71` prefabs
- Past aggregate: `67` prefabs
- root scale: both `[1, 1, 1]`
- missing scripts: `0`
- missing materials: `0`
- renderer bounds: non-empty

Current/Past の分類は次の方針。

- `Current` token を持つ prefab は Current aggregate のみ。
- `Past` token を持つ prefab は Past aggregate のみ。
- era token がない scene assembly / helper / neutral detail kit は両 aggregate に含める。
- source prefab 自体は編集せず、aggregate から nested prefab instance として参照する。

### 実装本体への copy/import checklist

1. `.meta` を保持したまま、次の directory を implementation worktree へコピーする。
   - `Assets/Prefabs/Zone1/Chapter1MapProduction/`
   - `Assets/Prefabs/Zone1/Chapter1DetailKitProduction/`
   - `Assets/Prefabs/Zone1/Chapter1GraphicsIntegration/`
   - `Assets/Art/Models/Zone1/Chapter1Map/`
   - `Assets/Art/Models/Zone1/Chapter1DetailKit/`
   - `Assets/Art/Materials/Zone1/Chapter1Map/`
   - `Assets/Art/Materials/Zone1/Chapter1/`
   - `Assets/Art/Textures/Zone1/Chapter1/`
   - `Assets/Art/Shaders/`
   - `Assets/Editor/AnemoraChapter1MapAssetSetup.cs`
   - `Assets/Editor/AnemoraChapter1AtlasPostprocessor.cs`
   - `Assets/Editor/AnemoraChapter1BuildingAssetSetup.cs`
   - `Assets/Editor/AnemoraChapter1GraphicsIntegrationContract.cs`
2. Unity import 後、prefab stale warning が blocker になった場合だけ `Anemora/Assets/Apply Chapter1 Map Import` を rerun する。
3. production prefab を再生成した場合は `Anemora/Assets/Build Chapter1 Graphics Integration Aggregates` を rerun する。
4. `Anemora_Chapter1.unity` では aggregate prefab を次の scene root 名で配置する。
   - `Ch1_GraphicsRoute_Current.prefab` -> `Chapter1_GraphicsIntegration_Current`
   - `Ch1_GraphicsRoute_Past.prefab` -> `Chapter1_GraphicsIntegration_Past`
5. scene 作成後に capture helper を走らせる。
   - `Anemora/Assets/Capture Chapter1 Map Prefab Review`
   - `Anemora/Assets/Capture Chapter1 Map Placement Review`
   - `Anemora/Assets/Capture Chapter1 Map Close Density Review`
   - 必要に応じて TOD / cinematic / DQ3R post-process review captures

### 追加検証

- Aggregate build:
  - command: `Unity.exe -batchmode -quit -projectPath ... -executeMethod Anemora.Editor.AnemoraChapter1GraphicsIntegrationContract.BuildChapter1GraphicsIntegrationAggregates`
  - log: `<temp>\anemora_ch1_graphics_aggregate_build.log`
  - result: pass
- Contract tests:
  - command: `Unity.exe -batchmode -projectPath ... -runTests -testPlatform editmode -testFilter Chapter1GraphicsIntegrationContractTests`
  - xml: `<temp>\anemora_ch1_graphics_contract_tests2.xml`
  - result: `7` total / `7` passed / `0` failed

`-quit` 付きの `-runTests` はこの環境では result XML を出さず compile smoke だけで終了したため、最終 test run は `-runTests` 単体で実行した。

## 2026-05-10 最終確認: implementation handover 仕上げ

### Aggregate / contract 状態

- `Assets/Prefabs/Zone1/Chapter1GraphicsIntegration/Ch1_GraphicsRoute_Current.prefab`
- `Assets/Prefabs/Zone1/Chapter1GraphicsIntegration/Ch1_GraphicsRoute_Past.prefab`
- `Assets/Prefabs/Zone1/Chapter1GraphicsIntegration/chapter1_graphics_integration_contract.json`

`chapter1_graphics_integration_contract.json` に `scene_assembly_hints` を追加した。再生成 menu は `Anemora/Assets/Build Chapter1 Graphics Integration Aggregates`。

Scene assembly hints:

- 推奨 parent root: `Chapter1_GraphicsRoot`
- `Ch1_GraphicsRoute_Current.prefab` は scene object 名 `Chapter1_GraphicsIntegration_Current` として配置。
- `Ch1_GraphicsRoute_Past.prefab` は scene object 名 `Chapter1_GraphicsIntegration_Past` として配置。
- 配置 transform: local position `(0, 0, 0)`, rotation `(0, 0, 0)`, scale `(1, 1, 1)`
- 初期 active 推奨: Current `true`, Past `false`
- aggregate prefab は visual-only。Dialogue / TimeManagement / Save / UI / camera rig / production scene logic component は追加しない。
- Collider は aggregate / production prefab roots で検出されていない。runtime blockers / collision / nav / camera collision は implementation-owned root に分離する。
- camera rig は aggregate 外に置き、`Anemora_Chapter1.unity` 側の camera/cinemachine setup で frame 調整する。

### Implementation session がコピーする directory / file

`.meta` を保持して次をコピーする。

- `Assets/Prefabs/Zone1/Chapter1MapProduction/`
- `Assets/Prefabs/Zone1/Chapter1DetailKitProduction/`
- `Assets/Prefabs/Zone1/Chapter1GraphicsIntegration/`
- `Assets/Art/Models/Zone1/Chapter1Map/`
- `Assets/Art/Models/Zone1/Chapter1DetailKit/`
- `Assets/Art/Materials/Zone1/Chapter1Map/`
- `Assets/Art/Materials/Zone1/Chapter1/`
- `Assets/Art/Textures/Zone1/Chapter1/`
- `Assets/Art/Shaders/`
- `Assets/Editor/AnemoraChapter1MapAssetSetup.cs`
- `Assets/Editor/AnemoraChapter1AtlasPostprocessor.cs`
- `Assets/Editor/AnemoraChapter1BuildingAssetSetup.cs`
- `Assets/Editor/AnemoraChapter1GraphicsIntegrationContract.cs`

### 最終 QA

- `Chapter1GraphicsIntegrationContractTests`: pass
  - XML: `<temp>\anemora_ch1_graphics_contract_tests_final2.xml`
  - result: `7` total / `7` passed / `0` failed
- batchmode import / shader compile smoke: pass
  - log: `<temp>\anemora_ch1_graphics_import_smoke_final2.log`
  - `Shader error` / `error CS` / compiler error / batchmode abort / exception hit: `0`
- static prefab scan:
  - 対象: `Chapter1GraphicsIntegration/`, `Chapter1MapProduction/`, `Chapter1DetailKitProduction/`
  - zero GUID script: hit なし
  - missing material: hit なし
  - missing nested prefab source: hit なし
  - Collider / trigger component text: hit なし

Prefab stale warning は今回の final smoke では implementation blocker と判断しなかった。implementation 側で blocker 化した場合のみ、次の順で rerun する。

1. `Anemora/Assets/Apply Chapter1 Map Import`
2. `Anemora/Assets/Build Chapter1 Graphics Integration Aggregates`
3. `Chapter1GraphicsIntegrationContractTests`

### 本体 session がコピー後に実行する validation

1. Unity import smoke:
   - `Unity.exe -batchmode -quit -projectPath <impl-worktree> -logFile <temp-log>`
   - expected: shader compile error / C# compile error なし。
2. Contract tests:
   - `Unity.exe -batchmode -projectPath <impl-worktree> -runTests -testPlatform editmode -testFilter Chapter1GraphicsIntegrationContractTests -testResults <temp-xml> -logFile <temp-log>`
   - expected: `7` passed / `0` failed。
3. Scene placement:
   - `Chapter1_GraphicsIntegration_Current` active
   - `Chapter1_GraphicsIntegration_Past` inactive
   - both under `Chapter1_GraphicsRoot`
4. Scene 作成後の capture helper:
   - `Anemora/Assets/Capture Chapter1 Map Prefab Review`
   - `Anemora/Assets/Capture Chapter1 Map Placement Review`
   - `Anemora/Assets/Capture Chapter1 Map Close Density Review`
   - 必要に応じて TOD / cinematic / DQ3R post-process review captures

### 残リスク

- capture 系 failure は `Anemora_Chapter1.unity` 本体統合後の validation pass として残す。
- character final sprites は別セッション待ち。graphics asset package 側では完了宣言しない。
- final scene lighting / production camera composition は graphics asset package 側では完了宣言しない。
- Unity import smoke の副作用で `Assets/AddressableAssetsData/link.xml` が削除扱いになることがある。Chapter 1 graphics package の範囲外なので都度 `git restore` で戻した。

## 2026-05-10 追記: capture / validation readiness

Implementation session が `Chapter1GraphicsIntegration` と依存 asset を取り込み、`Anemora_Chapter1.unity` に aggregate / runtime root / audio wiring を配置し始めたため、graphics session 側で scene 統合後の capture readiness を整理した。scene 本体はこの session では開かない。

### Scene-integrated capture に必要な受け渡し情報

Implementation session から graphics session に渡すもの:

- implementation worktree path
  - 例: `<worktree>`
- scene path
  - `Assets/Scenes/Chapter1/Anemora_Chapter1.unity`
- aggregate scene object state
  - `Chapter1_GraphicsIntegration_Current`: active
  - `Chapter1_GraphicsIntegration_Past`: inactive
  - Past review 時は Current inactive / Past active に切り替えて同じ camera framing で比較する。
- capture helper menu
  - `Anemora/Assets/Capture Chapter1 Map Prefab Review`
  - `Anemora/Assets/Capture Chapter1 Map Placement Review`
  - `Anemora/Assets/Capture Chapter1 Map Close Density Review`
  - 必要に応じて TOD / cinematic / DQ3R post-process review captures
- expected output path
  - `docs/devlog/screenshots/`
- batchmode command template
  - `Unity.exe -batchmode -quit -projectPath <impl-worktree> -executeMethod <capture-method> -logFile <temp-log>`

Scene を開いて撮る必要がある capture は、implementation 側から明示依頼が来るまで待機する。graphics package 側では capture helper / checklist / static validation だけを用意する。

### Static validation

追加 script:

- `tools/verify_chapter1_graphics_integration_static.py`

実行例:

```powershell
python tools/verify_chapter1_graphics_integration_static.py --repo <worktree>
```

この script は Unity scene を開かず、次だけを検証する。

- aggregate prefab 2 種
- `chapter1_graphics_integration_contract.json`
- shader 2 種
- atlas / material roots
- production prefab roots
- required editor helper scripts
- zero GUID script / missing material / missing nested prefab source text pattern

実行結果:

- graphics integration worktree: pass
  - `python tools/verify_chapter1_graphics_integration_static.py --repo <worktree>`
- implementation worktree: pass
  - `python tools/verify_chapter1_graphics_integration_static.py --repo <worktree>`

### Visual QA rubric update

`docs/dq3r_visual_rubric.md` に Chapter 1 integration scene 向け addendum を追加した。

- isometric strict framing
- Current/Past aggregate の読み分け
- character final sprites 未統合時の placeholder 前提

### 残リスク

- scene-integrated capture は `Anemora_Chapter1.unity` の配置が安定してから実行する。
- character final sprites / final scene lighting / production camera composition は graphics asset package 側では完了宣言しない。
- runtime / dialogue / save / UI / production scene 本体はこの session の対象外。
## 2026-05-10 追記: scene-integrated QA readiness update

Implementation session は `<worktree>` 側で graphics package を取り込み、`Assets/Scenes/Anemora_Chapter1.unity` の scene assembly に入り始めている。graphics session 側では production scene を直接編集せず、統合後レビューに備えた static validation / capture checklist / visual rubric を更新した。

### Static validation update

`tools/verify_chapter1_graphics_integration_static.py` を補強した。

- `--repo <Unity project root>` を受け取る。
- scene は開かない。
- aggregate prefab / contract JSON / shader 2 種 / material roots / texture root / production prefab roots / editor helper scripts を検証する。
- prefab text pattern scan:
  - zero GUID script reference
  - missing script component
  - missing nested prefab source
  - renderer material slots under `m_Materials:`
- `m_Material: {fileID: 0}` は collider physics material null でも出るため、renderer missing material としては扱わない。

実行結果:

```powershell
python tools\verify_chapter1_graphics_integration_static.py --repo <worktree>
```

Result: pass.

- `Chapter1GraphicsIntegration`: 2 aggregate prefabs
- `Chapter1MapProduction`: 75 prefabs
- `Chapter1DetailKitProduction`: 18 prefabs
- contract schema / scene names / scene assembly hints: ok
- shader / atlas / menu dependency lists: ok
- zero GUID script reference: none
- renderer missing material slots: none
- missing nested prefab source: none

### Capture readiness

Implementation から capture/polish 依頼を受ける前に必要な情報:

- implementation worktree path: `<worktree>`
- scene path: `Assets/Scenes/Anemora_Chapter1.unity`
- required scene objects:
  - `Chapter1_GraphicsRoot` or implementation-owned equivalent
  - `Root_Current`
  - `Root_Past`
  - `Chapter1_GraphicsIntegration_Current`
  - `Chapter1_GraphicsIntegration_Past`
- expected output root: `docs/devlog/screenshots/`

Active-state premise:

- graphics contract has Past initial inactive as an asset-package hint.
- implementation may keep `Root_Past` active for TimeFrame-system consistency.
- this is not a graphics package blocker.
- visual comparison captures should explicitly label Current-only / Past-only / mixed runtime-debug state.

Runtime collider / blocker / trigger / camera collision は implementation-owned root 側で分離する。graphics package は visual-only を維持する。

### Capture checklist viewpoints

全 viewpoint 共通で見る項目:

- isometric strict framing
- foreground occlusion density
- path readability
- Current/Past aggregate readability
- atlas/material seam
- dusk/decline tone stability

| Viewpoint | Capture intent | Extra notes |
|---|---|---|
| Niro house / southwest | home-side scale, entrance readability, route start composition | Placeholder character scale is acceptable. |
| Central plaza | landmark hierarchy and route branching | Avoid prop density that hides player/NPC silhouettes. |
| Library ruins / north | northern landmark identity and background silhouette | Landmark should read without UI/dialogue. |
| Aria house / street corner | S3 street density, awnings, facade seams, navigation read | Foreground occluders frame, not cover face/feet zones. |
| Mia house surroundings | residential cluster read and current/past era distinction | Avoid warm cards flattening the decline tone. |
| Kaia field | open-field readability and path continuity | Sparse foreground is allowed if route read stays clear. |
| Ruins foreshadow / forest entrance long view | distant silhouette and atmosphere | Suggest direction/decline without revealing too much. |
| Scene 5 chapter-end small-stone area | final-beat focus, ground seam, silhouette clarity | Spoiler-sensitive framing and filenames. |

### Still pending

- actual scene-integrated captures
- Current-only vs Past-only comparison sheets
- final character sprite judgment
- final scene lighting / production camera composition
## 2026-05-10 追記: visual issue intake / polish backlog readiness

Implementation scene capture が届いた瞬間に判定へ入れるよう、notes handover に capture review report template / issue taxonomy / minimum acceptance gate を追加した。今回も `Anemora_Chapter1.unity` は開いていない。

### Capture review report template

Handover に次の section を追加済み。

- capture set / timestamp / implementation commit or worktree note
- viewpoint coverage
- pass / needs polish / blocker classification
- Current/Past contrast findings
- path readability findings
- foreground occlusion findings
- material / shader / atlas findings
- scene 5 foreshadow risk findings
- character placeholder caveat
- polish backlog table

### Visual issue taxonomy

Capture 上の指摘は次の owner に分類する。

- graphics asset package owned
- implementation scene assembly owned
- runtime / collision / camera owned
- character final sprite dependency
- visual polish deferred

### Minimum acceptance gate

この gate は capture/polish review へ進むための最低条件であり、playable 完成を意味しない。

- route landmark が各 capture で識別できる。
- player path が主要地点で読める。
- Current/Past aggregate の差が scene 意図に沿っている、または mixed runtime/debug state として明記されている。
- atlas / shader / material の破綻が一見してない。
- foreground occlusion が player / NPC を常時隠さない。
- 図書館跡と中央広場が landmark として読める。
- scene 5 foreshadow が spoiler-heavy になっていない。
- character final sprites 未統合時は placeholder scale/contact/silhouette/framing のみ判定する。

### Implementation への返却 command

```powershell
python tools\verify_chapter1_graphics_integration_static.py --repo <worktree>
```

Capture helper names:

- `Anemora/Assets/Capture Chapter1 Map Prefab Review`
- `Anemora/Assets/Capture Chapter1 Map Placement Review`
- `Anemora/Assets/Capture Chapter1 Map Close Density Review`
- TOD / cinematic / DQ3R post-process capture helpers as needed

Expected output directory:

- `docs/devlog/screenshots/`
## 2026-05-10 追記: Phase A-D 自走完了

Production scene は開かず、implementation worktree は read-only static scan のみに限定した。新規 Meshy / Blender asset 制作、runtime / dialogue / save / UI 変更、commit / push / PR / staging は行っていない。

### Phase A: asset inventory / area manifest

追加:

- `docs/chapter1_graphics_asset_inventory.md`

概要:

- package root inventory
- aggregate prefab summary
- area summary
- prefab classification index
- ownership notes

Counts:

- aggregate prefabs: 2
- map production prefabs: 75
- detail kit production prefabs: 18
- Chapter 1 materials: 97

### Phase B: static quality analyzer

更新:

- `tools/verify_chapter1_graphics_integration_static.py`

追加検証:

- prefab count summary
- prefab root name uniqueness
- aggregate contract と実 prefab path の一致
- `.meta` presence
- duplicate meta GUID scan
- shader property token presence
- material shader/texture reference health
- renderer material array null check under `m_Materials:`
- missing nested prefab source check
- zero GUID script/reference check

`m_Material: {fileID: 0}` は collider physics material null でも出るため、renderer missing material としては扱わない。

### Phase C: scene assembly handoff pack

追加:

- `docs/chapter1_graphics_scene_assembly_handoff.md`

内容:

- graphics package copy manifest
- aggregate prefab paths
- recommended hierarchy
- Current/Past active-state caveat
- visual-only boundary
- implementation-owned camera / collision / runtime hooks
- area placement notes
- capture checklist
- fix routing
- validation commands
- not-playable caveat

### Phase D: validation

Static validator:

- graphics integration worktree: pass
- implementation worktree: pass
- aggregate prefabs: 2
- map production prefabs: 75
- detail kit production prefabs: 18
- unique prefab names: 95
- meta presence: 621 assets/directories checked
- unique meta GUIDs: 605
- shader tokens: 2 shaders ok
- material health: 97 materials / 97 shader refs / 290 texture refs

Unity:

- import / shader compile smoke: pass
  - log: `<temp>\anemora_ch1_graphics_phaseD_import_smoke.log`
  - shader/C# compiler/batchmode abort/exception hits: 0
- `Chapter1GraphicsIntegrationContractTests`: pass
  - XML: `<temp>\anemora_ch1_graphics_phaseD_contract_tests.xml`
  - `7` total / `7` passed / `0` failed
- `Chapter1BuildingAssetTests`: pass/skip, non-blocking
  - XML: `<temp>\anemora_ch1_graphics_phaseD_building_tests.xml`
  - `12` total / `7` passed / `0` failed / `5` skipped
  - skipped tests require review captures
- `Chapter1MapAssetTests`: known non-blocker failures
  - XML: `<temp>\anemora_ch1_graphics_phaseD_map_tests.xml`
  - `71` total / `49` passed / `4` failed / `18` skipped
  - failures are missing capture artifacts plus non-production prefab timestamp stale warnings
  - static production package validation still passes
- `git diff --check`: pass

Unity import smoke again marked `Assets/AddressableAssetsData/link.xml` deleted as an import side effect. It was restored because it is outside Chapter 1 graphics package scope.

### Implementation next commands / docs

Docs:

- `docs/chapter1_graphics_asset_inventory.md`
- `docs/chapter1_graphics_scene_assembly_handoff.md`
- `docs/dq3r_visual_rubric.md`

Static validation:

```powershell
python tools\verify_chapter1_graphics_integration_static.py --repo <worktree>
```

Contract test:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -projectPath '<worktree>' -runTests -testPlatform editmode -testFilter Chapter1GraphicsIntegrationContractTests -testResults '<temp>\anemora_ch1_impl_graphics_contract_tests.xml' -logFile '<temp>\anemora_ch1_impl_graphics_contract_tests.log'
```

Capture helpers after scene integration stabilizes:

- `Anemora/Assets/Capture Chapter1 Map Prefab Review`
- `Anemora/Assets/Capture Chapter1 Map Placement Review`
- `Anemora/Assets/Capture Chapter1 Map Close Density Review`
- TOD / cinematic / DQ3R post-process capture helpers as needed

### Residual risks

- scene-integrated captures are not run yet
- final character sprites remain blocked and are not production-graded
- final lighting / production camera composition remains deferred
- runtime / dialogue / save / UI / collision / camera behavior remain outside graphics package completion
