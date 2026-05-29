# feat(hd2d): unify sun direction

## Scope

Cycle 100 returns the Fast VS HD-2D work to the shadow foundation track. This cycle does not change house geometry, door gaps, story, map layout, characters, or imported assets.

Authored files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsHouseLightingDirector.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs`

## Goal Prompt

Use the reference images under `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference` as the visual target for warm sun direction, richer light/shadow separation, and future sunbeam/air-perspective work.

Map reference under `C:\Users\maro6\OneDrive\work\projects\anemora_reference\map_chapter_1\map_chapter_1` is acknowledged for later map/background cycles, but this cycle intentionally prioritizes shadow direction only.

## Worker Cycle

- Cycle-worker `019e566c-f319-74e0-bc8f-20bda70ec421` implemented the scoped prompt.
- Parent reviewed the diff and preserved the scope: lighting direction, profile/audit sync, validation, and review capture only.

Scoped prompt trace:

`SCOPED_PROMPT_ISSUED cycle=100 authored_file=C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs validate=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateUnifiedSunDirectionBatch capture=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dUnifiedSunDirectionCycle100ScreenshotsBatch`

## Implementation

- Added a shared sun-direction contract in the scene generator: outdoor Exterior/CentralPlaza use `Euler(52, -38, 0)`, Library uses `Euler(56, -38, 0)`, and Interior uses `Euler(48, -38, 0)`.
- Updated runtime `FastVsHouseLightingDirector` profiles to use the same azimuth so live area transitions match generated scene profiles.
- Updated the area-lighting audit to expect the same rotations.
- Adjusted paper-character and static directional cast shadow yaw to `142` so authored shadow overlays point consistently relative to the `-38` degree key-light azimuth.
- Added `ValidateUnifiedSunDirectionBatch`.
- Added `CaptureHd2dUnifiedSunDirectionCycle100ScreenshotsBatch`.

## Expected Evidence

Parent-review screenshots are expected under:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle100_unified_sun_direction_parent_review_20260524_01`

Expected files:

- `parent_review_01_current_exterior_overview.png`
- `parent_review_02_current_central_plaza_library_facade_overview.png`
- `parent_review_03_current_library_reto_desk_close.png`
- `parent_review_04_past_plaza_overview.png`

## Validation

Planned cycle-runner command:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 100 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateUnifiedSunDirectionBatch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dUnifiedSunDirectionCycle100ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_unified_sun_direction_cycle100.md' `
  -Audience parent_review `
  -CommitPath @(
    'Assets/Editor/AnemoraFastVsHouseSliceSetup.cs',
    'Assets/Scripts/FastVS/FastVsHouseLightingDirector.cs',
    'Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs',
    'docs/devlog/2026-05-24_fast_vs_hd2d_unified_sun_direction_cycle100.md',
    'docs/devlog/INDEX.md',
    'docs/devlog/screenshots/fast_vs_hd2d_cycle100_unified_sun_direction_parent_review_20260524_01'
  ) `
  -NoRollback
```

## Visual Gate

The pass bar is that the four parent-review screenshots share one readable sun/shadow direction. If this passes, the next large theme is exaggerated grounded shadows, not more house exterior geometry cleanup.
