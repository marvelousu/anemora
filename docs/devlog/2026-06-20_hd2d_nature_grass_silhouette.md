# HD2D nature grass silhouette

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-20

## Summary

This cycle follows the nature canopy breakup pass and targets the foreground and midground grass/shrub silhouettes that still read as isolated blade triplets at wide review distance. The implementation keeps all existing vegetation placement anchors and traversal surfaces intact, then adds deterministic non-colliding low leaf clusters and extra grass blades to make the authored natural scatter read as small grass masses rather than primitive sticks.

The accepted packet is `docs/review/2026-06-20T10-50_nature_grass_silhouette_r1/`. It refreshes all-map review images after adding low grass silhouette detail to the shared grass tuft generator and to Phase2 vegetation volumes/groves across all current/past maps.

## Implementation

- Added `D`, `E`, and `LowLeafA` elements to every `CreateGrassTuft` output while preserving the existing `A`, `B`, and `C` authored grass blades and their placement anchors.
- Added `CreateAuthoredGrassBladeFeature` and `CreateAuthoredGrassLeafCluster` helpers so the new grass detail is non-colliding and does not increase arrival-counting landmarks.
- Added `Ch1Surface_CurrentNatureGrassSilhouette` and `Ch1Surface_PastNatureGrassSilhouette` generated pixel materials for restrained current/past grass accents without retinting shared crop, dry brush, or leaf materials.
- Added `BladeC` and `LowLeafA` to each Phase2 vegetation volume cluster.
- Added `UnderstoryC` and `UnderstoryBladeC` to each Phase2 vegetation grove.
- Added validation coverage through `ValidateChapter1GrassTuftSilhouetteForPrefix` plus Phase2 volume/grove validation loops so the new low-grass detail is guarded across representative maps and all Phase2 map instances.
- Rejected the first visual capture because the grass accent color was too bright in current-side wide shots. The accepted r2 capture darkens and desaturates the grass silhouette material while keeping the added geometry.

## Visual Review

- Review packet: `docs/review/2026-06-20T10-50_nature_grass_silhouette_r1/`
- Contact sheet: `docs/review/2026-06-20T10-50_nature_grass_silhouette_r1/00_contact_sheet.png`
- All 13 all-map captures were refreshed from `docs/devlog/screenshots/chapter1_all_maps_cycle05/` and copied into the review packet.
- Shotdiff vs `docs/review/2026-06-20T09-40_nature_canopy_breakup_r1/`:
  - Default 0.5% triage budget reported no over-budget frames, which is expected for low-grass detail.
  - Visual 0.02% triage reported 12/13 changed frames; `13_scene6_sideview_auto.png` remained unchanged at 0%.
- Strongest movement at the visual triage threshold: `01_a1_a2_current.png` 0.2534%, `02_a1_a2_past.png` 0.1376%, `03_b1_b3_current.png` 0.1271%, `07_d1_d3_current.png` 0.0931%, and `11_f1_f6_current.png` 0.0869%.
- Representative checks:
  - `01_a1_a2_current.png`: foreground house-edge natural scatter reads as clustered low foliage rather than three isolated blades.
  - `03_b1_b3_current.png`: plaza frontage and tree-base weeds gain small massing without adding new blocking objects.
  - `09_e1_e3_current.png` and `10_e1_e3_past.png`: farm/orchard grass and field edges gain subtle low-leaf breakup while preserving crop tone.
  - `11_f1_f6_current.png` and `12_f1_f6_past.png`: bridge and ruins-side brush receives extra low silhouettes without changing the side-view traversal frame.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`:
  - r1 log: `Logs/nature_grass_silhouette_validate_r1.log`, passed before the visual color correction.
  - r2 log: `Logs/nature_grass_silhouette_validate_r2.log`, passed after the accepted color correction.
  - `Fast VS house slice validation passed.`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
  - r1 log: `Logs/nature_grass_silhouette_capture_r1.log`, rejected visually as too bright.
  - r2 log: `Logs/nature_grass_silhouette_capture_r2.log`, accepted.
  - Output copied into `docs/review/2026-06-20T10-50_nature_grass_silhouette_r1/`.
- Shotdiff triage:
  - Default r2 output: `Logs/nature_grass_silhouette_shotdiff_r2.json`, 0 frames above 0.5%, sideview unchanged.
  - Visual r2 output: `Logs/nature_grass_silhouette_shotdiff_r2_visual.json`, 12/13 changed above 0.02%, sideview unchanged.
- EditMode renderer freeze: passed, 36/36.
  - XML: `Logs/nature_grass_silhouette_editmode_r1.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed.
  - Note: `-runTests` was executed without `-quit` so Unity emitted the XML test result.
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed.
  - Log: `Logs/nature_grass_silhouette_asset_validation_r1.log`
  - `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed.
  - Log: `Logs/nature_grass_silhouette_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
  - Build timestamp: 2026-06-20 10:48:47 local time.
  - Build size: 667648 bytes.
- Player smoke: 24 seconds, stopped manually after startup.
  - Log: `Logs/nature_grass_silhouette_player_smoke_r1.log`
  - Failure scan for `error CS|Exception|Assert|NullReference|MissingReference|Failed|Crash|Fatal`: 0 matches.

Unity batch side effects were reverted before staging. The authored implementation remains scoped to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`; review images are mirrored through the review/R2 flow rather than staged as source.

## Next

- Continue realistic nature uplift with a larger structural pass for near-field shrubs and tree-base clusters so the effect is visible at gameplay camera distance, not only all-map wide review distance.
- After low vegetation is stable, revisit distant/near nature color harmony so new authored detail reads cohesive rather than speckled.
