# 2026-05-05 E0 URP Pipeline Asset

## Summary
- Created the Phase E0 URP setup path for Stage 3E:
  - `Assets/Settings/UniversalRenderPipeline.asset`
  - `Assets/Settings/UniversalRenderPipeline_Renderer.asset`
  - GraphicsSettings default render pipeline assignment
  - QualitySettings render pipeline assignment for all quality levels
  - `PortalStencilFeature` skeleton attached to the Forward+ renderer data
- Moved generated URP global settings assets under `Assets/Settings/` to keep rendering settings in one place.

## Stencil Bit Check
- URP 17.3.0 reserves `StencilUsage.UserMask = 0b00001111`, so only bits 0-3 are user-reserved.
- URP 17.3.0 uses bit 4 as `StencilLight = 0b00010000`.
- URP 17.3.0 uses bits 5-6 as `MaterialMask = 0b01100000`; bit 7 is reserved.
- Result: Stage 3E must not use stencil bit 4. `PortalStencilFeature` reserves bit 3 (`StencilMask = 0b00001000`) for portal masks.

## Notes For E1
- E1 should implement the portal mask and inside-portal draw passes inside `PortalStencilFeature`.
- ADR-0002 and `docs/STAGE3_E_PLAN.md` now record bit 3 as the selected portal stencil bit.
