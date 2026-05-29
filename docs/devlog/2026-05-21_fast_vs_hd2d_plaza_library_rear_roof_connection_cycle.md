# 2026-05-21 Fast VS HD2D Plaza Library Rear Roof Connection Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Scene target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- Screenshot output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_rear_roof_connection_20260521`

This cycle continues the user-requested plaza library exterior volume work. It adds a restrained rear roof and upper-wall connection pass so the plaza-map library reads less like a flat facade and more like a building volume extending backward within the current map range.

No Meshy/API token, paid asset purchase, external download, or new third-party asset was used.

## Worker Cycle

- Worker: `gpt-5.4-mini` session `019e492a-d336-72f3-ba7f-04f81afff44e`.
- Worker instruction: add visual-only, non-colliding current/past-matched rear roof connection cues; do not touch story, Time Window, route transitions, route lights, UI, font, input, character sprites, map coordinates, or collision.
- Parent review: adjusted material choices and review camera framing before accepting the cycle.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Wired `CreateCentralPlazaLibraryRearRoofConnectionPolish(...)` into `CreateCentralPlaza(...)` after `CreateCentralPlazaLibrarySideRecessFramingPolish(...)`.
- Added current-side non-colliding visual objects:
  - `Current_CentralPlaza_LibraryRearRoofConnection_RearRoofRidgeCapA`
  - `Current_CentralPlaza_LibraryRearRoofConnection_RearUpperWallUnderEaveBandA`
  - `Current_CentralPlaza_LibraryRearRoofConnection_WestRearRoofReturnA`
  - `Current_CentralPlaza_LibraryRearRoofConnection_EastRearRoofReturnA`
  - `Current_CentralPlaza_LibraryRearRoofConnection_WestRearCornerJoinA`
  - `Current_CentralPlaza_LibraryRearRoofConnection_EastRearCornerJoinA`
- Added past-side equivalents under `Past_CentralPlaza_LibraryRearRoofConnection_*`.
- Added `ValidateFastVsHd2dNinetySeventhCyclePlazaLibraryRearRoofConnection()`.
- Added `ValidateCentralPlazaLibraryRearRoofConnectionObject(...)`.
- Added `CaptureHd2dNinetySeventhCycleScreenshotsBatch()` and `CaptureHd2dNinetySeventhCycleScreenshotsToDirectory(...)`.
- Adjusted past-side rear/side accent materials that looked like floating bright plates in the new oblique review angle:
  - `Past_CentralPlaza_LibraryDeepExteriorVolume_RearWindowWarmHintA` now uses past exterior wall material.
  - Past side-recess lip/accent pieces now use past exterior wall material instead of window/warm-light material.
- Parent patch changed the new rear under-eave band from shadow/window-light style material to dust/current wall and past exterior wall style material to avoid adding a new black band or a floating white strip.
- Parent patch removed the initially proposed `RearCenterCapA` because it risked reading as a detached small plate.

The Unity scene was regenerated so the new visual-only objects and material adjustments are present in the checked-in scene.

## Validation

Parent validation logs:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle97_plaza_library_rear_roof_connection_parent_validate_retry_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle97_plaza_library_rear_roof_connection_parent_validate_fix_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle97_plaza_library_rear_roof_connection_parent_validate_fix2_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle97_plaza_library_rear_roof_connection_parent_validate_fix3_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle97_plaza_library_rear_roof_connection_parent_validate_fix4_20260521.log`
- Final result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture logs:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle97_plaza_library_rear_roof_connection_parent_capture_retry_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle97_plaza_library_rear_roof_connection_parent_capture_fix_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle97_plaza_library_rear_roof_connection_parent_capture_fix2_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle97_plaza_library_rear_roof_connection_parent_capture_fix3_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle97_plaza_library_rear_roof_connection_parent_capture_fix4_20260521.log`
- Final result: passed with `Fast VS ninety-seventh-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_rear_roof_connection_20260521`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle97_plaza_library_rear_roof_connection_parent_build_20260521.log`
- Result: passed with `Build Finished, Result: Success.` and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle97_plaza_library_rear_roof_connection_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_rear_roof_connection_20260521\01_current_plaza_library_rear_roof_connection_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_rear_roof_connection_20260521\02_past_plaza_library_rear_roof_connection_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_rear_roof_connection_20260521\03_current_plaza_library_rear_roof_connection_oblique.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_rear_roof_connection_20260521\04_past_plaza_library_rear_roof_connection_oblique.png`

## Notes

- The pass is intentionally conservative: it adds roof/wall/corner connection cues without changing navigation, trigger placement, route light placement, or Time Window same-coordinate behavior.
- The final oblique screenshots are wide enough to inspect the rear roof volume. They also exposed some older bright past-side accents; this cycle toned down the ones near the rear/side roof work area, but larger exterior window-light choices remain a separate future polish topic.
- Unity batchmode produced transient Addressables/ProjectSettings/importer changes during validation and build; these were excluded from the commit so only the intended script, regenerated scene, devlog, and screenshots remain.
