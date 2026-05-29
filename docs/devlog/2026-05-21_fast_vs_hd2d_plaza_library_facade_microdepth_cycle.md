# 2026-05-21 Fast VS HD2D Plaza Library Facade Microdepth Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_facade_microdepth_20260521`

This cycle adds a restrained microdepth pass around the central plaza library facade in Fast VS. The goal is to make the door, windows, and base read a little more clearly in HD-2D without changing story flow, dialogue, Time Window logic, transitions, controls, fonts, characters, collision layout, or map movement glow pads.

No external, paid, or API-generated assets were used. The pass uses existing Fast VS materials plus small code-generated non-colliding landmark plates.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateCentralPlazaLibraryFacadeMicrodepthPolish(...)` and wired it into `CreateCentralPlaza(...)` after the existing facade polish passes.
- Added the following new facade microdepth objects:
  - `Current_CentralPlaza_LibraryFacadeMicrodepth_DoorLeftEdgeWearA`
  - `Current_CentralPlaza_LibraryFacadeMicrodepth_DoorRightEdgeWearA`
  - `Current_CentralPlaza_LibraryFacadeMicrodepth_LeftWindowLowerDustA`
  - `Current_CentralPlaza_LibraryFacadeMicrodepth_RightWindowLowerDustA`
  - `Current_CentralPlaza_LibraryFacadeMicrodepth_BaseStoneChipA`
  - `Past_CentralPlaza_LibraryFacadeMicrodepth_DoorLeftWarmEdgeA`
  - `Past_CentralPlaza_LibraryFacadeMicrodepth_DoorRightWarmEdgeA`
  - `Past_CentralPlaza_LibraryFacadeMicrodepth_LeftWindowInnerGlowA`
  - `Past_CentralPlaza_LibraryFacadeMicrodepth_RightWindowInnerGlowA`
  - `Past_CentralPlaza_LibraryFacadeMicrodepth_BaseTileAccentA`
- Added `ValidateFastVsHd2dSixtyFirstCyclePlazaLibraryFacadeMicrodepth()` and `ValidatePlazaLibraryFacadeMicrodepthObject(...)` to verify the new objects exist, stay parented correctly, keep renderers and landmarks, avoid colliders, use `PropOrFeature`, remain non-arrival, use the expected material tokens, and keep `scale.y <= 0.08`.
- Kept `Current_CentralPlaza_LibraryDoorPanelsLeft`, `Past_CentralPlaza_LibraryDoorPanelsLeft`, `Current_CentralPlaza_ToLibrary_MapMoveGlowPad`, and `Past_CentralPlaza_ToLibrary_MapMoveGlowPad` present in validation.
- Added `CaptureHd2dSixtyFirstCycleScreenshotsBatch()` and `CaptureHd2dSixtyFirstCycleScreenshotsToDirectory(...)`.
- Added the new cycle validation call to `ValidateHouseSliceBatch()`.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-21_fast_vs_hd2d_plaza_library_facade_microdepth_cycle.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_facade_microdepth_20260521\`

## Validation

Worker validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle61_plaza_library_facade_microdepth_worker_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle61_plaza_library_facade_microdepth_worker_capture_20260521.log`
- Result: passed and wrote the requested screenshots.

Parent review:

- Screenshot review completed for all four outputs in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_facade_microdepth_20260521\`.
- Result: accepted. No black slab, giant cube, missing facade door, or obvious textureless white box regression was found. The close captures are door-heavy, but usable for this restrained facade microdepth pass.

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle61_plaza_library_facade_microdepth_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle61_plaza_library_facade_microdepth_parent_capture_20260521.log`
- Result: passed and rewrote the requested screenshots.

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle61_plaza_library_facade_microdepth_parent_build_20260521.log`
- Result: passed and built `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`.

Parent EXE smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle61_plaza_library_facade_microdepth_parent_smoke_20260521.log`
- Result: passed. The built EXE ran for 20 seconds in `-batchmode -nographics`; no `Error`, `Exception`, `NullReference`, `MissingReference`, `Failed`, `Crash`, font fallback, SSAO, DrawObjectsPass, or RenderGraph matches were found.

Unity licensing note:

- Unity logs still include `[Licensing::Module] Error: Access token is unavailable; failed to update`. It did not block validation, capture, build, or smoke. This is a Unity licensing warning, not an Anemora API-token requirement.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_facade_microdepth_20260521\01_current_plaza_library_facade_microdepth_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_facade_microdepth_20260521\02_past_plaza_library_facade_microdepth_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_facade_microdepth_20260521\03_current_plaza_library_door_window_microdepth_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_facade_microdepth_20260521\04_past_plaza_library_door_window_microdepth_close.png`

## External Assets

No external, paid, or API-generated assets were used.

## Residual Risk

- The new details are intentionally subtle, so the visual gain is incremental rather than a full authored facade pass.
- The close views still rely on the existing central plaza review camera composition, so a later pass could tune framing further if the facade needs more emphasis.
