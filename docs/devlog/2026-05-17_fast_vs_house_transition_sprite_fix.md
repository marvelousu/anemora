# 2026-05-17 Fast VS house transition / sprite fix

## Purpose

User review found two visible failures in the fast VS house slice:

- Niro used a multi-frame review strip as if it were one front-facing sprite.
- The house door was only represented by visual path pieces; it did not move the player.

This pass fixes both and adds validation so the same visible failures fail the batch build.

## Changes

- Replaced the active Niro sprite path with the v12 single-frame front asset:
  - `Assets/Art/Characters/FastVS/Niro/hero_niro_front_v12_64x96_review_only.png`
- Added v12 single-frame back / left / right assets for Niro player direction changes.
- Added `FastVsDirectionalSpriteAnimator`, which switches Niro's material between front/back/left/right based on WASD / arrow direction and local movement.
- Added validation that all four active Niro direction textures are exactly `64x96`, keep NPOT scaling disabled, and do not point to v45 / idle strip names.
- Added validation that the directional animator can switch all current and past direction materials.
- Removed the broad physical path that visually connected the house interior island to the house exterior island.
- Added door transition pads for both current and past map roots.
- Added `FastVsAreaDoorTransition`, which polls the player's local coordinate in the active V24 space and warps to the paired house door target.
- Added `TimeWindowPairedSpacePortalController.WarpPlayerToLocalForReview` so door movement preserves the active current/past state and V24 local-coordinate contract.
- Added validation for:
  - no `BroadInteriorExteriorRoute` objects in the generated scene,
  - required door pads exist,
  - both actual door transition components exist,
  - door trigger / target local coordinates match the intended values,
  - door warp works in both current and past time states.

## Coordinate note

This slice keeps the canonical Chapter 1 coordinate centers:

- `HouseInteriorCenter = (-8.35, 0, -8.35)`
- `HouseExteriorCenter = (8.20, 0, 8.20)`

The central plaza is not built in this slice. It should later be placed at `CentralPlazaVsCenter = (20.80, 0, 15.80)` so the house exterior is not treated as the plaza.

## Next review focus

- Open the rebuilt EXE and confirm walking into the glowing door pads switches between the house interior and exterior.
- Confirm Niro appears as a single front-facing pixel sprite, not a strip.
- Confirm the house interior and exterior read as separate map islands rather than a continuous hallway.
