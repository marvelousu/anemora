# 2026-06-09 HD2D point15 renderer isolation probe

## Context

- Project: Anemora Fast VS House Slice HD-2D recovery.
- Branch: `wip/hd2d-point15-recovery-20260609`.
- Baseline: point15 `e7277f0a`.
- Slice: D flicker/transparency/long-road isolation probe.
- Acceptance source: built-player capture only.
- Scope: data only; no renderer/generator/scene fix in this slice.
- Review folder: `docs/review/2026-06-09T12-10_renderer_isolation_probe/`.

## Probe

- Added `--anemora-house-slice-renderer-isolation-dir`.
- Runtime-only isolation; the scene and generator are not permanently changed by this probe.
- Captured baseline, transparent-overlay-off, long-road-stack-off, and past-long-road-off views.
- Each isolation variant disables matching renderers only around the capture, then restores them before the next variant.

## Build Evidence

- Log: `Logs/point15_renderer_isolation_build_validate_20260609T120220.log`.
- `DisplayProgressNotification: Build Successful`
- `Build Finished, Result: Success.`
- `PlayerBuildInfo duration=89954`
- Built player: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`.

## Built-Player Evidence

- Player log: `Logs/point15_renderer_isolation_player_20260609T121047.log`.
- `- Loaded All Assemblies, in  3.228 seconds`
- `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: end count=8`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Captures:

```text
01_baseline_current_plaza_library_facade.png                    1077005 1280x720
02_baseline_current_library_facade_close.png                     962471 1280x720
03_no_transparent_overlay_current_plaza_library_facade.png      1050204 1280x720
04_no_transparent_overlay_current_library_facade_close.png       742061 1280x720
05_no_long_road_stack_current_plaza_library_facade.png          1072968 1280x720
06_no_long_road_stack_current_library_facade_close.png           962423 1280x720
07_no_past_long_road_current_plaza_library_facade.png           1077006 1280x720
08_no_past_long_road_current_library_facade_close.png            962454 1280x720
```

## Isolation Counts

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=transparentOverlayOff matched=32 disabled=30
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=transparentOverlayOff restored=30
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=longRoadStackOff matched=8 disabled=7
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=longRoadStackOff restored=7
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=pastCentralPlazaLongRoadOff matched=4 disabled=4
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=pastCentralPlazaLongRoadOff restored=4
```

Transparent overlay disabled group includes `Cycle125_ReferenceDioramaShadow`, `Cycle126_CloseShadowBarMute`, `Cycle120_ReferenceLightColumn`, and `ShadowFoundationCycle70_LibraryDiagonalCastA`.

Long-road disabled group includes `RoadToHouseExterior`, `RoadToSouthEastQuarter`, `Cycle62_OuterGroundSkirt_NorthLowStreetContinuationA`, and `ScenicBackdrop_DistantRooflineA`.

## Difference Measurements

Sampled every 4 pixels, comparing each isolation shot against the matching baseline:

```text
WIDE transparent vs baseline: meanAbsRgb=4.922 changedSamplePct=7.049
CLOSE transparent vs baseline: meanAbsRgb=32.594 changedSamplePct=42.351
WIDE longRoad vs baseline: meanAbsRgb=0.137 changedSamplePct=0.295
CLOSE longRoad vs baseline: meanAbsRgb=0.000 changedSamplePct=0.000
WIDE pastLongRoad vs baseline: meanAbsRgb=0.000 changedSamplePct=0.003
CLOSE pastLongRoad vs baseline: meanAbsRgb=0.000 changedSamplePct=0.000
```

## Findings

- The facade close-up symptom is dominated by the transparent overlay stack, not the long-road/road-strip stack.
- Disabling `transparentOverlayOff` visibly removes the white haze/bright veil on the facade and floor in the close shot.
- Disabling `longRoadStackOff` does not materially change the close shot.
- Disabling only past long-road candidates has no visible/pixel-significant effect in the current-time close shot.
- This supports a next D fix candidate focused on the transparent overlay stack's material/depth/render-queue behavior before modifying road geometry.

## Viewer

- Propagate target: `work/chapter1-continuation-map-vs-20260524`.
- R2 upload: `uploaded 10 files for chapter1-continuation-map-vs-20260524/2026-06-09T12-10_renderer_isolation_probe (bucket TTL 45d); manifest now lists 73 paths`.
- Cloudflare deploy hook: `HookId=629936212 Status=200 Length=124`.
- Hook-only polling stayed stale for 30 polls: `256 cycles / 1201 images`, `hasTarget=False`.
- Git-triggered viewer rebuild commit: `ca0e830 chore: refresh review content 2026-06-09 renderer-isolation`.
- Public review page after rebuild: `257 cycles / 1209 images`, album `2026-06-09T12-10_renderer_isolation_probe` present.
- Public album check: `8 images`; all thumbnails loaded with `complete=true`, `naturalWidth=512`, `naturalHeight=288`.

## Next Action

- Propagate this cycle to R2 and `anemora-viewer`.
- After Tom/user review, plan the actual D fix as a narrow transparent-overlay material/render-queue/depth-write pass, with built-player before/after captures.
