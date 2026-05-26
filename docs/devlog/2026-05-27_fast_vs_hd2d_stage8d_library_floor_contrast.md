# Stage8d: Library Floor Contrast

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Cycle goal:
- Continue after Stage8c by adding stronger current-library floor contact shadows and warm floor runs.
- Keep the public-review anti-black safeguards active for every capture.

## Changes

- Added Stage8d batch entry points:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage8DLibraryFloorContrastBatch`
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage8DLibraryFloorContrastReferenceScreenshotsBatch`
- Added current-library-only floor contrast accents:
  - four warm/paper floor runs around the long and side tables,
  - three table contact-shadow runs,
  - two back-shelf light-break strips.
- The Stage8d accents are non-colliding and non-arrival paired-space landmarks.
- Extended the capture predicate so `stage8d_library_floor_contrast` keeps the Stage7z exterior camera framing and opaque-alpha capture path.

## Validation

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage8DLibraryFloorContrastBatch`
  - Exit 0.
  - Log: `Logs\stage8d_library_floor_contrast_validate.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage8DLibraryFloorContrastReferenceScreenshotsBatch`
  - Exit 0 with graphics enabled.
  - Log: `Logs\stage8d_library_floor_contrast_capture.log`
  - Internal capture output: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260527_stage8d_library_floor_contrast`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
  - Build finished successfully.
  - Log: `Logs\stage8d_library_floor_contrast_build.log`
- Built player smoke:
  - Log: `Logs\stage8d_library_floor_contrast_smoke.log`
  - 24-second null-graphics smoke was stopped after startup; no `Exception`, `NullReference`, `MissingReference`, assertion, shader error/warning, or C# compile-error matches were found.

## Public Review

- New review directory: `docs/review/2026-05-27T05-55/`
- Public set contains only project screenshots. No external game reference images, comparison boards, or obstructed route-close diagnostic shots are included.
- Build exe: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Review/build note: フォルダごと起動

## Dark-Region Measurement

- `home.png`: dark `<45` `0.190`, central dark `<45` `0.221`, mean `61.8`.
- `Home_outside.png`: dark `<45` `0.197`, central dark `<45` `0.156`, mean `66.6`.
- `library.png`: dark `<45` `0.203`, central dark `<45` `0.105`, mean `64.2`.
- `plaza_01.png`: dark `<45` `0.067`, central dark `<45` `0.089`, mean `89.5`.
- `plaza_02_niro_in_shadow.png`: dark `<45` `0.088`, central dark `<45` `0.109`, mean `82.6`.
- `tw_current_aperture.png`: dark `<45` `0.114`, central dark `<45` `0.004`, mean `82.3`.
- All new review PNGs have opaque alpha: `(255, 255)`.
- Library diff versus Stage8c: mean absolute pixel difference `0.261`, changed pixels `0.0065`.

## Remaining Gaps

- Stage8d improves floor read and contact depth, but the visual delta is still modest at full-frame scale.
- The current scene remains below target HD-2D reference quality in lighting hierarchy, material richness, and miniature depth.
