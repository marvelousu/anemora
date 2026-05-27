# HD-2D Sun Cycle 仕様書

| 項目 | 内容 |
|---|---|
| 対象ブランチ | `work/fast-vs-hd2d-shading-foundation-20260522` (継続) |
| 環境 | Unity 6.3 (6000.3.14f1) / URP 17.3.0 / Forward / HDR / Linear |
| スコープ | 動的太陽駆動 (AnemoraSunCycleDriver) + MapSunAnchor + SunPreset(4種) |
| 関連 | god rays (Buto) / Volumetric / VFX は本仕様書のスコープ外。HD2D_GODRAYS_SPEC.md (未作成) で別途定義 |
| 仕様作成日 | 2026-05-28 |
| 仕様作成 | Claude (cross-review session) |
| 実装担当 | Codex メインセッション |
| 前提合意 | 評価書 `OneDrive/work/projects/anemora_reference/hd2d_max_quality_evaluation_20260527.md` §10 の対話で確定した7項目 |

---

## 0. 前提と用語

### 0.1 既存 Tier 宣言からの逸脱

既存 `docs/HD2D_IMPLEMENTATION_PROPOSAL.md` (2026-05-18) は Tier 2 (簡素版) を宣言していた。本仕様書は Tom の対話判断 (2026-05-27〜28) によりこの宣言を上書きし、**Tier 3 相当 (動的太陽 + Volumetric + 多灯) に拡張**する。PITCH.md / SPEC.md の Tier 宣言更新は本仕様のスコープ外だが、本仕様適用後にメンテ更新が必要。

### 0.2 用語

| 用語 | 定義 |
|---|---|
| **SunPreset** | 太陽の状態を表す enum。`Morning` / `Noon` / `Evening` / `Night` の4値 |
| **SunPresetData** | 各 SunPreset の具体値 (Euler/Color/Intensity/Cookie/Sky/Fog/LUT 等) を保持する ScriptableObject |
| **AnemoraSunCycleDriver** | SunPreset を実際の Light/Volume/RenderSettings に反映する MonoBehaviour。シーンに1つ存在 |
| **MapSunAnchor** | シーン内に配置する MonoBehaviour。「このシーンは何時か」を 1 つの enum 値で表現 |
| **マップ遷移** | シーン遷移 (LoadScene / Additive Load) を指す。マップ遷移=時刻切替のタイミング |
| **paired-space** | TimeWindow Portal 越しに見える過去側シーン。本仕様では「現在と同一太陽」とする |

### 0.3 設計原則

1. **拡張容易性**: マップ追加時にコード変更不要(MapSunAnchorを置くだけ)
2. **イベント駆動への余地**: シーン途中で時刻を変える Runtime API を MapSunAnchor 側に持つ
3. **Codex 受渡しやすさ**: API は MonoBehaviour + ScriptableObject + enum のみで完結、依存ライブラリなし
4. **Fallback 安全性**: MapSunAnchor 未配置でも Default(`Noon`) で起動、エラーで死なない
5. **既存コードへの侵襲最小化**: FastVsHouseLightingDirector の4灯セット構造は維持、Main Directional のみ Driver が制御
6. **TimeWindow整合**: paired-space は現在側と同一太陽 (Portal越しの色味は変わらない)

---

## 1. SunPreset 4種 推奨値

`SunPresetData` ScriptableObject 4 個を作成する。Asset パスは
`Assets/Settings/SunCycle/SunPreset_<Name>.asset`。

参考画像 `reference_01` (乾燥地帯俯瞰、強い昼光と god rays) / `reference_02` (焚き火夜営、暖寒対比) を参考に、HD-2D ジャンルの一般推奨値を組み合わせて Claude が起点値を提示。Tom がリファレンスを見ながら Unity Editor で微調整する想定。

### 1.1 Morning (朝)

太陽は東寄り、低い俯角。暖かい朝の橙が世界を侵食。Chapter 開始のため穏やかなトーン。

| 項目 | 値 | 備考 |
|---|---|---|
| directionEuler | (X=24, Y=-118, Z=0) | 低い太陽、東寄り。X 大なるほど高い太陽 |
| lightColor | RGB(1.00, 0.85, 0.72) | 暖橙 |
| lightIntensity | 1.6 | URP gamma |
| cookieTint | RGB(1.0, 0.92, 0.78) | 暖白 |
| cookieSize | 11.0 | 木漏れ日pattern の投影スケール |
| skyTint | RGB(0.85, 0.72, 0.55) | Procedural Skybox Tint Color |
| skySunSize | 0.042 | Procedural Skybox SunSize |
| skySunSizeConvergence | 5 | 暈け強さ |
| fogColor | RGB(0.92, 0.78, 0.60) | 朝靄 |
| fogDensity | 0.012 | 弱め |
| bloomTint | RGB(1.00, 0.92, 0.78) | 暖白 |
| ambientLightColor | RGB(0.55, 0.50, 0.42) | RenderSettings.ambientLight |
| colorLookup | `LUT_Morning_Warm.png` | Contribution 0.6 |
| volumeTemperature | +12 | WhiteBalance.temperature |
| volumeTint | 0 | WhiteBalance.tint |

### 1.2 Noon (昼)

太陽はほぼ天頂寄り。最も明るく、コントラスト最大。god rays の見栄えが最も出る。

| 項目 | 値 | 備考 |
|---|---|---|
| directionEuler | (X=70, Y=-12, Z=0) | ほぼ真上、わずか南寄り |
| lightColor | RGB(1.00, 0.97, 0.90) | ほぼ白、わずか暖 |
| lightIntensity | 2.4 | 最大 |
| cookieTint | RGB(1.00, 0.98, 0.92) | 白 |
| cookieSize | 9.5 | やや密 |
| skyTint | RGB(0.62, 0.78, 0.96) | 青空 |
| skySunSize | 0.040 | 中 |
| skySunSizeConvergence | 8 | 強い |
| fogColor | RGB(0.82, 0.88, 0.95) | 青寄り |
| fogDensity | 0.008 | 透明寄り |
| bloomTint | RGB(1.00, 0.97, 0.90) | ニュートラル |
| ambientLightColor | RGB(0.48, 0.52, 0.55) | 中間 |
| colorLookup | `LUT_Daylight.png` | Contribution 0.6 |
| volumeTemperature | +8 | (現状値、維持) |
| volumeTint | 0 | |

### 1.3 Evening (夕)

太陽は西寄り、低い俯角。強い橙赤、長い影。reference_01 に最も近い帯。

| 項目 | 値 | 備考 |
|---|---|---|
| directionEuler | (X=12, Y=58, Z=0) | 低い太陽、西寄り |
| lightColor | RGB(1.00, 0.62, 0.42) | 強橙赤 |
| lightIntensity | 1.7 | 強いがNoonより低い |
| cookieTint | RGB(1.00, 0.72, 0.48) | 暖橙 |
| cookieSize | 12.0 | 大きいが影が長い |
| skyTint | RGB(0.92, 0.55, 0.42) | 橙の空 |
| skySunSize | 0.055 | 大 |
| skySunSizeConvergence | 4 | 柔らかい |
| fogColor | RGB(0.78, 0.52, 0.42) | 橙靄 |
| fogDensity | 0.018 | 強め (god rays が浮く) |
| bloomTint | RGB(1.00, 0.72, 0.48) | 暖橙 |
| ambientLightColor | RGB(0.50, 0.38, 0.32) | 暖暗 |
| colorLookup | `LUT_GoldenHour.png` | Contribution 0.7 (強め) |
| volumeTemperature | +18 | 強暖 |
| volumeTint | +4 | わずか magenta |

### 1.4 Night (夜)

月明かり相当の弱い寒色 Directional 1 灯。屋内 Point Light が暖の主役、対比で焚き火が映える。reference_02 に対応。

| 項目 | 値 | 備考 |
|---|---|---|
| directionEuler | (X=-35, Y=8, Z=0) | 月、低い俯角、北寄り |
| lightColor | RGB(0.42, 0.55, 0.72) | 寒青 |
| lightIntensity | 0.40 | 弱い |
| cookieTint | RGB(0.55, 0.65, 0.85) | 寒青 |
| cookieSize | 13.0 | 柔らかく広い |
| skyTint | RGB(0.10, 0.12, 0.22) | 深青黒 |
| skySunSize | 0.020 | 小 (=月の Disk) |
| skySunSizeConvergence | 10 | くっきり |
| fogColor | RGB(0.18, 0.22, 0.30) | 寒夜 |
| fogDensity | 0.022 | 濃いめ (夜の空気感) |
| bloomTint | RGB(0.78, 0.82, 1.00) | わずか寒 (HDR emission側で暖が乗る) |
| ambientLightColor | RGB(0.18, 0.22, 0.28) | 暗寒 |
| colorLookup | `LUT_Night_CoolBlue.png` | Contribution 0.7 |
| volumeTemperature | -12 | 寒 |
| volumeTint | -4 | わずか緑寄り |

### 1.5 LUT テクスチャ

4 種類のLUTを `Assets/Art/LUT/` に配置。32x32x32 (1024x32) PNG。

- `LUT_Morning_Warm.png`: 暖色ミッドトーン + 弱青影
- `LUT_Daylight.png`: ニュートラルフィルミック(現状値ベース)
- `LUT_GoldenHour.png`: 強い橙ハイライト + 紫影
- `LUT_Night_CoolBlue.png`: 寒色ミッドトーン + 黒締め + わずか紫ハイライト

LUT 入手:
- 自作 (DaVinci Resolve / Affinity Photo 等)
- Asset Store の Cinematic LUT Pack ($10-25)
- OSS の filmic LUT (GitHub に複数あり、要License確認)

Tom と相談: Phase B 開始時に LUT パック 1 つ購入を推奨。

---

## 2. AnemoraSunCycleDriver API

### 2.1 ファイル

`Assets/Scripts/FastVS/SunCycle/AnemoraSunCycleDriver.cs` (新規)

### 2.2 公開シグネチャ

```csharp
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Anemora.FastVS.SunCycle
{
    public enum SunPreset
    {
        Morning = 0,
        Noon = 1,
        Evening = 2,
        Night = 3,
    }

    [DisallowMultipleComponent]
    public sealed class AnemoraSunCycleDriver : MonoBehaviour
    {
        public static AnemoraSunCycleDriver Instance { get; private set; }

        [Header("References")]
        [SerializeField] private Light directionalSunLight;
        [SerializeField] private Volume globalVolume;

        [Header("Presets (要素4: Morning/Noon/Evening/Night の順)")]
        [SerializeField] private SunPresetData[] presets = new SunPresetData[4];

        [Header("Defaults")]
        [SerializeField] private SunPreset defaultPreset = SunPreset.Noon;
        [SerializeField, Range(0.05f, 10f)] private float transitionDuration = 1.8f;
        [SerializeField] private AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public SunPreset CurrentPreset { get; private set; }
        public SunPreset TargetPreset { get; private set; }
        public bool IsTransitioning { get; private set; }

        public event Action<SunPreset, SunPreset> OnPresetChanged;     // (from, to) ApplyPreset 完了時
        public event Action<SunPreset, SunPreset> OnPresetTransitionStart; // (from, to) 遷移開始時

        public void ApplyPreset(SunPreset preset, bool instant = false);
        public void RegisterMapSunAnchor(MapSunAnchor anchor);
        public SunPresetData GetPresetData(SunPreset preset);
    }
}
```

### 2.3 内部仕様

- `Awake()` で `Instance` を設定 (Singleton)。Duplicate なら Destroy
- `OnEnable()` で `SceneManager.sceneLoaded += HandleSceneLoaded`
- `OnDisable()` で event 解除
- `HandleSceneLoaded(Scene, LoadSceneMode mode)`:
  - mode == Additive なら無視 (paired-space 等の追加ロード)
  - mode == Single なら `FindFirstObjectByType<MapSunAnchor>(FindObjectsInactive.Exclude)` で取得、ある→`ApplyPreset(anchor.SunPreset, instant: true)`、無し→`ApplyPreset(defaultPreset, instant: true)`
- `RegisterMapSunAnchor(MapSunAnchor)`:
  - 各 Anchor の `OnEnable()` から呼ばれる
  - 引数 anchor の Priority が現在の Active Anchor より高ければ採用 (複数置き対応)
  - `ApplyPreset(anchor.SunPreset, instant: anchor.TransitionFromPrevious ? false : true)` を呼ぶ
- `ApplyPreset(SunPreset, bool instant)`:
  - `instant == true` なら遷移なしで即値適用
  - `instant == false` なら `transitionDuration` 秒で Lerp 開始 (Coroutine or Update)
- `Update()`:
  - `IsTransitioning == true` の間、`Lerp(prevData, targetData, t)` で値計算 → `ApplyValues(currentBlended)` を呼ぶ
- `ApplyValues(SunPresetData blended)`:
  - `directionalSunLight.transform.rotation = Quaternion.Euler(blended.directionEuler)`
  - `directionalSunLight.color = blended.lightColor`
  - `directionalSunLight.intensity = blended.lightIntensity`
  - `directionalSunLight.cookie = blended.cookieTexture` (Lerp 不可、targetData 値で即切替)
  - `directionalSunLight.cookieSize = blended.cookieSize`
  - `globalVolume.profile.TryGet<ColorLookup>(out var cl)` で texture/contribution 更新
  - `globalVolume.profile.TryGet<WhiteBalance>(out var wb)` で temperature/tint 更新
  - `globalVolume.profile.TryGet<Bloom>(out var bl)` で tint 更新
  - `RenderSettings.ambientLight = blended.ambientLightColor`
  - `RenderSettings.fogColor = blended.fogColor`
  - `RenderSettings.fogDensity = blended.fogDensity`
  - `RenderSettings.skybox.SetColor("_SkyTint", blended.skyTint)` (Procedural Skybox)
  - `RenderSettings.skybox.SetFloat("_SunSize", blended.skySunSize)`

### 2.4 Lerp ルール

| 項目 | Lerp 方式 |
|---|---|
| directionEuler | `Quaternion.Slerp` (Euler を一度 Quaternion 化) |
| lightColor / cookieTint / skyTint / fogColor / bloomTint / ambientLightColor | `Color.Lerp` (RGB linear) |
| lightIntensity / cookieSize / skySunSize / skySunSizeConvergence / fogDensity | `Mathf.Lerp` |
| volumeTemperature / volumeTint | `Mathf.Lerp` |
| colorLookup texture | Lerp 不可、`contribution` を 0 → 0.6 で texture 入れ替えのクロスフェード(2段階) |
| cookieTexture | 即切替、Lerp 不可 |

LUT のクロスフェード:
1. 遷移開始時に旧 LUT contribution を t (1→0) で減衰
2. 同時に新 LUT contribution を t (0→0.6) で増加
3. 0.5 秒経過した時点で旧 LUT texture を新 LUT に差し替え、contribution を旧↔新で再計算

実装が複雑であれば、まず単純に「遷移中 LUT contribution=0、遷移完了時に新 LUT を contribution=0.6 で適用」のシンプル版で開始する。

### 2.5 SunPresetData ScriptableObject

`Assets/Scripts/FastVS/SunCycle/SunPresetData.cs` (新規)

```csharp
using UnityEngine;

namespace Anemora.FastVS.SunCycle
{
    [CreateAssetMenu(fileName = "SunPreset", menuName = "anemora/HD2D/Sun Preset Data")]
    public sealed class SunPresetData : ScriptableObject
    {
        public SunPreset preset;

        [Header("Directional Light")]
        public Vector3 directionEuler;            // (X=pitch, Y=yaw, Z=roll)
        [ColorUsage(false, true)] public Color lightColor;
        [Range(0f, 4f)] public float lightIntensity = 1f;

        [Header("Cookie (木漏れ日)")]
        public Texture2D cookieTexture;
        [ColorUsage(false, false)] public Color cookieTint = Color.white;
        [Range(1f, 30f)] public float cookieSize = 9.5f;

        [Header("Sky (Procedural Skybox)")]
        [ColorUsage(false, false)] public Color skyTint;
        [Range(0f, 0.2f)] public float skySunSize = 0.04f;
        [Range(1f, 20f)] public float skySunSizeConvergence = 5f;

        [Header("Fog (RenderSettings)")]
        [ColorUsage(false, false)] public Color fogColor;
        [Range(0f, 0.2f)] public float fogDensity = 0.012f;

        [Header("Bloom (Volume.Bloom.tint)")]
        [ColorUsage(false, true)] public Color bloomTint = Color.white;

        [Header("Ambient (RenderSettings.ambientLight)")]
        [ColorUsage(false, false)] public Color ambientLightColor;

        [Header("Color Lookup (Volume.ColorLookup)")]
        public Texture2D colorLookup;
        [Range(0f, 1f)] public float lutContribution = 0.6f;

        [Header("White Balance (Volume.WhiteBalance)")]
        [Range(-100f, 100f)] public float volumeTemperature = 0f;
        [Range(-100f, 100f)] public float volumeTint = 0f;
    }
}
```

---

## 3. MapSunAnchor API

### 3.1 ファイル

`Assets/Scripts/FastVS/SunCycle/MapSunAnchor.cs` (新規)

### 3.2 公開シグネチャ

```csharp
using UnityEngine;

namespace Anemora.FastVS.SunCycle
{
    [AddComponentMenu("anemora/HD2D/Map Sun Anchor")]
    [DisallowMultipleComponent]
    public sealed class MapSunAnchor : MonoBehaviour
    {
        [Header("Preset")]
        [SerializeField] private SunPreset sunPreset = SunPreset.Noon;

        [Header("Priority (同シーン内に複数MapSunAnchorがある場合の優先度。大が優先)")]
        [SerializeField] private int priority = 0;

        [Header("Transition")]
        [Tooltip("true: 前シーンの SunPreset から transitionDuration 秒で Lerp / false: 即値で切替 (Default: false 推奨)")]
        [SerializeField] private bool transitionFromPrevious = false;

        [Header("Debug")]
        [SerializeField] private bool drawGizmo = true;

        public SunPreset SunPreset => sunPreset;
        public int Priority => priority;
        public bool TransitionFromPrevious => transitionFromPrevious;

        public void SetPresetAtRuntime(SunPreset newPreset, bool instant = false);

        private void OnEnable();        // Driver に自身を Register
        private void OnDrawGizmos();    // Scene View に太陽の方向を可視化
    }
}
```

### 3.3 内部仕様

- `OnEnable()`:
  - `AnemoraSunCycleDriver.Instance` (Singleton) を取得
  - 無ければ `FindFirstObjectByType<AnemoraSunCycleDriver>()`
  - 取得できたら `driver.RegisterMapSunAnchor(this)` を呼ぶ
  - 取得できなければ `Debug.LogWarning("[MapSunAnchor] AnemoraSunCycleDriver が見つかりません. Default は Noon")` を出力(エラーで死なない)
- `SetPresetAtRuntime(SunPreset, bool instant)`:
  - `sunPreset` フィールドを上書き
  - `driver.ApplyPreset(sunPreset, instant)` を呼ぶ
  - イベントスクリプトから「シーン途中で時刻を変える」用途
- `OnDrawGizmos()`:
  - 太陽位置を Euler から計算
  - Anchor の Transform 位置から太陽の方向に Gizmos.DrawLine で 5m 線を引く
  - SunPreset 名を Handles.Label で表示

### 3.4 Inspector 表示

Tom が Unity Editor で:
1. シーンに `MapSunAnchor` を Empty GameObject に AddComponent
2. Inspector で SunPreset を dropdown 選択(Morning/Noon/Evening/Night)
3. Scene View で太陽方向の Gizmo を確認
4. Play で動作確認

複数シーン編集が直感的(クリック→値変更で完了)。

---

## 4. 既存コードとの接続

### 4.1 FastVsHouseLightingDirector.cs

既存ファイル: `Assets/Scripts/FastVS/FastVsHouseLightingDirector.cs`

**改修方針**:
- 4灯セット (mainLight=Directional, warmFillLight=Point, coolRimLight=Directional, libraryWindowLight=Spot) のうち、**Main Directional は Driver 制御に移管**
- warmFill / coolRim / libraryWindow は Director 制御を維持(エリア別の暖寒対比、SunCycle に依存しない)
- `GetUnifiedSunKeyLightEulerDegrees(FastVsHouseArea area)` メソッドを削除 (または `[Obsolete]` でDeprecate)
- Director の `ApplyLightingSnapshot` で Main Directional に書き込んでいた値を Driver に置換:
  - `mainLight.transform.rotation = ...` → Driver が制御するので Director では書かない
  - `mainLight.color = ...` → 同上
  - `mainLight.intensity = ...` → 同上
  - `mainLight.cookie = ...` → 同上
- 上記の代わりに、Director の `BeginAreaTransitionForReview()` 等でエリア遷移時に `AnemoraSunCycleDriver.Instance.ApplyPreset(...)` を呼ぶ必要は **無い** (MapSunAnchor が SceneLoad 時にやる)

**ただし例外**: もし「同一シーン内でエリアの切替で時刻が動くケース」がある場合(Chapter1 で Home 室内 → Home 外、を別シーンにせず内部エリア管理している場合)、Director の area transition で `MapSunAnchor.SetPresetAtRuntime` 相当を呼び出すロジックが必要。Tom と確認: **HouseSlice の area enum (Interior / Exterior / CentralPlaza / Library) は別シーンか同一シーン内のエリアか**。

回答待ち。仮: HouseSlice は同一シーン内のエリア管理 → エリア遷移時にも SunPreset を切り替えたい場合、MapSunAnchor を複数置き or 単一 MapSunAnchor の SetPresetAtRuntime を Director から呼ぶ。

### 4.2 FastVsRealtimeLightShadowRig.cs

既存ファイル: `Assets/Scripts/FastVS/FastVsRealtimeLightShadowRig.cs` (1399行)

**改修方針**:

A. **Painted Overlay 削除** (Codex 判断)
   - 次のメンバ・メソッドを完全削除:
     - `EnsureCycle128CameraGrade` / `EnsureCycle131CameraPaint`
     - `Apply~CameraGrade` / `ApplyCycle131CameraPaintOverlay`
     - `SetCycle128CameraGradeActive` / `UpdateCycle128CameraGradeScale`
     - 関連の `cycle128GradeRoot` / `cycle128GradeRenderer` / `cycle131SunPaintRoot` 等のフィールド
     - Texture 生成ロジック (`Generate*Texture` の Painted 用部分)
     - `FastVS_CentralPlazaPaintedSoftShadow` / `Cycle128GradeTexture` / `Cycle131SunPaintTexture` の Name 定数
   - 結果: 1399行 → 推定 500-600 行台に短縮

B. **0.35s 周期の ApplyRendererShadowPolicy 廃止**
   - 既存: `LateUpdate` 内で 0.35s 経過判定 → 全 Renderer 走査
   - 新規: `Awake()` で 1 回 + `SceneManager.sceneLoaded` で 1 回 + Director の `BeginAreaTransitionForReview` 完了時に 1 回
   - 周期判定ロジック削除、走査ロジック (`FindObjectsByType<Renderer>` + foreach Material Role Tag 判定) は維持

C. **ApplyLightAndSky 改修**
   - 現状: area 別に if/else でDirectional の rotation/color/intensity/cookie を直接書く + Painted Overlay を有効化
   - 新規: Painted Overlay 関連を削除、Directional 制御は Driver に任せるので **Sky/Fog/Ambient だけを area 別に管理** (ただし SunCycle 適用後の値を上書きしないように、Director 順序を SunCycle 後にする)
   - 推奨: Sky/Fog/Ambient も SunCycle に統合し、Rig 側は WarmFill/CoolRim/LibraryWindow の Point/Spot Light 制御に専念

D. **Procedural Sun Cookie の生成**
   - 既存: `FastVS_CentralPlazaRealtimeSunCookieCycle147` の Plaza 専用 Cookie を Rig 内で生成
   - 改修方針: 4 種類の SunPreset 用 Cookie を `SunPresetData.cookieTexture` に静的に持たせる(Procedural 生成は維持するが、SunPresetData の `OnValidate` で生成して assign する Editor ツール化推奨)
   - または手動で `.png` を `Assets/Art/Cookies/` に配置

### 4.3 シーン側の作業

`Anemora_FastVS_HouseSlice.unity`:
1. Hierarchy に空 GameObject 「SunCycle」 作成
2. `AnemoraSunCycleDriver` を Add Component
3. directionalSunLight に既存 Directional Light を Assign
4. globalVolume に既存 Volume を Assign
5. presets に 4 個の SunPresetData を Assign
6. 同シーン内に空 GameObject 「MapSunAnchor」 作成
7. `MapSunAnchor` Add Component、SunPreset = (Chapter1の起点なら) `Morning`

`Chapter2` 以降の各シーンも同様に MapSunAnchor を1個ずつ配置。

---

## 5. TimeWindow paired-space 整合

仕様: **paired-space は現在側と同一太陽**

実装上の含意:
- Portal 越しに見える paired-space シーンは Additive Load される(と想定)
- `HandleSceneLoaded(Scene, LoadSceneMode.Additive)` では Driver は何もしない(現在側の SunPreset を継続)
- paired-space シーン内に MapSunAnchor を置く必要なし(置いても無視される)
- `PortalStencilFeature` (Renderer Feature) は Volume を共有するため、現在側の LUT/Fog/Bloom がそのまま適用される
- 結果: Portal越しは色味が変わらない、時代の表現は別手段(BGM/SFX/アセット差分/シルエット 等)に委ねる

### 5.1 将来拡張の余地

paired-space で別 SunPreset を使いたくなった場合の余地を残す:
- `MapSunAnchor.isPairedSpace = true` フラグを追加
- Driver 側で `LoadSceneMode.Additive` でも `isPairedSpace == true` の Anchor が見つかったら、別 Volume Profile に切替

ただし本仕様の Phase A スコープ外。実装しない。

---

## 6. Migration 手順 (Phase A の実装順序)

Codex 引渡し時の推奨手順:

### Step 0. 準備
- Buto / Artistic: Tilt Shift は Phase B 着手時に購入判断 (Phase A は無料完結)
- LUT 4 枚: OSS filmic LUT (MIT / CC0) を GitHub から取得し `Assets/Art/LUT/` に配置 (§10 Q3 回答)
- ブランチ: 既存 `work/fast-vs-hd2d-shading-foundation-20260522` 継続 (Tom 判断、別ブランチを切るなら `work/fast-vs-sun-cycle-driver`)
- Codex 着手前のサブタスク: `Anemora_FastVS_HouseSlice.unity` の area 管理方式 (同一シーン or 別シーン) を判定し devlog に記録 (§10 Q1 回答)

### Step 1. SunPreset/Driver/Anchor の枠を作る
1. `SunPreset` enum 定義
2. `SunPresetData` ScriptableObject 実装
3. 4 個の SunPresetData asset を作成 (Morning/Noon/Evening/Night、推奨値を入力)
4. `AnemoraSunCycleDriver` 実装 (Singleton, SceneLoaded フック, ApplyPreset, Update Lerp)
5. `MapSunAnchor` 実装 (OnEnable で Driver Register)
6. HouseSlice シーンに SunCycle GameObject + MapSunAnchor を配置 (Anchor の SunPreset = Morning 想定)
7. Editor で Play し、シーン Load 時に Driver が ApplyPreset(Morning) を呼ぶことを確認

### Step 2. 既存コード接続
1. `FastVsHouseLightingDirector.ApplyLightingSnapshot` から Main Directional 書込みを削除
2. `FastVsHouseLightingDirector.GetUnifiedSunKeyLightEulerDegrees` を Obsolete 化
3. `FastVsRealtimeLightShadowRig.ApplyLightAndSky` から Main Directional 書込みを削除 (Sky/Fog/Ambient は SunCycle に統合済なので Rig 側からも削除)
4. Editor で Play、ロゴ・遷移・エリア切替で Main Directional が Driver 経由で動くことを確認

### Step 3. Painted Overlay 完全削除
1. `FastVsRealtimeLightShadowRig.cs` から `Ensure*` / `Apply*Camera*` / `SetCycle128CameraGradeActive` / `cycle128GradeRoot` 等を完全削除
2. 削除後の Rig が Build 通ることを確認
3. 5 エリアスクショ取得し、god rays が消えたことを確認 (これは想定動作、Phase B で Buto が god rays を提供)

### Step 4. 0.35s 周期廃止
1. `ApplyRendererShadowPolicy` の周期判定を撤去
2. `Awake()` で 1 回、`SceneManager.sceneLoaded` で 1 回、Director の `BeginAreaTransitionForReview` 完了で 1 回呼ぶように変更
3. Editor で Play、Profiler で CPU 時間が 2.0ms 削減されることを観測

### Step 5. シェーダ軽量化
1. `FastVS_SurfaceRampLit.shader` の `Cull Off` → `Cull Back`
2. 自前 8-tap PCF の削除、URP `MainLightRealtimeShadow` 標準呼び出しのみに統一
3. `FastVS_SpriteCardRampUnlit.shader` の `Cull Off` は維持 (両面 abs(NdotL) 必須)
4. Build → 5 エリアスクショ → Tom 確認

### Step 6. 受入確認
- スクショ並置: 参考画像 reference_01 / 02 + 現在 + Migration 前
- 受入条件 §7 を満たすか Tom が判定
- 受入されたら commit + push (handoff skill 使用推奨)

---

## 7. 受入条件 (Phase A)

Phase A 完了の判定基準。**Claude/Codex の自己「OK」判定は禁止**。Tom が以下を 5 / 5 確認できたら完了。

1. **動的太陽が動く**: HouseSlice (Morning) → Plaza (Noon) → Library (Evening) と シーン遷移 した時、Directional Light の rotation/color/intensity が遷移し、影の方向が画面で変わる
2. **同一シーン内で MapSunAnchor.SetPresetAtRuntime() が動く**: Editor デバッグ用ボタンで時刻を強制切替 → 1.8 秒で Lerp 遷移する
3. **Painted Overlay が削除されている**: 旧 Cycle128/131 関連のクラス/メソッド/フィールドが `FastVsRealtimeLightShadowRig.cs` から消えている (grep で 0 件)
4. **0.35s 周期が消えている**: Profiler で `LateUpdate > FastVsRealtimeLightShadowRig.ApplyRendererShadowPolicy` のフレーム呼び出しが 0、SceneLoad 時のみ呼ばれる
5. **FPS が 60 維持**: Plaza 広域シーンで 60fps 安定 (旧版より 5-10 fps 向上を期待)

加えて TimeWindow が破綻していないこと:
- Portal 越しの paired-space で god rays/Cycle128 が二重表示にならない (削除済なので消えている想定)
- アパーチャ PNG (`tw_current_aperture.png`) が Migration 前後で大きく変わらない

---

## 8. 検証手順

### 8.1 ビルド前の Editor 検証
1. `Anemora_FastVS_HouseSlice.unity` シーンを開く
2. Hierarchy に SunCycle + MapSunAnchor が存在することを確認
3. Play モード起動
4. Scene View で Directional Light の rotation が MapSunAnchor の値に合うか確認
5. MapSunAnchor の SunPreset を Inspector で変更 → SetPresetAtRuntime → 1.8 秒で Lerp遷移
6. Console にエラー / 警告ゼロ

### 8.2 ビルド検証
1. `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe` をビルド
2. 起動 (フォルダごと、.exe 単体不可)
3. 5 エリア (Home / HomeOutside / Plaza / Plaza_NiroInShadow / Library) のスクショ取得 → `docs/review/<timestamp>/`
4. 参考画像 (`OneDrive/anemora_reference/reference/reference_01.png` / `02.png`) と並置
5. TimeWindow アパーチャ PNG を目視 (`project_anemora_timewindow_aperture` 準拠)

### 8.3 シーン資産 grep 検証
1. `Anemora_FastVS_HouseSlice.unity` に `MapSunAnchor` 文字列が 1 個以上出現
2. `AnemoraSunCycleDriver` 文字列が 1 個出現
3. `Cycle128` / `Cycle131` 関連文字列が `FastVsRealtimeLightShadowRig.cs` から消滅
4. `FastVsHouseLightingDirector.cs` から `mainLight.transform.rotation = ...` 等の Directional 直書きが消滅
5. シーン資産検証は `project_anemora_pipeline_provenance_gap` 準拠で実値を grep

### 8.4 Profiler 検証
1. Plaza シーンで Play
2. Window > Analysis > Profiler を開く
3. CPU Usage > Player Loop の `FastVsRealtimeLightShadowRig.LateUpdate` を確認、Migration 前後で:
   - 旧: 0.35 秒に 1 回 ~2.9ms スパイク
   - 新: SceneLoad 時のみ 1 回、それ以降フレーム呼び出しなし
4. GPU > Forward Lighting の Shadow Pass がほぼ同等 (Cull Off → Back で減るが微小)

---

## 9. リスクと対処

### R1. Director と Driver の制御競合
- 既存 Director の area transition 中に Driver が SunPreset を変えると、Director の `BeginAreaTransitionForReview` で Lerp している値が打ち消される
- 対処: Director が Main Directional に書き込まないように `ApplyLightingSnapshot` を改修(§4.1)。Director は warmFill/coolRim/libraryWindow のみ管理

### R2. シーン内に MapSunAnchor が無い
- 想定: 新規シーンを作って Anchor を置き忘れる
- 対処: Driver の `defaultPreset` (Noon) が fallback、`Debug.LogWarning` で気付ける

### R3. transitionDuration が長すぎて画面が暗くなり続ける
- 想定: シーン遷移時に旧時刻 → 新時刻の Lerp が 1.8 秒、その間プレイヤーは画面を見ている
- 対処: **`MapSunAnchor.transitionFromPrevious` のデフォルト値を `false` で確定**(§10 Q2 回答済)。Tom が「遷移を見せたい」と思うシーンだけ Inspector で true に上書き

### R4. LUT クロスフェードの実装が複雑
- 対処: Phase A は LUT contribution を遷移中 0 にする単純版で開始、Phase C で本格クロスフェードを Codex に追加実装させる

### R5. APV (Adaptive Probe Volumes) との競合
- APV は Bake 済のライティングを Probe で配信、Directional 動的回転を行うと Bake と齟齬が生じる
- 対処: APV を **静的環境光の供給源として割り切る**(ambient light の補完)、Directional 直接光は Driver で動的、Indirect は APV で静的。これは標準的な構成
- 必要に応じて APV を 4 SunPreset 別に Re-bake する手もあるが Phase A スコープ外

---

## 10. Tom 確認回答済 (2026-05-28)

すべて回答済。Codex 引渡し時はこの仕様で着手可。

1. **HouseSlice の area enum は同一シーン内のエリア管理か、別シーン管理か**
   - **回答: Codex 調査タスクとして残す**。SPEC は両方カバーする記述を維持
   - Codex は実装前に `Anemora_FastVS_HouseSlice.unity` の構造を読み、area=Interior/Exterior/CentralPlaza/Library が同一シーン内か別シーンか判定する
   - 同一シーン内なら: MapSunAnchor を 4 個置く or Director の `BeginAreaTransitionForReview` から `SetPresetAtRuntime` を呼ぶ実装を追加
   - 別シーン管理なら: 各シーンに MapSunAnchor を 1 個ずつ配置するだけ
   - 判定結果と採用実装を `docs/devlog/<date>_fast_vs_hd2d_sun_cycle_area_decision.md` に記録

2. **`transitionFromPrevious` のデフォルトを true/false**
   - **回答: false をデフォルト、Anchor ごとに個別上書き可**
   - `MapSunAnchor.transitionFromPrevious` のフィールドデフォルト値を `false` に
   - Tom がシーンごとに「ここは遷移を見せたい」と思うシーンで Inspector から `true` に上書き
   - SetPresetAtRuntime からの呼び出しは `instant = false` デフォルト (Lerp する) で運用

3. **LUT 4 枚をどう用意するか**
   - **回答: OSS の filmic LUT (GitHub 公開) を採用**
   - Codex は GitHub で MIT / CC0 ライセンスの filmic LUT 集を探し、以下に近い 4 種類をダウンロード:
     - Morning_Warm: 暖色ミッドトーン + 弱青影 (Kodak Vision3 250D 系)
     - Daylight: ニュートラルフィルミック (Filmic Cinematic)
     - GoldenHour: 強橙ハイライト + 紫影 (Sunset/Magic Hour 系)
     - Night_CoolBlue: 寒色ミッドトーン + 黒締め (Bleach Bypass / Night 系)
   - ファイル配置: `Assets/Art/LUT/LUT_<Preset>.png` (32x32x32 = 1024x32 px)
   - License記載を `docs/THIRD_PARTY_LICENSES.md` (or 既存の同等文書) に追加

4. **MapSunAnchor の Gizmo (Scene View 太陽方向表示)**
   - **回答: はい、実装する**
   - `OnDrawGizmos()`:
     - SunPreset の Euler から Sun の方向ベクトルを算出
     - `Gizmos.color = Color.yellow`
     - `Gizmos.DrawLine(transform.position, transform.position + sunDir * 5f)` で 5m の黄色線
     - `Handles.Label(transform.position + sunDir * 5f, sunPreset.ToString())` で末端に "Morning" 等の表示
     - 太陽位置に小さい球 `Gizmos.DrawSphere(transform.position + sunDir * 5f, 0.2f)`

---

## 11. 参照文書

| 文書 | 用途 |
|---|---|
| `OneDrive/anemora_reference/hd2d_max_quality_evaluation_20260527.md` | 評価書(本仕様の根拠) |
| `OneDrive/anemora_reference/hd2d_implementation_plan.md` | 旧実装計画(05-25, Stage1-2 完了済) |
| `OneDrive/anemora_reference/hd2d_research.md` | HD-2D 一般技術調査(05-25) |
| `OneDrive/anemora_reference/reference/reference_01.png` | 参考画像 (god rays / 俯瞰ジオラマ) |
| `OneDrive/anemora_reference/reference/reference_02.png` | 参考画像 (焚き火夜営 / 暖寒対比) |
| `docs/HD2D_IMPLEMENTATION_PROPOSAL.md` | 旧 HD-2D 提案(Tier 2 宣言、本仕様で上書き) |
| `docs/devlog/2026-05-27_fast_vs_hd2d_stage8n_library_loose_page_clusters.md` | 最新の Library 装飾 devlog |

---

## 12. 履歴

| 日付 | 内容 | 著者 |
|---|---|---|
| 2026-05-28 | 初版作成 (Phase A 仕様) | Claude (cross-review session) |
| 2026-05-28 | §10 Tom 確認 4 項目回答済(Codex調査タスク含む、transitionFromPrevious=false default、OSS LUT、Gizmo採用) | Claude (cross-review session) |
