# feat(hd2d): add plaza sunbeam shafts

## Scope

Cycle 113 follows parent review of Cycle 112. Cycle112 added an under-shadow warm exposure base so the plaza floor reads more sunlit, but the scene still lacks the reference-like feeling of sunlight passing through air. This cycle adds current-world-only atmospheric sunbeam shafts in the central plaza.

Authored file:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Expected generated side-effect files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_sunbeam_shafts_cycle113.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_plaza_sunbeam_shafts_cycle113.asset`

Out of scope:

- Main branch, map-layout changes, story/UI, doors, time-window behavior, character assets, house facade fixes, ProjectSettings, and `C:\Users\maro6\OneDrive\work\projects\anemora_reference\map_chapter_1\map_chapter_1`.

## Goal Prompt

Add soft current-plaza sunbeam shafts so the light feels like it passes through air, not only as floor overlays. The beams must be semi-transparent, diagonal, current-world-only, and not obstruct gameplay readability.

Reference directory:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference`

Map reference directory noted but not edited this cycle:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\map_chapter_1\map_chapter_1`

## Worker Cycle

- Cycle-worker: `019e5779-977f-7dc2-b89f-164b3da4aa5e`
- Worker role: `cycle-worker` / `gpt-5.4-mini`

Scoped prompt trace:

`SCOPED_PROMPT_ISSUED cycle=113 authored_file=C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs validate=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSunbeamShaftsCycle113Batch capture=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaSunbeamShaftsCycle113ScreenshotsBatch`

## Implementation Plan

- Add a generated transparent sunbeam shaft material/texture pair with soft diagonal beam bands and edge feathering.
- Add exactly three current-world-only, non-colliding vertical quads in the central plaza, aligned with the established sun direction.
- Validate texture metrics, object count, current-only placement, no colliders, material/texture provenance, render queue, and prior Cycle103-112 validations.
- Capture current plaza overview/close, past plaza guard, and current library guard screenshots for parent visual review.

## Expected Evidence

Parent-review screenshots are expected under:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle113_plaza_sunbeam_shafts_parent_review_20260524_01`

Expected files:

- `parent_review_01_current_central_plaza_sunbeam_shafts_overview.png`
- `parent_review_02_current_central_plaza_sunbeam_shafts_close.png`
- `parent_review_03_past_central_plaza_sunbeam_shafts_guard.png`
- `parent_review_04_current_library_sunbeam_shafts_guard.png`

## Validation

Planned cycle-runner command:

```powershell
$commitPaths = @(
  'Assets/Editor/AnemoraFastVsHouseSliceSetup.cs',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sunbeam_shafts_cycle113.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sunbeam_shafts_cycle113.mat.meta',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sunbeam_shafts_cycle113.asset',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sunbeam_shafts_cycle113.asset.meta',
  'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_sunbeam_shafts_cycle113.md',
  'docs/devlog/INDEX.md',
  'docs/devlog/screenshots/fast_vs_hd2d_cycle113_plaza_sunbeam_shafts_parent_review_20260524_01'
)
& .\tools\cycle-runner.ps1 -CycleNumber 113 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSunbeamShaftsCycle113Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaSunbeamShaftsCycle113ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_sunbeam_shafts_cycle113.md' `
  -Audience parent_review `
  -CommitPath $commitPaths `
  -NoRollback
```

## Visual Gate

Passing criteria:

- Current plaza overview shows visible but restrained atmospheric sunbeam shafts.
- Beams add depth and air-light without hiding Niro, the route, or the library facade.
- Past plaza and current library guard captures show no obvious regression.

## Cycle 113 failure (validate) -- 20260524-100509

```
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite.mat using Guid(57d1354643503484f8f9504dec7939e7) (NativeFormatImporter) -> (artifact id: '8d6144afb428b20ddb5b28b485258685') in 0.0020475 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite.mat using Guid(f79bd52245f424d42b668da63a26eed7) (NativeFormatImporter) -> (artifact id: '6c7634a35d7a5e593d016924efb11022') in 0.002253 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite.mat using Guid(f71c1a4bbbfa65e4a9dc8ceb1dbaab05) (NativeFormatImporter) -> (artifact id: '067c3ca3644ebbf71c59456785c01e71') in 0.0022282 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite.mat using Guid(28f2cae61d95125439828020176a4cfc) (NativeFormatImporter) -> (artifact id: '72016ad2a7c404cc799f9f50c6febaf5') in 0.0021089 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite.mat using Guid(a8c94ff07ef65ea4f8ad0dedea407b1e) (NativeFormatImporter) -> (artifact id: '63c8344cf78a915d285cd3eae78cb1fd') in 0.002142 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite.mat using Guid(b8841e13281e616468837ca346676037) (NativeFormatImporter) -> (artifact id: '1adcf1ea1109f3defbf059ddda5acb22') in 0.0019782 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite.mat using Guid(3acdb3d0d51b28d40854136863dba5b3) (NativeFormatImporter) -> (artifact id: '085d72f1844e837c1fa998fa9467b7c2') in 0.0019307 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.0018547 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce.mat using Guid(0bc31eecccdf1a840a5477d771512775) (NativeFormatImporter) -> (artifact id: '957df3d5816f00a64823db316222cbe2') in 0.0017786 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool.mat using Guid(0dd9370c56289ae4a8d0e1dddfc59477) (NativeFormatImporter) -> (artifact id: 'dff4420f29bfcf0b5e37a561766cc28b') in 0.0019424 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_route_move_floor_glow.mat using Guid(0dbdea9defe689a4ca05680804b173ca) (NativeFormatImporter) -> (artifact id: '76abd1cd4f418349642ae7527f51d632') in 0.0020419 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_back_sprite.mat using Guid(8d4799b24d7badb4d9d50d8f3546587c) (NativeFormatImporter) -> (artifact id: 'ea1cc68dacc196e87140893dcea1fad6') in 0.0020814 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.mat using Guid(0e535f48a85f05b43ad8f1b5cb2ecb4a) (NativeFormatImporter) -> (artifact id: '95a63720d5377afca26fb765e42e17fd') in 0.0018764 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_contact_shadow.asset using Guid(40c9d8116f275fd4c98bbdde1e793a32) (NativeFormatImporter) -> (artifact id: '17d86434566b3eaf148ab06195b0eaff') in 0.0019436 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite_shaded.asset using Guid(11367ee44066a2c4a933e1a65a54fa71) (NativeFormatImporter) -> (artifact id: 'b4ea198999a098bff466fc482aa2e085') in 0.0026006 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite_shaded.asset using Guid(31848d2268cea5d4a93190a2365133ca) (NativeFormatImporter) -> (artifact id: 'dfcf40487733ba25c6e6bad9c26fa7e7') in 0.0022978 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_library_window_light_cookie_soft.asset using Guid(e10a87518f970b6498f120403b5e87fa) (NativeFormatImporter) -> (artifact id: 'b6624ac7fb17375ff687066ce4c5208b') in 0.0021948 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_left_sprite_shaded.asset using Guid(f13736bef9726b045ba18071b949e2d2) (NativeFormatImporter) -> (artifact id: 'f6e48f8240644c12833b02b6b33897d1') in 0.0022634 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0020766 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce_soft.asset using Guid(0442e2f80b7dbc34e99a112ae1095f20) (NativeFormatImporter) -> (artifact id: 'a65e2dbc19e50b0fc0fa4598f134ffd8') in 0.0018891 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite_shaded.asset using Guid(246d9d1ed78f72147aa1f537daf9164e) (NativeFormatImporter) -> (artifact id: '59a4f02825ed43e073fd3814395235f5') in 0.0034247 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.asset using Guid(64767a48f06c7a4439175ba9f3f76969) (NativeFormatImporter) -> (artifact id: '2c34ef17951f1b28aef444ba8ef4c761') in 0.0020991 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_back_sprite_shaded.asset using Guid(646afdaa35aa1c84bbfca6d10ad8d956) (NativeFormatImporter) -> (artifact id: '71d57c8ff1c974a8f2d165c436f15504') in 0.0026819 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool_soft.asset using Guid(97afe9889f5e56f449c8468a1d0bfdf9) (NativeFormatImporter) -> (artifact id: 'b007a80dd3775e821df40936b5d7e159') in 0.002485 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_back_sprite_shaded.asset using Guid(383cf440301bf504a9b6c2a8a85c5c10) (NativeFormatImporter) -> (artifact id: '9a14722ac27a7c483b3d93443ba97166') in 0.0024421 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite_shaded.asset using Guid(e8b91a1d7f94f9d419ae3d2cb8aca69c) (NativeFormatImporter) -> (artifact id: '19f48d248392446507eb0aa3bd573d8d') in 0.0025271 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_left_sprite_shaded.asset using Guid(29c76ef0ee79e294abc159d852e30d6f) (NativeFormatImporter) -> (artifact id: '7a1e2d14c3f7f57972b5a415bb755af8') in 0.0023366 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite_shaded.asset using Guid(8af717035fe354841a2778b67ef47f60) (NativeFormatImporter) -> (artifact id: '636319e0046c8cb6ef7b2f1e0d8a2fbf') in 0.0021968 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite_shaded.asset using Guid(bcf85f4b6024bd345a6725cd7e3f8122) (NativeFormatImporter) -> (artifact id: '1134ffa9b5a09269a7a99f311dd08502') in 0.0024644 seconds
Start importing Assets/Settings/DefaultVolumeProfile.asset using Guid(2dc156ea366288a46859f93b6d3c2cd0) (NativeFormatImporter) -> (artifact id: 'b03cefda0696e6248b213f7dfd6e20a5') in 0.0020493 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_right_sprite_shaded.asset using Guid(7edaa2e8ce6f3f040896d2d92f57e792) (NativeFormatImporter) -> (artifact id: '4e696edc4775714bf55149c9029e9a87') in 0.0024099 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite_shaded.asset using Guid(8e8558fedae4d5441a0880d3e0ae5dde) (NativeFormatImporter) -> (artifact id: '7a36765b1626f751d5a96e3827f13ea3') in 0.0024703 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite_shaded.asset using Guid(afafc97e4daeeb2408f5830dde7c5f7c) (NativeFormatImporter) -> (artifact id: 'abb16328271b3f261d7f87df395bbbcc') in 0.0022204 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_front_sprite_shaded.asset using Guid(cfe6c4e4efe6c2d4bae69a5a1cb896ad) (NativeFormatImporter) -> (artifact id: 'a1fc470689ab7b251db5843fc102d4a4') in 0.0022737 seconds
Refreshing native plugins compatible for Editor in 0.72 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=fe07fb8ff690efe4ca8f681a19720e5f): Total: 0.169 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Scenes/Anemora_FastVS_HouseSlice.unity using Guid(5c2c1333e7d65ba4eac9369f107a6118) (DefaultImporter) -> (artifact id: '9282a2cc8e585ae9cb40ed36d3514027') in 0.0014315 seconds
Refreshing native plugins compatible for Editor in 0.56 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=b1f3412086c95d044a7d0ae6ab369495): Total: 0.050 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Asset Pipeline Refresh (id=5ed6896529a468347aaa7d0641dd3df9): Total: 0.028 seconds - Initiated by RefreshV2(NoUpdateAssetOptions)
Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:265)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaSunlitFloorIslandsVisibilityCycle110Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43880)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaBroadSunfieldCycle111Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43902)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaSunExposureBaseCycle112Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43908)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaSunbeamShaftsCycle113Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43914)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 265)

Opening scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
Unloading 0 Unused Serialized files (Serialized files now loaded: 0)
Loaded scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
	Deserialize:            257.043 ms
	Integration:            285.014 ms
	Integration of assets:  1.929 ms
	Thread Wait Time:       -1.897 ms
	Total Operation Time:   542.089 ms
Unloading 569 unused Assets / (5.4 MB). Loaded Objects now: 24224.
Memory consumption went from 261.8 MB to 256.4 MB.
Total: 11.829500 ms (FindLiveObjects: 1.902300 ms CreateObjectMapping: 0.512300 ms MarkObjects: 7.701000 ms  DeleteObjects: 1.712500 ms)

InvalidOperationException: House slice validation failed: cycle 113 plaza sunbeam shaft texture must preserve a visible beam peak without turning into a slab.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaSunbeamShaftsCycle113TextureMetrics () [0x00249] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:33986 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaSunbeamShaftsCycle113 () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:33780 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSunbeamShaftsCycle113Batch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:43915 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 33986)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSunbeamShaftsCycle113Batch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[10:06:45] Phase 'validate' FAILED with exit 1
[10:06:45] NoRollback set; preserving worktree after validate failure
```

## Cycle 113 failure (validate) -- 20260524-101352

```
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite.mat using Guid(57d1354643503484f8f9504dec7939e7) (NativeFormatImporter) -> (artifact id: '8d6144afb428b20ddb5b28b485258685') in 0.002055 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite.mat using Guid(f79bd52245f424d42b668da63a26eed7) (NativeFormatImporter) -> (artifact id: '6c7634a35d7a5e593d016924efb11022') in 0.001925 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite.mat using Guid(f71c1a4bbbfa65e4a9dc8ceb1dbaab05) (NativeFormatImporter) -> (artifact id: '067c3ca3644ebbf71c59456785c01e71') in 0.0017475 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite.mat using Guid(28f2cae61d95125439828020176a4cfc) (NativeFormatImporter) -> (artifact id: '72016ad2a7c404cc799f9f50c6febaf5') in 0.0019282 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite.mat using Guid(a8c94ff07ef65ea4f8ad0dedea407b1e) (NativeFormatImporter) -> (artifact id: '63c8344cf78a915d285cd3eae78cb1fd') in 0.002143 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite.mat using Guid(b8841e13281e616468837ca346676037) (NativeFormatImporter) -> (artifact id: '1adcf1ea1109f3defbf059ddda5acb22') in 0.0020163 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite.mat using Guid(3acdb3d0d51b28d40854136863dba5b3) (NativeFormatImporter) -> (artifact id: '085d72f1844e837c1fa998fa9467b7c2') in 0.0018321 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.0022818 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce.mat using Guid(0bc31eecccdf1a840a5477d771512775) (NativeFormatImporter) -> (artifact id: '957df3d5816f00a64823db316222cbe2') in 0.0019656 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool.mat using Guid(0dd9370c56289ae4a8d0e1dddfc59477) (NativeFormatImporter) -> (artifact id: 'dff4420f29bfcf0b5e37a561766cc28b') in 0.002017 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_route_move_floor_glow.mat using Guid(0dbdea9defe689a4ca05680804b173ca) (NativeFormatImporter) -> (artifact id: '76abd1cd4f418349642ae7527f51d632') in 0.0021884 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_back_sprite.mat using Guid(8d4799b24d7badb4d9d50d8f3546587c) (NativeFormatImporter) -> (artifact id: 'ea1cc68dacc196e87140893dcea1fad6') in 0.002053 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.mat using Guid(0e535f48a85f05b43ad8f1b5cb2ecb4a) (NativeFormatImporter) -> (artifact id: '95a63720d5377afca26fb765e42e17fd') in 0.0020354 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_contact_shadow.asset using Guid(40c9d8116f275fd4c98bbdde1e793a32) (NativeFormatImporter) -> (artifact id: '17d86434566b3eaf148ab06195b0eaff') in 0.0021369 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite_shaded.asset using Guid(11367ee44066a2c4a933e1a65a54fa71) (NativeFormatImporter) -> (artifact id: 'b4ea198999a098bff466fc482aa2e085') in 0.0030514 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite_shaded.asset using Guid(31848d2268cea5d4a93190a2365133ca) (NativeFormatImporter) -> (artifact id: 'dfcf40487733ba25c6e6bad9c26fa7e7') in 0.0027563 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_library_window_light_cookie_soft.asset using Guid(e10a87518f970b6498f120403b5e87fa) (NativeFormatImporter) -> (artifact id: 'b6624ac7fb17375ff687066ce4c5208b') in 0.0028418 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_left_sprite_shaded.asset using Guid(f13736bef9726b045ba18071b949e2d2) (NativeFormatImporter) -> (artifact id: 'f6e48f8240644c12833b02b6b33897d1') in 0.0028241 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0022674 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce_soft.asset using Guid(0442e2f80b7dbc34e99a112ae1095f20) (NativeFormatImporter) -> (artifact id: 'a65e2dbc19e50b0fc0fa4598f134ffd8') in 0.0023968 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite_shaded.asset using Guid(246d9d1ed78f72147aa1f537daf9164e) (NativeFormatImporter) -> (artifact id: '59a4f02825ed43e073fd3814395235f5') in 0.0025417 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.asset using Guid(64767a48f06c7a4439175ba9f3f76969) (NativeFormatImporter) -> (artifact id: '2c34ef17951f1b28aef444ba8ef4c761') in 0.00227 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_back_sprite_shaded.asset using Guid(646afdaa35aa1c84bbfca6d10ad8d956) (NativeFormatImporter) -> (artifact id: '71d57c8ff1c974a8f2d165c436f15504') in 0.0025579 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool_soft.asset using Guid(97afe9889f5e56f449c8468a1d0bfdf9) (NativeFormatImporter) -> (artifact id: 'b007a80dd3775e821df40936b5d7e159') in 0.0028359 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_back_sprite_shaded.asset using Guid(383cf440301bf504a9b6c2a8a85c5c10) (NativeFormatImporter) -> (artifact id: '9a14722ac27a7c483b3d93443ba97166') in 0.0026312 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite_shaded.asset using Guid(e8b91a1d7f94f9d419ae3d2cb8aca69c) (NativeFormatImporter) -> (artifact id: '19f48d248392446507eb0aa3bd573d8d') in 0.002611 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_left_sprite_shaded.asset using Guid(29c76ef0ee79e294abc159d852e30d6f) (NativeFormatImporter) -> (artifact id: '7a1e2d14c3f7f57972b5a415bb755af8') in 0.0024661 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite_shaded.asset using Guid(8af717035fe354841a2778b67ef47f60) (NativeFormatImporter) -> (artifact id: '636319e0046c8cb6ef7b2f1e0d8a2fbf') in 0.002487 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite_shaded.asset using Guid(bcf85f4b6024bd345a6725cd7e3f8122) (NativeFormatImporter) -> (artifact id: '1134ffa9b5a09269a7a99f311dd08502') in 0.0026612 seconds
Start importing Assets/Settings/DefaultVolumeProfile.asset using Guid(2dc156ea366288a46859f93b6d3c2cd0) (NativeFormatImporter) -> (artifact id: 'b03cefda0696e6248b213f7dfd6e20a5') in 0.0022613 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_right_sprite_shaded.asset using Guid(7edaa2e8ce6f3f040896d2d92f57e792) (NativeFormatImporter) -> (artifact id: '4e696edc4775714bf55149c9029e9a87') in 0.0025157 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite_shaded.asset using Guid(8e8558fedae4d5441a0880d3e0ae5dde) (NativeFormatImporter) -> (artifact id: '7a36765b1626f751d5a96e3827f13ea3') in 0.0026763 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite_shaded.asset using Guid(afafc97e4daeeb2408f5830dde7c5f7c) (NativeFormatImporter) -> (artifact id: 'abb16328271b3f261d7f87df395bbbcc') in 0.0028128 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_front_sprite_shaded.asset using Guid(cfe6c4e4efe6c2d4bae69a5a1cb896ad) (NativeFormatImporter) -> (artifact id: 'a1fc470689ab7b251db5843fc102d4a4') in 0.0026029 seconds
Refreshing native plugins compatible for Editor in 0.74 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=981b64b01c83265419c9b90910619050): Total: 0.181 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Scenes/Anemora_FastVS_HouseSlice.unity using Guid(5c2c1333e7d65ba4eac9369f107a6118) (DefaultImporter) -> (artifact id: '23137fccd7d9e78211fbe43df26e5229') in 0.0017923 seconds
Refreshing native plugins compatible for Editor in 0.63 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=511466acfd265b04bbfcb92efae173b9): Total: 0.050 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Asset Pipeline Refresh (id=9e159b3c615e85c49997c4b11f76faf6): Total: 0.028 seconds - Initiated by RefreshV2(NoUpdateAssetOptions)
Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:265)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaSunlitFloorIslandsVisibilityCycle110Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43880)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaBroadSunfieldCycle111Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43902)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaSunExposureBaseCycle112Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43908)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaSunbeamShaftsCycle113Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43914)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 265)

Opening scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
Unloading 0 Unused Serialized files (Serialized files now loaded: 0)
Loaded scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
	Deserialize:            268.391 ms
	Integration:            297.827 ms
	Integration of assets:  2.512 ms
	Thread Wait Time:       -2.477 ms
	Total Operation Time:   566.252 ms
Unloading 569 unused Assets / (10.5 MB). Loaded Objects now: 24224.
Memory consumption went from 259.6 MB to 249.1 MB.
Total: 14.644400 ms (FindLiveObjects: 3.380200 ms CreateObjectMapping: 0.635600 ms MarkObjects: 8.276600 ms  DeleteObjects: 2.350800 ms)

InvalidOperationException: House slice validation failed: cycle 113 plaza sunbeam shaft texture must preserve a visible beam peak without turning into a slab (beamMax=0.184, beamAverage=0.092).
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaSunbeamShaftsCycle113TextureMetrics () [0x00243] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:33986 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaSunbeamShaftsCycle113 () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:33780 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSunbeamShaftsCycle113Batch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:43915 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 33986)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSunbeamShaftsCycle113Batch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[10:15:09] Phase 'validate' FAILED with exit 1
[10:15:09] NoRollback set; preserving worktree after validate failure
```

## Cycle 113 failure (validate) -- 20260524-101634

```
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite.mat using Guid(57d1354643503484f8f9504dec7939e7) (NativeFormatImporter) -> (artifact id: '8d6144afb428b20ddb5b28b485258685') in 0.0019675 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite.mat using Guid(f79bd52245f424d42b668da63a26eed7) (NativeFormatImporter) -> (artifact id: '6c7634a35d7a5e593d016924efb11022') in 0.0021009 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite.mat using Guid(f71c1a4bbbfa65e4a9dc8ceb1dbaab05) (NativeFormatImporter) -> (artifact id: '067c3ca3644ebbf71c59456785c01e71') in 0.0021514 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite.mat using Guid(28f2cae61d95125439828020176a4cfc) (NativeFormatImporter) -> (artifact id: '72016ad2a7c404cc799f9f50c6febaf5') in 0.0020253 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite.mat using Guid(a8c94ff07ef65ea4f8ad0dedea407b1e) (NativeFormatImporter) -> (artifact id: '63c8344cf78a915d285cd3eae78cb1fd') in 0.0020407 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite.mat using Guid(b8841e13281e616468837ca346676037) (NativeFormatImporter) -> (artifact id: '1adcf1ea1109f3defbf059ddda5acb22') in 0.0021785 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite.mat using Guid(3acdb3d0d51b28d40854136863dba5b3) (NativeFormatImporter) -> (artifact id: '085d72f1844e837c1fa998fa9467b7c2') in 0.002075 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.0019623 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce.mat using Guid(0bc31eecccdf1a840a5477d771512775) (NativeFormatImporter) -> (artifact id: '957df3d5816f00a64823db316222cbe2') in 0.003046 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool.mat using Guid(0dd9370c56289ae4a8d0e1dddfc59477) (NativeFormatImporter) -> (artifact id: 'dff4420f29bfcf0b5e37a561766cc28b') in 0.0024752 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_route_move_floor_glow.mat using Guid(0dbdea9defe689a4ca05680804b173ca) (NativeFormatImporter) -> (artifact id: '76abd1cd4f418349642ae7527f51d632') in 0.0022068 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_back_sprite.mat using Guid(8d4799b24d7badb4d9d50d8f3546587c) (NativeFormatImporter) -> (artifact id: 'ea1cc68dacc196e87140893dcea1fad6') in 0.0022819 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.mat using Guid(0e535f48a85f05b43ad8f1b5cb2ecb4a) (NativeFormatImporter) -> (artifact id: '95a63720d5377afca26fb765e42e17fd') in 0.0021812 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_contact_shadow.asset using Guid(40c9d8116f275fd4c98bbdde1e793a32) (NativeFormatImporter) -> (artifact id: '17d86434566b3eaf148ab06195b0eaff') in 0.0021104 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite_shaded.asset using Guid(11367ee44066a2c4a933e1a65a54fa71) (NativeFormatImporter) -> (artifact id: 'b4ea198999a098bff466fc482aa2e085') in 0.0029001 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite_shaded.asset using Guid(31848d2268cea5d4a93190a2365133ca) (NativeFormatImporter) -> (artifact id: 'dfcf40487733ba25c6e6bad9c26fa7e7') in 0.0029026 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_library_window_light_cookie_soft.asset using Guid(e10a87518f970b6498f120403b5e87fa) (NativeFormatImporter) -> (artifact id: 'b6624ac7fb17375ff687066ce4c5208b') in 0.002811 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_left_sprite_shaded.asset using Guid(f13736bef9726b045ba18071b949e2d2) (NativeFormatImporter) -> (artifact id: 'f6e48f8240644c12833b02b6b33897d1') in 0.0029394 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.002326 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce_soft.asset using Guid(0442e2f80b7dbc34e99a112ae1095f20) (NativeFormatImporter) -> (artifact id: 'a65e2dbc19e50b0fc0fa4598f134ffd8') in 0.0033507 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite_shaded.asset using Guid(246d9d1ed78f72147aa1f537daf9164e) (NativeFormatImporter) -> (artifact id: '59a4f02825ed43e073fd3814395235f5') in 0.0041573 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.asset using Guid(64767a48f06c7a4439175ba9f3f76969) (NativeFormatImporter) -> (artifact id: '2c34ef17951f1b28aef444ba8ef4c761') in 0.003071 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_back_sprite_shaded.asset using Guid(646afdaa35aa1c84bbfca6d10ad8d956) (NativeFormatImporter) -> (artifact id: '71d57c8ff1c974a8f2d165c436f15504') in 0.0028599 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool_soft.asset using Guid(97afe9889f5e56f449c8468a1d0bfdf9) (NativeFormatImporter) -> (artifact id: 'b007a80dd3775e821df40936b5d7e159') in 0.0026765 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_back_sprite_shaded.asset using Guid(383cf440301bf504a9b6c2a8a85c5c10) (NativeFormatImporter) -> (artifact id: '9a14722ac27a7c483b3d93443ba97166') in 0.0027315 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite_shaded.asset using Guid(e8b91a1d7f94f9d419ae3d2cb8aca69c) (NativeFormatImporter) -> (artifact id: '19f48d248392446507eb0aa3bd573d8d') in 0.0031323 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_left_sprite_shaded.asset using Guid(29c76ef0ee79e294abc159d852e30d6f) (NativeFormatImporter) -> (artifact id: '7a1e2d14c3f7f57972b5a415bb755af8') in 0.0028969 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite_shaded.asset using Guid(8af717035fe354841a2778b67ef47f60) (NativeFormatImporter) -> (artifact id: '636319e0046c8cb6ef7b2f1e0d8a2fbf') in 0.0027835 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite_shaded.asset using Guid(bcf85f4b6024bd345a6725cd7e3f8122) (NativeFormatImporter) -> (artifact id: '1134ffa9b5a09269a7a99f311dd08502') in 0.0028548 seconds
Start importing Assets/Settings/DefaultVolumeProfile.asset using Guid(2dc156ea366288a46859f93b6d3c2cd0) (NativeFormatImporter) -> (artifact id: 'b03cefda0696e6248b213f7dfd6e20a5') in 0.0020963 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_right_sprite_shaded.asset using Guid(7edaa2e8ce6f3f040896d2d92f57e792) (NativeFormatImporter) -> (artifact id: '4e696edc4775714bf55149c9029e9a87') in 0.0023294 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite_shaded.asset using Guid(8e8558fedae4d5441a0880d3e0ae5dde) (NativeFormatImporter) -> (artifact id: '7a36765b1626f751d5a96e3827f13ea3') in 0.0030402 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite_shaded.asset using Guid(afafc97e4daeeb2408f5830dde7c5f7c) (NativeFormatImporter) -> (artifact id: 'abb16328271b3f261d7f87df395bbbcc') in 0.0025559 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_front_sprite_shaded.asset using Guid(cfe6c4e4efe6c2d4bae69a5a1cb896ad) (NativeFormatImporter) -> (artifact id: 'a1fc470689ab7b251db5843fc102d4a4') in 0.0026397 seconds
Refreshing native plugins compatible for Editor in 0.75 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=adf411542953b7640ab224d3e9be310d): Total: 0.192 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Scenes/Anemora_FastVS_HouseSlice.unity using Guid(5c2c1333e7d65ba4eac9369f107a6118) (DefaultImporter) -> (artifact id: '2eccd577af170763faf23d3167525a5e') in 0.0013337 seconds
Refreshing native plugins compatible for Editor in 0.70 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=e5345f641af69d145b7a46a90881f4b9): Total: 0.051 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Asset Pipeline Refresh (id=63ba7614813d2b442b49e70546d1dd95): Total: 0.028 seconds - Initiated by RefreshV2(NoUpdateAssetOptions)
Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:265)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaSunlitFloorIslandsVisibilityCycle110Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43880)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaBroadSunfieldCycle111Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43902)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaSunExposureBaseCycle112Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43908)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaSunbeamShaftsCycle113Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43914)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 265)

Opening scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
Unloading 0 Unused Serialized files (Serialized files now loaded: 0)
Loaded scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
	Deserialize:            265.199 ms
	Integration:            299.356 ms
	Integration of assets:  2.180 ms
	Thread Wait Time:       -2.101 ms
	Total Operation Time:   564.633 ms
Unloading 569 unused Assets / (5.7 MB). Loaded Objects now: 24224.
Memory consumption went from 261.7 MB to 256.0 MB.
Total: 12.663300 ms (FindLiveObjects: 2.051500 ms CreateObjectMapping: 0.475800 ms MarkObjects: 8.288400 ms  DeleteObjects: 1.846100 ms)

InvalidOperationException: House slice validation failed: cycle 113 plaza sunbeam shaft texture must preserve a visible beam peak without turning into a slab (beamMax=0.227, beamAverage=0.104).
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaSunbeamShaftsCycle113TextureMetrics () [0x00243] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:33986 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaSunbeamShaftsCycle113 () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:33780 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSunbeamShaftsCycle113Batch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:43915 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 33986)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSunbeamShaftsCycle113Batch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[10:17:47] Phase 'validate' FAILED with exit 1
[10:17:47] NoRollback set; preserving worktree after validate failure
```

## Cycle 113 failure (validate) -- 20260524-102043

```
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite.mat using Guid(57d1354643503484f8f9504dec7939e7) (NativeFormatImporter) -> (artifact id: '8d6144afb428b20ddb5b28b485258685') in 0.0026424 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite.mat using Guid(f79bd52245f424d42b668da63a26eed7) (NativeFormatImporter) -> (artifact id: '6c7634a35d7a5e593d016924efb11022') in 0.002673 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite.mat using Guid(f71c1a4bbbfa65e4a9dc8ceb1dbaab05) (NativeFormatImporter) -> (artifact id: '067c3ca3644ebbf71c59456785c01e71') in 0.0025459 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite.mat using Guid(28f2cae61d95125439828020176a4cfc) (NativeFormatImporter) -> (artifact id: '72016ad2a7c404cc799f9f50c6febaf5') in 0.0025396 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite.mat using Guid(a8c94ff07ef65ea4f8ad0dedea407b1e) (NativeFormatImporter) -> (artifact id: '63c8344cf78a915d285cd3eae78cb1fd') in 0.0029109 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite.mat using Guid(b8841e13281e616468837ca346676037) (NativeFormatImporter) -> (artifact id: '1adcf1ea1109f3defbf059ddda5acb22') in 0.0033335 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite.mat using Guid(3acdb3d0d51b28d40854136863dba5b3) (NativeFormatImporter) -> (artifact id: '085d72f1844e837c1fa998fa9467b7c2') in 0.0021325 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.0024421 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce.mat using Guid(0bc31eecccdf1a840a5477d771512775) (NativeFormatImporter) -> (artifact id: '957df3d5816f00a64823db316222cbe2') in 0.0023543 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool.mat using Guid(0dd9370c56289ae4a8d0e1dddfc59477) (NativeFormatImporter) -> (artifact id: 'dff4420f29bfcf0b5e37a561766cc28b') in 0.0022524 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_route_move_floor_glow.mat using Guid(0dbdea9defe689a4ca05680804b173ca) (NativeFormatImporter) -> (artifact id: '76abd1cd4f418349642ae7527f51d632') in 0.0025292 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_back_sprite.mat using Guid(8d4799b24d7badb4d9d50d8f3546587c) (NativeFormatImporter) -> (artifact id: 'ea1cc68dacc196e87140893dcea1fad6') in 0.0025541 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.mat using Guid(0e535f48a85f05b43ad8f1b5cb2ecb4a) (NativeFormatImporter) -> (artifact id: '95a63720d5377afca26fb765e42e17fd') in 0.0025228 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_contact_shadow.asset using Guid(40c9d8116f275fd4c98bbdde1e793a32) (NativeFormatImporter) -> (artifact id: '17d86434566b3eaf148ab06195b0eaff') in 0.0030648 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite_shaded.asset using Guid(11367ee44066a2c4a933e1a65a54fa71) (NativeFormatImporter) -> (artifact id: 'b4ea198999a098bff466fc482aa2e085') in 0.0031697 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite_shaded.asset using Guid(31848d2268cea5d4a93190a2365133ca) (NativeFormatImporter) -> (artifact id: 'dfcf40487733ba25c6e6bad9c26fa7e7') in 0.0036732 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_library_window_light_cookie_soft.asset using Guid(e10a87518f970b6498f120403b5e87fa) (NativeFormatImporter) -> (artifact id: 'b6624ac7fb17375ff687066ce4c5208b') in 0.0029235 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_left_sprite_shaded.asset using Guid(f13736bef9726b045ba18071b949e2d2) (NativeFormatImporter) -> (artifact id: 'f6e48f8240644c12833b02b6b33897d1') in 0.0028678 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0022424 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce_soft.asset using Guid(0442e2f80b7dbc34e99a112ae1095f20) (NativeFormatImporter) -> (artifact id: 'a65e2dbc19e50b0fc0fa4598f134ffd8') in 0.0024569 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite_shaded.asset using Guid(246d9d1ed78f72147aa1f537daf9164e) (NativeFormatImporter) -> (artifact id: '59a4f02825ed43e073fd3814395235f5') in 0.0029203 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.asset using Guid(64767a48f06c7a4439175ba9f3f76969) (NativeFormatImporter) -> (artifact id: '2c34ef17951f1b28aef444ba8ef4c761') in 0.0024345 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_back_sprite_shaded.asset using Guid(646afdaa35aa1c84bbfca6d10ad8d956) (NativeFormatImporter) -> (artifact id: '71d57c8ff1c974a8f2d165c436f15504') in 0.002647 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool_soft.asset using Guid(97afe9889f5e56f449c8468a1d0bfdf9) (NativeFormatImporter) -> (artifact id: 'b007a80dd3775e821df40936b5d7e159') in 0.0028092 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_back_sprite_shaded.asset using Guid(383cf440301bf504a9b6c2a8a85c5c10) (NativeFormatImporter) -> (artifact id: '9a14722ac27a7c483b3d93443ba97166') in 0.0031326 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite_shaded.asset using Guid(e8b91a1d7f94f9d419ae3d2cb8aca69c) (NativeFormatImporter) -> (artifact id: '19f48d248392446507eb0aa3bd573d8d') in 0.004078 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_left_sprite_shaded.asset using Guid(29c76ef0ee79e294abc159d852e30d6f) (NativeFormatImporter) -> (artifact id: '7a1e2d14c3f7f57972b5a415bb755af8') in 0.0032855 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite_shaded.asset using Guid(8af717035fe354841a2778b67ef47f60) (NativeFormatImporter) -> (artifact id: '636319e0046c8cb6ef7b2f1e0d8a2fbf') in 0.0039577 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite_shaded.asset using Guid(bcf85f4b6024bd345a6725cd7e3f8122) (NativeFormatImporter) -> (artifact id: '1134ffa9b5a09269a7a99f311dd08502') in 0.0031231 seconds
Start importing Assets/Settings/DefaultVolumeProfile.asset using Guid(2dc156ea366288a46859f93b6d3c2cd0) (NativeFormatImporter) -> (artifact id: 'b03cefda0696e6248b213f7dfd6e20a5') in 0.0027244 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_right_sprite_shaded.asset using Guid(7edaa2e8ce6f3f040896d2d92f57e792) (NativeFormatImporter) -> (artifact id: '4e696edc4775714bf55149c9029e9a87') in 0.0033563 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite_shaded.asset using Guid(8e8558fedae4d5441a0880d3e0ae5dde) (NativeFormatImporter) -> (artifact id: '7a36765b1626f751d5a96e3827f13ea3') in 0.0030811 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite_shaded.asset using Guid(afafc97e4daeeb2408f5830dde7c5f7c) (NativeFormatImporter) -> (artifact id: 'abb16328271b3f261d7f87df395bbbcc') in 0.0028288 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_front_sprite_shaded.asset using Guid(cfe6c4e4efe6c2d4bae69a5a1cb896ad) (NativeFormatImporter) -> (artifact id: 'a1fc470689ab7b251db5843fc102d4a4') in 0.0028416 seconds
Refreshing native plugins compatible for Editor in 0.66 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=c31be12d5987bde41baca01f62350fd5): Total: 0.206 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Scenes/Anemora_FastVS_HouseSlice.unity using Guid(5c2c1333e7d65ba4eac9369f107a6118) (DefaultImporter) -> (artifact id: '1d8da602c75118b5b691860fb9cc03c9') in 0.0017608 seconds
Refreshing native plugins compatible for Editor in 0.67 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=bfd2f44a580c4e74d8d93318ddf86375): Total: 0.052 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Asset Pipeline Refresh (id=9bd4d3956a9eba14ba1da9442c9b099c): Total: 0.028 seconds - Initiated by RefreshV2(NoUpdateAssetOptions)
Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:265)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaSunlitFloorIslandsVisibilityCycle110Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43880)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaBroadSunfieldCycle111Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43902)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaSunExposureBaseCycle112Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43908)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaSunbeamShaftsCycle113Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43914)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 265)

Opening scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
Unloading 0 Unused Serialized files (Serialized files now loaded: 0)
Loaded scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
	Deserialize:            283.793 ms
	Integration:            301.352 ms
	Integration of assets:  2.544 ms
	Thread Wait Time:       -2.288 ms
	Total Operation Time:   585.401 ms
Unloading 569 unused Assets / (5.2 MB). Loaded Objects now: 24224.
Memory consumption went from 259.7 MB to 254.5 MB.
Total: 13.695300 ms (FindLiveObjects: 1.979400 ms CreateObjectMapping: 0.406800 ms MarkObjects: 9.272100 ms  DeleteObjects: 2.035900 ms)

InvalidOperationException: House slice validation failed: cycle 113 plaza sunbeam shaft texture must preserve a visible beam peak without turning into a slab (beamMax=0.239, beamAverage=0.109).
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaSunbeamShaftsCycle113TextureMetrics () [0x00243] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:33986 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaSunbeamShaftsCycle113 () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:33780 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSunbeamShaftsCycle113Batch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:43915 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 33986)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSunbeamShaftsCycle113Batch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[10:21:56] Phase 'validate' FAILED with exit 1
[10:21:56] NoRollback set; preserving worktree after validate failure
```

## Cycle 113 failure (validate) -- 20260524-102307

```
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite.mat using Guid(57d1354643503484f8f9504dec7939e7) (NativeFormatImporter) -> (artifact id: '8d6144afb428b20ddb5b28b485258685') in 0.0020485 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite.mat using Guid(f79bd52245f424d42b668da63a26eed7) (NativeFormatImporter) -> (artifact id: '6c7634a35d7a5e593d016924efb11022') in 0.0020207 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite.mat using Guid(f71c1a4bbbfa65e4a9dc8ceb1dbaab05) (NativeFormatImporter) -> (artifact id: '067c3ca3644ebbf71c59456785c01e71') in 0.0023112 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite.mat using Guid(28f2cae61d95125439828020176a4cfc) (NativeFormatImporter) -> (artifact id: '72016ad2a7c404cc799f9f50c6febaf5') in 0.0023316 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite.mat using Guid(a8c94ff07ef65ea4f8ad0dedea407b1e) (NativeFormatImporter) -> (artifact id: '63c8344cf78a915d285cd3eae78cb1fd') in 0.0026128 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite.mat using Guid(b8841e13281e616468837ca346676037) (NativeFormatImporter) -> (artifact id: '1adcf1ea1109f3defbf059ddda5acb22') in 0.0026816 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite.mat using Guid(3acdb3d0d51b28d40854136863dba5b3) (NativeFormatImporter) -> (artifact id: '085d72f1844e837c1fa998fa9467b7c2') in 0.0023521 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.0021831 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce.mat using Guid(0bc31eecccdf1a840a5477d771512775) (NativeFormatImporter) -> (artifact id: '957df3d5816f00a64823db316222cbe2') in 0.0025022 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool.mat using Guid(0dd9370c56289ae4a8d0e1dddfc59477) (NativeFormatImporter) -> (artifact id: 'dff4420f29bfcf0b5e37a561766cc28b') in 0.0023863 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_route_move_floor_glow.mat using Guid(0dbdea9defe689a4ca05680804b173ca) (NativeFormatImporter) -> (artifact id: '76abd1cd4f418349642ae7527f51d632') in 0.0022178 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_back_sprite.mat using Guid(8d4799b24d7badb4d9d50d8f3546587c) (NativeFormatImporter) -> (artifact id: 'ea1cc68dacc196e87140893dcea1fad6') in 0.0021202 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.mat using Guid(0e535f48a85f05b43ad8f1b5cb2ecb4a) (NativeFormatImporter) -> (artifact id: '95a63720d5377afca26fb765e42e17fd') in 0.0021111 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_contact_shadow.asset using Guid(40c9d8116f275fd4c98bbdde1e793a32) (NativeFormatImporter) -> (artifact id: '17d86434566b3eaf148ab06195b0eaff') in 0.0023448 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite_shaded.asset using Guid(11367ee44066a2c4a933e1a65a54fa71) (NativeFormatImporter) -> (artifact id: 'b4ea198999a098bff466fc482aa2e085') in 0.0029518 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite_shaded.asset using Guid(31848d2268cea5d4a93190a2365133ca) (NativeFormatImporter) -> (artifact id: 'dfcf40487733ba25c6e6bad9c26fa7e7') in 0.0031802 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_library_window_light_cookie_soft.asset using Guid(e10a87518f970b6498f120403b5e87fa) (NativeFormatImporter) -> (artifact id: 'b6624ac7fb17375ff687066ce4c5208b') in 0.0028628 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_left_sprite_shaded.asset using Guid(f13736bef9726b045ba18071b949e2d2) (NativeFormatImporter) -> (artifact id: 'f6e48f8240644c12833b02b6b33897d1') in 0.0028731 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.002174 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce_soft.asset using Guid(0442e2f80b7dbc34e99a112ae1095f20) (NativeFormatImporter) -> (artifact id: 'a65e2dbc19e50b0fc0fa4598f134ffd8') in 0.0024216 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite_shaded.asset using Guid(246d9d1ed78f72147aa1f537daf9164e) (NativeFormatImporter) -> (artifact id: '59a4f02825ed43e073fd3814395235f5') in 0.0030613 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.asset using Guid(64767a48f06c7a4439175ba9f3f76969) (NativeFormatImporter) -> (artifact id: '2c34ef17951f1b28aef444ba8ef4c761') in 0.0028746 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_back_sprite_shaded.asset using Guid(646afdaa35aa1c84bbfca6d10ad8d956) (NativeFormatImporter) -> (artifact id: '71d57c8ff1c974a8f2d165c436f15504') in 0.0031496 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool_soft.asset using Guid(97afe9889f5e56f449c8468a1d0bfdf9) (NativeFormatImporter) -> (artifact id: 'b007a80dd3775e821df40936b5d7e159') in 0.0023705 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_back_sprite_shaded.asset using Guid(383cf440301bf504a9b6c2a8a85c5c10) (NativeFormatImporter) -> (artifact id: '9a14722ac27a7c483b3d93443ba97166') in 0.0033084 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite_shaded.asset using Guid(e8b91a1d7f94f9d419ae3d2cb8aca69c) (NativeFormatImporter) -> (artifact id: '19f48d248392446507eb0aa3bd573d8d') in 0.0031705 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_left_sprite_shaded.asset using Guid(29c76ef0ee79e294abc159d852e30d6f) (NativeFormatImporter) -> (artifact id: '7a1e2d14c3f7f57972b5a415bb755af8') in 0.0032496 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite_shaded.asset using Guid(8af717035fe354841a2778b67ef47f60) (NativeFormatImporter) -> (artifact id: '636319e0046c8cb6ef7b2f1e0d8a2fbf') in 0.003156 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite_shaded.asset using Guid(bcf85f4b6024bd345a6725cd7e3f8122) (NativeFormatImporter) -> (artifact id: '1134ffa9b5a09269a7a99f311dd08502') in 0.0029409 seconds
Start importing Assets/Settings/DefaultVolumeProfile.asset using Guid(2dc156ea366288a46859f93b6d3c2cd0) (NativeFormatImporter) -> (artifact id: 'b03cefda0696e6248b213f7dfd6e20a5') in 0.0023956 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_right_sprite_shaded.asset using Guid(7edaa2e8ce6f3f040896d2d92f57e792) (NativeFormatImporter) -> (artifact id: '4e696edc4775714bf55149c9029e9a87') in 0.0028389 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite_shaded.asset using Guid(8e8558fedae4d5441a0880d3e0ae5dde) (NativeFormatImporter) -> (artifact id: '7a36765b1626f751d5a96e3827f13ea3') in 0.0041799 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite_shaded.asset using Guid(afafc97e4daeeb2408f5830dde7c5f7c) (NativeFormatImporter) -> (artifact id: 'abb16328271b3f261d7f87df395bbbcc') in 0.0030436 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_front_sprite_shaded.asset using Guid(cfe6c4e4efe6c2d4bae69a5a1cb896ad) (NativeFormatImporter) -> (artifact id: 'a1fc470689ab7b251db5843fc102d4a4') in 0.003231 seconds
Refreshing native plugins compatible for Editor in 0.69 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=2e8f8d9d32fde1c4bb53bf57786f141c): Total: 0.209 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Scenes/Anemora_FastVS_HouseSlice.unity using Guid(5c2c1333e7d65ba4eac9369f107a6118) (DefaultImporter) -> (artifact id: 'e9dfe49f24f800828114198555a77a80') in 0.0018041 seconds
Refreshing native plugins compatible for Editor in 0.75 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=5dadec5b76d481043b954bb2d9718c3f): Total: 0.069 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Asset Pipeline Refresh (id=bd6951790782e3b46978d095e7d89de2): Total: 0.040 seconds - Initiated by RefreshV2(NoUpdateAssetOptions)
Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:265)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaSunlitFloorIslandsVisibilityCycle110Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43880)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaBroadSunfieldCycle111Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43902)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaSunExposureBaseCycle112Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43908)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaSunbeamShaftsCycle113Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:43914)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 265)

Opening scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
Unloading 0 Unused Serialized files (Serialized files now loaded: 0)
Loaded scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
	Deserialize:            314.082 ms
	Integration:            353.997 ms
	Integration of assets:  3.117 ms
	Thread Wait Time:       -2.959 ms
	Total Operation Time:   668.236 ms
Unloading 569 unused Assets / (5.6 MB). Loaded Objects now: 24224.
Memory consumption went from 259.9 MB to 254.3 MB.
Total: 16.397300 ms (FindLiveObjects: 2.151400 ms CreateObjectMapping: 0.747800 ms MarkObjects: 11.337800 ms  DeleteObjects: 2.158600 ms)

InvalidOperationException: House slice validation failed: cycle 113 plaza sunbeam shaft texture must preserve a visible beam peak without turning into a slab (beamMax=0.239, beamAverage=0.110).
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaSunbeamShaftsCycle113TextureMetrics () [0x00243] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:33986 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaSunbeamShaftsCycle113 () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:33780 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSunbeamShaftsCycle113Batch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:43915 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 33986)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSunbeamShaftsCycle113Batch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[10:24:20] Phase 'validate' FAILED with exit 1
[10:24:20] NoRollback set; preserving worktree after validate failure
```
