# HD2D photo vegetation canopy cards

Date: 2026-06-23
Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`

## Summary

- Continued the nature uplift by adding photo branch canopy cards to authored tree crowns.
- Switched the tree branch detail cards to branch cutout textures instead of low understory textures, so the crowns read more like trees from the wide review camera.
- Tuned the card scale/tint after the first pass looked too bright and leaf-blob-like; the accepted review capture is the darker r3 set.
- Kept the renderer contract frozen; no URP renderer features were added, removed, or reordered.

## Authored Changes

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Added `_PhotoCanopyBranchCardA`, `_PhotoCanopyBranchCardB`, and `_PhotoCanopyBranchCardC` to photo vegetation tree groups.
  - Added deterministic current/past canopy tinting and reused the selected branch cutout texture for crown cards.
  - Extended photo vegetation validation to require the new generated canopy card names for both current and past prefixes.

## Review Evidence

- Review packet: `docs/review/2026-06-23T15-07_photo_vegetation_canopy_cards_r1/`
- Contact sheet: `docs/devlog/screenshots/chapter1_all_maps_cycle05/00_contact_sheet_photo_vegetation_canopy_cards_r3.png`
- Latest build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed
  - Log: `Logs/photo_vegetation_canopy_cards_validate_r2.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed
  - Log: `Logs/photo_vegetation_canopy_cards_capture_r3.log`
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed
  - Log: `Logs/photo_vegetation_canopy_cards_asset_validation_r1.log`
- EditMode renderer freeze: passed
  - Result XML: `Logs/photo_vegetation_canopy_cards_editmode_r1.xml`
  - Result: 36 total / 36 passed / 0 failed
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed
  - Log: `Logs/photo_vegetation_canopy_cards_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Built player smoke: passed
  - Log: `Logs/photo_vegetation_canopy_cards_player_smoke_r1.log`
  - Error scan: 0 hits for exception, shader error, compute shader failure, and failed/error patterns.

## Notes

- `Assets/Settings/DefaultVolumeProfile.asset` was restored before the accepted build/capture because its overrideState-only changes were unrelated Unity serialization drift for this nature cycle.
- `Assets/AddressableAssetsData/link.xml` and `.meta` were restored after Unity batch runs to remove Addressables batch side effects.
