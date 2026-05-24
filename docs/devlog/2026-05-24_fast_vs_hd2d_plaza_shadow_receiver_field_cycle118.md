# feat(hd2d): layer plaza shadow receiver field

## Scope

Cycle 118 continues the fastest-path reference-shadow push after Cycle117. The prior pass added occluder-specific stamps, but the visual still leaned on broad diagonal bands. This cycle adds stronger top-layer receiver shadows on the plaza floor and vertical occluder shade on the library facade so the current central plaza reads more like layered HD-2D shadow from architecture, trees, and props.
After the first capture, the dark-only receiver pass still read too close to Cycle117, so this cycle also adds high-queue warm sun breakthrough islands on top of the new receiver shadows to force visible reference-like contrast instead of another subtle overlay.

Authored files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dShadingFoundationAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

Expected generated assets:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_shadow_receiver_field_cycle118.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_facade_occluder_shade_cycle118.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_sun_breakthrough_cycle118.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_plaza_shadow_receiver_field_cycle118.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_plaza_facade_occluder_shade_cycle118.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_plaza_sun_breakthrough_cycle118.asset`

Out of scope:

- Main branch, route logic, story/UI behavior, map layout changes, and unrelated Unity ProjectSettings churn.

## Goal Prompt

Reproduce the reference-image shadow quality by the shortest route, with speed prioritized over renderer architecture. Prefer visible shadow-density improvements on the current central plaza over speculative infrastructure work.

Reference directory:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference`

## Implementation Plan

- Add a high render-queue floor receiver-shadow material that breaks up Cycle116/117 broad bands with darker chipped shadow masses.
- Add a facade occluder-shade material for eave, door-pocket, and window falloff shadows on the library front.
- Add a higher render-queue warm sun breakthrough material so the new dark receiver pass produces obvious sun/shadow contrast instead of only lowering exposure.
- Place fourteen current-only shadow and sun-break overlays around the player lane, foreground, library approach, tree/ruin sides, fountain, and facade.
- Relax the shading-foundation vignette audit cap from the old soft-light limit to the stronger reference-shadow frame already validated in the scene setup.
- Validate current-only scoping, object counts, material/texture ownership, overlay metadata, and generated texture alpha metrics.
- Capture the same current overview/close and guard frames for direct comparison.
- Include the Cycle117 parent-review screenshots in this commit as evidence catch-up because the previous runner left them untracked after push.

## Expected Evidence

Parent-review screenshots are expected under:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle118_plaza_shadow_receiver_field_parent_review_20260524_01`

Expected files:

- `parent_review_01_current_central_plaza_shadow_receiver_field_overview.png`
- `parent_review_02_current_central_plaza_shadow_receiver_field_close.png`
- `parent_review_03_past_central_plaza_shadow_receiver_field_guard.png`
- `parent_review_04_current_library_shadow_receiver_field_guard.png`

## Validation

Planned cycle-runner command:

```powershell
$commitPaths = @(
  'Assets/Editor/AnemoraFastVsHouseSliceSetup.cs',
  'Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_shadow_receiver_field_cycle118.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_shadow_receiver_field_cycle118.mat.meta',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_facade_occluder_shade_cycle118.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_facade_occluder_shade_cycle118.mat.meta',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sun_breakthrough_cycle118.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sun_breakthrough_cycle118.mat.meta',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_shadow_receiver_field_cycle118.asset',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_shadow_receiver_field_cycle118.asset.meta',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_facade_occluder_shade_cycle118.asset',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_facade_occluder_shade_cycle118.asset.meta',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sun_breakthrough_cycle118.asset',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sun_breakthrough_cycle118.asset.meta',
  'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_shadow_receiver_field_cycle118.md',
  'docs/devlog/INDEX.md',
  'docs/devlog/screenshots/fast_vs_hd2d_cycle117_plaza_reference_shadow_quality_parent_review_20260524_01',
  'docs/devlog/screenshots/fast_vs_hd2d_cycle118_plaza_shadow_receiver_field_parent_review_20260524_01'
)
& .\tools\cycle-runner.ps1 -CycleNumber 118 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaShadowReceiverFieldCycle118Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaShadowReceiverFieldCycle118ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_shadow_receiver_field_cycle118.md' `
  -Audience parent_review `
  -CommitPath $commitPaths `
  -NoRollback
```

## Visual Gate

Passing criteria:

- Current plaza overview should show darker, chipped floor-shadow density across the player lane and library approach, not only broad sun stripes.
- Current close frame should show stronger facade eave/window/door falloff while preserving route marker readability.
- The player remains readable against the deeper receiver shadows.
- Past plaza and current library guard captures remain usable and are not redesigned.
