# HD2D midground edge closure prototype

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-15

## Investigation

- The all-map distant composition rollout improved the horizon, but the House Exterior Wide frame still read as a square playable board surrounded by empty water/void.
- The next blocker was structural, not palette polish: the view needed authored geometry between the playable tile and distant panorama.
- The first midground attempt put wide shelves too far behind the map. In the review frame those meshes became floating strips on the water plane, including black bands caused by thick geometry and poor camera read.
- The accepted direction for this prototype is narrower: one map first, House Exterior only, with a low edge skirt attached to the playable map boundary.

## Implementation

- Added `Current_HouseExterior_MidgroundEdgeClosure` and `Past_HouseExterior_MidgroundEdgeClosure` under the House Exterior map roots.
- Replaced the failed far-water shelf concept with 12 deterministic non-arrival geometry pieces per time state:
  - north/back shore shelves,
  - left and right bank shelves,
  - front shore shelves,
  - NE road continuation pieces,
  - low stone edge strips,
  - waterline stones,
  - compact treeline masses.
- Kept the geometry collision-free so the playable route and bridge traversal work from the previous cycle remain untouched.
- Added validation for root parenting, render layer, mesh count, material presence, no colliders, no distant-vista naming overlap, renderer no-shadow policy, edge-band bounds, terrain/prop mix, and Wide-camera visibility.
- Updated the edge-closure renderer policy to reapply both render layer and shadow settings after generation. This matches the distant-vista policy pattern because later setup paths can mutate renderer state.

## Rejected Iterations

- r1 generated the intended object count and passed basic validation, but the visual read was worse than baseline: a large black/green shelf occupied the upper-middle of the House Exterior frame.
- r2 moved the shelf farther away and lower, but still created floating water-island artifacts and black strips.
- r3 changed the structure from far-water shelves to map-edge skirts. This removed the distant floating slabs, but the edge pieces were still too thick and dark on the current-side frame.
- r4 flattened the skirts to near-ground thin surfaces and reduced the low-cliff pieces to small stone reads. This is accepted as a Phase C prototype, not as final production edge treatment.

## Verification

- Validate: `Logs/midground_edge_closure_validate_r6.log` passed with `Fast VS house slice validation passed.`
- Renderer freeze: `Logs/midground_edge_closure_editmode_r6.xml` passed `36/36`; `RendererFeatureSet_MatchesFrozenBaseline` result is `Passed`.
- Test-run note: do not pass `-quit` to Unity `-runTests`. The Test Runner exits on its own; adding `-quit` caused Unity to initialize and close before writing `testResults`.
- Asset validation: `Logs/midground_edge_closure_asset_validation_r3.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/midground_edge_closure_capture_r4.log` produced the 13 all-map Wide review PNGs.
- Review packet: `docs/review/2026-06-15T07-15_midground_edge_closure_prototype/` contains the 13 all-map frames, `00_contact_sheet.png`, and `devlog.txt`.
- Shotdiff: `Logs/shotdiff/midground_edge_closure_vs_all_maps_r2` compared against `docs/review/2026-06-15T05-52_distant_composition_all_maps`. Individual map changes are scoped to `01_a1_a2_current.png` at `1.9539%` and `02_a1_a2_past.png` at `0.2613%`; all other map frames are `0.0000%`. The contact sheet differs by layout regeneration and is not the visual acceptance signal.

## Visual Review

- Accepted for Phase C one-map prototype: House Exterior no longer has the rejected floating far-water shelf artifacts, and the new work is constrained to the map edge.
- Not production final: the current-side House Exterior is still too dark and the playable map still has a square-board read in the Wide review. This must be handled by the next cycle with stronger authored terrain breakup, better near/mid apron shape, and distant vista refinement rather than color-only changes.

## Next

- Start the next cycle with distant-vista quality refinement and stronger map-edge silhouette breakup.
- For distant quality, replace broad flat mountain ribbons with more authored ridge profiles, nested treeline cutouts, valley depth gaps, and fog/far-clip tuning.
- For map-edge quality, use irregular single-sided terrain meshes or authored low-poly terrain pieces so the edge breaks the rectangular board without adding black side faces.
- Keep bridge traversal and current/past route validation in place. Do not add colliders to decorative edge closure.
- Continue publishing every visual cycle through Validate, renderer-freeze EditMode without `-quit`, asset validation, all-map capture, shotdiff, devlog, R2 review upload, public viewer verification, pathspec commit, and push.
