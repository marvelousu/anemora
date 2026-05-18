# 2026-05-18 Fast VS Time Window / door visual fix

## User-visible issues

- Entering the Time Window made the surroundings outside the window become past.
- The back side near the Time Window edge allowed a front-side pierce-through.
- The house interior front wall made the door area harder to read.
- The exterior-to-interior door entry was not reliable enough.
- Door transition visuals still looked like a warp or mosaic effect.

## Changes

- Kept the main camera on the current visual layer even after entering the Time Window.
- Kept the player visible on the current render layer while allowing the controller state to enter other time.
- Hid the secondary other-time portal visual so only the current-side aperture frames the past view.
- Broadened the current-side back blocker from the exact opening to the window edge area.
- Added validation that a current-side back-edge movement is rejected and does not pierce through the window.
- Removed the interior front lip walls and vertical door effects.
- Replaced interior/exterior transition pads with flat floor glow pads.
- Moved the exterior return trigger farther out onto the porch/path approach.
- Changed the exterior door to a normal closed wood panel plus handle cue, instead of a warp-like glowing doorway.

## Validation

- `AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch` passes.
- Build output:
  - `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Runtime smoke log did not report `NullReference`, `Exception`, or `Error`.

## Notes

- Past collision remains active while in other time, but the main camera continues to show current surroundings outside the aperture. This matches the current fast-VS goal: the Time Window reads as a window, not as a full-screen era switch.
