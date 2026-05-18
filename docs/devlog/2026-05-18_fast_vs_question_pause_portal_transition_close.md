# 2026-05-18 Fast VS question position / Reto pause / portal transition close

## Scope

- Project: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample`
- Scene: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- Build: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## User Review Items

- The house-exit `?` is good, but should sit closer to Niro's head.
- The meaningful pause before `いえ。今のは、ただの独り言です。` disappeared and should be restored.
- If the player changes maps while a Time Window is open, a stray window appears near the right edge after the transition.

## Worker Cycle

- Plan: inspect the HUD question anchor, Reto opening sequence, and area transition / Time Window interaction before editing.
- Worker instruction: gpt-5.4-mini worker `019e39e2-6b72-7793-9288-4f246ffb844b` inspected the target files and reported likely patch points without editing files.
- Worker result: the worker identified `FastVsStoryRuntimeHud.UpdateQuestionPosition`, `FastVsStoryFlowController.RetoOpeningSteps`, `FastVsAreaDoorTransition.ExecuteTransition`, and `TimeWindowPairedSpacePortalController.ClosePortal` / a transition-close helper as the correct targets.
- Integrator review: the implementation below follows those targets, with editor validation added for the adjusted question offset, restored pre-line pause, and map-transition portal cleanup.

## Changes

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scripts\FastVS\FastVsStoryRuntimeHud.cs`
  - Lowered the door-beat `?` anchor from `1.72` to `1.46` world units over Niro, putting it closer to the character's head.
  - Exposed the offset for editor validation.
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scripts\FastVS\FastVsStoryDialoguePresenter.cs`
  - Matched the TMP fallback `?` anchor to the runtime HUD offset.
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scripts\FastVS\FastVsStoryFlowController.cs`
  - Matched the OnGUI fallback `?` anchor to the runtime HUD offset.
  - Inserted `scene1.reto.1c.library_history.pause_before_resolve_to_record` as a 1.85 second silent looking-up pause before `いえ。今のは、ただの独り言です。`.
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scripts\FastVS\FastVsAreaDoorTransition.cs`
  - Area transitions now close an open current-time Time Window before changing map sets and warping the player.
  - If Niro is in other-time space, the area transition is rejected instead of leaving a mismatched portal.
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scripts\TimeManagement\TimeWindowPairedSpacePortalController.cs`
  - Added `TryClosePortalForAreaTransition()` to close committed/preview portals and clear pending drag state during map transitions.
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
  - Validation now asserts the head-close `?` offset range.
  - Validation now asserts the silent pause occurs before Reto's `いえ。今のは、ただの独り言です。`.
  - Validation now opens a current-time Time Window, triggers exterior-to-plaza map travel, and asserts that no portal pair/preview remains afterward.

## Verification

- Build and validation passed:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Logs\fast_vs_build_validate_20260518_question_pause_portal_transition_close.log`
- Windows EXE updated:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Notes

- The existing screenshot capture path does not reliably include ScreenSpaceOverlay story HUD elements, so this pass relies on editor assertions for the `?` position contract.
- The map transition cleanup intentionally uses normal current-time close behavior and blocks transition if Niro is still in other-time space.
