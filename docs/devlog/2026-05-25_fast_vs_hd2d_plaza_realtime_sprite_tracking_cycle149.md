# feat(hd2d): track plaza sprite realtime light

## Intent

Cycle 148 softened receiver shadows, but characters and sprite/paper cards still risk reading as art placed over the map. Cycle 149 makes those sprite cards sample the same realtime light cookie and shadow attenuation as the plaza receivers, so the player-facing elements follow the live light setup.

## Scope

- Add URP light-cookie sampling to `FastVS_SpriteCardRampUnlit`.
- Use the light-cookie response in sprite/card tint and world-shadow response.
- Drive central-plaza sprite/card `_WorldLightStrength` and `_WorldShadowReceiveStrength` through the realtime rig while preserving lower defaults outside the plaza.
- Validate that central-plaza sprite/card renderers receive realtime light-tracking property blocks.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 149 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeSpriteTrackingCycle149Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaRealtimeSpriteTrackingCycle149ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_realtime_sprite_tracking_cycle149.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle149_plaza_realtime_sprite_tracking_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_follow_start.png`
- `parent_review_02_current_central_plaza_follow_forward.png`
- `parent_review_03_current_central_plaza_realtime_shadow_floor.png`
- `parent_review_04_current_central_plaza_realtime_shadow_facade.png`
- `parent_review_05_current_library_guard.png`

## Visual Gate

- Niro and sprite/paper cards should participate in the same realtime light/shadow field as the plaza floor and facade.
- The change must stay shader/light driven, not camera-space paint or map haze.
