# feat(hd2d): deepen plaza sun shadow grade

## Scope

Cycle 115 follows parent review of Cycle 114. Cycle114 added current-plaza cast-shadow contrast, but the visual still needs a stronger lighting foundation: darker ambient, warmer key sun, deeper shadows, and a slightly more faded camera grade. This cycle changes the central plaza lighting profile and adds validation/capture gates for that grade.

Authored files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsHouseLightingDirector.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Expected generated side-effect files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\DefaultVolumeProfile.asset` may be dirtied during validation/build and must not be committed unless explicitly staged by the parent.

Out of scope:

- Main branch, map-layout changes, story/UI, doors, time-window behavior, character assets, house facade fixes, and ProjectSettings.

## Goal Prompt

Deepen the current central plaza lighting grade so the same scene reads more like the reference: dark ambient base, strong warm sun, cooler weak rim, visible fog/air, and faded camera color. Prioritize shadow impression over map changes.

Reference directory:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference`

Map reference directory noted but not edited this cycle:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\map_chapter_1\map_chapter_1`

## Worker Cycle

- Cycle-worker: `019e5790-0339-7470-931c-5be681c597a5`
- Worker role: `cycle-worker` / `gpt-5.4-mini`

Scoped prompt trace:

`SCOPED_PROMPT_ISSUED cycle=115 authored_files=C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsHouseLightingDirector.cs;C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs validate=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSunShadowGradeCycle115Batch capture=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaSunShadowGradeCycle115ScreenshotsBatch`

## Implementation Plan

- Tune only the `FastVsHouseArea.CentralPlaza` runtime lighting profile toward stronger key-sun contrast: hotter key light, lower ambient, reduced warm fill, restrained cool rim, shorter warmer fog.
- Keep post-process contrast within the existing Shading Foundation v1 readability audit; use light/ambient values, not excess post-process contrast, for the main shadow gain.
- Keep house exterior, library, and interior profile behavior unchanged except any shared post-process grade explicitly applied through the setup code.
- Add a Cycle115 validation method that first runs Cycle114 validation and then applies the central plaza profile to assert key values.
- Add Cycle115 parent-review capture output for current plaza overview/close plus guard views.
- Preserve cleanup discipline: generated `DefaultVolumeProfile.asset`, scene, Addressables, and ProjectSettings dirt are not committed.

## Expected Evidence

Parent-review screenshots are expected under:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle115_plaza_sun_shadow_grade_parent_review_20260524_01`

Expected files:

- `parent_review_01_current_central_plaza_sun_shadow_grade_overview.png`
- `parent_review_02_current_central_plaza_sun_shadow_grade_close.png`
- `parent_review_03_past_central_plaza_sun_shadow_grade_guard.png`
- `parent_review_04_current_library_sun_shadow_grade_guard.png`

## Validation

Planned cycle-runner command:

```powershell
$commitPaths = @(
  'Assets/Scripts/FastVS/FastVsHouseLightingDirector.cs',
  'Assets/Editor/AnemoraFastVsHouseSliceSetup.cs',
  'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_sun_shadow_grade_cycle115.md',
  'docs/devlog/INDEX.md',
  'docs/devlog/screenshots/fast_vs_hd2d_cycle115_plaza_sun_shadow_grade_parent_review_20260524_01'
)
& .\tools\cycle-runner.ps1 -CycleNumber 115 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSunShadowGradeCycle115Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaSunShadowGradeCycle115ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_sun_shadow_grade_cycle115.md' `
  -Audience parent_review `
  -CommitPath $commitPaths `
  -NoRollback
```

## Visual Gate

Passing criteria:

- Current plaza overview shows a clearer warm-sun / dark-shadow separation than Cycle114.
- Niro and route readability remain acceptable.
- Library guard capture does not become crushed or unreadable.
- No map-layout or story behavior changes are introduced.

## Cycle 115 failure (build) -- 20260524-111400

```
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=96a63a261993c5d46948f18f1030df37): Total: 0.036 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Aria/resident_a_aria_normal_loop_breath_v01_4f_64x96_review_only.png using Guid(a8a8cb999611bfd4ca49e819b825b9fb) (TextureImporter) -> (artifact id: 'b4439db80e6d0ccbb84b2ae2c1308c2e') in 0.0048021 seconds
Refreshing native plugins compatible for Editor in 0.70 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=d3b6998509935b64e998e7aa46dfb947): Total: 0.035 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Aria/resident_a_aria_normal_loop_breath_v01_4f_64x96_review_only.png using Guid(a8a8cb999611bfd4ca49e819b825b9fb) (TextureImporter) -> (artifact id: 'b4439db80e6d0ccbb84b2ae2c1308c2e') in 0.0035867 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=3c2108de1a8b02e45a5ffe852b0f1ec5): Total: 0.028 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_writing_normal_loop_v02_6f_64x96.png using Guid(61b3cd5f5b4bc2b41953653a46c5c050) (TextureImporter) -> (artifact id: '7ff045c530812383dfde4d98fe46707e') in 0.0043364 seconds
Refreshing native plugins compatible for Editor in 0.67 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=a34039fa4a6747a46ae543200e5ff985): Total: 0.031 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_writing_normal_loop_v02_6f_64x96.png using Guid(61b3cd5f5b4bc2b41953653a46c5c050) (TextureImporter) -> (artifact id: '7ff045c530812383dfde4d98fe46707e') in 0.0041375 seconds
Refreshing native plugins compatible for Editor in 0.63 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=38749db47673b1540b760b70a8326b89): Total: 0.030 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0045488 seconds
Refreshing native plugins compatible for Editor in 0.73 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=882203c547712e04492f7e2109877e72): Total: 0.029 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0041648 seconds
Refreshing native plugins compatible for Editor in 0.60 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=24f755f61c55e9845a252598dfa1e41a): Total: 0.030 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0041451 seconds
Refreshing native plugins compatible for Editor in 0.65 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=95551d6449ef54f438eefe613a4402d0): Total: 0.031 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0045021 seconds
Refreshing native plugins compatible for Editor in 0.62 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=a8cf4e0a3412c7c4fadda24ccf7a59a4): Total: 0.032 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0053528 seconds
Refreshing native plugins compatible for Editor in 0.67 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=f996cb7df93690d45b16ab63e05301f0): Total: 0.031 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0053322 seconds
Refreshing native plugins compatible for Editor in 0.78 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=c88d347cfb3ff184ea6c714234ed06b0): Total: 0.039 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
HD2D material role audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dMaterialRoleFoundationAudit:VerifyMaterialRolesV1 () (at Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs:34)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:298)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:556)

(Filename: Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs Line: 34)

HD2D sprite card lighting audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSpriteCardLightingAudit:VerifySpriteCardLightingV1 () (at Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs:98)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:299)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:556)

(Filename: Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs Line: 98)

InvalidOperationException: Shading Foundation v1 audit failed:
- ColorAdjustments contrast must keep readability. (found 9).
  at Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit.VerifyShadingFoundationV1 () [0x00064] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dShadingFoundationAudit.cs:39 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x000a9] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:300 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:556 

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 39)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[11:17:29] Phase 'build' FAILED with exit 1
[11:17:29] NoRollback set; preserving worktree after build failure
```

## Cycle 115 failure (build) -- 20260524-112040

```
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=336ad1327179f4e49a4c829effcb413b): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_writing_normal_loop_v02_6f_64x96.png using Guid(61b3cd5f5b4bc2b41953653a46c5c050) (TextureImporter) -> (artifact id: '7ff045c530812383dfde4d98fe46707e') in 0.0036704 seconds
Refreshing native plugins compatible for Editor in 0.56 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=a9a382c73cc8b0a4a87179212c01cb2b): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0042402 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=b2b8de6d62ba37344a94ce14cbbe3f24): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0039527 seconds
Refreshing native plugins compatible for Editor in 0.60 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=01924671806845242bdda9f42b022eb8): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0034959 seconds
Refreshing native plugins compatible for Editor in 0.62 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=443daa644aea1f741814a50b01c408d8): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0036506 seconds
Refreshing native plugins compatible for Editor in 0.85 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=f5603930fedd66943967eef01c08a68c): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0039775 seconds
Refreshing native plugins compatible for Editor in 0.61 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=d48ee24ece7ebb14a9e820d6f75e3982): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.004258 seconds
Refreshing native plugins compatible for Editor in 0.62 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=3ec4ab1af337e9b4187dc9aa8756b58c): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
HD2D material role audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dMaterialRoleFoundationAudit:VerifyMaterialRolesV1 () (at Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs:34)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:298)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:556)

(Filename: Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs Line: 34)

HD2D sprite card lighting audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSpriteCardLightingAudit:VerifySpriteCardLightingV1 () (at Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs:98)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:299)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:556)

(Filename: Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs Line: 98)

Shading Foundation v1 audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit:VerifyShadingFoundationV1 () (at Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs:38)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:300)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:556)

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 38)

InvalidOperationException: House slice validation failed: ambient light must remain inside the HD-2D shading foundation profile range, found (0.112, 0.106, 0.100).
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dFirstCycleVisuals () [0x00123] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:29643 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x000ae] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:301 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:556 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 29643)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[11:23:57] Phase 'build' FAILED with exit 1
[11:23:57] NoRollback set; preserving worktree after build failure
```

## Cycle 115 failure (build) -- 20260524-112529

```
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=8edc11fba05a03b46bb7a3400803bc6e): Total: 0.038 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_writing_normal_loop_v02_6f_64x96.png using Guid(61b3cd5f5b4bc2b41953653a46c5c050) (TextureImporter) -> (artifact id: '7ff045c530812383dfde4d98fe46707e') in 0.0057214 seconds
Refreshing native plugins compatible for Editor in 1.04 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=a618e65dc21d0964f9fd18da18ff7381): Total: 0.045 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0061994 seconds
Refreshing native plugins compatible for Editor in 0.71 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=de62c33aaab1ac24c9d88bdef7340ab6): Total: 0.035 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0052576 seconds
Refreshing native plugins compatible for Editor in 0.67 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=8e85109db3a196c4b94838ed6285f710): Total: 0.039 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0040438 seconds
Refreshing native plugins compatible for Editor in 0.75 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=db62b40125bf05c40a55abc7698ded5b): Total: 0.030 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0054893 seconds
Refreshing native plugins compatible for Editor in 0.73 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=6cc8e7572ef7a38418d6887bf2badbab): Total: 0.036 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.004762 seconds
Refreshing native plugins compatible for Editor in 0.63 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=ab4a179aba7b5c3428162ee44b95f2eb): Total: 0.032 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0045224 seconds
Refreshing native plugins compatible for Editor in 0.73 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=418dfcfee35e3f240bd76d8abc4f31ba): Total: 0.032 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
HD2D material role audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dMaterialRoleFoundationAudit:VerifyMaterialRolesV1 () (at Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs:34)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:298)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:556)

(Filename: Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs Line: 34)

HD2D sprite card lighting audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSpriteCardLightingAudit:VerifySpriteCardLightingV1 () (at Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs:98)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:299)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:556)

(Filename: Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs Line: 98)

Shading Foundation v1 audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit:VerifyShadingFoundationV1 () (at Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs:38)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:300)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:556)

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 38)

InvalidOperationException: House slice validation failed: Directional Light shadow strength must stay in the HD-2D decisive shadow balance range.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dThirtySeventhCycleLightingBalance () [0x0007a] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:29743 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x000b3] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:302 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:556 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 29743)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[11:28:42] Phase 'build' FAILED with exit 1
[11:28:42] NoRollback set; preserving worktree after build failure
```

## Cycle 115 failure (build) -- 20260524-113004

```
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0043256 seconds
Refreshing native plugins compatible for Editor in 0.59 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=687eed689894f284ab0fdcbbe617e720): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0037377 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=b2322230a14f30c4f9b9c958f17eccf8): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0033952 seconds
Refreshing native plugins compatible for Editor in 0.60 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=2aaac9c3703cdd049a233ec2ab73e266): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0034125 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=23a56dbb256f9a344b31ad5732a988d6): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0038164 seconds
Refreshing native plugins compatible for Editor in 0.60 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=06de52ea7ea1cc44f9593496387fa067): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0037881 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=1eb9b3e6241ec18489c8ee357ceb52e4): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
HD2D material role audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dMaterialRoleFoundationAudit:VerifyMaterialRolesV1 () (at Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs:34)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:298)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:556)

(Filename: Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs Line: 34)

HD2D sprite card lighting audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSpriteCardLightingAudit:VerifySpriteCardLightingV1 () (at Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs:98)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:299)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:556)

(Filename: Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs Line: 98)

Shading Foundation v1 audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit:VerifyShadingFoundationV1 () (at Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs:38)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:300)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:556)

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 38)

InvalidOperationException: HD2D area lighting profile audit failed:
- Area lighting profile FastVS_HD2D_CentralPlazaLightingProfile must keep keyLightIntensity near 1.240, but was 1.380.
- Area lighting profile FastVS_HD2D_CentralPlazaLightingProfile must keep keyLightTint near RGBA(1.000, 0.910, 0.750, 1.000), but was RGBA(1.000, 0.860, 0.620, 1.000).
- Area lighting profile FastVS_HD2D_CentralPlazaLightingProfile must keep fillIntensity near 0.090, but was 0.055.
- Area lighting profile FastVS_HD2D_CentralPlazaLightingProfile must keep ambientIntensity near 0.153, but was 0.118.
- Area lighting profile FastVS_HD2D_CentralPlazaLightingProfile must keep ambientTint near RGBA(0.148, 0.154, 0.160, 1.000), but was RGBA(0.118, 0.113, 0.104, 1.000).
- Area lighting profile FastVS_HD2D_CentralPlazaLightingProfile must keep runtime ambient luminance near 0.118, but was 0.113.
  at Anemora.EditorTools.AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.VerifyAreaLightingProfilesV1 () [0x00318] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs:143 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x000c2] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:305 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:556 

(Filename: Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs Line: 143)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[11:33:22] Phase 'build' FAILED with exit 1
[11:33:22] NoRollback set; preserving worktree after build failure
```

## Cycle 115 failure (build) -- 20260524-113517

```
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dOverlayProfileFoundationAudit:VerifyOverlayProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs:402)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:306)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:556)

(Filename: Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs Line: 402)

HD2D surface profile audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceProfileFoundationAudit:VerifySurfaceProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs:104)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:307)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:556)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs Line: 104)

HD2D surface texture metric audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceTextureMetricAudit:VerifySurfaceTextureMetricsV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs:72)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:308)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:556)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs Line: 72)

HD2D lighting transition audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dLightingTransitionAudit:VerifyLightingTransitionV1 () (at Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs:106)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:309)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:556)

(Filename: Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs Line: 106)

Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.0030937 seconds
Refreshing native plugins compatible for Editor in 0.57 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=0f51b166f11e1214f9d18d8d1f7f29c5): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.0029621 seconds
Refreshing native plugins compatible for Editor in 0.53 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=6e98c7d80e3c27744bd7b6951805acc1): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Fast VS house exterior eave shadow softening validation passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateFastVsHd2dShadowFoundationCycle87HouseExteriorEaveShadowSoftening () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:42891)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:391)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:556)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 42891)

InvalidOperationException: House slice validation failed: CentralPlaza main light intensity must stay within 1.220-1.260, found 1.380.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateCycle49LightingProfileRange (Anemora.FastVS.FastVsHouseLightingDirector director, Anemora.FastVS.FastVsHouseArea area, UnityEngine.Light mainLight, UnityEngine.Light warmFill, UnityEngine.Light libraryWindow, System.Boolean expectFog, UnityEngine.Vector3 minAmbient, UnityEngine.Vector3 maxAmbient, System.Single minMainIntensity, System.Single maxMainIntensity, System.Single minShadowStrength, System.Single maxShadowStrength, System.Single minWarmFillIntensity, System.Single maxWarmFillIntensity, System.Boolean expectLibraryWindowEnabled, System.Single minLibraryWindowIntensity, System.Single maxLibraryWindowIntensity) [0x0001b] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:41913 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dCycle49DecisiveLightShadowContrast () [0x000e1] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:37104 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x0027f] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:394 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:556 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 37104)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[11:38:48] Phase 'build' FAILED with exit 1
[11:38:48] NoRollback set; preserving worktree after build failure
```

## Cycle 115 failure (build) -- 20260524-113943

```

Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.0032432 seconds
Refreshing native plugins compatible for Editor in 0.62 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=ffab86d85add35545b0094974d0ff2a0): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.0098324 seconds
Refreshing native plugins compatible for Editor in 0.65 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=ff32df50328b9ad4ba15c5be82b6f6b1): Total: 0.046 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Fast VS house exterior eave shadow softening validation passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateFastVsHd2dShadowFoundationCycle87HouseExteriorEaveShadowSoftening () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:42891)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:391)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:556)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 42891)

Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_outdoor_void_background.mat using Guid(57539333774454a4b9b68a2662c07ad5) (NativeFormatImporter) -> (artifact id: '99c018d2123986061894ad403d77f6fe') in 0.0029699 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.0022781 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_outdoor_void_background.mat using Guid(0ec66c653876c7244a13d4f32da1526a) (NativeFormatImporter) -> (artifact id: 'e1db2a1427d1c97a066681d67848d63a') in 0.0020686 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(efe4ace4ebe298d4bada9fe5c2546e89) (NativeFormatImporter) -> (artifact id: '84254ef753b242d39fea27ca378b0b03') in 0.0020011 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0018914 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.asset using Guid(5aa393eea6e946245b2e701795e51af0) (NativeFormatImporter) -> (artifact id: '184b95cb35aec2eba012f0ac3b8c8e5a') in 0.0021594 seconds
Refreshing native plugins compatible for Editor in 0.57 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=827843ce12fbeeb449950e6284111996): Total: 0.060 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(efe4ace4ebe298d4bada9fe5c2546e89) (NativeFormatImporter) -> (artifact id: 'a69f20e4e842686af22fc83b45a76019') in 0.0028679 seconds
Refreshing native plugins compatible for Editor in 0.54 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=71b64e3fd819300469739426800b00d2): Total: 0.041 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(0a33e84f626797a40b8d5687454e6620) (NativeFormatImporter) -> (artifact id: '2dfb0ee97db515b87db3e427f999c2a7') in 0.0026107 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.asset using Guid(b43c1d26d8af7144eac532f733d613ed) (NativeFormatImporter) -> (artifact id: '72b93ea802d61590522168319e686932') in 0.0023088 seconds
Refreshing native plugins compatible for Editor in 0.64 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=53c567b4544dfe94fa260002c58981f4): Total: 0.047 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(0a33e84f626797a40b8d5687454e6620) (NativeFormatImporter) -> (artifact id: '1811c45b24cb5a42b0d119f903087e3b') in 0.0029726 seconds
Refreshing native plugins compatible for Editor in 0.54 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=ef41daf76a639df4c91457a4f96e362f): Total: 0.042 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.mat using Guid(ba3c455e34bfdef488b2dbb1bdd0cd70) (NativeFormatImporter) -> (artifact id: 'dec47bc9f235a292b061ba7705817a0e') in 0.0028438 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.asset using Guid(6b757f77ea5d7164e8537193c67506bd) (NativeFormatImporter) -> (artifact id: 'edb251d8a3d8ffd6a09591c3a454c8a2') in 0.0026265 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=467abbcc76f52424fa6c472d1cb8cf5d): Total: 0.046 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.mat using Guid(ba3c455e34bfdef488b2dbb1bdd0cd70) (NativeFormatImporter) -> (artifact id: '1a5dfe9209479e4411a9a5728bc36da8') in 0.0030499 seconds
Refreshing native plugins compatible for Editor in 0.57 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=0119c8ee90489b345925a0a71148f761): Total: 0.041 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.mat using Guid(b09502d957a9b5141836a6b6ab31797f) (NativeFormatImporter) -> (artifact id: '3e5b99ed69b07b0978cb63a29b9cb0d4') in 0.0027366 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.asset using Guid(2bb9710db4a50874c944ff2cece89232) (NativeFormatImporter) -> (artifact id: '91e098c066551ce9c7048c5e7b3cb0d8') in 0.002939 seconds
Refreshing native plugins compatible for Editor in 0.54 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=724f665e9df5a6d48ae34f0b8b226712): Total: 0.046 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.mat using Guid(b09502d957a9b5141836a6b6ab31797f) (NativeFormatImporter) -> (artifact id: 'df71a74559f1cd0ef87c4679dd6814a6') in 0.0029292 seconds
Refreshing native plugins compatible for Editor in 0.63 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=dc6e2bfbd18d60d40ab7d25b1ddafb6b): Total: 0.043 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_occlusion_gradient.mat using Guid(d58f2dd58f72df641b0b13d70b9aaffb) (NativeFormatImporter) -> (artifact id: 'cce592b71f40c96c4099f5295e653a71') in 0.0029861 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_occlusion_gradient_soft.asset using Guid(c77c6612505c47d4aa1dac2b81f7897e) (NativeFormatImporter) -> (artifact id: '0a0b3058b133df474de29a6690dee7ed') in 0.0025942 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=2269c676f9d359947a429987f499c993): Total: 0.032 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
InvalidOperationException: House slice validation failed: central plaza outdoor sky clear color expected RGBA(0.227, 0.213, 0.189, 1.000) but was RGBA(0.188, 0.166, 0.136, 1.000).
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateColorApproximately (UnityEngine.Color actual, UnityEngine.Color expected, System.String label) [0x00064] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:26780 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateOutdoorSkyClearColorForReview () [0x00098] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:26767 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dEightyEighthCycleOutdoorSkyBackdrop () [0x0003c] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:25872 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x002fc] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:419 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:556 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 26767)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[11:43:49] Phase 'build' FAILED with exit 1
[11:43:49] NoRollback set; preserving worktree after build failure
```
