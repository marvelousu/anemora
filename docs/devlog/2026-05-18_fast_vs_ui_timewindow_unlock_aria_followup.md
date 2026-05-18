# 2026-05-18 Fast VS UI / Time Window Unlock / Aria Follow-up

## Context

User review found several Fast VS follow-up issues:

- Runtime dialogue text looked dark, as if the dialogue backing was in front of the text.
- Niro appeared visually buried while the opening dialogue box was visible.
- Time Window creation was available from startup, but it should unlock only during the Reto event.
- Aria was too small and had a floating square above her.
- The guide cue should show where to open the Time Window in the current library, not attach floor lights to past-side characters.
- Reto's lowering-arms animation looped during conversation instead of settling into lowered idle.
- Past book interaction could appear to freeze with no visible feedback.
- The upper-left debug overlay blocked the objective/log display.
- The library entrance glow flickered.

## Worker Cycle

Followed the requested cycle for this pass:

1. Parent session scoped the bug list and split off a focused gpt-5.4-mini worker task.
2. Worker owned `Assets/Scripts/FastVS/FastVsRetoWritingAnimator.cs`.
3. Parent reviewed the worker output and added one integration correction so `LookingUp` can return to lowered dialogue idle.
4. Parent integrated HUD, Time Window unlock, Aria, current-side cue, and validation changes.
5. A second gpt-5.4-mini worker performed read-only review across the touched files; no blocking inconsistency was reported.

## Changes

- `FastVsStoryRuntimeHud`
  - Moved the HUD to high-order `ScreenSpaceOverlay`.
  - Ensured text RectTransforms are later siblings than the panel backing.
  - Reduced the dialogue panel height/opacity so the bottom UI reads less like it is covering Niro's feet.

- `TimeWindowPairedSpacePortalController`
  - Added `runtimeInputEnabled`.
  - Startup input is disabled.
  - Drag-open is allowed only after the story unlocks it.
  - Close input is still allowed when a portal exists, preserving the existing "cannot close while in past" guard.

- `FastVsStoryFlowController`
  - Unlocks Time Window input only after the Reto activation sequence reaches the past-observation wait.
  - Shows `Current_Library_TimeWindowOpenCue_Book` only during the "open a window here" phase.
  - Hides that cue once the past-book event starts.
  - Added Aria interaction during past observation, returning to the same player-controlled observation state after the monologue.
  - Lengthened the requested pauses around the pocket/brush reaction and Reto's book-realization beats.
  - Keeps the book interaction manual through E / Space.

- `FastVsRetoWritingAnimator`
  - Keeps normal writing as `WritingRaised`.
  - Plays `Lowering` once at conversation start.
  - Holds `DialogueIdle` on the final lower-arms frame.
  - Uses `LookingUp` only for the specified reaction beats.
  - Returns from `LookingUp` to lowered idle for later normal dialogue.

- `AnemoraFastVsHouseSliceSetup`
  - Removed past-side floor guide glows and floating cue cubes for Aria/book.
  - Added current-side Time Window opening cue at the book coordinate.
  - Moved the book glow onto the book object.
  - Enlarged Aria to match the current character scale better.
  - Disabled the top-left debug overlay by serialized default.
  - Raised route glow pads slightly to avoid visual z-fighting/flicker.
  - Added validation for startup Time Window lock, event unlock, current cue visibility, Aria interaction, and removed guide-cube names.

## Verification

- Build and validation:
  - `Logs/fast_vs_build_validate_20260518_followup_ui_timewindow_aria.log`
  - Result: `Fast VS house slice validation passed.`
  - Result: `Fast VS house slice player built: Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`

- Review screenshots:
  - `docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/`
  - The camera-based capture does not include the new overlay HUD, but it confirms the past-side Aria square and past-side floor guide glows were removed.

- Standalone smoke:
  - `Logs/fast_vs_player_smoke_20260518_followup_ui_timewindow_aria.log`
  - Searched for `error|exception|failed|crash|NullReference|MissingReference`.
  - Result: no matches.

## Notes

- The smoke log still emits TextMeshPro default-font warnings when TMP text objects are created before the assigned Japanese font is applied. No runtime error was produced.
- The screenshot capture path should be revisited later if we want HUD-inclusive captures for `ScreenSpaceOverlay`; the current camera render path intentionally captures only world-space review framing.
