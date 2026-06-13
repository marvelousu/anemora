# 2026-06-13 HD2D point15 far-right strip removal

## Scope

- Fix the isolated right-edge tile / wood-strip fragments visible in the built-player wide CentralPlaza library-facade capture.
- Keep acceptance evidence built-player only.
- Preserve the renderer contract:
  - `RenderingMode=2`
  - `DepthPrimingMode=0`
  - `CopyDepthMode=0`
  - `PortalStencilFeatureActive=True`

## Important correction

The first `farRightStripPixel` attempt was invalid for ownership proof because it ran after `CaptureRendererIsolationClose`, so the camera was still in the close library-facade pose.

The probe was corrected to call `PositionRendererIsolationWideCamera` before ROI and pixel ownership logging. The valid ownership proof begins with:

- `docs\review\2026-06-13T05-15_far_right_strip_wide_pixel_probe`
- `Logs\point15_far_right_strip_wide_pixel_probe_20260613T051301.log`

## Code change

- `Assets\Scripts\FastVS\FastVsHouseRuntimeSmokeProbe.cs`
  - Added `PositionRendererIsolationWideCamera`.
  - Reused it from `CaptureRendererIsolationWide`.
  - Repositioned the camera back to the wide view before ROI/pixel contributor logging.
  - Replaced the old far-right sample points with visible component points:
    - `StoneTop=(1017,185)`
    - `StoneUpper=(1058,242)`
    - `StoneMid=(1113,322)`
    - `StoneLower=(1183,395)`
    - `WoodTop=(1096,226)`
    - `WoodMid=(1156,292)`
    - `WoodLower=(1209,355)`
- `Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
  - Removed these CentralPlaza generated objects for both current and past scenes:
    - `CentralPlaza_Cycle53_PerimeterWorld_EastLowRidgeA`
    - `CentralPlaza_Cycle55_HorizonSilhouette_EastGroundCarryA`
    - `CentralPlaza_Cycle54_EdgeBreakup_EastPathShelfA`
  - Removed the matching validation expectations.
  - Left HouseExterior and west/back CentralPlaza perimeter pieces untouched.

## Ownership proof

Built-player wide pixel probe:

- Review folder:
  - `docs\review\2026-06-13T05-15_far_right_strip_wide_pixel_probe`
- Player log:
  - `Logs\point15_far_right_strip_wide_pixel_probe_20260613T051301.log`
- Result:
  - `PLAYER_EXIT=0`
  - `PNG_COUNT=86`
  - `PNG_TOTAL_BYTES=75602907`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Baseline samples before removal:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: farRightStripPixelBaseline phase=baseline points=[StoneTop=(1017,185):(113,103,79,255) | StoneUpper=(1058,242):(107,97,74,255) | StoneMid=(1113,322):(64,58,47,255) | StoneLower=(1183,395):(116,105,80,255) | WoodTop=(1096,226):(98,87,62,255) | WoodMid=(1156,292):(124,110,77,255) | WoodLower=(1209,355):(111,99,70,255)]
```

Primary contributors:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: farRightStripPixelContributor phase=baseline rank=1 maxDelta=103 totalDelta=258 deltas=[StoneTop=0:(113,103,79,255),StoneUpper=0:(107,97,74,255),StoneMid=0:(64,58,47,255),StoneLower=0:(116,105,80,255),WoodTop=69:(61,73,80,255),WoodMid=103:(61,73,80,255),WoodLower=86:(61,73,80,255)] path="FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Cycle55_HorizonSilhouette_EastGroundCarryA" mat=name=FastVS_House_current_path,shader=Anemora/FastVS/SurfaceRampLit,queue=2000,tagQueue=Geometry,tagRenderType=Opaque,_Surface=<missing>,_Cull=<missing>,_ZWrite=<missing>,_ZTest=<missing>,_AlphaClip=<missing>,_SrcBlend=<missing>,_DstBlend=<missing>,_Color=<missing>,_BaseColor=(1,1,1,1)
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: farRightStripPixelContributor phase=baseline rank=2 maxDelta=83 totalDelta=223 deltas=[StoneTop=83:(61,73,80,255),StoneUpper=76:(61,73,80,255),StoneMid=51:(61,73,80,255),StoneLower=13:(121,109,76,255),WoodTop=0:(98,87,62,255),WoodMid=0:(124,110,77,255),WoodLower=0:(111,99,70,255)] path="FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Cycle53_PerimeterWorld_EastLowRidgeA" mat=name=FastVS_House_current_stone,shader=Anemora/FastVS/SurfaceRampLit,queue=2000,tagQueue=Geometry,tagRenderType=Opaque,_Surface=<missing>,_Cull=<missing>,_ZWrite=<missing>,_ZTest=<missing>,_AlphaClip=<missing>,_SrcBlend=<missing>,_DstBlend=<missing>,_Color=<missing>,_BaseColor=(1,1,1,1)
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: farRightStripPixelContributorSummary phase=baseline candidateRenderers=8 positive=2 logged=2
```

After removing `Cycle53` and `Cycle55`, one lower fragment remained:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: farRightStripPixelBaseline phase=baseline points=[StoneTop=(1017,185):(61,73,80,255) | StoneUpper=(1058,242):(61,73,80,255) | StoneMid=(1113,322):(61,73,80,255) | StoneLower=(1183,395):(121,109,76,255) | WoodTop=(1096,226):(61,73,80,255) | WoodMid=(1156,292):(61,73,80,255) | WoodLower=(1209,355):(61,73,80,255)]
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: farRightStripPixelContributor phase=baseline rank=1 maxDelta=100 totalDelta=100 deltas=[StoneTop=0:(61,73,80,255),StoneUpper=0:(61,73,80,255),StoneMid=0:(61,73,80,255),StoneLower=100:(61,73,80,255),WoodTop=0:(61,73,80,255),WoodMid=0:(61,73,80,255),WoodLower=0:(61,73,80,255)] path="FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Cycle54_EdgeBreakup_EastPathShelfA" mat=name=FastVS_House_current_path,shader=Anemora/FastVS/SurfaceRampLit,queue=2000,tagQueue=Geometry,tagRenderType=Opaque,_Surface=<missing>,_Cull=<missing>,_ZWrite=<missing>,_ZTest=<missing>,_AlphaClip=<missing>,_SrcBlend=<missing>,_DstBlend=<missing>,_Color=<missing>,_BaseColor=(1,1,1,1)
```

`Cycle54_EdgeBreakup_EastPathShelfA` was then removed as the final target.

## Build evidence

Final generator rebuild:

- Method:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Log:
  - `Logs\unity_build_validate_far_right_strip_removed_v2_20260613T052338.log`
- Result:

```text
UNITY_EXIT=0
Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity
Build Finished, Result: Success.
Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe
```

Built-player executable:

```text
FullName      : C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe
Length        : 667648
LastWriteTime : 6/13/2026 5:28:55 AM
```

## Final built-player proof

Renderer isolation capture after all three removals:

- Review folder:
  - `docs\review\2026-06-13T05-34_far_right_strip_removed_v2`
- Player log:
  - `Logs\point15_far_right_strip_removed_v2_20260613T052932.log`
- Result:
  - `PLAYER_EXIT=0`
  - `PNG_COUNT=86`
  - `PNG_TOTAL_BYTES=73189403`

Contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Final target pixel result:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: farRightStripPixelBaseline phase=baseline points=[StoneTop=(1017,185):(61,73,80,255) | StoneUpper=(1058,242):(61,73,80,255) | StoneMid=(1113,322):(61,73,80,255) | StoneLower=(1183,395):(61,73,80,255) | WoodTop=(1096,226):(61,73,80,255) | WoodMid=(1156,292):(61,73,80,255) | WoodLower=(1209,355):(61,73,80,255)]
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: farRightStripPixelContributorSummary phase=baseline candidateRenderers=5 positive=0 logged=0
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: end count=86
```

Visual crop evidence:

- `docs\review\2026-06-13T05-34_far_right_strip_removed_v2\01_baseline_current_plaza_library_facade_right_edge_crop.png`
- `docs\review\2026-06-13T05-34_far_right_strip_removed_v2\01_baseline_current_plaza_library_facade_right_edge_after_components.png`

Image analysis:

```text
RIGHT_EDGE_DIFF_FROM_WIDE_PROBE mean [6.434425837320574, 4.192302631578947, 2.3407057416267945] nonzero 25751 max ((0, 98), (0, 72), (0, 55))
AFTER_COMPONENTS [(16389, (900, 228, 1042, 516)), (2219, (900, 519, 1002, 559)), (137, (900, 318, 911, 334))]
```

The previous right-side detached component bboxes are gone. Remaining warm components in the crop are left-side in-map plaza structures.

## All-map guard capture

Built-player all-map capture after the generator change:

- Review folder:
  - `docs\review\2026-06-13T05-38_far_right_strip_removed_v2_allmaps`
- Player log:
  - `Logs\point15_far_right_strip_removed_v2_allmaps_20260613T053058.log`
- Result:
  - `PLAYER_EXIT=0`
  - `PNG_COUNT=13`
  - `PNG_TOTAL_BYTES=8330026`

Contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
ANEMORA_HOUSE_SLICE_CAPTURE: end count=13
```

Lighting ready state:

```text
ANEMORA_HOUSE_SLICE_LIGHTING_STATE: label=capture.ready expectedArea=CentralPlaza activeArea=CentralPlaza sunCurrent=Morning sunTarget=Morning sunTransitioning=False indoorSunSuppression=False main=name=Directional Light,enabled=True,type=Directional,intensity=1.500,color=(1.000,0.840,0.700,1.000),shadows=Soft,shadowStrength=0.800,shadowBias=0.012,shadowNormalBias=0.100 warm=name=FastVS_HD2D_WarmFillLight,enabled=True,type=Point,intensity=0.300,color=(1.000,0.640,0.340,1.000),shadows=None,shadowStrength=1.000,shadowBias=0.050,shadowNormalBias=0.400 cool=name=FastVS_HD2D_CoolRimLight,enabled=True,type=Directional,intensity=0.160,color=(0.460,0.580,0.920,1.000),shadows=None,shadowStrength=1.000,shadowBias=0.050,shadowNormalBias=0.400 ambientMode=Flat ambient=(0.420,0.380,0.320,1.000) ambientMax=0.420 fog=False fogColor=(0.910,0.770,0.590,1.000) fogDensity=0.011 shadowPolicy=renderers=11823,active=1599,enabled=11743,castOn=801,shadowsOnly=46,receive=1459,activeCastOn=64,activeReceive=176
```

## Follow-up note

The wide library-facade right-edge strip is removed. The all-map CentralPlaza view still contains other perimeter continuation pieces near the lower/front map edge; those are separate from this right-edge strip proof and should be diagnosed in a later slice if Tom flags them.
