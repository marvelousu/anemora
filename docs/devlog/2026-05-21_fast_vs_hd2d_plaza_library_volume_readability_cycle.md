# 2026-05-21 Fast VS HD2D Plaza Library Volume Readability Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_volume_readability_20260521`

This cycle adds deterministic visual-only HD-2D readability details to the central plaza library exterior so the building reads less like a flat facade and more like a volume extending backward within the current map bounds. Gameplay, route pads, Time Window behavior, story, UI/font, character behavior, and colliders are left untouched.

No API token, no paid asset purchase, and no external download was used.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Wired `CreateCentralPlazaLibraryVolumeReadabilityPolish(...)` after the existing rear-volume polish.
- Added side eave readability strips, roof seam, rear wall break, and rear ground-contact accents.
- Added `CaptureHd2dEightyFirstCycleScreenshotsBatch()` and `CaptureHd2dEightyFirstCycleScreenshotsToDirectory(...)`.
- Added `ValidateFastVsHd2dEightyFirstCyclePlazaLibraryVolumeReadability()`.
- Parent review corrected the eave object split so current keeps shadow eaves and past keeps highlight eaves, with validation guarding against accidental mixed current/past helper objects.

New visual-only objects:

- `Current_CentralPlaza_LibraryVolumeReadability_WestSideEaveShadowA`
- `Current_CentralPlaza_LibraryVolumeReadability_EastSideEaveShadowA`
- `Current_CentralPlaza_LibraryVolumeReadability_BackRoofSeamA`
- `Current_CentralPlaza_LibraryVolumeReadability_BackWallVerticalBreakA`
- `Current_CentralPlaza_LibraryVolumeReadability_RearGroundContactDustA`
- `Current_CentralPlaza_LibraryVolumeReadability_WestSideBaseShadowA`
- `Current_CentralPlaza_LibraryVolumeReadability_EastSideBaseShadowA`
- `Past_CentralPlaza_LibraryVolumeReadability_WestSideEaveHighlightA`
- `Past_CentralPlaza_LibraryVolumeReadability_EastSideEaveHighlightA`
- `Past_CentralPlaza_LibraryVolumeReadability_BackRoofSeamA`
- `Past_CentralPlaza_LibraryVolumeReadability_BackWallVerticalBreakA`
- `Past_CentralPlaza_LibraryVolumeReadability_RearGroundWarmA`
- `Past_CentralPlaza_LibraryVolumeReadability_WestSideBaseWarmA`
- `Past_CentralPlaza_LibraryVolumeReadability_EastSideBaseWarmA`

## Validation

Worker validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle81_plaza_library_volume_readability_worker_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Worker screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle81_plaza_library_volume_readability_worker_capture_20260521.log`
- Result: passed with `Fast VS eighty-first-cycle screenshots captured`.

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle81_plaza_library_volume_readability_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle81_plaza_library_volume_readability_parent_capture_20260521.log`
- Result: passed with `Fast VS eighty-first-cycle screenshots captured`.

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle81_plaza_library_volume_readability_parent_build_20260521.log`
- Result: passed with `Build Finished, Result: Success.`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle81_plaza_library_volume_readability_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_volume_readability_20260521\01_current_central_plaza_library_volume_readability_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_volume_readability_20260521\02_past_central_plaza_library_volume_readability_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_volume_readability_20260521\03_current_central_plaza_library_volume_readability_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_volume_readability_20260521\04_past_central_plaza_library_volume_readability_close.png`

## Notes

- The cycle stayed deterministic and visual-only.
- The parent session regenerated the screenshots after review fixes, then ran Unity validation, player build, and startup smoke.
- Unity produced unrelated auto-diffs in scene/material/settings files outside the intended ownership list; those were cleaned before committing this cycle.
