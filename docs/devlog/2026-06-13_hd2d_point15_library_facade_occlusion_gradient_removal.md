# 2026-06-13 HD2D point15 library facade occlusion gradient removal

## Scope

- Remove only the current-side transparent facade occlusion plane:
  - `Current_CentralPlaza_FramedLightPlanes_LibraryFacadeOcclusionGradientA`
- Keep the past-side counterpart generated.
- Keep the other framed light planes intact.
- Keep acceptance/evidence built-player only.
- Preserve the renderer contract:
  - `RenderingMode=2`
  - `DepthPrimingMode=0`
  - `CopyDepthMode=0`
  - `PortalStencilFeatureActive=True`

## Reason

After the roof-under thin band removal, the close-up probe still measured a smaller transparent-plane contribution:

```text
libraryTransparentFacadeOcclusionGradientOff: Disabled 1, MeanMax 0.064, PctMax 1.302
```

The measured renderer was a transparent zero-depth plane:

```text
path="FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_FramedLightPlanes_LibraryFacadeOcclusionGradientA"
mat=name=FastVS_House_hd2d_outdoor_occlusion_gradient,shader=Universal Render Pipeline/Unlit,queue=3007,tagQueue=<none>,tagRenderType=Transparent,_Surface=1,_Cull=0,_ZWrite=0,_ZTest=<missing>,_AlphaClip=0,_SrcBlend=5,_DstBlend=10,_Color=(0.13,0.14,0.17,0.28),_BaseColor=(0.13,0.14,0.17,0.28)
bounds=center=(20.800,3.000,24.160),size=(9.060,1.520,0.000),min=(16.270,2.240,24.160),max=(25.330,3.760,24.160)
```

This matched the remaining "transparent / fog-like facade plane" risk better than the opaque `RearWallBandA` or `Cycle60MidStoneCourse` candidates, whose close-up pixel deltas were zero.

## Code change

- `Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
  - Stopped generating `Current_CentralPlaza_FramedLightPlanes_LibraryFacadeOcclusionGradientA`.
  - Kept `Past_CentralPlaza_FramedLightPlanes_LibraryFacadeOcclusionGradientA`.
  - Updated validation to require the current-side object to remain non-generated.

## Build evidence

- Method:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Log:
  - `Logs\unity_build_validate_facade_occlusion_gradient_removed_20260613T0815.log`
- Result:

```text
Build Finished, Result: Success.
Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe
Exiting batchmode successfully now!
```

Built-player executable:

```text
FullName      : C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe
Length        : 667648
LastWriteTime : 6/13/2026 8:34:48 AM
```

## Built-player proof

### Close-up proof

- Review folder:
  - `docs\review\2026-06-13T08-40_library_facade_occlusion_gradient_removed_close`
- Player log:
  - `Logs\point15_library_facade_occlusion_gradient_removed_close_20260613T0840.log`
- Result:
  - `PNG_COUNT=39`
  - `PNG_TOTAL_BYTES=9975262`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Lighting state:

```text
ANEMORA_HOUSE_SLICE_LIGHTING_STATE: label=libraryRearClose.ready expectedArea=CentralPlaza activeArea=CentralPlaza sunCurrent=Morning sunTarget=Morning sunTransitioning=False indoorSunSuppression=False main=name=Directional Light,enabled=True,type=Directional,intensity=1.500,color=(1.000,0.840,0.700,1.000),shadows=Soft,shadowStrength=0.800,shadowBias=0.012,shadowNormalBias=0.100 warm=name=FastVS_HD2D_WarmFillLight,enabled=True,type=Point,intensity=0.300,color=(1.000,0.640,0.340,1.000),shadows=None,shadowStrength=1.000,shadowBias=0.050,shadowNormalBias=0.400 cool=name=FastVS_HD2D_CoolRimLight,enabled=True,type=Directional,intensity=0.160,color=(0.460,0.580,0.920,1.000),shadows=None,shadowStrength=1.000,shadowBias=0.050,shadowNormalBias=0.400 ambientMode=Flat ambient=(0.074,0.068,0.058,1.000) ambientMax=0.074 fog=False fogColor=(0.235,0.215,0.178,1.000) fogDensity=0.011 shadowPolicy=renderers=11817,active=1593,enabled=11737,castOn=801,shadowsOnly=46,receive=1458,activeCastOn=64,activeReceive=175
```

Removal proof:

```text
ANEMORA_HOUSE_SLICE_LIBRARY_REAR_CLOSE_PROBE: candidateSummary variant=libraryTransparentFacadeOcclusionGradientOff matched=0 logged=0
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=libraryTransparentFacadeOcclusionGradientOff matched=0 disabled=0 logged=[]
ANEMORA_HOUSE_SLICE_LIBRARY_REAR_CLOSE_PROBE: variant=libraryTransparentFacadeOcclusionGradientOff view=rearCenter saved=36_libraryTransparentFacadeOcclusionGradientOff_rearCenter.png disabled=0 baselineDelta meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600 cameraPos=(20.800,3.420,20.220) cameraEuler=(12.124,0.000,0.000)
ANEMORA_HOUSE_SLICE_LIBRARY_REAR_CLOSE_PROBE: variant=libraryTransparentFacadeOcclusionGradientOff view=rearEastOblique saved=37_libraryTransparentFacadeOcclusionGradientOff_rearEastOblique.png disabled=0 baselineDelta meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600 cameraPos=(22.950,3.340,20.520) cameraEuler=(9.826,27.098,0.000)
ANEMORA_HOUSE_SLICE_LIBRARY_REAR_CLOSE_PROBE: variant=libraryTransparentFacadeOcclusionGradientOff view=facadeUpper saved=38_libraryTransparentFacadeOcclusionGradientOff_facadeUpper.png disabled=0 baselineDelta meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600 cameraPos=(20.800,3.200,19.520) cameraEuler=(7.907,0.000,0.000)
ANEMORA_HOUSE_SLICE_LIBRARY_REAR_CLOSE_PROBE: end count=39
```

Measured improvement:

```text
libraryTransparentFacadeOcclusionGradientOff close PctMax: 1.302 -> 0.000
libraryTransparentFacadeOcclusionGradientOff close MeanMax: 0.064 -> 0.000
Current_CentralPlaza_FramedLightPlanes_LibraryFacadeOcclusionGradientA matched: 1 -> 0
```

### All-map proof

- Review folder:
  - `docs\review\2026-06-13T08-45_library_facade_occlusion_gradient_removed_allmaps`
- Player log:
  - `Logs\point15_library_facade_occlusion_gradient_removed_allmaps_20260613T0845.log`
- Result:
  - `PNG_COUNT=13`
  - `PNG_TOTAL_BYTES=8304409`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
ANEMORA_HOUSE_SLICE_CAPTURE: end count=13
```

Lighting state:

```text
ANEMORA_HOUSE_SLICE_LIGHTING_STATE: label=capture.ready expectedArea=CentralPlaza activeArea=CentralPlaza sunCurrent=Morning sunTarget=Morning sunTransitioning=False indoorSunSuppression=False main=name=Directional Light,enabled=True,type=Directional,intensity=1.500,color=(1.000,0.840,0.700,1.000),shadows=Soft,shadowStrength=0.800,shadowBias=0.012,shadowNormalBias=0.100 warm=name=FastVS_HD2D_WarmFillLight,enabled=True,type=Point,intensity=0.300,color=(1.000,0.640,0.340,1.000),shadows=None,shadowStrength=1.000,shadowBias=0.050,shadowNormalBias=0.400 cool=name=FastVS_HD2D_CoolRimLight,enabled=True,type=Directional,intensity=0.160,color=(0.460,0.580,0.920,1.000),shadows=None,shadowStrength=1.000,shadowBias=0.050,shadowNormalBias=0.400 ambientMode=Flat ambient=(0.420,0.380,0.320,1.000) ambientMax=0.420 fog=False fogColor=(0.910,0.770,0.590,1.000) fogDensity=0.011 shadowPolicy=renderers=11817,active=1593,enabled=11737,castOn=801,shadowsOnly=46,receive=1458,activeCastOn=64,activeReceive=175
```

### Motion proof

- Review folder:
  - `docs\review\2026-06-13T08-55_renderer_motion_after_facade_occlusion_gradient_removed`
- Player log:
  - `Logs\point15_renderer_motion_after_facade_occlusion_gradient_removed_20260613T0855.log`
- Result:
  - `PNG_COUNT=141`
  - `PNG_TOTAL_BYTES=105675333`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Motion summary:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: summary phase=motionFollowCapture frames=180 saved=13 tracked=11048 visible=min=342,max=525,mean=454.883,stddev=58.459 enabled=min=1540,max=1540,mean=1540.000,stddev=0.000 backgroundVisible=min=1,max=3,mean=2.400,stddev=0.611 visibleHashChanges=123 backgroundHashChanges=6 imageMeanAbsRgb=min=12.494,max=33.523,mean=23.834,stddev=6.801 imageChangedSamplePct=min=48.757,max=81.382,mean=73.661,stddev=9.147 deltaTime=min=0.019,max=0.333,mean=0.033,stddev=0.031 unscaledDeltaTime=min=0.019,max=0.901,mean=0.036,stddev=0.068
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: runtimeSummary phase=motionFollowRenderNoSave frames=180 tracked=11048 visible=min=342,max=544,mean=454.206,stddev=58.488 enabled=min=1540,max=1540,mean=1540.000,stddev=0.000 backgroundVisible=min=1,max=3,mean=2.372,stddev=0.615 visibleHashChanges=123 backgroundHashChanges=2 deltaTime=min=0.017,max=0.333,mean=0.026,stddev=0.024 unscaledDeltaTime=min=0.017,max=13.580,mean=0.099,stddev=1.008
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: end count=141
```

Post-removal grep proof:

```text
rg -n "Current_CentralPlaza_FramedLightPlanes_LibraryFacadeOcclusionGradientA" Assets\Scenes Assets\Art docs\review\2026-06-13T08-40_library_facade_occlusion_gradient_removed_close docs\review\2026-06-13T08-45_library_facade_occlusion_gradient_removed_allmaps docs\review\2026-06-13T08-55_renderer_motion_after_facade_occlusion_gradient_removed Logs\point15_library_facade_occlusion_gradient_removed_close_20260613T0840.log Logs\point15_library_facade_occlusion_gradient_removed_allmaps_20260613T0845.log Logs\point15_renderer_motion_after_facade_occlusion_gradient_removed_20260613T0855.log
exit=1, no matches
```

## Conclusion

- The current-side transparent facade occlusion plane is removed and no longer appears in built-player close/all-map/motion evidence.
- The close-up transparent-facade delta dropped from `PctMax=1.302` to `PctMax=0.000`.
- The renderer contract remained fixed: `RenderingMode=2`, `DepthPrimingMode=0`, `CopyDepthMode=0`, `PortalStencilFeatureActive=True`.
- No runtime renderer enable/disable flicker is measured: motion `enabled=min=1540,max=1540,mean=1540.000,stddev=0.000`.
- Remaining motion togglers are now opaque side/facade micro-geometry and the two transparent side cool-falloff planes. Treat those as a separate next slice; do not bundle them into this transparent facade removal.
