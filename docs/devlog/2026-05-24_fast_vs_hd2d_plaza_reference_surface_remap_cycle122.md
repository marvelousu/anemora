# feat(hd2d): remap plaza surfaces to reference sun

## Scope

Cycle 122 breaks from the small-overlay direction and adds a reference surface remap pass for the current central plaza. Cycle121 removed the legacy ribbon artifacts, but the close frame still read as dark geometry with a foreground light band. This cycle forces the visible stone square, library steps, facade, eave, windows, and door pocket into a clearer HD-2D light/shadow hierarchy.

Authored files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

Expected regenerated materials:

- `FastVS_House_hd2d_plaza_reference_surface_sun_cycle122.mat`
- `FastVS_House_hd2d_plaza_reference_surface_shadow_cycle122.mat`
- `FastVS_House_hd2d_plaza_reference_air_grade_cycle122.mat`

Expected regenerated textures:

- `FastVS_House_hd2d_plaza_reference_surface_sun_cycle122.asset`
- `FastVS_House_hd2d_plaza_reference_surface_shadow_cycle122.asset`
- `FastVS_House_hd2d_plaza_reference_air_grade_cycle122.asset`
- `FastVS_House_current_path_hd2d_plate.asset`

Out of scope:

- Main branch, route/story behavior, map topology, and unrelated Unity-generated settings churn.

## Goal Prompt

Continue toward reference-image shadow quality with speed prioritized. The current plaza needs a more obvious reference-like sunlit stone surface, harder eave/window/door shadows, and a faded air pass.

## Implementation Plan

- Add a high-queue Cycle122 surface-sun material/texture with broad chalky warm stone light rather than thin diagonal bands.
- Add a high-queue Cycle122 surface-shadow material/texture for decisive eave, door-pocket, window, step, and foreground cuts.
- Add a subtle air-grade material/texture to mute the back wall and upper depth.
- Place large surface remap overlays on the current plaza only: stone square, library step, raised side stones, facade rake, upper facade, back wall air, eave, door, windows, step contact, and foreground occlusion.
- Lighten the current path texture slightly so sunlit stone can wash toward the pale HD-2D reference while keeping dark seams.
- Validate material queues, texture alpha metrics, current-only object count, and key profile contracts.
- Capture current overview/close plus past/library guard frames.

## Expected Evidence

Parent-review screenshots are expected under:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle122_plaza_reference_surface_remap_parent_review_20260524_01`

Expected files:

- `parent_review_01_current_central_plaza_reference_surface_remap_overview.png`
- `parent_review_02_current_central_plaza_reference_surface_remap_close.png`
- `parent_review_03_past_central_plaza_reference_surface_remap_guard.png`
- `parent_review_04_current_library_reference_surface_remap_guard.png`

## Validation

Planned cycle-runner command:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 122 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaReferenceSurfaceRemapCycle122Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaReferenceSurfaceRemapCycle122ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_reference_surface_remap_cycle122.md' `
  -Audience parent_review `
  -NoRollback
```

## Visual Gate

Passing criteria:

- Current close frame should show brighter sunlit stone across the main square and library step, not only the foreground band.
- Eave, door, windows, and step contact shadows should read as hard compositional anchors.
- Back wall and upper depth should be slightly faded and warm, not flat black/brown.
- No thin diagonal white legacy ribbon should return.
- Past plaza and library guard frames remain usable.

## Cycle 122 failure (build) -- 20260524-205745

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
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:359)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:616)

(Filename: Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs Line: 98)

Shading Foundation v1 audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit:VerifyShadingFoundationV1 () (at Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs:38)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:360)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:616)

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 38)

HD2D area lighting profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dAreaLightingProfileFoundationAudit:VerifyAreaLightingProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs:142)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:365)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:616)

(Filename: Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs Line: 142)

HD2D overlay profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dOverlayProfileFoundationAudit:VerifyOverlayProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs:402)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:366)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:616)

(Filename: Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs Line: 402)

HD2D surface profile audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceProfileFoundationAudit:VerifySurfaceProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs:104)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:367)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:616)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs Line: 104)

InvalidOperationException: HD2D surface texture metric audit failed:
- Current.HouseExterior.Wall.Facade on Current_HouseExterior_FacadeWallLeftPanel: Current.HouseExterior.Wall.Facade on Current_HouseExterior_FacadeWallLeftPanel average luminance 0.501 is outside review band 0.060-0.470.
- Current.CentralPlaza.Wall.LibraryFacade on Current_CentralPlaza_LibraryNorthFacade: Current.CentralPlaza.Wall.LibraryFacade on Current_CentralPlaza_LibraryNorthFacade average luminance 0.501 is outside review band 0.060-0.470.
  at Anemora.EditorTools.AnemoraFastVsHd2dSurfaceTextureMetricAudit.VerifySurfaceTextureMetricsV1 () [0x0004e] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs:73 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x000d1] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:368 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:616 

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs Line: 73)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[21:00:38] Phase 'build' FAILED with exit 1
[21:00:38] NoRollback set; preserving worktree after build failure
```
