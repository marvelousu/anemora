# test(hd2d): add house door sightline diagnostics

Cycle: 81  
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`  
Authored file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

## Goal

Cycle 80 confirmed that transparent depth-testing alone did not change the problematic close review. Cycle 81 adds diagnostic no-player sightline captures so we can distinguish a real facade geometry issue from a close-review camera/framing issue before making another visual change.

## Worker Scope

SCOPED_PROMPT_ISSUED cycle=81 authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle81ScreenshotsBatch`

Worker returned:

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - none
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle81ScreenshotsBatch
notes: Kept the entire change inside the authored editor file, added only diagnostic validation/capture helpers, and avoided any scene/material/asset edits.
```

## Implementation

- Added `ValidateFastVsHd2dShadowFoundationCycle81HouseDoorSightlineDiagnostics()`.
- Validates that the Cycle79 front leaf remains visible, non-colliding, and forward of the original closed door panel.
- Added `CaptureHd2dShadowFoundationCycle81ScreenshotsBatch()`.
- Captures current/past medium, lower-close, and oblique-left no-player door sightlines.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle81ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle81_door_sightline_parent_review_20260523_01\parent_review_01_current_house_exterior_door_sightline_medium.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle81_door_sightline_parent_review_20260523_01\parent_review_02_current_house_exterior_door_sightline_lower_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle81_door_sightline_parent_review_20260523_01\parent_review_03_current_house_exterior_door_sightline_oblique_left.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle81_door_sightline_parent_review_20260523_01\parent_review_04_past_house_exterior_door_sightline_medium.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle81_door_sightline_parent_review_20260523_01\parent_review_05_past_house_exterior_door_sightline_lower_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle81_door_sightline_parent_review_20260523_01\parent_review_06_past_house_exterior_door_sightline_oblique_left.png`

## Review Notes

Parent visual review should use these captures to decide whether the blue-gray door close image is caused by the house facade itself or by the previous close-review camera path intersecting/looking through porch layers.
