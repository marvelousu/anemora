# Stage 4 time-window prefab material guard

Date: 2026-05-08
Scope: GFX-1 / GFX-2 portal VFX readability guard
Branch: `codex/stage4-graphics-foundation-20260508`

## Summary

Added an EditMode guard that the runtime local time-window prefab is wired to the polished frame and veil materials. This keeps the dedicated frame shader from drifting into an unused review-only asset.

## Changes

- `GraphicsFoundationAssetTests.TimeWindowDioramaPrefabUsesFrameAndVeilMaterials`
  - Loads `Assets/Prefabs/Portal/TimeWindow_Diorama.prefab`.
  - Verifies all 12 frame-bar renderers use `Debug_Current.mat`, which now resolves to `Anemora/Portal/TimeVolumeFrame`.
  - Verifies the serialized `spaceVeilMaterial` reference points to `TimeVolume_SpaceVeil.mat`.

## Verification

- Targeted EditMode
  - Command: `-runTests -testPlatform EditMode -testFilter Anemora.Tests.EditMode.GraphicsFoundationAssetTests`
  - Result: `11/11` passed

- Full EditMode
  - Result: `51/51` passed
  - Source scan: EditMode `50`, PlayMode `34`

Checked log patterns: `error CS` `0`, shader error `0`, shader warning `0`, exception `0`, assertion `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, TextMesh Pro Essential Resources `0`, NullReference `0`, MissingReference `0`, YAML `0`.

## Caveats

- No prefab or material value changed in this pass.
- This guard verifies wiring, not final art direction.
