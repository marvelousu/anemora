# 2026-06-09 HD2D point15 renderer diagnostics data only

## Context

- Project: Anemora Fast VS House Slice HD-2D recovery.
- Branch: `wip/hd2d-point15-recovery-20260609`.
- Baseline: point15 `e7277f0a`.
- Slice: D flicker/transparency/z-fight diagnostics.
- Acceptance source: built-player only.
- Scope: data only; no flicker/transparency fix in this slice.
- Review folder: `docs/review/2026-06-09T10-08_renderer_diagnostics_data_only/`.

## Probe

- Added `--anemora-house-slice-renderer-diagnostics-dir`.
- Captures three built-player views for facade/long-road inspection.
- Logs renderer material state, render queue, `_Surface`, `_Cull`, `_ZWrite`, `_ZTest`, bounds, XZ/Y overlap pairs, and 120-frame renderer visibility/enabled stats.

## Build Evidence

- Log: `Logs/point15_renderer_diagnostics_build_validate_20260609T095557.log`.
- `Fast VS house slice validation passed.`
- `DisplayProgressNotification: Build Successful`
- `Build Finished, Result: Success.`
- `PlayerBuildInfo duration=85742`

## Built-Player Evidence

- Player log: `Logs/point15_renderer_diagnostics_player_20260609T100410.log`.
- Captures:
  - `01_current_plaza_library_facade.png`
  - `02_past_library_facade_long_road.png`
  - `03_current_library_facade_close.png`

## anemora-viewer Propagation

- R2 upload: `uploaded 6 files for chapter1-continuation-map-vs-20260524/2026-06-09T10-08_renderer_diagnostics_data_only (bucket TTL 45d); manifest now lists 47 paths`.
- Initial public check before propagation was stale: `254 cycles · 1185 images`, `HasRendererDiagnostics=false`.
- Cloudflare deploy hook: `HookId=629936212 Status=200 Length=124`.
- Git-triggered viewer rebuild commit: `9185124 chore: refresh review content 2026-06-09 renderer-diagnostics`.
- Public review page after rebuild: `255 cycles · 1188 images`, album `2026-06-09T10-08_renderer_diagnostics_data_only` present.
- Public album check: 3 images loaded, all `complete=true`, all `naturalWidth=512`, `naturalHeight=288`.
- Public devlog page check: `visibilitySummary` present, `visible=min=518,max=790,mean=520.758,stddev=25.257` present, `overlapSummary candidates=1573 pairs=3984` present.

- 2026-06-09 10:50 JST recheck after user reported stale viewer: public review DOM still returned `255 cycles ﾂｷ 1188 images`; cards present for `2026-06-09T10-08_renderer_diagnostics_data_only`, `2026-06-09T09-39_time_window_back_passthrough_fix`, and `2026-06-09T07-43_table_object_removed_final`.
- Same recheck: relevant public thumbnails loaded in-browser with `complete=true`, `naturalWidth=512`, `naturalHeight=288`. The A lighting/shadow first-pass cycle was not present because that built-player capture had not run yet.

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Summary:

```text
ANEMORA_HOUSE_SLICE_RENDERER_DIAGNOSTICS: materialSummary totalSceneRenderers=11836 interesting=11067 logged=180
ANEMORA_HOUSE_SLICE_RENDERER_DIAGNOSTICS: overlapSummary candidates=1573 pairs=3984 logged=120
ANEMORA_HOUSE_SLICE_RENDERER_DIAGNOSTICS: visibilitySummary tracked=11067 frames=120 visible=min=518,max=790,mean=520.758,stddev=25.257 enabled=min=1574,max=1574,mean=1574.000,stddev=0.000 toggled=305 logged=80 camera="Main Camera"
ANEMORA_HOUSE_SLICE_RENDERER_DIAGNOSTICS: end count=3
```

## Key Findings

- Renderer enable state is stable across the 120-frame sample: `enabled=min=1574,max=1574,mean=1574.000,stddev=0.000`.
- Visibility is not stable: `visible=min=518,max=790,mean=520.758,stddev=25.257`, with `toggled=305`.
- This points away from runtime `renderer.enabled` blinking and toward culling/visibility, camera/frustum, overlay depth, or render-order effects.
- Transparent/ZWrite concern count from logged material samples: 14.
- The long-road-like object is visible in `01_current_plaza_library_facade.png` and `02_past_library_facade_long_road.png` as a long band outside the plaza/library composition.

## Transparent / ZWrite Candidates

```text
Current_CentralPlaza_Cycle126_CloseShadowBarMute_GroundAirUnifierA / Current_CentralPlaza_Cycle125_ReferenceDioramaShadow_ReferenceReceiverLiftA: areaXZ=78.165, queue=3124/3122, _Surface=1, _ZWrite=0, _Cull=0
Current_CentralPlaza_Cycle126_CloseShadowBarMute_GroundAirUnifierA / Current_CentralPlaza_Cycle125_ReferenceDioramaShadow_StoneSunMatteFieldA: areaXZ=62.792, queue=3124/3122, _Surface=1, _ZWrite=0, _Cull=0
Current_CentralPlaza_Cycle125_ReferenceDioramaShadow_StoneSunMatteFieldA / Current_CentralPlaza_Cycle125_ReferenceDioramaShadow_ReferenceReceiverLiftA: areaXZ=62.002, queue=3122/3122, _Surface=1, _ZWrite=0, _Cull=0
Current_CentralPlaza_Cycle126_CloseShadowBarMute_CloseGridWashA / Current_CentralPlaza_Cycle126_CloseShadowBarMute_GroundAirUnifierA: areaXZ=58.924, queue=3122/3124, _Surface=1, _ZWrite=0, _Cull=0
Current_CentralPlaza_Cycle120_ReferenceLightColumn_ShadowPlayerLeftA: queue=3088, tagRenderType=Transparent, _Surface=1, _Cull=0, _ZWrite=0
Past_CentralPlaza_ShadowFoundationCycle70_LibraryDiagonalCastA: queue=3007, tagRenderType=Transparent, _Surface=1, _Cull=0, _ZWrite=0
Past_CentralPlaza_ScenicBackdrop_DistantRooflineA: queue=2994, tagRenderType=Transparent, _Surface=1, _Cull=0, _ZWrite=0
```

## Long Road / Opaque Overlap Candidates

```text
Current_CentralPlaza_Cycle62_OuterGroundSkirt_NorthLowStreetContinuationA / Past_CentralPlaza_Cycle62_OuterGroundSkirt_NorthLowStreetContinuationA: areaXZ=28.014, centerYDelta=0.000, both opaque queue=2000
Past_CentralPlaza_RoadToHouseExterior / Current_CentralPlaza_RoadToHouseExterior: areaXZ=18.301, centerYDelta=0.000, both opaque queue=2000
Past_CentralPlaza_RoadToSouthEastQuarter / Current_CentralPlaza_RoadToSouthEastQuarter: areaXZ=17.260, centerYDelta=0.000, both opaque queue=2000
Current_CentralPlaza_PixelGround / Current_CentralPlaza_RoadToHouseExterior: areaXZ=18.301, centerYDelta=0.060, both opaque queue=2000
Current_CentralPlaza_PixelGround / Current_CentralPlaza_RoadToSouthEastQuarter: areaXZ=17.260, centerYDelta=0.060, both opaque queue=2000
```

## Reference Notes

- Unity render queues: Geometry is 2000, AlphaTest is 2450, Transparent is 3000; Transparent is for alpha-blended shaders that do not write depth.
- Unity sorting: render queues at 2501+ use transparent sorting behavior, unlike opaque queues.
- Unity 2D sorting notes: transparent renderers are ordered by sorting layer/order, render queue, distance to camera, sorting group, material/shader, then tiebreaker.
- `Renderer.isVisible` is true when a renderer needs to be rendered by any camera, including cases such as shadows, so it is a visibility/culling signal rather than a direct proof of on-screen pixels.

## Next Action

- Do not fix in this slice.
- Next diagnosis/fix candidate: isolate the large transparent ground/light overlay stack (`Cycle125/126/120`), then separately isolate current/past duplicate road/ground strips that occupy identical XZ with identical Y.
- Any fix must be validated by built-player capture plus renderer diagnostics rerun, then propagated to anemora-viewer.
