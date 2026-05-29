# 2026-05-21 Fast VS HD2D House Interior Prop Readability Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_interior_prop_readability_20260521`

This cycle adds a deterministic visual-only HD-2D readability pass for Niro's house interior. It keeps gameplay untouched and only sharpens the authored read on the bed textiles, tabletop book/papers, and right-side shelf/broken stack.

No API token, no paid asset purchase, and no external download was used.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Wired `CreateHouseInteriorPropReadabilityPolish(...)` into `CreateInterior(...)` after the existing current/past prop branch and before furniture grounding polish.
- Added `CaptureHd2dSeventyEighthCycleScreenshotsBatch()` and `CaptureHd2dSeventyEighthCycleScreenshotsToDirectory(...)`.
- Added `ValidateFastVsHd2dSeventyEighthCycleHouseInteriorPropReadability()`.
- Added `ValidateHouseInteriorPropReadabilityObject(...)`.
- Added `ValidateVectorWithinRange(...)`.

New visual-only objects:

- `Current_HouseInterior_PropReadability_BedBlanketEdgeHighlightA`
- `Current_HouseInterior_PropReadability_BedPillowShadowA`
- `Current_HouseInterior_PropReadability_TableBookPageEdgeA`
- `Current_HouseInterior_PropReadability_TableBookSpineA`
- `Current_HouseInterior_PropReadability_TablePaperCornerA`
- `Current_HouseInterior_PropReadability_ShelfBrokenSideEdgeA`
- `Current_HouseInterior_PropReadability_ShelfDustLineA`
- `Past_HouseInterior_PropReadability_BedBlanketEdgeHighlightA`
- `Past_HouseInterior_PropReadability_BedPillowShadowA`
- `Past_HouseInterior_PropReadability_TableBookPageEdgeA`
- `Past_HouseInterior_PropReadability_TableBookSpineA`
- `Past_HouseInterior_PropReadability_TablePaperCornerA`
- `Past_HouseInterior_PropReadability_ShelfSideEdgeA`
- `Past_HouseInterior_PropReadability_ShelfWarmLineA`

## Validation

Worker validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle78_house_interior_prop_readability_worker_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Worker screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle78_house_interior_prop_readability_worker_capture_20260521.log`
- Result: passed with `Fast VS seventy-eighth-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_interior_prop_readability_20260521`

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle78_house_interior_prop_readability_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle78_house_interior_prop_readability_parent_capture_20260521.log`
- Result: passed with `Fast VS seventy-eighth-cycle screenshots captured`.

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle78_house_interior_prop_readability_parent_build_20260521.log`
- Result: passed with `Build Finished, Result: Success.`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle78_house_interior_prop_readability_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_interior_prop_readability_20260521\01_current_house_interior_bed_table_prop_readability.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_interior_prop_readability_20260521\02_past_house_interior_bed_table_prop_readability.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_interior_prop_readability_20260521\03_current_house_interior_shelf_prop_readability.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_interior_prop_readability_20260521\04_past_house_interior_shelf_prop_readability.png`

## Notes

- The cycle stayed deterministic and visual-only.
- The work ran through Unity validation and screenshot capture successfully.
- Unity also produced unrelated auto-diffs in scene/material/settings files outside the ownership list; those were cleaned before committing this cycle.
