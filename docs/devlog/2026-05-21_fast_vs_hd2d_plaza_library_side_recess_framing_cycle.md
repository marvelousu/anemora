# 2026-05-21 Fast VS HD2D Plaza Library Side Recess Framing Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Scene target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- Screenshot output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_side_recess_framing_20260521`

This cycle follows the plaza library deep exterior and side-wall surface passes. It frames the dark vertical side-wall elements so they read more as side recesses, narrow structural bays, or window slots rather than ungrounded black rods. It keeps the existing plaza footprint, transition points, story, Time Window behavior, route lights, and collision contracts unchanged.

No Meshy/API token, paid asset purchase, external download, or new third-party asset was used.

## Worker Cycle

- Worker: `gpt-5.4-mini` session `019e492a-d336-72f3-ba7f-04f81afff44e`.
- Worker instruction: add visual-only, non-colliding current/past-matched side-recess framing to the plaza library exterior; avoid story, Time Window, movement, route light, UI, font, input, and collision edits.
- Parent review: accepted the pass after changing the current-side recess cap/lip materials from shadow-heavy choices to current-stone choices so the pass would not reinforce the black-rod problem.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Wired `CreateCentralPlazaLibrarySideRecessFramingPolish(...)` into `CreateCentralPlaza(...)` after `CreateCentralPlazaLibrarySideWallSurfaceTuningPolish(...)`.
- Added current-side non-colliding visual objects:
  - `Current_CentralPlaza_LibrarySideRecessFraming_WestTopCap`
  - `Current_CentralPlaza_LibrarySideRecessFraming_WestBottomCap`
  - `Current_CentralPlaza_LibrarySideRecessFraming_WestLeftLip`
  - `Current_CentralPlaza_LibrarySideRecessFraming_WestRightLip`
  - `Current_CentralPlaza_LibrarySideRecessFraming_EastTopCap`
  - `Current_CentralPlaza_LibrarySideRecessFraming_EastBottomCap`
  - `Current_CentralPlaza_LibrarySideRecessFraming_EastLeftLip`
  - `Current_CentralPlaza_LibrarySideRecessFraming_EastRightLip`
- Added past-side equivalents under `Past_CentralPlaza_LibrarySideRecessFraming_*`, with small warm-light cues inside the recesses.
- Added `ValidateFastVsHd2dNinetySixthCyclePlazaLibrarySideRecessFraming()`.
- Added `ValidateCentralPlazaLibrarySideRecessFramingObject(...)`.
- Added `CaptureHd2dNinetySixthCycleScreenshotsBatch()` and `CaptureHd2dNinetySixthCycleScreenshotsToDirectory(...)`.

The Unity scene was regenerated so the new visual-only objects are present in the checked-in scene.

## Validation

Parent validation logs:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle96_plaza_library_side_recess_framing_parent_validate_retry_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle96_plaza_library_side_recess_framing_parent_validate_fix_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture logs:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle96_plaza_library_side_recess_framing_parent_capture_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle96_plaza_library_side_recess_framing_parent_capture_fix_20260521.log`
- Result: passed with `Fast VS ninety-sixth-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_side_recess_framing_20260521`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle96_plaza_library_side_recess_framing_parent_build_20260521.log`
- Result: passed with `Build Finished, Result: Success.` and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle96_plaza_library_side_recess_framing_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_side_recess_framing_20260521\01_current_plaza_library_side_recess_framing_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_side_recess_framing_20260521\02_past_plaza_library_side_recess_framing_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_side_recess_framing_20260521\03_current_plaza_library_side_recess_framing_oblique.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_side_recess_framing_20260521\04_past_plaza_library_side_recess_framing_oblique.png`

## Notes

- The pass is intentionally small. The current-side vertical recess still reads dark, but it now has caps/lips and less isolated black-rod behavior without adding new collision or route risk.
- The user's added tasks for outdoor sky/background and a more three-dimensional plaza library exterior are now represented by the prior pushed sky/horizon cycle and plaza-library deep exterior cycles. This side-recess pass is a follow-up cleanup on the same plaza library volume workstream.
- Unity batchmode produced transient Addressables/ProjectSettings/importer changes during validation and build; these were excluded from the commit so only the intended script, regenerated scene, devlog, and screenshots remain.
