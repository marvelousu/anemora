# feat(hd2d): remove plaza realtime sun ribbons

## Intent

After restoring the VS-style plaza camera and blue-gray sky, the foreground still read as a painted diagonal sun/haze band. Cycle 152 keeps the realtime Directional Light cookie, but removes its broad diagonal ribbon math so light no longer looks like a map-painted overlay.

## Scope

- Preserve the realtime central-plaza Directional Light cookie and cycles 147-151 realtime shadow stack.
- Replace the previous diagonal/side-slash cookie bands with a soft dapple field and subtle broad variation.
- Keep map haze, sky-wash, painted light columns, and camera paint plates suppressed.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 152 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaRealtimeCookieNoRibbonCycle152Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaRealtimeCookieNoRibbonCycle152ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_realtime_cookie_no_ribbon_cycle152.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle152_plaza_realtime_cookie_no_ribbon_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_follow_start.png`
- `parent_review_02_current_central_plaza_follow_forward.png`
- `parent_review_03_current_central_plaza_realtime_shadow_floor.png`
- `parent_review_04_current_central_plaza_realtime_shadow_facade.png`
- `parent_review_05_current_library_guard.png`

## Visual Gate

- The plaza should no longer show a large diagonal painted sun ribbon across the foreground.
- Realtime light/shadow tracking should remain intact: Directional Light cookie, live shadowmap receivers, sprite response, and visible prop casters must still validate.
