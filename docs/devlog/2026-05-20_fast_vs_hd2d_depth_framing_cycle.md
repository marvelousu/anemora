# 2026-05-20 Fast VS HD2D Depth Framing Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Public baseline not touched: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample`
- Main implementation file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Generated scene: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`

This cycle adds local depth/framing primitives rather than a global darkening pass. The goal is to improve HD-2D stage read through subtle object-local shadow bands and warm pools while preserving gameplay, route layout, story flow, and Time Window behavior.

## Planning / Worker Cycle

- Parent planned a small depth/framing pass after the close-review screenshot cycle exposed remaining flatness around wall seams, facade eaves, Reto's desk, and the library upper gallery.
- Worker: `019e41ad-3060-77d3-8f24-60861b993611` (`Boole`, gpt-5.4-mini).
- Worker added `CreateHd2dDepthFraming(...)`, `CaptureHd2dSeventhCycleScreenshotsBatch()`, transparent local materials, and a seventh-cycle validator.
- Parent review changed the implementation so the new objects are parented under each area map root rather than directly under the current/past root. This keeps them inside the existing area visibility contract.
- Parent review lowered the depth-shadow alpha from `0.22` to `0.12` to avoid a broad black-rectangle read.

## Implementation

Added local non-colliding depth/framing objects:

- `Current_HouseInterior_BackWall_DepthBand`
- `Past_HouseInterior_BackWall_DepthBand`
- `Current_HouseInterior_Table_WarmLightPool`
- `Past_HouseInterior_Table_WarmLightPool`
- `Current_HouseExterior_Door_DepthPool`
- `Past_HouseExterior_Door_WarmPool`
- `Current_CentralPlaza_LibraryFacade_DepthUnderEave`
- `Past_CentralPlaza_LibraryFacade_WindowWarmPool`
- `Current_Library_BackShelf_DepthBand`
- `Past_Library_BackShelf_DepthBand`
- `Current_Library_RetoDesk_WarmPool`
- `Current_Library_SecondFloor_UnderGalleryDepth_Left`
- `Current_Library_SecondFloor_UnderGalleryDepth_Right`
- `Past_Library_SecondFloor_UnderGalleryDepth_Left`
- `Past_Library_SecondFloor_UnderGalleryDepth_Right`

Generated / updated materials:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_depth_shadow.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_warm_light_pool.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_niro_contact_shadow.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_timewriter_pocket_yellow_glow.mat`

The last two existing materials were updated to match the render queues already enforced by their generator helpers.

## Screenshot Evidence

Output directory:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_depth_framing_20260520`

Captured files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_depth_framing_20260520\01_interior_niro_shadow.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_depth_framing_20260520\02_exterior_niro_shadow.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_depth_framing_20260520\03_library_reto_desk.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_depth_framing_20260520\04_library_reto_talk_loop.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_depth_framing_20260520\05_library_past_no_temp_people.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_depth_framing_20260520\06_library_dialogue_tmp_font.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_depth_framing_20260520\07_plaza_library_facade_current.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_depth_framing_20260520\08_plaza_library_facade_past.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_depth_framing_20260520\09_library_timewriter_pocket_glow.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_depth_framing_20260520\10_library_current_yellow_timewindow_cues.png`

## Verification

Worker validation:

- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle7_worker_validate_20260520.log`
- Result: passed.

Parent validation:

- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle7_validate_parent_20260520.log`
- Result: passed.

Screenshot capture:

- Initial capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle7_capture_20260520.log`
- Retry log after parent alpha/parenting fix: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle7_capture_retry2_20260520.log`
- A first retry log exists at `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle7_capture_retry1_20260520.log`; it stopped because a previous Unity batch process still held the project lock.

Build:

- Command log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle7_build_20260520.log`
- Result: success.
- Key lines: `Fast VS house slice validation passed.`, `Build Finished, Result: Success.`, and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Build output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Player smoke:

- Player log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle7_player_smoke_20260520.log`
- Result: launched for 20 seconds, then was intentionally stopped.
- Checked patterns: `Error`, `Exception`, `NullReference`, `MissingReference`, `Failed`, `Crash`, `Font Atlas Texture`, `LiberationSans`, `ScreenSpaceAmbientOcclusion`, `DrawObjectsPass`, `RenderGraph`.
- Match count: 0.

## Visual Review

- The library current/past shots no longer read as a single flat room plane; under-gallery depth and back-shelf bands add separation without covering bookshelves or Reto.
- The plaza facade retains readable door/window textures. The past warm pool remains subtle and does not cover the windows.
- The house exterior door gains a stronger porch grounding cue without changing the doorway transition.
- The pass still needs future authored asset work for higher-quality objects, but it is a low-risk improvement over a broad lighting/darkening change.

## Boundaries

- Live Unity MCP was not available in this Codex session, so verification used Unity batch-mode editor execution, screenshot capture, build, and player smoke.
- Meshy/API and paid external assets were not used. Current quality bottleneck is local composition and material staging, so importing a paid pack in this cycle would have slowed the route without addressing the immediate flatness issue.
- Unity-generated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\ProjectSettings` and `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\AddressableAssetsData` side effects were restored before commit selection.
