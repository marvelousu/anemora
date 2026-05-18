# Stage 4 Chapter 1 Map DQ3R Next Pass 3 Scene Assembly

Date: 2026-05-09
Scope: Blender-only Chapter 1 / Antela scene assembly kit generation. Unity was not launched and no production scene was saved.

## Inputs

- `<notes>/_handover/anemora-map-generation-session-dq3r-next-instructions-3-2026-05-09.md`
- Existing proposal D five-scene Chapter 1 map batch.
- Existing `Chapter1_Antela_SurfaceAtlas_A` 1024px / 50-tile atlas contract.
- Existing schema v2 placement and readability data from next pass 2.

## Generated Assets

The map batch now records:

- `55` assets in `Assets/Art/Models/Zone1/Chapter1Map/chapter1_map_assets_manifest.json`.
- `41` FBX files under `Assets/Art/Models/Zone1/Chapter1Map/`.
- `14` FBX files under `Assets/Art/Models/Zone1/Chapter1DetailKit/`.
- `75` total non-meta FBX files under `Assets/Art/Models/Zone1/`.

New scene-assembly FBX assets:

- `Ch1_S1_Library_SceneAssembly_A.fbx`
- `Ch1_S1_Library_SceneAssembly_B.fbx`
- `Ch1_S2_MiaHouse_SceneAssembly_A.fbx`
- `Ch1_S2_MiaHouse_SceneAssembly_B.fbx`
- `Ch1_S3_Current_SceneAssembly_A.fbx`
- `Ch1_S3_Current_SceneAssembly_B.fbx`
- `Ch1_S3_Past_SceneAssembly_A.fbx`
- `Ch1_S3_Past_SceneAssembly_B.fbx`
- `Ch1_S4_Current_SceneAssembly_A.fbx`
- `Ch1_S4_Current_SceneAssembly_B.fbx`
- `Ch1_S4_Past_SceneAssembly_A.fbx`
- `Ch1_S4_Past_SceneAssembly_B.fbx`
- `Ch1_S5_NorthRuins_SceneAssembly_A.fbx`
- `Ch1_S5_NorthRuins_SceneAssembly_B.fbx`
- `Ch1_TimeWindow_Adjacent_SceneAssembly_A.fbx`
- `Ch1_TimeWindow_Adjacent_SceneAssembly_B.fbx`

New shared helper-kit FBX assets:

- `Assets/Art/Models/Zone1/Chapter1DetailKit/Ch1_LightingShadowHelperKit.fbx`
- `Assets/Art/Models/Zone1/Chapter1DetailKit/Ch1_FogAndDepthHelperKit.fbx`
- `Assets/Art/Models/Zone1/Chapter1DetailKit/Ch1_TimeWindowSurfaceHelperKit.fbx`

## Scene Assembly Manifest

Added `Assets/Art/Models/Zone1/Chapter1Map/chapter1_scene_assembly_manifest.json` with `schema_version: 3`.

The manifest records:

- `8` beats: `S1_Library`, `S2_MiaHouse`, `S3_CurrentStreet`, `S3_PastMarket`, `S4_CurrentField`, `S4_PastField`, `S5_NorthRuins`, and `TimeWindow_Adjacent`.
- `24` camera composition bundles: readability, density, and cinematic-review bundle per beat.
- `25` 64x96 character integration anchors with 1.5m nominal height, contact-shadow receiver flags, and occlusion safety hints.
- Per-beat A/B/C density variants, visible scene rectangles, walkable lanes, safe face zones, lower-body cover zones, hotspot records, and remove-first-if-cluttered lists.
- Shared helper-kit import priority for lighting / shadow, fog / depth, and Time Window floor-surface tuning.

The Time Window rule remains unchanged: thin planar world-space visual window only, with floor/wall adjacency traces. This pass does not add a gate, arch, ring, thick portal, or physical opening.

## Placement Manifest V3

`Assets/Art/Models/Zone1/Chapter1Map/chapter1_unity_placement_manifest.json` is now `schema_version: 3`.

Counts:

- `52` placement records.
- `16` new scene-assembly placement records.
- `3` new helper-kit placement records.
- Existing schema v2 fields remain present.
- New top-level links point to the scene assembly manifest and surface atlas v2.

## Surface Atlas V2

Added an authored surface atlas v2 while preserving the v1 import contract:

- `Assets/Art/Textures/Zone1/Chapter1/Chapter1_Antela_SurfaceAtlas_A_v2.png`
- `Assets/Art/Textures/Zone1/Chapter1/Chapter1_Antela_SurfaceAtlas_A_v2_normal.png`
- `Assets/Art/Textures/Zone1/Chapter1/Chapter1_Antela_SurfaceAtlas_A_v2_orm.png`
- `Assets/Art/Textures/Zone1/Chapter1/chapter1_surface_atlas_v2_manifest.json`

Compatibility:

- `1024 x 1024` atlas size.
- `50` tiles.
- `128 x 128` tile cells.
- v1 tile IDs preserved.
- v1 UV rects preserved.
- UV layer remains `Chapter1_Antela_SurfaceAtlas_A`.

Review image:

- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_texture_atlas_v2_before_after.png`

## Review Packet

Per beat, next pass 3 now outputs:

- assembly A/B isometric review
- top-down review
- single-beat isometric review
- route / collision overlay PNG
- occluder ON/OFF PNG

Key review images:

- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_next3_scene_assembly_overview.png`
- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_next3_s3_currentstreet_assembly_sheet.png`
- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_next3_s3_pastmarket_assembly_sheet.png`
- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_next3_s4_currentfield_assembly_sheet.png`
- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_next3_s4_pastfield_assembly_sheet.png`
- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_next3_s5_northruins_assembly_sheet.png`
- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_next3_density_abc_sheet.png`
- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_next3_character_anchor_lanes.png`
- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_next3_time_window_surface_sheet.png`

The scene assembly render camera was adjusted after the first pass because early images framed the kits too far toward the lower-right. The final review images are centered enough for layout review.

## Unity Handoff

Import first:

1. Scene assembly A/B FBX for S3 Current/Past.
2. Scene assembly A/B FBX for S4 Current/Past.
3. `Ch1_S5_NorthRuins_SceneAssembly_A/B.fbx`.
4. `Ch1_TimeWindow_Adjacent_SceneAssembly_A/B.fbx`.
5. `Ch1_LightingShadowHelperKit.fbx`.
6. `Ch1_FogAndDepthHelperKit.fbx`.
7. `Ch1_TimeWindowSurfaceHelperKit.fbx`.
8. `Chapter1_Antela_SurfaceAtlas_A_v2` albedo / normal / ORM material remap.

Capture priority after Unity import:

1. S3 Current/Past density bundle B.
2. S4 Current/Past density bundle B.
3. S5 NorthRuins density bundle B.
4. TimeWindow Adjacent cinematic-review bundle C.
5. Occluder ON/OFF and route/collision overlays for all eight beats.
6. Atlas v1/v2 material comparison under the existing fixed isometric camera.

Still missing for stronger DQ3R read:

- Unity-side material remap and lighting capture over atlas v2.
- Production camera captures with review-only material slots hidden.
- A final remove-first-if-cluttered pass once real 64x96 characters are reviewed in scene.
- Hand-authored small surface irregularities for roof edges, wall-base dirt, and contact shadows where procedural placement still reads too even.

## Verification

Commands run:

- `python -m py_compile tools\generate_chapter1_map_assets_blender.py`
- Blender 4.5.5 LTS background full generation with `tools\generate_chapter1_map_assets_blender.py`
- Blender 4.5.5 LTS background targeted `render_scene_assembly_review_set()` after review-camera centering fix
- Static manifest/path/triangle/UV/tile-reference/placement/PNG checks
- `git diff --check`

Static result:

- `41` Chapter1Map FBX files.
- `14` Chapter1DetailKit FBX files.
- `55` map manifest assets.
- `14` detail-kit manifest assets.
- `52` placement records.
- `8` scene assembly beats.
- `24` scene camera bundles.
- `25` 64x96 character anchors.
- `50` atlas v1 tiles and `50` atlas v2 tiles with matching IDs and UV rects.
- Review PNGs checked at `1920 x 1080`.
- Atlas albedo PNGs checked at `1024 x 1024`.
