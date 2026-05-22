# 2026-05-23 Fast VS HD2D House Facade Porch Closure Cycle 52

## Scope

- Branch: `work/fast-vs-hd2d-shading-foundation-20260522`
- Project: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Scene: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- Setup source: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

## Intent

Close the visible house facade leak where the closed front door still allowed the interior to be seen from outside. The fix follows the review direction: move the porch-post-like objects forward, fill the wall volume behind and beside them, and add a wooden eave/awning board above so the posts read as supported architecture rather than detached columns.

## Implementation

- Moved `Current_HouseExterior_PorchLeftPost`, `Current_HouseExterior_PorchRightPost`, `Past_HouseExterior_PorchLeftPost`, and `Past_HouseExterior_PorchRightPost` forward on the local Z axis and slightly thickened them.
- Added side wall fill slabs behind the two porch posts:
  - `Current_HouseExterior_Cycle52_PorchFacadeClosure_LeftSideWallA`
  - `Current_HouseExterior_Cycle52_PorchFacadeClosure_RightSideWallA`
  - `Past_HouseExterior_Cycle52_PorchFacadeClosure_LeftSideWallA`
  - `Past_HouseExterior_Cycle52_PorchFacadeClosure_RightSideWallA`
- Added outer cheek wall extensions to close the remaining visible side shafts next to the closed door:
  - `Current_HouseExterior_Cycle52_PorchFacadeClosure_LeftOuterCheekWallA`
  - `Current_HouseExterior_Cycle52_PorchFacadeClosure_RightOuterCheekWallA`
  - `Past_HouseExterior_Cycle52_PorchFacadeClosure_LeftOuterCheekWallA`
  - `Past_HouseExterior_Cycle52_PorchFacadeClosure_RightOuterCheekWallA`
- Added a wooden front awning board, an under-awning occlusion band, and small post caps for current/past.
- Added `ValidateFastVsHd2dCycle52HouseFacadePorchClosure()` and `ValidateForwardPorchPostObject()` so the forward posts, wall fills, eave board, and non-arrival closure props are checked in the batch validator.

## Worker / Review

- A focused gpt-5.4-mini worker reviewed the likely structural offenders and identified the porch post objects as the primary suspicious elements to move/close around.
- Parent review adjusted the worker coordinate suggestion because the local scene convention puts the camera/front side at the more negative local Z direction.
- MCP Unity live resources were not available in this Codex session, so verification used Unity Editor batch execution and screenshot review.

## Verification

- `git diff --check -- C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_facade_porch_closure_cycle52_validate_parent_20260523.log`
- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dCloseReviewScreenshotsBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_facade_porch_closure_cycle52_close_review_parent_20260523_retry.log`
- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_facade_porch_closure_cycle52_visual_snapshot_parent_20260523.log`

All three verification commands passed. The first close-review screenshot attempt was started concurrently with another Unity process and exited with code 1 before loading the project; the retry above was run alone and passed.

## Evidence

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle52_house_facade_porch_closure_parent_review_20260523_01\02_house_exterior_door_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle52_house_facade_porch_closure_parent_review_20260523_01\02_current_house_exterior_visual_snapshot.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle52_house_facade_porch_closure_parent_review_20260523_01\visual_snapshot_metrics_cycle10_20260522.md`

## Result

The close-review screenshot now shows the exterior door surrounded by continuous wall volume and supported by forward porch posts plus the eave board. The old visual leak where the outside camera could see interior space beside the closed door is no longer visible in the saved evidence screenshots.

## Next

Resume the broader HD-2D outdoor/background/shading work after committing and pushing this focused house-facade fix.
