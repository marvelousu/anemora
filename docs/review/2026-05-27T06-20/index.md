# Stage8e: Library Lamp Bloom

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Stage8e adds current-library lamp focal points with warm halos, floor pools, and conservative local point lights. This is a viewer-facing review checkpoint, not a final visual judgment.

## Build

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Launch the whole folder: **フォルダごと起動**

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice`

## Capture

Internal capture output:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260527_stage8e_library_lamp_bloom`

This public review copy intentionally excludes external target reference images, comparison boards, and obstructed route-close diagnostic shots.

## Verification

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage8ELibraryLampBloomBatch`: exit 0.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage8ELibraryLampBloomReferenceScreenshotsBatch`: exit 0 with graphics enabled.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: `Build Finished, Result: Success.`
- Built player smoke: 24-second null-graphics smoke stopped after startup; no exception/shader/C# compile-error matches were found.
- New review PNG alpha: opaque `(255, 255)`.

## Dark-Region Measurement

- `home.png`: dark `<45` `0.190`, central dark `<45` `0.221`.
- `Home_outside.png`: dark `<45` `0.197`, central dark `<45` `0.156`.
- `library.png`: dark `<45` `0.201`, central dark `<45` `0.103`.
- `plaza_01.png`: dark `<45` `0.067`, central dark `<45` `0.089`.
- `plaza_02_niro_in_shadow.png`: dark `<45` `0.088`, central dark `<45` `0.109`.
- `tw_current_aperture.png`: dark `<45` `0.114`, central dark `<45` `0.004`.

## Images

![home](home.png)

![Home_outside](Home_outside.png)

![library](library.png)

![plaza_01](plaza_01.png)

![plaza_02_niro_in_shadow](plaza_02_niro_in_shadow.png)

![tw_current_aperture](tw_current_aperture.png)

## Current Gap Evaluation

- The review set is inspectable and non-black, with no external reference or comparison images.
- Stage8e creates clearer Library lamp focal points, but the full-frame result still needs stronger authored light, atmosphere, and material richness.
- The scene remains substantially below target HD-2D quality.

Judgment pending. Work continues until an explicit stop instruction.
