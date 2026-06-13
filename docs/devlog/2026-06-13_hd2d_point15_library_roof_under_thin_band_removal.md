# 2026-06-13 HD2D point15 library roof-under thin band removal

## Scope

- Split the library rear / side thin-detail candidates with built-player close-up probes.
- Remove only the current-side `Current_CentralPlaza_LibraryFacadeDetail_RoofUnderThinBand` generator output after the close-up probe isolated it as the dominant horizontal strip.
- Keep the past-side counterpart generated.
- Keep acceptance/evidence built-player only.
- Preserve the renderer contract:
  - `RenderingMode=2`
  - `DepthPrimingMode=0`
  - `CopyDepthMode=0`
  - `PortalStencilFeatureActive=True`

## Built-player diagnosis before removal

Close-up probe:

- Review folder:
  - `docs\review\2026-06-13T07-20_library_rear_close_probe`
- Player log:
  - `Logs\point15_library_rear_close_probe_20260613T0720.log`
- Result:
  - `PNG_COUNT=33`
  - `PNG_TOTAL_BYTES=8677333`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Close-up pixel diff summary:

```text
DIFF rearCenter_thinOpaques base=00_baseline_rearCenter.png variant=03_libraryRearThinOpaquesOff_rearCenter.png changed=63923 samples=921600 changedPct=6.936 meanMaxChannel=0.527 max=69
DIFF rearEast_thinOpaques base=01_baseline_rearEastOblique.png variant=04_libraryRearThinOpaquesOff_rearEastOblique.png changed=43703 samples=921600 changedPct=4.742 meanMaxChannel=0.607 max=129
DIFF facadeUpper_thinOpaques base=02_baseline_facadeUpper.png variant=05_libraryRearThinOpaquesOff_facadeUpper.png changed=58669 samples=921600 changedPct=6.366 meanMaxChannel=0.373 max=57
DIFF rearCenter_facadeDust base=00_baseline_rearCenter.png variant=27_libraryRear_FacadeUpperDustChipsOff_rearCenter.png changed=4290 samples=921600 changedPct=0.466 meanMaxChannel=0.263 max=139
DIFF rearEast_transparentFacade base=01_baseline_rearEastOblique.png variant=31_libraryTransparentFacadeOcclusionGradientOff_rearEastOblique.png changed=13799 samples=921600 changedPct=1.497 meanMaxChannel=0.079 max=7
```

V2 close-up probe:

- Review folder:
  - `docs\review\2026-06-13T07-25_library_rear_close_probe_v2`
- Player log:
  - `Logs\point15_library_rear_close_probe_v2_20260613T0725.log`
- Result:
  - `PNG_COUNT=39`
  - `PNG_TOTAL_BYTES=10236391`

The v2 probe isolated one object as the same visual contributor as the broader thin-opaque set:

```text
libraryRear_FacadeRoofUnderThinBandOff: Disabled 1, MeanMax 0.527, PctMax 6.172, MaxView rearCenter, MeanAvg 0.427, PctAvg 5.010
libraryRearThinOpaquesOff: Disabled 9, MeanMax 0.527, PctMax 6.172, MeanAvg 0.430, PctAvg 4.902
libraryRear_ThinShadowBandsOff: Disabled 4, MeanMax 0.018, PctMax 0.104
```

Conclusion before removal:

- The visually dominant close-up horizontal strip was `Current_CentralPlaza_LibraryFacadeDetail_RoofUnderThinBand`.
- The transparent/depth-plane candidates were measurable but much smaller and were not removed in this slice.

## Code change

- `Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
  - Stopped generating `Current_CentralPlaza_LibraryFacadeDetail_RoofUnderThinBand`.
  - Kept `Past_CentralPlaza_LibraryFacadeDetail_RoofUnderThinBand`.
  - Updated validation to require current-side removal and keep the past-side detail object.
- `Assets\Scripts\FastVS\FastVsHouseRuntimeSmokeProbe.cs`
  - Kept and extended the close-up / motion isolation probes used for this built-player proof.

## Build evidence

- Method:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Log:
  - `Logs\unity_build_validate_roof_under_thin_band_removed_20260613T0731.log`
- Result:

```text
Build Finished, Result: Success.
Batchmode quit successfully invoked - shutting down!
Exiting batchmode successfully now!
```

Built-player executable after build:

```text
C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe
LastWriteTime: 6/13/2026 7:29:22 AM
```

## Built-player proof after removal

### Close-up proof

- Review folder:
  - `docs\review\2026-06-13T07-36_library_roof_under_band_removed_close`
- Player log:
  - `Logs\point15_library_roof_under_band_removed_close_20260613T0736.log`
- Result:
  - `PNG_COUNT=39`
  - `PNG_TOTAL_BYTES=9972281`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Lighting state:

```text
ANEMORA_HOUSE_SLICE_LIGHTING_STATE: label=libraryRearClose.ready expectedArea=CentralPlaza activeArea=CentralPlaza camera=FastVS_HouseSliceCaptureCamera activeCamera=FastVS_HouseSliceCaptureCamera cameraPos=(9.500,9.000,-2.600) cameraRot=(63.800,0.000,0.000) mainLight=FastVS_House_DirectionalKey intensity=0.850 color=(1.000,0.812,0.560,1.000) ambient=(0.074,0.068,0.058,1.000) ambientMax=0.074 skybox=<null> realtimeShadowMode=Hard baked=False shadowStrength=0.550 shadows=Soft shadowPolicy=renderers=11818,active=1594,enabled=11738,castOn=801,shadowsOnly=46,receive=1458,activeCastOn=64,activeReceive=175
```

Removal proof:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: candidateSummary variant=libraryRearThinOpaquesOff matched=8 logged=8
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=libraryRearThinOpaquesOff matched=8 disabled=8 logged=[...]
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: candidateSummary variant=libraryRear_FacadeRoofUnderThinBandOff matched=0 logged=0
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=libraryRear_FacadeRoofUnderThinBandOff matched=0 disabled=0 logged=[]
ANEMORA_HOUSE_SLICE_LIBRARY_REAR_CLOSE_PROBE: end count=39
```

Post-removal close-up comparison:

```text
libraryRearThinOpaquesOff: Disabled 8, MeanMax 0.015, PctMax 0.021, MaxView rearEastOblique, MeanAvg 0.008, PctAvg 0.007
libraryRear_FacadeRoofUnderThinBandOff: Disabled 0, MeanMax 0.000, PctMax 0.000
libraryTransparentDepthPlanesOff: Disabled 3, MeanMax 0.064, PctMax 1.302
libraryTransparentFacadeOcclusionGradientOff: Disabled 1, MeanMax 0.064, PctMax 1.302
libraryRear_FacadeUpperDustChipsOff: Disabled 3, MeanMax 0.176, PctMax 0.384
```

Measured improvement:

```text
libraryRearThinOpaquesOff close PctMax: 6.172 -> 0.021
libraryRearThinOpaquesOff close MeanMax: 0.527 -> 0.015
Current_CentralPlaza_LibraryFacadeDetail_RoofUnderThinBand matched: 1 -> 0
```

### All-map proof

- Review folder:
  - `docs\review\2026-06-13T07-45_library_roof_under_band_removed_allmaps`
- Player log:
  - `Logs\point15_library_roof_under_band_removed_allmaps_20260613T0745.log`
- Result:
  - `PNG_COUNT=13`
  - `PNG_TOTAL_BYTES=8304202`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Lighting state:

```text
ANEMORA_HOUSE_SLICE_LIGHTING_STATE: label=capture.ready expectedArea=CentralPlaza activeArea=CentralPlaza camera=FastVS_HouseSliceCaptureCamera activeCamera=FastVS_HouseSliceCaptureCamera cameraPos=(4.400,6.500,-2.800) cameraRot=(62.000,0.000,0.000) mainLight=FastVS_House_DirectionalKey intensity=1.050 color=(1.000,0.812,0.560,1.000) ambient=(0.420,0.380,0.320,1.000) ambientMax=0.420 skybox=<null> realtimeShadowMode=Hard baked=False shadowStrength=0.620 shadows=Soft shadowPolicy=renderers=11818,active=1594,enabled=11738,castOn=801,shadowsOnly=46,receive=1458,activeCastOn=64,activeReceive=175
ANEMORA_HOUSE_SLICE_CAPTURE: end count=13
```

Scene/log grep:

```text
rg -n "Current_CentralPlaza_LibraryFacadeDetail_RoofUnderThinBand" Assets\Scenes Assets\Art docs\review\2026-06-13T07-45_library_roof_under_band_removed_allmaps Logs\point15_library_roof_under_band_removed_allmaps_20260613T0745.log
exit=1, no matches
```

### Motion proof

- Review folder:
  - `docs\review\2026-06-13T07-55_renderer_motion_after_roof_under_band_removed`
- Player log:
  - `Logs\point15_renderer_motion_after_roof_under_band_removed_20260613T0755.log`
- Result:
  - `PNG_COUNT=141`
  - `PNG_TOTAL_BYTES=105798883`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Motion summary:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: summary phase=motionFollowCapture frames=180 saved=13 tracked=11049 visible=min=343,max=525,mean=454.944,stddev=58.304 enabled=min=1541,max=1541,mean=1541.000,stddev=0.000 backgroundVisible=min=1,max=3,mean=2.400,stddev=0.611 visibleHashChanges=121 backgroundHashChanges=6 imageMeanAbsRgb=min=13.603,max=33.534,mean=23.916,stddev=6.724 imageChangedSamplePct=min=50.764,max=81.389,mean=73.744,stddev=8.799 deltaTime=min=0.013,max=0.333,mean=0.029,stddev=0.038 unscaledDeltaTime=min=0.013,max=0.756,mean=0.032,stddev=0.066
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: runtimeSummary phase=motionFollowRenderNoSave frames=180 tracked=11049 visible=min=343,max=545,mean=453.983,stddev=57.910 enabled=min=1541,max=1541,mean=1541.000,stddev=0.000 backgroundVisible=min=1,max=3,mean=2.372,stddev=0.615 visibleHashChanges=125 backgroundHashChanges=2 deltaTime=min=0.015,max=0.333,mean=0.020,stddev=0.024 unscaledDeltaTime=min=0.015,max=11.566,mean=0.083,stddev=0.858
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=libraryRear_FacadeRoofUnderThinBandOff matched=0 disabled=0 logged=[]
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: end count=141
```

Post-removal motion comparison:

```text
libraryRearThinOpaquesOff: Frames 8, MeanMax 0.039, PctMax 0.326, MaxFrame 150, MeanAvg 0.016, PctAvg 0.136
libraryRear_SideSurfaceRearWallBandOff: MeanMax 0.034, PctMax 0.326
libraryRear_RoofSideRearWallBandOff: MeanMax 0.021, PctMax 0.174
```

Motion delta against the previous paver-removal probe:

```text
libraryRearThinOpaquesOff motion PctMax: 0.347 -> 0.326
visibleHashChanges capture: 130 -> 121
enabled stddev: 0 -> 0
```

Top remaining motion togglers:

```text
index=0 visibleToggles=12 path="...Current_CentralPlaza_LibrarySideSurfaceBreakup_RearWallBandA"
index=1 visibleToggles=11 path="...Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_EastMidStoneCourseA"
index=2 visibleToggles=11 path="...Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_WestMidStoneCourseA"
index=3 visibleToggles=9 path="...Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_EastMidStoneCourseB"
index=4 visibleToggles=9 path="...Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_WestMidStoneCourseB"
index=5 visibleToggles=9 path="...Current_CentralPlaza_LibraryFacadeSurfaceBreakup_UpperDustChipCenterA"
index=6 visibleToggles=7 path="...Current_CentralPlaza_LibraryFacadeSurfaceBreakup_UpperDustChipLeftA"
index=7 visibleToggles=5 path="...Current_CentralPlaza_FramedLightPlanes_LibraryFacadeOcclusionGradientA"
```

## Conclusion

- The close-up horizontal roof/facade strip was removed by non-generating the current-side `Current_CentralPlaza_LibraryFacadeDetail_RoofUnderThinBand`.
- The close-up dominant thin-opaque delta dropped from `PctMax=6.172` to `PctMax=0.021`.
- The object no longer appears in the current scene/log probe: `matched=0`.
- Full-frame motion changed only slightly because the moving-camera path is now dominated by smaller remaining library side/facade pieces, not the removed roof-under band.
- No runtime renderer enable/disable flicker is measured in this cycle: `enabled=min=1541,max=1541,mean=1541.000,stddev=0.000`.
- Next candidate should be another narrow built-player probe/fix for `LibrarySideSurfaceBreakup_RearWallBandA`, `Cycle60_LibrarySideWallMaterialBreakup_*MidStoneCourse*`, or the small `LibraryFacadeSurfaceBreakup_UpperDustChip*` pieces. Avoid broad transparent/background removal until a close-up or motion probe proves ownership.
