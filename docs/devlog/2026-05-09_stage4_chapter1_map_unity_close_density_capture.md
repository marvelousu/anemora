# Stage 4 Chapter 1 Map Unity Close-Density Capture

Date: 2026-05-09
Branch: `codex/stage4-graphics-foundation-20260508`

## Scope

This pass adds Unity-side close-density review capture automation for the Chapter 1 next3 map assembly set, then verifies the newly arrived next4 material/depth FBX batch imports through the same prefab pipeline. It does not save a production scene.

The goal is to review the map-generation session's S3 / S4 / S5 / Time Window density output under Unity materials and lighting, using production-candidate prefabs where possible so review-only route / character marker renderers stay hidden.

## Implementation

Updated `Assets/Editor/AnemoraChapter1MapAssetSetup.cs` with:

- Unity menu: `Anemora/Assets/Capture Chapter1 Map Close Density Review`
- Batchmode method: `Anemora.Editor.AnemoraChapter1MapAssetSetup.CaptureChapter1MapCloseDensityReview`
- `chapter1_scene_assembly_manifest.json` capture-priority support
- production-candidate prefab preference for close captures
- scene assembly density variant selection
- placement-manifest dressing placement inclusion by `camera_bundle_id`
- review-scale normalization before capture
- fixed-pitch HD-2D camera framing after bounds normalization
- per-era close-density lighting / fog defaults

The first attempt produced blank / off-frame captures because the generated FBX bounds were hundreds of Unity units wide after import and the camera was placed too far into fog. The final implementation normalizes each review bundle to a capture target dimension before computing the camera, matching the existing prefab / placement review strategy.

## Captures

| Capture | SHA256 |
|---|---|
| `docs/devlog/screenshots/stage4_chapter1_map_unity_close_s3_currentstreet.png` | `40DF394975F7DAFE9832D070F54C13132457AE876A676C455088D358CC55A5DF` |
| `docs/devlog/screenshots/stage4_chapter1_map_unity_close_s3_pastmarket.png` | `CCC6AEDF552DF086BC32A565D611E61997BFB37F1343984DE9B4D2DD3FB3A988` |
| `docs/devlog/screenshots/stage4_chapter1_map_unity_close_s4_currentfield.png` | `520A103CE9B9DAA1037BE3B88D88C3C5EC68E6B00846D2A84E7B4F4C62642A93` |
| `docs/devlog/screenshots/stage4_chapter1_map_unity_close_s4_pastfield.png` | `3DA3C7BAEBB32B103B618910D7E9269FB0151B66F413F245EE84194B17ABEF80` |
| `docs/devlog/screenshots/stage4_chapter1_map_unity_close_s5_northruins.png` | `89971FA65697107A405FB8B23370C869B00C441EFF3896D09FDDB18BEE829DA0` |
| `docs/devlog/screenshots/stage4_chapter1_map_unity_close_timewindow_adjacent.png` | `4C1818FA704D701D2A9AE8AFC0D194B44B92498B12E9182DB8B15C4DBEA068C4` |

All captures are `1920 x 1080`. A sample-color pass confirmed the captures are nonblank after the normalization fix, and `Chapter1MapCloseDensityReviewCapturesExist` now asserts sample-color variety so a future off-frame or blank capture fails tests instead of silently passing dimensions only.

## Next4 Import Sync

After the map-generation session produced the next4 material/depth batch, the same Unity import automation generated:

- `73` Chapter1Map review prefabs.
- `15` Chapter1DetailKit review prefabs.
- `73` Chapter1Map production-candidate prefabs.
- `15` Chapter1DetailKit production-candidate prefabs.

`Chapter1MapManifestCoversExpectedAssets` now treats `chapter1_next4_material_depth_manifest.json` and `chapter1_next4_production_clean_manifest.json` as valid coverage for next4 FBX files, while the original `chapter1_map_assets_manifest.json` remains the authoritative triangle-budget manifest for the next3 base set.

## Verification

- Unity batchmode `Anemora.Editor.AnemoraChapter1MapAssetSetup.CaptureChapter1MapCloseDensityReview`: passed.
- Targeted Unity EditMode `Anemora.Tests.EditMode.Chapter1MapAssetTests`: `12/12` passed.
- Latest results: `%TEMP%/AnemoraCodexLogs/20260509_gfx_foundation_next4_guard_tests/chapter1_map_tests_next4_guard.xml`.
- Log scan found no compiler errors, shader errors, exceptions, asserts, null references, or missing-method errors. The only scan hit was the existing `TextureImporter.spritesheet` obsolete API warning in `Stage4CharacterTransferReviewSetup`.

## Read

The close captures show that the next3 map assets now have enough mass and layering for Unity-side review, but the visual ceiling is still limited by material painting and overlay control. The next map-generation pass should focus on atlas v3, separate decal overlays, foreground safety, and light / shadow / fog cards rather than more raw scene modules.
