# feat(hd2d): add plaza realtime light shadow rig

## Intent

Cycle 126 proved that adding more static overlay bands is no longer the fastest route. It removed some blackness but introduced broad horizontal light bars that still read unlike the reference images.

Cycle 127 breaks that premise: central plaza review lighting now uses a realtime directional light/shadow rig, disables the accumulated static overlay stack for the current plaza, and adds invisible shadow casters so the scene can produce actual soft cast shadows instead of painted bands.

## Scope

- Add `FastVsRealtimeLightShadowRig` to enforce outdoor realtime shadow policy at runtime/review time.
- Attach the rig in the generated house slice scene and wire it to area visibility, camera, and main directional light.
- Make central plaza and exterior use solid camera sky color, disabled fog, strong low-bias soft directional shadows, and stronger material light/shadow response.
- Disable active current-plaza static light/fog/shadow overlays except map move glow pads.
- Add current-plaza realtime shadow casters for:
  - library eave,
  - left canopy,
  - right crate,
  - front broken beam.
- Update sprite-card ramp shadow response so characters can cast/receive realtime shadows.
- Add dedicated realtime shadow validation and parent-review screenshots.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 127 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeLightShadowCycle127Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaRealtimeLightShadowCycle127ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_realtime_light_shadow_cycle127.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle127_plaza_realtime_light_shadow_parent_review_20260524_01`

- `parent_review_01_current_central_plaza_realtime_shadow_vs_camera_overview.png`
- `parent_review_02_current_central_plaza_realtime_shadow_close.png`
- `parent_review_03_current_central_plaza_realtime_shadow_follow_probe.png`
- `parent_review_04_current_library_realtime_shadow_guard.png`

## Visual Gate

- Current plaza should no longer be driven by the accumulated static overlay bands.
- Sun/shadow contrast should come from actual soft realtime casts.
- The current plaza should show stronger readable shadow direction and less flat stage-light striping.
- Current library guard capture should remain stable.

## Cycle 127 failure (validate) -- 20260524-230110

```
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_left_sprite.mat using Guid(5590a3bdf193f5747855c04828f83adc) (NativeFormatImporter) -> (artifact id: '2de0e52d48bc0e6fcce108912353c7c5') in 0.0020075 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite.mat using Guid(56b5ac4afa850e94fa26bf722f31b14b) (NativeFormatImporter) -> (artifact id: '54a076f71584de57e5f5a78259e82aed') in 0.0020136 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_depth_shadow.mat using Guid(07afc9c1f7f8ab4439760cab6c8b9c1e) (NativeFormatImporter) -> (artifact id: '68a4feae9900f9534bb049459ffb3678') in 0.0017992 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_map_move_floor_glow.mat using Guid(373d809c8f785b3439635e59312211de) (NativeFormatImporter) -> (artifact id: '24bd8e64df90c660225053541b103ab0') in 0.0017489 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite.mat using Guid(57d1354643503484f8f9504dec7939e7) (NativeFormatImporter) -> (artifact id: 'ae199d5588e90d54739e2e9cc3518edf') in 0.0019403 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite.mat using Guid(f79bd52245f424d42b668da63a26eed7) (NativeFormatImporter) -> (artifact id: 'd016f6c6bfe5a8d7a92e22f41b997571') in 0.0019493 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite.mat using Guid(f71c1a4bbbfa65e4a9dc8ceb1dbaab05) (NativeFormatImporter) -> (artifact id: 'e342d908f824adaa900b4ee5d95c9868') in 0.0018401 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite.mat using Guid(28f2cae61d95125439828020176a4cfc) (NativeFormatImporter) -> (artifact id: 'b49974d791a159e0cadc8512486e286c') in 0.0018876 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite.mat using Guid(a8c94ff07ef65ea4f8ad0dedea407b1e) (NativeFormatImporter) -> (artifact id: '8b7ab3dc256a393e65925d59687a6e53') in 0.0019242 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite.mat using Guid(b8841e13281e616468837ca346676037) (NativeFormatImporter) -> (artifact id: '7b6d9ce9adccdb07806575cb30b63431') in 0.0018767 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite.mat using Guid(3acdb3d0d51b28d40854136863dba5b3) (NativeFormatImporter) -> (artifact id: '81df77ffa70d3159b01f869c2cc2c83e') in 0.0018436 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.0020538 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce.mat using Guid(0bc31eecccdf1a840a5477d771512775) (NativeFormatImporter) -> (artifact id: '957df3d5816f00a64823db316222cbe2') in 0.002189 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool.mat using Guid(0dd9370c56289ae4a8d0e1dddfc59477) (NativeFormatImporter) -> (artifact id: 'cfd906379f8a62ffaf4c7136665bf0f5') in 0.0027398 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_route_move_floor_glow.mat using Guid(0dbdea9defe689a4ca05680804b173ca) (NativeFormatImporter) -> (artifact id: '26e11471e027e487c6b513b0683a06d9') in 0.0020931 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_back_sprite.mat using Guid(8d4799b24d7badb4d9d50d8f3546587c) (NativeFormatImporter) -> (artifact id: '7bbcbe07c0ffa9b009452a56ae78abbd') in 0.0020405 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.mat using Guid(0e535f48a85f05b43ad8f1b5cb2ecb4a) (NativeFormatImporter) -> (artifact id: '95a63720d5377afca26fb765e42e17fd') in 0.0020134 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_contact_shadow.asset using Guid(40c9d8116f275fd4c98bbdde1e793a32) (NativeFormatImporter) -> (artifact id: '17d86434566b3eaf148ab06195b0eaff') in 0.0024423 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite_shaded.asset using Guid(11367ee44066a2c4a933e1a65a54fa71) (NativeFormatImporter) -> (artifact id: 'b4ea198999a098bff466fc482aa2e085') in 0.0030823 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite_shaded.asset using Guid(31848d2268cea5d4a93190a2365133ca) (NativeFormatImporter) -> (artifact id: 'dfcf40487733ba25c6e6bad9c26fa7e7') in 0.0025692 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_library_window_light_cookie_soft.asset using Guid(e10a87518f970b6498f120403b5e87fa) (NativeFormatImporter) -> (artifact id: 'b6624ac7fb17375ff687066ce4c5208b') in 0.0026888 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_left_sprite_shaded.asset using Guid(f13736bef9726b045ba18071b949e2d2) (NativeFormatImporter) -> (artifact id: 'f6e48f8240644c12833b02b6b33897d1') in 0.0027885 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0023689 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce_soft.asset using Guid(0442e2f80b7dbc34e99a112ae1095f20) (NativeFormatImporter) -> (artifact id: 'a65e2dbc19e50b0fc0fa4598f134ffd8') in 0.0021863 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite_shaded.asset using Guid(246d9d1ed78f72147aa1f537daf9164e) (NativeFormatImporter) -> (artifact id: '59a4f02825ed43e073fd3814395235f5') in 0.0026244 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.asset using Guid(64767a48f06c7a4439175ba9f3f76969) (NativeFormatImporter) -> (artifact id: '2c34ef17951f1b28aef444ba8ef4c761') in 0.0023235 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_back_sprite_shaded.asset using Guid(646afdaa35aa1c84bbfca6d10ad8d956) (NativeFormatImporter) -> (artifact id: '71d57c8ff1c974a8f2d165c436f15504') in 0.0025816 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool_soft.asset using Guid(97afe9889f5e56f449c8468a1d0bfdf9) (NativeFormatImporter) -> (artifact id: 'b007a80dd3775e821df40936b5d7e159') in 0.0024229 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_back_sprite_shaded.asset using Guid(383cf440301bf504a9b6c2a8a85c5c10) (NativeFormatImporter) -> (artifact id: '9a14722ac27a7c483b3d93443ba97166') in 0.0025341 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite_shaded.asset using Guid(e8b91a1d7f94f9d419ae3d2cb8aca69c) (NativeFormatImporter) -> (artifact id: '19f48d248392446507eb0aa3bd573d8d') in 0.0025372 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_left_sprite_shaded.asset using Guid(29c76ef0ee79e294abc159d852e30d6f) (NativeFormatImporter) -> (artifact id: '7a1e2d14c3f7f57972b5a415bb755af8') in 0.0025261 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite_shaded.asset using Guid(8af717035fe354841a2778b67ef47f60) (NativeFormatImporter) -> (artifact id: '636319e0046c8cb6ef7b2f1e0d8a2fbf') in 0.0024915 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite_shaded.asset using Guid(bcf85f4b6024bd345a6725cd7e3f8122) (NativeFormatImporter) -> (artifact id: '1134ffa9b5a09269a7a99f311dd08502') in 0.0027633 seconds
Start importing Assets/Settings/DefaultVolumeProfile.asset using Guid(2dc156ea366288a46859f93b6d3c2cd0) (NativeFormatImporter) -> (artifact id: '25538e5d27d1bd3c782afb4302706fbd') in 0.0024443 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_right_sprite_shaded.asset using Guid(7edaa2e8ce6f3f040896d2d92f57e792) (NativeFormatImporter) -> (artifact id: '4e696edc4775714bf55149c9029e9a87') in 0.0028423 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite_shaded.asset using Guid(8e8558fedae4d5441a0880d3e0ae5dde) (NativeFormatImporter) -> (artifact id: '7a36765b1626f751d5a96e3827f13ea3') in 0.002843 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite_shaded.asset using Guid(afafc97e4daeeb2408f5830dde7c5f7c) (NativeFormatImporter) -> (artifact id: 'abb16328271b3f261d7f87df395bbbcc') in 0.0024798 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_front_sprite_shaded.asset using Guid(cfe6c4e4efe6c2d4bae69a5a1cb896ad) (NativeFormatImporter) -> (artifact id: 'a1fc470689ab7b251db5843fc102d4a4') in 0.0028167 seconds
Refreshing native plugins compatible for Editor in 0.54 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=35cb5d3eb3fac2e449236b23c89451ff): Total: 0.193 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Scenes/Anemora_FastVS_HouseSlice.unity using Guid(5c2c1333e7d65ba4eac9369f107a6118) (DefaultImporter) -> (artifact id: '08ea24c7473b6fb8b41cb1bb69d3f848') in 0.0012625 seconds
Refreshing native plugins compatible for Editor in 0.59 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=f1959f80e70a81c41a92cee618162bc4): Total: 0.041 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Asset Pipeline Refresh (id=bdb7b0b73bfdbb24da652a013f875c4e): Total: 0.027 seconds - Initiated by RefreshV2(NoUpdateAssetOptions)
Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:337)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaRealtimeLightShadowCycle127Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:46658)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 337)

Opening scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
Unloading 0 Unused Serialized files (Serialized files now loaded: 0)
Loaded scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
	Deserialize:            254.390 ms
	Integration:            284.213 ms
	Integration of assets:  4.094 ms
	Thread Wait Time:       -4.065 ms
	Total Operation Time:   538.631 ms
Unloading 617 unused Assets / (5.4 MB). Loaded Objects now: 25100.
Memory consumption went from 231.2 MB to 225.7 MB.
Total: 12.329300 ms (FindLiveObjects: 2.027300 ms CreateObjectMapping: 0.390000 ms MarkObjects: 7.621500 ms  DeleteObjects: 2.289600 ms)

InvalidOperationException: House slice validation failed: cycle 127 realtime shadow caster Current_CentralPlaza_RealtimeShadowCasterCycle127_LibraryEave must be ShadowsOnly and non-receiving.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaRealtimeLightShadowCycle127 () [0x003ca] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:36223 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeLightShadowCycle127Batch () [0x00011] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:46660 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 36223)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeLightShadowCycle127Batch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[23:11:22] Phase 'validate' FAILED with exit 1
[23:11:22] NoRollback set; preserving worktree after validate failure
```

## Cycle 127 failure (build) -- 20260524-231304

```
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=c450651f112d6f3428fcc09107a61e57): Total: 0.024 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Aria/resident_a_aria_normal_loop_breath_v01_4f_64x96_review_only.png using Guid(a8a8cb999611bfd4ca49e819b825b9fb) (TextureImporter) -> (artifact id: 'b4439db80e6d0ccbb84b2ae2c1308c2e') in 0.0033288 seconds
Refreshing native plugins compatible for Editor in 0.59 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=c19186b268deb734d9a5a05be930ed2e): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Aria/resident_a_aria_normal_loop_breath_v01_4f_64x96_review_only.png using Guid(a8a8cb999611bfd4ca49e819b825b9fb) (TextureImporter) -> (artifact id: 'b4439db80e6d0ccbb84b2ae2c1308c2e') in 0.0032813 seconds
Refreshing native plugins compatible for Editor in 0.56 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=4aa3af2d52ac9f1448a79cd2d0ff95bb): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Aria/resident_a_aria_normal_loop_breath_v01_4f_64x96_review_only.png using Guid(a8a8cb999611bfd4ca49e819b825b9fb) (TextureImporter) -> (artifact id: 'b4439db80e6d0ccbb84b2ae2c1308c2e') in 0.0047174 seconds
Refreshing native plugins compatible for Editor in 0.59 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=f85e2df395858d645ae726addc2ab3a6): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Aria/resident_a_aria_normal_loop_breath_v01_4f_64x96_review_only.png using Guid(a8a8cb999611bfd4ca49e819b825b9fb) (TextureImporter) -> (artifact id: 'b4439db80e6d0ccbb84b2ae2c1308c2e') in 0.0037904 seconds
Refreshing native plugins compatible for Editor in 0.54 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=0dc67d40f902a2445bedb7f9425e324e): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_writing_normal_loop_v02_6f_64x96.png using Guid(61b3cd5f5b4bc2b41953653a46c5c050) (TextureImporter) -> (artifact id: '7ff045c530812383dfde4d98fe46707e') in 0.0039919 seconds
Refreshing native plugins compatible for Editor in 0.57 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=79df91a41f6fe3748820f0e0c1447221): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_writing_normal_loop_v02_6f_64x96.png using Guid(61b3cd5f5b4bc2b41953653a46c5c050) (TextureImporter) -> (artifact id: '7ff045c530812383dfde4d98fe46707e') in 0.0038009 seconds
Refreshing native plugins compatible for Editor in 0.61 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=2423d9da567118a4f87279ecf50d79de): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0037722 seconds
Refreshing native plugins compatible for Editor in 0.57 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=e7b036c5105dd3741b4628e608a6debc): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0037926 seconds
Refreshing native plugins compatible for Editor in 0.56 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=8b5a55ead3ec850449da9c3dc2563d2e): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0032537 seconds
Refreshing native plugins compatible for Editor in 0.61 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=07199e820c610b0499b7057f09c10e81): Total: 0.024 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0038167 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=555eb158a2d92d4458df743d61279775): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0041987 seconds
Refreshing native plugins compatible for Editor in 0.57 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=f7588634e4c7e194a9e1012d7342eaa6): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0039903 seconds
Refreshing native plugins compatible for Editor in 0.56 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=786aa2742aafe3f45bebf5b817e62964): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
InvalidOperationException: HD2D material role audit failed:
- Material Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_exterior_wall.mat must keep _DirectionalLightStrength in the 0.04-0.46 range, but was 0.600.
- Material Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_exterior_wall.mat must keep _ShadowReceiveStrength in the 0.05-0.60 range, but was 0.680.
- Material Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_ground.mat must keep _DirectionalLightStrength in the 0.04-0.46 range, but was 0.600.
- Material Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_ground.mat must keep _ShadowReceiveStrength in the 0.05-0.60 range, but was 0.680.
- Material Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_path.mat must keep _DirectionalLightStrength in the 0.04-0.46 range, but was 0.600.
- Material Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_path.mat must keep _ShadowReceiveStrength in the 0.05-0.60 range, but was 0.680.
- Material Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_roof.mat must keep _DirectionalLightStrength in the 0.04-0.46 range, but was 0.600.
- Material Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_roof.mat must keep _ShadowReceiveStrength in the 0.05-0.60 range, but was 0.680.
- Material Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_wood_floor.mat must keep _DirectionalLightStrength in the 0.04-0.46 range, but was 0.600.
- Material Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_wood_floor.mat must keep _ShadowReceiveStrength in the 0.05-0.60 range, but was 0.680.
- Material Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_interior_wall.mat must keep _DirectionalLightStrength in the 0.04-0.46 range, but was 0.600.
- Material Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_interior_wall.mat must keep _ShadowReceiveStrength in the 0.05-0.60 range, but was 0.680.
- Material Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_exterior_wall.mat must keep _DirectionalLightStrength in the 0.04-0.46 range, but was 0.600.
- Material Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_exterior_wall.mat must keep _ShadowReceiveStrength in the 0.05-0.60 range, but was 0.680.
- Material Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_furniture.mat must keep _DirectionalLightStrength in the 0.04-0.46 range, but was 0.600.
- Material Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_furniture.mat must keep _ShadowReceiveStrength in the 0.05-0.60 range, but was 0.680.
- Material Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_book.mat must keep _DirectionalLightStrength in the 0.04-0.46 range, but was 0.600.
- Material Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_book.mat must keep _ShadowReceiveStrength in the 0.05-0.60 range, but was 0.680.
  at Anemora.EditorTools.AnemoraFastVsHd2dMaterialRoleFoundationAudit.VerifyMaterialRolesV1 () [0x00064] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs:35 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x0009f] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:367 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:625 

(Filename: Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs Line: 35)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[23:15:46] Phase 'build' FAILED with exit 1
[23:15:46] NoRollback set; preserving worktree after build failure
```

## Cycle 127 failure (build) -- 20260524-231645

```
Asset Pipeline Refresh (id=b74785a087a9ff948954c5841e2da242): Total: 0.024 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0040282 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=48e46d18352cf40438fb859b24f843f6): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.00393 seconds
Refreshing native plugins compatible for Editor in 0.56 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=6c09410db56991d42a2bfa20590e1c43): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0035508 seconds
Refreshing native plugins compatible for Editor in 0.57 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=5ca9266b1776e3c4194126d675ea55ee): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0032397 seconds
Refreshing native plugins compatible for Editor in 0.52 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=1dc35eb0ab320154d929117aaa2d9962): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0038755 seconds
Refreshing native plugins compatible for Editor in 0.56 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=402eab939cafbd3499498898f10d3134): Total: 0.024 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0035806 seconds
Refreshing native plugins compatible for Editor in 0.62 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=212535b80b60c794ea5d130222a533aa): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
HD2D material role audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dMaterialRoleFoundationAudit:VerifyMaterialRolesV1 () (at Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs:34)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:367)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs Line: 34)

InvalidOperationException: HD2D sprite card lighting audit failed:
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_front_sprite.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_back_sprite.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_left_sprite.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_right_sprite.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_front_sprite.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_front_sprite.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_back_sprite.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_left_sprite.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_reto_v02_writing_loop_sprite.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_reto_v02_lower_arms_sprite.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_reto_v02_talk_loop_sprite.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_reto_v02_raise_arms_sprite.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_aria_v46_normal_loop_breath_sprite.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_tree3_sprite_cc0.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_tree3_sprite_cc0.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_north_hedge_sprite_a_cc0.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_north_hedge_sprite_b_cc0.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_north_hedge_sprite_a_cc0.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_north_hedge_sprite_b_cc0.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_central_plaza_north_tree_line_sprite_a_cc0.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_central_plaza_north_tree_line_sprite_b_cc0.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_central_plaza_north_tree_line_sprite_a_cc0.mat (0.150).
- Sprite card material _WorldShadowReceiveStrength is out of range: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_central_plaza_north_tree_line_sprite_b_cc0.mat (0.150).
  at Anemora.EditorTools.AnemoraFastVsHd2dSpriteCardLightingAudit.VerifySpriteCardLightingV1 () [0x001c9] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dSpriteCardLightingAudit.cs:99 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x000a4] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:368 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:625 

(Filename: Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs Line: 99)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[23:19:27] Phase 'build' FAILED with exit 1
[23:19:27] NoRollback set; preserving worktree after build failure
```

## Cycle 127 failure (build) -- 20260524-232045

```
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=deff44e1a99f93e49b6a166c4aea0a5c): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_writing_normal_loop_v02_6f_64x96.png using Guid(61b3cd5f5b4bc2b41953653a46c5c050) (TextureImporter) -> (artifact id: '7ff045c530812383dfde4d98fe46707e') in 0.0035679 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=bbab19b3c4ff693438b2c1cffffb682a): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0038199 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=510844366b9281c42a0fbae4401e0e1c): Total: 0.024 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0036094 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=b97b590fb8cbecb418b4ab30b2b19690): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0038373 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=0767945708ec89b41b2e7f76bbd53827): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0034553 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=d673e225d25897749ae1974143002aeb): Total: 0.024 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0036369 seconds
Refreshing native plugins compatible for Editor in 0.60 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=40d581f57f4b0a446bef9bd9bd4f189a): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0036149 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=8b5aa7f61c9b41e47a7b9a4505d1c38a): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
HD2D material role audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dMaterialRoleFoundationAudit:VerifyMaterialRolesV1 () (at Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs:34)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:367)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs Line: 34)

HD2D sprite card lighting audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSpriteCardLightingAudit:VerifySpriteCardLightingV1 () (at Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs:98)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:368)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs Line: 98)

Shading Foundation v1 audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit:VerifyShadingFoundationV1 () (at Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs:38)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:369)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 38)

InvalidOperationException: House slice validation failed: central plaza lighting profile must keep sun contrast and outdoor fog active.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dShadingFoundationLightingDirector () [0x001a9] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:30635 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x000b8] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:372 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:625 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 30635)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[23:23:26] Phase 'build' FAILED with exit 1
[23:23:26] NoRollback set; preserving worktree after build failure
```

## Cycle 127 failure (build) -- 20260524-232520

```
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=2f9b2f40316a63a41a8881833986111b): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0036394 seconds
Refreshing native plugins compatible for Editor in 0.54 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=7e39b83bb2bc4724dbbe421cbac0d2d8): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0035391 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=d0b4780a61ad6724489826808dff63d4): Total: 0.024 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0036369 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=3151ce087fed65148887bb40e8ba25b9): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0038744 seconds
Refreshing native plugins compatible for Editor in 0.56 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=c8157dc7585084243bd5757fe162dcf5): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0038937 seconds
Refreshing native plugins compatible for Editor in 0.56 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=707ac651a4ce41e46aaf8ba4a485c293): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
HD2D material role audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dMaterialRoleFoundationAudit:VerifyMaterialRolesV1 () (at Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs:34)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:367)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs Line: 34)

HD2D sprite card lighting audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSpriteCardLightingAudit:VerifySpriteCardLightingV1 () (at Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs:98)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:368)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs Line: 98)

Shading Foundation v1 audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit:VerifyShadingFoundationV1 () (at Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs:38)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:369)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 38)

InvalidOperationException: HD2D area lighting profile audit failed:
- Area lighting profile FastVS_HD2D_CentralPlazaLightingProfile must keep keyLightIntensity near 1.840, but was 1.720.
- Area lighting profile FastVS_HD2D_CentralPlazaLightingProfile must keep keyLightTint near RGBA(1.000, 0.740, 0.420, 1.000), but was RGBA(1.000, 0.860, 0.620, 1.000).
- Area lighting profile FastVS_HD2D_CentralPlazaLightingProfile must keep fillIntensity near 0.012, but was 0.020.
- Area lighting profile FastVS_HD2D_CentralPlazaLightingProfile must keep fillTint near RGBA(1.000, 0.550, 0.260, 1.000), but was RGBA(1.000, 0.640, 0.340, 1.000).
- Area lighting profile FastVS_HD2D_CentralPlazaLightingProfile must keep ambientIntensity near 0.045, but was 0.067.
- Area lighting profile FastVS_HD2D_CentralPlazaLightingProfile must keep ambientTint near RGBA(0.052, 0.044, 0.034, 1.000), but was RGBA(0.074, 0.068, 0.058, 1.000).
- Area lighting profile FastVS_HD2D_CentralPlazaLightingProfile must keep runtime ambient luminance near 0.067, but was 0.069.
  at Anemora.EditorTools.AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.VerifyAreaLightingProfilesV1 () [0x00318] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs:143 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x000c2] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:374 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:625 

(Filename: Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs Line: 143)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[23:28:00] Phase 'build' FAILED with exit 1
[23:28:00] NoRollback set; preserving worktree after build failure
```

## Cycle 127 failure (build) -- 20260524-233214

```
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dOverlayProfileFoundationAudit:VerifyOverlayProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs:402)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:375)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs Line: 402)

HD2D surface profile audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceProfileFoundationAudit:VerifySurfaceProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs:104)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:376)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs Line: 104)

HD2D surface texture metric audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceTextureMetricAudit:VerifySurfaceTextureMetricsV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs:72)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:377)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs Line: 72)

HD2D lighting transition audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dLightingTransitionAudit:VerifyLightingTransitionV1 () (at Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs:106)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:378)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs Line: 106)

Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.0031036 seconds
Refreshing native plugins compatible for Editor in 0.59 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=340ca0fc112d14a4e828c9c15ec06ee6): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.0028117 seconds
Refreshing native plugins compatible for Editor in 0.59 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=ad708a5e2cfa14c48bc049af4937e8a4): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Fast VS house exterior eave shadow softening validation passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateFastVsHd2dShadowFoundationCycle87HouseExteriorEaveShadowSoftening () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:44934)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:460)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 44934)

InvalidOperationException: House slice validation failed: CentralPlaza main light intensity must stay within 1.820-1.860, found 1.720.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateCycle49LightingProfileRange (Anemora.FastVS.FastVsHouseLightingDirector director, Anemora.FastVS.FastVsHouseArea area, UnityEngine.Light mainLight, UnityEngine.Light warmFill, UnityEngine.Light libraryWindow, System.Boolean expectFog, UnityEngine.Vector3 minAmbient, UnityEngine.Vector3 maxAmbient, System.Single minMainIntensity, System.Single maxMainIntensity, System.Single minShadowStrength, System.Single maxShadowStrength, System.Single minWarmFillIntensity, System.Single maxWarmFillIntensity, System.Boolean expectLibraryWindowEnabled, System.Single minLibraryWindowIntensity, System.Single maxLibraryWindowIntensity) [0x0001b] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:43956 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dCycle49DecisiveLightShadowContrast () [0x000e1] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:39087 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x0027f] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:463 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:625 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 39087)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[23:35:00] Phase 'build' FAILED with exit 1
[23:35:00] NoRollback set; preserving worktree after build failure
```

## Cycle 127 failure (build) -- 20260524-233733

```
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dOverlayProfileFoundationAudit:VerifyOverlayProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs:402)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:375)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs Line: 402)

HD2D surface profile audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceProfileFoundationAudit:VerifySurfaceProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs:104)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:376)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs Line: 104)

HD2D surface texture metric audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceTextureMetricAudit:VerifySurfaceTextureMetricsV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs:72)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:377)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs Line: 72)

HD2D lighting transition audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dLightingTransitionAudit:VerifyLightingTransitionV1 () (at Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs:106)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:378)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs Line: 106)

Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.0031153 seconds
Refreshing native plugins compatible for Editor in 0.64 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=9dd28ef3e69718f46b639ebcce5b7067): Total: 0.029 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.0030311 seconds
Refreshing native plugins compatible for Editor in 0.56 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=d18326acecb5280418f2c3ed99d63649): Total: 0.028 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Fast VS house exterior eave shadow softening validation passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateFastVsHd2dShadowFoundationCycle87HouseExteriorEaveShadowSoftening () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:44934)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:460)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 44934)

InvalidOperationException: House slice validation failed: Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_exterior_wall.mat must keep _DirectionalLightStrength in the 0.40-0.44 range, but was 0.460.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateMaterialFloatBand (System.String materialId, System.String propertyName, System.Single min, System.Single max) [0x00081] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:49143 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dCycle51MaterialLightShadowResponse () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:43303 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x00293] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:467 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:625 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 49143)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[23:40:15] Phase 'build' FAILED with exit 1
[23:40:15] NoRollback set; preserving worktree after build failure
```

## Cycle 127 failure (build) -- 20260524-234117

```
(Filename: Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs Line: 106)

Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.003288 seconds
Refreshing native plugins compatible for Editor in 0.57 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=e9204e17d2ed9de48a645732f326da0f): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.0027067 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=5dd4f9c1b2317c84a866173b88b178df): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Fast VS house exterior eave shadow softening validation passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateFastVsHd2dShadowFoundationCycle87HouseExteriorEaveShadowSoftening () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:44934)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:460)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 44934)

Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_outdoor_void_background.mat using Guid(57539333774454a4b9b68a2662c07ad5) (NativeFormatImporter) -> (artifact id: '99c018d2123986061894ad403d77f6fe') in 0.0028585 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.0019751 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_outdoor_void_background.mat using Guid(0ec66c653876c7244a13d4f32da1526a) (NativeFormatImporter) -> (artifact id: 'e1db2a1427d1c97a066681d67848d63a') in 0.0018802 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(efe4ace4ebe298d4bada9fe5c2546e89) (NativeFormatImporter) -> (artifact id: '84254ef753b242d39fea27ca378b0b03') in 0.0019472 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0019273 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.asset using Guid(5aa393eea6e946245b2e701795e51af0) (NativeFormatImporter) -> (artifact id: '184b95cb35aec2eba012f0ac3b8c8e5a') in 0.0021196 seconds
Refreshing native plugins compatible for Editor in 0.51 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=2611ed5fc724cd9469b095eb0564b1ed): Total: 0.057 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(efe4ace4ebe298d4bada9fe5c2546e89) (NativeFormatImporter) -> (artifact id: 'a69f20e4e842686af22fc83b45a76019') in 0.0024859 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=eab106f0fb6a82c49813e385ebfe0141): Total: 0.041 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(0a33e84f626797a40b8d5687454e6620) (NativeFormatImporter) -> (artifact id: '2dfb0ee97db515b87db3e427f999c2a7') in 0.0025127 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.asset using Guid(b43c1d26d8af7144eac532f733d613ed) (NativeFormatImporter) -> (artifact id: '72b93ea802d61590522168319e686932') in 0.0026058 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=4b538d74c2876eb438a633994424700d): Total: 0.047 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(0a33e84f626797a40b8d5687454e6620) (NativeFormatImporter) -> (artifact id: '1811c45b24cb5a42b0d119f903087e3b') in 0.002841 seconds
Refreshing native plugins compatible for Editor in 0.54 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=d48ec8c048c705f4f932cc789fdf35c8): Total: 0.044 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.mat using Guid(ba3c455e34bfdef488b2dbb1bdd0cd70) (NativeFormatImporter) -> (artifact id: 'dec47bc9f235a292b061ba7705817a0e') in 0.0029043 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.asset using Guid(6b757f77ea5d7164e8537193c67506bd) (NativeFormatImporter) -> (artifact id: 'edb251d8a3d8ffd6a09591c3a454c8a2') in 0.0028581 seconds
Refreshing native plugins compatible for Editor in 0.68 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=44f099baf128dcd479a51b6712d8b649): Total: 0.050 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.mat using Guid(ba3c455e34bfdef488b2dbb1bdd0cd70) (NativeFormatImporter) -> (artifact id: '1a5dfe9209479e4411a9a5728bc36da8') in 0.0029263 seconds
Refreshing native plugins compatible for Editor in 0.56 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=eb025df7e516e2b4e9a73577f27cb9bc): Total: 0.045 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.mat using Guid(b09502d957a9b5141836a6b6ab31797f) (NativeFormatImporter) -> (artifact id: '3e5b99ed69b07b0978cb63a29b9cb0d4') in 0.0026424 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.asset using Guid(2bb9710db4a50874c944ff2cece89232) (NativeFormatImporter) -> (artifact id: '91e098c066551ce9c7048c5e7b3cb0d8') in 0.0025713 seconds
Refreshing native plugins compatible for Editor in 0.52 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=80acc758d745f04409eced44192f1549): Total: 0.044 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.mat using Guid(b09502d957a9b5141836a6b6ab31797f) (NativeFormatImporter) -> (artifact id: 'df71a74559f1cd0ef87c4679dd6814a6') in 0.0024519 seconds
Refreshing native plugins compatible for Editor in 0.53 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=464e213b2ca823f44acfbae02e209733): Total: 0.039 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_occlusion_gradient.mat using Guid(d58f2dd58f72df641b0b13d70b9aaffb) (NativeFormatImporter) -> (artifact id: 'cce592b71f40c96c4099f5295e653a71') in 0.0025563 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_occlusion_gradient_soft.asset using Guid(c77c6612505c47d4aa1dac2b81f7897e) (NativeFormatImporter) -> (artifact id: '0a0b3058b133df474de29a6690dee7ed') in 0.0029804 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=e6cf2e255a6ffaa45a94aeb9c43de1a8): Total: 0.031 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
InvalidOperationException: House slice validation failed: Current_CentralPlaza_LibraryBackwardVolume_BackRoofPlaneA must be shadow-safe.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateCentralPlazaLibraryBackwardVolumeObject (System.String objectName, System.String expectedMaterialToken, System.String expectedParentName, System.String expectedLandmarkIdPrefix, UnityEngine.Vector3 minLocalPosition, UnityEngine.Vector3 maxLocalPosition, System.Single maxScaleX, System.Single maxScaleY, System.Single maxScaleZ) [0x0005c] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:28303 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dEightySeventhCyclePlazaLibraryBackwardVolume () [0x0002c] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:26650 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x002f7] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:487 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:625 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 26650)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[23:44:00] Phase 'build' FAILED with exit 1
[23:44:00] NoRollback set; preserving worktree after build failure
```

## Cycle 127 failure (validate) -- 20260524-234723

```
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_left_sprite.mat using Guid(5590a3bdf193f5747855c04828f83adc) (NativeFormatImporter) -> (artifact id: 'e7c8a9a348e94b0284f8b47f7ac80cb7') in 0.0018031 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite.mat using Guid(56b5ac4afa850e94fa26bf722f31b14b) (NativeFormatImporter) -> (artifact id: 'f7e017ad6bce709be21936a4bb8efe19') in 0.0017467 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_depth_shadow.mat using Guid(07afc9c1f7f8ab4439760cab6c8b9c1e) (NativeFormatImporter) -> (artifact id: '68a4feae9900f9534bb049459ffb3678') in 0.0018651 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_map_move_floor_glow.mat using Guid(373d809c8f785b3439635e59312211de) (NativeFormatImporter) -> (artifact id: '24bd8e64df90c660225053541b103ab0') in 0.0018525 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite.mat using Guid(57d1354643503484f8f9504dec7939e7) (NativeFormatImporter) -> (artifact id: '8d6144afb428b20ddb5b28b485258685') in 0.0018709 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite.mat using Guid(f79bd52245f424d42b668da63a26eed7) (NativeFormatImporter) -> (artifact id: '6c7634a35d7a5e593d016924efb11022') in 0.0019652 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite.mat using Guid(f71c1a4bbbfa65e4a9dc8ceb1dbaab05) (NativeFormatImporter) -> (artifact id: '067c3ca3644ebbf71c59456785c01e71') in 0.0017329 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite.mat using Guid(28f2cae61d95125439828020176a4cfc) (NativeFormatImporter) -> (artifact id: '72016ad2a7c404cc799f9f50c6febaf5') in 0.0017891 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite.mat using Guid(a8c94ff07ef65ea4f8ad0dedea407b1e) (NativeFormatImporter) -> (artifact id: '63c8344cf78a915d285cd3eae78cb1fd') in 0.0018138 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite.mat using Guid(b8841e13281e616468837ca346676037) (NativeFormatImporter) -> (artifact id: '1adcf1ea1109f3defbf059ddda5acb22') in 0.0018234 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite.mat using Guid(3acdb3d0d51b28d40854136863dba5b3) (NativeFormatImporter) -> (artifact id: '085d72f1844e837c1fa998fa9467b7c2') in 0.0018351 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.0019303 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce.mat using Guid(0bc31eecccdf1a840a5477d771512775) (NativeFormatImporter) -> (artifact id: '957df3d5816f00a64823db316222cbe2') in 0.0018929 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool.mat using Guid(0dd9370c56289ae4a8d0e1dddfc59477) (NativeFormatImporter) -> (artifact id: 'cfd906379f8a62ffaf4c7136665bf0f5') in 0.002025 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_route_move_floor_glow.mat using Guid(0dbdea9defe689a4ca05680804b173ca) (NativeFormatImporter) -> (artifact id: '26e11471e027e487c6b513b0683a06d9') in 0.0018362 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_back_sprite.mat using Guid(8d4799b24d7badb4d9d50d8f3546587c) (NativeFormatImporter) -> (artifact id: 'ea1cc68dacc196e87140893dcea1fad6') in 0.0017784 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.mat using Guid(0e535f48a85f05b43ad8f1b5cb2ecb4a) (NativeFormatImporter) -> (artifact id: '95a63720d5377afca26fb765e42e17fd') in 0.0016487 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_contact_shadow.asset using Guid(40c9d8116f275fd4c98bbdde1e793a32) (NativeFormatImporter) -> (artifact id: '17d86434566b3eaf148ab06195b0eaff') in 0.0019264 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite_shaded.asset using Guid(11367ee44066a2c4a933e1a65a54fa71) (NativeFormatImporter) -> (artifact id: 'b4ea198999a098bff466fc482aa2e085') in 0.0026362 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite_shaded.asset using Guid(31848d2268cea5d4a93190a2365133ca) (NativeFormatImporter) -> (artifact id: 'dfcf40487733ba25c6e6bad9c26fa7e7') in 0.0023595 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_library_window_light_cookie_soft.asset using Guid(e10a87518f970b6498f120403b5e87fa) (NativeFormatImporter) -> (artifact id: 'b6624ac7fb17375ff687066ce4c5208b') in 0.0021291 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_left_sprite_shaded.asset using Guid(f13736bef9726b045ba18071b949e2d2) (NativeFormatImporter) -> (artifact id: 'f6e48f8240644c12833b02b6b33897d1') in 0.0023315 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0021832 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce_soft.asset using Guid(0442e2f80b7dbc34e99a112ae1095f20) (NativeFormatImporter) -> (artifact id: 'a65e2dbc19e50b0fc0fa4598f134ffd8') in 0.0020352 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite_shaded.asset using Guid(246d9d1ed78f72147aa1f537daf9164e) (NativeFormatImporter) -> (artifact id: '59a4f02825ed43e073fd3814395235f5') in 0.0024805 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.asset using Guid(64767a48f06c7a4439175ba9f3f76969) (NativeFormatImporter) -> (artifact id: '2c34ef17951f1b28aef444ba8ef4c761') in 0.0019595 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_back_sprite_shaded.asset using Guid(646afdaa35aa1c84bbfca6d10ad8d956) (NativeFormatImporter) -> (artifact id: '71d57c8ff1c974a8f2d165c436f15504') in 0.0021559 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool_soft.asset using Guid(97afe9889f5e56f449c8468a1d0bfdf9) (NativeFormatImporter) -> (artifact id: 'b007a80dd3775e821df40936b5d7e159') in 0.0020145 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_back_sprite_shaded.asset using Guid(383cf440301bf504a9b6c2a8a85c5c10) (NativeFormatImporter) -> (artifact id: '9a14722ac27a7c483b3d93443ba97166') in 0.0024309 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite_shaded.asset using Guid(e8b91a1d7f94f9d419ae3d2cb8aca69c) (NativeFormatImporter) -> (artifact id: '19f48d248392446507eb0aa3bd573d8d') in 0.002785 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_left_sprite_shaded.asset using Guid(29c76ef0ee79e294abc159d852e30d6f) (NativeFormatImporter) -> (artifact id: '7a1e2d14c3f7f57972b5a415bb755af8') in 0.0022719 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite_shaded.asset using Guid(8af717035fe354841a2778b67ef47f60) (NativeFormatImporter) -> (artifact id: '636319e0046c8cb6ef7b2f1e0d8a2fbf') in 0.002381 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite_shaded.asset using Guid(bcf85f4b6024bd345a6725cd7e3f8122) (NativeFormatImporter) -> (artifact id: '1134ffa9b5a09269a7a99f311dd08502') in 0.002391 seconds
Start importing Assets/Settings/DefaultVolumeProfile.asset using Guid(2dc156ea366288a46859f93b6d3c2cd0) (NativeFormatImporter) -> (artifact id: '25538e5d27d1bd3c782afb4302706fbd') in 0.0018039 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_right_sprite_shaded.asset using Guid(7edaa2e8ce6f3f040896d2d92f57e792) (NativeFormatImporter) -> (artifact id: '4e696edc4775714bf55149c9029e9a87') in 0.0023509 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite_shaded.asset using Guid(8e8558fedae4d5441a0880d3e0ae5dde) (NativeFormatImporter) -> (artifact id: '7a36765b1626f751d5a96e3827f13ea3') in 0.0027091 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite_shaded.asset using Guid(afafc97e4daeeb2408f5830dde7c5f7c) (NativeFormatImporter) -> (artifact id: 'abb16328271b3f261d7f87df395bbbcc') in 0.0023026 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_front_sprite_shaded.asset using Guid(cfe6c4e4efe6c2d4bae69a5a1cb896ad) (NativeFormatImporter) -> (artifact id: 'a1fc470689ab7b251db5843fc102d4a4') in 0.0022346 seconds
Refreshing native plugins compatible for Editor in 0.54 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=1362f1e5a999f444f919da6c22fbd601): Total: 0.176 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Scenes/Anemora_FastVS_HouseSlice.unity using Guid(5c2c1333e7d65ba4eac9369f107a6118) (DefaultImporter) -> (artifact id: '96d5a76ef99f256d714dc52a58f42e61') in 0.0011947 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=c72171e9bcbc27945865b3cfb52b2c97): Total: 0.047 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Asset Pipeline Refresh (id=a0e14ff28bff6d346b6677e1998d0fa7): Total: 0.027 seconds - Initiated by RefreshV2(NoUpdateAssetOptions)
Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:337)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaRealtimeLightShadowCycle127Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:46659)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 337)

Opening scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
Unloading 0 Unused Serialized files (Serialized files now loaded: 0)
Loaded scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
	Deserialize:            246.566 ms
	Integration:            280.486 ms
	Integration of assets:  2.040 ms
	Thread Wait Time:       -1.892 ms
	Total Operation Time:   527.199 ms
Unloading 617 unused Assets / (10.7 MB). Loaded Objects now: 25100.
Memory consumption went from 238.6 MB to 227.9 MB.
Total: 12.047900 ms (FindLiveObjects: 1.963100 ms CreateObjectMapping: 0.391800 ms MarkObjects: 7.915500 ms  DeleteObjects: 1.776300 ms)

InvalidOperationException: House slice validation failed: cycle 127 needs at least 24 central-plaza realtime shadow receivers, found 1.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaRealtimeLightShadowCycle127 () [0x003ca] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:36224 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeLightShadowCycle127Batch () [0x00011] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:46661 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 36224)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeLightShadowCycle127Batch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[23:48:08] Phase 'validate' FAILED with exit 1
[23:48:08] NoRollback set; preserving worktree after validate failure
```

## Cycle 127 failure (build) -- 20260524-235136

```
Refreshing native plugins compatible for Editor in 0.57 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=972269e360d00bc4eb91b649d2652a32): Total: 0.024 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.003364 seconds
Refreshing native plugins compatible for Editor in 0.56 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=693ce41b3a095804893d0569fc845456): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.003778 seconds
Refreshing native plugins compatible for Editor in 0.56 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=a0f19835941a31f40b82bf71d717a3ca): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0038444 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=27e65632600e7ba4ba42f1250dff431c): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
HD2D material role audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dMaterialRoleFoundationAudit:VerifyMaterialRolesV1 () (at Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs:34)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:367)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs Line: 34)

HD2D sprite card lighting audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSpriteCardLightingAudit:VerifySpriteCardLightingV1 () (at Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs:98)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:368)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs Line: 98)

Shading Foundation v1 audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit:VerifyShadingFoundationV1 () (at Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs:38)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:369)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 38)

HD2D area lighting profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dAreaLightingProfileFoundationAudit:VerifyAreaLightingProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs:142)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:374)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs Line: 142)

InvalidOperationException: HD2D overlay profile audit failed:
- HD2D overlay profile Current_CentralPlaza_StaticDirectionalCastShadow_LibraryFacade must keep its MeshRenderer enabled.
- HD2D overlay profile Current_CentralPlaza_SurfaceDirectionalShade_LibraryFacade must keep its MeshRenderer enabled.
  at Anemora.EditorTools.AnemoraFastVsHd2dOverlayProfileFoundationAudit.VerifyOverlayProfilesV1 () [0x007b1] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs:403 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x000c7] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:375 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:625 

(Filename: Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs Line: 403)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[23:54:12] Phase 'build' FAILED with exit 1
[23:54:12] NoRollback set; preserving worktree after build failure
```

## Cycle 127 failure (validate) -- 20260524-234525

```
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_left_sprite.mat using Guid(5590a3bdf193f5747855c04828f83adc) (NativeFormatImporter) -> (artifact id: 'e7c8a9a348e94b0284f8b47f7ac80cb7') in 0.0019503 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite.mat using Guid(56b5ac4afa850e94fa26bf722f31b14b) (NativeFormatImporter) -> (artifact id: 'f7e017ad6bce709be21936a4bb8efe19') in 0.0018874 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_depth_shadow.mat using Guid(07afc9c1f7f8ab4439760cab6c8b9c1e) (NativeFormatImporter) -> (artifact id: '68a4feae9900f9534bb049459ffb3678') in 0.0026111 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_map_move_floor_glow.mat using Guid(373d809c8f785b3439635e59312211de) (NativeFormatImporter) -> (artifact id: '24bd8e64df90c660225053541b103ab0') in 0.0020638 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite.mat using Guid(57d1354643503484f8f9504dec7939e7) (NativeFormatImporter) -> (artifact id: '8d6144afb428b20ddb5b28b485258685') in 0.0020752 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite.mat using Guid(f79bd52245f424d42b668da63a26eed7) (NativeFormatImporter) -> (artifact id: '6c7634a35d7a5e593d016924efb11022') in 0.0017792 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite.mat using Guid(f71c1a4bbbfa65e4a9dc8ceb1dbaab05) (NativeFormatImporter) -> (artifact id: '067c3ca3644ebbf71c59456785c01e71') in 0.0018532 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite.mat using Guid(28f2cae61d95125439828020176a4cfc) (NativeFormatImporter) -> (artifact id: '72016ad2a7c404cc799f9f50c6febaf5') in 0.0018666 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite.mat using Guid(a8c94ff07ef65ea4f8ad0dedea407b1e) (NativeFormatImporter) -> (artifact id: '63c8344cf78a915d285cd3eae78cb1fd') in 0.0018725 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite.mat using Guid(b8841e13281e616468837ca346676037) (NativeFormatImporter) -> (artifact id: '1adcf1ea1109f3defbf059ddda5acb22') in 0.0020083 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite.mat using Guid(3acdb3d0d51b28d40854136863dba5b3) (NativeFormatImporter) -> (artifact id: '085d72f1844e837c1fa998fa9467b7c2') in 0.0020044 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.0018671 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce.mat using Guid(0bc31eecccdf1a840a5477d771512775) (NativeFormatImporter) -> (artifact id: '957df3d5816f00a64823db316222cbe2') in 0.002023 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool.mat using Guid(0dd9370c56289ae4a8d0e1dddfc59477) (NativeFormatImporter) -> (artifact id: 'cfd906379f8a62ffaf4c7136665bf0f5') in 0.0020323 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_route_move_floor_glow.mat using Guid(0dbdea9defe689a4ca05680804b173ca) (NativeFormatImporter) -> (artifact id: '26e11471e027e487c6b513b0683a06d9') in 0.0019187 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_back_sprite.mat using Guid(8d4799b24d7badb4d9d50d8f3546587c) (NativeFormatImporter) -> (artifact id: 'ea1cc68dacc196e87140893dcea1fad6') in 0.0020101 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.mat using Guid(0e535f48a85f05b43ad8f1b5cb2ecb4a) (NativeFormatImporter) -> (artifact id: '95a63720d5377afca26fb765e42e17fd') in 0.0020413 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_contact_shadow.asset using Guid(40c9d8116f275fd4c98bbdde1e793a32) (NativeFormatImporter) -> (artifact id: '17d86434566b3eaf148ab06195b0eaff') in 0.0019954 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite_shaded.asset using Guid(11367ee44066a2c4a933e1a65a54fa71) (NativeFormatImporter) -> (artifact id: 'b4ea198999a098bff466fc482aa2e085') in 0.0024668 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite_shaded.asset using Guid(31848d2268cea5d4a93190a2365133ca) (NativeFormatImporter) -> (artifact id: 'dfcf40487733ba25c6e6bad9c26fa7e7') in 0.0024389 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_library_window_light_cookie_soft.asset using Guid(e10a87518f970b6498f120403b5e87fa) (NativeFormatImporter) -> (artifact id: 'b6624ac7fb17375ff687066ce4c5208b') in 0.0020551 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_left_sprite_shaded.asset using Guid(f13736bef9726b045ba18071b949e2d2) (NativeFormatImporter) -> (artifact id: 'f6e48f8240644c12833b02b6b33897d1') in 0.0028485 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0019678 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce_soft.asset using Guid(0442e2f80b7dbc34e99a112ae1095f20) (NativeFormatImporter) -> (artifact id: 'a65e2dbc19e50b0fc0fa4598f134ffd8') in 0.0018666 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite_shaded.asset using Guid(246d9d1ed78f72147aa1f537daf9164e) (NativeFormatImporter) -> (artifact id: '59a4f02825ed43e073fd3814395235f5') in 0.0025553 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.asset using Guid(64767a48f06c7a4439175ba9f3f76969) (NativeFormatImporter) -> (artifact id: '2c34ef17951f1b28aef444ba8ef4c761') in 0.0019396 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_back_sprite_shaded.asset using Guid(646afdaa35aa1c84bbfca6d10ad8d956) (NativeFormatImporter) -> (artifact id: '71d57c8ff1c974a8f2d165c436f15504') in 0.002183 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool_soft.asset using Guid(97afe9889f5e56f449c8468a1d0bfdf9) (NativeFormatImporter) -> (artifact id: 'b007a80dd3775e821df40936b5d7e159') in 0.0027102 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_back_sprite_shaded.asset using Guid(383cf440301bf504a9b6c2a8a85c5c10) (NativeFormatImporter) -> (artifact id: '9a14722ac27a7c483b3d93443ba97166') in 0.0023411 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite_shaded.asset using Guid(e8b91a1d7f94f9d419ae3d2cb8aca69c) (NativeFormatImporter) -> (artifact id: '19f48d248392446507eb0aa3bd573d8d') in 0.0022689 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_left_sprite_shaded.asset using Guid(29c76ef0ee79e294abc159d852e30d6f) (NativeFormatImporter) -> (artifact id: '7a1e2d14c3f7f57972b5a415bb755af8') in 0.0021834 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite_shaded.asset using Guid(8af717035fe354841a2778b67ef47f60) (NativeFormatImporter) -> (artifact id: '636319e0046c8cb6ef7b2f1e0d8a2fbf') in 0.0023905 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite_shaded.asset using Guid(bcf85f4b6024bd345a6725cd7e3f8122) (NativeFormatImporter) -> (artifact id: '1134ffa9b5a09269a7a99f311dd08502') in 0.0023542 seconds
Start importing Assets/Settings/DefaultVolumeProfile.asset using Guid(2dc156ea366288a46859f93b6d3c2cd0) (NativeFormatImporter) -> (artifact id: '25538e5d27d1bd3c782afb4302706fbd') in 0.0018802 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_right_sprite_shaded.asset using Guid(7edaa2e8ce6f3f040896d2d92f57e792) (NativeFormatImporter) -> (artifact id: '4e696edc4775714bf55149c9029e9a87') in 0.0022351 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite_shaded.asset using Guid(8e8558fedae4d5441a0880d3e0ae5dde) (NativeFormatImporter) -> (artifact id: '7a36765b1626f751d5a96e3827f13ea3') in 0.0022078 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite_shaded.asset using Guid(afafc97e4daeeb2408f5830dde7c5f7c) (NativeFormatImporter) -> (artifact id: 'abb16328271b3f261d7f87df395bbbcc') in 0.0022229 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_front_sprite_shaded.asset using Guid(cfe6c4e4efe6c2d4bae69a5a1cb896ad) (NativeFormatImporter) -> (artifact id: 'a1fc470689ab7b251db5843fc102d4a4') in 0.0027152 seconds
Refreshing native plugins compatible for Editor in 0.59 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=cae606c0ccbc45a4fb27144a8d655348): Total: 0.178 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Scenes/Anemora_FastVS_HouseSlice.unity using Guid(5c2c1333e7d65ba4eac9369f107a6118) (DefaultImporter) -> (artifact id: 'd3a513c715d65e4343fe1db3448bbd74') in 0.0013251 seconds
Refreshing native plugins compatible for Editor in 0.57 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=86e6ad4169845344298789733930f8ae): Total: 0.047 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Asset Pipeline Refresh (id=98b8d255f8c3e0945bcf07343e3efb8b): Total: 0.029 seconds - Initiated by RefreshV2(NoUpdateAssetOptions)
Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:337)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidatePlazaRealtimeLightShadowCycle127Batch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:46659)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 337)

Opening scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
Unloading 0 Unused Serialized files (Serialized files now loaded: 0)
Loaded scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
	Deserialize:            250.647 ms
	Integration:            300.739 ms
	Integration of assets:  2.200 ms
	Thread Wait Time:       -2.007 ms
	Total Operation Time:   551.580 ms
Unloading 617 unused Assets / (6.5 MB). Loaded Objects now: 25100.
Memory consumption went from 257.6 MB to 251.1 MB.
Total: 12.572200 ms (FindLiveObjects: 2.351900 ms CreateObjectMapping: 0.496900 ms MarkObjects: 8.056400 ms  DeleteObjects: 1.665900 ms)

InvalidOperationException: House slice validation failed: cycle 127 needs at least 24 central-plaza realtime shadow receivers, found 1.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPlazaRealtimeLightShadowCycle127 () [0x003ca] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:36224 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeLightShadowCycle127Batch () [0x00011] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:46661 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 36224)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeLightShadowCycle127Batch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[23:54:14] Phase 'validate' FAILED with exit 1
[23:54:14] NoRollback set; preserving worktree after validate failure
```

## Cycle 127 failure (validate) -- 20260524-235634

```
[23:56:34] Cycle runner starting
[23:56:34]   CycleNumber    : 127
[23:56:34]   ProjectPath    : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work
[23:56:34]   BatchTool      : C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe
[23:56:34]   ValidateMethod : Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeLightShadowCycle127Batch
[23:56:34]   CaptureMethod  : Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaRealtimeLightShadowCycle127ScreenshotsBatch
[23:56:34]   BuildMethod    : Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch
[23:56:34]   Audience       : parent_review
[23:56:34]   CaptureOutDir  : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots
[23:56:34]   DevlogPath     : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-24_fast_vs_hd2d_plaza_realtime_light_shadow_cycle127.md
[23:56:34]   SmokeSeconds   : 20
[23:56:34]   SmokePatterns  : Error|Exception|Assert|NullReference|Font Atlas Texture|DrawObjectsPass|RenderGraph
[23:56:34]   CommitPath     : Assets/Art/Shaders/FastVS/FastVS_SpriteCardRampUnlit.shader; Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs; Assets/Editor/AnemoraFastVsHd2dVisualSnapshotAudit.cs; Assets/Editor/AnemoraFastVsHouseSliceSetup.cs; Assets/Scripts/FastVS/FastVsHouseLightingDirector.cs; Assets/Scripts/FastVS/FastVsRealtimeLightShadowRig.cs; Assets/Scripts/FastVS/FastVsRealtimeLightShadowRig.cs.meta; Assets/Scripts/FastVS/FastVsVisualDirectionGuide.cs; docs/devlog/2026-05-24_fast_vs_hd2d_plaza_realtime_light_shadow_cycle127.md; docs/devlog/INDEX.md; docs/devlog/screenshots/fast_vs_hd2d_cycle127_plaza_realtime_light_shadow_parent_review_20260524_01
[23:56:34]   NoRollback     : True
[23:56:34]   RunLog         : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\tools\logs\cycle-127-20260524-235634.log
[23:56:34] Phase 'validate' begin: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeLightShadowCycle127Batch

===== validate batch log (C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\cycle-127-20260524-235634-validate.log) =====
[Licensing::Module] Trying to connect to existing licensing client channel...
Built from '6000.3/staging' branch; Version is '6000.3.14f1 (d68c3f99a318) revision 14060607'; Using compiler version '194234433'; Build Type 'Release'
[Licensing::IpcConnector] Successfully connected to: "LicenseClient-maro6" at "2026-05-24T14:56:34.835526Z"
OS: 'Windows 11  (10.0.26200) Core' Language: 'en' Physical Memory: 14177 MB
BatchMode: 1, IsHumanControllingUs: 0, StartBugReporterOnCrash: 0, Is64bit: 1
System  architecture: x64
Process architecture: x64
Date: 2026-05-24T14:56:34Z

COMMAND LINE ARGUMENTS:
C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe
-batchmode
-quit
-projectPath
C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work
-executeMethod
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeLightShadowCycle127Batch
-logFile
C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\cycle-127-20260524-235634-validate.log
Successfully changed project path to: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work
C:/Users/maro6/Documents/Unity/Anemora-fast-vs-v24-hd2d-work
Exiting without the bug reporter. Application will terminate with return code 1
[23:56:36] Phase 'validate' FAILED with exit 1
[23:56:36] NoRollback set; preserving worktree after validate failure
```

## Cycle 127 failure (build) -- 20260524-235452

```
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=a72808492749c924f8bd8e45010dfaf2): Total: 0.043 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_central_plaza_outdoor_background_sky_depth_roofline.mat using Guid(6875275f142b09b4c8e5762b5f4903e2) (NativeFormatImporter) -> (artifact id: 'afba84a891277c23bb0c89f78c8a42b4') in 0.002338 seconds
Refreshing native plugins compatible for Editor in 0.51 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=b16909431bc83104faa89c736974758c): Total: 0.037 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_central_plaza_outdoor_background_sky_depth_far_edge_haze.mat using Guid(f47a4be054e80fb4c9c6c359194ffa59) (NativeFormatImporter) -> (artifact id: '153c59155c20e93b91eb3972abbb59c7') in 0.0025243 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_current_central_plaza_outdoor_background_sky_depth_far_edge_haze.asset using Guid(fed97cfe7c78d434aa63d39d88cef75d) (NativeFormatImporter) -> (artifact id: '7b4cdf7b994ba8ca28f20246a851eba6') in 0.0022464 seconds
Refreshing native plugins compatible for Editor in 0.48 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=20e7d318b9bc2074da0ec7c51020277e): Total: 0.041 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_central_plaza_outdoor_background_sky_depth_far_edge_haze.mat using Guid(f47a4be054e80fb4c9c6c359194ffa59) (NativeFormatImporter) -> (artifact id: '47fd6968bdcf5a8d626e386147548962') in 0.0024749 seconds
Refreshing native plugins compatible for Editor in 0.47 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=d50c320723dedce4b82598cd7c52e11d): Total: 0.035 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_occlusion_gradient.mat using Guid(d58f2dd58f72df641b0b13d70b9aaffb) (NativeFormatImporter) -> (artifact id: 'cce592b71f40c96c4099f5295e653a71') in 0.0026615 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_cool_light_pool.mat using Guid(3836918c03d307e4e83b9e69d1733deb) (NativeFormatImporter) -> (artifact id: 'ce8e0ecc851d07b782e1eb73d6b28144') in 0.0023794 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool.mat using Guid(0dd9370c56289ae4a8d0e1dddfc59477) (NativeFormatImporter) -> (artifact id: 'cfd906379f8a62ffaf4c7136665bf0f5') in 0.0025529 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_cool_light_pool_soft.asset using Guid(702ad339304e9f04b8081add64955e78) (NativeFormatImporter) -> (artifact id: '39eaedb2ba7aa5ff98b6524f90815707') in 0.0028871 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool_soft.asset using Guid(97afe9889f5e56f449c8468a1d0bfdf9) (NativeFormatImporter) -> (artifact id: 'b007a80dd3775e821df40936b5d7e159') in 0.0026116 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_occlusion_gradient_soft.asset using Guid(c77c6612505c47d4aa1dac2b81f7897e) (NativeFormatImporter) -> (artifact id: '0a0b3058b133df474de29a6690dee7ed') in 0.0027759 seconds
Refreshing native plugins compatible for Editor in 0.50 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=208ccd64abbcccb4c9166302a958f295): Total: 0.045 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_warm_stage_light.mat using Guid(7d448af81f20e3a43a8cdaf0e2a3080c) (NativeFormatImporter) -> (artifact id: 'a9c31eb73750a4c222f02ebdd15e48eb') in 0.002979 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_warm_stage_light_soft.asset using Guid(321ba12a26ebb484a8e286635fa65b40) (NativeFormatImporter) -> (artifact id: '32670b279f8ea3761d4cf06a5400270c') in 0.0029909 seconds
Refreshing native plugins compatible for Editor in 0.52 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=5361de8e39ef404468718836e2e61375): Total: 0.032 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_central_plaza_outdoor_void_background_backdrop_foundation_horizon_roofline.mat using Guid(b11bd83b83d6ac642a988b7acda08b1c) (NativeFormatImporter) -> (artifact id: '0f5a9822ba86d0cc5deb753531ad0f30') in 0.002533 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_cool_light_pool.mat using Guid(3836918c03d307e4e83b9e69d1733deb) (NativeFormatImporter) -> (artifact id: 'ce8e0ecc851d07b782e1eb73d6b28144') in 0.0022247 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_central_plaza_scenic_backdrop_sky_curtain.mat using Guid(58d92ac3da95bf44fbea925c4b234416) (NativeFormatImporter) -> (artifact id: '52881f67403b28c9ef41f63f8d915fe0') in 0.0019977 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_central_plaza_outdoor_void_background_backdrop_foundation_sky_back_plane.mat using Guid(3c38c93103bc1534897c20cc804d3a44) (NativeFormatImporter) -> (artifact id: 'd6bf7e9a87b736239dcbb4ed5a4e5eb0') in 0.0020489 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_cool_light_pool_soft.asset using Guid(702ad339304e9f04b8081add64955e78) (NativeFormatImporter) -> (artifact id: '39eaedb2ba7aa5ff98b6524f90815707') in 0.0022318 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_current_central_plaza_scenic_backdrop_sky_curtain.asset using Guid(8e442ac31a6ca7445ba0f907d8148b6b) (NativeFormatImporter) -> (artifact id: '03ef2c3ea54cc132d6b6746b3bf9026d') in 0.0024801 seconds
Refreshing native plugins compatible for Editor in 0.51 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=64ad047dd478de740b0f80a658a0e33c): Total: 0.058 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_central_plaza_scenic_backdrop_sky_curtain.mat using Guid(58d92ac3da95bf44fbea925c4b234416) (NativeFormatImporter) -> (artifact id: '0f30c195541fc92024df9997a064122e') in 0.0024868 seconds
Refreshing native plugins compatible for Editor in 0.50 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=2ecd1a999cb150d4ea3c46052fd180f9): Total: 0.037 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_central_plaza_scenic_backdrop_low_haze_band.mat using Guid(8cb179b743679c744a216a5f6308b32a) (NativeFormatImporter) -> (artifact id: 'fcb75a64fb00ba544b65e7da29d5b1a4') in 0.0024109 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_current_central_plaza_scenic_backdrop_low_haze_band.asset using Guid(66eadf25f2c48734c81609a8bae7c875) (NativeFormatImporter) -> (artifact id: 'ee0f449f768aa31d9738d8a158bd61bb') in 0.0030341 seconds
Refreshing native plugins compatible for Editor in 0.54 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=e2dce011a158b6842bb1aed1ed20265a): Total: 0.046 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_central_plaza_scenic_backdrop_low_haze_band.mat using Guid(8cb179b743679c744a216a5f6308b32a) (NativeFormatImporter) -> (artifact id: '4fd4205de886f155ab25fe06701550aa') in 0.0024757 seconds
Refreshing native plugins compatible for Editor in 0.51 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=a44037f7160965d4ba2e03f84c61ce26): Total: 0.040 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_central_plaza_scenic_backdrop_distant_roofline.mat using Guid(5dfb53f1ea8b80041bbe648b323cd807) (NativeFormatImporter) -> (artifact id: 'a572d025d7061e4ee2a697dd020431d3') in 0.0026038 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_current_central_plaza_scenic_backdrop_distant_roofline.asset using Guid(72cdfbf2a829986498833d11fef2d5b7) (NativeFormatImporter) -> (artifact id: 'f4e03917b8342959d27283fed3d93f0b') in 0.00297 seconds
Refreshing native plugins compatible for Editor in 0.57 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=a7054acf44aa4a34992cc6bf45b660a9): Total: 0.045 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_central_plaza_scenic_backdrop_distant_roofline.mat using Guid(5dfb53f1ea8b80041bbe648b323cd807) (NativeFormatImporter) -> (artifact id: 'eefa8c32360e6cab581b698f0d3f51d1') in 0.0024343 seconds
Refreshing native plugins compatible for Editor in 0.59 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=67c0a39b9ac14fa48a0b6aa1bd3b20c8): Total: 0.039 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_central_plaza_scenic_backdrop_left_sky_wrap.mat using Guid(ddee45e768185ae48954e1c9969616ee) (NativeFormatImporter) -> (artifact id: '1714c012d5d39314cd76c7353abf0ba0') in 0.0026 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_current_central_plaza_scenic_backdrop_left_sky_wrap.asset using Guid(0fb5f7d3e437b8b469220eb5219131d5) (NativeFormatImporter) -> (artifact id: '4c70cf0e129b58da74c28d41975bf1a9') in 0.0025516 seconds
Refreshing native plugins compatible for Editor in 0.49 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=2aab95d5f3ea4724bbaee436680e9ec1): Total: 0.043 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_central_plaza_scenic_backdrop_left_sky_wrap.mat using Guid(ddee45e768185ae48954e1c9969616ee) (NativeFormatImporter) -> (artifact id: 'a85ff38e5e6d210aefc622f11f32db3e') in 0.0027132 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=46f482b187a26c640abe26da286bc223): Total: 0.038 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_central_plaza_scenic_backdrop_right_sky_wrap.mat using Guid(cd5c2bf6df6639242ba7396a5c7a7c09) (NativeFormatImporter) -> (artifact id: '61bc82ce625db3145e5e8b18594f38de') in 0.0025028 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_current_central_plaza_scenic_backdrop_right_sky_wrap.asset using Guid(8692f925eb5e1a347976fa7b8dcd4dc0) (NativeFormatImporter) -> (artifact id: '567db4fbae3c56d88a74542a604a97be') in 0.0027631 seconds
Refreshing native plugins compatible for Editor in 0.52 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=2719002cd975c2a47b6f650805156790): Total: 0.045 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_central_plaza_scenic_backdrop_right_sky_wrap.mat using Guid(cd5c2bf6df6639242ba7396a5c7a7c09) (NativeFormatImporter) -> (artifact id: '8cadb78b179af2f80558d1385c3ee8a2') in 0.0027013 seconds
Refreshing native plugins compatible for Editor in 0.54 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=9dce97db8a470ad4db48c3af9c003d71): Total: 0.041 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
[23:58:53] Phase 'build' FAILED with exit -1
[23:58:53] NoRollback set; preserving worktree after build failure
```

## Cycle 127 failure (build) -- 20260524-235731

```

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 38)

HD2D area lighting profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dAreaLightingProfileFoundationAudit:VerifyAreaLightingProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs:142)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:374)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs Line: 142)

HD2D overlay profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dOverlayProfileFoundationAudit:VerifyOverlayProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs:402)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:375)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs Line: 402)

HD2D surface profile audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceProfileFoundationAudit:VerifySurfaceProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs:104)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:376)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs Line: 104)

HD2D surface texture metric audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceTextureMetricAudit:VerifySurfaceTextureMetricsV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs:72)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:377)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs Line: 72)

HD2D lighting transition audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dLightingTransitionAudit:VerifyLightingTransitionV1 () (at Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs:106)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:378)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs Line: 106)

InvalidOperationException: House slice validation failed: Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteLeft must keep its renderer enabled.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateOutdoorVoidBackgroundTreatmentObject (System.String objectName, System.String expectedParentName, System.String expectedMaterialToken, System.Single maxScaleX, System.Single maxScaleY, System.Single maxScaleZ) [0x00252] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:45608 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dFiftyEighthCycleOutdoorVoidBackgroundTreatment () [0x0015e] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:31425 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x0012b] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:395 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:625 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 45608)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[00:00:02] Phase 'build' FAILED with exit 1
[00:00:02] NoRollback set; preserving worktree after build failure
```

## Cycle 127 failure (build) -- 20260525-000102

```
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dAreaLightingProfileFoundationAudit:VerifyAreaLightingProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs:142)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:374)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs Line: 142)

HD2D overlay profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dOverlayProfileFoundationAudit:VerifyOverlayProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs:402)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:375)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs Line: 402)

HD2D surface profile audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceProfileFoundationAudit:VerifySurfaceProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs:104)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:376)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs Line: 104)

HD2D surface texture metric audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceTextureMetricAudit:VerifySurfaceTextureMetricsV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs:72)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:377)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs Line: 72)

HD2D lighting transition audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dLightingTransitionAudit:VerifyLightingTransitionV1 () (at Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs:106)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:378)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs Line: 106)

Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.0029721 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=47b947bce2dcf0947831c7c038e31770): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.0026678 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=35432e3ca5c969a449a4dc9412bf9504): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
InvalidOperationException: House slice validation failed: Current_CentralPlaza_Cycle103_DappledGroundShadow_LibraryApproachA must have an enabled MeshRenderer with a material.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateCycle103DappledGroundShadowObject (System.String objectName, System.String expectedParentName, Anemora.FastVS.FastVsHouseArea expectedArea, System.Boolean currentWorld, UnityEngine.Vector3 minLocalPosition, UnityEngine.Vector3 maxLocalPosition, UnityEngine.Vector3 minLocalScale, UnityEngine.Vector3 maxLocalScale, System.Single expectedYawDegrees) [0x00515] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:32957 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dCycle103CinematicDappledGroundShadows () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:32963 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x00220] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:444 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:625 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 32963)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[00:03:44] Phase 'build' FAILED with exit 1
[00:03:44] NoRollback set; preserving worktree after build failure
```

## Cycle 127 failure (validate) -- 20260525-000104

```
[00:01:04] Cycle runner starting
[00:01:04]   CycleNumber    : 127
[00:01:04]   ProjectPath    : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work
[00:01:04]   BatchTool      : C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe
[00:01:04]   ValidateMethod : Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeLightShadowCycle127Batch
[00:01:04]   CaptureMethod  : Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaRealtimeLightShadowCycle127ScreenshotsBatch
[00:01:04]   BuildMethod    : Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch
[00:01:04]   Audience       : parent_review
[00:01:04]   CaptureOutDir  : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots
[00:01:04]   DevlogPath     : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-24_fast_vs_hd2d_plaza_realtime_light_shadow_cycle127.md
[00:01:04]   SmokeSeconds   : 20
[00:01:04]   SmokePatterns  : Error|Exception|Assert|NullReference|Font Atlas Texture|DrawObjectsPass|RenderGraph
[00:01:04]   CommitPath     : Assets/Art/Shaders/FastVS/FastVS_SpriteCardRampUnlit.shader; Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs; Assets/Editor/AnemoraFastVsHd2dVisualSnapshotAudit.cs; Assets/Editor/AnemoraFastVsHouseSliceSetup.cs; Assets/Scripts/FastVS/FastVsHouseLightingDirector.cs; Assets/Scripts/FastVS/FastVsRealtimeLightShadowRig.cs; Assets/Scripts/FastVS/FastVsRealtimeLightShadowRig.cs.meta; Assets/Scripts/FastVS/FastVsVisualDirectionGuide.cs; docs/devlog/2026-05-24_fast_vs_hd2d_plaza_realtime_light_shadow_cycle127.md; docs/devlog/INDEX.md; docs/devlog/screenshots/fast_vs_hd2d_cycle127_plaza_realtime_light_shadow_parent_review_20260524_01
[00:01:04]   NoRollback     : True
[00:01:04]   RunLog         : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\tools\logs\cycle-127-20260525-000104.log
[00:01:04] Phase 'validate' begin: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeLightShadowCycle127Batch

===== validate batch log (C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\cycle-127-20260525-000104-validate.log) =====
[Licensing::Module] Trying to connect to existing licensing client channel...
Built from '6000.3/staging' branch; Version is '6000.3.14f1 (d68c3f99a318) revision 14060607'; Using compiler version '194234433'; Build Type 'Release'
OS: 'Windows 11  (10.0.26200) Core' Language: 'en' Physical Memory: 14177 MB
[Licensing::IpcConnector] Successfully connected to: "LicenseClient-maro6" at "2026-05-24T15:01:04.6132182Z"
BatchMode: 1, IsHumanControllingUs: 0, StartBugReporterOnCrash: 0, Is64bit: 1
System  architecture: x64
Process architecture: x64
Date: 2026-05-24T15:01:04Z

COMMAND LINE ARGUMENTS:
C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe
-batchmode
-quit
-projectPath
C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work
-executeMethod
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeLightShadowCycle127Batch
-logFile
C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\cycle-127-20260525-000104-validate.log
Successfully changed project path to: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work
C:/Users/maro6/Documents/Unity/Anemora-fast-vs-v24-hd2d-work
Exiting without the bug reporter. Application will terminate with return code 1
[00:03:45] Phase 'validate' FAILED with exit 1
[00:03:45] NoRollback set; preserving worktree after validate failure
```

## Cycle 127 failure (build) -- 20260525-000602

```

Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.003067 seconds
Refreshing native plugins compatible for Editor in 0.59 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=ea3404fa3cf4d3a4bb7a22171db40158): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.0028322 seconds
Refreshing native plugins compatible for Editor in 0.66 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=40aea5638b5aea04fb3c9e5979d9b6da): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Fast VS house exterior eave shadow softening validation passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateFastVsHd2dShadowFoundationCycle87HouseExteriorEaveShadowSoftening () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:44934)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:460)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 44934)

Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_outdoor_void_background.mat using Guid(57539333774454a4b9b68a2662c07ad5) (NativeFormatImporter) -> (artifact id: '99c018d2123986061894ad403d77f6fe') in 0.0028042 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.001999 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_outdoor_void_background.mat using Guid(0ec66c653876c7244a13d4f32da1526a) (NativeFormatImporter) -> (artifact id: 'e1db2a1427d1c97a066681d67848d63a') in 0.0019108 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(efe4ace4ebe298d4bada9fe5c2546e89) (NativeFormatImporter) -> (artifact id: '84254ef753b242d39fea27ca378b0b03') in 0.0019864 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0021516 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.asset using Guid(5aa393eea6e946245b2e701795e51af0) (NativeFormatImporter) -> (artifact id: '184b95cb35aec2eba012f0ac3b8c8e5a') in 0.0023217 seconds
Refreshing native plugins compatible for Editor in 0.51 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=06eef2e39e953c844ac07b2d6246ccab): Total: 0.059 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(efe4ace4ebe298d4bada9fe5c2546e89) (NativeFormatImporter) -> (artifact id: 'a69f20e4e842686af22fc83b45a76019') in 0.0027358 seconds
Refreshing native plugins compatible for Editor in 0.53 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=2685ad206ffac1e44a8ac379da1b0b16): Total: 0.039 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(0a33e84f626797a40b8d5687454e6620) (NativeFormatImporter) -> (artifact id: '2dfb0ee97db515b87db3e427f999c2a7') in 0.0026324 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.asset using Guid(b43c1d26d8af7144eac532f733d613ed) (NativeFormatImporter) -> (artifact id: '72b93ea802d61590522168319e686932') in 0.0025512 seconds
Refreshing native plugins compatible for Editor in 0.53 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=a66c794cda403d843abd6bdd0056b088): Total: 0.044 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(0a33e84f626797a40b8d5687454e6620) (NativeFormatImporter) -> (artifact id: '1811c45b24cb5a42b0d119f903087e3b') in 0.0027515 seconds
Refreshing native plugins compatible for Editor in 0.60 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=20bd1c1c8281ae04e823d4860159f7b6): Total: 0.042 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.mat using Guid(ba3c455e34bfdef488b2dbb1bdd0cd70) (NativeFormatImporter) -> (artifact id: 'dec47bc9f235a292b061ba7705817a0e') in 0.0026207 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.asset using Guid(6b757f77ea5d7164e8537193c67506bd) (NativeFormatImporter) -> (artifact id: 'edb251d8a3d8ffd6a09591c3a454c8a2') in 0.0029042 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=6b4af5ea04942fb4cb9a5a3fa85cd9f6): Total: 0.044 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.mat using Guid(ba3c455e34bfdef488b2dbb1bdd0cd70) (NativeFormatImporter) -> (artifact id: '1a5dfe9209479e4411a9a5728bc36da8') in 0.0025219 seconds
Refreshing native plugins compatible for Editor in 0.53 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=e3b87a9cf331bb84ebc25dde5388df44): Total: 0.038 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.mat using Guid(b09502d957a9b5141836a6b6ab31797f) (NativeFormatImporter) -> (artifact id: '3e5b99ed69b07b0978cb63a29b9cb0d4') in 0.0025675 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.asset using Guid(2bb9710db4a50874c944ff2cece89232) (NativeFormatImporter) -> (artifact id: '91e098c066551ce9c7048c5e7b3cb0d8') in 0.0026717 seconds
Refreshing native plugins compatible for Editor in 0.60 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=810fb6edba714674097ddb478a972286): Total: 0.045 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.mat using Guid(b09502d957a9b5141836a6b6ab31797f) (NativeFormatImporter) -> (artifact id: 'df71a74559f1cd0ef87c4679dd6814a6') in 0.002607 seconds
Refreshing native plugins compatible for Editor in 0.52 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=5d90a77ffb23b964d8ff1799e0d3d2f0): Total: 0.039 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_occlusion_gradient.mat using Guid(d58f2dd58f72df641b0b13d70b9aaffb) (NativeFormatImporter) -> (artifact id: 'cce592b71f40c96c4099f5295e653a71') in 0.0025159 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_occlusion_gradient_soft.asset using Guid(c77c6612505c47d4aa1dac2b81f7897e) (NativeFormatImporter) -> (artifact id: '0a0b3058b133df474de29a6690dee7ed') in 0.0024212 seconds
Refreshing native plugins compatible for Editor in 0.52 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=d4879e9aeaa091a4bb135131c5b6dae2): Total: 0.030 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
InvalidOperationException: House slice validation failed: central plaza outdoor sky clear color expected RGBA(0.196, 0.158, 0.104, 1.000) but was RGBA(0.620, 0.580, 0.470, 1.000).
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateColorApproximately (UnityEngine.Color actual, UnityEngine.Color expected, System.String label) [0x00064] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:27592 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateOutdoorSkyClearColorForReview () [0x00098] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:27579 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dEightyEighthCycleOutdoorSkyBackdrop () [0x0003c] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:26676 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x002fc] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:488 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:625 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 27579)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[00:08:48] Phase 'build' FAILED with exit 1
[00:08:48] NoRollback set; preserving worktree after build failure
```

## Cycle 127 failure (build) -- 20260525-000939

```

Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.00295 seconds
Refreshing native plugins compatible for Editor in 0.60 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=df8a67fa42e3b8f46b9ae113dd76dacf): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.0028821 seconds
Refreshing native plugins compatible for Editor in 0.57 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=3e7be7178323b2549953098371a4e678): Total: 0.024 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Fast VS house exterior eave shadow softening validation passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateFastVsHd2dShadowFoundationCycle87HouseExteriorEaveShadowSoftening () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:44934)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:460)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 44934)

Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_outdoor_void_background.mat using Guid(57539333774454a4b9b68a2662c07ad5) (NativeFormatImporter) -> (artifact id: '99c018d2123986061894ad403d77f6fe') in 0.0029089 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.002099 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_outdoor_void_background.mat using Guid(0ec66c653876c7244a13d4f32da1526a) (NativeFormatImporter) -> (artifact id: 'e1db2a1427d1c97a066681d67848d63a') in 0.0021375 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(efe4ace4ebe298d4bada9fe5c2546e89) (NativeFormatImporter) -> (artifact id: '84254ef753b242d39fea27ca378b0b03') in 0.0020695 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0021247 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.asset using Guid(5aa393eea6e946245b2e701795e51af0) (NativeFormatImporter) -> (artifact id: '184b95cb35aec2eba012f0ac3b8c8e5a') in 0.0020933 seconds
Refreshing native plugins compatible for Editor in 0.56 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=f28f30e4746be724b96150f4dbd386b9): Total: 0.058 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(efe4ace4ebe298d4bada9fe5c2546e89) (NativeFormatImporter) -> (artifact id: 'a69f20e4e842686af22fc83b45a76019') in 0.0026263 seconds
Refreshing native plugins compatible for Editor in 0.60 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=88009540361f3f0449fcf86f1ec2f284): Total: 0.040 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(0a33e84f626797a40b8d5687454e6620) (NativeFormatImporter) -> (artifact id: '2dfb0ee97db515b87db3e427f999c2a7') in 0.0026334 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.asset using Guid(b43c1d26d8af7144eac532f733d613ed) (NativeFormatImporter) -> (artifact id: '72b93ea802d61590522168319e686932') in 0.0023324 seconds
Refreshing native plugins compatible for Editor in 0.71 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=44a1c0302dc66a34fa2b470e921274dd): Total: 0.049 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(0a33e84f626797a40b8d5687454e6620) (NativeFormatImporter) -> (artifact id: '1811c45b24cb5a42b0d119f903087e3b') in 0.0025396 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=795ce1dcb04afaf48aff375ffabe7981): Total: 0.042 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.mat using Guid(ba3c455e34bfdef488b2dbb1bdd0cd70) (NativeFormatImporter) -> (artifact id: 'dec47bc9f235a292b061ba7705817a0e') in 0.0025809 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.asset using Guid(6b757f77ea5d7164e8537193c67506bd) (NativeFormatImporter) -> (artifact id: 'edb251d8a3d8ffd6a09591c3a454c8a2') in 0.0028983 seconds
Refreshing native plugins compatible for Editor in 0.53 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=30f723ebf15427543aac4d516d91cbc0): Total: 0.046 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.mat using Guid(ba3c455e34bfdef488b2dbb1bdd0cd70) (NativeFormatImporter) -> (artifact id: '1a5dfe9209479e4411a9a5728bc36da8') in 0.0025533 seconds
Refreshing native plugins compatible for Editor in 0.62 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=5b98e12026d91ea47bf777a9064e3ef2): Total: 0.040 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.mat using Guid(b09502d957a9b5141836a6b6ab31797f) (NativeFormatImporter) -> (artifact id: '3e5b99ed69b07b0978cb63a29b9cb0d4') in 0.0028282 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.asset using Guid(2bb9710db4a50874c944ff2cece89232) (NativeFormatImporter) -> (artifact id: '91e098c066551ce9c7048c5e7b3cb0d8') in 0.0025989 seconds
Refreshing native plugins compatible for Editor in 0.51 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=02373c93952941c40b9d98375c306e59): Total: 0.045 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.mat using Guid(b09502d957a9b5141836a6b6ab31797f) (NativeFormatImporter) -> (artifact id: 'df71a74559f1cd0ef87c4679dd6814a6') in 0.0027145 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=d555f5c4b78b5fa45ade6c80dfb036c7): Total: 0.039 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_occlusion_gradient.mat using Guid(d58f2dd58f72df641b0b13d70b9aaffb) (NativeFormatImporter) -> (artifact id: 'cce592b71f40c96c4099f5295e653a71') in 0.0027217 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_occlusion_gradient_soft.asset using Guid(c77c6612505c47d4aa1dac2b81f7897e) (NativeFormatImporter) -> (artifact id: '0a0b3058b133df474de29a6690dee7ed') in 0.0027496 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=bf67f65803cda48498f4d134a4c80887): Total: 0.031 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
InvalidOperationException: House slice validation failed: Current_CentralPlaza_Cycle62_OuterGroundSkirt_NorthLowStreetContinuationA must be shadow-safe.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateCentralPlazaLibraryDeepExteriorVolumeObject (System.String objectName, System.String expectedMaterialToken, System.String expectedParentName, System.String expectedLandmarkIdPrefix, UnityEngine.Vector3 minLocalPosition, UnityEngine.Vector3 maxLocalPosition, System.Single maxScaleX, System.Single maxScaleY, System.Single maxScaleZ) [0x0005c] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:27786 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.<ValidateFastVsHd2dCycle62CentralPlazaOuterGroundSkirt>g__Validate|887_0 (System.String objectName, System.String parentName, System.String materialToken, System.String expectedPrefix, UnityEngine.Vector3 minLocalPosition, UnityEngine.Vector3 maxLocalPosition, System.Single maxScaleX, System.Single maxScaleY, System.Single maxScaleZ) [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:28116 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dCycle62CentralPlazaOuterGroundSkirt () [0x00018] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:28128 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x0039c] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:520 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:625 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 28128)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[00:12:26] Phase 'build' FAILED with exit 1
[00:12:26] NoRollback set; preserving worktree after build failure
```

## Cycle 127 failure (build) -- 20260525-001323

```

Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.0031042 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=7849c4d1ef8245b4895a82e6a897b753): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.002844 seconds
Refreshing native plugins compatible for Editor in 0.60 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=217956dae599bd94399faf5754c56edb): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Fast VS house exterior eave shadow softening validation passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateFastVsHd2dShadowFoundationCycle87HouseExteriorEaveShadowSoftening () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:44934)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:460)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:625)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 44934)

Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_outdoor_void_background.mat using Guid(57539333774454a4b9b68a2662c07ad5) (NativeFormatImporter) -> (artifact id: '99c018d2123986061894ad403d77f6fe') in 0.0033014 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.0030182 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_outdoor_void_background.mat using Guid(0ec66c653876c7244a13d4f32da1526a) (NativeFormatImporter) -> (artifact id: 'e1db2a1427d1c97a066681d67848d63a') in 0.0027775 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(efe4ace4ebe298d4bada9fe5c2546e89) (NativeFormatImporter) -> (artifact id: '84254ef753b242d39fea27ca378b0b03') in 0.0025841 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0026132 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.asset using Guid(5aa393eea6e946245b2e701795e51af0) (NativeFormatImporter) -> (artifact id: '184b95cb35aec2eba012f0ac3b8c8e5a') in 0.0023731 seconds
Refreshing native plugins compatible for Editor in 0.71 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=920911ad69a8da34ab2cef1c365476a1): Total: 0.068 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(efe4ace4ebe298d4bada9fe5c2546e89) (NativeFormatImporter) -> (artifact id: 'a69f20e4e842686af22fc83b45a76019') in 0.0025003 seconds
Refreshing native plugins compatible for Editor in 0.51 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=d657bcd9021e58245b3fe671b5d0ae99): Total: 0.039 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(0a33e84f626797a40b8d5687454e6620) (NativeFormatImporter) -> (artifact id: '2dfb0ee97db515b87db3e427f999c2a7') in 0.0024815 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.asset using Guid(b43c1d26d8af7144eac532f733d613ed) (NativeFormatImporter) -> (artifact id: '72b93ea802d61590522168319e686932') in 0.0023067 seconds
Refreshing native plugins compatible for Editor in 0.54 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=93bfb66b32821cf4ab5531068acf1a74): Total: 0.046 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(0a33e84f626797a40b8d5687454e6620) (NativeFormatImporter) -> (artifact id: '1811c45b24cb5a42b0d119f903087e3b') in 0.003238 seconds
Refreshing native plugins compatible for Editor in 0.57 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=71fe3cc85c12f4b4c9bee1826bdddee4): Total: 0.040 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.mat using Guid(ba3c455e34bfdef488b2dbb1bdd0cd70) (NativeFormatImporter) -> (artifact id: 'dec47bc9f235a292b061ba7705817a0e') in 0.0030337 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.asset using Guid(6b757f77ea5d7164e8537193c67506bd) (NativeFormatImporter) -> (artifact id: 'edb251d8a3d8ffd6a09591c3a454c8a2') in 0.0031157 seconds
Refreshing native plugins compatible for Editor in 0.65 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=f3ad0b50d4474794a8bfd11632e6b7ce): Total: 0.049 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.mat using Guid(ba3c455e34bfdef488b2dbb1bdd0cd70) (NativeFormatImporter) -> (artifact id: '1a5dfe9209479e4411a9a5728bc36da8') in 0.0024985 seconds
Refreshing native plugins compatible for Editor in 0.64 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=aa7c7197d5efd9c44b118f29e6bd2b71): Total: 0.041 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.mat using Guid(b09502d957a9b5141836a6b6ab31797f) (NativeFormatImporter) -> (artifact id: '3e5b99ed69b07b0978cb63a29b9cb0d4') in 0.0032444 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.asset using Guid(2bb9710db4a50874c944ff2cece89232) (NativeFormatImporter) -> (artifact id: '91e098c066551ce9c7048c5e7b3cb0d8') in 0.0027093 seconds
Refreshing native plugins compatible for Editor in 0.52 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=37ce0729ef8497a46a21d2c70823b2df): Total: 0.048 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.mat using Guid(b09502d957a9b5141836a6b6ab31797f) (NativeFormatImporter) -> (artifact id: 'df71a74559f1cd0ef87c4679dd6814a6') in 0.0034743 seconds
Refreshing native plugins compatible for Editor in 0.51 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=d45324f8e871ff946b99e4846898eec8): Total: 0.043 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_occlusion_gradient.mat using Guid(d58f2dd58f72df641b0b13d70b9aaffb) (NativeFormatImporter) -> (artifact id: 'cce592b71f40c96c4099f5295e653a71') in 0.0025757 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_occlusion_gradient_soft.asset using Guid(c77c6612505c47d4aa1dac2b81f7897e) (NativeFormatImporter) -> (artifact id: '0a0b3058b133df474de29a6690dee7ed') in 0.0024511 seconds
Refreshing native plugins compatible for Editor in 0.54 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=94ede26e26a14d34e9ef5a4933a7916c): Total: 0.031 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
InvalidOperationException: House slice validation failed: Current_CentralPlaza_Cycle63_ScenicHorizonGrounding_BackNorthPathStripA must remain shadow-safe.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateCentralPlazaScenicHorizonGroundingObject (System.String objectName, System.String expectedParentName, System.String expectedMaterialToken, System.String expectedLandmarkIdPrefix, UnityEngine.Vector3 minLocalPosition, UnityEngine.Vector3 maxLocalPosition, UnityEngine.Vector3 minLocalScale, UnityEngine.Vector3 maxLocalScale) [0x000d8] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:27819 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.<ValidateFastVsHd2dCycle63CentralPlazaScenicHorizonGrounding>g__Validate|889_0 (System.String objectName, System.String parentName, System.String materialToken, System.String expectedPrefix, UnityEngine.Vector3 minLocalPosition, UnityEngine.Vector3 maxLocalPosition, UnityEngine.Vector3 minLocalScale, UnityEngine.Vector3 maxLocalScale) [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:28172 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dCycle63CentralPlazaScenicHorizonGrounding () [0x00018] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:28183 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x003a1] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:521 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:625 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 28183)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[00:16:13] Phase 'build' FAILED with exit 1
[00:16:13] NoRollback set; preserving worktree after build failure
```
