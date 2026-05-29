# Fast VS HD-2D Stage 7 Bokeh Focus

Date: 2026-05-25
Branch: work/fast-vs-hd2d-shading-foundation-20260522

## Change

- Switched the shared house-slice DepthOfField override from Gaussian to Bokeh with focus distance 5.4, aperture 2.4, focal length 85, six blades, and high-quality sampling.
- Added `FastVsHd2dDepthOfFieldFocusController` so runtime review cameras can drive Bokeh focus distance from the player target.
- Added a conservative fullscreen tilt-shift pass after post-processing via `FastVS HD2D Stage7 TiltShift`.
- Added Stage 7 capture and validation entrypoints, then wired the Stage 7 gate into the full house-slice validation batch.

## Evidence

- Validate: `Logs/stage7-bokeh-focus-validate-full-gfx.log`
- Capture: `Logs/stage7-bokeh-focus-capture-gfx.log`
- Build: `Logs/stage7-bokeh-focus-build-gfx.log`
- Smoke: `Logs/stage7-bokeh-focus-smoke.log`, `MatchCount=0`
- Review directory: `docs/review/2026-05-25T18-56/`
- External screenshots: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260525_stage7\`
- Player build: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Stage 7 captures differ from Stage 4 by file hash for all five area screenshots and `tw_current_aperture.png`.
- TimeWindow aperture PNG was read visually and is not black.
- `PortalStencilFeature` remains active in `Assets/Settings/UniversalRenderPipeline_Renderer.asset`.
- Paired-space serialized roots remain present in `Assets/Scenes/Anemora_FastVS_HouseSlice.unity`.

## Gap Evaluation

- Stage 7 adds a visible focus treatment, but the result is still a screen-space blur/darken effect rather than the reference images' integrated miniature-camera depth, air, and lighting.
- The plaza still reads as hard planar shadows over sparse geometry; it lacks layered foliage breakup, bright atmospheric shafts, and warm/cool color separation.
- Exterior and library captures still expose construction-like surfaces, repeated bands, and weak asset/material hierarchy.
- The Bokeh focus controller is runtime-oriented, but the authored scene still lacks the APV/GI, outline discipline, VFX particles, and area-specific art density needed for reference-level HD-2D.
- The TimeWindow aperture remains visible, but the portal composition, border treatment, and captured facade are still far from the Octopath reference quality target.
