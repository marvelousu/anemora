# 2026-05-21 Fast VS HD2D Plaza Library Exterior Depth Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_exterior_depth_20260521`

This cycle improves the central plaza library exterior so it reads as a deeper building mass instead of a flat facade board. The pass keeps the current/past split intact, does not touch the entrance, windows, map transitions, colliders, or story hooks, and stays within the current map range.

The sky/background treatment is intentionally not implemented in this cycle. It remains a candidate for the next pass only.

No API token, paid asset purchase, or external art source was used. The change relies on existing materials plus code-generated non-arrival landmark cubes.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateCentralPlazaLibraryExteriorDepthPolish(...)`.
- Wired it into `CreateCentralPlaza(...)` immediately after `CreateCentralPlazaLibraryFacadeSurfaceBreakupPolish(...)` and before `CreateCentralPlazaLibraryApproachHd2dPolish(...)`.
- Added 18 new visual-only, non-arrival landmark cubes:
  - `Current_CentralPlaza_LibraryExteriorDepth_RoofRearMassA`
  - `Current_CentralPlaza_LibraryExteriorDepth_RoofRearMassB`
  - `Current_CentralPlaza_LibraryExteriorDepth_RoofRearLipA`
  - `Current_CentralPlaza_LibraryExteriorDepth_WestSideWallDepthA`
  - `Current_CentralPlaza_LibraryExteriorDepth_WestSideWallDepthB`
  - `Current_CentralPlaza_LibraryExteriorDepth_EastSideWallDepthA`
  - `Current_CentralPlaza_LibraryExteriorDepth_EastSideWallDepthB`
  - `Current_CentralPlaza_LibraryExteriorDepth_RearUpperDustBandA`
  - `Current_CentralPlaza_LibraryExteriorDepth_RearLowerDustBandA`
  - `Past_CentralPlaza_LibraryExteriorDepth_RoofRearMassA`
  - `Past_CentralPlaza_LibraryExteriorDepth_RoofRearMassB`
  - `Past_CentralPlaza_LibraryExteriorDepth_RoofRearLipA`
  - `Past_CentralPlaza_LibraryExteriorDepth_WestSideWallDepthA`
  - `Past_CentralPlaza_LibraryExteriorDepth_WestSideWallDepthB`
  - `Past_CentralPlaza_LibraryExteriorDepth_EastSideWallDepthA`
  - `Past_CentralPlaza_LibraryExteriorDepth_EastSideWallDepthB`
  - `Past_CentralPlaza_LibraryExteriorDepth_RearUpperWarmBandA`
  - `Past_CentralPlaza_LibraryExteriorDepth_RearLowerStoneBandA`
- Kept the additions visual-only by using `CreateNonArrivalLandmarkCube(...)` with `PropOrFeature`, `countsForArrival = false`, and no collider.
- Added `ValidateFastVsHd2dSeventyFirstCyclePlazaLibraryExteriorDepth()` and `ValidatePlazaLibraryExteriorDepthObject(...)`.
- Validation now checks:
  - every new object exists;
  - the objects remain under `Current_CentralPlazaMap_SeparateSpace` or `Past_CentralPlazaMap_SeparateSpace`;
  - each object keeps a renderer and material;
  - each object has no collider;
  - each landmark remains `PropOrFeature`;
  - each landmark remains non-arrival;
  - each landmark id starts with `Current.central_plaza.library_exterior_depth.` or `Past.central_plaza.library_exterior_depth.`;
  - each object stays within the intended plaza depth range and scale bounds;
  - `Current_CentralPlaza_ToLibrary_MapMoveGlowPad`, `Past_CentralPlaza_ToLibrary_MapMoveGlowPad`, `Current_CentralPlaza_LibraryDoorPanelsLeft`, `Past_CentralPlaza_LibraryDoorPanelsLeft`, `Current_CentralPlaza_LibraryNorthFacade`, and `Past_CentralPlaza_LibraryNorthFacade` remain present;
  - the current door panel does not use `doorway_dark`.
- Added `CaptureHd2dSeventyFirstCycleScreenshotsBatch()` and `CaptureHd2dSeventyFirstCycleScreenshotsToDirectory(...)`.

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`:

- Bumped the status/version line to `v6.35`.
- Increased root-level markdown coverage to 311.
- Increased dated devlog coverage to 309.
- Increased screenshot evidence coverage to 510.
- Increased the `2026-05-21` count to 18.
- Added this cycle to the `2026-05-21` table.

## Validation

Worker validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle71_plaza_library_exterior_depth_worker_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Worker screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle71_plaza_library_exterior_depth_worker_capture_20260521.log`
- Result: passed and wrote the requested screenshots.

Parent review notes:

- The first oblique screenshot framing was too close to the roof and did not show enough facade context.
- The parent pass adjusted only the screenshot capture camera framing, then reran capture.
- The final screenshots show a readable rear roof mass and side-wall depth while preserving the plaza-to-library glow, door, and facade.
- The black void above the exterior remains visible in the oblique review screenshots and is intentionally left for the next sky/background cycle.

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle71_plaza_library_exterior_depth_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle71_plaza_library_exterior_depth_parent_capture_review_20260521.log`
- Result: passed and refreshed the screenshots listed below.

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle71_plaza_library_exterior_depth_parent_build_20260521.log`
- Result: passed with `Build Finished, Result: Success.`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle71_plaza_library_exterior_depth_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

Unity licensing note:

- The log includes the usual licensing noise during batchmode startup, but the validation and screenshot capture both completed successfully. No Anemora API token was used.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_exterior_depth_20260521\01_current_plaza_library_exterior_depth_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_exterior_depth_20260521\02_past_plaza_library_exterior_depth_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_exterior_depth_20260521\03_current_plaza_library_exterior_depth_oblique.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_exterior_depth_20260521\04_past_plaza_library_exterior_depth_oblique.png`

## Next Cycle Candidate

- Add the sky/background treatment for the central plaza so the exterior depth reads against a less empty void.
