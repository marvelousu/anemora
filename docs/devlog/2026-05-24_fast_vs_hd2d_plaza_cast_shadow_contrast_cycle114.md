# feat(hd2d): deepen plaza cast shadow contrast

## Scope

Cycle 114 follows parent review of Cycle 113. Cycle113 added visible plaza sunbeam shafts, but the current plaza still reads as a dark flat stage with light streaks rather than a reference-like space shaped by strong sunlight and shadow. This cycle adds current-world-only, non-colliding cast-shadow contrast anchors across the central plaza floor.

Authored file:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Expected generated side-effect files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_cast_shadow_contrast_cycle114.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_plaza_cast_shadow_contrast_cycle114.asset`

Out of scope:

- Main branch, map-layout changes, story/UI, doors, time-window behavior, character assets, house facade fixes, ProjectSettings, and `C:\Users\maro6\OneDrive\work\projects\anemora_reference\map_chapter_1\map_chapter_1`.

## Goal Prompt

Add stronger plaza cast-shadow contrast so the current central plaza reads closer to the reference image: a warm sunlit floor cut by broad, soft-edged, broken dark shadows. The change must prioritize shadow readability over map changes.

Reference directory:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference`

Map reference directory noted but not edited this cycle:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\map_chapter_1\map_chapter_1`

## Worker Cycle

- Cycle-worker: `019e5790-0339-7470-931c-5be681c597a5`
- Worker role: `cycle-worker` / `gpt-5.4-mini`

Scoped prompt trace:

`SCOPED_PROMPT_ISSUED cycle=114 authored_file=C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs validate=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaCastShadowContrastCycle114Batch capture=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaCastShadowContrastCycle114ScreenshotsBatch`

## Implementation Plan

- Add a generated transparent dark cast-shadow material/texture pair with broken penumbra edges and a readable center alpha.
- Add exactly four current-world-only, non-colliding horizontal quads in the central plaza: library facade falloff, approach diagonal, foreground cast band, and right-side breakup.
- Render the shadow contrast under later sunbeam/sunfield overlays, using `FastVsHd2dMaterialRole.ContactShadow`.
- Validate texture metrics, object count, current-only placement, no colliders, material/texture provenance, render queue, and prior Cycle103-113 validations.
- Capture current plaza overview/close plus past plaza and current library guards for parent visual review.

## Expected Evidence

Parent-review screenshots are expected under:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle114_plaza_cast_shadow_contrast_parent_review_20260524_01`

Expected files:

- `parent_review_01_current_central_plaza_cast_shadow_contrast_overview.png`
- `parent_review_02_current_central_plaza_cast_shadow_contrast_close.png`
- `parent_review_03_past_central_plaza_cast_shadow_contrast_guard.png`
- `parent_review_04_current_library_cast_shadow_contrast_guard.png`

## Validation

Planned cycle-runner command:

```powershell
$commitPaths = @(
  'Assets/Editor/AnemoraFastVsHouseSliceSetup.cs',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_cast_shadow_contrast_cycle114.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_cast_shadow_contrast_cycle114.mat.meta',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_cast_shadow_contrast_cycle114.asset',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_cast_shadow_contrast_cycle114.asset.meta',
  'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_cast_shadow_contrast_cycle114.md',
  'docs/devlog/INDEX.md',
  'docs/devlog/screenshots/fast_vs_hd2d_cycle114_plaza_cast_shadow_contrast_parent_review_20260524_01'
)
& .\tools\cycle-runner.ps1 -CycleNumber 114 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaCastShadowContrastCycle114Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaCastShadowContrastCycle114ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_cast_shadow_contrast_cycle114.md' `
  -Audience parent_review `
  -CommitPath $commitPaths `
  -NoRollback
```

## Visual Gate

Passing criteria:

- Current plaza overview shows a stronger sunlight/shadow composition at a glance.
- Shadow anchors are dark enough to shape the floor but remain feathered, broken, and non-blocking.
- Sunfield and sunbeam cycles still read above the new darker base.
- Past plaza and current library guard captures show no obvious regression.

## Cycle 114 failure (validate) -- 20260524-104440

```
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite.mat using Guid(f79bd52245f424d42b668da63a26eed7) (NativeFormatImporter) -> (artifact id: '6c7634a35d7a5e593d016924efb11022') in 0.0023941 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite.mat using Guid(f71c1a4bbbfa65e4a9dc8ceb1dbaab05) (NativeFormatImporter) -> (artifact id: '067c3ca3644ebbf71c59456785c01e71') in 0.0025228 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite.mat using Guid(28f2cae61d95125439828020176a4cfc) (NativeFormatImporter) -> (artifact id: '72016ad2a7c404cc799f9f50c6febaf5') in 0.0025035 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite.mat using Guid(a8c94ff07ef65ea4f8ad0dedea407b1e) (NativeFormatImporter) -> (artifact id: '63c8344cf78a915d285cd3eae78cb1fd') in 0.0022446 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite.mat using Guid(b8841e13281e616468837ca346676037) (NativeFormatImporter) -> (artifact id: '1adcf1ea1109f3defbf059ddda5acb22') in 0.002346 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite.mat using Guid(3acdb3d0d51b28d40854136863dba5b3) (NativeFormatImporter) -> (artifact id: '085d72f1844e837c1fa998fa9467b7c2') in 0.0021898 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.0022268 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce.mat using Guid(0bc31eecccdf1a840a5477d771512775) (NativeFormatImporter) -> (artifact id: '957df3d5816f00a64823db316222cbe2') in 0.0023341 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool.mat using Guid(0dd9370c56289ae4a8d0e1dddfc59477) (NativeFormatImporter) -> (artifact id: 'dff4420f29bfcf0b5e37a561766cc28b') in 0.0026072 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_route_move_floor_glow.mat using Guid(0dbdea9defe689a4ca05680804b173ca) (NativeFormatImporter) -> (artifact id: '76abd1cd4f418349642ae7527f51d632') in 0.0023726 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_back_sprite.mat using Guid(8d4799b24d7badb4d9d50d8f3546587c) (NativeFormatImporter) -> (artifact id: 'ea1cc68dacc196e87140893dcea1fad6') in 0.0024611 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.mat using Guid(0e535f48a85f05b43ad8f1b5cb2ecb4a) (NativeFormatImporter) -> (artifact id: '95a63720d5377afca26fb765e42e17fd') in 0.002527 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_contact_shadow.asset using Guid(40c9d8116f275fd4c98bbdde1e793a32) (NativeFormatImporter) -> (artifact id: '17d86434566b3eaf148ab06195b0eaff') in 0.0026466 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite_shaded.asset using Guid(11367ee44066a2c4a933e1a65a54fa71) (NativeFormatImporter) -> (artifact id: 'b4ea198999a098bff466fc482aa2e085') in 0.0036971 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite_shaded.asset using Guid(31848d2268cea5d4a93190a2365133ca) (NativeFormatImporter) -> (artifact id: 'dfcf40487733ba25c6e6bad9c26fa7e7') in 0.003351 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_library_window_light_cookie_soft.asset using Guid(e10a87518f970b6498f120403b5e87fa) (NativeFormatImporter) -> (artifact id: 'b6624ac7fb17375ff687066ce4c5208b') in 0.0028401 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_left_sprite_shaded.asset using Guid(f13736bef9726b045ba18071b949e2d2) (NativeFormatImporter) -> (artifact id: 'f6e48f8240644c12833b02b6b33897d1') in 0.0026325 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0022999 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce_soft.asset using Guid(0442e2f80b7dbc34e99a112ae1095f20) (NativeFormatImporter) -> (artifact id: 'a65e2dbc19e50b0fc0fa4598f134ffd8') in 0.0026066 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite_shaded.asset using Guid(246d9d1ed78f72147aa1f537daf9164e) (NativeFormatImporter) -> (artifact id: '59a4f02825ed43e073fd3814395235f5') in 0.002847 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.asset using Guid(64767a48f06c7a4439175ba9f3f76969) (NativeFormatImporter) -> (artifact id: '2c34ef17951f1b28aef444ba8ef4c761') in 0.002394 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_back_sprite_shaded.asset using Guid(646afdaa35aa1c84bbfca6d10ad8d956) (NativeFormatImporter) -> (artifact id: '71d57c8ff1c974a8f2d165c436f15504') in 0.0027809 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool_soft.asset using Guid(97afe9889f5e56f449c8468a1d0bfdf9) (NativeFormatImporter) -> (artifact id: 'b007a80dd3775e821df40936b5d7e159') in 0.0025229 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_back_sprite_shaded.asset using Guid(383cf440301bf504a9b6c2a8a85c5c10) (NativeFormatImporter) -> (artifact id: '9a14722ac27a7c483b3d93443ba97166') in 0.0031462 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite_shaded.asset using Guid(e8b91a1d7f94f9d419ae3d2cb8aca69c) (NativeFormatImporter) -> (artifact id: '19f48d248392446507eb0aa3bd573d8d') in 0.0030994 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_left_sprite_shaded.asset using Guid(29c76ef0ee79e294abc159d852e30d6f) (NativeFormatImporter) -> (artifact id: '7a1e2d14c3f7f57972b5a415bb755af8') in 0.0032035 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite_shaded.asset using Guid(8af717035fe354841a2778b67ef47f60) (NativeFormatImporter) -> (artifact id: '636319e0046c8cb6ef7b2f1e0d8a2fbf') in 0.0034013 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite_shaded.asset using Guid(bcf85f4b6024bd345a6725cd7e3f8122) (NativeFormatImporter) -> (artifact id: '1134ffa9b5a09269a7a99f311dd08502') in 0.0033749 seconds
Start importing Assets/Settings/DefaultVolumeProfile.asset using Guid(2dc156ea366288a46859f93b6d3c2cd0) (NativeFormatImporter) -> (artifact id: 'b03cefda0696e6248b213f7dfd6e20a5') in 0.0030631 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_right_sprite_shaded.asset using Guid(7edaa2e8ce6f3f040896d2d92f57e792) (NativeFormatImporter) -> (artifact id: '4e696edc4775714bf55149c9029e9a87') in 0.003115 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite_shaded.asset using Guid(8e8558fedae4d5441a0880d3e0ae5dde) (NativeFormatImporter) -> (artifact id: '7a36765b1626f751d5a96e3827f13ea3') in 0.0027054 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite_shaded.asset using Guid(afafc97e4daeeb2408f5830dde7c5f7c) (NativeFormatImporter) -> (artifact id: 'abb16328271b3f261d7f87df395bbbcc') in 0.0025261 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_front_sprite_shaded.asset using Guid(cfe6c4e4efe6c2d4bae69a5a1cb896ad) (NativeFormatImporter) -> (artifact id: 'a1fc470689ab7b251db5843fc102d4a4') in 0.0030591 seconds
Refreshing native plugins compatible for Editor in 0.67 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=f5847d0996a5ff0439ef592edc2ed978): Total: 0.200 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Scenes/Anemora_FastVS_HouseSlice.unity using Guid(5c2c1333e7d65ba4eac9369f107a6118) (DefaultImporter) -> (artifact id: '6a454be057f752116394bd5feb05e30e') in 0.0014237 seconds
Refreshing native plugins compatible for Editor in 0.59 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=60b6d6a13f6efc84ebf1ee31942e3c71): Total: 0.044 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Asset Pipeline Refresh (id=5d0ee54cf28fad24faf459be80513d07): Total: 0.027 seconds - Initiated by RefreshV2(NoUpdateAssetOptions)
Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:268)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaSunlitFloorIslandsVisibilityCycle110Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:44234)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaBroadSunfieldCycle111Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:44256)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaSunExposureBaseCycle112Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:44262)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaSunbeamShaftsCycle113Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:44268)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaCastShadowContrastCycle114Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:44274)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 268)

Opening scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
Unloading 0 Unused Serialized files (Serialized files now loaded: 0)
Loaded scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
	Deserialize:            266.948 ms
	Integration:            306.801 ms
	Integration of assets:  2.371 ms
	Thread Wait Time:       -2.339 ms
	Total Operation Time:   573.780 ms
Unloading 571 unused Assets / (4.8 MB). Loaded Objects now: 24250.
Memory consumption went from 263.9 MB to 259.1 MB.
Total: 13.104100 ms (FindLiveObjects: 2.544200 ms CreateObjectMapping: 0.608700 ms MarkObjects: 7.923100 ms  DeleteObjects: 2.026900 ms)

InvalidOperationException: House slice validation failed: cycle 114 plaza cast-shadow contrast texture must preserve a visible shadow peak without turning into a slab (shadowPeak=0.153, shadowAverage=0.052).
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaCastShadowContrastCycle114TextureMetrics () [0x00247] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:34335 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaCastShadowContrastCycle114 () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:34081 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaCastShadowContrastCycle114Batch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:44275 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 34335)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaCastShadowContrastCycle114Batch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[10:52:51] Phase 'validate' FAILED with exit 1
[10:52:51] NoRollback set; preserving worktree after validate failure
```
