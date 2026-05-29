# 2026-05-20 Fast VS HD2D House Exterior Hedge Sprites Cycle

## Scope

- User goal: improve HD-2D visual quality quickly while preserving gameplay, map movement, time-window behavior, colliders, route glows, and public/main-branch boundaries.
- External/API assets were allowed for this branch, but this cycle intentionally reused the already committed CC0 OpenGameArt tree sprite instead of fetching any new paid or external asset.
- Cycle target: replace the remaining boxy house-exterior north hedge blocks with smaller sprite-based planting so the house edge reads less like green cubes.
- Paid asset candidates should be reported before adoption; none were adopted in this cycle.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_house_exterior_north_hedge_sprite_a_cc0.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_house_exterior_north_hedge_sprite_b_cc0.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_past_house_exterior_north_hedge_sprite_a_cc0.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_past_house_exterior_north_hedge_sprite_b_cc0.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_hedge_sprites_20260520\`

## Implementation

- Refactored the existing OpenGameArt tree-sprite creation path into a reusable helper.
- Reused the same CC0 tree sprite for:
  - the existing house exterior external tree sprite landmark
  - the four new house-exterior hedge sprite landmarks
- Removed the old cube hedge visuals from the house exterior edge dressing path.
- Parent review rejected the first capture because a separate past-side fence leaf cube still read as a green block. The parent correction removed the house-exterior cube tuft/leaf/fence-flower remnants from the review area and extended validation so they cannot silently return.
- Updated the outdoor edge-dressing validation to check the new sprite names and their material ids.
- Added a dedicated forty-ninth-cycle validation pass to guard the new hedge sprite set, confirm the old cube hedge names stay absent, and keep the route glow pads present.
- Added a forty-ninth-cycle screenshot capture batch for the hedge-sprite exterior review set.

## Verification

Validation command:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle49_hedge_sprites_parent_validate3_20260520.log'
```

Result: passed with `Fast VS house slice validation passed.`

Validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle49_hedge_sprites_parent_validate3_20260520.log`

Capture command:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dFortyNinthCycleScreenshotsBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle49_hedge_sprites_parent_capture3_20260520.log'
```

Result: passed with `Fast VS forty-ninth-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_hedge_sprites_20260520`

Capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle49_hedge_sprites_parent_capture3_20260520.log`

Screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_hedge_sprites_20260520\01_current_house_exterior_hedge_sprites_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_hedge_sprites_20260520\02_current_house_exterior_hedge_sprites_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_hedge_sprites_20260520\03_past_house_exterior_hedge_sprites_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_hedge_sprites_20260520\04_past_house_exterior_hedge_sprites_close.png`

Build result:

- Build log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle49_hedge_sprites_parent_build_20260520.log`
- Result lines: `Fast VS house slice validation passed.` and `Build Finished, Result: Success.`
- EXE: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- EXE size: `667648`
- EXE last write time: `2026-05-20 22:05:16`

Smoke result:

- Smoke log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle49_hedge_sprites_parent_smoke_20260520.log`
- Headless smoke stopped the process after 20 seconds.
- Error-pattern match count: `0`

## Residual Risk

- The reused tree sprite still reads more like a small sapling cluster than a bespoke hedge shrub.
- The black background remains a larger visual gap for later exterior polish; this cycle intentionally stayed scoped to removing blocky house-exterior vegetation.
