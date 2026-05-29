# feat(hd2d): tune global realtime camera exposure

## Intent

Cycle 161 proved that realtime light/shadow is no longer central-plaza-only, but the parent review still exposed visible problems: the exterior capture was too high and gray, and the library read as black-crushed rather than deliberately shadowed.

Cycle 162 makes the global lighting change visible by correcting VS follow camera profiles, giving Exterior a procedural skybox, lifting Library realtime exposure, and raising the realtime receiver direct-light response across the global policy.

## Scope

- Lower and tighten the Exterior follow camera so review is not dominated by a roof/top-down crop.
- Pull back the Library follow camera for a more VS-like readable view.
- Replace Exterior's flat gray solid background with a runtime procedural skybox.
- Lift Library realtime sun intensity and ambient floor while keeping shadows strong.
- Raise realtime surface receiver direct-light and shadow response for Exterior, CentralPlaza, and Library.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 162 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateGlobalRealtimeCameraExposureCycle162Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureGlobalRealtimeCameraExposureCycle162ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_global_realtime_camera_exposure_cycle162.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle162_global_realtime_camera_exposure_parent_review_20260525_01`

- `parent_review_01_current_exterior_realtime_light.png`
- `parent_review_02_current_central_plaza_realtime_light.png`
- `parent_review_03_current_library_realtime_light.png`
- `parent_review_04_current_exterior_realtime_shadow_receiver.png`
- `parent_review_05_current_library_realtime_shadow_receiver.png`

## Visual Gate

- Exterior should no longer look like a flat gray-sky top-down diagnostic crop.
- Library should keep strong cast shadows but remain readable.
- The same realtime receiver/caster rule must remain active in Exterior, CentralPlaza, and Library.

## Cycle 162 failure (build) -- 20260525-095255

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

InvalidOperationException: House slice validation failed: exterior follow camera position offset expected (0.90, 3.85, -7.10), but got (0.70, 2.85, -5.25).
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateVectorNear (System.String label, UnityEngine.Vector3 actual, UnityEngine.Vector3 expected) [0x0000e] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:53544 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dThirtyFourthCycleHouseExteriorHeroReadability () [0x007df] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:51432 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x00185] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:413 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:625 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 51432)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[09:57:08] Phase 'build' FAILED with exit 1
[09:57:08] NoRollback set; preserving worktree after build failure
```
