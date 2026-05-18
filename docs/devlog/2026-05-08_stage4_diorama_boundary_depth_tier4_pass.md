# Stage 4 Diorama Boundary Depth Tier 4 Pass

Date: 2026-05-08
Branch: `codex/stage4-graphics-foundation-20260508`

## Scope

Responded to the visual gap that the main scene still read as a flat tiled board after the earlier Tier 4 lighting / fog / soft-contact passes. This pass promotes a production-safe diorama boundary and far-depth layer into `Anemora_Main` without changing gameplay portal ordering, camera controls, story, NPC design, or map logic.

## Changes

- Added `Stage4DioramaBoundarySetup` editor automation to create and place Current / Past diorama-boundary prefabs under the existing Zone1 side roots.
- Added low floor lips / side skirts to reduce the black void around the play surface.
- Added `Anemora/Zone1/DioramaBackdrop`, a small URP shader for a stable procedural backdrop gradient with subtle grain, avoiding PNG gradient banding.
- Added Current / Past backdrop, horizon mist, boundary, and distant-silhouette materials.
- Added non-colliding distant set-piece silhouettes using existing Zone1 prefabs, tinted through dedicated distant materials so they read as background depth rather than interactable foreground content.
- Refreshed main-scene graphics review captures.

## Assets

- `Assets/Editor/Stage4DioramaBoundarySetup.cs`
- `Assets/Art/Materials/Zone1/Stage4DioramaBackdrop.shader`
- `Assets/Art/Materials/Zone1/Stage4_DioramaBoundary_Current.mat`
- `Assets/Art/Materials/Zone1/Stage4_DioramaBoundary_Past.mat`
- `Assets/Art/Materials/Zone1/Stage4_DioramaBackdrop_Current.mat`
- `Assets/Art/Materials/Zone1/Stage4_DioramaBackdrop_Past.mat`
- `Assets/Art/Materials/Zone1/Stage4_DioramaMist_Current.mat`
- `Assets/Art/Materials/Zone1/Stage4_DioramaMist_Past.mat`
- `Assets/Art/Materials/Zone1/Stage4_DioramaDistant_Current.mat`
- `Assets/Art/Materials/Zone1/Stage4_DioramaDistant_Past.mat`
- `Assets/Prefabs/Zone1/Stage4_DioramaBoundary_Current.prefab`
- `Assets/Prefabs/Zone1/Stage4_DioramaBoundary_Past.prefab`

## Screenshots

- `docs/devlog/screenshots/stage4_main_scene_graphics_current.png`
  - SHA-256: `2D4EE97B3789C4EE6F05DA6FB4A12C050AC6361F0A1B36FB6995EBC4C45BD8F9`
- `docs/devlog/screenshots/stage4_main_scene_graphics_proposed_soft.png`
  - SHA-256: `40F34C95B79B6345A2659A309A9168D3BF470180F8085C07D0B274E50921CDB8`
- `docs/devlog/screenshots/stage4_main_scene_graphics_review_sheet.png`
  - SHA-256: `4256FD7F966B07C676D2D9CF1C712ABFCB3B7B2330BD4D3CF15C9FF9EF1112F8`

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
  - Output: `Builds/Stage4Smoke/2026-05-08-graphics-foundation-diorama-boundary/Anemora_Stage4_GraphicsFoundation_DioramaBoundary_Smoke.exe`
  - Build folder: `192` files, `126.307 MiB`
- 30 second player log smoke:
  - checked-pattern count: `0`

## Visual Assessment

This is a visible improvement over the prior lighting-only passes: the scene no longer falls into a pure black void beyond the floor, and the far side now has a backdrop, haze band, and silhouetted geometry. It is still not DQ3R-class HD-2D. The next major blockers are the repeated square floor pattern, low foreground/background composition density, and the current camera framing. The next graphics pass should attack floor surface repetition before making a larger camera decision.
