# HD2D broad water surface

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-20

## Summary

This cycle follows the water reflection ribbon pass and targets the remaining broad, flat distant water bodies. The prior cycle made shoreline ribbons visible, but the all-map wide review still had large gray or overly still lake surfaces, especially the Aria Street past frame. This pass adds deterministic low-poly broad water sheet geometry so open water reads as authored terrain-scale surface rather than untextured void-adjacent fill.

The accepted packet is `docs/review/2026-06-20T07-00_broad_water_surface_r1/`. It keeps renderer features frozen while adding `BroadWaterSurface` outer sheets, central `BasinSheet` coverage, current/past `Ch1Distant_*BroadWaterSurface` materials, validation count coverage, and camera visibility coverage.

## Implementation

- Added `BroadWaterSurfaceSheetCount`, `BroadWaterSurfaceBasinSheetCount`, mesh count, and visible minimum constants.
- Added `CreateBroadWaterSurfaceSheetMesh`, a double-sided low-poly water sheet with deterministic chipped edges, flow lanes, and low relief for lit surface direction.
- Added `CreateChapter1PhaseIBroadWaterSurfaceForOutdoorMaps` and per-map `CreateChapter1PhaseIBroadWaterSurface` generation across all outdoor current/past maps.
- Placed outer sheets around the open-water ring and added centered basin sheets so large interior lake bodies, including `08_d1_d3_past.png`, no longer remain flat gray.
- Added `EnsureBroadWaterSurfaceMaterial` and `EnsureBroadWaterSurfacePixelMaterial` with muted blue-green current water and olive past water, deliberately avoiding white highlights and the earlier haze failure mode.
- Added `ValidateFastVsHd2dBroadWaterSurfaceAllMaps` with generated texture checks, root parenting/layer checks, non-colliding renderer policy, mesh-density checks, material texture checks, radius band checks, and camera visibility checks.
- Rejected r1 because the central basin remained too flat, accepted r2 structurally after adding basin sheets, then tuned r3 to lower saturation and smoothness for a more natural water read.
- Kept renderer features, deterministic placement, non-colliding geometry, and authored-file scope unchanged.

## Visual Review

- Review packet: `docs/review/2026-06-20T07-00_broad_water_surface_r1/`
- Contact sheet: `docs/review/2026-06-20T07-00_broad_water_surface_r1/00_contact_sheet.png`
- All 13 all-map captures were refreshed from `docs/devlog/screenshots/chapter1_all_maps_cycle05/` and copied into the review packet.
- Shotdiff vs `docs/review/2026-06-20T05-35_water_reflection_ribbons_r1/`: 12/13 frames changed over the 0.02% automated budget. The side-view stability frame remained unchanged.
- Strongest movement: `07_d1_d3_current.png` 6.901%, `09_e1_e3_current.png` 5.9378%, `05_c1_c3_current.png` 5.816%, `04_b1_b3_past.png` 5.5373%.
- Key target movement: `08_d1_d3_past.png` changed 3.0966%; the central gray lake body is now covered by muted basin water sheets.
- Representative checks:
  - `07_d1_d3_current.png`: the broad lake behind town now has continuous low-poly flow direction instead of a flat fill.
  - `08_d1_d3_past.png`: basin sheets cover the central lake while keeping the past palette olive and non-white.
  - `01_a1_a2_current.png`: the exterior water ring gains larger surface bodies without changing traversal.
  - `13_scene6_sideview_auto.png`: side-view traversal/stability frame remains unchanged.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`:
  - r1 log: `Logs/broad_water_surface_validate_r1.log`, passed with exit code 0.
  - r2 log: `Logs/broad_water_surface_validate_r2.log`, passed with exit code 0.
  - r3 log: `Logs/broad_water_surface_validate_r3.log`, passed with exit code 0.
  - `Fast VS house slice validation passed.`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
  - r3 log: `Logs/broad_water_surface_capture_r3.log`, exit code 0.
  - Output copied into `docs/review/2026-06-20T07-00_broad_water_surface_r1/`.
- Shotdiff triage:
  - r1 output: `Logs/broad_water_surface_shotdiff_r1.json`, rejected because the central basin still read too flat.
  - r2 output: `Logs/broad_water_surface_shotdiff_r2.json`, accepted structurally but too saturated on current water.
  - r3 output: `Logs/broad_water_surface_shotdiff_r3.json`, accepted with 12/13 frames changed and sideview unchanged.
- EditMode renderer freeze: passed, 36/36.
  - XML: `Logs/broad_water_surface_editmode_r1.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed.
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed.
  - Log: `Logs/broad_water_surface_asset_validation_r1.log`
  - `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed.
  - Log: `Logs/broad_water_surface_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
  - Build timestamp: 2026-06-20 06:58:31 local time.
- Player smoke: 24 seconds, stopped manually after startup.
  - Log: `Logs/broad_water_surface_player_smoke_r1.log`
  - Case-sensitive failure scan for `Error|Exception|Assert|NullReference|MissingReference|Failed|RenderGraph`: 0 matches.

Unity batch side effects were reverted before staging. The authored implementation remains scoped to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`; review images are mirrored through the review/R2 flow rather than staged as source.

## Next

- Continue natural realism by shaping far-shore tree silhouettes and species-specific canopy clusters against the now-richer water bodies.
- Inspect whether the broad water sheets should later receive area-specific flow direction or bank occlusion once the distant nature pass advances.
