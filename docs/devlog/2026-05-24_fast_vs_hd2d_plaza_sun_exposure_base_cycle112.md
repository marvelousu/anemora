# feat(hd2d): add plaza sun exposure base

## Scope

Cycle 112 follows parent review of Cycle 111. Cycle111 added a visible broad sunfield direction, but the current plaza still tended to read as light streaks on top of a dark floor. This cycle adds an under-shadow warm sun-exposure base so the floor itself reads as sunlit and the darker dappled shadows sit above it.

Authored file:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Expected generated side-effect files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_sun_exposure_base_cycle112.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_plaza_sun_exposure_base_cycle112.asset`

Out of scope:

- Main branch, map-layout changes, story/UI, doors, time-window behavior, character assets, house facade fixes, ProjectSettings, and `C:\Users\maro6\OneDrive\work\projects\anemora_reference\map_chapter_1\map_chapter_1`.

## Goal Prompt

Add a warm transparent base-light layer under the plaza's later shadow overlays so the current central plaza floor itself reads as sun-exposed. The effect must be broad and mottled, not a thin ray layer or flat color board.

Reference directory:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference`

Map reference directory noted but not edited this cycle:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\map_chapter_1\map_chapter_1`

## Worker Cycle

- Cycle-worker: `019e575b-3463-7762-9653-bec73ee0c523`
- Worker role: `cycle-worker` / `gpt-5.4-mini`

Scoped prompt trace:

`SCOPED_PROMPT_ISSUED cycle=112 authored_file=C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs validate=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSunExposureBaseCycle112Batch capture=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaSunExposureBaseCycle112ScreenshotsBatch`

## Implementation Plan

- Add a generated sun-exposure base material/texture pair with render queue below the later plaza shadow/highlight overlays.
- Add two or three current-world-only, non-colliding horizontal floor quads to warm the plaza floor underneath shadow detail.
- Validate texture metrics, render queue ordering, object count, current-only placement, material/texture provenance, non-collision, and overlay profile metadata.
- Capture current plaza overview/close, past plaza guard, and current library guard screenshots for parent visual review.

## Expected Evidence

Parent-review screenshots are expected under:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle112_plaza_sun_exposure_base_parent_review_20260524_01`

Expected files:

- `parent_review_01_current_central_plaza_sun_exposure_base_overview.png`
- `parent_review_02_current_central_plaza_sun_exposure_base_close.png`
- `parent_review_03_past_central_plaza_sun_exposure_base_guard.png`
- `parent_review_04_current_library_sun_exposure_base_guard.png`

## Validation

Planned cycle-runner command:

```powershell
$commitPaths = @(
  'Assets/Editor/AnemoraFastVsHouseSliceSetup.cs',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sun_exposure_base_cycle112.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sun_exposure_base_cycle112.mat.meta',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sun_exposure_base_cycle112.asset',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sun_exposure_base_cycle112.asset.meta',
  'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_sun_exposure_base_cycle112.md',
  'docs/devlog/INDEX.md',
  'docs/devlog/screenshots/fast_vs_hd2d_cycle112_plaza_sun_exposure_base_parent_review_20260524_01'
)
& .\tools\cycle-runner.ps1 -CycleNumber 112 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSunExposureBaseCycle112Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaSunExposureBaseCycle112ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_sun_exposure_base_cycle112.md' `
  -Audience parent_review `
  -CommitPath $commitPaths `
  -NoRollback
```

## Visual Gate

Passing criteria:

- Current plaza overview reads as a sunlit floor with shadows above it, not only as bright lines on a dark plane.
- The base light remains warm, mottled, and bounded, without becoming a flat yellow slab.
- Existing dappled and directional shadows remain visible over the base.
- Past plaza and current library guard captures show no obvious regression.

## Cycle 112 failure (validate) -- 20260524-092929

```
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_map_move_floor_glow.mat using Guid(373d809c8f785b3439635e59312211de) (NativeFormatImporter) -> (artifact id: '70688f62928dc32fdf23a0c22c596252') in 0.0024848 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite.mat using Guid(57d1354643503484f8f9504dec7939e7) (NativeFormatImporter) -> (artifact id: '8d6144afb428b20ddb5b28b485258685') in 0.0023177 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite.mat using Guid(f79bd52245f424d42b668da63a26eed7) (NativeFormatImporter) -> (artifact id: '6c7634a35d7a5e593d016924efb11022') in 0.0024899 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite.mat using Guid(f71c1a4bbbfa65e4a9dc8ceb1dbaab05) (NativeFormatImporter) -> (artifact id: '067c3ca3644ebbf71c59456785c01e71') in 0.0023137 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite.mat using Guid(28f2cae61d95125439828020176a4cfc) (NativeFormatImporter) -> (artifact id: '72016ad2a7c404cc799f9f50c6febaf5') in 0.0022362 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite.mat using Guid(a8c94ff07ef65ea4f8ad0dedea407b1e) (NativeFormatImporter) -> (artifact id: '63c8344cf78a915d285cd3eae78cb1fd') in 0.0024512 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite.mat using Guid(b8841e13281e616468837ca346676037) (NativeFormatImporter) -> (artifact id: '1adcf1ea1109f3defbf059ddda5acb22') in 0.002406 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite.mat using Guid(3acdb3d0d51b28d40854136863dba5b3) (NativeFormatImporter) -> (artifact id: '085d72f1844e837c1fa998fa9467b7c2') in 0.0024357 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.0022468 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce.mat using Guid(0bc31eecccdf1a840a5477d771512775) (NativeFormatImporter) -> (artifact id: '957df3d5816f00a64823db316222cbe2') in 0.0024751 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool.mat using Guid(0dd9370c56289ae4a8d0e1dddfc59477) (NativeFormatImporter) -> (artifact id: 'dff4420f29bfcf0b5e37a561766cc28b') in 0.0022072 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_route_move_floor_glow.mat using Guid(0dbdea9defe689a4ca05680804b173ca) (NativeFormatImporter) -> (artifact id: '76abd1cd4f418349642ae7527f51d632') in 0.0021803 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_back_sprite.mat using Guid(8d4799b24d7badb4d9d50d8f3546587c) (NativeFormatImporter) -> (artifact id: 'ea1cc68dacc196e87140893dcea1fad6') in 0.0026264 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.mat using Guid(0e535f48a85f05b43ad8f1b5cb2ecb4a) (NativeFormatImporter) -> (artifact id: '95a63720d5377afca26fb765e42e17fd') in 0.0025724 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_contact_shadow.asset using Guid(40c9d8116f275fd4c98bbdde1e793a32) (NativeFormatImporter) -> (artifact id: '17d86434566b3eaf148ab06195b0eaff') in 0.0023527 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite_shaded.asset using Guid(11367ee44066a2c4a933e1a65a54fa71) (NativeFormatImporter) -> (artifact id: 'b4ea198999a098bff466fc482aa2e085') in 0.0028702 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite_shaded.asset using Guid(31848d2268cea5d4a93190a2365133ca) (NativeFormatImporter) -> (artifact id: 'dfcf40487733ba25c6e6bad9c26fa7e7') in 0.0030813 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_library_window_light_cookie_soft.asset using Guid(e10a87518f970b6498f120403b5e87fa) (NativeFormatImporter) -> (artifact id: 'b6624ac7fb17375ff687066ce4c5208b') in 0.002774 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_left_sprite_shaded.asset using Guid(f13736bef9726b045ba18071b949e2d2) (NativeFormatImporter) -> (artifact id: 'f6e48f8240644c12833b02b6b33897d1') in 0.0035647 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0031371 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce_soft.asset using Guid(0442e2f80b7dbc34e99a112ae1095f20) (NativeFormatImporter) -> (artifact id: 'a65e2dbc19e50b0fc0fa4598f134ffd8') in 0.0024614 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite_shaded.asset using Guid(246d9d1ed78f72147aa1f537daf9164e) (NativeFormatImporter) -> (artifact id: '59a4f02825ed43e073fd3814395235f5') in 0.0030865 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.asset using Guid(64767a48f06c7a4439175ba9f3f76969) (NativeFormatImporter) -> (artifact id: '2c34ef17951f1b28aef444ba8ef4c761') in 0.0026817 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_back_sprite_shaded.asset using Guid(646afdaa35aa1c84bbfca6d10ad8d956) (NativeFormatImporter) -> (artifact id: '71d57c8ff1c974a8f2d165c436f15504') in 0.0031049 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool_soft.asset using Guid(97afe9889f5e56f449c8468a1d0bfdf9) (NativeFormatImporter) -> (artifact id: 'b007a80dd3775e821df40936b5d7e159') in 0.0028303 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_back_sprite_shaded.asset using Guid(383cf440301bf504a9b6c2a8a85c5c10) (NativeFormatImporter) -> (artifact id: '9a14722ac27a7c483b3d93443ba97166') in 0.0027825 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite_shaded.asset using Guid(e8b91a1d7f94f9d419ae3d2cb8aca69c) (NativeFormatImporter) -> (artifact id: '19f48d248392446507eb0aa3bd573d8d') in 0.0027581 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_left_sprite_shaded.asset using Guid(29c76ef0ee79e294abc159d852e30d6f) (NativeFormatImporter) -> (artifact id: '7a1e2d14c3f7f57972b5a415bb755af8') in 0.0027299 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite_shaded.asset using Guid(8af717035fe354841a2778b67ef47f60) (NativeFormatImporter) -> (artifact id: '636319e0046c8cb6ef7b2f1e0d8a2fbf') in 0.0029015 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite_shaded.asset using Guid(bcf85f4b6024bd345a6725cd7e3f8122) (NativeFormatImporter) -> (artifact id: '1134ffa9b5a09269a7a99f311dd08502') in 0.0028378 seconds
Start importing Assets/Settings/DefaultVolumeProfile.asset using Guid(2dc156ea366288a46859f93b6d3c2cd0) (NativeFormatImporter) -> (artifact id: 'b03cefda0696e6248b213f7dfd6e20a5') in 0.0024748 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_right_sprite_shaded.asset using Guid(7edaa2e8ce6f3f040896d2d92f57e792) (NativeFormatImporter) -> (artifact id: '4e696edc4775714bf55149c9029e9a87') in 0.0031212 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite_shaded.asset using Guid(8e8558fedae4d5441a0880d3e0ae5dde) (NativeFormatImporter) -> (artifact id: '7a36765b1626f751d5a96e3827f13ea3') in 0.0030123 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite_shaded.asset using Guid(afafc97e4daeeb2408f5830dde7c5f7c) (NativeFormatImporter) -> (artifact id: 'abb16328271b3f261d7f87df395bbbcc') in 0.0032219 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_front_sprite_shaded.asset using Guid(cfe6c4e4efe6c2d4bae69a5a1cb896ad) (NativeFormatImporter) -> (artifact id: 'a1fc470689ab7b251db5843fc102d4a4') in 0.003077 seconds
Refreshing native plugins compatible for Editor in 0.86 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=99d68aa0d0cf71848badb01260f2074d): Total: 0.206 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Scenes/Anemora_FastVS_HouseSlice.unity using Guid(5c2c1333e7d65ba4eac9369f107a6118) (DefaultImporter) -> (artifact id: 'd030106460a93946dc8d063c5521876d') in 0.0016747 seconds
Refreshing native plugins compatible for Editor in 0.72 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=c04733d24661b1a46821bab3fb5ce64f): Total: 0.049 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Asset Pipeline Refresh (id=3a520caa35640ac45bd73f2e5ade896a): Total: 0.028 seconds - Initiated by RefreshV2(NoUpdateAssetOptions)
Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:262)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaSunlitFloorIslandsVisibilityCycle110Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43579)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaBroadSunfieldCycle111Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43601)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaSunExposureBaseCycle112Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43607)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 262)

Opening scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
Unloading 0 Unused Serialized files (Serialized files now loaded: 0)
Loaded scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
	Deserialize:            263.528 ms
	Integration:            392.749 ms
	Integration of assets:  1.722 ms
	Thread Wait Time:       -1.698 ms
	Total Operation Time:   656.300 ms
Unloading 567 unused Assets / (5.6 MB). Loaded Objects now: 24207.
Memory consumption went from 261.8 MB to 256.1 MB.
Total: 18.505700 ms (FindLiveObjects: 3.063000 ms CreateObjectMapping: 0.623600 ms MarkObjects: 12.402700 ms  DeleteObjects: 2.415100 ms)

InvalidOperationException: House slice validation failed: cycle 112 plaza sun exposure base texture must contain a visible broad warm field.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaSunExposureBaseCycle112TextureMetrics () [0x00273] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:33457 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dCycle112CentralPlazaSunExposureBase () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:33648 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSunExposureBaseCycle112Batch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:43608 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 33457)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSunExposureBaseCycle112Batch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[09:31:44] Phase 'validate' FAILED with exit 1
[09:31:44] NoRollback set; preserving worktree after validate failure
```

## Cycle 112 failure (validate) -- 20260524-093424

```
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_map_move_floor_glow.mat using Guid(373d809c8f785b3439635e59312211de) (NativeFormatImporter) -> (artifact id: '70688f62928dc32fdf23a0c22c596252') in 0.0019121 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite.mat using Guid(57d1354643503484f8f9504dec7939e7) (NativeFormatImporter) -> (artifact id: '8d6144afb428b20ddb5b28b485258685') in 0.0017384 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite.mat using Guid(f79bd52245f424d42b668da63a26eed7) (NativeFormatImporter) -> (artifact id: '6c7634a35d7a5e593d016924efb11022') in 0.001711 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite.mat using Guid(f71c1a4bbbfa65e4a9dc8ceb1dbaab05) (NativeFormatImporter) -> (artifact id: '067c3ca3644ebbf71c59456785c01e71') in 0.0016916 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite.mat using Guid(28f2cae61d95125439828020176a4cfc) (NativeFormatImporter) -> (artifact id: '72016ad2a7c404cc799f9f50c6febaf5') in 0.0019098 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite.mat using Guid(a8c94ff07ef65ea4f8ad0dedea407b1e) (NativeFormatImporter) -> (artifact id: '63c8344cf78a915d285cd3eae78cb1fd') in 0.0023723 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite.mat using Guid(b8841e13281e616468837ca346676037) (NativeFormatImporter) -> (artifact id: '1adcf1ea1109f3defbf059ddda5acb22') in 0.0021522 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite.mat using Guid(3acdb3d0d51b28d40854136863dba5b3) (NativeFormatImporter) -> (artifact id: '085d72f1844e837c1fa998fa9467b7c2') in 0.0020083 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.0018614 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce.mat using Guid(0bc31eecccdf1a840a5477d771512775) (NativeFormatImporter) -> (artifact id: '957df3d5816f00a64823db316222cbe2') in 0.0017532 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool.mat using Guid(0dd9370c56289ae4a8d0e1dddfc59477) (NativeFormatImporter) -> (artifact id: 'dff4420f29bfcf0b5e37a561766cc28b') in 0.0018355 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_route_move_floor_glow.mat using Guid(0dbdea9defe689a4ca05680804b173ca) (NativeFormatImporter) -> (artifact id: '76abd1cd4f418349642ae7527f51d632') in 0.001764 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_back_sprite.mat using Guid(8d4799b24d7badb4d9d50d8f3546587c) (NativeFormatImporter) -> (artifact id: 'ea1cc68dacc196e87140893dcea1fad6') in 0.0018985 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.mat using Guid(0e535f48a85f05b43ad8f1b5cb2ecb4a) (NativeFormatImporter) -> (artifact id: '95a63720d5377afca26fb765e42e17fd') in 0.0022878 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_contact_shadow.asset using Guid(40c9d8116f275fd4c98bbdde1e793a32) (NativeFormatImporter) -> (artifact id: '17d86434566b3eaf148ab06195b0eaff') in 0.0023718 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite_shaded.asset using Guid(11367ee44066a2c4a933e1a65a54fa71) (NativeFormatImporter) -> (artifact id: 'b4ea198999a098bff466fc482aa2e085') in 0.0027773 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite_shaded.asset using Guid(31848d2268cea5d4a93190a2365133ca) (NativeFormatImporter) -> (artifact id: 'dfcf40487733ba25c6e6bad9c26fa7e7') in 0.0027393 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_library_window_light_cookie_soft.asset using Guid(e10a87518f970b6498f120403b5e87fa) (NativeFormatImporter) -> (artifact id: 'b6624ac7fb17375ff687066ce4c5208b') in 0.0024029 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_left_sprite_shaded.asset using Guid(f13736bef9726b045ba18071b949e2d2) (NativeFormatImporter) -> (artifact id: 'f6e48f8240644c12833b02b6b33897d1') in 0.003147 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0025414 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce_soft.asset using Guid(0442e2f80b7dbc34e99a112ae1095f20) (NativeFormatImporter) -> (artifact id: 'a65e2dbc19e50b0fc0fa4598f134ffd8') in 0.002065 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite_shaded.asset using Guid(246d9d1ed78f72147aa1f537daf9164e) (NativeFormatImporter) -> (artifact id: '59a4f02825ed43e073fd3814395235f5') in 0.0025418 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.asset using Guid(64767a48f06c7a4439175ba9f3f76969) (NativeFormatImporter) -> (artifact id: '2c34ef17951f1b28aef444ba8ef4c761') in 0.0020748 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_back_sprite_shaded.asset using Guid(646afdaa35aa1c84bbfca6d10ad8d956) (NativeFormatImporter) -> (artifact id: '71d57c8ff1c974a8f2d165c436f15504') in 0.002384 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool_soft.asset using Guid(97afe9889f5e56f449c8468a1d0bfdf9) (NativeFormatImporter) -> (artifact id: 'b007a80dd3775e821df40936b5d7e159') in 0.0022902 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_back_sprite_shaded.asset using Guid(383cf440301bf504a9b6c2a8a85c5c10) (NativeFormatImporter) -> (artifact id: '9a14722ac27a7c483b3d93443ba97166') in 0.0026298 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite_shaded.asset using Guid(e8b91a1d7f94f9d419ae3d2cb8aca69c) (NativeFormatImporter) -> (artifact id: '19f48d248392446507eb0aa3bd573d8d') in 0.002675 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_left_sprite_shaded.asset using Guid(29c76ef0ee79e294abc159d852e30d6f) (NativeFormatImporter) -> (artifact id: '7a1e2d14c3f7f57972b5a415bb755af8') in 0.0022564 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite_shaded.asset using Guid(8af717035fe354841a2778b67ef47f60) (NativeFormatImporter) -> (artifact id: '636319e0046c8cb6ef7b2f1e0d8a2fbf') in 0.0023175 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite_shaded.asset using Guid(bcf85f4b6024bd345a6725cd7e3f8122) (NativeFormatImporter) -> (artifact id: '1134ffa9b5a09269a7a99f311dd08502') in 0.0028554 seconds
Start importing Assets/Settings/DefaultVolumeProfile.asset using Guid(2dc156ea366288a46859f93b6d3c2cd0) (NativeFormatImporter) -> (artifact id: 'b03cefda0696e6248b213f7dfd6e20a5') in 0.0023748 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_right_sprite_shaded.asset using Guid(7edaa2e8ce6f3f040896d2d92f57e792) (NativeFormatImporter) -> (artifact id: '4e696edc4775714bf55149c9029e9a87') in 0.0028458 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite_shaded.asset using Guid(8e8558fedae4d5441a0880d3e0ae5dde) (NativeFormatImporter) -> (artifact id: '7a36765b1626f751d5a96e3827f13ea3') in 0.0025137 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite_shaded.asset using Guid(afafc97e4daeeb2408f5830dde7c5f7c) (NativeFormatImporter) -> (artifact id: 'abb16328271b3f261d7f87df395bbbcc') in 0.0023403 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_front_sprite_shaded.asset using Guid(cfe6c4e4efe6c2d4bae69a5a1cb896ad) (NativeFormatImporter) -> (artifact id: 'a1fc470689ab7b251db5843fc102d4a4') in 0.0022612 seconds
Refreshing native plugins compatible for Editor in 0.71 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=d4e1b1bb3f59da446bc752c6aadda9b1): Total: 0.176 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Scenes/Anemora_FastVS_HouseSlice.unity using Guid(5c2c1333e7d65ba4eac9369f107a6118) (DefaultImporter) -> (artifact id: '9c7aa3445b3a2c77d5ee44e5b6d7b174') in 0.0014879 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=1cb9d8bca53587942909ab26b8df1d0d): Total: 0.045 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Asset Pipeline Refresh (id=c882e50c3ddf9a04290e9309dfee2316): Total: 0.028 seconds - Initiated by RefreshV2(NoUpdateAssetOptions)
Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:262)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaSunlitFloorIslandsVisibilityCycle110Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43579)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaBroadSunfieldCycle111Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43601)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaSunExposureBaseCycle112Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43607)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 262)

Opening scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
Unloading 0 Unused Serialized files (Serialized files now loaded: 0)
Loaded scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
	Deserialize:            259.466 ms
	Integration:            293.318 ms
	Integration of assets:  2.075 ms
	Thread Wait Time:       -1.985 ms
	Total Operation Time:   552.873 ms
Unloading 567 unused Assets / (5.6 MB). Loaded Objects now: 24207.
Memory consumption went from 261.6 MB to 256.1 MB.
Total: 12.945400 ms (FindLiveObjects: 2.081600 ms CreateObjectMapping: 0.434600 ms MarkObjects: 8.523600 ms  DeleteObjects: 1.904600 ms)

InvalidOperationException: House slice validation failed: cycle 112 plaza sun exposure base texture center must stay warm without turning into a slab.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaSunExposureBaseCycle112TextureMetrics () [0x00172] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:33431 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dCycle112CentralPlazaSunExposureBase () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:33648 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSunExposureBaseCycle112Batch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:43608 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 33431)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSunExposureBaseCycle112Batch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[09:44:17] Phase 'validate' FAILED with exit 1
[09:44:17] NoRollback set; preserving worktree after validate failure
```
