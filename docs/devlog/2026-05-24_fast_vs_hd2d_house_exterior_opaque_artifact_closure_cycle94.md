# feat(hd2d): close house exterior opaque artifacts

## Scope

Cycle 94 follows the Cycle 93 parent PNG review. Cycle 93 reduced the original sky bars but still left the house exterior reading as a staged set: a large bluish transparent front face and broad black void slabs remained visible around the closed door, porch, and side edges. This cycle stays focused on the house exterior facade before returning to broader sun, atmosphere, or background work.

Authored file:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Side-effect files:

- None

SCOPED_PROMPT_ISSUED cycle=94 authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle94ScreenshotsBatch`

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - None
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle94ScreenshotsBatch
notes: Kept all changes inside the single authored file, added the new opaque closure and cycle 94 capture flow, and avoided touching scenes, materials, or any out-of-scope paths.
```

## Implementation Plan

- Shrink the house exterior framed-light planes so they no longer read as large transparent walls:
  - under-eave occlusion becomes a narrow band,
  - right side light becomes a small edge accent,
  - porch leading light becomes a thin floor/bounce strip.
- Add a Cycle94 opaque artifact-closure helper after the Cycle93 leak-closure pass.
- Use only existing opaque wall, stone, and furniture materials for the broad visible closure pieces.
- Add Current/Past front upper apron, lower left/right foundation fills, front plinth skirt, and roof underside return cap.
- Validate that broad closure pieces are non-arrival, collider-free, parented correctly, within expected transforms, and do not use shadow, doorway, occlusion, light-pool, warm-stage, or sky-bar materials.
- Capture focused house exterior screenshots for visual comparison against Cycle93.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle94ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle94_house_exterior_opaque_artifact_closure_parent_review_20260524_01\parent_review_01_current_house_exterior_opaque_closure_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle94_house_exterior_opaque_artifact_closure_parent_review_20260524_01\parent_review_02_current_house_exterior_opaque_closure_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle94_house_exterior_opaque_artifact_closure_parent_review_20260524_01\parent_review_03_current_house_exterior_opaque_closure_upper_facade.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle94_house_exterior_opaque_artifact_closure_parent_review_20260524_01\parent_review_04_past_house_exterior_opaque_closure_overview.png`

## Parent Review Notes

The visual gate is whether the house front now reads as an intentionally closed facade rather than a stage with transparent overlays and black backing slabs. A passing validation remains necessary but is not visual sign-off.

## Cycle 94 failure (validate) -- 20260524-023113

```
HD2D overlay profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dOverlayProfileFoundationAudit:VerifyOverlayProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs:402)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:265)

(Filename: Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs Line: 402)

HD2D surface profile audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceProfileFoundationAudit:VerifySurfaceProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs:104)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:266)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs Line: 104)

HD2D surface texture metric audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceTextureMetricAudit:VerifySurfaceTextureMetricsV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs:72)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:267)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs Line: 72)

HD2D lighting transition audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dLightingTransitionAudit:VerifyLightingTransitionV1 () (at Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs:106)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:268)

(Filename: Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs Line: 106)

Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.003563 seconds
Refreshing native plugins compatible for Editor in 0.70 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=0b7d75fc3ea6fa64baa621877e33e1f6): Total: 0.044 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.0038279 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=96152016d3059a14c8bd76762601a609): Total: 0.029 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Fast VS house exterior eave shadow softening validation passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateFastVsHd2dShadowFoundationCycle87HouseExteriorEaveShadowSoftening () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:37195)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:345)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 37195)

InvalidOperationException: House slice validation failed: Current_HouseExterior_FramedLightPlanes_UnderEaveOcclusionGradientA local position expected within (-1.10, 1.84, -1.42) and (-0.98, 1.92, -1.30), but got (-1.04, 2.00, -1.48).
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateVectorWithinRange (System.String label, UnityEngine.Vector3 actual, UnityEngine.Vector3 minInclusive, UnityEngine.Vector3 maxInclusive) [0x00054] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:41533 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseExteriorFacadeBackdropReadabilityObject (System.String objectName, System.String expectedMaterialToken, System.String expectedParentName, UnityEngine.Vector3 minLocalPosition, UnityEngine.Vector3 maxLocalPosition, UnityEngine.Vector3 minLocalScale, UnityEngine.Vector3 maxLocalScale) [0x001ab] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:38907 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dShadowFoundationCycle78HouseDoorLightPlaneClearance () [0x0004f] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:36208 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x002a7] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:361 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 36208)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[02:41:32] Phase 'validate' FAILED with exit 1
[02:41:32] NoRollback set; preserving worktree after validate failure
```
