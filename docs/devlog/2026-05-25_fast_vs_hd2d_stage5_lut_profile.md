# Fast VS HD-2D Stage 5 LUT Profile

Date: 2026-05-25
Branch: work/fast-vs-hd2d-shading-foundation-20260522

## Change

- Added deterministic 32x32x32 flattened LUT generation for:
  - `Assets/Art/Textures/FastVS/HouseSlice/LUT_Daylight_Plaza.png`
  - `Assets/Art/Textures/FastVS/HouseSlice/LUT_Indoor_Warm.png`
  - `Assets/Art/Textures/FastVS/HouseSlice/LUT_TimeWindow_Past.png`
- Wired the global `DefaultVolumeProfile` `ColorLookup` to `LUT_Daylight_Plaza.png` with contribution `0.60`.
- Added Stage 5 validation coverage for LUT dimensions/import settings, `ColorLookup` texture, and contribution.
- Added Stage 5 capture entrypoint for the 5-area reference set plus TimeWindow aperture.

## Evidence

- Validate: `Logs/stage5-lut-validate-gfx.log`
- Capture: `Logs/stage5-lut-capture-gfx.log`
- Build: `Logs/stage5-lut-build-gfx.log`
- Smoke: `Logs/stage5-lut-smoke.log`
- Review directory: `docs/review/2026-05-25T16-34/`
- External screenshots: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260525_stage5\`
- Player build: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Gap Evaluation

- The global Daylight LUT is active, but area-specific runtime LUT switching is not implemented yet; home/library still receive the plaza/daylight grade.
- The reference images still have much richer aerial haze, local warm/cool separation, micro shadow layering, and material response.
- The plaza facade and ground still read flat compared with the Octopath-level reference; LUT alone does not create missing surface relief or volumetric shafts.
- Interior/library contrast remains heavy and geometric; the LUT does not solve the missing emissive/glow hierarchy planned for Stage 2.
- TimeWindow aperture is visible and not black, but its interior image still lacks the reference-grade depth, fog, and graded past-space separation.
