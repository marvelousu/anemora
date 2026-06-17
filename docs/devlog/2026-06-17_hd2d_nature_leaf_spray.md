# HD2D nature leaf spray

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-17

## Context

- The previous conifer/strata packet improved the far panorama, but several near and mid-distance trees still read as rounded green masses in the all-map review sheet.
- This cycle stayed in the deterministic authored setup file and did not introduce new material assets, renderer features, Random/Time/DateTime placement, or generator-dependent assets.
- The first leaf-spray capture was visually too small and stayed below the shotdiff threshold on every map, so it was treated as a plateau guard miss and revised before acceptance.

## Change

- Added deterministic `LeafSpray` mesh generation for sparse outer-leaf quads that break the spherical tree silhouette.
- Added `LeafSprayA`, `LeafSprayB`, `LeafSprayTop`, and a small `BranchSprayA` to the main authored low-poly tree template.
- Added matching `LeafSprayA`, `LeafSprayB`, and `BranchSprayA` detail to Phase2 vegetation groves and farm orchard trees so the natural treatment is consistent across maps.
- Extended validation to require representative `LeafSpray` objects on Chapter 1 tree samples, Phase2 groves, and HouseExterior primary/secondary trees.

## Visual Review

- Accepted packet: `docs/review/2026-06-17T15-24_nature_leaf_spray_r2/`.
- Contact sheet: `docs/review/2026-06-17T15-24_nature_leaf_spray_r2/00_contact_sheet.png`.
- The tracked `docs/devlog/screenshots/chapter1_all_maps_cycle05/` overwrite was restored after the accepted images were copied into the review packet, matching the visual-cycle commit discipline.
- Discarded packet: `docs/review/2026-06-17T14-57_nature_leaf_spray_r1/`; r1 was too subtle and shotdiff reported all captures below the 0.5% change budget.
- Shotdiff versus `docs/review/2026-06-17T13-55_nature_conifer_strata_r1/` changed 5 of 12 wide current/past captures above threshold, with the strongest visible deltas on HouseExterior and MiaHouse where larger near trees frame the map.
- White-haze check: `Logs/nature_leaf_spray_image_metrics_r2.txt` reports `broadWhiteHaze=no`; max bright-white ratio is 0.2066%.
- Black-surface check: the same metrics report `buildingBlackRegression=no-broad-nearblack`; max near-black ratio is 3.1751%.

## Verification

- Validate: `Logs/nature_leaf_spray_validate_r3.log` passed with `Fast VS house slice validation passed.` and no `error CS` entries.
- Build: `Logs/nature_leaf_spray_build_r2.log` passed with `Fast VS house slice validation passed.`, `Build Finished, Result: Success.`, and rebuilt `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`.
- Renderer freeze: `Logs/nature_leaf_spray_editmode_r2.xml` passed all 36 EditMode tests, including `RendererFeatureSet_MatchesFrozenBaseline`.
- Asset validation: `Logs/nature_leaf_spray_asset_validation_r2.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/nature_leaf_spray_capture_r2.log` wrote 13 PNGs to `docs/devlog/screenshots/chapter1_all_maps_cycle05/`; those captures were copied into the accepted review packet.
- Shotdiff: `Logs/shotdiff/nature_leaf_spray_vs_conifer_strata_r2/summary.txt` records the accepted visual delta versus the previous nature packet.
- Image metrics: `Logs/nature_leaf_spray_image_metrics_r2.txt` recorded opaque alpha for every screenshot and no broad bright-white or near-black regression.

## Next

- Replace the most exposed near-tree crowns with richer generated or hand-authored tree assets when generator/MCP tooling becomes available.
- Continue pushing the far panorama toward less stylized terrain and tree species variation while preserving the renderer freeze.
- Keep the plateau guard active: subtle leaf polish should not be accepted unless the review sheet or shotdiff shows it.
