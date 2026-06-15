# HD2D foreground edge breakup

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-15

## Investigation

- The foreground shoreline closure fixed the board-edge void, but the Wide review still exposed bright flat foreground reads in current House Exterior and Central Plaza.
- The issue was structural rather than tone polish: the newly generated shoreline materials were still flat fills, and several camera-facing strips needed smaller authored breakup patches with texture and irregular silhouette.
- The pass keeps bridge and route traversal untouched. All new breakup geometry is render-only and collision-free so the repaired bridge route remains the single traversal contract.

## Implementation

- Added `ForegroundEdgeBreakup` roots to every Chapter 1 outdoor map in both current and past:
  - House Exterior,
  - Central Plaza,
  - Mia House,
  - Aria Street,
  - Kaia Farm,
  - Ruins.
- Each root adds deterministic, collision-free near-camera pieces:
  - `GroundPatch` textured low shelves to cover large bright foreground slabs,
  - `PathPatch` strips to continue road material into the foreground edge,
  - `DetailPatch` low banks to break hard shoreline silhouettes.
- Converted the existing `Ch1Distant_*ForegroundShoreline*` terrain, path, stone, and reed materials from flat fills to generated point/repeat textures so they no longer read as blank painted planes.
- Added `Ch1Surface_*ForegroundEdgeBreakup*` generated materials for terrain, path, and detail patches. These stay separate from cycle materials and do not change renderer features.
- Added validation for generated texture presence, root parenting, layer, mesh density, no colliders, low relief, material naming, texture binding, non-shadow renderer policy, landmark markers, category counts, and Wide-camera coverage.

## Verification

- Validate: `Logs/foreground_edge_breakup_validate_r6.log` passed with `Fast VS house slice validation passed.`
- Renderer freeze: `Logs/foreground_edge_breakup_editmode_r1.xml` passed `36/36`; `RendererFeatureSet_MatchesFrozenBaseline` result is `Passed`.
- Asset validation: `Logs/foreground_edge_breakup_asset_validation_r1.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/foreground_edge_breakup_capture_r1.log` produced the 13 all-map Wide review PNGs.
- Review packet: `docs/review/2026-06-15T13-30_foreground_edge_breakup/` contains the 13 all-map frames, `00_contact_sheet.png`, and `devlog.txt`.
- Shotdiff: `Logs/shotdiff/foreground_edge_breakup_vs_shoreline_r1/` compared against `docs/review/2026-06-15T11-47_foreground_shoreline_closure`. The contact sheet changed by `6.7557%`. Over-budget Wide frames were `01_a1_a2_current.png` 3.2152%, `02_a1_a2_past.png` 0.5725%, `03_b1_b3_current.png` 4.4615%, `04_b1_b3_past.png` 1.5139%, `05_c1_c3_current.png` 0.6188%, `06_c1_c3_past.png` 0.5681%, `07_d1_d3_current.png` 1.0079%, `08_d1_d3_past.png` 0.5788%, `09_e1_e3_current.png` 0.6934%, `11_f1_f6_current.png` 1.3289%, and `12_f1_f6_past.png` 0.7428%. `10_e1_e3_past.png` and the side-view frame stayed under the triage budget.

## Visual Review

- Accepted as a foreground flat-surface cleanup pass: current Central Plaza no longer has the large blank white foreground slab, and current House Exterior has more textured ground/path breakup at the camera-facing edge.
- The change is intentionally strongest in current House Exterior and Central Plaza; other maps receive smaller near-edge texture and silhouette variation without moving playable routes.
- Remaining issue: current House Exterior still has small bright distant waterline/island holes in the upper-center panorama. The next structural pass should close those far-shore holes directly rather than adding more foreground polish.

## Next

- Trace the distant bright waterline holes visible in `01_a1_a2_current.png` and add authored far-shore closure or material replacement where the panorama still leaks.
- Continue increasing distant vista quality with layered landform silhouettes, far-bank vegetation, and atmospheric depth once the remaining white holes are gone.
- Keep collision and route validation focused on the already repaired bridge traversal; visual edge dressing remains collision-free unless explicitly promoted to walkable geometry.
