# HD2D distant quality refinement

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-15

## Investigation

- The previous map-edge terrain apron cycle closed the House Exterior board-edge problem, but the all-map review still showed distant panorama bands reading as broad flat ribbons.
- The blocker was geometry structure rather than sky/fog color. The existing distant ring already had enough total objects, but the main forest and relief meshes used long connected columns that did not break the silhouette at character-art quality.
- The target for this cycle was therefore all-map distant ridge/treeline quality: add visible authored contour detail to current and past without adding renderer features, colliders, random placement, or another flat sky/backdrop layer.

## Implementation

- Added deterministic `DistantVista_RidgeFacet` detail meshes to every outdoor map and time frame:
  - two depth bands per area, derived from area/time/segment seed values,
  - jagged ridge peaks, saddles, lower cuts, and depth thickness,
  - `Ch1Distant_*RidgeFacet*` materials kept separate from existing cycle materials.
- Added deterministic `DistantVista_TreelineFold` meshes to every outdoor map and time frame:
  - front-biased tree crown folds to break the lower panorama band,
  - collision-free meshes with `TimeWindowPairedSpaceLandmark` markers and `countsForArrival=false`.
- Raised existing distant forest and relief mesh column density so the baseline ring itself has finer crowns and ridge rhythm instead of long flat spans.
- Extended distant panorama validation to require the new ridge/treeline detail mesh counts and to ensure those details are visible in the all-map Wide camera.

## Rejected Iterations

- r1 added the detail layers but kept them too far back and low contrast. Shotdiff at a 0.05% budget changed only `07_d1_d3_current.png` and `09_e1_e3_current.png`, so it was rejected as too subtle.
- r2 moved detail forward and increased height/width. Current maps changed clearly, but past maps remained mostly under the threshold because the warm haze and material colors swallowed the folds.
- r3 kept the stronger geometry and darkened only the past detail materials, producing visible current/past changes across the all-map packet.

## Verification

- Validate: `Logs/distant_quality_refinement_validate_r3.log` passed with `Fast VS house slice validation passed.`
- Renderer freeze: `Logs/distant_quality_refinement_editmode_r3.xml` passed `36/36`; `RendererFeatureSet_MatchesFrozenBaseline` result is `Passed`.
- Asset validation: `Logs/distant_quality_refinement_asset_validation_r3.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/distant_quality_refinement_capture_r3.log` produced the 13 all-map Wide review PNGs.
- Review packet: `docs/review/2026-06-15T09-54_distant_quality_refinement/` contains the 13 all-map frames, `00_contact_sheet.png`, and `devlog.txt`.
- Shotdiff: `Logs/shotdiff/distant_quality_refinement_vs_map_edge_r3` compared against `docs/review/2026-06-15T08-15_map_edge_terrain_apron`. All 12 current/past Wide map frames changed over the 0.05% triage budget: `01_a1_a2_current.png` 0.1938%, `02_a1_a2_past.png` 0.5531%, `03_b1_b3_current.png` 0.3548%, `04_b1_b3_past.png` 0.3167%, `05_c1_c3_current.png` 0.4423%, `06_c1_c3_past.png` 0.4093%, `07_d1_d3_current.png` 1.3711%, `08_d1_d3_past.png` 1.2504%, `09_e1_e3_current.png` 2.2342%, `10_e1_e3_past.png` 1.2908%, `11_f1_f6_current.png` 0.8273%, and `12_f1_f6_past.png` 0.4947%. The side-view frame remained unchanged.

## Visual Review

- Accepted as an all-map distant quality refinement: current and past panoramas now have more broken ridge profiles, nested darker treeline folds, and fewer uninterrupted low-poly bands.
- This still is not the end-state for authored production scenery. The broad water/void moat between playable maps and the panorama remains visible in several Wide views, so the next structural pass should reduce that empty middle distance with stronger midground terrain shelves, road continuations, and area-specific foreground landforms rather than more color tuning.

## Next

- Continue with mid-distance landform closure across all maps, not just House Exterior.
- Keep any new midground pieces collision-free unless they are explicitly route/traversal surfaces.
- Preserve the bridge character traversal validation and renderer feature freeze while expanding visual coverage.
