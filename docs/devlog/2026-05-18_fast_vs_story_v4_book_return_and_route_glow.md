# 2026-05-18 Fast VS story v4 book return and route glow

## Scope

- Target scene: `Assets/Scenes/Anemora_FastVS_HouseSlice.unity`
- Target build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- User correction: remove the unapproved `(なんとなく、重い)` line and confirm the Reto story one beat at a time.

## Cycle

- Main session prepared the implementation plan and constraints.
- `gpt-5.4-mini` worker `Nash` implemented the first script-side pass for the Reto book-return flow.
- Main session reviewed the worker patch, corrected the opening wake handling, scene wiring, validation expectations, and map presentation.
- Unity batch validation/build was run after fixes.

## Story Decisions

- Opening bed line is now:
  - `夢を見ていたような、夢を見ていなかったような。`
- Removed from runtime:
  - `(なんとなく、重い)`
  - `(...からっぽ)`
- Kept / changed runtime beats:
  - `(...誰も)` is kept.
  - `(...あの子)` was replaced by `(...人)`.
  - Past-side objective is now taking/finding the book.
  - Returning to the current library no longer resumes automatically.
  - The player must talk to Reto again to show the book.
  - After the Reto return event completes, the returned book appears on the current-side desk.

## Reto Flow

- First Reto interaction still covers the library history and Timewriter activation.
- Past-library observation now includes:
  - `(...ここに、本が)`
  - `(...本を、見つけた)`
  - `(...人)`
  - return-to-current guide
- Current-library return sequence now includes:
  - `(...本を、レトに見せる)`
  - `...?`
  - a short pause
  - `...本物だ`
  - a longer pause with Reto looking up
  - `...そうですか。`
  - `...あなたのような方が、来てくれるとは。`
  - Mia hint
  - VS clear

## Map / Presentation

- Moved the starting point nearer to Niro's bed.
- Replaced flat square transfer pads with round, pulsing glow discs.
- Raised transfer glows above nearby floor/prop surfaces so they are less likely to be hidden by tiles.
- Added a small exterior door-entry glow at Niro's house.
- Moved Reto behind the writing desk, with the table placed in front of him.
- Added wider back-wall bookshelves and extra library tables to make the library read more like an interior archive.
- The TMP dialogue presenter was left in the scene as a disabled experiment.
- Runtime story text now falls back to the earlier OnGUI panel because it is currently more readable and closer to the preferred presentation quality.

## Validation

Unity batch validation passed:

- `Fast VS house slice validation passed.`
- Log: `%TEMP%\anemora_fastvs_v4_story_build_validate5_20260518.log`

Player build succeeded:

- `Build Finished, Result: Success.`
- `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`

Runtime smoke:

- 18-second `-batchmode -nographics` launch produced no `error|exception|failed|crash|NullReference` hits.
- Log: `%TEMP%\anemora_fastvs_v4_story_smoke_20260518.log`

Review screenshots were refreshed:

- `docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/01_interior_niro_shadow.png`
- `docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/03_library_reto_desk.png`
- `docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/06_library_dialogue_tmp_font.png`

Note: the screenshot capture path uses direct camera rendering, so the final OnGUI dialogue panel is not included in those PNGs.

Known non-fatal batch warnings:

- Unity licensing warning during batchmode startup.
- Existing Code Coverage `System.Numerics.Vector*` resolution warnings during player build.
