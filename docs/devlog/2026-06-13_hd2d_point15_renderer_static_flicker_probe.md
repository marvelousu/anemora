# 2026-06-13 HD2D point15 renderer static flicker probe

## Scope

- Continue after the CentralPlaza `Cycle53_PerimeterWorld_EastFieldA` removal.
- Separate true static renderer flicker from normal camera-motion/frustum visibility changes.
- Add data-only built-player probe support; no visual generator change in this slice.
- Keep the renderer contract unchanged:
  - `RenderingMode=2`
  - `DepthPrimingMode=0`
  - `CopyDepthMode=0`
  - `PortalStencilFeatureActive=True`

## Code change

- `Assets/Scripts/FastVS/FastVsHouseRuntimeSmokeProbe.cs`
  - Added `--anemora-house-slice-renderer-static-dir`.
  - Added `RunRendererStaticProbe`.
  - Added three static phases using the same CentralPlaza path positions as the motion probe:
    - `static_start_frame000`
    - `static_mid_frame090`
    - `static_end_frame179`
  - Added per-frame image sampling without writing every frame.
  - Captures PNG evidence every 15 frames plus frame 179.

## Build evidence

- Build method:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildHouseSlicePlayer`
- Build log:
  - `Logs\unity_build_static_probe_20260613T0236.log`
- Result:
  - `Build Finished, Result: Success.`

Built-player executable after the code-only probe build:

```text
FullName      : C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe
Length        : 667648
LastWriteTime : 6/13/2026 2:39:08 AM
```

The log also contains the existing Unity codecoverage package `System.Numerics.Vector* Failed to resolve` messages seen in previous successful builds. The final build result remained success.

## Discarded run

- Review folder:
  - `docs\review\2026-06-13T03-05_renderer_static_probe_after_eastfield_removed`
- Player log:
  - `Logs\point15_renderer_static_after_eastfield_removed_20260613T0305.log`
- Result:
  - `PNG_COUNT=39`
  - `PNG_TOTAL_BYTES=690612`

This run used `-nographics`, and the PNGs were gray flat frames. It is retained only as a warning and is not acceptance evidence.

## Accepted built-player evidence

- Review folder:
  - `docs\review\2026-06-13T03-25_renderer_static_probe_after_eastfield_removed_exitcoded`
- Player log:
  - `Logs\point15_renderer_static_after_eastfield_removed_20260613T0325.log`
- Result:
  - `PLAYER_EXIT=0`
  - `PNG_COUNT=39`
  - `PNG_TOTAL_BYTES=29222050`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Static start:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: staticSummary phase=static_start_frame000 frames=180 tracked=11060 visible=min=517,max=517,mean=517.000,stddev=0.000 enabled=min=1552,max=1552,mean=1552.000,stddev=0.000 backgroundVisible=min=3,max=3,mean=3.000,stddev=0.000 visibleHashChanges=0 backgroundHashChanges=0 imageMeanAbsRgb=min=0.000,max=0.001,mean=0.000,stddev=0.000 imageChangedSamplePct=min=0.000,max=0.007,mean=0.000,stddev=0.001 deltaTime=min=0.033,max=0.333,mean=0.052,stddev=0.034 unscaledDeltaTime=min=0.033,max=0.976,mean=0.056,stddev=0.077
```

Static middle:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: staticVisibleDelta phase=static_mid_frame090 frame=1
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: staticBackgroundVisibleDelta phase=static_mid_frame090 frame=1 previous=5569A502 current=A18DF36A currentSample=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteLeft | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteRight]
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: staticSummary phase=static_mid_frame090 frames=180 tracked=11060 visible=min=468,max=533,mean=468.361,stddev=4.831 enabled=min=1552,max=1552,mean=1552.000,stddev=0.000 backgroundVisible=min=2,max=3,mean=2.006,stddev=0.074 visibleHashChanges=1 backgroundHashChanges=1 imageMeanAbsRgb=min=0.000,max=0.001,mean=0.000,stddev=0.000 imageChangedSamplePct=min=0.000,max=0.007,mean=0.000,stddev=0.001 deltaTime=min=0.032,max=0.333,mean=0.049,stddev=0.027 unscaledDeltaTime=min=0.032,max=0.348,mean=0.049,stddev=0.028
```

Static end:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: staticVisibleDelta phase=static_end_frame179 frame=1
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: staticBackgroundVisibleDelta phase=static_end_frame179 frame=1 previous=A18DF36A current=DAA5E1A7 currentSample=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteRight]
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: staticSummary phase=static_end_frame179 frames=180 tracked=11060 visible=min=341,max=478,mean=341.761,stddev=10.183 enabled=min=1552,max=1552,mean=1552.000,stddev=0.000 backgroundVisible=min=1,max=2,mean=1.006,stddev=0.074 visibleHashChanges=1 backgroundHashChanges=1 imageMeanAbsRgb=min=0.000,max=0.006,mean=0.001,stddev=0.001 imageChangedSamplePct=min=0.000,max=0.035,mean=0.001,stddev=0.003 deltaTime=min=0.031,max=0.333,mean=0.049,stddev=0.027 unscaledDeltaTime=min=0.031,max=0.369,mean=0.050,stddev=0.029
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: staticEnd count=39
```

Visual check:

- `26_static_end_frame179_frame_000.png` was manually opened and is a valid rendered CentralPlaza/library-front image.
- The accepted run was executed without `-nographics`.

## Findings

- Static camera/player frames do not show meaningful image flicker:
  - Start max `imageChangedSamplePct=0.007`.
  - Middle max `imageChangedSamplePct=0.007`.
  - End max `imageChangedSamplePct=0.035`.
- Static start has `visibleHashChanges=0`.
- Static middle and end each have one `visibleHashChanges=1` event at frame 1 only.
- The frame 1 hash change is initial renderer visibility stabilization, not continuous flicker. The image delta remains effectively zero.
- The earlier motion probe still has large `visibleHashChanges` because the player/camera path crosses many renderer visibility boundaries:
  - after EastField removal `motionFollowCapture visibleHashChanges=124`
  - after EastField removal `motionFollowRenderNoSave visibleHashChanges=126`
- The remaining OutdoorVoidBackground visibility hash changes are visually negligible in the static probe.

## Next action

- Propagate this review folder to R2 and anemora-viewer.
- Continue small right-edge fragment classification separately; it is not explained by static flicker.
- If the user still sees object pop while walking, add a follow-camera ROI/performance probe that records camera frustum state and frame time around the exact visible pop location.
