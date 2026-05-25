# Chapter 1 map content refine cycle51-52

Date: 2026-05-26
Branch: work/chapter1-continuation-map-vs-20260524

## Scope

- Targeted the D1-D3 street-corner continuation map.
- Kept map scale, route trigger positions, and the VS area unchanged.
- Focused on content placement and map readability: stall remnants, D3 turn, D2 node apron, and current-vs-past ruin contrast.

## Changes

- Added `CreateStreetCornerCycle51StallRemnantReadabilityDetails(...)` and wired it into the street-corner market detail flow.
- Current D1:
  - added broken cloth, awning gaps, fallen posts, loose planks, stone feet, weeds, and cross planks to make the northern stall row read as abandoned remnants instead of intact booths,
  - kept the four-slot stall rhythm from the reference while varying collapse details.
- Past D1:
  - added clean cloth folds, posts, crates, and small merchant items so the same row still reads as functional stalls.
- Added `CreateStreetCornerCycle52RuinTurnReviewFixes(...)` after subagent review.
- D3:
  - widened the diagonal road merge with continuous path bands from the lower road toward the northeast turn.
- D2:
  - added a small path apron around the plaza-edge move point so it reads as a plaza boundary node rather than a house/object interaction point.
- Current D1/D2 ruins:
  - added dust, dark gaps, broken wall caps, rubble, and roof-cap breaks to interrupt overly intact house silhouettes,
  - left the past houses intact.

## Review

- Parent visual review compared:
  - `docs/devlog/screenshots/chapter1_all_maps_cycle05/07_d1_d3_current.png`
  - `docs/devlog/screenshots/chapter1_all_maps_cycle05/08_d1_d3_past.png`
  - street-corner reference diagrams.
- Subagent review flagged:
  - current ruins still reading too much like houses,
  - D3 turn being too pinched,
  - D2 move point competing with nearby clutter.
- Follow-up from review: cycle52 widened D3, added D2 apron, and added current-only ruin-break overlays before final validation.

## Validation

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
- Unity `ValidateChapter1AllMapsBatch`
  - log: `Logs/chapter1_cycle52_validate.log`
  - result: passed
- Unity `CaptureChapter1AllMapsCycle05ScreenshotsBatch`
  - log: `Logs/chapter1_cycle52_capture.log`
  - result: passed
- Unity `BuildAndValidateBatch`
  - log: `Logs/chapter1_cycle52_build.log`
  - result: passed
  - note: log included a nonfatal Bee caching-client warning, with Unity exit code 0.
- Player smoke
  - log: `Logs/chapter1_cycle52_player_smoke.log`
  - result: no fatal matches

## Review Assets

- `docs/review/2026-05-26T06-10/01_d1_d3_current.png`
- `docs/review/2026-05-26T06-10/02_d1_d3_past.png`
