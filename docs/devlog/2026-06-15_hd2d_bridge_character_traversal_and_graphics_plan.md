# HD2D bridge character traversal and graphics plan

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-15

## Investigation

- The active branch remains `wip/hd2d-point15-recovery-20260612`. The old continuation wording is historical/contextual; this cycle continues the point15 recovery branch.
- The prior bridge support cycles proved object presence, material families, enabled colliders, and corridor clearance, but they still did not prove that the `CharacterController` could physically walk from F1 to F6.
- The bridge had two conflicting physical layers: the long low `F1_To_F6_Path` route was walkable, while the raised visible bridge deck, open walk line, thresholds, and midpoint pier had colliders at character-capsule height. That could let route-clearance checks pass while the actual player movement gets stopped by raised `PathOrFloor` boxes.
- Visual review still shows the distant vista and vegetation moving in the right direction, but the authored-production gap is now in asset specificity: silhouette families, per-area midground composition, terrain texture density, building-surface material separation, and gameplay-readable route dressing.

## Implementation

- Kept the raised `Current/Past_CentralPlaza_Chapter1_F1_BridgeDeck` as non-blocking presentation geometry so the bridge keeps its visible shape without becoming a horizontal wall for the player capsule.
- Lowered the colliding bridge walk line, thresholds, and surviving midpoint pier into the same playable height band as the long route support.
- Added current and past `CharacterController` traversal validation for the bridge route. The new guard:
  - activates the Ruins map,
  - places the player in current or past time,
  - raycasts each F1-to-F6 waypoint for `PathOrFloor` support,
  - moves with `MovePlayerLocalForReview(..., useCharacterController: true)` in small deterministic steps,
  - fails if movement is blocked, if the player falls out of the bridge height band, if the active time side changes, or if F6 is not reached.
- No renderer features, renderer ordering, runtime input contracts, or procedural sky paths were changed.

## Verification

- Validate: `Logs/bridge_character_traversal_validate_r1.log` passed with `Fast VS house slice validation passed.` The pass includes the new current/past CharacterController bridge traversal guard.
- Renderer freeze: `Logs/bridge_character_traversal_editmode_r2.xml` passed `36/36`; `RendererFeatureSet_MatchesFrozenBaseline` result is `Passed`.
- Asset validation: `Logs/bridge_character_traversal_asset_validation_r1.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/bridge_character_traversal_capture_r1.log` produced 13 all-map Wide PNGs, copied to `docs/review/2026-06-15T03-06_bridge_character_traversal/` with `00_contact_sheet.png`.
- Shotdiff: `Logs/shotdiff/bridge_character_traversal_vs_phase2_vegetation_r2` compared against `docs/review/2026-06-15T02-17_phase2_vegetation_volume`. Individual frames stayed below the 0.5% review budget; only `11_f1_f6_current.png` changed `0.0136%` and `12_f1_f6_past.png` changed `0.0122%`. The contact sheet changed because it was regenerated.
- Visual review: current and past F1/F6 frames keep the bridge readable after the collision-layer cleanup; no void, clipping wall, or obvious bridge visual regression was observed.

## Graphics Uplift Plan

Phase A, distant vista authoring reset: choose one representative map, then rebuild the far ring from generic low-poly bands into authored silhouette families: valley shelves, broken treelines, peak clusters, lower berms, and area-specific landmarks. Acceptance is not color polish; it requires visible parallax, non-repeating silhouettes, and no wall-like banding. If it does not read, inspect mesh quality, band distance, fog range, far clip, and camera framing before touching color constants.

Phase B, per-area panorama rollout: after Phase A reads on one map, roll the same vista grammar across all outdoor current/past maps. Each area gets a distinct distant signature but shares material discipline and deterministic placement. Capture every map pair and reject a rollout if only the contact sheet changes.

Phase C, midground and map-edge closure: replace hard map edges with authored intermediate geography: low cliffs, road continuations, waterline shelves, background paths, settlement fragments, and tree masses that sit between the playable tile and far panorama. This is where the map stops feeling like a square board floating in space.

Phase D, vegetation production kit: replace remaining primitive-looking vegetation with a small authored species kit: low-poly trunks, canopy clusters, reeds, grass clumps, dead scrub, and flower/seed heads. First map establishes scale, silhouette, collider policy, and material palette; rollout reuses existing coordinates before adding density passes.

Phase E, ground and surface material pass: finish the `Ch1Ground_*` / `Ch1Surface_*` 2K material separation, then break tile repetition with edge decals, soil-to-stone blends, worn lanes, puddle/dust masks, and current/past damage variants. Keep these materials out of legacy cycle material names.

Phase F, building authored depth: add modular wall returns, roof fascia, window/door trims, under-eave shadow planes, damage chunks, and inhabited-past details. This phase should make buildings read as constructed volumes rather than decorated boxes.

Phase G, route and bridge playability: keep this cycle's bridge traversal guard, then build the full bridge puzzle in a later cycle: current collapse, past repair readability, midpoint pier time-window hop, and built-player route proof from F1 to midpoint to F6. Do not remove the current direct support until the puzzle proof replaces it.

Phase H, lighting and atmosphere: use RenderSettings, Volume overrides, and APV rebake only. Current should be cooler, lower, and more damaged; past should be warmer, clearer, and more inhabited. Renderer features remain frozen.

Phase I, review operations hardening: every visual cycle must publish the full packet: Validate, EditMode renderer freeze, asset validation, all-map capture, shotdiff triage, devlog, review directory, R2 upload, public viewer verification, pathspec commit, push. A local Unity pass without review images and devlog is not accepted.

Phase J, production closeout: once all outdoor maps meet the above, run a final all-map acceptance pass for distant vista, vegetation, ground/building surfaces, lighting contrast, and route playability. Only then start the separate authored-file reduction task.
