# HD2D imported nature scale

Date: 2026-06-23
Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`

## Summary

- Continued the nature readability pass by increasing the on-map imported nature model presence for trees, grass, bush, and plant companions.
- Rejected the experimental distant photo forest layer after review capture because it produced floating leaf patches on hills; the accepted change is limited to imported nature scale/tone readability.
- Existing CC0/Textured Nature tree models now read more clearly as pines/birches/branching trees from the wide review cameras, instead of being visually dominated by the older blocky silhouettes.
- Kept the renderer contract frozen; no URP renderer features were added, removed, or reordered.

## Authored Changes

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Raised the scale clamps for imported nature tree companions.
  - Increased deterministic grass, secondary grass, bush, and plant companion scale around tree bases.
  - Increased the textured nature scale multipliers for pine, birch, dead-tree, default textured, and non-textured imported nature paths.

## Review Evidence

- Review packet: `docs/review/2026-06-23T19-00_imported_nature_scale_r1/`
- Contact sheet: `docs/devlog/screenshots/chapter1_all_maps_cycle05/contact_imported_nature_scale_r1.png`
- Latest build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Full build path: `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Visual Triage

- Shotdiff versus the prior review packet changed 11/13 all-map images above the 0.5% review threshold.
- Largest changes:
  - `01_a1_a2_current.png`: 4.4715%
  - `03_b1_b3_current.png`: 3.6474%
  - `05_c1_c3_current.png`: 3.6465%
  - `02_a1_a2_past.png`: 2.6339%
  - `07_d1_d3_current.png`: 2.3661%
- `12_f1_f6_past.png` stayed below threshold at 0.3177%, and `13_scene6_sideview_auto.png` was unchanged.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed
  - Log: `Logs/imported_nature_scale_validate_r1.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed
  - Log: `Logs/imported_nature_scale_capture_r1.log`
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed
  - Log: `Logs/imported_nature_scale_asset_validation_r1.log`
  - Result: `[AssetValidation] OK`
- EditMode renderer freeze: passed
  - Result XML: `Logs/imported_nature_scale_editmode_r3.xml`
  - Result: 36 total / 36 passed / 0 failed
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed
  - Log: `Logs/imported_nature_scale_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Built player smoke: passed
  - Log: `Logs/imported_nature_scale_player_smoke_r1.log`
  - Pass marker: `ANEMORA_HOUSE_SLICE_SMOKE_PASS`
  - Failure scan: 0 fail markers after excluding the renderer-contract `error=<none>` field.

## Notes

- The trial `PhotoForestLayer` distant layer was intentionally discarded before this accepted cycle because it failed visual review with floating patches and weak composition improvement.
- `Assets/AddressableAssetsData/link.xml`, `Assets/AddressableAssetsData/link.xml.meta`, `Assets/Settings/DefaultVolumeProfile.asset`, and generated photo vegetation material drift were restored after Unity batch runs.
