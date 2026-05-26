# Fast VS HD-2D Stage8g Library Hero Lighting

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

## Scope

Stage8g makes the current Library review more visibly changed than Stage8f by adding:

- warm tabletop hero highlights around the main reading table
- a broader warm floor pool around the Library focal area
- warm back-shelf and back-rail highlights
- two current-space-only point lights to lift the table and rear shelf
- restrained cool side separation planes

The Stage8g objects are current-space only, non-colliding, and non-arrival TimeWindow paired-space landmarks.

## Cycle Note

The first Stage8g capture produced no pixel delta against Stage8f because the initial accents were hidden or too weak. The same cycle was corrected before publication by moving the visible highlights onto tabletop/back-shelf surfaces and adding stronger current-space point lights.

The cycle-worker patch was not adopted because it contained large unrelated Kaia/Ruins edits outside the scoped Library file region. The final implementation was applied manually in the parent session and kept to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`.

## Validation

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage8GLibraryHeroLightingBatch`: exit 0.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage8GLibraryHeroLightingReferenceScreenshotsBatch`: exit 0.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: `Build Finished, Result: Success.`
- Built player smoke: 24-second null-graphics startup smoke; no exception/shader/C# compile-error patterns were found.

Logs:

- `Logs/stage8g_library_hero_lighting_validate.log`
- `Logs/stage8g_library_hero_lighting_capture.log`
- `Logs/stage8g_library_hero_lighting_build.log`
- `Logs/stage8g_library_hero_lighting_smoke.log`

## Capture

Internal capture output:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260527_stage8g_library_hero_lighting`

Public review:

`docs/review/2026-05-27T07-31/`

Build exe:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

フォルダごと起動:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice`

## Dark-Region Measurement

- `01_plaza_overview.png`: mean `92.8`, dark `<45` `0.060`, central dark `<45` `0.081`.
- `02_library_hero_lighting.png`: mean `69.0`, dark `<45` `0.184`, central dark `<45` `0.076`.
- `03_timewindow_aperture.png`: mean `85.9`, dark `<45` `0.107`, central dark `<45` `0.002`.
- `04_home_interior.png`: mean `64.9`, dark `<45` `0.147`, central dark `<45` `0.178`.
- `05_house_exterior.png`: mean `69.7`, dark `<45` `0.158`, central dark `<45` `0.147`.
- `06_plaza_shadow_route.png`: mean `85.5`, dark `<45` `0.080`, central dark `<45` `0.095`.

Compared with Stage8f, `02_library_hero_lighting.png` measured `mean_abs=1.487` and changed pixel ratio `0.3230`. Other standard review frames are unchanged because the authored work is scoped to the current Library.

## Current Gap Evaluation

- The review set is inspectable and non-black, with no external reference or comparison images.
- Stage8g is more visible than Stage8f in the Library frame, but the result is still a modest lighting lift rather than a complete HD-2D transformation.
- The scene remains substantially below target HD-2D quality; next work should keep increasing authored focal hierarchy and material richness.
