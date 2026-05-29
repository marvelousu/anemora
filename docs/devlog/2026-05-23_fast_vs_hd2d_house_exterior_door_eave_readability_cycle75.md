# feat(hd2d): soften house doorway eave bars

Cycle: 75  
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`  
Authored file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

## Goal

Cycle 74 improved the black vertical doorway fill, but the close review still had a hard black horizontal bar and Niro in the frame. Cycle 75 softens the eave / AO bars into translucent occlusion and moves the close review player position off-frame so the facade can be judged directly.

## Worker Scope

SCOPED_PROMPT_ISSUED cycle=75 authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle75ScreenshotsBatch`

Worker returned:

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - none
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle75ScreenshotsBatch
notes: Softened the eave/AO bands to `hd2d_outdoor_occlusion_gradient`, added Cycle 75 validation/capture entry points, and avoided touching scenes, materials, ProjectSettings, or any other files.
```

## Implementation

- Changed `HeroReadability_EaveUndersideBandA` to use the existing outdoor occlusion gradient material.
- Changed `HeroReadability_FacadeAOBandUpperA` and `FacadeAOBandLowerA` to the same translucent occlusion material.
- Reduced the thickness of those horizontal bars so they read as contact depth instead of black boards.
- Added `ValidateFastVsHd2dShadowFoundationCycle75DoorwayEaveReadability()`.
- Added `CaptureHd2dShadowFoundationCycle75ScreenshotsBatch()` with player-off-frame close review captures.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle75ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle75_door_eave_parent_review_20260523_01\parent_review_01_current_house_exterior_door_eave_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle75_door_eave_parent_review_20260523_01\parent_review_02_past_house_exterior_door_eave_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle75_door_eave_parent_review_20260523_01\parent_review_03_current_house_exterior_door_eave_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle75_door_eave_parent_review_20260523_01\parent_review_04_past_house_exterior_door_eave_overview.png`

## Review Notes

Parent visual review should confirm the close screenshot no longer has Niro blocking the doorway and that the horizontal eave/AO bars read as depth rather than flat black bands.

