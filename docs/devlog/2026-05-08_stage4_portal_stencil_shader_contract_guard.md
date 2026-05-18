# Stage 4 portal stencil shader contract guard

Date: 2026-05-08
Scope: GFX-1 Portal / stencil visual polish
Branch: `codex/stage4-graphics-foundation-20260508`

## Summary

Added an EditMode guard for the ADR-0002 portal stencil shader contract. This keeps the defense-in-depth dual-pass shader design explicit while avoiding any runtime visual or scene changes.

## Changes

- `GraphicsFoundationAssetTests.PortalStencilShadersKeepAdr0002DualPassContract`
  - Verifies `PortalMask.shader` keeps both `UniversalForward` and `AnemoraPortalMask` LightMode passes.
  - Verifies `InsideOnly.shader` keeps both `UniversalForward` and `AnemoraPortalInside` LightMode passes.
  - Verifies both shaders keep stencil `Ref 8`, `ReadMask 8`, and `WriteMask 8`.
  - Verifies mask shader keeps `Comp Always` / `Pass Replace`.
  - Verifies inside-only shader keeps `Comp Equal` / `Pass Keep`.

## Verification

- Targeted EditMode
  - Command: `-runTests -testPlatform EditMode -testFilter Anemora.Tests.EditMode.GraphicsFoundationAssetTests`
  - Result: `10/10` passed

- Full EditMode
  - Result: `50/50` passed
  - Source scan: EditMode `49`, PlayMode `34`

Checked log patterns: `error CS` `0`, shader error `0`, shader warning `0`, exception `0`, assertion `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, TextMesh Pro Essential Resources `0`, NullReference `0`, MissingReference `0`, YAML `0`.

## Caveats

- No shader code path or material value changed in this pass.
- The guard intentionally preserves the dual-pass design; removing `UniversalForward` or the custom LightMode pass would need an ADR-aligned decision.
