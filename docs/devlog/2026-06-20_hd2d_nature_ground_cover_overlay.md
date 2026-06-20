# HD2D nature ground cover overlay

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-20

## Summary

This cycle follows the tree-base shrub pass and targets another remaining blockout artifact: many outdoor verge/brush areas still had flat cube-strip plant patches under otherwise authored vegetation. The accepted r2 pass adds deterministic non-colliding ground-cover overlays across HouseExterior, MiaHouse, AriaStreet, KaiaFarm, and Ruins so these edges read as low natural foliage rather than painted rectangles.

The review packet is `docs/review/2026-06-20T13-05_nature_ground_cover_overlay_r1/`. Existing placement anchors, route pads, colliders, and renderer features remain unchanged.

## Implementation

- Added `CreateAuthoredGroundCoverPatch`, a shared authored overlay made from two `LeafCluster` meshes, one `LeafSpray`, and two leaning `GrassBlade` meshes.
- Kept the legacy flat patch objects in place as ground tint/landmark continuity, then added `_GroundCover*` feature meshes above them with `countsForArrival=false`.
- Applied the overlay to HouseExterior front plant patches and cycle94 road-verge patches.
- Applied the overlay to MiaHouse underbrush and lower plant patches.
- Applied the overlay to AriaStreet lower verge beds.
- Applied the overlay to KaiaFarm field-end and right grass-edge patches.
- Applied the overlay to Ruins bridge brush patches and right settlement brush/clump patches.
- Added validation coverage through `ValidateChapter1GroundCoverPatchForPrefix` for representative A/C/D/E/F current and past map samples.
- Rejected r1 because the first pass changed only 3/13 review frames above the 0.02% visual threshold; C/D/E/F were still too subtle.
- Accepted r2 after raising the foliage mass and adding a second leaf cluster, producing visible low ground-cover in 9/13 review frames while keeping the side-view frame unchanged.

## Visual Review

- Review packet: `docs/review/2026-06-20T13-05_nature_ground_cover_overlay_r1/`
- Contact sheet: `docs/review/2026-06-20T13-05_nature_ground_cover_overlay_r1/00_contact_sheet.png`
- All 13 all-map captures were refreshed from `docs/devlog/screenshots/chapter1_all_maps_cycle05/` and copied into the review packet.
- Shotdiff baseline: `docs/review/2026-06-20T11-55_nature_tree_base_shrub_r1/`
- r1 visual triage: `Logs/nature_ground_cover_shotdiff_r1_visual.json`, rejected with 3/13 changed above 0.02%.
- r2 visual triage: `Logs/nature_ground_cover_shotdiff_r2_visual.json`, accepted with 9/13 changed above 0.02%.
- Strongest r2 movement: `01_a1_a2_current.png` 0.2992%, `02_a1_a2_past.png` 0.1773%, `07_d1_d3_current.png` 0.0806%, `12_f1_f6_past.png` 0.0536%, and `08_d1_d3_past.png` 0.0467%.
- `03_b1_b3_current.png`, `04_b1_b3_past.png`, and `13_scene6_sideview_auto.png` remained unchanged; B was not targeted in this cycle and the side-view traversal evidence stayed stable.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`:
  - r1 log: `Logs/nature_ground_cover_validate_r1.log`, passed before the visual rejection.
  - r2 log: `Logs/nature_ground_cover_validate_r2.log`, passed after the accepted structural increase.
  - `Fast VS house slice validation passed.`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
  - r1 log: `Logs/nature_ground_cover_capture_r1.log`, rejected visually as too subtle outside A.
  - r2 log: `Logs/nature_ground_cover_capture_r2.log`, accepted.
  - Output copied into `docs/review/2026-06-20T13-05_nature_ground_cover_overlay_r1/`.
- EditMode renderer freeze: passed.
  - XML: `Logs/nature_ground_cover_editmode_r1.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed.
  - Note: `-runTests` was executed without `-quit` so Unity emitted the XML test result.
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed.
  - Log: `Logs/nature_ground_cover_asset_validation_r1.log`
  - `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed.
  - Log: `Logs/nature_ground_cover_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
  - Build timestamp: 2026-06-20 13:01:57 local time.
  - Build size: 667648 bytes.
- Player smoke: 24 seconds, stopped manually after startup.
  - Log: `Logs/nature_ground_cover_player_smoke_r1.log`
  - Failure scan for `error CS|Exception|Assert|NullReference|MissingReference|Failed|Crash|Fatal`: 0 matches.

Unity batch side effects were reverted before staging. The authored implementation remains scoped to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`; review images are mirrored through the review/R2 flow rather than staged as source.

## Next

- Continue nature uplift by adding medium-height natural silhouettes for the still-unchanged B/plaza map and remaining farm/ruins edge cases.
- Start a dedicated bridge traversal/readability cycle after the next nature visual pass so the visual bridge and actual crossing affordance are verified together.
