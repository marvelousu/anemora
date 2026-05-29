# feat(hd2d): rebalance plaza reference shadow grade

## Intent

Cycle 132 finally made the plaza shadow treatment obvious, but the central sun patch was too broad and flat. Cycle 133 pulls the overexposed light plate back, fixes the runtime sun intensity to a narrower non-blown range, and keeps the late camera grade focused on dark contact, haze, and small sun accents.

## Scope

- Clamp the central-plaza runtime directional light to a fixed reference-shadow value instead of preserving earlier over-bright values.
- Enable central-plaza fog during the runtime grade for a warmer atmospheric falloff.
- Repaint the camera grade textures at higher resolution with darker edge/contact bands and smaller sun flecks.
- Suppress the oversized Cycle 130 sun patch at runtime so the plaza floor no longer blooms into one flat light blob.
- Add Cycle 133 validation, parent-review screenshots, and `docs/review/` output.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 133 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaReferenceShadowRebalanceCycle133Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaReferenceShadowRebalanceCycle133ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_reference_shadow_rebalance_cycle133.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle133_plaza_reference_shadow_rebalance_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_reference_shadow_rebalance_follow.png`
- `parent_review_02_current_central_plaza_reference_shadow_rebalance_floor.png`
- `parent_review_03_current_central_plaza_reference_shadow_rebalance_facade.png`
- `parent_review_04_current_library_reference_shadow_rebalance_guard.png`

## Visual Gate

- Central plaza should no longer be dominated by a single pale floor blob.
- The plaza floor and facade should retain dark bands, contact shade, and warm haze.
- Small light flecks and shafts should remain visible enough to read as sunlight without flattening the scene.
- Library guard should remain visually unchanged.
