# feat(hd2d): add cinematic sun grade

Cycle: 84  
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`  
Authored file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

## Goal

The parent review asked whether the HD-2D pass would read better if the whole image became a little darker, with a visible sun and a slightly faded camera treatment. Cycle 84 implements the first safe visual slice of that direction: an outdoor background sun disc plus a low-alpha sky veil, without changing runtime lighting or global post-processing yet.

## Worker Scope

SCOPED_PROMPT_ISSUED cycle=84 authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle84ScreenshotsBatch`

Worker returned:

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_cinematic_sun_disc_cycle84.mat
  - Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_cinematic_sun_disc_cycle84.mat.meta
  - Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_cinematic_sky_veil_cycle84.mat
  - Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_cinematic_sky_veil_cycle84.mat.meta
  - Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_cinematic_sun_disc_cycle84.asset
  - Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_cinematic_sun_disc_cycle84.asset.meta
  - Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_cinematic_sky_veil_cycle84.asset
  - Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_cinematic_sky_veil_cycle84.asset.meta
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle84ScreenshotsBatch
notes: Kept the change confined to the authored editor file plus the allowed sun/veil material, texture, and meta assets, and avoided touching the lighting director or global Volume.
```

## Implementation

- Added `CreateOutdoorCinematicSunGradeCycle84()` and wired it into the house exterior and central plaza outdoor map builders.
- Added a generated transparent warm sun-disc texture/material.
- Added a generated transparent cool/desaturated sky-veil texture/material.
- Added current/past objects for house exterior and central plaza, all non-colliding and non-arrival.
- Added `ValidateFastVsHd2dShadowFoundationCycle84CinematicSunGrade()`.
- Added `CaptureHd2dShadowFoundationCycle84ScreenshotsBatch()`.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle84ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle84_cinematic_sun_grade_parent_review_20260523_01\parent_review_01_current_house_exterior_cinematic_grade_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle84_cinematic_sun_grade_parent_review_20260523_01\parent_review_02_past_house_exterior_cinematic_grade_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle84_cinematic_sun_grade_parent_review_20260523_01\parent_review_03_current_central_plaza_cinematic_grade_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle84_cinematic_sun_grade_parent_review_20260523_01\parent_review_04_past_central_plaza_cinematic_grade_overview.png`

## Review Notes

This cycle is expected to be a visible but conservative background/camera-feel pass. If the sun or veil reads well, the next cycle should move to the actual runtime lighting director and post-process grade: lower ambient/exposure, stronger warm key direction, and slightly reduced saturation.
