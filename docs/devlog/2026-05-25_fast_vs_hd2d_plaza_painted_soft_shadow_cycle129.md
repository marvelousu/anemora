# feat(hd2d): paint plaza soft shadow grade

## Intent

Cycle 128 made the plaza visibly darker and directional, but the realtime cube-caster shadows were too hard, too large, and still unlike the reference images. Cycle 129 takes the faster visual route: suppress the hard caster stack and replace it with painted, transparent floor shadow/light textures that can carry HD-2D style softness immediately.

## Scope

- Suppress the Cycle 127/128 hard realtime shadow caster objects in the current central plaza.
- Add four current-plaza transparent ground quads that bypass the legacy overlay profile stack:
  - broad soft painted floor shade,
  - canopy shade,
  - foreground shade,
  - warm light flecks.
- Add generated bilinear painted soft-shadow and warm-fleck textures.
- Reduce Cycle 128 camera-grade edge/ray opacity so it grades the image without producing obvious vertical bars.
- Add Cycle 129 validation and parent-review screenshots.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 129 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaPaintedSoftShadowCycle129Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaPaintedSoftShadowCycle129ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_painted_soft_shadow_cycle129.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle129_plaza_painted_soft_shadow_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_painted_soft_shadow_follow.png`
- `parent_review_02_current_central_plaza_painted_soft_shadow_floor_detail.png`
- `parent_review_03_current_central_plaza_painted_soft_shadow_facade.png`
- `parent_review_04_current_library_painted_soft_shadow_guard.png`

## Visual Gate

- Central plaza should no longer show huge blocky realtime shadow silhouettes.
- Floor should read as soft layered shade with warm flecks, closer to the reference image language.
- Follow and facade shots should keep a usable composition with character/facade/floor in frame.
- Library guard should remain unaffected.
