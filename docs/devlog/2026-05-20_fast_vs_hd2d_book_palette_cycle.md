# 2026-05-20 Fast VS HD2D Book Palette Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Public baseline not touched: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample`
- Main implementation file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Generated textures:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_book_spines_hd2d_plate.asset`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_bookshelf_front_painted_hd2d.asset`

This cycle focuses on the library bookshelf/book palette. The goal was to reduce the toy-like rainbow read and make the past library shelves look more like old-library book rows while preserving the existing layout, story flow, Time Window behavior, and UI.

## Planning / Worker Cycle

- Parent planned a small texture-generation pass after the previous screenshots showed that the past library shelves were still too saturated and barcode-like.
- Worker: `019e41ad-3060-77d3-8f24-60861b993611` (`Boole`, gpt-5.4-mini).
- Worker first muted the palette and added `ValidateFastVsHd2dEighthCycleBookPalette()` plus `CaptureHd2dEighthCycleScreenshotsBatch()`.
- Parent review found the first result still too close to thin rainbow stripes.
- Worker review-fix changed the book spines from fixed 8 px columns to deterministic variable-width runs, reduced the bookshelf-front palette to fewer muted old-library colors, and added width-variation validation.

## Implementation

Updated procedural bookshelf/book rendering in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- `SampleBookShelfTexturePixel(...)` now resolves each pixel against variable-width book runs instead of uniform columns.
- `PickBookSpineColor(...)` now uses a muted palette centered on oxblood, aged leather, faded navy, dull ochre, parchment, dusty violet-brown, and muted green.
- Book readability is now carried more by edge shadows, center paper/gold accents, and top/bottom lines rather than bright primary colors.
- `BookshelfFrontMaterial(...)` applies a slightly warm base tint for the painted bookshelf-front panels.
- `ValidateFastVsHd2dEighthCycleBookPalette()` checks generated texture presence, exact texture sizes, material texture wiring, palette saturation/value limits, and spine-width variation.
- `CaptureHd2dEighthCycleScreenshotsBatch()` captures the standard review set to the book-palette evidence directory.

Updated material tint outputs:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_bookshelf_front_painted_hd2d_Past_Library_BackWallBookshelfFrontTexturePanel.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_bookshelf_front_painted_hd2d_Past_Library_LeftSideBookshelf_BookshelfFrontTexturePanel.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_bookshelf_front_painted_hd2d_Past_Library_RightSideBookshelf_BookshelfFrontTexturePanel.mat`

## Screenshot Evidence

Output directory:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_book_palette_20260520`

Captured files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_book_palette_20260520\01_interior_niro_shadow.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_book_palette_20260520\02_exterior_niro_shadow.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_book_palette_20260520\03_library_reto_desk.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_book_palette_20260520\04_library_reto_talk_loop.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_book_palette_20260520\05_library_past_no_temp_people.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_book_palette_20260520\06_library_dialogue_tmp_font.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_book_palette_20260520\07_plaza_library_facade_current.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_book_palette_20260520\08_plaza_library_facade_past.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_book_palette_20260520\09_library_timewriter_pocket_glow.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_book_palette_20260520\10_library_current_yellow_timewindow_cues.png`

## Verification

Worker validation:

- First worker log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle8_worker_validate_20260520.log`
- Review-fix worker log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle8_worker_validate_reviewfix_20260520.log`
- Result: passed.

Parent validation:

- First parent log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle8_validate_parent_20260520.log`
- Review-fix parent log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle8_validate_parent_reviewfix_20260520.log`
- Result: passed.

Screenshot capture:

- First capture retry log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle8_capture_retry1_20260520.log`
- Final review-fix capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle8_capture_reviewfix_retry1_20260520.log`
- The non-retry capture logs stopped because a previous Unity batch process still held the project lock; retries succeeded after the process exited.

Build:

- Command log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle8_build_20260520.log`
- Result: success.
- Key lines: `Fast VS house slice validation passed.`, `Build Finished, Result: Success.`, and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Build output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Player smoke:

- Player log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle8_player_smoke_20260520.log`
- Result: launched for 20 seconds, then was intentionally stopped.
- Checked patterns: `Error`, `Exception`, `NullReference`, `MissingReference`, `Failed`, `Crash`, `Font Atlas Texture`, `LiberationSans`, `ScreenSpaceAmbientOcclusion`, `DrawObjectsPass`, `RenderGraph`.
- Match count: 0.

## Visual Review

- The past library bookshelf is less primary-color-heavy than the previous cycle.
- Variable book widths reduce the uniform stripe/barcode read.
- The shelves still remain stylized and procedural. A later authored/AI-generated atlas may improve this further, but this cycle removes the most obvious saturation problem without importing a new dependency.

## Boundaries

- Live Unity MCP was not available in this Codex session, so verification used Unity batch-mode editor execution, screenshot capture, build, and player smoke.
- Meshy/API and paid external assets were not used in this cycle. The change was limited to existing procedural textures because the immediate issue was palette/readability and could be addressed without adding licensing or import overhead.
- Unity-generated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\ProjectSettings`, `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\AddressableAssetsData`, and `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Resources` side effects were restored or removed before commit selection.
