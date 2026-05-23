# feat(hd2d): add faded camera grade

Cycle: 86  
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`  
Authored files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dRenderAssetSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dShadingFoundationAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\DefaultVolumeProfile.asset`

## Goal

Cycle 84 added the first visible sun/sky layer, and Cycle 85 strengthened the warm sun-key lighting. Cycle 86 applies the requested slightly faded camera treatment: darker exposure, reduced saturation, restrained bloom, and a light vignette while keeping Tonemapping on Neutral and keeping DepthOfField/FilmGrain disabled.

## Worker Scope

SCOPED_PROMPT_ISSUED cycle=86a authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dRenderAssetSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle86ScreenshotsBatch`

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dRenderAssetSetup.cs
side_effect_files:
  - none
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle86ScreenshotsBatch
notes: Replaced only the requested Bloom, ColorAdjustments, and Vignette values plus the stale summary string; left Tonemapping, DepthOfField, FilmGrain, pipeline, renderer, SSAO, and optional color grade logic untouched.
```

SCOPED_PROMPT_ISSUED cycle=86b authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dShadingFoundationAudit.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle86ScreenshotsBatch`

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dShadingFoundationAudit.cs
side_effect_files:
  - none
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle86ScreenshotsBatch
notes: Updated only the faded camera grade thresholds/messages and report prose in the authored audit file; I avoided any changes to pipeline, renderer, SSAO, optional SH/LGG validation, paths, or public APIs.
```

SCOPED_PROMPT_ISSUED cycle=86c authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle86ScreenshotsBatch`

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - none
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle86ScreenshotsBatch
notes: Kept this capture-only, added just the new cycle86 batch entry point plus one private helper, and avoided any changes to ValidateHouseSliceBatch, scene setup, lighting, assets, or existing cycle84/85 methods.
```

## Implementation

- Updated the render setup contract to write a darker, slightly desaturated camera grade.
- Applied the render setup in Unity batch mode so `DefaultVolumeProfile.asset` carries the actual runtime values.
- Mirrored the target values in the shading foundation audit.
- Added `CaptureHd2dShadowFoundationCycle86ScreenshotsBatch()` for parent-review captures across exterior, plaza, library, and interior.
- Kept DepthOfField and FilmGrain disabled so the prototype remains readable.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle86ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle86_faded_camera_grade_parent_review_20260523_01\parent_review_01_current_house_exterior_faded_grade_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle86_faded_camera_grade_parent_review_20260523_01\parent_review_02_current_central_plaza_faded_grade_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle86_faded_camera_grade_parent_review_20260523_01\parent_review_03_current_library_faded_grade_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle86_faded_camera_grade_parent_review_20260523_01\parent_review_04_current_house_interior_faded_grade_overview.png`

## Review Notes

This cycle is deliberately global and small. If the screenshots read too flat, the next cycle should retune exposure/saturation after visual review instead of replacing the sun-key work from Cycle 85.
