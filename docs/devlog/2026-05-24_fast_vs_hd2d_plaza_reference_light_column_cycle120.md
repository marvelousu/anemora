# feat(hd2d): add plaza reference light column

## Scope

Cycle 120 deliberately breaks from the small overlay iteration path. This pass adds a high render-queue current-plaza light/shadow/air grade plate over Cycle119 so the frame reads closer to the reference: a pale vertical sun column, dark contact-shadow pockets, a suppressor for old sun ribbons, and faded air depth instead of broad gold bands.

Authored files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

Expected generated assets:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_reference_light_column_shadow_cycle120.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_reference_light_column_sun_cycle120.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_reference_light_column_air_cycle120.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_plaza_reference_light_column_shadow_cycle120.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_plaza_reference_light_column_sun_cycle120.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_plaza_reference_light_column_air_cycle120.asset`

Out of scope:

- Main branch, route logic, story/UI behavior, map geometry redesign, and unrelated Unity ProjectSettings churn.

## Goal Prompt

Current Cycle119 screenshots are still too close to the previous banded look. Prioritize the reference image's shadow and sunlight read even if the existing assumptions need to be overridden.

Reference directory:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference`

## Implementation Plan

- Add 12 current-world-only overlays after Cycle119 so they sit on top of the prior repaint stack.
- Use five dark occlusion plates to close the foreground, left player lane, library base, right edge, and facade pockets.
- Use one top-queue dark suppressor plate to knock down the remaining legacy foreground sun ribbon.
- Use three cream-white sun plates to create a visible vertical column, ground catch, and library-base dust lift.
- Use three air-depth plates to soften the back wall and foreground without returning to the old gold haze.
- Keep all Cycle120 overlays non-colliding and validate material paths, texture paths, render queues, overlay kinds, object counts, and texture alpha metrics.
- Capture the same current overview/close and past/library guard frames for direct comparison.

## Expected Evidence

Parent-review screenshots are expected under:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle120_plaza_reference_light_column_parent_review_20260524_01`

Expected files:

- `parent_review_01_current_central_plaza_reference_light_column_overview.png`
- `parent_review_02_current_central_plaza_reference_light_column_close.png`
- `parent_review_03_past_central_plaza_reference_light_column_guard.png`
- `parent_review_04_current_library_reference_light_column_guard.png`

## Validation

Planned cycle-runner command:

```powershell
$commitPaths = @(
  'Assets/Editor/AnemoraFastVsHouseSliceSetup.cs',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_light_column_shadow_cycle120.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_light_column_shadow_cycle120.mat.meta',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_light_column_sun_cycle120.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_light_column_sun_cycle120.mat.meta',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_light_column_air_cycle120.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_light_column_air_cycle120.mat.meta',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_light_column_shadow_cycle120.asset',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_light_column_shadow_cycle120.asset.meta',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_light_column_sun_cycle120.asset',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_light_column_sun_cycle120.asset.meta',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_light_column_air_cycle120.asset',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_light_column_air_cycle120.asset.meta',
  'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_reference_light_column_cycle120.md',
  'docs/devlog/INDEX.md',
  'docs/devlog/screenshots/fast_vs_hd2d_cycle120_plaza_reference_light_column_parent_review_20260524_01'
)
& .\tools\cycle-runner.ps1 -CycleNumber 120 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaReferenceLightColumnCycle120Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaReferenceLightColumnCycle120ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_reference_light_column_cycle120.md' `
  -Audience parent_review `
  -CommitPath $commitPaths `
  -NoRollback
```

## Visual Gate

Passing criteria:

- Current plaza overview should show a readable vertical cream-white light column rather than only horizontal or diagonal gold wash.
- Foreground, left lane, facade base, and right edge should visibly darken into stronger reference-like shadow masses.
- Air depth should pale the back wall and distance without flattening the player lane completely.
- Past plaza and current library guard captures remain usable and are not redesigned.
