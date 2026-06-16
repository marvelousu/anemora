# HD2D terrain surface quilt

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-17

## Investigation

- The latest wide captures no longer had the old white haze or black building-surface regression, but the outdoor maps still read as broad uniform floor, field, and grass slabs between the playable set dressing and the distant panorama.
- The first terrain-quilt attempt added enough breakup, but built-player review showed unacceptable black terrain facets in `03_b1_b3_current.png`.
- The black facets were not a renderer-feature issue. They came from low-relief terrain meshes using lit surface materials at shallow camera angles.

## Change

- Added deterministic `TerrainSurfaceQuilt` roots to every outdoor current/past map: Exterior, CentralPlaza, MiaHouse, AriaStreet, KaiaFarm, and Ruins.
- Each root now authors small low-relief ground panels, field bands, path shoulders, and grass seams from the existing map coordinates and area-derived seeds.
- Added dedicated `Ch1Ground_*_TerrainSurfaceQuilt*2K` generated materials and textures so this work stays separate from the existing Phase 3 cycle materials.
- Terrain-quilt materials are generated as unlit while retaining the `Ch1Ground_` naming and production-surface texture path. Validation now checks that these materials stay unlit, preventing the low-angle black-facet regression from returning.
- Final tuning reduced patch footprint and shifted current-era colors toward soil, stone, and muted grass so the breakup reads as authored surface variation rather than large green-gray slabs.

## Visual Review

- Accepted packet: `docs/review/2026-06-17T05-26_terrain_surface_quilt_final/`.
- `03_b1_b3_current.png`: the rejected black terrain facets are gone; the old white haze also remains absent.
- `01_a1_a2_current.png`, `05_c1_c3_current.png`, `09_e1_e3_current.png`, and `11_f1_f6_current.png`: broad ground, field, and edge surfaces now have visible authored breakup across the all-map wide frames.
- `00_contact_sheet.png`: all 13 built-player captures are present for quick review.
- Shotdiff triage against the preceding unlit candidate is intentionally over threshold because the final pass reduced patch size and color contrast across the all-map set.

## Verification

- Validate: `Logs/terrain_surface_quilt_validate_r5.log` passed with `Fast VS house slice validation passed.`
- Renderer freeze: `Logs/terrain_surface_quilt_editmode_r4.xml` passed 36/36 EditMode tests, including `RendererFeatureSet_MatchesFrozenBaseline`.
- Asset validation: `Logs/terrain_surface_quilt_asset_validation_r4.log` passed with `[AssetValidation] OK`.
- Build: `Logs/terrain_surface_quilt_build_r4.log` passed and rebuilt `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`.
- Built-player capture: `Logs/terrain_surface_quilt_player_capture_r4.log` passed and wrote 13 PNGs to `docs/review/2026-06-17T05-26_terrain_surface_quilt_final/`.
- Shotdiff: `Logs/shotdiff/terrain_surface_quilt_final_vs_unlit_r3/summary.txt` recorded the final tuning delta.

## Next

- Continue Phase 3 by moving from surface breakup to denser authored ground/building material structure, including atlas/PBR separation where feasible.
- Continue Phase 2/3 polish on vegetation silhouettes near the camera; some broad shrubs still read as simplified masses in the wide frames.
- Keep the white-haze and black-facet guards in mind when adding any new surface or atmospheric overlay.
