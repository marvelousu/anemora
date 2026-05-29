# 2026-05-20 Fast VS HD2D Current Library Side Shelf Visibility Cycle

## Scope

Cycle20 raises the readability of the current-world library side shelves without changing route, camera, collider, animation, or dialogue behavior.

The specific issue from cycle19 was that the current-side empty shelves still read too dark in close review, especially on the left shelf, where the front face could collapse toward a black board.

## Changes

- Added a new procedural HD2D texture sample in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:
  - `SampleCurrentEmptyBookshelfFrontHd2dPixel(int x, int y)`
  - texture id: `current_empty_bookshelf_front_hd2d`
- Added a new material helper:
  - `CurrentEmptyBookshelfFrontMaterial(string panelId, Vector2 textureScale)`
  - material name token: `FastVS_House_current_empty_bookshelf_front_hd2d_{panelId}`
- Added a new panel helper:
  - `CreateCurrentEmptyShelfTexturePanel(...)`
  - current-side object names:
    - `Current_Library_LeftSideBookshelf_EmptyShelfFrontTexturePanel`
    - `Current_Library_RightSideBookshelf_EmptyShelfFrontTexturePanel`
- Inserted the new empty shelf front texture panel into `CreateCurrentLibraryEmptySideBookshelf(...)` before the dust/broken/detail layer.
- Added `ValidateFastVsHd2dTwentiethCycleCurrentLibrarySideShelfVisibility()` and wired it into `ValidateHouseSliceBatch()`.
- Added `CaptureHd2dTwentiethCycleScreenshotsBatch()` for the new visibility review set.

The empty-shelf texture keeps the current-world palette in gray-brown wood tones, but it now has:

- brighter shelf edges,
- visible shelf board divisions,
- subtle recess shading,
- dust and chip noise,
- a few sparse paper/residual-book color breaks.

It stays empty in composition. It does not switch the current side into the book-filled texture panel used by the past side.

## External Assets

No Meshy, API, or external asset was used.

Reason: this was a narrow visibility fix on an existing procedural pipeline. A local generated texture panel was faster, safer, and more consistent with the current HD2D material set than pulling in a new bookshelf asset.

## Verification

- Worker validate log:
  `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle20_worker_validate_20260520.log`
- Worker capture log:
  `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle20_worker_capture_20260520.log`
- Parent validate log:
  `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle20_parent_validate_20260520.log`
- Parent build log:
  `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle20_build_retry_20260520.log`
- Parent player smoke log:
  `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle20_player_smoke_20260520.log`

Result: validation passed, screenshot capture completed successfully, build finished with `Result: Success`, and player smoke returned `match_count=0`.

Note: the first parent build attempt hit Unity's transient project-lock guard after validation. A retry after the editor process fully exited succeeded. Unity batchmode also emitted the usual licensing/access-token and `LogAssemblyErrors (0ms)` lines; these did not block validation, build, or smoke.

## Evidence

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_side_shelf_visibility_20260520\01_current_left_empty_shelf_visibility.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_side_shelf_visibility_20260520\02_current_right_empty_shelf_visibility.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_side_shelf_visibility_20260520\03_past_left_full_shelf_reference.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_side_shelf_visibility_20260520\04_past_right_full_shelf_reference.png`
