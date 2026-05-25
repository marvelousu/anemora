# feat(hd2d): clamp exterior realtime camera anchor

## Intent

Cycle 162 improved global realtime camera/exposure, but the exterior review still read as a roof-blocked diagnostic crop. That makes the shadow work hard to judge and does not match the requested VS-like camera behavior.

Cycle 163 clamps the Exterior follow camera anchor away from the house edge, so the realtime lighting view opens onto the exterior ground/road instead of being dominated by foreground roof geometry.

## Scope

- Clamp Exterior follow-camera anchor X/Z to the open exterior road/yard side.
- Keep the Cycle162 Exterior follow camera offset, procedural skybox, and global realtime receiver/caster policy.
- Move the exterior close shadow capture from roof/foliage to an exterior road/ground receiver view.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 163 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateGlobalRealtimeExteriorAnchorCycle163Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureGlobalRealtimeExteriorAnchorCycle163ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_global_realtime_exterior_anchor_cycle163.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle163_global_realtime_exterior_anchor_parent_review_20260525_01`

- `parent_review_01_current_exterior_realtime_light.png`
- `parent_review_02_current_central_plaza_realtime_light.png`
- `parent_review_03_current_library_realtime_light.png`
- `parent_review_04_current_exterior_realtime_shadow_receiver.png`
- `parent_review_05_current_library_realtime_shadow_receiver.png`

## Visual Gate

- Exterior should no longer be mostly foreground roof.
- Exterior shadow receiver review should show ground/road light and shadow.
- CentralPlaza and Library must stay under the same realtime multi-area validation.
