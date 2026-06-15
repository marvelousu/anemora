# HD2D distant vista production-depth prototype

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-15

## Investigation

- Continued the environment uplift on the active `wip/hd2d-point15-recovery-20260612` line. The earlier continuation branch remains historical/contextual; this cycle stayed on point15 recovery.
- Rechecked the latest all-map review packet and confirmed the main distant-view blocker is still the House Exterior panorama reading as broad low-poly bands. Vegetation and nearfield detail have improved, but the upper far ring still dominated the shot.
- Tested a small production-depth prototype first. The first pass added real geometry, but shotdiff showed only subtle top-edge movement. The visible issue was not color polish; the existing far ring was visually overpowering the added pieces.
- Adjusted the prototype toward structure: closer foreground canopy folds, horizon terraces, far peak/foothill meshes, and textured Exterior-only distant bands. The accepted pass changes House Exterior only while leaving every other map unchanged.
- Bridge status: `ValidateHouseSliceBatch` still includes the current/past bridge traversal validation path. It passed after this change, so the route scaffold is not broken. This is still not a substitute for built-player traversal evidence.

## Change

- Added `DistantVista_ProductionDepth` meshes for House Exterior only:
  - foreground canopy folds near the 62m ring,
  - foothill ridge facets,
  - back-peak masses,
  - one valley-mouth layer,
  - horizon-terrace shelves that break the central band.
- Swapped House Exterior base distant bands and foothill forest to `Ch1Distant_*HouseExterior*Textured` PixelMaterial variants while keeping the existing flat-band materials for the other maps.
- Added validation guards so the House Exterior production-depth prototype requires:
  - 14 production-depth meshes,
  - at least 7 visible production-depth meshes in the wide review camera,
  - textured materials on production-depth meshes,
  - textured materials on the Exterior base panorama bands and foothill forest.
- Kept the work deterministic: no `Random`, `Time`, or `DateTime` placement was introduced. Placement is derived from area/index/seed salts only.
- Kept the distant vista visual-only: no colliders, no arrival landmarks, and no renderer-feature changes.

## Graphics Plan

Phase 1F, production-depth rollout: expand the House Exterior grammar to the remaining five outdoor maps after this prototype. Each area gets the same quality floor but a distinct silhouette family: civic terraces for plaza, residential garden ridges for Mia/Aria, field/orchard bands for Kaia, and broken stone/bridge-adjacent ridges for Ruins. Acceptance requires per-map wide-frame movement, visible non-flat silhouettes, and no new wall-like upper bands.

Phase 1G, panorama parallax proof: add a short camera offset proof for one or two maps after rollout. The far ring must separate into near/mid/far layers under camera movement, not behave like a flat sky card.

Phase 2D, vegetation species kit: continue replacing generic rounded vegetation with authored species families. Add reeds, saplings, scrub, orchard rows, dead branches, stump clusters, and map-specific canopy shapes. Preserve existing coordinates first; densify only after the replacement kit reads.

Phase 3A, terrain material density: push `Ch1Ground_*` and `Ch1Surface_*` 2K separation beyond House Exterior into the outdoor maps. Break repetition with shoulders, worn lanes, chipped stones, grass/dirt blends, waterline wetness, furrows, rubble dust, and current/past damage masks.

Phase 3B, constructed object depth: add roof fascia, under-eave planes, wall returns, stone banding, chipped plaster, window/door trims, bridge-side construction traces, fences, signboards, tools, and settlement props. The target is built volumes, not decorated boxes.

Phase 4, atmosphere and lighting: once geometry density reads, strengthen current/past contrast through allowed fog, skybox, Volume overrides, material response, and APV rebake only. Renderer Features remain frozen.

Phase 5, bridge player proof: keep the existing validation guard, then produce built-player evidence for F1 to midpoint to F6 traversal. The bridge must prove current-side damage/readability, past-side repair/readability, route continuity, and pickup/placement state behavior before the route is considered complete.

Phase 6, review hardening: every visual cycle must finish with Validate, EditMode renderer freeze, asset validation, all-map capture, shotdiff, visual review, devlog, review packet with `devlog.txt`, R2 upload, viewer verification, pathspec commit, and push.

## Verification

- Validate: `Logs/distant_vista_production_depth_validate_r5.log` passed with `Fast VS house slice validation passed.` The same pass covers the bridge traversal validator.
- Renderer freeze: `Logs/distant_vista_production_depth_editmode_r5.xml` passed 36/36 EditMode tests, including `RendererFeatureSet_MatchesFrozenBaseline`.
- Asset validation: `Logs/distant_vista_production_depth_asset_validation_r5.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/distant_vista_production_depth_capture_r5.log` produced 13 all-map Wide PNGs in `docs/devlog/screenshots/chapter1_all_maps_cycle05`.
- Review packet: `docs/review/2026-06-15T18-24_distant_vista_production_depth/` contains the 13 all-map frames and `00_contact_sheet.png`.
- Shotdiff: `Logs/shotdiff/distant_vista_production_depth_vs_vegetation_branching_r5/` compared against `docs/review/2026-06-15T17-49_vegetation_branching`. Only the intended House Exterior frames changed: `01_a1_a2_current.png` 3.0053% and `02_a1_a2_past.png` 0.9392%. All other map frames remained 0.0000%; the contact sheet is a size mismatch due to regenerated layout.
- Visual review: House Exterior now shows a more authored distant vista with textured distant bands and stronger layered foreground/mid/far silhouettes. The other maps remain unchanged in this one-map prototype cycle.

## Next

- Upload this review packet to R2, verify public viewer propagation, commit/push this cycle, then roll the production-depth grammar across the other outdoor maps.
- After the all-map distant rollout, return to built-player bridge traversal proof so the bridge is validated by runtime evidence, not only editor-side route guards.
