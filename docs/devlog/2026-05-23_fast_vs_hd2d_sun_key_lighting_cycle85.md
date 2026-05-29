# feat(hd2d): strengthen sun key lighting

Cycle: 85  
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`  
Authored files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsHouseLightingDirector.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs`

## Goal

Cycle 84 added the first visible sun/sky-grade layer. Cycle 85 moves into the lighting foundation itself: keep the frame slightly darker, make the key light read more like a warm sun, and reduce fill/ambient where possible without changing the global Volume yet.

## Worker Scope

SCOPED_PROMPT_ISSUED cycle=85a authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsHouseLightingDirector.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle85ScreenshotsBatch`

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsHouseLightingDirector.cs
side_effect_files:
  - none
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle85ScreenshotsBatch
notes: Replaced only the requested CreateProfile lighting constants; left fog, camera background, light types, cool rim values, transition duration, and library spot geometry untouched.
```

SCOPED_PROMPT_ISSUED cycle=85b authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle85ScreenshotsBatch`

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - none
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle85ScreenshotsBatch
notes: Updated the scene-setup lighting mirrors and added cycle85 validate/capture hooks only in the authored editor file, leaving the unrelated runtime worktree change untouched.
```

SCOPED_PROMPT_ISSUED cycle=85c authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle85ScreenshotsBatch`

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs
side_effect_files:
  - none
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle85ScreenshotsBatch
notes: Replaced only the four audit constant blocks in VerifyAreaLightingProfilesV1() and left tolerances, names, luminance bands, and validation logic unchanged.
```

## Implementation

- Raised outdoor main-light contrast to the high end of the accepted sun-key range.
- Warmed the key tint for exterior, plaza, library, and interior.
- Lowered warm fill and ambient values within the existing readability envelope.
- Mirrored the values across runtime profile, generated scene review profiles, and the area lighting audit.
- Added `ValidateFastVsHd2dShadowFoundationCycle85SunKeyLighting()`.
- Added `CaptureHd2dShadowFoundationCycle85ScreenshotsBatch()` with current house exterior, plaza, library, and interior views.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle85ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle85_sun_key_lighting_parent_review_20260523_01\parent_review_01_current_house_exterior_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle85_sun_key_lighting_parent_review_20260523_01\parent_review_02_current_central_plaza_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle85_sun_key_lighting_parent_review_20260523_01\parent_review_03_current_library_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle85_sun_key_lighting_parent_review_20260523_01\parent_review_04_current_house_interior_overview.png`

## Review Notes

This cycle intentionally does not change `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\DefaultVolumeProfile.asset`. The next cycle should handle the faded/desaturated camera grade and update the render asset setup plus shading foundation audit together.
