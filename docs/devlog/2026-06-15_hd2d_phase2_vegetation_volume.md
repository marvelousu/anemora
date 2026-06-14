# HD2D Phase2 vegetation volume

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-15

## Investigation

- Re-read the current status, agent rules, environment uplift handoff, recent distant panorama records, and the bridge support validation record before editing.
- The distant panorama pass has removed the hard void and now gives every outdoor map a real 3D distant vista, but the local foreground and midground still depended too much on primitive-looking vegetation, sparse small tufts, and flat ground read. The wide review made the gap clear: character sprite quality is now ahead of authored environment density.
- The first Phase2 vegetation attempts added deterministic low-poly plant clusters to every outdoor map, but r1/r2 only moved most individual frames by less than 0.5%. That was treated as a plateau, not accepted polish.
- The accepted direction changed structure rather than tint: every outdoor map now gets larger authored grove silhouettes with trunks, multiple faceted canopy lobes, low canopy base, understory leaves, and grass blades. This creates readable tree forms from the wide camera while keeping collisions out of the route layer.
- Branch check: the current implementation branch is still `wip/hd2d-point15-recovery-20260612`. `AGENTS.md` says `docs/STATUS.md` is authoritative; `work/chapter1-continuation-*` remains Chapter 1 continuation history/content, not the active environment uplift line.
- Review publishing check: the repo already has a global discipline hook at `tools/review/validate-devlog-review-sync.ps1`, wired to `.github/workflows/review-sync-guard.yml`. R2 upload remains required because review images stay out of git.
- Bridge investigation remains open after this vegetation cycle. The current bridge support scaffold validates F1-to-F6 support objects and colliders, but actual traversal acceptance still requires built-player route proof, current-side collapse blocking, past-side repair readability, and stone pickup/placement state evidence.

## Change

- Added `CreateChapter1Phase2VegetationVolumeForOutdoorMaps` to the authored setup path after the distant panorama creation and before surface/lighting passes.
- Added six deterministic `Phase2VegetationVolume` clusters per outdoor map for both current and past spaces:
  - House exterior.
  - Central plaza.
  - Mia house exterior.
  - Aria street.
  - Kaia farm.
  - Ruins / bridge area.
- Added three larger `Phase2VegetationGrove` tree forms per outdoor map, each assembled from authored mesh parts: low-poly trunk, crown A, crown B, lower canopy, canopy base, understory leaves, and grass.
- Kept all new vegetation collider-free and marked as non-arrival `PropOrFeature` landmarks so visuals do not create accidental route blockers.
- Used deterministic placement and variation from area/object names only; no `Random`, `Time`, or `DateTime` placement.
- Tightened validation so every new plant and grove part must be a MeshFilter/MeshRenderer object using an authored mesh token (`LeafCluster`, `GrassBlade`, `LowPolyTrunk`, `LowPolyCanopy`, or `LowPolyBlossom`), with no cube/sphere mesh fallback and no colliders.
- Added a current-house exterior scale/height adjustment after r5 so grove trunks no longer read as vertical poles in the A current wide shot.

## Graphics Plan

Phase 1D, distant panorama production pass: keep the existing 3D ring, but move from "visible horizon" to authored vista composition. Work one map first, then all maps. Add parallax proof captures, taller foreground-to-midground terrain occluders, clearer water/valley cuts, area-specific horizon language, and fog/farClip checks. Acceptance: no void, circular panorama, visible near/mid/far depth, and a camera-motion proof that the vista is 3D rather than a flat sky image.

Phase 2B, vegetation kit replacement: replace the in-code authored placeholders with a proper low-poly vegetation kit when Meshy/Blender asset generation is available again. Start from the coordinates now established here, then swap mesh/material references for trunk, canopy, shrub, reed, grass, flower, stump, dry scrub, and fallen-branch variants. Acceptance: no primitive cube/sphere vegetation remains, every outdoor map has foreground/midground plant silhouettes, and route collision remains intentional.

Phase 2C, vegetation composition: add map-specific plant ecology instead of uniform scattering. House exterior gets domestic yard trees and fence-edge weeds; plaza gets trimmed civic greenery and ruined overgrowth; Mia/Aria get residential shrubs and street-edge saplings; Kaia gets orchard/field rows; ruins get bridge-edge scrub and reclaimed stone. Acceptance: each map is identifiable from vegetation alone in the contact sheet.

Phase 3A, ground texel density and breakup: make the 2K Chapter 1 ground materials carry the scene instead of acting as flat tiles. Add dirt lanes, grass-to-path shoulders, wet/dry patches, chipped stone edges, field furrows, route scuffs, and non-repeating edge strips. New materials stay in `Ch1Ground_*` / `Ch1Surface_*` namespaces.

Phase 3B, architecture and set dressing: raise building surfaces with authored trim, roof edges, eave thickness, wall wear, rubble piles, market props, farm tools, library stone bands, and house/fence contact detail. Acceptance: facades read as built objects from the wide camera, not boxes with materials.

Phase 4, lighting and atmosphere: after geometry density is readable, tune current/past Volume and APV presets. Current should read cooler, damaged, and lower-energy; past should read warmer, inhabited, and clearer. Renderer Features remain frozen; fog, skybox, Volume overrides, and APV are allowed.

Phase 5, bridge route and puzzle acceptance: turn the support scaffold into playable evidence. Build-player proof must include F1 -> bridge midpoint -> F6 traversal, blocked current-side shortcut at the collapsed span, readable past-side repair through the time window, stone pickup/placement state, and review images or video from both sides.

Phase 6, close-camera review: after wide maps pass, add close/composition captures for each map so foreground asset quality can be judged at player scale. This is where low-quality mesh silhouettes, bad material texel density, or collision/path readability problems should be caught.

Phase 7, publishing discipline: every visual cycle must finish the full packet: Validate, EditMode renderer freeze, asset validation, all-map capture, shotdiff triage, devlog, review directory, R2 upload, public viewer verification, pathspec commit, push. A Unity batch pass without review images is not considered a completed visual cycle.

## Verification

- Validate: `Logs/phase2_vegetation_volume_validate_r6.log` passed with `Fast VS house slice validation passed.` and return code 0.
- Renderer freeze: `Logs/phase2_vegetation_volume_editmode_r6.xml` passed 36/36 EditMode tests, including `RendererFeatureSet_MatchesFrozenBaseline`.
- Asset validation: `Logs/phase2_vegetation_volume_asset_validation_r6.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/phase2_vegetation_volume_capture_r6.log` produced 13 all-map Wide PNGs, copied to `docs/review/2026-06-15T02-17_phase2_vegetation_volume/` with `00_contact_sheet.png`.
- Shotdiff: `Logs/shotdiff/phase2_vegetation_volume_vs_area_signatures_r6` compared against `docs/review/2026-06-15T00-38_distant_area_signatures`. Ten files changed over the 0.5% budget: `00_contact_sheet.png`, `02_a1_a2_past.png`, `03_b1_b3_current.png`, `04_b1_b3_past.png`, `05_c1_c3_current.png`, `06_c1_c3_past.png`, `07_d1_d3_current.png`, `08_d1_d3_past.png`, `10_e1_e3_past.png`, and `12_f1_f6_past.png`.
- Visual review: r1/r2 were rejected as too subtle. r3 established large authored groves. r4 added low canopy bases. r5 made current grove crowns more readable. r6 adjusted the house exterior current grove height/scale so the A current shot no longer reads as vertical bare poles.
- Side effects cleaned before commit: Unity dirtied generated `link.xml`, generated materials, material/texture metadata, Volume assets, and tracked screenshot copies during validation/capture. These batch side effects were reverted with pathspecs; review images remain local/R2 artifacts under `docs/review/2026-06-15T02-17_phase2_vegetation_volume/`.
