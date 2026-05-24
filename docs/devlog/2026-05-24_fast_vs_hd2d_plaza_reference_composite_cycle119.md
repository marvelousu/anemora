# feat(hd2d): repaint plaza reference composite

## Scope

Cycle 119 takes the shortest visual route after Cycle118: stop letting the old broad gold sun bands define the frame. This pass adds a high render-queue current-plaza reference composite that repaints the central plaza floor with a muted stone matte, then re-layers dark occluder shadows, small warm sun flecks, and a light atmospheric veil above the prior overlay stack.

Authored files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dShadingFoundationAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Shaders\FastVS\FastVS_SurfaceRampLit.shader`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsHouseLightingDirector.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\DefaultVolumeProfile.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

Expected generated assets:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_reference_stone_matte_cycle119.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_reference_deep_shadow_cycle119.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_reference_sun_fleck_cycle119.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_reference_air_veil_cycle119.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_plaza_reference_stone_matte_cycle119.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_plaza_reference_deep_shadow_cycle119.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_plaza_reference_sun_fleck_cycle119.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_plaza_reference_air_veil_cycle119.asset`

Existing SurfaceRampLit material assets under `Assets\Art\Materials\FastVS\HouseSlice` are intentionally regenerated for the stronger ramp response. The legacy plaza sun overlay materials for cycles 104/106/109/111/112/113/116/118 are also intentionally dimmed so Cycle119 is not dominated by broad gold bands.

Out of scope:

- Main branch, route logic, story/UI behavior, map geometry redesign, and unrelated Unity ProjectSettings churn.

## Goal Prompt

Reproduce the reference-image shadow quality by the shortest route, with speed prioritized over renderer architecture. The result should visibly replace the banded overlay look with a muted HD-2D floor surface, strong falling shadows, and small sunlit breaks.

Reference directory:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference`

## Implementation Plan

- Add a high-queue stone matte material that neutralizes the older gold bands and gives the floor a faded beige receiver surface.
- Add a high-queue deep shadow material for canopy, library-base, fountain, foreground, and right-edge occluder masses.
- Add small sun-fleck overlays instead of another broad sun strip.
- Add a facade/foreground air veil to restore the reference-like haze after the floor repaint.
- Strengthen the central plaza light profile and surface ramp assumptions so real scene lighting also supports stronger sun/shadow separation.
- Keep all Cycle119 overlays current-world only and non-colliding.
- Validate object counts, material ownership, render queue ordering, overlay metadata, and generated texture alpha metrics.
- Capture the same current overview/close and past/library guard frames for direct comparison.

## Expected Evidence

Parent-review screenshots are expected under:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle119_plaza_reference_composite_parent_review_20260524_01`

Expected files:

- `parent_review_01_current_central_plaza_reference_composite_overview.png`
- `parent_review_02_current_central_plaza_reference_composite_close.png`
- `parent_review_03_past_central_plaza_reference_composite_guard.png`
- `parent_review_04_current_library_reference_composite_guard.png`

## Validation

Planned cycle-runner command:

```powershell
$commitPaths = @(
  'Assets/Editor/AnemoraFastVsHouseSliceSetup.cs',
  'Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs',
  'Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs',
  'Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs',
  'Assets/Art/Shaders/FastVS/FastVS_SurfaceRampLit.shader',
  'Assets/Scripts/FastVS/FastVsHouseLightingDirector.cs',
  'Assets/Settings/DefaultVolumeProfile.asset',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_stone_matte_cycle119.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_stone_matte_cycle119.mat.meta',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_deep_shadow_cycle119.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_deep_shadow_cycle119.mat.meta',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_sun_fleck_cycle119.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_sun_fleck_cycle119.mat.meta',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_air_veil_cycle119.mat',
  'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_air_veil_cycle119.mat.meta',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_stone_matte_cycle119.asset',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_stone_matte_cycle119.asset.meta',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_deep_shadow_cycle119.asset',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_deep_shadow_cycle119.asset.meta',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_sun_fleck_cycle119.asset',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_sun_fleck_cycle119.asset.meta',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_air_veil_cycle119.asset',
  'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_air_veil_cycle119.asset.meta',
  'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_reference_composite_cycle119.md',
  'docs/devlog/INDEX.md',
  'docs/devlog/screenshots/fast_vs_hd2d_cycle119_plaza_reference_composite_parent_review_20260524_01'
)
& .\tools\cycle-runner.ps1 -CycleNumber 119 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaReferenceCompositeCycle119Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaReferenceCompositeCycle119ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_reference_composite_cycle119.md' `
  -Audience parent_review `
  -CommitPath $commitPaths `
  -NoRollback
```

## Visual Gate

Passing criteria:

- Current plaza overview should no longer be dominated by broad gold diagonal bands.
- Floor should read as a muted HD-2D stone receiver with dark falling shadow masses and small warm sun breaks.
- Current close frame should keep route readability while showing stronger facade/ground shadow separation.
- Past plaza and current library guard captures remain usable and are not redesigned.

## Runner Result

- `tools\cycle-runner.ps1` passed validate / capture / build / smoke for Cycle119 at 2026-05-24 15:58 JST.
- Parent-review screenshots were written to the expected Cycle119 directory.
