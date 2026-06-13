# 2026-06-12 HD2D point15 renderer library rear upper brow removal

## Scope

- Continue the point15 flicker / transparent-outline diagnostic line after `libraryRearThinOpaquesOff` was identified as the only high-delta object group in the dynamic-character-hidden built-player probe.
- Split the 10 `libraryRearThinOpaquesOff` renderers into individual isolation variants.
- Remove only the dominant current-time object confirmed by built-player measurement:
  - `Current_CentralPlaza_ArchitectureSurfaceDepth_EntranceUpperBrowA`
- Keep the renderer contract unchanged:
  - `RenderingMode=2`
  - `DepthPrimingMode=0`
  - `CopyDepthMode=0`
  - `PortalStencilFeatureActive=True`
- Acceptance source remains built-player evidence only.

## Code change

- `Assets/Scripts/FastVS/FastVsHouseRuntimeSmokeProbe.cs`
  - Added exact-name isolation support for each `libraryRearThinOpaquesOff` child candidate.
  - Kept the dynamic-character hide guard active for object-only measurements.
- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Stopped generating current-time `Current_CentralPlaza_ArchitectureSurfaceDepth_EntranceUpperBrowA`.
  - Removed its current-time validation row.
  - Kept past-time `Past_CentralPlaza_ArchitectureSurfaceDepth_EntranceUpperBrowA` unchanged.

## Individual isolation build evidence

- Build log:
  - `Logs\point15_renderer_library_rear_individual_isolation_build_validate_20260612T214550.log`
- Build result:
  - `UNITY_EXIT=0`
- Built-player capture:
  - Player log: `Logs\point15_renderer_library_rear_individual_isolation_player_20260612T214923.log`
  - Review folder: `docs\review\2026-06-12T21-49_renderer_library_rear_individual_isolation`
  - Captures: `141` PNGs
  - Player result: `PLAYER_EXIT=0`

## Individual isolation renderer contract

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

## Individual isolation motion measurement

Capture pass:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: summary phase=motionFollowCapture frames=180 saved=13 tracked=11063 visible=min=344,max=527,mean=455.983,stddev=58.220 enabled=min=1555,max=1555,mean=1555.000,stddev=0.000 backgroundVisible=min=1,max=3,mean=2.389,stddev=0.609 visibleHashChanges=125 backgroundHashChanges=4 imageMeanAbsRgb=min=14.345,max=33.738,mean=24.036,stddev=6.737 imageChangedSamplePct=min=52.222,max=81.653,mean=74.028,stddev=8.538 deltaTime=min=0.013,max=0.333,mean=0.025,stddev=0.037 unscaledDeltaTime=min=0.013,max=0.701,mean=0.028,stddev=0.064
```

RenderNoSave runtime pass:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: runtimeSummary phase=motionFollowRenderNoSave frames=180 tracked=11063 visible=min=344,max=547,mean=455.394,stddev=57.941 enabled=min=1555,max=1555,mean=1555.000,stddev=0.000 backgroundVisible=min=1,max=3,mean=2.372,stddev=0.615 visibleHashChanges=124 backgroundHashChanges=2 deltaTime=min=0.014,max=0.333,mean=0.019,stddev=0.024 unscaledDeltaTime=min=0.014,max=11.120,mean=0.079,stddev=0.825
```

Dynamic-character guard:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=motionDynamicCharactersHidden matched=28 disabled=27
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=motionDynamicCharactersHidden restored=27
```

## Individual isolation result

The broad group still measured high before removal:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=libraryRearThinOpaquesOff matched=10 disabled=10
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=libraryRearThinOpaquesOff frame=150 saved=58_libraryRearThinOpaquesOff_frame_150.png disabled=10 baselineDelta meanAbsRgb=0.250 changedSamplePct=1.104 changed=159 samples=14400 playerLocal=(21.020,0.020,21.363) cameraPos=(21.020,2.816,16.813)
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=libraryRearThinOpaquesOff frame=165 saved=59_libraryRearThinOpaquesOff_frame_165.png disabled=10 baselineDelta meanAbsRgb=0.274 changedSamplePct=0.694 changed=100 samples=14400 playerLocal=(20.967,0.020,21.880) cameraPos=(20.967,2.794,17.330)
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=libraryRearThinOpaquesOff frame=179 saved=60_libraryRearThinOpaquesOff_frame_179.png disabled=10 baselineDelta meanAbsRgb=0.236 changedSamplePct=0.701 changed=101 samples=14400 playerLocal=(20.950,0.020,22.050) cameraPos=(20.950,2.799,17.500)
```

`Current_CentralPlaza_ArchitectureSurfaceDepth_EntranceUpperBrowA` was the dominant individual candidate:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=libraryRear_ArchitectureUpperBrowOff matched=1 disabled=1 logged=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_ArchitectureSurfaceDepth_EntranceUpperBrowA]
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=libraryRear_ArchitectureUpperBrowOff frame=150 saved=138_libraryRear_ArchitectureUpperBrowOff_frame_150.png disabled=1 baselineDelta meanAbsRgb=0.201 changedSamplePct=0.667 changed=96 samples=14400 playerLocal=(21.020,0.020,21.363) cameraPos=(21.020,2.816,16.813)
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=libraryRear_ArchitectureUpperBrowOff frame=165 saved=139_libraryRear_ArchitectureUpperBrowOff_frame_165.png disabled=1 baselineDelta meanAbsRgb=0.274 changedSamplePct=0.694 changed=100 samples=14400 playerLocal=(20.967,0.020,21.880) cameraPos=(20.967,2.794,17.330)
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=libraryRear_ArchitectureUpperBrowOff frame=179 saved=140_libraryRear_ArchitectureUpperBrowOff_frame_179.png disabled=1 baselineDelta meanAbsRgb=0.236 changedSamplePct=0.701 changed=101 samples=14400 playerLocal=(20.950,0.020,22.050) cameraPos=(20.950,2.799,17.500)
```

Secondary candidates were much smaller:

- `libraryRear_RoofSideRearWallBandOff`: measured `meanAbsRgb=0.000..0.037`, `changedSamplePct=0.000..0.354`.
- `libraryRear_SideSurfaceRearWallBandOff`: measured `meanAbsRgb=0.000..0.032`, `changedSamplePct=0.000..0.278`.
- The Cycle60 mid-stone courses, entry roof lip, front under-eave depth line, and facade roof thin band measured near-zero in this run.

## Invalid removal attempt discarded

- Build log:
  - `Logs\point15_renderer_architecture_upper_brow_removed_build_validate_20260612T215245.log`
- Player log:
  - `Logs\point15_renderer_architecture_upper_brow_removed_player_20260612T215516.log`
- Review folder:
  - `docs\review\2026-06-12T21-55_renderer_architecture_upper_brow_removed`
- Discard reason:
  - This attempt used `BuildHouseSlicePlayer` after a generator change.
  - `BuildHouseSlicePlayer` only calls `CreateHouseSliceScene()` when the scene file is missing, so it built the old existing scene.
  - Built-player evidence proved the object still existed:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=libraryRearThinOpaquesOff matched=10 disabled=10
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=libraryRear_ArchitectureUpperBrowOff matched=1 disabled=1 logged=[FastVS_Current_NiroHouseInteriorExterior/Current_CentralPlazaMap_SeparateSpace/Current_CentralPlaza_ArchitectureSurfaceDepth_EntranceUpperBrowA]
```

This review folder is diagnostic only and must not be propagated as accepted evidence.

## Correct removal build evidence

- Correct batch method:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Build log:
  - `Logs\point15_renderer_architecture_upper_brow_removed_buildandvalidate_20260612T215724.log`
- Build result:
  - `UNITY_EXIT=0`
- Validation:

```text
Fast VS house slice validation passed.
DisplayProgressNotification: Build Successful
Build Finished, Result: Success.
Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe
```

- Built-player executable:
  - `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
  - LastWriteTime: `6/12/2026 10:02:42 PM`
  - Length: `667648`

## Correct removal built-player evidence

- Player log:
  - `Logs\point15_renderer_architecture_upper_brow_removed_regen_player_20260612T220331.log`
- Review folder:
  - `docs\review\2026-06-12T22-03_renderer_architecture_upper_brow_removed_regen`
- Captures:
  - `141` PNGs
- Player result:
  - `PLAYER_EXIT=0`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Dynamic-character guard:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=motionDynamicCharactersHidden matched=28 disabled=27
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=motionDynamicCharactersHidden restored=27
```

Motion capture pass:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: summary phase=motionFollowCapture frames=180 saved=13 tracked=11062 visible=min=343,max=526,mean=455.256,stddev=58.416 enabled=min=1554,max=1554,mean=1554.000,stddev=0.000 backgroundVisible=min=1,max=3,mean=2.389,stddev=0.609 visibleHashChanges=125 backgroundHashChanges=4 imageMeanAbsRgb=min=13.515,max=33.591,mean=23.965,stddev=6.703 imageChangedSamplePct=min=49.458,max=81.389,mean=73.948,stddev=8.940 deltaTime=min=0.013,max=0.333,mean=0.025,stddev=0.037 unscaledDeltaTime=min=0.013,max=0.738,mean=0.028,stddev=0.065
```

RenderNoSave runtime pass:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: runtimeSummary phase=motionFollowRenderNoSave frames=180 tracked=11062 visible=min=343,max=546,mean=454.644,stddev=58.421 enabled=min=1554,max=1554,mean=1554.000,stddev=0.000 backgroundVisible=min=1,max=3,mean=2.372,stddev=0.615 visibleHashChanges=125 backgroundHashChanges=2 deltaTime=min=0.013,max=0.333,mean=0.019,stddev=0.024 unscaledDeltaTime=min=0.013,max=10.923,mean=0.078,stddev=0.811
```

## Removal proof

The broad group dropped from 10 matched renderers to 9:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=libraryRearThinOpaquesOff matched=9 disabled=9
```

The exact removed target dropped from 1 matched renderer to 0:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=libraryRear_ArchitectureUpperBrowOff matched=0 disabled=0 logged=[]
```

The removed name is absent from the correct regenerated built-player log:

```text
Select-String -Path Logs\point15_renderer_architecture_upper_brow_removed_regen_player_20260612T220331.log -Pattern 'ArchitectureSurfaceDepth_EntranceUpperBrowA' | Measure-Object
Count: 0
```

Tracked / enabled renderer counts also dropped by one:

- Before removal:
  - `tracked=11063`
  - `enabled=min=1555,max=1555,mean=1555.000,stddev=0.000`
- After correct regen removal:
  - `tracked=11062`
  - `enabled=min=1554,max=1554,mean=1554.000,stddev=0.000`

## Post-removal measurements

`libraryRearThinOpaquesOff` after removal:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=libraryRearThinOpaquesOff frame=90 saved=55_libraryRearThinOpaquesOff_frame_090.png disabled=9 baselineDelta meanAbsRgb=0.019 changedSamplePct=0.153 changed=22 samples=14400 playerLocal=(21.125,0.020,17.321) cameraPos=(21.125,2.797,12.670)
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=libraryRearThinOpaquesOff frame=105 saved=56_libraryRearThinOpaquesOff_frame_105.png disabled=9 baselineDelta meanAbsRgb=0.039 changedSamplePct=0.306 changed=44 samples=14400 playerLocal=(21.321,0.020,18.418) cameraPos=(21.484,2.803,13.868)
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=libraryRearThinOpaquesOff frame=120 saved=57_libraryRearThinOpaquesOff_frame_120.png disabled=9 baselineDelta meanAbsRgb=0.044 changedSamplePct=0.340 changed=49 samples=14400 playerLocal=(21.204,0.020,19.563) cameraPos=(21.204,2.798,15.013)
```

The removed exact variant now measures only baseline sampling noise:

```text
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=libraryRear_ArchitectureUpperBrowOff frame=150 saved=138_libraryRear_ArchitectureUpperBrowOff_frame_150.png disabled=0 baselineDelta meanAbsRgb=0.007 changedSamplePct=0.049 changed=7 samples=14400 playerLocal=(21.020,0.020,21.363) cameraPos=(21.020,2.797,16.813)
ANEMORA_HOUSE_SLICE_RENDERER_MOTION_PROBE: isolationVariant=libraryRear_ArchitectureUpperBrowOff frame=179 saved=140_libraryRear_ArchitectureUpperBrowOff_frame_179.png disabled=0 baselineDelta meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=14400 playerLocal=(20.950,0.020,22.050) cameraPos=(20.950,2.793,17.500)
```

Residual secondary candidates after removal:

- `libraryRear_RoofSideRearWallBandOff`: measured `meanAbsRgb=0.000..0.024`, `changedSamplePct=0.000..0.194`.
- `libraryRear_SideSurfaceRearWallBandOff`: measured `meanAbsRgb=0.000..0.025`, `changedSamplePct=0.000..0.188`.

## Findings

- The built-player individual split identified `Current_CentralPlaza_ArchitectureSurfaceDepth_EntranceUpperBrowA` as the dominant measured member of `libraryRearThinOpaquesOff`.
- Removing only that current-time object reduced the broad group from `meanAbsRgb=0.038..0.274`, `changedSamplePct=0.118..1.104` to `meanAbsRgb=0.000..0.044`, `changedSamplePct=0.000..0.340`.
- The invalid `BuildHouseSlicePlayer` attempt is a process hazard: generator changes require `BuildAndValidateBatch`, otherwise the existing scene can be rebuilt unchanged.
- The remaining object-only deltas are much smaller and concentrated around `LibraryRoofSideDepth_RearWallBandA` / `LibrarySideSurfaceBreakup_RearWallBandA`.
- Enabled state remains stable; this slice still supports frustum / visibility-state popping of thin geometry rather than runtime renderer enable-disable churn.

## Next action

- Propagate `docs\review\2026-06-12T22-03_renderer_architecture_upper_brow_removed_regen` to anemora-viewer.
- Run a fresh built-player all-map or corridor-specific visual pass after publication, because the user accepted the visible state but still wants the broader fog / pop line pushed to the next useful point.
- Do not remove `LibraryRoofSideDepth_RearWallBandA` or `LibrarySideSurfaceBreakup_RearWallBandA` yet unless a visual acceptance pass shows they are still objectionable.
- For every later generator edit, use `BuildAndValidateBatch` before player capture.

## Viewer

- Propagate target:
  - `work/chapter1-continuation-map-vs-20260524`
- R2 upload:
  - Initial attempt timed out at `121694` ms and emitted wrangler `EPIPE`; treated as incomplete.
  - Re-run completed:
    - `uploaded 143 files for chapter1-continuation-map-vs-20260524/2026-06-12T22-03_renderer_architecture_upper_brow_removed_regen (bucket TTL 45d); manifest now lists 606 paths`
- anemora-viewer local refresh:
  - `node scripts/setup-content.mjs`: success.
  - `node scripts/setup-r2-images.mjs`: `chapter1-continuation-map-vs-20260524: fetched 602/606 files`; four 404s were pre-existing `2026-06-09T04-12_allmaps/logs/*` entries, not this cycle.
  - `node scripts/collect-content.mjs`: first `120` second run timed out; re-run completed with `files: 6008, docs: 975, images: 3322, unsupported: 600` and wrote `src\data\branches.json (1 branches, 975 docs, 3133 images)`.
  - `npm run build:fast`: success; `1541 page(s) built in 81.04s`.
- Local generated files:
  - Album: `dist\chapter1-continuation-map-vs-20260524\gallery\docs\review\2026-06-12T22-03_renderer_architecture_upper_brow_removed_regen\index.html` length `208932`
  - Devlog: `dist\chapter1-continuation-map-vs-20260524\docs\docs\devlog\2026-06-12_hd2d_point15_renderer_library_rear_upper_brow_removal\index.html` length `1038153`
  - Original PNG: `dist\originals\chapter1-continuation-map-vs-20260524\docs\review\2026-06-12T22-03_renderer_architecture_upper_brow_removed_regen\11_motion_frame_165.png` length `604156`
  - Thumb WEBP: `dist\thumbs\chapter1-continuation-map-vs-20260524\docs\review\2026-06-12T22-03_renderer_architecture_upper_brow_removed_regen\140_libraryRear_ArchitectureUpperBrowOff_frame_179.webp` length `8334`
- anemora-viewer commits:
  - `e2975de chore: refresh upper brow removal review`
  - `43f9057 chore: trigger upper brow review deploy`
- Public verification:
  - Album URL: `https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/gallery/docs/review/2026-06-12T22-03_renderer_architecture_upper_brow_removed_regen/`
  - Devlog URL: `https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/docs/docs/devlog/2026-06-12_hd2d_point15_renderer_library_rear_upper_brow_removal/`
  - Original PNG URL: `https://anemora-viewer.pages.dev/originals/chapter1-continuation-map-vs-20260524/docs/review/2026-06-12T22-03_renderer_architecture_upper_brow_removed_regen/11_motion_frame_165.png`
  - Thumb WEBP URL: `https://anemora-viewer.pages.dev/thumbs/chapter1-continuation-map-vs-20260524/docs/review/2026-06-12T22-03_renderer_architecture_upper_brow_removed_regen/140_libraryRear_ArchitectureUpperBrowOff_frame_179.webp`
  - First public content reflection observed after the real-diff deploy trigger push:
    - `album=200 length=208932 ok=True`
    - `devlog=200 length=1038248 ok=True`
    - `png=200 length=604156 ok=True`
    - `webp=200 length=8334 ok=True`
