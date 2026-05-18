# Fast VS facade, red cue, and text fix

Date: 2026-05-18

## User Checklist

- Restore the current-side plaza library door; it had become black even though it was fine before.
- Make the red floor cue look like the map-move glow with only the color changed.
- Give the red cube marker a visible texture/pattern.
- Change Niro's house-exit line from `...` to `...筆?`.
- Change the Reto-event `...筆` line to `...筆が...?`.
- Fix past plaza library window panes that looked untextured.
- Replace the black bar on the current dry fountain with wood-like debris.
- Improve the bookshelf/book texture presentation.

## Worker Cycle

- Text worker (`gpt-5.4-mini`) updated `FastVsStoryFlowController.cs`.
- Scene worker (`gpt-5.4-mini`) was assigned the scene/material work but did not return within the working window and was closed.
- Parent session implemented the scene/material fixes, added validation, rebuilt the scene/player, captured screenshots, and ran a player smoke check.

## Changes

- `<repo>/Assets/Scripts/FastVS/FastVsStoryFlowController.cs`
  - `(...筆)` -> `(...筆が...?)`
  - `(...)` in the house-exit brush beat -> `(...筆?)`
- `<repo>/Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Current plaza library door panels now use current wood/fence materials, not `DoorwayDark`.
  - Past plaza library window panes all use `WindowLight`.
  - Red floor cue uses the same flat glow-material style as the map-move pads.
  - Past target-book red cube marker now uses a patterned `red_marker` material.
  - Current dry fountain now has wood planks and a small crack detail instead of a large black bar.
  - The `Book` pixel pattern was made less flat for shelf/bookshelf presentation.
  - Validation now catches the black-current-door regression, missing past window pane material, dry-fountain black-bar regression, and red marker material regression.

## Validation

- Build and structure validation:
  - `<repo>/Logs/fast_vs_build_validate_20260518_facade_redcue_text_fix.log`
  - Result: no error patterns found.
- Screenshot capture:
  - `<repo>/Logs/fast_vs_capture_review_20260518_facade_redcue_text_fix.log`
  - Output directory: `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518`
- Player smoke:
  - `<repo>/Logs/fast_vs_player_smoke_20260518_facade_redcue_text_fix.log`
  - No matching runtime exception patterns were found.

## Review Images

- Current plaza facade:
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/07_plaza_library_facade_current.png`
- Past plaza facade:
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/08_plaza_library_facade_past.png`
- Past library shelves and red marker:
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/05_library_past_no_temp_people.png`
