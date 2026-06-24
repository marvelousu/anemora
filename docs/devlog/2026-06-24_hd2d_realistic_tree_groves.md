# HD2D realistic tree groves

Date: 2026-06-24
Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`

## Summary

- Continued the realistic nature uplift after the all-map foreground wild-grass pass, focusing on the build review concern that outdoor nature still did not read strongly enough as actual trees.
- Added deterministic tree grove clusters to every outdoor current/past map. Each map now receives four grove anchors, each with three imported tree models plus bush, grass, fallen-log, ground-cover, and authored grass-tuft companions.
- Kept the accepted path free of photo vegetation cards; this cycle uses existing CC0/Textured Nature models and authored mesh foliage only.
- Reduced current-side imported leaf/grass saturation and textured-leaf tone ranges so the new groves read less neon and more natural while preserving the current/past contrast.
- Added validation that every map has the grove root, expected imported tree/bush/grass/log counts, non-colliding objects, non-shadow renderers, and wide-camera visibility.

## Authored Changes

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Added `CreateChapter1RealisticTreeGrovesForOutdoorMaps(...)` and `CreateChapter1RealisticTreeGrove(...)`.
  - Added `RealisticTreeGroveClusterCount`, `RealisticTreeGroveTreesPerCluster`, and `RealisticTreeGroveVisibleMinimum`.
  - Added deterministic side/back grove offsets per map using existing area centers and hash-derived jitter.
  - Added `ValidateFastVsHd2dRealisticTreeGrovesAllMaps()` with root, layer, collider, renderer, count, and camera coverage checks.
  - Tuned current-side imported leaf/grass and textured leaf colors downward to avoid toy-bright foliage.

## Review Evidence

- Review packet: `docs/review/2026-06-24T16-19_realistic_tree_groves_r1/`
- Contact sheet: `docs/review/2026-06-24T16-19_realistic_tree_groves_r1/00_contact_sheet.png`
- Latest build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Full build path: `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Visual Triage

- Shotdiff versus `docs/review/2026-06-24T14-36_realistic_foreground_wild_grass_r1/` changed 12/13 all-map images above the 0.5% review threshold; the side-view image was unchanged.
- Largest all-map changes:
  - `01_a1_a2_current.png`: 13.7697%
  - `02_a1_a2_past.png`: 12.2821%
  - `03_b1_b3_current.png`: 12.0672%
  - `04_b1_b3_past.png`: 9.6331%
  - `06_c1_c3_past.png`: 8.8773%
- Visual read: every wide outdoor frame now has large, recognizable tree silhouettes and trunks rather than only low grass/nearfield dressing. Current-side foliage is still stylized low-poly, but less saturated after the tone pass.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed after the final tone pass
  - Log: `Logs/realistic_tree_groves_validate_r2.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed after the final tone pass
  - Log: `Logs/realistic_tree_groves_capture_r2.log`
  - Output: `docs/devlog/screenshots/chapter1_all_maps_cycle05/`
- EditMode renderer freeze: passed
  - Result XML: `Logs/realistic_tree_groves_editmode_r1.xml`
  - Result: 36 total / 36 passed / 0 failed
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed
  - Log: `Logs/realistic_tree_groves_assetvalidation_r1.log`
  - Result: `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed
  - Log: `Logs/realistic_tree_groves_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Built player smoke: passed
  - Log: `Logs/realistic_tree_groves_player_smoke_r1.log`
  - Pass marker: `ANEMORA_HOUSE_SLICE_SMOKE_PASS`

## Notes

- Review images are R2/viewer artifacts and are not staged into git. The local packet contains the latest all-map frames plus `00_contact_sheet.png`.
- Unity batch side effects must be restored before staging: `link.xml`, generated material/texture assets, Volume assets, and raw `docs/devlog/screenshots/chapter1_all_maps_cycle05/` overwrites remain working evidence only.
