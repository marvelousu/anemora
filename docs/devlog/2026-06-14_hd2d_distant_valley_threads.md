# HD2D distant valley threads

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-14

## Investigation

- Continued the distant-panorama quality pass after bridge support validation. The visible issue was that the real 3D vista now had a readable ring and area landmarks, but the midground still read as stacked horizontal bands rather than terrain with directional cuts.
- First `DistantVista_ValleyThread` attempt validated, but it was a low ground strip hidden under the existing apron/coppice layers. Shotdiff versus `2026-06-14T21-24_bridge_support_validation` showed only `00_contact_sheet.png` over budget at 0.8685%; individual frames topped out at 0.0027%. That pass was rejected as a plateau.
- The accepted version changes the layer from flat ground strips into visible low valley-wall cuts with vertical silhouette, keeping the same deterministic area/segment seed scheme.

## Change

- Added one deterministic `DistantVista_ValleyThread` mesh per distant-vista segment in every current/past panorama root.
- Positioned the layer at radius `62.8`, in front of the old midground/forest bands, so it reads in the all-map wide camera instead of disappearing behind the apron.
- Rebuilt the mesh as a 12-column x 4-row vertical cut with drifting notches and raised banks. This gives the panorama diagonal valley structure rather than another flat color band.
- Added `Ch1Distant_CurrentValleyThread` and `Ch1Distant_PastValleyThread` material factories in the authored setup file only.
- Updated distant-vista validation to require the new valley-thread count and the increased per-root authored segment total. The meshes remain collider-free, render-layer scoped, and registered as non-arrival `PropOrFeature` landmarks.

## Verification

- Validate: `Logs/distant_valley_thread_validate_r2.log` passed with return code 0. The earlier r1 validation also passed, but the pass was rejected visually because it plateaued.
- Renderer freeze: `Logs/distant_valley_thread_editmode_r2.xml` passed 36/36 EditMode tests, including `RendererFeatureSet_MatchesFrozenBaseline`.
- Asset validation: `Logs/distant_valley_thread_asset_validation_r2.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/distant_valley_thread_capture_r2.log` produced the Cycle05 all-map Wide set in `docs/review/2026-06-14T22-51_distant_valley_threads/`.
- Shotdiff: `Logs/shotdiff/distant_valley_threads_vs_bridge_support_r2` compared against `docs/review/2026-06-14T21-24_bridge_support_validation`. The accepted pass changed 13/14 PNGs: current frames moved by roughly 1.34-1.75%, past frames by roughly 0.66-1.38%, contact sheet by 1.8176%, and only the side-view auto frame stayed unchanged.
- Visual review: the contact sheet now shows visible dark/green valley-wall cuts across the distant midground, especially in Exterior, CentralPlaza, AriaStreet, KaiaFarm, and Ruins. The rejected r1 pass is kept only as local shotdiff evidence, not as accepted review output.
- R2 review upload: `tools/r2/r2-upload-review.ps1` uploaded 16 files for `wip-hd2d-point15-recovery-20260612/2026-06-14T22-51_distant_valley_threads`; the branch manifest now lists 112 paths and the viewer entry point is `https://anemora-viewer.pages.dev/wip-hd2d-point15-recovery-20260612/review`.
- Side effects: Unity dirtied `link.xml`, generated material assets, texture/meta files, Volume assets, and tracked screenshots during validation/capture. All unintended changes must be reverted before commit staging; only the authored setup file plus this devlog/index should be staged.
