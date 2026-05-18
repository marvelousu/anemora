# Stage 4 Chapter 1 Map Next3 Unity Import

Date: 2026-05-09
Branch: `codex/stage4-graphics-foundation-20260508`

## Scope

This pass imports the latest Chapter 1 / Antela map batch into Unity review and production-candidate prefab roots. It does not save a production scene.

## Inputs

- `Assets/Art/Models/Zone1/Chapter1Map/`
- `Assets/Art/Models/Zone1/Chapter1DetailKit/`
- `Assets/Art/Models/Zone1/Chapter1Map/chapter1_map_assets_manifest.json`
- `Assets/Art/Models/Zone1/Chapter1Map/chapter1_unity_placement_manifest.json`
- `Assets/Art/Textures/Zone1/Chapter1/chapter1_antela_surface_atlas_manifest.json`

## Unity Import

Updated `Assets/Editor/AnemoraChapter1MapAssetSetup.cs` so the import/review path follows the manifest-era asset set instead of a fixed 30-prefab list.

Current imported counts:

- `41` Chapter1Map review prefabs under `Assets/Prefabs/Zone1/Chapter1Map/`
- `14` Chapter1DetailKit review prefabs under `Assets/Prefabs/Zone1/Chapter1DetailKit/`
- `41` Chapter1Map production-candidate prefabs under `Assets/Prefabs/Zone1/Chapter1MapProduction/`
- `14` Chapter1DetailKit production-candidate prefabs under `Assets/Prefabs/Zone1/Chapter1DetailKitProduction/`

Newly covered groups include:

- S1 / S2 / S3 / S4 / S5 scene assembly A/B FBX assets
- `Ch1_Dressing_*_AB` dressing kits
- `Ch1_LightingShadowHelperKit`
- `Ch1_FogAndDepthHelperKit`
- `Ch1_TimeWindowSurfaceHelperKit`

The production-candidate prefab writer now treats `Ch1_Dressing_*` and `*HelperKit` assets as DetailKit assets, and it hides renderers whose imported material names are fully review-only marker slots.

## Captures

Refreshed:

- `docs/devlog/screenshots/stage4_chapter1_map_prefab_review.png`
  - SHA256: `7EE83871D413D756321F9E862966F113ACFB64C565C7455DCAF75E4B300DB2DB`
- `docs/devlog/screenshots/stage4_chapter1_map_unity_placement_review.png`
  - SHA256: `909599527E405D7282D9D0ED54F8DBE2D6EC2C0570091FEA3B03DDA44C574063`

The placement capture consumes the current schema v3 placement manifest and remains a temporary-scene review capture.

## Verification

- Unity batchmode `Anemora.Editor.AnemoraChapter1MapAssetSetup.CaptureChapter1MapPrefabReview`: passed.
- Unity batchmode `Anemora.Editor.AnemoraChapter1MapAssetSetup.CaptureChapter1MapPlacementReview`: passed.
- Targeted Unity EditMode `Anemora.Tests.EditMode.Chapter1MapAssetTests`: `11/11` passed.
- Latest results: `%TEMP%/AnemoraCodexLogs/20260509_gfx_foundation_next3_tests/chapter1_map_tests.xml`.
- Log scan found no compiler errors, shader errors, exceptions, asserts, null references, or missing-method errors beyond known Unity licensing/socket startup noise.

## Read

This closes the gap between the latest map-generation output and Unity review. The next visual risk is no longer missing import coverage; it is whether the scene assembly / dressing / helper-kit material grouping is rich enough under the game camera. The auto material remap is intentionally conservative, so the next Tier4 pass should focus on lighting, transparent helper balance, and camera-framed close captures instead of more raw FBX count.
