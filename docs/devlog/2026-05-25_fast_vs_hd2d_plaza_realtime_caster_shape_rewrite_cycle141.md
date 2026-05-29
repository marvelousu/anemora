# feat(hd2d): rewrite plaza realtime caster shapes

## Intent

Cycle 140 proved that irregular realtime mesh casters read better than painted haze or camera plates, but older cube-slab casters still dominated the frame with rectangular shadow masses. Cycle 141 keeps the lighting fully realtime and rewrites the worst legacy caster shapes into ragged mesh silhouettes.

## Scope

- Replace 10 existing central-plaza realtime caster meshes in Cycle 127, 128, 134, and 138 objects.
- Preserve their world positions, rotations, names, `ShadowsOnly` behavior, and non-collider status so runtime shadow policy still owns them.
- Keep the corrected Cycle 137 VS follow camera and stable sky color.
- Do not add painted floor shade, fog, haze, air veil, or screen-space light plates.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 141 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeCasterShapeRewriteCycle141Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaRealtimeCasterShapeRewriteCycle141ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_realtime_caster_shape_rewrite_cycle141.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle141_plaza_realtime_caster_shape_rewrite_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_follow_start.png`
- `parent_review_02_current_central_plaza_follow_forward.png`
- `parent_review_03_current_central_plaza_realtime_shadow_floor.png`
- `parent_review_04_current_central_plaza_realtime_shadow_facade.png`
- `parent_review_05_current_library_guard.png`

## Visual Gate

- The dominant plaza shadows should be less rectangular while remaining tied to realtime `Directional Light` shadow casting.
- The corrected follow-camera view should stay on the playable plaza, not the old map edge.
- Sky/background should remain a simple stable VS clear color, with no restored fog veil.
