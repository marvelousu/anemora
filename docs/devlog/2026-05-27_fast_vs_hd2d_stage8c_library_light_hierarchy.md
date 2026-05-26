# Stage8c: Library Light Hierarchy

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Cycle goal:
- Continue after Stage8b by adding current-library warm floor pools and cool window washes.
- Keep the public-review anti-black safeguards active for every capture.

## Changes

- Added Stage8c batch entry points:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage8CLibraryLightHierarchyBatch`
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage8CLibraryLightHierarchyReferenceScreenshotsBatch`
- Added current-library-only light hierarchy planes:
  - four warm floor/back-shelf bounce planes,
  - two cool side window shaft planes,
  - two cool floor wash planes.
- The Stage8c planes are non-colliding and non-arrival paired-space landmarks.
- Extended the capture predicate so `stage8c_library_light_hierarchy` keeps the Stage7z exterior camera framing.

## Validation

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage8CLibraryLightHierarchyBatch`
  - Exit 0.
  - Log: `Logs\stage8c_library_light_hierarchy_validate.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage8CLibraryLightHierarchyReferenceScreenshotsBatch`
  - Exit 0 with graphics enabled.
  - Log: `Logs\stage8c_library_light_hierarchy_capture.log`
  - Internal capture output: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260527_stage8c_library_light_hierarchy`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
  - Build finished successfully.
  - Log: `Logs\stage8c_library_light_hierarchy_build.log`
- Built player smoke:
  - Log: `Logs\stage8c_library_light_hierarchy_smoke.log`
  - 24-second null-graphics smoke was stopped after startup; no `Exception`, `NullReference`, `MissingReference`, assertion, shader error/warning, or C# compile-error matches were found.

## Public Review

- New review directory: `docs/review/2026-05-27T05-36/`
- Public set contains only project screenshots. No external game reference images, comparison boards, or obstructed route-close diagnostic shots are included.
- Build exe: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Review/build note: フォルダごと起動

## Dark-Region Measurement

- `home.png`: dark `<45` `0.024`, central dark `<45` `0.023`, mean `61.8`.
- `Home_outside.png`: dark `<45` `0.073`, central dark `<45` `0.077`, mean `66.6`.
- `library.png`: dark `<45` `0.158`, central dark `<45` `0.033`, mean `64.0`.
- `plaza_01.png`: dark `<45` `0.042`, central dark `<45` `0.050`, mean `89.5`.
- `plaza_02_niro_in_shadow.png`: dark `<45` `0.056`, central dark `<45` `0.059`, mean `82.6`.
- `tw_current_aperture.png`: dark `<45` `0.037`, central dark `<45` `0.000`, mean `82.3`.
- All new review PNGs have opaque alpha: `(255, 255)`.
- Library diff versus Stage8b: mean absolute pixel difference `0.109`, changed pixels `0.0142`.

## Remaining Gaps

- Stage8c is structurally in place, but the captured visual change is small; the Library still needs stronger authored light/shadow contrast.
- The current scene remains below target HD-2D reference quality in atmosphere, material richness, and miniature depth.
