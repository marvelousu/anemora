# Fast VS HD-2D Stage 6a Painted God Rays

Date: 2026-05-25 20:44 JST
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`
Review: `docs/review/2026-05-25T20-44/`

## Changes

- Used `cycle-worker` for `Assets/Scripts/FastVS/FastVsRealtimeLightShadowRig.cs`.
- Stayed on the already revived Painted Overlay path and did not add Volumetric Fog.
- Strengthened the existing `FastVS_Cycle128RayTexture` and `FastVS_Cycle131SunPaintTexture` alpha shaping through helper methods while preserving `ApplyCycle131CameraPaintOverlay(isRealtimeOutdoor)`.
- Added Stage 6a validate/capture batch wrappers in `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`.

## Validation

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage6PaintedGodRaysBatch`
  - Log: `Logs/stage6-painted-rays-validate-single.log`
  - Exit code: 0
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
  - Log: `Logs/stage6-painted-rays-validate-full-gfx.log`
  - Exit code: 0
  - Final marker: `Fast VS house slice validation passed.`
- Capture:
  - Method: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage6PaintedGodRaysReferenceScreenshotsBatch`
  - Log: `Logs/stage6-painted-rays-capture-gfx.log`
  - Output: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260525_stage6_painted_rays`
- Build:
  - Method: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildHouseSlicePlayer`
  - Log: `Logs/stage6-painted-rays-build-gfx.log`
  - Exit code: 0
- Smoke:
  - Log: `Logs/stage6-painted-rays-smoke.log`
  - Result: killed after 20 seconds, error-pattern match count 0

## TimeWindow / Paired Space

- `PortalStencilFeature` remained active in `Assets/Settings/UniversalRenderPipeline_Renderer.asset`.
- Serialized scene check before cleanup found:
  - `FastVS_Current_NiroHouseInteriorExterior`
  - `FastVS_Past_NiroHouseInteriorExterior`
  - `Current_CentralPlazaMap_SeparateSpace`
  - `Past_CentralPlazaMap_SeparateSpace`
  - `TimeWindowPairedSpacePortalController`
- `tw_current_aperture.png` was visually checked and was not black.
- `Assets/Scenes/Anemora_Chapter1.unity` is absent, so no Chapter1 APPLY / INTEGRATOR / REFRESH path was used.

## Build

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Launch the whole `Builds\FastVS_HouseSlice` folder, not the exe alone.

## Gap Notes

- The added rays still read mostly as a screen-space painted veil, not physical volumetric light falling through the scene.
- Exterior and plaza are still far from the reference's diorama-scale atmospheric depth; the building facade and ground remain flat, gray, and oversized.
- The dark vertical overlay bands dominate more than the warm shafts; this is a visible Stage 6a weakness.
- Library remains too dim and sparse compared with the camp/night reference; fire/camp composition and authored warm lighting are still absent.
- TimeWindow aperture renders, but its orange frame and flat wall read strongly against the target HD-2D look.
