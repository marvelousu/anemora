# 2026-05-22 Fast VS HD2D Surface Directional Shade Texture Cycle 20

Scope: Fast VS / HD2D vertical surface shade foundation.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Goal

Replace the surface directional shade overlay texture with a deterministic v2 128x128 RGBA32 asset that reads as a soft diagonal / vertical falloff instead of a flat dark plate. This cycle is a texture-foundation step for reducing the rectangular shadow look on the house facade, plaza library facade, and library back shelf.

## Full Paths

- Setup and generator: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Overlay audit: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs`
- Texture PNG: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_surface_directional_shade_overlay_soft.png`
- Texture meta: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_surface_directional_shade_overlay_soft.png.meta`
- Generated report: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_surface_directional_shade_texture_cycle20_20260522\surface_directional_shade_texture_cycle20_20260522.md`
- Devlog index: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- Unity scene side effect: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- Parent validate log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_directional_shade_texture_cycle20_validate_parent_20260522.log`
- Parent report log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_directional_shade_texture_cycle20_report_parent_20260522.log`
- Parent snapshot log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_directional_shade_texture_cycle20_capture_parent_20260522.log`

## Implementation

- Changed `EnsureSurfaceDirectionalShadeOverlayTexture()` to always regenerate and reimport the texture instead of reusing the previous PNG when it already existed.
- Switched the texture to a deterministic 128x128 RGBA32 write path with bilinear filtering, clamp wrap, readable import, no mipmaps, and uncompressed output.
- Built the alpha field as a soft upper-left / diagonal / vertical falloff with restrained noise so the overlay keeps its HD-2D texture feel without reading as a rectangular plate.
- Kept the dark cool RGB tint in the same asset family and preserved the asset name/path:
  - `FastVS_House_surface_directional_shade_overlay_soft.png`
  - `FastVS_House_surface_directional_shade_overlay_soft`
- Tightened `ValidateSurfaceDirectionalShadeOverlayTexture()` to require 128x128 exactly and to check the new asymmetry and corner limits.
- Added `WriteSurfaceDirectionalShadeTextureCycle20ReportBatch()` in the overlay audit so the PNG can be regenerated, read, measured, and written into a markdown report in one batch step.
- Updated the devlog index so the cycle-20 record is discoverable under 2026-05-22.

## Validation

Commands run from repo root:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_directional_shade_texture_cycle20_validate_worker_20260522_run2.log'
```

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dOverlayProfileFoundationAudit.WriteSurfaceDirectionalShadeTextureCycle20ReportBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_directional_shade_texture_cycle20_report_worker_20260522.log'
```

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_directional_shade_texture_cycle20_capture_worker_20260522.log'
```

Results:

- ValidateHouseSliceBatch: passed. Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_directional_shade_texture_cycle20_validate_worker_20260522_run2.log`
- Texture report writer: passed. Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_directional_shade_texture_cycle20_report_worker_20260522.log`
- Visual snapshot audit: passed. Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_directional_shade_texture_cycle20_capture_worker_20260522.log`
- Parent ValidateHouseSliceBatch: passed. Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_directional_shade_texture_cycle20_validate_parent_20260522.log`
- Parent texture report writer: passed. Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_directional_shade_texture_cycle20_report_parent_20260522.log`
- Parent visual snapshot audit: passed. Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_directional_shade_texture_cycle20_capture_parent_20260522.log`

## Texture Metrics

From `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_surface_directional_shade_texture_cycle20_20260522\surface_directional_shade_texture_cycle20_20260522.md`:

- Width: 128
- Height: 128
- Center alpha: 0.059
- Max alpha: 0.098
- Left edge alpha: 0.000
- Right edge alpha: 0.000
- Top-left interior alpha: 0.082
- Lower-right interior alpha: 0.027
- Top-left corner alpha: 0.000
- Top-right corner alpha: 0.000
- Bottom-left corner alpha: 0.000
- Bottom-right corner alpha: 0.000
- Result: PASS

## Expected Visible Effect

- The house facade, plaza library facade, and library back shelf should pick up a softer, more directional shade cue that reads as surface depth instead of a hard shadow slab.
- The strongest shade remains biased toward the upper-left / left-diagonal side, which gives the overlay a more natural HD-2D wall-falloff shape.

## Residual Risk

- The new texture is deterministic, but future importer changes could still alter the PNG if the asset gets reimported with different settings.
- Unity updated the house slice scene and several generated asset files during the batch runs; those are left in place for parent review.
- The report and validation both depend on the current texture path staying stable at `Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_surface_directional_shade_overlay_soft.png`.
