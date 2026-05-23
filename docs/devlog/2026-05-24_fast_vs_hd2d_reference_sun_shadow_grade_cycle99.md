# feat(hd2d): strengthen reference sun shadows

## Scope

Cycle 99 shifts the HD-2D work back to the requested shading foundation. The cycle intentionally avoids house-gap, facade, terrain, door, prop, map-layout, and asset-import cleanup.

Authored files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsHouseLightingDirector.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs`

## Goal Prompt

Use the reference image as a sensibility target, not as an asset source:

> Build an HD-2D lighting foundation with a visible warm sun direction, lower ambient lift, stronger soft directional shadows, readable contact darkness, and a slightly faded warm camera grade. Preserve the VS map and story behavior. Do not hide geometry artifacts with unrelated facade edits during this cycle. Validate with current and past outdoor screenshots plus a library interior close view.

## Worker Cycle

- Worker `019e564b-f314-7f13-a4c5-9547257f1a5b` implemented the editor setup pass in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`.
- Worker `019e5653-9298-7782-aecc-ee9bef65b489` synchronized runtime area lighting in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsHouseLightingDirector.cs`.
- Worker `019e5653-e658-7173-a964-12421545397c` synchronized the area-lighting audit in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs`.
- Parent review corrected numeric validation ranges and ambient luminance values so the generated scene, runtime director, and audit agree.

## Implementation

- Added `ApplyReferenceSunShadowGradeCycle99` as a scene-generation baseline for warm key light, stronger soft shadows, lower ambient, and subtle outdoor fog.
- Raised outdoor key light strength and shadow strength for house exterior and central plaza profiles.
- Lowered outdoor ambient and fill intensity to increase the light/shadow separation instead of simply darkening the whole picture.
- Kept library darker but readable with the window light path intact.
- Kept interior warm and non-foggy.
- Added `ValidateReferenceSunShadowGradeBatch`.
- Added `CaptureHd2dReferenceSunShadowGradeCycle99ScreenshotsBatch`.

## Expected Evidence

Parent-review screenshots are expected under:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle99_reference_sun_shadow_grade_parent_review_20260524_01`

Expected files:

- `parent_review_01_current_house_exterior_overview.png`
- `parent_review_02_current_central_plaza_library_facade_overview.png`
- `parent_review_03_current_library_reto_desk_close.png`
- `parent_review_04_past_plaza_overview.png`

## Validation

Planned cycle-runner command:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 99 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateReferenceSunShadowGradeBatch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dReferenceSunShadowGradeCycle99ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_reference_sun_shadow_grade_cycle99.md' `
  -Audience parent_review `
  -CommitPath @(
    'Assets/Editor/AnemoraFastVsHouseSliceSetup.cs',
    'Assets/Scripts/FastVS/FastVsHouseLightingDirector.cs',
    'Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs',
    'docs/devlog/2026-05-24_fast_vs_hd2d_reference_sun_shadow_grade_cycle99.md',
    'docs/devlog/INDEX.md',
    'docs/devlog/screenshots/fast_vs_hd2d_cycle99_reference_sun_shadow_grade_parent_review_20260524_01'
  ) `
  -NoRollback
```

## Visual Gate

The desired pass/fail bar is whether the screenshots read as sun-directed and shadow-shaped at a glance. If the result still reads as merely darker, the next cycle should tune shadow overlays/material response and not return to house geometry cleanup.

## Cycle 99 failure (build) -- 20260524-043741

```
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=cdb6e3895c049a8479c86b93236baa6d): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_writing_normal_loop_v02_6f_64x96.png using Guid(61b3cd5f5b4bc2b41953653a46c5c050) (TextureImporter) -> (artifact id: '7ff045c530812383dfde4d98fe46707e') in 0.0038782 seconds
Refreshing native plugins compatible for Editor in 0.59 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=ddbbac78e2dc1e34eb7ee13bbf8ea3c3): Total: 0.029 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0049354 seconds
Refreshing native plugins compatible for Editor in 0.61 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=6e57dde60cbdd004781ddbad1ecdc020): Total: 0.031 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.003686 seconds
Refreshing native plugins compatible for Editor in 0.61 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=29bd2dab97f859c49acd88f347f60245): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0036305 seconds
Refreshing native plugins compatible for Editor in 0.60 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=760ff52536c503d4b9f84d8627a27f3b): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0037853 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=85c19a43c281d664381004c52b34c1e6): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.004448 seconds
Refreshing native plugins compatible for Editor in 0.52 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=cd78386cec55e414aa3fa609e8aeba0a): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0038358 seconds
Refreshing native plugins compatible for Editor in 0.59 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=c253c08ee2ef7c04b8d3a0ea70ee1619): Total: 0.028 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
HD2D material role audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dMaterialRoleFoundationAudit:VerifyMaterialRolesV1 () (at Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs:34)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:257)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:514)

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
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:514)

(Filename: Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs Line: 98)

Shading Foundation v1 audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit:VerifyShadingFoundationV1 () (at Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs:38)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:259)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:514)

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 38)

InvalidOperationException: House slice validation failed: Directional Light shadow strength must stay in the HD-2D decisive shadow balance range.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dThirtySeventhCycleLightingBalance () [0x0007a] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:29294 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x000b3] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:261 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:514 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 29294)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[04:40:09] Phase 'build' FAILED with exit 1
[04:40:09] NoRollback set; preserving worktree after build failure
```

## Cycle 99 failure (build) -- 20260524-044051

```

Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.0040756 seconds
Refreshing native plugins compatible for Editor in 0.64 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=4aa6d784675912d4ebdaa5f9d8b4882f): Total: 0.032 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.0027962 seconds
Refreshing native plugins compatible for Editor in 0.59 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=4563df49aef0bb646a292a002c1453ff): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Fast VS house exterior eave shadow softening validation passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateFastVsHd2dShadowFoundationCycle87HouseExteriorEaveShadowSoftening () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:38572)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:349)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:514)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 38572)

Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_outdoor_void_background.mat using Guid(57539333774454a4b9b68a2662c07ad5) (NativeFormatImporter) -> (artifact id: '99c018d2123986061894ad403d77f6fe') in 0.0034524 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.0023768 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_outdoor_void_background.mat using Guid(0ec66c653876c7244a13d4f32da1526a) (NativeFormatImporter) -> (artifact id: 'e1db2a1427d1c97a066681d67848d63a') in 0.002025 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(efe4ace4ebe298d4bada9fe5c2546e89) (NativeFormatImporter) -> (artifact id: '84254ef753b242d39fea27ca378b0b03') in 0.0019624 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0021484 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.asset using Guid(5aa393eea6e946245b2e701795e51af0) (NativeFormatImporter) -> (artifact id: '184b95cb35aec2eba012f0ac3b8c8e5a') in 0.0024057 seconds
Refreshing native plugins compatible for Editor in 0.88 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=1be7d14ca44729746bec8562fc9dd826): Total: 0.067 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(efe4ace4ebe298d4bada9fe5c2546e89) (NativeFormatImporter) -> (artifact id: 'a69f20e4e842686af22fc83b45a76019') in 0.0028427 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=06657ba7e67d5eb47bcd8656a2a030ab): Total: 0.044 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(0a33e84f626797a40b8d5687454e6620) (NativeFormatImporter) -> (artifact id: '2dfb0ee97db515b87db3e427f999c2a7') in 0.0028967 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.asset using Guid(b43c1d26d8af7144eac532f733d613ed) (NativeFormatImporter) -> (artifact id: '72b93ea802d61590522168319e686932') in 0.0025972 seconds
Refreshing native plugins compatible for Editor in 0.64 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=bebfa51e0550fff45b06ea790ff33007): Total: 0.051 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(0a33e84f626797a40b8d5687454e6620) (NativeFormatImporter) -> (artifact id: '1811c45b24cb5a42b0d119f903087e3b') in 0.0033692 seconds
Refreshing native plugins compatible for Editor in 0.59 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=ddf8ffd56ce9a5d46bf6a6e0fa1ddd6e): Total: 0.048 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.mat using Guid(ba3c455e34bfdef488b2dbb1bdd0cd70) (NativeFormatImporter) -> (artifact id: 'dec47bc9f235a292b061ba7705817a0e') in 0.0030636 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.asset using Guid(6b757f77ea5d7164e8537193c67506bd) (NativeFormatImporter) -> (artifact id: 'edb251d8a3d8ffd6a09591c3a454c8a2') in 0.0030568 seconds
Refreshing native plugins compatible for Editor in 0.64 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=a70fc3f51240d944389aca41609ac6a5): Total: 0.052 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.mat using Guid(ba3c455e34bfdef488b2dbb1bdd0cd70) (NativeFormatImporter) -> (artifact id: '1a5dfe9209479e4411a9a5728bc36da8') in 0.003347 seconds
Refreshing native plugins compatible for Editor in 0.67 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=59d154fd944598249b6592064a52ff23): Total: 0.045 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.mat using Guid(b09502d957a9b5141836a6b6ab31797f) (NativeFormatImporter) -> (artifact id: '3e5b99ed69b07b0978cb63a29b9cb0d4') in 0.0029293 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.asset using Guid(2bb9710db4a50874c944ff2cece89232) (NativeFormatImporter) -> (artifact id: '91e098c066551ce9c7048c5e7b3cb0d8') in 0.0032606 seconds
Refreshing native plugins compatible for Editor in 1.00 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=286b3c09191be104aa64529e1ab05737): Total: 0.057 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.mat using Guid(b09502d957a9b5141836a6b6ab31797f) (NativeFormatImporter) -> (artifact id: 'df71a74559f1cd0ef87c4679dd6814a6') in 0.0028699 seconds
Refreshing native plugins compatible for Editor in 0.83 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=b574a87826179064089c6fce5e90ae46): Total: 0.048 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_occlusion_gradient.mat using Guid(d58f2dd58f72df641b0b13d70b9aaffb) (NativeFormatImporter) -> (artifact id: 'cce592b71f40c96c4099f5295e653a71') in 0.0029029 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_occlusion_gradient_soft.asset using Guid(c77c6612505c47d4aa1dac2b81f7897e) (NativeFormatImporter) -> (artifact id: '0a0b3058b133df474de29a6690dee7ed') in 0.0031807 seconds
Refreshing native plugins compatible for Editor in 0.57 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=8c3d9ba8d3ae2224f87fbaff8b070d2c): Total: 0.038 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
InvalidOperationException: House slice validation failed: house exterior outdoor sky clear color expected RGBA(0.214, 0.282, 0.348, 1.000) but was RGBA(0.234, 0.221, 0.198, 1.000).
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateColorApproximately (UnityEngine.Color actual, UnityEngine.Color expected, System.String label) [0x00064] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:26331 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateOutdoorSkyClearColorForReview () [0x00080] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:26316 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dEightyEighthCycleOutdoorSkyBackdrop () [0x0003c] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:25423 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x002f7] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:377 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:514 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 26316)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[04:44:17] Phase 'build' FAILED with exit 1
[04:44:17] NoRollback set; preserving worktree after build failure
```
