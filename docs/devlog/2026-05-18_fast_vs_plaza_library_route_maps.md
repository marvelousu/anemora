# 2026-05-18 Fast VS plaza / library route maps

## User-visible request

- Keep the current house slice behavior and extend the VS route to Central Plaza and Library.
- Switch from seamless exterior travel to explicit map changes, using glowing floor pads like the house interior pad.
- Add a small glow at Niro's house exterior door entry point.
- Keep the V24 same-coordinate current/past Time Window behavior.

## Cycle

- Planned the route extension and delegated the first implementation pass to a worker.
- Worker added the area enum expansion, plaza/library blockout maps, route pads, and route transitions.
- Local review then tightened the Time Window bounds, exterior door cue, new map boundary colliders, and validation.
- Ran Unity batch validation/build and a runtime smoke test.

## Changes

- `FastVsHouseArea` now covers:
  - `Interior`
  - `Exterior`
  - `CentralPlaza`
  - `Library`
- Current/past map visibility now isolates all four map sets, while keeping both time roots at the same world coordinate.
- Added separate Central Plaza and Library map roots under both current and past spaces.
- Added glowing floor-pad transitions:
  - house interior -> house exterior
  - house exterior -> house interior
  - house exterior -> central plaza
  - central plaza -> house exterior
  - central plaza -> library
  - library -> central plaza
- Added a small exterior door glow at Niro's house entry.
- Expanded the Time Window region from `42 x 42` to `78 x 58` so the library route area remains inside the V24 generation bounds.
- Added invisible boundary colliders around Central Plaza and Library to prevent walking off the new separate maps.

## Validation

- `AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch` passed.
- The validation now checks:
  - same-coordinate current/past roots
  - all four active-area isolation states
  - required map move glow pads
  - Central Plaza and Library required blockout objects
  - route transition targets and fade/hold timings
  - reachable trigger execution for every route pad
  - invisible boundary colliders for the new maps
  - Time Window region coverage for the library route area
- Player build completed:
  - `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Runtime smoke test ran the EXE for 18 seconds with no `error`, `exception`, `failed`, `crash`, or `NullReference` matches.

## Notes

- Graphics are still intentionally blockout-level. The map route and transition behavior are the priority for this pass.
- Unity batch validation under `-nographics` still emits non-fatal render texture warnings during aperture rendering checks. The validation and player build both complete successfully.
- Unity also emits non-fatal Code Coverage package `System.Numerics` resolution messages during build.
