# 2026-05-22 Fast VS HD2D Sprite Card World Light Bridge Cycle 24

- Branch: `work/fast-vs-hd2d-shading-foundation-20260522`
- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Goal

Add a restrained world-light bridge to `Anemora/FastVS/SpriteCardRampUnlit` so the Fast VS sprite cards for Niro, Reto, Aria, and sprite-card vegetation can read a small amount of URP main-light color and shadow attenuation without losing the existing paper-edge and rim treatment from Cycle 23.

## Implementation

- Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Shaders\FastVS\FastVS_SpriteCardRampUnlit.shader` to add `_WorldLightStrength` and `_WorldShadowReceiveStrength`, include URP lighting support, pass world position to the fragment stage, and sample `GetMainLight(TransformWorldToShadowCoord(...))` at low strength.
- Kept alpha cutout behavior unchanged and left the Cycle 23 paper-edge, paper-rim, and lower-shade grading intact.
- Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` so `CreateSpriteCardMaterial(...)` writes the new world-light defaults when the shader exposes the properties.
- Extended `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dSpriteCardLightingAudit.cs` so sprite-card validation now requires the new world-light properties and the cycle-24 report writer can inspect `niro_front_sprite`, `niro_walk_front_sprite`, `reto_v02_writing_loop_sprite`, and `aria_v46_normal_loop_breath_sprite`.
- Added the cycle-24 report writer that writes a markdown report under `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_sprite_card_world_light_bridge_cycle24_20260522\`.
- Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md` with the new cycle-24 entry and refreshed the coverage counts.

## Validation

1. Diff sanity check
   - Command: `git diff --check`
   - Result: PASS before Unity ran; the command only reported the usual CRLF-to-LF warning for `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`.
   - Note: a later rerun after the Unity batch passes was noisy because Unity regenerated unrelated scene/material/meta assets that parent cleanup will handle.

2. Cycle 24 report writer
   - Command: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dSpriteCardLightingAudit.WriteSpriteCardWorldLightBridgeCycle24ReportBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_world_light_bridge_cycle24_report_worker_20260522.log'`
   - Result: PASS
   - Report: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_sprite_card_world_light_bridge_cycle24_20260522\sprite_card_world_light_bridge_cycle24_20260522.md`
   - Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_world_light_bridge_cycle24_report_worker_20260522.log`

3. House slice validation
   - Command: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_world_light_bridge_cycle24_validate_worker_20260522.log'`
   - Result: PASS
   - Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_world_light_bridge_cycle24_validate_worker_20260522.log`

4. Parent cycle 24 report writer
   - Command: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dSpriteCardLightingAudit.WriteSpriteCardWorldLightBridgeCycle24ReportBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_world_light_bridge_cycle24_report_parent_20260522.log'`
   - Result: PASS
   - Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_world_light_bridge_cycle24_report_parent_20260522.log`

5. Parent house slice validation
   - Command: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_world_light_bridge_cycle24_validate_parent_20260522.log'`
   - Result: PASS
   - Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_world_light_bridge_cycle24_validate_parent_20260522.log`

6. Parent visual snapshot audit
   - Command: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_world_light_bridge_cycle24_capture_parent_20260522.log'`
   - Result: PASS
   - Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_world_light_bridge_cycle24_capture_parent_20260522.log`

## Parent Review Notes

- Parent review kept the bridge strength deliberately low (`_WorldLightStrength = 0.08`, `_WorldShadowReceiveStrength = 0.05`) so this pass connects sprite cards to scene lighting without repeating the earlier "shadow just got darker" failure mode.
- Parent cleanup kept only the intentional sprite-card material property persistence and restored unrelated Unity-generated scene, Addressables, ProjectSettings, visual-snapshot, overlay material, and texture/meta churn.
- No story/dialogue strings, map topology, character asset paths, gameplay flow, URP pipeline asset, or renderer feature settings were intentionally changed in this cycle.
