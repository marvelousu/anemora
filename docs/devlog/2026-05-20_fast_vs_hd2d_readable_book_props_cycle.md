# 2026-05-20 Fast VS HD2D Readable Book Props Cycle

## Purpose

Improve readability for the small book props that matter in Fast VS HD-2D screenshots and gameplay:
house Timewriter book, Reto desk books, past target book, and past reading table books.

## Implementation

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Extended `CreateReadableBookProp(...)` with low-profile visual child cubes for readable book detail.
- Added these child names to every readable book root:
  - `_CoverBorderTop`
  - `_CoverBorderBottom`
  - `_CoverForeEdgeHighlight`
  - `_PageGutterLine`
  - `_PageEdgeLineA`
  - `_BookmarkSlip`
- Added open-page detail children for open books:
  - `_OpenPageCenterFold`
  - `_OpenPageCornerHighlight`
- Kept the new detail objects collider-free and visual-only.
- Added `ValidateFastVsHd2dThirtyEighthCycleReadableBookProps()` and wired it into `ValidateHouseSliceBatch()`.
- Added `CaptureHd2dThirtyEighthCycleScreenshotsBatch()` and `CaptureHd2dThirtyEighthCycleScreenshotsToDirectory(...)`.

## Verification

Validation command:

`& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle38_worker_validate_20260520.log'`

Result: passed.

Parent result: passed.

Capture command:

`& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dThirtyEighthCycleScreenshotsBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle38_worker_capture_20260520.log'`

Result: passed.

Parent result: passed.

Parent build: `BuildAndValidateBatch` passed.

Parent EXE smoke: 20 second batch launch completed with `match_count=0`.

## Screenshot Evidence

Output directory:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_readable_book_props_20260520`

Captured files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_readable_book_props_20260520\01_house_timewriter_book_detail.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_readable_book_props_20260520\02_library_reto_desk_books_detail.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_readable_book_props_20260520\03_past_target_book_detail.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_readable_book_props_20260520\04_past_reading_table_books_detail.png`

## Assets

- No external assets were used.
- No paid assets were used.
