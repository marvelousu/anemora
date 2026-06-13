# 2026-06-12 HD2D point15 renderer flicker motion probe RenderNoSave

## Scope

- Continue the point15 flicker / object-pop diagnostic line after the HighSunbeam alpha 0.72 slice.
- Fix the diagnostic probe only; do not change renderer behavior, materials, generator output, or scene content in this slice.
- Replace the invalid `Renderer.isVisible` no-capture probe with a built-player `RenderNoSave` pass that renders the camera to a temporary RenderTexture before reading `Renderer.isVisible`.
- Acceptance source remains built-player evidence only.

## Code change

- `Assets/Scripts/FastVS/FastVsHouseRuntimeSmokeProbe.cs`
  - Added `RenderCameraForVisibilitySample(Camera camera)`.
  - `SampleRendererFlickerRuntime(...)` now calls `RenderCameraForVisibilitySample(camera)` before sampling `Renderer.isVisible`.
  - `RunRendererMotionProbe(...)` now calls `RenderCameraForVisibilitySample(camera)` every motion frame before sampling the visible renderer set.
  - `SampleRendererMotionRuntime(...)` now calls `RenderCameraForVisibilitySample(camera)` before sampling.
  - Renamed diagnostic labels from `runtimeWarmNoCapture` to `runtimeWarmRenderNoSave`, and from `motionFollowNoCapture` to `motionFollowRenderNoSave`.

## Build evidence

- Build log:
  - `Logs\point15_renderer_visibility_probe_rendernosave_build_validate_20260612T181238.log`
- Exit:
  - `0`
- Key lines:
  - `DisplayProgressNotification: Build Successful`
  - `Build Finished, Result: Success.`
  - `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Invalid attempt discarded

- The first built-player re-run used `-nographics`.
- It produced gray blank PNGs and zero image deltas, so those folders were deleted from `docs/review/` and are not propagated to anemora-viewer.
- The remaining logs are diagnostic only:
  - `Logs\point15_renderer_flicker_probe_rendernosave_player_20260612T182237.log`
  - `Logs\point15_renderer_motion_probe_rendernosave_player_20260612T182311.log`
- Do not use the `-nographics` image measurements for visual review.

## Built-player evidence

- Valid fixed-camera player log:
  - `Logs\point15_renderer_flicker_probe_rendernosave_windowed_player_20260612T182414.log`
- Valid fixed-camera review folder:
  - `docs\review\2026-06-12T18-24_renderer_flicker_probe_rendernosave_windowed`
- Valid motion player log:
  - `Logs\point15_renderer_motion_probe_rendernosave_windowed_player_20260612T182443.log`
- Valid motion review folder:
  - `docs\review\2026-06-12T18-24_renderer_motion_probe_rendernosave_windowed`
- Captures:
  - fixed-camera: 25 PNGs
  - motion: 13 PNGs

## Renderer contract

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

## Lighting state

```text
ANEMORA_HOUSE_SLICE_LIGHTING_STATE: label=rendererFlickerProbe.ready expectedArea=CentralPlaza activeArea=CentralPlaza sunCurrent=Morning sunTarget=Morning sunTransitioning=False indoorSunSuppression=False main=name=Directional Light,enabled=True,type=Directional,intensity=1.500,color=(1.000,0.840,0.700,1.000),shadows=Soft,shadowStrength=0.800,shadowBias=0.012,shadowNormalBias=0.100 warm=name=FastVS_HD2D_WarmFillLight,enabled=True,type=Point,intensity=0.300,color=(1.000,0.640,0.340,1.000),shadows=None,shadowStrength=1.000,shadowBias=0.050,shadowNormalBias=0.400 cool=name=FastVS_HD2D_CoolRimLight,enabled=True,type=Directional,intensity=0.160,color=(0.460,0.580,0.920,1.000),shadows=None,shadowStrength=1.000,shadowBias=0.050,shadowNormalBias=0.400 ambientMode=Flat ambient=(0.074,0.068,0.058,1.000) ambientMax=0.074 fog=False fogColor=(0.235,0.215,0.178,1.000) fogDensity=0.011 shadowPolicy=renderers=11832,active=1608,enabled=11752,castOn=801,shadowsOnly=46,receive=1459,activeCastOn=64,activeReceive=176
```

## Fixed-camera measurements

Capture pass:

```text
ANEMORA_HOUSE_SLICE_RENDERER_FLICKER_PROBE: summary phase=captureFrames frames=24 saved=25 tracked=11063 visible=min=567,max=780,mean=575.875,stddev=42.563 enabled=min=1562,max=1562,mean=1562.000,stddev=0.000 visibleHashChanges=1 imageMeanAbsRgb=min=0.000,max=0.215,mean=0.018,stddev=0.050 imageChangedSamplePct=min=0.000,max=1.563,mean=0.124,stddev=0.355 deltaTime=min=0.080,max=0.333,mean=0.095,stddev=0.050 unscaledDeltaTime=min=0.080,max=1.106,mean=0.128,stddev=0.204
```

RenderNoSave runtime pass:

```text
ANEMORA_HOUSE_SLICE_RENDERER_FLICKER_PROBE: runtimeFrame phase=runtimeWarmRenderNoSave index=0 deltaTime=0.3333 unscaledDeltaTime=0.3436 realtime=5.786 visible=603 enabled=1562 visibleHash=0x537A6363 cameraPos=(21.450,2.957,11.600) cameraEuler=(22.097,0.000,0.000) fov=38.000 mask=-268435457
ANEMORA_HOUSE_SLICE_RENDERER_FLICKER_PROBE: runtimeFrame phase=runtimeWarmRenderNoSave index=30 deltaTime=0.0127 unscaledDeltaTime=0.0127 realtime=6.188 visible=508 enabled=1562 visibleHash=0xC038E654 cameraPos=(21.450,2.957,11.600) cameraEuler=(22.097,0.000,0.000) fov=38.000 mask=-268435457
ANEMORA_HOUSE_SLICE_RENDERER_FLICKER_PROBE: runtimeFrame phase=runtimeWarmRenderNoSave index=60 deltaTime=0.0158 unscaledDeltaTime=0.0158 realtime=6.602 visible=508 enabled=1562 visibleHash=0xC038E654 cameraPos=(21.450,2.957,11.600) cameraEuler=(22.097,0.000,0.000) fov=38.000 mask=-268435457
ANEMORA_HOUSE_SLICE_RENDERER_FLICKER_PROBE: runtimeFrame phase=runtimeWarmRenderNoSave index=90 deltaTime=0.0123 unscaledDeltaTime=0.0123 realtime=7.009 visible=508 enabled=1562 visibleHash=0xC038E654 cameraPos=(21.450,2.957,11.600) cameraEuler=(22.097,0.000,0.000) fov=38.000 mask=-268435457
ANEMORA_HOUSE_SLICE_RENDERER_FLICKER_PROBE: runtimeFrame phase=runtimeWarmRenderNoSave index=120 deltaTime=0.0122 unscaledDeltaTime=0.0122 realtime=7.422 visible=508 enabled=1562 visibleHash=0xC038E654 cameraPos=(21.450,2.957,11.600) cameraEuler=(22.097,0.000,0.000) fov=38.000 mask=-268435457
ANEMORA_HOUSE_SLICE_RENDERER_FLICKER_PROBE: runtimeFrame phase=runtimeWarmRenderNoSave index=150 deltaTime=0.0118 unscaledDeltaTime=0.0118 realtime=7.769 visible=508 enabled=1562 visibleHash=0xC038E654 cameraPos=(21.450,2.957,11.600) cameraEuler=(22.097,0.000,0.000) fov=38.000 mask=-268435457
ANEMORA_HOUSE_SLICE_RENDERER_FLICKER_PROBE: runtimeFrame phase=runtimeWarmRenderNoSave index=179 deltaTime=0.0132 unscaledDeltaTime=0.0132 realtime=8.146 visible=508 enabled=1562 visibleHash=0xC038E654 cameraPos=(21.450,2.957,11.600) cameraEuler=(22.097,0.000,0.000) fov=38.000 mask=-268435457
ANEMORA_HOUSE_SLICE_RENDERER_FLICKER_PROBE: runtimeSummary phase=runtimeWarmRenderNoSave frames=180 tracked=11063 visible=min=508,max=603,mean=508.528,stddev=7.061 enabled=min=1562,max=1562,mean=1562.000,stddev=0.000 visibleHashChanges=1 deltaTime=min=0.010,max=0.333,mean=0.015,stddev=0.024 unscaledDeltaTime=min=0.010,max=0.344,mean=0.015,stddev=0.025
```

## Motion measurements

Capture pass:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: summary phase=motionFollowCapture frames=180 saved=13 tracked=11063 visible=min=349,max=534,mean=463.244,stddev=58.733 enabled=min=1562,max=1562,mean=1562.000,stddev=0.000 backgroundVisible=min=1,max=3,mean=2.400,stddev=0.611 visibleHashChanges=119 backgroundHashChanges=6 imageMeanAbsRgb=min=11.881,max=33.552,mean=23.908,stddev=6.851 imageChangedSamplePct=min=48.639,max=81.326,mean=73.907,stddev=8.983 deltaTime=min=0.017,max=0.333,mean=0.031,stddev=0.038 unscaledDeltaTime=min=0.017,max=0.579,mean=0.033,stddev=0.057
```

RenderNoSave runtime pass:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: runtimeSummary phase=motionFollowRenderNoSave frames=180 tracked=11063 visible=min=349,max=545,mean=462.650,stddev=58.597 enabled=min=1562,max=1562,mean=1562.000,stddev=0.000 backgroundVisible=min=1,max=3,mean=2.372,stddev=0.615 visibleHashChanges=132 backgroundHashChanges=2 deltaTime=min=0.019,max=0.333,mean=0.026,stddev=0.023 unscaledDeltaTime=min=0.019,max=0.424,mean=0.026,stddev=0.030
```

Background visible-set transitions:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: backgroundVisibleDelta frame=80 previous=5569A502 current=A18DF36A currentSample=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteLeft | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteRight]
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: backgroundVisibleDelta frame=91 previous=A18DF36A current=6C6F13A6 currentSample=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_EastEdgeWash | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteLeft | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteRight]
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: backgroundVisibleDelta frame=93 previous=6C6F13A6 current=A18DF36A currentSample=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteLeft | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteRight]
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: backgroundVisibleDelta frame=106 previous=A18DF36A current=6C6F13A6 currentSample=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_EastEdgeWash | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteLeft | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteRight]
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: backgroundVisibleDelta frame=108 previous=6C6F13A6 current=A18DF36A currentSample=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteLeft | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteRight]
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: backgroundVisibleDelta frame=168 previous=A18DF36A current=DAA5E1A7 currentSample=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteRight]
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: runtimeBackgroundVisibleDelta phase=motionFollowRenderNoSave frame=80 previous=5569A502 current=A18DF36A currentSample=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteLeft | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteRight]
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: runtimeBackgroundVisibleDelta phase=motionFollowRenderNoSave frame=167 previous=A18DF36A current=DAA5E1A7 currentSample=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteRight]
```

## Findings

- The fixed-camera close view does not show repeated renderer blinking after warm-up.
- In the fixed-camera capture pass, `enabled` is constant: `enabled=min=1562,max=1562,mean=1562.000,stddev=0.000`.
- In the fixed-camera RenderNoSave pass, frame 1 is the only visible-set transition; frame 30 through frame 179 stay at `visible=508` with `visibleHash=0xC038E654`.
- The previous `Renderer.isVisible` no-capture zeros were a diagnostic artifact. Rendering to a temporary RT before sampling restores valid visible counts.
- During camera motion, `enabled` remains constant at 1562 while visible sets change heavily. This is consistent with camera/frustum movement rather than objects being enabled/disabled at runtime.
- The motion background transitions are concentrated in `Current_CentralPlaza_OutdoorVoidBackground_*`, especially:
  - `OutdoorVoidBackground_NorthSilhouetteLeft`
  - `OutdoorVoidBackground_NorthSilhouetteRight`
  - `OutdoorVoidBackground_EastEdgeWash`
- This slice does not prove the user-visible flicker is fixed. It narrows the next suspect set to background/void silhouettes and edge-wash objects under moving-camera conditions.

## Next action

- Propagate the two valid review folders to anemora-viewer.
- Next diagnostic/fix slice should isolate `OutdoorVoidBackground_*` under a moving camera:
  - log material `renderQueue`, `_Surface`, `_Cull`, `_ZWrite`, `_ZTest`, bounds, and world position for the three named objects;
  - capture motion variants with `OutdoorVoidBackground_EastEdgeWash` disabled, then with all `OutdoorVoidBackground_*` disabled;
  - do not change gameplay/lighting until that isolation result is measured.

## Viewer

- Propagate target:
  - `work/chapter1-continuation-map-vs-20260524`
- R2 upload, first valid pass:
  - `uploaded 26 files for chapter1-continuation-map-vs-20260524/2026-06-12T18-24_renderer_flicker_probe_rendernosave_windowed (bucket TTL 45d); manifest now lists 346 paths`
  - `uploaded 14 files for chapter1-continuation-map-vs-20260524/2026-06-12T18-24_renderer_motion_probe_rendernosave_windowed (bucket TTL 45d); manifest now lists 360 paths`
- `devlog.txt` format fix:
  - First line must be the bare `docs/devlog/...md` path.
  - The initial `Devlog: docs/devlog/...` prefix produced a malformed local viewer link and did not upload the markdown devlog.
- R2 upload after `devlog.txt` fix:
  - `uploaded 27 files for chapter1-continuation-map-vs-20260524/2026-06-12T18-24_renderer_flicker_probe_rendernosave_windowed (bucket TTL 45d); manifest now lists 361 paths`
  - `uploaded 15 files for chapter1-continuation-map-vs-20260524/2026-06-12T18-24_renderer_motion_probe_rendernosave_windowed (bucket TTL 45d); manifest now lists 361 paths`
- Local viewer build:
  - `npm run build`
  - Exit: `0`
  - `setup-r2-images`: `chapter1-continuation-map-vs-20260524: fetched 357/361 files`
  - `collect-content`: `files: 5763, docs: 972, images: 3083, unsupported: 600`
  - Existing missing screenshot / PWA glob warnings remained; they were not introduced by this slice.
- Local dist checks:
  - `dist\chapter1-continuation-map-vs-20260524\gallery\docs\review\2026-06-12T18-24_renderer_flicker_probe_rendernosave_windowed\index.html exists=True length=43079`
  - `dist\chapter1-continuation-map-vs-20260524\gallery\docs\review\2026-06-12T18-24_renderer_motion_probe_rendernosave_windowed\index.html exists=True length=28591`
  - `dist\chapter1-continuation-map-vs-20260524\docs\docs\devlog\2026-06-12_hd2d_point15_renderer_flicker_motion_probe_rendernosave\index.html exists=True length=1029972`
  - `flicker hasMalformed=False hasGoodLink=True hasFrame24=True`
  - `motion hasMalformed=False hasGoodLink=True hasFrame179=True`
  - `devlog hasRuntime=True hasOutdoor=True hasInvalid=True length=1029970`
- Viewer commit:
  - `4feefbc chore: refresh renderer flicker motion review`
- Public review polling:
  - Attempts 1-29 returned `404`.
  - Attempt 30 returned `flickerLen=43077 motionLen=28587 devLen=1030064 okF=True okM=True okD=True`.
- Public URLs:
  - `https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/gallery/docs/review/2026-06-12T18-24_renderer_flicker_probe_rendernosave_windowed/`
  - `https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/gallery/docs/review/2026-06-12T18-24_renderer_motion_probe_rendernosave_windowed/`
  - `https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/docs/docs/devlog/2026-06-12_hd2d_point15_renderer_flicker_motion_probe_rendernosave/`
- Public image checks:
  - `24_close_frame_24.png`: `status=200 type=image/png length=787343`
  - `24_close_frame_24.webp`: `status=200 type=image/webp length=11874`
  - `12_motion_frame_179.png`: `status=200 type=image/png length=617344`
  - `12_motion_frame_179.webp`: `status=200 type=image/webp length=8950`
