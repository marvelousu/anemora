# Fast VS marker, glow, bookshelf, and plaza-window revert

Date: 2026-05-18

## User Checklist

- Make the red square marker flat: no frame, no mosaic texture.
- Stop the red floor glow from appearing/disappearing visually.
- Change Reto-event text from `...筆が...?` to `筆が...!`.
- Treat bookshelf texture as a full book-row image/panel pasted across the shelf face.
- Revert the plaza map library windows to the previous version.

## Worker Cycle

- Text worker (`gpt-5.4-mini`, Kepler) handled the small dialogue edit in `FastVsStoryFlowController.cs`.
- Parent session reviewed the worker result, made the scene/material changes in `AnemoraFastVsHouseSliceSetup.cs`, regenerated the scene/player, captured review images, and ran a player smoke check.

## Changes

- `<repo>/Assets/Scripts/FastVS/FastVsStoryFlowController.cs`
  - Updated Niro's Reto-event brush line to `(筆が...!)`.
- `<repo>/Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Red floor cues now use the round glow primitive without the map-move pulse component, so the current-side cue stays visually stable.
  - The target-book red cube marker is now a fixed flat red marker and validation rejects retained `_BaseMap` / `_MainTex` textures.
  - `FlatMaterial` now clears retained texture slots, preventing old procedural textures from staying on reused material assets.
  - Past library back-wall shelves now include three book-row texture panels in front of the shelf structure.
  - Past plaza library windows are restored to the previous half-lit pattern: upper-left/lower-right lit, upper-right/lower-left framed.
  - Validation was updated for the fixed red marker, stable red floor cue, book-row panels, and reverted plaza-window pattern.

## Validation

- Build and structure validation:
  - `<repo>/Logs/fast_vs_build_validate_20260518_marker_glow_bookshelf_revert.log`
  - Result: build succeeded and batchmode exited with return code 0.
- Screenshot capture:
  - `<repo>/Logs/fast_vs_capture_review_20260518_marker_glow_bookshelf_revert.log`
  - Output directory: `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518`
- Player smoke:
  - `<repo>/Logs/fast_vs_player_smoke_20260518_marker_glow_bookshelf_revert.log`
  - No matching runtime exception patterns were found during the short launch check.

## Review Images

- Past library shelves and red marker:
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/05_library_past_no_temp_people.png`
- Current plaza facade:
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/07_plaza_library_facade_current.png`
- Past plaza facade:
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/08_plaza_library_facade_past.png`
