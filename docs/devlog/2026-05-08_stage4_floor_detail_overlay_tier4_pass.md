# Stage 4 Floor Detail Overlay Tier 4 Pass

Date: 2026-05-08
Branch: `codex/stage4-graphics-foundation-20260508`

## Scope

This pass adds a production-facing floor detail overlay to the Stage 4 diorama boundary prefabs. The goal is to make the main floor read less like a flat procedural plane by layering chipped tile seams, small crack strokes, and faint edge highlights over the existing floor-underlay and floor-breakup passes.

This is still a graphics-foundation pass, not a final authored environment-art pass. The result improves material density, but it does not by itself reach a DQ3R-class HD-2D look; that level still requires stronger authored 3D set pieces, composition work, asset-specific texture art, and final lighting direction.

## Assets

- Added `Assets/Art/Materials/Zone1/Stage4FloorDetailOverlay.shader`.
- Added Current / Past floor detail materials:
  - `Assets/Art/Materials/Zone1/Stage4_FloorDetail_Current.mat`
  - `Assets/Art/Materials/Zone1/Stage4_FloorDetail_Past.mat`
- Updated `Assets/Editor/Stage4DioramaBoundarySetup.cs` so `ApplyStage4DioramaBoundaries` creates the floor detail materials and adds a non-colliding `FloorDetailChippedTileOverlay` quad to each Stage 4 diorama boundary prefab.
- Updated `Assets/Prefabs/Zone1/Stage4_DioramaBoundary_Current.prefab` and `Assets/Prefabs/Zone1/Stage4_DioramaBoundary_Past.prefab`.
- Extended `GraphicsFoundationAssetTests.MainSceneUsesStage4DioramaBoundaryDepthPrefabs` to guard the floor detail shader, material parameters, prefab child, and material references.

## Visual Artifacts

Refreshed main-scene graphics review captures:

- `docs/devlog/screenshots/stage4_main_scene_graphics_current.png`
  - SHA-256: `B9D5806EE0A6A470AE4E5F8DDE2D41472C1B60E842FBEDECECAD94FC19366605`
- `docs/devlog/screenshots/stage4_main_scene_graphics_proposed_soft.png`
  - SHA-256: `BA03CB61B3DE7320D607E92589FA9A68E8D98A6B7A48F4B619E32F52777F2510`
- `docs/devlog/screenshots/stage4_main_scene_graphics_review_sheet.png`
  - SHA-256: `8ABFFB8ECC23E7B0C476DB4A8A1817E3F9B29C08F63687C6ED3C762A93433D45`

## Verification

- `Stage4DioramaBoundarySetup.ApplyStage4DioramaBoundaries`: exit `0`.
- `Stage4GraphicsBaselineCapture.CaptureMainSceneSoftGradeReview`: exit `0`.
  - Final capture log checked for `Shader error`, `Shader warning`, `RenderTexture.Create failed`, `DrawObjectsPass`, `RecordRenderGraph`, `RenderGraph`, `Exception`, `NullReference`, `MissingReference`, and `Assertion failed`: `0`.
- Targeted EditMode `Anemora.Tests.EditMode.GraphicsFoundationAssetTests`: `19/19` passed.
- Targeted PlayMode `Anemora.Tests.PlayMode.MainSceneStartupLogTests`: `3/3` passed.
- Windows build smoke:
  - `Builds/Stage4Smoke/2026-05-08-graphics-foundation-floor-detail/Anemora_Stage4_GraphicsFoundation_FloorDetail_Smoke.exe`
  - Build folder size: `126.544 MiB`.
- 30 second player smoke:
  - Player log checked for `Error`, `Exception`, `Assert`, `DrawObjectsPass`, `RecordRenderGraph`, `RenderGraph`, `NullReference`, `MissingReference`, `Failed`, `TextMesh Pro Essential Resources`, and `ScreenSpaceAmbientOcclusion`: `0`.

## Caveats

- The local workspace contains unrelated untracked Prototype assets with compile errors. Verification temporarily moved those Prototype paths outside `Assets/` during Unity runs and restored them afterward. The committed graphics files do not depend on the Prototype files.
- Graphics-enabled capture was used for the final screenshot run. `-nographics` capture previously produced `RenderTexture.Create failed`, so it is not a valid mode for visual capture.
- The visible result is incremental. The next higher-impact Tier 4 work should target authored set-piece surface language, foreground/midground silhouette density, lighting composition, and final camera review rather than additional tiny shader constants.
