# 2026-05-18 Fast VS skip opening wake line

## Scope

- Project: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample`
- Scene: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- Build: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## User Review Items

- Remove the initial VS-only line `夢を見ていたような、夢を見ていなかったような。`.
- Keep this as a Fast VS branch-only adjustment.
- Keep the later house-exit Timewriter brush beat and Reto story flow intact.

## Worker Cycle

- Plan: find the opening-wake render paths, skip that beat in VS only, and update editor validation so the line cannot return.
- Worker instruction: gpt-5.4-mini worker `019e3a3d-8d0c-7081-9ff4-835dece2e620` inspected the opening-wake code and validation touch points without editing.
- Worker result: the worker confirmed the relevant targets were `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scripts\FastVS\FastVsStoryFlowController.cs` and `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`.
- Integrator review: the final patch removes the line from all Fast VS opening render paths and keeps the later door brush trigger unchanged.

## Changes

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scripts\FastVS\FastVsStoryFlowController.cs`
  - Starts the VS branch at `opening.house_interior` instead of rendering the opening wake dialogue.
  - Added `CompleteOpeningWakeWithoutDialogue()` and reused it from `Awake()`, `TriggerOpeningWakeForReview()`, and the legacy opening advance path.
  - Removed the opening line from OnGUI, TMP fallback, and runtime HUD presentation paths.
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
  - Updated `ValidateFastVsStoryFlow()` to assert that the VS branch starts playable in the house interior with no opening line or active HUD dialogue.

## Verification

- Build and validation passed:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Logs\fast_vs_build_validate_20260518_skip_opening_wake_line.log`
- Windows EXE updated:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Notes

- The house-exit `?` and Timewriter brush reveal still run at the interior exit trigger; only the very first wake dialogue is skipped.
- Unity build generation temporarily rewrote unrelated scene/material/addressables files. Those generated changes were removed from this instruction slice before commit.
