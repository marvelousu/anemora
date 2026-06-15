# HD2D nearfield dressing

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-15

## Investigation

- The distant landform facet pass improved the upper panorama, but House Exterior and Central Plaza still read as broad lower foreground/midground panels with too little authored edge detail.
- The first nearfield dressing attempt validated, but shotdiff showed House Exterior current at only `0.2325%`; the raised detail was too peripheral and did not move the weakest Wide frame enough.
- The accepted pass adds a front curb category and pulls stone/hedge detail toward the review camera so the foreground edge changes structurally instead of relying on color polish.

## Implementation

- Added deterministic `NearfieldDressing` roots to every Chapter 1 outdoor map in both current and past:
  - House Exterior,
  - Central Plaza,
  - Mia House,
  - Aria Street,
  - Kaia Farm,
  - Ruins.
- Each root adds render-only, collision-free authored meshes:
  - `TerracePatch` ground shelves to break broad flat foreground panels,
  - `PathShard` pieces aligned to the existing readable path direction,
  - `FrontCurb` low stone curbs placed closer to the Wide camera,
  - `StoneMarker` low raised stones for scale and edge rhythm,
  - `HedgeRim` low vegetation folds for softer foreground silhouettes.
- Added generated textured materials named `Ch1Surface_*NearfieldDressing*` for ground, path, stone, and hedge reads. These stay outside the legacy cycle materials and do not change renderer features.
- Extended validation for generated texture presence, root parenting, layer, mesh density, no colliders, nearfield band limits, low relief, material naming, texture binding, non-shadow renderer policy, landmark markers, category counts, and Wide-camera coverage.

## Verification

- Validate: `Logs/nearfield_dressing_validate_r3.log` passed with exit code 0.
- Renderer freeze: `Logs/nearfield_dressing_editmode_r1.xml` passed `36/36`; `RendererFeatureSet_MatchesFrozenBaseline` result is `Passed`.
- Asset validation: `Logs/nearfield_dressing_asset_validation_r1.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/nearfield_dressing_capture_r2.log` produced the 13 all-map Wide review PNGs.
- Review packet: `docs/review/2026-06-15T17-07_nearfield_dressing/` contains the 13 all-map frames, `00_contact_sheet.png`, and `devlog.txt`.
- R2 review upload: `tools/r2/r2-upload-review.ps1` uploaded 16 files for `wip-hd2d-point15-recovery-20260612/2026-06-15T17-07_nearfield_dressing`; the branch manifest now lists 354 paths.
- Shotdiff: `Logs/shotdiff/nearfield_dressing_vs_landform_r2/` compared against `docs/review/2026-06-15T16-12_landform_facets`. All 12 Wide frames moved over budget: `01_a1_a2_current.png` 0.6705%, `02_a1_a2_past.png` 1.1587%, `03_b1_b3_current.png` 2.5291%, `04_b1_b3_past.png` 2.7377%, `05_c1_c3_current.png` 1.6028%, `06_c1_c3_past.png` 1.6664%, `07_d1_d3_current.png` 2.0216%, `08_d1_d3_past.png` 1.1201%, `09_e1_e3_current.png` 1.6985%, `10_e1_e3_past.png` 1.7702%, `11_f1_f6_current.png` 1.3414%, and `12_f1_f6_past.png` 1.0834%. The contact sheet changed by 7.6577%; the side-view frame remained unchanged.

## Visual Review

- Accepted as a foreground/midground structure pass: House Exterior current now crosses the change threshold, Central Plaza current/past move strongly, and every outdoor Wide frame has visible authored nearfield interruption.
- The pass remains render-only and does not touch bridge traversal, playable collision, renderer features, APV, or Volume setup.
- The environment is still not at final authored production quality. The next graphics pass should move from compositional geometry into higher-quality vegetation assets, stronger ground/surface material breakup, and area-specific prop language.

## Next

- Replace remaining primitive-looking vegetation and edge silhouettes with authored low-poly clusters while preserving existing placement coordinates where possible.
- Add area-specific ground and building surface breakup so the current/past maps do not rely on the same repeated tile read.
- Keep bridge traversal in the verification loop, but avoid changing collision unless a fresh traversal repro proves a real blocker.
