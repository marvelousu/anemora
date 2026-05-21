# 2026-05-22 Fast VS HD2D House Exterior Ground Microcontrast Cycle 17

## Scope

Cycle 17 tightens the current-world house exterior ground texture so the yard reads with more micro-contrast without darkening the whole exterior or touching global light, postprocess, dialogue, time windows, or map transitions.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Intent

Cycle 16 showed `Current.HouseExterior.Ground.Yard` at `LocalContrast 0.0129`, which was below the desired floor for a readable exterior grass plane. The target for this cycle was to push the current yard surface to at least `0.016`, ideally around `0.018`, while keeping average luminance in the `0.18` to `0.23` range.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_house_exterior_ground_microcontrast_cycle17.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_ground_microcontrast_cycle17_20260522\surface_texture_metrics_cycle17_20260522.md`

## Cycle 16 Problem

The yard texture was readable but too flat in local contrast. Cycle 16 metrics showed:

- `Current.HouseExterior.Ground.Yard`
- `AvgLum 0.194`
- `LocalContrast 0.0129`

Past grass already sat higher at `LocalContrast 0.0183`, so the current yard needed slightly sharper separation between soil, blade streaks, and fine noise rather than a global brightness shift.

## Implementation

`SampleCurrentGrassHd2dPixel()` now uses a slightly more separated current grass palette, with darker soil/shadow notes and a modestly brighter highlight range.

`SampleGrassAndSoilHd2dPixel()` now gives the current side a little more current-only soil blend, clump breakup, diagonal blade streaking, and fine-noise contrast. The past side stays largely unchanged.

`AnemoraFastVsHd2dSurfaceTextureMetricAudit` now has a local-contrast gate for readable textures, with a lower threshold for ground surfaces. Color fallback surfaces are excluded from that check. The same audit class also now exposes a Cycle 17 report writer for the house exterior ground microcontrast batch, while keeping the existing Cycle 16 report path intact.

## Validation Performed

- `git status --short --branch`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_ground_microcontrast_cycle17_validate_worker_20260522.log"`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dSurfaceTextureMetricAudit.WriteHouseExteriorGroundMicrocontrastCycle17MetricsBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_ground_microcontrast_cycle17_metrics_worker_20260522.log"`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_ground_microcontrast_cycle17_capture_worker_20260522.log"`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_ground_microcontrast_cycle17_validate_parent_20260522.log"`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dSurfaceTextureMetricAudit.WriteHouseExteriorGroundMicrocontrastCycle17MetricsBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_ground_microcontrast_cycle17_metrics_parent_20260522.log"`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_ground_microcontrast_cycle17_capture_parent_20260522.log"`
- `git diff --check -- . ':(exclude)Assets/Scenes/Anemora_FastVS_HouseSlice.unity'`

## Results

- `Fast VS house slice validation passed.`
- `HD2D surface texture metric report written: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_ground_microcontrast_cycle17_20260522\surface_texture_metrics_cycle17_20260522.md`
- `Current.HouseExterior.Ground.Yard` in the Cycle 17 report: `AvgLum 0.193`, `LocalContrast 0.0183`
- `Fast VS HD2D visual snapshot audit passed: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`

## Report File

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_ground_microcontrast_cycle17_20260522\surface_texture_metrics_cycle17_20260522.md`

## Risks

- Batchmode still writes unrelated scene/project/asset churn in this worktree, including the existing visual snapshot evidence set and project settings files.
- The new gate only covers readable textures; color fallback remains exempt by design.
- If later art changes push the yard texture back toward a flatter noise field, the current threshold will need another pass rather than a lighting change.
