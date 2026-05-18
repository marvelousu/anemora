# 2026-05-18 Fast VS door brush / Reto pause refine

## Scope

- Project: `<repo>`
- Scene: `<repo>/Assets/Scenes/Anemora_FastVS_HouseSlice.unity`
- Build: `<repo>/Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`

## Worker cycle

- Plan: apply only the latest review deltas: door pre-exit wording/order, less abrupt house exit, Reto resolve line/pause, yellow guide wording, and a visible down/up beat between `...本物だ` and `...そうですか。`.
- Worker instruction: gpt-5.4-mini worker `019e39b4-fdbb-7e03-b725-5c2686699a65` inspected the relevant Fast VS scripts and editor validation without editing files.
- Review result: worker confirmed the same patch targets: `FastVsStoryFlowController`, `FastVsStoryRuntimeHud`, `FastVsStoryDialoguePresenter`, `FastVsAreaDoorTransition`, and `AnemoraFastVsHouseSliceSetup`.

## Changes

- `<repo>/Assets/Scripts/FastVS/FastVsStoryFlowController.cs`
  - Door pre-exit line changed to `(ポケットに、何か...)`.
  - Door beat order changed to `?` only, then `?` plus brush image, then `(...筆?)`.
  - Door brush trigger is now a separate pre-transition local box just before the interior-to-exterior warp trigger.
  - Reto line changed to `いえ。今のは、ただの独り言です。`.
  - Long Timewriter pocket-glow pause extended to 3.65 seconds.
  - Reto return beat now uses `...本物だ` in looking-up pose, then a face-down/dialogue-idle pause, then a looking-up pause before `...そうですか。`.
  - Past-observation guide now says `黄色い光の近くで、時の窓を開く。`.
- `<repo>/Assets/Scripts/FastVS/FastVsStoryRuntimeHud.cs`
  - Door beat HUD now separates question visibility from brush visibility.
  - Brush image is positioned above the player/question instead of sharing a single boolean with the `?`.
  - Persistent objective is hidden after door-beat story UI is activated, preventing stale guide text during the beat.
- `<repo>/Assets/Scripts/FastVS/FastVsStoryDialoguePresenter.cs`
  - TMP presenter fallback follows the same separated question/brush visibility contract.
- `<repo>/Assets/Scripts/FastVS/FastVsAreaDoorTransition.cs`
  - Interior-to-exterior story block is checked before the actual transition trigger so the event can fire just before movement.
- `<repo>/Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Scene generation serializes the new door-brush pre-trigger.
  - Validation now asserts the door beat page order, revised pocket line, revised Reto line, and Reto down/up motion beat.

## Verification

- Build and validation passed:
  - `<repo>/Logs/fast_vs_build_validate_20260518_door_brush_reto_pause_refine_v2.log`
- Screenshot capture passed:
  - `<repo>/Logs/fast_vs_capture_review_20260518_door_brush_reto_pause_refine.log`
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/09_library_timewriter_pocket_glow.png`
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/10_library_current_yellow_timewindow_cues.png`

## Notes

- The door beat order is enforced by editor validation rather than only by visual review.
- Existing screenshot capture uses camera render and does not reliably capture ScreenSpaceOverlay dialogue UI; story/HUD assertions cover the door beat text/order for this pass.
