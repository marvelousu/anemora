# feat(hd2d): add plaza realtime dapple casters

## Intent

Cycle 137 fixed the broken follow framing and removed the unexplained dark sky field. Cycle 138 keeps the no-paint/no-fog premise and improves the shadow shape itself by adding realtime `ShadowsOnly` casters for branch, rafter, and canopy detail.

## Scope

- Keep the Cycle 137 VS follow framing and warm clear color.
- Lower central-plaza directional-light bias for tighter realtime shadow detail.
- Add twelve invisible realtime dapple/rafter casters over the central plaza.
- Do not re-enable camera-space plates, painted fog, or painted shadow quads.
- Capture the same follow/floor/facade parent-review set to verify the new casters move with the realtime light.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 138 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeDappleCasterCycle138Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaRealtimeDappleCasterCycle138ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_realtime_dapple_caster_cycle138.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle138_plaza_realtime_dapple_caster_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_follow_start.png`
- `parent_review_02_current_central_plaza_follow_forward.png`
- `parent_review_03_current_central_plaza_realtime_shadow_floor.png`
- `parent_review_04_current_central_plaza_realtime_shadow_facade.png`
- `parent_review_05_current_library_guard.png`

## Visual Gate

- Follow shots must remain on the plaza floor/facade.
- New shadow detail must come from realtime `ShadowsOnly` casters, not map haze or overlay paint.
- Shadow edges should read as richer branch/rafter breakup rather than only broad rectangular slabs.
