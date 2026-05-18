# Stage 4 soft shadow light tuning Tier 4 pass

Date: 2026-05-08
Scope: Tier 4 graphics push / main-scene shadow softness
Branch: `codex/stage4-graphics-foundation-20260508`

## Summary

Tuned the production main scene directional light so the previously enabled URP soft-shadow support has visible effect in the main scene. This keeps the light direction, camera setup, scene layout, render scale, and production soft-grade Volume unchanged.

## Changes

- `Assets/Scenes/Anemora_Main.unity`
  - Sets the main `Directional Light` shadow strength from `1` to `0.92`.
  - Sets the main `Directional Light` shadow angle from `0` to `1.2`.

- `GraphicsFoundationAssetTests.MainSceneDirectionalLightUsesTier4SoftShadowTuning`
  - Guards the main scene directional light soft-shadow tuning.

## Capture Artifacts

Updated by `Stage4GraphicsBaselineCapture.CaptureMainSceneSoftGradeReview`:

- `docs/devlog/screenshots/stage4_main_scene_graphics_current.png`
- `docs/devlog/screenshots/stage4_main_scene_graphics_proposed_soft.png`
- `docs/devlog/screenshots/stage4_main_scene_graphics_review_sheet.png`

Stable SHA-256 hashes:

- `stage4_main_scene_graphics_current.png`: `2FBF2A80E9E1085DD0B98C5EBC30456017B75201CEA1F67BAC5D93DEFE1FDB45`
- `stage4_main_scene_graphics_proposed_soft.png`: `619CBC97C8229692F19B2602932D8363493372AA04D588A692A62E018139028B`
- `stage4_main_scene_graphics_review_sheet.png`: `215467B8F672437D5A04D3D7CEFB1F5E7AD841CCF60210BE4D53F0F31C8381EE`

## Verification

- Main-scene capture executeMethod
  - Command: `-executeMethod Anemora.EditorTools.Stage4GraphicsBaselineCapture.CaptureMainSceneSoftGradeReview`
  - Result: Unity exit code `0`

- Targeted EditMode
  - Command: `-runTests -testPlatform EditMode -testFilter Anemora.Tests.EditMode.GraphicsFoundationAssetTests`
  - Result: `14/14` passed

- Full EditMode
  - Result: `54/54` passed
  - Source scan: EditMode `53`, PlayMode `34`

- Targeted PlayMode
  - Command: `-runTests -testPlatform PlayMode -testFilter Anemora.Tests.PlayMode.MainSceneStartupLogTests`
  - Result: `3/3` passed

- Windows build smoke
  - Output: `Builds/Stage4Smoke/2026-05-08-graphics-foundation-soft-shadow-light-tuning/Anemora_Stage4_GraphicsFoundation_SoftShadowLightTuning_Smoke.exe`
  - Build folder files: `192`
  - Build folder disk size: `131,789,564` bytes / `125.684 MiB`
  - Unity exit code: `0`
  - Build log marker: `Build Finished, Result: Success.`

- Player smoke
  - Ran generated Windows player for 30 seconds at `1280 x 720`, fullscreen off.
  - Process exit `-1` is expected because the smoke window stops the player.
  - Checked player-log patterns: Error `0`, Exception `0`, Assert `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, NullReference `0`, MissingReference `0`, Failed `0`, TextMesh Pro Essential Resources `0`.

Checked editor/test/capture log patterns: `error CS` `0`, shader error `0`, shader warning `0`, exception `0`, assertion `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, TextMesh Pro Essential Resources `0`, NullReference `0`, MissingReference `0`, YAML `0`.

Build log caveats:

- `Exception` matches are package path / reflection lines, not runtime exceptions.
- `RenderGraph` / `DrawObjectsPass` matches are build size report asset paths, not runtime warnings.

## Tier 4 Notes

- This is a small production-facing tuning pass intended to make the active soft-shadow pipeline visible without changing the established camera or zone composition.
- Shadow strength remains high enough to preserve readability and grounding, while the angle removes the hard technical edge from the baseline light.
