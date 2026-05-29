# Fast VS HD-2D Stage8f Library Upper Atmosphere

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

## Scope

Stage8f adds current-library upper-atmosphere accents:

- back-shelf warm shaft planes
- left/right upper cool shaft planes
- back-rail warm rim
- upper-gallery underside dust accents

All Stage8f objects are current-space only, non-colliding, non-arrival TimeWindow paired-space landmarks.

## Public Review Black-Image Guard

The user reported that public review images looked completely black. The generated PNGs and currently deployed public originals/thumbs measured non-black, but the review set was still curated more aggressively for visibility:

- The Stage8f public review files are numbered so the viewer representative thumbnail starts with the bright plaza overview, not a darker exterior or diagnostic frame.
- External reference images, comparison boards, and obstructed route-close diagnostic captures are not included.
- All review PNGs are opaque alpha `(255, 255)`.

## Validation

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage8FLibraryUpperAtmosphereBatch`: exit 0.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage8FLibraryUpperAtmosphereReferenceScreenshotsBatch`: exit 0.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: `Build Finished, Result: Success.`
- Built player smoke: 24-second null-graphics startup smoke; no exception/shader/C# compile-error patterns were found.

Logs:

- `Logs/stage8f_library_upper_atmosphere_validate.log`
- `Logs/stage8f_library_upper_atmosphere_capture.log`
- `Logs/stage8f_library_upper_atmosphere_build.log`
- `Logs/stage8f_library_upper_atmosphere_smoke.log`

## Capture

Internal capture output:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260527_stage8f_library_upper_atmosphere`

Public review:

`docs/review/2026-05-27T06-40/`

Build exe:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

フォルダごと起動:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice`

## Dark-Region Measurement

- `01_plaza_overview.png`: mean `92.8`, dark `<45` `0.060`, central dark `<45` `0.081`.
- `02_library_upper_atmosphere.png`: mean `67.5`, dark `<45` `0.186`, central dark `<45` `0.083`.
- `03_timewindow_aperture.png`: mean `85.9`, dark `<45` `0.107`, central dark `<45` `0.002`.
- `04_home_interior.png`: mean `64.9`, dark `<45` `0.147`, central dark `<45` `0.178`.
- `05_house_exterior.png`: mean `69.7`, dark `<45` `0.158`, central dark `<45` `0.147`.
- `06_plaza_shadow_route.png`: mean `85.5`, dark `<45` `0.080`, central dark `<45` `0.095`.

## Current Gap Evaluation

- Stage8f adds more authored upper-library separation, but the visual delta from Stage8e is very small in full-frame review.
- The Library still needs stronger authored focal hierarchy, atmosphere, material richness, and composition before it approaches target HD-2D quality.
- Work continues; this is not a final visual approval.
