# Time Frame Prototype Display Surface Polish

Date: 2026-05-08

## Summary

Polished the accepted Time Frame prototype direction as a thin world-space display surface. The change keeps the fixed-camera basis, planar 3D placement, drag-created rectangle, Q/E era switching, click-through interaction, and ESC close behavior intact.

This does not turn the window into a thick gate, screen-space UI, player-facing billboard, or physical opening in the current map.

## Changes

- `Assets/Art/Materials/Prototype/TimeFramePortalPlane.shader`
  - Added restrained controls for glass tint, inner shade, surface opacity, soft edge, rim, vignette, distortion, and scanline.
  - Added subtle reflection bands, edge grain, corner accents, and inner hairline controls so the plane reads as a thin world-space display surface without becoming a thick gate.
  - Empty portal-camera background now stays nearly transparent instead of filling the dragged rectangle as a dark panel.
  - Raised the restrained default surface-read values for rim, reflection, corner accent, inner hairline, and frame alpha so the accepted planar window reads more clearly in DQ3R-style dense scenes.
- `Assets/Scripts/Prototypes/TimeFrame/TimeFramePrototypeWorldWindow.cs`
  - Applies the new material controls from serialized runtime fields.
  - Applies the new reflection / edge-grain / corner / inner-line controls from serialized runtime fields.
  - Clears the portal camera with alpha `0` so rendered era geometry dominates the window while empty space keeps only a very thin surface read.
  - Allocates the portal RenderTexture from the source camera pixel size when available.
  - Uses edit-mode safe cleanup for generated runtime materials, meshes, colliders, and RenderTextures so review capture logs do not emit `Destroy may not be called from edit mode`.
- `Assets/Editor/Prototypes/TimeFramePrototypeSceneSetup.cs`
  - Added `Anemora/Prototypes/Capture Time Frame Window Review`.
  - Batch entry point: `Anemora.EditorTools.Prototypes.TimeFramePrototypeSceneSetup.CapturePrototypeWindowReview`.
  - Captures Past, Current, and a side-by-side review sheet at 1920 x 1080 per panel.
  - Keeps the generated `TimeFramePortalPlane.mat` and scene instance values aligned with the stronger restrained display-surface defaults.
- `Assets/Tests/EditMode/Prototypes/TimeFramePrototypePolygonUtilityTests.cs`
  - Added shader/material contract checks for the restrained display-surface defaults.
  - Added a source guard for edit-mode safe generated-object cleanup.

## Captures

- `docs/devlog/screenshots/timeframe_prototype_window_past.png`
- `docs/devlog/screenshots/timeframe_prototype_window_current.png`
- `docs/devlog/screenshots/timeframe_prototype_window_review_sheet.png`

Latest review sheet SHA256:

- `FBB308AEBF8CEE76DE5352E2257F1C16C45434EBBDF4AF3C2501272C5D399EEE`

Latest individual SHA256:

- Current: `BF9579272A5E0CDE2460B177E28559D4980B06F27FA964EC94AE4211F8BB0452`
- Past: `E9D1B5933F2C53B0057301F997F0F54E59334FBC2F7EBDA52F4716CEF6BB840C`

## Verification

- Prototype capture:
  - Unity exit code: `0`
  - Capture outputs refreshed.
  - `Destroy may not be called from edit mode` matches: `0`
  - Shader error / exception / RenderGraph / DrawObjectsPass matches: `0`
  - Unity licensing / primary socket startup messages appeared before successful capture; these are editor startup noise.
- Targeted EditMode:
  - `TimeFramePrototypePolygonUtilityTests`: `6/6 passed` after the stronger surface-read defaults and edit-mode cleanup source guard.
  - Result XML: `%TEMP%/AnemoraCodexLogs/20260508_gfx_foundation_timeframe/timeframe_polygon_tests_after_destroy_cleanup_guard.xml`
- Graphics foundation guard:
  - `GraphicsFoundationAssetTests`: `19/19 passed` before this focused Time Frame refresh; not rerun in this final cleanup pass.

## Caveats

- This is still prototype-scene visual polish. DQ3R-class quality needs the generated building / prop / character source art to land, followed by scene-level material and lighting integration.
- The border is intentionally quiet. Strong bloom, chromatic aberration, heavy refraction, or a thick ornate frame would conflict with the accepted flat world-window direction unless explicitly approved.
- Current / Past tint values are placeholders for readability, not final era color direction.
