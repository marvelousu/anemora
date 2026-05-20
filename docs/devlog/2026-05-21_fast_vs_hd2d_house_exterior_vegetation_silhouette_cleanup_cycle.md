# 2026-05-21 Fast VS HD2D House Exterior Vegetation Silhouette Cleanup Cycle

## Scope

- Cycle57: house exterior vegetation silhouette cleanup
- Goal: replace the block-like house exterior tree crown breakup with thin sprite panels that read as vegetation at close range, while keeping dialogue, story, Time Window behavior, fonts, character logic, map transitions, and input unchanged.
- No paid assets, external downloads, or new APIs were used. The pass reuses the repo's existing CC0 tree sprite path and existing pixel materials.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Wired `CreateHouseExteriorTreeCrownSilhouetteBreakup(...)` into `CreateExterior(...)` so the crown cleanup objects are actually instantiated.
- Reworked `CreateHouseExteriorTreeCrownSilhouetteBreakup(...)` to use thin cropped tree-crown panels with the existing `tree3_0` CC0 sprite material instead of cube lobes.
- Tightened the current/past crown breakup placement so the pieces read as small foliage accents rather than a single green box at the road shoulder.
- Parent review rejected the first worker pass where full tree sprites read as small trees. The accepted version crops only the crown UV region and places the accents inside the existing large tree silhouette.
- Updated `ValidateFastVsHd2dFortySeventhCycleHouseExteriorTreeCrownSilhouette()` to validate the tree3 sprite material on the breakup objects.
- Added `ValidateFastVsHd2dFiftySeventhCycleHouseExteriorVegetationSilhouetteCleanup()` and `ValidateHouseExteriorTreeCrownSilhouetteCleanupObject(...)` to enforce:
  - current/past crown cleanup objects exist
  - parent is `Current_HouseExteriorMap_SeparateSpace` / `Past_HouseExteriorMap_SeparateSpace`
  - collider-free
  - `TimeWindowPairedSpaceLandmarkKind.PropOrFeature`
  - non-arrival metadata
  - tree3 sprite texture
  - quad mesh
  - cropped crown UVs that exclude the trunk
  - small flat scale limits
- Added `CaptureHd2dFiftySeventhCycleScreenshotsBatch()` and `CaptureHd2dFiftySeventhCycleScreenshotsToDirectory(...)`.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-21_fast_vs_hd2d_house_exterior_vegetation_silhouette_cleanup_cycle.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_vegetation_silhouette_cleanup_20260521\`

## Validation

Worker validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle57_house_exterior_vegetation_worker_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Worker capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle57_house_exterior_vegetation_worker_capture_20260521.log`
- Result: captured the 4 requested screenshots.

Parent validation logs:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle57_house_exterior_vegetation_parent_validate_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle57_house_exterior_vegetation_parent_validate2_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle57_house_exterior_vegetation_parent_validate3_20260521.log`
- Result: final parent validation passed with `Fast VS house slice validation passed.`

Parent capture logs:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle57_house_exterior_vegetation_parent_capture_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle57_house_exterior_vegetation_parent_capture2_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle57_house_exterior_vegetation_parent_capture3_20260521.log`
- Result: final parent capture regenerated the 4 screenshots after the UV-crop and placement corrections.

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle57_house_exterior_vegetation_parent_build_20260521.log`
- Result: passed validation and completed with `Build Finished, Result: Success.`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle57_house_exterior_vegetation_parent_smoke_20260521.log`
- Result: 20-second exe smoke, `match_count=0`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_vegetation_silhouette_cleanup_20260521\01_current_house_exterior_vegetation_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_vegetation_silhouette_cleanup_20260521\02_past_house_exterior_vegetation_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_vegetation_silhouette_cleanup_20260521\03_current_house_exterior_near_niro_tree_road_shoulder_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_vegetation_silhouette_cleanup_20260521\04_past_house_exterior_near_niro_tree_road_shoulder_close.png`

## External Assets

No external or paid assets were used. The cleanup reuses the repo's existing CC0 `tree3_0` sprite path and existing pixel materials.

## Residual Risk

- The close shot is improved, but the vegetation still occupies a dense cluster at extreme close range. A future pass can improve this further by replacing the single large tree sprite with a more deliberately authored multi-layer tree asset.
- Unity left unrelated auto-generated scene/project/meta diffs during worker validation. Parent cleanup removes them before commit.
