# 2026-05-22 Fast VS HD2D Character Contact Shadow Texture Cycle 22

- Branch: `work/fast-vs-hd2d-shading-foundation-20260522`
- Goal: improve the generated shared character contact shadow so Niro, Reto, and Aria read as grounded HD-2D paper characters instead of a thin flat rectangle.

## Implementation

- Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` so `EnsureCharacterContactShadowTexture()` regenerates `FastVS_House_character_contact_shadow.asset` as a 96x48 bilinear texture with transparent corners, smooth edge fade, a broad soft body oval, two stronger foot-contact lobes, and a restrained tail bias.
- Added deterministic validation for the shared contact-shadow texture in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` and wired it into the house slice validation path.
- Added the cycle 22 report writer in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs` so the texture can be measured and written to a markdown report after the scene is created.

## Validation

1. Worker report writer compile/runtime check
   - Command: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dOverlayProfileFoundationAudit.WriteCharacterContactShadowTextureCycle22ReportBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_character_contact_shadow_texture_cycle22_report_worker_20260522.log'`
   - Result: PASS
   - Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_character_contact_shadow_texture_cycle22_report_worker_20260522.log`

2. Worker house slice validation
   - Command: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_character_contact_shadow_texture_cycle22_validate_worker_20260522.log'`
   - Result: FAIL
   - Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_character_contact_shadow_texture_cycle22_validate_worker_20260522.log`
   - Failure: `House slice validation failed: Reto [1.E] past observation did not start with the canonical Niro thought.`
   - Parent diagnosis: this was not pre-existing. The worker had accidentally changed several Japanese story validation literals while editing the same file. Parent review restored those unrelated story-validation lines before final validation.

3. Parent house slice validation after restoring unrelated story-validation changes
   - Command: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_character_contact_shadow_texture_cycle22_validate_parent_20260522.log'`
   - Result: PASS
   - Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_character_contact_shadow_texture_cycle22_validate_parent_20260522.log`

4. Parent report writer
   - Command: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dOverlayProfileFoundationAudit.WriteCharacterContactShadowTextureCycle22ReportBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_character_contact_shadow_texture_cycle22_report_parent_20260522.log'`
   - Result: PASS
   - Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_character_contact_shadow_texture_cycle22_report_parent_20260522.log`

5. Parent visual snapshot audit
   - Command: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_character_contact_shadow_texture_cycle22_capture_parent_20260522.log'`
   - Result: PASS
   - Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_character_contact_shadow_texture_cycle22_capture_parent_20260522.log`

## Metrics

- Texture size: 96x48
- Center alpha: 0.165
- Max alpha: 0.255
- Left edge alpha: 0.000
- Right edge alpha: 0.000
- Left foot alpha: 0.220
- Right foot alpha: 0.212
- Top edge alpha: 0.000
- Bottom edge alpha: 0.000
- Top-left corner alpha: 0.000
- Top-right corner alpha: 0.000
- Bottom-left corner alpha: 0.000
- Bottom-right corner alpha: 0.000
- Result: PASS

## Expected Visible Effect

- Niro, Reto, and Aria should sit on a broader, softer contact shape instead of a narrow rectangular strip.
- The feet should still read clearly at small scale because the two lower lobes hold more weight than the center body.
- The silhouette should fade away at the edges and corners so the texture does not print as a hard plate.

## Side Effects Left For Parent Cleanup

Unity regenerated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`, Addressables temp files, ProjectSettings, material/meta whitespace, and rolling cycle10 snapshot files during validation and report runs. Parent review restored those unrelated side effects before commit so the cycle-22 commit stays limited to the intentional texture, generator, audit, report, and devlog changes.
