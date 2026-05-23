# feat(hd2d): soften house eave shadow bands

Cycle: 87  
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`  
Authored files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dRenderAssetSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\DefaultVolumeProfile.asset`

## Goal

Cycle 86 added the darker faded camera grade. The house exterior review frame exposed a problem: several current-side eave and roof-contact bands were using the opaque shadow material and read as heavy black bars under the new grade. Cycle 87 softens only those current-side house exterior strips by moving them to the translucent `hd2d_depth_shadow` material and tightening their scale where needed.

## Worker Scope

SCOPED_PROMPT_ISSUED cycle=87 authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle87ScreenshotsBatch`

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - none
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle87ScreenshotsBatch
notes: Kept the change inside the authored editor file and updated the existing current-side validators to match the new `hd2d_depth_shadow` materials and softened scale thresholds.
```

SCOPED_PROMPT_ISSUED cycle=87b authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dRenderAssetSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle87ScreenshotsBatch`

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dRenderAssetSetup.cs
side_effect_files:
  - none
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle87ScreenshotsBatch
notes: Replaced the duplicated DepthOfField and FilmGrain disable blocks with a persistence helper that updates the serialized active flag and marks the component dirty, without changing any tuning values or unrelated setup paths.
```

SCOPED_PROMPT_ISSUED cycle=87c authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle87ScreenshotsBatch`

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - none
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle87ScreenshotsBatch
notes: Added a separate landmark validation helper for the two cycle87 eave-shadow objects and left the stricter non-arrival validator unchanged; no scene generation or out-of-scope files were touched.
```

SCOPED_PROMPT_ISSUED cycle=87d authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle87ScreenshotsBatch`

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - none
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle87ScreenshotsBatch
notes: Removed only the two local scene recreation/open calls from the cycle 87 validation helper; I did not touch the capture flow or any other validator logic.
```

## Implementation

- Replaced current-side house eave, roof-contact, and under-eave composition strips with `hd2d_depth_shadow`.
- Kept past-side warm eave treatments unchanged.
- Left the porch step, tree, and larger cast-shadow treatments unchanged.
- Added `ValidateFastVsHd2dShadowFoundationCycle87HouseExteriorEaveShadowSoftening()`.
- Added `CaptureHd2dShadowFoundationCycle87ScreenshotsBatch()` for overview and oblique door/eave review PNGs.
- Fixed the render setup reproduction path so disabled `DepthOfField` and `FilmGrain` persist to `DefaultVolumeProfile.asset` as `active: 0`.
- Corrected the Cycle87 eave validation so objects intentionally authored by `CreateLandmarkCube` are not rejected by a non-arrival-only assertion.
- Corrected the Cycle87 validator lifecycle so it does not recreate/open the scene while `ValidateHouseSliceBatch()` still owns live controller references.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle87ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle87_eave_shadow_softening_parent_review_20260523_01\parent_review_01_current_house_exterior_eave_shadow_softening_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle87_eave_shadow_softening_parent_review_20260523_01\parent_review_02_current_house_exterior_door_eave_oblique.png`

## Review Notes

This is a local correction after the faded grade, not a global lighting retune. The parent visual gate should compare whether the house face is less dominated by black horizontal strips while preserving readable contact shadow.

## Cycle 87 failure (validate) -- 20260523-225458

```
Start importing Assets/Art/Characters/FastVS/Aria/resident_a_aria_normal_loop_breath_v01_4f_64x96_review_only.png using Guid(a8a8cb999611bfd4ca49e819b825b9fb) (TextureImporter) -> (artifact id: 'b4439db80e6d0ccbb84b2ae2c1308c2e') in 0.0036428 seconds
Refreshing native plugins compatible for Editor in 0.54 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=38a1afbdad490aa49b38343f9457a261): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Aria/resident_a_aria_normal_loop_breath_v01_4f_64x96_review_only.png using Guid(a8a8cb999611bfd4ca49e819b825b9fb) (TextureImporter) -> (artifact id: 'b4439db80e6d0ccbb84b2ae2c1308c2e') in 0.0040715 seconds
Refreshing native plugins compatible for Editor in 0.53 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=9d5394b3c0a72a040bf24de40d123681): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Aria/resident_a_aria_normal_loop_breath_v01_4f_64x96_review_only.png using Guid(a8a8cb999611bfd4ca49e819b825b9fb) (TextureImporter) -> (artifact id: 'b4439db80e6d0ccbb84b2ae2c1308c2e') in 0.0046962 seconds
Refreshing native plugins compatible for Editor in 0.52 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=9bd03434d78314b42830f3639bc81230): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_writing_normal_loop_v02_6f_64x96.png using Guid(61b3cd5f5b4bc2b41953653a46c5c050) (TextureImporter) -> (artifact id: '7ff045c530812383dfde4d98fe46707e') in 0.0035823 seconds
Refreshing native plugins compatible for Editor in 0.60 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=52cd550d00bdba142b3c653e23573170): Total: 0.024 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_writing_normal_loop_v02_6f_64x96.png using Guid(61b3cd5f5b4bc2b41953653a46c5c050) (TextureImporter) -> (artifact id: '7ff045c530812383dfde4d98fe46707e') in 0.003855 seconds
Refreshing native plugins compatible for Editor in 0.54 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=ac9676b13f5dbf44a83654270f977eee): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0038911 seconds
Refreshing native plugins compatible for Editor in 0.60 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=64a7552e7ceb0574ab55cdccca33a1a6): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0038522 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=1471ac8114565c14da743f25aacf0e8a): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.003695 seconds
Refreshing native plugins compatible for Editor in 0.71 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=626dfd1a3b388d84aae784918ffbd37c): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0036412 seconds
Refreshing native plugins compatible for Editor in 0.57 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=e6a4b2cede9d12c40ba9b06bbf68a55b): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0043915 seconds
Refreshing native plugins compatible for Editor in 0.56 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=d3c61084d3b417748a096d6c5bd5ce45): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0038741 seconds
Refreshing native plugins compatible for Editor in 0.60 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=5aad023d0ed9a054c8bf5610eefc8010): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
HD2D material role audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dMaterialRoleFoundationAudit:VerifyMaterialRolesV1 () (at Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs:34)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:251)

(Filename: Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs Line: 34)

HD2D sprite card lighting audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSpriteCardLightingAudit:VerifySpriteCardLightingV1 () (at Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs:98)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:252)

(Filename: Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs Line: 98)

InvalidOperationException: Shading Foundation v1 audit failed:
- DepthOfField must be disabled.
- FilmGrain must be disabled.
  at Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit.VerifyShadingFoundationV1 () [0x00064] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dShadingFoundationAudit.cs:39 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x000a9] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:253 

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 39)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[22:57:09] Phase 'validate' FAILED with exit 1
[22:57:09] NoRollback set; preserving worktree after validate failure
```

Recovery note: this validate failure came from the render foundation reproduction path, not from the eave-shadow geometry. Cycle 87b added serialized `active: 0` persistence for `DepthOfField` and `FilmGrain`, and the parent reran `ApplyShadingFoundationV1()` before the final runner pass.

## Cycle 87 failure (validate) -- 20260523-230458

```
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite.mat using Guid(56b5ac4afa850e94fa26bf722f31b14b) (NativeFormatImporter) -> (artifact id: 'f7e017ad6bce709be21936a4bb8efe19') in 0.0024685 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_depth_shadow.mat using Guid(07afc9c1f7f8ab4439760cab6c8b9c1e) (NativeFormatImporter) -> (artifact id: '68a4feae9900f9534bb049459ffb3678') in 0.0019946 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_map_move_floor_glow.mat using Guid(373d809c8f785b3439635e59312211de) (NativeFormatImporter) -> (artifact id: '70688f62928dc32fdf23a0c22c596252') in 0.0018542 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite.mat using Guid(57d1354643503484f8f9504dec7939e7) (NativeFormatImporter) -> (artifact id: '8d6144afb428b20ddb5b28b485258685') in 0.0018848 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite.mat using Guid(f79bd52245f424d42b668da63a26eed7) (NativeFormatImporter) -> (artifact id: '6c7634a35d7a5e593d016924efb11022') in 0.0019012 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite.mat using Guid(f71c1a4bbbfa65e4a9dc8ceb1dbaab05) (NativeFormatImporter) -> (artifact id: '067c3ca3644ebbf71c59456785c01e71') in 0.0020196 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite.mat using Guid(28f2cae61d95125439828020176a4cfc) (NativeFormatImporter) -> (artifact id: '72016ad2a7c404cc799f9f50c6febaf5') in 0.0021442 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite.mat using Guid(a8c94ff07ef65ea4f8ad0dedea407b1e) (NativeFormatImporter) -> (artifact id: '63c8344cf78a915d285cd3eae78cb1fd') in 0.0020803 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite.mat using Guid(b8841e13281e616468837ca346676037) (NativeFormatImporter) -> (artifact id: '1adcf1ea1109f3defbf059ddda5acb22') in 0.0021163 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite.mat using Guid(3acdb3d0d51b28d40854136863dba5b3) (NativeFormatImporter) -> (artifact id: '085d72f1844e837c1fa998fa9467b7c2') in 0.0020715 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.0022301 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce.mat using Guid(0bc31eecccdf1a840a5477d771512775) (NativeFormatImporter) -> (artifact id: '957df3d5816f00a64823db316222cbe2') in 0.0023336 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool.mat using Guid(0dd9370c56289ae4a8d0e1dddfc59477) (NativeFormatImporter) -> (artifact id: 'dff4420f29bfcf0b5e37a561766cc28b') in 0.0023648 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_route_move_floor_glow.mat using Guid(0dbdea9defe689a4ca05680804b173ca) (NativeFormatImporter) -> (artifact id: '76abd1cd4f418349642ae7527f51d632') in 0.002236 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_back_sprite.mat using Guid(8d4799b24d7badb4d9d50d8f3546587c) (NativeFormatImporter) -> (artifact id: 'ea1cc68dacc196e87140893dcea1fad6') in 0.0022362 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.mat using Guid(0e535f48a85f05b43ad8f1b5cb2ecb4a) (NativeFormatImporter) -> (artifact id: '95a63720d5377afca26fb765e42e17fd') in 0.0019723 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_contact_shadow.asset using Guid(40c9d8116f275fd4c98bbdde1e793a32) (NativeFormatImporter) -> (artifact id: '52cb61e301d2359c974e9144aae52aea') in 0.0023065 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite_shaded.asset using Guid(11367ee44066a2c4a933e1a65a54fa71) (NativeFormatImporter) -> (artifact id: 'b4ea198999a098bff466fc482aa2e085') in 0.002766 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite_shaded.asset using Guid(31848d2268cea5d4a93190a2365133ca) (NativeFormatImporter) -> (artifact id: 'dfcf40487733ba25c6e6bad9c26fa7e7') in 0.002453 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_library_window_light_cookie_soft.asset using Guid(e10a87518f970b6498f120403b5e87fa) (NativeFormatImporter) -> (artifact id: 'b6624ac7fb17375ff687066ce4c5208b') in 0.0026147 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_left_sprite_shaded.asset using Guid(f13736bef9726b045ba18071b949e2d2) (NativeFormatImporter) -> (artifact id: 'f6e48f8240644c12833b02b6b33897d1') in 0.0025481 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0022188 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce_soft.asset using Guid(0442e2f80b7dbc34e99a112ae1095f20) (NativeFormatImporter) -> (artifact id: '4f07e5b7718abd9c2a1c79a9ce8eb28f') in 0.002345 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite_shaded.asset using Guid(246d9d1ed78f72147aa1f537daf9164e) (NativeFormatImporter) -> (artifact id: '59a4f02825ed43e073fd3814395235f5') in 0.0028532 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.asset using Guid(64767a48f06c7a4439175ba9f3f76969) (NativeFormatImporter) -> (artifact id: '2c34ef17951f1b28aef444ba8ef4c761') in 0.0023202 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_back_sprite_shaded.asset using Guid(646afdaa35aa1c84bbfca6d10ad8d956) (NativeFormatImporter) -> (artifact id: '71d57c8ff1c974a8f2d165c436f15504') in 0.0023985 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool_soft.asset using Guid(97afe9889f5e56f449c8468a1d0bfdf9) (NativeFormatImporter) -> (artifact id: 'b007a80dd3775e821df40936b5d7e159') in 0.0021806 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_back_sprite_shaded.asset using Guid(383cf440301bf504a9b6c2a8a85c5c10) (NativeFormatImporter) -> (artifact id: '9a14722ac27a7c483b3d93443ba97166') in 0.0025402 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite_shaded.asset using Guid(e8b91a1d7f94f9d419ae3d2cb8aca69c) (NativeFormatImporter) -> (artifact id: '19f48d248392446507eb0aa3bd573d8d') in 0.0028804 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_left_sprite_shaded.asset using Guid(29c76ef0ee79e294abc159d852e30d6f) (NativeFormatImporter) -> (artifact id: '7a1e2d14c3f7f57972b5a415bb755af8') in 0.0027171 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite_shaded.asset using Guid(8af717035fe354841a2778b67ef47f60) (NativeFormatImporter) -> (artifact id: '636319e0046c8cb6ef7b2f1e0d8a2fbf') in 0.0030768 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite_shaded.asset using Guid(bcf85f4b6024bd345a6725cd7e3f8122) (NativeFormatImporter) -> (artifact id: '1134ffa9b5a09269a7a99f311dd08502') in 0.0026551 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_right_sprite_shaded.asset using Guid(7edaa2e8ce6f3f040896d2d92f57e792) (NativeFormatImporter) -> (artifact id: '4e696edc4775714bf55149c9029e9a87') in 0.0025204 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite_shaded.asset using Guid(8e8558fedae4d5441a0880d3e0ae5dde) (NativeFormatImporter) -> (artifact id: '7a36765b1626f751d5a96e3827f13ea3') in 0.0026569 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite_shaded.asset using Guid(afafc97e4daeeb2408f5830dde7c5f7c) (NativeFormatImporter) -> (artifact id: 'abb16328271b3f261d7f87df395bbbcc') in 0.0026902 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_front_sprite_shaded.asset using Guid(cfe6c4e4efe6c2d4bae69a5a1cb896ad) (NativeFormatImporter) -> (artifact id: 'a1fc470689ab7b251db5843fc102d4a4') in 0.0025969 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=b8e69ee0c17bd654db1bd7998fa58446): Total: 0.182 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Scenes/Anemora_FastVS_HouseSlice.unity using Guid(5c2c1333e7d65ba4eac9369f107a6118) (DefaultImporter) -> (artifact id: 'bd0a094b3db6bfa1f42cd028c7bf834a') in 0.0013804 seconds
Refreshing native plugins compatible for Editor in 0.76 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=12e6b3e7fb0faea45a65082adcf4c792): Total: 0.050 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Asset Pipeline Refresh (id=e8e95c0afb41d9a4e8e9284a7b6732ed): Total: 0.024 seconds - Initiated by RefreshV2(NoUpdateAssetOptions)
Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:221)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateFastVsHd2dShadowFoundationCycle87HouseExteriorEaveShadowSoftening () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:35380)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:333)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 221)

Opening scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
Unloading 0 Unused Serialized files (Serialized files now loaded: 0)
Loaded scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
	Deserialize:            238.123 ms
	Integration:            303.493 ms
	Integration of assets:  1.856 ms
	Thread Wait Time:       -1.819 ms
	Total Operation Time:   541.653 ms
Unloading 545 unused Assets / (6.1 MB). Loaded Objects now: 23545.
Memory consumption went from 253.6 MB to 247.5 MB.
Total: 11.560800 ms (FindLiveObjects: 1.908500 ms CreateObjectMapping: 0.406100 ms MarkObjects: 7.554200 ms  DeleteObjects: 1.690400 ms)

InvalidOperationException: House slice validation failed: Current_HouseExterior_UnderEaveShadowBand must keep a non-arrival PropOrFeature landmark.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateNonArrivalLandmarkCubeObject (System.String objectName, System.String expectedParentName, UnityEngine.Vector3 referenceCenter, System.String expectedMaterialToken, Anemora.TimeManagement.TimeWindowPairedSpaceLandmarkKind expectedKind, UnityEngine.Vector3 minLocalPosition, UnityEngine.Vector3 maxLocalPosition, UnityEngine.Vector3 minLocalScale, UnityEngine.Vector3 maxLocalScale) [0x00204] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:36200 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dShadowFoundationCycle87HouseExteriorEaveShadowSoftening () [0x00011] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:35385 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x00239] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:333 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 35385)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[23:05:59] Phase 'validate' FAILED with exit 1
[23:05:59] NoRollback set; preserving worktree after validate failure
```

## Cycle 87 failure (validate) -- 20260523-231217

```
Unloading 0 Unused Serialized files (Serialized files now loaded: 0)
Loaded scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
	Deserialize:            268.647 ms
	Integration:            299.504 ms
	Integration of assets:  2.440 ms
	Thread Wait Time:       -2.194 ms
	Total Operation Time:   568.396 ms
Unloading 545 unused Assets / (6.0 MB). Loaded Objects now: 23545.
Memory consumption went from 244.7 MB to 238.8 MB.
Total: 12.476600 ms (FindLiveObjects: 2.160500 ms CreateObjectMapping: 0.527900 ms MarkObjects: 8.093000 ms  DeleteObjects: 1.692900 ms)

Fast VS house exterior eave shadow softening validation passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateFastVsHd2dShadowFoundationCycle87HouseExteriorEaveShadowSoftening () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:35473)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:333)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 35473)

Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_outdoor_void_background.mat using Guid(57539333774454a4b9b68a2662c07ad5) (NativeFormatImporter) -> (artifact id: '99c018d2123986061894ad403d77f6fe') in 0.0028407 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_outdoor_void_background.mat using Guid(0ec66c653876c7244a13d4f32da1526a) (NativeFormatImporter) -> (artifact id: 'e1db2a1427d1c97a066681d67848d63a') in 0.0023852 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(efe4ace4ebe298d4bada9fe5c2546e89) (NativeFormatImporter) -> (artifact id: '84254ef753b242d39fea27ca378b0b03') in 0.0019525 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.asset using Guid(5aa393eea6e946245b2e701795e51af0) (NativeFormatImporter) -> (artifact id: '184b95cb35aec2eba012f0ac3b8c8e5a') in 0.0022223 seconds
Refreshing native plugins compatible for Editor in 0.60 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=f90590814fa4b1745b97a95debc410c7): Total: 0.054 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(efe4ace4ebe298d4bada9fe5c2546e89) (NativeFormatImporter) -> (artifact id: 'a69f20e4e842686af22fc83b45a76019') in 0.0026803 seconds
Refreshing native plugins compatible for Editor in 0.54 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=b68828a3eb0e74f47b59c80ab876a47a): Total: 0.040 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(0a33e84f626797a40b8d5687454e6620) (NativeFormatImporter) -> (artifact id: '2dfb0ee97db515b87db3e427f999c2a7') in 0.0030021 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.asset using Guid(b43c1d26d8af7144eac532f733d613ed) (NativeFormatImporter) -> (artifact id: '72b93ea802d61590522168319e686932') in 0.0024557 seconds
Refreshing native plugins compatible for Editor in 0.49 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=1051de47c4bcf5445a9d4c06cf30a4f8): Total: 0.050 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(0a33e84f626797a40b8d5687454e6620) (NativeFormatImporter) -> (artifact id: '1811c45b24cb5a42b0d119f903087e3b') in 0.0027852 seconds
Refreshing native plugins compatible for Editor in 0.53 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=7e5d8faf603defc4a958ef7a6905a4db): Total: 0.041 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.mat using Guid(ba3c455e34bfdef488b2dbb1bdd0cd70) (NativeFormatImporter) -> (artifact id: 'dec47bc9f235a292b061ba7705817a0e') in 0.0027573 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.asset using Guid(6b757f77ea5d7164e8537193c67506bd) (NativeFormatImporter) -> (artifact id: 'edb251d8a3d8ffd6a09591c3a454c8a2') in 0.0026101 seconds
Refreshing native plugins compatible for Editor in 0.52 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=c250ed4e330610745a8bb323dc0855a3): Total: 0.046 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.mat using Guid(ba3c455e34bfdef488b2dbb1bdd0cd70) (NativeFormatImporter) -> (artifact id: '1a5dfe9209479e4411a9a5728bc36da8') in 0.0027441 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=cd6d2e6aadb508c4091529230059f750): Total: 0.040 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.mat using Guid(b09502d957a9b5141836a6b6ab31797f) (NativeFormatImporter) -> (artifact id: '3e5b99ed69b07b0978cb63a29b9cb0d4') in 0.0027279 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.asset using Guid(2bb9710db4a50874c944ff2cece89232) (NativeFormatImporter) -> (artifact id: '91e098c066551ce9c7048c5e7b3cb0d8') in 0.0030352 seconds
Refreshing native plugins compatible for Editor in 0.63 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=95bd4140d4c69f34b97385a9c3399f68): Total: 0.048 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.mat using Guid(b09502d957a9b5141836a6b6ab31797f) (NativeFormatImporter) -> (artifact id: 'df71a74559f1cd0ef87c4679dd6814a6') in 0.002496 seconds
Refreshing native plugins compatible for Editor in 0.66 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=0c2328d96c811fd419615a6c5ccc38b0): Total: 0.042 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_occlusion_gradient.mat using Guid(d58f2dd58f72df641b0b13d70b9aaffb) (NativeFormatImporter) -> (artifact id: 'cce592b71f40c96c4099f5295e653a71') in 0.002826 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_occlusion_gradient_soft.asset using Guid(c77c6612505c47d4aa1dac2b81f7897e) (NativeFormatImporter) -> (artifact id: '0a0b3058b133df474de29a6690dee7ed') in 0.0028116 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=e3be7e027ac24ce43829dbeb914c3a80): Total: 0.032 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
MissingReferenceException: The object of type 'UnityEngine.Transform' has been destroyed but you are still trying to access it.
Your script should either check if it is null or you should not destroy the object.
  at UnityEngine.Object+MarshalledUnityObject.TryThrowEditorNullExceptionObject (UnityEngine.Object unityObj, System.String parameterName) [0x0006a] in <6567e2a645a94ef68fffcf0e5cb82d14>:0 
  at UnityEngine.Bindings.ThrowHelper.ThrowNullReferenceException (System.Object obj) [0x00010] in <6567e2a645a94ef68fffcf0e5cb82d14>:0 
  at UnityEngine.Transform.TransformPoint (UnityEngine.Vector3 position) [0x00006] in <6567e2a645a94ef68fffcf0e5cb82d14>:0 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateCameraStaysOnSameCoordinateRoot (Anemora.TimeManagement.TimeWindowPairedSpacePortalController controller) [0x0002b] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:22672 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x00400] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:424 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 22672)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[23:15:02] Phase 'validate' FAILED with exit 1
[23:15:02] NoRollback set; preserving worktree after validate failure
```
