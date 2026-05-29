# feat(hd2d): clean outdoor backdrop foreground

## Scope

Cycle 89 follows the Cycle88 billboard conversion. The plaza review improved, but the house exterior review capture was not usable because foreground roof/eave geometry and dark backdrop bands crossed the shot. This cycle keeps the dark, faded camera direction and fixes the review/capture path plus the house exterior backdrop placement so later sky and sun work has a cleaner baseline.

Authored file:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

SCOPED_PROMPT_ISSUED cycle=89 authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle89ScreenshotsBatch`

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - none
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle89ScreenshotsBatch
notes: Kept the changes confined to capture/validation helpers and current house exterior backdrop placement; avoided touching scene assets, build outputs, or post-processing.
```

## Implementation

- Restore Cycle85 capture filenames after the parent-side Cycle88 filename correction hit the wrong helper.
- Keep Cycle88 screenshot evidence paths aligned with the actual committed runner output.
- Add Cycle89 captures from a front gameplay-review house exterior position and the current plaza overview.
- Nudge house exterior far backdrop/haze/tree-line billboards behind the house exterior foreground without changing gameplay geometry or colliders.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle89ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle89_outdoor_backdrop_foreground_cleanup_parent_review_20260523_01\parent_review_01_current_house_exterior_front_backdrop_cleanup_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle89_outdoor_backdrop_foreground_cleanup_parent_review_20260523_01\parent_review_02_current_central_plaza_backdrop_cleanup_overview.png`

## Review Notes

Visual approval for this cycle is whether the house exterior no longer reads as crossed by black backdrop bars in the normal front gameplay-review composition. It is not the final sky/background art pass.

## Cycle 89 failure (validate) -- 20260523-235243

```
Start importing Assets/Art/Characters/FastVS/Aria/resident_a_aria_normal_loop_breath_v01_4f_64x96_review_only.png using Guid(a8a8cb999611bfd4ca49e819b825b9fb) (TextureImporter) -> (artifact id: 'b4439db80e6d0ccbb84b2ae2c1308c2e') in 0.0032749 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=9a106ba8407c0794982eaf977fe2b3ff): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Aria/resident_a_aria_normal_loop_breath_v01_4f_64x96_review_only.png using Guid(a8a8cb999611bfd4ca49e819b825b9fb) (TextureImporter) -> (artifact id: 'b4439db80e6d0ccbb84b2ae2c1308c2e') in 0.0035801 seconds
Refreshing native plugins compatible for Editor in 0.57 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=e73bbedaab811f34083686ffd9413971): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Aria/resident_a_aria_normal_loop_breath_v01_4f_64x96_review_only.png using Guid(a8a8cb999611bfd4ca49e819b825b9fb) (TextureImporter) -> (artifact id: 'b4439db80e6d0ccbb84b2ae2c1308c2e') in 0.0037575 seconds
Refreshing native plugins compatible for Editor in 0.62 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=6369cfe385b8d61498c44ff136772435): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_writing_normal_loop_v02_6f_64x96.png using Guid(61b3cd5f5b4bc2b41953653a46c5c050) (TextureImporter) -> (artifact id: '7ff045c530812383dfde4d98fe46707e') in 0.0037133 seconds
Refreshing native plugins compatible for Editor in 0.57 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=193e696e450eba7478971b9736869130): Total: 0.024 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_writing_normal_loop_v02_6f_64x96.png using Guid(61b3cd5f5b4bc2b41953653a46c5c050) (TextureImporter) -> (artifact id: '7ff045c530812383dfde4d98fe46707e') in 0.0039326 seconds
Refreshing native plugins compatible for Editor in 0.59 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=a465be08bc72597469d31246717183ea): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0036639 seconds
Refreshing native plugins compatible for Editor in 0.56 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=8e4cb60da7cc03349acb3e4e76e4f9dd): Total: 0.023 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0038314 seconds
Refreshing native plugins compatible for Editor in 0.56 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=74a259ccde3ca2e40b25fc635c550518): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0035007 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=cfe4d7f2857b95b4185a0b99328ad7bf): Total: 0.024 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0035933 seconds
Refreshing native plugins compatible for Editor in 0.54 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=508aa0df88588ab47b0c3c180ebfcfeb): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0036901 seconds
Refreshing native plugins compatible for Editor in 0.67 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=569ecf4905d49c442863022689b36818): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0042894 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=a7ec75d66ec78c643a0da4662bd06cee): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
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
[00:01:25] Phase 'validate' FAILED with exit 1
[00:01:25] NoRollback set; preserving worktree after validate failure
```

## Cycle 89 failure (validate) -- 20260524-000333

```
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit:VerifyShadingFoundationV1 () (at Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs:38)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:253)

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 38)

HD2D area lighting profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dAreaLightingProfileFoundationAudit:VerifyAreaLightingProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs:137)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:258)

(Filename: Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs Line: 137)

HD2D overlay profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dOverlayProfileFoundationAudit:VerifyOverlayProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs:402)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:259)

(Filename: Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs Line: 402)

HD2D surface profile audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceProfileFoundationAudit:VerifySurfaceProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs:104)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:260)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs Line: 104)

HD2D surface texture metric audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceTextureMetricAudit:VerifySurfaceTextureMetricsV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs:72)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:261)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs Line: 72)

HD2D lighting transition audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dLightingTransitionAudit:VerifyLightingTransitionV1 () (at Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs:106)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:262)

(Filename: Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs Line: 106)

InvalidOperationException: House slice validation failed: Current_HouseExterior_BackdropFoundation_HorizonTreeLine local position expected within (-0.80, 0.40, 9.00) and (0.80, 1.20, 9.80), but got (-8.20, 0.84, 1.52).
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateVectorWithinRange (System.String label, UnityEngine.Vector3 actual, UnityEngine.Vector3 minInclusive, UnityEngine.Vector3 maxInclusive) [0x00054] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:39972 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateOcclusionBackdropObject (System.String objectName, System.String expectedParentName, System.String expectedMaterialToken, UnityEngine.Vector3 minLocalPosition, UnityEngine.Vector3 maxLocalPosition, UnityEngine.Vector3 minLocalScale, UnityEngine.Vector3 maxLocalScale) [0x0018b] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:37154 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dTwentySeventhCycleExteriorOcclusionBackdropFoundation () [0x00c1c] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:29054 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x00130] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:280 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 29054)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[00:04:12] Phase 'validate' FAILED with exit 1
[00:04:12] NoRollback set; preserving worktree after validate failure
```
