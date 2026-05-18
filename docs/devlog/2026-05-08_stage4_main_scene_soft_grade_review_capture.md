# Stage 4 main scene soft-grade review capture

Date: 2026-05-08
Scope: GFX-2 Stage 4 visual baseline profile
Branch: `codex/stage4-graphics-foundation-20260508`

## Summary

Added a main-scene current/proposed review capture path for the soft-grade profile. The capture opens `Anemora_Main` temporarily, renders the current camera view, renders a proposed soft-grade preview, builds a side-by-side sheet, and returns to the previously open scene without saving production scene changes.

## Changes

- `Stage4GraphicsBaselineCapture.CaptureMainSceneSoftGradeReview`
  - Opens `Assets/Scenes/Anemora_Main.unity` only for capture.
  - Uses the active Main Camera when available.
  - Writes current, proposed, and review sheet PNGs at `1920 x 1080`.
  - Restores camera `renderPostProcessing`, camera target texture, and camera volume layer mask after capture.

- `Stage4GraphicsBaselineCapture`
  - Uses `UniversalRenderPipeline.SingleCameraRequest` for single-camera offscreen capture.
  - Applies a screenshot-only CPU soft-grade preview to proposed PNGs after `ReadPixels`.
  - The CPU preview approximates the review profile values: small exposure lift, mild contrast / saturation, warm filter, and soft vignette.

## Capture Artifacts

- `docs/devlog/screenshots/stage4_main_scene_graphics_current.png`
- `docs/devlog/screenshots/stage4_main_scene_graphics_proposed_soft.png`
- `docs/devlog/screenshots/stage4_main_scene_graphics_review_sheet.png`

Stable SHA-256 hashes:

- `stage4_main_scene_graphics_current.png`: `A876C5F124692D3772D4A8FC4E580A9D696DB96AF541CACD6407D7A316227D4A`
- `stage4_main_scene_graphics_proposed_soft.png`: `512FF06C571CB34E35B2E4CFDF788176AC4080C8D842AA8835F7AB21316C2CA0`
- `stage4_main_scene_graphics_review_sheet.png`: `A99C6BFA4C8CEF5ABE455AD0B63B18655DD1F8DCEF98C150753EA04090C35711`

Sample average luma check:

- Current: `76.78`
- Proposed soft: `69.67`
- Review sheet: `64.89`

## Verification

- Main scene capture executeMethod
  - Command: `-executeMethod Anemora.EditorTools.Stage4GraphicsBaselineCapture.CaptureMainSceneSoftGradeReview`
  - Result: Unity exit code `0`
  - Checked patterns: `error CS` `0`, shader error `0`, shader warning `0`, exception `0`, assertion `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, TextMesh Pro Essential Resources `0`

- Baseline capture executeMethod
  - Command: `-executeMethod Anemora.EditorTools.Stage4GraphicsBaselineCapture.CaptureAll`
  - Result: Unity exit code `0`
  - Checked patterns: `error CS` `0`, shader error `0`, shader warning `0`, exception `0`, assertion `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, TextMesh Pro Essential Resources `0`

## Caveats

- The proposed PNG is a review preview, not a runtime screenshot of a production-applied Volume.
- Stronger grade, bloom, final palette, and production scene application remain approval-gated.
