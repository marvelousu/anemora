# 2026-05-18 Fast VS close guard / route source / layout cleanup

## User-visible request

- Do not allow the Time Window to be closed while Niro is in past-side space.
- Shorten the northeast road on Niro's house exterior map.
- Remove mysterious transition triggers in the central plaza and along the northeast road.
- Make the small house-door map-move glow visible.
- Give the plaza more depth and make the library exterior broader.
- Ensure map transitions have visible glow pads.
- Remove unexplained mosaic-like rods and the black box in the library interior.

## Changes

- `TimeWindowPairedSpacePortalController.ClosePortal()` now rejects close attempts while `playerInOtherTime` is true.
- Added validation that trying to close in past-side space keeps the portal open and marks the close as rejected.
- Added `sourceArea` to `FastVsAreaDoorTransition`.
  - A transition now fires only when its own source map is active.
  - This prevents exterior route triggers from firing at overlapping same-coordinate plaza positions.
- Shortened the house exterior northeast road and moved the exterior -> plaza glow pad to the road end.
- Reduced the exterior door glow pad size and added a distinct small top glow at Niro's house door.
- Moved the plaza -> library glow pad to the library entrance instead of the plaza center.
- Deepened the plaza map and moved the library facade farther back.
- Broadened the library facade and adjusted road/path widths so the scene reads less like a single central trigger.
- Replaced unexplained rod-like cues:
  - house interior red rod -> small book cue on the table
  - library red route bar -> small table trace/book cue
  - library black archive-door box -> wooden archive shelf hint

## Validation

- `AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch` passed.
- Validation now checks:
  - portal close is rejected while Niro is in past-side space
  - every route transition has the expected source area and target area
  - inactive route triggers do not fire from another active map
  - removed mosaic/black-box cue objects are absent
  - door and route glow pads still exist
- Player build completed:
  - `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Runtime smoke test ran the EXE for 18 seconds with no `error`, `exception`, `failed`, `crash`, or `NullReference` matches.

## Notes

- The same-coordinate V24 model is preserved. The fix is not coordinate separation; it is source-area gating on transition triggers.
- Unity batch validation under `-nographics` still emits non-fatal render texture warnings during aperture render checks. Validation and player build complete successfully.
