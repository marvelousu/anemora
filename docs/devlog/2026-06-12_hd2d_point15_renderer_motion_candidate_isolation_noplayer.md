# 2026-06-12 HD2D point15 renderer motion candidate isolation without player

## Scope

- Continue the point15 flicker / transparent-outline diagnostic line after the OutdoorVoidBackground pose-isolation probe.
- Re-test the front-road / library transparent / library rear candidate groups with dynamic character renderers hidden, because the previous `20-49` capture was contaminated by player animation and visibility changes.
- Change diagnostic probe code only. Do not change scene content, materials, lighting, gameplay, or renderer settings in this slice.
- Acceptance source remains built-player evidence only.

## Code change

- `Assets/Scripts/FastVS/FastVsHouseRuntimeSmokeProbe.cs`
  - Added `motionDynamicCharactersHidden` isolation at the beginning of `RunRendererMotionProbe()`.
  - Hidden renderer matches include the player paper sprite, player contact / bounce / foot / directional cast shadows, player reference light/shadow helper objects, and `FastVS_SpriteCharacter_*` NPC sprites / contact shadows.
  - Added candidate isolation variants:
    - `frontRoadLongThinGroundOff`
    - `libraryTransparentDepthPlanesOff`
    - `libraryRearThinOpaquesOff`

## Invalid attempt discarded

- `docs\review\2026-06-12T20-49_renderer_motion_candidate_isolation`
  - Diagnostic only; not propagated.
  - Discard reason: baseline and isolation frames included player animation / player visibility differences, so baseline-to-variant deltas were not object-only measurements.

## Build evidence

- Build log:
  - `Logs\point15_renderer_motion_candidate_isolation_noplayer_build_validate_20260612T205317.log`
- Build exit:
  - `UNITY_EXIT=0`
- Build key lines:
  - `DisplayProgressNotification: Build Successful`
  - `Build Finished, Result: Success.`
  - `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Built player timestamp:
  - `2026-06-12 20:54:43`

## Built-player evidence

- Player log:
  - `Logs\point15_renderer_motion_candidate_isolation_noplayer_player_20260612T205529.log`
- Review folder:
  - `docs\review\2026-06-12T20-55_renderer_motion_candidate_isolation_noplayer`
- Player exit:
  - `PLAYER_EXIT=0`
- Captures:
  - 61 PNGs
  - 13 baseline motion frames
  - 8 OutdoorVoidBackground east wash frames
  - 8 OutdoorVoidBackground north silhouette frames
  - 8 OutdoorVoidBackground all-current-plaza frames
  - 8 front-road long-thin-ground frames
  - 8 library transparent-depth-plane frames
  - 8 library rear thin-opaque frames

## Renderer contract

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

## Dynamic character hide guard

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=motionDynamicCharactersHidden matched=28 disabled=27
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=motionDynamicCharactersHidden restored=27
```

Visual inspection:

- `docs\review\2026-06-12T20-55_renderer_motion_candidate_isolation_noplayer\06_motion_frame_090.png`
- The player is not visible in the baseline frame. This run is valid as an environment / object-only diagnostic.

## Motion measurement

Capture pass:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: summary phase=motionFollowCapture frames=180 saved=13 tracked=11063 visible=min=344,max=527,mean=456.278,stddev=58.344 enabled=min=1555,max=1555,mean=1555.000,stddev=0.000 backgroundVisible=min=1,max=3,mean=2.400,stddev=0.611 visibleHashChanges=126 backgroundHashChanges=6 imageMeanAbsRgb=min=14.227,max=34.015,mean=23.979,stddev=6.749 imageChangedSamplePct=min=53.292,max=81.361,mean=74.131,stddev=8.349 deltaTime=min=0.014,max=0.333,mean=0.027,stddev=0.038 unscaledDeltaTime=min=0.014,max=0.813,mean=0.030,stddev=0.069
```

RenderNoSave runtime pass:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: runtimeSummary phase=motionFollowRenderNoSave frames=180 tracked=11063 visible=min=344,max=545,mean=455.506,stddev=58.184 enabled=min=1555,max=1555,mean=1555.000,stddev=0.000 backgroundVisible=min=1,max=3,mean=2.372,stddev=0.615 visibleHashChanges=129 backgroundHashChanges=2 deltaTime=min=0.014,max=0.333,mean=0.020,stddev=0.024 unscaledDeltaTime=min=0.014,max=4.926,mean=0.046,stddev=0.365
```

## Candidate groups

`frontRoadLongThinGroundOff`:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=frontRoadLongThinGroundOff matched=4 disabled=4 logged=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_EdgeDressing_EastLowWall | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Cycle43_OuterEastShelfA | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorWorldEnvelope_RightPerimeterShoulderB | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Cycle62_OuterGroundSkirt_EastStreetContinuationA]
```

`libraryTransparentDepthPlanesOff`:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=libraryTransparentDepthPlanesOff matched=3 disabled=3 logged=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Cycle59_LibraryRoofUndersideShadow_EastSideWallCoolFalloffA | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_FramedLightPlanes_LibraryFacadeOcclusionGradientA | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Cycle59_LibraryRoofUndersideShadow_WestSideWallCoolFalloffA]
```

`libraryRearThinOpaquesOff`:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=libraryRearThinOpaquesOff matched=10 disabled=10 logged=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_WestMidStoneCourseB | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_WestMidStoneCourseA | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_ArchitectureSurfaceDepth_EntranceUpperBrowA | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_LibrarySideSurfaceBreakup_RearWallBandA | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_EastMidStoneCourseB | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_LibraryFrontDepth_UnderEaveDepthLineA | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_Cycle60_LibrarySideWallMaterialBreakup_EastMidStoneCourseA | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_LibraryRoofSideDepth_RearWallBandA | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_LibraryFacadeDetail_RoofUnderThinBand | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_LibraryEntryDepth_RoofLipUndersideShadowA]
```

## Isolation result

Full measured variant ranges:

- `outdoorVoidBackgroundEastEdgeWashOff`: meanAbsRgb `0.000` to `0.007`, changedSamplePct `0.000` to `0.076`, maxFrame `120`, maxChanged `11`.
- `outdoorVoidBackgroundNorthSilhouettesOff`: meanAbsRgb `0.000` to `0.007`, changedSamplePct `0.000` to `0.076`, maxFrame `120`, maxChanged `11`.
- `outdoorVoidBackgroundAllCurrentCentralPlazaOff`: meanAbsRgb `0.000` to `0.007`, changedSamplePct `0.000` to `0.076`, maxFrame `120`, maxChanged `11`.
- `frontRoadLongThinGroundOff`: meanAbsRgb `0.000` to `0.007`, changedSamplePct `0.000` to `0.076`, maxFrame `120`, maxChanged `11`.
- `libraryTransparentDepthPlanesOff`: meanAbsRgb `0.000` to `0.007`, changedSamplePct `0.000` to `0.076`, maxFrame `120`, maxChanged `11`.
- `libraryRearThinOpaquesOff`: meanAbsRgb `0.039` to `0.275`, changedSamplePct `0.139` to `1.090`, maxFrame `179`, maxChanged `138`.

Representative `libraryRearThinOpaquesOff` lines:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=libraryRearThinOpaquesOff frame=90 saved=55_libraryRearThinOpaquesOff_frame_090.png disabled=10 baselineDelta meanAbsRgb=0.078 changedSamplePct=0.438 changed=63 samples=14400 playerLocal=(21.125,0.020,17.321) cameraPos=(21.125,2.800,12.658)
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=libraryRearThinOpaquesOff frame=150 saved=58_libraryRearThinOpaquesOff_frame_150.png disabled=10 baselineDelta meanAbsRgb=0.181 changedSamplePct=1.090 changed=157 samples=14400 playerLocal=(21.020,0.020,21.363) cameraPos=(21.020,2.812,16.813)
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=libraryRearThinOpaquesOff frame=179 saved=60_libraryRearThinOpaquesOff_frame_179.png disabled=10 baselineDelta meanAbsRgb=0.275 changedSamplePct=0.958 changed=138 samples=14400 playerLocal=(20.950,0.020,22.050) cameraPos=(20.950,2.810,17.500)
```

## Findings

- Once player / character renderers are hidden, the previous broad candidate groups collapse to near-zero deltas.
- The user-suspected long-road object group and the transparent library depth-plane group are not supported as the main visual culprit in this probe: both measured `meanAbsRgb=0.000..0.007`.
- The only measured group with a remaining object-only contribution is `libraryRearThinOpaquesOff`, at `meanAbsRgb=0.039..0.275` and `changedSamplePct=0.139..1.090`.
- The visible-set churn still exists: capture pass `visibleHashChanges=126`, runtime pass `visibleHashChanges=129`, while enabled state remains stable at `enabled=min=1555,max=1555,mean=1555.000,stddev=0.000`.
- This points away from runtime enable/disable flicker and toward camera-frustum / visibility-state popping of thin library-rear opaque detail geometry.

## Next action

- Propagate `docs\review\2026-06-12T20-55_renderer_motion_candidate_isolation_noplayer` to anemora-viewer.
- Next probe should split the 10 `libraryRearThinOpaquesOff` objects one-by-one, still with dynamic characters hidden, and preserve exact baseline poses.
- Do not apply a visual fix yet. The next slice should identify the exact object(s) inside the 10-renderer group before deleting, disabling, moving, or material-changing anything.

## Viewer

- Propagate target:
  - `work/chapter1-continuation-map-vs-20260524`
- R2 upload:
  - First attempt used a Windows backslash branch argument and produced non-canonical slug `work-chapter1-continuation-map-vs-20260524`; corrected immediately.
  - Correct upload: `uploaded 63 files for chapter1-continuation-map-vs-20260524/2026-06-12T20-55_renderer_motion_candidate_isolation_noplayer (bucket TTL 45d); manifest now lists 463 paths`
- Local viewer build:
  - `npm run build`
  - Result: timed out after `604035` ms, but generated the target `dist` album and devlog before timeout.
  - Local generated album: `dist\chapter1-continuation-map-vs-20260524\gallery\docs\review\2026-06-12T20-55_renderer_motion_candidate_isolation_noplayer\index.html` length `94724`
  - Local generated devlog: `dist\chapter1-continuation-map-vs-20260524\docs\docs\devlog\2026-06-12_hd2d_point15_renderer_motion_candidate_isolation_noplayer\index.html` length `1029788`
  - Local generated original PNG: `dist\originals\chapter1-continuation-map-vs-20260524\docs\review\2026-06-12T20-55_renderer_motion_candidate_isolation_noplayer\06_motion_frame_090.png` length `815071`
  - Local generated thumb WEBP: `dist\thumbs\chapter1-continuation-map-vs-20260524\docs\review\2026-06-12T20-55_renderer_motion_candidate_isolation_noplayer\55_libraryRearThinOpaquesOff_frame_090.webp` length `13088`
- anemora-viewer commit:
  - `e5c9c7d chore: refresh motion candidate review`
- Public verification:
  - Album URL: `https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/gallery/docs/review/2026-06-12T20-55_renderer_motion_candidate_isolation_noplayer/`
  - Devlog URL: `https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/docs/docs/devlog/2026-06-12_hd2d_point15_renderer_motion_candidate_isolation_noplayer/`
  - Original PNG URL: `https://anemora-viewer.pages.dev/originals/chapter1-continuation-map-vs-20260524/docs/review/2026-06-12T20-55_renderer_motion_candidate_isolation_noplayer/06_motion_frame_090.png`
  - Thumb WEBP URL: `https://anemora-viewer.pages.dev/thumbs/chapter1-continuation-map-vs-20260524/docs/review/2026-06-12T20-55_renderer_motion_candidate_isolation_noplayer/55_libraryRearThinOpaquesOff_frame_090.webp`
  - First public reflection: attempt `4`
  - Album: `200`, contained `61 images` and `55_libraryRearThinOpaquesOff_frame_090`
  - Devlog: `200`, contained `motion candidate isolation without player`, `meanAbsRgb`, and `0.275`
  - Original PNG: `200`, length `815071`
  - Thumb WEBP: `200`, length `13088`
