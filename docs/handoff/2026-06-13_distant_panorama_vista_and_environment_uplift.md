# Handoff: 遠景パノラマ・ビスタ追加 + 環境アップリフト

実装担当: Codex (Windows / Unity 6000.3.14f1)。本書は 2026-06-13 の環境監査を受けた次フェーズの実装指示。
公開リポにつき **外部の商用タイトル名・固有名を本書・コード・アセット名・コミットに一切含めない**（参照は一般語「open-world panoramic distant vista / 遠景パノラマ」で統一）。

---

## 1. Context

- プロジェクト: Anemora、HD-2D（ビルボードのキャラ・スプライト + 3D 低ポリ背景）。カメラは透視追従（`RuntimeVsFollowCameraOffset = new Vector3(0f, 2.75f, -4.55f)`、`AnemoraFastVsHouseSliceSetup.cs:410`）。
- マップは離散空間。各マップ中心は `CentralPlazaVsCenter` 基準のオフセット定数:
  - `Chapter1MiaHouseMapCenter` (`:448`), `Chapter1AriaStreetMapCenter` (`:449`), `Chapter1KaiaFarmMapCenter` (`:450`), `Chapter1RuinsMapCenter` (`:451`)。各マップに「現在 (current)」「過去 (past)」の2空間がある。
- シーン生成のエントリ: `CreateHouseSliceScene()` (`:545`)。カメラ生成 `CreateCamera(currentRoot)` (`:565`)。
- **現状の問題（実画確認・監査で確定）**: マップ端で void（素のプレーン）が露出し、遠景が無い。背景は cycle 27〜110 で何度も触られているが、その実装は **平面の void スラブ + sky の色ウォッシュ**であり、遠景の立体感・パララックスが生まれていない:
  - `CreateOutdoorVoidBackgroundTreatment(...)` (`:21546`) と `CreateOutdoorVoidBackgroundSlab(...)` (`:21653`) = 平面スラブで穴を塞ぐだけ。
  - `CreateOutdoorSkyWashTreatment` (`:21669`) / `CreateOutdoorSkyDetailPolish` (`:21721`) / `...SkyHorizonLayeringPolish` (`:21788`) / `...SkyAtmosphereDepthPolish` (`:21828`) / `...HorizonDepthCleanupPolish` (`:21895`) / `...FarEdgeTransitionPolish` (`:21950`) = sky の色・帯の polish の積み重ね。
  - これらは「polish」命名が示すとおり**平面アプローチへの微調整の堆積**＝ plateau。新たな sky 色 polish をもう1枚足しても遠景にはならない。
- **凍結済み（重要）**: 2026-06-13 に renderer 構成を契約テストで凍結した（`Assets/Tests/RendererContract/RendererContractTest.cs`、ベースライン `__golden/renderer_contract.txt`）。**URP の Renderer Feature（PortalStencil / SSAO / TiltShift / Outline）を足す・消す・並べ替えると EditMode テストが落ちる**。意図的に変える時のみ `ANEMORA_RENDERER_REBASELINE=1` で再生成しコミット。
  - 注: `RenderSettings.fog` / `RenderSettings.skybox` は **Renderer Feature ではない**ので凍結対象外。本実装で fog を有効化してよい（むしろ必須、§3-4）。
- 監査の全文・診断は `~/work/anemora-audit-20260613-fable/`（ローカル）と `docs/devlog/2026-06-13_env_audit_renderer_freeze_proposal.md`。

## 2. ゴール（何を作るか）

各マップの周囲に、**遠景の山・自然が円形（360°）に見える開けたパノラマ**を作る。プレイヤー/カメラが向きを変えても、プレイ領域の外側に遠い山並みと自然が連なって見え、マップ端の void が消える。平面の sky 画ではなく、**実際の遠景3Dジオメトリ + 大気遠近（fog）**で奥行きとパララックスを出す。

なぜ平面でなく立体か: 透視追従カメラが動くと、遠景3Dは近景とパララックス差で動き「遠くにある」と読める。平面スラブ/sky 画は貼り付いて見え、これが現状の限界。

**過去に却下された手は繰り返さない**: 手続き生成の空（procedural sky）と `CreateOutdoorBackdrop/CreateOutdoorCloudCluster` は「粗い」と却下済み（memory 記録）。今回は**生成または authored の遠景メッシュ**を使い、質を担保する。

## 3. 実装手順（番号付き・写経密度）

cycle 方式（`tools/cycle-runner.ps1`）で進める。authored file は `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs` 1本。**最初に1マップ（推奨: Mia House）だけで“遠景の見え”を確立してから全マップへ展開**（plateau ガード: 同一対象で微調整を10サイクル超回さない。見えが出ないなら手法を疑う）。

### 3-1. 遠景メッシュの調達（cycle 着手前のアセット準備）
1. 遠景の山・丘・樹林シルエット用の低ポリメッシュを用意する。第一候補は Meshy MCP（接続済み）:
   - `mcp__meshy__meshy_text_to_3d_preview` で "low poly distant mountain ridge, stylized, flat-shaded" 等 → refine → glb。または手元のコンセプト画から `mcp__meshy__meshy_image_to_3d`。
   - 樹林は「low poly tree cluster silhouette」。
   - 取り込み先: `Assets/Art/Models/DistantVista/`（新規）。インポート後 **`Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch` を必ず一度回す**（過大ポリ・missing ref を弾く。`MeshTriangleWarnThreshold = 20000` 未満に保つ）。
2. 代替（生成が間に合わない初手）: 1枚の稜線をスケールの違う2〜3個の三角プリズム/押し出しメッシュで代用し“見え”の検証だけ先に進めてよい。ただし最終はメッシュ品質を上げる。

### 3-2. 新メソッド: 遠景リングの配置
3. `AnemoraFastVsHouseSliceSetup.cs` に新規ヘルパを追加（既存 `CreateOutdoor*` 群の並びに置く、目安 `:22136` 付近の後）:
   ```csharp
   // Builds a ring of distant low-poly relief (mountains/treelines) around a map
   // center so the horizon reads as an open panoramic vista instead of a void edge.
   // Real 3D at distance -> parallax under the perspective follow camera.
   private static void CreateDistantPanoramaVista(
       Transform root, string prefix, bool past, FastVsHouseArea area, Materials materials)
   {
       // 1) ひとつの親 "DistantVista" を root 下に作る（既存 landmark 命名規約に合わせる）。
       // 2) リング配置: 半径 R を「プレイ領域の外」かつ「farClip 内」に取る。
       //    R の初期値 ~ 90f（マップ毎に微調整可）。角度 N = 14（360/14 ≒ 25.7°刻み）。
       //    各セグメントに遠景メッシュを置き、Y回転で中心へ正対させる。
       // 3) 深度バンド 2〜3 層: near hills(低・手前・彩度高) / mid range / far peaks(高・奥・青寄せ)。
       //    層ごとに R と高さスケールと色（materials の遠景用 mat）を変える。
       // 4) 高さ・水平位置を擬似ランダムでばらす（決定論: area と index から導出、Random 不使用）。
       // 5) past=true は稜線をより低彩度・かすませ、現在/過去の対比を出す。
   }
   ```
   - 罠: **`Random`/`Time`/`DateTime` を使わない**（cycle の決定論規律）。ばらつきは `area`・セグメント index・既存の決定論ハッシュ系から導出する。
   - 罠: メッシュは **far clip 内**に収める。必要なら `CreateCamera` 側の farClipPlane を確認し、足りなければ別 cycle で調整（renderer feature ではないので凍結に抵触しない）。
4. 遠景用マテリアルを `Materials` 構造体（`:～56 プロパティ`）に追加するか、専用 mat を生成して渡す。命名は `Ch1Distant_*` 等、**外部固有名を含めない**。

### 3-3. 大気遠近（fog）でマップ端の継ぎ目を消す
5. シーン生成内（`CreateHouseSliceScene` か lighting セットアップ箇所）で `RenderSettings.fog = true` にし、`RenderSettings.fogMode = FogMode.Linear`、`fogStartDistance`/`fogEndDistance` を「プレイ領域の縁〜遠景リング手前」に合わせる。`fogColor` は時間帯（current/past）で出し分け、遠景メッシュの足元がヘイズに溶けてマップ端の void が見えなくなるようにする。
   - これが**継ぎ目隠し**の主役。遠景リング + fog の2点で void 露出は解消する。
6. 上空は既存の sky 処理を活かす: `CreateOutdoorSkyWashTreatment` 系を**リングより上の空**として残し、`CreateOutdoorVoidBackgroundTreatment`（平面スラブ `:21546`）は**新ビスタで置換または背面へ撤去**（スラブが遠景の手前に出ないこと）。

### 3-4. 呼び出し配線
7. 既存の outdoor 構築が各 area の current/past に対して呼ばれている箇所（`CreateOutdoor*` の呼び出し元、`CreateHouseMap` 系を grep して特定）に `CreateDistantPanoramaVista(root, prefix, past, area, materials)` を1行で足す。**既存呼び出しのリファクタはしない**（helper + 1行配線の原則）。

### 3-5. 検証メソッド（cycle-runner の Validate フェーズ）
8. `private static void ValidateFastVsHd2dDistantPanoramaVista()` を追加し `ValidateHouseSliceBatch()`（`:592`）の末尾付近に呼び出しを足す。アサート:
   - 各 area の current/past root 下に "DistantVista" 親が存在。
   - リングのセグメント数が想定値（例 14×バンド数）以上。
   - `RenderSettings.fog == true` かつ fogColor が想定レンジ。
   - 平面 void スラブが遠景の手前に残っていない（撤去または背面）。
   失敗時は `throw new InvalidOperationException(...)`（既存 Validate と同形式、例: `:50974`）。

## 4. 二次改善（ビスタ確立後、優先度順）

監査が挙げた環境のブロックアウト性。ビスタの後に着手:
1. **植生の実体化**: 緑のキューブ/球プレースホルダ → 低ポリの木・草モデル（Meshy image→3D）。`ValidateImportedAssetsBatch` でポリ数検収。
2. **地面の質感**: 均一タイル → PolyHaven の CC0 PBR（blender-mcp 経由で取得）で土/石畳/草の塗り分け。**`Assets/Art/.../Anemora_Zone1_Atlas_512.png`（512px）を 2K 化**してテクセル密度を上げる。
3. **ライティング**: current/past の Volume プリセット化 + APV 再ベイク。renderer feature は触らない（凍結）。

## 5. Smoke / Acceptance

1. コンパイル + 検証（Unity バッチ）:
   ```powershell
   & "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit `
     -projectPath . -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile validate.log
   ```
   期待: ログに `error CS` 0、Validate の例外なし、末尾 return code 0。
2. レンダラ凍結が壊れていないこと:
   ```powershell
   & "...Unity.exe" -batchmode -projectPath . -runTests -testPlatform EditMode -testResults rc.xml -logFile rc.log
   ```
   期待: `RendererFeatureSet_MatchesFrozenBaseline` が **Passed**（fog/skybox は feature でないので落ちない。落ちたら renderer feature を触ってしまっている → 戻す）。
3. アセット検収:
   ```powershell
   & "...Unity.exe" -batchmode -quit -projectPath . -executeMethod Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch -logFile assets.log
   ```
   期待: `[AssetValidation] OK ...`。over-tris や review-only 混入が出たら対処。
4. 視覚レビュー: 既存の全マップ Wide キャプチャ（`CaptureChapter1AllMaps...` 系、出力 `01_a1_a2_current.png` 〜 `12_f1_f6_past.png`）を撮り、(a) どのマップも端に void が無い、(b) 遠景の山並みが円形に連なって見える、(c) カメラ移動で遠景がパララックスする、を確認。
   - `~/codex-cycle-kit/tools/shotdiff/` で前サイクルとの差分トリアージ → 変化したマップだけ目視（G2 縮小）。
5. 生 Unity バッチは副作用で `Assets/AddressableAssetsData/link.xml` 等を変えることがある。サイクル後に `git status` を見て、意図しない変更は `git checkout --` で戻す（cycle-runner 経由なら hardening 済）。

## 6. Open Risks / 触ってはいけない箇所

- **renderer 凍結**: URP の Renderer Feature を足す/消す/並べ替えると EditMode テストが落ちる。遠景・fog・skybox は feature でないので可。迷ったら §5-2 を回す。
- **外部固有名の禁止**: コミット文・ブランチ名・メソッド名・マテリアル名・asset パス・本書を含む doc に外部の商用タイトル名を出さない（公開 hygiene）。参照は「遠景パノラマ / open-world distant vista」。
- **決定論規律**: 配置の乱れに `Random`/`Time`/`DateTime` を使わない（cycle の hash 等価が壊れる）。
- **plateau ガード**: ビスタの“見え”が出ないまま sky 色 polish を足し続けない。出ないなら「遠景メッシュの質・距離・fog レンジ・カメラ farClip」のどれかが原因。手法を疑い、構成（メッシュ/距離/fog）から見直す。
- **authored file 規模**: `AnemoraFastVsHouseSliceSetup.cs` は 5.9MB/81k行（bloat-guard allowlist 済）。メソッド追加は可だがコンパイル時間に留意。減量（partial 分割等）は別タスクで、cycle 方式を畳む時に行う（毎サイクル編集中の今は merge 衝突を生むため未着手）。
- **過去/現在の二重空間**: 各 area に current/past root がある。ビスタは両方に作る。past は意匠を変える（監査 memory: 過去マップは現在の瓦礫雑然を引き継がず、別の見た目）。
- **farClip / fog の相互作用**: fogEnd を farClip より十分内側に。遠景リングが fogEnd より遠いと消える。R と fogEnd の整合を 3-2/3-3 で合わせる。
