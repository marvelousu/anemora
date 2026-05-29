# feat(hd2d): rebuild plaza reference diorama shadow

## Intent

Cycle 125 stops adding small corrections on top of the accumulated plaza lighting stack. The Cycle 124 result still had broad horizontal white and black bands, visible hard grid seams in the close shot, and a weak reference-like relation between sunlit stone, contact shadow, and backlit air.

This cycle rebuilds the current central plaza lighting read around a simpler HD-2D diorama target: pale desaturated receiver stone, compact dark contact shadows, and a warmer back air layer.

## Scope

- Add current-only `CreateCentralPlazaReferenceDioramaShadowCycle125`.
- Disable the active current central-plaza lighting overlays from the older shadow/sun cycles that were fighting each other visually.
- Add 12 current-only Cycle 125 overlays:
  - broad matte stone sun receivers,
  - close-frame seam softening,
  - facade sun patch,
  - compact left/right/eave/player contact shadows,
  - rear haze and a high sunbeam column.
- Add new generated Cycle 125 sun, shadow, and air materials/textures with higher render queues than Cycle 122-124.
- Lighten the generated current path plate and reduce dark seam dominance so close captures do not read as a black tile grid before lighting is evaluated.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 125 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaReferenceDioramaShadowCycle125Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaReferenceDioramaShadowCycle125ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_reference_diorama_shadow_cycle125.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle125_plaza_reference_diorama_shadow_parent_review_20260524_01`

- `parent_review_01_current_central_plaza_reference_surface_remap_overview.png`
- `parent_review_02_current_central_plaza_reference_surface_remap_close.png`
- `parent_review_03_past_central_plaza_reference_surface_remap_guard.png`
- `parent_review_04_current_library_reference_surface_remap_guard.png`

## Visual Gate

- Current plaza should no longer be dominated by the old horizontal sun/shadow bands.
- The overview should show a clear pale sunlit receiver with short, readable dark casts.
- The close shot should have weaker stone grid darkness and stronger lighting hierarchy.
- Past plaza and current library guard captures should remain unchanged in intent.

## Cycle 125 failure (build) -- 20260524-221639

```
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

InvalidOperationException: HD2D surface texture metric audit failed:
- Current.HouseExterior.Path.ToInterior on Current_HouseExterior_PathToInterior: Current.HouseExterior.Path.ToInterior on Current_HouseExterior_PathToInterior average luminance 0.511 is outside review band 0.060-0.460.
- Current.CentralPlaza.Ground.Paving on Current_CentralPlaza_StoneSquare: Current.CentralPlaza.Ground.Paving on Current_CentralPlaza_StoneSquare average luminance 0.511 is outside review band 0.060-0.470.
  at Anemora.EditorTools.AnemoraFastVsHd2dSurfaceTextureMetricAudit.VerifySurfaceTextureMetricsV1 () [0x0004e] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs:73 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x000d1] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:377 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:625 

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs Line: 73)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[22:20:19] Phase 'build' FAILED with exit 1
[22:20:19] NoRollback set; preserving worktree after build failure
```
