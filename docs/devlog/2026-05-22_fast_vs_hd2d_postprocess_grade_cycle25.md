# 2026-05-22 Fast VS HD2D Postprocess Grade Cycle 25

## Context

- Branch: `work/fast-vs-hd2d-shading-foundation-20260522`
- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Goal: make the existing URP / renderer / Volume HD-2D shading foundation easier to discover and safer to regress before any further visible shading changes.

## Implementation

- Added a Cycle 25 batch report writer in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dShadingFoundationAudit.cs`.
- The new menu item calls `CreateHouseSliceScene()`, then `VerifyShadingFoundationV1()`, then writes a Markdown report to `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_postprocess_grade_cycle25_20260522\postprocess_grade_cycle25_20260522.md`.
- The report prints actual values from `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\UniversalRenderPipeline.asset`, `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\UniversalRenderPipeline_Renderer.asset`, and `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\DefaultVolumeProfile.asset`.
- `ValidateHouseSliceBatch()` now calls `AnemoraFastVsHd2dShadingFoundationAudit.VerifyShadingFoundationV1()` immediately after the material-role and sprite-card foundation audits.
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\DefaultVolumeProfile.asset` was normalized through the existing `AnemoraFastVsHd2dRenderAssetSetup.ApplyFastVsHd2dRenderAssets()` path after validation exposed that `DepthOfField` and `FilmGrain` were still active. No new grade target values were introduced.

## Worker Note

- The Cycle 25 task was assigned twice to `gpt-5.4-mini` workers, but both attempts failed because the selected model was at capacity.
- The second worker attempt left a partial implementation in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dShadingFoundationAudit.cs`; parent review repaired compile-risk issues and completed the cycle locally rather than leaving the branch stalled.

## Planned Worker Validation Commands

The intended worker commands were not executed by `gpt-5.4-mini` because both worker attempts failed at model-capacity allocation.

```powershell
git diff --check
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit.WritePostprocessGradeCycle25ReportBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_postprocess_grade_cycle25_report_worker_20260522.log'
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_postprocess_grade_cycle25_validate_worker_20260522.log'
```

## Parent Validation

1. Diff sanity check
   - Command: `git diff --check`
   - Result: PASS before Unity batch validation; only the usual CRLF warning for `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`.

2. Render asset normalization
   - Command: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dRenderAssetSetup.ApplyFastVsHd2dRenderAssets -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_postprocess_grade_cycle25_apply_render_assets_parent_20260522.log'`
   - Result: PASS
   - Reason: the first report attempt exposed that `DepthOfField` and `FilmGrain` were active despite the v1 audit contract requiring them disabled.

3. Cycle 25 report writer
   - Command: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit.WritePostprocessGradeCycle25ReportBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_postprocess_grade_cycle25_report_parent_20260522.log'`
   - Result: PASS
   - Report: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_postprocess_grade_cycle25_20260522\postprocess_grade_cycle25_20260522.md`

4. House slice validation
   - Command: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_postprocess_grade_cycle25_validate_parent_20260522.log'`
   - Result: PASS

5. Visual snapshot audit
   - Command: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_postprocess_grade_cycle25_capture_parent_20260522.log'`
   - Result: PASS after removing a stale `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Temp\UnityLockfile`; no Unity process was running.
