# Stage 4 Chapter 1 Map - DQ3R Pass 7 Reception Integration

Date: 2026-05-09 (late evening)

Status: Pass 7 fully landed by the map generation session and received cleanly by the graphics-foundation pre-built scaffolding. All 6 graceful Pass 7 reception tests in `Chapter1MapAssetTests` flipped from `Assert.Ignore` (skip) to pass on the first run after the deliverables landed. Targeted `Chapter1MapAssetTests` `44 testcase / 43 pass / 1 pre-existing skip / 0 fail`. Static verifiers `5+6+7 = 25 / 25` checks pass.

## What Pass 7 delivered

Per `notes/_handover/anemora-map-generation-session-dq3r-next-pass7-progress-2026-05-09.md`:

- **Animated banner cloth kit (Priority A)**: 7 banner mesh variants in `Ch1_Next7_AnimatedBannerCloth.fbx` (market_banner_red / blue / gold / green, drying_laundry_a/b, store_flag_white) + `Chapter1AntelaAnimatedCloth.shader` (URP Lit + vertex sine wave + alpha-clip, UniversalForward + ShadowCaster) + `chapter1_next7_animated_cloth_manifest.json`
- **6 skybox variant kit (Priority B)**: 6 lat-long PNGs (Day / Dawn / Dusk / Night / Storm / MagicHour) + 6 `URP/Skybox/Panoramic` materials at `Assets/Art/Materials/Zone1/Chapter1Sky/Chapter1_Antela_Skybox_*.mat` + `chapter1_next7_skybox_manifest.json`
- **Character backlight FBX kit (Priority C)**: `Ch1_Next7_CharacterBacklightKit.fbx` with 8 per-character entries + `chapter1_next7_character_backlight_manifest.json`
- **Sub-tile detail families C / D / E (Priority D)**: 3 single-channel grain textures (organic micro-grain / fabric thread / cracked plaster) added alongside Pass 6 A/B
- **Micro-prop density kit (Priority E)**: `Ch1_Next7_MicroPropDensityKit.fbx` with 12-16 micro-props + `chapter1_next7_micro_prop_density_manifest.json` (`emissive_when_variant` declared per emissive prop)
- **Static verifier (Priority F)**: `tools/verify_chapter1_next7_static.py` (8 checks, all pass)

Pass 6 contract preserved: `Chapter1AntelaSurfaceAtlas.shader` 11 inputs intact, atlas v3 UV / 50 tile IDs / UV layer name `Chapter1_Antela_SurfaceAtlas_A` stable across all manifests.

## What graphics-foundation pre-built reception did

The reception was scaffolded a few minutes before Pass 7 actually landed, so the integration was a no-op observation event:

### Tests flipped on first run after Pass 7 landed

The 6 graceful Pass 7 tests moved from skip to pass without any test edit:

| Test | Pre-Pass 7 status | Post-Pass 7 status |
|---|---|---|
| `Chapter1MapPass7AnimatedClothManifestExists` | Skip (manifest absent) | Pass (manifest declares ≥5 cloth meshes; actually 7) |
| `Chapter1MapPass7SkyboxVariantsArePresent` | Skip (textures absent) | Pass (all 6 lat-long PNGs landed) |
| `Chapter1MapPass7CharacterBacklightKitIsPresent` | Skip (FBX absent) | Pass (FBX + manifest landed; manifest enumerates ≥8 entries) |
| `Chapter1MapPass7SubTileDetailFamiliesCDEArePresent` | Skip (textures absent) | Pass (all 3 grain textures landed) |
| `Chapter1MapPass7MicroPropDensityKitIsPresent` | Skip (FBX absent) | Pass (FBX + manifest landed) |
| `Chapter1MapPass7VerifierScriptIsPresent` | Skip (verifier absent) | Pass (verifier landed; verifier itself runs 8/8 checks) |

### Importer presets activated

`ApplyAtlasV3AndDecalImporterSettings` already enumerated the 6 skybox lat-long sRGB PNGs and the 3 sub-tile detail grain textures via `EnumeratePass7SkyboxTextures` and `Pass7SubTileDetailTextures`. On the first import after Pass 7 landed, those textures got the same compression / wrap / filter / mipmap presets the Pass 6 textures use, with no manual touch required.

### Loaders are live

The pre-built graceful loaders now return non-null assets:

- `TryLoadAnimatedClothShader` → `Anemora/Chapter1AntelaAnimatedCloth` shader
- `TryLoadPass7SkyboxMaterial(tod)` → per-TOD `URP/Skybox/Panoramic` material
- `TryLoadCharacterBacklightKit` → backlight FBX prefab GameObject
- `TryLoadAnimatedBannerClothKit` → banner cloth FBX prefab GameObject
- `TryLoadMicroPropDensityKit` → micro-prop density FBX prefab GameObject

### Capture path wires

Two integration points actually run user-visible behavior changes (interactive Editor only — gated by `!Application.isBatchMode` mirroring DOF / rim / atlas v4 manifest override):

1. `CreateTimeOfDayLighting` calls `TryLoadPass7SkyboxMaterial(tod)` and assigns `RenderSettings.skybox` when non-null, then triggers `DynamicGI.UpdateEnvironment()` so ambient pickup matches the new sky.
2. `CreateCharacterRimLightPlane` calls `TryLoadCharacterBacklightKit` first; when the kit is present, it instantiates the per-character entry prefab via `PrefabUtility.InstantiatePrefab` (looked up by `ResolveBacklightKitChildName(label)` mapping `HeroNiro → hero`, `ResidentL_Luna → luna`, etc.) and skips the procedural quad. The procedural quad remains the fallback when the kit is absent or a character entry is missing.

The animated banner cloth and micro-prop density kits have loaders ready but no automatic placement yet — Pass 7 polish will hook them into per-beat scene assembly (S2/S3 anchors for cloth, S2/S3 + S4 for props) once Tom validates the kits in the interactive Editor.

## Verification

- `python tools/verify_chapter1_next5_static.py` — 10/10 ✓ (Pass 5 contract unchanged after Pass 7 added)
- `python tools/verify_chapter1_next6_static.py` — 7/7 ✓ (Pass 6 contract unchanged; atlas v4 / variants / decals / sub-tile A-B / shader untouched)
- `python tools/verify_chapter1_next7_static.py` — 8/8 ✓
- Targeted `Chapter1MapAssetTests` (Unity 6000.3.14f1 batchmode -runTests EditMode) — `44 testcase / 43 pass / 1 pre-existing skip / 0 fail`

## What is still review-only

- All Pass 7 deliverables ship with `production_safe` / `review_only` flags per the manifests; Storm_A skybox is `review_only`, others are `production_safe`.
- Pass 7 did NOT promote characters to production. Same v4/v10/v11 disqualification holds.
- Time Window stays a thin world-space visual window — Pass 7 did not add any 3D portal arch / gate.
- Production runtime sprite GUIDs in `Assets/Art/Sprites/Hero/v2/` and `Resident_*/v2/` remain untouched.
- Animated banner cloth + micro-prop density kits have loaders ready but no per-beat placement code yet; review-only until Tom validates in interactive Editor.

## Pass 7 follow-up wire-ins (this same session)

After Pass 7 landed and the reception scaffolding flipped green, the orchestrator chained additional wire-ins so per-beat captures actually use the Pass 7 art:

### Per-beat animated cloth placement

`PlaceAnimatedClothForBeat(parent, beatKey)` reads `chapter1_next7_animated_cloth_manifest.json` (Pass7AnimatedClothManifest data class with `clothes` array + cloth_id / size_m / anchor_xy / wave_frequency_hz / wave_amplitude_world / wind_direction_xz / intended_scene_anchors / segments_x_y) and instantiates each cloth FBX child whose `intended_scene_anchors` matches the current beat (`MatchesBeat` is prefix-tolerant so S3_PastMarket ↔ S3_Past). Per-cloth `_WaveFrequency` / `_WaveAmplitude` / `_WindDirectionXZ` are bound via `MaterialPropertyBlock` from the manifest. Wired into `CaptureChapter1MapDQ3RHeroCinematic8K` / `CaptureChapter1MapDQ3RHeroCinematic4K` / `CaptureChapter1MapDQ3RPostProcessReview` next to `PlaceCharacterAnchorBillboardsForBeat`. Skipped in batchmode.

### Per-beat micro-prop density scatter

`PlaceMicroPropsForBeat(parent, beatKey, variantId)` reads `chapter1_next7_micro_prop_density_manifest.json` (Pass7MicroPropDensityManifest data class with `props` array + prop_id / intended_scene_anchors / recommended_offset_xyz / is_emissive / emissive_when_variant) and scatters props at `recommended_offset_xyz`. Emissive props (lanterns) enable emission via MPB only when the current variant ID matches `emissive_when_variant` (e.g., Lantern_A / Night_A / Storm_A / Dusk_A) — a clean integration of pass 5 atmosphere variant gating with pass 7 prop emission.

### Sub-tile family per-slot resolver

`ResolveSubTileFamilyForSlot(slotKey)` returns the best Pass 7 sub-tile detail family per material slot keyword:

- A (medium grain stone/road/wood): stone / road / wood / brick / rubble
- C (organic micro-grain Pass 7): soil / grass / leaves / fruit / vegetation / organic / plant
- D (fabric thread Pass 7): cloth / paper / banner / tapestry / flag / fabric
- E (cracked plaster Pass 7): plaster / patch / crack / repair
- B (small grain plaster/cloth/paper Pass 6 default): fallback

Wired into `LoadOrCreateMaterials` so each Chapter1 material binds the appropriate detail texture instead of the global default. Applied only when the custom shader is bound (interactive Editor only).

## Pass 8 reception scaffolding (pre-built)

In parallel with Pass 7 follow-up, Pass 8 reception scaffolding was pre-built so when Pass 8 lands (atlas v5 + decal sheet F + wind zone preset + tone matrix doc + verifier), the integration is a no-op observation:

- 6 graceful tests in `Chapter1MapAssetTests`: `Pass8AtlasV5ManifestDeclaresFullAuthoredCoverage` / `Pass8AtlasV5TexturesArePresent` / `Pass8DecalSheetFIsAntiPairedWithParticles` / `Pass8WindZonePresetKitIsPresent` / `Pass8LightingToneMatrixDocIsPresent` / `Pass8VerifierScriptIsPresent`
- `LoadAtlasManifest` priority chain extended to v5 → v4 → v3 (interactive only)
- `Pass8AtlasV5Textures` (4 PNG) + `Pass8DecalSheets` (sheet F) importer presets reserved
- `TryLoadPass8WindZoneKit` / `TryLoadPass8FootprintDecalSheet` graceful loaders
- Wind anchor wire-in via `TryResolveNearestWindAnchorDirection`: places kit FBX once per scene root (cached + renderers hidden), per-cloth picks beat-relevant or nearest-distance child as `_WindDirectionXZ` source. Falls back to manifest static value when Pass 8 absent.
- Footprint trail wire-in: when character billboard sheet is `walk_front_*` / `walk_back_*` AND Pass 8 sheet F has shipped, spawns horizontal alpha-blend decal quad behind (or in front of) character based on walk direction. Sheet F sampled at the first decal cell (footprint_trail @ row 0 col 0 in 4×3 grid).

Targeted `Chapter1MapAssetTests` is now `50 testcase / 44 pass / 6 skip / 0 fail`. The 6 skips will flip to pass once Pass 8 lands its corresponding artifacts, exactly as the Pass 7 reception scaffolding did before Pass 7 landed.

## Next: Pass 8 candidates

Pass 8 instructions issued at `notes/_handover/anemora-map-generation-session-dq3r-next-instructions-8-atlas-v5-motion-decals-wind-tone-2026-05-09.md` cover atlas v5 (50/50 authored), decal sheet F (FootprintTrail anti-paired with particles, character-motion-paired), wind zone preset FBX, per-TOD lighting tone matrix doc, and the next-pass verifier.
