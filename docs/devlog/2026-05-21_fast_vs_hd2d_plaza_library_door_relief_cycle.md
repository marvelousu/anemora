# 2026-05-21 Fast VS HD2D Plaza Library Door Relief Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Scene target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- Screenshot output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_door_relief_20260521`

This cycle improves the central plaza library exterior by adding shallow relief details to the large front double door. The scope is deliberately limited to door readability: center seam, plank bands, hinge/bracket cues, reveal strips, and low threshold/contact pieces.

No Meshy/API token, paid asset purchase, external download, or new third-party asset was used.

## Worker Cycle

- Worker: `gpt-5.4-mini` session `019e4a99-d8b0-7181-a14b-04375ca1677c`.
- Worker instruction: add a conservative door relief/readability pass, keep all pieces non-colliding and non-arrival, use existing door/fence/stone/shadow/dust material families, add validation and screenshot capture, and avoid roof/eave, route, story, UI, input, character, Time Window, and coordinate changes.
- Parent review: checked current/past close screenshots and the current overview. The double door reads more physical, the route glow remains unobstructed, and no roof/eave or player-path artifacts were introduced.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateCentralPlazaLibraryDoorReliefPolish(...)` and called it after the window reveal/depth pass.
- Added current and past door relief objects:
  - `*_CenterSeamA`
  - `*_UpperBandA`
  - `*_MidBandA`
  - `*_LowerBandA`
  - `*_LeftHingePlateA`
  - `*_RightHingePlateA`
  - `*_LeftRevealStripA`
  - `*_RightRevealStripA`
  - `*_LeftThresholdStripA`
  - `*_RightThresholdStripA`
  - `*_LeftBottomChipA`
  - `*_RightBottomChipA`
- Kept all added objects non-colliding and non-arrival via `CreateNonArrivalLandmarkCubeShadowSafe(...)`.
- Used existing `current_library_door_detail`, `past_library_door_detail`, fence, stone, shadow, dust, and exterior wall material families; explicitly avoided bright `window_light` and `warm_light` materials.
- Added `ValidateFastVsHd2dOneHundredFourthCyclePlazaLibraryDoorRelief()`.
- Added `ValidateCentralPlazaLibraryDoorReliefObject(...)`, checking parent, renderer/material, no collider, no shadows, landmark id prefix, PropOrFeature kind, non-arrival status, placement range, scale range, and no bright light material.
- Added `CaptureHd2dOneHundredFourthCycleScreenshotsBatch()` and `CaptureHd2dOneHundredFourthCycleScreenshotsToDirectory(...)`.

The Unity scene was regenerated so the door relief objects are present in the checked-in scene.

## Validation

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle104_door_relief_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle104_door_relief_parent_capture_20260521.log`
- Result: passed with `Fast VS one-hundred-fourth-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_door_relief_20260521`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle104_door_relief_parent_build_20260521.log`
- Result: passed with `Build Finished, Result: Success.` and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle104_door_relief_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_door_relief_20260521\01_current_plaza_library_door_relief_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_door_relief_20260521\02_past_plaza_library_door_relief_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_door_relief_20260521\03_current_plaza_library_door_relief_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_door_relief_20260521\04_past_plaza_library_door_relief_close.png`

## Notes

- This pass deliberately avoids roof/eave changes because the earlier roof-thickness direction produced visible artifacts.
- The new threshold/contact pieces are non-colliding and do not change route movement or map transition behavior.
- Unity batchmode produced transient Addressables/ProjectSettings/importer/material changes during validation and build; these were excluded from the commit so only the intended script, regenerated scene, devlog, and screenshots remain.
