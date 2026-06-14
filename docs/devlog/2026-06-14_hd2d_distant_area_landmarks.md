# HD2D distant area landmarks

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-14

## Investigation

- Continued Phase 1B after the midground valley-break cycle. The remaining quality issue was that every map still shared a similar mountain-ring identity, even though the void and broad-band problems were reduced.
- First area-landmark attempt validated, but shotdiff versus `2026-06-14T19-26_distant_midground_valley_breaks` was only 0.00-0.01% per frame. The meshes existed but were too small and too far back, so the pass was rejected as a plateau.
- The accepted version moves the landmarks inward, increases the count from 3 to 5 per panorama area, and uses larger blocky profiles so the review images show a deliberate horizon identity change.

## Change

- Added five deterministic `DistantVista_AreaLandmark` meshes per current/past panorama area.
- Assigned map-specific profiles:
  - Exterior, MiaHouse, AriaStreet, and CentralPlaza receive distant settlement/outbuilding rhythms.
  - KaiaFarm receives lower cultivated-band forms.
  - Ruins receives broken relic-like vertical teeth.
- Kept the meshes collider-free, render-layer scoped, and registered as non-arrival `PropOrFeature` landmarks.
- Added `Ch1Distant_CurrentAreaLandmark` and `Ch1Distant_PastAreaLandmark` material factories in the authored setup file only.
- Raised distant-vista validation to require the new per-area landmark count in each current/past vista root.

## Verification

- Validate: `Logs/distant_area_landmarks_validate_r2.log` passed with `Fast VS house slice validation passed.` and return code 0.
- Renderer freeze: `Logs/distant_area_landmarks_editmode_r1.xml` passed 36/36 EditMode tests, including `RendererFeatureSet_MatchesFrozenBaseline`.
- Asset validation: `Logs/distant_area_landmarks_asset_validation_r1.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/distant_area_landmarks_capture_r2.log` produced the Cycle05 all-map Wide set in `docs/review/2026-06-14T20-26_distant_area_landmarks/`.
- Shotdiff: `Logs/shotdiff/distant_area_landmarks_vs_midground_valley_final` compared against `docs/review/2026-06-14T19-26_distant_midground_valley_breaks`. The intended visible changes are strongest in Exterior/CentralPlaza/MiaHouse/AriaStreet at roughly 2.58-4.42%, with KaiaFarm/Ruins smaller but visible in the contact sheet.
- Visual review: the contact sheet now shows per-area horizon teeth instead of only a uniform ring. The next vista pass should tune individual profiles rather than add another global layer.
- Side effects: Unity dirtied `link.xml`, generated material assets, texture/meta files, Volume assets, and tracked screenshots during validation/capture. All unintended changes were reverted before commit staging.
