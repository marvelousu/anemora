# 2026-05-20 Fast VS HD2D House Exterior Facade Texture Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Devlog target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-20_fast_vs_hd2d_house_exterior_facade_texture_cycle.md`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_facade_texture_20260520`

This cycle refines the procedural facade wall and roof textures for Niro's house so the HD-2D exterior reads more clearly as visible wall material and roof material. It does not change house geometry, route triggers, door logic, story flow, player state, camera runtime behavior, colliders, or drop guards.

## Implementation

- Improved `SampleWeatheredWallPlatePixel(...)` so the facade uses clearer vertical seams, stronger horizontal courses, softer shadow mixing, and less noise-heavy weathering.
- Improved `SampleRoofShinglePlatePixel(...)` so the roof reads as larger shingle rows with clearer row edges, staggered offsets, and restrained color variation.
- Added `ValidateFastVsHd2dThirtiethCycleHouseExteriorFacadeTextures()` to `ValidateHouseSliceBatch()`.
- Added `ValidateTextureLuminanceContrast(string textureId, Vector2Int a, Vector2Int b, float minDelta, string label)` to confirm seam-vs-field and row-vs-field contrast on generated textures.
- Added `CaptureHd2dThirtiethCycleScreenshotsBatch()` and `CaptureHd2dThirtiethCycleScreenshotsToDirectory(string outputDirectory)` for facade and roof close review evidence.

## Screenshot Evidence

Output directory:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_facade_texture_20260520`

Captured files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_facade_texture_20260520\01_current_house_facade_texture_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_facade_texture_20260520\02_past_house_facade_texture_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_facade_texture_20260520\03_current_house_roof_texture_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_facade_texture_20260520\04_past_house_roof_texture_close.png`

## Verification

Validation command:

`& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle30_worker_validate_20260520.log'`

- Result: passed.
- Validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle30_worker_validate_20260520.log`

Capture command:

`& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dThirtiethCycleScreenshotsBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle30_worker_capture_20260520.log'`

- Result: passed.
- Capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle30_worker_capture_20260520.log`

Parent validation log:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle30_parent_validate_20260520.log`

- Result: passed.

Parent screenshot capture log:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle30_parent_capture_20260520.log`

- Result: passed.

Build log:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle30_build_20260520.log`

- Result: success. The log contains `Build Finished, Result: Success.`
- Build output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Player smoke log:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle30_player_smoke_20260520.log`

- Result: launched for 20 seconds and was intentionally stopped; checked error-pattern match count was 0.

## Notes

- Meshy/API/paid external assets were not used.
- The new validation checks only the generated repeat textures and the expected facade/roof material bindings.
- The screenshots are close-review evidence only; no gameplay-facing layout changes were introduced.
