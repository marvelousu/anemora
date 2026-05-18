# Stage 4 frame bevel line Tier 4 pass

Date: 2026-05-08
Scope: Tier 4 graphics push / time-window frame edge readability
Branch: `codex/stage4-graphics-foundation-20260508`

## Summary

Added a lightweight shader bevel-line illusion to the time-window frame bars. The pass uses object-space cube coordinates to brighten only areas where two axes approach a cube edge, improving edge readability without adding geometry, textures, or a heavier material model.

## Changes

- `Assets/Art/Materials/Portal/TimeVolumeFrame.shader`
  - Adds `_EdgeLineStrength`.
  - Computes a second-largest object-space coordinate mask to isolate cube-bar edge lines.
  - Adds a restrained rim-color edge lift after the existing lit / rim blend.

- `Assets/Art/Materials/Portal/Debug_Current.mat`
  - Sets `_EdgeLineStrength` to `0.08`.

- `Assets/Art/Materials/Portal/Debug_Past.mat`
  - Sets `_EdgeLineStrength` to `0.07`.

- `GraphicsFoundationAssetTests.TimeVolumeFrameMaterialsUseDedicatedReadableShader`
  - Guards `_EdgeLineStrength` presence and conservative range.

## Capture Artifacts

Updated by `Stage4GraphicsBaselineCapture.CaptureAll`:

- `docs/devlog/screenshots/stage4_graphics_baseline_veil_before.png`
- `docs/devlog/screenshots/stage4_graphics_baseline_veil_proposed.png`
- `docs/devlog/screenshots/stage4_graphics_baseline_veil_review_sheet.png`
- `docs/devlog/screenshots/stage4_graphics_postprocess_current.png`
- `docs/devlog/screenshots/stage4_graphics_postprocess_proposed_soft.png`
- `docs/devlog/screenshots/stage4_graphics_postprocess_review_sheet.png`

Stable SHA-256 hashes:

- `stage4_graphics_baseline_veil_before.png`: `B9991E6DBDD1821B73B2FDD68D36F263A4EA14CDAA78E1DE7876C8F9A21DA308`
- `stage4_graphics_baseline_veil_proposed.png`: `D8C303BDA269BD00E58267B490A8AB2030106B4C4304CE2653D01ACFEC2B9776`
- `stage4_graphics_baseline_veil_review_sheet.png`: `4D76402D7D6310CF367A0B8662B57E556DE38CD7549CC4A6FA05FC1A4D729DF1`
- `stage4_graphics_postprocess_current.png`: `93370C15B4788E1DE9A1414408A8D811C7860EF560B4C9270ECF6661A7E5C12B`
- `stage4_graphics_postprocess_proposed_soft.png`: `81775AEB0D61D345854A8682FBBCE838D8CAF59FAD0B4FC1702689C52E5C30F2`
- `stage4_graphics_postprocess_review_sheet.png`: `A002ECD9CF3F5D34D693A50D9AD557AF191C222078E7C79AE3A11918BC7A5ACA`

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
  - Output: `Builds/Stage4Smoke/2026-05-08-graphics-foundation-frame-bevel-line/Anemora_Stage4_GraphicsFoundation_FrameBevelLine_Smoke.exe`
  - Build folder files: `192`
  - Build folder disk size: `131,794,140` bytes / `125.689 MiB`
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

- The effect is procedural and does not add mesh density.
- Values are deliberately low so the frame reads more dimensional without becoming a bright outline.
