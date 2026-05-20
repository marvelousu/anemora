# 2026-05-21 Fast VS HD2D Plaza Library Facade Surface Breakup Cycle

## Purpose

Cycle70 adds a restrained surface breakup pass to the central plaza library exterior so the facade reads less like one large flat plane from the plaza while keeping the existing entrance, windows, route glow pads, Time Window logic, dialogue, map switching, and collider layout unchanged.

## Files Touched

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-21_fast_vs_hd2d_plaza_library_facade_surface_breakup_cycle.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

## Implementation

- Added `CreateCentralPlazaLibraryFacadeSurfaceBreakupPolish(...)` and called it from `CreateCentralPlaza(...)` after the existing microdepth pass.
- Added 8 non-arrival current-side breakup objects:
  - `Current_CentralPlaza_LibraryFacadeSurfaceBreakup_UpperDustChipLeftA`
  - `Current_CentralPlaza_LibraryFacadeSurfaceBreakup_UpperDustChipCenterA`
  - `Current_CentralPlaza_LibraryFacadeSurfaceBreakup_UpperDustChipRightA`
  - `Current_CentralPlaza_LibraryFacadeSurfaceBreakup_LeftWingChipA`
  - `Current_CentralPlaza_LibraryFacadeSurfaceBreakup_RightWingChipA`
  - `Current_CentralPlaza_LibraryFacadeSurfaceBreakup_EntranceUpperPatchA`
  - `Current_CentralPlaza_LibraryFacadeSurfaceBreakup_BaseDustFleckLeftA`
  - `Current_CentralPlaza_LibraryFacadeSurfaceBreakup_BaseDustFleckRightA`
- Added 8 non-arrival past-side breakup objects:
  - `Past_CentralPlaza_LibraryFacadeSurfaceBreakup_UpperStoneBandA`
  - `Past_CentralPlaza_LibraryFacadeSurfaceBreakup_LeftWingTrimBandA`
  - `Past_CentralPlaza_LibraryFacadeSurfaceBreakup_RightWingTrimBandA`
  - `Past_CentralPlaza_LibraryFacadeSurfaceBreakup_EntranceWarmBandA`
  - `Past_CentralPlaza_LibraryFacadeSurfaceBreakup_LeftUpperLightTickA`
  - `Past_CentralPlaza_LibraryFacadeSurfaceBreakup_RightUpperLightTickA`
  - `Past_CentralPlaza_LibraryFacadeSurfaceBreakup_BaseWarmTileLeftA`
  - `Past_CentralPlaza_LibraryFacadeSurfaceBreakup_BaseWarmTileRightA`
- Added `ValidateFastVsHd2dSeventiethCyclePlazaLibraryFacadeSurfaceBreakup()` and `ValidatePlazaLibraryFacadeSurfaceBreakupObject(...)` to verify parentage, non-arrival landmark state, `PropOrFeature`, collider absence, material tokens, thin z thickness, required existing library facade objects, and the current door panel not using `doorway_dark`.
- Added `CaptureHd2dSeventiethCycleScreenshotsBatch()` and `CaptureHd2dSeventiethCycleScreenshotsToDirectory(...)`.

## Validation

- Worker validate:
  `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle70_plaza_library_facade_surface_breakup_worker_validate_20260521.log`
  Result: passed with `Fast VS house slice validation passed.`
- Worker capture:
  `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle70_plaza_library_facade_surface_breakup_worker_capture_20260521.log`
  Result: passed with `Fast VS seventieth-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_facade_surface_breakup_20260521`
- Parent validate:
  `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle70_plaza_library_facade_surface_breakup_parent_validate_r3_20260521.log`
  Result: passed with `Fast VS house slice validation passed.`
- Parent capture:
  `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle70_plaza_library_facade_surface_breakup_parent_capture_20260521.log`
  Result: passed with `Fast VS seventieth-cycle screenshots captured`.
- Parent build:
  `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle70_plaza_library_facade_surface_breakup_parent_build_20260521.log`
  Result: passed with `Build Finished, Result: Success.`
- Parent player smoke:
  `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle70_plaza_library_facade_surface_breakup_parent_smoke_20260521.log`
  Result: 20-second startup smoke, `match_count=0`.
- Unity licensing logs still include `Access token is unavailable; failed to update`, but that did not block validation or screenshot capture.

## Parent Review Notes

- The current and past plaza-library facade screenshots were reviewed after the parent adjustment. The pass remains intentionally small: route glow pads, door panels, window panes, map transition surfaces, dialogue, and Time Window behavior were not changed.
- The upper facade breakup was lowered and shortened after worker review so it does not add a new large bar across the roof line.
- A pre-existing small grate-like element above the roof line is still visible in close capture. It is not handled in this cycle and is better addressed together with the follow-up library-depth task.

## Screenshot Evidence

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_facade_surface_breakup_20260521\01_current_plaza_library_facade_surface_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_facade_surface_breakup_20260521\02_past_plaza_library_facade_surface_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_facade_surface_breakup_20260521\03_current_plaza_library_facade_surface_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_facade_surface_breakup_20260521\04_past_plaza_library_facade_surface_close.png`

## Asset / API Note

- No API token addition was required.
- No paid asset purchase was required.

## Follow-up Tasks Added

- Sky/background creation is now a follow-up HD-2D task. The previous rough sky attempt was reverted, so the next pass should be split into a small, reviewable implementation with dedicated screenshots before adoption.
- Central plaza library exterior depth is now a follow-up HD-2D task. The facade should become less like a flat stage front by extending the library volume backward within the current plaza map bounds, while preserving existing door, window, route glow, map transition, and collision contracts.
