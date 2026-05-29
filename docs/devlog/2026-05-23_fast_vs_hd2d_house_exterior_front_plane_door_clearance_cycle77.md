# feat(hd2d): clear house front plane from door

Cycle: 77  
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`  
Authored file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

## Goal

Cycle 76 confirmed the no-player door review camera, but the close screenshot still showed a large front-plane wall piece covering the closed door. Cycle 77 moves that front-plane cap above the doorway so the exterior reads as a closed facade without exposing the interior or hiding the door face.

## Worker Scope

SCOPED_PROMPT_ISSUED cycle=77 authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle77ScreenshotsBatch`

Worker returned:

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - none
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle77ScreenshotsBatch
notes: Kept the change confined to the authored editor file, avoided scenes/materials/project settings, and added only deterministic cycle-77 validation/capture wiring plus the door-clearance geometry adjustment.
```

## Implementation

- Shrunk `Current_HouseExterior_HeroReadability_FrontPlaneCapA` and `Past_HouseExterior_HeroReadability_FrontPlaneCapA` from a tall door-covering slab into an upper lintel/backing strip.
- Added narrow left/right return pieces beside the cap so the facade still has a readable front edge.
- Updated the existing facade-backdrop validation bounds for the new front-plane cap geometry.
- Added `ValidateFastVsHd2dShadowFoundationCycle77FrontPlaneDoorClearance()`.
- Added `CaptureHd2dShadowFoundationCycle77ScreenshotsBatch()` for current/past close and overview evidence.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle77ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle77_front_plane_parent_review_20260523_01\parent_review_01_current_house_exterior_front_plane_door_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle77_front_plane_parent_review_20260523_01\parent_review_02_past_house_exterior_front_plane_door_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle77_front_plane_parent_review_20260523_01\parent_review_03_current_house_exterior_front_plane_door_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle77_front_plane_parent_review_20260523_01\parent_review_04_past_house_exterior_front_plane_door_overview.png`

## Review Notes

Parent visual review should confirm that the closed exterior door is visible in the close shot, the interior is no longer visible through the side gap, and the doorway is not replaced by a black or blue-gray slab.
