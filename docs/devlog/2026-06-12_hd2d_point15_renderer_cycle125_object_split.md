# 2026-06-12 HD2D point15 renderer Cycle125 object split

## Scope

- Continue the point15 recovery line from the accepted library-front marker removal and pale-wash suppression state.
- Keep the library-front sun shaft/time-of-day behavior unchanged in this slice.
- Use built-player captures only; editor and gate visuals are not acceptance evidence.
- Measure the remaining library-front haze/fog candidates inside the transparent overlay stack before changing visuals again.

## Code change

- `Assets/Scripts/FastVS/FastVsHouseRuntimeSmokeProbe.cs`
  - Extended `--anemora-house-slice-renderer-isolation-dir` from 18 captures to 40 captures.
  - Split the broad transparent overlay suspect into:
    - `transparentCycle120Off`
    - `transparentCycle125Off`
    - `transparentCycle126Off`
    - `transparentShadowFoundation70Off`
  - Split `Cycle125_ReferenceDioramaShadow` into object-level variants:
    - `cycle125LibraryEaveHardContactOff`
    - `cycle125CenterChalkSunCatchOff`
    - `cycle125RightCrateProjectedCastOff`
    - `cycle125LeftCanopyDappleGroundOff`
    - `cycle125BackDepthHazeOff`
    - `cycle125HighSunbeamColumnOff`
    - `cycle125PlayerTinyContactOff`

## Build evidence

- Transparent split build log:
  - `Logs\point15_renderer_transparent_split_build_validate_20260612T140326.log`
- Cycle125 object split build log:
  - `Logs\point15_renderer_cycle125_object_split_build_validate_20260612T142409.log`
- Key lines in both build logs:
  - `DisplayProgressNotification: Build Successful`
  - `Build Finished, Result: Success.`
  - `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Built-player evidence

- Transparent split player log:
  - `Logs\point15_renderer_transparent_split_player_20260612T141953.log`
- Transparent split review folder:
  - `docs\review\2026-06-12T14-19_renderer_transparent_split`
- Transparent split captures:
  - 26 PNGs
- Cycle125 object split player log:
  - `Logs\point15_renderer_cycle125_object_split_player_20260612T143513.log`
- Cycle125 object split review folder:
  - `docs\review\2026-06-12T14-35_renderer_cycle125_object_split`
- Cycle125 object split captures:
  - 40 PNGs

## Renderer contract

- Built-player contract line:
  - `ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>`
- Pale-wash suppression remained active:
  - `[Point15Recovery] ApplyRendererShadowPolicy area=CentralPlaza: disabled 14 library-front pale wash renderer(s).`

## Transparent split measured results

- `transparentOverlayOff`
  - wide: `meanAbsRgb=0.490 changedSamplePct=1.627 changed=937 samples=57600`
  - close: `meanAbsRgb=3.190 changedSamplePct=11.023 changed=6349 samples=57600`
- `backgroundSkyDepthOff`
  - wide: `meanAbsRgb=0.001 changedSamplePct=0.000 changed=0 samples=57600`
  - close: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
- `outdoorSkyDetailOff`
  - wide: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
  - close: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
- `scenicBackdropOff`
  - wide: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
  - close: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
- `backgroundEnvelopeOff`
  - wide: `meanAbsRgb=0.001 changedSamplePct=0.000 changed=0 samples=57600`
  - close: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
- `transparentCycle120Off`
  - wide: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
  - close: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
- `transparentCycle125Off`
  - wide: `meanAbsRgb=0.420 changedSamplePct=1.460 changed=841 samples=57600`
  - close: `meanAbsRgb=3.002 changedSamplePct=10.580 changed=6094 samples=57600`
- `transparentCycle126Off`
  - wide: `meanAbsRgb=0.073 changedSamplePct=0.182 changed=105 samples=57600`
  - close: `meanAbsRgb=0.197 changedSamplePct=0.474 changed=273 samples=57600`
- `transparentShadowFoundation70Off`
  - wide: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
  - close: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`

## Cycle125 object split measured results

- `cycle125LibraryEaveHardContactOff`
  - matched/disabled: `matched=1 disabled=1`
  - wide: `meanAbsRgb=0.003 changedSamplePct=0.038 changed=22 samples=57600`
  - close: `meanAbsRgb=0.010 changedSamplePct=0.092 changed=53 samples=57600`
- `cycle125CenterChalkSunCatchOff`
  - matched/disabled: `matched=1 disabled=1`
  - wide: `meanAbsRgb=0.152 changedSamplePct=0.342 changed=197 samples=57600`
  - close: `meanAbsRgb=0.756 changedSamplePct=1.674 changed=964 samples=57600`
- `cycle125RightCrateProjectedCastOff`
  - matched/disabled: `matched=1 disabled=1`
  - wide: `meanAbsRgb=0.014 changedSamplePct=0.080 changed=46 samples=57600`
  - close: `meanAbsRgb=0.121 changedSamplePct=0.660 changed=380 samples=57600`
- `cycle125LeftCanopyDappleGroundOff`
  - matched/disabled: `matched=1 disabled=1`
  - wide: `meanAbsRgb=0.012 changedSamplePct=0.113 changed=65 samples=57600`
  - close: `meanAbsRgb=0.049 changedSamplePct=0.479 changed=276 samples=57600`
- `cycle125BackDepthHazeOff`
  - matched/disabled: `matched=1 disabled=0`
  - wide: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
  - close: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
- `cycle125HighSunbeamColumnOff`
  - matched/disabled: `matched=1 disabled=1`
  - wide: `meanAbsRgb=0.240 changedSamplePct=0.887 changed=511 samples=57600`
  - close: `meanAbsRgb=2.070 changedSamplePct=7.675 changed=4421 samples=57600`
- `cycle125PlayerTinyContactOff`
  - matched/disabled: `matched=1 disabled=1`
  - wide: `meanAbsRgb=0.002 changedSamplePct=0.007 changed=4 samples=57600`
  - close: `meanAbsRgb=0.007 changedSamplePct=0.023 changed=13 samples=57600`
- End marker:
  - `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: end count=40`

## Interpretation

- The remaining close-view response is not the background envelope, sky depth, outdoor sky detail, scenic backdrop, Cycle120, or ShadowFoundation70.
- `Cycle125_ReferenceDioramaShadow` is the dominant transparent-stack contributor in this camera:
  - close: `meanAbsRgb=3.002 changedSamplePct=10.580 changed=6094 samples=57600`
- The largest single Cycle125 contributor is `HighSunbeamColumnA`:
  - close: `meanAbsRgb=2.070 changedSamplePct=7.675 changed=4421 samples=57600`
- `BackDepthHazeA` did not change the captured image:
  - `matched=1 disabled=0`
  - close: `meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
- Visual inspection of `38_no_cycle125_high_sunbeam_column_current_library_facade_close.png` shows that disabling `HighSunbeamColumnA` removes the left-window-to-door sunbeam component. This overlaps the user's separate concern that the library-front shaft changes around the time-window state.

## Decision

- Do not remove all Cycle125 renderers.
- Do not treat `HighSunbeamColumnA` as a safe haze-only deletion in this slice, because it is the measured body of the library-front sunbeam.
- Keep the accepted 14-renderer pale-wash suppression in place.
- Next slice should probe the sunbeam state around time-window open/closed and time-of-day before any deletion or generation change.

