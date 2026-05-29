# 2026-05-22 Fast VS HD2D Warm Light Pool Foundation Cycle 14

## Scope

Cycle 14 makes the existing warm light pools softer and brings the key light-pool overlays under the HD2D overlay profile foundation. This is a restrained local-light pass, not a broad brightness change.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Intent

The goal was to improve the feel of the existing warm pools around the interior table, the exterior door threshold, the plaza library facade window, and the library desk / entry-floor regions while keeping the change product-focused and subtle.

This cycle does not reapply `Anemora/FastVS/SurfaceRampLit` to the current interior floor, wall, or furniture.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_warm_light_pool.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_warm_light_pool_soft.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_warm_light_pool_soft.asset.meta`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_warm_light_pool_foundation_cycle14.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\04_current_library_visual_snapshot.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\visual_snapshot_metrics_cycle10_20260522.md`

## Implementation Notes

`EnsureHd2dWarmLightPoolMaterial()` now uses a generated soft oval texture, `FastVS_House_hd2d_warm_light_pool_soft`, instead of a flat transparent fill. The material keeps the existing `hd2d_warm_light_pool` id/name, the `OverlayGlow` role, and render queue `3009`.

`CreateHd2dDepthFraming()` now captures the returned light-pool cubes directly and attaches `FastVsHd2dOverlayProfile` for:

- `Current_HouseInterior_Table_WarmLightPool`
- `Past_HouseInterior_Table_WarmLightPool`
- `Past_HouseExterior_Door_WarmPool`
- `Past_CentralPlaza_LibraryFacade_WindowWarmPool`
- `Current_Library_RetoDesk_WarmPool`
- `Current_Library_EntryFloor_SoftDustPool`

The overlay profile audit now validates expected material roles explicitly. Light pools are checked as `OverlayGlow`, while the existing shadow and shade overlays remain checked as `ContactShadow`.

`AddHd2dOverlayProfile()` now forces profile targets to keep `MeshRenderer` enabled, `shadowCastingMode` off, and `receiveShadows` false.

## Validation Performed

- `git status --short --branch`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_warm_light_pool_foundation_cycle14_validate_worker_20260522.log"`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_warm_light_pool_foundation_cycle14_capture_worker_20260522.log"`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_warm_light_pool_foundation_cycle14_validate_parent_20260522.log"`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_warm_light_pool_foundation_cycle14_capture_parent_20260522.log"`
- `git diff --check -- . ':(exclude)Assets/Scenes/Anemora_FastVS_HouseSlice.unity'`

## Results

- `Fast VS house slice validation passed.`
- `Fast VS HD2D visual snapshot audit passed: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`
- `git diff --check` passed.

## Unity Side Effects

Unity batchmode also regenerated unrelated import churn and scene/settings assets in the worktree. Those were not manually edited:

- `Assets/AddressableAssetsData/link.xml`
- `Assets/AddressableAssetsData/link.xml.meta`
- `Assets/Art/External/OpenGameArt/edomin_tree_sprites_cc0/tree3_0.png.meta`
- `Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_tree3_sprite_cc0.mat`
- `Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_tree3_sprite_cc0.mat`
- `Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_static_directional_cast_shadow.mat`
- `Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_surface_directional_shade_overlay.mat`
- `Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_static_directional_cast_shadow_soft.png.meta`
- `Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_surface_directional_shade_overlay_soft.png.meta`
- `Assets/Scenes/Anemora_FastVS_HouseSlice.unity`
- `ProjectSettings/GraphicsSettings.asset`
- `ProjectSettings/QualitySettings.asset`
- `Assets/AddressableAssetsData/Windows.meta`
- `ProjectSettings/SceneTemplateSettings.json`

## Risks

- The warm pools are still intentionally subtle; this pass improves local softness, not scene-wide exposure.
- The overlay profile audit is now stricter about material role and light-pool texture identity, so future light-pool tuning should update the audit together with the generator.
