# feat(hd2d): add cinematic dappled ground shadows

## Scope

Cycle 103 continues the Fast VS HD-2D shading foundation after Cycle 102 corrected the grounded-shadow visual evidence. The current user priority is shadow quality, so this cycle stays on shadow composition and does not implement the new map reference yet.

Authored file:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Generated side-effect files expected from batch validation/capture:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_dappled_cast_shadow_cycle103.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_dappled_cast_shadow_cycle103.asset`

Documentation files authored by parent:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-24_fast_vs_hd2d_cinematic_dappled_ground_shadows_cycle103.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

Out of scope:

- Story, UI, doors, time-window behavior, character assets, desks/shelves, existing map layout, ProjectSettings, and main branch.
- Map reference implementation from `C:\Users\maro6\OneDrive\work\projects\anemora_reference\map_chapter_1\map_chapter_1`.

## Goal Prompt

Move the scene away from flat darkening and toward the reference-like HD-2D shadow language: broad directional shadow fields, smaller organic dark pockets, and soft transparent edges that break up the floor without becoming rectangular black boards.

Reference directory:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference`

## Worker Cycle

- Cycle-worker: `019e56ae-c8f6-7b33-9e18-126fa2a05ce7`
- Worker role: `cycle-worker` / `gpt-5.4-mini`

Scoped prompt trace:

`SCOPED_PROMPT_ISSUED cycle=103 authored_file=C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs validate=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateCinematicDappledGroundShadowsBatch capture=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dCinematicDappledGroundShadowsCycle103ScreenshotsBatch`

## Implementation Plan

- Add a deterministic generated `hd2d_dappled_cast_shadow_cycle103` texture and transparent contact-shadow material.
- Place non-colliding horizontal shadow quads on central plaza and library floors in both current/past spaces.
- Validate texture alpha metrics and object placement so the dappled shadows stay soft, horizontal, non-rectangular, and shadow-safe.
- Capture plaza/library overview and ReTo close views for parent visual review.

## Expected Evidence

Parent-review screenshots are expected under:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle103_cinematic_dappled_ground_shadows_parent_review_20260524_01`

Expected files:

- `parent_review_01_current_central_plaza_dappled_shadow_overview.png`
- `parent_review_02_past_central_plaza_dappled_shadow_overview.png`
- `parent_review_03_current_library_dappled_floor_shadow_wide.png`
- `parent_review_04_current_library_reto_dappled_shadow_close.png`

## Validation

Planned cycle-runner command:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 103 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateCinematicDappledGroundShadowsBatch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dCinematicDappledGroundShadowsCycle103ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_cinematic_dappled_ground_shadows_cycle103.md' `
  -Audience parent_review `
  -CommitPath @(
    'Assets/Editor/AnemoraFastVsHouseSliceSetup.cs',
    'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_dappled_cast_shadow_cycle103.mat',
    'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_dappled_cast_shadow_cycle103.mat.meta',
    'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_dappled_cast_shadow_cycle103.asset',
    'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_dappled_cast_shadow_cycle103.asset.meta',
    'docs/devlog/2026-05-24_fast_vs_hd2d_cinematic_dappled_ground_shadows_cycle103.md',
    'docs/devlog/INDEX.md',
    'docs/devlog/screenshots/fast_vs_hd2d_cycle103_cinematic_dappled_ground_shadows_parent_review_20260524_01'
  ) `
  -NoRollback
```

## Visual Gate

Passing criteria:

- Shadows read as varied dappled floor shade rather than flat dark rectangles.
- Existing grounded character/prop shadows remain readable.
- Plaza and library floors gain stronger tonal rhythm without hiding navigation cues or story objects.

## Retry Note

The first validate attempt found the generated dappled texture was slightly under the intended opacity contract (`0.278` max alpha against a `0.28` lower bound). Parent adjusted only the texture density coefficients before rerunning the cycle gate.

The second validate attempt found a validation-coordinate mismatch: the actual shadow placement correctly used existing absolute map coordinates, while the new validator expected area-relative coordinates. Parent updated only the validator bounds to match the repository's scene-generation contract before rerunning the full gate.
