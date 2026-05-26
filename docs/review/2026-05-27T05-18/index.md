# Stage8b: Library Shelf Density

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Stage8b increases current-library shelf density with larger book/page bands on the rear and side shelves. This is a viewer-facing review checkpoint, not a final visual judgment.

## Build

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Launch the whole folder: **フォルダごと起動**

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice`

## Capture

Internal capture output:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260527_stage8b_library_shelf_density`

This public review copy intentionally excludes external target reference images, comparison boards, and obstructed route-close diagnostic shots.

## Verification

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage8BLibraryShelfDensityBatch`: exit 0.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage8BLibraryShelfDensityReferenceScreenshotsBatch`: exit 0 with graphics enabled.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: `Build Finished, Result: Success.`
- Built player smoke: 24-second null-graphics smoke stopped after startup; no exception/shader/C# compile-error matches were found.
- New review PNG alpha: opaque `(255, 255)`.

## Dark-Region Measurement

- `home.png`: dark `<45` `0.024`, central dark `<45` `0.023`.
- `Home_outside.png`: dark `<45` `0.073`, central dark `<45` `0.077`.
- `library.png`: dark `<45` `0.158`, central dark `<45` `0.033`.
- `plaza_01.png`: dark `<45` `0.042`, central dark `<45` `0.050`.
- `plaza_02_niro_in_shadow.png`: dark `<45` `0.056`, central dark `<45` `0.059`.
- `tw_current_aperture.png`: dark `<45` `0.037`, central dark `<45` `0.000`.

## Images

![home](home.png)

![Home_outside](Home_outside.png)

![library](library.png)

![plaza_01](plaza_01.png)

![plaza_02_niro_in_shadow](plaza_02_niro_in_shadow.png)

![tw_current_aperture](tw_current_aperture.png)

## Current Gap Evaluation

- Library shelves are more legible than Stage8a, but the result still lacks reference-level density, light hierarchy, and atmospheric depth.
- The public set remains free of black-alpha review images, but the overall scene is still substantially below the target HD-2D quality.

Judgment pending. Work continues until an explicit stop instruction.
