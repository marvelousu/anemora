# 2026-05-22 Fast VS HD2D Sprite Card Edge Rim Cycle 23

- Branch: `work/fast-vs-hd2d-shading-foundation-20260522`
- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Goal

Add a restrained, deterministic paper-edge and rim layer to `Anemora/FastVS/SpriteCardRampUnlit` so the Fast VS paper sprites read less like flat pasted cutouts without changing gameplay, story/dialogue, map topology, or the existing sprite asset paths.

## Implementation

- Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Shaders\FastVS\FastVS_SpriteCardRampUnlit.shader` to add `_PaperEdgeStrength`, `_PaperRimStrength`, and `_PaperLowerShadeStrength` properties.
- Kept the existing `_RampStrength`, `_TopLight`, `_SideShade`, and `_FloorShade` grade intact, then added frame-clamped neighbor alpha sampling through `_BaseMap_TexelSize` so the edge treatment stays inside each sprite frame.
- Used the sampled alpha differences to drive a restrained warm top-left rim and a cool right/lower edge tint while leaving alpha unchanged and returning immediately for transparent pixels.
- Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` so `CreateSpriteCardMaterial(...)` writes the new strength properties when the shader exposes them.
- Extended `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dSpriteCardLightingAudit.cs` so sprite-card validation now checks the new properties and the cycle-23 batch report can inspect `niro_front_sprite`, `niro_walk_front_sprite`, `reto_v02_writing_loop_sprite`, and `aria_v46_normal_loop_breath_sprite`.
- Added the report writer `WriteSpriteCardEdgeRimCycle23ReportBatch()` and the generated markdown report path under `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_sprite_card_edge_rim_cycle23_20260522\`.
- Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md` with the new cycle-23 entry.

## Parent Review Notes

- Parent review adjusted the shader's neighbor alpha sample from a frame-scale-divided offset to a 1.25 source-texel offset. This keeps the paper rim from becoming too thick on multi-frame sprite strips while still preventing cross-frame sampling.
- Parent cleanup kept the intentional sprite-card material property additions and restored unrelated Unity-generated scene, Addressables, ProjectSettings, visual-snapshot, overlay material, and texture meta churn.
- No story/dialogue strings, map topology, character asset paths, or gameplay flow were intentionally changed in this cycle.

## Validation

1. Diff sanity check
   - Command: `git diff --check`
   - Result: PASS
   - Note: parent cleanup removed Unity trailing-whitespace churn from the two external tree sprite-card materials. Git reported the usual CRLF/LF warning for `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`, but no diff-check errors.

2. Cycle 23 report writer
   - Command: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dSpriteCardLightingAudit.WriteSpriteCardEdgeRimCycle23ReportBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_edge_rim_cycle23_report_worker_20260522.log'`
   - Result: PASS
   - Report: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_sprite_card_edge_rim_cycle23_20260522\sprite_card_edge_rim_cycle23_20260522.md`
   - Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_edge_rim_cycle23_report_worker_20260522.log`

3. House slice validation
   - Command: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_edge_rim_cycle23_validate_worker_20260522.log'`
   - Result: PASS
   - Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_edge_rim_cycle23_validate_worker_20260522.log`

4. Parent cycle 23 report writer after review adjustment
   - Command: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dSpriteCardLightingAudit.WriteSpriteCardEdgeRimCycle23ReportBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_edge_rim_cycle23_report_parent_20260522.log'`
   - Result: PASS
   - Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_edge_rim_cycle23_report_parent_20260522.log`

5. Parent house slice validation after review adjustment
   - Command: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_edge_rim_cycle23_validate_parent_20260522.log'`
   - Result: PASS
   - Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_edge_rim_cycle23_validate_parent_20260522.log`

6. Parent visual snapshot audit
   - Command: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_edge_rim_cycle23_capture_parent_20260522.log'`
   - Result: PASS
   - Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_edge_rim_cycle23_capture_parent_20260522.log`

## Side Effects Cleaned By Parent

Unity regenerated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`, `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\AddressableAssetsData\link.xml`, `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\ProjectSettings\GraphicsSettings.asset`, `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\ProjectSettings\QualitySettings.asset`, rolling visual snapshot PNGs, and several unrelated overlay/texture meta files during validation. Parent review restored those unrelated side effects before commit. The remaining material changes are intentional sprite-card property persistence for the new shader inputs.
