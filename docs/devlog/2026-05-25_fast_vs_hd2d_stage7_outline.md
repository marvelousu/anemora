# Fast VS HD-2D Stage 7 Outline

Date: 2026-05-25
Branch: work/fast-vs-hd2d-shading-foundation-20260522

## Change

- Added a conservative fullscreen outline pass named `FastVS HD2D Stage7 Outline`.
- Added `Anemora/FastVS/OutlineFullscreen`, a screen-space color/depth edge shader with low intensity and highlight guarding to avoid heavy black-line styling.
- Added deterministic outline material setup at `Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_stage7_outline.mat`.
- Added Stage 7 outline capture and validation entrypoints, then wired the Stage 7 outline gate into the full house-slice validation batch.

## Evidence

- Validate: `Logs/stage7-outline-validate-full-gfx.log`
- Capture: `Logs/stage7-outline-capture-gfx.log`
- Build: `Logs/stage7-outline-build-gfx.log`
- Smoke: `Logs/stage7-outline-smoke.log`, `MatchCount=0`
- Review directory: `docs/review/2026-05-25T19-17/`
- External screenshots: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260525_stage7_outline\`
- Player build: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Stage 7 outline captures differ from Stage 7 bokeh captures by file hash for all five area screenshots and `tw_current_aperture.png`.
- TimeWindow aperture PNG was read visually and is not black.
- `PortalStencilFeature` remains active in `Assets/Settings/UniversalRenderPipeline_Renderer.asset`.
- Paired-space serialized roots remain present in `Assets/Scenes/Anemora_FastVS_HouseSlice.unity`.

## Gap Evaluation

- The outline pass is visible but intentionally weak; it does not yet create the hand-authored character/prop separation seen in the reference images.
- Screen-space edge detection outlines construction seams and aperture/window geometry as readily as characters, so it is not a substitute for art-directed per-role outlines.
- Plaza still lacks reference-grade foliage breakup, volumetric shafts, material richness, and populated scene density.
- Exterior and library still expose repeated surfaces and unfinished massing; outline does not solve asset density or lighting composition.
- TimeWindow aperture remains visible, but the portal border and captured facade still read as debug-like compared with Octopath-level HD-2D composition.
