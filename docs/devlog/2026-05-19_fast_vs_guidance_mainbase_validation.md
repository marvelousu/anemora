# 2026-05-19 Fast VS Guidance Mainbase Validation

## Scope

Work branch:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample-hd2d-work`
- Branch: `work/post-vs-public-20260518`

Public branch boundary:

- `main` is treated as the canonical public Fast VS build.
- This session does not commit to or modify `main`.
- The reviewed `main` guidance-log recovery was merged into `work/post-vs-public-20260518` as the gameplay base.

## User Review Items

- Restore the lower-left guidance log that was lost after the pre-public Time Window hint change.
- Keep the Time Window creation guidance as: `黄色い光の近くに、左ドラッグで時の窓を開く。`
- Preserve the reviewed `main` fix where possible because it had already been checked in another session.
- Keep validation improvements that catch invisible HUD panel regressions.
- Use gpt-5.4-mini worker review as part of the development cycle.

## Worker Cycle

- Plan: use `main` as the implementation base, keep only validation/runtime-HUD ordering improvements on the work branch, then verify by Unity batch validation and Windows build.
- Worker instruction: gpt-5.4-mini worker `019e3d83-9dd0-7f93-bcf7-2e675a310f3b` reviewed the staged guidance/HUD diffs in read-only mode.
- Worker result: the worker found the three focused files consistent with the plan and confirmed the requested Time Window wording was present.
- Integrator review: the parent session kept the `main` gameplay flow and retained the `RuntimeHudObjectivePanelActiveForReview` validation hook plus runtime HUD display-order fix.

## Changes

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample-hd2d-work\Assets\Scripts\FastVS\FastVsStoryFlowController.cs`
  - Kept the reviewed route/objective guidance behavior from `main`.
  - Preserved the exact Time Window hint: `黄色い光の近くに、左ドラッグで時の窓を開く。`
  - Added `RuntimeHudObjectivePanelActiveForReview` so validation can distinguish stored objective text from actually visible HUD.
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample-hd2d-work\Assets\Scripts\FastVS\FastVsStoryRuntimeHud.cs`
  - Ensured `ShowObjective()` and `HideAll()` clear dialogue/guide/question/brush state before applying persistent objective visibility.
  - This prevents the objective text from existing internally while the panel stays hidden because a previous UI state still blocks it.
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
  - Strengthened `ValidateFastVsStoryFlow()` to verify lower-left HUD text and panel active state.
  - Added route checks for interior, exterior, plaza, library, Reto prompt, Time Window unlock, Aria cue, book cue, Reto return, and VS clear.
  - Added checks that the objective panel is hidden during dialogue, guide, question, and brush beats.
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample-hd2d-work\docs\devlog\2026-05-18_fast_vs_left_bottom_timewindow_hint.md`
  - Restored the pre-existing devlog record after the `main` merge attempted to drop it from the work branch.

## Verification

- Unity MCP callable resources were checked from Codex and none were exposed in this session, so the verification path used Unity batchmode instead of live MCP scene inspection.
- Unity validation passed:
  - Command: `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
  - Log: `C:\Users\maro6\AppData\Local\Temp\anemora_fast_vs_guidance_mainbase_validate_20260519.log`
  - Result line: `Fast VS house slice validation passed.`
- Windows player build passed:
  - Command: `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildHouseSlicePlayer`
  - Log: `C:\Users\maro6\AppData\Local\Temp\anemora_fast_vs_guidance_mainbase_build_20260519.log`
  - Result line: `Build Finished, Result: Success.`
  - Player: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Windows player smoke passed:
  - The built player was launched hidden for 10 seconds and then terminated.
  - Player log: `C:\Users\maro6\AppData\LocalLow\DefaultCompany\Anemora\Player.log`
  - No `Exception`, `Error`, `NullReference`, `MissingReference`, `ArgumentException`, `Crash`, or `Failed` entries were found.

## Notes

- Unity batchmode generated nondeterministic project/addressables diffs during validation. Those generated changes are excluded from the semantic commit.
- `main` remains the public snapshot; this work is retained only on `work/post-vs-public-20260518`.
