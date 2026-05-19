# 2026-05-20 Fast VS HD2D Path Stone Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Public baseline not touched: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample`
- Main implementation file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Generated textures:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_current_path_hd2d_plate.asset`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_past_path_hd2d_plate.asset`

This cycle improves the exterior/plaza path material. The previous `current_path` / `past_path` materials used a simple pixel grid that made the plaza floor and road surfaces read as artificial blocks. The change keeps all route positions, colliders, story flow, Time Window behavior, UI, and font assets unchanged.

## Planning / Worker Cycle

- Parent selected the path/stone surface as the next low-risk HD-2D quality target after the book-palette cycle.
- Worker: `019e41ad-3060-77d3-8f24-60861b993611` (`Boole`, gpt-5.4-mini).
- Worker replaced `current_path` and `past_path` material generation with deterministic 128x128 painted flagstone plates.
- Parent reviewed the screenshots and accepted the pass because the stone square and exterior approach now read more like authored stone while preserving the yellow/orange transition lights.

## Implementation

Updated procedural path rendering in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- `current_path` now uses `PaintedSurfaceMaterial("current_path", "current_path_hd2d_plate", ...)`.
- `past_path` now uses `PaintedSurfaceMaterial("past_path", "past_path_hd2d_plate", ...)`.
- Added `SampleCurrentPathHd2dPixel(...)` and `SamplePastPathHd2dPixel(...)`.
- Added shared flagstone helpers for deterministic variable-width/height stone cells, seams, chips, cracks, dust, and subtle center shading.
- Added `ValidateFastVsHd2dNinthCyclePathStone()` to verify generated texture assets, exact sizes, material texture references, and scene-object assignments for the plaza stone square and house exterior path.
- Added `CaptureHd2dNinthCycleScreenshotsBatch()` for review evidence.

Updated material outputs:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_path.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_past_path.mat`

## Screenshot Evidence

Output directory:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_path_stone_20260520`

Captured files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_path_stone_20260520\01_interior_niro_shadow.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_path_stone_20260520\02_exterior_niro_shadow.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_path_stone_20260520\03_library_reto_desk.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_path_stone_20260520\04_library_reto_talk_loop.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_path_stone_20260520\05_library_past_no_temp_people.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_path_stone_20260520\06_library_dialogue_tmp_font.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_path_stone_20260520\07_plaza_library_facade_current.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_path_stone_20260520\08_plaza_library_facade_past.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_path_stone_20260520\09_library_timewriter_pocket_glow.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_path_stone_20260520\10_library_current_yellow_timewindow_cues.png`

## Verification

Worker validation:

- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle9_worker_validate_20260520.log`
- Result: passed.

Parent validation:

- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle9_validate_parent_20260520.log`
- Result: passed.

Screenshot capture:

- Initial capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle9_capture_20260520.log`
- Retry log after Unity project lock release: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle9_capture_retry1_20260520.log`
- Result: passed.

Build:

- Command log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle9_build_20260520.log`
- Result: success.
- Key lines: `Fast VS house slice validation passed.`, `Build Finished, Result: Success.`, and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Build output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Player smoke:

- Player log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle9_player_smoke_20260520.log`
- Result: launched for 20 seconds, then was intentionally stopped.
- Checked patterns: `Error`, `Exception`, `NullReference`, `MissingReference`, `Failed`, `Crash`, `Font Atlas Texture`, `LiberationSans`, `ScreenSpaceAmbientOcclusion`, `DrawObjectsPass`, `RenderGraph`.
- Match count: 0.

## Visual Review

- The plaza stone square no longer reads as a uniform brown grid; it has more worn-stone variation and localized cracks/chips.
- The past plaza remains warmer and cleaner than the current plaza.
- The house exterior path and northeast road now share a more authored-looking surface, without changing route geometry.
- The material is still procedural, but it is a clear improvement over the previous repeated block texture.

## Boundaries

- Live Unity MCP was not available in this Codex session, so verification used Unity batch-mode editor execution, screenshot capture, build, and player smoke.
- Meshy/API and paid external assets were not used in this cycle. This pass was specifically a route-safe material replacement for existing path surfaces.
- Unity-generated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\ProjectSettings`, `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\AddressableAssetsData`, `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Resources`, and generated scene side effects were restored or removed before commit selection.
