# 2026-05-20 Fast VS HD2D Plaza Library Approach Cycle

## Purpose

Improve the central plaza approach to the library so the facade and door read as intentional HD-2D set dressing. This cycle stays visual-only. It does not change story, dialogue, font, controls, Time Window behavior, map transition behavior, route trigger positions, or map move glow pad behavior.

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Scene builder: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Devlog: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-20_fast_vs_hd2d_plaza_library_approach_cycle.md`
- Index: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- Screenshot output directory: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_approach_20260520`

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-20_fast_vs_hd2d_plaza_library_approach_cycle.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

## Implementation Summary

- Added `CreateCentralPlazaLibraryApproachHd2dPolish(Transform root, string prefix, bool past, Materials materials, Vector3 c, Material stone, Material trim, Material path)`.
- Called it from `CreateCentralPlaza(...)` after the plaza library facade landmark polish pass and before the stone-square border pieces.
- Added thin shared approach details under `Current_CentralPlazaMap_SeparateSpace` and `Past_CentralPlazaMap_SeparateSpace`:
  - `Current_CentralPlaza_LibraryApproach_StepLowerVisual`
  - `Current_CentralPlaza_LibraryApproach_StepUpperLeftVisual`
  - `Current_CentralPlaza_LibraryApproach_StepUpperRightVisual`
  - `Current_CentralPlaza_LibraryApproach_SideCurbWestVisual`
  - `Current_CentralPlaza_LibraryApproach_SideCurbEastVisual`
  - `Current_CentralPlaza_LibraryApproach_ThresholdShadowBand`
  - `Past_CentralPlaza_LibraryApproach_StepLowerVisual`
  - `Past_CentralPlaza_LibraryApproach_StepUpperLeftVisual`
  - `Past_CentralPlaza_LibraryApproach_StepUpperRightVisual`
  - `Past_CentralPlaza_LibraryApproach_SideCurbWestVisual`
  - `Past_CentralPlaza_LibraryApproach_SideCurbEastVisual`
  - `Past_CentralPlaza_LibraryApproach_ThresholdShadowBand`
- Added current-only decay details:
  - `Current_CentralPlaza_LibraryApproach_BrokenPaverA`
  - `Current_CentralPlaza_LibraryApproach_DustScuffA`
  - `Current_CentralPlaza_LibraryApproach_CurbChipA`
- Added past-only maintained details:
  - `Past_CentralPlaza_LibraryApproach_CleanTileInsetA`
  - `Past_CentralPlaza_LibraryApproach_LampBaseWestA`
  - `Past_CentralPlaza_LibraryApproach_LampBaseEastA`
- Added `ValidateFastVsHd2dFortyFourthCyclePlazaLibraryApproach()` and wired it into `ValidateHouseSliceBatch()`.
- Added `CaptureHd2dFortyFourthCycleScreenshotsBatch()` and its private directory helper.
- Kept the new pieces thin, non-colliding, collider-free, and limited to the existing procedural material set.

## Validation Commands

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle44_worker_validate_20260520.log'
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dFortyFourthCycleScreenshotsBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle44_worker_capture_20260520.log'
```

## Validation Result

- Validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle44_worker_validate_20260520.log`
- Screenshot log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle44_worker_capture_20260520.log`
- Result: passed.

Parent review validation:

- Unity validation: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle44_parent_validate_20260520.log`
  - Result: passed. The log contains `Fast VS house slice validation passed.`
- Screenshot capture: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle44_parent_capture_20260520.log`
  - Result: passed. The log contains `Fast VS forty-fourth-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_approach_20260520`.
- Build: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle44_parent_build_20260520.log`
  - Result: passed. The log contains `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`.
- Player smoke: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle44_parent_smoke_20260520.log`
  - Result: passed. The runtime log scan returned `match_count=0` for error/exception/missing-reference patterns.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_approach_20260520\01_current_plaza_library_approach_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_approach_20260520\02_past_plaza_library_approach_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_approach_20260520\03_current_plaza_library_approach_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_approach_20260520\04_past_plaza_library_approach_close.png`

## Notes

- External asset note: none used.
- The approach detail is intentionally low-profile so it does not cover the orange/cyan route glow at the library door.
- Current-time detail leans broken and dusty.
- Past-time detail leans clean and maintained.

## Risks and Next Checks

- The close review framing could still benefit from another camera pass if the parent review wants the glow pad more centered.
- The new shared steps and curbs are thin by design, so the next review should confirm they read clearly at gameplay distance and do not visually merge into the facade.
