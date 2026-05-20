# 2026-05-21 Fast VS HD2D Library Reading Table Grounding Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_reading_table_grounding_20260521`

This cycle adds a narrow library reading-table grounding pass for both timelines. The goal was to make the existing tables, books, and Reto desk read as attached to the floor/table plane without changing table scale, placement, story flow, interactions, Time Window behavior, UI, fonts, character logic, or colliders.

No external, paid, or API-generated assets were used. The pass uses existing Fast VS materials and small code-generated non-colliding landmark cubes only.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateLibraryReadingTableGroundingPolish(...)` and wired it into `CreateLibrary(...)` after the readable microprop pass.
- Added subtle current-side grounding around Reto's desk and side table area:
  - `Current_Library_ReadingTableGrounding_RetoDeskFloorShadowA`
  - `Current_Library_ReadingTableGrounding_RetoBookContactA`
  - `Current_Library_ReadingTableGrounding_CurrentSideTableEdgeChipA`
  - `Current_Library_ReadingTableGrounding_CurrentSideTableDustStripA`
- Added past-side grounding for two clean reading tables and their books:
  - `Past_Library_ReadingTableGrounding_LeftTableFloorShadowA`
  - `Past_Library_ReadingTableGrounding_RightTableFloorShadowA`
  - `Past_Library_ReadingTableGrounding_LeftTableEdgeAccentA`
  - `Past_Library_ReadingTableGrounding_RightTableEdgeAccentA`
  - `Past_Library_ReadingTableGrounding_LeftBookContactA`
  - `Past_Library_ReadingTableGrounding_RightBookContactA`
- Kept all new objects non-colliding, `TimeWindowPairedSpaceLandmarkKind.PropOrFeature`, and `countsForArrival = false`.
- Added `ValidateFastVsHd2dSixtySixthCycleLibraryReadingTableGrounding()` and `ValidateLibraryReadingTableGroundingObject(...)`.
- Added `CaptureHd2dSixtySixthCycleScreenshotsBatch()` and `CaptureHd2dSixtySixthCycleScreenshotsToDirectory(...)`.

## Validation

Validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle66_library_reading_table_grounding_worker_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle66_library_reading_table_grounding_worker_capture_20260521.log`
- Result: passed and generated the requested screenshot set.

Parent review:

- Reviewed all four worker screenshots.
- `04_past_library_table_book_contact_close.png` initially included too much Niro/yellow marker presence, then too much existing table-front dark edge. Adjusted only the Cycle66 review camera coordinates so the close shot focuses more directly on the past table book/contact detail.
- The remaining dark table edge is from the existing table structure rather than a new long shadow prop.
- No gameplay, story, Time Window, character, font, UI, map-transition, collider, or map-size behavior was changed during the parent fix.

Parent screenshot capture logs:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle66_library_reading_table_grounding_parent_capture_r1_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle66_library_reading_table_grounding_parent_capture_r2_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle66_library_reading_table_grounding_parent_capture_r3_20260521.log`
- Result: final `r3` capture passed and regenerated the screenshots listed below.

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle66_library_reading_table_grounding_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle66_library_reading_table_grounding_parent_build_20260521.log`
- Result: passed with `Fast VS house slice validation passed.` and rebuilt `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`.

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle66_library_reading_table_grounding_parent_smoke_20260521.log`
- Result: passed a 20 second batchmode startup smoke run. The process was intentionally stopped after the smoke window.

Unity licensing note:

- The logs still contain `[Licensing::Module] Error: Access token is unavailable; failed to update`. That is Unity licensing noise and did not block validation, capture, build, or smoke. It is not an Anemora API-token requirement.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_reading_table_grounding_20260521\01_current_library_reto_desk_grounding_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_reading_table_grounding_20260521\02_past_library_reading_tables_grounding_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_reading_table_grounding_20260521\03_current_library_reto_book_contact_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_reading_table_grounding_20260521\04_past_library_table_book_contact_close.png`

## External Assets

No external, paid, or API-generated assets were used.

## Residual Risk

- The new pieces are intentionally subtle, so the improvement is mostly grounding/readability rather than a larger authored prop pass.
- The capture frames still include surrounding library context; the requested contact details are present, but a later pass could tune composition more aggressively if needed.
