# feat(hd2d): tune plaza reference camera grade

## Intent

The reference images rely heavily on lens grade: bright bloom, controlled vignette, and stronger distance blur. The scene now has realtime shadow/light tracking, so Cycle 145 moves the remaining atmosphere work into the existing global postprocess profile rather than painting haze directly onto the map.

## Scope

- Retune `DefaultVolumeProfile` ColorAdjustments, Bloom, Vignette, and Gaussian DepthOfField toward the reference grade.
- Update the setup/audit code so future shading-foundation refreshes keep the same camera-grade contract.
- Keep realtime directional shadows, mesh casters, and receiver shader response from Cycles 140-144.
- Keep RenderSettings fog disabled and avoid map-space haze/air veil overlays.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 145 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaReferenceCameraGradeCycle145Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaReferenceCameraGradeCycle145ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_reference_camera_grade_cycle145.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle145_plaza_reference_camera_grade_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_follow_start.png`
- `parent_review_02_current_central_plaza_follow_forward.png`
- `parent_review_03_current_central_plaza_realtime_shadow_floor.png`
- `parent_review_04_current_central_plaza_realtime_shadow_facade.png`
- `parent_review_05_current_library_guard.png`

## Visual Gate

- Atmosphere should come from postprocess lens grade, not painted fog quads.
- Foreground/background should pick up more HD-2D depth blur while the playable center remains readable.
- Realtime shadows and sunlit response should remain visible after the grade.
