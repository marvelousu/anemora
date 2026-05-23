# feat(hd2d): break up house ground shadow slab

## Scope

Cycle 96 follows the Cycle 95 parent PNG review. The house facade was more naturalized, but the current house overview still showed broad black rectangular slabs. The likely cause was the old Cycle69 broad ground shadow footprint reading as a single plate. This cycle keeps contact shadowing but breaks up the obvious slab.

Authored file:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Side-effect files:

- None

SCOPED_PROMPT_ISSUED cycle=96 authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle96ScreenshotsBatch`

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - None
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle96ScreenshotsBatch
notes: Kept the change scoped to the authored file, tightened Cycle69 ranges, added Cycle96 breakup geometry/validation/capture, and avoided any scene or asset edits.
```

## Implementation Plan

- Reduce `HouseGroundCastBroadA` from a large 7x3 style rectangle to a smaller under-house shadow.
- Keep porch, road-edge, and yard falloff shadows but keep them out of the left/right facade-side rectangles.
- Add localized contact shadow pieces for porch footings, facade base, and left/right side base screens.
- Validate both Current and Past variants for parent, material, transform, non-collision, non-arrival landmark state, and shadow-safe renderer settings.
- Capture current overview, current close, current no-player lower-facade/ground, and past overview screenshots.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle96ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle96_house_exterior_ground_shadow_breakup_parent_review_20260524_01\parent_review_01_current_house_exterior_ground_shadow_breakup_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle96_house_exterior_ground_shadow_breakup_parent_review_20260524_01\parent_review_02_current_house_exterior_ground_shadow_breakup_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle96_house_exterior_ground_shadow_breakup_parent_review_20260524_01\parent_review_03_current_house_exterior_ground_shadow_breakup_lower_facade_no_player.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle96_house_exterior_ground_shadow_breakup_parent_review_20260524_01\parent_review_04_past_house_exterior_ground_shadow_breakup_overview.png`

## Parent Review Notes

The visual gate is whether the current house exterior overview no longer reads as if a broad black shadow board is lying under and beside the house. Localized exaggerated HD-2D contact shadows should remain.
