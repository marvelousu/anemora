# 2026-05-20 Fast VS HD2D Current Library Atmosphere Cycle

## Scope

Cycle21 improves the current-side library atmosphere without touching story, dialogue, UI, TimeWindow flow, player behavior, camera logic, route logic, collider behavior, or animation.

The target area is the current library only. The goal is to stop the side shelves, entry floor, and Reto desk zone from reading as flat boards by adding very thin, broad atmosphere layers for light, shadow, and dust.

## Changes

- Edited:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Added current-library-only HD-2D depth framing objects inside `CreateHd2dDepthFraming(HouseMapAreas currentAreas, HouseMapAreas pastAreas)`:
  - `Current_Library_LeftSideShelf_SoftDustLift`
  - `Current_Library_RightSideShelf_SoftDustLift`
  - `Current_Library_EntryFloor_SoftDustPool`
  - `Current_Library_RetoDesk_SideFalloffShadow`
- Reused the existing generated material helpers:
  - `EnsureHd2dWarmLightPoolMaterial()`
  - `EnsureHd2dDepthShadowMaterial()`
- Added validation:
  - `ValidateFastVsHd2dTwentyFirstCycleCurrentLibraryAtmosphere()`
  - wired into `ValidateHouseSliceBatch()` immediately after cycle20 validation
- Added screenshot capture:
  - `CaptureHd2dTwentyFirstCycleScreenshotsBatch()`
  - `CaptureHd2dTwentyFirstCycleScreenshotsToDirectory(...)`

Material tokens used by validation:

- warm pools: `hd2d_warm_light_pool`
- falloff shadow: `hd2d_depth_shadow`

Implementation notes:

- All four additions are non-colliding visual layers.
- No past-side counterparts were added.
- The new layers were kept thin and broad so they behave like atmosphere, not solid scenery.

## External Assets

No external assets or Meshy content were used.

Reason: this was a small atmosphere pass that fit the existing procedural material system, so adding lightweight local layers was faster and lower risk than bringing in new art.

## Verification

- Worker validate log:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle21_worker_validate_20260520.log`
  - Result: passed
- Worker capture log:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle21_worker_capture_20260520.log`
  - Result: passed
- Parent validate log:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle21_parent_validate_20260520.log`
  - Result: passed
- Parent build log:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle21_build_20260520.log`
  - Result: `Build Finished, Result: Success.`
- Parent player smoke log:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle21_player_smoke_20260520.log`
  - Result: `match_count=0`

The validation run reported `Fast VS house slice validation passed.` and the capture run reported `Fast VS twenty-first-cycle screenshots captured: ...`.

Unity batchmode emitted the usual licensing/access-token and `LogAssemblyErrors (0ms)` lines. They did not block validation, build, or player smoke.

## Evidence

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_atmosphere_20260520\01_current_library_entry_dust_pool.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_atmosphere_20260520\02_current_library_left_shelf_dust_lift.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_atmosphere_20260520\03_current_library_reto_desk_falloff.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_atmosphere_20260520\04_past_library_reference_unchanged.png`
