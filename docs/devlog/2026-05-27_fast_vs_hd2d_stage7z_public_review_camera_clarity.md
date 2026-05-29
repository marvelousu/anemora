# Stage7z: Public Review Camera Clarity

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Cycle goal:
- Continue after the Stage7y alpha fix by reducing the exterior review shot's roof/wall obstruction.
- Keep public review images readable while acknowledging the scene is still below target HD-2D quality.

## Changes

- Added Stage7z batch entry points:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage7PublicReviewCameraClarityBatch`
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage7PublicReviewCameraClarityReferenceScreenshotsBatch`
- Added a Stage7z capture mode keyed by `stage7z_public_review_camera_clarity`.
- Reframed the public Exterior review capture from the previous close, high roof-dominant view to a lower, pulled-back camera:
  - camera offset: `(1.25, 1.85, -8.05)`
  - look offset: `(0.10, 0.72, 0.38)`
  - FOV: `30`
- Kept the opaque-PNG capture guard from Stage7y active for this cycle.

## Validation

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage7PublicReviewCameraClarityBatch`
  - Exit 0.
  - Log: `Logs\stage7z_public_review_camera_clarity_validate.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage7PublicReviewCameraClarityReferenceScreenshotsBatch`
  - Exit 0 with graphics enabled.
  - Log: `Logs\stage7z_public_review_camera_clarity_capture.log`
  - Internal capture output: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260527_stage7z_public_review_camera_clarity`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
  - Build finished successfully.
  - Log: `Logs\stage7z_public_review_camera_clarity_build.log`
- Built player smoke:
  - Log: `Logs\stage7z_public_review_camera_clarity_smoke.log`
  - 24-second null-graphics smoke was stopped after startup; no `Exception`, `NullReference`, `MissingReference`, assertion, shader error/warning, or C# compile-error matches were found.

## Public Review

- New review directory: `docs/review/2026-05-27T03-57/`
- Public set contains only project screenshots. No external game reference images, comparison boards, or obstructed route-close diagnostic shots are included.
- Build exe: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Review/build note: フォルダごと起動

## Dark-Region Measurement

- `home.png`: dark `<45` `0.147`, central dark `<45` `0.170`, mean `64.9`.
- `Home_outside.png`: dark `<45` `0.158`, central dark `<45` `0.146`, mean `69.7`.
- `library.png`: dark `<45` `0.207`, central dark `<45` `0.076`, mean `64.4`.
- `plaza_01.png`: dark `<45` `0.060`, central dark `<45` `0.077`, mean `92.8`.
- `plaza_02_niro_in_shadow.png`: dark `<45` `0.080`, central dark `<45` `0.093`, mean `85.5`.
- `tw_current_aperture.png`: dark `<45` `0.107`, central dark `<45` `0.002`, mean `85.9`.
- All new review PNGs have opaque alpha: `(255, 255)`.

## Remaining Gaps

- The exterior shot is more inspectable, but it still reads as a large flat facade/roof mass rather than a strong miniature HD-2D composition.
- Library remains readable in the center but still lacks reference-level warm/cool hierarchy, material richness, and atmospheric depth.
- Overall HD-2D quality remains substantially below the target reference.
