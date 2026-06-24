# HD2D scenic relief backdrop

Date: 2026-06-24
Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`

## Summary

- Continued the distant-vista quality push after the realistic-depth panorama pass.
- Added deterministic ScenicRelief forest/ridge meshes to every outdoor current/past distant panorama root.
- Built the new layer as actual 3D mesh geometry with textured materials so it parallax-composes with the existing low-poly panorama instead of acting as a flat sky image.
- Kept the renderer feature set frozen. No URP renderer feature was added, removed, or reordered.
- Preserved the accepted nature path: authored/generated Unity meshes plus existing CC0/Textured Nature assets. The rejected photo vegetation card path remains out of the accepted build.

## Authored Changes

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Added a two-layer ScenicRelief pass for distant front woodline and high ridge silhouettes.
  - Added deterministic placement per area, time state, layer, and span without `Random`, `Time`, or `DateTime`.
  - Added ridge/fold mesh generation with textured materials for needle canopy and rock-strata breakup.
  - Extended distant panorama validation to require ScenicRelief mesh count, material texture presence, and wide-camera visibility.

## Review Evidence

- Review packet: `docs/review/2026-06-24T20-52_scenic_relief_backdrop_r1/`
- Contact sheet: `docs/review/2026-06-24T20-52_scenic_relief_backdrop_r1/00_contact_sheet.png`
- Latest build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Full build path: `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Visual Triage

- Shotdiff versus `docs/review/2026-06-24T17-05_realistic_depth_panorama_r1/` at the normal 0.5% review threshold: 2/14 changed.
  - `00_contact_sheet.png`: 0.6977%
  - `12_f1_f6_past.png`: 0.6259%
- Visual-threshold shotdiff at 0.05%: 7/14 changed.
  - D/E/F outdoor views now show stronger upper-horizon ridge/forest relief.
  - A/B/C remain mostly unchanged in pixel metrics because the new far layer is largely hidden by the accepted near tree-grove mass and existing distant panorama bands.
- Visual read: this cycle improves the high-horizon read in the most open review angles, but it is not the final nature-quality ceiling. The next graphics cycle should target close/mid natural assets and tree silhouettes directly.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed
  - Log: `Logs/scenic_relief_validate_r4.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed
  - Log: `Logs/scenic_relief_capture_r4.log`
  - Output: `docs/devlog/screenshots/chapter1_all_maps_cycle05/`
- EditMode renderer freeze: passed
  - Result XML: `Logs/scenic_relief_editmode_r2.xml`
  - Result: 36 total / 36 passed / 0 failed
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed
  - Log: `Logs/scenic_relief_assetvalidation_r1.log`
  - Result: `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed
  - Log: `Logs/scenic_relief_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Built player smoke: passed
  - Log: `Logs/scenic_relief_player_smoke_r2.log`
  - Pass marker: `ANEMORA_HOUSE_SLICE_SMOKE_PASS`

## Notes

- The first implementation named the new objects `DistantVista_ScenicBackdrop`, which collided with the legacy flat-backdrop guard. The accepted path renames the runtime objects and materials to `DistantVista_ScenicRelief`.
- Review images are R2/viewer artifacts and are not staged into git. The local packet contains the latest all-map frames plus `00_contact_sheet.png`.
- Unity batch side effects must be restored before staging: `link.xml`, generated material/texture assets, and raw `docs/devlog/screenshots/chapter1_all_maps_cycle05/` overwrites remain working evidence only.
