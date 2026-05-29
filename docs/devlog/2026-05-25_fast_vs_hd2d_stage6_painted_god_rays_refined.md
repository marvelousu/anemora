# Fast VS HD-2D Stage 6b Painted God Rays Refined

Date: 2026-05-25 20:56 JST
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`
Review: `docs/review/2026-05-25T20-56/`

## Changes

- Used `cycle-worker` for `Assets/Scripts/FastVS/FastVsRealtimeLightShadowRig.cs`.
- Reduced the dark painted grade/shadow alpha through local shaping helpers while keeping the existing conservative clamp ceilings.
- Kept warm ray shaping in the existing `FastVS_Cycle128RayTexture` and `FastVS_Cycle131SunPaintTexture` path.
- Added a refined Stage 6 capture wrapper that writes to `reference/20260525_stage6_painted_rays_refined`.
- Did not add Volumetric Fog, VFX Graph, new runtime camera framing, or new scene assets.

## Validation

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage6PaintedGodRaysBatch`
  - Log: `Logs/stage6-painted-rays-refined-validate-single.log`
  - Exit code: 0
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
  - Log: `Logs/stage6-painted-rays-refined-validate-full-gfx.log`
  - Exit code: 0
  - Final marker: `Fast VS house slice validation passed.`
- Capture:
  - Method: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage6PaintedGodRaysRefinedReferenceScreenshotsBatch`
  - Log: `Logs/stage6-painted-rays-refined-capture-gfx.log`
  - Output: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260525_stage6_painted_rays_refined`
- Build:
  - Method: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildHouseSlicePlayer`
  - Log: `Logs/stage6-painted-rays-refined-build-gfx.log`
  - Exit code: 0
  - Noted log noise: code coverage package `System.Numerics.* failed to resolve`; `move_path failed: No error`; build still returned 0.
- Smoke:
  - Log: `Logs/stage6-painted-rays-refined-smoke.log`
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

- Refined plaza is slightly less veiled than Stage 6a, but still reads as screen-space paint rather than volumetric light.
- Warm rays remain weak compared with reference_01's bright, physically legible shafts.
- Exterior composition remains oversized and flat; it does not approach the wide diorama readability of reference_01.
- Library remains structurally and atmospherically far from reference_02: no campfire composition, no dense warm/cool light hierarchy, sparse particles only.
- TimeWindow aperture is functional, but the orange border and flat wall treatment remain far outside the target HD-2D look.
