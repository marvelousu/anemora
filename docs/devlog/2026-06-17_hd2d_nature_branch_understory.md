# HD2D nature branch understory

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-17

## Context

- The user asked today's work to push distant scenery and natural graphics toward a more realistic look. The accepted nature-stand packet improved the forest ring, but the first branch/understory attempt produced near-zero shotdiff and therefore did not satisfy the plateau guard.
- This cycle kept the renderer feature set frozen and stayed in the authored deterministic setup file. No Random/Time/DateTime placement was introduced.
- Blender/Meshy MCP tools were not exposed in this Codex session, so this slice used deterministic Unity meshes and existing approved materials.

## Change

- Added visible distant branch-trace and understory layers to each natural distant panorama segment across all outdoor maps, current and past.
- Moved those layers slightly toward the camera-facing side of the vista ring and increased branch/understory scale so the forest edge reads as individual trees instead of a flat green band.
- Added occasional emergent branch leaders inside the distant tree stands to break the smooth canopy silhouette.
- Refined local authored trees by splitting the crown shape with stronger leaf fans and a smaller upper branch reveal, reducing the primitive round-tree read without changing existing placement coordinates.
- Extended validation to require the new distant branch-trace and understory objects and camera visibility minimums.

## Visual Review

- Accepted packet: `docs/review/2026-06-17T12-49_nature_branch_understory_r4/`.
- Earlier r2/r3 captures were discarded because r2 was visually invisible and r3 made the near-tree upper branches too black/heavy. r4 keeps the visible crown breakup while reducing the black branch noise.
- Shotdiff versus `docs/review/2026-06-17T10-45_nature_realism_stands_r2/` changed 10 of 12 wide current/past map captures, with the largest map-level deltas on Exterior and MiaHouse where the revised tree crowns are most visible. KaiaFarm remained nearly unchanged because the visible trees in that framing are less affected.
- White-haze check: `Logs/nature_branch_understory_image_metrics_r4.txt` reports `broadWhiteHaze=no`; max bright-white ratio is 0.2438%.
- Black-surface check: the same metrics report `buildingBlackRegression=no-broad-nearblack`; max near-black ratio is 2.0353% and the wide captures do not reproduce the previous all-building black surface failure.

## Verification

- Validate: `Logs/nature_branch_understory_validate_r2.log` passed with `Fast VS house slice validation passed.` and return code 0.
- Build: `Logs/nature_branch_understory_build_r3.log` passed with `Fast VS house slice validation passed.`, `Build Finished, Result: Success.`, and rebuilt `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`.
- Renderer freeze: `Logs/nature_branch_understory_editmode_r2.xml` passed all 36 EditMode tests, including `RendererFeatureSet_MatchesFrozenBaseline`.
- Asset validation: `Logs/nature_branch_understory_asset_validation_r2.log` passed with `[AssetValidation] OK`.
- Built-player capture: `Logs/nature_branch_understory_player_capture_r4.log` wrote 13 PNGs to `docs/review/2026-06-17T12-49_nature_branch_understory_r4/` and logged the frozen renderer contract.
- Shotdiff: `Logs/shotdiff/nature_branch_understory_vs_stands_r4/summary.txt` records the accepted visual delta versus the previous nature packet.
- Image metrics: `Logs/nature_branch_understory_image_metrics_r4.txt` recorded opaque alpha for every screenshot and no broad bright-white region.

## Next

- Continue with higher-fidelity generated tree assets when a generator/MCP tool is available, especially for the large near/mid-distance trees still visible as low-poly clusters.
- Replace the remaining distant low-poly mountain/treeline bands with richer authored forms rather than only color or fog tuning.
- Keep the white-haze, black-surface, and renderer-freeze checks in every visual cycle.
