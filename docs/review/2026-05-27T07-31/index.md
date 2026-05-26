# Stage8g: Library Hero Lighting

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Stage8g adds current-Library hero lighting around the table and rear shelf. This review set is curated for public viewer legibility: numbered filenames make the representative thumbnail start from the bright plaza overview, and no external reference images, comparison boards, or obstructed route-close diagnostic shots are included.

## Build

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

フォルダごと起動:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice`

## Capture

Internal capture output:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260527_stage8g_library_hero_lighting`

Public review directory:

`docs/review/2026-05-27T07-31/`

## Verification

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage8GLibraryHeroLightingBatch`: exit 0.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage8GLibraryHeroLightingReferenceScreenshotsBatch`: exit 0.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: `Build Finished, Result: Success.`
- Built player smoke: 24-second null-graphics startup smoke; no exception/shader/C# compile-error patterns were found.
- New review PNG alpha: opaque `(255, 255)`.

## Dark-Region Measurement

- `01_plaza_overview.png`: mean `92.8`, dark `<45` `0.060`, central dark `<45` `0.081`.
- `02_library_hero_lighting.png`: mean `69.0`, dark `<45` `0.184`, central dark `<45` `0.076`.
- `03_timewindow_aperture.png`: mean `85.9`, dark `<45` `0.107`, central dark `<45` `0.002`.
- `04_home_interior.png`: mean `64.9`, dark `<45` `0.147`, central dark `<45` `0.178`.
- `05_house_exterior.png`: mean `69.7`, dark `<45` `0.158`, central dark `<45` `0.147`.
- `06_plaza_shadow_route.png`: mean `85.5`, dark `<45` `0.080`, central dark `<45` `0.095`.

Compared with Stage8f, `02_library_hero_lighting.png` measured `mean_abs=1.487` and changed pixel ratio `0.3230`.

## Images

![plaza overview](01_plaza_overview.png)

![library hero lighting](02_library_hero_lighting.png)

![timewindow aperture](03_timewindow_aperture.png)

![home interior](04_home_interior.png)

![house exterior](05_house_exterior.png)

![plaza shadow route](06_plaza_shadow_route.png)

## Current Gap Evaluation

- The review set is inspectable and non-black, with no external reference or comparison images.
- Stage8g is more visible than Stage8f in the Library frame, but the result remains a modest lighting lift.
- The scene remains substantially below target HD-2D quality.

Judgment pending. Work continues until an explicit stop instruction.
