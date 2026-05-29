# fix(fast-vs): keep exterior exit camera off tree

## Intent

The latest build made the house exit feel unusable: after leaving the house, the exterior follow camera jumped away from Niro and framed the tree area. That looked like the player was embedded in the tree and made the route hard to control.

Cycle 164 replaces the over-aggressive exterior camera clamp with a playable house-exit floor bound, then adds validation that runs the actual interior-to-exterior door transition, checks the follow anchor remains on the exit target, checks it is well away from the tree, and confirms the player can move after the warp.

## Scope

- Move the Exterior follow-camera minimum anchor from the tree/road cluster back to the house-exit playable bounds.
- Add a permanent house-slice validation gate for the interior door exit camera and movement clearance.
- Add Cycle164 review screenshots focused on the exit moment, one movement step after exit, and the exterior road follow guard.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 164 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateExteriorExitTreeClearanceCycle164Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureExteriorExitTreeClearanceCycle164ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_exterior_exit_tree_clearance_cycle164.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle164_exterior_exit_tree_clearance_parent_review_20260525_01`

- `parent_review_01_current_exterior_after_house_exit_playable_camera.png`
- `parent_review_02_current_exterior_after_exit_move_guard.png`
- `parent_review_03_current_exterior_road_follow_guard.png`

## Visual Gate

- Leaving the house should frame Niro at the exterior exit, not the tree.
- The follow camera should remain playable after the first movement step.
- The road-side exterior guard should still render under the realtime light/shadow setup.
