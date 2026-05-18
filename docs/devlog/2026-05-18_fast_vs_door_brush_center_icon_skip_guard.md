# 2026-05-18 Fast VS door brush center icon / skip guard

## Scope

- Project: `<repo>`
- Scene: `<repo>/Assets/Scenes/Anemora_FastVS_HouseSlice.unity`
- Build: `<repo>/Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`

## User Review Items

- House-exit beat was interpreted incorrectly: `?` must appear directly over Niro's head, then disappear, then the brush picture appears at screen center.
- The player could sometimes exit without seeing the house-exit brush beat.
- The post-`...本物だ` Reto motion beat was still not visually apparent.
- A box-like ruin prop was visibly intersecting the back/right library bookshelf area.

## Worker Cycle

- Plan: keep the current Fast VS implementation, fix only the reported interaction/visual defects, add editor validation for each defect, then rebuild the Windows executable.
- Worker instruction: gpt-5.4-mini worker `019e39c9-3b00-7980-9031-df3526ddf3be` inspected the door transition, story flow, HUD, Reto animator, and library ruin placement without editing files.
- Worker result: the worker identified `storyFlow` wiring on door transitions, the door-beat HUD placement, Reto's non-visible `LookingUp`/idle transition, and the current-library toppled book stack as the likely patch targets.
- Integrator review: the changes below were applied locally and then guarded by `AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`.

## Changes

- `<repo>/Assets/Scripts/FastVS/FastVsStoryFlowController.cs`
  - Door beat now keeps page 0 as `?` only above Niro.
  - Door beat page 1/2 hides `?` and shows the brush picture at the screen center.
  - Interior-to-exterior transition can force-start the brush beat if the player reaches the actual door trigger without hitting the pre-trigger.
  - Reto's post-`...本物だ` motion now uses explicit lowering and raising animation states before `...そうですか。`.
- `<repo>/Assets/Scripts/FastVS/FastVsStoryRuntimeHud.cs`
  - Runtime HUD now renders the brush as a centered framed `RawImage`, using `<repo>/Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_timewriter_brush_icon_v01.png`.
  - Review properties expose question state, brush state, brush position, and brush texture name.
- `<repo>/Assets/Scripts/FastVS/FastVsStoryDialoguePresenter.cs`
  - TMP presenter fallback now uses the same centered brush-image UI.
- `<repo>/Assets/Scripts/FastVS/FastVsAreaDoorTransition.cs`
  - Interior door transitions now expose whether `storyFlow` is serialized, and runtime transition checks the story block again at the actual trigger.
- `<repo>/Assets/Scripts/FastVS/FastVsRetoWritingAnimator.cs`
  - Added explicit `SetLoweringForReview()` and `SetRaisingForReview()` entry points.
  - `LookingUp` now holds the last talk-loop frame instead of visually matching normal dialogue idle.
- `<repo>/Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Scene generation serializes `storyFlow` into all area door transitions.
  - Generates and imports `<repo>/Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_timewriter_brush_icon_v01.png`.
  - Validation asserts the actual door trigger starts the brush beat, page 0 is question-only, page 1/2 are centered brush-only, and the Reto post-`...本物だ` beats enter lowering/raising states.
  - The current-library toppled book stack was moved lower and away from the right-back bookshelf silhouette.

## Verification

- Build and validation passed:
  - `<repo>/Logs/fast_vs_build_validate_20260518_door_brush_center_icon_skip_guard.log`
- Windows EXE updated:
  - `<repo>/Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Screenshot capture passed:
  - `<repo>/Logs/fast_vs_capture_review_20260518_door_brush_center_icon_skip_guard.log`
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/03_library_reto_desk.png`
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/05_library_past_no_temp_people.png`
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/07_plaza_library_facade_current.png`

## Notes

- The brush picture is a speed-first generated local PNG, not a final art lock. It is intentionally isolated at `<repo>/Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_timewriter_brush_icon_v01.png` so it can be replaced by Meshy/API art later without touching story flow.
- Existing screenshot capture still does not reliably include ScreenSpaceOverlay HUD layers, so the door-beat visual order is enforced by editor assertions.
