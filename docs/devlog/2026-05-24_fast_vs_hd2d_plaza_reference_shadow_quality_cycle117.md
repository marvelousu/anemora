# feat(hd2d): stamp plaza reference shadow quality

## Scope

Cycle 117 is a speed-first visual pass for the reference shadow target. Cycle116 established a much stronger sun/shadow grade, but the parent review found the image still read too much like broad bands. This cycle keeps the solar reset and adds occluder-specific contact/cast-shadow stamps so the current central plaza has more HD-2D shadow texture: eave grounding, door/window drop shadows, fountain and notice-board casts, and broken dappled tree shade.

Authored files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dShadingFoundationAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

Expected generated assets:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_reference_shadow_stack_cycle117.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_reference_dapple_shadow_cycle117.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_plaza_reference_shadow_stack_cycle117.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_plaza_reference_dapple_shadow_cycle117.asset`

Out of scope:

- Main branch, map redesign, story/UI behavior, route logic, and unrelated ProjectSettings churn.

## Goal Prompt

Reproduce the reference-image shadow quality by the shortest path and with speed as the top priority. Do not spend this cycle on physical renderer infrastructure if direct shadow stamps create a faster visible match.

Reference directory:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference`

## Implementation Plan

- Add two generated current-plaza shadow textures: a dense contact/cast-shadow stack and a broken dappled-tree shadow.
- Place twelve current-only shadow overlays around the library eave, door/window drops, fountain, notice board, step edge, tree canopy areas, right ruin edge, and foreground.
- Slightly reduce the Cycle116 broad sun floor alpha so the new occluder shadows read as shaped shadows instead of being washed by light bands.
- Raise the global vignette from subtle readability support to a stronger reference-style shadow frame.
- Validate material ownership, texture metrics, object count, current-world scoping, non-colliding overlays, and overlay profile metadata.
- Capture the same current plaza overview/close and guard frames for direct parent review comparison.

## Expected Evidence

Parent-review screenshots are expected under:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle117_plaza_reference_shadow_quality_parent_review_20260524_01`

Expected files:

- `parent_review_01_current_central_plaza_reference_shadow_quality_overview.png`
- `parent_review_02_current_central_plaza_reference_shadow_quality_close.png`
- `parent_review_03_past_central_plaza_reference_shadow_quality_guard.png`
- `parent_review_04_current_library_reference_shadow_quality_guard.png`

## Validation

Planned cycle-runner command:

```powershell
$commitPaths = @(
  'Assets/Editor/AnemoraFastVsHouseSliceSetup.cs',
  'Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs',
  'Assets/Settings/DefaultVolumeProfile.asset',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_solar_reset_sun_cycle116.mat',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_solar_reset_sun_cycle116.asset',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_shadow_stack_cycle117.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_shadow_stack_cycle117.mat.meta',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_dapple_shadow_cycle117.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_dapple_shadow_cycle117.mat.meta',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_shadow_stack_cycle117.asset',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_shadow_stack_cycle117.asset.meta',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_dapple_shadow_cycle117.asset',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_dapple_shadow_cycle117.asset.meta',
  'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_reference_shadow_quality_cycle117.md',
  'docs/devlog/INDEX.md',
  'docs/devlog/screenshots/fast_vs_hd2d_cycle117_plaza_reference_shadow_quality_parent_review_20260524_01'
)
& .\tools\cycle-runner.ps1 -CycleNumber 117 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaReferenceShadowQualityCycle117Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaReferenceShadowQualityCycle117ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_reference_shadow_quality_cycle117.md' `
  -Audience parent_review `
  -CommitPath $commitPaths `
  -NoRollback
```

## Visual Gate

Passing criteria:

- Current plaza overview must show more small and medium occluder-shaped shadows than Cycle116, not only broad diagonal strips.
- Library base/door, fountain, notice board, and foreground should feel grounded by contact/cast shadows.
- Dappled floor shade should break up the plaza surface without covering the route marker or player readability.
- Past plaza and current library guard captures remain usable.
