# Stage 4 portal stencil feature guard polish

Date: 2026-05-08
Scope: GFX-1 Portal / stencil visual polish
Branch: `codex/stage4-graphics-foundation-20260508`

## Summary

This pass keeps the portal look and gameplay ordering unchanged while tightening the URP Renderer Feature defaults, pass ordering contract, and diagnostics surface.

## Changes

- `PortalStencilFeature`
  - Exposes the ADR-0002 compatible two-pass contract as `EnqueuedPassCount` and `InsidePassEvent`.
  - Reuses static mask / inside shader tag arrays instead of allocating them during `Create()`.
  - Adds a Stage 4 portal stencil Header / Tooltip block for the mask layer and inside-only layer fields.
  - Rebuilds passes from `OnValidate()` after serialized field edits.
  - Treats a null renderer as a defensive no-enqueue path and records diagnostics as `0` passes.

- `GraphicsFoundationAssetTests`
  - Adds `PortalStencilFeatureDefaultsRemainAdr0002Compatible`.
  - Locks the stencil bit `3`, stencil mask `8`, two-pass enqueue contract, immediate inside-pass ordering, and default layer masks `~0`.

## Verification

- Targeted EditMode
  - Command: `-runTests -testPlatform EditMode -testFilter Anemora.Tests.EditMode.GraphicsFoundationAssetTests`
  - Result: `2/2` passed

- Targeted PlayMode
  - Command: `-runTests -testPlatform PlayMode -testFilter Anemora.Tests.PlayMode.PortalStencilFeatureSmokeTest`
  - Result: `1/1` passed
  - Checked portal warning patterns: DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`
  - Log caveat: Unity licensing startup emitted existing handshake text containing `Error`; no game/runtime exception/assert was observed.

- Full EditMode
  - Result: `42/42` passed
  - Checked patterns: `error CS` `0`, shader error `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, TextMesh Pro Essential Resources `0`

## User Decision Items

None. This pass does not change art direction, camera, palette, production scene lighting, or portal gameplay ordering.
