# Stage 4 Floor Surface Palette Tier 4 Pass

Date: 2026-05-08

## Scope

This pass pushes the production floor read a little closer to the HD-2D target without changing camera composition, gameplay collision, portal flip ordering, or story-facing layout.

The previous floor work added underlay separation and transparent breakup / chipped-detail overlays. The remaining issue was that the repeated floor tiles still read as a flat gray board. This pass normalizes the actual `Floor_Stone` and `Floor_Wood` surface material palette so the floor carries clearer warm stone, dark stone, moss, and wood separation before post-process is applied.

## Changes

- Updated `Stage4Zone1MaterialSetup` so `ApplyStage4FloorUnderlayMaterials` also normalizes extracted floor-surface materials.
- Retuned `Stage4_FloorUnderlay_Current.mat` and `Stage4_FloorUnderlay_Past.mat` to sit under the stronger surface palette.
- Normalized floor-surface materials under `Assets/Art/Materials/Zone1/FloorSurfaces/`:
  - warm stone: `0.47, 0.41, 0.30`
  - dark stone: `0.31, 0.30, 0.25`
  - moss stone: `0.23, 0.34, 0.22`
  - warm wood: `0.42, 0.25, 0.13`
- Kept the materials URP Lit, opaque, low smoothness, non-metallic, and with specular / environment reflections disabled.
- Extended `GraphicsFoundationAssetTests.MainSceneFloorUnderlayUsesDedicatedMaterials` to guard the floor-surface palette.

## Files

- `Assets/Editor/Stage4Zone1MaterialSetup.cs`
- `Assets/Art/Materials/Zone1/Stage4_FloorUnderlay_Current.mat`
- `Assets/Art/Materials/Zone1/Stage4_FloorUnderlay_Past.mat`
- `Assets/Art/Materials/Zone1/FloorSurfaces/`
- `Assets/Prefabs/Zone1/Floor_Stone.prefab`
- `Assets/Prefabs/Zone1/Floor_Wood.prefab`
- `Assets/Tests/EditMode/GraphicsFoundationAssetTests.cs`
- `docs/devlog/screenshots/stage4_main_scene_graphics_current.png`
- `docs/devlog/screenshots/stage4_main_scene_graphics_proposed_soft.png`
- `docs/devlog/screenshots/stage4_main_scene_graphics_review_sheet.png`

## Screenshots

- `docs/devlog/screenshots/stage4_main_scene_graphics_current.png`
  - SHA-256: `D42D79706E1185B04BD4A725725C7748A92E32FFC235693444F2D1020BDC2815`
- `docs/devlog/screenshots/stage4_main_scene_graphics_proposed_soft.png`
  - SHA-256: `17FC8F4CAC27CEFDA21E88DCDBC72F25176F899BB3A2370353CECDDDAE281606`
- `docs/devlog/screenshots/stage4_main_scene_graphics_review_sheet.png`
  - SHA-256: `B6DFC749A9DFBF65D547F0EA53F9C15AA49399895B7DCAADDECD1752F8B494A9`

## Verification

- `Anemora.EditorTools.Stage4Zone1MaterialSetup.ApplyStage4FloorUnderlayMaterials`
  - Exit code: `0`
  - Log caveat: Unity licensing handshake strings appear during batchmode startup.
- `Anemora.EditorTools.Stage4GraphicsBaselineCapture.CaptureMainSceneSoftGradeReview`
  - Exit code: `0`
  - Checked shader error / shader warning / RenderTexture.Create failed / DrawObjectsPass / RecordRenderGraph / RenderGraph / exception / missing reference / null reference matches: `0`
- `GraphicsFoundationAssetTests`
  - Targeted run: `21/21` passed
  - Result XML: `%TEMP%/AnemoraCodexLogs/20260508_gfx_foundation_targeted/graphics_foundation_tests_after_floor_surface_palette.xml`
- `MainSceneStartupLogTests`
  - Targeted run: `3/3` passed
  - Result XML: `%TEMP%/AnemoraCodexLogs/20260508_gfx_foundation_targeted/main_scene_startup_log_tests_after_floor_surface_palette.xml`

## Visual Assessment

The main scene now has more readable material separation in the floor and no longer relies only on post-process or transparent overlays for tonal breakup. It is still below a DQ3R-class result because the production camera, authored floor texture language, and building/facade density are still conservative. The next high-impact graphics work should keep moving real Chapter 1 Antela building density and facade layers into controlled production or sandbox review captures rather than adding only small shader constants.
