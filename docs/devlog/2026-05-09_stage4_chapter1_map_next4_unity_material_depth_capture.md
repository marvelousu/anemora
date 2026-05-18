# 2026-05-09 Stage 4 Chapter 1 Map Next4 Unity Material / Depth Capture

## Context

Pass 4 from the map generation session shipped a heavy material-depth kit (atlas v3, decal overlay sheet, 32 next4 FBX, light/shadow/depth card kit, 8 material-depth beat records). The graphics foundation side already imported the next4 FBX into 73 Chapter1Map review prefabs + 15 Chapter1DetailKit review prefabs (and the matching production-candidate roots), refreshed six close-density Unity captures, and added a nonblank / sample-color guard to `Chapter1MapAssetTests`.

This entry records the next4 Unity material slot routing, the new material-depth capture menu, and the targeted test extension that closes the loop on the next4 import.

## Changes

### `Assets/Editor/AnemoraChapter1MapAssetSetup.cs`

- Added 15 next4 helper material slots to the `MaterialColors` and `MaterialAtlasSlots` dictionaries:
  - `lightcard_window_warm`, `lightcard_candle_warm`
  - `shadowcard_eave_soft`, `shadowcard_interior_cool`
  - `fog_depth_soft`
  - `time_window_inner_rim`, `time_window_reflection`, `time_window_scuff`
  - `decal_receiver`, `depth_contact_receiver`
  - `material_mask_warm`, `material_mask_cool`, `material_mask_grime`
  - `foreground_branch_mask`, `background_silhouette_soft`
- Replaced the substring-driven emissive flag with an explicit `EmissiveMaterialKeys` HashSet so `time_window_trace` and `time_window_scuff` no longer pick up `_EMISSION` accidentally; kept the existing `window_warm` / `window_dim` / `light_warm` / `lightcard_*` / `time_window_inner_rim` / `time_window_reflection` slots as emissive.
- Refactored `ConfigureMaterialSurface` to a `SurfaceCategory` switch (`Opaque` / `AlphaBlend` / `Additive` / `Multiply` / `AlphaClip`) so light cards get URP additive blending, shadow cards get multiply blending, foreground branch / background silhouette cards get alpha-clipped opaque rendering, and `decal_receiver` stays opaque while still routing through the URP Lit pipeline.
- Added `MaterialRenderQueueOverrides` so each next4 alpha-blend slot uses the manifest's `sort_order_intent` (e.g. `fog_depth_soft` → 2470, `lightcard_*` / `time_window_inner_rim` / `time_window_reflection` → 2460, `shadowcard_*` / `time_window_scuff` / `depth_contact_receiver` / `material_mask_warm/cool` → 2450, `material_mask_grime` → 2440).
- Extended `IsTransparentReviewMaterial` to include the new transparent / additive / multiply / alpha-clip categories so `NormalStrengthForMaterial` returns the conservative 0.04 strength for soft cards.
- Added `TryResolveNext4MaterialByName` ahead of the existing substring router in `ResolveMaterial` so next4 imported material names resolve to the new dedicated slots before falling through to legacy `shadow` / `fog` / `window` / `light` matches.
- Added `ApplyAtlasV3AndDecalImporterSettings()` and called it from `ApplyChapter1MapImport()`. The helper enforces consistent texture importer settings for the surface atlas v3 albedo / normal / ORM / height-or-edge PNGs and the transparent decal overlay albedo / normal PNGs (Texture2D, Repeat, Bilinear, mipmaps on, max size 1024, sRGB only on albedo, NormalMap importer type for normals, no sprite mode, default compression).
- Added the menu method `[MenuItem("Anemora/Assets/Capture Chapter1 Map Next4 Material Depth Review")]` plus `LoadNext4MaterialDepthManifest`, `SyntheticNext4Bundle`, `PlaceNext4MaterialDepthBeat`, `CreateNext4PanelBase`, `Next4ReviewTargetDimension`, and `Next4CapturePathForBeat`. The menu reads `chapter1_next4_material_depth_manifest.json`, instantiates each beat's production-candidate prefabs (plus the shared light/shadow/depth card kit), reuses the existing close-density render settings / lighting / camera helpers, and writes one 1920 x 1080 PNG per beat to `docs/devlog/screenshots/stage4_chapter1_map_unity_next4_<beat_key_lower>_material_depth.png`.

### `Assets/Tests/EditMode/Chapter1MapAssetTests.cs`

- Added five tests that close the next4 loop:
  - `Chapter1MapNext4MaterialDepthManifestExists` — checks that `chapter1_next4_material_depth_manifest.json` exists, has 8 beat records, has at least 15 helper slot records, and exposes a non-empty `shared_helper_asset_id`.
  - `Chapter1MapNext4MaterialSlotsAreRouted` — uses reflection (via `AppDomain.CurrentDomain.GetAssemblies()` and `SafeGetTypes`) to read the editor's private `MaterialColors` and `MaterialAtlasSlots` dictionaries and asserts that all 15 next4 slot keys are present.
  - `Chapter1MapNext4MaterialDepthReviewCapturesExist` — validates the eight PNGs exist with `1920 x 1080` dimensions and at least 16 unique sample colors using the shared `AssertPngHasVisibleSampleVariety` guard. The test uses `Assert.Ignore` if the menu has not yet been run in the worktree, mirroring the production-prefab-import-not-yet-run pattern from `Chapter1MapPrefabsUseDedicatedMaterialsWhenImported`.
  - `Chapter1MapAtlasV3TexturesAreImportedAsExpected` — checks the atlas v3 albedo / normal / ORM (and optional height-or-edge) PNGs have the expected texture importer settings.
  - `Chapter1MapDecalOverlayTexturesAreImportedAsExpected` — checks the decal overlay albedo (and optional normal) PNGs have the expected texture importer settings.

## Verification

- Unity batchmode `executeMethod` on `Anemora.Editor.AnemoraChapter1MapAssetSetup.CaptureChapter1MapNext4MaterialDepthReview` exited with code `0` after generating eight 1920 x 1080 production-candidate review PNGs (file sizes between approximately 150 KB and 250 KB):
  - `docs/devlog/screenshots/stage4_chapter1_map_unity_next4_s1_library_material_depth.png`
  - `docs/devlog/screenshots/stage4_chapter1_map_unity_next4_s2_miahouse_material_depth.png`
  - `docs/devlog/screenshots/stage4_chapter1_map_unity_next4_s3_current_material_depth.png`
  - `docs/devlog/screenshots/stage4_chapter1_map_unity_next4_s3_past_material_depth.png`
  - `docs/devlog/screenshots/stage4_chapter1_map_unity_next4_s4_current_material_depth.png`
  - `docs/devlog/screenshots/stage4_chapter1_map_unity_next4_s4_past_material_depth.png`
  - `docs/devlog/screenshots/stage4_chapter1_map_unity_next4_s5_north_material_depth.png`
  - `docs/devlog/screenshots/stage4_chapter1_map_unity_next4_timewindow_adjacent_material_depth.png`
- Unity log scan: no compile errors, no shader errors, no exceptions, no asserts, no null refs, no missing methods. Known non-blocking noise: Unity licensing / socket startup messages, the existing `TextureImporter.spritesheet` obsolete API warning in `Stage4CharacterTransferReviewSetup`, and a transient FBX `SourceAssetDB` modification-time mismatch on `Ch1_S5_NorthRuins_Current_DQ3RClose.fbx` that resolved during reimport.
- Production scene was not opened or saved.

- Targeted EditMode `Anemora.Tests.EditMode.Chapter1MapAssetTests` passed `17/17` (12 existing + 5 new), 0 failed, 0 inconclusive, 0 skipped, total duration approximately 5.3 seconds. Result XML at `%TEMP%\AnemoraCodexLogs\20260509_gfx_foundation_next4_tests\chapter1_map_tests.xml`.
- Unity batchmode side effects were cleaned up after the run: `Assets/AddressableAssetsData/link.xml`, `link.xml.meta`, `ProjectSettings/GraphicsSettings.asset`, and `ProjectSettings/QualitySettings.asset` were restored from git, and `Assets/AddressableAssetsData/Windows.meta` and `tools/__pycache__/` were removed.

## Notes / Caveats

- The Unity batchmode menu also re-runs `ApplyChapter1MapImport` to keep the production-candidate prefabs and material/texture cache aligned with the next4 manifest. This is intentional but means a clean-state run takes longer than the targeted capture alone.
- `decal_receiver` is the only opaque next4 slot. It still appears in `MaterialAtlasSlots` so the existing atlas tile cache path can populate a base texture for it.
- `ResolveSurfaceCategory` returns `Opaque` for `decal_receiver` explicitly; the rest of the next4 keys are routed by the `AdditiveMaterialKeys` / `MultiplyMaterialKeys` / `AlphaClipMaterialKeys` HashSets or fall through to the alpha-blend path via `MaterialRenderQueueOverrides`.
- The `_Blend` URP Lit shader property values used here follow the URP enum (`Alpha=0`, `Premultiply=1`, `Additive=2`, `Multiply=3`); the actual blend factors (`_SrcBlend` / `_DstBlend`) are still the source of truth for rendering.
