# Stage 4 veil weave depth Tier 4 pass

Date: 2026-05-08
Scope: Tier 4 graphics push / time-window veil material richness
Branch: `codex/stage4-graphics-foundation-20260508`

## Summary

Added lightweight visual structure to the time-window veil so it reads less like a flat transparent plane and more like layered HD-2D temporal glass. The pass keeps the existing palette, opacity envelope, render queue, and camera direction intact.

## Changes

- `Assets/Art/Materials/Portal/TimeVolumeVeil.shader`
  - Adds `_WeaveScale` and `_WeaveStrength` for a subtle crossing wave pattern.
  - Adds `_DepthGlowStrength` for a restrained center-depth glow.
  - Keeps the shader transparent, untextured, and lightweight.

- `Assets/Art/Materials/Portal/TimeVolume_SpaceVeil.mat`
  - Sets `_WeaveScale` to `8`.
  - Sets `_WeaveStrength` to `0.08`.
  - Sets `_DepthGlowStrength` to `0.12`.

- `GraphicsFoundationAssetTests.TimeVolumeSpaceVeilUsesStage4VeilShader`
  - Guards the new weave and depth-glow material properties.

## Capture Artifacts

Updated by `Stage4GraphicsBaselineCapture.CaptureAll`:

- `docs/devlog/screenshots/stage4_graphics_baseline_veil_review_sheet.png`
- `docs/devlog/screenshots/stage4_graphics_postprocess_current.png`
- `docs/devlog/screenshots/stage4_graphics_postprocess_proposed_soft.png`
- `docs/devlog/screenshots/stage4_graphics_postprocess_review_sheet.png`

Stable SHA-256 hashes:

- `stage4_graphics_baseline_veil_review_sheet.png`: `BC63C425358072D5DD7991169CA41465E75FAEBF0A130F03F4EC48C0485B8863`
- `stage4_graphics_postprocess_current.png`: `A6617F712FF6D52E107A88AAA3592D69F64E2E93234DDD0CF380645BF0CFADE7`
- `stage4_graphics_postprocess_proposed_soft.png`: `82FB3378AC90B32FED23D57D9D687A6465A8F16EF9FE87110A48E3002CD29E19`
- `stage4_graphics_postprocess_review_sheet.png`: `7184A475C19A9B2FCE867A81A063D897EA957A9B726BD94B247FE246BE5C83C7`

## Verification

- CaptureAll executeMethod
  - Command: `-executeMethod Anemora.EditorTools.Stage4GraphicsBaselineCapture.CaptureAll`
  - Result: Unity exit code `0`

- Targeted EditMode
  - Command: `-runTests -testPlatform EditMode -testFilter Anemora.Tests.EditMode.GraphicsFoundationAssetTests`
  - Result: `14/14` passed

- Full EditMode
  - Result: `54/54` passed

- Targeted PlayMode
  - Command: `-runTests -testPlatform PlayMode -testFilter Anemora.Tests.PlayMode.MainSceneStartupLogTests`
  - Result: `3/3` passed

- Windows build smoke
  - Output: `Builds/Stage4Smoke/2026-05-08-graphics-foundation-veil-weave-depth/Anemora_Stage4_GraphicsFoundation_VeilWeaveDepth_Smoke.exe`
  - Build folder files: `193`
  - Build folder disk size: `131,987,329` bytes / `125.873 MiB`
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

- This is still a shader-only polish pass; it does not require new texture assets.
- The material values are intentionally conservative to preserve readability of characters and interactables through the veil.
