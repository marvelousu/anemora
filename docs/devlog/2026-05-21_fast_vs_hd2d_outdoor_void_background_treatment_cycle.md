# 2026-05-21 Fast VS HD2D Outdoor Void Background Treatment Cycle

## Scope

- Cycle58: outdoor void background treatment for house exterior and central plaza.
- Goal: reduce the harsh black void at the far north / outer edges with low-key distant silhouettes and dark atmospheric slabs.
- No dialogue, story flow, Time Window behavior, font assets, map transitions, input handling, or collision logic were changed.
- No external, paid, or downloaded assets were used. The pass uses code-generated transparent slabs and existing materials only.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateOutdoorVoidBackgroundTreatment(...)` and `CreateOutdoorVoidBackgroundSlab(...)`.
- Added `EnsureOutdoorVoidBackgroundMaterial(...)` for low-alpha transparent unlit materials.
- Added current/past outdoor background treatment objects for both house exterior and central plaza:
  - `Current_HouseExterior_OutdoorVoidBackground_NorthSilhouetteLeft`
  - `Current_HouseExterior_OutdoorVoidBackground_NorthSilhouetteCenter`
  - `Current_HouseExterior_OutdoorVoidBackground_NorthSilhouetteRight`
  - `Current_HouseExterior_OutdoorVoidBackground_WestEdgeWash`
  - `Current_HouseExterior_OutdoorVoidBackground_EastEdgeWash`
  - `Past_HouseExterior_OutdoorVoidBackground_*` equivalents
  - `Current_CentralPlaza_OutdoorVoidBackground_*` equivalents
  - `Past_CentralPlaza_OutdoorVoidBackground_*` equivalents
- Wired the new treatment into the end of `CreateExterior(...)` and `CreateCentralPlaza(...)`.
- Added `ValidateFastVsHd2dFiftyEighthCycleOutdoorVoidBackgroundTreatment()` and `ValidateOutdoorVoidBackgroundTreatmentObject(...)`, and wired the validation into `ValidateHouseSliceBatch()`.
- Added `CaptureHd2dFiftyEighthCycleScreenshotsBatch()` and `CaptureHd2dFiftyEighthCycleScreenshotsToDirectory(...)`.
- Adjusted the house exterior capture angle so the overview reads as a subtle distant treatment instead of a giant wall.
- Parent review lowered all background slabs and reduced their alpha after the first screenshot review, because the house exterior view still showed floating dark rectangular panels against the void.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-21_fast_vs_hd2d_outdoor_void_background_treatment_cycle.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_void_background_treatment_20260521\`

## Validation

Validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle58_outdoor_void_background_worker_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle58_outdoor_void_background_worker_capture_20260521.log`
- Result: captured the 4 requested screenshots.

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle58_outdoor_void_background_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle58_outdoor_void_background_parent_capture_20260521.log`
- Result: regenerated the 4 screenshots after lowering and fading the slabs.

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle58_outdoor_void_background_parent_build_20260521.log`
- Result: passed validation and completed with `Build Finished, Result: Success.`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle58_outdoor_void_background_parent_smoke_20260521.log`
- Result: 20-second exe smoke, `match_count=0`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_void_background_treatment_20260521\01_current_house_exterior_void_background_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_void_background_treatment_20260521\02_past_house_exterior_void_background_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_void_background_treatment_20260521\03_current_central_plaza_void_background_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_void_background_treatment_20260521\04_past_central_plaza_void_background_overview.png`

## External Assets

No external or paid assets were used.

## Residual Risk

- The new treatment is intentionally subtle and is not a fully modeled distant sky or background asset.
- The house exterior overview is still conservative by design; the remaining black areas are acceptable because the added slabs stay understated.
