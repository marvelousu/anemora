# 2026-05-18 Fast VS Story Reto Clear and Visual Review Fix

## Request

- The previous Niro shading pass was not visually acceptable; it looked like a thin dark rectangle around the sprite.
- Continue feature work by implementing the VS story path through Reto's story clear.
- Opening starts from waking at the bed; full wake animation will be added later.
- When leaving the house, Niro should stop, show a `?` overhead, and show a brush image floating in the center.
- Reto should be placed at the desk and use this rough state flow:
  - writing / hand raised
  - talk starts: hand lowers
  - idle / looks up
  - after conversation: hand raises and returns to writing

## Cycle

- Main session re-read the canonical references:
  - `2026-05-09_chapter1_scene1_v3_final.md`
  - `2026-05-12_chapter1_vs_story_canon_inventory.md`
  - `chapter1_vs_story_timewindow_recovery_instruction_20260514.md`
- A `gpt-5.4-mini` worker was given a narrow script-only task, but it did not return in time for this review cycle and was shut down.
- Main session implemented the runtime story controller, Reto state animator, scene wiring, validation, and visual screenshot capture directly.
- Main session visually reviewed generated screenshots and corrected a bad temporary Reto head card before rebuilding.

## Visual Fix

- Removed the full-card `FastVS_PlayerSpriteShadingOverlay_Niro` scene object.
- Replaced it with baked per-pixel sprite shading generated from the source Niro textures.
- Kept only the separate soft contact shadow under Niro.
- Validation now rejects the old overlay object and checks that Niro sprite materials use generated shaded textures.

Screenshots written to:

- `docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/01_interior_niro_shadow.png`
- `docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/02_exterior_niro_shadow.png`
- `docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/03_library_reto_desk.png`

Visual review result:

- Niro no longer has a visible dark rectangle around the sprite.
- Reto's first temporary head overlay produced a large beige square and was removed.
- Reto now uses the existing Resident_B sprite plus separate arm / pen pose parts.

## Story Runtime

Added:

- `Assets/Scripts/FastVS/FastVsStoryFlowController.cs`
- `Assets/Scripts/FastVS/FastVsRetoWritingAnimator.cs`

Scene wiring now creates:

- `FastVS_Chapter1_StoryFlow_RetoClear`
- `FastVS_Reto_WritingAtDesk`

Implemented beats:

- Opening hint: Niro wakes near the bed and should leave the house.
- First exterior exit beat:
  - movement locks
  - `?` appears over Niro
  - a brush image floats in the center
  - advancing releases movement
- Reto event:
  - [1.B] Reto initial encounter
  - [1.C] Library history and empty shelf thought
  - [1.D] Timewriter reaction / tutorial prompt
  - [1.E] Past library observation prompt
  - [1.F] Return to present, no book appears
  - [1.G] Mia house hint
  - VS clear flag

The story text is intentionally lightweight `OnGUI` text for this fast branch. It preserves the canonical beat order and can be replaced later by the formal dialogue/localization stack.

## Validation

Added validation for:

- Reto exists at the library desk.
- Reto has arm and pen pose parts.
- Door Timewriter beat freezes and releases player movement.
- Reto event starts at `[1.B]`.
- Story advances through `[1.B]-[1.G]` and reaches `vs.clear`.
- Reto returns to raised writing pose after the event.
- Niro shading is baked into sprite textures rather than a full-card overlay.

## Verification

- `git diff --check` passed for the edited files.
- Unity review screenshots captured successfully.
- Unity batch validation passed:
  - `Fast VS house slice validation passed.`
- Player build succeeded:
  - `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- 18-second `-batchmode -nographics` smoke run produced no error, exception, crash, or `NullReference` log entries.

Known non-fatal batch warnings:

- Unity licensing access-token update warning.
- Existing Code Coverage `System.Numerics.Vector*` resolution warnings.
- `RenderTexture.Create failed` warnings can still appear under `-nographics` validation runs.
