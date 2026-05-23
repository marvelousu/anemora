# feat(hd2d): consolidate house exterior artifacts

## Scope

Cycle 97 follows the Cycle 96 parent PNG review. The house exterior structure was closer, but the overview still read as if broad black boards and a translucent facade plane were sitting around the house. This cycle keeps the work structural and local before moving to sun-direction or post-processing changes.

Authored file:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Side-effect files:

- None

SCOPED_PROMPT_ISSUED cycle=97 authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle97ScreenshotsBatch`

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - None
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle97ScreenshotsBatch
notes: Kept all edits inside the authored file, added the Cycle97 facade artifact consolidation path, and avoided touching unrelated workspace files.
```

Parent correction: the worker reduced the Cycle69 broad falloff only for the current world branch. The parent applied the same reductions to the past branch and updated the existing Cycle69 validation ranges so the change is symmetric.

## Implementation Plan

- Add an opaque facade consolidation helper after the Cycle 96 house exterior helper.
- Hide the remaining blue/translucent facade read with existing wall, roof, stone, and wood materials rather than occlusion plates.
- Reduce old broad Cycle 69 outdoor falloff shadows so they no longer read as black boards in overview.
- Keep localized Cycle 96 contact shadows for grounding.
- Add validation and a four-frame parent review capture for current overview, current close, current no-player lower facade, and past overview.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle97ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle97_house_exterior_artifact_consolidation_parent_review_20260524_01\parent_review_01_current_house_exterior_artifact_consolidation_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle97_house_exterior_artifact_consolidation_parent_review_20260524_01\parent_review_02_current_house_exterior_artifact_consolidation_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle97_house_exterior_artifact_consolidation_parent_review_20260524_01\parent_review_03_current_house_exterior_artifact_consolidation_lower_facade_no_player.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle97_house_exterior_artifact_consolidation_parent_review_20260524_01\parent_review_04_past_house_exterior_artifact_consolidation_overview.png`

## Parent Review Notes

Visual gate: the house exterior overview should no longer show broad black board-like shadows or a blue translucent facade read. The close/no-player captures should still look physically closed and made from wall/wood/stone materials.
