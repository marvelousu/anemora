# HD2D nature tree base shrub

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-20

## Summary

This cycle follows the grass silhouette pass and targets the next visible nature gap: authored trees had richer crowns and grass tufts, but many tree bases still read as a trunk emerging from a flat patch. The accepted pass adds deterministic non-colliding base shrubs and low leaf sprays to normal authored trees, farm nut trees, and Phase2 vegetation groves so the natural assets read as rooted clusters rather than isolated vertical props.

The accepted packet is `docs/review/2026-06-20T11-55_nature_tree_base_shrub_r1/`. It keeps placement anchors, traversal surfaces, colliders, and renderer features unchanged.

## Implementation

- Added `CreateAuthoredTreeBaseShrub`, which creates deterministic `BaseShrubA`, `BaseShrubB`, and `BaseLeafSprayA` feature meshes using existing authored `LeafCluster` and `LeafSpray` mesh generators.
- Added the base shrub cluster to `CreateAuthoredLowPolyTree` using current/past `Ch1Surface_*NatureGrassSilhouette` accent materials.
- Added the same base shrub cluster to `CreateFarmNutTree` with a slightly larger scale so orchard trees gain grounded undergrowth.
- Added the base shrub cluster to every `CreateChapter1Phase2VegetationGrove` instance, which spreads the treatment across all outdoor current/past maps.
- Added validation coverage through `ValidateChapter1TreeBaseShrubForPrefix` for representative trees, farm nut trees, HouseExterior primary/secondary trees, and all Phase2 groves.
- Rejected r1 because the new geometry sat under the canopy and only 2/13 review frames changed at the visual threshold.
- Rejected r2 as still too localized after moving the base shrubs outward; it changed 5/13 frames but remained weighted toward HouseExterior/Farm.
- Accepted r3 after adding the same base-shrub structure to Phase2 groves, producing broad all-map movement without touching traversal.

## Visual Review

- Review packet: `docs/review/2026-06-20T11-55_nature_tree_base_shrub_r1/`
- Contact sheet: `docs/review/2026-06-20T11-55_nature_tree_base_shrub_r1/00_contact_sheet.png`
- All 13 all-map captures were refreshed from `docs/devlog/screenshots/chapter1_all_maps_cycle05/` and copied into the review packet.
- Shotdiff vs `docs/review/2026-06-20T10-50_nature_grass_silhouette_r1/`: accepted r3 changed 12/13 frames above the 0.02% visual threshold; `13_scene6_sideview_auto.png` remained unchanged at 0%.
- Strongest movement: `01_a1_a2_current.png` 0.1600%, `02_a1_a2_past.png` 0.1149%, `09_e1_e3_current.png` 0.0792%, `10_e1_e3_past.png` 0.0602%, and `05_c1_c3_current.png` 0.0361%.
- Representative checks:
  - `01_a1_a2_current.png` and `02_a1_a2_past.png`: house-side trees and nearby vegetation read as grounded clusters rather than trunk-on-plane props.
  - `03_b1_b3_current.png` and `04_b1_b3_past.png`: plaza-side vegetation gains low base mass without changing the building frontage.
  - `09_e1_e3_current.png` and `10_e1_e3_past.png`: orchard and farm groves gain root-zone foliage while preserving field readability.
  - `11_f1_f6_current.png` and `12_f1_f6_past.png`: ruins/bridge grove vegetation receives subtle low clusters while the side-view traversal frame remains unchanged.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`:
  - r1 log: `Logs/nature_tree_base_shrub_validate_r1.log`, passed before the r2/r3 visual correction.
  - r2 log: `Logs/nature_tree_base_shrub_validate_r2.log`, passed after the accepted Phase2 grove expansion.
  - `Fast VS house slice validation passed.`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
  - r1 log: `Logs/nature_tree_base_shrub_capture_r1.log`, rejected visually as hidden under tree canopies.
  - r2 log: `Logs/nature_tree_base_shrub_capture_r2.log`, rejected as too localized.
  - r3 log: `Logs/nature_tree_base_shrub_capture_r3.log`, accepted.
  - Output copied into `docs/review/2026-06-20T11-55_nature_tree_base_shrub_r1/`.
- Shotdiff triage:
  - r1 output: `Logs/nature_tree_base_shrub_shotdiff_r1_visual.json`, rejected with only 2/13 changed above 0.02%.
  - r2 output: `Logs/nature_tree_base_shrub_shotdiff_r2_visual.json`, rejected with 5/13 changed above 0.02%.
  - r3 output: `Logs/nature_tree_base_shrub_shotdiff_r3_visual.json`, accepted with 12/13 changed above 0.02% and sideview unchanged.
- EditMode renderer freeze: passed, 36/36.
  - XML: `Logs/nature_tree_base_shrub_editmode_r1.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed.
  - Note: `-runTests` was executed without `-quit` so Unity emitted the XML test result.
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed.
  - Log: `Logs/nature_tree_base_shrub_asset_validation_r1.log`
  - `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed.
  - Log: `Logs/nature_tree_base_shrub_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
  - Build timestamp: 2026-06-20 11:53:52 local time.
  - Build size: 667648 bytes.
- Player smoke: 24 seconds, stopped manually after startup.
  - Log: `Logs/nature_tree_base_shrub_player_smoke_r1.log`
  - Failure scan for `error CS|Exception|Assert|NullReference|MissingReference|Failed|Crash|Fatal`: 0 matches.

Unity batch side effects were reverted before staging. The authored implementation remains scoped to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`; review images are mirrored through the review/R2 flow rather than staged as source.

## Next

- Continue nature uplift by replacing the remaining flat cuboid brush patches near ruins, road verges, and map boundaries with authored low-poly shrub/ground-cover meshes.
- Keep the r3 threshold as the minimum bar for future nature cycles: if all-map movement is lower, first change structure or coverage rather than only material color.
