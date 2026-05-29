# Fast VS HD-2D Stage8h Library Material Readability

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

## Scope

Stage8h adds current-Library material readability accents:

- warm tabletop surfaces and page highlights on the main reading table
- right-desk warm/page accents
- subtle warm floor plank bands
- small table contact shadows
- back-shelf page/book markers

The Stage8h objects are current-space only, non-colliding, and non-arrival TimeWindow paired-space landmarks.

## Validation

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage8HLibraryMaterialReadabilityBatch`: exit 0.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage8HLibraryMaterialReadabilityReferenceScreenshotsBatch`: exit 0.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: `Build Finished, Result: Success.`
- Built player smoke: 24-second null-graphics startup smoke; no exception/shader/C# compile-error patterns were found.

Logs:

- `Logs/stage8h_library_material_readability_validate.log`
- `Logs/stage8h_library_material_readability_capture.log`
- `Logs/stage8h_library_material_readability_build.log`
- `Logs/stage8h_library_material_readability_smoke.log`

## Capture

Internal capture output:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260527_stage8h_library_material_readability`

Public review:

`docs/review/2026-05-27T07-54/`

Build exe:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

フォルダごと起動:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice`

## Dark-Region Measurement

- `01_plaza_overview.png`: mean `92.8`, dark `<45` `0.060`, central dark `<45` `0.081`.
- `02_library_material_readability.png`: mean `69.3`, dark `<45` `0.184`, central dark `<45` `0.077`.
- `03_timewindow_aperture.png`: mean `85.9`, dark `<45` `0.107`, central dark `<45` `0.002`.
- `04_home_interior.png`: mean `64.9`, dark `<45` `0.147`, central dark `<45` `0.178`.
- `05_house_exterior.png`: mean `69.7`, dark `<45` `0.158`, central dark `<45` `0.147`.
- `06_plaza_shadow_route.png`: mean `85.5`, dark `<45` `0.080`, central dark `<45` `0.095`.

Compared with Stage8g, `02_library_material_readability.png` measured `mean_abs=0.578` and changed pixel ratio `0.0176`. Other standard review frames are unchanged because the authored work is scoped to the current Library.

## Current Gap Evaluation

- The review set is inspectable and non-black, with no external reference or comparison images.
- Stage8h improves Library table/floor/shelf readability locally, but it is still a small authored pass.
- The scene remains substantially below target HD-2D quality; upcoming cycles should prioritize stronger composition, material richness, and authored lighting depth.
