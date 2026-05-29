# feat(hd2d): globalize realtime light and shadow

## Intent

Cycle 161 pivots away from another central-plaza-only tweak. The review problem is that the captures and validation were proving a narrow plaza frame, so the work could keep adding local shadow changes without demonstrating that light and shadow are a global realtime rule.

This cycle makes Exterior, CentralPlaza, and Library share the realtime Directional Light / receiver / caster policy and captures all three areas as evidence.

## Scope

- Apply the realtime outdoor lighting profile to `Exterior`, `CentralPlaza`, and `Library`.
- Expand realtime receiver grading from central-plaza surfaces to current-world outdoor/library floors, walls, doors, roofs, and matching named receivers.
- Let current-world exterior/library props participate as realtime visible shadow casters instead of only central-plaza objects.
- Keep the central-plaza realtime sun cookie, but prevent Exterior/Library from borrowing that plaza-specific cookie.
- Replace the Cycle161 capture gate with multi-area screenshots, so visual review cannot pass by changing only one plaza crop.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 161 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateGlobalRealtimeLightingCycle161Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureGlobalRealtimeLightingCycle161ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_global_realtime_lighting_cycle161.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle161_global_realtime_lighting_parent_review_20260525_01`

- `parent_review_01_current_exterior_realtime_light.png`
- `parent_review_02_current_central_plaza_realtime_light.png`
- `parent_review_03_current_library_realtime_light.png`
- `parent_review_04_current_exterior_realtime_shadow_receiver.png`
- `parent_review_05_current_library_realtime_shadow_receiver.png`

## Visual Gate

- Exterior, CentralPlaza, and Library must all visibly respond to realtime light/shadow policy.
- CentralPlaza may keep its dedicated realtime sun cookie; Exterior and Library must use direct realtime light without the plaza cookie.
- Review must reject this cycle if the only visible change is another central-plaza shadow patch.
