# test(hd2d): correct grounded shadow visual gate

## Scope

Cycle 102 is a narrow parent-review evidence correction for the Fast VS HD-2D shading foundation. Cycle 101 strengthened grounded shadows, but one parent-review capture clipped into the house exterior wall and was not useful visual evidence. This cycle keeps the shadow implementation untouched and adds a clean capture batch that proves the grounded shadow result from usable plaza/library angles.

Authored file:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Documentation files authored by parent:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-24_fast_vs_hd2d_grounded_shadow_visual_gate_cycle102.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

Out of scope:

- Shadow texture algorithms, sun/light constants, story, UI, map layout, house facade geometry, generated assets, and ProjectSettings.
- Map reference implementation from `C:\Users\maro6\OneDrive\work\projects\anemora_reference\map_chapter_1\map_chapter_1`; the current user priority remains shadow.

## Worker Cycle

- Cycle-worker: `019e56a3-62b8-7511-943a-27c4fadbbf0c`
- Worker role: `cycle-worker` / `gpt-5.4-mini`

Scoped prompt trace:

`SCOPED_PROMPT_ISSUED cycle=102 authored_file=C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs validate=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateGroundedShadowVisualGateBatch capture=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dGroundedShadowVisualGateCycle102ScreenshotsBatch`

## Implementation Plan

- Add `ValidateGroundedShadowVisualGateBatch` as a thin wrapper around `ValidateExaggeratedGroundedShadowsBatch`.
- Add `CaptureHd2dGroundedShadowVisualGateCycle102ScreenshotsBatch`.
- Add a private Cycle102 capture helper that copies the Cycle101 scene setup and `CYCLE_AUDIENCE` output prefix behavior.
- Capture four grounded-shadow review images from non-clipped CentralPlaza/Library viewpoints.
- Keep house exterior capture out of this visual gate because the current priority is shadow evidence, not facade geometry.

## Expected Evidence

Parent-review screenshots are expected under:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle102_grounded_shadow_visual_gate_parent_review_20260524_01`

Expected files:

- `parent_review_01_current_central_plaza_niro_grounding_overview.png`
- `parent_review_02_current_library_reto_desk_grounding_close.png`
- `parent_review_03_past_central_plaza_grounding_overview.png`
- `parent_review_04_current_library_floor_shadow_wide.png`

## Validation

Planned cycle-runner command:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 102 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateGroundedShadowVisualGateBatch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dGroundedShadowVisualGateCycle102ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_grounded_shadow_visual_gate_cycle102.md' `
  -Audience parent_review `
  -CommitPath @(
    'Assets/Editor/AnemoraFastVsHouseSliceSetup.cs',
    'docs/devlog/2026-05-24_fast_vs_hd2d_grounded_shadow_visual_gate_cycle102.md',
    'docs/devlog/INDEX.md',
    'docs/devlog/screenshots/fast_vs_hd2d_cycle102_grounded_shadow_visual_gate_parent_review_20260524_01'
  ) `
  -NoRollback
```

## Visual Gate

Passing criteria:

- No screenshot is clipped into the house exterior wall or dominated by invalid foreground geometry.
- Niro, ReTo, and the library/plaza objects read as attached to the ground through visible contact and directional shadow masses.
- The result is good enough to move from grounded-shadow proof to the next large theme: sunbeam/lens flare.
