# Fast VS HD-2D Stage 7e Tilt-Shift Refined

Date: 2026-05-25 21:09 JST
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`
Review: `docs/review/2026-05-25T21-09/`

## Changes

- Used `cycle-worker` for `Assets/Art/Shaders/FastVS/FastVS_TiltShiftFullscreen.shader`.
- Kept the existing shader name, pass name, URP fullscreen blit path, and material properties.
- Added shader-local blur mask shaping and a normalized two-ring blur to make out-of-focus top/bottom regions slightly stronger.
- Added a refined Stage 7 capture wrapper that writes to `reference/20260525_stage7_tiltshift_refined`.

## Validation

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage7BokehFocusBatch`
  - Log: `Logs/stage7-tiltshift-refined-validate-single.log`
  - Exit code: 0
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
  - Log: `Logs/stage7-tiltshift-refined-validate-full-gfx.log`
  - Exit code: 0
  - Final marker: `Fast VS house slice validation passed.`
- Capture:
  - Method: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage7TiltShiftRefinedReferenceScreenshotsBatch`
  - Log: `Logs/stage7-tiltshift-refined-capture-gfx.log`
  - Output: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260525_stage7_tiltshift_refined`
- Build:
  - Method: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildHouseSlicePlayer`
  - Log: `Logs/stage7-tiltshift-refined-build-gfx.log`
  - Exit code: 0
  - TiltShift shader compiled in build log.
- Smoke:
  - Log: `Logs/stage7-tiltshift-refined-smoke.log`
  - Result: killed after 20 seconds, error-pattern match count 0

## TimeWindow / Paired Space

- `PortalStencilFeature` remained active in `Assets/Settings/UniversalRenderPipeline_Renderer.asset`.
- Serialized scene check before cleanup found:
  - `FastVS_Current_NiroHouseInteriorExterior`
  - `FastVS_Past_NiroHouseInteriorExterior`
  - `Current_CentralPlazaMap_SeparateSpace`
  - `Past_CentralPlazaMap_SeparateSpace`
  - `TimeWindowPairedSpacePortalController`
  - Stage 7 APV local volumes
- `tw_current_aperture.png` was visually checked and was not black.
- `Assets/Scenes/Anemora_Chapter1.unity` is absent, so no Chapter1 APPLY / INTEGRATOR / REFRESH path was used.

## Build

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Launch the whole `Builds\FastVS_HouseSlice` folder, not the exe alone.

## Gap Notes

- Tilt-shift refinement is visible but modest; the scene still lacks reference_01's strong miniature depth and scale readability.
- Plaza/exterior still read as large flat wall/ground surfaces rather than densely authored HD-2D diorama terrain.
- Painted god rays remain screen-space and weak compared with the reference's volumetric shafts.
- Library still lacks reference_02's campfire composition, character staging, warm/cool light contrast, and dense atmospheric background.
- TimeWindow aperture renders, but the orange border and flat portal wall remain visually far from target quality.
