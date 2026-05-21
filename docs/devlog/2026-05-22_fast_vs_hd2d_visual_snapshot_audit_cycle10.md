# 2026-05-22 Fast VS HD2D Visual Snapshot Audit Cycle 10

## Scope

Cycle 10 adds a bounded visual snapshot and audit foundation for Fast VS HD-2D. It captures representative world views from Unity batchmode, writes PNG evidence, and computes machine-readable luminance and contrast gates. This is validation and evidence capture, not final art polish.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Intent

The goal is a repeatable visual gate that can tell us when HD-2D changes are producing blank, black, white, or overly flat captures, instead of only making shadows darker. The batch audit captures four representative current-world views: house interior, house exterior, central plaza, and library. It uses the existing House Slice scene, the current-space portal controller, the area visibility controller, and the visual direction guide to force a stable review setup before rendering 1280x720 PNG evidence.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dVisualSnapshotAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_interior_floor.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_interior_wall.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_furniture.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_visual_snapshot_audit_cycle10.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

## Parent Review Correction

The first Cycle 10 visual snapshot run failed the house-interior local contrast gate. A separate probe with the pre-existing house-interior capture path showed that the current interior had become too dark after Cycle 9's `SurfaceRampLit` rollout, not that the new camera was simply framed incorrectly.

This cycle therefore narrows the first `SurfaceRampLit` pass away from current-world house interior floor, wall, and furniture materials. Those materials remain `Universal Render Pipeline/Lit` until a dedicated interior ramp pass can be designed and validated without crushing readability. The visual snapshot audit now records this kind of regression as PNG evidence plus luminance and local-contrast metrics.

## Output Evidence

- Screenshot directory:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`
- PNGs:
  - `01_current_house_interior_visual_snapshot.png`
  - `02_current_house_exterior_visual_snapshot.png`
  - `03_current_central_plaza_visual_snapshot.png`
  - `04_current_library_visual_snapshot.png`
- Metrics file:
  - `visual_snapshot_metrics_cycle10_20260522.md`

## Validation Performed

- `git diff --check -- . ':(exclude)Assets/Scenes/Anemora_FastVS_HouseSlice.unity'`
- Unity batch house validation:
  - `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_visual_snapshot_audit_cycle10_validate_parent_20260522.log'`
  - Result: `Fast VS house slice validation passed.`
- Unity batch visual snapshot audit:
  - `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_visual_snapshot_audit_cycle10_capture_parent_20260522.log'`
  - Result: `Fast VS HD2D visual snapshot audit passed.`

## Cleanup

- Keep the new editor audit file, this devlog, the index update, and the generated PNG/metrics evidence unless a capture turns out to be broken or blank.
- Revert or remove Unity side effects that are not part of this cycle if they appear in diff output, including Addressables link files, `Windows.meta`, scene file churn, ProjectSettings `Graphics` / `Quality` / `SceneTemplate` churn, tree import metas, and any unrelated static or surface shadow import assets touched by batchmode.
- Clean trailing whitespace on any new text or meta files left modified.

## Risks

- The gates are intentionally conservative and are meant to catch obviously broken visual output, not to certify final composition quality.
- The batch relies on the existing House Slice scene build path and Unity render behavior staying stable in batchmode.
- If future graphics work changes scene framing or clear colors, these thresholds may need a small adjustment, but the snapshot gate should remain the same shape.

## Next Steps

- Use the captured PNGs and metrics file as the reference loop for subsequent HD-2D lighting and material work.
- Expand the same snapshot-audit pattern only when additional representative views need machine-readable review coverage.
