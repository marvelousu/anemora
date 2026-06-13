# 2026-06-13 HD2D point15 CentralPlaza EastField removal

## Scope

- Continue the point15 long-road / far-outline diagnostic after the upper-brow removal.
- Identify the large right-side road-like object in built-player captures by renderer name before changing generator output.
- Remove only the confirmed CentralPlaza east perimeter field object family:
  - `Current_CentralPlaza_Cycle53_PerimeterWorld_EastFieldA`
  - `Past_CentralPlaza_Cycle53_PerimeterWorld_EastFieldA`
- Keep HouseExterior `Cycle53_PerimeterWorld_EastFieldA` unchanged.
- Keep the renderer contract unchanged:
  - `RenderingMode=2`
  - `DepthPrimingMode=0`
  - `CopyDepthMode=0`
  - `PortalStencilFeatureActive=True`

## Code change

- `Assets/Scripts/FastVS/FastVsHouseRuntimeSmokeProbe.cs`
  - Added a right-outer-road ROI renderer log for built-player isolation.
  - Added diagnostic isolation variants:
    - `eastPerimeterFieldOff`
    - `backPathBandsOff`
    - `horizonRightDepthOff`
- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Stopped generating CentralPlaza `Cycle53_PerimeterWorld_EastFieldA`.
  - Removed the matching current/past validation rows.

## Pre-fix ROI evidence

- Review folder:
  - `docs\review\2026-06-13T01-45_renderer_right_outer_road_roi_probe`
- Player log:
  - `Logs\point15_renderer_right_outer_road_roi_probe_20260613T0145.log`
- Result:
  - `PLAYER_EXIT=0`
  - `PNG_COUNT=52`
  - `PNG_TOTAL_BYTES=47810215`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

ROI result:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: rightOuterRoadRoi roiViewport=(0.730,0.120,0.990,0.880) candidates=128 logged=64
```

Important candidate from the ROI log:

```text
rank=50 path="FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Cycle53_PerimeterWorld_EastFieldA" rect=(0.896,0.582,1.069,0.585) objectCoverage=0.543 minDepth=17.073 bounds=center=(32.140,0.012,20.680),size=(3.600,0.034,18.400),min=(30.340,-0.005,11.480),max=(33.940,0.029,29.880),mat=name=FastVS_House_current_path,shader=Anemora/FastVS/SurfaceRampLit,queue=2000
```

## Pre-fix isolation proof

- Review folder:
  - `docs\review\2026-06-13T02-05_renderer_far_right_variant_probe`
- Player log:
  - `Logs\point15_renderer_far_right_variant_probe_20260613T0205.log`
- Result:
  - `PLAYER_EXIT=0`
  - `PNG_COUNT=58`
  - `PNG_TOTAL_BYTES=53264714`

The right-side large road-like object was isolated to `eastPerimeterFieldOff`:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=eastPerimeterFieldOff matched=2 disabled=2 logged=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Cycle53_PerimeterWorld_EastFieldA | FastVS_Past_NiroHouseInteriorExterior/Past_CentralPlazaMap_SeparateSpace/Past_CentralPlaza_Cycle53_PerimeterWorld_EastFieldA]
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=eastPerimeterFieldOff view=wide baselineDelta meanAbsRgb=1.608 changedSamplePct=5.622 changed=3238 samples=57600
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=eastPerimeterFieldOff view=close baselineDelta meanAbsRgb=0.011 changedSamplePct=0.040 changed=23 samples=57600
```

Control variants were much smaller:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=backPathBandsOff view=wide baselineDelta meanAbsRgb=0.036 changedSamplePct=0.156 changed=90 samples=57600
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=horizonRightDepthOff view=wide baselineDelta meanAbsRgb=0.043 changedSamplePct=0.134 changed=77 samples=57600
```

Visual check:

- `01_baseline_current_plaza_library_facade.png`: large beige road-like board visible on the right.
- `53_no_east_perimeter_field_current_plaza_library_facade.png`: that large beige board is absent.
- `55_no_back_path_bands_current_plaza_library_facade.png` and `57_no_horizon_right_depth_current_plaza_library_facade.png`: the large board remains.

## BuildAndValidate evidence

- Correct batch method:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Build log:
  - `Logs\unity_build_validate_remove_central_plaza_eastfield_20260613T0215.log`
- Build result:
  - `UNITY_EXIT=0`

Build log key lines:

```text
Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity
Build Finished, Result: Success.
Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe
```

Built-player executable after the generator change:

```text
FullName      : C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe
Length        : 667648
LastWriteTime : 6/13/2026 1:44:44 AM
```

The log also contains the existing Unity codecoverage package `System.Numerics.Vector* Failed to resolve` messages seen in previous successful builds. The final build result remained success.

## Fixed isolation evidence

- Review folder:
  - `docs\review\2026-06-13T02-25_renderer_eastfield_removed_isolation`
- Player log:
  - `Logs\point15_renderer_eastfield_removed_isolation_20260613T0225.log`
- Result:
  - `PLAYER_EXIT=0`
  - `PNG_COUNT=58`
  - `PNG_TOTAL_BYTES=50974269`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

The removed renderer family no longer matches:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: rightOuterRoadRoi roiViewport=(0.730,0.120,0.990,0.880) candidates=127 logged=64
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=eastPerimeterFieldOff matched=0 disabled=0 logged=[]
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=eastPerimeterFieldOff view=wide baselineDelta meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=eastPerimeterFieldOff view=close baselineDelta meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: end count=58
```

Visual check:

- `01_baseline_current_plaza_library_facade.png`: the large right-side beige road-like board is gone.
- Small right-edge detached pieces remain and are separate renderers; they were not part of this confirmed EastField removal.

## Fixed all-map evidence

- Review folder:
  - `docs\review\2026-06-13T02-40_eastfield_removed_allmaps`
- Player log:
  - `Logs\point15_eastfield_removed_allmaps_20260613T0240.log`
- Result:
  - `PLAYER_EXIT=0`
  - `PNG_COUNT=13`
  - `PNG_TOTAL_BYTES=8440515`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Capture completion:

```text
ANEMORA_HOUSE_SLICE_CAPTURE: end count=13
PLAYER_EXIT=0
```

Visual review notes:

- `03_b1_b3_current.png`: the large right-side road-like board is absent.
- `04_b1_b3_past.png`: the same large right-side road-like board is absent in the past view.
- Other all-map captures completed with no player failure.

## Findings

- The large right-side road-like object was not a shader transparency bug by itself; it was a generated CentralPlaza perimeter field that was visible from the all-map review camera.
- The confirmed renderer family was removed by generator change and stayed absent after `BuildAndValidateBatch`.
- The point15 renderer contract remained intact in both the isolation and all-map built-player runs.
- Remaining right-edge small detached ground pieces and sideview fragments are separate from `Cycle53_PerimeterWorld_EastFieldA` and should be handled by the next data-first slice if Tom asks to remove them.

## Next action

- Propagate `docs\review\2026-06-13T02-40_eastfield_removed_allmaps` and the supporting isolation folder to anemora-viewer.
- Continue the planned flicker/visibility diagnostic line after viewer propagation, using built-player evidence only.
