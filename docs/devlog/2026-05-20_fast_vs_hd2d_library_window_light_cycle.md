# 2026-05-20 Fast VS HD2D Library Window Light Cycle

## Scope
- Added HD-2D window light accents inside the library: thin shafts from the side windows and soft floor light pools.
- Kept story, UI, Time Window logic, characters, movement, doors, cameras, colliders, and route behavior unchanged.
- Used only in-repo generated textures and materials. No external or paid assets were used.
- Avoided flat opaque boards by using a generated transparent gradient texture for the light quads.

## Implementation
- Updated `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`.
- Added the new helper methods:
  - `EnsureHd2dLibraryWindowLightMaterial()`
  - `EnsureHd2dLibraryWindowLightTexture()`
  - `CreateHd2dLibraryWindowLightQuad(...)`
  - `CreateLibraryWindowLightAccents(...)`
  - `ValidateFastVsHd2dTwentyFourthCycleLibraryWindowLight()`
  - `ValidateLibraryWindowLightObject(...)`
  - `CaptureHd2dTwentyFourthCycleScreenshotsBatch()`
  - `CaptureHd2dTwentyFourthCycleScreenshotsToDirectory(...)`
- Added the required objects in both current and past library spaces:
  - `Current_Library_WindowLightShaft_Left`
  - `Current_Library_WindowLightShaft_Right`
  - `Current_Library_WindowLightPool_LeftFloor`
  - `Current_Library_WindowLightPool_RightFloor`
  - `Past_Library_WindowLightShaft_Left`
  - `Past_Library_WindowLightShaft_Right`
  - `Past_Library_WindowLightPool_LeftFloor`
  - `Past_Library_WindowLightPool_RightFloor`
- The light texture is generated at `128x160` as `FastVS_House_hd2d_library_window_light_soft.asset`.
- The material is `FastVS_House_hd2d_library_window_light.mat` and uses a transparent unlit setup with a warm tinted gradient.

## Verification
- Validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle24_worker_validate_20260520.log`
- Validation result: passed with no `InvalidOperationException` after the final run.
- Capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle24_worker_capture_20260520.log`
- Capture result: passed and wrote 4 PNGs to `docs/devlog/screenshots/fast_vs_hd2d_library_window_light_20260520/`.
- Parent validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle24_parent_validate_20260520.log`
- Parent validation result: passed with `Fast VS house slice validation passed.`
- Parent capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle24_parent_capture_20260520.log`
- Parent capture result: passed and regenerated the 4 screenshot files under `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_window_light_20260520`.
- Parent build log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle24_build_20260520.log`
- Parent build result: `Build Finished, Result: Success.`
- Player smoke log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle24_player_smoke_20260520.log`
- Player smoke result: 20 second headless run, stopped intentionally, `match_count=0`.

## Notes
- The new light quads remain `TimeWindowPairedSpaceLandmarkKind.PropOrFeature` and have no colliders.
- Existing library windows, Reto desk light, and Aria presence were preserved.
- The final screenshots use the in-repo generated transparent gradient texture only.
- Parent visual review accepted the current/past window light as a subtle HD-2D depth cue rather than a flat opaque rectangle.
