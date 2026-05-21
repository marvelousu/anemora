# 2026-05-22 Fast VS HD2D Overlay Profile Foundation Cycle 13

## Scope

Cycle 13 adds a metadata-and-audit foundation for HD-2D overlay authoring in the Fast VS house slice. It does not change the visual target in any meaningful way; it only makes the existing contact shadows, directional cast shadows, and vertical shade overlays reviewable as structured scene data.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Intent

The goal is to make the current overlay set auditable by type, area, world state, subject dynamism, intended opacity band, intended footprint, and intended tint, so later cycles can validate or revise the overlays without guessing which objects are contact shadows, cast shadows, or vertical shade panels.

This cycle keeps the current interior floor, wall, and furniture on URP Lit. It does not reapply `Anemora/FastVS/SurfaceRampLit` there.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs.meta`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsHd2dOverlayProfile.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsHd2dOverlayProfile.cs.meta`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_overlay_profile_foundation_cycle13.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

## Implementation Notes

`FastVsHd2dOverlayProfile` is a review-only marker component in `Anemora.FastVS`. It stores:

- `overlayId`
- `areaId`
- `overlayKind`
- `currentWorld`
- `dynamicSubject`
- `opacityBand`
- `footprintWorldSize`
- `intendedTint`

`AnemoraFastVsHouseSliceSetup` now attaches that profile to the curated overlay set while the scene is generated, including:

- Niro contact shadow, foot contact, and directional cast shadow
- Reto contact shadow, foot contact, and directional cast shadow
- Aria contact shadow, foot contact, and directional cast shadow
- current and past house exterior static directional cast shadows
- current and past central plaza static directional cast shadows
- current and past library static directional cast shadows
- current and past house exterior surface directional shade overlays
- current and past central plaza surface directional shade overlays
- current and past library surface directional shade overlays

Parent review replaced the initial `GameObject.Find` attachment sites with direct references returned by the overlay creation helpers, so profile creation fails at the exact generator call site if a target is unexpectedly null.

The new editor audit, `AnemoraFastVsHd2dOverlayProfileFoundationAudit`, checks that each object still exists in `Assets/Scenes/Anemora_FastVS_HouseSlice.unity`, still carries `FastVsHd2dOverlayProfile`, still has `MeshRenderer` shadow casting disabled, still does not receive shadows, and still uses the `AnemoraFastVsHd2dRole=ContactShadow` material tag for the shadow and shade overlay kinds.

## Validation Performed

- `git status --short --branch`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_overlay_profile_foundation_cycle13_validate_worker_20260522.log"`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_overlay_profile_foundation_cycle13_capture_worker_20260522.log"`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_overlay_profile_foundation_cycle13_validate_parent_20260522.log"`
- `& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -quit -projectPath "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work" -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile "C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_overlay_profile_foundation_cycle13_capture_parent_20260522.log"`
- `git diff --check -- . ':(exclude)Assets/Scenes/Anemora_FastVS_HouseSlice.unity'`

## Results

- `Fast VS house slice validation passed.`
- `Fast VS HD2D visual snapshot audit passed: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`
- `git diff --check` passed.

## Unity Side Effects

Unity batchmode left unrelated import churn in the worktree, including:

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

Those files are batch side effects, not part of the overlay-profile foundation itself.

## Risks

- The new audit is intentionally strict about object identity and metadata values, so any later overlay tuning will need a corresponding audit update.
- The component is review metadata only. It does not change rendering behavior at runtime.
