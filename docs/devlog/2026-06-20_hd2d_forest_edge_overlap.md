# HD2D forest edge overlap

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-20

## Summary

This cycle follows the broad water surface pass and targets the far-shore woodland band. The prior review improved water bodies, but the distant nature still had long, flat green bands in wide views. This pass adds deterministic low-poly forest-edge overlap geometry so the panorama reads as layered woodland crowns instead of a single flat silhouette.

The accepted packet is `docs/review/2026-06-20T08-25_forest_edge_overlap_r1/`. It keeps renderer features frozen while adding `DistantVista_ForestEdgeOverlap` meshes, current/past `Ch1Distant_*ForestEdgeOverlap` materials, validation count coverage, and camera visibility coverage.

## Implementation

- Added `DistantPanoramaVistaForestEdgeOverlapCount` and `DistantPanoramaVistaForestEdgeOverlapVisibleMinimum`.
- Added `CreateDistantPanoramaVistaForestEdgeOverlaps` across all current/past outdoor distant panorama roots.
- Added `CreateDistantPanoramaVistaForestEdgeOverlapMesh`, built from deterministic canopy lobes and leaf-fringe pieces for an uneven crown line.
- Added `EnsureDistantPanoramaVistaForestEdgeOverlapMaterial` with a distinct dark/highlight foliage range so the layer separates from existing natural canopy without becoming neon or mist-like.
- Updated distant panorama validation to require forest-edge overlap counts, renderer policy, mesh density, non-collision, ring radius, and camera visibility.
- Rejected r1 because the new layer was structurally valid but visually plateaued: shotdiff was effectively zero because the layer sat behind or blended into the existing natural tree stands.
- Accepted r2 after moving the layer to the front upper edge of the woodland ring, reducing inward push, increasing crown height/depth, and retuning the foliage material so the overlap registers in pixels.
- Kept renderer features, deterministic placement, non-colliding geometry, and authored-file scope unchanged.

## Visual Review

- Review packet: `docs/review/2026-06-20T08-25_forest_edge_overlap_r1/`
- Contact sheet: `docs/review/2026-06-20T08-25_forest_edge_overlap_r1/00_contact_sheet.png`
- All 13 all-map captures were refreshed from `docs/devlog/screenshots/chapter1_all_maps_cycle05/` and copied into the review packet.
- Shotdiff vs `docs/review/2026-06-20T07-00_broad_water_surface_r1/`: r1 was rejected with 0/13 changed; r2 was accepted with 8/13 frames changed and the side-view stability frame unchanged.
- Strongest movement: `09_e1_e3_current.png` 1.2272%, `07_d1_d3_current.png` 1.0711%, `12_f1_f6_past.png` 0.7441%, `11_f1_f6_current.png` 0.7100%, `08_d1_d3_past.png` 0.5047%.
- Representative checks:
  - `07_d1_d3_current.png`: the long far woodland now has visible crown overlap above the water ring.
  - `09_e1_e3_current.png`: the broad panorama gains darker and lighter treetop strata rather than a single green shelf.
  - `11_f1_f6_current.png` and `12_f1_f6_past.png`: wide map coverage shows the new layer across the full distant arc.
  - `13_scene6_sideview_auto.png`: side-view traversal/stability frame remains unchanged.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`:
  - r2 log: `Logs/forest_edge_overlap_validate_r2.log`, passed with exit code 0 after the initial radius fix.
  - r3 log: `Logs/forest_edge_overlap_validate_r3.log`, passed with exit code 0 after the visual r2 reposition.
  - `Fast VS house slice validation passed.`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
  - r2 log: `Logs/forest_edge_overlap_capture_r2.log`, exit code 0.
  - Output copied into `docs/review/2026-06-20T08-25_forest_edge_overlap_r1/`.
- Shotdiff triage:
  - r1 output: `Logs/forest_edge_overlap_shotdiff_r1.json`, rejected because the change was visually plateaued.
  - r2 output: `Logs/forest_edge_overlap_shotdiff_r2.json`, accepted with 8/13 frames changed and sideview unchanged.
- EditMode renderer freeze: passed, 36/36.
  - XML: `Logs/forest_edge_overlap_editmode_r4.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed.
  - Note: `-runTests` must be executed without `-quit` in this Unity environment; r1-r3 with `-quit` only compiled/refreshed and did not emit XML.
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed.
  - Log: `Logs/forest_edge_overlap_asset_validation_r1.log`
  - `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed.
  - Log: `Logs/forest_edge_overlap_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
  - Build timestamp: 2026-06-20 08:21:17 local time.
- Player smoke: 24 seconds, stopped manually after startup.
  - Log: `Logs/forest_edge_overlap_player_smoke_r1.log`
  - Case-sensitive failure scan for `Error|Exception|Assert|NullReference|MissingReference|Failed|Crash|Fatal`: 0 matches.

Unity batch side effects were reverted before staging. The authored implementation remains scoped to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`; review images are mirrored through the review/R2 flow rather than staged as source.

## Next

- Continue nature realism by addressing the near/mid vegetation that still reads too dark and clumped in the all-map current views.
- Consider a dedicated pass for authored tree model silhouettes and branch structure in the player-visible map edges, separate from distant panorama geometry.
