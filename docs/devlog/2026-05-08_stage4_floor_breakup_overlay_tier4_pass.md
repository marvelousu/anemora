# Stage 4 Floor Breakup Overlay Tier 4 Pass

Date: 2026-05-08
Branch: `codex/stage4-graphics-foundation-20260508`

## Scope

Continued the Tier 4 graphics push after the diorama-boundary pass. The immediate visual target was the main scene's repeated square-floor read: it still looked like a flat tiled board even after soft shadows, fog, soft contact occlusion, and backdrop depth.

This pass stays production-safe: no gameplay logic, portal flip ordering, camera controls, story, NPC design, sprite adoption, or final palette decisions were changed.

## Changes

- Added `Anemora/Zone1/FloorBreakupOverlay`, a transparent URP procedural overlay shader for soft floor wear, stain, and broad tonal breakup.
- Added Current / Past floor-breakup materials with restrained alpha and side-specific tinting.
- Extended `Stage4DioramaBoundarySetup` so the existing Current / Past diorama-boundary prefabs include five non-colliding floor overlay quads.
- Guarded the shader, materials, render queue, prefab children, and material references through `GraphicsFoundationAssetTests.MainSceneUsesStage4DioramaBoundaryDepthPrefabs`.
- Refreshed main-scene graphics review screenshots after the overlay pass.

## Assets

- `Assets/Art/Materials/Zone1/Stage4FloorBreakupOverlay.shader`
- `Assets/Art/Materials/Zone1/Stage4_FloorBreakup_Current.mat`
- `Assets/Art/Materials/Zone1/Stage4_FloorBreakup_Past.mat`
- `Assets/Editor/Stage4DioramaBoundarySetup.cs`
- `Assets/Prefabs/Zone1/Stage4_DioramaBoundary_Current.prefab`
- `Assets/Prefabs/Zone1/Stage4_DioramaBoundary_Past.prefab`
- `Assets/Tests/EditMode/GraphicsFoundationAssetTests.cs`

## Screenshots

- `docs/devlog/screenshots/stage4_main_scene_graphics_current.png`
  - SHA-256: `8D70AC56B5EE458F6853204A12630F0515A932F94EF5BF6395D7211AE4AC33FD`
- `docs/devlog/screenshots/stage4_main_scene_graphics_proposed_soft.png`
  - SHA-256: `8D58BFCF18ED2584AF97A6F5CBCD6F6CCD3483754A0FA0A3A6859908C82DB589`
- `docs/devlog/screenshots/stage4_main_scene_graphics_review_sheet.png`
  - SHA-256: `57845E483646AE705909A7325D73718F5FCBEDA169A9F240DC79C7F2AF891AD4`

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
  - Output: `Builds/Stage4Smoke/2026-05-08-graphics-foundation-floor-breakup/Anemora_Stage4_GraphicsFoundation_FloorBreakup_Smoke.exe`
  - Build folder: `194` files, `126.565 MiB`
- 30 second player log smoke:
  - checked-pattern count: `0`

## Visual Assessment

The overlay makes the floor less mechanically repetitive, especially in the foreground and around the portal / central play area. It is still not a DQ3R-class HD-2D read. The main blockers remain authored environment density, stronger screen composition, richer depth-layer silhouettes, and higher-quality floor / wall asset language. The next Tier 4 pass should move from subtle renderer polish into visible set dressing and composition density.
