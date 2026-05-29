# 2026-05-23 Fast VS HD2D Outdoor Horizon Silhouette Blend Cycle 55

## Scope

- Branch: `work/fast-vs-hd2d-shading-foundation-20260522`
- Project: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Scene: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- Setup source: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

## Intent

Cycle 53 widened the outdoor ground envelope and Cycle 54 broke up the outer edges, but the outdoor maps still risk reading as isolated map slabs. This cycle adds a restrained low horizon silhouette layer around the house exterior and central plaza. It avoids a broad sky rewrite and uses existing ground, stone, path, and dust materials only.

## Implementation

- Added `CreateOutdoorHorizonSilhouetteBlendCycle55(...)`.
- Called it immediately after `CreateOutdoorPerimeterEdgeBreakupCycle54(...)` for:
  - `FastVsHouseArea.Exterior`
  - `FastVsHouseArea.CentralPlaza`
- Added current/past non-arrival house exterior horizon pieces:
  - rear grass and path bands
  - rear stone rises
  - west/east ground carry strips
  - rear dust/stone underlay
- Added current/past non-arrival central plaza horizon pieces:
  - rear grass and path bands
  - rear stone rises
  - west/east ground carry strips
  - rear ground underlay
  - front broken ground edge
- Added `ValidateFastVsHd2dCycle55OutdoorHorizonSilhouetteBlend()` and wired it into `ValidateHouseSliceBatch()`.
- All new geometry uses `CreateNonArrivalLandmarkCubeShadowSafe(...)`, so it does not affect movement, map transitions, or Time Window traversal.

## Worker / Review

- A gpt-5.4-mini worker implemented the bounded code change from a parent-written procedure.
- Parent review checked the generated method, call sites, validation coverage, generated scene, and screenshots before recording this cycle.
- The worker only changed `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`.

## Verification

- `git diff --check -- C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_horizon_silhouette_cycle55_validate_parent_20260523.log`
- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_horizon_silhouette_cycle55_visual_snapshot_parent_20260523.log`
- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dCloseReviewScreenshotsBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_horizon_silhouette_cycle55_close_review_parent_20260523.log`

All three Unity commands passed.

## Evidence

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle55_outdoor_horizon_silhouette_blend_parent_review_20260523_01\01_current_house_exterior_visual_snapshot_cycle55.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle55_outdoor_horizon_silhouette_blend_parent_review_20260523_01\02_current_central_plaza_visual_snapshot_cycle55.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle55_outdoor_horizon_silhouette_blend_parent_review_20260523_01\03_visual_snapshot_metrics_cycle55.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle55_outdoor_horizon_silhouette_blend_parent_review_20260523_01\04_house_exterior_door_close_cycle55.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle55_outdoor_horizon_silhouette_blend_parent_review_20260523_01\05_plaza_library_door_current_close_cycle55.png`

## Result

The house exterior and central plaza now have low distant bands and side carry strips that reduce the floating-slab read without adding a broad new sky curtain. The house facade close screenshot confirms the previously reported door-side interior leak remains closed. This is still a foundation pass: stronger sky/horizon art and building-volume polish should continue in later cycles.
