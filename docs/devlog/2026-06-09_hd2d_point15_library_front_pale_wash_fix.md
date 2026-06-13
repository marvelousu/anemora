# 2026-06-09 HD2D point15 library front pale wash fix

## Context

- Project: Anemora Fast VS House Slice HD-2D recovery.
- Branch: `wip/hd2d-point15-recovery-20260609`.
- Baseline: point15 `e7277f0a`.
- Slice: D library-front white haze / pale wash removal.
- Acceptance source: built-player capture only.
- Review folder: `docs/review/2026-06-09T21-55_library_front_pale_wash_fix/`.
- Latest player path for user review: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`.

## Prior Fix Source

- Previous successful approach: commit `4ac2108c fix(hd2d): strip pale fake-look surface washes at runtime (recovery v3 base)`.
- The useful mechanism was runtime suppression inside `FastVsRealtimeLightShadowRig.ApplyRendererShadowPolicy`, before overlay-profile renderers are force re-enabled.
- The old scope was too broad for point15 recovery. This slice keeps the same mechanism but narrows the matcher to measured library-front pale wash objects only.

## Code Change

- File: `Assets/Scripts/FastVS/FastVsRealtimeLightShadowRig.cs`.
- Added `IsLibraryFrontPaleWashRenderer(Renderer renderer)`.
- Runtime-only suppression: `Application.isPlaying && IsLibraryFrontPaleWashRenderer(renderer)`.
- Suppressed current central-plaza pale wash names from the measured Cycle120/123/125/126 stack:
  - `Cycle120_ReferenceLightColumn_AirDepthForegroundWashA`
  - `Cycle120_ReferenceLightColumn_AirDepthTopVeilA`
  - `Cycle123_ReferenceAerialLift_FacadePaleStoneBloomA`
  - `Cycle123_ReferenceAerialLift_GroundAtmosphericWashA`
  - `Cycle123_ReferenceAerialLift_PlayerLanePaleCatchA`
  - `Cycle123_ReferenceAerialLift_WholeFacadeDepthWashA`
  - `Cycle125_ReferenceDioramaShadow_BackStepPaleSunA`
  - `Cycle125_ReferenceDioramaShadow_CloseSeamSunMuteA`
  - `Cycle125_ReferenceDioramaShadow_FacadeReferenceSunPatchA`
  - `Cycle125_ReferenceDioramaShadow_ReferenceReceiverLiftA`
  - `Cycle125_ReferenceDioramaShadow_StoneSunMatteFieldA`
  - `Cycle126_CloseShadowBarMute_CloseGridWashA`
  - `Cycle126_CloseShadowBarMute_GroundAirUnifierA`
  - `Cycle126_CloseShadowBarMute_StepAirLiftA`
- Kept contact/shadow/player-reference exclusions so grounding/contact cues are not removed by this narrow pass.

## Build Evidence

- Log: `Logs/point15_library_front_pale_wash_fix_build_validate_20260609T215213.log`.
- `Fast VS house slice validation passed.`
- `DisplayProgressNotification: Build Successful`
- `Build Finished, Result: Success.`
- `PlayerBuildInfo duration=97796`
- Built player: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`.

## Built-Player Evidence

- Player log: `Logs/point15_library_front_pale_wash_fix_player_20260609T215500.log`.
- `- Loaded All Assemblies, in  4.421 seconds`
- Runtime suppression log: `[Point15Recovery] ApplyRendererShadowPolicy area=CentralPlaza: disabled 14 library-front pale wash renderer(s).`
- Isolation probe completed: `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: end count=8`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Captures:

```text
01_baseline_current_plaza_library_facade.png                    1056982 1280x720
02_baseline_current_library_facade_close.png                     797097 1280x720
03_no_transparent_overlay_current_plaza_library_facade.png      1050330 1280x720
04_no_transparent_overlay_current_library_facade_close.png       742068 1280x720
05_no_long_road_stack_current_plaza_library_facade.png          1053096 1280x720
06_no_long_road_stack_current_library_facade_close.png           797114 1280x720
07_no_past_long_road_current_plaza_library_facade.png           1056982 1280x720
08_no_past_long_road_current_library_facade_close.png            797082 1280x720
```

Isolation counts after fix:

```text
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=transparentOverlayOff matched=32 disabled=21
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=transparentOverlayOff restored=21
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=longRoadStackOff matched=8 disabled=7
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=longRoadStackOff restored=7
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=pastCentralPlazaLongRoadOff matched=4 disabled=4
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=pastCentralPlazaLongRoadOff restored=4
```

## Pixel Measurements

Method: 4 px sampling, RGB mean absolute difference, changed sample threshold `> 1.0`.

```text
OLD close transparent-off vs baseline: meanAbsRgb=32.594 changedSamplePct=49.356 samples=57600
NEW close transparent-off vs baseline: meanAbsRgb=3.199 changedSamplePct=12.644 samples=57600
OLD close baseline vs NEW close baseline: meanAbsRgb=29.762 changedSamplePct=46.863 samples=57600
NEW close long-road-off vs baseline: meanAbsRgb=0.000 changedSamplePct=0.000 samples=57600
OLD wide baseline vs NEW wide baseline: meanAbsRgb=4.526 changedSamplePct=7.752 samples=57600
NEW wide transparent-off vs baseline: meanAbsRgb=0.462 changedSamplePct=1.819 samples=57600
```

## Findings

- The broad white haze in front of the library facade is removed in the new built-player baseline.
- The close-shot transparent-overlay delta dropped from `meanAbsRgb=32.594` to `meanAbsRgb=3.199`.
- The new close-shot long-road-off delta is `meanAbsRgb=0.000`, so the visible close-shot haze is not caused by the long-road stack.
- Residual transparent overlay influence remains (`changedSamplePct=12.644` in the close shot), but the large white wash rejected by the user is no longer present in the built-player baseline.
- This slice intentionally does not touch generator output, scene YAML, long-road geometry, or the renderer contract.

## Viewer

- Propagate target: `work/chapter1-continuation-map-vs-20260524`.
- R2 upload: `uploaded 10 files for chapter1-continuation-map-vs-20260524/2026-06-09T21-55_library_front_pale_wash_fix (bucket TTL 45d); manifest now lists 126 paths`.
- Viewer rebuild commit: `e982b74 chore: refresh review content 2026-06-09 library-front-pale-wash-fix`.
- Public review check: `261 cycles · 1254 images`, target album present.
- Public album check: `status=200`, `albumLen=23626`, `okAlbum=True`.
- Public devlog check: `status=200`, `devLen=1011898`, `okDev=True`.

## Next Action

- Keep the user-review build path stable while the user checks the latest player.
- Next D work should focus on residual transparent stack artifacts only after this white-haze fix is visually accepted or rejected.
