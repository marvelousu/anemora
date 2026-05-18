# 2026-05-18 Fast VS library shelf / window / objective cleanup

## Scope

- Project: `<repo>`
- Scene: `<repo>/Assets/Scenes/Anemora_FastVS_HouseSlice.unity`
- Build: `<repo>/Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`

## User Review Items

- Remove the box overlapping behind the left library side bookshelf.
- Stop both side bookshelves from penetrating the second-floor balcony.
- After the two past-library flags are complete and Niro is back in the current library, the guide should point to Reto's desk instead of still saying to return through the Time Window.
- Reduce the tile-like / texture-missing look on the library side window panels and the central plaza library facade windows.

## Worker Cycle

- Plan: identify exact generator objects, runtime objective branch, and validation hooks before editing.
- Worker instruction: gpt-5.4-mini worker `019e39f0-6a65-7150-b2f3-5e0f45359d1f` inspected the target files and reported patch points without editing.
- Worker result: the worker identified `Current_Library_Ruin_CollapsedShelfPile`, `CreateLibrarySideBookshelfFrame`, `CreateCentralPlazaLibraryFacadeWindow`, `ValidateCentralPlazaLibraryFacadeDetails`, `ValidateLibrarySideBookshelfFrame`, and the `waitingForRetoBookShow` objective branch as the relevant targets.
- Integrator review: the final patch follows those targets, adds validation for the new contracts, and rebuilds the EXE.

## Changes

- `<repo>/Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Removed `Current_Library_Ruin_CollapsedShelfPile`, the current-library left-side overlap box.
  - Lowered both current and past side bookshelf frames so their top caps stay below the second-floor balcony.
  - Lowered the side bookshelf front texture panels to match the shorter frame.
  - Changed central plaza library window panes so all four panes use the pane material rather than mixing in fence/frame material.
  - Added a dedicated `PixelPattern.Window` material pattern for `window_light` and `empty_window`, replacing the checker pattern that read as tile-like blocks.
  - Expanded validation for facade window pane materials, side bookshelf height, side bookshelf texture panel placement, and absence of the removed overlap box.
  - Added a story validation assertion that the post-past-flags current-side guide becomes `レトの机へ戻る。`.
- `<repo>/Assets/Scripts/FastVS/FastVsStoryFlowController.cs`
  - Centralized the `waitingForRetoBookShow` objective text.
  - Current side away from Reto now shows `レトの机へ戻る。`; near Reto shows `E: レトに本を見せる`; other-time side still tells the player to return through the Time Window.
  - Exposed runtime HUD objective text for editor validation.

## Verification

- Build and validation passed:
  - `<repo>/Logs/fast_vs_build_validate_20260518_library_shelf_window_objective_cleanup.log`
- Review screenshots regenerated:
  - `<repo>/Logs/fast_vs_capture_review_20260518_library_shelf_window_objective_cleanup.log`
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/03_library_reto_desk.png`
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/05_library_past_no_temp_people.png`
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/07_plaza_library_facade_current.png`
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/08_plaza_library_facade_past.png`
- Windows EXE updated:
  - `<repo>/Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`

## Notes

- The plaza facade window shapes are still simple blockout geometry, but the pane material is now consistent and no longer alternates with fence-colored panes.
- The current-side library side bookshelves remain intentionally empty; the past-side shelves keep the external front-facing bookshelf texture.
