# Stage 4 inside-only debug rim polish

Date: 2026-05-08
Scope: GFX-1 Portal / stencil visual polish
Branch: `codex/stage4-graphics-foundation-20260508`

## Summary

Improved the sandbox/debug inside-only portal material readability with a restrained warm rim and explicit ambient/direct lighting controls. This targets `Sandbox_E1_Stencil` review clarity and does not change portal gameplay ordering or stencil bit allocation.

## Changes

- `Assets/Art/Materials/Portal/InsideOnly.shader`
  - Adds `_RimColor`, `_AmbientStrength`, `_DirectStrength`, `_RimStrength`, and `_RimPower`.
  - Blends the existing blue lit color toward a low-strength warm rim based on view angle.
  - Keeps opaque queue, stencil `Ref 8`, `Comp Equal`, `Pass Keep`, `ZWrite On`, and the `AnemoraPortalInside` LightMode pass.

- `Assets/Art/Materials/Portal/InsideOnly.mat`
  - Sets ambient/direct/rim defaults for a readable debug cube without pushing the look into bloom-heavy or high-contrast territory.

- `Assets/Tests/EditMode/GraphicsFoundationAssetTests.cs`
  - Adds `PortalInsideOnlyDebugMaterialUsesReadableRimLighting`.
  - Guards the shader name and conservative rim/lighting property ranges.

## Verification

- E1 screenshot refresh
  - Command: `-executeMethod Anemora.EditorTools.AnemoraE1ParallelSetup.CaptureE1Screenshots`
  - Result: Unity exit code `0`
  - Updated artifacts:
    - `docs/devlog/screenshots/e1_portal_front.png`
    - `docs/devlog/screenshots/e1_portal_back.png`
  - Unchanged artifact:
    - `docs/devlog/screenshots/e1_portal_side.png`
  - Output dimensions: `1280 x 720`
  - Stable SHA-256 hashes:
    - `e1_portal_front.png`: `6661E757A1AEDCC81D259EF6616F539177D5D15FBF8ED7DF4281532FCD0B7F37`
    - `e1_portal_side.png`: `984D406358A2D0174D3A52C4C13722240950EAB48577195D6D5F1F723D820F66`
    - `e1_portal_back.png`: `2AD2BE41A8776DFA746FD113AD19211D4AD77356D8A900BF10B765CCAF13C406`
  - Checked patterns: `error CS` `0`, shader error `0`, shader warning `0`, exception `0`, assertion `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, TextMesh Pro Essential Resources `0`

- Targeted EditMode
  - Command: `-runTests -testPlatform EditMode -testFilter Anemora.Tests.EditMode.GraphicsFoundationAssetTests`
  - Result: `4/4` passed
  - Checked patterns: `error CS` `0`, shader error `0`, shader warning `0`, exception `0`, assertion `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, TextMesh Pro Essential Resources `0`

- Targeted PlayMode
  - Command: `-runTests -testPlatform PlayMode -testFilter Anemora.Tests.PlayMode.PortalStencilFeatureSmokeTest`
  - Result: `1/1` passed
  - Checked patterns: `error CS` `0`, shader error `0`, shader warning `0`, exception `0`, assertion `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, TextMesh Pro Essential Resources `0`

- Full EditMode follow-up
  - Result: `44/44` passed
  - Checked patterns: `error CS` `0`, shader error `0`, shader warning `0`, exception `0`, assertion `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, TextMesh Pro Essential Resources `0`, YAML `0`

## User Decision Items

None. This pass is limited to the inside-only debug material and does not apply a production palette, camera, or lighting change.
