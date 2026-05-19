# 2026-05-20 Fast VS HD2D Object Detail Cycle

## Summary

Implemented the fifth HD-2D polish cycle on the active HD-2D work branch:

- Branch: `work/fast-vs-hd2d-polish-20260520`
- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Public baseline preserved: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample` on `main` was not edited.

This cycle moved from broad material texture passes to object-level readability. It does not change story flow, route contracts, Time Window behavior, HUD copy, dialogue font assets, transition logic, save behavior, or event flags.

## Planning And Delegation

The established development cycle was followed:

- Parent session prepared the Cycle 5 scope and worker handoff.
- Worker: `019e418c-41c2-7cf0-a9c4-51ca8620229c` (`Anscombe`, gpt-5.4-mini) owned `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`.
- Parent review confirmed the worker touched only the generator source before Unity generation.
- Parent review confirmed that new objects are non-colliding detail props and that the changes did not touch gameplay, collision rules, Time Window behavior, story/dialogue, HUD, font, input, or transition logic.
- Parent review removed Unity build/import side effects from `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\AddressableAssetsData` and `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\ProjectSettings`.

## Implemented Scope

Generator source:

- Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`.
- Added `CaptureHd2dFifthCycleScreenshotsBatch()`.
- Added `ValidateFastVsHd2dFifthCycleObjectDetails()`.

Generated texture assets:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_current_house_door_detail_hd2d_plate.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_past_house_door_detail_hd2d_plate.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_current_library_door_detail_hd2d_plate.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_past_library_door_detail_hd2d_plate.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_current_rubble_detail_hd2d_plate.asset`

Generated material assets:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_house_door_detail.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_past_house_door_detail.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_library_door_detail.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_past_library_door_detail.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_rubble_detail.mat`

Object-level detail changes:

- `Current_HouseExterior_DoorClosedPanel` now uses the current house door detail material.
- `Past_HouseExterior_DoorClosedPanel` now uses the past house door detail material.
- `Current_CentralPlaza_LibraryDoorPanelsLeft` and `Current_CentralPlaza_LibraryDoorPanelsRight` now use the current library door detail material.
- `Past_CentralPlaza_LibraryDoorPanelsLeft` and `Past_CentralPlaza_LibraryDoorPanelsRight` now use the past library door detail material.
- Open books created through `CreateReadableBookProp(...)` now receive small non-colliding page line objects:
  - `{objectName}_OpenPageLeft_LineA`
  - `{objectName}_OpenPageLeft_LineB`
  - `{objectName}_OpenPageRight_LineA`
  - `{objectName}_OpenPageRight_LineB`
- Current library now has small current-only rubble/readability props:
  - `Current_Library_Ruin_Detail_BookShardA`
  - `Current_Library_Ruin_Detail_BookShardB`
  - `Current_Library_Ruin_Detail_BrokenPlankA`
  - `Current_Library_Ruin_Detail_StoneChipA`

Explicitly not implemented in this cycle:

- No fullscreen pixelization pass.
- No dialogue font changes.
- No UI changes.
- No story, dialogue, route, save, or Time Window behavior changes.
- No paid asset purchase.
- No external asset download or Meshy/API generation.

## Verification

MCP boundary:

- No live Unity MCP resources were available in this Codex session.
- Unity Editor live MCP inspection was therefore unavailable for this pass.
- Verification used Unity batch methods, generated scene assertions, screenshot capture, build, and player smoke.

Scene regeneration:

- Command log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle5_create_scene_20260520.log`
- Result: success.
- Key line: `Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity`
- Output scene: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`

Batch validation:

- Command log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle5_validate_20260520.log`
- Result: success.
- Key line: `Fast VS house slice validation passed.`
- Validation covers generated texture dimensions, point filtering, repeat wrap mode, opaque color variation, material texture assignment, house door and library door texture assignment, current-only rubble detail object presence, absence of equivalent past rubble detail objects, and open-book page line presence.

Screenshot batch:

- Command log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle5_capture_20260520.log`
- Output directory: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_object_details_20260520`
- Captures:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_object_details_20260520\01_interior_niro_shadow.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_object_details_20260520\02_exterior_niro_shadow.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_object_details_20260520\03_library_reto_desk.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_object_details_20260520\04_library_reto_talk_loop.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_object_details_20260520\05_library_past_no_temp_people.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_object_details_20260520\06_library_dialogue_tmp_font.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_object_details_20260520\07_plaza_library_facade_current.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_object_details_20260520\08_plaza_library_facade_past.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_object_details_20260520\09_library_timewriter_pocket_glow.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_object_details_20260520\10_library_current_yellow_timewindow_cues.png`

Parent screenshot review:

- `02_exterior_niro_shadow.png`: Niro's exterior door now reads as a panelled wooden door rather than a generic plank slab.
- `07_plaza_library_facade_current.png`: current library door panels now have vertical board and inset detail while route glow remained unchanged.
- `03_library_reto_desk.png`: current library rubble gained small fragments and did not intrude on Reto or the player route.
- `01_interior_niro_shadow.png`: interior and open-book areas remained stable after page-line detail was added.

Windows build:

- Command log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle5_build_20260520.log`
- Result: success.
- Key lines: `Build Finished, Result: Success.` and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Build output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Player smoke:

- Player log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle5_player_smoke_20260520.log`
- Result: launched for 20 seconds, then was intentionally stopped.
- Checked patterns: `Error`, `Exception`, `NullReference`, `MissingReference`, `Failed`, `Crash`, `Font Atlas Texture`, `LiberationSans`, `ScreenSpaceAmbientOcclusion`, `DrawObjectsPass`, `RenderGraph`.
- Match count: 0.

Diff hygiene:

- Worker check: `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs` passed.
- Parent check: `git diff --check -- Assets/Editor docs/devlog` passed before commit selection.
- Unity-generated side effects in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\AddressableAssetsData` and `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\ProjectSettings` were removed before commit selection.

## Review Notes

This cycle improves readability where material changes alone were starting to plateau. The door panels now look authored, and current-only rubble has more object-level variety without adding new collision.

Known limitations after this cycle:

- Book page line details are small in the standard review camera; a closer camera or interaction-focused screenshot would better evaluate them.
- Door textures are still generated plates, so they read best at medium distance and may need bespoke asset art later.
- Current library rubble still needs more silhouette variation if the camera moves closer in a later production view.

Recommended next cycle:

- Add targeted close-review screenshot cameras for Niro's house, library door, Reto desk, and rubble so small detail passes can be reviewed without relying only on wide screenshots.
- After close-review cameras exist, iterate on the worst visible prop rather than continuing broad material passes.
