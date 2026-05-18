# Stage 4 graphics capture artifact guard

Date: 2026-05-08
Scope: GFX-2 Stage 4 visual baseline profile / verification hardening
Branch: `codex/stage4-graphics-foundation-20260508`

## Summary

Added EditMode guards for the generated Stage 4 graphics review PNGs. This protects the graphics review loop from silently keeping missing, blank, corrupt, wrong-size, or unchanged current/proposed capture artifacts.

## Changes

- `GraphicsFoundationAssetTests.Stage4GraphicsReviewScreenshotsExistAndDiffer`
  - Verifies core graphics review screenshots exist under `docs/devlog/screenshots/`.
  - Verifies each PNG decodes successfully.
  - Locks expected capture dimensions to `1920 x 1080`.
  - Verifies artifacts are not trivially small.
  - Verifies current/proposed pairs differ at the byte level for both veil and post-process review captures.

- `GraphicsFoundationAssetTests.Stage4MainSceneReviewScreenshotsExistAndDiffer`
  - Applies the same decode / size / current-vs-proposed difference guard to the main-scene soft-grade review PNGs.

- `GraphicsFoundationAssetTests.Stage4PortalSandboxReviewScreenshotsExist`
  - Verifies the E1 portal front / side / back review screenshots still exist and decode at `1280 x 720`.

Guarded artifacts:

- `stage4_graphics_baseline_veil_before.png`
- `stage4_graphics_baseline_veil_proposed.png`
- `stage4_graphics_baseline_veil_review_sheet.png`
- `stage4_graphics_postprocess_current.png`
- `stage4_graphics_postprocess_proposed_soft.png`
- `stage4_graphics_postprocess_review_sheet.png`
- `stage4_main_scene_graphics_current.png`
- `stage4_main_scene_graphics_proposed_soft.png`
- `stage4_main_scene_graphics_review_sheet.png`
- `e1_portal_front.png`
- `e1_portal_side.png`
- `e1_portal_back.png`

## Verification

- Targeted EditMode
  - Command: `-runTests -testPlatform EditMode -testFilter Anemora.Tests.EditMode.GraphicsFoundationAssetTests`
  - Result: `8/8` passed
  - Checked patterns: `error CS` `0`, shader error `0`, shader warning `0`, exception `0`, assertion `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, TextMesh Pro Essential Resources `0`, YAML `0`

- Full EditMode
  - Result: `48/48` passed
  - Source scan: EditMode `47`, PlayMode `34`
  - Checked patterns: `error CS` `0`, shader error `0`, shader warning `0`, exception `0`, assertion `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, TextMesh Pro Essential Resources `0`, YAML `0`

## Caveats

- This is objective artifact QA, not a visual taste decision.
- Human review is still required for final color, frame readability, and HD-2D feel.
