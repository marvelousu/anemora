# feat(hd2d): restore plaza focus shadow clarity

## Intent

Cycle 124 follows Cycle 123's aerial lift. Cycle 123 removed the dark overview curtain, but the foreground wash covered the player and close-frame stone too heavily.

This cycle keeps the brighter reference direction while restoring the focused HD-2D read: a clear central subject, pale stone, hard shadow accents, and air pushed farther back.

## Scope

- Add current-only `CreateCentralPlazaReferenceFocusShadowCycle124`.
- Suppress Cycle 123's broad foreground `GroundAtmosphericWashA` and `PlayerLanePaleCatchA`.
- Add 8 current-only overlays using the already validated Cycle 122 sun/air/shadow materials:
  - smaller focus sun plate and back-step sun trim,
  - back-only aerial haze and high facade depth air,
  - player contact, left/right hard cast streaks, and a step lip ink line.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 124 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaReferenceFocusShadowCycle124Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaReferenceFocusShadowCycle124ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_reference_focus_shadow_cycle124.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle124_plaza_reference_focus_shadow_parent_review_20260524_01`

- `parent_review_01_current_central_plaza_reference_surface_remap_overview.png`
- `parent_review_02_current_central_plaza_reference_surface_remap_close.png`
- `parent_review_03_past_central_plaza_reference_surface_remap_guard.png`
- `parent_review_04_current_library_reference_surface_remap_guard.png`

## Visual Gate

- Player and center floor should be more readable than Cycle 123.
- The bright stone read should remain, but no foreground whiteout should dominate the close frame.
- Hard cast lines should be present enough to read as sun direction and eave/prop occlusion.
- Past plaza and current library guard captures should remain unchanged in intent.
