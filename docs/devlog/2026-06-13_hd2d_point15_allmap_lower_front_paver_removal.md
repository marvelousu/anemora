# 2026-06-13 HD2D point15 all-map lower/front paver removal

## Scope

- Remove the two detached lower/front CentralPlaza all-map paver fragments proven by built-player pixel contributor logging.
- Keep the larger front apron / diagonal entry structures diagnostic-only in this slice.
- Keep acceptance evidence built-player only.
- Preserve the renderer contract:
  - `RenderingMode=2`
  - `DepthPrimingMode=0`
  - `CopyDepthMode=0`
  - `PortalStencilFeatureActive=True`

## Ownership proof before removal

Built-player lower/front pixel probe:

- Review folder:
  - `docs\review\2026-06-13T06-05_allmap_lower_front_probe`
- Player log:
  - `Logs\point15_allmap_lower_front_probe_20260613T060400.log`
- Result:
  - `PLAYER_EXIT=0`
  - `PNG_COUNT=13`
  - `PNG_TOTAL_BYTES=8330121`

Baseline samples:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: allMapLowerFrontPixelBaseline phase=CentralPlaza.current points=[BottomRightTile=(963,706):(135,123,91,255) | BottomLeftTile=(117,707):(112,101,78,255) | FrontRightDiagonal=(865,552):(114,102,72,255) | FrontCenterBrick=(586,577):(67,61,49,255)]
```

Primary contributors:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: allMapLowerFrontPixelContributor phase=CentralPlaza.current rank=1 maxDelta=276 totalDelta=276 deltas=[BottomRightTile=276:(26,30,17,255),BottomLeftTile=0:(112,101,78,255),FrontRightDiagonal=0:(114,102,72,255),FrontCenterBrick=0:(67,61,49,255)] path="FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Cycle54_EdgeBreakup_FrontBrokenPaverB" mat=name=FastVS_House_current_stone,shader=Anemora/FastVS/SurfaceRampLit,queue=2000,tagQueue=Geometry,tagRenderType=Opaque,_Surface=<missing>,_Cull=<missing>,_ZWrite=<missing>,_ZTest=<missing>,_AlphaClip=<missing>,_SrcBlend=<missing>,_DstBlend=<missing>,_Color=<missing>,_BaseColor=(1,1,1,1)
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: allMapLowerFrontPixelContributor phase=CentralPlaza.current rank=2 maxDelta=212 totalDelta=212 deltas=[BottomRightTile=0:(135,123,91,255),BottomLeftTile=212:(29,32,18,255),FrontRightDiagonal=0:(114,102,72,255),FrontCenterBrick=0:(67,61,49,255)] path="FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Cycle54_EdgeBreakup_FrontBrokenPaverA" mat=name=FastVS_House_current_stone,shader=Anemora/FastVS/SurfaceRampLit,queue=2000,tagQueue=Geometry,tagRenderType=Opaque,_Surface=<missing>,_Cull=<missing>,_ZWrite=<missing>,_ZTest=<missing>,_AlphaClip=<missing>,_SrcBlend=<missing>,_DstBlend=<missing>,_Color=<missing>,_BaseColor=(1,1,1,1)
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: allMapLowerFrontPixelContributorSummary phase=CentralPlaza.current candidateRenderers=8 positive=4 logged=4
```

Comparison-only contributors in the same probe:

- `Current_CentralPlaza_Cycle62_OuterGroundSkirt_SouthEdgeGroundShoulderA`
- `Current_CentralPlaza_Chapter1_B_Cycle39_B3_DiagonalEntry`

Those remain in place because this slice targeted only the detached lower-left and lower-right paver fragments.

## Code change

- `Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
  - Removed these generated objects for both current and past CentralPlaza scenes:
    - `CentralPlaza_Cycle54_EdgeBreakup_FrontBrokenPaverA`
    - `CentralPlaza_Cycle54_EdgeBreakup_FrontBrokenPaverB`
  - Removed the four matching validation expectations.
- `Assets\Scripts\FastVS\FastVsHouseRuntimeSmokeProbe.cs`
  - Kept the data-only lower/front pixel contributor probe in the capture path for post-removal verification.

## Build evidence

The first generator rebuild attempts failed in batchmode with `-nographics` during APV initialization:

- `Logs\unity_build_validate_allmap_lower_front_removed_20260613T060805.log`
- `Logs\unity_build_validate_allmap_lower_front_removed_retry_20260613T061120.log`

Observed failure:

```text
ArgumentException: Kernel 'UploadData' not found.
RenderTexture.Create failed
Failed to set the active render target, ensure that it is a valid render target.
```

The same `BuildAndValidateBatch` succeeded after removing only `-nographics` from the Unity batch invocation:

- Method:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Log:
  - `Logs\unity_build_validate_allmap_lower_front_removed_graphics_20260613T061247.log`
- Result:

```text
UNITY_EXIT=0
Build Finished, Result: Success.
Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe
```

Built-player executable:

```text
FullName      : C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe
Length        : 667648
LastWriteTime : 6/13/2026 6:18:14 AM
```

## Final built-player proof

Built-player all-map capture after removal:

- Review folder:
  - `docs\review\2026-06-13T06-25_allmap_lower_front_removed`
- Player log:
  - `Logs\point15_allmap_lower_front_removed_20260613T061926.log`
- Result:
  - `PLAYER_EXIT=0`
  - `PNG_COUNT=13`
  - `PNG_TOTAL_BYTES=8304794`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Post-removal samples:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: allMapLowerFrontPixelBaseline phase=CentralPlaza.current points=[BottomRightTile=(963,706):(26,30,17,255) | BottomLeftTile=(117,707):(29,32,18,255) | FrontRightDiagonal=(865,552):(114,102,72,255) | FrontCenterBrick=(586,577):(67,61,49,255)]
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: allMapLowerFrontPixelContributorSummary phase=CentralPlaza.current candidateRenderers=6 positive=3 logged=3
ANEMORA_HOUSE_SLICE_CAPTURE: end count=13
```

The removed paver objects no longer appear in the contributor list. The former paver pixels now resolve to dark ground / underlying `Current_CentralPlaza_PixelGround`.

Remaining contributors after removal:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: allMapLowerFrontPixelContributor phase=CentralPlaza.current rank=1 maxDelta=223 totalDelta=419 deltas=[BottomRightTile=223:(118,105,73,255),BottomLeftTile=196:(109,97,69,255),FrontRightDiagonal=0:(114,102,72,255),FrontCenterBrick=0:(67,61,49,255)] path="FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_PixelGround" mat=name=FastVS_House_current_grass,shader=Anemora/FastVS/SurfaceRampLit,queue=2000,tagQueue=Geometry,tagRenderType=Opaque,_Surface=<missing>,_Cull=<missing>,_ZWrite=<missing>,_ZTest=<missing>,_AlphaClip=<missing>,_SrcBlend=<missing>,_DstBlend=<missing>,_Color=<missing>,_BaseColor=(1,1,1,1)
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: allMapLowerFrontPixelContributor phase=CentralPlaza.current rank=2 maxDelta=93 totalDelta=93 deltas=[BottomRightTile=0:(26,30,17,255),BottomLeftTile=0:(29,32,18,255),FrontRightDiagonal=0:(114,102,72,255),FrontCenterBrick=93:(107,96,67,255)] path="FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Cycle62_OuterGroundSkirt_SouthEdgeGroundShoulderA" mat=name=FastVS_House_current_stone,shader=Anemora/FastVS/SurfaceRampLit,queue=2000,tagQueue=Geometry,tagRenderType=Opaque,_Surface=<missing>,_Cull=<missing>,_ZWrite=<missing>,_ZTest=<missing>,_AlphaClip=<missing>,_SrcBlend=<missing>,_DstBlend=<missing>,_Color=<missing>,_BaseColor=(1,1,1,1)
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: allMapLowerFrontPixelContributor phase=CentralPlaza.current rank=3 maxDelta=9 totalDelta=9 deltas=[BottomRightTile=0:(26,30,17,255),BottomLeftTile=0:(29,32,18,255),FrontRightDiagonal=9:(110,99,70,255),FrontCenterBrick=0:(67,61,49,255)] path="FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Chapter1_B_Cycle39_B3_DiagonalEntry" mat=name=FastVS_House_current_path,shader=Anemora/FastVS/SurfaceRampLit,queue=2000,tagQueue=Geometry,tagRenderType=Opaque,_Surface=<missing>,_Cull=<missing>,_ZWrite=<missing>,_ZTest=<missing>,_AlphaClip=<missing>,_SrcBlend=<missing>,_DstBlend=<missing>,_Color=<missing>,_BaseColor=(1,1,1,1)
```

Image evidence:

- `docs\review\2026-06-13T06-25_allmap_lower_front_removed\03_b1_b3_current_lower_front_crop.png`
- `docs\review\2026-06-13T06-25_allmap_lower_front_removed\03_b1_b3_current_lower_front_components.png`
- `docs\review\2026-06-13T06-25_allmap_lower_front_removed\03_b1_b3_current_lower_front_diff_from_probe.png`

Diff measurement against the pre-removal probe capture:

```text
LOWER_FRONT_DIFF nonzero=8360 meanMaxChannel=63.18504784689 max=118 maxPt=126,714
```

## Follow-up note

The detached lower-left and lower-right paver fragments are removed. The larger front apron / diagonal entry and lower path continuation still exist and should be treated as a separate diagnostic or design decision, not as part of this paver-removal slice.
