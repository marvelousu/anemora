# URP PortalStencilFeature RenderObjectsPass Migration

Date: 2026-05-06

## Summary

Migrated `PortalStencilFeature` away from URP internal `UnityEngine.Rendering.Universal.Internal.DrawObjectsPass` and onto the public URP `RenderObjectsPass`.

This keeps the same two custom LightMode passes:

- `AnemoraPortalMask`
- `AnemoraPortalInside`

The public runtime API used by portal flip ordering is unchanged:

- `PortalStencilFeature.SetLayerMasks(...)`

## Changed Files

- `Assets/Scripts/TimeManagement/Portal/PortalStencilFeature.cs`
- `Assets/Tests/PlayMode/PortalStencilFeatureSmokeTest.cs`

## Verification

Automated tests:

- EditMode: `32/32` passed.
- PlayMode: `29/29` passed.

Build and runtime:

- Windows Standalone build: success.
- Build path: `C:\Users\maro6\Documents\Unity\Anemora-stage4-phase0\Builds\Stage4Phase0URP\Anemora_Stage4_URP.exe`.
- 30 second player run log: `stage4_phase0_player_30s.log`.
- `DrawObjectsPass does not have an implementation of the RecordRenderGraph method` count: `0`.
- No `Exception`, `Error`, `Warning`, `DrawObjectsPass`, or `RecordRenderGraph` lines were found in the 30 second player log.

## Test Coverage Update

`PortalStencilFeatureSmokeTest.SandboxSceneEnqueuesPortalStencilPasses` now counts the exact legacy RenderGraph warning string while rendering the Sandbox E1 stencil scene and asserts that it stays at `0`.

## Caveats

The migration keeps the current opaque render queue assumption because `PortalMask.shader` and `InsideOnly.shader` both declare `Queue = Geometry` and `RenderType = Opaque`.

Manual visual review of portal pixels in the running build is still useful, but the existing portal smoke, boundary, demo playable, EditMode, PlayMode, build, and player-log checks all passed after the migration.
