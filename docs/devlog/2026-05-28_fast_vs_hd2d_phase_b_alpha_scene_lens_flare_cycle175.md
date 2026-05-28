# feat(hd2d): add phase b alpha scene lens flare setup

Cycle 175 implements the second Phase B-alpha slice: the generated HouseSlice scene now attaches a Unity SRP lens flare component to the directional sun, creates a local procedural `LensFlareData_Sun.asset`, and keeps the default volume profile's ScreenSpaceLensFlare override active at the Phase B-alpha baseline values.

## Scope

- Added `Assets/Art/LensFlare/LensFlareData_Sun.asset` with six procedural lens flare elements.
- Added generated-scene wiring so `Directional Light` receives `LensFlareComponentSRP` and references `LensFlareData_Sun.asset`.
- Added ScreenSpaceLensFlare profile setup: intensity 0.4, low sample count, quarter resolution, warm tint.
- Kept URP data-driven and screen-space lens flare support flags enabled.
- Checked for local `m_VolumetricFogEnabled`; this URP asset/package does not expose that serialized property in the current project state.

## Validation Plan

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseBAlphaSceneLensFlareBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseBAlphaSceneLensFlareCycle175ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player with `-batchmode -nographics`, pattern count must be 0.

## Review Notes

- This cycle adds visible sun flare plumbing and captures the four SunCycle diagnostic views plus TimeWindow aperture.
- Full Phase B-alpha god rays are still a gap because local URP 17 package cache did not expose a compiled `VolumetricFog` override class; the next cycle should add a fallback visible-light shaft pass or proceed to Buto B-beta if Buto is available/imported.
- Build exe path for Tom: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- 起動時は `Builds/FastVS_HouseSlice/` フォルダごと起動。

## Cycle 175 failure (validate) -- 20260528-165656

```
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_contact_shadow.asset using Guid(40c9d8116f275fd4c98bbdde1e793a32) (NativeFormatImporter) -> (artifact id: '17d86434566b3eaf148ab06195b0eaff') in 0.0020231 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite_shaded.asset using Guid(11367ee44066a2c4a933e1a65a54fa71) (NativeFormatImporter) -> (artifact id: 'b4ea198999a098bff466fc482aa2e085') in 0.0026657 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite_shaded.asset using Guid(31848d2268cea5d4a93190a2365133ca) (NativeFormatImporter) -> (artifact id: 'dfcf40487733ba25c6e6bad9c26fa7e7') in 0.0024304 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_library_window_light_cookie_soft.asset using Guid(e10a87518f970b6498f120403b5e87fa) (NativeFormatImporter) -> (artifact id: 'b6624ac7fb17375ff687066ce4c5208b') in 0.0021838 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_left_sprite_shaded.asset using Guid(f13736bef9726b045ba18071b949e2d2) (NativeFormatImporter) -> (artifact id: 'f6e48f8240644c12833b02b6b33897d1') in 0.0023045 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0019264 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce_soft.asset using Guid(0442e2f80b7dbc34e99a112ae1095f20) (NativeFormatImporter) -> (artifact id: 'a65e2dbc19e50b0fc0fa4598f134ffd8') in 0.0020225 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite_shaded.asset using Guid(246d9d1ed78f72147aa1f537daf9164e) (NativeFormatImporter) -> (artifact id: '59a4f02825ed43e073fd3814395235f5') in 0.0025323 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.asset using Guid(64767a48f06c7a4439175ba9f3f76969) (NativeFormatImporter) -> (artifact id: '2c34ef17951f1b28aef444ba8ef4c761') in 0.0020211 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_back_sprite_shaded.asset using Guid(646afdaa35aa1c84bbfca6d10ad8d956) (NativeFormatImporter) -> (artifact id: '71d57c8ff1c974a8f2d165c436f15504') in 0.0022396 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_stage7_route_move_glow_pad_soft.asset using Guid(86ea6aec710b7924485228367eef6d7a) (NativeFormatImporter) -> (artifact id: 'ea527bd67d269bf39e4e984ed7dcde41') in 0.0022607 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool_soft.asset using Guid(97afe9889f5e56f449c8468a1d0bfdf9) (NativeFormatImporter) -> (artifact id: 'b007a80dd3775e821df40936b5d7e159') in 0.0020278 seconds
Start importing Assets/Art/LensFlare/LensFlareData_Sun.asset using Guid(b7c2a4e0f84b4d18a6b29f5b3e2c7d91) (NativeFormatImporter) -> (artifact id: '063069773417b8a15d3537485051d327') in 0.0020775 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_back_sprite_shaded.asset using Guid(383cf440301bf504a9b6c2a8a85c5c10) (NativeFormatImporter) -> (artifact id: '9a14722ac27a7c483b3d93443ba97166') in 0.0024874 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite_shaded.asset using Guid(e8b91a1d7f94f9d419ae3d2cb8aca69c) (NativeFormatImporter) -> (artifact id: '19f48d248392446507eb0aa3bd573d8d') in 0.0024759 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_left_sprite_shaded.asset using Guid(29c76ef0ee79e294abc159d852e30d6f) (NativeFormatImporter) -> (artifact id: '7a1e2d14c3f7f57972b5a415bb755af8') in 0.0022816 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite_shaded.asset using Guid(8af717035fe354841a2778b67ef47f60) (NativeFormatImporter) -> (artifact id: '636319e0046c8cb6ef7b2f1e0d8a2fbf') in 0.0024264 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite_shaded.asset using Guid(bcf85f4b6024bd345a6725cd7e3f8122) (NativeFormatImporter) -> (artifact id: '1134ffa9b5a09269a7a99f311dd08502') in 0.0028387 seconds
Start importing Assets/Settings/DefaultVolumeProfile.asset using Guid(2dc156ea366288a46859f93b6d3c2cd0) (NativeFormatImporter) -> (artifact id: '1ec695c0ae06a9e64d0635815a723476') in 0.001951 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_right_sprite_shaded.asset using Guid(7edaa2e8ce6f3f040896d2d92f57e792) (NativeFormatImporter) -> (artifact id: '4e696edc4775714bf55149c9029e9a87') in 0.0023374 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite_shaded.asset using Guid(8e8558fedae4d5441a0880d3e0ae5dde) (NativeFormatImporter) -> (artifact id: '7a36765b1626f751d5a96e3827f13ea3') in 0.0022226 seconds
Start importing Assets/Settings/UniversalRenderPipeline.asset using Guid(dea01fc39b8e00e45aa7df6ce357d723) (NativeFormatImporter) -> (artifact id: '778c58ae3f6832e2c8a2de9832c5aef0') in 0.0017508 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite_shaded.asset using Guid(afafc97e4daeeb2408f5830dde7c5f7c) (NativeFormatImporter) -> (artifact id: 'abb16328271b3f261d7f87df395bbbcc') in 0.0022348 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_front_sprite_shaded.asset using Guid(cfe6c4e4efe6c2d4bae69a5a1cb896ad) (NativeFormatImporter) -> (artifact id: 'a1fc470689ab7b251db5843fc102d4a4') in 0.0027016 seconds
Refreshing native plugins compatible for Editor in 0.53 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=49f077ea86e79d541b11410040fedc09): Total: 0.194 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Scenes/Anemora_FastVS_HouseSlice.unity using Guid(5c2c1333e7d65ba4eac9369f107a6118) (DefaultImporter) -> (artifact id: '484c80f31669967295398aa30e40f6d4') in 0.0015951 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=72de9cd2e5a5c0c4f9c390b0d0a30f4a): Total: 0.042 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Settings/FastVS_HouseSlice_Stage7_APV_BakingSet.asset using Guid(39eb4da4a134565418e1b0d9b899f98f) (NativeFormatImporter) -> (artifact id: 'f68e72d51c63b7db24fd9b398dd07dc8') in 0.0150061 seconds
Refreshing native plugins compatible for Editor in 0.76 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=2c2d12b113d5d9f4fac9c8fe8510ed28): Total: 0.039 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Scenes/Anemora_FastVS_HouseSlice.unity using Guid(5c2c1333e7d65ba4eac9369f107a6118) (DefaultImporter) -> (artifact id: 'e36f801b11107b44d755ad42b69c9915') in 0.0012379 seconds
Refreshing native plugins compatible for Editor in 0.57 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=6dee697c62de81b4f8dd03a1f084cdc1): Total: 0.044 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Settings/FastVS_HouseSlice_Stage7_APV_BakingSet.asset using Guid(39eb4da4a134565418e1b0d9b899f98f) (NativeFormatImporter) -> (artifact id: 'f68e72d51c63b7db24fd9b398dd07dc8') in 0.0022664 seconds
Refreshing native plugins compatible for Editor in 0.56 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=d57c261c0073b5f409b74264940adece): Total: 0.022 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Asset Pipeline Refresh (id=b62698c2f8fdc214892b60d46382887f): Total: 0.026 seconds - Initiated by RefreshV2(NoUpdateAssetOptions)
Asset Pipeline Refresh (id=790ba385e5bd61d4cb0325915ca0a7c2): Total: 0.003 seconds - Initiated by RefreshV2(NoUpdateAssetOptions)
Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:449)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHd2dPhaseBAlphaSceneLensFlareBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:1364)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 449)

Opening scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
[Licensing::Client] Successfully resolved entitlement details
Unloading 0 Unused Serialized files (Serialized files now loaded: 0)
Loaded scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
	Deserialize:            308.796 ms
	Integration:            290.445 ms
	Integration of assets:  5.226 ms
	Thread Wait Time:       -5.212 ms
	Total Operation Time:   599.256 ms
Unloading 637 unused Assets / (1.4 MB). Loaded Objects now: 26549.
Memory consumption went from 290.2 MB to 288.8 MB.
Total: 21.078700 ms (FindLiveObjects: 2.070200 ms CreateObjectMapping: 0.315500 ms MarkObjects: 17.444000 ms  DeleteObjects: 1.247500 ms)

InvalidOperationException: House slice validation failed: phase B-alpha directional lens flare intensity must stay near the Morning sun preset default, found 0.300.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseBAlphaSceneLensFlareSetup () [0x00093] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:29714 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseBAlphaSceneLensFlareBatch () [0x00016] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:1367 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 29714)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseBAlphaSceneLensFlareBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[17:07:17] Phase 'validate' FAILED with exit 1
[17:07:17] NoRollback set; preserving worktree after validate failure
```
