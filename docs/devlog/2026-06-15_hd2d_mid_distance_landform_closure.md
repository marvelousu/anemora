# HD2D mid-distance landform closure

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-15

## Investigation

- The distant quality refinement improved ridge and treeline silhouettes, but the all-map Wide review still showed a broad empty middle-distance band between the playable boards and the panorama ring.
- The remaining blocker was structural: more color/fog tuning would not remove the large water/void moat read. The scene needed actual authored landform geometry between the close map edge and the far panorama.
- The earlier House Exterior midground and map-edge prototypes were intentionally one-map-first. This cycle promotes that idea into an all-outdoor-map layer while keeping it collision-free so route movement and the repaired bridge traversal remain untouched.

## Implementation

- Added `MidDistanceLandformClosure` roots to every Chapter 1 outdoor map in both current and past:
  - House Exterior,
  - Central Plaza,
  - Mia House,
  - Aria Street,
  - Kaia Farm,
  - Ruins.
- Each root now builds deterministic, collision-free middle-distance geometry:
  - `TerrainShelf` meshes: irregular low terrain shelves around the map,
  - `LowBank` meshes: low raised earth/stone silhouettes behind the shelves,
  - `CoppiceFold` meshes: near treeline folds to break the flat horizon band,
  - `PathThread` meshes: short road/path continuations aimed into the panorama.
- Added `Ch1Distant_*MidDistanceLandform*` materials for terrain, banks, path threads, and coppice folds. They are generated from the authored setup and stay separate from renderer features.
- Added all-map validation for root parenting, render layer, mesh density, expected terrain/bank/coppice/path counts, no colliders, middle-distance radius range, non-shadow renderer policy, material naming, landmark markers, and Wide-camera visibility.

## Verification

- Validate: `Logs/mid_distance_landform_closure_validate_r1.log` passed with `Fast VS house slice validation passed.`
- Renderer freeze: `Logs/mid_distance_landform_closure_editmode_r1.xml` passed `36/36`; `RendererFeatureSet_MatchesFrozenBaseline` result is `Passed`.
- Asset validation: `Logs/mid_distance_landform_closure_asset_validation_r1.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/mid_distance_landform_closure_capture_r1.log` produced the 13 all-map Wide review PNGs.
- Review packet: `docs/review/2026-06-15T10-46_mid_distance_landform_closure/` contains the 13 all-map frames, `00_contact_sheet.png`, and `devlog.txt`.
- Shotdiff: `Logs/shotdiff/mid_distance_landform_closure_vs_distant_quality_r1` compared against `docs/review/2026-06-15T09-54_distant_quality_refinement`. The contact sheet changed by `17.0902%`. All 12 current/past Wide map frames changed over the 0.05% triage budget: `01_a1_a2_current.png` 10.8187%, `02_a1_a2_past.png` 9.2895%, `03_b1_b3_current.png` 7.8314%, `04_b1_b3_past.png` 6.7126%, `05_c1_c3_current.png` 9.4954%, `06_c1_c3_past.png` 8.3958%, `07_d1_d3_current.png` 9.4619%, `08_d1_d3_past.png` 7.3952%, `09_e1_e3_current.png` 6.1220%, `10_e1_e3_past.png` 6.5012%, `11_f1_f6_current.png` 7.3453%, and `12_f1_f6_past.png` 4.2925%. The side-view frame remained unchanged.

## Visual Review

- Accepted as an all-map middle-distance closure pass: the outdoor maps now have a visible authored landform band between the playable area and panorama, with path continuations and low tree/earth silhouettes breaking the empty ring.
- This is still not the final production environment state. Several frames still show hard foreground board edges and large water/shoreline areas, especially where the camera sees the near side of the playable map. The next structural pass should author foreground shoreline skirts, approach-road continuation, and map-edge occlusion details instead of adding more far panorama color polish.

## Next

- Continue with foreground shoreline and near-edge closure across all maps.
- Keep decorative middle/foreground geometry collision-free unless a piece is explicitly part of a traversal surface.
- Preserve bridge character traversal validation, renderer feature freeze, R2 review publication, and viewer refresh for every visual cycle.
