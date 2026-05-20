# 2026-05-20 Fast VS HD2D Central Plaza Tree Line Sprites Cycle

## Scope

- User goal: improve HD-2D visual quality quickly while preserving gameplay, map movement, time-window behavior, colliders, route glows, triggers, and the existing time-window presentation.
- External/API assets were allowed for the branch, but this cycle intentionally reused the already committed CC0 OpenGameArt tree sprite instead of fetching any new external or paid asset.
- Paid assets were not adopted in this cycle.
- Cycle target: replace the remaining blocky Central Plaza north tree-line cubes with sprite-based tree-line landmarks using the existing CC0 tree sprite.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_central_plaza_north_tree_line_sprite_a_cc0.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_central_plaza_north_tree_line_sprite_a_cc0.mat.meta`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_central_plaza_north_tree_line_sprite_b_cc0.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_central_plaza_north_tree_line_sprite_b_cc0.mat.meta`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_past_central_plaza_north_tree_line_sprite_a_cc0.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_past_central_plaza_north_tree_line_sprite_a_cc0.mat.meta`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_past_central_plaza_north_tree_line_sprite_b_cc0.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_past_central_plaza_north_tree_line_sprite_b_cc0.mat.meta`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-20_fast_vs_hd2d_central_plaza_tree_line_sprites_cycle.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_central_plaza_tree_line_sprites_20260520\`

## Implementation

- Renamed the reusable external tree-sprite helper so it no longer reads as house-specific.
- Replaced the Central Plaza north tree-line cube dressing with four CC0 sprite landmarks using the already committed OpenGameArt tree sprite.
- Kept WestLowWall, EastLowWall, map movement, route glows, triggers, and time-window behavior intact.
- Added dedicated validation for the new Central Plaza sprite set and explicitly asserted that the old cube names stay absent.
- Added a fiftieth-cycle screenshot batch for current/past overview and close views of the Central Plaza tree line.
- Parent review adjusted the capture framing after the first worker capture focused on the wall/door area rather than the tree sprite itself.

## Verification

Validation command:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle50_plaza_tree_line_parent_validate_20260520.log'
```

Result: passed with `Fast VS house slice validation passed.`

Validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle50_plaza_tree_line_parent_validate_20260520.log`

Capture command:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dFiftiethCycleScreenshotsBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle50_plaza_tree_line_parent_capture_20260520.log'
```

Result: passed with `Fast VS fiftieth-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_central_plaza_tree_line_sprites_20260520`

Capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle50_plaza_tree_line_parent_capture_20260520.log`

Screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_central_plaza_tree_line_sprites_20260520\01_current_central_plaza_tree_line_sprites_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_central_plaza_tree_line_sprites_20260520\02_current_central_plaza_tree_line_sprites_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_central_plaza_tree_line_sprites_20260520\03_past_central_plaza_tree_line_sprites_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_central_plaza_tree_line_sprites_20260520\04_past_central_plaza_tree_line_sprites_close.png`

Build result:

- Build log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle50_plaza_tree_line_parent_build_20260520.log`
- Result lines: `Fast VS house slice validation passed.` and `Build Finished, Result: Success.`
- EXE: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- EXE size: `667648`
- EXE last write time: `2026-05-20 22:23:49`

Smoke result:

- Smoke log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle50_plaza_tree_line_parent_smoke_20260520.log`
- Headless smoke stopped the process after 20 seconds.
- Error-pattern match count: `0`

## Residual Risk

- The same tree sprite is reused across all four placements, so repetition remains.
- The black void behind the map and the flat plaza ground are now more visible relative to the better tree sprite and remain good candidates for later polish.
