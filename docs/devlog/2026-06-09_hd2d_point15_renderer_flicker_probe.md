# 2026-06-09 HD2D point15 renderer flicker probe

## Context

- Project: Anemora Fast VS House Slice HD-2D recovery.
- Branch: `wip/hd2d-point15-recovery-20260609`.
- Baseline: point15 `e7277f0a`.
- Slice: D flicker/performance/long-road diagnostics.
- Acceptance source: built-player capture only.
- Scope: data only; no renderer/material/generator fix in this slice.
- Accepted review folder: `docs/review/2026-06-09T15-08_renderer_flicker_probe_windowed/`.

## Probe

- Added built-player-only argument: `--anemora-house-slice-renderer-flicker-dir`.
- Captures one wide context frame and 24 fixed-camera close frames around the current central-plaza library facade.
- Logs per-frame `deltaTime`, `unscaledDeltaTime`, camera pose, visible renderer count/hash, enabled renderer count, image mean absolute RGB delta, and changed sample percent.
- Adds a 180-frame no-capture runtime pass in a windowed player to separate actual runtime stability from per-frame PNG capture overhead.
- The probe does not permanently disable renderers, change materials, or regenerate scene data.

## Build Evidence

- Log: `Logs/point15_renderer_flicker_probe_build_validate_20260609T150026.log`.
- `DisplayProgressNotification: Build Successful`
- `Build Finished, Result: Success.`
- `PlayerBuildInfo duration=76455`
- Built player: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`.

## Built-Player Evidence

- Accepted player log: `Logs/point15_renderer_flicker_probe_windowed_player_20260609T150845.log`.
- `- Loaded All Assemblies, in  0.158 seconds`
- `ANEMORA_HOUSE_SLICE_RENDERER_FLICKER_PROBE: end count=25`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Captured PNGs:

```text
00_context_current_plaza_library_facade.png 1077100 bytes 1280x720
01_close_frame_01.png                        955117 bytes 1280x720
02_close_frame_02.png                        957938 bytes 1280x720
03_close_frame_03.png                        957762 bytes 1280x720
04_close_frame_04.png                        957814 bytes 1280x720
05_close_frame_05.png                        957750 bytes 1280x720
06_close_frame_06.png                        957847 bytes 1280x720
07_close_frame_07.png                        957882 bytes 1280x720
08_close_frame_08.png                        957747 bytes 1280x720
09_close_frame_09.png                        957852 bytes 1280x720
10_close_frame_10.png                        957820 bytes 1280x720
11_close_frame_11.png                        957798 bytes 1280x720
12_close_frame_12.png                        957806 bytes 1280x720
13_close_frame_13.png                        957756 bytes 1280x720
14_close_frame_14.png                        957809 bytes 1280x720
15_close_frame_15.png                        957781 bytes 1280x720
16_close_frame_16.png                        957810 bytes 1280x720
17_close_frame_17.png                        957745 bytes 1280x720
18_close_frame_18.png                        957909 bytes 1280x720
19_close_frame_19.png                        957889 bytes 1280x720
20_close_frame_20.png                        957824 bytes 1280x720
21_close_frame_21.png                        957797 bytes 1280x720
22_close_frame_22.png                        957751 bytes 1280x720
23_close_frame_23.png                        957855 bytes 1280x720
24_close_frame_24.png                        957834 bytes 1280x720
```

## Measurements

Fixed-camera capture pass:

```text
ANEMORA_HOUSE_SLICE_RENDERER_FLICKER_PROBE: summary phase=captureFrames frames=24 saved=25 tracked=11067 visible=min=577,max=790,mean=585.875,stddev=42.563 enabled=min=1574,max=1574,mean=1574.000,stddev=0.000 visibleHashChanges=1 imageMeanAbsRgb=min=0.000,max=0.207,mean=0.018,stddev=0.049 imageChangedSamplePct=min=0.000,max=1.556,mean=0.123,stddev=0.354 deltaTime=min=0.084,max=0.333,mean=0.102,stddev=0.049 unscaledDeltaTime=min=0.084,max=0.548,mean=0.111,stddev=0.092
```

Windowed no-capture runtime pass:

```text
ANEMORA_HOUSE_SLICE_RENDERER_FLICKER_PROBE: runtimeSummary phase=runtimeWarmNoCapture frames=180 tracked=11067 visible=min=518,max=577,mean=518.328,stddev=4.385 enabled=min=1574,max=1574,mean=1574.000,stddev=0.000 visibleHashChanges=1 deltaTime=min=0.012,max=0.305,mean=0.019,stddev=0.022 unscaledDeltaTime=min=0.012,max=0.305,mean=0.019,stddev=0.022
```

Warm-up visible-set deltas:

```text
ANEMORA_HOUSE_SLICE_RENDERER_FLICKER_PROBE: visibleDelta frame=1 added=0 removed=213 ...
ANEMORA_HOUSE_SLICE_RENDERER_FLICKER_PROBE: runtimeVisibleDelta phase=runtimeWarmNoCapture frame=1
ANEMORA_HOUSE_SLICE_RENDERER_FLICKER_PROBE: visibleDelta frame=1 added=36 removed=95 ...
```

Transparent/background candidates found in the runtime toggle sample:

```text
Current_CentralPlaza_OutdoorBackgroundSkyDepth_LeftSkyWrapA: queue=2994, tagRenderType=Transparent, _Surface=1, _Cull=0, _ZWrite=0
Current_CentralPlaza_OutdoorBackgroundSkyDepth_RightSkyWrapA: queue=2994, tagRenderType=Transparent, _Surface=1, _Cull=0, _ZWrite=0
Current_CentralPlaza_OutdoorBackgroundSkyDepth_RooflineCenterA: queue=2994, tagRenderType=Transparent, _Surface=1, _Cull=0, _ZWrite=0
Current_CentralPlaza_OutdoorBackgroundSkyDepth_RooflineLeftA: queue=2994, tagRenderType=Transparent, _Surface=1, _Cull=0, _ZWrite=0
Current_CentralPlaza_OutdoorBackgroundSkyDepth_RooflineRightA: queue=2994, tagRenderType=Transparent, _Surface=1, _Cull=0, _ZWrite=0
Current_CentralPlaza_OutdoorBackgroundSkyDepth_SkyCurtainA: queue=2994, tagRenderType=Transparent, _Surface=1, _Cull=0, _ZWrite=0
Current_CentralPlaza_OutdoorSkyDetail_CloudRakeA: queue=2993, tagRenderType=Transparent, _Surface=1, _Cull=0, _ZWrite=0
Current_CentralPlaza_OutdoorSkyDetail_CloudRakeB: queue=2993, tagRenderType=Transparent, _Surface=1, _Cull=0, _ZWrite=0
Current_CentralPlaza_OutdoorSkyDetail_DistantRooflineA: queue=2993, tagRenderType=Transparent, _Surface=1, _Cull=0, _ZWrite=0
```

## Findings

- The fixed close camera did not show repeated renderer enabled blinking: `enabled=min=1574,max=1574,mean=1574.000,stddev=0.000`.
- Visible-set changes occur at frame 0 to 1 warm-up, then remain stable for the captured fixed view.
- Image differences are small in the fixed close sequence: `imageMeanAbsRgb max=0.207`, `imageChangedSamplePct max=1.556`.
- The right/back "long road" object is visible in `00_context_current_plaza_library_facade.png`; this should be treated separately from the facade close-up transparent veil.
- The transparent/background sky-depth stack around `OutdoorBackgroundSkyDepth` / `OutdoorSkyDetail` is confirmed as transparent, two-sided, non-depth-writing (`_Surface=1`, `_Cull=0`, `_ZWrite=0`) and can contribute to background bands/outline bleed.
- Batchmode no-capture `Renderer.isVisible` is not valid for this diagnosis because the real game view does not render; the accepted runtime measurement is the windowed built-player run.

## Next Action

- Do not apply a visual fix in this slice.
- Next D fix candidate should be a narrow transparent/background-depth pass that separates:
  - facade close haze/veil stack from `Cycle125/126/120` and `ShadowFoundationCycle70`;
  - outdoor background/long-road/sky-depth bands from `OutdoorBackgroundSkyDepth`, `OutdoorSkyDetail`, and road/ground continuation objects.
- Any fix must be built-player captured, compared against this probe, and propagated to `anemora-viewer`.

## Viewer

- Propagate target: `work/chapter1-continuation-map-vs-20260524`.
- R2 upload: `uploaded 27 files for chapter1-continuation-map-vs-20260524/2026-06-09T15-08_renderer_flicker_probe_windowed (bucket TTL 45d); manifest now lists 116 paths`.
- Git-triggered viewer rebuild commit: `2ac867b chore: refresh review content 2026-06-09 renderer-flicker-probe`.
- Public review polling: polls 1-34 stayed stale at `length=400048`, `hasTarget=False`, `has260=False`, `has1246=False`; poll 35 returned `status=200`, `length=401769`, `hasTarget=True`, `has260=True`, `has1246=True`.
- Public review page: `https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/review/`.
- Public review DOM check: title `Review -- work/chapter1-continuation-map-vs-20260524`; `has260=True`; `has1246=True`; target album present.
- Public latest thumbnail: `thumbs/chapter1-continuation-map-vs-20260524/docs/review/2026-06-09T15-08_renderer_flicker_probe_windowed/00_context_current_plaza_library_facade.webp`, `complete=True`, `naturalWidth=512`, `naturalHeight=288`.
- Public album page: `https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/gallery/docs/review/2026-06-09T15-08_renderer_flicker_probe_windowed/`.
- Public album DOM check: `imageCount=25`, `allLoaded=True`; first and last thumbnails loaded with `naturalWidth=512`, `naturalHeight=288`.
- Public devlog initial DOM check before this final viewer section: title `docs/devlog/2026-06-09_hd2d_point15_renderer_flicker_probe.md -- Docs`, `textLength=74475`; found `runtimeSummary phase=runtimeWarmNoCapture`, `imageChangedSamplePct=min=0.000,max=1.556`, and `Current_CentralPlaza_OutdoorBackgroundSkyDepth_RooflineCenterA`; the earlier placeholder was still present before this update.
- Public viewer recheck after user report that `anemora-viewer` was stale, review HTML with cache-buster: `status=200`, `length=401769`, `has_renderer_album=True`, `has_timewindow_album=True`, `has_260_cycles=True`, `has_1246_images=True`, `has_259_cycles=False`, `has_1221_images=False`.
- Public viewer recheck, correct album URL: `https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/gallery/docs/review/2026-06-09T15-08_renderer_flicker_probe_windowed/`; HTML `status=200`, `length=40896`, `hasTarget=True`, `hasDevlog=True`, `hasContext=True`, `hasFrame24=True`.
- Public viewer recheck, album embedded image routes: `imgCount=25`, `origCount=25`, `thumbCount=25`, `hasPrevB=True`, `hasNextNull=True`.
- Public viewer recheck, image fetches: `/originals/chapter1-continuation-map-vs-20260524/docs/review/2026-06-09T15-08_renderer_flicker_probe_windowed/24_close_frame_24.png` returned `status=200`, `type=image/png`, `length=957834`; `/thumbs/chapter1-continuation-map-vs-20260524/docs/review/2026-06-09T15-08_renderer_flicker_probe_windowed/24_close_frame_24.webp` returned `status=200`, `type=image/webp`, `length=13020`.
- Public viewer recheck, devlog route: `https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/docs/docs/devlog/2026-06-09_hd2d_point15_renderer_flicker_probe/` returned `status=200`, `length=1015355`, `hasRuntimeSummary=True`, `hasR2=True`, `hasPending=False`. The route without trailing slash returns `308`; use the trailing-slash URL for direct checks.
- Public viewer redeploy after devlog recheck upload: viewer commit `8c881a5 chore: refresh review devlog 2026-06-09 renderer-flicker-probe recheck`; poll 1-42 kept `okDev=False`; poll 43 returned `reviewLen=401769`, `albumLen=40896`, `devLen=1017306`, `okReview=True`, `okAlbum=True`, `okDev=True`.
