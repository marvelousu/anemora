# 2026-05-21 Fast VS HD2D Library Readable Microprops Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_readable_microprops_20260521`

This cycle adds a restrained readable-microprop pass for the library interior. The intent is to make books, desk papers, and shelf ledgers read more clearly in the VS library without changing story flow, dialogue, Time Window logic, transitions, controls, font handling, character animation, or collision layout.

No external, paid, or API-generated assets were used. The pass uses existing Fast VS materials and code-generated non-colliding prop cubes.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateLibraryReadableMicropropPolish(...)` and wired it into `CreateLibrary(...)`.
- Added small current-side readable objects around Reto's desk, the floor book area, and side shelves:
  - `Current_Library_ReadableMicroprops_RetoDeskPaperLineA`
  - `Current_Library_ReadableMicroprops_RetoDeskBookmarkA`
  - `Current_Library_ReadableMicroprops_FloorOpenBookA`
  - `Current_Library_ReadableMicroprops_LeftShelfLooseBookA`
  - `Current_Library_ReadableMicroprops_RightShelfLooseBookA`
- Added small past-side readable objects on clean reading tables and side shelves:
  - `Past_Library_ReadableMicroprops_TableOpenBook_LeftFront`
  - `Past_Library_ReadableMicroprops_TableClosedBook_CenterRear`
  - `Past_Library_ReadableMicroprops_LeftShelfLedgerA`
  - `Past_Library_ReadableMicroprops_RightShelfLedgerA`
- Added `SetLandmarksCountForArrival(...)` so nested visual book-detail landmarks in this cycle do not count as Time Window arrival targets.
- Added `ValidateFastVsHd2dSixtiethCycleLibraryReadableMicroprops()`, `ValidateLibraryReadableMicropropObject(...)`, and `ValidateLandmarkChildrenDoNotCountForArrival(...)`.
- Added `CaptureHd2dSixtiethCycleScreenshotsBatch()` and `CaptureHd2dSixtiethCycleScreenshotsToDirectory(...)`.
- Parent review corrected the worker pass by changing past table-book base materials from furniture material to `materials.Book`, suppressing the existing past target/Aria marker only during the side-shelf review screenshot, and validating nested detail landmarks as non-arrival.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-21_fast_vs_hd2d_library_readable_microprops_cycle.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_readable_microprops_20260521\`

## Validation

Worker validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle60_library_microprops_worker_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle60_library_microprops_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Screenshot capture logs:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle60_library_microprops_worker_capture_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle60_library_microprops_parent_capture_r2_20260521.log`
- Result: captured the 4 requested screenshots. Parent review confirmed the large yellow marker in the side-shelf screenshot was an existing guidance marker and removed it from the review capture only.

Build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle60_library_microprops_parent_build_20260521.log`
- Result: build succeeded and produced `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`.

Smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle60_library_microprops_parent_smoke_20260521.log`
- Result: 20 second headless player smoke was stopped intentionally after launch; error-pattern match count was `0`.

Unity licensing note:

- Unity logs still include `[Licensing::Module] Error: Access token is unavailable; failed to update`. It did not fail validation, capture, build, or smoke, and it is not an Anemora API-token requirement.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_readable_microprops_20260521\01_current_library_reto_desk_microprops.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_readable_microprops_20260521\02_current_library_floor_book_microprops.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_readable_microprops_20260521\03_past_library_table_microprops.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_readable_microprops_20260521\04_past_library_side_shelf_microprops.png`

## External Assets

No external, paid, or API-generated assets were used.

## Residual Risk

- These are small material-driven props, so the improvement is incremental rather than a full authored library prop pass.
- The current floor-book review image still includes surrounding furniture and player framing; the object is valid, but future capture passes may use a cleaner camera angle for easier visual review.
- A later high-quality pass may still benefit from curated authored book/table assets, but this cycle did not require API tokens or paid assets.
