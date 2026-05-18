# Stage 4 soft shadows Tier 4 pass

Date: 2026-05-08
Scope: Tier 4 graphics push / lighting and shadow polish
Branch: `codex/stage4-graphics-foundation-20260508`

## Summary

Enabled URP soft shadows as the first production-facing Tier 4 graphics push. This keeps render scale, MSAA, main light shadowmap resolution, camera setup, palette, and scene composition unchanged while replacing the hard-shadow pipeline baseline with soft-shadow support.

## Changes

- `Assets/Settings/UniversalRenderPipeline.asset`
  - Enables `m_SoftShadowsSupported`.
  - Unity 6000.3.14f1 also reserialized URP keyword prefilter fields when the asset was imported and built.

- `GraphicsFoundationAssetTests.UniversalRenderPipelineSupportsTier4SoftShadows`
  - Guards main light shadows.
  - Guards soft shadows enabled.
  - Guards main-light shadowmap resolution at `2048` or higher.
  - Guards render scale remains `1.0`.

## Capture Artifacts

Updated by `Stage4GraphicsBaselineCapture.CaptureMainSceneSoftGradeReview`:

- `docs/devlog/screenshots/stage4_main_scene_graphics_current.png`
- `docs/devlog/screenshots/stage4_main_scene_graphics_proposed_soft.png`
- `docs/devlog/screenshots/stage4_main_scene_graphics_review_sheet.png`

Stable SHA-256 hashes:

- `stage4_main_scene_graphics_current.png`: `E73FCBAAD29AB9AEA7F4E37C1A9BEF5038461C8C251E7C003622FC9762CE2EDD`
- `stage4_main_scene_graphics_proposed_soft.png`: `AF9108451CCF43F3867A75D636C5995D2D1F43DABCF5823AB594E5BD38486F29`
- `stage4_main_scene_graphics_review_sheet.png`: `9CAD0314DD9F21B58711B1A0611F6919689993C94EDC58348B37B8966B362976`

## Verification

- Targeted EditMode
  - Command: `-runTests -testPlatform EditMode -testFilter Anemora.Tests.EditMode.GraphicsFoundationAssetTests`
  - Result: `12/12` passed

- CaptureAll executeMethod
  - Command: `-executeMethod Anemora.EditorTools.Stage4GraphicsBaselineCapture.CaptureAll`
  - Result: Unity exit code `0`

- Main-scene capture executeMethod
  - Command: `-executeMethod Anemora.EditorTools.Stage4GraphicsBaselineCapture.CaptureMainSceneSoftGradeReview`
  - Result: Unity exit code `0`

- Full EditMode
  - Result: `52/52` passed
  - Source scan: EditMode `51`, PlayMode `34`

- Targeted PlayMode
  - Command: `-runTests -testPlatform PlayMode -testFilter Anemora.Tests.PlayMode.MainSceneStartupLogTests`
  - Result: `3/3` passed

- Windows build smoke
  - Output: `Builds/Stage4Smoke/2026-05-08-graphics-foundation-soft-shadows/Anemora_Stage4_GraphicsFoundation_SoftShadows_Smoke.exe`
  - Build folder files: `192`
  - Build folder disk size: `131,788,536` bytes / `125.683 MiB`
  - Unity exit code: `0`
  - Build log marker: `Build Finished, Result: Success.`

- Player smoke
  - Ran generated Windows player for 30 seconds at `1280 x 720`, fullscreen off.
  - Process exit `-1` is expected because the smoke window stops the player.
  - Checked player-log patterns: Error `0`, Exception `0`, Assert `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, NullReference `0`, MissingReference `0`, Failed `0`, TextMesh Pro Essential Resources `0`.

Checked editor/test/capture log patterns: `error CS` `0`, shader error `0`, shader warning `0`, exception `0`, assertion `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, TextMesh Pro Essential Resources `0`, NullReference `0`, MissingReference `0`, YAML `0`.

Build log caveats:

- `Exception` matches are package reflection lines from `com.unity.testtools.codecoverage` / `ReportGeneratorMerged.dll` resolving `System.Numerics` fields.
- `RenderGraph` / `DrawObjectsPass` matches are build size report asset paths, not runtime warnings.

## Tier 4 Notes

- This is a production-facing Tier 4 step, not just a review artifact.
- It improves shadow softness without changing camera direction, map layout, story, character art, or final palette decisions.
- A longer FPS / memory profiling run should be refreshed after more Tier 4 lighting and scene-density changes accumulate.
