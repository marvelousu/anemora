# Stage 4 portal flash profile asset

Date: 2026-05-08
Scope: GFX-1 Portal / stencil visual polish
Branch: `codex/stage4-graphics-foundation-20260508`

## Summary

Populated the assigned `PortalFlash_VolumeProfile.asset` with the same inspectable `ColorAdjustments` defaults that `PortalFlashPlayer` already enforces at runtime. This keeps visible behavior aligned with the existing runtime path while making the flash profile statically reviewable.

## Changes

- `Assets/Settings/Portal/PortalFlash_VolumeProfile.asset`
  - Adds one `ColorAdjustments` component.
  - Sets `postExposure = 2.5`.
  - Sets `colorFilter = white`.

- `Stage4GraphicsBaselineCapture`
  - Adds `CreateOrUpdatePortalFlashVolumeProfileAsset`.
  - Reuses the existing VolumeProfile component helper for portal flash profile maintenance.

- `GraphicsFoundationAssetTests`
  - Adds `PortalFlashProfileContainsInspectableRuntimeDefaults`.
  - Guards the assigned portal flash profile and its `ColorAdjustments.postExposure` value.

## Verification

- Portal flash profile executeMethod
  - Command: `-executeMethod Anemora.EditorTools.Stage4GraphicsBaselineCapture.CreateOrUpdatePortalFlashVolumeProfileAsset`
  - Result: Unity exit code `0`

- Targeted EditMode
  - Command: `-runTests -testPlatform EditMode -testFilter Anemora.Tests.EditMode.GraphicsFoundationAssetTests`
  - Result: `9/9` passed

- Targeted PlayMode
  - Command: `-runTests -testPlatform PlayMode -testFilter Anemora.Tests.PlayMode.MainSceneStartupLogTests`
  - Result: `3/3` passed

- Full EditMode
  - Result: `49/49` passed
  - Source scan: EditMode `48`, PlayMode `34`

- Windows build smoke
  - Output: `Builds/Stage4Smoke/2026-05-08-graphics-foundation-portal-flash-profile/Anemora_Stage4_GraphicsFoundation_PortalFlashProfile_Smoke.exe`
  - Build folder files: `192`
  - Build folder disk size: `131,763,668` bytes / `125.660 MiB`
  - Unity exit code: `0`
  - Build log marker: `Build Finished, Result: Success.`

- Player smoke
  - Ran generated Windows player for 30 seconds at `1280 x 720`, fullscreen off.
  - Process exit `-1` is expected because the smoke window stops the player.
  - Checked player-log patterns: Error `0`, Exception `0`, Assert `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, NullReference `0`, MissingReference `0`, Failed `0`, TextMesh Pro Essential Resources `0`.

Checked editor/test log patterns: `error CS` `0`, shader error `0`, shader warning `0`, exception `0`, assertion `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, TextMesh Pro Essential Resources `0`, NullReference `0`, MissingReference `0`, YAML `0`.

Build log caveats:

- `Exception` matches are package reflection lines from `com.unity.testtools.codecoverage` / `ReportGeneratorMerged.dll` resolving `System.Numerics` fields.
- `RenderGraph` / `DrawObjectsPass` matches are build size report asset paths, not runtime warnings.

## Caveats

- This does not change `PortalFlashPlayer.maxPostExposure`; it makes the assigned template asset match the existing runtime default.
- Final portal-flash strength and timing remain design-review items if a different feel is desired.
