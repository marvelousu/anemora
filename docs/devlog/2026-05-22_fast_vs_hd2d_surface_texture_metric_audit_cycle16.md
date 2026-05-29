# 2026-05-22 Fast VS HD2D Surface Texture Metric Audit Cycle 16

## Scope

Cycle 16 adds an Editor-only texture metric audit for the major HD-2D surface profiles introduced in Cycle 15. The goal is to make surface texture/color readability measurable in Unity Editor without changing lighting, postprocess, shaders, dialogue, time windows, or map transitions.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Intent

The metric layer is a review foundation, not a shading pass. It samples the material's primary texture when readable, otherwise falls back to material color, and checks whether the surface reads like a minimally legible HD-2D plane in the current house slice scene.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs.meta`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_surface_texture_metric_audit_cycle16.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_surface_texture_metrics_cycle16_20260522\surface_texture_metrics_cycle16_20260522.md`

## Audit Details

`AnemoraFastVsHd2dSurfaceTextureMetricAudit.VerifySurfaceTextureMetricsV1()` now scans `Resources.FindObjectsOfTypeAll<FastVsHd2dSurfaceProfile>()`, filters to the house slice scene, resolves `_BaseMap`, `_MainTex`, then `mainTexture`, and falls back to `_BaseColor`, `_Color`, or `color` when needed.

For texture samples it uses an 8px-ish grid, computes average luminance, min/max luminance, luminance range, average absolute neighbor delta, and distinct luminance buckets, then checks:

- material/texture presence
- average luminance in the allowed range
- luminance range threshold, with the wall color-fallback exception handled separately
- distinct bucket count, with color fallback allowed to report a one-bucket warning instead of a validation failure
- the expanded `TargetLuminanceBandForReview` window

`WriteSurfaceTextureMetricsV1Batch()` uses the same calculations, writes a Markdown table sorted by `SurfaceId`, and refreshes the AssetDatabase without dirtying the tree during `ValidateHouseSliceBatch()`.

The report writer regenerates the house-slice scene before collecting metrics so it can be run from a clean checkout where the saved scene has not yet been rebuilt by the generator.

## Validation Performed

- `git status --short --branch`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_texture_metric_audit_cycle16_validate_worker_20260522.log"`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dSurfaceTextureMetricAudit.WriteSurfaceTextureMetricsV1Batch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_texture_metric_audit_cycle16_metrics_worker_20260522.log"`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_texture_metric_audit_cycle16_capture_worker_20260522.log"`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_texture_metric_audit_cycle16_validate_parent_20260522.log"`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dSurfaceTextureMetricAudit.WriteSurfaceTextureMetricsV1Batch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_texture_metric_audit_cycle16_metrics_parent_r2_20260522.log"`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_texture_metric_audit_cycle16_capture_parent_20260522.log"`
- `git diff --check -- . ':(exclude)Assets/Scenes/Anemora_FastVS_HouseSlice.unity'`

## Results

- `Fast VS house slice validation passed.`
- `HD2D surface texture metric audit passed`
- `HD2D surface texture metric report written: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_surface_texture_metrics_cycle16_20260522\surface_texture_metrics_cycle16_20260522.md`
- `Fast VS HD2D visual snapshot audit passed: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`

## Report File

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_surface_texture_metrics_cycle16_20260522\surface_texture_metrics_cycle16_20260522.md`

## Unity Side Effects

Unity batchmode left unrelated workspace churn in the tree. These are reported here and will be cleaned by the parent session:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\AddressableAssetsData\link.xml`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\AddressableAssetsData\link.xml.meta`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\AddressableAssetsData\Windows.meta`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\External\OpenGameArt\edomin_tree_sprites_cc0\tree3_0.png.meta`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_house_exterior_tree3_sprite_cc0.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_past_house_exterior_tree3_sprite_cc0.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_static_directional_cast_shadow.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_surface_directional_shade_overlay.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_warm_light_pool_soft.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_static_directional_cast_shadow_soft.png.meta`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_surface_directional_shade_overlay_soft.png.meta`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\ProjectSettings\GraphicsSettings.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\ProjectSettings\QualitySettings.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\ProjectSettings\SceneTemplateSettings.json`

## Risks

- The metric audit is only a review gate; it does not change visible shading yet.
- Color fallback surfaces are intentionally tolerated so the audit can continue to measure walls and other surfaces that do not carry a readable texture.
- Future surface work should keep the audit aligned with any new material/texture swaps so the report stays meaningful.
