# Fast VS HD-2D Stage8j Library Table Light Depth

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

## Scope

Stage8j adds a small authored current-Library pass around the main reading table:

- warm floor spill under the long table
- diagonal cool floor break near the table front
- warm front rim and page glow on the long table
- a readable open-book prop on the long table
- ink-well core, table shadow, and small contact/cast shadows

The Stage8j objects are current-space only, non-colliding, and non-arrival TimeWindow paired-space landmarks. The Stage8i public review camera composition is reused for the Library frame so the added table detail stays visible in `docs/review`.

## Validation

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage8JLibraryTableLightDepthBatch`: exit 0.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage8JLibraryTableLightDepthReferenceScreenshotsBatch`: exit 0.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: `Build Finished, Result: Success.`
- Built player smoke: 24-second null-graphics startup smoke; exception/shader/C# compile-error pattern scan returned 0 matches.

Logs:

- `Logs/stage8j_library_table_light_depth_validate.log`
- `Logs/stage8j_library_table_light_depth_capture.log`
- `Logs/stage8j_library_table_light_depth_build.log`
- `Logs/stage8j_library_table_light_depth_smoke.log`

## Capture

Internal capture output:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260527_stage8j_library_table_light_depth`

Public review:

`docs/review/2026-05-27T08-55/`

Build exe:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

フォルダごと起動:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice`

## Dark-Region Measurement

- `01_plaza_overview.png`: mean `89.5`, dark `<45` `0.067`, central dark `<45` `0.086`, alpha `(255,255)`.
- `02_library_table_light_depth.png`: mean `71.5`, dark `<45` `0.148`, central dark `<45` `0.092`, alpha `(255,255)`.
- `03_timewindow_aperture.png`: mean `82.3`, dark `<45` `0.114`, central dark `<45` `0.005`, alpha `(255,255)`.
- `04_home_interior.png`: mean `61.8`, dark `<45` `0.190`, central dark `<45` `0.213`, alpha `(255,255)`.
- `05_house_exterior.png`: mean `66.6`, dark `<45` `0.197`, central dark `<45` `0.156`, alpha `(255,255)`.
- `06_plaza_shadow_route.png`: mean `82.6`, dark `<45` `0.088`, central dark `<45` `0.106`, alpha `(255,255)`.

Compared with Stage8i, `02_library_table_light_depth.png` measured `mean_abs=0.642` and changed pixel ratio `0.0379`. Other standard review frames are unchanged because the authored work is scoped to the current Library.

## Current Gap Evaluation

- The review set contains only public viewer-facing current-cycle images, with no external reference images or comparison boards.
- Stage8j adds authored Library table light/material depth, but it remains a narrow pass rather than a final visual judgment.
- The scene remains substantially below target HD-2D quality; additional authored material richness, lighting hierarchy, and composition work are still needed.
