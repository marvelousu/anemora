# 2026-06-13 HD2D point15 renderer motion probe after paver removal

## Scope

- Re-run the built-player renderer motion probe after the EastField, far-right strip, and lower/front paver removal slices.
- Do not change renderer behavior in this cycle.
- Keep acceptance/evidence built-player only.
- Preserve the renderer contract:
  - `RenderingMode=2`
  - `DepthPrimingMode=0`
  - `CopyDepthMode=0`
  - `PortalStencilFeatureActive=True`

## Built-player evidence

- Review folder:
  - `docs\review\2026-06-13T06-50_renderer_motion_after_paver_removed`
- Player log:
  - `Logs\point15_renderer_motion_after_paver_removed_20260613T064143.log`
- Result:
  - `PLAYER_EXIT=0`
  - `PNG_COUNT=141`
  - `PNG_TOTAL_BYTES=105871212`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Motion summary:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: summary phase=motionFollowCapture frames=180 saved=13 tracked=11050 visible=min=343,max=525,mean=454.522,stddev=58.030 enabled=min=1542,max=1542,mean=1542.000,stddev=0.000 backgroundVisible=min=1,max=3,mean=2.389,stddev=0.609 visibleHashChanges=130 backgroundHashChanges=4 imageMeanAbsRgb=min=12.454,max=33.468,mean=23.893,stddev=6.806 imageChangedSamplePct=min=47.757,max=80.931,mean=73.717,stddev=9.343 deltaTime=min=0.013,max=0.333,mean=0.025,stddev=0.037 unscaledDeltaTime=min=0.013,max=0.728,mean=0.028,stddev=0.064
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: runtimeSummary phase=motionFollowRenderNoSave frames=180 tracked=11050 visible=min=343,max=545,mean=453.906,stddev=57.911 enabled=min=1542,max=1542,mean=1542.000,stddev=0.000 backgroundVisible=min=1,max=3,mean=2.372,stddev=0.615 visibleHashChanges=126 backgroundHashChanges=2 deltaTime=min=0.014,max=0.333,mean=0.019,stddev=0.024 unscaledDeltaTime=min=0.014,max=11.546,mean=0.081,stddev=0.857
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: end count=141
```

## Background hash events

The background visible set still changes only at a few moving-camera frames:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: backgroundVisibleDelta frame=80 previous=5569A502 current=A18DF36A currentSample=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteLeft | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteRight]
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: backgroundVisibleDelta frame=91 previous=A18DF36A current=6C6F13A6 currentSample=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_EastEdgeWash | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteLeft | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteRight]
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: backgroundVisibleDelta frame=93 previous=6C6F13A6 current=A18DF36A currentSample=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteLeft | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteRight]
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: backgroundVisibleDelta frame=168 previous=A18DF36A current=DAA5E1A7 currentSample=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteRight]
```

## Variant-off visual comparison

Same-frame comparisons against baseline show that disabling the `OutdoorVoidBackground*` variants is visually negligible in the current motion path:

```text
outdoorVoidBackgroundEastEdgeWashOff max sampled diff: MEAN=0.0067 CHANGED_PCT=0.201 MAX=10
outdoorVoidBackgroundNorthSilhouettesOff max sampled diff: MEAN=0.0067 CHANGED_PCT=0.201 MAX=10
outdoorVoidBackgroundAllCurrentCentralPlazaOff max sampled diff: MEAN=0.0067 CHANGED_PCT=0.201 MAX=10
frontRoadLongThinGroundOff max sampled diff: MEAN=0.0067 CHANGED_PCT=0.201 MAX=10
libraryTransparentDepthPlanesOff max sampled diff: MEAN=0.0067 CHANGED_PCT=0.201 MAX=10
libraryRear_ArchitectureUpperBrowOff max sampled diff: MEAN=0.0067 CHANGED_PCT=0.201 MAX=10
libraryRearThinOpaquesOff max sampled diff: MEAN=0.0320 CHANGED_PCT=0.9549 MAX=13
```

The biggest visual delta among the tested variants is `libraryRearThinOpaquesOff`, but even that is still under 1 percent changed samples in the sampled comparison.

## Top visible togglers

All listed top togglers have `enabledToggles=0`; the objects are not being enabled/disabled at runtime. The motion probe is seeing renderer visibility/frustum changes during camera/player movement.

Top entries by visible toggles:

```text
index=0 visibleToggles=13 path="...Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_EastMidStoneCourseA" size=(0.045,0.045,0.840)
index=1 visibleToggles=13 path="...Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_WestMidStoneCourseA" size=(0.045,0.045,0.840)
index=2 visibleToggles=9 path="...Current_CentralPlaza_LibraryRearVolume_RearDustBreakA" size=(1.362,0.040,0.265)
index=3 visibleToggles=8 path="...Current_CentralPlaza_LibrarySideSurfaceBreakup_RearWallBandA" material=FastVS_House_shadow
index=4 visibleToggles=7 path="...Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_EastMidStoneCourseB" size=(0.045,0.045,0.780)
index=5 visibleToggles=7 path="...Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_WestMidStoneCourseB" size=(0.045,0.045,0.780)
index=6 visibleToggles=7 path="...Current_CentralPlaza_LibraryFacadeSurfaceBreakup_UpperDustChipCenterA" size=(0.520,0.040,0.018)
index=9 visibleToggles=6 path="...Current_CentralPlaza_Cycle59_LibraryRoofUndersideShadow_EastSideWallCoolFalloffA" material=FastVS_House_hd2d_depth_shadow queue=2990
index=10 visibleToggles=6 path="...Current_CentralPlaza_Cycle59_LibraryRoofUndersideShadow_WestSideWallCoolFalloffA" material=FastVS_House_hd2d_depth_shadow queue=2990
index=11 visibleToggles=5 path="...Current_CentralPlaza_FramedLightPlanes_LibraryFacadeOcclusionGradientA" material=FastVS_House_hd2d_outdoor_occlusion_gradient queue=3007
```

## Conclusion

- The latest motion probe does not support `OutdoorVoidBackground*` as the main visible flicker source.
- No renderer enable/disable flicker is measured: `enabled=min=1542,max=1542,stddev=0`.
- The remaining measurable motion pop is concentrated in very thin library rear / side / facade detail geometry and some transparent shadow/occlusion planes.
- Next fix candidate should be a narrow library rear micro-geometry cleanup or stabilization probe, not a broad background/void removal.
