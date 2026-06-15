# HD2D distant vista all-map rollout

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-15

## Investigation

- Continued on the active point15 recovery line. The earlier continuation branch remains historical/contextual; this cycle kept the implementation on `wip/hd2d-point15-recovery-20260612`.
- Reviewed the previous House Exterior production-depth packet and confirmed the issue after rollout would not be a simple color-polish problem. The distant panorama needed all-map geometry coverage, but the first all-map pass also exposed a texel-density problem: high-frequency `Stone`/`Grass` repeat textures read as visible grids and then as large square patches once the tiling was reduced.
- Treated that as a composition/material-scale issue, not a palette issue. The accepted pass keeps real 3D distant geometry and parallax candidates, but moves the far rings toward atmospheric macro surfaces so the silhouettes, depth bands, tree line, waterline, and fog carry the view.
- Bridge status remains separated from visual acceptance: `ValidateHouseSliceBatch` still covers the bridge traversal validator path, and it passed in this cycle. That is useful guard coverage, but it is not yet built-player proof that the bridge can be physically crossed in the runtime build.

## Change

- Rolled the production-depth distant vista grammar from House Exterior to every outdoor Chapter 1 map:
  - House Exterior,
  - Central Plaza,
  - Mia House,
  - Aria Street,
  - Kaia Farm,
  - Ruins.
- Increased the production-depth mesh budget from 14 to 18 meshes per eligible map and raised the wide-camera visibility guard from 7 to 9 visible meshes.
- Added area-aware silhouette controls for angle bias, angle scale, radius scale, width scale, height scale, and height bias. The maps now share the same quality floor while keeping distinct horizon placement and massing.
- Added area-specific production-depth material ids such as `Ch1Distant_CurrentRuinsProductionDepthBackPeak`, avoiding shared generated material collisions across maps.
- Rebalanced the distant material scale after visual review:
  - removed visible `Stone` grid patterns from far peaks and back ridges,
  - switched far/back production-depth layers to low-contrast `Noise`,
  - reduced distant texture tiling to atmospheric macro scale so far rings render as authored landform planes rather than repeating wallpaper,
  - preserved generated texture references so validation still catches missing textured materials.
- Kept the work visual-only: no colliders, no gameplay triggers, no new renderer features, and no procedural sky/backdrop path.

## Graphics Plan

Phase 1G, distant vista mesh authoring: keep this all-map rollout as the wide-frame foundation, then replace the remaining obviously generated mountain/card shapes with authored low-poly landform kits. Build one approved kit first, then expand: ridge shelves, soft valley saddles, broken peaks, low shore banks, forest crowns, and ruin-side stone silhouettes. Acceptance requires no void at map edges, circular coverage in wide shots, readable foreground/mid/far parallax candidates, and no visible texture wallpaper.

Phase 1H, parallax and camera proof: add a camera-offset review method for at least House Exterior and Ruins. The distant ring must show real layer separation under camera movement. If it reads like a flat sky plane, fix mesh radius/depth/farClip/fog before adding more colors.

Phase 2, vegetation authored kit: replace remaining primitive-like vegetation with a small authored species kit. Use current coordinates first, then densify only after replacement is readable. Required families: broadleaf tree, thin sapling, hedge row, reed cluster, orchard row, dead branch, stump, scrub clump, and ruin overgrowth. Current/past variants should differ by health, density, hue, and silhouette, not only material tint.

Phase 3, ground and surface density: push the 2K `Ch1Ground_*` and `Ch1Surface_*` material separation across every outdoor map. Break the uniform tile read with path shoulders, worn lanes, chipped stone, dirt/grass blends, wet waterline bands, furrows, rubble dust, moss seams, and current/past damage masks. New materials stay in the `Ch1Ground_*` / `Ch1Surface_*` namespace and do not mix into existing cycle materials.

Phase 4, constructed objects and settlement depth: add authored roof fascia, under-eave planes, wall returns, stone banding, chipped plaster, trim, door/window depth, fences, tools, signboards, carts, laundry, field equipment, and bridge-side construction traces. The target is built volume and material separation, not decorated boxes.

Phase 5, bridge traversal and bridge art: keep the editor validator, but produce built-player traversal proof from F1 to midpoint to F6 in both current and past states. In the same phase, make the bridge read as a route: planks, rail breaks, supports, repaired past-side spans, damage cues, approach ramps, collision continuity, and pickup/placement state behavior. Acceptance is runtime crossing evidence, not only static screenshots.

Phase 6, lighting and atmosphere: after geometry and materials read, strengthen current/past contrast with allowed fog, skybox, Volume overrides, material response, and APV rebake. Renderer Features remain frozen. Acceptance requires the same geometry to read as colder/current and warmer/past without hiding the scene in fog.

Phase 7, review pipeline hardening: every graphics cycle must finish with Validate, EditMode renderer freeze, AssetValidation, all-map Wide captures, shotdiff triage, visual review, devlog, `docs/review/<cycle>/devlog.txt`, R2 upload, viewer refresh, pathspec commit, and push. If the viewer marker file is stale, verify the actual review/gallery/devlog routes before treating the deploy as broken.

## Verification

- Validate: `Logs/distant_vista_rollout_validate_r6.log` passed with `Fast VS house slice validation passed.` The same pass includes the bridge traversal validator.
- Renderer freeze: `Logs/distant_vista_rollout_editmode_r6.xml` passed 36/36 EditMode tests, including `RendererFeatureSet_MatchesFrozenBaseline`.
- Asset validation: `Logs/distant_vista_rollout_asset_validation_r6.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/distant_vista_rollout_capture_r6.log` produced 13 all-map Wide PNGs in `docs/devlog/screenshots/chapter1_all_maps_cycle05`.
- Review packet: `docs/review/2026-06-15T20-17_distant_vista_all_map_rollout/` contains the 13 all-map frames, `00_contact_sheet.png`, and `devlog.txt`.
- Shotdiff: `Logs/shotdiff/distant_vista_all_map_rollout_vs_production_depth_r6/` compared against `docs/review/2026-06-15T18-24_distant_vista_production_depth`. The all-map wide frames changed as expected: `01` 2.8549%, `02` 2.4193%, `03` 1.6480%, `04` 1.1551%, `05` 2.7150%, `06` 3.5109%, `07` 2.2457%, `08` 2.0807%, `09` 1.4602%, `10` 1.4528%, `11` 1.7473%, `12` 1.9905%. `13_scene6_sideview_auto.png` stayed unchanged at 0.0000%.
- Visual review: every outdoor current/past wide frame now has non-void distant coverage. The accepted r6 pass removes the most distracting repeat-grid/patch artifacts from the far rings by making the remote landforms read as atmospheric planes.

## Next

- Upload this review packet to R2, refresh anemora-viewer, and verify the public review/gallery/devlog routes.
- Start the next cycle on authored distant landform kits or bridge runtime traversal proof. If bridge crossing is still not physically possible in the built player, prioritize collision/ramp/path continuity over additional visual dressing.
