# feat(hd2d): reset plaza solar shadow grade

## Scope

Cycle 116 replaces the incomplete sunlit-chisel attempt with a broader central-plaza lighting reset. The user feedback after Cycle115/Cycle116 was that the image was still not changing enough, so this cycle intentionally breaks the previous small-overlay assumption and changes the plaza light profile, camera grade, depth-of-field contract, and generated sun/shadow/air overlays together.

Authored files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsHouseLightingDirector.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dShadingFoundationAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_map_move_floor_glow.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_route_move_floor_glow.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_past_map_move_floor_glow.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_past_route_move_floor_glow.mat`

Expected generated assets:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_solar_reset_sun_cycle116.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_solar_reset_shadow_cycle116.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_solar_reset_air_cycle116.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_plaza_solar_reset_sun_cycle116.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_plaza_solar_reset_shadow_cycle116.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_plaza_solar_reset_air_cycle116.asset`

Out of scope:

- Main branch, story/UI behavior, route logic, house facade closure work, map redesign, and unrelated ProjectSettings churn.

## Goal Prompt

Make the current central plaza read closer to the reference HD-2D target in one visible step: stronger warm sunlight, lower ambient floor, much deeper directional shadow impression, shorter warmer fog, a faded camera grade, and controllable Gaussian depth-of-field. Do not continue the old small chisel highlight approach if it cannot create a visible change.

Reference directory:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference`

## Implementation Plan

- Rebase the central plaza lighting profile around a lower sun elevation, hotter key light, full shadow strength, darker ambient, minimal fill, and warmer close fog.
- Update the runtime lighting director and area-profile audit to agree on the new central-plaza contract.
- Move the volume/camera grade from the earlier readability-only baseline to a more reference-like faded contrast grade with controlled Gaussian depth-of-field.
- Replace the incomplete Cycle116 chisel artifact with three generated overlay families: sun floor/facade cuts, broad dark shadow bands, and warm air shafts/veil.
- De-emphasize route/move glow pads so the brighter sun grade is not dominated by opaque orange gameplay markers.
- Add validation for the new overlay object count, material/texture ownership, texture alpha metrics, and central-plaza lighting/postprocess contract.
- Preserve guard captures for past plaza and current library so the reset is scoped to the current central plaza review path.

## Expected Evidence

Parent-review screenshots are expected under:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle116_plaza_solar_shadow_reset_parent_review_20260524_01`

Expected files:

- `parent_review_01_current_central_plaza_solar_shadow_reset_overview.png`
- `parent_review_02_current_central_plaza_solar_shadow_reset_close.png`
- `parent_review_03_past_central_plaza_solar_shadow_reset_guard.png`
- `parent_review_04_current_library_solar_shadow_reset_guard.png`

## Validation

Planned cycle-runner command:

```powershell
$commitPaths = @(
  'Assets/Scripts/FastVS/FastVsHouseLightingDirector.cs',
  'Assets/Editor/AnemoraFastVsHouseSliceSetup.cs',
  'Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs',
  'Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_map_move_floor_glow.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_route_move_floor_glow.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_map_move_floor_glow.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_route_move_floor_glow.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_solar_reset_sun_cycle116.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_solar_reset_sun_cycle116.mat.meta',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_solar_reset_shadow_cycle116.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_solar_reset_shadow_cycle116.mat.meta',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_solar_reset_air_cycle116.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_solar_reset_air_cycle116.mat.meta',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_solar_reset_sun_cycle116.asset',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_solar_reset_sun_cycle116.asset.meta',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_solar_reset_shadow_cycle116.asset',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_solar_reset_shadow_cycle116.asset.meta',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_solar_reset_air_cycle116.asset',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_solar_reset_air_cycle116.asset.meta',
  'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_solar_shadow_reset_cycle116.md',
  'docs/devlog/INDEX.md',
  'docs/devlog/screenshots/fast_vs_hd2d_cycle116_plaza_solar_shadow_reset_parent_review_20260524_01'
)
& .\tools\cycle-runner.ps1 -CycleNumber 116 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSolarShadowResetCycle116Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaSolarShadowResetCycle116ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_solar_shadow_reset_cycle116.md' `
  -Audience parent_review `
  -CommitPath $commitPaths `
  -NoRollback
```

## Visual Gate

Passing criteria:

- Current plaza overview must show a visible difference from Cycle115: darker grounded shadow masses and stronger warm sun fields in the same frame.
- The image should read more faded and atmospheric, not merely dimmer.
- Niro and the library approach route remain readable.
- Past plaza and current library guard captures remain usable and are not accidentally redesigned.

## Parent Review Preflight

- Manual validate preflight passed with `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaSolarShadowResetCycle116Batch`.
- Manual capture preflight passed with `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaSolarShadowResetCycle116ScreenshotsBatch`.
- Parent visual check accepted the reset direction after reducing the route glow pads: the current plaza now has a visibly stronger warm-sun / dark-shadow separation than Cycle115 while preserving route and guard readability.

## Cycle 116 failure (build) -- 20260524-125245

```
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=4dec8ae224929b5478963c2e4001b610): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_writing_normal_loop_v02_6f_64x96.png using Guid(61b3cd5f5b4bc2b41953653a46c5c050) (TextureImporter) -> (artifact id: '7ff045c530812383dfde4d98fe46707e') in 0.0042898 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=52c299a3076b4844f9dcfcd6bf438939): Total: 0.028 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0039539 seconds
Refreshing native plugins compatible for Editor in 0.73 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=5208edbdbfe8e5c4882ea6f2ac774203): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0039604 seconds
Refreshing native plugins compatible for Editor in 0.61 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=83609224b0b21c242b8b222ac858d315): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0035782 seconds
Refreshing native plugins compatible for Editor in 0.54 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=49791c05fbe7acd4fa63e3ad61dca096): Total: 0.047 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0043482 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=1282f77e38bd3fb46a972add1c265b00): Total: 0.028 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0036452 seconds
Refreshing native plugins compatible for Editor in 0.59 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=e49cfa5432b297841ae8d2834f0afc29): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0039263 seconds
Refreshing native plugins compatible for Editor in 0.53 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=bc1dbd9f84ce8fa46b43c6c7001c5bf0): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
HD2D material role audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dMaterialRoleFoundationAudit:VerifyMaterialRolesV1 () (at Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs:34)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:307)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:565)

(Filename: Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs Line: 34)

HD2D sprite card lighting audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSpriteCardLightingAudit:VerifySpriteCardLightingV1 () (at Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs:98)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:308)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:565)

(Filename: Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs Line: 98)

Shading Foundation v1 audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit:VerifyShadingFoundationV1 () (at Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs:38)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:309)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:565)

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 38)

InvalidOperationException: House slice validation failed: ambient light must remain inside the HD-2D shading foundation profile range, found (0.074, 0.066, 0.054).
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dFirstCycleVisuals () [0x00123] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:29737 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x000ae] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:310 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:565 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 29737)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[12:56:18] Phase 'build' FAILED with exit 1
[12:56:18] NoRollback set; preserving worktree after build failure
```

## Cycle 116 failure (build) -- 20260524-125835

```
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:565)

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 38)

HD2D area lighting profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dAreaLightingProfileFoundationAudit:VerifyAreaLightingProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs:142)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:314)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:565)

(Filename: Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs Line: 142)

HD2D overlay profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dOverlayProfileFoundationAudit:VerifyOverlayProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs:402)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:315)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:565)

(Filename: Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs Line: 402)

HD2D surface profile audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceProfileFoundationAudit:VerifySurfaceProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs:104)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:316)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:565)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs Line: 104)

HD2D surface texture metric audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceTextureMetricAudit:VerifySurfaceTextureMetricsV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs:72)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:317)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:565)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs Line: 72)

HD2D lighting transition audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dLightingTransitionAudit:VerifyLightingTransitionV1 () (at Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs:106)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:318)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:565)

(Filename: Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs Line: 106)

InvalidOperationException: House slice validation failed: cycle 92 color adjustments must stay in the faded dusk range, found postExposure=-0.160, saturation=-22.000, contrast=9.000, filter=(1.080, 0.980, 0.860).
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dShadowFoundationCycle92FadedDuskCameraGrade () [0x001fc] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:26075 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x0014e] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:342 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:565 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 26075)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[13:01:36] Phase 'build' FAILED with exit 1
[13:01:36] NoRollback set; preserving worktree after build failure
```
