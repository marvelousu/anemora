# feat(hd2d): deepen faded dusk camera grade

## Scope

Cycle 92 tests the user's proposal that the scene may read closer to HD-2D if the whole image is slightly darker, with a visible sun anchor and a faded camera grade. This cycle does not change map geometry, Time Window behavior, UI, route logic, fonts, dialogue, or character assets. It focuses on the global camera grade and capture evidence only.

Authored file:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Side-effect file:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\DefaultVolumeProfile.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dShadingFoundationAudit.cs`

SCOPED_PROMPT_ISSUED cycle=92 authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle92ScreenshotsBatch`

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\DefaultVolumeProfile.asset
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle92ScreenshotsBatch
notes: Added the Cycle92 profile sync, validation, and four-shot capture flow without touching any files outside the authored editor file and the allowed volume profile asset.
```

## Implementation Plan

- Apply the darker faded dusk camera grade through `CreateHd2dGlobalVolume()` so generated scenes and the checked-in `DefaultVolumeProfile.asset` stay aligned.
- Keep the grade restrained: post exposure around `-0.21`, saturation around `-14`, warm faded color filter, low bloom, subtle vignette, no active film grain, and no active depth of field.
- Validate the profile values through `ValidateHouseSliceBatch()`.
- Capture current house exterior, current central plaza, past house exterior, and current library overview screenshots for parent visual review.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle92ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle92_faded_dusk_camera_grade_parent_review_20260524_01\parent_review_01_current_house_exterior_faded_dusk_camera_grade_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle92_faded_dusk_camera_grade_parent_review_20260524_01\parent_review_02_current_central_plaza_faded_dusk_camera_grade_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle92_faded_dusk_camera_grade_parent_review_20260524_01\parent_review_03_past_house_exterior_faded_dusk_camera_grade_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle92_faded_dusk_camera_grade_parent_review_20260524_01\parent_review_04_current_library_faded_dusk_camera_grade_overview.png`

## Parent Review Notes

This pass should make the outdoor scenes feel less flat and more intentionally photographed. It is not expected to fix the current house exterior's black horizontal bars or architectural holes; those remain geometry/composition cleanup work for the next cycles.

## Parent Correction

Before running the cycle, the parent removed a stale `Bloom.skipIterations` reference because URP 17's `Bloom` API no longer exposes that member, set `Bloom.downscale` through `BloomDownscaleMode.Half`, and persisted `FilmGrain.active = 0` in `DefaultVolumeProfile.asset`.

The first runner validate failed because `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dShadingFoundationAudit.cs` still enforced the older Cycle25 camera grade values. The parent widened that audit to the Cycle92 faded dusk grade contract before retrying.

## Cycle 92 failure (validate) -- 20260524-013500

```
Asset Pipeline Refresh (id=4880415252bba494a94e2c57f3a823d5): Total: 0.035 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Aria/resident_a_aria_normal_loop_breath_v01_4f_64x96_review_only.png using Guid(a8a8cb999611bfd4ca49e819b825b9fb) (TextureImporter) -> (artifact id: 'b4439db80e6d0ccbb84b2ae2c1308c2e') in 0.0049506 seconds
Refreshing native plugins compatible for Editor in 0.93 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=edb4382e648d2dc42bd3bfc33f3024d3): Total: 0.040 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Aria/resident_a_aria_normal_loop_breath_v01_4f_64x96_review_only.png using Guid(a8a8cb999611bfd4ca49e819b825b9fb) (TextureImporter) -> (artifact id: 'b4439db80e6d0ccbb84b2ae2c1308c2e') in 0.0053291 seconds
Refreshing native plugins compatible for Editor in 0.86 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=5664698f9152874438d5b81f56747a9f): Total: 0.038 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_writing_normal_loop_v02_6f_64x96.png using Guid(61b3cd5f5b4bc2b41953653a46c5c050) (TextureImporter) -> (artifact id: '7ff045c530812383dfde4d98fe46707e') in 0.0059503 seconds
Refreshing native plugins compatible for Editor in 0.88 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=7d98f26b33b5dd6428bfcd5afafb387a): Total: 0.035 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_writing_normal_loop_v02_6f_64x96.png using Guid(61b3cd5f5b4bc2b41953653a46c5c050) (TextureImporter) -> (artifact id: '7ff045c530812383dfde4d98fe46707e') in 0.005706 seconds
Refreshing native plugins compatible for Editor in 0.76 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=bb3ad25869eeb9e428fe8b111ea9cb89): Total: 0.041 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.00766 seconds
Refreshing native plugins compatible for Editor in 0.92 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=225df43307215564d836a8649280bbbf): Total: 0.048 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0087384 seconds
Refreshing native plugins compatible for Editor in 0.93 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=73adb42d2ad37ac4dad45322dc4feca7): Total: 0.057 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0069307 seconds
Refreshing native plugins compatible for Editor in 0.96 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=860693ed9e27b1d43b21bb47809bb0c7): Total: 0.046 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0057391 seconds
Refreshing native plugins compatible for Editor in 0.94 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=9927f32e67d8a6641bee85d620f4f685): Total: 0.048 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0073537 seconds
Refreshing native plugins compatible for Editor in 0.73 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=a0e6544199d481646bde5eab2ba4529a): Total: 0.046 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0055317 seconds
Refreshing native plugins compatible for Editor in 0.81 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=e2e77eab90dc28644b16c9f16c813481): Total: 0.038 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
HD2D material role audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dMaterialRoleFoundationAudit:VerifyMaterialRolesV1 () (at Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs:34)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:257)

(Filename: Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs Line: 34)

HD2D sprite card lighting audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSpriteCardLightingAudit:VerifySpriteCardLightingV1 () (at Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs:98)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:258)

(Filename: Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs Line: 98)

InvalidOperationException: Shading Foundation v1 audit failed:
- Bloom intensity must be near 0.08. (found 0.1).
- Bloom scatter must be near 0.47. (found 0.5).
- ColorAdjustments post exposure must be near -0.08. (found -0.21).
- ColorAdjustments saturation must be near -6. (found -14).
- Vignette intensity must be at or below 0.050.
  at Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit.VerifyShadingFoundationV1 () [0x00064] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dShadingFoundationAudit.cs:39 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x000a9] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:259 

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 39)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[01:36:46] Phase 'validate' FAILED with exit 1
[01:36:46] NoRollback set; preserving worktree after validate failure
```
