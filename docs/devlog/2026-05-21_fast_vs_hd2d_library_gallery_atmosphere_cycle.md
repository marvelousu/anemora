# 2026-05-21 Fast VS HD2D Library Gallery Atmosphere Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_gallery_atmosphere_20260521`

This cycle adds a narrow visual-only HD-2D atmosphere pass around the library upper-gallery underside and back-wall area. The current library reads more ruined and shadowed; the past library reads warmer and cleaner. Gameplay, route pads, Time Window behavior, story, UI/font, character behavior, scene transition, and colliders were left untouched.

No API token, paid asset purchase, or external download was used.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Wired `CreateLibraryGalleryAtmospherePolish(...)` immediately after `CreateLibraryUpperGallerySupportPolish(...)`.
- Added six current non-arrival landmark cubes:
  - `Current_Library_GalleryAtmosphere_BackUndersideDustShadowA`
  - `Current_Library_GalleryAtmosphere_BackUndersideBrokenShadowB`
  - `Current_Library_GalleryAtmosphere_LeftBalconyWallDustFallA`
  - `Current_Library_GalleryAtmosphere_RightBalconyWallDustFallA`
  - `Current_Library_GalleryAtmosphere_BackPillarFootShadowA`
  - `Current_Library_GalleryAtmosphere_BackShelfTopDustA`
- Added six past non-arrival landmark cubes:
  - `Past_Library_GalleryAtmosphere_BackUndersideWarmGlowA`
  - `Past_Library_GalleryAtmosphere_BackUndersideSoftShadowB`
  - `Past_Library_GalleryAtmosphere_LeftBalconyWallWarmEdgeA`
  - `Past_Library_GalleryAtmosphere_RightBalconyWallWarmEdgeA`
  - `Past_Library_GalleryAtmosphere_BackPillarFootWarmA`
  - `Past_Library_GalleryAtmosphere_BackShelfTopHighlightA`
- Added `ValidateFastVsHd2dEightySecondCycleLibraryGalleryAtmosphere()`.
- Added `CaptureHd2dEightySecondCycleScreenshotsBatch()` and `CaptureHd2dEightySecondCycleScreenshotsToDirectory(...)`.
- Wired the new validation into `ValidateHouseSliceBatch()`.

## Validation

Worker validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle82_library_gallery_atmosphere_worker_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`
- Note: Unity emitted an unrelated `Licensing::Module` access-token error line during startup, but the batch completed successfully.

Worker screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle82_library_gallery_atmosphere_worker_capture_20260521.log`
- Result: passed with `Fast VS eighty-second-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_gallery_atmosphere_20260521`
- Note: Unity emitted the same unrelated licensing startup noise, but the capture batch completed successfully.

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle82_library_gallery_atmosphere_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle82_library_gallery_atmosphere_parent_capture_20260521.log`
- Result: passed with `Fast VS eighty-second-cycle screenshots captured`.

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle82_library_gallery_atmosphere_parent_build_20260521.log`
- Result: passed with `Build Finished, Result: Success.`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle82_library_gallery_atmosphere_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_gallery_atmosphere_20260521\01_current_library_gallery_atmosphere_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_gallery_atmosphere_20260521\02_past_library_gallery_atmosphere_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_gallery_atmosphere_20260521\03_current_library_gallery_atmosphere_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_gallery_atmosphere_20260521\04_past_library_gallery_atmosphere_close.png`

## Notes

- The pass stayed deterministic and non-colliding.
- All new objects are thin `PropOrFeature` landmarks built from `CreateNonArrivalLandmarkCube(...)`.
- The parent session regenerated the screenshots, then ran Unity validation, player build, and startup smoke.
- Unity produced incidental refresh churn in scene, material, and project settings files outside the owned edit set; those were cleaned before committing this cycle.
