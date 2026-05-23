# feat(hd2d): blend house door jamb

Cycle: 82  
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`  
Authored file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

## Goal

Cycle 81 showed that gameplay-like lower and oblique views read the house door correctly, but the left door edge still looked like a detached raw wood slab. Cycle 82 blends that jamb into the facade with a narrow occlusion strip and wall-colored transition pieces.

## Worker Scope

SCOPED_PROMPT_ISSUED cycle=82 authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle82ScreenshotsBatch`

Worker returned:

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - none
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle82ScreenshotsBatch
notes: I kept the scope to the authored Unity editor file only, and avoided any scene/material/asset edits outside that file while updating the doorway fill, adding cycle 82 blend pieces, and wiring the new validate/capture entry points.
```

## Implementation

- Repositioned `HouseExterior_HeroReadability_DoorwayDarkFillA` into a narrow occlusion strip using the outdoor occlusion gradient material.
- Added `Cycle82_LeftDoorJambWallBlendA` to make the left jamb read as wall/trim instead of a detached plank.
- Added `Cycle82_LeftDoorJambTopShadowA` under the awning.
- Updated the Cycle74 doorway depth validation to match the new narrow occlusion-strip role.
- Added `ValidateFastVsHd2dShadowFoundationCycle82DoorJambBlend()`.
- Added `CaptureHd2dShadowFoundationCycle82ScreenshotsBatch()` using the Cycle81 gameplay-like lower/oblique review views.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle82ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle82_door_jamb_blend_parent_review_20260523_01\parent_review_01_current_house_exterior_door_jamb_lower_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle82_door_jamb_blend_parent_review_20260523_01\parent_review_02_current_house_exterior_door_jamb_oblique_left.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle82_door_jamb_blend_parent_review_20260523_01\parent_review_03_past_house_exterior_door_jamb_lower_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle82_door_jamb_blend_parent_review_20260523_01\parent_review_04_past_house_exterior_door_jamb_oblique_left.png`

## Review Notes

Parent visual review should compare against Cycle81 lower/oblique shots and confirm that the left door edge no longer reads as a loose raw plank while preserving the closed-door silhouette.
