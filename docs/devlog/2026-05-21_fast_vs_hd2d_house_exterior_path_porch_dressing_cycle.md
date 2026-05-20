# 2026-05-21 Fast VS HD2D House Exterior Path/Porch Dressing Cycle

## Scope

- Cycle56: house exterior path/porch dressing
- Goal: break up the flat feel around the doorway, path shoulders, northeast road edge, and fence-side ground without touching dialogue, Time Window behavior, map movement, fonts, characters, or story flags.
- No external or paid assets were adopted. The pass uses existing materials plus small cube/slab dressing only.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateHouseExteriorPathPorchDressing(...)` and called it from `CreateExterior(...)` after `CreateExteriorDetails(...)`.
- Added 14 current/past non-colliding path/porch dressing objects under `Current_HouseExteriorMap_SeparateSpace` and `Past_HouseExteriorMap_SeparateSpace`:
  - `Current_HouseExterior_PathPorch_DoorstepContactShadowA`
  - `Current_HouseExterior_PathPorch_PathLeftShoulderA`
  - `Current_HouseExterior_PathPorch_PathRightShoulderA`
  - `Current_HouseExterior_PathPorch_PorchStoneChipA`
  - `Current_HouseExterior_PathPorch_RoadShoulderPebbleA`
  - `Current_HouseExterior_PathPorch_RoadEdgeShadowA`
  - `Current_HouseExterior_PathPorch_YardPatchNearFenceA`
  - and the matching `Past_` versions of the same seven objects
- Added `ValidateFastVsHd2dFiftySixthCycleHouseExteriorPathPorchDressing()` and `ValidateHouseExteriorPathPorchDressingObject(...)`, and wired the validation into `ValidateHouseSliceBatch()`.
- Added `CaptureHd2dFiftySixthCycleScreenshotsBatch()` and `CaptureHd2dFiftySixthCycleScreenshotsToDirectory(...)`.
- Parent review adjusted the path shoulder and past road pebble materials from bright leaf variants to dust/past-stone variants after screenshot review, because the close shot read as a green block instead of path dressing.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-21_fast_vs_hd2d_house_exterior_path_porch_dressing_cycle.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_path_porch_dressing_20260521\`

## Validation

Worker validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle56_house_exterior_path_porch_worker_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Worker capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle56_house_exterior_path_porch_worker_capture_20260521.log`
- Result: captured the 4 requested screenshots.

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle56_house_exterior_path_porch_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle56_house_exterior_path_porch_parent_capture_20260521.log`
- Result: captured the 4 requested screenshots after the material adjustment.

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle56_house_exterior_path_porch_parent_build_20260521.log`
- Result: passed validation and completed with `Build Finished, Result: Success.`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle56_house_exterior_path_porch_parent_smoke_20260521.log`
- Result: 20-second exe smoke, `match_count=0`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_path_porch_dressing_20260521\01_current_house_exterior_path_porch_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_path_porch_dressing_20260521\02_past_house_exterior_path_porch_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_path_porch_dressing_20260521\03_current_house_exterior_road_shoulder_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_path_porch_dressing_20260521\04_past_house_exterior_road_shoulder_close.png`

## External Assets

No external or paid assets were used.

## Residual Risk

- The new dressing is intentionally thin and non-blocking, but the road close shot still depends on the existing review-camera composition staying readable.
- The existing blocky green tree/hedge silhouette remains visible in the house exterior close shot. It is not part of this cycle's path/porch dressing and should be handled in a separate vegetation polish cycle.
