# 2026-05-21 Fast VS HD2D Plaza Library Entry Depth Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_entry_depth_20260521`

This cycle adds a small visual-only HD-2D pass to the central plaza library entrance so the facade reads less like a flat pasted wall. The pass reinforces the doorway recess, threshold, side-wall returns, base contact, and roof lip within the existing plaza map bounds. Gameplay, route pads, Time Window behavior, story, UI/font, character behavior, scene transitions, and colliders were left untouched.

No API token, paid asset purchase, or external download was used.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Wired `CreatePlazaLibraryEntryDepthPolish(...)` after the previous plaza library volume readability pass.
- Added six current non-arrival landmark cubes:
  - `Current_CentralPlaza_LibraryEntryDepth_DoorRecessShadowA`
  - `Current_CentralPlaza_LibraryEntryDepth_InnerThresholdDustA`
  - `Current_CentralPlaza_LibraryEntryDepth_LeftReturnShadowA`
  - `Current_CentralPlaza_LibraryEntryDepth_RightReturnShadowA`
  - `Current_CentralPlaza_LibraryEntryDepth_BaseContactDustA`
  - `Current_CentralPlaza_LibraryEntryDepth_RoofLipUndersideShadowA`
- Added six past non-arrival landmark cubes:
  - `Past_CentralPlaza_LibraryEntryDepth_DoorRecessWarmA`
  - `Past_CentralPlaza_LibraryEntryDepth_InnerThresholdWarmA`
  - `Past_CentralPlaza_LibraryEntryDepth_LeftReturnHighlightA`
  - `Past_CentralPlaza_LibraryEntryDepth_RightReturnHighlightA`
  - `Past_CentralPlaza_LibraryEntryDepth_BaseContactWarmA`
  - `Past_CentralPlaza_LibraryEntryDepth_RoofLipWarmUndersideA`
- Added `ValidateFastVsHd2dEightyFourthCyclePlazaLibraryEntryDepth()`.
- Added `CaptureHd2dEightyFourthCycleScreenshotsBatch()` and `CaptureHd2dEightyFourthCycleScreenshotsToDirectory(...)`.
- Wired the new validation into `ValidateHouseSliceBatch()`.

## Validation

Worker handoff:

- Worker `019e48cc-29ec-75e3-939a-eeac73d04f62` produced the initial entry-depth generation and screenshot scaffold but did not complete validation or devlog before shutdown.
- Parent session reviewed the patch, added the missing validation wiring, tightened scale constraints, and completed validation, capture, build, smoke, and repository hygiene.

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle84_plaza_library_entry_depth_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle84_plaza_library_entry_depth_parent_capture_20260521.log`
- Result: passed with `Fast VS eighty-fourth-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_entry_depth_20260521`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle84_plaza_library_entry_depth_parent_build_20260521.log`
- Result: passed with `Build Finished, Result: Success.`
- Note: Unity emitted unrelated startup/license/import noise, but the batch completed successfully.

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle84_plaza_library_entry_depth_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_entry_depth_20260521\01_current_plaza_library_entry_depth_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_entry_depth_20260521\02_past_plaza_library_entry_depth_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_entry_depth_20260521\03_current_plaza_library_entry_depth_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_entry_depth_20260521\04_past_plaza_library_entry_depth_close.png`

## Notes

- The pass stayed deterministic and non-colliding.
- All new objects are `PropOrFeature` landmarks built from `CreateNonArrivalLandmarkCube(...)`.
- Large vertical recess panels were avoided; doorway depth is expressed with thin strips plus side returns so the entrance does not turn into a dark rectangle.
- Existing central plaza to library route pads and library door panels are asserted by validation to protect the transition contract.
