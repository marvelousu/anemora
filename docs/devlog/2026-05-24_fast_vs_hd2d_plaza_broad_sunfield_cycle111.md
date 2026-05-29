# feat(hd2d): add plaza broad sunfield

## Scope

Cycle 111 follows parent review of Cycle 110. Cycle110 made the small sunlit islands stronger and passed validation/build/smoke, but the current plaza overview still read mostly as a dark plane with thin highlights. This cycle adds a broader current-world-only sunfield layer so the plaza has a clear sunlit surface area against the darker dappled shadows.

Authored file:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Expected generated side-effect files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_broad_sunfield_cycle111.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_plaza_broad_sunfield_cycle111.asset`

Out of scope:

- Main branch, map-layout changes, story/UI, doors, time-window behavior, character assets, house facade fixes, ProjectSettings, and `C:\Users\maro6\OneDrive\work\projects\anemora_reference\map_chapter_1\map_chapter_1`.

## Goal Prompt

Add a broad, feathered, warm sunfield across the current central plaza floor so the overview has an immediately readable sunlight corridor between darker shadows. The sunfield must be textured, bounded, and current-world-only.

Reference directory:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference`

Map reference directory noted but not edited this cycle:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\map_chapter_1\map_chapter_1`

## Worker Cycle

- Cycle-worker: `019e574d-143b-7f60-8b71-2797b5fdb3f4`
- Worker role: `cycle-worker` / `gpt-5.4-mini`

Scoped prompt trace:

`SCOPED_PROMPT_ISSUED cycle=111 authored_file=C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs validate=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaBroadSunfieldCycle111Batch capture=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaBroadSunfieldCycle111ScreenshotsBatch`

## Implementation Plan

- Add a generated broad sunfield material/texture pair with warm, desaturated transparent light.
- Add exactly three non-colliding, current-world-only horizontal sunfield quads in the central plaza, aligned with the existing sun direction.
- Validate texture metrics, object count, current-only placement, material/texture provenance, render queue, non-collision, and overlay metadata.
- Capture current plaza overview/close, past plaza guard, and current library guard screenshots for parent visual review.

## Expected Evidence

Parent-review screenshots are expected under:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle111_plaza_broad_sunfield_parent_review_20260524_01`

Expected files:

- `parent_review_01_current_central_plaza_broad_sunfield_overview.png`
- `parent_review_02_current_central_plaza_broad_sunfield_close.png`
- `parent_review_03_past_central_plaza_broad_sunfield_guard.png`
- `parent_review_04_current_library_broad_sunfield_guard.png`

## Validation

Planned cycle-runner command:

```powershell
$commitPaths = @(
  'Assets/Editor/AnemoraFastVsHouseSliceSetup.cs',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_broad_sunfield_cycle111.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_broad_sunfield_cycle111.mat.meta',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_broad_sunfield_cycle111.asset',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_broad_sunfield_cycle111.asset.meta',
  'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_broad_sunfield_cycle111.md',
  'docs/devlog/INDEX.md',
  'docs/devlog/screenshots/fast_vs_hd2d_cycle111_plaza_broad_sunfield_parent_review_20260524_01'
)
& .\tools\cycle-runner.ps1 -CycleNumber 111 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaBroadSunfieldCycle111Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaBroadSunfieldCycle111ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_broad_sunfield_cycle111.md' `
  -Audience parent_review `
  -CommitPath $commitPaths `
  -NoRollback
```

## Visual Gate

Passing criteria:

- Current plaza overview shows a clear broad warm sunlit area rather than only thin highlight lines.
- The sunfield remains feathered and broken enough that it does not read as a flat yellow board.
- Existing darker shadows remain visible and frame the sunlight.
- Past plaza and current library guard captures show no obvious regression.
