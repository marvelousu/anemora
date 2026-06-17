# HD2D architectural surface accents

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-17

## Investigation

- After the terrain surface quilt pass, the outdoor maps had better ground breakup but buildings still read as flat blockout planes in the all-map wide captures.
- The first architectural accent attempt validated but was too subtle in built-player review. Several anchors were either hidden by existing geometry or too small to move the actual image.
- The next iterations made the accents visible, but the past-era trim used bright frame/path colors that produced white/cyan bands in the wide captures. That was too close to the previously rejected white haze, so the final pass muted those accents before acceptance.

## Change

- Added deterministic `ArchitecturalSurfaceAccents` roots to every outdoor current/past map: Exterior, CentralPlaza, MiaHouse, AriaStreet, KaiaFarm, and Ruins.
- Each map now gets 16 non-colliding, shadow-disabled building surface accents: facade wear, patch breaks, sill/contact shadow strips, pilasters, masonry/lintel bands, roof wear, eave shadow, fascia, and smaller trim.
- Ruins also gets 6 bridge deck accents: long wear/shadow threads plus four transverse planks to make the bridge read less like a single flat strip.
- Added dedicated `Ch1Surface_*_ArchitecturalAccent*2K` generated materials and 2K textures for the new building accents. These stay separate from the existing Phase 3 cycle materials.
- Final r5 tuning reduces past-era bloom risk by tinting architectural accent materials, shrinking the longest roof/facade white-prone strips, and using muted surface materials for past-era structural trim instead of the bright portal/path colors.

## Visual Review

- Accepted packet: `docs/review/2026-06-17T08-12_architectural_surface_accents_r6/`.
- `03_b1_b3_current.png` and `04_b1_b3_past.png`: the CentralPlaza facade now has authored contact/shadow/material lines without reintroducing the previous black building-surface failure.
- `09_e1_e3_current.png` and `10_e1_e3_past.png`: KaiaFarm gains visible farm-building and upper-structure accents instead of reading as a flat field/roof band.
- `11_f1_f6_current.png`: the ruins bridge now has visible deck threads and planks in the all-map wide frame.
- White-haze check: the broad white fog/sun-wash artifact remains absent in the accepted packet. The remaining small white/blue canopy highlights in D/F past are pre-existing cloth/roof elements, not new architectural haze planes.
- Final r6 re-capture keeps the r5 image result after PNG source-size reduction: all 13 player screenshots are unchanged or below 0.003% pixel delta versus r5.
- Shotdiff against the previous accepted terrain-surface packet changed the intended building/bridge maps while keeping side-view unchanged.

## Verification

- Validate/build: `Logs/architectural_surface_accents_build_r6.log` passed with `Fast VS house slice validation passed.` and `Build Finished, Result: Success.`
- Renderer freeze: `Logs/architectural_surface_accents_editmode_r6.xml` passed EditMode tests, including `RendererFeatureSet_MatchesFrozenBaseline`.
- Asset validation: `Logs/architectural_surface_accents_asset_validation_r6.log` passed with `[AssetValidation] OK`.
- Built-player capture: `Logs/architectural_surface_accents_player_capture_r6.log` passed and wrote 13 PNGs to `docs/review/2026-06-17T08-12_architectural_surface_accents_r6/`.
- Source-size guard: the final `ArchitecturalAccent*2K_2k.png` files were palette-compressed without changing resolution, keeping each committed source below the 5 MB bloat guard.
- Shotdiff: `Logs/shotdiff/architectural_surface_accents_vs_terrain_quilt_r6/summary.txt` compared against `docs/review/2026-06-17T05-26_terrain_surface_quilt_final/`; `Logs/shotdiff/architectural_surface_accents_r5_vs_r6/summary.txt` confirms the texture-size reduction did not materially change player screenshots.

## Next

- Continue Phase 3 by replacing more flat building faces with authored roof, wall, and trim segmentation rather than broad overlays.
- Follow up on bridge traversal and close bridge visuals with a dedicated proof packet; this cycle only improves the bridge's visual deck read.
- Keep the white-haze/black-surface guards active when adding any new lit surface, especially on past-era wide captures.
