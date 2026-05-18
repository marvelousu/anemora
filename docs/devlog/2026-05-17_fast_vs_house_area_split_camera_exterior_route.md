# 2026-05-17 Fast VS house area split / camera / exterior route

## Purpose

User review found three issues in the house slice:

- Entering the Time Window still made the player appear to rotate by 90 degrees.
- The interior and exterior still read as one continuous map.
- The exterior house and route needed to move toward the planned Chapter 1 map flow.

## Changes

- Changed the camera guide so it follows the active V24 root:
  - current root while the player is in current time,
  - past root while the player is in other-time space.
- Made the camera snap immediately when the active time root or active house area changes, instead of lerping across the distant current/past roots.
- Changed paper billboards to face the camera forward direction rather than the camera world position, avoiding sideways rotation caused by the physical gap between paired roots.
- Added validation that the camera anchor resolves to the past root after a current-to-past transfer.
- Split the house into active map sets:
  - `Current/Past_HouseInteriorMap_SeparateSpace`
  - `Current/Past_HouseExteriorMap_SeparateSpace`
- Added `FastVsHouseAreaVisibility` so only the active map set is visible/collidable.
- Updated door transitions so entering the interior door activates the exterior map set, and entering the exterior door activates the interior map set.
- Removed the continuous 42m shared ground from the generated slice so the two areas no longer read as one physical map.
- Improved the exterior house with:
  - foundation line,
  - side/back wall depth,
  - layered roof slopes/eaves,
  - chimney,
  - door frame,
  - porch deck and lower step.
- Extended a collidable north-east road from Niro's house exterior toward the future central plaza route.

## Validation added

- Fails if the broad physical route or full continuous ground appears in the scene.
- Fails if the active interior/exterior map sets are not isolated.
- Fails if the north-east road approach is missing.
- Fails if door transitions do not switch to the intended active map set.
- Fails if the camera guide still anchors to the current root after moving the player to past time.

## Coordinate note

This still preserves the canonical centers:

- `HouseInteriorCenter = (-8.35, 0, -8.35)`
- `HouseExteriorCenter = (8.20, 0, 8.20)`

The road now points toward the later `CentralPlazaVsCenter = (20.80, 0, 15.80)`, but the plaza itself is not built in this pass.
