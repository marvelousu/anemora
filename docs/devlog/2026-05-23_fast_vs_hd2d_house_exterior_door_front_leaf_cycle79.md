# feat(hd2d): add house door front leaf seal

Cycle: 79  
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`  
Authored file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

## Goal

Cycle 78 narrowed the under-eave light plane, but close review still showed a blue-gray patch where the closed door should read. Cycle 79 adds a visual-only front door leaf so the exterior facade presents a sealed, closed wooden door from close and overview angles.

## Worker Scope

SCOPED_PROMPT_ISSUED cycle=79 authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle79ScreenshotsBatch`

Worker returned:

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - none
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle79ScreenshotsBatch
notes: Added the Cycle79 front-door veneer, validation, and no-player capture wiring inside the single authored file only, and avoided any scene/material/metadata edits outside scope.
```

## Implementation

- Added `Current_HouseExterior_Cycle79_DoorFrontLeafSealA` and `Past_HouseExterior_Cycle79_DoorFrontLeafSealA`.
- Added thin seam/band details to help the front leaf read as a wooden closed door rather than a flat block.
- Kept all added pieces visual-only and non-colliding.
- Added `ValidateFastVsHd2dShadowFoundationCycle79DoorFrontLeafSeal()`.
- Added `CaptureHd2dShadowFoundationCycle79ScreenshotsBatch()` for current/past close and overview evidence.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle79ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle79_door_front_leaf_parent_review_20260523_01\parent_review_01_current_house_exterior_door_front_leaf_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle79_door_front_leaf_parent_review_20260523_01\parent_review_02_past_house_exterior_door_front_leaf_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle79_door_front_leaf_parent_review_20260523_01\parent_review_03_current_house_exterior_door_front_leaf_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle79_door_front_leaf_parent_review_20260523_01\parent_review_04_past_house_exterior_door_front_leaf_overview.png`

## Review Notes

Parent visual review should confirm that the close image now shows a closed wooden door instead of a blue-gray patch, and that overview still reads as the same house exterior without exposing the interior.
