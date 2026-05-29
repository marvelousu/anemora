# 2026-05-20 Fast VS HD2D Facade Detail Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Public baseline not touched: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample`
- Main implementation file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

This cycle adds small architectural facade details to the house exterior and central-plaza library exterior. It avoids sky/background replacements, global darkening, story changes, UI changes, Time Window changes, and route/collider changes.

## Planning / Worker Cycle

- Parent created the detailed implementation instruction for gpt-5.4-mini worker `019e41ad-3060-77d3-8f24-60861b993611`.
- The worker did not produce a completed response or usable diff during the wait window and was shut down to avoid late conflicting edits.
- Parent implemented the same scoped plan directly and verified it in batch mode.

## Implementation

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CaptureHd2dTwelfthCycleScreenshotsBatch()`.
- Added `CreateExteriorFacadeDepthDetails(...)` for non-colliding house facade pieces.
- Added `CreateCentralPlazaLibraryFacadeDepthDetails(...)` for non-colliding library facade pieces.
- Added `ValidateFastVsHd2dTwelfthCycleFacadeDetails()` and wired it into `ValidateHouseSliceBatch()`.
- Added `ValidateFacadeDetailObject(...)` for object/material/collider/landmark checks.

Representative added objects:

- `Current_HouseExterior_FacadeDetail_LeftCornerPost`
- `Past_HouseExterior_FacadeDetail_LeftCornerPost`
- `Current_HouseExterior_FacadeDetail_EaveBraceLeft`
- `Past_HouseExterior_FacadeDetail_EaveBraceLeft`
- `Current_HouseExterior_FacadeDetail_LeftWindowStoneSill`
- `Past_HouseExterior_FacadeDetail_LeftWindowStoneSill`
- `Current_CentralPlaza_LibraryFacadeDetail_LeftPilaster`
- `Past_CentralPlaza_LibraryFacadeDetail_LeftPilaster`
- `Current_CentralPlaza_LibraryFacadeDetail_EntranceCanopyLip`
- `Past_CentralPlaza_LibraryFacadeDetail_EntranceCanopyLip`
- `Current_CentralPlaza_LibraryFacadeDetail_LeftWindowStoneSill`
- `Past_CentralPlaza_LibraryFacadeDetail_LeftWindowStoneSill`

## Screenshot Evidence

Output directory:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_facade_detail_20260520`

Captured files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_facade_detail_20260520\01_interior_niro_shadow.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_facade_detail_20260520\02_exterior_niro_shadow.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_facade_detail_20260520\03_library_reto_desk.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_facade_detail_20260520\04_library_reto_talk_loop.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_facade_detail_20260520\05_library_past_no_temp_people.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_facade_detail_20260520\06_library_dialogue_tmp_font.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_facade_detail_20260520\07_plaza_library_facade_current.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_facade_detail_20260520\08_plaza_library_facade_past.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_facade_detail_20260520\09_library_timewriter_pocket_glow.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_facade_detail_20260520\10_library_current_yellow_timewindow_cues.png`

## Verification

- Parent validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle12_validate_parent_20260520.log`
- Result: passed.
- Parent screenshot capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle12_capture_parent_20260520.log`
- Result: passed.
- Build log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle12_build_20260520.log`
- Result: success.
- Build output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Player smoke log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle12_player_smoke_20260520.log`
- Result: launched for 20 seconds and was intentionally stopped; checked error-pattern match count was 0.

## Notes

- Meshy/API and paid external assets were not used in this cycle.
- Paid assets remain better suited to a larger consistent-pack replacement pass rather than a small facade trim pass.
- The validation explicitly re-checks the library door panel textures and library route glow pads so facade details do not regress the previous door/transition work.
