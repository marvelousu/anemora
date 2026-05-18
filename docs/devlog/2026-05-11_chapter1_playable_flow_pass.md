# Chapter 1 Playable Flow Pass - 2026-05-11

## Summary

Playable-first priority is now active. This pass stops using capture/validator artifacts as the milestone and makes the Chapter 1 review build behave more like a playable slice.

Implemented:
- Added `Chapter1PlayableFlowController`.
- Added a small in-game objective overlay for the current Chapter 1 route.
- Gated `ChapterTransitionController.requiredRawFlag` with `progression.chapter1.route_ready_for_close`.
- Scene setup now wires `Chapter1_PlayableFlow` and assigns the route-ready gate to the chapter close trigger.
- Chapter close no longer starts just because the player walks into S5 before core route actions are reflected.

## Current Playable State

Currently playable at component/smoke level:
- Player movement and section camera anchors.
- v3.2 Time Window local/range flow: draw/place a restrained floor/range window, enter, walk, interact, close/return, reflect current-side trace.
- Scene 1 book action: `take_book_001` reflected to current family book trace.
- Scene 2 sightline reveal and no-fade route guidance.
- Scene 3/4 proximity dialogue source route, using non-freezing text where configured.
- Scene 4 spice action: `touch_spice_jar_001` reflected through the trace manifest.
- Scene 5 side-view chapter close, title, save, and same-scene restore.

## Blocking Game Feel Before This Pass

The main route-causality issue was that S5 could close the chapter with an empty `requiredRawFlag`. A player could walk to the north ruins and trigger the chapter close without doing the Chapter 1 Time Window interactions.

This pass fixes that by setting the chapter close gate to `progression.chapter1.route_ready_for_close`. That flag is set only after the reflected book action and reflected spice trace action are both present.

## Known Gaps

- The objective overlay is VS scaffolding, not final UI art.
- Chapter 2 scene loading is intentionally disabled; the save/title handoff completes and returns to the same scene.
- Character sprites are placeholders until user-approved import.
- Normal gameplay camera remains isometric strict; only the chapter-end cinematic uses side view.
- Route polish still needs hands-on playtesting for collision feel, path readability, and whether the hints are enough without overexplaining.

## Next 3 Fixes

1. Add a small route-block feedback check around the S5 close trigger if playtest shows the current objective text is too easy to miss.
2. Run a live/manual route smoke against the new EXE and adjust player start, camera anchors, and blocker colliders only where they block the route.
3. Tighten Time Window first-use feedback if the user cannot reliably discover F / Shift-drag / Esc and E interactions.

## User-Checkable Build

- EXE: `<temp>\anemora_ch1_playable_flow_20260511_005434\Anemora_Chapter1.exe`
- Build log: `<temp>\anemora_ch1_playable_flow_build_20260511_005434.log`
- Build result: succeeded, warnings=0, errors=0.

## Smoke Checklist

- Launch the EXE.
- Confirm objective text appears in the upper-left.
- Walk to S5 early; chapter close should not start before route actions are complete.
- Use Time Window controls from the objective text: `F`, `Shift + drag`, `E`, `Esc`.
- Complete book and spice actions, close/return from the window, then proceed to S5.
- Confirm chapter close side-view, pebble kick, title, and save handoff play.

## Validation

- Compile/import smoke: `<temp>\anemora_ch1_playable_flow_compile2.log` clean for C# / shader compile blockers.
- Scene setup: `<temp>\anemora_ch1_playable_flow_scene_setup2.log` completed.
- PlayMode targeted route-gate tests: `<temp>\anemora_ch1_playable_flow_playmode.xml`, 10/10 passed.
- Runtime validator actual scene: `<temp>\anemora_ch1_playable_flow_runtime_validator.log`, Info=215, Warning=3, Error=0, PendingWiring=0.
- `git diff --check`: passed.
