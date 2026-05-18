# 2026-05-18 Fast VS transition / aperture player / depth fix

## User-visible issues

- Interior/exterior map switching was instant and felt like a hard cut.
- After crossing the Time Window, Niro did not appear in the past-side aperture view.
- The ground inside the Time Window still appeared slightly lower than the current ground.

## Cycle

- Planned the split:
  - portal aperture player rendering to worker
  - map transition feel and aperture depth alignment locally
- Reviewed and integrated the worker patch.
- Added validation/build/smoke checks after integration.

## Changes

- Added a short runtime fade for house interior/exterior door transitions.
  - Validation/review trigger methods still switch immediately so editor checks remain deterministic.
  - Player movement is paused while the fade is active.
- Updated aperture portal camera culling.
  - `current -> other` portal camera includes the player layer only when Niro is actually in other time.
  - `other -> current` portal camera includes the player layer only when Niro is in current time.
  - The paired current/past roots remain isolated; this does not make the full past root visible outside the aperture.
- Raised Time Window `groundClearance` to keep the window bottom above the visible floor top.
- Reduced `aperturePlaneOffset` to `0.001` to further reduce parallax between the window quad and the generated portal plane.
- Added validation for aperture player-layer culling, visible door transition fade settings, and non-embedded aperture bottom placement.

## Validation

- `AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch` passes.
- Build output:
  - `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Runtime smoke log did not report `NullReference`, `Exception`, or `Error`.

## Notes

- The remaining perceived ground mismatch was most likely a combination of the window bottom being a little embedded below the visible floor top and the aperture quad being offset forward from the portal plane.
- Unity still emits non-fatal `System.Numerics` messages from the Code Coverage package during build; the player build succeeds.
