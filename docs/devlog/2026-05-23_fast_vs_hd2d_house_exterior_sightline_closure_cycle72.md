# feat(hd2d): close house exterior sightline gaps

Cycle: 72  
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`  
Authored file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

## Goal

Fix the current house exterior review failure where the doorway / porch area still reads as an open side gap or a dark slab from the outside. This cycle deliberately does not tune global shadow opacity; it closes the facade sightline first and adds a review capture that actually frames the doorway.

## Worker Scope

SCOPED_PROMPT_ISSUED cycle=72 authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle72ScreenshotsBatch`

Worker returned:

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - none
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle72ScreenshotsBatch
notes: Kept the change confined to the single authored file and avoided any Cycle70/Cycle71 shadow tuning or out-of-scope scene/material edits.
```

## Implementation

- Added `CreateHouseExteriorPorchSightlineClosureCycle72(...)`.
- Wired the helper after `CreateHouseExteriorPorchGapClosureCycle64(...)` so it sits in front of the existing porch gap closure work.
- Added four non-arrival, visual-only closure pieces for both current and past house exterior spaces:
  - `Current_HouseExterior_Cycle72_PorchSightlineClosure_LeftFrontWallFillA`
  - `Current_HouseExterior_Cycle72_PorchSightlineClosure_RightFrontWallFillA`
  - `Current_HouseExterior_Cycle72_PorchSightlineClosure_FrontEaveBoardA`
  - `Current_HouseExterior_Cycle72_PorchSightlineClosure_UnderEaveOcclusionA`
  - matching `Past_...` variants.
- Added `ValidateFastVsHd2dShadowFoundationCycle72HouseExteriorSightlineClosure()`.
- Added `CaptureHd2dShadowFoundationCycle72ScreenshotsBatch()` with close and overview captures for current and past house exterior door/facade review.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle72ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_shadow_foundation_cycle72_house_exterior_sightline_closure_parent_review_20260523_01\parent_review_01_current_house_exterior_doorway_sightline_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_shadow_foundation_cycle72_house_exterior_sightline_closure_parent_review_20260523_01\parent_review_02_past_house_exterior_doorway_sightline_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_shadow_foundation_cycle72_house_exterior_sightline_closure_parent_review_20260523_01\parent_review_03_current_house_exterior_facade_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_shadow_foundation_cycle72_house_exterior_sightline_closure_parent_review_20260523_01\parent_review_04_past_house_exterior_facade_overview.png`

## Review Notes

Parent diff review caught and corrected one accidental worker drift in the older Cycle 70 capture block before running validation. The final authored diff keeps Cycle 70 / Cycle 71 shadow tuning intact and limits the functional change to Cycle 72 house exterior sightline closure plus new capture/validation entry points.

