# Stage7y: Exterior/Library Public Legibility and Opaque Review PNGs

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Cycle goal:
- Respond to the public-review report that the review page was showing black images.
- Keep moving the Exterior and Library public-review candidates toward inspectable lighting without presenting the result as final HD-2D quality.

## Root Cause

- The latest public original PNGs had visible RGB content but mostly transparent alpha.
- The viewer thumbnail pipeline composites PNG alpha against black, so `thumbs/*.webp` became almost fully black even when the original PNG RGB looked readable locally.
- Example before this cycle: public `2026-05-27T02-48/plaza_01.png` original measured `mean=89.5`, but its public thumbnail measured `mean=19.7` and `dark45=0.791`.

## Changes

- Updated `SaveCameraPng()` so review captures force all output PNG alpha values to `255`.
- Added screenshot output validation that rejects any saved PNG with non-opaque alpha.
- Normalized existing tracked `docs/review/**/*.png` files with transparent alpha to opaque alpha so previously generated public thumbnails can be rebuilt from non-transparent source images.
- Removed 11 tracked historical `docs/review` PNGs that still exceeded the public-review dark-region scan after alpha normalization, and removed their `index.md` image links where present.
- Continued Stage7y Exterior/Library legibility work:
  - Lifted realtime outdoor side/floor shade colors.
  - Reduced Exterior and Library review shadow strength.
  - Reduced outdoor receiver shadow/texture strength.
  - Raised Exterior and Library realtime ambient and main light values.
  - Lifted outdoor sprite-card world light and reduced sprite world shadow receive.
  - Added Stage7y validation and capture batch entry points.

## Validation

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage7ExteriorLibraryPublicLegibilityBatch`
  - Exit 0.
  - Log: `Logs\stage7y_alpha_opaque_validate.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage7ExteriorLibraryPublicLegibilityReferenceScreenshotsBatch`
  - Exit 0 with graphics enabled.
  - Log: `Logs\stage7y_alpha_opaque_capture.log`
  - Internal capture output: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260527_stage7y_exterior_library_public_legibility`
- Local alpha scan after normalization:
  - `docs/review/**/*.png`: `alpha_not_opaque=0`.
- Tracked/staged public dark-region scan:
  - `dark_bad=0` for the scan gates used here (`overall dark <45 >= 0.45` or `central dark <45 >= 0.30`).
- Local thumbnail conversion check:
  - `docs/review/2026-05-27T03-26/plaza_01.png` -> WebP test measured `mean=89.5`, `dark45=0.065`.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
  - Build finished successfully.
  - Log: `Logs\stage7y_alpha_opaque_build.log`
- Built player smoke:
  - Log: `Logs\stage7y_alpha_opaque_smoke.log`
  - 24-second null-graphics smoke was stopped after startup; no `Exception`, `NullReference`, `MissingReference`, assertion, shader error/warning, or C# compile-error matches were found.

## Public Review

- New review directory: `docs/review/2026-05-27T03-26/`
- Public set contains only project screenshots. No external game reference images, comparison boards, or obstructed route-close diagnostic shots are included.
- Build exe: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Review/build note: フォルダごと起動

## Dark-Region Measurement

- `home.png`: dark `<45` `0.190`, central dark `<45` `0.213`, mean `61.8`.
- `Home_outside.png`: dark `<45` `0.234`, central dark `<45` `0.212`, mean `67.3`.
- `library.png`: dark `<45` `0.222`, central dark `<45` `0.096`, mean `61.8`.
- `plaza_01.png`: dark `<45` `0.067`, central dark `<45` `0.086`, mean `89.5`.
- `plaza_02_niro_in_shadow.png`: dark `<45` `0.088`, central dark `<45` `0.106`, mean `82.6`.
- `tw_current_aperture.png`: dark `<45` `0.114`, central dark `<45` `0.005`, mean `82.3`.

## Remaining Gaps

- This cycle fixes the black thumbnail/public-review artifact and improves inspectability, but it does not close the HD-2D gap.
- Exterior composition is still dominated by roof/facade mass and needs a better authored review camera and outdoor depth structure.
- Library still needs richer material response, shelf readability, and depth staging.
- Overall HD-2D quality remains substantially below the target reference.
