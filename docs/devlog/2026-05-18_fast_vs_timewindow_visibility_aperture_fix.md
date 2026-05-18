# 2026-05-18 Fast VS Time Window visibility / aperture fix

## User-visible issues

- Niro disappeared after crossing the Time Window.
- The scene could start with the past-side brightness visible until a Time Window was created.
- A stray past-side Niro sprite was visible near the right edge of the past house when opening an exterior window.
- The window aperture appeared slightly lower than the current ground.
- Current/past props crossing the generated window plane could be visible inside the aperture and create unstable overlap cases.

## Production cycle note

- Restored the requested cycle for this pass:
  - plan
  - worker implementation pass
  - local review and integration
  - validation/build
  - devlog

## Changes

- Added startup render-layer initialization for the current root, past root, and player before any Time Window exists.
- Kept Niro on a neutral visible player layer instead of switching him onto the hidden past/current space layers.
- Updated the visual guide camera mask to always show current surroundings, portal frame, and player, while keeping the past root hidden outside the aperture.
- Removed the past-side Niro review sprite from the house slice and added validation that it is not regenerated.
- Reapplied the portal frame layer after root layer refreshes so the generated portal objects do not drift back onto map layers.
- Added aperture-overlap suppression: non-floor renderers intersecting the generated window rectangle are temporarily disabled on both current and past roots while the window is open, then restored on close/rebuild.
- Reduced the aperture quad offset from `0.024` to `0.006` to minimize the perceived ground-height/parallax mismatch at the window plane.

## Validation

- `AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch` passes.
- Build output:
  - `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Runtime smoke log did not report `NullReference`, `Exception`, or `Error`.

## Notes

- The slight ground drop was most likely aperture-plane offset/parallax, not a current/past floor coordinate mismatch or camera desync. Current and past floor objects still share the same coordinate contract; this pass reduces the visible offset and keeps the camera anchored to the current coordinate root.
- Unity still logs non-fatal `System.Numerics` resolution messages from the Code Coverage package during build. The build result is successful.
