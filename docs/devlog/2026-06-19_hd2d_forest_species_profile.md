# HD2D forest species profile

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-19

## Summary

This cycle follows the realistic nature depth pass. The prior cycle made the forest ring visible, but the far-tree profile still read too much like a single green wall with hard branch strokes. This pass adds deterministic species grouping and softens the distant branch tone so the far slopes read more like mixed broadleaf/conifer forest.

The accepted packet is `docs/review/2026-06-19T22-22_forest_species_profile_r1/`. r1 was rejected as a plateau because only one wide frame crossed the 0.05% shotdiff threshold. r2 moves the species clusters farther forward, scales them up, and gives the broadleaf/conifer materials enough separation to show in wide review images.

## Implementation

- Added one deterministic `DistantVista_NaturalSpeciesCluster` mesh per panorama segment.
- Split the new species layer into broadleaf and conifer profiles using area/index-derived hash variation.
- Added dedicated `Ch1Distant_*NaturalSpeciesBroadleaf`, `Ch1Distant_*NaturalSpeciesConifer`, and `Ch1Distant_*NaturalBranchTrace` materials.
- Changed distant branch traces from dark trunk material to muted green-brown branch trace material so the far forest reads less like dead sticks.
- Extended validation to require species-cluster generation and camera visibility while preserving the frozen renderer-feature contract.

## Visual Review

- Review packet: `docs/review/2026-06-19T22-22_forest_species_profile_r1/`
- Contact sheet: `docs/review/2026-06-19T22-22_forest_species_profile_r1/00_contact_sheet.png`
- All 13 all-map captures were refreshed from `docs/devlog/screenshots/chapter1_all_maps_cycle05/` and copied into the review packet after r2.
- Shotdiff vs `docs/review/2026-06-19T20-53_realistic_nature_depth_r1/`: r2 changed 10/12 wide current/past frames plus the contact sheet. Strongest movement was `11_f1_f6_current.png` at 0.7349%, `08_d1_d3_past.png` at 0.7256%, and `10_e1_e3_past.png` at 0.4283%.
- Representative checks:
  - `01_a1_a2_current.png`: house-exterior far shore gains visible broadleaf clusters without reopening void gaps.
  - `08_d1_d3_past.png`: route-side far slope shows mixed forest profiles instead of a single repeated band.
  - `11_f1_f6_current.png`: final-route backdrop gains brighter crown grouping while the lake and mid-ring remain readable.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed.
  - Log: `Logs/distant_forest_species_profile_validate_r2.log`
  - `Fast VS house slice validation passed.`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
  - Log: `Logs/distant_forest_species_profile_capture_r2.log`
  - Output copied into `docs/review/2026-06-19T22-22_forest_species_profile_r1/`.
- Shotdiff triage: passed as a visible r2 change after the r1 plateau.
  - Output: `Logs/shotdiff/forest_species_profile_vs_realistic_nature_r2/`
- EditMode renderer freeze: passed, 36/36.
  - XML: `Logs/distant_forest_species_profile_editmode_r1.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed.
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed.
  - Log: `Logs/distant_forest_species_profile_asset_validation_r1.log`
  - `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed.
  - Log: `Logs/distant_forest_species_profile_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
  - Build timestamp: 2026-06-19 22:51:10 local time.
- Player smoke: 24 seconds, stopped manually after startup.
  - Log: `Logs/distant_forest_species_profile_player_smoke_r1.log`
  - Case-sensitive failure scan for `Error|Exception|Assert|NullReference|MissingReference|Failed|RenderGraph`: 0 matches.
- R2 review upload: passed.
  - Uploaded 17 files.
  - Manifest: `manifests/wip-hd2d-point15-recovery-20260612.json` lists 802 paths.
- `tools/review/validate-devlog-review-sync.ps1`: passed.

Unity batch side effects will be reverted before staging. The authored implementation remains scoped to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`; review images are mirrored through the review/R2 flow rather than staged as source.

## Next

- Continue reducing far-hill tiling and texture noise so the improved tree profiles sit on more natural terrain.
- Add area-specific forest silhouettes so A/B/C/D/E/F do not all share the same mixed-forest rhythm.
- Resume bridge traversal and walkable-affordance checks after this visual line has another stable review pass.
