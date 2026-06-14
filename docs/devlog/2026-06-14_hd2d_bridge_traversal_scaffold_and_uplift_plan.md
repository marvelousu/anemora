# 2026-06-14 HD2D bridge traversal scaffold and uplift plan

Scope: continue the environment uplift after the distant-panorama quality pass, investigate the remaining graphics plan, and address the Chapter 1 ruins bridge traversal gap without changing the frozen renderer feature set.

## Investigation

- The live review viewer now updates from R2 after a Cloudflare Pages deploy-hook rebuild. The direct R2 manifest is the source of review images; the viewer consumes it at build time, so an upload after an Anemora push must be followed by a rebuild trigger.
- The prior distant vista pass removed the visible void and pushed the panorama farther out, but the next visual bottlenecks are still authored silhouette quality, near-ground repetition, low vegetation variety, and map-specific foreground/midground composition.
- The bridge code already had a visible F1 bridge deck and a logical F1-to-F6 path. The gap was that the bridge visual surfaces were non-colliding, and route validation only proved absence of blocking colliders, not that the bridge deck, thresholds, or midpoint pier existed as walkable support.
- Chapter 1 canon still calls for the full S5 bridge puzzle: current-side collapse, past-side repaired bridge, a surviving midpoint pier, two time-window hops, and a current-to-past stone repair beat. This cycle does not claim that full puzzle as complete; it establishes the traversal scaffold and guards the midpoint-pier route so the next bridge puzzle cycle has a stable base.

## Implementation

- Made `Current/Past_CentralPlaza_Chapter1_F1_BridgeDeck` keep colliders while remaining `PathOrFloor`, so the visible deck is no longer just presentation geometry.
- Made `BridgeOpenWalkLine`, both bridge thresholds, and the new `BridgeMidPierSurvives` colliding `PathOrFloor` surfaces in both current and past spaces.
- Split the ruins route validation from one broad "F1 arrival to F6" check into two checks: F1 arrival to the bridge midpoint pier, then midpoint pier to F6.
- Added `ValidateChapter1BridgeTraversalScaffold`, which requires the bridge deck, open walk line, thresholds, and midpoint pier to keep enabled colliders, expected material families, `PathOrFloor` landmarks, scale ranges, and authored local heights.

## Graphics Plan

Phase 1, distant vista quality: move from a generic ring to authored per-area silhouettes. Add silhouette families for ridges, valley shelves, treelines, and far peak breaks; tune fog/far clip only after geometry reads; review wide current/past captures and reject any pass that collapses back into wall-like bands.

Phase 2, authored vegetation: replace remaining primitive-looking shrubs/tufts with a small deterministic species kit: low-poly trunk/canopy trees, grass clumps, reeds, flowers, and dead scrub. Keep existing placement coordinates first, then add per-map density passes once the silhouettes read.

Phase 3, ground and building surface production: finish the 2K outdoor material separation, then de-grid walkable surfaces with edge decals, lane wear, soil/stone transitions, roof/facade trim, and current/past damage variants. Keep new material names in `Ch1Ground_*` / `Ch1Surface_*`.

Phase 4, lighting and atmosphere: build current/past Volume presets and APV bake targets per area. Current should read cooler, damaged, and lower-contrast; past should read warmer, inhabited, and materially cleaner. Renderer Features remain frozen.

Phase 5, bridge puzzle completion: remove the current-side direct bridge shortcut after the midpoint pier scaffold is proven; block collapsed spans; make past bridge halves readable through the time-window flow; add stone pickup/placement state; add built-player route evidence from F1 to midpoint to F6.

Phase 6, review operations: every visual cycle must produce a devlog, `docs/review/<cycle>/` images locally, R2 upload, viewer rebuild check, and shotdiff triage. A cycle is not accepted just because Validate passes.

## Verification

- Validate: `Logs/bridge_traversal_scaffold_validate_r1.log` passed with `Fast VS house slice validation passed.` and return code 0.
- Renderer freeze: `Logs/bridge_traversal_scaffold_editmode_r1.xml` passed 36/36 EditMode tests, including `RendererFeatureSet_MatchesFrozenBaseline`.
- Asset validation: `Logs/bridge_traversal_scaffold_asset_validation_r1.log` passed with `[AssetValidation] OK`.
- Review capture: `Logs/bridge_traversal_scaffold_capture_r1.log` produced the Cycle05 all-map Wide set in `docs/review/2026-06-14T17-54_bridge_traversal_scaffold/`.
- Shotdiff: `Logs/shotdiff/bridge_traversal_scaffold_vs_distant_panorama_quality` reported 14/14 unchanged versus `2026-06-14T17-14_distant_panorama_quality_uplift`; this is expected because the cycle changes bridge support/collider validation, not visible pixels.
- R2 review upload: `tools/r2/r2-upload-review.ps1 -CycleDir docs/review/2026-06-14T17-54_bridge_traversal_scaffold -Branch wip/hd2d-point15-recovery-20260612` uploaded 15 files and updated the branch manifest to 47 paths.
- Viewer pre-push check: the Cloudflare Pages deploy hook accepted manual trigger `c953a1f0-bfa3-4e57-903c-5772c0adf116`, but the live review page still showed 2 cycles / 28 images before the Anemora branch push. The branch push should retrigger the configured Anemora webhook and must be checked after push.
- Side effects: Unity dirtied `link.xml`, material/meta, Volume, and tracked screenshot files during batch validation/capture; all unintended changes were reverted, leaving only the authored setup file plus this devlog/index update staged for commit.
