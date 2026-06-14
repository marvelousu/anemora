# HD2D distant midground valley breaks

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-14

## Investigation

- Rechecked the post-bridge review set after the first distant panorama quality pass.
- The previous pass removed the map-edge void and increased radial depth, but the wide frames still read as large authored bands instead of a production vista with near/mid/far separation.
- The first midground attempt used a low horizontal shelf. Validate exposed a naming collision with the runtime shadow policy because `Shelf` is treated as a shadow-casting surface. After renaming to `MidgroundValleyBreak`, the pass validated, but shotdiff showed almost no visual change, so the pass was rejected as a plateau.
- The accepted direction adds taller foreground/midground silhouettes rather than continuing color polish: valley-break terrain forms plus a closer coppice layer that interrupts the circular band and creates readable parallax-friendly depth in current and past reviews.

## Change

- Added deterministic `DistantVista_MidgroundValleyBreak` geometry at the inner vista radius for all Chapter 1 panorama areas.
- Added deterministic `DistantVista_ForegroundCoppice` silhouettes between the playable map edge and the previous foothill forest layer.
- Kept the new layers collider-free, render-layer scoped, and registered as non-arrival `PropOrFeature` landmarks.
- Added `Ch1Distant_Current/PastMidgroundValleyBreak` and `Ch1Distant_Current/PastForegroundCoppice` material families through the authored setup file only; no generated material assets are committed.
- Raised vista validation to require the extra two layers across every panorama area, so a future regression cannot silently return to the thinner ring.

## Graphics Plan

Phase 1A, distant vista structure: continue breaking the panorama into foreground coppice, valley terrain, foothill forest, near hills, mid treeline, and far peaks. Reject passes that only move colors while shotdiff and contact sheets still read as broad bands.

Phase 1B, per-area vista authorship: give each map a recognizable horizon profile. Exterior and Central Plaza need lower valley openings; Aria Street and Mia House need settlement/field hints; Kaia Farm needs cultivated bands; Ruins needs broken stone silhouettes and bridge-facing terrain breaks.

Phase 1C, parallax proof: add a small two-camera review capture for at least one map and measure foreground/mid/far relative motion. This confirms the vista is real 3D geometry instead of a flat sky painting.

Phase 2, vegetation production: replace primitive-looking plant reads with a small authored low-poly kit: trunks, canopies, shrubs, grass tufts, reeds, flowers, dead scrub, and stump/debris variants. Preserve existing placement coordinates first, then add deterministic area/index offsets only where composition needs density.

Phase 3, ground and building production: finish the Chapter 1 2K material split, then break tile repetition with edge chips, dirt lanes, wet/dry value variation, roof trim, plaster wear, and stone transition meshes. Keep new materials in `Ch1Ground_*` and `Ch1Surface_*` namespaces.

Phase 4, lighting and atmosphere: build per-time Volume and APV presets after geometry and materials read. Current should feel cooler and damaged; past should feel warmer and inhabited. Renderer Features remain frozen.

Phase 5, bridge traversal and puzzle: now that the traversal scaffold exists, move from support validation to actual route proof. The bridge must support F1-to-midpoint-to-F6 traversal, current-side collapse, past-side repair readability, and built-player evidence before the bridge puzzle is accepted.

Phase 6, operations and review hygiene: every visual cycle must update devlog, local review images, R2 manifest, live viewer, and shotdiff triage. A cycle is not complete if review images or devlog are absent, even when Unity validation passes.

## Verification

- Validate: `Logs/distant_foreground_coppice_validate_r2.log` passed with `Fast VS house slice validation passed.` and return code 0.
- Renderer freeze: `Logs/distant_foreground_coppice_editmode_r2.xml` passed 36/36 EditMode tests, including `RendererFeatureSet_MatchesFrozenBaseline`.
- Asset validation: `Logs/distant_foreground_coppice_asset_validation_r1.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/distant_foreground_coppice_capture_r2.log` produced the Cycle05 all-map Wide set in `docs/review/2026-06-14T19-26_distant_midground_valley_breaks/`.
- Shotdiff: `Logs/shotdiff/distant_foreground_coppice_vs_bridge_scaffold_r2` compared against `docs/review/2026-06-14T17-54_bridge_traversal_scaffold`. Current frames moved by roughly 0.65-1.66%; past frames moved by roughly 1.02-2.06% after the past material contrast correction. `00_contact_sheet.png` is a size mismatch because the review contact-sheet layout differs from the previous compact sheet.
- Visual review: the contact sheet shows added near/mid vegetation and valley breaks across current and past wide captures. The pass improves depth, but the next vista cycle should add per-area horizon authorship rather than more global ring layers.
- Side effects: Unity dirtied `link.xml`, generated material assets, texture/meta files, Volume assets, and tracked screenshot files during batch validation/capture. All unintended changes were reverted; only the authored setup file and devlog/index updates are staged for commit.
