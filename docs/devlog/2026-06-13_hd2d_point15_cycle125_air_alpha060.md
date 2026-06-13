# 2026-06-13 HD2D point15 Cycle125 air alpha 0.60

## Scope

- Continue the point15 fog / white-haze line after the library facade occlusion gradient removal.
- Keep the built-player as the only visual acceptance source.
- Do not delete the library-front sunbeam cue.
- Tune only the Cycle125 air material alpha from `0.72` to `0.60`.

## Pre-tune reprobe

- Review folder:
  - `docs\review\2026-06-13T09-20_renderer_front_fog_reprobe_after_facade_gradient_removed`
- Player log:
  - `Logs\point15_renderer_front_fog_reprobe_after_facade_gradient_removed_20260613T0920.log`
- Captures:
  - `PNG_COUNT=86`
  - `PNG_TOTAL_BYTES=72582104`
- Completion:
  - `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: end count=86`
- Contract:
  - `ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>`
- Pale-wash suppression:
  - `[Point15Recovery] ApplyRendererShadowPolicy area=CentralPlaza: disabled 14 library-front pale wash renderer(s).`

Pre-tune residual close-shot deltas:

```text
transparentOverlayOff close: meanAbsRgb=2.612 changedSamplePct=10.446 changed=6017 samples=57600
transparentCycle125Off close: meanAbsRgb=2.424 changedSamplePct=10.003 changed=5762 samples=57600
cycle125HighSunbeamColumnOff close: meanAbsRgb=1.492 changedSamplePct=7.099 changed=4089 samples=57600
cycle125CenterChalkSunCatchOff close: meanAbsRgb=0.756 changedSamplePct=1.674 changed=964 samples=57600
cycle125BackDepthHazeOff close: meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600
```

Interpretation: after facade occlusion gradient removal, the remaining measured close-shot white haze was still dominated by Cycle125, with `HighSunbeamColumnA` as the largest single active contributor. `BackDepthHazeA` remained disabled/no-op.

## Code change

- `Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
  - Changed `EnsureHd2dPlazaReferenceDioramaAirCycle125Material()` tint from `new Color(1f, 1f, 1f, 0.72f)` to `new Color(1f, 1f, 1f, 0.60f)`.
- Regenerated material:
  - `Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_reference_diorama_air_cycle125.mat`
  - `_BaseColor: {r: 1, g: 1, b: 1, a: 0.6}`
  - `_Color: {r: 1, g: 1, b: 1, a: 0.6}`

## Build evidence

- Build log:
  - `Logs\unity_build_validate_cycle125_air_alpha060_20260613T0915.log`
- Key lines:

```text
Fast VS house slice validation passed.
DisplayProgressNotification: Build Successful
Build Finished, Result: Success.
Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe
Exiting batchmode successfully now!
```

- Built player:
  - `Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
  - `Length=667648`
  - `LastWriteTime=2026-06-13 09:19:08`

## Built-player evidence

### Renderer isolation

- Review folder:
  - `docs\review\2026-06-13T09-30_renderer_front_fog_alpha060_isolation`
- Player log:
  - `Logs\point15_renderer_front_fog_alpha060_isolation_20260613T0930.log`
- Captures:
  - `PNG_COUNT=86`
  - `PNG_TOTAL_BYTES=72445437`
- Completion:
  - `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: end count=86`
- Contract:
  - `ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>`
- Pale-wash suppression:
  - `[Point15Recovery] ApplyRendererShadowPolicy area=CentralPlaza: disabled 14 library-front pale wash renderer(s).`

Alpha 0.60 residual close-shot deltas:

```text
transparentOverlayOff close: meanAbsRgb=2.363 changedSamplePct=10.175 changed=5861 samples=57600
transparentCycle125Off close: meanAbsRgb=2.174 changedSamplePct=9.733 changed=5606 samples=57600
cycle125HighSunbeamColumnOff close: meanAbsRgb=1.243 changedSamplePct=6.828 changed=3933 samples=57600
cycle125CenterChalkSunCatchOff close: meanAbsRgb=0.756 changedSamplePct=1.674 changed=964 samples=57600
cycle125BackDepthHazeOff close: meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600
```

Delta vs pre-tune reprobe:

```text
transparentOverlayOff close meanAbsRgb: 2.612 -> 2.363
transparentCycle125Off close meanAbsRgb: 2.424 -> 2.174
cycle125HighSunbeamColumnOff close meanAbsRgb: 1.492 -> 1.243
cycle125HighSunbeamColumnOff close changedSamplePct: 7.099 -> 6.828
```

### All-map capture

- Review folder:
  - `docs\review\2026-06-13T09-35_cycle125_air_alpha060_allmaps`
- Player log:
  - `Logs\point15_cycle125_air_alpha060_allmaps_20260613T0935.log`
- Captures:
  - `PNG_COUNT=13`
  - `PNG_TOTAL_BYTES=8304380`
- Completion:
  - `ANEMORA_HOUSE_SLICE_CAPTURE: end count=13`
- CentralPlaza lighting:
  - `main intensity=1.500`
  - `warm intensity=0.300`
  - `cool intensity=0.160`
  - `ambientMax=0.074`
  - `fog=False`

### Motion probe

- Review folder:
  - `docs\review\2026-06-13T09-45_renderer_motion_after_cycle125_air_alpha060`
- Player log:
  - `Logs\point15_renderer_motion_after_cycle125_air_alpha060_20260613T0945.log`
- Captures:
  - `PNG_COUNT=141`
  - `PNG_TOTAL_BYTES=105404724`
- Completion:
  - `ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: end count=141`
- Summary:
  - `visible=min=342,max=525,mean=455.311,stddev=59.025`
  - `enabled=min=1540,max=1540,mean=1540.000,stddev=0.000`
  - `backgroundVisible=min=1,max=3,mean=2.400,stddev=0.611`
  - `visibleHashChanges=128`
  - `backgroundHashChanges=6`
  - `imageMeanAbsRgb=min=12.680,max=34.137,mean=23.775,stddev=6.921`
  - `imageChangedSamplePct=min=50.347,max=81.021,mean=73.307,stddev=8.798`
- Runtime summary:
  - `visible=min=343,max=544,mean=456.083,stddev=59.380`
  - `enabled=min=1540,max=1540,mean=1540.000,stddev=0.000`
  - `backgroundVisible=min=1,max=3,mean=2.417,stddev=0.622`
  - `visibleHashChanges=126`
  - `backgroundHashChanges=4`

## Interpretation

- The alpha 0.60 slice reduced the measured library-front haze while preserving the sunbeam object.
- The largest active single contributor remains `HighSunbeamColumnA`, but its close-shot contribution dropped from `meanAbsRgb=1.492` to `1.243`.
- `BackDepthHazeA` is still inactive/no-op.
- Renderer enabled state did not flicker in the motion probe: `enabled=min=1540,max=1540,stddev=0.000`.
- This slice does not address portal aperture compositing, time-window physics, or the remaining opaque visibility toggles.

## Viewer

- Propagate target:
  - `work/chapter1-continuation-map-vs-20260524`
- R2 uploads:
  - `uploaded 87 files for chapter1-continuation-map-vs-20260524/2026-06-13T09-20_renderer_front_fog_reprobe_after_facade_gradient_removed (bucket TTL 45d); manifest now lists 1757 paths`
  - `uploaded 87 files for chapter1-continuation-map-vs-20260524/2026-06-13T09-30_renderer_front_fog_alpha060_isolation (bucket TTL 45d); manifest now lists 1844 paths`
  - `uploaded 14 files for chapter1-continuation-map-vs-20260524/2026-06-13T09-35_cycle125_air_alpha060_allmaps (bucket TTL 45d); manifest now lists 1858 paths`
  - `uploaded 142 files for chapter1-continuation-map-vs-20260524/2026-06-13T09-45_renderer_motion_after_cycle125_air_alpha060 (bucket TTL 45d); manifest now lists 2000 paths`
- anemora-viewer marker target:
  - `2026-06-13T09:45:00+09:00 R2 Cycle125 air alpha 0.60 review sync`
- anemora-viewer rebuild:
  - `setup-r2-images: fetched 1996/2000 safe files (2000 manifest paths)`
  - `collect-content: files: 7402, docs: 979, images: 4687, unsupported: 602`
  - `collect-content: wrote src\data\branches.json (1 branches, 979 docs, 4498 images)`
  - `npm run build:fast: 1568 page(s) built in 125.86s`
  - Commit: `cc7d294 chore: refresh cycle125 alpha review deploy`
- Public marker:
  - `attempt=44 status=200 body=2026-06-13T09:45:00+09:00 R2 Cycle125 air alpha 0.60 review sync`
- Public gallery checks:
  - `200 LEN=144624 https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/gallery/docs/review/2026-06-13T09-20_renderer_front_fog_reprobe_after_facade_gradient_removed/`
  - `200 LEN=136403 https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/gallery/docs/review/2026-06-13T09-30_renderer_front_fog_alpha060_isolation/`
  - `200 LEN=27746 https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/gallery/docs/review/2026-06-13T09-35_cycle125_air_alpha060_allmaps/`
  - `200 LEN=206758 https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/gallery/docs/review/2026-06-13T09-45_renderer_motion_after_cycle125_air_alpha060/`
- Public representative PNG checks:
  - `200 LEN=759123 .../38_no_cycle125_high_sunbeam_column_current_library_facade_close.png`
  - `200 LEN=903683 .../03_b1_b3_current.png`
  - `200 LEN=814794 .../06_motion_frame_090.png`
