# feat(hd2d): depth-test transparent world overlays

Cycle: 80  
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`  
Authored file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

## Goal

Cycles 77 through 79 improved the house exterior door geometry, but close review still showed transparent blue-gray world overlays drawing across the doorway. Cycle 80 changes the transparent world material setup so those overlays depth-test against opaque geometry instead of drawing over the closed door.

## Worker Scope

SCOPED_PROMPT_ISSUED cycle=80 authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle80ScreenshotsBatch`

Worker returned:

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - none
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle80ScreenshotsBatch
notes: Kept the change local to the authored editor file, avoided any scene/material asset edits, and left renderQueue values unchanged while adding the new depth-test audit and cycle 80 no-player capture path.
```

## Implementation

- Updated `ConfigureTransparentMaterial(...)` to set `_ZTest` to `CompareFunction.LessEqual` when available.
- Kept `_ZWrite` disabled and left render queues unchanged.
- Added `ValidateFastVsHd2dShadowFoundationCycle80TransparentDepthTest()`.
- Added `CaptureHd2dShadowFoundationCycle80ScreenshotsBatch()` for no-player close and overview evidence.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle80ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle80_transparent_depth_parent_review_20260523_01\parent_review_01_current_house_exterior_transparent_depth_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle80_transparent_depth_parent_review_20260523_01\parent_review_02_past_house_exterior_transparent_depth_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle80_transparent_depth_parent_review_20260523_01\parent_review_03_current_house_exterior_transparent_depth_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle80_transparent_depth_parent_review_20260523_01\parent_review_04_past_house_exterior_transparent_depth_overview.png`

## Review Notes

Parent visual review should confirm whether transparent sky/backdrop/light planes stop covering the closed door in close review while preserving the outdoor atmosphere in overview shots.
