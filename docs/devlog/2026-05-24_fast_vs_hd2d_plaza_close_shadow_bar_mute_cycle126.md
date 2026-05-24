# feat(hd2d): mute plaza close shadow bars

## Intent

Cycle 125 made the central plaza visibly brighter, but the close parent review still read as a black horizontal threshold and a hard tile grid before the HD-2D light/shadow hierarchy could work.

Cycle 126 makes a faster, more destructive visual pass on the current central plaza: remove the old broad cast-shadow bar sources, soften the current path plate texture, and replace the close read with a pale receiver wash plus short contact shadows.

## Scope

- Add current-only `CreateCentralPlazaReferenceCloseShadowBarMuteCycle126`.
- Disable close-shot bar sources:
  - old current central-plaza static directional cast shadows,
  - old surface directional shade overlays,
  - Cycle 68/70 shadow foundation ribbons,
  - the library approach threshold shadow cube,
  - a few legacy facade/cool-light strips that reintroduced flat horizontal bands.
- Add 6 current-only Cycle 126 overlays using the Cycle 125 materials:
  - warm mute over the black library approach bar,
  - close-frame grid wash,
  - two air unifier layers,
  - short step/player contact shadows.
- Retune the generated current path plate so seams are less black and less grid-dominant while staying inside the HD2D surface metric audit.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 126 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaReferenceCloseShadowBarMuteCycle126Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaReferenceCloseShadowBarMuteCycle126ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_close_shadow_bar_mute_cycle126.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle126_plaza_close_shadow_bar_mute_parent_review_20260524_01`

- `parent_review_01_current_central_plaza_reference_surface_remap_overview.png`
- `parent_review_02_current_central_plaza_reference_surface_remap_close.png`
- `parent_review_03_past_central_plaza_reference_surface_remap_guard.png`
- `parent_review_04_current_library_reference_surface_remap_guard.png`

## Visual Gate

- The close shot should no longer be dominated by the black library approach bar.
- The current path/stone grid should recede under a pale, desaturated receiver.
- Dark detail should come from compact contact shadows, not broad horizontal ribbons.
- Past plaza and current library guard captures should remain unchanged in intent.
