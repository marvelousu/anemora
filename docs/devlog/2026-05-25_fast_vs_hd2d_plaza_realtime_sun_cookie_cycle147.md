# feat(hd2d): project plaza realtime sun cookie

## Intent

Cycle 146 still read like hard shadow geometry over a flat map. Cycle 147 moves the next visible light pattern into the realtime Directional Light itself, restores the central-plaza follow camera to the VS side-view profile, and keeps fake sky/haze plates disabled.

## Scope

- Add a runtime generated 128x128 Directional Light cookie for the current central plaza.
- Sample URP `_LIGHT_COOKIES` in `FastVS_SurfaceRampLit` so the receiver shader responds to light projection instead of painted map haze.
- Raise the plaza sun intensity enough for cookie contrast to read through existing realtime casters.
- Restore central plaza follow framing to the VS-style side camera: `(0, 2.75, -4.55)` looking at `(0, 0.72, 0.45)` with `38` FOV.
- Validate that fog and fake central-plaza sky/haze plates remain off.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 147 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeSunCookieCycle147Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaRealtimeSunCookieCycle147ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_realtime_sun_cookie_cycle147.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle147_plaza_realtime_sun_cookie_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_follow_start.png`
- `parent_review_02_current_central_plaza_follow_forward.png`
- `parent_review_03_current_central_plaza_realtime_shadow_floor.png`
- `parent_review_04_current_central_plaza_realtime_shadow_facade.png`
- `parent_review_05_current_library_guard.png`

## Visual Gate

- The follow shots must use the VS side-view camera, not the low forward-looking recovery camera.
- The central plaza must show realtime caster shadows plus light-cookie modulation from the Directional Light.
- No map-painted fog, camera shadow plates, or fake sky/haze planes may be visible in the current central plaza.

## Cycle 147 failure (validate) -- 20260525-062053

```
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool.mat using Guid(0dd9370c56289ae4a8d0e1dddfc59477) (NativeFormatImporter) -> (artifact id: 'cfd906379f8a62ffaf4c7136665bf0f5') in 0.0020085 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_route_move_floor_glow.mat using Guid(0dbdea9defe689a4ca05680804b173ca) (NativeFormatImporter) -> (artifact id: '26e11471e027e487c6b513b0683a06d9') in 0.0020826 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_back_sprite.mat using Guid(8d4799b24d7badb4d9d50d8f3546587c) (NativeFormatImporter) -> (artifact id: 'ea1cc68dacc196e87140893dcea1fad6') in 0.0019416 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.mat using Guid(0e535f48a85f05b43ad8f1b5cb2ecb4a) (NativeFormatImporter) -> (artifact id: '95a63720d5377afca26fb765e42e17fd') in 0.0018534 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_contact_shadow.asset using Guid(40c9d8116f275fd4c98bbdde1e793a32) (NativeFormatImporter) -> (artifact id: '17d86434566b3eaf148ab06195b0eaff') in 0.002037 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite_shaded.asset using Guid(11367ee44066a2c4a933e1a65a54fa71) (NativeFormatImporter) -> (artifact id: 'b4ea198999a098bff466fc482aa2e085') in 0.0024307 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite_shaded.asset using Guid(31848d2268cea5d4a93190a2365133ca) (NativeFormatImporter) -> (artifact id: 'dfcf40487733ba25c6e6bad9c26fa7e7') in 0.0028516 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_library_window_light_cookie_soft.asset using Guid(e10a87518f970b6498f120403b5e87fa) (NativeFormatImporter) -> (artifact id: 'b6624ac7fb17375ff687066ce4c5208b') in 0.0023693 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_left_sprite_shaded.asset using Guid(f13736bef9726b045ba18071b949e2d2) (NativeFormatImporter) -> (artifact id: 'f6e48f8240644c12833b02b6b33897d1') in 0.0022096 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0018622 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce_soft.asset using Guid(0442e2f80b7dbc34e99a112ae1095f20) (NativeFormatImporter) -> (artifact id: 'a65e2dbc19e50b0fc0fa4598f134ffd8') in 0.0019365 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite_shaded.asset using Guid(246d9d1ed78f72147aa1f537daf9164e) (NativeFormatImporter) -> (artifact id: '59a4f02825ed43e073fd3814395235f5') in 0.0026982 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.asset using Guid(64767a48f06c7a4439175ba9f3f76969) (NativeFormatImporter) -> (artifact id: '2c34ef17951f1b28aef444ba8ef4c761') in 0.0028023 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_back_sprite_shaded.asset using Guid(646afdaa35aa1c84bbfca6d10ad8d956) (NativeFormatImporter) -> (artifact id: '71d57c8ff1c974a8f2d165c436f15504') in 0.0027883 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool_soft.asset using Guid(97afe9889f5e56f449c8468a1d0bfdf9) (NativeFormatImporter) -> (artifact id: 'b007a80dd3775e821df40936b5d7e159') in 0.0023389 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_back_sprite_shaded.asset using Guid(383cf440301bf504a9b6c2a8a85c5c10) (NativeFormatImporter) -> (artifact id: '9a14722ac27a7c483b3d93443ba97166') in 0.0022993 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite_shaded.asset using Guid(e8b91a1d7f94f9d419ae3d2cb8aca69c) (NativeFormatImporter) -> (artifact id: '19f48d248392446507eb0aa3bd573d8d') in 0.0024493 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_left_sprite_shaded.asset using Guid(29c76ef0ee79e294abc159d852e30d6f) (NativeFormatImporter) -> (artifact id: '7a1e2d14c3f7f57972b5a415bb755af8') in 0.0024162 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite_shaded.asset using Guid(8af717035fe354841a2778b67ef47f60) (NativeFormatImporter) -> (artifact id: '636319e0046c8cb6ef7b2f1e0d8a2fbf') in 0.0027069 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite_shaded.asset using Guid(bcf85f4b6024bd345a6725cd7e3f8122) (NativeFormatImporter) -> (artifact id: '1134ffa9b5a09269a7a99f311dd08502') in 0.0025868 seconds
Start importing Assets/Settings/DefaultVolumeProfile.asset using Guid(2dc156ea366288a46859f93b6d3c2cd0) (NativeFormatImporter) -> (artifact id: '0ea9a860dbdcfbea417dada328e83374') in 0.001977 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_right_sprite_shaded.asset using Guid(7edaa2e8ce6f3f040896d2d92f57e792) (NativeFormatImporter) -> (artifact id: '4e696edc4775714bf55149c9029e9a87') in 0.0022267 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite_shaded.asset using Guid(8e8558fedae4d5441a0880d3e0ae5dde) (NativeFormatImporter) -> (artifact id: '7a36765b1626f751d5a96e3827f13ea3') in 0.0024077 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite_shaded.asset using Guid(afafc97e4daeeb2408f5830dde7c5f7c) (NativeFormatImporter) -> (artifact id: 'abb16328271b3f261d7f87df395bbbcc') in 0.0024727 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_front_sprite_shaded.asset using Guid(cfe6c4e4efe6c2d4bae69a5a1cb896ad) (NativeFormatImporter) -> (artifact id: 'a1fc470689ab7b251db5843fc102d4a4') in 0.0025669 seconds
Refreshing native plugins compatible for Editor in 0.53 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=cf10fe7d3bdbfc44ab00b375641768dd): Total: 0.182 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Scenes/Anemora_FastVS_HouseSlice.unity using Guid(5c2c1333e7d65ba4eac9369f107a6118) (DefaultImporter) -> (artifact id: 'd75cdd46fcdea1ecc5d5f9aec080055d') in 0.001281 seconds
Refreshing native plugins compatible for Editor in 0.59 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=08a583c6b4d2c56458768beca81c2fbf): Total: 0.046 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Asset Pipeline Refresh (id=746edb6f0829edf4c9be2f7bccf97056): Total: 0.027 seconds - Initiated by RefreshV2(NoUpdateAssetOptions)
Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:337)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaRealtimeSunCookieCycle147Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:49053)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 337)

Opening scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
Unloading 0 Unused Serialized files (Serialized files now loaded: 0)
Loaded scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
	Deserialize:            265.489 ms
	Integration:            288.018 ms
	Integration of assets:  3.991 ms
	Thread Wait Time:       -3.968 ms
	Total Operation Time:   553.531 ms
Unloading 629 unused Assets / (8.3 MB). Loaded Objects now: 25402.
Memory consumption went from 254.8 MB to 246.4 MB.
Total: 12.248600 ms (FindLiveObjects: 2.196900 ms CreateObjectMapping: 0.553400 ms MarkObjects: 7.619500 ms  DeleteObjects: 1.878000 ms)

InvalidOperationException: House slice validation failed: cycle 134 must use realtime sun/shadow values, found intensity=2.450, shadowStrength=0.920.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaRealtimeShadowRecoveryCycle134 () [0x000d8] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:37741 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaVsCameraRecoveryCycle135 () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:37852 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaMapSafeCameraCycle136 () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:37869 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaFollowRealtimeTrackingCycle137 () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:37874 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaRealtimeDappleCasterCycle138 () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:37904 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaRealtimeSurfaceResponseCycle139 () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:37943 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaRealtimeMeshCasterCycle140 () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:38005 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaRealtimeCasterShapeRewriteCycle141 () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:38038 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaRealtimeShadowTextureCycle142 () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:38080 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaRealtimeShadowLiftCycle143 () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:38112 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaRealtimeSunLiftCycle144 () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:38144 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaReferenceCameraGradeCycle145 () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:38177 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaFacadeShadowLiftCycle146 () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:38217 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaRealtimeSunCookieCycle147 () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:38264 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeSunCookieCycle147Batch () [0x00011] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:49055 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 37741)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeSunCookieCycle147Batch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[06:31:12] Phase 'validate' FAILED with exit 1
[06:31:12] NoRollback set; preserving worktree after validate failure
```
