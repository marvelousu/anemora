# feat(hd2d): add no-player house door review captures

Cycle: 76  
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`  
Authored file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

## Goal

Cycle 75 softened the eave/AO bars, but the close review was still not reliable because the facade camera was using a player-inclusive close helper. Cycle 76 adds no-player close review helpers so the house exterior door and porch can be judged without Niro or player-layer artifacts blocking the doorway.

## Worker Scope

SCOPED_PROMPT_ISSUED cycle=76 authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle76ScreenshotsBatch`

Worker returned:

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - none
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle76ScreenshotsBatch
notes: Kept the change inside the single authored file, added the no-player close helpers and Cycle76 entry points, and avoided any scene/material/ProjectSettings edits.
```

## Implementation

- Added `CaptureCloseReviewScreenshotWithoutPlayer(...)`.
- Added `CaptureCloseOtherTimeReviewScreenshotWithoutPlayer(...)`.
- Both helpers temporarily remove `PlayerVisibleRenderLayerForReview` from the camera culling mask and restore the prior mask afterward.
- Added `CaptureHd2dShadowFoundationCycle76ScreenshotsBatch()` with no-player current/past door close captures.
- Added `ValidateFastVsHd2dShadowFoundationCycle76NoPlayerDoorReviewCapture()`.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle76ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle76_no_player_door_parent_review_20260523_01\parent_review_01_current_house_exterior_no_player_door_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle76_no_player_door_parent_review_20260523_01\parent_review_02_past_house_exterior_no_player_door_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle76_no_player_door_parent_review_20260523_01\parent_review_03_current_house_exterior_no_player_door_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle76_no_player_door_parent_review_20260523_01\parent_review_04_past_house_exterior_no_player_door_overview.png`

## Review Notes

Parent visual review should judge whether the close shot now frames the closed door/porch without Niro or player-layer artifacts. This is a review-foundation cycle, not a new geometry pass.

