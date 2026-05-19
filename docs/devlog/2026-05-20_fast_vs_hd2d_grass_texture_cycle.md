# 2026-05-20 Fast VS HD2D Grass Texture Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Public baseline not touched: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample`
- Main implementation file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Generated textures:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_current_grass_hd2d_plate.asset`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_past_grass_hd2d_plate.asset`

This cycle improves outdoor grass/soil surfaces after the path-stone pass. It deliberately avoids reintroducing a sky/background pass, because the prior sky attempt read as too rough. The change keeps all route positions, colliders, story flow, Time Window behavior, UI, and font assets unchanged.

## Planning / Worker Cycle

- Parent selected outdoor grass as the next HD-2D quality target because the old `PixelPattern.Grass` looked too much like a large checker/noise tile around the house and plaza.
- Worker: `019e41ad-3060-77d3-8f24-60861b993611` (`Boole`, gpt-5.4-mini).
- Worker first replaced `current_grass` / `past_grass` with generated painted grass plates.
- Parent review rejected the first visual pass because the grass still read as large green rectangular tiles.
- Worker review-fix removed the visible cell-boundary treatment and rebuilt the sampler around smooth low-frequency noise, fine noise, subtle leaf lines, and soil masks.

## Implementation

Updated procedural grass rendering in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- `current_grass` now uses `PaintedSurfaceMaterial("current_grass", "current_grass_hd2d_plate", ...)`.
- `past_grass` now uses `PaintedSurfaceMaterial("past_grass", "past_grass_hd2d_plate", ...)`.
- Added `SampleCurrentGrassHd2dPixel(...)` and `SamplePastGrassHd2dPixel(...)`.
- Added `SampleGrassAndSoilHd2dPixel(...)` with smooth deterministic noise, soil blend masks, clumps, thin diagonal leaf lines, and subdued color ranges.
- Added `SampleSmoothValueNoise2D(...)` for non-blocky grass/soil variation.
- Added `ValidateFastVsHd2dTenthCycleGrassTexture()` to verify generated texture assets, exact sizes, material texture references, and scene-object assignments for house exterior and central plaza ground.
- Added `CaptureHd2dTenthCycleScreenshotsBatch()` for review evidence.

Updated material outputs:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_grass.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_past_grass.mat`

## Screenshot Evidence

Output directory:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_grass_texture_20260520`

Captured files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_grass_texture_20260520\01_interior_niro_shadow.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_grass_texture_20260520\02_exterior_niro_shadow.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_grass_texture_20260520\03_library_reto_desk.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_grass_texture_20260520\04_library_reto_talk_loop.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_grass_texture_20260520\05_library_past_no_temp_people.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_grass_texture_20260520\06_library_dialogue_tmp_font.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_grass_texture_20260520\07_plaza_library_facade_current.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_grass_texture_20260520\08_plaza_library_facade_past.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_grass_texture_20260520\09_library_timewriter_pocket_glow.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_grass_texture_20260520\10_library_current_yellow_timewindow_cues.png`

## Verification

Worker validation:

- First worker log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle10_worker_validate_20260520.log`
- Review-fix worker log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle10_worker_validate_reviewfix_20260520.log`
- Result: passed.

Parent validation:

- First parent log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle10_validate_parent_20260520.log`
- Review-fix parent log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle10_validate_parent_reviewfix_20260520.log`
- Result: passed.

Screenshot capture:

- First capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle10_capture_retry1_20260520.log`
- Final review-fix capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle10_capture_reviewfix_20260520.log`
- Result: passed.

Build:

- Command log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle10_build_20260520.log`
- Result: success.
- Key lines: `Fast VS house slice validation passed.`, `Build Finished, Result: Success.`, and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Build output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Note: the build log contains Unity package/file-system noise such as `move_path failed: No error`, but the build result is success and the player smoke scan was clean.

Player smoke:

- Player log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle10_player_smoke_20260520.log`
- Result: launched for 20 seconds, then was intentionally stopped.
- Checked patterns: `Error`, `Exception`, `NullReference`, `MissingReference`, `Failed`, `Crash`, `Font Atlas Texture`, `LiberationSans`, `ScreenSpaceAmbientOcclusion`, `DrawObjectsPass`, `RenderGraph`.
- Match count: 0.

## Visual Review

- The rejected first grass pass looked like large green tiles; it was not committed as-is.
- The review-fix pass removes the obvious rectangle-cell read and leaves grass as a quieter supporting surface around the plaza and house.
- The current side remains darker and drier; the past side remains slightly greener and cleaner.
- Grass texture is intentionally less busy than the stone path so it does not compete with movement glows or the library facade.

## Boundaries

- Live Unity MCP was not available in this Codex session, so verification used Unity batch-mode editor execution, screenshot capture, build, and player smoke.
- Meshy/API and paid external assets were not used in this cycle. The target was a route-safe background material replacement.
- Unity-generated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\ProjectSettings`, `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\AddressableAssetsData`, `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Resources`, and generated scene side effects were restored or removed before commit selection.
