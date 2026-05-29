# feat(hd2d): lift plaza aerial sun grade

## Intent

Cycle 123 continues the fast reference-quality shadow pass after Cycle 122 established brighter stone and hard anchor shadows.

The target is the reference read: pale sunlit stone first, hard building-cast shadows second, then warm air depth. Cycle 122 improved the close frame, but the overview still read too dark and banded.

## Scope

- Add a current-only `CreateCentralPlazaReferenceAerialLiftCycle123` layer after Cycle 122.
- Reuse Cycle 122 sun, air, and shadow materials to move faster and avoid expanding the shader/material surface.
- Suppress the Cycle 122 foreground occlusion band in the current plaza so overview does not collapse into a dark foreground curtain.
- Add 10 current-only overlays:
  - broad backfield and player-lane pale sun lifts,
  - facade pale stone bloom,
  - ground and facade warm air washes,
  - continued vertical sun shaft,
  - diagonal roof-cast shadows and a refined door/step hard shadow.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 123 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaReferenceAerialLiftCycle123Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaReferenceAerialLiftCycle123ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_reference_aerial_lift_cycle123.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle123_plaza_reference_aerial_lift_parent_review_20260524_01`

- `parent_review_01_current_central_plaza_reference_surface_remap_overview.png`
- `parent_review_02_current_central_plaza_reference_surface_remap_close.png`
- `parent_review_03_past_central_plaza_reference_surface_remap_guard.png`
- `parent_review_04_current_library_reference_surface_remap_guard.png`

## Visual Gate

- Current overview should no longer be dominated by the dark foreground curtain.
- Current close frame should keep the Cycle 122 pale stone read while gaining more readable warm air.
- Hard shadows should look like roof/eave/door casts, not random black overlay bands.
- Past plaza and library guard captures remain usable.
