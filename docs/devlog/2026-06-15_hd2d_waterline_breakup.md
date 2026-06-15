# HD2D waterline breakup

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-15

## Investigation

- The far-shore hole closure removed the white distant holes, but the all-map Wide contact sheet still read as broad horizontal water and shore bands.
- The first waterline breakup attempt validated but only moved individual frames by at most `0.1858%`; that was rejected as a plateau because the new geometry was too small and too far back to affect the composition.
- The accepted pass moves the breakup pieces closer to the visible water ribbon and increases their authored width, depth, and reed-shadow mass. This is a structural change, not a color-only polish pass.

## Implementation

- Added `WaterlineBreakup` roots to every Chapter 1 outdoor map in both current and past:
  - House Exterior,
  - Central Plaza,
  - Mia House,
  - Aria Street,
  - Kaia Farm,
  - Ruins.
- Each root adds deterministic, collision-free water-edge pieces:
  - `BankShelf` irregular terrain shelves that bite into the horizontal water ribbon,
  - `ReedShadow` low vegetation silhouettes that break the far-bank line,
  - `ShallowRun` darker shallow-water strips that add overlap without making new white glints.
- Added separate generated materials named `Ch1Distant_*WaterlineBreakup*` for bank, reed, and shallow-water reads. These stay outside the legacy cycle materials and do not change renderer features.
- Added validation for generated texture presence, root parenting, layer, mesh density, no colliders, distance band, low relief, material naming, texture binding, non-shadow renderer policy, landmark markers, category counts, and Wide-camera coverage.

## Verification

- Validate: `Logs/waterline_breakup_validate_r2.log` passed with `Fast VS house slice validation passed.`
- Renderer freeze: `Logs/waterline_breakup_editmode_r1.xml` passed `36/36`; `RendererFeatureSet_MatchesFrozenBaseline` result is `Passed`.
- Asset validation: `Logs/waterline_breakup_asset_validation_r1.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/waterline_breakup_capture_r2.log` produced the 13 all-map Wide review PNGs.
- Review packet: `docs/review/2026-06-15T15-15_waterline_breakup/` contains the 13 all-map frames, `00_contact_sheet.png`, and `devlog.txt`.
- R2 review upload: `tools/r2/r2-upload-review.ps1` uploaded 16 files for `wip-hd2d-point15-recovery-20260612/2026-06-15T15-15_waterline_breakup`; the branch manifest now lists 322 paths.
- Bright-blob regression check on `01_a1_a2_current.png`: the far-shore region remains at 0 pale components at thresholds 150, 160, and 170.
- Shotdiff: `Logs/shotdiff/waterline_breakup_vs_far_shore_r2/` compared against `docs/review/2026-06-15T14-12_far_shore_hole_closure`. Over-budget Wide frames were `01_a1_a2_current.png` 0.7802%, `02_a1_a2_past.png` 0.5143%, `03_b1_b3_current.png` 0.7998%, `05_c1_c3_current.png` 0.6210%, `06_c1_c3_past.png` 0.5499%, `07_d1_d3_current.png` 0.7275%, and `09_e1_e3_current.png` 0.5760%. The contact sheet changed by `0.2787%`; the side-view frame remained unchanged.

## Visual Review

- Accepted as a waterline structure pass: current maps now have visible interruptions along the broad water ribbons, and past maps get smaller but still authored bank/shallow-water overlaps.
- The change deliberately leaves bridge traversal and playable collision untouched; every new mesh is render-only.
- Remaining issue: the distant hills and treeline still repeat as stacked bands. The next pass should add stronger per-area landform facets and occlusion overlaps rather than widening the waterline treatment further.

## Next

- Build a per-area distant landform facet pass that changes silhouettes and occlusion, especially in the upper third of the Wide captures.
- Add skyline/treeline variation by area so House Exterior, Central Plaza, Mia House, Aria Street, Kaia Farm, and Ruins are recognizable from the panorama alone.
- Continue rejecting passes where only tiny diffs or contact-sheet layout changes appear.
