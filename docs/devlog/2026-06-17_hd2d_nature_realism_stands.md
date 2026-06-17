# HD2D nature realism stands

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-17

## Investigation

- The user asked to shift today's work toward realistic distant scenery and natural graphics. The previous accepted packet improved building surfaces, but the distant vista still read as a dark low-poly band in many wide captures.
- No Blender/Meshy MCP tool is exposed in this Codex session. The repository still has Meshy helper scripts and API environment access, but this cycle stayed inside deterministic Unity mesh/material generation to avoid blocking the review loop.
- The accepted direction for this slice is to add visible authored natural layers first, then continue with richer generated assets in the next cycle if the visual plateau remains.

## Change

- Added a deterministic natural tree-stand layer to every distant panorama vista segment in all outdoor maps, current and past.
- Each segment now has separate distant canopy, leaf-surface accent, and trunk meshes so the forest ring no longer relies only on flat treeline silhouettes.
- Increased distant tree-stand density and height variation from 13 to 17 procedural tree forms per segment, with extra lobe layers on selected trees.
- Added dedicated generated materials and textures:
  - `Ch1Distant_CurrentNaturalCanopy`
  - `Ch1Distant_CurrentNaturalCanopyAccent`
  - `Ch1Distant_CurrentNaturalTrunk`
  - `Ch1Distant_PastNaturalCanopy`
  - `Ch1Distant_PastNaturalCanopyAccent`
  - `Ch1Distant_PastNaturalTrunk`
- Improved authored local tree meshes by adding branch-fork and leaf-fan submeshes, and raised trunk/canopy mesh density so existing trees read less like primitive blobs.
- Extended validation so every distant vista requires natural canopy/accent/trunk triplets and visible canopy accents in the wide review camera.

## Visual Review

- Accepted packet: `docs/review/2026-06-17T10-45_nature_realism_stands_r2/`.
- All 12 wide current/past map captures were refreshed; `13_scene6_sideview_auto.png` stayed unchanged, as expected.
- White-haze check: the previously reported broad white haze is not present in this packet. Bright-pixel ratios remain near zero in the new image metrics.
- Black-surface check: the previous all-building black material failure is not reproduced in the wide packet.
- The distant ring now contains authored tree masses, trunk strips, and green canopy accents. It is improved but still not final realistic-quality vegetation; the next cycle should push stronger generated/tree asset fidelity rather than only color tuning.

## Verification

- Validate: `Logs/nature_realism_validate_r2.log` passed with `Fast VS house slice validation passed.` and return code 0.
- Build: `Logs/nature_realism_build_r2.log` passed with `Fast VS house slice validation passed.`, `Build Finished, Result: Success.`, and rebuilt `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`.
- Renderer freeze: `Logs/nature_realism_editmode_r3.xml` passed all 36 EditMode tests, including `RendererFeatureSet_MatchesFrozenBaseline`.
- Asset validation: `Logs/nature_realism_asset_validation_r2.log` passed with `[AssetValidation] OK`.
- Built-player capture: `Logs/nature_realism_player_capture_r2.log` wrote 13 PNGs to `docs/review/2026-06-17T10-45_nature_realism_stands_r2/` with the frozen renderer contract logged.
- Shotdiff: `Logs/shotdiff/nature_realism_stands_vs_architectural_surface_accents_r2/summary.txt` changed all 12 wide map images versus `docs/review/2026-06-17T08-12_architectural_surface_accents_r6/`, while the sideview remained unchanged.
- Image metrics: `Logs/nature_realism_image_metrics_r2.txt` recorded opaque alpha for every screenshot and no broad bright-white region.

## Next

- Continue nature realism with higher-fidelity generated tree assets where available, especially near/mid-distance trees that are large enough to judge in screenshots.
- Add species variation and non-uniform forest silhouettes without adding renderer features.
- Keep the white-haze and black-surface checks active while improving lighting/material response.
