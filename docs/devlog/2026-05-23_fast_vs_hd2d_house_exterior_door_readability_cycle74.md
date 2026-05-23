# feat(hd2d): replace black doorway fill with readable depth

Cycle: 74  
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`  
Authored file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

## Goal

Cycle 73 improved the house exterior overview, but the close shot still showed a large black vertical block around the door. Cycle 74 replaces that black doorway fill with a narrow door-detail depth strip and a thin soft occlusion line, preserving the closed exterior while avoiding a flat black board.

## Worker Scope

SCOPED_PROMPT_ISSUED cycle=74 authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle74ScreenshotsBatch`

Worker returned:

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - none
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle74ScreenshotsBatch
notes: Kept the change confined to the one authored Unity editor file and avoided any scene, material, texture, docs, or ProjectSettings edits.
```

## Implementation

- Changed `HouseExterior_HeroReadability_DoorwayDarkFillA` from `DoorwayDark` to the appropriate house door detail material.
- Narrowed and repositioned that strip so it reads as left-side door depth rather than a central black block.
- Added `Cycle74_DoorwaySoftDepthLineA` using the existing outdoor occlusion gradient material.
- Updated the hero readability validation expectations for `DoorwayDarkFillA`.
- Added `ValidateFastVsHd2dShadowFoundationCycle74DoorwayReadableDepth()`.
- Added `CaptureHd2dShadowFoundationCycle74ScreenshotsBatch()` with a four-shot door readability capture set.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle74ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle74_door_readability_parent_review_20260523_01\parent_review_01_current_house_exterior_door_readability_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle74_door_readability_parent_review_20260523_01\parent_review_02_past_house_exterior_door_readability_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle74_door_readability_parent_review_20260523_01\parent_review_03_current_house_exterior_door_readability_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle74_door_readability_parent_review_20260523_01\parent_review_04_past_house_exterior_door_readability_overview.png`

## Review Notes

Parent visual review should compare against Cycle 73 close captures and confirm the doorway no longer reads as a black vertical slab while the house exterior still appears closed.

