# HD2D distant area signatures

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-15

## Investigation

- Re-read the environment uplift handoff, the recent distant panorama records, the bridge scaffold record, and the current authored setup implementation before editing.
- The previous accepted vista cycles established a real low-poly 3D panorama ring, terrain apron, valley threads, foreground coppices, foothill forest, and area landmarks. The remaining gap was not another global ring; the maps still needed more authored local identity in the horizon.
- The first area-signature attempt placed 4 per-area meshes at the farther landmark distance. Validate passed, but shotdiff showed only the contact sheet over budget and individual frames topped out at 0.4280%, so the pass was rejected as a plateau.
- The accepted direction moved the signatures forward, widened them, raised their silhouettes, and increased their material contrast. This is a structural visibility change, not a color-only polish pass.
- Bridge investigation remains separate from this vista cycle: the F1-to-F6 bridge support scaffold now validates colliding `PathOrFloor` surfaces, but the full route/puzzle acceptance still needs built-player route evidence, current-side collapse blocking, past-side repair readability, and pickup/placement state proof.

## Change

- Added deterministic `DistantVista_AreaSignature` geometry for every Chapter 1 panorama area in current and past roots.
- Added 4 authored signature meshes per area, with profiles chosen by map:
  - Exterior and Central Plaza: lower valley openings mixed with settlement/roofline pulses.
  - Mia House and Aria Street: field/settlement horizon hints.
  - Kaia Farm: cultivated terrace bands plus one roofline pulse.
  - Ruins: broken stone silhouettes and bridge-facing terrain breaks.
- Kept the new objects collider-free, render-layer scoped, and registered as non-arrival `PropOrFeature` landmarks.
- Added the `Ch1Distant_CurrentAreaSignature` / `Ch1Distant_PastAreaSignature` material family through the authored setup only; generated material assets are not committed.
- Raised distant panorama validation to require `DistantPanoramaVistaAreaSignatureCount` meshes in each panorama root, in addition to the existing rings, valley threads, coppices, forests, and area landmarks.

## Graphics Plan

Phase 1C, parallax proof: add a small two-camera review capture for one or two outdoor maps and report foreground/mid/far relative motion. This will prove the panorama is real 3D space and catch regressions that look good only from the one wide camera.

Phase 2, vegetation production: move from the current in-code authored low-poly plant placeholders toward a production vegetation kit. Keep existing placement coordinates first, replace mesh/material references with better trunks, canopies, shrubs, reeds, grass clusters, flower clumps, dead scrub, stumps, and debris variants, then add deterministic area/index offsets where density or composition needs it.

Phase 3, ground and building production: continue the 2K Chapter 1 surface split and add local breakup meshes: chipped edges, dirt lanes, wet/dry value variation, roof trim, plaster wear, stone transition strips, and non-repeating road shoulders. Keep new materials in `Ch1Ground_*` and `Ch1Surface_*` namespaces.

Phase 4, lighting and atmosphere: after geometry/material density is readable, tune current/past Volume and APV presets. Current should read cooler and damaged; past should read warmer and inhabited. Renderer Features stay frozen.

Phase 5, bridge route and puzzle: turn the bridge scaffold into actual traversal acceptance. Required proof: F1 -> midpoint -> F6 route in built player, blocked current-side shortcut where the span is collapsed, readable past-side repair through the time-window, stone pickup/placement state, and review captures showing the route from both sides.

Phase 6, operations: every visual cycle must publish the full packet: devlog, review directory with images, shotdiff triage, R2 upload, public viewer verification, and pathspec commit. A Unity batch pass without review images is not a completed visual cycle.

## Verification

- Validate: `Logs/area_signature_validate_r3.log` passed with `Fast VS house slice validation passed.` and return code 0.
- Renderer freeze: `Logs/area_signature_editmode_r4.xml` passed 36/36 EditMode tests, including `RendererFeatureSet_MatchesFrozenBaseline`.
- Asset validation: `Logs/area_signature_asset_validation_r3.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/area_signature_capture_r3.log` produced 13 all-map Wide PNGs, copied to `docs/review/2026-06-15T00-38_distant_area_signatures/` with `00_contact_sheet.png`.
- Shotdiff: `Logs/shotdiff/distant_area_signatures_vs_valley_threads_r3` compared against `docs/review/2026-06-14T22-51_distant_valley_threads`. Seven files changed over the 0.5% budget: `00_contact_sheet.png`, `01_a1_a2_current.png`, `03_b1_b3_current.png`, `05_c1_c3_current.png`, `07_d1_d3_current.png`, `09_e1_e3_current.png`, and `11_f1_f6_current.png`.
- R2 review upload: `tools/r2/r2-upload-review.ps1 -CycleDir docs/review/2026-06-15T00-38_distant_area_signatures -Branch wip/hd2d-point15-recovery-20260612` uploaded 16 files; the branch manifest now lists 128 paths.
- Visual review: current outdoor maps now show distinct larger near-horizon signatures instead of only global bands. Past maps retain the existing warm distant silhouettes; their new signature material is generated and byte-level frames changed, but the old past vista was already close enough that shotdiff remains under 0.5% for individual past frames.
- Side effects: Unity dirtied `link.xml`, generated material assets, material/texture metadata, Volume assets, and tracked screenshot files during validation/capture. All unintended changes were reverted; only the authored setup file and devlog/index updates are intended for commit.
