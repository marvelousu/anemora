# 2026-05-18 Fast VS left-bottom Time Window hint

## Scope

- Project: `<repo>`
- Scene: `<repo>/Assets/Scenes/Anemora_FastVS_HouseSlice.unity`
- Build: `<repo>/Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`

## User Review Items

- Keep Git checkpoints strict because reverting a single disliked direction was too costly without small commits.
- Change the lower-left guide so it only shows the Time Window creation method, briefly.
- Do not keep route/objective text such as Reto desk guidance in the lower-left HUD.

## Worker Cycle

- Plan: isolate the lower-left HUD source, then add validation so story-objective text cannot return there.
- Worker instruction: gpt-5.4-mini worker `019e3a2d-ac80-79b0-b779-9a405b70619b` inspected the HUD and validation touch points without editing.
- Worker result: the worker identified `<repo>/Assets/Scripts/FastVS/FastVsStoryFlowController.cs`, `<repo>/Assets/Scripts/FastVS/FastVsStoryRuntimeHud.cs`, and `<repo>/Assets/Editor/AnemoraFastVsHouseSliceSetup.cs` as the relevant patch and validation points.
- Integrator review: the final patch keeps the runtime HUD renderer unchanged and narrows the text source/scene setup to a single creation hint.

## Changes

- `<repo>/Assets/Scripts/FastVS/FastVsStoryFlowController.cs`
  - Added `FastVsStoryFlowController.TimeWindowCreationHintTextForReview` with the short text `左ドラッグで時の窓を描く`.
  - Removed lower-left story objective branches from runtime HUD, TMP fallback, and OnGUI fallback presentation.
  - The lower-left persistent HUD is now empty until Time Window input unlocks, then shows only the Time Window creation hint.
- `<repo>/Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Reused the same short hint for the generated visual direction guide text.
  - Added editor validation that the HUD is empty before unlock, shows only the Time Window creation hint after unlock, and still does not revert to story guidance after the past-library flags.
- `<repo>/Assets/Scenes/Anemora_FastVS_HouseSlice.unity`
  - Updated the serialized guide hint line to match the new short text.

## Verification

- Build and validation passed:
  - `<repo>/Logs/fast_vs_build_validate_20260518_left_bottom_timewindow_hint.log`
- Review screenshots regenerated:
  - `<repo>/Logs/fast_vs_capture_review_20260518_left_bottom_timewindow_hint.log`
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/10_library_current_yellow_timewindow_cues.png`
- Minimal-scene validation passed after removing Unity's nondeterministic scene reserialization from the commit:
  - `<repo>/Logs/fast_vs_validate_20260518_left_bottom_timewindow_hint_min_scene.log`
- Windows EXE updated:
  - `<repo>/Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`

## Notes

- A Fast VS baseline commit was created first so future one-step rollback has a stable anchor: `f31608e Baseline Fast VS V24 sample state`.
- Unity scene generation rewrote many fileIDs during validation. Those generated ordering changes were intentionally removed from this slice, leaving only the semantic scene hint change.
