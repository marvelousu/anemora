# 2026-05-21 Fast VS HD2D Current Library Ruin Grounding Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_ruin_grounding_20260521`

This cycle adds a restrained current-library ruin grounding pass so the HD-2D library floor no longer reads as uniformly flat in the current timeline. The pass focuses on small floor debris, shelf-contact dust, a light entry chip, a back-shelf paper scrap, and a small floor book-spine prop.

The work does not touch past-library content, story flow, Time Window cues, map transitions, fonts, characters, Reto/Aria placement, book pickup behavior, or the lower-left guidance log.

No external, API, or paid assets were used. The pass uses existing Fast VS materials and code-generated non-colliding landmark cubes only.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateCurrentLibraryRuinGroundingPolish(...)` and wired it into `CreateLibrary(...)` only on the current branch.
- Added six current-library grounding objects:
  - `Current_Library_RuinGrounding_RetoDeskDustContactA`
  - `Current_Library_RuinGrounding_LeftShelfDustContactA`
  - `Current_Library_RuinGrounding_RightShelfDustContactA`
  - `Current_Library_RuinGrounding_EntryStoneChipA`
  - `Current_Library_RuinGrounding_BackShelfPaperScrapA`
  - `Current_Library_RuinGrounding_FloorBookSpineA`
- Kept the new pieces very small and thin, non-colliding, tagged as `TimeWindowPairedSpaceLandmarkKind.PropOrFeature`, and marked `countsForArrival = false` through `CreateNonArrivalLandmarkCube(...)`.
- Added `ValidateFastVsHd2dSixtyFourthCycleCurrentLibraryRuinGrounding()` and `ValidateCurrentLibraryRuinGroundingObject(...)`.
- Added `CaptureHd2dSixtyFourthCycleScreenshotsBatch()` and `CaptureHd2dSixtyFourthCycleScreenshotsToDirectory(...)`.
- Added the new cycle validation call to `ValidateHouseSliceBatch()`.

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`:

- Bumped the status/version line to `v6.28`.
- Increased root-level markdown coverage by 1.
- Increased screenshot coverage by 4.
- Increased the `2026-05-21` dated record count by 1.
- Added this cycle to the `2026-05-21` devlog table.

Added:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-21_fast_vs_hd2d_current_library_ruin_grounding_cycle.md`

## Validation

Worker validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle64_current_library_ruin_grounding_worker_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Worker screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle64_current_library_ruin_grounding_worker_capture_20260521.log`
- Result: passed and wrote the requested screenshots.

Parent review:

- The first worker capture made `03_current_library_side_shelf_grounding_close.png` read as mostly flat floor and let Niro block part of `04_current_library_entry_floor_grounding_close.png`.
- Adjusted only the Cycle64 review camera/player coordinates in `CaptureHd2dSixtyFourthCycleScreenshotsToDirectory(...)`.
- Regenerated and reviewed the screenshots. The side shelf contact and entry floor grounding are now visible without Niro blocking the close shot.
- No gameplay, story, Time Window, character, font, or map-transition behavior was changed during the parent fix.

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle64_current_library_ruin_grounding_parent_capture_r1_20260521.log`
- Result: passed and regenerated the four screenshots listed below.

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle64_current_library_ruin_grounding_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle64_current_library_ruin_grounding_parent_build_20260521.log`
- Result: passed and rebuilt `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`.

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle64_current_library_ruin_grounding_parent_smoke_20260521.log`
- Result: passed a 20 second batchmode startup smoke run. The process was intentionally stopped after the smoke window.

Unity licensing note:

- The logs still contain `[Licensing::Module] Error: Access token is unavailable; failed to update`. That is Unity licensing noise and did not block validation, capture, build, or smoke. It is not an Anemora API-token requirement.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_ruin_grounding_20260521\01_current_library_ruin_grounding_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_ruin_grounding_20260521\02_current_library_reto_desk_grounding_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_ruin_grounding_20260521\03_current_library_side_shelf_grounding_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_current_library_ruin_grounding_20260521\04_current_library_entry_floor_grounding_close.png`

## External Assets

No external, API, or paid assets were used.

## Residual Risk

- The pass is intentionally subtle, so the visual gain is incremental rather than a fully authored cleanup.
- The current overview and close framing still rely on the existing review camera setup, so the strongest cue may vary slightly by device or render timing.
- The new pieces are non-colliding and parented under the current library map root, but they remain thin enough that future lighting or framing changes could make them less noticeable.
