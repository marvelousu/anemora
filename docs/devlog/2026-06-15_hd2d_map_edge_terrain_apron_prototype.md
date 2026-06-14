# HD2D map-edge terrain apron prototype

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-15

## Investigation

- The midground edge closure prototype removed the rejected floating far-water shelves, but House Exterior still read as a square board surrounded by water.
- The next structural blocker was the hard playable-map perimeter, not palette tuning. The edge needed top-surface terrain pieces that break the silhouette without adding black side faces or collision.
- The all-map review also showed that the distant panorama still needs a later ridge/treeline quality pass. This cycle deliberately stays one-map-first on House Exterior map-edge terrain, then leaves distant refinement as the next cycle.

## Implementation

- Added `Current_HouseExterior_MapEdgeTerrainApron` and `Past_HouseExterior_MapEdgeTerrainApron` under the House Exterior map roots.
- Added deterministic, collision-free terrain apron meshes around the House Exterior perimeter:
  - irregular back/left/right/front shore and promontory top surfaces,
  - a NE road fan continuation so the road no longer reads as cut off at the map edge,
  - shorter waterline shingle pieces to break the hard rectangle without drawing a bright outline.
- Meshes are authored grid surfaces with deterministic vertex drift from seed values. They use only top faces so they do not create the black vertical side faces that failed the earlier shelf attempts.
- Added validation for root parenting, render layer, mesh count, mesh density, no colliders, no distant-vista naming overlap, flat top-surface bounds, non-shadow renderers, intended House Exterior edge-band bounds, terrain/road/stone mix, and Wide-camera visibility.
- Kept bridge traversal and route validation untouched. Decorative apron meshes do not count for arrival and do not add colliders.

## Rejected Iterations

- r1 used double-sided coplanar terrain triangles. `RecalculateNormals()` made several close apron pieces render as black bands, especially current-side road/stone pieces.
- r2 changed the terrain apron meshes to single-sided top triangles and shrank the closest stone piece. This removed the black band failure.
- r3 added stronger stone waterline pieces to avoid a plateau, but the right and left sides read as bright straight rails rather than authored shore breakup.
- r4 shortened those waterline pieces and switched the longer ones to path material. This reduced the white outline read.
- r5 restored a larger road fan after the normal fix, keeping the road-continuation read without reintroducing black bands.

## Verification

- Validate: `Logs/map_edge_terrain_apron_validate_r5.log` passed with `Fast VS house slice validation passed.`
- Renderer freeze: `Logs/map_edge_terrain_apron_editmode_r2.xml` passed `36/36`; `RendererFeatureSet_MatchesFrozenBaseline` result is `Passed`.
- Asset validation: `Logs/map_edge_terrain_apron_asset_validation_r2.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/map_edge_terrain_apron_capture_r5.log` produced the 13 all-map Wide review PNGs.
- Review packet: `docs/review/2026-06-15T08-15_map_edge_terrain_apron/` contains the 13 all-map frames, `00_contact_sheet.png`, and `devlog.txt`.
- Shotdiff: `Logs/shotdiff/map_edge_terrain_apron_vs_midground_r5` compared against `docs/review/2026-06-15T07-15_midground_edge_closure_prototype`. The contact sheet differs at `25.7209%`. Individual map changes are scoped to House Exterior: `01_a1_a2_current.png` at `0.3079%` and `02_a1_a2_past.png` at `0.3426%`; all other map frames are `0.0000%`.

## Visual Review

- Accepted as a Phase D one-map terrain-apron prototype: the rejected black/floating shelf look is gone, House Exterior gets visible road-edge and waterline breakup, and the change is constrained to the intended current/past House Exterior maps.
- Not production final: the current-side House Exterior still reads darker than the character art deserves, and the distant ring still has broad low-poly ribbon reads. The next cycle should improve distant ridge profiles and foreground/midground treeline quality rather than continuing small edge polish.

## Next

- Start the next cycle on distant-vista quality refinement rather than more local edge constants.
- Replace broad flat mountain ribbons with more authored ridge silhouettes, nested treeline cuts, and stronger valley depth gaps.
- Keep any new distant meshes deterministic, collision-free, separate from renderer features, and validated through the same all-map capture plus shotdiff path.
