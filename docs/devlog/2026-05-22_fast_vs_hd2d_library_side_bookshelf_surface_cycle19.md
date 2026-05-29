# 2026-05-22 Fast VS HD2D Library Side Bookshelf Surface Cycle 19

## Goal

Add HD-2D surface profile and audit coverage for the existing left/right library side bookshelf texture panels so the current-world empty/ruined shelves and the past-world painted shelves stay visible in metrics and do not silently fall back to generic furniture tokens.

## Full Paths

- Setup: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Metric audit: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs`
- Devlog: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_library_side_bookshelf_surface_cycle19.md`
- Metrics report: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_side_bookshelf_surface_cycle19_20260522\surface_texture_metrics_cycle19_20260522.md`
- Worker validate log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_library_side_bookshelf_surface_cycle19_validate_worker_20260522.log`
- Worker metrics log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_library_side_bookshelf_surface_cycle19_metrics_worker_20260522.log`
- Worker snapshot log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_library_side_bookshelf_surface_cycle19_capture_worker_20260522.log`
- Parent validate log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_library_side_bookshelf_surface_cycle19_validate_parent_20260522.log`
- Parent metrics log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_library_side_bookshelf_surface_cycle19_metrics_parent_20260522.log`
- Parent snapshot log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_library_side_bookshelf_surface_cycle19_capture_parent_20260522.log`

## Implementation

- Added `FastVsHd2dSurfaceProfile` coverage for:
  - `Current_Library_LeftSideBookshelf_EmptyShelfFrontTexturePanel`
  - `Current_Library_RightSideBookshelf_EmptyShelfFrontTexturePanel`
  - `Past_Library_LeftSideBookshelf_BookshelfFrontTexturePanel`
  - `Past_Library_RightSideBookshelf_BookshelfFrontTexturePanel`
- Mapped the new surface IDs to:
  - `Current.Library.Bookshelf.Side.Left`
  - `Current.Library.Bookshelf.Side.Right`
  - `Past.Library.Bookshelf.Side.Left`
  - `Past.Library.Bookshelf.Side.Right`
- Kept the token contract aligned with the existing bookshelf textures:
  - current side panels -> `current_empty_bookshelf_front_hd2d`
  - past side panels -> `bookshelf_front_painted_hd2d`
- Broadened the bookshelf token audit from `*.Library.Bookshelf.Back` to every surface ID containing `*.Library.Bookshelf.` so the side shelf profiles are audited alongside the back shelf profile.
- Added `WriteLibrarySideBookshelfSurfaceCycle19MetricsBatch()` and its menu item so the report is generated from a freshly rebuilt HouseSlice scene.
- Added explicit texture validation calls for the four side shelf panel objects in the back-bookshelf validation batch.

## Validation

- `ValidateHouseSliceBatch`: passed. Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_library_side_bookshelf_surface_cycle19_validate_worker_20260522.log`
- `WriteLibrarySideBookshelfSurfaceCycle19MetricsBatch`: passed. Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_library_side_bookshelf_surface_cycle19_metrics_worker_20260522.log`
- `CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch`: passed. Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_library_side_bookshelf_surface_cycle19_capture_worker_20260522.log`
- Parent `ValidateHouseSliceBatch`: passed. Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_library_side_bookshelf_surface_cycle19_validate_parent_20260522.log`
- Parent `WriteLibrarySideBookshelfSurfaceCycle19MetricsBatch`: passed. Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_library_side_bookshelf_surface_cycle19_metrics_parent_20260522.log`
- Parent `CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch`: passed. Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_library_side_bookshelf_surface_cycle19_capture_parent_20260522.log`
- Report file verified at `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_side_bookshelf_surface_cycle19_20260522\surface_texture_metrics_cycle19_20260522.md`, including the four side-shelf rows:
  - `Current.Library.Bookshelf.Side.Left`
  - `Current.Library.Bookshelf.Side.Right`
  - `Past.Library.Bookshelf.Side.Left`
  - `Past.Library.Bookshelf.Side.Right`

## Expected Visible Effect

- The current library side shelves remain empty, worn, and ruined, but their front texture panels are now tracked as dedicated HD-2D bookshelf surfaces.
- The past library side shelves remain filled/painted and are now tracked with the same dedicated shelf-surface coverage.

## Residual Risk

- The new audit only covers surface profiles, so if a future helper removes the profile component from the panel objects, the token check will stop seeing the shelf even if the mesh still renders correctly.
- The side shelf texture panel checks stay tied to the current object names; renames would need a matching audit update.
- Unity also updated `Assets/Scenes/Anemora_FastVS_HouseSlice.unity` and several unrelated generated assets while rebuilding the scene for the batch tools; those were left in place for parent review.
