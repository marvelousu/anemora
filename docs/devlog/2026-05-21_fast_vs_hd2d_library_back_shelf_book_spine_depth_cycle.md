# 2026-05-21 Fast VS HD2D Library Back Shelf Book-Spine Depth Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Scene target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- Screenshot output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_back_shelf_book_spine_depth_20260521`

This cycle shifts from the plaza exterior back to the library interior. It adds shallow book-spine and shelf-depth relief to the back wall shelf while preserving the current/past contrast: the past shelf is organized and book-filled, while the current shelf remains ruined and sparse.

No Meshy/API token, paid asset purchase, external download, or new third-party asset was used.

## Worker Cycle

- Worker: `gpt-5.4-mini` session `019e4a99-d8b0-7181-a14b-04375ca1677c`.
- Worker instruction: add only a conservative back-shelf book-spine/depth pass, keep every object non-colliding and non-arrival, preserve current/past differences, add validation and screenshot capture, and avoid gameplay, story, Time Window, route, UI, input, character, and coordinate changes.
- Parent review/fix: corrected the current-side center shadow chip material from dust to shadow so implementation and validation matched. The close screenshot camera was also adjusted lower for clearer shelf evidence, though the upper void remains visible in close shots.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateLibraryBackShelfBookSpineDepthPolish(...)` and called it after `CreateLibraryBackBookshelfFramePolish(...)`.
- Added past back-shelf objects:
  - `*_LeftClusterSpineA`
  - `*_LeftClusterSpineB`
  - `*_LeftClusterSpineC`
  - `*_CenterClusterSpineA`
  - `*_CenterClusterSpineB`
  - `*_CenterClusterSpineC`
  - `*_RightClusterSpineA`
  - `*_RightClusterSpineB`
  - `*_RightClusterSpineC`
  - `*_TopShelfLipA`
  - `*_LowerContactBandA`
- Added current back-shelf objects:
  - `*_LeftLooseSpineA`
  - `*_RightLooseSpineA`
  - `*_LeftBrokenBandA`
  - `*_CenterDustBandA`
  - `*_CenterShadowChipA`
  - `*_RightBrokenBandA`
  - `*_TopShelfLipA`
  - `*_LowerContactBandA`
- Kept all added objects non-colliding and non-arrival via `CreateNonArrivalLandmarkCubeShadowSafe(...)`.
- Used book, furniture/wood, fence/trim, dust, and shadow material families; explicitly avoided `window_light`, `warm_light`, `lamp`, and `red_light`.
- Added `ValidateFastVsHd2dOneHundredFifthCycleLibraryBackShelfBookSpineDepth()`.
- Added `ValidateLibraryBackShelfBookSpineDepthObject(...)`, checking parent, renderer/material, no collider, no shadows, landmark id prefix, PropOrFeature kind, non-arrival status, placement range, scale range, and no bright light material.
- Added `CaptureHd2dOneHundredFifthCycleScreenshotsBatch()` and `CaptureHd2dOneHundredFifthCycleScreenshotsToDirectory(...)`.

The Unity scene was regenerated so the back-shelf book-spine/depth objects are present in the checked-in scene.

## Validation

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle105_back_shelf_book_spine_depth_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle105_back_shelf_book_spine_depth_parent_capture_20260521.log`
- Result: passed with `Fast VS one-hundred-fifth-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_back_shelf_book_spine_depth_20260521`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle105_back_shelf_book_spine_depth_parent_build_20260521.log`
- Result: passed with `Build Finished, Result: Success.` and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle105_back_shelf_book_spine_depth_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_back_shelf_book_spine_depth_20260521\01_current_library_back_shelf_book_spine_depth_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_back_shelf_book_spine_depth_20260521\02_past_library_back_shelf_book_spine_depth_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_back_shelf_book_spine_depth_20260521\03_current_library_back_shelf_book_spine_depth_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_back_shelf_book_spine_depth_20260521\04_past_library_back_shelf_book_spine_depth_close.png`

## Notes

- The close screenshots retain a large dark upper area because the shot is aimed at the high back-shelf wall; the overview shots provide the better full-room read.
- The added shelf details do not change Reto, Aria, event book, interaction, route, or Time Window behavior.
- Unity batchmode produced transient Addressables/ProjectSettings/importer/material changes during validation and build; these were excluded from the commit so only the intended script, regenerated scene, devlog, and screenshots remain.
