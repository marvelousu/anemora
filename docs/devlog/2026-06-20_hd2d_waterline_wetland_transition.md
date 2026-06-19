# HD2D waterline wetland transition

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-20

## Summary

This cycle follows the distant slope-contour pass. The panorama now has stronger terrain relief, but several wide captures still showed broad flat gray water and shore ribbons at the map edge. This pass adds authored low-poly wetland transition geometry to the existing waterline breakup layer so lakes and rivers read as bordered natural spaces rather than exposed void-like bands.

The accepted packet is `docs/review/2026-06-20T02-12_waterline_wetland_transition_r1/`. It keeps renderer features frozen while adding deterministic `WaterlineBreakup_WetlandMat` meshes, current/past `Ch1Distant_*WaterlineBreakupWetland` materials, validation count coverage, and camera visibility coverage.

## Implementation

- Added `WaterlineBreakupWetlandMatCount` and a wetland visible minimum so the new waterline layer is enforced by validation.
- Added `CreateWaterlineBreakupWetlandMatMesh`, a low double-sided wetland grid with irregular chipped edges, hummock lifts, and shallow meander cuts.
- Extended `CreateChapter1PhaseIWaterlineBreakup` to add ten deterministic wetland mats per outdoor current/past map.
- Added `EnsureWaterlineBreakupWetlandMaterial` with current/past `Ch1Distant_*WaterlineBreakupWetland` textured grass materials.
- Extended waterline validation to require wetland textures, wetland mesh counts, textured material use, and camera visibility.
- Rejected r1 placement as too subtle on past-side captures; accepted r2 after alternating near/far radius bands, larger mats, and stronger current/past material separation.
- Kept renderer features, placement determinism, non-colliding geometry, and authored-file scope unchanged.

## Visual Review

- Review packet: `docs/review/2026-06-20T02-12_waterline_wetland_transition_r1/`
- Contact sheet: `docs/review/2026-06-20T02-12_waterline_wetland_transition_r1/00_contact_sheet.png`
- All 13 all-map captures were refreshed from `docs/devlog/screenshots/chapter1_all_maps_cycle05/` and copied into the review packet.
- Shotdiff vs `docs/review/2026-06-20T00-59_distant_slope_contours_r1/`: all 12 wide current/past frames changed, while the side-view stability frame remained unchanged. Strongest movement was `07_d1_d3_current.png` at 1.4776%, `01_a1_a2_current.png` at 1.4433%, `09_e1_e3_current.png` at 1.3799%, and `05_c1_c3_current.png` at 1.3142%.
- Representative checks:
  - `03_b1_b3_current.png`: plaza/library background gains visible green wetland shelves around the waterline without reintroducing the white haze.
  - `04_b1_b3_past.png`: past-side waterline gains ochre wetland transitions instead of only gray water bands.
  - `07_d1_d3_current.png`: route-side lake now has a wetland edge between the playable map and forest ring.
  - `11_f1_f6_current.png`: final-route lake gains small natural mats that break the broad flat water surface without covering buildings or route geometry.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed.
  - r1 log: `Logs/waterline_wetland_validate_r1.log`
  - r2 log: `Logs/waterline_wetland_validate_r2.log`
  - `Fast VS house slice validation passed.`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
  - r2 log: `Logs/waterline_wetland_capture_r2.log`
  - Output copied into `docs/review/2026-06-20T02-12_waterline_wetland_transition_r1/`.
- Shotdiff triage:
  - r1 output: `Logs/waterline_wetland_shotdiff_r1/`, rejected because only five wide frames crossed the 0.05% threshold and past-side movement remained too weak.
  - r2 output: `Logs/waterline_wetland_shotdiff_r2/`, accepted with all 12 wide current/past frames changed and strongest movement at 1.4776%.
- EditMode renderer freeze: passed, 36/36.
  - XML: `Logs/waterline_wetland_editmode_r2.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed.
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed.
  - Log: `Logs/waterline_wetland_asset_validation_r2.log`
  - `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed.
  - Log: `Logs/waterline_wetland_build_r2.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
  - Build timestamp: 2026-06-20 02:10:03 local time.
- Player smoke: 24 seconds, stopped manually after startup.
  - Log: `Logs/waterline_wetland_player_smoke_r2.log`
  - Case-sensitive failure scan for `Error|Exception|Assert|NullReference|MissingReference|Failed|RenderGraph`: 0 matches.

Unity batch side effects will be reverted before staging. The authored implementation remains scoped to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`; review images are mirrored through the review/R2 flow rather than staged as source.

## Next

- Continue replacing broad flat water and terrain ribbons with authored low-poly transition geometry where the all-map contact sheet still reads as too planar.
- Add more per-area waterline detail where individual maps still share the same lake silhouette.
- Resume bridge traversal and walkable-affordance checks after the current visual uplift strand is safely checkpointed.
