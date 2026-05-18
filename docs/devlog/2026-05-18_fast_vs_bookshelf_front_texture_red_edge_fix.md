# Fast VS bookshelf front texture and red-edge fix

Date: 2026-05-18

## User Checklist

- Make the red floor light move like the map-transition floor lights.
- Make the red cube frame cover every edge, not only the visible face.
- Make the red cube frame thinner.
- Replace the broken bookshelf approach with a front-facing bookshelf image panel, as a flat front-facing blocky bookshelf face.
- Apply the bookshelf front texture to the side bookshelves as well.

## Worker Cycle

- Read-only worker (`gpt-5.4-mini`, Hilbert) reviewed the relevant functions and validation/screenshot targets.
- Parent session implemented the changes in the scene generator, rebuilt the scene/player, captured screenshots, and ran a player smoke check.

## Changes

- `<repo>/Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - `CreateFloorGlowCue` now uses the same `FastVsMapMoveGlowPulse` behavior as the route/map-transition glow pads.
  - `CreateRedCubeMarkerWithOutline` now creates all 12 cube edges with thinner black bars.
  - Added a generated `bookshelf_front` material/texture that draws a flat front-facing bookshelf: wooden frame/shelves plus horizontal rows of upright book spines.
  - Back-wall past bookshelf panels now use `bookshelf_front` only; the prior individual shelf book runs were removed from the wall.
  - Side past bookshelves now place the bookshelf texture panels on the visible front face.
  - Validation now requires the red floor cue pulse, all 12 red-cube edge bars, thin frame bars, and `bookshelf_front` on the back/side shelf panels.

## Validation

- Build and structure validation:
  - `<repo>/Logs/fast_vs_build_validate_20260518_bookshelf_front_red_edges_v2.log`
  - Result: build succeeded and batchmode exited with return code 0.
- Screenshot capture:
  - `<repo>/Logs/fast_vs_capture_review_20260518_bookshelf_front_red_edges_v2.log`
  - Output directory: `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518`
- Player smoke:
  - `<repo>/Logs/fast_vs_player_smoke_20260518_bookshelf_front_red_edges.log`
  - No matching runtime exception patterns were found during the short launch check.

## Review Images

- Past library bookshelf panels and red markers:
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/05_library_past_no_temp_people.png`
