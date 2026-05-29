# 2026-05-22 Fast VS HD2D Static Directional Shadow Texture Cycle 21

Scope: Fast VS / HD2D static directional cast shadow texture foundation.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Goal

Upgrade the shared static directional cast shadow texture for the house facade, central plaza library facade, and library back shelf so it reads as a restrained HD-2D cast shadow with a stronger contact core, a softer directional tail, and no flat rectangular silhouette.

## Implementation

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

- Kept the texture identity and import settings for `FastVS_House_static_directional_cast_shadow_soft.png`.
- Reworked `EnsureStaticDirectionalCastShadowTexture()` into a deterministic v2 alpha field with:
  - a stronger contact lobe around the left/mid body,
  - a tapered directional tail toward the right side,
  - transparent corners and edge fade,
  - subtle deterministic noise to avoid a flat plate read.
- Strengthened the static shadow validation path to check:
  - center alpha,
  - core sample alpha at `(64,42)`,
  - tail sample alpha at `(108,40)`,
  - left/right edge alpha,
  - all four corners,
  - max alpha bounds,
  - tail remaining softer than the core.

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs`

- Added `WriteStaticDirectionalShadowTextureCycle21ReportBatch()`.
- Added menu item: `Tools/Anemora/Write HD2D Static Directional Shadow Texture Cycle 21 Report`.
- The writer calls `AnemoraFastVsHouseSliceSetup.CreateHouseSliceScene()` first, reads the texture PNG from the full Windows path, and writes the markdown report to the cycle-21 screenshot directory.

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

- Added the new dated devlog entry and updated the coverage counts.

## Validation

Worker commands run:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_static_directional_shadow_texture_cycle21_validate_worker_20260522.log'
```

- Result: Pass
- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_static_directional_shadow_texture_cycle21_validate_worker_20260522.log`

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dOverlayProfileFoundationAudit.WriteStaticDirectionalShadowTextureCycle21ReportBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_static_directional_shadow_texture_cycle21_report_worker_20260522.log'
```

- Result: Pass
- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_static_directional_shadow_texture_cycle21_report_worker_20260522.log`
- Report: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_static_directional_shadow_texture_cycle21_20260522\static_directional_shadow_texture_cycle21_20260522.md`

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_static_directional_shadow_texture_cycle21_capture_worker_20260522.log'
```

- Result: Pass
- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_static_directional_shadow_texture_cycle21_capture_worker_20260522.log`

Parent review commands run:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_static_directional_shadow_texture_cycle21_validate_parent_20260522.log'
```

- Result: Pass
- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_static_directional_shadow_texture_cycle21_validate_parent_20260522.log`

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dOverlayProfileFoundationAudit.WriteStaticDirectionalShadowTextureCycle21ReportBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_static_directional_shadow_texture_cycle21_report_parent_20260522.log'
```

- Result: Pass
- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_static_directional_shadow_texture_cycle21_report_parent_20260522.log`
- Report: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_static_directional_shadow_texture_cycle21_20260522\static_directional_shadow_texture_cycle21_20260522.md`

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_static_directional_shadow_texture_cycle21_capture_parent_20260522.log'
```

- Result: Pass
- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_static_directional_shadow_texture_cycle21_capture_parent_20260522.log`

## Report Metrics

Source report:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_static_directional_shadow_texture_cycle21_20260522\static_directional_shadow_texture_cycle21_20260522.md`

| Metric | Value |
|---|---:|
| Width | 160 |
| Height | 80 |
| Center alpha | 0.125 |
| Max alpha | 0.173 |
| Left edge alpha | 0.000 |
| Right edge alpha | 0.000 |
| Core sample alpha | 0.133 |
| Tail sample alpha | 0.071 |
| Top-left corner alpha | 0.000 |
| Top-right corner alpha | 0.000 |
| Bottom-left corner alpha | 0.000 |
| Bottom-right corner alpha | 0.000 |
| Result | PASS |

## Expected Visible Effect

The house facade, central plaza library facade, and library back shelf should pick up a restrained cast-shadow shape that reads as contact and depth first, then trails off to the right as a softer directional shadow. It should not read like a flat black slab.

## Residual Risk

The texture and alpha checks are deterministic, but the final in-scene read still depends on lighting, camera angle, and how strongly the overlay is scaled in the parent scene. This cycle intentionally keeps the existing scene structure and material bindings; only the shared PNG, deterministic generator, and metric report changed.

## Unity Side Effects

Unity also regenerated scene YAML IDs, import metadata whitespace, Addressables temp files, ProjectSettings, and the rolling cycle-10 visual snapshot files during validation and snapshot capture. Parent review restored those unrelated side effects before commit so the cycle-21 commit stays limited to the intentional texture, generator, audit, report, and devlog changes.
