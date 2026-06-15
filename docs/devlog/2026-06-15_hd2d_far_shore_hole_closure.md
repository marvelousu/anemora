# HD2D far-shore hole closure

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-15

## Investigation

- The foreground edge breakup removed the largest near-camera flat reads, but current House Exterior still showed three pale holes in the upper-center distant panorama.
- The first far-shore closure attempt added terrain shelves, low banks, and coppice folds, but one bright patch remained nearly unchanged. That rejected plateau confirmed the issue was not just missing background mass.
- A temporary renderer diagnostic traced the visible bright components to the House Exterior Cycle55 back path band and the mid-distance landform path-thread material. The diagnostic code was removed before final validation.
- Root cause: flat, bright path/horizon materials were reading as white distant voids from the Wide camera. The accepted fix therefore replaces those reads structurally instead of continuing color polish.

## Implementation

- Added `FarShoreHoleClosure` roots to every Chapter 1 outdoor map in both current and past:
  - House Exterior,
  - Central Plaza,
  - Mia House,
  - Aria Street,
  - Kaia Farm,
  - Ruins.
- Each root adds deterministic, collision-free distant closure meshes:
  - `TerrainShelf` landform shelves,
  - `LowBank` far-bank relief,
  - `CoppiceFold` low vegetation silhouettes.
- Added separate generated materials named `Ch1Distant_*FarShoreHoleClosure*` for terrain, bank, and coppice reads. These materials are textured, non-metallic, low-smoothness, and do not change renderer features.
- Converted `Ch1Distant_*MidDistanceLandformPath` from flat color to textured generated path material so path threads no longer read as pale holes.
- Swapped the House Exterior Cycle55 back path band from the generic path material to the far-shore terrain material and updated validation to require that authored material token.
- Added validation for texture generation, root parenting, layer, mesh counts, no colliders, distance band, low relief, material naming, texture binding, non-shadow renderer policy, landmark markers, category counts, and Wide-camera coverage.

## Verification

- Validate: `Logs/far_shore_hole_closure_validate_r2.log` passed with `Fast VS house slice validation passed.`
- Renderer freeze: `Logs/far_shore_hole_closure_editmode_r1.xml` passed `36/36`; `RendererFeatureSet_MatchesFrozenBaseline` result is `Passed`.
- Asset validation: `Logs/far_shore_hole_closure_asset_validation_r1.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/far_shore_hole_closure_capture_r5.log` produced the 13 all-map Wide review PNGs.
- Review packet: `docs/review/2026-06-15T14-12_far_shore_hole_closure/` contains the 13 all-map frames, `00_contact_sheet.png`, and `devlog.txt`.
- R2 review upload: `tools/r2/r2-upload-review.ps1` uploaded 16 files for `wip-hd2d-point15-recovery-20260612/2026-06-15T14-12_far_shore_hole_closure`; the branch manifest now lists 306 paths.
- Bright-blob check on `01_a1_a2_current.png`: the previous packet had 3 pale components at thresholds 150 and 160; this packet has 0 at thresholds 150, 160, and 170 in the same far-shore region.
- Shotdiff: `Logs/shotdiff/far_shore_hole_closure_vs_edge_breakup_r1/` compared against `docs/review/2026-06-15T13-30_foreground_edge_breakup`. The contact sheet changed by `29.6161%`; the only over-budget Wide frame was `01_a1_a2_current.png` at `0.6262%`. All other current/past Wide frames stayed under the `0.5%` triage budget, and the side-view frame remained unchanged.

## Visual Review

- Accepted as a targeted far-shore hole closure pass: current House Exterior no longer has the three pale upper-center distant holes.
- The change is intentionally concentrated in House Exterior current, with small generated closure meshes still present on every outdoor current/past map to keep validation and future rollout grammar consistent.
- Remaining issue: the panorama still reads too much like layered flat bands. The next structural pass should improve water/shore integration, landform facet silhouette, and far-bank repetition instead of adding color-only polish.

## Next

- Improve distant river and shoreline composition so water is not a uniform horizontal ribbon.
- Add stronger authored landform facets and occlusion overlaps to reduce the remaining cardboard-band read.
- Continue keeping all visual closure meshes collision-free; bridge traversal remains covered by the existing character traversal validation.
