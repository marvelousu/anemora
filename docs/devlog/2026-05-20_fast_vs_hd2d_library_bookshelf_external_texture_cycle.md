# 2026-05-20 Fast VS HD2D Library Bookshelf External Texture Cycle

## Scope

- User goal: improve visual quality quickly using API or external assets where useful, while reporting paid assets before adoption.
- Cycle52 replaces the past-library filled bookshelf procedural texture source with a CC0 external pixel bookshelf source. The target is the back wall bookshelf plus the left/right side bookshelves in the past library.
- The current-side empty/ruined shelf texture remains unchanged so the current/past contrast is preserved.
- No paid asset was adopted in this cycle.

## Source

- Source page: https://opengameart.org/content/bookshelf-3
- Direct file: https://opengameart.org/sites/default/files/bookshelf_2.png
- Author: AlejandroHaibi
- License: CC0
- Usage note: the source PNG is tiled into `FastVS_House_bookshelf_front_painted_hd2d.asset` so existing material names and scene validation contracts continue to work.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\External\OpenGameArt\alejandrohaibi_bookshelf_cc0\bookshelf_2.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\External\OpenGameArt\alejandrohaibi_bookshelf_cc0\README.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\External\OpenGameArt\alejandrohaibi_bookshelf_cc0.meta`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_bookshelf_front_painted_hd2d.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-20_fast_vs_hd2d_library_bookshelf_external_texture_cycle.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_bookshelf_external_texture_20260520\`

## Implementation

- Added the OpenGameArt CC0 bookshelf source path and importer validation.
- Changed `BookshelfFrontMaterial(...)` to keep the existing `bookshelf_front_painted_hd2d` material/texture contract while filling the generated repeat texture from the cropped bookshelf portion of `bookshelf_2.png`.
- Muted the external source slightly toward the library wood palette so the new books read as shelves without becoming bright primary-color noise.
- Kept current-side empty shelves on `current_empty_bookshelf_front_hd2d`.
- Added Cycle52 validation for the external source texture, the generated repeat texture, and the three past-library bookshelf panels.
- Added focused screenshots for past back-wall, left side, and right side bookshelves plus a current-side empty-shelf regression frame.

## Verification

Validation command:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle52_bookshelf_parent_validate7_20260520.log'
```

Result: passed with `Fast VS house slice validation passed.`

Capture command:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dFiftySecondCycleScreenshotsBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle52_bookshelf_parent_capture4_20260520.log'
```

Result: passed with `Fast VS fifty-second-cycle screenshots captured`.

Screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_bookshelf_external_texture_20260520\01_past_library_back_wall_external_bookshelf_texture.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_bookshelf_external_texture_20260520\02_past_library_left_side_external_bookshelf_texture.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_bookshelf_external_texture_20260520\03_past_library_right_side_external_bookshelf_texture.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_bookshelf_external_texture_20260520\04_current_library_empty_side_shelf_regression.png`

Build command:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle52_bookshelf_parent_build_20260520.log'
```

Result: passed with validation and `Build Finished, Result: Success.`

Player smoke:

- Command: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe -batchmode -nographics`
- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle52_bookshelf_parent_smoke_20260520.log`
- Result: stopped after 20 seconds, `match_count=0`.

## Residual Risk

- The source is only 64x64, so the shelves are readable but still low-detail when scaled across the large library wall.
- The tiled back-wall panel may need a larger custom or paid library asset later if the library becomes a primary visual showcase.
- External source attribution is preserved locally in `README.md`; no paid or non-CC0 asset was adopted.
