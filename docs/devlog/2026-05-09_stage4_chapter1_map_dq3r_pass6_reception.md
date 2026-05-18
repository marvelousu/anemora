# Stage 4 Chapter 1 Map - DQ3R Pass 6 Reception Integration

Date: 2026-05-09 (late evening)

Status: Pass 6 fully integrated on the graphics-foundation side. Targeted `Chapter1MapAssetTests` `38/38` (37 pass + 1 pre-existing skip).

## What Pass 6 delivered

Map session pass 6 (`notes/_handover/anemora-map-generation-session-dq3r-next-pass6-progress-2026-05-09.md`):

- Atlas v4 with 38 / 50 hand-painted tiles (UV-stable drop-in for v3)
- 4 new atmosphere variants: Snow_A / Storm_A (review-only) / Lantern_A / MagicHour_A — each with paired override masks (`color_tint`, `value_mask`, `saturation_mask`, `emissive_mask` or `specular_mask`)
- 4 new decal overlay sheets: B_RainPool / C_DustPile / D_EmberScatter / E_SnowDrift, each carrying a new `pairs_with_particle_kind` field
- 2 sub-tile detail textures: A (medium grain stone/road/wood) / B (smaller grain plaster/cloth/paper)
- Custom URP shader `Assets/Art/Shaders/Chapter1AntelaSurfaceAtlas.shader` (HLSL ShaderLab, URP Forward + ShadowCaster + DepthOnly, 11 inputs)
- Static verifier `tools/verify_chapter1_next6_static.py` — 7 / 7 checks passing locally

## What graphics-foundation integrated

### Material pipeline

- `LoadOrCreateMaterials` (`Assets/Editor/AnemoraChapter1MapAssetSetup.cs`) now prefers the custom shader via `TryLoadChapter1AntelaSurfaceShader` (graceful fallback to URP/Lit when shader is missing).
- `LoadAtlasManifest` overrides `albedo` and `normal` to atlas v4 paths whenever `chapter1_surface_atlas_v4_manifest.json` is present, so all atlas tile slicing in `TryCreateAtlasTileTexture` automatically picks up the hand-painted detail.
- `BindCustomSurfaceShaderInputs` sets the default neutral variant binding (`_VariantStrength=0`, `_VariantTintRgb=white`, `_VariantEmissiveBoost=0`) plus sub-tile detail (`_SubTileStrength=0.25`) on every Chapter1 material so renderers without per-renderer override stay stable.

### Importer presets

- `ApplyAtlasV3AndDecalImporterSettings` extended to also import the pass 6 atlas v4 textures, the 4 new variant masters, the 4 new decal sheets, and the 2 sub-tile detail textures with the same compression / mipmap / wrap presets used for v3.

### Per-renderer real variant masks

`ApplyAtmosphereVariantRendererTint` now has a `(Transform root, string variantId, string beatKey)` overload. When the shared material's shader is the custom Chapter1 surface shader, the per-renderer `MaterialPropertyBlock` binds the actual variant masks (`_VariantTintMap`, `_VariantValueMap`, `_VariantSaturationMap`, `_VariantEmissiveMap`) loaded via `TryLoadVariantPass6Masks`, plus per-beat `_VariantStrength` / `_VariantEmissiveBoost` / `_SubTileStrength` resolved by `ResolveBeatVariantPass6Strengths` to match the table in `docs/Chapter1AntelaSurfaceShader.md`:

| Beat | Variant | `_VariantStrength` | `_VariantEmissiveBoost` | `_SubTileStrength` |
|---|---|---|---|---|
| S1 / S2 | Lantern_A / Day_A | 0.55 / 0.30 | 0.6 / 0.0 | 0.30 |
| S3_CurrentStreet | Dusk_A / Wet_A / Lantern_A | 0.50 / 0.55 / 0.55 | 0.3 / 0.1 / 0.5 | 0.30 |
| S3_PastMarket | GoldenHour_A / MagicHour_A | 0.55 / 0.60 | 0.3 / 0.4 | 0.30 |
| S4_CurrentField | Day_A / Dawn_A | 0.30 / 0.45 | 0.0 / 0.2 | 0.20 |
| S4_PastField | GoldenHour_A / MagicHour_A | 0.55 / 0.60 | 0.3 / 0.4 | 0.20 |
| S5_NorthRuins | Dusk_A / Snow_A / Storm_A | 0.55 / 0.60 / 0.70 | 0.0 / 0.0 / 0.1 | 0.35 |
| TimeWindow | Day_A / Dusk_A | 0.40 / 0.50 | 0.4 / 0.5 | 0.25 |

The legacy `_BaseColor` multiply path remains as fallback when the shared material is on URP/Lit (e.g., when atlas v4 hasn't landed yet, or in the legacy half of the v3-vs-v4 comparison capture).

### Post-process additions

- URP Gaussian DepthOfField added to `CreateDQ3RReviewVolume` with per-beat tuning via `ResolveBeatDepthOfField`: interior 6-14m, outdoor 10-22m, S5 fog 14-28m, Time Window 8-16m. Gaussian rather than Bokeh because all DQ3R cameras are orthographic. Caveat: `DepthOfField` Volume component is added only when `!Application.isBatchMode` because URP DOF + 4K render target + multiple particle systems triggers `RenderTexture.Create failed` warnings and a hard crash in batchmode; interactive Editor runs get the full DOF effect.
- Per-character rim-light backlight plane via `CreateCharacterRimLightPlane` — additive cool-blue plane (0.78, 0.86, 1.05, alpha 0.18) at 1.18× width × 1.05× height, -0.18m local Z behind every character billboard. Picked up by URP bloom for subtle painterly halo. Caveat: skipped in `Application.isBatchMode` because 4K + 8K captures already stress URP particle billboard rendering; interactive Editor runs get the rim halo.

### Reception tests

7 new tests in `Chapter1MapAssetTests` (`Pass6AtlasV4ManifestIsAtlasV3Compatible` / `Pass6AtlasV4TexturesArePresent` / `Pass6NewAtmosphereVariantsArePresent` / `Pass6NewDecalSheetsArePresent` / `Pass6SubTileDetailTexturesArePresent` / `Pass6CustomShaderIsPresent` / `Pass6VerifierScriptIsPresent`). Each uses `Assert.Ignore` until the corresponding pass 6 asset lands, then flips to `Pass`.

### Comparison capture

`Anemora/Assets/Capture Chapter1 Map Atlas V3 V4 Comparison Review` runs S3_Past Market beat at 1920×1080 twice — once with `ForceLegacyAtlasShader = true` (atlas v3 + URP/Lit) and once with the default pass 6 path (atlas v4 + custom shader). The Python composite `render_atlas_v3_v4_compare_sheet` in `tools/render_dq3r_review_sheets.py` produces a 3840×1080 split sheet at `docs/devlog/screenshots/stage4_chapter1_map_unity_atlas_v3_v4_compare_sheet.png` so the hand-painted detail is visible side-by-side.

Caveat: in `-batchmode -nographics`, this menu produces blank captures (URP emits `RenderTexture.Create failed` warnings during the second `LoadOrCreateMaterials` cycle and the camera renders solid). The menu still works interactively in the Editor (Window → General → Game view route), and the v3-vs-v4 visual difference is also fully observable through the existing 4K / 8K hero cinematic captures, which automatically pick up atlas v4 + custom shader once Pass 6 lands. The Python compositor gracefully skips when the comparison source PNGs are absent.

### Character v10 sync menu

`Stage4CharacterTransferReviewSetup.SyncCharacterTransferV10ProportionLock` added — graceful skip on missing source root, per-character subfolder enumeration (`hero`, `aria`, `mia`, `dario`, `luna`, `kaia`, `karla`, `kairo`) with shared and per-character file lists, copies into `Assets/Art/Sprites/Review/Chapter1DQ3RTransferV10` review-only without touching production runtime sprite GUIDs in `Assets/Art/Sprites/Hero/v2/` or `Resident_*/v2/`.

## Verification

- `python tools/verify_chapter1_next5_static.py` — 10 / 10 checks pass (Pass 5 contract unchanged after Pass 6 manifest extension).
- `python tools/verify_chapter1_next6_static.py` — 7 / 7 checks pass (Pass 6 atlas v4 / variants / decals / sub-tile / shader / UV stability).
- Targeted `Chapter1MapAssetTests` (Unity 6000.3.14f1 batchmode -runTests EditMode) — `38 / 38` (`testcasecount=38 passed=37 failed=0 skipped=1`; the single skip is the pre-existing `Chapter1MapPrefabsUseDedicatedMaterialsWhenImported`).
- `python tools/render_dq3r_review_sheets.py` — composites all 5 sheets (dashboard / TOD overview / Pass 5 rig overview / 8K hero overview / atlas v3-v4 compare). The atlas comparison sheet skips gracefully when comparison captures haven't been generated yet.

## What is still review-only

- Atlas v4 lives review-only in the manifest contract; the graphics-foundation integration uses it as the production base, but production scenes are still untouched (no scene save).
- Storm_A variant remains `review_only:true` (extreme dark, non-production). Snow_A / Lantern_A / MagicHour_A are `production_safe:true`.
- Character v10 packs (Mia / Dario / Luna / Kaia / Karla / Kairo + Aria back/left/right) remain pending Tom's external image-gen. Production runtime sprite GUIDs are NOT promoted by this pass.
- Time Window stays a thin world-space surface — Pass 6 did not add any 3D portal arch / gate. Per-beat post-process for Time Window (Bloom 0.75 / Vignette 0.40 / DOF 8-16m) keeps the ethereal read consistent.

## Next: Pass 7

Pass 7 instructions issued at `notes/_handover/anemora-map-generation-session-dq3r-next-instructions-7-animation-skybox-density-2026-05-09.md` cover the gaps Pass 6 cannot solve (animated banner cloth, time-of-day skybox, character backlight FBX kit, sub-tile families C/D/E, micro-prop density). Char session source-waiting tasks issued at `notes/_handover/anemora-character-generation-session-instructions-source-waiting-tasks-2026-05-09.md`.

## Pass 7 reception scaffolding (graceful pre-build)

Graphics foundation pre-built the receiving end so Pass 7 deliveries flip immediately on landing:

- 6 new EditMode tests in `Chapter1MapAssetTests` (`Pass7AnimatedClothManifestExists` / `Pass7SkyboxVariantsArePresent` / `Pass7CharacterBacklightKitIsPresent` / `Pass7SubTileDetailFamiliesCDEArePresent` / `Pass7MicroPropDensityKitIsPresent` / `Pass7VerifierScriptIsPresent`), each `Assert.Ignore`-skipping when assets are missing.
- `ApplyAtlasV3AndDecalImporterSettings` extended with `EnumeratePass7SkyboxTextures` (6 lat-long sRGB variants Day/Dawn/Dusk/Night/Storm/MagicHour) and `Pass7SubTileDetailTextures` (3 single-channel grain textures C OrganicMicroGrain / D FabricThread / E CrackedPlaster) so Pass 7 PNGs auto-import with the same TextureImporter preset chain Pass 6 uses.
- Path constants reserved: `Pass7AnimatedClothManifestPath`, `Pass7SkyboxManifestPath`, `Pass7CharacterBacklightManifestPath`, `Pass7MicroPropDensityManifestPath`, `Chapter1AntelaAnimatedClothShaderPath`.

Targeted test count after pass 7 reception scaffolding: `44 testcase / 38 pass / 6 skip / 0 fail`. The 6 skips flip to pass once Pass 7 lands its corresponding artifacts.

## V10 character sprite path resolver

`ResolveCharacterSpriteSheet` (added in `AnemoraChapter1MapAssetSetup.cs`) auto-upgrades per-beat rosters from V3 (`Chapter1DQ3RTransferV3/runtime_locomotion_candidates/<slug>/`) to V10 (`Chapter1DQ3RTransferV10/<slug>/runtime_candidate_64x96/`) once `SyncCharacterTransferV10ProportionLock` populates the V10 review root. `PlaceCharacterAnchorBillboardsForBeat` calls the resolver for every roster entry, so each character's V10 sprite is picked up the moment its source-PNG-driven build lands — no roster edits required. The fallback to V3 is preserved for characters whose V10 source PNGs Tom hasn't generated yet.

Sync trigger policy: graphics-foundation orchestrator runs `SyncCharacterTransferV10ProportionLock` automatically when the character session ships a per-character front-complete handover (e.g., `anemora-character-generation-claude-mia-v10-front-complete-2026-05-09.md`). Production runtime sprite GUIDs in `Assets/Art/Sprites/Hero/v2/` and `Resident_*/v2/` remain untouched until Tom's explicit promotion authorization.
