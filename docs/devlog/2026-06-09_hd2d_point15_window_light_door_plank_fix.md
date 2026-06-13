# 2026-06-09 HD2D point15 window light and door plank fix

## Context

- Project: Anemora Fast VS House Slice HD-2D recovery.
- Branch: `wip/hd2d-point15-recovery-20260609`.
- Baseline: point15 `e7277f0a`.
- Slice: user review follow-up for library-front time-window light/shadow stability and the door-front pillar-like object.
- User screenshot: `C:\Users\maro6\Pictures\Screenshots\Screenshot 2026-06-09 222113.png`.
- Acceptance source: built-player capture only.
- Review folder: `docs/review/2026-06-09T22-56_window_door_final/`.
- Latest player path for user review: `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260609\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`.

## User-Reported Issues

- The library-front incoming light / shadow looked different while the time window was open.
- A pillar-like object in front of the library door, circled in the user screenshot, looked distracting.

## 2026-06-10 Correction

- This door-object finding was superseded by the 2026-06-10 marker probe.
- The screenshot-circled pillar-like object was not `Current_CentralPlaza_LibraryDoorCenterPlank`.
- The built-player isolation that actually removed it was `Current_CentralPlaza_Chapter1_B2_LibraryFrontMarker`.
- See `docs/devlog/2026-06-10_hd2d_point15_library_front_marker_removal.md`.

## Code Change

- File: `Assets/Scripts/TimeManagement/TimeWindowPairedSpacePortalController.cs`.
  - Added review summaries for aperture renderer suppression and visual-overlay exemptions.
  - Changed aperture-intersecting renderer suppression so visual light/shadow/air/dust/wash overlays are exempted from the global portal-open suppression path.
  - Logged `ANEMORA_TIME_WINDOW_APERTURE_SUPPRESSION` with suppressed and exempted renderer samples.
- File: `Assets/Scripts/FastVS/FastVsHouseRuntimeSmokeProbe.cs`.
  - Added `--anemora-house-slice-window-door-review-dir`.
  - Added built-player captures for door plank isolation and window closed/open/closed-after comparison.
  - Added renderer contract, door candidate, light renderer, suppression, and image-delta logs.
- File: `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`.
  - Removed generation of `Current_CentralPlaza_LibraryDoorCenterPlank`.
  - Added the same object to obsolete validation so scene regeneration fails if it reappears.

## Build Evidence

- Log: `Logs/point15_window_door_final_build_validate_20260609T224843.log`.
- `Fast VS house slice validation passed.`
- `DisplayProgressNotification: Build Successful`
- `Build Finished, Result: Success.`
- `PlayerBuildInfo duration=90796`
- Built player: `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260609\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`.
- Built player timestamp: `6/9/2026 10:55:38 PM`.
- Built player size: `667648`.

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

## Built-Player Evidence

- Player log: `Logs/point15_window_door_final_player_20260609T225704.log`.
- Probe exit: `PLAYER_EXIT=0`.
- Review folder: `docs/review/2026-06-09T22-56_window_door_final/`.

Captures:

```text
01_closed_door_light_baseline.png 800674
02_no_center_plank.png            798917
03_no_door_panels.png             799039
04_no_door_relief_depth.png       796647
10_window_closed_before_open.png  799262
11_window_open.png                830925
12_window_closed_after_open.png   799263
```

Door-plank proof:

```text
doorCandidates=[Current_CentralPlaza_LibraryDoorCenterPlank=missing | ...]
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=centerPlankOff matched=0 disabled=0 logged=[]
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=centerPlankOff restored=0
```

Aperture suppression proof:

```text
ANEMORA_TIME_WINDOW_APERTURE_SUPPRESSION: suppressed=14 exemptedVisualOverlay=27
WINDOW_OPEN_STATE centerPlank=missing suppressedRenderers=14
WINDOW_CLOSED_AFTER centerPlank=missing suppressedRenderers=0
```

Visual overlay exemptions include the light/shadow families that previously could be disabled during portal-open suppression:

```text
Current_CentralPlaza_Cycle113_SunbeamShaft_MidPlazaA
Current_CentralPlaza_Cycle118_ShadowReceiverField_SunBreakPlayerLaneA
Current_CentralPlaza_Cycle119_ReferenceComposite_DeepShadowFountainA
Current_CentralPlaza_Cycle120_ReferenceLightColumn_ShadowPlayerLeftA
Current_CentralPlaza_Cycle121_LegacySunRibbonCleanSun_FloorCobbleWarmPathA
Current_CentralPlaza_Cycle125_ReferenceDioramaShadow_LeftCanopyDappleGroundA
```

Pixel measurements:

```text
ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedBefore_vs_open meanAbsRgb=8.308 changedSamplePct=22.161 changed=12765 samples=57600
ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedBefore_vs_closedAfter meanAbsRgb=0.002 changedSamplePct=0.003 changed=2 samples=57600
ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedDoorBaseline_vs_closedBeforePortal meanAbsRgb=0.465 changedSamplePct=1.861 changed=1072 samples=57600
```

## Findings

- Superseded on 2026-06-10: the screenshot-circled pillar-like object did not match `Current_CentralPlaza_LibraryDoorCenterPlank`. It was later identified as `Current_CentralPlaza_Chapter1_B2_LibraryFrontMarker` and removed in `docs/devlog/2026-06-10_hd2d_point15_library_front_marker_removal.md`.
- The open time-window frame still changes the portal rectangle itself, as expected, with `closedBefore_vs_open meanAbsRgb=8.308`.
- The scene returns to the same light/shadow state after closing the portal: `closedBefore_vs_closedAfter meanAbsRgb=0.002`, `changedSamplePct=0.003`, `changed=2 / 57600`.
- The built-player open-state log confirms light/shadow overlay renderers are exempted from aperture suppression (`exemptedVisualOverlay=27`) instead of being disabled as aperture-intersecting geometry.

## Viewer

- R2 upload: `uploaded 9 files for chapter1-continuation-map-vs-20260524/2026-06-09T22-56_window_door_final (bucket TTL 45d); manifest now lists 135 paths`.
- anemora-viewer content refresh:
  - `node scripts/setup-r2-images.mjs`: `chapter1-continuation-map-vs-20260524: fetched 131/135 files`, `fetched 131 images from R2`.
  - The 4 missing R2 entries were old `2026-06-09T04-12_allmaps/logs/...` paths, not this review cycle.
  - `node scripts/collect-content.mjs`: `files: 5541, docs: 963, images: 2874, unsupported: 604`.
  - `node scripts/collect-content.mjs`: `wrote src\data\branches.json (1 branches, 963 docs, 2685 images)`.
  - `npm run build:fast`: `1518 page(s) built in 109.07s`, `Complete!`.
  - `npx pagefind --site dist --output-subdir pagefind`: `Indexed 1518 pages`, `Indexed 68988 words`.
- anemora-viewer commits:
  - `feb68e1 chore: refresh review content 2026-06-09 window-door-final`
  - `d0a0b86 chore: default review R2 image source`
- Public review check:
  - `reviewLen=405065 okReview=True`
  - `albumLen=20678 okAlbum=True`
  - `devLen=1012871 okDev=True`
  - `11_window_open.png Status=200 Length=830925 ContentType=image/png`

## Next Action

- Ask the user to review the latest player path above, especially the open-window transitional look.
