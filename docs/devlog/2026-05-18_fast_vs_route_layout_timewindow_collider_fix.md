# 2026-05-18 Fast VS route layout / Time Window collider fix

## User-visible request

- Move the house exterior -> plaza transition farther along the northeast road.
- Prevent immediate bounce-back after map transitions.
- Rework the plaza image toward a central plaza with roads extending southwest and southeast, and a large library building standing at the far side.
- Make the library interior broader and add story-relevant desk/book/shelf assets.
- Fix the Time Window bug where creating a window around current-side Niro could expose the window's collision volume in current space and trap him.

## Cycle

- Planned the fix and gave the map-layout portion to a worker.
- The worker did not complete in time, so the local pass implemented the route/layout changes directly.
- Reviewed the Time Window collision path and patched the controller-side collider state.
- Ran Unity validation/build and a runtime smoke test.

## Changes

- Moved `ExteriorToPlazaTriggerCenter` deeper onto the northeast road.
- Moved all route spawn targets away from their return pads and added validation to catch too-close route spawns.
- Reworked the plaza blockout:
  - southwest road back toward Niro's house exterior
  - southeast road out of the plaza
  - path from plaza center to a large library facade
  - larger centered library front with door, roof, wings, and window cues
- Expanded the library interior blockout:
  - wider floor and side/back wall hints
  - larger shelf rows
  - service desk
  - reading table
  - open book, index cards, scattered manuscript cues, and archive door hint
- Time Window generated other-time wall colliders now sync to the player time side:
  - disabled while Niro is in current time
  - enabled only after Niro enters other-time space

## Validation

- `AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch` passed.
- Validation now additionally checks:
  - required plaza southeast road and large library facade objects
  - required library reading table and book objects
  - route spawn clearance from return pads
  - other-time wall colliders are disabled while Niro is current-side
  - other-time wall colliders are enabled after Niro enters past-side space
- Player build completed:
  - `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Runtime smoke test ran the EXE for 18 seconds with no `error`, `exception`, `failed`, `crash`, or `NullReference` matches.

## Notes

- The graphics are still blockout-level, but the plaza/library composition now follows the intended route silhouette more closely.
- Unity batch validation under `-nographics` still emits non-fatal render texture warnings during aperture render checks. The validation and player build complete successfully.
- Unity also emits non-fatal Code Coverage package `System.Numerics` resolution messages during build.
