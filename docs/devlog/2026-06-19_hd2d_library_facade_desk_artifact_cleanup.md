# HD2D library facade and desk artifact cleanup

Date: 2026-06-19
Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`

## Summary

This cycle addresses two review-image issues from the current environment uplift line:

- `04_plaza_context.png` still showed a pale library-front haze after the earlier cleanup.
- `06_library_context.png` still showed a bright, stray-looking desk/table texture in the current library.

The accepted candidate is `docs/review/2026-06-19T12-04_library_facade_desk_cleanup_r9/`. The plaza facade haze is removed from the current library approach, and the library table hotspot is reduced enough that the remaining book/table cue reads as authored scene dressing rather than a blown-out texture plate.

## Implementation

- Added a focused review-feedback cleanup pass in `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`.
- Disabled the rejected current-plaza facade haze/light helper planes that were washing the library front.
- Disabled rejected current-library table/floor/page/lamp-core overlays that read as stray bright texture slabs.
- Kept the authored current library desk books present, but changed their current-side page/spine materials away from the high-value sign/red materials.
- Reduced `FastVS_HD2D_PhaseCAlpha_LibraryWarmEmissiveLight` from the previous high-intensity table hotspot to a restrained warm cue.
- Added validation coverage so the rejected helper planes remain inactive/removed and the window-light cookie no longer projects a gridded texture onto desks/floor.

## Review Images

- `docs/review/2026-06-19T12-04_library_facade_desk_cleanup_r9/contact_sheet.png`
- Key frames:
  - `docs/review/2026-06-19T12-04_library_facade_desk_cleanup_r9/04_plaza_context.png`
  - `docs/review/2026-06-19T12-04_library_facade_desk_cleanup_r9/06_library_context.png`

Earlier rejected comparison packets were kept locally for traceability:

- `docs/review/2026-06-19T10-43_library_facade_desk_cleanup_r2/`
- `docs/review/2026-06-19T11-02_library_facade_desk_cleanup_r3/`
- `docs/review/2026-06-19T11-14_library_facade_desk_cleanup_r4/`
- `docs/review/2026-06-19T11-24_library_facade_desk_cleanup_r5/`
- `docs/review/2026-06-19T11-35_library_facade_desk_cleanup_r6/`
- `docs/review/2026-06-19T11-44_library_facade_desk_cleanup_r7/`
- `docs/review/2026-06-19T11-52_library_facade_desk_cleanup_r8/`

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage7PortalFacadeBrightnessReferenceScreenshotsBatch`: passed.
  - Log: `Logs/library_facade_desk_cleanup_capture_r9.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed.
  - Log: `Logs/library_facade_desk_cleanup_validate_r6.log`
- EditMode renderer freeze: passed, 36/36.
  - XML: `Logs/library_facade_desk_cleanup_editmode_r3.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed.
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed.
  - Log: `Logs/library_facade_desk_cleanup_asset_validation_r2.log`
  - `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed.
  - Log: `Logs/library_facade_desk_cleanup_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- R2 review upload: passed for r1-r9.
  - Accepted packet r9 uploaded 12 files for `wip-hd2d-point15-recovery-20260612/2026-06-19T12-04_library_facade_desk_cleanup_r9`.
  - Final branch manifest after uploading local comparison packets lists 666 paths.
- `tools/review/validate-devlog-review-sync.ps1`: passed.

Unity batch side effects were reverted after each batch; the intended tracked change for this cycle is the authored setup file plus this devlog/index update.
