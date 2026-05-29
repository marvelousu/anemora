# 2026-05-21 Fast VS HD2D Plaza Library Backward Volume Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_backward_volume_20260521`

This cycle addresses the user's added task that the central plaza library exterior still reads too much like a flat facade. The pass keeps the current plaza bounds and adds a small visual-only rear volume layer so the library reads as a building extending backward from the entrance wall.

No API token, paid asset purchase, or external download was used.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Wired `CreateCentralPlazaLibraryBackwardVolumePolish(...)` after the existing plaza library exterior/rear/readability volume passes.
- Added five current-side non-colliding visual objects:
  - `Current_CentralPlaza_LibraryBackwardVolume_BackRoofPlaneA`
  - `Current_CentralPlaza_LibraryBackwardVolume_WestReturnFaceA`
  - `Current_CentralPlaza_LibraryBackwardVolume_EastReturnFaceA`
  - `Current_CentralPlaza_LibraryBackwardVolume_RearBaseContactStripA`
  - `Current_CentralPlaza_LibraryBackwardVolume_UpperEaveSeamA`
- Added five past-side non-colliding visual objects:
  - `Past_CentralPlaza_LibraryBackwardVolume_BackRoofPlaneA`
  - `Past_CentralPlaza_LibraryBackwardVolume_WestReturnFaceA`
  - `Past_CentralPlaza_LibraryBackwardVolume_EastReturnFaceA`
  - `Past_CentralPlaza_LibraryBackwardVolume_RearBaseContactStripA`
  - `Past_CentralPlaza_LibraryBackwardVolume_UpperEaveSeamA`
- Added `CreateNonArrivalLandmarkCubeShadowSafe(...)` so the new volume props stay visual-only and do not cast/receive shadows.
- Added `ValidateFastVsHd2dEightySeventhCyclePlazaLibraryBackwardVolume()`.
- Added `CaptureHd2dEightySeventhCycleScreenshotsBatch()` and `CaptureHd2dEightySeventhCycleScreenshotsToDirectory(...)`.
- Corrected the oblique screenshot camera after the first capture landed too close to the roof/side wall and was not reviewable.

## Validation

Worker handoff:

- Worker `019e4902-e046-7d53-bdf0-965499793f92` was assigned the detailed Cycle87 implementation prompt.
- The worker produced the initial code diff but did not return a final report or validation logs before shutdown.
- Parent session reviewed the diff, fixed the oblique screenshot camera, and completed validation, capture, build, smoke, and repository hygiene.

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle87_plaza_library_backward_volume_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`
- Note: Unity emitted existing obsolete API warnings unrelated to this cycle.

Parent screenshot capture logs:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle87_plaza_library_backward_volume_parent_capture_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle87_plaza_library_backward_volume_parent_capture_rerun_20260521.log`
- Result: passed with `Fast VS eighty-seventh-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_backward_volume_20260521`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle87_plaza_library_backward_volume_parent_build_20260521.log`
- Result: passed with `Fast VS house slice validation passed.` and `Build Finished, Result: Success.`
- Note: Unity emitted unrelated startup/license/import noise, but the batch completed successfully.

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle87_plaza_library_backward_volume_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_backward_volume_20260521\01_current_plaza_library_backward_volume_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_backward_volume_20260521\02_past_plaza_library_backward_volume_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_backward_volume_20260521\03_current_plaza_library_backward_volume_oblique.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_backward_volume_20260521\04_past_plaza_library_backward_volume_oblique.png`

## Notes

- This pass intentionally does not change the route pads, library entrance trigger, map bounds, Time Window behavior, UI/font, story, characters, or gameplay colliders.
- The oblique screenshots show the building's side/rear roof mass more clearly, but the outdoor sky/background is still a separate follow-up task.
