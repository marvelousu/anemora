# feat(hd2d): naturalize house exterior facade

## Scope

Cycle 95 follows the Cycle 94 parent PNG review. Cycle 94 reduced the large transparent front face, but the overview still showed broad black rectangular staging areas on both sides of the house and the new upper apron read as one flat gray slab. This cycle keeps the focus on the house exterior before returning to wider lighting and atmosphere work.

Authored file:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Side-effect files:

- None

SCOPED_PROMPT_ISSUED cycle=95 authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle95ScreenshotsBatch`

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - None
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle95ScreenshotsBatch
notes: Kept the change confined to the authored editor file, added the cycle-95 facade naturalization helper plus batch validation/capture wiring, and avoided touching scenes, assets, or other out-of-scope files.
```

## Implementation Plan

- Add lower left/right ground shoulders in front of the remaining black side staging rectangles.
- Add left/right stone base screens to turn the side voids into plausible foundation mass.
- Add thin vertical battens, lower trim, and an eave cap over the Cycle94 upper apron so it reads as a constructed facade surface instead of a blank slab.
- Validate Current and Past variants for parenting, material identity, local position/scale, non-collision, non-arrival landmark state, and absence of broad shadow/doorway/occlusion/light/sky-bar material tokens.
- Capture the same house exterior review angles as Cycle94 for direct visual comparison.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle95ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle95_house_exterior_facade_naturalization_parent_review_20260524_01\parent_review_01_current_house_exterior_facade_naturalization_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle95_house_exterior_facade_naturalization_parent_review_20260524_01\parent_review_02_current_house_exterior_facade_naturalization_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle95_house_exterior_facade_naturalization_parent_review_20260524_01\parent_review_03_current_house_exterior_facade_naturalization_upper_facade.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle95_house_exterior_facade_naturalization_parent_review_20260524_01\parent_review_04_past_house_exterior_facade_naturalization_overview.png`

## Parent Review Notes

The visual gate is narrow: the side black rectangles should be less exposed in the overview, and the upper apron should read less like a flat pasted board. Passing validation remains necessary but is not visual sign-off.
