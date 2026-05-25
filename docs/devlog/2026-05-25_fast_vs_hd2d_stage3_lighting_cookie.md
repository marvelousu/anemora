# Fast VS HD-2D Stage 3 Lighting Cookie

Date: 2026-05-25
Branch: work/fast-vs-hd2d-shading-foundation-20260522

## Change

- Raised Stage 3 area lighting values: interior key/fill 1.20/0.60, exterior 1.80/0.40, central plaza fill 0.30, library key/fill/window 1.70/0.50/1.50.
- Added a runtime procedural exterior sun cookie and a runtime procedural library window cookie.
- Raised SurfaceRampLit shadow receive default to 0.35 and shifted the textured shadow tint cooler.
- Updated validation gates that still expected pre-Stage 3 lighting values.
- Added Stage 3 capture and validation entrypoints.

## Evidence

- Validate: `Logs/stage3-light-cookie-validate-full-gfx.log`
- Capture: `Logs/stage3-light-cookie-capture-gfx.log`
- Build: `Logs/stage3-light-cookie-build-gfx.log`
- Smoke: `Logs/stage3-light-cookie-smoke.log`, `MatchCount=0`
- Review directory: `docs/review/2026-05-25T18-06/`
- External screenshots: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260525_stage3\`
- Player build: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Gap Evaluation

- Stage 3 increases direct-light contrast, but the plaza shadow mass is still blunt and reads as large hard shapes rather than reference-grade layered foliage/cookie detail.
- Exterior and interior shots remain close, flat, and asset-construction-like; they do not reach the reference image's diorama scale, atmospheric perspective, or material density.
- The library window cookie is visible, but the room still lacks volumetric shafts, warm/cool hierarchy, and dense foreground/background separation.
- The global Daylight LUT remains shared across areas; Stage 3 does not solve area-specific grading.
- TimeWindow aperture is visible and not black, but the image still lacks depth fog, composited air, and reference-level color separation.
