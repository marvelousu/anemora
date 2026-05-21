# 2026-05-21 Fast VS HD2D Plaza Library Window Reveal Depth Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Scene target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- Screenshot output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_window_reveal_depth_20260521`

This cycle improves the central plaza library exterior by adding restrained reveal/depth pieces around the left and right facade windows. The goal is to make the windows read less flat without touching the rejected roof/eave direction or changing any gameplay surface.

No Meshy/API token, paid asset purchase, external download, or new third-party asset was used.

## Worker Cycle

- Worker: `gpt-5.4-mini` session `019e4a99-d8b0-7181-a14b-04375ca1677c`.
- Worker instruction: add only a conservative library window reveal/depth polish pass, keep all new pieces non-colliding and non-arrival, use muted existing material families, add validation and screenshot capture, and avoid roof/eave, route, story, UI, input, character, Time Window, and coordinate changes.
- Parent review: checked current/past overview and current oblique screenshots. The added pieces increase facade depth around the windows, keep the library entrance and route glow clear, and do not introduce visible roof artifacts or floating route blockers.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateCentralPlazaLibraryWindowRevealDepthPolish(...)` and called it after the lower-facade grounding pass.
- Added current and past window reveal/depth objects:
  - `*_LeftRevealOuterStripA`
  - `*_LeftRevealInnerStripA`
  - `*_LeftTopLipA`
  - `*_LeftBottomSupportA`
  - `*_LeftBottomChipA`
  - `*_RightRevealOuterStripA`
  - `*_RightRevealInnerStripA`
  - `*_RightTopLipA`
  - `*_RightBottomSupportA`
  - `*_RightBottomChipA`
- Kept all added objects non-colliding and non-arrival via `CreateNonArrivalLandmarkCubeShadowSafe(...)`.
- Used muted wall, fence/trim, stone, shadow, and dust material families; explicitly avoided bright `window_light` and `warm_light` materials.
- Added `ValidateFastVsHd2dOneHundredThirdCyclePlazaLibraryWindowRevealDepth()`.
- Added `ValidateCentralPlazaLibraryWindowRevealDepthObject(...)`, checking parent, renderer/material, no collider, no shadows, landmark id prefix, PropOrFeature kind, non-arrival status, placement range, scale range, and no bright light material.
- Added `CaptureHd2dOneHundredThirdCycleScreenshotsBatch()` and `CaptureHd2dOneHundredThirdCycleScreenshotsToDirectory(...)`.

The Unity scene was regenerated so the window reveal/depth objects are present in the checked-in scene.

## Validation

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle103_window_reveal_depth_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle103_window_reveal_depth_parent_capture_20260521.log`
- Result: passed with `Fast VS one-hundred-third-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_window_reveal_depth_20260521`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle103_window_reveal_depth_parent_build_20260521.log`
- Result: passed with `Build Finished, Result: Success.` and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle103_window_reveal_depth_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_window_reveal_depth_20260521\01_current_plaza_library_window_reveal_depth_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_window_reveal_depth_20260521\02_past_plaza_library_window_reveal_depth_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_window_reveal_depth_20260521\03_current_plaza_library_window_reveal_depth_oblique.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_window_reveal_depth_20260521\04_past_plaza_library_window_reveal_depth_oblique.png`

## Notes

- This pass deliberately avoids roof/eave changes because the earlier roof-thickness direction produced visible artifacts.
- The new objects are intentionally small relief pieces. They are not collision, route, or story objects.
- Unity batchmode produced transient Addressables/ProjectSettings/importer/material changes during validation and build; these were excluded from the commit so only the intended script, regenerated scene, devlog, and screenshots remain.
