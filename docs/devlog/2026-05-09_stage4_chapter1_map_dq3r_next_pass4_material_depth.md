# 2026-05-09 Stage 4 Chapter 1 Map DQ3R Next Pass 4 Material Depth

## Scope

This pass follows `<notes>/_handover/anemora-map-generation-session-dq3r-next-instructions-4-integration-material-depth-2026-05-09.md`.

Unity was not launched, and no production scene was saved. The work extends the existing Chapter1Map Blender generation batch with review/import assets that graphics foundation can remap in Unity after review.

## Added

- Added Antela surface atlas v3:
  - `Assets/Art/Textures/Zone1/Chapter1/Chapter1_Antela_SurfaceAtlas_A_v3.png`
  - `Assets/Art/Textures/Zone1/Chapter1/Chapter1_Antela_SurfaceAtlas_A_v3_normal.png`
  - `Assets/Art/Textures/Zone1/Chapter1/Chapter1_Antela_SurfaceAtlas_A_v3_orm.png`
  - `Assets/Art/Textures/Zone1/Chapter1/Chapter1_Antela_SurfaceAtlas_A_v3_height_or_edge.png`
  - `Assets/Art/Textures/Zone1/Chapter1/chapter1_surface_atlas_v3_manifest.json`
- Added transparent decal overlay atlas:
  - `Assets/Art/Textures/Zone1/Chapter1/Chapter1_Antela_DecalOverlay_A.png`
  - `Assets/Art/Textures/Zone1/Chapter1/Chapter1_Antela_DecalOverlay_A_normal.png`
  - `Assets/Art/Textures/Zone1/Chapter1/chapter1_decal_overlay_manifest.json`
- Added 32 Next4 scene-specific depth FBX kits under `Assets/Art/Models/Zone1/Chapter1Map/`:
  - 8 `DepthLayer_A`
  - 8 `DepthLayer_B`
  - 8 `DecalReceivers`
  - 8 `BackgroundSilhouette`
- Added one shared helper FBX under `Assets/Art/Models/Zone1/Chapter1DetailKit/`:
  - `Ch1_Next4_LightShadowDepthCardKit.fbx`
- Added manifest handoff files:
  - `Assets/Art/Models/Zone1/Chapter1Map/chapter1_next4_material_depth_manifest.json`
  - `Assets/Art/Models/Zone1/Chapter1Map/chapter1_next4_occluder_safety_manifest.json`
  - `Assets/Art/Models/Zone1/Chapter1Map/chapter1_next4_production_clean_manifest.json`

## Material Slots

New Unity-facing material slot names include:

- `lightcard_window_warm`
- `lightcard_candle_warm`
- `shadowcard_eave_soft`
- `shadowcard_interior_cool`
- `fog_depth_soft`
- `time_window_inner_rim`
- `time_window_reflection`
- `time_window_scuff`
- `decal_receiver`
- `depth_contact_receiver`
- `material_mask_warm`
- `material_mask_cool`
- `material_mask_grime`
- `foreground_branch_mask`
- `background_silhouette_soft`

The Time Window treatment remains a thin world-space visual surface with floor/wall trace cards only. No thick gate, ring, portal arch, or physical opening was added.

## Review Images

Updated review outputs:

- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_texture_atlas_v3_before_after.png`
- `docs/devlog/screenshots/stage4_chapter1_map_dq3r_texture_atlas_v3_tile_notes.png`
- `docs/devlog/screenshots/stage4_chapter1_map_next4_decal_overlay_sheet.png`
- `docs/devlog/screenshots/stage4_chapter1_map_next4_depth_layer_overview.png`
- `docs/devlog/screenshots/stage4_chapter1_map_next4_occluder_safety_sheet.png`
- `docs/devlog/screenshots/stage4_chapter1_map_next4_s1_library_close_material_depth.png`
- `docs/devlog/screenshots/stage4_chapter1_map_next4_s2_miahouse_close_material_depth.png`
- `docs/devlog/screenshots/stage4_chapter1_map_next4_s3_current_close_material_depth.png`
- `docs/devlog/screenshots/stage4_chapter1_map_next4_s3_past_close_material_depth.png`
- `docs/devlog/screenshots/stage4_chapter1_map_next4_s4_current_close_material_depth.png`
- `docs/devlog/screenshots/stage4_chapter1_map_next4_s4_past_close_material_depth.png`
- `docs/devlog/screenshots/stage4_chapter1_map_next4_s5_north_close_material_depth.png`
- `docs/devlog/screenshots/stage4_chapter1_map_next4_timewindow_close_material_depth.png`

## Verification

Commands run:

- `python -m py_compile tools/generate_chapter1_map_assets_blender.py`
- `C:\Program Files\Blender Foundation\Blender 4.5\blender.exe --background --python tools/generate_chapter1_map_assets_blender.py`
- `C:\Program Files\Blender Foundation\Blender 4.5\blender.exe --background --python-expr "... g.render_next4_close_review_set()"`
- JSON / PNG / triangle / material-slot static validation script
- `git diff --check -- tools/generate_chapter1_map_assets_blender.py CHANGELOG.md docs/ASSET_STRUCTURE.md docs/devlog/INDEX.md docs/legal/asset_ledger.md`

Static validation result:

- Main manifest assets: 88
- DetailKit assets: 15
- Next4 assets: 33
- Unity placement records: 85
- Scene assembly beats: 8
- Next4 material-depth beats: 8
- Decal overlay entries: 28
- Atlas v3 tiles: 50
- Production-safe asset records: 45

Final visual QA:

- Close material-depth PNGs use a marker-light context pass rather than the full scene assembly review overlays.
- In-scene text labels were removed from the close PNGs after QA because foreground occluders could crop them; filenames and manifests carry the beat identity.

## Handoff Notes

Graphics foundation should prioritize Unity import/capture in this order:

1. S3 Current and S3 Past close material-depth captures.
2. S4 Current/Past field captures with atlas v3 and decal overlay toggles.
3. S5 north ruins fog/depth capture.
4. Time Window adjacent close capture with only `time_window_trace`, `time_window_inner_rim`, `time_window_reflection`, and `time_window_scuff`.
5. S1/S2 interior foreground occluder safety review.

Still missing for a stronger DQ3R read:

- Hand-authored Unity material tuning for additive/multiply card blend modes.
- Capture-side comparison of atlas v2 vs v3 after URP Lit remap.
- Character v5/final 64x96 silhouettes are still blocked, so current review uses neutral 1.5m markers.
