# 2026-05-21 Fast VS HD2D Plaza Library Lower Facade Grounding Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Scene target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- Screenshot output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_lower_facade_grounding_20260521`

This cycle improves the central plaza library exterior by adding small lower-facade grounding details around the entrance base, side pilaster bottoms, and lower window support areas. It follows the safer post-roof-attempt direction: make the library read as more physically grounded without changing route lights, collision, story, UI, input, Time Window behavior, or map coordinates.

No Meshy/API token, paid asset purchase, external download, or new third-party asset was used.

## Worker Cycle

- Worker: `gpt-5.4-mini` session `019e4a99-d8b0-7181-a14b-04375ca1677c`.
- Worker instruction: add a conservative lower-facade grounding pass for the central plaza library exterior, use only existing muted material families, keep all objects non-colliding and non-arrival, add validation and screenshots, and avoid gameplay/story/UI/Time Window/route changes.
- Parent review: checked current and past overview/oblique screenshots and accepted the pass because the added base pieces read as subtle grounding, the route glow remains visible, and no obvious floating bars, roof artifacts, or collision hazards were introduced.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateCentralPlazaLibraryLowerFacadeGroundingPolish(...)` and called it after the entry plinth depth pass.
- Added current and past lower-facade objects:
  - `*_LeftPilasterBaseA`
  - `*_LeftPilasterBaseB`
  - `*_RightPilasterBaseA`
  - `*_RightPilasterBaseB`
  - `*_LeftWindowSillSupportA`
  - `*_RightWindowSillSupportA`
  - `*_LeftBottomContactChipA`
  - `*_RightBottomContactChipA`
  - `*_LeftVerticalBaseStripA`
  - `*_RightVerticalBaseStripA`
  - `*_CenterBottomContactChipA`
- Kept all added objects non-colliding and non-arrival via `CreateNonArrivalLandmarkCubeShadowSafe(...)`.
- Used muted stone, trim, wall, shadow, and dust material families; explicitly avoided bright `window_light` and `warm_light` materials.
- Added `ValidateFastVsHd2dOneHundredSecondCyclePlazaLibraryLowerFacadeGrounding()`.
- Added `ValidateCentralPlazaLibraryLowerFacadeGroundingObject(...)`, checking parent, renderer/material, no collider, no shadows, landmark id prefix, PropOrFeature kind, non-arrival status, placement range, scale range, and no bright light material.
- Added `CaptureHd2dOneHundredSecondCycleScreenshotsBatch()` and `CaptureHd2dOneHundredSecondCycleScreenshotsToDirectory(...)`.

The Unity scene was regenerated so the lower-facade grounding objects are present in the checked-in scene.

## Validation

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle102_lower_facade_grounding_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle102_lower_facade_grounding_parent_capture_20260521.log`
- Result: passed with `Fast VS one-hundred-second-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_lower_facade_grounding_20260521`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle102_lower_facade_grounding_parent_build_20260521.log`
- Result: passed with `Build Finished, Result: Success.` and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle102_lower_facade_grounding_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_lower_facade_grounding_20260521\01_current_plaza_library_lower_facade_grounding_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_lower_facade_grounding_20260521\02_past_plaza_library_lower_facade_grounding_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_lower_facade_grounding_20260521\03_current_plaza_library_lower_facade_grounding_oblique.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_lower_facade_grounding_20260521\04_past_plaza_library_lower_facade_grounding_oblique.png`

## Notes

- This pass intentionally stays small after the rejected roof/eave attempt. The library gains more base contact and lower-facade density without changing the main silhouette.
- Unity batchmode produced transient Addressables/ProjectSettings/importer/material changes during validation and build; these were excluded from the commit so only the intended script, regenerated scene, devlog, and screenshots remain.
