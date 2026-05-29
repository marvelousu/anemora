# 2026-05-20 Fast VS HD2D Character Contact Shadow Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_character_contact_shadow_20260520`

This cycle tightens the HD-2D grounding for Niro, Reto, and Aria by adding subtle character contact shadows. It does not change story, dialogue, font, UI, Time Window behavior, route triggers, door/area transitions, camera runtime logic, map geometry scale, character animation state machines, or colliders. Meshy and other external assets were not used.

## Implementation

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added a reusable `CreateCharacterContactShadow(...)` quad helper for horizontal, non-colliding contact shadows.
- Generalized the shadow material path through `EnsureCharacterContactShadowMaterial(...)` and `EnsureCharacterContactShadowTexture(...)`.
- Kept `FastVS_PlayerContactShadow_Niro` under the player and tightened its scale and opacity.
- Added `Current_Library_Reto_ContactShadow` under `Current_LibraryMap_SeparateSpace`.
- Added `Past_Library_Aria_ContactShadow` under `Past_LibraryMap_SeparateSpace`.
- Added `ValidateFastVsHd2dSeventeenthCycleCharacterContactShadows()` and wired it into `ValidateHouseSliceBatch()`.
- Added `CaptureHd2dSeventeenthCycleScreenshotsBatch()` and a matching directory capture helper.

Generated/updated local assets:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_character_contact_shadow.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_niro_contact_shadow.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_reto_contact_shadow.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_aria_contact_shadow.mat`

Representative added objects:

- `FastVS_PlayerContactShadow_Niro`
- `Current_Library_Reto_ContactShadow`
- `Past_Library_Aria_ContactShadow`

## Verification Plan

- Validate with Unity batch mode and write the log to `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle17_worker_validate_20260520.log`.
- Capture screenshots with Unity batch mode and write the log to `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle17_worker_capture_20260520.log`.
- Confirm the three contact-shadow objects exist, keep renderers/materials, remain collider-free, stay horizontal, and keep the expected parent relationships.
- Confirm the screenshots show the character placement cues without reopening the earlier black-rectangle problem.

## Verification Results

- Validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle17_worker_validate_20260520.log`
- Result: passed. Unity completed the batch run without validation exceptions.
- Screenshot capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle17_worker_capture_20260520.log`
- Result: passed. Unity exited cleanly after writing the PNGs.
- Screenshot folder: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_character_contact_shadow_20260520`
- Captured screenshots:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_character_contact_shadow_20260520\01_interior_niro_contact_shadow.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_character_contact_shadow_20260520\02_exterior_niro_contact_shadow.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_character_contact_shadow_20260520\03_library_reto_contact_shadow.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_character_contact_shadow_20260520\04_past_library_aria_contact_shadow.png`

## Visual Review

- Reviewed the four generated PNGs.
- The contact shadows read as soft grounding cues rather than flat black rectangles.
- The library frames are the closest to the edge of being a little directional, but they still stay subtle and do not hide the desk or character silhouettes.

## Parent Review

- Parent adjusted only the first screenshot camera/player review position so the Niro interior contact shadow is easier to inspect without bed/wall occlusion. Gameplay start position, route logic, and player placement were not changed.
- Parent validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle17_parent_validate_20260520.log`
- Parent validation result: passed.
- Parent screenshot capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle17_parent_capture_20260520.log`
- Parent screenshot capture result: passed.
- Parent build log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle17_build_20260520.log`
- Parent build result: passed, with `Build Finished, Result: Success.`
- Player smoke log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle17_player_smoke_20260520.log`
- Player smoke result: passed. The process was stopped after 20 seconds as planned and produced `match_count=0` for the runtime error scan.
- Known benign log noise: Unity batchmode still emits the licensing access-token warning and `LogAssemblyErrors` timing lines in this environment.
