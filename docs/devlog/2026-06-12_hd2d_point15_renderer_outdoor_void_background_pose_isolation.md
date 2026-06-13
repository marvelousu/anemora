# 2026-06-12 HD2D point15 OutdoorVoidBackground pose isolation

## Scope

- Continue the point15 flicker / object-pop diagnostic line after the RenderNoSave motion probe.
- Test whether `Current_CentralPlaza_OutdoorVoidBackground_*` is the user-visible popping / transparent-outline culprit under a moving camera.
- Change diagnostic probe code only. Do not change scene content, materials, lighting, gameplay, or renderer settings in this slice.
- Acceptance source remains built-player evidence only.

## Code change

- `Assets/Scripts/FastVS/FastVsHouseRuntimeSmokeProbe.cs`
  - Added `LogRendererMotionBackgroundDiagnosticTargets()` to log every current central plaza `OutdoorVoidBackground_*` renderer with active/enabled state, transform, bounds, shader, render queue, `_Surface`, `_Cull`, `_ZWrite`, `_ZTest`, blend state, and color.
  - Added pose-reused isolation captures for:
    - `outdoorVoidBackgroundEastEdgeWashOff`
    - `outdoorVoidBackgroundNorthSilhouettesOff`
    - `outdoorVoidBackgroundAllCurrentCentralPlazaOff`
  - The probe now stores baseline player/camera poses for selected motion frames and reuses those exact poses before each isolation capture.

## Invalid attempts discarded

- `docs\review\2026-06-12T19-24_renderer_outdoor_void_background_isolation`
  - Discarded because the isolation variant skipped the same-frame yield/camera settle path, so baseline-to-variant camera pose was not equivalent.
- `docs\review\2026-06-12T19-41_renderer_outdoor_void_background_isolation`
  - Discarded because the variant path sampled selected frames only and still did not reproduce the same smoothed camera state as the baseline sequence.
- The invalid review folders were deleted and must not be propagated.
- The remaining logs are diagnostic only:
  - `Logs\point15_renderer_outdoor_void_background_isolation_player_20260612T192412.log`
  - `Logs\point15_renderer_outdoor_void_background_isolation_player_20260612T194154.log`

## Build evidence

- Failed intermediate build:
  - `Logs\point15_renderer_outdoor_void_isolation_yield_build_validate_20260612T192527.log`
  - Cause: `CS1623: Iterators cannot have ref, in or out parameters`
- Final build log:
  - `Logs\point15_renderer_outdoor_void_isolation_pose_build_validate_20260612T194328.log`
- Final build exit:
  - `0`
- Final build key lines:
  - `DisplayProgressNotification: Build Successful`
  - `Build Finished, Result: Success.`
  - `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Built-player evidence

- Player log:
  - `Logs\point15_renderer_outdoor_void_background_pose_isolation_player_20260612T194919.log`
- Review folder:
  - `docs\review\2026-06-12T19-49_renderer_outdoor_void_background_pose_isolation`
- Captures:
  - 37 PNGs
  - 13 baseline motion frames
  - 8 `outdoorVoidBackgroundEastEdgeWashOff` frames
  - 8 `outdoorVoidBackgroundNorthSilhouettesOff` frames
  - 8 `outdoorVoidBackgroundAllCurrentCentralPlazaOff` frames

## Renderer contract

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

## OutdoorVoidBackground diagnostics

- Matched current central plaza `OutdoorVoidBackground_*` renderers:
  - 5
- Material:
  - `FastVS_House_current_central_plaza_outdoor_void_background`
  - shader: `Universal Render Pipeline/Unlit`
  - queue: `2991`
  - `tagRenderType=Transparent`
  - `_Surface=1`
  - `_Cull=0`
  - `_ZWrite=0`
  - `_ZTest=<missing>`
  - `_SrcBlend=5`
  - `_DstBlend=10`
  - `_Color=(0.1,0.11,0.1,0.038)`
  - `_BaseColor=(0.1,0.11,0.1,0.038)`
- Renderer states:
  - `Current_CentralPlaza_OutdoorVoidBackground_WestEdgeWash`: `active=True enabled=True`
  - `Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteCenter`: `active=False enabled=True`
  - `Current_CentralPlaza_OutdoorVoidBackground_EastEdgeWash`: `active=True enabled=True`
  - `Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteLeft`: `active=True enabled=True`
  - `Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteRight`: `active=True enabled=True`

## Motion measurement

Capture pass:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: summary phase=motionFollowCapture frames=180 saved=13 tracked=11063 visible=min=349,max=534,mean=462.789,stddev=58.640 enabled=min=1562,max=1562,mean=1562.000,stddev=0.000 backgroundVisible=min=1,max=3,mean=2.389,stddev=0.609 visibleHashChanges=126 backgroundHashChanges=4 imageMeanAbsRgb=min=13.360,max=33.430,mean=23.872,stddev=6.623 imageChangedSamplePct=min=51.285,max=81.535,mean=73.897,stddev=8.525 deltaTime=min=0.013,max=0.333,mean=0.025,stddev=0.038 unscaledDeltaTime=min=0.013,max=0.531,mean=0.027,stddev=0.055
```

RenderNoSave runtime pass:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: runtimeSummary phase=motionFollowRenderNoSave frames=180 tracked=11063 visible=min=349,max=554,mean=462.128,stddev=58.401 enabled=min=1562,max=1562,mean=1562.000,stddev=0.000 backgroundVisible=min=1,max=3,mean=2.372,stddev=0.615 visibleHashChanges=126 backgroundHashChanges=2 deltaTime=min=0.013,max=0.333,mean=0.018,stddev=0.024 unscaledDeltaTime=min=0.013,max=2.322,mean=0.029,stddev=0.171
```

Background visible-set transitions:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: backgroundVisibleDelta frame=80 previous=5569A502 current=A18DF36A currentSample=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteLeft | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteRight]
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: backgroundVisibleDelta frame=91 previous=A18DF36A current=6C6F13A6 currentSample=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_EastEdgeWash | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteLeft | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteRight]
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: backgroundVisibleDelta frame=93 previous=6C6F13A6 current=A18DF36A currentSample=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteLeft | FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteRight]
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: backgroundVisibleDelta frame=168 previous=A18DF36A current=DAA5E1A7 currentSample=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_OutdoorVoidBackground_NorthSilhouetteRight]
```

## Isolation result

`outdoorVoidBackgroundEastEdgeWashOff`:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=outdoorVoidBackgroundEastEdgeWashOff frame=0 saved=13_outdoorVoidBackgroundEastEdgeWashOff_frame_000.png disabled=1 baselineDelta meanAbsRgb=0.537 changedSamplePct=1.549 changed=223 samples=14400 playerLocal=(18.100,0.020,13.700) cameraPos=(18.100,2.957,9.150)
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=outdoorVoidBackgroundEastEdgeWashOff frame=90 saved=15_outdoorVoidBackgroundEastEdgeWashOff_frame_090.png disabled=1 baselineDelta meanAbsRgb=1.041 changedSamplePct=2.632 changed=379 samples=14400 playerLocal=(21.125,0.020,17.321) cameraPos=(21.125,2.794,12.681)
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=outdoorVoidBackgroundEastEdgeWashOff frame=179 saved=20_outdoorVoidBackgroundEastEdgeWashOff_frame_179.png disabled=1 baselineDelta meanAbsRgb=0.596 changedSamplePct=1.611 changed=232 samples=14400 playerLocal=(20.950,0.020,22.050) cameraPos=(20.950,2.795,17.500)
```

`outdoorVoidBackgroundNorthSilhouettesOff`:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=outdoorVoidBackgroundNorthSilhouettesOff frame=0 saved=21_outdoorVoidBackgroundNorthSilhouettesOff_frame_000.png disabled=3 baselineDelta meanAbsRgb=0.537 changedSamplePct=1.549 changed=223 samples=14400 playerLocal=(18.100,0.020,13.700) cameraPos=(18.100,2.957,9.150)
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=outdoorVoidBackgroundNorthSilhouettesOff frame=90 saved=23_outdoorVoidBackgroundNorthSilhouettesOff_frame_090.png disabled=3 baselineDelta meanAbsRgb=1.041 changedSamplePct=2.632 changed=379 samples=14400 playerLocal=(21.125,0.020,17.321) cameraPos=(21.125,2.794,12.681)
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=outdoorVoidBackgroundNorthSilhouettesOff frame=179 saved=28_outdoorVoidBackgroundNorthSilhouettesOff_frame_179.png disabled=3 baselineDelta meanAbsRgb=0.596 changedSamplePct=1.611 changed=232 samples=14400 playerLocal=(20.950,0.020,22.050) cameraPos=(20.950,2.795,17.500)
```

`outdoorVoidBackgroundAllCurrentCentralPlazaOff`:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=outdoorVoidBackgroundAllCurrentCentralPlazaOff frame=0 saved=29_outdoorVoidBackgroundAllCurrentCentralPlazaOff_frame_000.png disabled=5 baselineDelta meanAbsRgb=0.537 changedSamplePct=1.549 changed=223 samples=14400 playerLocal=(18.100,0.020,13.700) cameraPos=(18.100,2.957,9.150)
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=outdoorVoidBackgroundAllCurrentCentralPlazaOff frame=90 saved=31_outdoorVoidBackgroundAllCurrentCentralPlazaOff_frame_090.png disabled=5 baselineDelta meanAbsRgb=1.041 changedSamplePct=2.632 changed=379 samples=14400 playerLocal=(21.125,0.020,17.321) cameraPos=(21.125,2.794,12.681)
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=outdoorVoidBackgroundAllCurrentCentralPlazaOff frame=179 saved=36_outdoorVoidBackgroundAllCurrentCentralPlazaOff_frame_179.png disabled=5 baselineDelta meanAbsRgb=0.596 changedSamplePct=1.611 changed=232 samples=14400 playerLocal=(20.950,0.020,22.050) cameraPos=(20.950,2.795,17.500)
```

Full measured variant ranges:

- `outdoorVoidBackgroundEastEdgeWashOff`: meanAbsRgb `0.463` to `1.041`, changedSamplePct `1.229` to `2.632`.
- `outdoorVoidBackgroundNorthSilhouettesOff`: meanAbsRgb `0.463` to `1.041`, changedSamplePct `1.229` to `2.632`.
- `outdoorVoidBackgroundAllCurrentCentralPlazaOff`: meanAbsRgb `0.463` to `1.041`, changedSamplePct `1.229` to `2.632`.

## Findings

- `OutdoorVoidBackground_*` does enter and leave the camera-visible set during motion.
- Runtime enabled state is stable: `enabled=min=1562,max=1562,mean=1562.000,stddev=0.000`.
- The strongest `OutdoorVoidBackground_EastEdgeWash` toggle is short-lived: `visibleToggles=2`, `visibleFrames=2/180`, `enabledToggles=0`.
- With exact pose reuse, disabling `EastEdgeWash`, all north silhouettes, or all current central plaza `OutdoorVoidBackground_*` produces nearly identical small deltas.
- Therefore `OutdoorVoidBackground_*` is a measured background-visible-set participant, but this slice does not support it as the main user-visible popping / transparent-outline culprit.
- The next diagnostic should move from this low-alpha void-background material to other motion-toggling objects around the library/front-road silhouette, especially opaque thin geometry, floor/decal slivers, and any transparent horizon / depth planes with larger visual contribution.

## Next action

- Propagate `docs\review\2026-06-12T19-49_renderer_outdoor_void_background_pose_isolation` to anemora-viewer.
- Keep this as data only; do not delete or disable `OutdoorVoidBackground_*` based on this result.
- Next probe should sort motion toggles by actual image delta, not only by `Renderer.isVisible`, and should target the front-library long-road / outline-pass-through complaint.

## Viewer

- Propagate target:
  - `work/chapter1-continuation-map-vs-20260524`
- R2 upload:
  - `uploaded 39 files for chapter1-continuation-map-vs-20260524/2026-06-12T19-49_renderer_outdoor_void_background_pose_isolation (bucket TTL 45d); manifest now lists 400 paths`
- anemora-viewer commit:
  - `036c859 chore: refresh outdoor void review`
- Local viewer build:
  - `npm run build`
  - Exit: `0`
  - R2 fetch line: `[setup-r2-images] chapter1-continuation-map-vs-20260524: fetched 396/400 files`
- Local dist verification:
  - Album: `dist\chapter1-continuation-map-vs-20260524\gallery\docs\review\2026-06-12T19-49_renderer_outdoor_void_background_pose_isolation\index.html` length `63347`
  - Devlog: `dist\chapter1-continuation-map-vs-20260524\docs\docs\devlog\2026-06-12_hd2d_point15_renderer_outdoor_void_background_pose_isolation\index.html` length `1030272`
  - Original PNG: `dist\originals\chapter1-continuation-map-vs-20260524\docs\review\2026-06-12T19-49_renderer_outdoor_void_background_pose_isolation\06_motion_frame_090.png` length `826215`
  - Thumb WEBP: `dist\thumbs\chapter1-continuation-map-vs-20260524\docs\review\2026-06-12T19-49_renderer_outdoor_void_background_pose_isolation\31_outdoorVoidBackgroundAllCurrentCentralPlazaOff_frame_090.webp` length `13208`
- Public verification:
  - Album URL: `https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/gallery/docs/review/2026-06-12T19-49_renderer_outdoor_void_background_pose_isolation/`
  - Devlog URL: `https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/docs/docs/devlog/2026-06-12_hd2d_point15_renderer_outdoor_void_background_pose_isolation/`
  - Original PNG URL: `https://anemora-viewer.pages.dev/originals/chapter1-continuation-map-vs-20260524/docs/review/2026-06-12T19-49_renderer_outdoor_void_background_pose_isolation/06_motion_frame_090.png`
  - Thumb WEBP URL: `https://anemora-viewer.pages.dev/thumbs/chapter1-continuation-map-vs-20260524/docs/review/2026-06-12T19-49_renderer_outdoor_void_background_pose_isolation/31_outdoorVoidBackgroundAllCurrentCentralPlazaOff_frame_090.webp`
  - First public album/image reflection: attempt `23`
  - Album: `200`, contained `37 images` and `31_outdoorVoidBackgroundAllCurrentCentralPlazaOff_frame_090`
  - Devlog: `200`, length `1030366`, contained `OutdoorVoidBackground pose isolation`, `meanAbsRgb`, `0.463`, `main user-visible popping`, and `Full measured variant ranges`
  - Original PNG: `200`, `image/png`, length `826215`
  - Thumb WEBP: `200`, `image/webp`, length `13208`
