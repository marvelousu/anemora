# feat(hd2d): break plaza sunbreak into streaks

## Scope

Cycle 105 follows parent review of Cycle 104. The current plaza is no longer a flat black mass, but the added sunlight still reads too much like translucent rectangular boards. This cycle keeps the current shadow/light direction and breaks the sunbreak into thinner, more organic streaks.

Authored file:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Expected regenerated side-effect files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_sunbreak_cycle104.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_plaza_sunbreak_cycle104.asset`

Out of scope:

- Main branch, story/UI, doors, time-window behavior, character assets, map layout, ProjectSettings, and map reference implementation.

## Goal Prompt

Make the current plaza sunbreak feel closer to reference-style sunlight: narrow diagonal streaks, dappled gaps, soft feathering, and no obvious rectangular overlay footprint.

Reference directory:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference`

## Worker Cycle

- Cycle-worker: `019e56e4-2f58-7ce0-9a48-a100e8f697ff`
- Worker role: `cycle-worker` / `gpt-5.4-mini`

Scoped prompt trace:

`SCOPED_PROMPT_ISSUED cycle=105 authored_file=C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs validate=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateOutdoorSunbreakStreakBreakupBatch capture=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureOutdoorSunbreakStreakBreakupCycle105ScreenshotsBatch`

## Implementation Plan

- Refine the generated plaza sunbreak material/texture and/or quad scales so the light is streaked rather than board-like.
- Keep the sunbreak current-world-only and non-colliding.
- Add a Cycle105 validation entry point and capture entry point.
- Capture current plaza overview/close, past plaza guard, and current library guard screenshots for parent visual review.

## Expected Evidence

Parent-review screenshots are expected under:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle105_outdoor_sunbreak_streak_breakup_parent_review_20260524_01`

Expected files:

- `parent_review_01_current_central_plaza_streak_breakup_overview.png`
- `parent_review_02_current_central_plaza_niro_grounding_close.png`
- `parent_review_03_past_central_plaza_guard.png`
- `parent_review_04_current_library_shadow_guard.png`

## Validation

Planned cycle-runner command:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 105 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateOutdoorSunbreakStreakBreakupBatch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureOutdoorSunbreakStreakBreakupCycle105ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_outdoor_sunbreak_streak_breakup_cycle105.md' `
  -Audience parent_review `
  -CommitPath @(
    'Assets/Editor/AnemoraFastVsHouseSliceSetup.cs',
    'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sunbreak_cycle104.mat',
    'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sunbreak_cycle104.mat.meta',
    'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sunbreak_cycle104.asset',
    'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sunbreak_cycle104.asset.meta',
    'docs/devlog/2026-05-24_fast_vs_hd2d_outdoor_sunbreak_streak_breakup_cycle105.md',
    'docs/devlog/INDEX.md',
    'docs/devlog/screenshots/fast_vs_hd2d_cycle105_outdoor_sunbreak_streak_breakup_parent_review_20260524_01'
  ) `
  -NoRollback
```

## Visual Gate

Passing criteria:

- The current plaza sunbreak no longer reads as a broad rectangular translucent board.
- The scene keeps the Cycle104 benefit: current plaza is not a single muddy shadow mass.
- Past plaza and current library guard captures show no obvious regression.
