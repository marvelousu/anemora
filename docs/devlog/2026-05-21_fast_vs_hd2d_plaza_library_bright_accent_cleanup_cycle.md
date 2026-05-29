# 2026-05-21 Fast VS HD2D Plaza Library Bright Accent Cleanup Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Scene target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- Screenshot output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_bright_accent_cleanup_20260521`

This cycle cleans up the past-side plaza library exterior accents that read as floating white plates in the wide oblique review angle. The real lit window panes remain; only small glint/highlight/tick accent cubes were changed to muted structural trim.

No Meshy/API token, paid asset purchase, external download, or new third-party asset was used.

## Worker Cycle

- Worker: `gpt-5.4-mini` session `019e492a-d336-72f3-ba7f-04f81afff44e`.
- Worker instruction: keep actual past window panes lit, but move small exterior accent cubes away from `window_light` / `warm_light_pool`; do not touch gameplay, story, Time Window, route transitions, route lights, UI, font, input, characters, map coordinates, or collision.
- Parent review: corrected a Cycle97 validation expectation that the worker briefly changed back to `shadow`; the actual Cycle97 under-eave band remains `dust`. Parent also revised the new Cycle98 validation helper so it fits existing legacy accent objects instead of assuming every target used the newer non-arrival helper.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Changed past-side small library facade architecture highlights from `window_light` to muted trim:
  - `Past_CentralPlaza_LibraryFacadeArchitecture_LeftWindowHighlight`
  - `Past_CentralPlaza_LibraryFacadeArchitecture_RightWindowHighlight`
- Changed past-side small library facade landmark glints from `window_light` to muted trim:
  - `Past_CentralPlaza_LibraryFacadeLandmark_LeftWindowGlintA`
  - `Past_CentralPlaza_LibraryFacadeLandmark_RightWindowGlintA`
- Changed past-side small library facade microdepth inner glow strips from `window_light` to muted trim:
  - `Past_CentralPlaza_LibraryFacadeMicrodepth_LeftWindowInnerGlowA`
  - `Past_CentralPlaza_LibraryFacadeMicrodepth_RightWindowInnerGlowA`
- Changed past-side small upper light ticks from `window_light` to muted trim:
  - `Past_CentralPlaza_LibraryFacadeSurfaceBreakup_LeftUpperLightTickA`
  - `Past_CentralPlaza_LibraryFacadeSurfaceBreakup_RightUpperLightTickA`
- Updated the existing validation token checks for the above objects from `window_light` to `past_fence`.
- Added `ValidateFastVsHd2dNinetyEighthCyclePlazaLibraryExteriorBrightAccentCleanup()`.
- Added `ValidateCentralPlazaLibraryBrightAccentCleanupObject(...)`, which verifies the targeted small accents exist, remain under `Past_CentralPlazaMap_SeparateSpace`, stay near the facade, use the expected muted material token, and do not use `window_light` or `warm_light_pool`.
- Added `CaptureHd2dNinetyEighthCycleScreenshotsBatch()` and `CaptureHd2dNinetyEighthCycleScreenshotsToDirectory(...)`.

The Unity scene was regenerated so the muted accent materials are present in the checked-in scene.

## Validation

Parent validation logs:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle98_plaza_library_bright_accent_cleanup_parent_validate_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle98_plaza_library_bright_accent_cleanup_parent_validate_retry_20260521.log`
- Final result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle98_plaza_library_bright_accent_cleanup_parent_capture_20260521.log`
- Result: passed with `Fast VS ninety-eighth-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_bright_accent_cleanup_20260521`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle98_plaza_library_bright_accent_cleanup_parent_build_20260521.log`
- Result: passed with `Build Finished, Result: Success.` and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle98_plaza_library_bright_accent_cleanup_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_bright_accent_cleanup_20260521\01_current_plaza_library_bright_accent_cleanup_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_bright_accent_cleanup_20260521\02_past_plaza_library_bright_accent_cleanup_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_bright_accent_cleanup_20260521\03_current_plaza_library_bright_accent_cleanup_oblique.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_bright_accent_cleanup_20260521\04_past_plaza_library_bright_accent_cleanup_oblique.png`

## Notes

- The broad past oblique screenshot now shows the targeted small accents as dark/structural details rather than bright floating strips. The real lit window panes remain visible.
- This cycle intentionally avoids changing the major past window material and avoids moving or deleting exterior architecture.
- Unity batchmode produced transient Addressables/ProjectSettings/importer changes during validation and build; these were excluded from the commit so only the intended script, regenerated scene, devlog, and screenshots remain.
