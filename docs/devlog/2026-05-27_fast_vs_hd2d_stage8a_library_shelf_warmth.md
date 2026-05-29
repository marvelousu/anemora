# Stage8a: Library Shelf Warmth

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Cycle goal:
- Continue after Stage7z by making the current Library public review image less empty and gray.
- Keep the Stage7y opaque-PNG guard and the Stage7z public exterior camera framing active for this and later review captures.

## Changes

- Added Stage8a batch entry points:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage8ALibraryShelfWarmthBatch`
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage8ALibraryShelfWarmthReferenceScreenshotsBatch`
- Added current-library-only shelf warmth props:
  - three rear shelf lip accents,
  - fifteen back-shelf book spine strips across upper/middle/lower rows,
  - two warm shelf glints,
  - two small table/page warmth accents.
- The Stage8a props are non-colliding and non-arrival paired-space landmarks so current-only visual detail does not affect TimeWindow pairing.
- Extended the public review capture predicate so `stage8a_library_shelf_warmth` uses the Stage7z low pulled-back exterior camera instead of the older obstructed exterior frame.
- Kept `SaveCameraPng()` opaque-alpha output guarded by validation.

## Validation

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage8ALibraryShelfWarmthBatch`
  - Exit 0.
  - Logs: `Logs\stage8a_library_shelf_warmth_validate.log`, `Logs\stage8a_library_shelf_warmth_validate_after_shadow_policy.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage8ALibraryShelfWarmthReferenceScreenshotsBatch`
  - Exit 0 with graphics enabled.
  - Log: `Logs\stage8a_library_shelf_warmth_capture.log`
  - Internal capture output: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260527_stage8a_library_shelf_warmth`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
  - Build finished successfully after tightening the Stage8a validation to tolerate existing renderer visibility policy.
  - Log: `Logs\stage8a_library_shelf_warmth_build_final.log`
- Built player smoke:
  - Log: `Logs\stage8a_library_shelf_warmth_smoke.log`
  - 24-second null-graphics smoke was stopped after startup; no `Exception`, `NullReference`, `MissingReference`, assertion, shader error/warning, or C# compile-error matches were found.

## Public Review

- New review directory: `docs/review/2026-05-27T04-34/`
- Public set contains only project screenshots. No external game reference images, comparison boards, or obstructed route-close diagnostic shots are included.
- Build exe: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Review/build note: フォルダごと起動

## Dark-Region Measurement

- `home.png`: dark `<45` `0.024`, central dark `<45` `0.023`, mean `61.8`.
- `Home_outside.png`: dark `<45` `0.073`, central dark `<45` `0.077`, mean `66.6`.
- `library.png`: dark `<45` `0.173`, central dark `<45` `0.033`, mean `62.3`.
- `plaza_01.png`: dark `<45` `0.042`, central dark `<45` `0.050`, mean `89.5`.
- `plaza_02_niro_in_shadow.png`: dark `<45` `0.056`, central dark `<45` `0.059`, mean `82.6`.
- `tw_current_aperture.png`: dark `<45` `0.037`, central dark `<45` `0.000`, mean `82.3`.
- All new review PNGs have opaque alpha: `(255, 255)`.

## Remaining Gaps

- Library is more readable than the previous empty-shelf pass, but the shelf richness and warm/cool hierarchy still remain far below target HD-2D reference quality.
- The exterior and plaza shots remain inspectable in this set, but they still lack reference-level miniature depth, atmosphere, and material contrast.
- Overall HD-2D quality remains substantially below the target reference.
