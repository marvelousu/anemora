# 2026-05-22 Fast VS HD2D Surface Profile Foundation Cycle 15

## Scope

Cycle 15 adds a review/audit-only HD-2D surface profile layer so major environment surfaces can be checked against the same rules before further shading work lands. The visual change is intentionally minimal; the main output is metadata and validation coverage.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Intent

The goal was to make the important floor, wall, furniture, bookshelf, ground, road, and facade surfaces reviewable without changing global lighting, postprocess, or shader behavior.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsHd2dSurfaceProfile.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsHd2dSurfaceProfile.cs.meta`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs.meta`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_surface_profile_foundation_cycle15.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

## Implementation Notes

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsHd2dSurfaceProfile.cs` adds a review-only `MonoBehaviour` marker and the `FastVsHd2dSurfaceKind` enum for audit metadata.

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` now exposes `AddHd2dSurfaceProfile(...)` and attaches surface profiles to the main current/past house interior, house exterior, central plaza, and library surfaces. The new calls cover floor, wall, bed, table, ground, road, roof, desk, and bookshelf surfaces without changing their materials.

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs` scans scene-only `FastVsHd2dSurfaceProfile` instances with `Resources.FindObjectsOfTypeAll<FastVsHd2dSurfaceProfile>()`, checks the required surface IDs, validates the bands and material token rules, and accepts surface materials when their `AnemoraFastVsHd2dRole` tag is one of `SurfaceLit`, `PaperCard`, or `PortalWindow`.

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` also now runs `AnemoraFastVsHd2dSurfaceProfileFoundationAudit.VerifySurfaceProfilesV1();` alongside the existing material, area lighting, and overlay foundation checks.

## Validation Performed

- `git status --short --branch`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_profile_foundation_cycle15_validate_worker_20260522.log"`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_profile_foundation_cycle15_capture_worker_20260522.log"`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_profile_foundation_cycle15_validate_parent_20260522.log"`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_profile_foundation_cycle15_capture_parent_20260522.log"`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_profile_foundation_cycle15_validate_parent_r2_20260522.log"`
- `git diff --check -- . ':(exclude)Assets/Scenes/Anemora_FastVS_HouseSlice.unity'`

## Results

- `Fast VS house slice validation passed.`
- `HD2D surface profile audit passed`
- `Fast VS HD2D visual snapshot audit passed: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`
- `git diff --check -- . ':(exclude)Assets/Scenes/Anemora_FastVS_HouseSlice.unity'` passed after unrelated Unity-generated side effects were cleaned from the worktree.

## Unity Side Effects

Unity batchmode regenerated the following unrelated import churn and scene/settings assets during worker/parent validation. They were cleaned before commit:

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

- The new surface layer is audit coverage, not a shading pass, so the visible change remains intentionally small.
- The surface audit is only as good as the IDs attached in the generator; future surface work should update the required ID list together with the creation sites.
