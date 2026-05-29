# feat(hd2d): soften house porch occlusion slab

Cycle: 73  
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`  
Authored file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

## Goal

Cycle 72 closed the house exterior sightline gaps, but the close screenshot still read as a large black slab under the porch/eave. Cycle 73 keeps the closure geometry but changes the occlusion treatment so it reads as soft under-eave depth rather than a flat black board.

## Worker Scope

SCOPED_PROMPT_ISSUED cycle=73 authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle73ScreenshotsBatch`

Worker returned:

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - none
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle73ScreenshotsBatch
notes: Kept all changes inside the authored file, updated the Cycle72 under-eave piece to the translucent gradient material, and avoided any edits to scenes, materials, docs, or ProjectSettings.
```

## Implementation

- Kept `CreateHouseExteriorPorchSightlineClosureCycle72(...)` wired into the house exterior build path.
- Changed `UnderEaveOcclusionA` from solid `materials.Shadow` to `EnsureHd2dOutdoorOcclusionGradientMaterial()`.
- Reduced the Cycle72 front wall fills, front eave board, and under-eave occlusion thickness so they remain sightline blockers without becoming a dominant black slab.
- Added `ValidateFastVsHd2dShadowFoundationCycle73PorchOcclusionReadability()`.
- Added `ValidateHouseExteriorPorchOcclusionReadabilityObject(...)`.
- Added `CaptureHd2dShadowFoundationCycle73ScreenshotsBatch()` and a four-shot capture set focused on the porch occlusion readability.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle73ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle73_porch_parent_review_20260523_01\parent_review_01_current_house_exterior_porch_occlusion_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle73_porch_parent_review_20260523_01\parent_review_02_past_house_exterior_porch_occlusion_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle73_porch_parent_review_20260523_01\parent_review_03_current_house_exterior_porch_readability_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle73_porch_parent_review_20260523_01\parent_review_04_past_house_exterior_porch_readability_overview.png`

## Review Notes

Parent review should focus on whether the current close porch image no longer contains a hard black horizontal slab, while still keeping the door-side voids visually closed from the exterior.


## Runner Note

First capture attempt failed because the original screenshot directory made the third PNG path 263 characters long on Windows. The capture directory was shortened to docs/devlog/screenshots/fast_vs_hd2d_cycle73_porch_parent_review_20260523_01 before rerunning the cycle.

