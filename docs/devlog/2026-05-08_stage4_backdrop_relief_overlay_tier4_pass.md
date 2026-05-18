# Stage 4 Backdrop Relief Overlay Tier 4 Pass

Date: 2026-05-08
Branch: `codex/stage4-graphics-foundation-20260508`

## Scope

Continued the Tier 4 visual push after the set-dressing density pass. The broken arch and foreground silhouettes improved composition, but the back wall still read as a large flat plane. This pass adds a restrained procedural masonry / wear overlay to the existing diorama backdrop.

No gameplay logic, portal flip ordering, story, NPC design, sprite adoption, camera controls, or final palette direction were changed.

## Changes

- Added `Anemora/Zone1/DioramaReliefOverlay`, a transparent URP overlay shader for broken masonry seams, wall stains, and procedural wear.
- Added Current / Past diorama relief materials with side-specific tint and line alpha.
- Extended `Stage4DioramaBoundarySetup` to place a non-colliding `BackdropReliefOverlay` quad in each Current / Past diorama-boundary prefab.
- Guarded the relief shader, materials, render queue, prefab child, and material references through `GraphicsFoundationAssetTests.MainSceneUsesStage4DioramaBoundaryDepthPrefabs`.
- Refreshed main-scene graphics review screenshots.

## Assets

- `Assets/Art/Materials/Zone1/Stage4DioramaReliefOverlay.shader`
- `Assets/Art/Materials/Zone1/Stage4_DioramaRelief_Current.mat`
- `Assets/Art/Materials/Zone1/Stage4_DioramaRelief_Past.mat`
- `Assets/Editor/Stage4DioramaBoundarySetup.cs`
- `Assets/Prefabs/Zone1/Stage4_DioramaBoundary_Current.prefab`
- `Assets/Prefabs/Zone1/Stage4_DioramaBoundary_Past.prefab`
- `Assets/Tests/EditMode/GraphicsFoundationAssetTests.cs`

## Screenshots

- `docs/devlog/screenshots/stage4_main_scene_graphics_current.png`
  - SHA-256: `197FE3B08A9434D577D893BB4766C2253C0BB70BBB6A4A2283BD0939851830F5`
- `docs/devlog/screenshots/stage4_main_scene_graphics_proposed_soft.png`
  - SHA-256: `37C55A314F5E2F94E05D8DB56C3EC7C6B5B2A9C4D734C7C5AB307988B84889B5`
- `docs/devlog/screenshots/stage4_main_scene_graphics_review_sheet.png`
  - SHA-256: `5FA05232C27714FFA0A2AF08C7715CB1F680863042CC8F7E874216600B7A55BF`

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
  - Output: `Builds/Stage4Smoke/2026-05-08-graphics-foundation-relief-overlay/Anemora_Stage4_GraphicsFoundation_ReliefOverlay_Smoke.exe`
  - Build folder: `192` files, `126.325 MiB`
- 30 second player log smoke:
  - checked-pattern count: `0`

## Workspace Caveat

The local workspace still contains unrelated untracked Prototype assets with editor compile errors. Unity verification temporarily moved only those untracked Prototype paths outside `Assets/` and restored them afterward; they are not staged or part of this graphics commit.

## Visual Assessment

The back plane now has visible masonry / wear structure instead of a blank gradient, so the scene has a stronger HD-2D layered backdrop read. It remains below DQ3R quality because the geometry is still procedural and low-poly, not authored production environment art. The next blocker is material language on the floor / ruin pieces and camera composition.
