# feat(hd2d): close house exterior leak bars

## Scope

Cycle 93 addresses the remaining house exterior readability break that was still visible after Cycle 91 and Cycle 92: the black horizontal staging bars and see-through gaps around the front roofline and door-side porch edges. This cycle stays on the current HD-2D shading branch and does not touch route logic, Time Window behavior, story, UI, fonts, character sprites, or `main`.

Authored file:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Side-effect files:

- None

SCOPED_PROMPT_ISSUED cycle=93 authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle93ScreenshotsBatch`

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - None
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle93ScreenshotsBatch
notes: Stayed inside the single authored file, used existing materials/helpers only, and avoided adding any assets, scenes, docs, or build outputs.
```

## Implementation Plan

- Add a Cycle93 facade leak-closure pass immediately after the Cycle91 sky-bar mask in the generated house exterior.
- Use architectural closure pieces instead of more black staging masks:
  - front eave fascia with the existing furniture/wood material,
  - front roof lip with the existing roof material,
  - upper wall backing panel with the current/past exterior wall material,
  - left and right front cheek panels to block porch side see-through gaps,
  - a very thin under-eave ambient strip using the existing outdoor occlusion gradient only as secondary contact shading.
- Add validation that both Current and Past house exterior spaces contain the closure pieces, use expected non-shadow materials for the broad visible parts, and remain shadow-safe.
- Add focused parent-review screenshots for the house exterior overview, close eave/door framing, upper facade, and past overview.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle93ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle93_house_exterior_leak_closure_parent_review_20260524_01\parent_review_01_current_house_exterior_leak_closure_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle93_house_exterior_leak_closure_parent_review_20260524_01\parent_review_02_current_house_exterior_leak_closure_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle93_house_exterior_leak_closure_parent_review_20260524_01\parent_review_03_current_house_exterior_leak_closure_upper_facade.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle93_house_exterior_leak_closure_parent_review_20260524_01\parent_review_04_past_house_exterior_leak_closure_overview.png`

## Parent Review Notes

The visual gate for this cycle is specific: the house front should no longer show obvious black horizontal bars or open holes beside the closed door/porch. If the screenshots still show visible staging geometry, the next cycle should keep working on house facade closure before moving back to broader sunbeam, atmosphere, or background tasks.
