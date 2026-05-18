# Chapter 1 TimeWindow Frame FX And Publicfixes Recovery

Date: 2026-05-16
Recovered: 2026-05-19
Source notes:
- `notes/_handover/anemora-timewindow-v24-frame-and-spawn-effect-procedure-2026-05-16.md`
- `notes/_handover/anemora-chapter1-library-timewindow-blocker-fix3-procedure-2026-05-16.md`
- `notes/_handover/anemora-chapter1-publicfix4-library-timewindow-polish-procedure-2026-05-16.md`
- `notes/_handover/anemora-chapter1-publicfix5-library-book-state-and-portal-overlap-procedure-2026-05-16.md`

## Recovery note

No root-level `docs/devlog/2026-05-16_*.md` file was found during recovery. The notes repo,
however, preserved four procedure / verification handovers for a sequence of TimeWindow frame,
Library, book-state, and portal-overlap fixes. This file reconstructs that missing 2026-05-16
devlog from those primary notes.

## Frame and spawn-effect pass

The day began with a tightly scoped TimeWindow V24 frame/effect pass. The current positive
baseline already had:

- Genuine 1280x720 captures.
- Reto beat C history dialogue rendering correctly.
- State reaching `DWaiting` after dialogue.
- Prompt showing the next TimeWindow-use objective.
- Existing smoke `RESULT PASS`.

The pass was restricted to:

- Add a readable window frame.
- Add a visible generation/spawn effect.
- Preserve Reto chain, Library event state, TimeWindow area binding, Niro art stability, and
  portal collision behavior.

The source note records:

- Output build: `<temp>/anemora_ch1_playable_timewindow_framefx_20260516/Anemora_Chapter1.exe`
- Smoke evidence directory: `<temp>/anemora_ch1_timewindow_framefx_smoke_20260516_visible/`
- Built-player smoke: `RESULT PASS`
- Touched-file `git diff --check`: passed
- Report label: `[Claude -> orchestration] TimeWindow frame/effect pass ready`

## Publicfix3: Library / TimeWindow blockers

After user review, the next note treated the mismatch between the reported frame/effect build
and actual play experience as a possible stale-EXE / wrong-build issue first, then fixed Library
and TimeWindow blockers:

- Library repeat inspect felt frozen because the completed message could not be dismissed fast
  enough.
- The past book was hard to find.
- The frame/effect might not be present in the build the user had tested.
- The window side/front containment leaked enough that the player could exit through the side.

The procedure required:

- Verify the latest frame/effect EXE and evidence first.
- Make repeat-inspect dismissal ready within a very short delay.
- Add clear past-book cueing.
- Add near/front side containment around the frame.
- Keep the older behavior baseline passing.

The source note records:

- Output build: `<temp>/anemora_ch1_playable_publicfix3_20260516/Anemora_Chapter1.exe`
- Built-player smoke: `RESULT PASS`
- Evidence names including `library_repeat_inspect_ready.png`.
- Report label: `[Claude -> orchestration] Chapter1 publicfix3 Library TimeWindow blockers fixed`

## Publicfix4: Library / TimeWindow polish

The next pass narrowed to four remaining public-preview issues:

1. Return the repeat-inspect completed line recorded in the source note to normal manual
   `E/Space` close behavior rather than forced auto-close.
2. Fix Niro/player stacking after TimeWindow generation.
3. Make Reto/past-book location and cueing clearer.
4. Adjust the past side toward a more realistic tone while preserving a readable past/current
   difference.

The procedure explicitly prioritized Niro stack safety and normal log close behavior before
color polish.

The source note records:

- Output build: `<temp>/anemora_ch1_playable_publicfix4_20260516/Anemora_Chapter1.exe`
- Built-player smoke: `RESULT PASS`
- Evidence names including:
  - `tw_player_near_frame_commit_safe.png`
  - `tw_player_edge_commit_safe.png`
  - `tw_player_can_move_after_commit.png`
  - `library_repeat_inspect_manual_close_prompt.png`
  - `library_repeat_inspect_after_espace_close.png`
  - `library_past_book_cue_visible.png`
  - `library_past_book_prompt.png`
  - `library_past_book_found.png`
  - `tw_library_past_tone_realistic.png`
- Report label: `[Claude -> orchestration] Chapter1 publicfix4 Library/TimeWindow polish ready`

## Publicfix5: book state and portal overlap

The final 2026-05-16 procedure responded to user review that the publicfix4 book objective was
visible but the event state was broken:

- Creating a TimeWindow where Niro stood in the Library could push Niro/player outside the map.
- A past-book prompt recorded in the source note appeared, but the book could not be acquired
  and story progress did not advance.
- Book-related prompts could appear even when no TimeWindow was open.
- The book log was still too long.
- Talking to Reto and inspecting the book could show overlapping logs.

The intended repair model split the issue into two systems:

- Portal overlap / safe-position handling.
- Library past-book state management.

For public preview, the note preferred rejecting generation if the player overlapped the would-be
blocker/frame, rather than trying a risky relocation. It also called for simple Chapter 1 flags
instead of a complex inventory system.

The source note records:

- Output build: `<temp>/anemora_ch1_playable_publicfix5_20260516/Anemora_Chapter1.exe`
- Built-player smoke: `RESULT PASS`
- Evidence names including:
  - `tw_library_overlap_commit_rejected.png`
  - `tw_library_overlap_player_still_in_map.png`
  - `tw_library_safe_commit_after_step_away.png`
  - `library_past_book_before_interact.png`
  - `library_past_book_found_log.png`
  - `library_past_book_log_manual_close.png`
  - `library_past_book_after_progress.png`
- Report label: `[Claude -> orchestration] Chapter1 publicfix5 Library book state + portal overlap fixed`

## Boundaries and later preservation

Every 2026-05-16 source note retained these guardrails:

- Do not commit/push unless Tom explicitly asks.
- Do not rewrite Reto story text broadly.
- Do not replace V24 TimeWindow architecture.
- Do not re-enable old TimeFrame / TimeWindow / Diorama systems.
- Do not import new graphics or character assets.
- Keep the work in the Chapter 1 implementation worktree.

The later repository history contains `c03ee41 checkpoint: preserve chapter1 current state`,
which preserved a broad Chapter 1 state after these public-preview recovery passes. This devlog
records the missing day-level history behind that preserved state.
