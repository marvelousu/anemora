# feat(hd2d): add plaza sun slash highlights

## Scope

Cycle 106 follows parent review of Cycle 105. Cycle105 removed the rectangular board look, but the current plaza again became too dark in the overview. This cycle adds a separate, narrow, current-only sunlight slash layer to restore visible sun/shadow contrast without returning to broad translucent boards.

Authored file:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Expected generated side-effect files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_sun_slash_cycle106.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_plaza_sun_slash_cycle106.asset`

Out of scope:

- Main branch, story/UI, doors, time-window behavior, character assets, map layout, ProjectSettings, and map reference implementation.

## Goal Prompt

Add crisp, narrow, soft-edged sunlight slashes over the current central plaza so the floor reads as sun cutting through shadow rather than a single dark field or a broad bright plate.

Reference directory:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference`

## Worker Cycle

- Cycle-worker: `019e56ef-b411-7c42-a448-e300b3b2a44c`
- Worker role: `cycle-worker` / `gpt-5.4-mini`

Scoped prompt trace:

`SCOPED_PROMPT_ISSUED cycle=106 authored_file=C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs validate=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateOutdoorSunSlashHighlightsBatch capture=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureOutdoorSunSlashHighlightsCycle106ScreenshotsBatch`

## Implementation Plan

- Add a deterministic generated plaza sun-slash texture/material pair.
- Add 5-7 narrow current-only non-colliding sun slash quads on the central plaza floor.
- Validate current-only placement, non-collision, overlay profile metadata, and texture edge/alpha metrics.
- Capture current plaza overview/close, past plaza guard, and current library guard screenshots for parent visual review.

## Expected Evidence

Parent-review screenshots are expected under:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle106_outdoor_sun_slash_highlights_parent_review_20260524_01`

Expected files:

- `parent_review_01_current_central_plaza_sun_slash_overview.png`
- `parent_review_02_current_central_plaza_sun_slash_close.png`
- `parent_review_03_past_central_plaza_guard.png`
- `parent_review_04_current_library_guard_wide.png`

## Validation

Planned cycle-runner command:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 106 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateOutdoorSunSlashHighlightsBatch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureOutdoorSunSlashHighlightsCycle106ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_outdoor_sun_slash_highlights_cycle106.md' `
  -Audience parent_review `
  -CommitPath @(
    'Assets/Editor/AnemoraFastVsHouseSliceSetup.cs',
    'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sun_slash_cycle106.mat',
    'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sun_slash_cycle106.mat.meta',
    'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sun_slash_cycle106.asset',
    'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sun_slash_cycle106.asset.meta',
    'docs/devlog/2026-05-24_fast_vs_hd2d_outdoor_sun_slash_highlights_cycle106.md',
    'docs/devlog/INDEX.md',
    'docs/devlog/screenshots/fast_vs_hd2d_cycle106_outdoor_sun_slash_highlights_parent_review_20260524_01'
  ) `
  -NoRollback
```

## Visual Gate

Passing criteria:

- Current plaza has visible, narrow sunlight accents without broad rectangular overlay artifacts.
- The added light does not erase grounded character/prop shadows.
- Past plaza and current library guard captures show no obvious regression.
