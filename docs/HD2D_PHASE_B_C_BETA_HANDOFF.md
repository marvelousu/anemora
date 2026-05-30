# HD-2D Phase B-β / C-β 実装 handoff (Codex 向け)

| 項目 | 内容 |
|---|---|
| 対象ブランチ | `work/chapter1-continuation-map-vs-20260524` |
| worktree | `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample` |
| 環境 | Unity 6000.3.14f1 / URP 17.3 / Forward / HDR / Linear / Mono backend / **PC のみ** |
| 起点コミット | `8e6acc57` (HD-2D foundation マージ + 検証済) |
| 作成 | Claude (cross-review)、2026-05-30。実装は Codex |
| 検証 | Unity batchmode (Claude が実証した手順、§4) |

---

## 1. Context

### 1.1 経緯
- HD-2D foundation (Phase A-C: SunCycleDriver/MapSunAnchor、Painted Overlay 撤去、event 駆動 shadow policy、URP標準 Volumetric/Lens Flare、Emissive VFX) を chapter1 継続ブランチにマージ済 (`5c7c510b`〜`8e6acc57`)。compile/validate/build/runtime/Play 検証済。
- A〜B-α〜C-α は完了。本 handoff は **購入アセット導入** の2段:
  - **B-β**: URP標準 Volumetric を **Buto** の Volumetric Fog + Sun god rays に置換
  - **C-β**: 自前 Tilt-Shift を **Fronkon Artistic: Tilt Shift** に差替

### 1.2 import 済アセット (Tom が手動 import 済) — **公開リポなので commit 禁止**
- **Buto**: `Packages/com.occasoftware.buto/` (ソース実体が worktree に存在)。scripting define `BUTO` / `OCCASOFTWARE` は `Editor/Symbols/AddCustomScriptingSymbols.cs:14` が自動注入 (手動不要)
- **Artistic: Tilt Shift**: `Assets/FronkonGames/Artistic/TiltShift/`
- ⚠️ **ライセンス制約**: anemora は public リポ (`feedback_anemora_full_public`)。Buto/Fronkon は**有償 Asset Store アセット**で、ソースを public リポに commit すると EULA (Single Entity) 違反になる。**これらのフォルダは .gitignore して絶対に commit しない**。各開発環境 (Codex の worktree 含む) が**自分の Asset Store アカウントから import** すること。
- `packages-lock.json` の Buto 参照行や `ProjectSettings/*` の変更も、有償パッケージの所在を晒さない範囲で扱う (基本コミットしない、または Buto 参照を含まない差分のみ)。

### 1.3 既存の C-β scaffold (Codex が cycle178 で作成済、活用すること)
生成器に Phase C-β の **検出/診断** ロジックが既にある (実 swap は未実装、Fronkon 未 import だったため fallback 維持):
- `ValidateHd2dPhaseCBetaArtisticTiltShiftAdoptionBatch` (`AnemoraFastVsHouseSliceSetup.cs:1894`)
- `ValidateHd2dPhaseCBetaArtisticTiltShiftAdoptionReportState` (`:48599`)
- `BuildHd2dPhaseCBetaArtisticTiltShiftAdoptionDiagnosticsLines` (`:48606`)
- renderer 検出: `:48710` `ContainsAnyToken(rendererAssetText, Stage7TiltShiftFeatureName)`
- 診断レポート出力先: `docs/devlog/screenshots/fast_vs_hd2d_phase_c_beta_artistic_tiltshift_adoption_cycle178_parent_review_20260528_01/phase_c_beta_artistic_tiltshift_adoption_diagnostics.md`
→ **この診断ロジックを、Fronkon import 済を前提に「実 swap + 採用判定」へ拡張する**。ゼロから作らない。

### 1.4 ゴールと採否
- B-β / C-β は実装後、**B-α vs B-β / C-α vs C-β を 5エリア並置スクショして Tom が採用判断**。採用されない結果もあり得る ($55 はリスク投資)。採否を devlog に記録。

---

## 2. Numbered Mechanical Fixes

### Step 0. 有償アセットを gitignore (commit せず、各自 import) — **公開リポ前提**
**有償アセットのソースを public リポに commit しない**。代わりに `.gitignore` で除外し、Buto/Fronkon は各環境が Asset Store My Assets から import する運用。
```
cd C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample
# .gitignore に追記 (まだ無ければ):
#   /Assets/FronkonGames/
#   /Assets/FronkonGames.meta
#   /Packages/com.occasoftware.buto/
git checkout -- Assets/AddressableAssetsData/link.xml Assets/AddressableAssetsData/link.xml.meta   # Addressables副作用を戻す
git status   # FronkonGames/ buto/ が untracked のまま & ignore されることを確認
```
注意: `Packages/manifest.json`/`packages-lock.json` に Buto を local/registry 参照として残す場合でも、`Packages/com.occasoftware.buto/` のソース実体は ignore する。CI/別 clone では Buto/Fronkon が物理的に存在しない前提で、import 手順を README/handoff に明記。Tom と「有償アセットの配布方法 (各自 import / 私的ミラー等)」を要確認。

### Step 1. C-β — 自前 Stage7 TiltShift を無効化
- Renderer asset: `Assets/Settings/UniversalRenderPipeline_Renderer.asset` の **`m_Name: FastVS HD2D Stage7 TiltShift`** (line 70) ブロックの `m_Active: 1` (line 72) → **`m_Active: 0`**。
- ただしこの asset は生成器が ensure する。`AnemoraFastVsHouseSliceSetup.cs` の Stage7 TiltShift feature ensure ロジック (`:49713` 付近 `fullScreenFeature.name == Stage7TiltShiftFeatureName`、material/shader ロード `:49720-49721`) を見て、**生成器側で feature の `SetActive(false)` 相当 or 追加スキップ** に変える (手で asset を編集しても生成器が再生成で上書きするため、生成器が真実の源 — `project_anemora_pipeline_provenance_gap`)。
- const: `Stage7TiltShiftFeatureName="FastVS HD2D Stage7 TiltShift"` (`:117`), `Stage7TiltShiftShaderName="Anemora/FastVS/TiltShiftFullscreen"` (`:118`), `Stage7TiltShiftMaterialPath=.../FastVS_House_hd2d_stage7_tilt_shift.mat` (`:119`)。Shader/Material は残置で良い (feature を切るだけ)。

### Step 2. C-β — Fronkon Artistic Tilt Shift を有効化
§3.B 参照。
1. Renderer asset に `FronkonGames.Artistic.TiltShift.TiltShift` (ScriptableRendererFeature) を追加 (生成器で programmatic に。既存の Stage7 ensure と同じ手法で `rendererData.rendererFeatures.Add(...)` + `EditorUtility.SetDirty`)。
2. `DefaultVolumeProfile.asset` に `TiltShiftVolume` (VolumeComponent) override を追加: `profile.Add<FronkonGames.Artistic.TiltShift.TiltShiftVolume>(true)`。
3. `intensity.overrideState=true; intensity.value=1f`。
4. HD-2D Octopath 風の初期値 (要 Tom 調整):
   - `angle.value = 0f` (水平バンド = 上下ボケ)
   - `aperture.value = 0.7f` (中央シャープ帯の幅、狭め)
   - `offset.value = 0f` (主人公が画面中央なら0、俯瞰で下寄せたいなら負値)
   - `blur.value = 1.2f`、`blurCurve.value = 3f`、`quality = Quality.High`
   - focused/unfocused color は neutral 維持

### Step 3. B-β — Buto Volumetric Fog 有効化
§3.A 参照。
1. Renderer asset に `OccaSoftware.Buto.Runtime.ButoRenderFeature` (ScriptableRendererFeature) を追加 (programmatic、RenderPassEvent 既定)。
2. `DefaultVolumeProfile.asset` に `ButoVolumetricFog` (VolumeComponent) override を追加: `profile.Add<OccaSoftware.Buto.Runtime.ButoVolumetricFog>(true)`。
3. `mode` を `VolumetricFogMode.On`、`mode.overrideState=true`。
4. 既存 URP標準 Volumetric を切る: `CreateHd2dAtmosphere` (`:41107`) 周辺で URP の volumetric fog を有効化している箇所があれば無効化 (Buto と二重描画回避)。`SunPresetData.volumetricFogEnabled` の用途も Buto 側に切替。
5. Camera に `postProcessEnabled=true` (CreateCamera `:38559` で確認、既定 true のはず)。

### Step 4. B-β — Sun god rays (ButoLight)
- Directional Light は `CreateLighting` (`:38610`) の `:38612` `new GameObject("Directional Light", typeof(Light))` → `light.type=LightType.Directional` (`:38614`) で生成され、`:38656/38666` で LightingDirector/RealtimeRig に SerializedSet される。
- ここで **`light.gameObject.AddComponent<OccaSoftware.Buto.Runtime.ButoLight>()`** を追加。`inheritDataFromLightComponent=true` (Sun の色/強度を継承)。god rays は ButoLight が無いと出ない (URP main light 単独では散乱しない)。
- 既存の `ApplyDirectionalLightVolumetricScattering` (Driver `:464`、reflection で useVolumetricScattering を立てる) は Buto 採用時は不要 → ButoLight に置換 or 併存可 (URP標準を切るなら reflection 経路も無効化)。

### Step 5. SunCycle 連動 (時刻で fog を出し分け)
- `SunPresetData` (`Assets/Scripts/FastVS/SunCycle/SunPresetData.cs`) は **既に Volumetric Fog フィールドを持つ** (`:29-34`):
  `volumetricFogEnabled`(bool), `volumetricAnisotropy`(float -1..1, 既定0.6), `volumetricMeanFreePath`(0..500, 既定100), `volumetricBaseHeight`(float), `volumetricMaximumHeight`(0..500, 既定30)。
  → **新フィールド追加せず Buto にマップ** (推奨): Buto `anisotropy.value = volumetricAnisotropy`、Buto `fogDensity.value ≈ 1f / max(1, volumetricMeanFreePath)` (meanFreePath は density の逆数的意味)、Buto `baseHeight.value = volumetricBaseHeight`、Buto `maxDistanceVolumetric`/`attenuationBoundarySize` に `volumetricMaximumHeight` を割当。色を時刻連動したいなら `litColor`/`shadowedColor` 用に `fogColor`(既存) を流用 or `butoLitColor` フィールド新設。
- Driver 拡張点 (`Assets/Scripts/FastVS/SunCycle/AnemoraSunCycleDriver.cs`, 1501行):
  - `ApplyVolumetricFog(SunRuntimeValues values)` (`:461`) ← **ここを Buto 駆動に書き換える**。現在 URP標準 volumetric 想定。`VolumeManager.instance.stack.GetComponent<ButoVolumetricFog>()` または `globalVolume.profile.TryGet<ButoVolumetricFog>(out var buto)` で取得し、`buto.fogDensity.overrideState=true; buto.fogDensity.value=...` 等を設定。
  - `ApplyVolumeValues` (`:422`、TryGet<ColorLookup/WhiteBalance/Bloom> の既存パターン) と同じ書き方で揃える。
  - `ApplyOptionalSunEffects` (`:456`) が `ApplyDirectionalLightVolumetricScattering`(`:464`) + `ApplyVolumetricFog`(`:461`) を呼ぶ。Buto 採用時は前者を ButoLight 有効化に変更。
  - lerp: `SunRuntimeValues.Lerp` (`:107`) で preset 間補間。Buto パラメータも lerp 対象にするなら SunRuntimeValues に持たせる (既存 volumetric* が SunRuntimeValues に入っているか確認 `:404` 付近)。
- C-β tilt-shift も時刻連動するなら同様に `TiltShiftVolume` を Driver で TryGet して focus を調整可 (任意、まずは静的でよい)。

### Step 6. 生成器配線
- B-β/C-β の Volume override 追加は `CreateHd2dGlobalVolume` (`:38833`、`AssetDatabase.LoadAssetAtPath<VolumeProfile>("Assets/Settings/DefaultVolumeProfile.asset")` → `profile.TryGet<T>(out _)` else `profile.Add<T>(true)` パターン、実例 `:40959-40971`) に追記、または新規 `CreateHd2dPhaseBBetaButo` / `CreateHd2dPhaseCBetaTiltShift` を作り `CreateHouseSliceScene` (本体 `:467`) の `CreateHd2dAtmosphere`/`CreateHd2dDepthFraming` 近辺で呼ぶ。
- Renderer feature の programmatic 追加は既存 Stage7 ensure ロジック (`:49700` 付近) を参考に。

---

## 3. API 詳細

### 3.A Buto (recon 確定)
- **構成**: ① `OccaSoftware.Buto.Runtime.ButoRenderFeature` (ScriptableRendererFeature) を URP ScriptableRendererData に追加 + ② `OccaSoftware.Buto.Runtime.ButoVolumetricFog` (VolumeComponent) を Volume profile に追加 + ③ god rays は `OccaSoftware.Buto.Runtime.ButoLight` (MonoBehaviour, `[RequireComponent(Light)]`) を Light に付与。
- **ButoVolumetricFog 主要 VolumeParameter**: `mode`(VolumetricFogMode Off/On), `fogDensity`(MinFloatParameter), `anisotropy`(ClampedFloatParameter), `maxDistanceVolumetric`(MinFloatParameter), `baseHeight`(FloatParameter), `attenuationBoundarySize`(MinFloatParameter), `litColor`/`shadowedColor`(ColorParameter), `directionalForward`/`directionalBack`(ColorParameter), `noiseWindSpeed`(Vector3Parameter), `noiseTiling`(MinFloatParameter), `octaves`(ClampedIntParameter), `gain`/`lacunarity`(ClampedFloatParameter)。`QualityLevel` enum: Low/Medium/High/Cinematic/Custom。
- **ButoLight 主要プロパティ**: `lightColor`(Vector4 `:30`), `lightIntensity`(float `:65`), `lightRange`(float `:50`), `inheritDataFromLightComponent`(bool `:13`), `bias`(float `:46`)。
- **runtime 取得**: `VolumeManager.instance.stack.GetComponent<ButoVolumetricFog>()` (例: `Samples/DemoContent/Scripts/AnimateButoSetting.cs:22`) または `profile.TryGet<ButoVolumetricFog>(out var fog)`。設定は `fog.fogDensity.overrideState=true; fog.fogDensity.value=...`。
- **写経用 (editor/生成器スニペット)**:
  ```csharp
  using OccaSoftware.Buto.Runtime;
  // Volume profile に override
  if (!profile.TryGet<ButoVolumetricFog>(out var buto)) buto = profile.Add<ButoVolumetricFog>(true);
  buto.mode.overrideState = true; buto.mode.value = VolumetricFogMode.On;
  buto.anisotropy.overrideState = true; buto.anisotropy.value = 0.6f;
  buto.fogDensity.overrideState = true; buto.fogDensity.value = 0.02f;
  // Directional sun に god ray
  var butoLight = sunLight.gameObject.GetComponent<ButoLight>() ?? sunLight.gameObject.AddComponent<ButoLight>();
  butoLight.inheritDataFromLightComponent = true;
  // Renderer feature 追加 (ScriptableRendererData rd):
  // 既存 Stage7 ensure 同様に rd.rendererFeatures に ButoRenderFeature を Add し EditorUtility.SetDirty(rd)
  ```
- **gotchas**: (1) ButoRenderFeature が renderer asset に**無いと fog が出ない** (Volume だけでは描画されない)。(2) god rays は ButoLight 必須、Sun の URP main light 単独では散乱しない。(3) ButoLight は Light コンポーネント同居必須。(4) Camera は postProcessEnabled=true 必要、preview/reflection camera はスキップ。(5) define BUTO/OCCASOFTWARE は自動注入。
- ファイル: `Packages/com.occasoftware.buto/Runtime/Overrides/ButoVolumetricFog.cs:15`, `Runtime/Renderer Features/ButoRenderFeature.cs:9`, `Runtime/Components/ButoLight.cs:10`, `Runtime/Utilities/Params.cs:5`。

### 3.B Fronkon Artistic Tilt Shift (recon 確定)
- **構成**: ① `FronkonGames.Artistic.TiltShift.TiltShift` (ScriptableRendererFeature) を renderer asset に追加 + ② `FronkonGames.Artistic.TiltShift.TiltShiftVolume` (VolumeComponent, IPostProcessComponent) を Volume profile に追加。RenderGraph の2パス Blit (水平→垂直の分離ブラー)。
- **TiltShiftVolume 主要パラメータ** (`Runtime/TiltShiftVolume.cs:45-100`): `intensity`(FloatSliderParameterLinear [0,1]), `quality`(enum High/Normal/Fast), `angle`(deg [-90,90]、**0=水平バンド=上下ボケ**), `aperture`([0.1,5] 焦点帯の幅、小=狭), `offset`([-1.5,1.5] 帯の縦位置), `blur`([0,10]), `blurCurve`([1,10] 落ち方), `distortion`/`distortionScale`(歪み), `focusedBrightness/Contrast/Gamma/Hue/Saturation`(焦点域のみ), `unfocusedXXX`(ボケ域のみ)。
- **runtime 取得**: `VolumeManager.instance.stack.GetComponent<TiltShiftVolume>()`。有効化条件: `intensity.overrideState=true && intensity.value>0` かつ postProcessEnabled。
- **写経用**:
  ```csharp
  using FronkonGames.Artistic.TiltShift;
  if (!profile.TryGet<TiltShiftVolume>(out var ts)) ts = profile.Add<TiltShiftVolume>(true);
  ts.intensity.overrideState = true; ts.intensity.value = 1f;
  ts.angle.overrideState = true; ts.angle.value = 0f;        // 水平バンド
  ts.aperture.overrideState = true; ts.aperture.value = 0.7f;
  ts.offset.overrideState = true; ts.offset.value = 0f;
  ts.blur.overrideState = true; ts.blur.value = 1.2f;
  ts.blurCurve.overrideState = true; ts.blurCurve.value = 3f;
  // Renderer feature: rd.rendererFeatures に TiltShift を Add + SetDirty
  ```
- **gotchas**: angle=0 が水平焦点帯 (Octopath 風)、±90 で縦。aperture 小で焦点帯が狭い。intensity<=0 / overrideState=false / postProcess off で無効。blur は quality enum で内部係数 (Fast=4/Normal=2/High=1) 倍。Debug mode は Editor のみ。define 不要。
- ファイル: `Assets/FronkonGames/Artistic/TiltShift/Runtime/TiltShift.cs:33`, `TiltShiftVolume.cs:31`, `TiltShift.Pass.cs:76-150`, `Resources/Shaders/ArtisticTiltShift_URP.shader`, `Runtime/Internal/Constants.cs:19` (ShaderName='ArtisticTiltShift')。

### 3.C 生成器の差込点 (`Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`)
| 用途 | メソッド / 行 |
|---|---|
| Volume profile + override 追加 | `CreateHd2dGlobalVolume` `:38833` (Load `DefaultVolumeProfile.asset` `:38841`、`profile.TryGet<T>()`/`profile.Add<T>(true)` パターン実例 `:40959-40971`) |
| Camera 生成 | `CreateCamera` `:38559` |
| ライト群生成 (Sun) | `CreateLighting` `:38610` → Directional Light `:38612-38614`、director/rig SerializedSet `:38656/38666` (**ButoLight をここで sun に付与**) |
| SunCycle 配線 | `CreateHd2dPhaseASunCycleSceneWiring` `:38855` |
| 大気/フォグ | `CreateHd2dAtmosphere` `:41107` (URP volumetric を切る箇所候補) |
| APV | `CreateHd2dStage7ApvVolumes` `:41455` |
| Depth framing | `CreateHd2dDepthFraming` `:10522` (CreateHouseSliceScene `:503` で呼ぶ) |
| 自前 Stage7 TiltShift ensure | `:49700` 付近 (`fullScreenFeature.name==Stage7TiltShiftFeatureName` `:49713`、material/shader `:49720-49721`) → **無効化対象** |
| 既存 C-β 検出/診断 | `:48599` `ValidateHd2dPhaseCBetaArtisticTiltShiftAdoptionReportState`、`:48710` renderer 検出 → **拡張** |

**Renderer features** (`Assets/Settings/UniversalRenderPipeline_Renderer.asset`):
- `FastVS HD2D Stage7 TiltShift` (line 70, m_Active **1→0 にする**)
- `PortalStencilFeature` (line 90, 維持)
- `FastVS HD2D Soft Contact Occlusion` (SSAO, line 110, 維持)
- `FastVS HD2D Stage7 Outline` (line 136, 維持)
- **追加**: `ButoRenderFeature`, `FronkonGames...TiltShift` (programmatic)

### 3.E Optional 統合 (#if ガード) — **公開リポ compile-safety の必須要件** (採用方針: 選択肢1)
有償アセットは gitignore 済なので、**Buto/Fronkon を参照するコードは scripting-define ガードで囲み、アセット不在の公開 clone/CI でも compile が通る**ようにする。

- **assembly 構成 (確認済、好都合)**: 生成器 `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs` (asmdef 無し → `Assembly-CSharp-Editor`)、Driver `Assets/Scripts/FastVS/SunCycle/*.cs` (asmdef 無し → `Assembly-CSharp`) は**デフォルト assembly = 存在する asmdef を自動参照**。よって **asmdef のハード参照を足す/外す必要なし**。`#if` ガードだけで成立。
  - 注意: Buto/Fronkon の Runtime asmdef (`OccaSoftware.Buto.Runtime.asmdef` / `FronkonGames.Artistic.TiltShift.asmdef`) の **"Auto Referenced" が true** であること (false だとアセット在っても Assembly-CSharp から型が見えず compile 不可)。
- **Buto define**: `BUTO` / `OCCASOFTWARE` は `Packages/com.occasoftware.buto/Editor/Symbols/AddCustomScriptingSymbols.cs` の `[InitializeOnLoad]` が PlayerSettings に自動注入。Buto 参照コードは `#if BUTO ... #endif` で囲む。
  - **罠**: この注入は define を**追加するだけで削除しない**。Buto を後で外しても `BUTO` define が ProjectSettings に残り `#if BUTO` が有効のまま → 型欠落で compile 不可。アセット不在 compile テスト時は define を手動クリアする。
- **Fronkon define**: 自動 define **無し**。Codex が下記 auto-define エディタスクリプトを新規作成 (first-party コード = 公開コミット可)。`FRONKON_TILTSHIFT` を付与し、**不在時は自動除去**する (Buto より堅牢):
  ```csharp
  // Assets/Editor/FronkonTiltShiftDefineInjector.cs  (first-party、commit 可)
  #if UNITY_EDITOR
  using System.Linq; using UnityEditor; using UnityEditor.Build;
  [InitializeOnLoad]
  internal static class FronkonTiltShiftDefineInjector {
      const string Define = "FRONKON_TILTSHIFT";
      static FronkonTiltShiftDefineInjector() {
          bool present = System.AppDomain.CurrentDomain.GetAssemblies()
              .Any(a => a.GetType("FronkonGames.Artistic.TiltShift.TiltShiftVolume") != null);
          var grp = NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
          var defines = PlayerSettings.GetScriptingDefineSymbols(grp).Split(';').Where(s => s.Length > 0).ToList();
          bool has = defines.Contains(Define);
          if (present && !has) { defines.Add(Define); PlayerSettings.SetScriptingDefineSymbols(grp, string.Join(";", defines)); }
          else if (!present && has) { defines.Remove(Define); PlayerSettings.SetScriptingDefineSymbols(grp, string.Join(";", defines)); }
      }
  }
  #endif
  ```
  Fronkon 参照コードは `#if FRONKON_TILTSHIFT ... #endif` で囲む。
- **ガード対象**: §Step2-5 で書く Buto/Fronkon 型参照 (`using OccaSoftware.Buto.Runtime;`, `using FronkonGames.Artistic.TiltShift;`, `profile.Add<ButoVolumetricFog>()`, `TiltShiftVolume`, `ButoLight`, renderer feature の `ButoRenderFeature`/`TiltShift` 追加) はすべてガード内。define off 時は「自前 B-α/C-α のまま」になるよう else 経路 or 無処理にする。
- **既存パターン流用**: cycle178 の C-β scaffold は文字列/リフレクション検出 (`:48710`) で型参照を避けている。検出系はそのままでも compile-safe。実 swap の型参照部分だけ `#if` で囲めば良い。

### 3.D SunCycle (`Assets/Scripts/FastVS/SunCycle/`)
- `SunPresetData.cs` — フィールド: preset / directionEuler / lightColor / lightIntensity / cookieTexture,Tint,Size / skyTint,skySunSize,skySunSizeConvergence / fogColor,fogDensity / **volumetricFogEnabled, volumetricAnisotropy, volumetricMeanFreePath, volumetricBaseHeight, volumetricMaximumHeight** (`:29-34`) / bloomTint / ambientLightColor / screenSpaceLensFlareIntensity / sunLensFlareIntensity / colorLookup,lutContribution / volumeTemperature,volumeTint。
- `AnemoraSunCycleDriver.cs` (1501行): `ApplyValues` `:348` → `RenderSettings` (ambient/fog/skybox `:366-371`) + `ApplyVolumeValues` `:422` (TryGet<ColorLookup `:430`/WhiteBalance `:439`/Bloom `:448>`) + `ApplyOptionalSunEffects` `:456` → `ApplyDirectionalLightVolumetricScattering` `:464` (reflection `TrySetMemberValue` useVolumetricScattering 等 `:471-474`) + **`ApplyVolumetricFog` `:461`** ← **Buto 駆動の主拡張点**。preset 補間 `SunRuntimeValues.Lerp` `:107`。`MapSunAnchor` 経由で preset 適用 `:203/262-272`。
- **拡張方針**: `ApplyVolumetricFog` を ButoVolumetricFog 駆動に書換 (既存 volumetric* フィールドを Buto params にマップ、§Step5)。`ApplyDirectionalLightVolumetricScattering` を ButoLight 有効化に置換。新フィールドは原則不要 (既存で足りる)。

---

## 4. Smoke Test Steps (検証プロトコル — Claude 実証済)

Unity: `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe`。各 batchmode 実行は **自前 `EXIT=$?` echo + log の "return code"/例外 grep で真の合否判定** (background 完了通知の exit code は信用しない — 0 と出て実際 1 だった実績あり)。

0. **公開 compile-safety (Option1 の肝、最重要)**: 有償アセットを一時退避して compile が通るか確認。
   ```
   # Fronkon/Buto を一時退避 + define クリア
   mv Assets/FronkonGames /tmp/_fg ; mv Packages/com.occasoftware.buto /tmp/_buto
   # ProjectSettings/ProjectSettings.asset の scripting define から BUTO;OCCASOFTWARE;FRONKON_TILTSHIFT を一時除去
   Unity.exe -batchmode -quit -projectPath "<wt>" -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CreateHouseSliceScene -logFile "<wt>\Temp\v0.log"; echo EXIT=$?
   # 期待: error CS 0件、return code 0 (ガードで Buto/Fronkon コードが除外され compile 成功)
   mv /tmp/_fg Assets/FronkonGames ; mv /tmp/_buto Packages/com.occasoftware.buto   # 復元
   ```
   これが通らない = #if ガード漏れ or asmdef ハード参照が残っている。**公開リポを壊すので最優先で確認**。
1. **コンパイル+再生成 (アセット在)**: `Unity.exe -batchmode -quit -projectPath "<wt>" -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CreateHouseSliceScene -logFile "<wt>\Temp\v.log"; echo EXIT=$?`
   期待: log に `Fast VS house slice scene created`, `Exiting batchmode successfully now!`, `return code 0`、`error CS` 0件。define on で Buto/Fronkon コードが有効化される。
2. **ドア/ルート検証**: `-executeMethod ...ValidateHouseSliceBatch`
   期待: 例外なし `return code 0`、`missing chapter 1 continuation route marker` 無し。
3. **.exe ビルド**: `-executeMethod ...BuildAndValidateBatch`
   期待: `Fast VS house slice player built: ...Anemora_FastVS_HouseSlice.exe`, `return code 0`。
4. **.exe ランタイム** (PowerShell 18秒で kill):
   ```powershell
   $p=Start-Process "<wt>\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe" -ArgumentList @("-logFile","<wt>\Temp\p.log","-screen-width","960","-screen-height","540") -PassThru; Start-Sleep 18; if(!$p.HasExited){Stop-Process $p.Id -Force}
   ```
   期待: Player.log に GfxDevice 作成・scene load・**例外/NullReference 0件**。Buto compute shader / TiltShift のエラーが出ないこと。fps 極端低下が無いか。
5. **5エリアスクショ + Tom 目視ゲート**: home/Home_outside/plaza/plaza_niro_in_shadow/library + TimeWindow アパーチャ。B-α vs B-β / C-α vs C-β 並置。**自己判定せず Tom 提示**、exe フルパス添付 (`feedback_anemora_present_build_exe`, `feedback_anemora_gfx_quality_bar`)。
   - **⚠️ レビュー画像の運用 (順序依存)**: repo 衛生伝播 (`Documents/Unity/anemora-handoff-repo-hygiene-propagation.md`) が **先に** map-vs に入る。それが入った後は `docs/review/` / `docs/devlog/screenshots/` への **git add が bloat-guard に拒否される**。スクショは**ローカル生成のみ (git add しない)** とし、`tools\r2\r2-upload-review.ps1 -CycleDir docs/review/<ts> -Branch work/chapter1-continuation-map-vs-20260524` で **R2 へアップロード** (wrangler 認証要。`~/.cf_token` 削除済→ `wrangler login`)。devlog .md 本体はコミット可。衛生伝播がまだ入っていなければ従来通りで可。
6. **シーン資産 grep 実証**: 変更が `Anemora_FastVS_HouseSlice.unity` / `UniversalRenderPipeline_Renderer.asset` / `DefaultVolumeProfile.asset` に焼かれたか実値 grep。

---

## 5. Open Risks / 触ってはいけない箇所

1. **FilmGrain 非決定性** (`project_anemora_scene_regen_filmgrain`): 単発 `CreateHouseSliceScene` が DefaultVolumeProfile の FilmGrain を `active:0` に落とす。再生成後 `grep -c 'active: 1' Assets/Settings/DefaultVolumeProfile.asset` が **19** か確認。落ちてたら 2パス(`BuildAndValidateBatch`)再生成 or 手動再有効化してコミット。Buto/TiltShift の Volume override 追加で override 総数が増える点に注意 (期待値が 19 から変わる)。
2. **URP標準 Volumetric との二重** : Buto 有効化時は URP 標準 volumetric / `ApplyDirectionalLightVolumetricScattering` の reflection 経路を切る (二重描画/競合回避)。
3. **link.xml 削除**: Addressables 副作用。コミット前に `git checkout -- Assets/AddressableAssetsData/link.xml*`。
4. **生成シーン bloat** (`project_anemora_repo_bloat`): `Anemora_FastVS_HouseSlice.unity` 28MB。再生成毎に巨大 diff。レビュー PNG も含め無駄コミット回避。
5. **Light.shadowResolution 警告** (既存・非致命): ランタイムで毎フレーム出る。B-β/C-β で増減しないか一応見る。
6. **採用は Tom 判断**: 入れても自前 (B-α/C-α) が良ければ Buto/TiltShift override を `active:false`/feature 無効化で残置 or 撤去。採否を devlog に記録。
7. **スモークは Update バイパス** (`feedback_anemora_smoke_bypasses_update`): 見た目は `.exe` ランタイム + Tom Play が唯一の根拠。
8. **main は immutable** (`project_anemora_hd2d_chapter1_merge`): main へ merge しない。chapter1-continuation-map-vs 上で作業。
9. **TimeWindow アパーチャ**: Buto fog/god ray が Portal 越し paired-space で二重/破綻しないか `tw_*_aperture.png` 目視 (`project_anemora_timewindow_aperture`)。
10. **生成器が真実の源** (`project_anemora_pipeline_provenance_gap`): renderer asset / volume profile / scene を手編集しても生成器の再実行で上書きされる。**変更は生成器コードに入れて再生成**。
11. **#if ガード漏れ = 公開リポ破壊**: Buto/Fronkon 型参照が1つでもガード外に出ると、アセット不在環境で compile 不可。Smoke Test step 0 (アセット退避 compile) を必ず緑にしてからコミット。
12. **define の残留**: Buto の auto-define は追加のみ・削除しない。Fronkon は本 handoff の injector で不在時除去。アセット退避テスト時は ProjectSettings の `BUTO;OCCASOFTWARE;FRONKON_TILTSHIFT` を手動クリアしないと #if が誤って有効化されたままになる。
13. **Auto Referenced**: `OccaSoftware.Buto.Runtime.asmdef` / `FronkonGames.Artistic.TiltShift.asmdef` の Auto Referenced が true であること (false だとアセット在でも Assembly-CSharp から型が見えない)。
14. **順序依存 (repo衛生伝播が先)**: `anemora-handoff-repo-hygiene-propagation.md` を先に map-vs へ入れる。その後はレビュー画像を git add すると bloat-guard が拒否 → R2 運用 (§4 step5)。本 B-β/C-β を衛生伝播より先に着手する場合のみ従来コミット可。3本 (CI/CD → B-β/C-β → character) は別セッション・別ドメインで、生成器 (`AnemoraFastVsHouseSliceSetup.cs`) と .gitignore を共有するため、着手前に最新 origin を取り込む。

---

## 6. 参照
- SPEC (Phase A 太陽): `docs/HD2D_SUN_CYCLE_SPEC.md`
- 評価書: OneDrive `anemora_reference/hd2d_max_quality_evaluation_20260527.md`
- セッション記録: OneDrive `anemora_reference/session_log_2026-05-30_hd2d_merge.md`
- recon raw 結果: `C:\Users\maro6\AppData\Local\Temp\claude\C--Users-maro6\39e5576a-9021-4f8b-852c-fbce4b95752c\tasks\wrxz6jdc8.output` (Buto/Tilt の完全 keyApis/gotchas)
