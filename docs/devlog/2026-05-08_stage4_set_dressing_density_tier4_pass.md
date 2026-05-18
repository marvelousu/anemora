# Stage 4 Set Dressing Density Tier 4 Pass

Date: 2026-05-08
Branch: `codex/stage4-graphics-foundation-20260508`

## Scope

Continued the push toward a more legible HD-2D scene read after the floor-breakup overlay. The floor was less repetitive, but the shot still lacked a strong authored background silhouette and foreground / midground density.

This pass stays within production-safe prefab polish. It does not change gameplay logic, portal flip ordering, story, NPC design, sprite adoption, camera controls, or final palette direction.

## Changes

- Added Current / Past diorama foreground silhouette materials.
- Extended the Stage 4 diorama-boundary prefabs with asymmetric broken back-arch pieces, midground broken columns and low rubble, foreground silhouettes, and side midground dressing using existing Zone1 prefabs.
- Kept all added set dressing non-colliding and on the correct Current / Past visual layers.
- Guarded the new materials and prefab children through `GraphicsFoundationAssetTests.MainSceneUsesStage4DioramaBoundaryDepthPrefabs`.
- Refreshed main-scene graphics review screenshots.

## Assets

- `Assets/Art/Materials/Zone1/Stage4_DioramaForeground_Current.mat`
- `Assets/Art/Materials/Zone1/Stage4_DioramaForeground_Past.mat`
- `Assets/Editor/Stage4DioramaBoundarySetup.cs`
- `Assets/Prefabs/Zone1/Stage4_DioramaBoundary_Current.prefab`
- `Assets/Prefabs/Zone1/Stage4_DioramaBoundary_Past.prefab`
- `Assets/Tests/EditMode/GraphicsFoundationAssetTests.cs`

## Screenshots

- `docs/devlog/screenshots/stage4_main_scene_graphics_current.png`
  - SHA-256: `9B2933B586DC6A4F9CD2EB228511E88503D5889CD9B4EFA3D59A9E2741829D5E`
- `docs/devlog/screenshots/stage4_main_scene_graphics_proposed_soft.png`
  - SHA-256: `815DB7093C865F3CD3FC70B824128BEBC6AB5B215889F3537F8365B0BDE500DC`
- `docs/devlog/screenshots/stage4_main_scene_graphics_review_sheet.png`
  - SHA-256: `2F6CAFE143438D33085A56F96923F025C92316BD7CFB768E4810E7E639583A94`

## Verification

- `Stage4DioramaBoundarySetup.ApplyStage4DioramaBoundaries`
  - exit `0`
  - `error CS` / shader error / shader warning / exception / null / missing reference: `0`
- `Stage4GraphicsBaselineCapture.CaptureMainSceneSoftGradeReview`
  - exit `0`
  - shader error / shader warning / DrawObjectsPass / RecordRenderGraph / RenderGraph / exception: `0`
- Targeted EditMode:
  - `Anemora.Tests.EditMode.GraphicsFoundationAssetTests`
  - `19/19` passed
- Targeted PlayMode:
  - `Anemora.Tests.PlayMode.MainSceneStartupLogTests`
  - `3/3` passed
- Windows build smoke:
  - Output: `Builds/Stage4Smoke/2026-05-08-graphics-foundation-set-dressing-density/Anemora_Stage4_GraphicsFoundation_SetDressingDensity_Smoke.exe`
  - Build folder: `193` files, `126.502 MiB`
- 30 second player log smoke:
  - checked-pattern count: `0`

## Workspace Caveat

The local workspace contains unrelated untracked Prototype assets with editor compile errors. Unity verification in this pass temporarily moved only those untracked Prototype paths outside `Assets/` and restored them afterward; they are not staged or part of this graphics commit.

## Visual Assessment

This pass is more visible than the earlier shader-only work: the shot now has a broken back-arch silhouette and additional edge / midground dressing, so the scene reads less like an empty tiled board. It is still not DQ3R-class. The next visual blockers are authored texture quality, stronger material language on the floor / ruin pieces, and camera / composition decisions that may need explicit approval if applied to production.
