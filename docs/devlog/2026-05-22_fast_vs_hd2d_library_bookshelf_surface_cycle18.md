# 2026-05-22 Fast VS HD2D Library Bookshelf Surface Cycle 18

## Goal

Move the profile-bearing library back bookshelf surfaces off generic furniture materials and onto the dedicated bookshelf textures, while keeping the current-world shelf empty/ruined rather than book-filled.

## Full Paths

- Setup: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Metric audit: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs`
- Devlog: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_library_bookshelf_surface_cycle18.md`
- Metrics report: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_bookshelf_surface_cycle18_20260522\surface_texture_metrics_cycle18_20260522.md`
- Worker validate log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_library_bookshelf_surface_cycle18_validate_worker_20260522.log`
- Worker metrics log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_library_bookshelf_surface_cycle18_metrics_worker_20260522.log`
- Worker snapshot log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_library_bookshelf_surface_cycle18_capture_worker_20260522.log`
- Parent validate log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_library_bookshelf_surface_cycle18_validate_parent_20260522.log`
- Parent metrics log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_library_bookshelf_surface_cycle18_metrics_parent_20260522.log`
- Parent snapshot log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_library_bookshelf_surface_cycle18_capture_parent_20260522.log`

## Implementation

- Changed `CreateLibrary` so `*_Library_BackWallShelfWide` uses `BookshelfFrontMaterial(...)` in the past world and `CurrentEmptyBookshelfFrontMaterial(...)` in the current world.
- Updated the attached surface profile token for `Past.Library.Bookshelf.Back` and `Current.Library.Bookshelf.Back` to:
  - `bookshelf_front_painted_hd2d`
  - `current_empty_bookshelf_front_hd2d`
- Added a bookshelf-specific metric audit check that rejects generic furniture token fallbacks and requires the canonical bookshelf token for both current and past `*.Library.Bookshelf.Back` profiles.
- Added `WriteLibraryBookshelfSurfaceCycle18MetricsBatch()` and made it rebuild the HouseSlice scene before collecting metrics.
- Extended house-slice validation to check the back-wall shelf materials directly.
- Parent review narrowed the token audit to `*.Library.Bookshelf.Back` so future side-shelf profiles can use their own dedicated texture contracts without inheriting this back-shelf-only assertion.

## Validation

- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_library_bookshelf_surface_cycle18_validate_parent_20260522.log`
- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dSurfaceTextureMetricAudit.WriteLibraryBookshelfSurfaceCycle18MetricsBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_library_bookshelf_surface_cycle18_metrics_parent_20260522.log`
- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_library_bookshelf_surface_cycle18_capture_parent_20260522.log`
- Parent results:
  - `ValidateHouseSliceBatch`: passed.
  - `WriteLibraryBookshelfSurfaceCycle18MetricsBatch`: passed.
  - `CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch`: passed.

## Expected Visible Effect

- The current library back shelf stays empty and ruined, but its surface now reads as a dedicated empty bookshelf material instead of generic furniture.
- The past library back shelf reads as a dedicated bookshelf surface rather than a generic furniture block.

## Residual Risk

- The audit normalizes material names by the current `FastVS_House_` naming pattern. If helper naming changes, the token resolver may need a small update.
- The report covers the whole house slice, so unrelated material drift could still surface in the same table even though the bookshelf-specific checks are focused.
