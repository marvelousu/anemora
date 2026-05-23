# feat(hd2d): tighten house door light plane clearance

Cycle: 78  
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`  
Authored file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

## Goal

Cycle 77 moved the front-plane cap away from the closed door, but the close review still showed a large blue-gray translucent panel across the doorway. Cycle 78 narrows the house exterior under-eave occlusion light plane so it acts as a shadow band rather than a door-covering panel.

## Worker Scope

SCOPED_PROMPT_ISSUED cycle=78 authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle78ScreenshotsBatch`

Worker returned:

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - none
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle78ScreenshotsBatch
notes: Kept the change scoped to the single authored editor file and reused existing no-player capture helpers without touching scenes, materials, or asset metadata.
```

## Implementation

- Reduced `Current_HouseExterior_FramedLightPlanes_UnderEaveOcclusionGradientA` from a tall facade overlay to a narrow under-eave band.
- Reduced `Past_HouseExterior_FramedLightPlanes_UnderEaveOcclusionGradientA` to the same narrow role.
- Updated `ValidateFastVsHd2dOneHundredTwelfthCycleOutdoorFramedLightPlanes()` to match the new geometry.
- Added `ValidateFastVsHd2dShadowFoundationCycle78HouseDoorLightPlaneClearance()`.
- Added `CaptureHd2dShadowFoundationCycle78ScreenshotsBatch()` for no-player close/overview evidence.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle78ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle78_door_light_plane_parent_review_20260523_01\parent_review_01_current_house_exterior_no_player_door_light_plane_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle78_door_light_plane_parent_review_20260523_01\parent_review_02_past_house_exterior_no_player_door_light_plane_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle78_door_light_plane_parent_review_20260523_01\parent_review_03_current_house_exterior_no_player_door_light_plane_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle78_door_light_plane_parent_review_20260523_01\parent_review_04_past_house_exterior_no_player_door_light_plane_overview.png`

## Review Notes

Parent visual review should verify that the closed door face remains visible in the close image, the under-eave shadow still reads as a shadow band, and the facade no longer has a full-height blue-gray panel across the doorway.
