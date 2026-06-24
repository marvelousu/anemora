# HD2D realistic foreground wild grass

Date: 2026-06-24
Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`

## Summary

- Continued the realistic nature uplift after the under-canopy pass, focusing on the weak lower-edge and foreground nature visible in all-map wide review frames.
- Added a deterministic foreground wild-grass layer for every outdoor current/past map, using existing CC0 imported grass/plant meshes combined with authored ground-cover patches and grass tufts.
- Kept the accepted path free of photo vegetation cards because the previous build review showed dark card artifacts; this cycle uses model and authored mesh silhouettes only.
- Added validation that every map gets the new root, no colliders are introduced, the expected imported grass/plant and authored low-cover counts exist, and the wide review camera sees the layer.

## Authored Changes

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Added `CreateChapter1RealisticForegroundWildGrassForOutdoorMaps(...)` and deterministic per-area foreground wild-grass clusters.
  - Each current/past outdoor map now receives 9 foreground clusters with two imported grass fans, one imported leaf plant, one authored ground-cover patch, and two grass tufts per cluster.
  - Reused existing unlit imported nature materials and authored grass/ground-cover mesh helpers to avoid black-card artifacts.
  - Added `ValidateFastVsHd2dRealisticForegroundWildGrassAllMaps()` to enforce all-map coverage, render-layer policy, no-collider policy, non-shadow renderers, and wide-camera visibility.

## Review Evidence

- Review packet: `docs/review/2026-06-24T14-36_realistic_foreground_wild_grass_r1/`
- Contact sheet: `docs/review/2026-06-24T14-36_realistic_foreground_wild_grass_r1/00_contact_sheet.png`
- Latest build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Full build path: `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Visual Triage

- Shotdiff versus `docs/review/2026-06-24T02-33_realistic_nature_under_canopy_r1/` changed 2/13 all-map images above the 0.5% review threshold; the contact sheet size differs because this packet rebuilt the sheet layout.
- Largest all-map changes:
  - `09_e1_e3_current.png`: 0.6982%
  - `03_b1_b3_current.png`: 0.5049%
  - `04_b1_b3_past.png`: 0.4600%
  - `07_d1_d3_current.png`: 0.4565%
  - `11_f1_f6_current.png`: 0.4219%
- Visual read: the lower foreground band now has more grass and low leaf silhouettes without reintroducing dark photo-card blotches. Remaining graphics work should continue replacing coarse tree/canopy silhouettes with higher-quality authored or external vegetation assets, especially in far/background masses.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed
  - Log: `Logs/realistic_foreground_wild_grass_validate_r1.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed
  - Log: `Logs/realistic_foreground_wild_grass_capture_r1.log`
  - Output: `docs/devlog/screenshots/chapter1_all_maps_cycle05/`
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed
  - Log: `Logs/realistic_foreground_wild_grass_assetvalidation_r1.log`
  - Result: `[AssetValidation] OK`
- EditMode renderer freeze: passed
  - Result XML: `Logs/realistic_foreground_wild_grass_editmode_r2.xml`
  - Result: 36 total / 36 passed / 0 failed
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed
  - Log: `Logs/realistic_foreground_wild_grass_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Built player smoke: passed
  - Log: `Logs/realistic_foreground_wild_grass_player_smoke_r1.log`
  - Pass marker: `ANEMORA_HOUSE_SLICE_SMOKE_PASS`
  - Failure scan: 0 meaningful failure markers.

## Notes

- Review images are R2/viewer artifacts and are not staged into git. The local packet contains the latest all-map frames plus `00_contact_sheet.png`.
- Unity batch side effects must be restored before staging: `link.xml`, generated material/texture assets, and raw `docs/devlog/screenshots/chapter1_all_maps_cycle05/` overwrites remain working evidence only.
