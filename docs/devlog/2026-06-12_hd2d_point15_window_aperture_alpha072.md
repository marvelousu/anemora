# 2026-06-12 HD2D point15 window aperture alpha 0.72

## Scope

- Continue from the window sunbeam state probe.
- Address the user's report that the library-front sun shaft / shadow read appears to change when the Time Window is open.
- Keep the Cycle125 sunbeam object itself.
- Use built-player evidence only.

## Code change

- `Assets/Art/Materials/Portal/PortalApertureOverlay.shader`
  - Changed the live portal aperture overlay from `Queue=Geometry+10` / `RenderType=Opaque` / `ZWrite On` to `Queue=Transparent-10` / `RenderType=Transparent` / `Blend SrcAlpha OneMinusSrcAlpha` / `ZWrite Off`.
- `Assets/Scripts/TimeManagement/TimeWindowPairedSpacePortalController.cs`
  - Changed generated aperture material render queue from `2010` to `2990`.
  - Added `PortalApertureCompositeAlpha = 0.72f`.
  - Clamp `_Color` / `_BaseColor` alpha to 0.72 without making future lower-alpha material presets more opaque.
- `Assets/Scripts/FastVS/FastVsHouseRuntimeSmokeProbe.cs`
  - Fixed current-portal isolation matching from stale `TW_V21_CurrentPortal_MatchingCoordinate` to generated current portal paths.
  - Added open-window isolation captures:
    - `currentApertureOff`
    - `currentPortalFrameOnlyOff`
    - `currentPortalAllOff`
  - Added `_Color` / `_BaseColor` to renderer material state logging.

## Build evidence

- Probe condition fix build:
  - `Logs\point15_window_aperture_isolation_probe2_build_validate_20260612T161642.log`
- Queue-only build:
  - `Logs\point15_window_aperture_transparent_queue_build_validate_20260612T162700.log`
- Final alpha 0.72 build:
  - `Logs\point15_window_aperture_alpha072_build_validate_20260612T163658.log`
- Final build key lines:
  - `DisplayProgressNotification: Build Successful`
  - `Build Finished, Result: Success.`
  - `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Built-player evidence

- Valid pre-fix aperture isolation log:
  - `Logs\point15_window_aperture_isolation_probe2_gfx_player_20260612T162359.log`
- Queue-only player log:
  - `Logs\point15_window_aperture_transparent_queue_player_20260612T163321.log`
- Final alpha 0.72 player log:
  - `Logs\point15_window_aperture_alpha072_player_20260612T164251.log`
- Final review folder:
  - `docs\review\2026-06-12T16-42_window_aperture_alpha072`
- Captures:
  - 23 PNGs
- Main comparison captures:
  - `10_window_closed_before_open.png`
  - `11_window_open.png`
  - `12_window_closed_after_open.png`
  - `13_window_open_current_aperture_off.png`
  - `14_window_open_current_frame_only_off.png`
  - `15_window_open_current_portal_all_off.png`

## Renderer contract

- Final built-player contract line:
  - `ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>`

## Superseded invalid measurement

- `Logs\point15_window_aperture_isolation_probe2_player_20260612T162314.log`
  - This run used `-nographics` and produced gray captures.
  - It is not accepted visual evidence.

## Aperture isolation baseline

- Corrected gfx pre-fix isolation:
  - `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedBefore_vs_open meanAbsRgb=8.340 changedSamplePct=20.219 changed=11646 samples=57600`
  - `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=currentApertureOff matched=1 disabled=1`
  - `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta open_vs_currentApertureOff meanAbsRgb=7.656 changedSamplePct=19.413 changed=11182 samples=57600`
  - `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedBefore_vs_currentApertureOff meanAbsRgb=0.933 changedSamplePct=2.389 changed=1376 samples=57600`
  - `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=currentPortalFrameOnlyOff matched=5 disabled=5`
  - `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta open_vs_currentPortalFrameOnlyOff meanAbsRgb=0.897 changedSamplePct=2.366 changed=1363 samples=57600`
  - `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedBefore_vs_currentPortalFrameOnlyOff meanAbsRgb=7.657 changedSamplePct=19.443 changed=11199 samples=57600`
  - `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=currentPortalAllOff matched=6 disabled=6`
  - `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta open_vs_currentPortalAllOff meanAbsRgb=8.351 changedSamplePct=20.214 changed=11643 samples=57600`
  - `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedBefore_vs_currentPortalAllOff meanAbsRgb=0.020 changedSamplePct=0.085 changed=49 samples=57600`

## Queue-only result

- Queue-only changed the material contract but did not materially reduce the visual delta:
  - `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedBefore_vs_open meanAbsRgb=8.479 changedSamplePct=20.417 changed=11760 samples=57600`
  - `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedBefore_vs_currentApertureOff meanAbsRgb=0.933 changedSamplePct=2.382 changed=1372 samples=57600`
  - Aperture material state included `queue=2990,tagQueue=Transparent-10,tagRenderType=Transparent`.

## Final alpha 0.72 result

- Aperture material state:
  - `name=Current_LivePortalApertureMaterial,shader=Anemora/Review/PortalApertureOverlay,queue=2990,tagQueue=Transparent-10,tagRenderType=Transparent,_Color=(1,1,1,0.72)`
- Final image deltas:
  - `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedBefore_vs_open meanAbsRgb=6.291 changedSamplePct=20.280 changed=11681 samples=57600`
  - `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=currentApertureOff matched=1 disabled=1`
  - `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta open_vs_currentApertureOff meanAbsRgb=5.581 changedSamplePct=19.450 changed=11203 samples=57600`
  - `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedBefore_vs_currentApertureOff meanAbsRgb=0.933 changedSamplePct=2.391 changed=1377 samples=57600`
  - `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=currentPortalFrameOnlyOff matched=5 disabled=5`
  - `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta open_vs_currentPortalFrameOnlyOff meanAbsRgb=0.907 changedSamplePct=2.382 changed=1372 samples=57600`
  - `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedBefore_vs_currentPortalFrameOnlyOff meanAbsRgb=5.605 changedSamplePct=19.483 changed=11222 samples=57600`
  - `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=currentPortalAllOff matched=6 disabled=6`
  - `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta open_vs_currentPortalAllOff meanAbsRgb=6.278 changedSamplePct=20.253 changed=11666 samples=57600`
  - `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedBefore_vs_currentPortalAllOff meanAbsRgb=0.020 changedSamplePct=0.089 changed=51 samples=57600`
  - `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedBefore_vs_closedAfter meanAbsRgb=0.004 changedSamplePct=0.038 changed=22 samples=57600`
  - `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: end count=23`

## Interpretation

- The actual light and SunCycle state did not switch in the previous slice.
- The open-window visual change is the current live portal aperture compositing over the floor/facade.
- Disabling current aperture alone nearly returns the closed image:
  - pre-fix `closedBefore_vs_currentApertureOff meanAbsRgb=0.933`
  - final `closedBefore_vs_currentApertureOff meanAbsRgb=0.933`
- Queue/ZWrite correction is still the correct rendering contract for a transparent aperture, but the visible improvement comes from `alpha=0.72`.
- `alpha=0.72` reduces the open delta from the queue-only `8.479` to `6.291` while keeping the portal readable.

## Next slice

- Upload this review cycle to anemora-viewer for visual judgment.
- Continue with the planned fog / haze line after the viewer update.
- If Tom wants an even subtler aperture, test a separate alpha slice instead of changing SunCycle or deleting the Cycle125 sunbeam.
