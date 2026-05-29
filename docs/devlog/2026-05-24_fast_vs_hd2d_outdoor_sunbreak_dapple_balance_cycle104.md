# feat(hd2d): balance outdoor sunbreak against dappled shadows

## Scope

Cycle 104 follows the parent visual review of Cycle 103. The current central plaza gained broad dappled shadows, but the current-world overview became too muddy and read as a dark plane. This cycle keeps the focus on HD-2D shadow quality by adding sunlight break-through over the current central plaza without touching the map reference work.

Authored file:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Expected generated side-effect files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_sunbreak_cycle104.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_plaza_sunbreak_cycle104.asset`

Out of scope:

- Main branch, story/UI, doors, time-window behavior, character assets, map layout, ProjectSettings, and documentation edits by the worker.
- Map reference implementation from `C:\Users\maro6\OneDrive\work\projects\anemora_reference\map_chapter_1\map_chapter_1`.

## Goal Prompt

Balance the current central plaza so the floor reads closer to the reference image: warm sunlight cutting through darker shadow fields, with enough tonal separation to avoid a flat black-board impression.

Reference directory:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference`

## Worker Cycle

- Cycle-worker: `019e56d4-9007-75a1-82bd-989ccd666ab6`
- Worker role: `cycle-worker` / `gpt-5.4-mini`

Scoped prompt trace:

`SCOPED_PROMPT_ISSUED cycle=104 authored_file=C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs validate=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateOutdoorSunbreakDappleBalanceBatch capture=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureOutdoorSunbreakDappleBalanceCycle104ScreenshotsBatch`

## Implementation Plan

- Add a deterministic generated plaza sunbreak texture/material pair.
- Add current-world-only central plaza warm sunbreak quads over the dappled floor shadow field.
- Validate that the sunbreak objects are non-colliding, current-only, horizontal, and profiled as `LightPool` overlays.
- Capture current plaza overview/close, past plaza comparison, and library guard screenshots for parent visual review.

## Expected Evidence

Parent-review screenshots are expected under:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle104_outdoor_sunbreak_dapple_balance_parent_review_20260524_01`

Expected files:

- `parent_review_01_current_central_plaza_sunbreak_overview.png`
- `parent_review_02_past_central_plaza_sunbreak_overview.png`
- `parent_review_03_current_central_plaza_niro_grounding_close.png`
- `parent_review_04_current_library_shadow_guard_wide.png`

## Validation

Planned cycle-runner command:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 104 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateOutdoorSunbreakDappleBalanceBatch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureOutdoorSunbreakDappleBalanceCycle104ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_outdoor_sunbreak_dapple_balance_cycle104.md' `
  -Audience parent_review `
  -CommitPath @(
    'Assets/Editor/AnemoraFastVsHouseSliceSetup.cs',
    'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sunbreak_cycle104.mat',
    'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sunbreak_cycle104.mat.meta',
    'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sunbreak_cycle104.asset',
    'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sunbreak_cycle104.asset.meta',
    'docs/devlog/2026-05-24_fast_vs_hd2d_outdoor_sunbreak_dapple_balance_cycle104.md',
    'docs/devlog/INDEX.md',
    'docs/devlog/screenshots/fast_vs_hd2d_cycle104_outdoor_sunbreak_dapple_balance_parent_review_20260524_01'
  ) `
  -NoRollback
```

## Visual Gate

Passing criteria:

- Current plaza no longer reads as one flat dark floor mass.
- Warm sunlight break-up remains soft and diagonal, not a rectangular bright plate.
- Existing character contact/directional shadows and library floor shadow quality remain readable.
