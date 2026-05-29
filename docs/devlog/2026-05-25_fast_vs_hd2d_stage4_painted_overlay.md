# Fast VS HD-2D Stage 4 Painted Overlay

Date: 2026-05-25
Branch: work/fast-vs-hd2d-shading-foundation-20260522

## Change

- Re-enabled the existing Cycle128/131 camera-space painted overlay only for runtime outdoor areas: Exterior, CentralPlaza, and Library.
- Kept Interior overlay off and validated that the camera overlay root becomes inactive after returning to Interior.
- Softened the overlay alpha clamps in the runtime rig to keep this as a conservative painted-overlay revival rather than a new decoration pass.
- Added Stage 4 capture and validation entrypoints, then wired the Stage 4 gate into the full house-slice validation batch.

## Evidence

- Validate: `Logs/stage4-painted-overlay-validate-full-gfx.log`
- Capture: `Logs/stage4-painted-overlay-capture-gfx.log`
- Build: `Logs/stage4-painted-overlay-build-gfx.log`
- Smoke: `Logs/stage4-painted-overlay-smoke.log`, `MatchCount=0`
- Review directory: `docs/review/2026-05-25T18-24/`
- External screenshots: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260525_stage4\`
- Player build: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- TimeWindow aperture PNG was read visually and is not black.
- `PortalStencilFeature` remains active in `Assets/Settings/UniversalRenderPipeline_Renderer.asset`.
- Paired-space serialized roots remain present in `Assets/Scenes/Anemora_FastVS_HouseSlice.unity`.

## Gap Evaluation

- Stage 4 makes the outdoor captures darker and more graphic, but the plaza shadow still reads as broad hard masses rather than reference-grade layered foliage and atmospheric light.
- The overlay does not provide true volumetric shafts, tilt-shift depth of field, mist, or the soft distance falloff visible in the Octopath reference images.
- Exterior and library captures still expose construction-like geometry, repeated texture bands, and weak material hierarchy.
- The TimeWindow aperture is visible, but the portal border and captured facade remain far from the target composition, lighting, and color separation.
- This pass does not solve area-specific grading, APV/GI, outline treatment, VFX particles, or asset density.
