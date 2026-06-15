# HD2D distant landform facets

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-15

## Investigation

- The waterline pass improved the lower edge read, but the upper distant panorama still repeated as stacked, mostly horizontal mountain and treeline bands.
- The first landform facet attempt validated but only moved two Wide frames over budget. That was treated as a plateau because the pieces were too narrow, too low, and too close in color to the existing panorama.
- The accepted pass moves the new facets into the forward review arc, increases width/height, and gives current/past distinct textured ridge/tree materials so the upper silhouette changes structurally rather than by tint polish.

## Implementation

- Added deterministic `DistantVista_LandformFacet` meshes inside every Chapter 1 outdoor `DistantVista` root in both current and past.
- Each map now gets eight authored foreground/back landform overlaps:
  - `FrontTreeMass` pieces sit in front of the existing horizon bands and break the lower treeline.
  - `BackRidge` pieces sit behind them and change the upper mountain silhouette.
- Added generated textured materials named `Ch1Distant_*LandformFacetBackRidge` and `Ch1Distant_*LandformFacetFrontTree`.
- Extended validation for generated texture presence, luminance range, root mesh count, textured material binding, no collision, no shadows, and Wide-camera landform facet visibility.

## Verification

- Validate: `Logs/landform_facets_validate_r2.log` passed with exit code 0.
- Renderer freeze: `Logs/landform_facets_editmode_r1.xml` passed `36/36`; `RendererFeatureSet_MatchesFrozenBaseline` result is `Passed`.
- Asset validation: `Logs/landform_facets_asset_validation_r1.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/landform_facets_capture_r2.log` produced the 13 all-map Wide review PNGs.
- Review packet: `docs/review/2026-06-15T16-12_landform_facets/` contains the 13 all-map frames, `00_contact_sheet.png`, and `devlog.txt`.
- R2 review upload: `tools/r2/r2-upload-review.ps1` uploaded 16 files for `wip-hd2d-point15-recovery-20260612/2026-06-15T16-12_landform_facets`; the branch manifest now lists 338 paths.
- Shotdiff: `Logs/shotdiff/landform_facets_vs_waterline_r2/` compared against `docs/review/2026-06-15T15-15_waterline_breakup`. Over-budget Wide frames were `04_b1_b3_past.png` 0.6693%, `05_c1_c3_current.png` 0.7270%, `06_c1_c3_past.png` 2.3094%, `07_d1_d3_current.png` 4.3827%, `08_d1_d3_past.png` 3.4634%, `09_e1_e3_current.png` 5.7343%, `10_e1_e3_past.png` 4.5123%, `11_f1_f6_current.png` 3.1522%, and `12_f1_f6_past.png` 3.8581%.

## Visual Review

- Accepted as an upper-panorama structure pass: C/D/E/F current and past now show obvious authored mountain/treeline overlap instead of only smooth bands.
- House Exterior and Central Plaza current still move less in shotdiff because the prior distant shape already occupied the same horizon band, but the new pieces are visible in the contact sheet and validation requires at least four landform facets in the Wide camera.
- The change remains render-only and does not touch bridge traversal, playable collision, renderer features, APV, or Volume setup.

## Next

- Add a second pass aimed specifically at House Exterior and Central Plaza foreground/midground reads so the lower map edge no longer relies on the same broad apron language.
- Continue toward authored production quality by alternating structural mesh passes with asset/material passes; avoid small color-only changes when shotdiff and contact sheets do not move.
