# Stage 4 Chapter 1 Map DQ3R Next Pass 2

Date: 2026-05-09
Scope: Blender-only Chapter 1 / Antela map asset generation follow-up. Unity was not launched and no production scene was saved.

## Inputs

- `<notes>/_handover/anemora-map-generation-session-dq3r-next-instructions-2-2026-05-09.md`
- Existing Chapter 1 proposal D five-scene map batch.
- Existing `Chapter1_Antela_SurfaceAtlas_A` atlas contract.

## Generated Assets

- Regenerated `36` Chapter 1 map manifest assets.
- `25` FBX assets remain under `Assets/Art/Models/Zone1/Chapter1Map/`.
- `11` FBX assets now live under `Assets/Art/Models/Zone1/Chapter1DetailKit/`.
- Added six production-toggleable A/B dressing kits:
  - `Ch1_Dressing_S3_Current_CloseStreet_AB.fbx`
  - `Ch1_Dressing_S3_Past_CloseStreet_AB.fbx`
  - `Ch1_Dressing_S4_KaiaField_Current_AB.fbx`
  - `Ch1_Dressing_S4_KaiaField_Past_AB.fbx`
  - `Ch1_Dressing_S5_NorthRuins_Current_AB.fbx`
  - `Ch1_Dressing_S0_S2_Interior_Occluder_AB.fbx`

The dressing kits add camera-specific roofline, ground contact, debris, market, field, ruin, and interior foreground variants without adding player-facing text or story labels to production meshes.

## Atlas Push

- Kept the existing `Chapter1_Antela_SurfaceAtlas_A` contract.
- Regenerated:
  - `Assets/Art/Textures/Zone1/Chapter1/Chapter1_Antela_SurfaceAtlas_A.png`
  - `Assets/Art/Textures/Zone1/Chapter1/Chapter1_Antela_SurfaceAtlas_A_normal.png`
  - `Assets/Art/Textures/Zone1/Chapter1/Chapter1_Antela_SurfaceAtlas_A_orm.png`
  - `Assets/Art/Textures/Zone1/Chapter1/chapter1_antela_surface_atlas_manifest.json`
- Atlas remains `1024 x 1024`, `50` tiles, `128 x 128` tile cells.
- Each atlas tile now carries `authoring_notes` for Unity/material review.
- No new atlas tile IDs were required; all generated UV tile references resolve.

Review image:

- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_texture_atlas_authoring_before_after.png`

## Placement Manifest V2

`Assets/Art/Models/Zone1/Chapter1Map/chapter1_unity_placement_manifest.json` now has `schema_version: 2`.

Counts:

- `33` placement records.
- `12` new dressing placement records.
- `5` composition bundles.

V2 fields added to every placement:

- `production_toggle_group`
- `camera_bundle_id`
- `occlusion_risk`
- `walkable_clearance_width_m`
- `character_scale_anchor_position`
- `foreground_safe_area_rect`
- `recommended_sorting_intent`
- `alpha_intent`
- `time_window_relation`

## Readability Data

Added `Assets/Art/Models/Zone1/Chapter1Map/chapter1_readability_review_manifest.json`.

The manifest records:

- `5` camera bundle readability records.
- `10` review-only PNGs: route overlay and occluder ON/OFF pair per bundle.
- Approximate 1.5m character anchors.
- Hotspot clearance notes.
- Remove-first-if-cluttered lists for optional dressing.

Review images include:

- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_s3_current_route_overlay.png`
- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_s3_current_occluder_onoff.png`
- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_s3_past_route_overlay.png`
- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_s3_past_occluder_onoff.png`
- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_s4_pair_route_overlay.png`
- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_s4_pair_occluder_onoff.png`
- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_s5_current_route_overlay.png`
- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_s5_current_occluder_onoff.png`
- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_s0_s2_interiors_route_overlay.png`
- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_s0_s2_interiors_occluder_onoff.png`

## Time Window Adjacent Art

`Ch1_DetailKit_TimeWindowAdjacent.fbx` was regenerated with more production-oriented floor-adjacent pieces:

- dust displacement
- faint fixed-camera rim strips
- side reflection slivers
- era-blend scuffs
- soft contact shadows

The Time Window remains a thin planar world-space visual window. This pass does not add a gate, arch, physical opening, or thick frame.

Review image:

- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_time_window_production_adjacent_sheet.png`

## Unity Handoff

Import first:

1. `Assets/Art/Models/Zone1/Chapter1DetailKit/Ch1_Dressing_S3_Current_CloseStreet_AB.fbx`
2. `Assets/Art/Models/Zone1/Chapter1DetailKit/Ch1_Dressing_S3_Past_CloseStreet_AB.fbx`
3. `Assets/Art/Models/Zone1/Chapter1DetailKit/Ch1_Dressing_S4_KaiaField_Current_AB.fbx`
4. `Assets/Art/Models/Zone1/Chapter1DetailKit/Ch1_Dressing_S4_KaiaField_Past_AB.fbx`
5. `Assets/Art/Models/Zone1/Chapter1DetailKit/Ch1_Dressing_S5_NorthRuins_Current_AB.fbx`
6. `Assets/Art/Models/Zone1/Chapter1DetailKit/Ch1_Dressing_S0_S2_Interior_Occluder_AB.fbx`
7. Regenerated `Assets/Art/Models/Zone1/Chapter1DetailKit/Ch1_DetailKit_TimeWindowAdjacent.fbx`

Capture priority after Unity import:

1. S3 Current/Past close street with dressing A/B toggles.
2. S4 Kaia field Current/Past pair with Time Window adjacent scuffs.
3. S5 north ruins current with fog/curb dressing.
4. S0-S2 interior foreground occluder ON/OFF.
5. Atlas/material tile review for authored surface read.

Still missing for stronger DQ3R read:

- Unity-side lighting/post capture over the new authored atlas.
- Fine material remap for new dressing slots where existing auto assignment is too flat.
- Production camera review of remove-first-if-cluttered variants.

## Verification

Commands run:

- `python -m py_compile tools\generate_chapter1_map_assets_blender.py`
- Blender 4.5.5 LTS background generation with `tools\generate_chapter1_map_assets_blender.py`
- Static manifest/path/triangle/UV/tile-reference/placement/PNG checks
- `git diff --check`

Static result:

- `36` generated assets, `11` detail-kit assets.
- `33` placement records, `12` dressing placement records.
- `50` atlas tiles, all with authoring notes.
- `36` assets with atlas UV tile references.
- `31` map review PNGs, `6` detail review PNGs, `10` readability review PNGs checked at `1920 x 1080`.
- Atlas albedo / normal / ORM checked at `1024 x 1024`.
- Whitespace diff check passed.
