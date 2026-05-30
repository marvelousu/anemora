# HANDOFF: Chapter 1 building interiors — Mia house + Aria house (map-vs)

受け手: `work/chapter1-continuation-map-vs-20260524` を担当する Codex(Windows) セッション。実装は Codex 側。検証は Unity batchmode + .exe + R2 レビュー。
作成: 2026-05-30 / Claude(Opus, 管理セッション)。

---

## 1. Context

- worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample`、branch `work/chapter1-continuation-map-vs-20260524`、**現 tip `5bc5a2ad`**（直前の character runtime population 完了済み）。着手前に `git fetch origin && git merge origin/...` で最新化。
- 直前の状況: Chapter 1 named cast（Mia/Kaia/Dario/Karla/Kairo/Luna）+ generic NPC を**屋外エリアに新規配置済み**（仮配置）。
- **問題**: Mia/Aria 等の建物は**外観のみで屋内（インテリア）が無い**。完全なインテリアは Niro 家だけ（+図書館は机周り）。そのため屋内が想定されるキャラ（Mia の作業、Aria 家で Karla が Aria に商いを教える S3 レッスン）が入る空間が無く、配置を検証できない。
- **本タスク**: **Mia 家と Aria 家のインテリアを Niro 家インテリアを雛形に新規作成**し、屋内キャラをそこへ移す。Tom 承認スコープ = この2件のみ（Kaia 農家等は対象外）。
- 前提（前セッション群の結果）: bloat-guard 有効（`docs/review/`・scene・APV は git add 不可）、レビュー画像は R2（`tools\r2\r2-upload-review.ps1`、`CLOUDFLARE_API_TOKEN` は User env 設定済）、生成 scene/APV は untracked build artifact（commit しない）、B-β/C-β 採用済（Buto/Fronkon の有償アセットは触らない・commit 禁止）。`git add -A` 禁止（WIP を巻き込まない、pathspec stage）。生成器 `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs` は bloat-guard allowlist 済（true source 5MB超）。

## 2. アーキテクチャ（SeparateSpace エリア制・これを理解してから着手）

マップは**独立座標空間のエリア**に分かれ、`FastVsHouseAreaVisibility` が「同時に1エリアのみ表示」を制御。door 通過で「表示エリア切替＋プレイヤー teleport」。Niro インテリアはこのモデルの完成例。

- **エリア enum**: `Assets/Scripts/FastVS/FastVsHouseAreaVisibility.cs:5` `enum FastVsHouseArea { Interior, Exterior, CentralPlaza, Library, MiaHouse, AriaStreet, KaiaFarm, Ruins, Chapter1End }`。
- **マップ組み立て**: `CreateHouseMap(root, past, materials)`（生成器:9902）。`CreateMapSetRoot(root, "{prefix}_HouseInteriorMap_SeparateSpace")` / `..._HouseExteriorMap_SeparateSpace"` で area ごとの map-set root を作り、各 area builder（`CreateInterior` 等）を呼び、`HouseMapAreas` を返す。
- **HouseMapAreas struct**: 生成器:90691（ctor 引数 interior/exterior/centralPlaza/library/miaHouse/ariaStreet/kaiaFarm/ruins/chapter1End）。
- **visibility 登録**: `CreateHouseAreaVisibility(currentAreas, pastAreas)`（生成器:9997）が `SerializedSet(visibility, "currentInteriorMap", currentAreas.Interior)` …の形で各 area map を `FastVsHouseAreaVisibility` に流し込み、`activeArea`/`SetActiveAreaForReview` を Interior に。
- **door**: `CreateHouseDoorTransitions(controller, player, areaVisibility, story)`（生成器:38559）が `CreateAreaDoorTransition(name, controller, player, areaVisibility, ...)`（:38743、component `FastVsAreaDoorTransition`）で door を生成。Niro の雛形ペア = `FastVS_DoorTransition_Interior_To_Exterior`(:38561) / `FastVS_DoorTransition_Exterior_To_Interior`(:38573)。既存に `CentralPlaza_B3_To_MiaHouse_C1`(:38634)・`MiaHouse_C3_To_AriaStreet_D1`(:38658) 等あり（MiaHouse/AriaStreet は**屋外**エリア）。
- **Niro インテリア雛形**: `CreateInterior(root, prefix, past, materials)`（:10033）。`HouseInteriorCenter`(:403) を中心に `CreateLandmarkCube`（床/back・left・right 壁/wainscot/trim）＋ `AddHd2dSurfaceProfile(..., FastVsHouseArea.Interior, ...)`。家具/小物/照明は polish 群: `CreateHouseInteriorFurnitureGroundingPolish`(:10186) / `CreateHouseInteriorPropReadabilityPolish`(:10297) / `CreateHouseInteriorRoomDepthPolish`(:10527) / `CreateHouseInteriorPropDetailSlab`(:34518)。past/current でマテリアル分岐（`past ? materials.PastWoodFloor : materials.CurrentInteriorFloor` 等）。
- **キャラ配置**: 位置定数 生成器:446-454（`Chapter1MiaRuntimePosition = Chapter1MiaHouseMapCenter + (-1.92,0.02,0.62)`、`Chapter1KarlaRuntimePosition = Chapter1AriaStreetMapCenter + (7.76,0.02,3.42)` 等）。spawn は `CreateChapter1AriaHouseRuntimePopulation`(:11214)、Mia は MiaHouse 屋外 population(:11155 付近)。検証 `ValidateChapter1RuntimeCharacterPopulation`(:605, 実体 42876付近 `ValidateRuntimeCharacterSprite(...)`)。

## 3. Canon 制約（インテリアにも適用・違反は作り直し）

- **過去/現在で見た目が変わる**（古代構造=図書館だけ不変）。Mia/Aria インテリアは current/past でマテリアル/様式を分岐（`CreateInterior` の past 分岐に倣う）。間取り（広さ/形/入口構造）はほぼ同一で era 様式だけ差。
- **Niro は唯一の異物・NPC は特別役割なし**。Mia=寡婦/単身 30-40、元縫製/織り（warm、内職で多忙）→ インテリアに縫製/布の小物可。Aria 家=商人の娘 Aria に Karla が商いを教える（S3、ledger/帳簿系小物可）。装飾は控えめ・grounding 優先。
- player-facing 禁止語（層/ベール剥離/観測者磨耗、Antela 前面化）。**メタデータ禁止語**（commit/PR/branch/メソッド名/公開doc）: ループ/観測者/真層/偽記憶/Robot_X/Echo/エリュトリア/第4の壁。メソッド名は中立に（例 `CreateMiaInterior`/`CreateAriaInterior`）。
- **Niro 家インテリア・図書館・既存屋外配置を回帰させない**（新規 area 追加のみ）。

## 4. Numbered Mechanical Work（work-unit ごとにレビュー）

### A. area 定義の追加
1. `Assets/Scripts/FastVS/FastVsHouseAreaVisibility.cs`: `enum FastVsHouseArea` に **`MiaInterior`, `AriaInterior`** を追加（末尾推奨、既存値の順序を壊さない）。
2. 同コンポーネントに serialized map フィールド `currentMiaInteriorMap`/`pastMiaInteriorMap`/`currentAriaInteriorMap`/`pastAriaInteriorMap` を追加し、既存の show/hide ロジック（area→map 表示切替）に新 area を組み込む（既存 Interior/Exterior と同じ扱い）。
3. 生成器 `HouseMapAreas` struct(:90691) に `MiaInterior`/`AriaInterior` GameObject を追加（ctor 引数 + プロパティ）。

### B. インテリア構築（Niro 雛形を emulate）
4. 新メソッド `CreateMiaInterior(root, prefix, past, materials)` / `CreateAriaInterior(...)` を `CreateInterior`(:10033) を雛形に作成。各々**独自の interior center 定数**（例 `MiaInteriorCenter`/`AriaInteriorCenter`、Niro と別座標の SeparateSpace）。床/壁/surface profile（`FastVsHouseArea.MiaInterior`/`AriaInterior` を渡す）/家具/照明。Mia=縫製小物、Aria 家=帳簿小物を軽く。past/current 分岐必須。
5. `CreateHouseMap`(:9902) で `{prefix}_MiaInteriorMap_SeparateSpace`/`{prefix}_AriaInteriorMap_SeparateSpace` の map-set root を `CreateMapSetRoot` で作り、4 の builder を呼び、戻り `HouseMapAreas` に含める。
6. `CreateHouseAreaVisibility`(:9997) で `SerializedSet(visibility, "currentMiaInteriorMap", currentAreas.MiaInterior)` …を追加（current/past × Mia/Aria の4本）。

### C. door 遷移
7. `CreateHouseDoorTransitions`(:38559) に `CreateAreaDoorTransition`(:38743) でペアを追加（Niro の Interior_To_Exterior(:38561)/Exterior_To_Interior(:38573) を雛形に）:
   - `FastVS_DoorTransition_MiaHouse_To_MiaInterior` / `..._MiaInterior_To_MiaHouse`（Mia 家外観の入口位置 ↔ MiaInterior 入口）
   - `FastVS_DoorTransition_AriaStreet_To_AriaInterior` / `..._AriaInterior_To_AriaStreet`
   入口は Mia 家/Aria 家**外観の扉位置**に合わせる（外観 builder `CreateMiaHouseExteriorContinuation`:11336 / `CreateAriaHousePlazaContinuation`:14691 の扉/正面を参照）。

### D. 屋内キャラの移動（仮配置）
8. **屋内へ**: Mia → MiaInterior（作業机付近）、Aria + Karla → AriaInterior（Karla が Aria に教える構図）。`Chapter1MiaRuntimePosition`/`Chapter1KarlaRuntimePosition`（+ Aria 用）を**新 interior center 基準**に更新し、spawn を interior map root 配下へ（`CreateChapter1AriaHouseRuntimePopulation`:11214 等を分岐 or 新 interior population メソッド）。
9. **屋外のまま**: DarioStreet/Kairo/Luna（街角）、DarioFarm/Kaia（Kaia 農場）、Ruins NPC。
10. 位置は**仮**（Tom がレビューで動かす前提・過剰投資しない）。屋内に入れて breathing・contact shadow・接地が出ていることを優先。

### E. 検証拡張
11. `ValidateChapter1RuntimeCharacterPopulation`(:605/42876付近) を、Mia/Aria/Karla が**新 interior area の想定位置**に居るよう更新。door 検証（`ValidateHouseMapSeparationAndDoorTransitions`:606）に新 door ペアを追加し、新 area の map separation（他 area と座標重複しない SeparateSpace）を検証。

## 5. Smoke Test Steps（合否は自前 EXIT echo + log の return code/例外 grep。background 通知の exit code は信用しない）

Unity: `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe`。

1. `CreateHouseSliceScene`（CS error 0、return code 0）。
2. `ValidateHouseSliceBatch`（既存検証＋新 area/door/char 検証 pass、Niro インテリア/図書館/既存屋外配置が回帰しない）。
3. `BuildAndValidateBatch`（.exe ビルド成功）。**FilmGrain 罠**: 再生成後 `grep -c 'active: 1' Assets/Settings/DefaultVolumeProfile.asset` が **20** か確認（2パス推奨）。
4. .exe 18秒 runtime smoke、Player.log 例外/error 0、新 door 通過で MiaInterior/AriaInterior に入れて Mia/Karla/Aria が見える、戻れる。
5. レビュー画像: Mia 家外観→扉→MiaInterior（Mia）、Aria 街→扉→AriaInterior（Aria+Karla）、各 current/past → `docs/review/<ts>/`（`devlog.txt` 必須）→ **R2 アップロード**（git add しない）→ exe フルパス添付で Tom 目視。

## 6. Open Risks / 触ってはいけない所

- **SeparateSpace の座標重複**: 新 interior center は既存 area と座標が被らない別空間に置く（被ると visibility 切替で両方見える/抜ける）。`ValidateHouseMapSeparationAndDoorTransitions` が検出するはず。
- **enum 値追加の順序**: `FastVsHouseArea` の既存値順を変えない（シリアライズ値ずれ防止）。新値は末尾。
- **配置は仮**（D-10）。座標微調整に時間を使わない。
- **bloat-guard**: `docs/review/`・scene・APV を git add しない（レビューは R2、scene は再生成のみ）。
- **EULA**: Buto/Fronkon の有償アセットに触らない・commit しない。**生成 interior は first-party なので生成器コードを commit してよい**（scene 本体は untracked のまま）。
- **pathspec stage 厳守**（`git add -A` 禁止）: B-β/C-β WIP（packages-lock/ProjectSettings 系）を巻き込まない。
- **SoT は生成器**（手編集シーン禁止・main immutable）。canon（§3）順守。
- 関連一次資料: character handoff `docs/CHAPTER1_CHARACTER_RUNTIME_HANDOFF.md`、元計画 `C:\Users\maro6\Documents\Unity\Anemora-chapter1-continuation-work\docs\CHAPTER1_FULL_VS_CHARACTER_RUNTIME_IMPLEMENTATION_PLAN.md`。
- 完了後、管理セッションへ「Mia/Aria インテリア実装完了、map-vs tip = <hash>、屋内配置レビュー待ち」と返す。
