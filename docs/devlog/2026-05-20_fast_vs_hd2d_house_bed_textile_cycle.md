# 2026-05-20 Fast VS HD2D House Bed Textile Cycle

## Purpose

Cycle 26 improves the Niro-house bed so it reads as a wood-framed bed with cloth bedding rather than a stone-like block. The work stays local to the house interior and does not touch characters, Time Window behavior, dialogue, route transitions, or public `main`.

## Implementation

Updated:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_bed.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_past_bed.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_current_bed_hd2d_plate.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_past_bed_hd2d_plate.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_pillow_hd2d_plate.asset`

Added generated scene objects:

- `Current_NiroBed_PaperPixelBed_QuiltLongSeamA`
- `Current_NiroBed_PaperPixelBed_QuiltLongSeamB`
- `Current_NiroBed_PaperPixelBed_FootFoldSoft`
- `Current_NiroBed_PillowPixel_TopCrease`
- `Past_NiroBed_PaperPixelBed_QuiltLongSeamA`
- `Past_NiroBed_PaperPixelBed_QuiltLongSeamB`
- `Past_NiroBed_PaperPixelBed_FootFoldSoft`
- `Past_NiroBed_PillowPixel_TopCrease`

Implementation notes:

- The bed base now uses the wood/furniture material while the blanket keeps the bed textile material.
- Current-side bedding uses a muted blue-green unlit textile so the cloth color survives the desaturated current-side lighting.
- Past-side bedding keeps a cleaner pale blue/white textile.
- Bed textile tiling was reduced from `2x2` to `1x1` to avoid small repeating blocks that read as stone.
- The cycle also fixes the bed screenshot framing so the evidence image actually focuses on the bed.

## Verification

- Parent validate log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle26_parent_validate_final_20260520.log`
- Parent validate result: passed with `Fast VS house slice validation passed.`
- Final capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle26_parent_capture_final2_20260520.log`
- Final capture result: passed and wrote 2 PNGs.
- Build log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle26_build_20260520.log`
- Build result: passed with `Build Finished, Result: Success.`
- Player smoke log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle26_player_smoke_20260520.log`
- Player smoke result: 20-second headless run stopped intentionally, `match_count=0`.

Captured screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_bed_textile_20260520\01_current_house_bed_textile.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_bed_textile_20260520\02_past_house_bed_textile.png`

## Known Constraints

- The current-side pillow and room still inherit the intentionally desaturated current-world treatment.
- This cycle only addresses Niro-house bed textiles. Other interior props remain separate polish targets.
