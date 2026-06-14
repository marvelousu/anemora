# HD2D distant composition all-map rollout

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-15

## Investigation

- The active implementation frontier remains `wip/hd2d-point15-recovery-20260612`; the continuous wording in older handoff/status text is historical context, not a separate branch for this recovery line.
- The previous cycle proved the composition grammar on F1-F6 Ruins, but the all-map contact sheet still showed A/B/C/D/E as generic far bands behind square playable boards.
- The quality failure was structural: several maps had distant mountains, but no area-specific terrain rhythm between the map edge and far panorama. More color tuning would have been plateau polish.
- This cycle therefore advanced Phase B of the graphics plan: roll the accepted distant composition grammar across every outdoor current/past map while preserving deterministic placement, renderer freeze, and review publishing discipline.

## Implementation

- Expanded the `DistantVista_CompositionPrototype` layer from Ruins-only to all six outdoor areas: House Exterior, Central Plaza, Mia House, Aria Street, Kaia Farm, and Ruins.
- Kept the per-map count at 9 deterministic meshes and assigned area-specific profile sequences so each outdoor map gets a different horizon rhythm:
  - House Exterior: lower terrain shelves and broken treelines with a restrained peak anchor.
  - Central Plaza: stronger peak clusters and valley cuts behind the public-square silhouette.
  - Mia House: lower homestead treelines and shelves.
  - Aria Street: route-oriented valley cuts and directional ridges.
  - Kaia Farm: broad low shelves and tree masses.
  - Ruins: retained the previous terrain shelf, broken treeline, peak cluster, and valley-cut composition.
- Added area-aware angle, width, height, depth, and vertical placement functions while leaving the accepted far-ring radius guard intact.
- Added a distant-vista renderer policy pass for outdoor roots and validation entry points. Later validation helpers mutate renderer shadow settings, so the distant vista policy is reapplied before distant validation checks the no-shadow contract.
- Did not change renderer features, renderer feature order, fog/sky renderer features, colliders, route logic, random placement, time/date-dependent placement, or runtime input contracts.

## Rejected Iterations

- r1 put composition prototypes too close to the map on House Exterior and failed the intended radius guard.
- r2-r4 failed distant-vista shadow validation even though generated renderers started with `ShadowCastingMode.Off`. Debugging proved another validation path changed renderer shadow state after generation.
- r5 was a debug-only validation pass to compare generated and post-mutation renderer policy state. The debug logging was removed before the accepted implementation.
- r6 still used negative radius offsets on House Exterior and failed the minimum-radius guard. The accepted pass keeps all area rollout prototypes inside the established distant ring.

## Verification

- Validate: `Logs/distant_composition_all_maps_validate_r7.log` passed with `Fast VS house slice validation passed.`
- Renderer freeze: `Logs/distant_composition_all_maps_editmode_r3.xml` passed `36/36`; `RendererFeatureSet_MatchesFrozenBaseline` result is `Passed`.
- Asset validation: `Logs/distant_composition_all_maps_asset_validation_r1.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/distant_composition_all_maps_capture_r1.log` produced the 13 all-map Wide review PNGs.
- Review packet: `docs/review/2026-06-15T05-52_distant_composition_all_maps/` contains the 13 all-map frames, `00_contact_sheet.png`, and `devlog.txt`.
- Shotdiff: `Logs/shotdiff/distant_composition_all_maps_vs_prototype_r1` compared against `docs/review/2026-06-15T03-49_distant_composition_prototype`. The changed frames were `00_contact_sheet.png` at `57.0978%`, `01_a1_a2_current.png` at `2.8808%`, `03_b1_b3_current.png` at `4.2984%`, `05_c1_c3_current.png` at `3.0055%`, `07_d1_d3_current.png` at `1.4832%`, and `09_e1_e3_current.png` at `1.4542%`. Ruins remained unchanged from the prior accepted prototype; the prior parallax-only proof frames are intentionally absent from this standard all-map packet.
- Visual review: the current-side outdoor maps now carry a visible composed mountain/treeline/valley horizon instead of only a repeated far band. The rollout is accepted as Phase B progress, but not as production final: the playable map still reads as a square board in several views, and the side-view frame remains too dark for final review.

## Next

- Start Phase C next: midground and map-edge closure. Add authored cliffs, waterline shelves, road continuations, background paths, settlement fragments, and near tree masses between the playable tile and the distant panorama.
- Treat the current water/void band as the next structural quality blocker. If the view stalls, inspect mesh distance, edge geometry, fog range, far clip, camera framing, and texel density before changing colors.
- Keep the bridge traversal guard from the previous cycle. Do not replace the direct bridge support with a puzzle route until a current-collapse/past-repair route has built-player proof.
- Continue publishing every visual cycle through Validate, renderer-freeze EditMode, asset validation, all-map capture, shotdiff, devlog, R2 review upload, public viewer verification, pathspec commit, and push.
