# 2026-05-18 Fast VS UI Cue / Library / Home Polish

## Scope

This pass addressed the latest review items for the Fast VS V24 sample:

- Hide the lower-left persistent guide while dialogue / guide / brush / question UI is active.
- Remove the non-player objective text for checking the brush around the house-exit beat.
- Replace current-side Time Window guidance cubes with red round floor glows.
- Keep book / person markers as cube-style markers but add motion.
- Restore current-library ruin dressing, reduce bright white book-page blocks, lengthen past-library tables, and add past-library door / window texture panels.
- Rework Niro house bed and table books into more readable blockout props.

## Worker Cycle

Plan -> gpt-5.4-mini worker instructions -> parent review / integration -> validation was followed.

- UI worker: `019e38c5-2a5e-7a91-b77e-3409d424ef23`
  - Edited `<repo>/Assets/Scripts/FastVS/FastVsStoryRuntimeHud.cs`
  - Edited `<repo>/Assets/Scripts/FastVS/FastVsStoryFlowController.cs`
- Scene worker: `019e38c6-6a17-7c53-9f1e-6e3fa86b3de3`
  - Edited `<repo>/Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
- Parent integration then fixed:
  - residual `"筆を確かめる。"` objective text
  - a `void` method `return true;` compile break
  - bookshelf texture-panel direction
  - current-library ruin props
  - past-library door / window texture validation
  - book-page material brightness on house, current-library, and past-library table books

## Changed Files

- `<repo>/Assets/Scripts/FastVS/FastVsStoryRuntimeHud.cs`
- `<repo>/Assets/Scripts/FastVS/FastVsStoryFlowController.cs`
- `<repo>/Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
- `<repo>/docs/devlog/2026-05-18_fast_vs_ui_cue_library_home_polish.md`
- `<repo>/docs/devlog/INDEX.md`

## Validation

- Build and structural validation passed.
  - `<repo>/Logs/fast_vs_build_validate_20260518_ui_cue_library_home_polish.log`
  - Built player: `<repo>/Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Review screenshot capture passed.
  - `<repo>/Logs/fast_vs_capture_review_20260518_ui_cue_library_home_polish.log`
  - Screenshot folder: `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518`
- Headless player smoke passed with no exception matches.
  - `<repo>/Logs/fast_vs_player_smoke_20260518_ui_cue_library_home_polish.log`

## Visual Review Notes

- `01_interior_niro_shadow.png`: Niro house bed is now a multi-part prop with blanket, rails, headboard, footboard, and a readable book material pass.
- `03_library_reto_desk.png`: current library has restored derelict dressing and no extra side text marker for book handoff.
- `05_library_past_no_temp_people.png`: past tables are longer and ordered, back-wall books are texture panels with protruding dividers, and the previous bright white floating block is no longer visible in this capture.

## Residual Risk

- The screenshot capture method does not currently show an active dialogue log box, so the "persistent guide hidden during dialogue" change is covered by runtime HUD logic and compile / smoke validation, not by a dialogue screenshot.
- The red cube marker over the past target book is intentionally still a cube marker, now animated by `FastVsMapMoveGlowPulse`. Its exact visual taste should be checked in manual play.
