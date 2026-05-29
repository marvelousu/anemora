# 2026-05-22 Fast VS HD2D Outdoor Composition Sky Backdrop Foundation Cycle 32

File: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_outdoor_composition_sky_backdrop_cycle32.md`

## Scope
- Added an outdoor composition foundation for the Fast VS HD-2D branch.
- Targeted the user's review point that exterior maps still lacked sky/background context and that the house exterior close view over-emphasized crude wall/roof construction.
- Kept gameplay, story, time-window, transition, collider, and dialogue behavior unchanged.

## Files Changed
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsVisualDirectionGuide.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dVisualSnapshotAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\02_current_house_exterior_visual_snapshot.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\03_current_central_plaza_visual_snapshot.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\visual_snapshot_metrics_cycle10_20260522.md`
- New generated composition sky/horizon/edge-wrap material and texture assets under:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\`

## Implementation
- Added area-specific follow camera profiles in `FastVsVisualDirectionGuide`.
  - House exterior now uses a farther and higher camera profile to expose outdoor context and sky.
  - Central plaza now uses a wider outdoor profile that frames the library facade and surrounding sky.
  - Interior and library camera behavior stays close to the prior baseline.
- Updated `AnemoraFastVsHd2dVisualSnapshotAudit` so each visual snapshot can specify its own camera offset, look offset, and target FOV.
- Added `CreateOutdoorCompositionSkyBackdropFoundation(...)` for house exterior and central plaza.
  - Current and past worlds receive matched non-colliding sky panel, horizon band, and side wrap objects.
  - The new backdrop objects use `TimeWindowPairedSpaceLandmarkKind.PropOrFeature` and `countsForArrival=false`.
- Extended the generated outdoor scenic backdrop texture/material pipeline with `composition_sky`, `composition_horizon`, and `composition_edge_wrap` layer behavior.
- Added `ValidateFastVsHd2dThirtySecondCycleOutdoorCompositionSkyBackdropFoundation()`.

## Validation Commands
- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_composition_sky_backdrop_cycle32_validate_parent2_20260522.log`
- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_composition_sky_backdrop_cycle32_capture_parent2_20260522.log`

## Result
- Parent validation log included `Fast VS house slice validation passed.`
- Parent visual snapshot audit log included `Fast VS HD2D visual snapshot audit passed: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`.
- Visual review:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\02_current_house_exterior_visual_snapshot.png` now shows sky/background context instead of only the close facade/roof.
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\03_current_central_plaza_visual_snapshot.png` now reads more like an outdoor plaza with a framed library facade and distant sky.

## Residual Risk
- The house exterior still has oversized roof/chimney mass and block-like facade proportions. The next high-value cycle should rebuild the house exterior proportions rather than add more overlays.
- The new sky/backdrop is procedural and useful as a foundation, but it is not final art.
- Unity batch runs still emit licensing token warnings and noisy asset refresh side effects; unrelated reserialization was cleaned before commit.
