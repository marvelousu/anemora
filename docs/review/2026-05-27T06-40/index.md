# Stage8f: Library Upper Atmosphere

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Stage8f adds subtle current-library upper-atmosphere shafts and gallery accents. This review set is curated for public viewer legibility after the black-image complaint: numbered filenames make the representative thumbnail start from the brighter plaza overview, and no external reference images, comparison boards, or obstructed diagnostic route shots are included.

## Build

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

フォルダごと起動:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice`

## Capture

Internal capture output:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260527_stage8f_library_upper_atmosphere`

Public review directory:

`docs/review/2026-05-27T06-40/`

## Verification

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage8FLibraryUpperAtmosphereBatch`: exit 0.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage8FLibraryUpperAtmosphereReferenceScreenshotsBatch`: exit 0.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: `Build Finished, Result: Success.`
- Built player smoke: 24-second null-graphics startup smoke; no exception/shader/C# compile-error patterns were found.
- New review PNG alpha: opaque `(255, 255)`.

## Dark-Region Measurement

- `01_plaza_overview.png`: mean `92.8`, dark `<45` `0.060`, central dark `<45` `0.081`.
- `02_library_upper_atmosphere.png`: mean `67.5`, dark `<45` `0.186`, central dark `<45` `0.083`.
- `03_timewindow_aperture.png`: mean `85.9`, dark `<45` `0.107`, central dark `<45` `0.002`.
- `04_home_interior.png`: mean `64.9`, dark `<45` `0.147`, central dark `<45` `0.178`.
- `05_house_exterior.png`: mean `69.7`, dark `<45` `0.158`, central dark `<45` `0.147`.
- `06_plaza_shadow_route.png`: mean `85.5`, dark `<45` `0.080`, central dark `<45` `0.095`.

## Images

![plaza overview](01_plaza_overview.png)

![library upper atmosphere](02_library_upper_atmosphere.png)

![timewindow aperture](03_timewindow_aperture.png)

![home interior](04_home_interior.png)

![house exterior](05_house_exterior.png)

![plaza shadow route](06_plaza_shadow_route.png)

## Current Gap Evaluation

- The review set is inspectable and non-black, with no external reference or comparison images.
- Stage8f's full-frame Library delta is still too subtle; the next cycle should produce a larger visual change rather than another near-invisible pass.
- The scene remains substantially below target HD-2D quality.

Judgment pending. Work continues until an explicit stop instruction.
