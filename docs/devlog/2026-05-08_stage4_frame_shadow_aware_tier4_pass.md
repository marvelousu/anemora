# Stage 4 frame shadow-aware Tier 4 pass

Date: 2026-05-08
Scope: Tier 4 graphics push / portal frame lighting integration
Branch: `codex/stage4-graphics-foundation-20260508`

## Summary

Updated the dedicated time-volume frame shader so frame bars participate in the URP main-light shadow pipeline instead of only using the main light direction. This makes the time-window frame respond more coherently to the production soft-shadow and light tuning passes.

## Changes

- `Assets/Art/Materials/Portal/TimeVolumeFrame.shader`
  - Adds URP main light shadow variants.
  - Passes shadow coordinates from vertex to fragment.
  - Uses `GetMainLight(shadowCoord)` and `mainLight.shadowAttenuation`.
  - Adds `_ShadowSoftnessFloor` to keep the stylized frame readable under soft shadow.

- `Assets/Art/Materials/Portal/Debug_Current.mat`
  - Sets `_ShadowSoftnessFloor` to `0.74`.

- `Assets/Art/Materials/Portal/Debug_Past.mat`
  - Sets `_ShadowSoftnessFloor` to `0.78`.

- `GraphicsFoundationAssetTests.TimeVolumeFrameMaterialsUseDedicatedReadableShader`
  - Guards `_ShadowSoftnessFloor` presence and conservative range.

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
  - Output: `Builds/Stage4Smoke/2026-05-08-graphics-foundation-frame-shadow-aware/Anemora_Stage4_GraphicsFoundation_FrameShadowAware_Smoke.exe`
  - Build folder files: `192`
  - Build folder disk size: `131,793,004` bytes / `125.688 MiB`
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

- The shader remains intentionally stylized and lightweight; this is not a full PBR material conversion.
- `_ShadowSoftnessFloor` prevents the frame from becoming muddy while still making it sit inside the scene lighting.
