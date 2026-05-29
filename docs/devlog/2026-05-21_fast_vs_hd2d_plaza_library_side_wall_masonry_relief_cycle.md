# 2026-05-21 Fast VS HD2D Plaza Library Side Wall Masonry Relief Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Scene target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- Screenshot output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_side_wall_masonry_relief_20260521`

This cycle continues the plaza library 3D exterior work by adding muted side-wall masonry relief to reduce the large-flat-plane feeling in the oblique view.

No Meshy/API token, paid asset purchase, external download, or new third-party asset was used.

## Worker Cycle

- Worker: `gpt-5.4-mini` session `019e492a-d336-72f3-ba7f-04f81afff44e`.
- Worker instruction: add a small Cycle100 side-wall relief pass for the central plaza library, with non-colliding PropOrFeature objects only, muted materials only, no gameplay/story/Time Window/route/UI/font/input/character/collision/map-coordinate changes, plus validation and screenshot helpers.
- Parent review: the first worker pass was structurally correct but too subtle; after moving relief outward and brightening the ribs, the current oblique shot showed a floating black horizontal bar. Parent changed the under-eave material from `shadow` / `past_roof` to muted `dust` / `past_exterior_wall` and moved the inner relief back toward the wall face. Final screenshots no longer show the floating bar.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateCentralPlazaLibrarySideWallMasonryReliefPolish(...)` and called it after `CreateCentralPlazaLibraryRearRoofConnectionPolish(...)`.
- Added current and past west/east side-wall relief objects:
  - `*_WestVerticalRibA`
  - `*_WestVerticalRibB`
  - `*_WestHorizontalCourseA`
  - `*_WestUnderEaveShadowA`
  - `*_WestRearCornerCapA`
  - mirrored `East*` variants
- Kept all new objects non-colliding and non-arrival via `CreateNonArrivalLandmarkCubeShadowSafe(...)`.
- Used muted material families only:
  - current side ribs/caps/courses: `current_stone`, `dust`
  - past side ribs/caps/courses: `past_stone`, `past_fence`, `past_exterior_wall`
  - explicitly avoided `window_light` and `warm_light`
- Added `ValidateFastVsHd2dOneHundredthCyclePlazaLibrarySideWallMasonryRelief()`.
- Added `ValidateCentralPlazaLibrarySideWallMasonryReliefObject(...)`, checking parent, material token, no collider, no shadows, landmark id prefix, PropOrFeature kind, non-arrival status, placement range, scale range, and no bright light material.
- Added `CaptureHd2dOneHundredthCycleScreenshotsBatch()` and `CaptureHd2dOneHundredthCycleScreenshotsToDirectory(...)`.

The Unity scene was regenerated so the side-wall relief objects are present in the checked-in scene.

## Validation

Parent validation logs:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle100_plaza_library_side_wall_masonry_relief_parent_validate_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle100_plaza_library_side_wall_masonry_relief_parent_validate_fix_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle100_plaza_library_side_wall_masonry_relief_parent_validate_fix2_20260521.log`
- Final result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture logs:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle100_plaza_library_side_wall_masonry_relief_parent_capture_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle100_plaza_library_side_wall_masonry_relief_parent_capture_fix_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle100_plaza_library_side_wall_masonry_relief_parent_capture_fix2_20260521.log`
- Final result: passed with `Fast VS one-hundredth-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_side_wall_masonry_relief_20260521`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle100_plaza_library_side_wall_masonry_relief_parent_build_20260521.log`
- Result: passed with `Build Finished, Result: Success.` and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle100_plaza_library_side_wall_masonry_relief_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_side_wall_masonry_relief_20260521\01_current_plaza_library_side_wall_masonry_relief_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_side_wall_masonry_relief_20260521\02_past_plaza_library_side_wall_masonry_relief_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_side_wall_masonry_relief_20260521\03_current_plaza_library_side_wall_masonry_relief_oblique.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_side_wall_masonry_relief_20260521\04_past_plaza_library_side_wall_masonry_relief_oblique.png`

## Notes

- The cycle intentionally avoids stronger lighting/glow, because the recent exterior cycles had several cases where bright accents read as floating plates.
- The effect is modest in the overview shot and more legible in the oblique side-wall shot, which is the intended review angle for this cycle.
- Unity batchmode produced transient Addressables/ProjectSettings/importer/material changes during validation and build; these were excluded from the commit so only the intended script, regenerated scene, devlog, and screenshots remain.
