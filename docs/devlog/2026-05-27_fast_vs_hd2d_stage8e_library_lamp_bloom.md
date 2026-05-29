# Stage8e: Library Lamp Bloom

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Cycle goal:
- Continue after Stage8d by adding visible current-library warm lamp focal points.
- Make the Library capture show a clearer Bloom/lighting target without using external reference images.

## Changes

- Added Stage8e batch entry points:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage8ELibraryLampBloomBatch`
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage8ELibraryLampBloomReferenceScreenshotsBatch`
- Added three current-library lamp stations:
  - long-table lamp,
  - Reto-desk lamp,
  - back-shelf lamp.
- Each station includes a small visible lamp core, a transparent emissive halo, a warm floor pool, and a conservative current-space point light.
- All visible Stage8e objects are non-colliding and non-arrival TimeWindow paired-space landmarks.
- Extended the capture predicate so `stage8e_library_lamp_bloom` keeps the public-review camera framing and opaque-alpha capture path.

## Validation

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage8ELibraryLampBloomBatch`
  - Exit 0.
  - Log: `Logs\stage8e_library_lamp_bloom_validate.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage8ELibraryLampBloomReferenceScreenshotsBatch`
  - Exit 0 with graphics enabled.
  - Log: `Logs\stage8e_library_lamp_bloom_capture.log`
  - Internal capture output: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260527_stage8e_library_lamp_bloom`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
  - Build finished successfully.
  - Log: `Logs\stage8e_library_lamp_bloom_build.log`
- Built player smoke:
  - Log: `Logs\stage8e_library_lamp_bloom_smoke.log`
  - 24-second null-graphics smoke was stopped after startup; no `Exception`, `NullReference`, `MissingReference`, assertion, shader error/warning, or C# compile-error matches were found.

## Public Review

- New review directory: `docs/review/2026-05-27T06-20/`
- Public set contains only project screenshots. No external game reference images, comparison boards, or obstructed route-close diagnostic shots are included.
- Build exe: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Review/build note: フォルダごと起動

## Dark-Region Measurement

- `home.png`: dark `<45` `0.190`, central dark `<45` `0.221`, mean `61.8`.
- `Home_outside.png`: dark `<45` `0.197`, central dark `<45` `0.156`, mean `66.6`.
- `library.png`: dark `<45` `0.201`, central dark `<45` `0.103`, mean `64.6`.
- `plaza_01.png`: dark `<45` `0.067`, central dark `<45` `0.089`, mean `89.5`.
- `plaza_02_niro_in_shadow.png`: dark `<45` `0.088`, central dark `<45` `0.109`, mean `82.6`.
- `tw_current_aperture.png`: dark `<45` `0.114`, central dark `<45` `0.004`, mean `82.3`.
- All new review PNGs have opaque alpha: `(255, 255)`.
- Library diff versus Stage8d: mean absolute pixel difference `0.438`, changed pixels `0.0388`.

## Remaining Gaps

- Stage8e makes a clearer Library light focal point, but the scene still needs stronger overall art direction and material richness.
- The current scene remains below target HD-2D reference quality in lighting hierarchy, atmosphere, and miniature depth.
