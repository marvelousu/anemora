# Stage8b: Library Shelf Density

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Cycle goal:
- Continue after Stage8a by increasing visible book density and color mass in the current Library review image.
- Preserve the public-review anti-black safeguards: opaque PNG output and the Stage7z low pulled-back exterior camera.

## Changes

- Added Stage8b batch entry points:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage8BLibraryShelfDensityBatch`
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage8BLibraryShelfDensityReferenceScreenshotsBatch`
- Added current-library-only shelf density props:
  - seven larger back-shelf book/page/warm clusters,
  - five left/right side-shelf book bands,
  - three table book/page stacks.
- Stage8b props are non-colliding and non-arrival paired-space landmarks, so they do not alter TimeWindow arrival matching.
- Extended the capture predicate so `stage8b_library_shelf_density` keeps the Stage7z exterior camera framing.

## Validation

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage8BLibraryShelfDensityBatch`
  - Exit 0.
  - Log: `Logs\stage8b_library_shelf_density_validate.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage8BLibraryShelfDensityReferenceScreenshotsBatch`
  - Exit 0 with graphics enabled.
  - Log: `Logs\stage8b_library_shelf_density_capture.log`
  - Internal capture output: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260527_stage8b_library_shelf_density`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
  - Build finished successfully.
  - Log: `Logs\stage8b_library_shelf_density_build.log`
- Built player smoke:
  - Log: `Logs\stage8b_library_shelf_density_smoke.log`
  - 24-second null-graphics smoke was stopped after startup; no `Exception`, `NullReference`, `MissingReference`, assertion, shader error/warning, or C# compile-error matches were found.

## Public Review

- New review directory: `docs/review/2026-05-27T05-18/`
- Public set contains only project screenshots. No external game reference images, comparison boards, or obstructed route-close diagnostic shots are included.
- Build exe: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Review/build note: フォルダごと起動

## Dark-Region Measurement

- `home.png`: dark `<45` `0.024`, central dark `<45` `0.023`, mean `61.8`.
- `Home_outside.png`: dark `<45` `0.073`, central dark `<45` `0.077`, mean `66.6`.
- `library.png`: dark `<45` `0.158`, central dark `<45` `0.033`, mean `63.8`.
- `plaza_01.png`: dark `<45` `0.042`, central dark `<45` `0.050`, mean `89.5`.
- `plaza_02_niro_in_shadow.png`: dark `<45` `0.056`, central dark `<45` `0.059`, mean `82.6`.
- `tw_current_aperture.png`: dark `<45` `0.037`, central dark `<45` `0.000`, mean `82.3`.
- All new review PNGs have opaque alpha: `(255, 255)`.
- Library diff versus Stage8a: mean absolute pixel difference `2.008`, changed pixels `0.0395`.

## Remaining Gaps

- Library shelves now read less empty, but the scene still lacks reference-level warm/cool light hierarchy, volumetric depth, and authored prop richness.
- The book bands are an incremental visibility layer, not a final art pass.
- Overall HD-2D quality remains substantially below the target reference.
