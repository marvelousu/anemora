# 2026-05-21 Fast VS HD2D Plaza Library Entry Plinth Depth Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Scene target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- Screenshot output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_entry_plinth_depth_20260521`

This cycle improves the central plaza library exterior by adding low entrance plinth and step-depth cues. It intentionally avoids roof/eave edits after a rejected local roof-edge attempt produced visible artifacts.

No Meshy/API token, paid asset purchase, external download, or new third-party asset was used.

## Worker Cycle

- Worker: `gpt-5.4-mini` session `019e4a99-d8b0-7181-a14b-04375ca1677c`.
- Worker instruction: avoid the roof/eave geometry path, add a safer entry plinth/step depth pass around the library entrance, keep all new objects non-colliding PropOrFeature objects, avoid bright materials, and do not touch gameplay, story, Time Window, route transitions, route lights, UI, font, input, characters, or map coordinates.
- Parent review: validated that the orange route glow remains visible, the added low stonework reads as entry grounding rather than a route obstruction, and no roof artifacts were introduced.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateCentralPlazaLibraryEntryPlinthDepthPolish(...)` and called it after `CreatePlazaLibraryEntryDepthPolish(...)`.
- Added current and past entry-depth objects:
  - `*_FrontRiserA`
  - `*_FrontContactStripA`
  - `*_WestSidePlinthA`
  - `*_EastSidePlinthA`
  - `*_WestSideCapA`
  - `*_EastSideCapA`
  - `*_WestReturnStoneA`
  - `*_EastReturnStoneA`
  - `*_RearThresholdTrimA`
  - `*_RearDustBandA`
- Kept all added objects non-colliding and non-arrival via `CreateNonArrivalLandmarkCubeShadowSafe(...)`.
- Used only muted stone/fence/exterior wall/dust/shadow material families, explicitly avoiding `window_light` and `warm_light`.
- Added `ValidateFastVsHd2dOneHundredFirstCyclePlazaLibraryEntryPlinthDepth()`.
- Added `ValidateCentralPlazaLibraryEntryPlinthDepthObject(...)`, checking parent, renderer/material, no collider, no shadows, landmark id prefix, PropOrFeature kind, non-arrival status, placement range, scale range, and no bright light material.
- Added `CaptureHd2dOneHundredFirstCycleScreenshotsBatch()` and `CaptureHd2dOneHundredFirstCycleScreenshotsToDirectory(...)`.

The Unity scene was regenerated so the entry plinth objects are present in the checked-in scene.

## Validation

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle101_entry_plinth_depth_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle101_entry_plinth_depth_parent_capture_20260521.log`
- Result: passed with `Fast VS one-hundred-first-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_entry_plinth_depth_20260521`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle101_entry_plinth_depth_parent_build_20260521.log`
- Result: passed with `Build Finished, Result: Success.` and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle101_entry_plinth_depth_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_entry_plinth_depth_20260521\01_current_plaza_library_entry_plinth_depth_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_entry_plinth_depth_20260521\02_past_plaza_library_entry_plinth_depth_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_entry_plinth_depth_20260521\03_current_plaza_library_entry_plinth_depth_oblique.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_entry_plinth_depth_20260521\04_past_plaza_library_entry_plinth_depth_oblique.png`

## Notes

- A roof/eave thickness attempt was explicitly reverted before this cycle was committed because it produced visible roof artifacts in the oblique screenshot.
- The final chosen scope is intentionally smaller but safer: the library entrance gains a more grounded, layered approach without risking roof silhouette artifacts.
- Unity batchmode produced transient Addressables/ProjectSettings/importer/material changes during validation and build; these were excluded from the commit so only the intended script, regenerated scene, devlog, and screenshots remain.
