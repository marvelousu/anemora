# Stage 4 Diorama Terrace Tier 4 Pass

Date: 2026-05-08
Branch: `codex/stage4-graphics-foundation-20260508`

## Scope

This pass moves the Stage 4 main-scene environment a step away from a flat prototype floor by adding non-colliding raised terrace geometry, side walks, broken front steps, and a framed portal approach to the Current / Past diorama-boundary prefabs.

The intent is to make the scene read more like an HD-2D diorama set: layered floor height, visible risers, foreground step silhouettes, and material separation between the base stone and trim stone. This is still below a DQ3R-class authored environment because the assets remain mostly procedural / reused prefabs, but it is a more visible Tier 4 direction than small shader-value polish.

## Assets

- Added Current / Past trim materials:
  - `Assets/Art/Materials/Zone1/Stage4_DioramaTrim_Current.mat`
  - `Assets/Art/Materials/Zone1/Stage4_DioramaTrim_Past.mat`
- Updated `Assets/Editor/Stage4DioramaBoundarySetup.cs` so `ApplyStage4DioramaBoundaries` creates the trim materials and adds the terrace / step geometry to both diorama-boundary prefabs.
- Updated `Assets/Prefabs/Zone1/Stage4_DioramaBoundary_Current.prefab` and `Assets/Prefabs/Zone1/Stage4_DioramaBoundary_Past.prefab`.
- Cleaned stale `_SURFACE_TYPE_TRANSPARENT` invalid keywords from the custom floor-breakup overlay materials and stopped reapplying that keyword for custom floor-breakup / floor-detail shaders.
- Extended `GraphicsFoundationAssetTests.MainSceneUsesStage4DioramaBoundaryDepthPrefabs` to guard trim materials, raised terrace children, broken front steps, portal approach trim, and the custom overlay keyword cleanup.

## Visual Artifacts

Refreshed main-scene graphics review captures:

- `docs/devlog/screenshots/stage4_main_scene_graphics_current.png`
  - SHA-256: `92C906230042F0D0CF969647A5367A5CE0C3CCA3A79AEDB13A5B866BF4FCB5DC`
- `docs/devlog/screenshots/stage4_main_scene_graphics_proposed_soft.png`
  - SHA-256: `9406E642F66293CAFCEDF6732E7D0401ABABF8914362717A99C8EA21ED1707B9`
- `docs/devlog/screenshots/stage4_main_scene_graphics_review_sheet.png`
  - SHA-256: `3EB0733F0DC3601999694019C54F9DC05C585893E0B92F1B123A508795C2D005`

## Verification

- `Stage4DioramaBoundarySetup.ApplyStage4DioramaBoundaries`: exit `0`.
  - Checked log patterns for shader / CS / exception / null / missing-reference failures: `0`.
- `Stage4GraphicsBaselineCapture.CaptureMainSceneSoftGradeReview`: exit `0`.
  - Checked patterns for `Shader error`, `Shader warning`, `RenderTexture.Create failed`, `DrawObjectsPass`, `RecordRenderGraph`, `RenderGraph`, `Exception`, `NullReference`, `MissingReference`, `Assertion failed`, and `CS[0-9]{4}`: `0`.
- Targeted EditMode `Anemora.Tests.EditMode.GraphicsFoundationAssetTests`: `19/19` passed.
- Targeted PlayMode `Anemora.Tests.PlayMode.MainSceneStartupLogTests`: `3/3` passed.
- Windows build smoke:
  - `Builds/Stage4Smoke/2026-05-08-graphics-foundation-diorama-terrace/Anemora_Stage4_GraphicsFoundation_DioramaTerrace_Smoke.exe`
  - Build folder size: `126.532 MiB`.
  - `BuildFailedException`, shader errors / warnings, CS errors, null / missing references: `0`.
  - Log contains known code coverage `ReportGeneratorMerged.dll` System.Numerics resolve messages; they did not fail the build.
- 30 second player smoke:
  - Player log checked for `Error`, `Exception`, `Assert`, `DrawObjectsPass`, `RecordRenderGraph`, `RenderGraph`, `NullReference`, `MissingReference`, `Failed`, `TextMesh Pro Essential Resources`, and `ScreenSpaceAmbientOcclusion`: `0`.

## Caveats

- The local workspace still contains unrelated untracked Prototype assets with compile errors. Unity verification temporarily moved those paths outside `Assets/` and restored them after each run.
- The raised terrace pass is intentionally production-facing, but it remains conservative: no camera, palette, story, or gameplay-collider changes were made.
- The scene is visually denser now, but the next major leap toward a DQ3R-like result requires either imported / authored Zone1 meshes and textures or a deliberate camera/composition production change.
