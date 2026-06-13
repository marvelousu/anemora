# 2026-06-13 HD2D point15 rightmost detached renderer probe

## Scope

- Continue after the CentralPlaza `Cycle53_PerimeterWorld_EastFieldA` removal and static flicker probe.
- Classify the remaining rightmost detached road-like / plank-like fragments in the built-player wide CentralPlaza library-facade capture.
- Add data-only built-player probe support; no generator removal and no visual fix in this slice.
- Keep the renderer contract unchanged:
  - `RenderingMode=2`
  - `DepthPrimingMode=0`
  - `CopyDepthMode=0`
  - `PortalStencilFeatureActive=True`

## Code change

- `Assets/Scripts/FastVS/FastVsHouseRuntimeSmokeProbe.cs`
  - Increased right-outer-road ROI logging from 64 to 160 candidates.
  - Added a narrower `farRightLooseRoadRoi` log for the visually detached right edge.
  - Added diagnostic isolation variants:
    - `rightRuinBlockOff`
    - `rightLotHardscapeOff`
    - `rightLotPlantingOff`
    - `rightOuterCycle4347Off`
    - `rightOuterCycle62Off`
    - `outdoorWorldEnvelopeRightOff`
    - `outdoorWorldRearSideRidgeRightOff`
    - `cycle63ScenicHorizonEastOff`
    - `outdoorVoidNorthSilhouetteRightOff`
    - `farRightLooseDepthComboOff`
    - `perimeterBackFieldOff`
    - `perimeterBackRidgeOff`
    - `perimeterBackFieldRidgeOff`
  - Added rightmost pixel contributor diagnostics for the annotated crop points:
    - `A=(1076,230)`
    - `B=(1142,250)`
    - `C=(1145,340)`
    - `D=(1220,315)`
  - Added a second contributor pass with `RightRuinBlock` pre-disabled.

## Build evidence

Final build method:

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildHouseSlicePlayer`

Final build log:

- `Logs\unity_build_rightmost_pixel_second_20260613T0420.log`

Result:

```text
UNITY_EXIT=0
Build Finished, Result: Success.
```

Built-player executable after the code-only probe build:

```text
FullName      : C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe
Length        : 667648
LastWriteTime : 6/13/2026 3:53:43 AM
```

The build logs also contain the existing Unity codecoverage package `System.Numerics.Vector* Failed to resolve` messages seen in previous successful builds. The final build result remained success.

## Built-player evidence

### Split isolation

- Review folder:
  - `docs\review\2026-06-13T03-35_right_edge_split_isolation`
- Player log:
  - `Logs\point15_right_edge_split_isolation_20260613T0335.log`
- Result:
  - `PLAYER_EXIT=0`
  - `PNG_COUNT=78`
  - `PNG_TOTAL_BYTES=68588292`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Key split variant measurements:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=rightRuinBlockOff matched=2 disabled=2 logged=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Chapter1_B_RightRuinBlock | FastVS_Past_NiroHouseInteriorExterior/Past_CentralPlazaMap_SeparateSpace/Past_CentralPlaza_Chapter1_B_RightRuinBlock]
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=rightRuinBlockOff view=wide baselineDelta meanAbsRgb=0.129 changedSamplePct=0.450 changed=259 samples=57600
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=rightRuinBlockOff view=close baselineDelta meanAbsRgb=1.263 changedSamplePct=4.344 changed=2502 samples=57600
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=rightLotHardscapeOff view=wide baselineDelta meanAbsRgb=0.029 changedSamplePct=0.116 changed=67 samples=57600
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=rightLotPlantingOff view=wide baselineDelta meanAbsRgb=0.009 changedSamplePct=0.023 changed=13 samples=57600
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=rightOuterCycle4347Off view=wide baselineDelta meanAbsRgb=0.330 changedSamplePct=0.780 changed=449 samples=57600
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=rightOuterCycle62Off view=wide baselineDelta meanAbsRgb=0.015 changedSamplePct=0.153 changed=88 samples=57600
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=outdoorWorldEnvelopeRightOff view=wide baselineDelta meanAbsRgb=0.450 changedSamplePct=1.620 changed=933 samples=57600
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=outdoorWorldRearSideRidgeRightOff view=wide baselineDelta meanAbsRgb=0.000 changedSamplePct=0.003 changed=2 samples=57600
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=cycle63ScenicHorizonEastOff view=wide baselineDelta meanAbsRgb=0.006 changedSamplePct=0.038 changed=22 samples=57600
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=outdoorVoidNorthSilhouetteRightOff view=wide baselineDelta meanAbsRgb=0.001 changedSamplePct=0.017 changed=10 samples=57600
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=farRightLooseDepthComboOff view=wide baselineDelta meanAbsRgb=0.530 changedSamplePct=1.193 changed=687 samples=57600
```

### ROI-wide isolation

- Review folder:
  - `docs\review\2026-06-13T03-45_right_edge_roiwide_isolation`
- Player log:
  - `Logs\point15_right_edge_roiwide_isolation_20260613T0345.log`
- Result:
  - `PLAYER_EXIT=0`
  - `PNG_COUNT=78`
  - `PNG_TOTAL_BYTES=68588934`

Expanded ROI counts:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: rightOuterRoadRoi roiViewport=(0.730,0.120,0.990,0.880) candidates=127 logged=127
```

`farRightLooseRoadRoi` ranked the visible right edge candidates. Important entries:

```text
rank=1 name=Current_CentralPlaza_Chapter1_B_RightRuinBlock rect=(0.781,0.432,1.008,0.752) depth=8.814
rank=5 name=Current_CentralPlaza_HorizonScenicDepth_FarRightBlockA rect=(0.808,0.577,0.884,0.625) depth=16.207
rank=8 name=Current_CentralPlaza_OutdoorWorldEnvelope_RearSideRidgeB rect=(0.767,0.504,0.893,0.543) depth=11.204
rank=9 name=Current_CentralPlaza_Cycle47_GroundSkirtEastA rect=(0.970,0.511,0.991,0.537) depth=13.249
rank=13 name=Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteRight rect=(0.765,0.586,0.829,0.621) depth=13.854
rank=29 name=Current_CentralPlaza_Cycle53_PerimeterWorld_BackFieldA rect=(-0.091,0.576,0.967,0.617) depth=16.522
rank=49 name=Current_CentralPlaza_EdgeDressing_EastLowWall rect=(0.942,-0.116,2.227,0.569) depth=3.358
rank=55 name=Current_CentralPlaza_Cycle43_OuterEastShelfA rect=(0.910,-6.761,16.303,0.524) depth=0.396
```

### Perimeter back-field controls

- Review folder:
  - `docs\review\2026-06-13T03-55_perimeter_back_isolation`
- Player log:
  - `Logs\point15_perimeter_back_isolation_20260613T0355.log`
- Result:
  - `PLAYER_EXIT=0`
  - `PNG_COUNT=84`
  - `PNG_TOTAL_BYTES=73861180`

Perimeter back-field measurements:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=perimeterBackFieldOff matched=2 disabled=2 logged=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Cycle53_PerimeterWorld_BackFieldA | FastVS_Past_NiroHouseInteriorExterior/Past_CentralPlazaMap_SeparateSpace/Past_CentralPlaza_Cycle53_PerimeterWorld_BackFieldA]
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=perimeterBackFieldOff view=wide baselineDelta meanAbsRgb=0.325 changedSamplePct=0.991 changed=571 samples=57600
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=perimeterBackRidgeOff matched=2 disabled=2 logged=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Cycle53_PerimeterWorld_BackRidgeA | FastVS_Past_NiroHouseInteriorExterior/Past_CentralPlazaMap_SeparateSpace/Past_CentralPlaza_Cycle53_PerimeterWorld_BackRidgeA]
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=perimeterBackRidgeOff view=wide baselineDelta meanAbsRgb=0.084 changedSamplePct=0.295 changed=170 samples=57600
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=perimeterBackFieldRidgeOff matched=4 disabled=4 logged=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Cycle53_PerimeterWorld_BackRidgeA | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Cycle53_PerimeterWorld_BackFieldA | FastVS_Past_NiroHouseInteriorExterior/Past_CentralPlazaMap_SeparateSpace/Past_CentralPlaza_Cycle53_PerimeterWorld_BackRidgeA | FastVS_Past_NiroHouseInteriorExterior/Past_CentralPlazaMap_SeparateSpace/Past_CentralPlaza_Cycle53_PerimeterWorld_BackFieldA]
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=perimeterBackFieldRidgeOff view=wide baselineDelta meanAbsRgb=0.409 changedSamplePct=1.283 changed=739 samples=57600
```

These controls move background material but do not own the annotated rightmost plank pixels.

### Pixel contributor proof

- Review folder:
  - `docs\review\2026-06-13T04-20_rightmost_pixel_second_probe`
- Player log:
  - `Logs\point15_rightmost_pixel_second_probe_20260613T0420.log`
- Result:
  - `PLAYER_EXIT=0`
  - `PNG_COUNT=84`
  - `PNG_TOTAL_BYTES=73866725`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Baseline pixel colors:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: rightmostPixelBaseline phase=baseline points=[A=(1076,230):(69,61,51,255) | B=(1142,250):(58,51,42,255) | C=(1145,340):(64,57,46,255) | D=(1220,315):(60,53,44,255)]
```

Only one baseline positive contributor was found:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: rightmostPixelContributor phase=baseline rank=1 maxDelta=143 totalDelta=396 deltas=[A=99:(43,102,19,255),B=66:(62,74,81,255),C=88:(28,33,18,255),D=143:(5,5,4,255)] path="FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Chapter1_B_RightRuinBlock" mat=name=FastVS_House_current_exterior_wall,shader=Anemora/FastVS/SurfaceRampLit,queue=2000,tagQueue=Geometry,tagRenderType=Opaque,_Surface=<missing>,_Cull=<missing>,_ZWrite=<missing>,_ZTest=<missing>,_AlphaClip=<missing>,_SrcBlend=<missing>,_DstBlend=<missing>,_Color=<missing>,_BaseColor=(1,1,1,1)
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: rightmostPixelContributorSummary phase=baseline candidateRenderers=13 positive=1 logged=1
```

Second pass with `RightRuinBlock` pre-disabled:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=rightmostPixelPreset_afterRightRuinBlockOff matched=2 disabled=2 logged=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Chapter1_B_RightRuinBlock | FastVS_Past_NiroHouseInteriorExterior/Past_CentralPlazaMap_SeparateSpace/Past_CentralPlaza_Chapter1_B_RightRuinBlock]
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: rightmostPixelBaseline phase=afterRightRuinBlockOff points=[A=(1076,230):(43,102,19,255) | B=(1142,250):(62,74,81,255) | C=(1145,340):(28,33,18,255) | D=(1220,315):(5,5,4,255)]
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: rightmostPixelContributor phase=afterRightRuinBlockOff rank=1 maxDelta=202 totalDelta=202 deltas=[A=0:(43,102,19,255),B=0:(62,74,81,255),C=0:(28,33,18,255),D=202:(62,73,81,255)] path="FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_EdgeDressing_EastLowWall" mat=name=FastVS_House_current_stone,shader=Anemora/FastVS/SurfaceRampLit,queue=2000,tagQueue=Geometry,tagRenderType=Opaque,_Surface=<missing>,_Cull=<missing>,_ZWrite=<missing>,_ZTest=<missing>,_AlphaClip=<missing>,_SrcBlend=<missing>,_DstBlend=<missing>,_Color=<missing>,_BaseColor=(1,1,1,1)
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: rightmostPixelContributor phase=afterRightRuinBlockOff rank=2 maxDelta=113 totalDelta=113 deltas=[A=113:(65,76,84,255),B=0:(62,74,81,255),C=0:(28,33,18,255),D=0:(5,5,4,255)] path="FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_EdgeDressing_NorthTreeLineSpriteB" mat=name=FastVS_House_current_central_plaza_north_tree_line_sprite_b_cc0,shader=Anemora/FastVS/SpriteCardRampLit,queue=2450,tagQueue=AlphaTest,tagRenderType=TransparentCutout,_Surface=0,_Cull=0,_ZWrite=1,_ZTest=<missing>,_AlphaClip=1,_SrcBlend=1,_DstBlend=0,_Color=<missing>,_BaseColor=(0.58,0.67,0.53,1)
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: rightmostPixelContributorSummary phase=afterRightRuinBlockOff candidateRenderers=12 positive=2 logged=2
```

Visual close check:

- `02_baseline_current_library_facade_close.png`: `RightRuinBlock` is part of the large right-side library wall composition.
- `60_no_right_ruin_block_current_library_facade_close.png`: removing it exposes the side background and changes the close view substantially.
- Therefore full generator removal of `RightRuinBlock` is not a safe next step without a separate architectural split/replacement.

## Findings

- The remaining annotated rightmost detached plank pixels are owned first by `Current_CentralPlaza_Chapter1_B_RightRuinBlock`.
- This is not the removed `Cycle53_PerimeterWorld_EastFieldA`.
- `perimeterBackFieldOff`, `perimeterBackRidgeOff`, `horizonRightDepthOff`, and right-outer cycle variants move nearby background or edge pixels, but the A-D point ownership test identifies `RightRuinBlock` as the primary visible contributor.
- Full `RightRuinBlock` deletion is risky because the close built-player view changes by `meanAbsRgb=1.263 changedSamplePct=4.344 changed=2502 samples=57600`.
- After `RightRuinBlock` is pre-disabled, residual point contributors are:
  - `EdgeDressing_EastLowWall` at D.
  - `EdgeDressing_NorthTreeLineSpriteB` at A.
  - B and C no longer have a positive single-renderer contributor in the candidate set.

## Next action

- Propagate `docs\review\2026-06-13T04-20_rightmost_pixel_second_probe` to R2 and anemora-viewer.
- Do not remove `RightRuinBlock` wholesale.
- Next safe implementation slice should split or replace `RightRuinBlock` so the close right-wall silhouette remains while the far-wide detached projection is reduced. Candidate approaches:
  - split the wall into visible close-facing segments plus a smaller side/depth cap,
  - shrink/rotate the right ruin block only enough to remove the detached wide-camera plank,
  - add a purpose-built right-wall replacement and then remove the old monolithic block.
- Any such visual change must run `BuildAndValidateBatch`, then built-player close and all-map captures, then devlog/R2/anemora-viewer propagation.
