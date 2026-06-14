# HD2D distant composition prototype

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-15

## Investigation

- The active implementation frontier remains `wip/hd2d-point15-recovery-20260612`; the old continuation wording is historical/contextual, not a separate branch to continue.
- The previous distant panorama work removed the worst void problem, but visual review still read as generic low-poly bands with repeated horizon pieces. The main failure was composition quality, not color tuning.
- Bridge traversal was addressed in the prior cycle with a current/past `CharacterController` guard, so this cycle returned to the most visible graphics gap: the F1-F6 Ruins vista.
- The acceptance target for this cycle was one-map-first: make Ruins show authored terrain shelves, broken treeline mass, peak clusters, and valley cuts before rolling the grammar to every map.

## Implementation

- Added a Ruins-only distant composition prototype layer with 9 deterministic `DistantVista_CompositionPrototype` meshes.
- Introduced four silhouette profiles: terrain shelves, broken treeline, peak clusters, and valley cuts. These sit between the existing far panorama ring and the playable map edge so the horizon reads as composed geography rather than a simple backdrop band.
- Broadened the existing Ruins area landmark and area signature profiles to remove thin needle/tooth silhouettes that looked artificial at wide review scale.
- Added validation for the prototype:
  - Ruins must contain at least 9 composition prototype meshes.
  - The wide review camera must see at least 3 composition prototype renderers.
- Added `CaptureDistantPanoramaVistaParallaxProofBatch` to publish a right-shifted Ruins parallax proof alongside the normal all-map Wide capture.
- No renderer features, renderer feature order, procedural sky paths, colliders, random placement, time/date-dependent placement, or runtime input contracts were changed.

## Rejected Iterations

- r1 improved the Ruins horizon but still leaned on thin spike silhouettes, so it was rejected as another plateau polish attempt.
- r2 broadened the ridge profiles and moved the visible read toward terrain mass, shelves, and broken treeline. That version became the all-map review capture.
- The first standalone parallax capture emitted left/center current frames with missing ground after cold area isolation. Those images were rejected and removed from the review packet. The final parallax batch warms the isolated Ruins setup once, deletes the warmup frames, then captures only the right-shifted proof pair.

## Verification

- Validate: `Logs/distant_composition_prototype_validate_r4.log` passed with `Fast VS house slice validation passed.`
- Renderer freeze: `Logs/distant_composition_prototype_editmode_r4.xml` passed `36/36`; `RendererFeatureSet_MatchesFrozenBaseline` result is `Passed`.
- Asset validation: `Logs/distant_composition_prototype_asset_validation_r4.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/distant_composition_prototype_capture_r2.log` produced the 13 all-map Wide review PNGs. `Logs/distant_composition_prototype_parallax_capture_r5.log` produced `14_ruins_parallax_right_current.png` and `15_ruins_parallax_right_past.png`.
- Review packet: `docs/review/2026-06-15T03-49_distant_composition_prototype/` contains the 13 all-map frames, 2 parallax proof frames, `00_contact_sheet.png`, and `devlog.txt`.
- Shotdiff: `Logs/shotdiff/distant_composition_prototype_vs_bridge_r6` compared against `docs/review/2026-06-15T03-06_bridge_character_traversal`. The changed frames were `00_contact_sheet.png` at `55.2342%` and `11_f1_f6_current.png` at `1.5105%`; `12_f1_f6_past.png` was `0.0451%`, and the remaining all-map frames were unchanged or effectively unchanged. The two parallax proof images were new files.
- Visual review: Ruins current now has a wider broken ridge/treeline composition instead of a plain void or repeated band. Ruins past keeps the warmer distant read, while the new parallax proof pair confirms the far meshes remain real 3D geometry.

## Next

- Continue Phase A until Ruins reads as authored production geography, not just acceptable geometry. If it stalls, inspect mesh density, band distance, fog range, far clip, and texel density before changing colors.
- Roll the accepted composition grammar to all maps only after Ruins is visibly strong.
- Add a midground edge-closure pass so playable-map edges transition through cliffs, road continuations, water shelves, and tree masses before the far panorama.
- Keep the bridge route guard in place. A later route-design cycle can replace the direct support with a current-collapse/past-repair puzzle only after built-player traversal proof exists.
