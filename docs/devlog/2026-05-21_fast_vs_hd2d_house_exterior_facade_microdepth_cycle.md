# 2026-05-21 Fast VS HD2D House Exterior Facade Microdepth Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_facade_microdepth_20260521`

This cycle adds a restrained microdepth pass around Niro's house exterior facade. The goal is to make the door, windows, and base read a little more clearly in HD-2D without changing story flow, dialogue, Time Window logic, transitions, controls, fonts, characters, collision layout, map movement glow pads, or the existing path/porch dressing pass.

No external, paid, or API-generated assets were used. The pass uses existing Fast VS materials plus small code-generated non-colliding landmark plates.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateHouseExteriorFacadeMicrodepthPolish(...)` and wired it into `CreateExterior(...)` after `CreateHouseExteriorPathPorchDressing(...)` and before the current/past exterior branch.
- Added the following new facade microdepth objects:
  - `Current_HouseExterior_FacadeMicrodepth_DoorLeftEdgeWearA`
  - `Current_HouseExterior_FacadeMicrodepth_DoorRightEdgeWearA`
  - `Current_HouseExterior_FacadeMicrodepth_LeftWindowLowerDustA`
  - `Current_HouseExterior_FacadeMicrodepth_RightWindowLowerDustA`
  - `Current_HouseExterior_FacadeMicrodepth_BaseStoneChipA`
  - `Past_HouseExterior_FacadeMicrodepth_DoorLeftWarmEdgeA`
  - `Past_HouseExterior_FacadeMicrodepth_DoorRightWarmEdgeA`
  - `Past_HouseExterior_FacadeMicrodepth_LeftWindowInnerGlowA`
  - `Past_HouseExterior_FacadeMicrodepth_RightWindowInnerGlowA`
  - `Past_HouseExterior_FacadeMicrodepth_BaseTileAccentA`
- Kept the new pieces non-colliding, tagged as `TimeWindowPairedSpaceLandmarkKind.PropOrFeature`, and marked `countsForArrival = false` by using `CreateNonArrivalLandmarkCube(...)`.
- Added `ValidateFastVsHd2dSixtyThirdCycleHouseExteriorFacadeMicrodepth()` and `ValidateHouseExteriorFacadeMicrodepthObject(...)` to verify the new objects exist, stay parented correctly, keep renderers and landmarks, avoid colliders, use `PropOrFeature`, remain non-arrival, use expected material tokens, and keep `scale.y <= 0.08`.
- Kept `Current_HouseExterior_MapMoveGlowPad`, `Past_HouseExterior_MapMoveGlowPad`, `Current_HouseExterior_DoorEntrySmallGlow`, `Past_HouseExterior_DoorEntrySmallGlow`, `Current_HouseExterior_DoorClosedPanel`, and `Past_HouseExterior_DoorClosedPanel` present in validation.
- Added `CaptureHd2dSixtyThirdCycleScreenshotsBatch()` and `CaptureHd2dSixtyThirdCycleScreenshotsToDirectory(...)`.
- Added the new cycle validation call to `ValidateHouseSliceBatch()`.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-21_fast_vs_hd2d_house_exterior_facade_microdepth_cycle.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_facade_microdepth_20260521\`

## Validation

Worker validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle63_house_exterior_facade_microdepth_worker_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Worker screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle63_house_exterior_facade_microdepth_worker_capture_20260521.log`
- Result: passed and wrote the requested screenshots.

Parent review:

- Screenshot review completed for all four outputs in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_facade_microdepth_20260521\`.
- Result: accepted. The facade accents are subtle, and no giant slab, white box, missing door, broken map-move glow, or story-marker regression was found. The close captures remain door-heavy, but they are sufficient for this restrained house facade microdepth pass.

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle63_house_exterior_facade_microdepth_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle63_house_exterior_facade_microdepth_parent_capture_20260521.log`
- Result: passed and rewrote the requested screenshots.

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle63_house_exterior_facade_microdepth_parent_build_20260521.log`
- Result: passed and built `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`.

Parent EXE smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle63_house_exterior_facade_microdepth_parent_smoke_20260521.log`
- Result: passed. The built EXE ran for 20 seconds in `-batchmode -nographics`; no `Error`, `Exception`, `NullReference`, `MissingReference`, `Failed`, `Crash`, font fallback, SSAO, DrawObjectsPass, or RenderGraph matches were found.

Unity licensing note:

- Unity logs still include `[Licensing::Module] Error: Access token is unavailable; failed to update`. It did not block validation, capture, build, or smoke. This is a Unity licensing warning, not an Anemora API-token requirement.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_facade_microdepth_20260521\01_current_house_exterior_facade_microdepth_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_facade_microdepth_20260521\02_past_house_exterior_facade_microdepth_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_facade_microdepth_20260521\03_current_house_exterior_door_window_microdepth_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_facade_microdepth_20260521\04_past_house_exterior_door_window_microdepth_close.png`

## External Assets

No external, API, or paid assets were used.

## Residual Risk

- The new details are intentionally subtle, so the visual gain is incremental rather than a full authored facade pass.
- The close views rely on the existing review camera composition, so a later pass could tune framing further if the facade needs more emphasis.
