# feat(hd2d): trim broad plaza realtime casters

## Intent

Cycle 154 raised shadow sampling quality, but the central plaza still had large blocky realtime shadow shapes because several invisible `ShadowsOnly` casters were too broad. Cycle 155 keeps those casters realtime and shadow-only, but shrinks the broad slab meshes so visible props and narrower occluders carry more of the shadow read.

## Scope

- Preserve realtime Directional Light, VeryHigh shadow resolution, 9-tap receiver filtering, cookie, VS camera, blue-gray sky, sprite response, and visible prop casters.
- Reduce the mesh bounds for the broad Cycle 127/128/134/138 central-plaza shadow casters.
- Keep all trimmed casters enabled as `ShadowsOnly` and non-receiving.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 155 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaTrimBroadRealtimeCastersCycle155Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaTrimBroadRealtimeCastersCycle155ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_trim_broad_realtime_casters_cycle155.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle155_plaza_trim_broad_realtime_casters_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_follow_start.png`
- `parent_review_02_current_central_plaza_follow_forward.png`
- `parent_review_03_current_central_plaza_realtime_shadow_floor.png`
- `parent_review_04_current_central_plaza_realtime_shadow_facade.png`
- `parent_review_05_current_library_guard.png`

## Visual Gate

- Large realtime shadows should remain present, but fewer should read as oversized invisible slabs.
- The change must not reintroduce painted haze, static shadow ribbons, or non-realtime light plates.
