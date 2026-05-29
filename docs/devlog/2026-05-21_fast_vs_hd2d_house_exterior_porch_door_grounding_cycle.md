# 2026-05-21 Fast VS HD2D House Exterior Porch Door Grounding Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Scene target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- Screenshot output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_porch_door_grounding_20260521`

This cycle returns to the early VS route and adds shallow grounding details around Niro house exterior porch/door. The intent is to make the exterior door and porch feel more physical without changing the door transition, route glow pads, triggers, or coordinates.

No Meshy/API token, paid asset purchase, external download, or new third-party asset was used.

## Worker Cycle

- Worker: `gpt-5.4-mini` session `019e4a99-d8b0-7181-a14b-04375ca1677c`.
- Worker instruction: add a conservative porch/door grounding pass, keep all pieces non-colliding and non-arrival, preserve map-move glow pads, add validation and screenshot capture, and avoid gameplay, story, Time Window, route, UI, input, character, and coordinate changes.
- Parent review/fix: corrected the worker call site to use the actual `CreateExterior(...)` material locals by adding `stone` and `trim` derived from current/past stone/fence materials. Also corrected validator position checks to compare against `HouseExteriorCenter`-relative offsets.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateHouseExteriorPorchDoorGroundingPolish(...)` and called it after existing house exterior facade/path/eave polish passes.
- Added current and past porch/door grounding objects:
  - `*_CenterDoorSeamA`
  - `*_TopFrameContactA`
  - `*_LeftFrameBaseA`
  - `*_RightFrameBaseA`
  - `*_LeftThresholdStripA`
  - `*_RightThresholdStripA`
  - `*_LeftStepShadowA`
  - `*_RightStepShadowA`
  - current-only `*_LeftDoorWearA`
  - current-only `*_RightDoorWearA`
  - past-only `*_LeftDoorChipA`
  - past-only `*_RightDoorChipA`
- Kept all added objects non-colliding and non-arrival via `CreateNonArrivalLandmarkCubeShadowSafe(...)`.
- Used house door detail, fence/trim, stone, dust, and shadow material families; explicitly avoided `window_light` and `warm_light`.
- Added `ValidateFastVsHd2dOneHundredSeventhCycleHouseExteriorPorchDoorGrounding()`.
- Added `ValidateHouseExteriorPorchDoorGroundingObject(...)`, checking parent, renderer/material, no collider, no shadows, landmark id prefix, PropOrFeature kind, non-arrival status, placement range, scale range, and no bright light material.
- Added route-glow existence checks for house exterior map move pads in the Cycle107 validation.
- Added `CaptureHd2dOneHundredSeventhCycleScreenshotsBatch()` and `CaptureHd2dOneHundredSeventhCycleScreenshotsToDirectory(...)`.

The Unity scene was regenerated so the porch/door grounding objects are present in the checked-in scene.

## Validation

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle107_house_exterior_porch_door_grounding_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle107_house_exterior_porch_door_grounding_parent_capture_20260521.log`
- Result: passed with `Fast VS one-hundred-seventh-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_porch_door_grounding_20260521`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle107_house_exterior_porch_door_grounding_parent_build_20260521.log`
- Result: passed with `Build Finished, Result: Success.` and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle107_house_exterior_porch_door_grounding_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_porch_door_grounding_20260521\01_current_house_exterior_porch_door_grounding_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_porch_door_grounding_20260521\02_past_house_exterior_porch_door_grounding_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_porch_door_grounding_20260521\03_current_house_exterior_porch_door_grounding_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_porch_door_grounding_20260521\04_past_house_exterior_porch_door_grounding_close.png`

## Notes

- The screenshots show an existing small red marker in the exterior area; this cycle did not introduce that marker and does not change marker/UI behavior.
- The new threshold and shadow pieces are non-colliding and validated to coexist with the house exterior door and route glow objects.
- Unity batchmode produced transient Addressables/ProjectSettings/importer/material changes during validation and build; these were excluded from the commit so only the intended script, regenerated scene, devlog, and screenshots remain.
