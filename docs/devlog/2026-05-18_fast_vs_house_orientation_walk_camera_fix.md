# 2026-05-18 Fast VS house orientation / walk / camera fix

## User-visible issues

- The player still could not reliably exit the house.
- The exterior house doorway read as if the building was facing away from the north-east road.
- Niro kept using idle animation while walking.
- A black strip appeared at the player's feet.
- Entering the Time Window made the camera jump to the past-side copy.

## Changes

- Moved the exterior house facade, porch, door trigger, door pad, and return target to the road-facing south side.
- Kept the north-east road direction intact and placed the post-exit target on the path side of the door.
- Added a door reachability review path that places the player on each door pad and verifies the same runtime trigger test fires.
- Changed the current/past roots to occupy the same world coordinate, matching the V24 same-coordinate behavior instead of a side-by-side review layout.
- Added active-time camera culling so the camera shows only the current layer or the past layer without moving sideways.
- Added active-time physics isolation so overlapping current/past roots do not double-collide.
- Added real Niro walk strips from the v45 source gallery and switched `FastVsDirectionalSpriteAnimator` so:
  - stopped uses idle materials,
  - movement uses walk materials,
  - frame count is read per texture strip.
- Removed the sprite ground-shadow quad that was reading as a black line under the feet.

## Validation

- `AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch` passes.
- Build output:
  - `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Runtime smoke log did not report `NullReference`, `Exception`, or `Error`.

## Notes

- Niro left/right assignments remain swapped intentionally because the imported side sprites read reversed in the current camera orientation.
- The Unity code coverage package still logs non-fatal `System.Numerics.*` resolution messages during build, but validation and player build both complete successfully.
