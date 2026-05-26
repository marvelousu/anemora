# Fast VS HD-2D Stage8i Library Review Composition

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

## Scope

Stage8i changes the Library public review capture composition for this stage onward. The goal is review legibility: the previous wide Library frame showed the whole room but made table, book, lighting, and floor detail too small to inspect.

The authored scene content from Stage8h is unchanged. Stage8i only changes the Stage8i capture branch to:

- move the Library review anchor closer to the table/readability work
- reduce FOV to focus the Library table/shelf region
- keep the non-black public-review camera path and TimeWindow aperture checks

## Validation

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage8ILibraryReviewCompositionBatch`: exit 0.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage8ILibraryReviewCompositionReferenceScreenshotsBatch`: exit 0.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: `Build Finished, Result: Success.`
- Built player smoke: 24-second null-graphics startup smoke; no exception/shader/C# compile-error patterns were found.

Logs:

- `Logs/stage8i_library_review_composition_validate.log`
- `Logs/stage8i_library_review_composition_capture.log`
- `Logs/stage8i_library_review_composition_build.log`
- `Logs/stage8i_library_review_composition_smoke.log`

## Capture

Internal capture output:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260527_stage8i_library_review_composition`

Public review:

`docs/review/2026-05-27T08-21/`

Build exe:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

フォルダごと起動:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice`

## Dark-Region Measurement

- `01_plaza_overview.png`: mean `92.8`, dark `<45` `0.060`, central dark `<45` `0.081`.
- `02_library_review_composition.png`: mean `75.0`, dark `<45` `0.134`, central dark `<45` `0.077`.
- `03_timewindow_aperture.png`: mean `85.9`, dark `<45` `0.107`, central dark `<45` `0.002`.
- `04_home_interior.png`: mean `64.9`, dark `<45` `0.147`, central dark `<45` `0.178`.
- `05_house_exterior.png`: mean `69.7`, dark `<45` `0.158`, central dark `<45` `0.147`.
- `06_plaza_shadow_route.png`: mean `85.5`, dark `<45` `0.080`, central dark `<45` `0.095`.

Compared with Stage8h, `02_library_review_composition.png` measured `mean_abs=34.072` and changed pixel ratio `0.9976` due to the composition change.

## Current Gap Evaluation

- The review set is inspectable and non-black, with no external reference or comparison images.
- Stage8i makes Library detail easier to inspect in public review, but it is a capture/readability improvement rather than new runtime art content.
- The scene remains substantially below target HD-2D quality; future cycles still need stronger authored art and lighting.
