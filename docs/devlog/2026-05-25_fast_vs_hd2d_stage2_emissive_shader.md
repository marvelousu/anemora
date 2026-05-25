# Fast VS HD-2D Stage 2 Emissive Shader

Date: 2026-05-25
Branch: work/fast-vs-hd2d-shading-foundation-20260522

## Change

- Added `_EmissionMap`, `[HDR] _EmissionColor`, and `_EmissionIntensity` support to `FastVS_SurfaceRampLit.shader` and `FastVS_SpriteCardRampUnlit.shader`.
- Applied the sprite-card emissive path narrowly to `window_light` and `hd2d_library_window_light` PortalWindow materials.
- Added Stage 2 validation for shader emission properties, target material shader assignment, emission maps, warm HDR color, and intensity.
- Added Stage 2 capture entrypoint for the 5-area reference set plus TimeWindow aperture.

## Evidence

- Validate: `Logs/stage2-emissive-validate-gfx.log`
- Capture: `Logs/stage2-emissive-capture-gfx.log`
- Build: `Logs/stage2-emissive-build-gfx.log`
- Smoke: `Logs/stage2-emissive-smoke.log`, `MatchCount=0`
- Review directory: `docs/review/2026-05-25T17-10/`
- External screenshots: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260525_stage2\`
- Player build: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Gap Evaluation

- The emissive path creates stronger window peaks, but it does not produce the reference-level bloom atmosphere or local warm/cool lighting hierarchy by itself.
- The home and library shots still read as geometric/flat surfaces with limited air depth and weak material response.
- Plaza screenshots remain far below the reference: facade relief, ground breakup, layered shadow quality, and aerial haze are still insufficient.
- The global Stage 5 Daylight LUT still affects all areas; Stage 2 does not add area-specific runtime grade switching.
- TimeWindow aperture remains visible and not black, but the aperture image still lacks reference-grade fog, depth separation, and past-space color design.
