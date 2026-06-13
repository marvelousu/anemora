# 2026-06-10 HD2D point15 renderer background isolation

## Scope

- Continue the point15 recovery line from the accepted library-front marker removal state.
- Do not change the time-of-day gate for the library-front sun shaft in this slice.
- Add built-player isolation evidence for the remaining library-front haze/fog and far-road/outline concern.

## Code change

- `Assets/Scripts/FastVS/FastVsHouseRuntimeSmokeProbe.cs`
  - Extended `--anemora-house-slice-renderer-isolation-dir` from 8 captures to 18 captures.
  - Added baseline delta logging for every isolation variant:
    - `meanAbsRgb`
    - `changedSamplePct`
    - `changed`
    - `samples`
  - Split the prior broad suspects into:
    - `backgroundSkyDepthOff`
    - `outdoorSkyDetailOff`
    - `scenicBackdropOff`
    - `roadGeometryOff`
    - `backgroundEnvelopeOff`

## Build recovery note

- First rebuild attempts hit host capacity/pagefile pressure:
  - `GetLastError: 1455 (0x000005AF): The paging file is too small for this operation to complete.`
- To proceed without touching source evidence, only regenerable Unity caches were removed:
  - current worktree `Library\Bee`, `Library\BurstCache`, `Library\Artifacts`
  - Unity local cache folders
  - current worktree `Library\PackageCache`
  - `C:\Users\maro6\Documents\Unity\Anemora-revert-proof-scratch\Library`
- `Anemora-p3-recovery` was not cleaned.
- Successful rebuild used reduced Unity worker count:
  - `-job-worker-count 2`

## Build evidence

- Build log:
  - `Logs\point15_renderer_background_isolation_build_validate_20260610T023500.log`
- Key lines:
  - `JobSystem: Creating JobQueue using job-worker-count value 2`
  - `DisplayProgressNotification: Build Successful`
  - `Build Finished, Result: Success.`
  - `PlayerBuildInfo duration=125901`
  - `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260609\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Built-player evidence

- Player log:
  - `Logs\point15_renderer_background_isolation_player_20260610T083800.log`
- Review folder:
  - `docs\review\2026-06-10T08-38_renderer_background_isolation`
- Captures:
  - `01_baseline_current_plaza_library_facade.png`
  - `02_baseline_current_library_facade_close.png`
  - `03_no_transparent_overlay_current_plaza_library_facade.png`
  - `04_no_transparent_overlay_current_library_facade_close.png`
  - `05_no_long_road_stack_current_plaza_library_facade.png`
  - `06_no_long_road_stack_current_library_facade_close.png`
  - `07_no_past_long_road_current_plaza_library_facade.png`
  - `08_no_past_long_road_current_library_facade_close.png`
  - `09_no_background_sky_depth_current_plaza_library_facade.png`
  - `10_no_background_sky_depth_current_library_facade_close.png`
  - `11_no_outdoor_sky_detail_current_plaza_library_facade.png`
  - `12_no_outdoor_sky_detail_current_library_facade_close.png`
  - `13_no_scenic_backdrop_current_plaza_library_facade.png`
  - `14_no_scenic_backdrop_current_library_facade_close.png`
  - `15_no_road_geometry_current_plaza_library_facade.png`
  - `16_no_road_geometry_current_library_facade_close.png`
  - `17_no_background_envelope_current_plaza_library_facade.png`
  - `18_no_background_envelope_current_library_facade_close.png`

## Measured results

- Renderer contract:
  - `RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True`
- Pale-wash suppression still active:
  - `[Point15Recovery] ApplyRendererShadowPolicy area=CentralPlaza: disabled 14 library-front pale wash renderer(s).`
- `transparentOverlayOff`
  - wide: `meanAbsRgb=0.490 changedSamplePct=1.627 changed=937 samples=57600`
  - close: `meanAbsRgb=3.190 changedSamplePct=11.023 changed=6349 samples=57600`
- `longRoadStackOff`
  - wide: `meanAbsRgb=0.123 changedSamplePct=0.540 changed=311 samples=57600`
  - close: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
- `pastCentralPlazaLongRoadOff`
  - wide: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
  - close: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
- `backgroundSkyDepthOff`
  - wide: `meanAbsRgb=0.001 changedSamplePct=0.000 changed=0 samples=57600`
  - close: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
- `outdoorSkyDetailOff`
  - wide: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
  - close: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
- `scenicBackdropOff`
  - wide: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
  - close: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
- `roadGeometryOff`
  - wide: `meanAbsRgb=0.123 changedSamplePct=0.540 changed=311 samples=57600`
  - close: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
- `backgroundEnvelopeOff`
  - wide: `meanAbsRgb=0.001 changedSamplePct=0.003 changed=2 samples=57600`
  - close: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`

## Interpretation

- The remaining library-front fog/haze response is not caused by `OutdoorBackgroundSkyDepth`, `OutdoorSkyDetail`, `ScenicBackdrop`, or the combined background envelope in this camera.
- The measurable remaining close-view haze response is still in the transparent overlay stack.
- The long-road/road-geometry stack only affects the wide context view and does not affect the close facade sample.

## Next slice

- Do not remove the entire transparent overlay stack.
- Split `transparentOverlayOff` into smaller built-player variants before fixing:
  - `Cycle120_ReferenceLightColumn`
  - `Cycle125_ReferenceDioramaShadow`
  - `Cycle126_CloseShadowBarMute`
  - `ShadowFoundationCycle70_LibraryDiagonalCastA`
- Keep the already accepted 14 pale-wash suppression in place.
