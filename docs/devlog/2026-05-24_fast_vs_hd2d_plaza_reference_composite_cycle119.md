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





## Cycle 119 failure (validate) -- 20260524-155304

```
[15:53:04] Cycle runner starting
[15:53:04]   CycleNumber    : 119
[15:53:04]   ProjectPath    : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work
[15:53:04]   BatchTool      : C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe
[15:53:04]   ValidateMethod : Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaReferenceCompositeCycle119Batch
[15:53:04]   CaptureMethod  : Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaReferenceCompositeCycle119ScreenshotsBatch
[15:53:04]   BuildMethod    : Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch
[15:53:05]   Audience       : parent_review
[15:53:05]   CaptureOutDir  : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots
[15:53:05]   DevlogPath     : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-24_fast_vs_hd2d_plaza_reference_composite_cycle119.md
[15:53:05]   SmokeSeconds   : 20
[15:53:05]   SmokePatterns  : Error|Exception|Assert|NullReference|Font Atlas Texture|DrawObjectsPass|RenderGraph
[15:53:05]   CommitPath     : Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_book.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_bookshelf_front_painted_hd2d_Past_Library_BackWallBookshelfFrontTexturePanel.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_bookshelf_front_painted_hd2d_Past_Library_BackWallShelfWide.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_bookshelf_front_painted_hd2d_Past_Library_LeftSideBookshelf_BookshelfFrontTexturePanel.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_bookshelf_front_painted_hd2d_Past_Library_RightSideBookshelf_BookshelfFrontTexturePanel.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_empty_bookshelf_front_hd2d_Current_Library_BackWallShelfWide.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_empty_bookshelf_front_hd2d_Current_Library_LeftSideBookshelf_EmptyShelfFrontTexturePanel.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_empty_bookshelf_front_hd2d_Current_Library_RightSideBookshelf_EmptyShelfFrontTexturePanel.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_exterior_wall.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_fence.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_grass.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_ground.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_door_detail.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_leaf.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_library_door_detail.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_path.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_roof.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_rubble_detail.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_stone.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_dust.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_broad_sunfield_cycle111.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_air_veil_cycle119.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_air_veil_cycle119.mat.meta; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_deep_shadow_cycle119.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_deep_shadow_cycle119.mat.meta; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_stone_matte_cycle119.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_stone_matte_cycle119.mat.meta; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_sun_fleck_cycle119.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_sun_fleck_cycle119.mat.meta; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_solar_reset_air_cycle116.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_solar_reset_sun_cycle116.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sun_breakthrough_cycle118.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sun_exposure_base_cycle112.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sun_slash_cycle106.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sunbeam_shafts_cycle113.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sunbreak_cycle104.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sunlit_islands_cycle109.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_leaf.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_bed.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_exterior_wall.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_fence.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_furniture.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_grass.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_door_detail.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_interior_wall.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_library_door_detail.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_path.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_roof.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_stone.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_wood_floor.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_pillow.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_rope.mat; Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_sign_paint.mat; Assets/Art/Shaders/FastVS/FastVS_SurfaceRampLit.shader; Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_air_veil_cycle119.asset; Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_air_veil_cycle119.asset.meta; Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_deep_shadow_cycle119.asset; Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_deep_shadow_cycle119.asset.meta; Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_stone_matte_cycle119.asset; Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_stone_matte_cycle119.asset.meta; Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_sun_fleck_cycle119.asset; Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_plaza_reference_sun_fleck_cycle119.asset.meta; Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs; Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs; Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs; Assets/Editor/AnemoraFastVsHouseSliceSetup.cs; Assets/Scripts/FastVS/FastVsHouseLightingDirector.cs; Assets/Settings/DefaultVolumeProfile.asset; docs/devlog/2026-05-24_fast_vs_hd2d_plaza_reference_composite_cycle119.md; docs/devlog/INDEX.md; docs/devlog/screenshots/fast_vs_hd2d_cycle119_plaza_reference_composite_parent_review_20260524_01
[15:53:05]   NoRollback     : True
[15:53:05]   RunLog         : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\tools\logs\cycle-119-20260524-155304.log
[15:53:05] Phase 'validate' begin: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaReferenceCompositeCycle119Batch

===== validate batch log (C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\cycle-119-20260524-155304-validate.log) =====
[Licensing::Module] Trying to connect to existing licensing client channel...
Built from '6000.3/staging' branch; Version is '6000.3.14f1 (d68c3f99a318) revision 14060607'; Using compiler version '194234433'; Build Type 'Release'
[Licensing::IpcConnector] Successfully connected to: "LicenseClient-maro6" at "2026-05-24T06:53:05.3347934Z"
OS: 'Windows 11  (10.0.26200) Core' Language: 'en' Physical Memory: 14177 MB
BatchMode: 1, IsHumanControllingUs: 0, StartBugReporterOnCrash: 0, Is64bit: 1
System  architecture: x64
Process architecture: x64
Date: 2026-05-24T06:53:05Z

COMMAND LINE ARGUMENTS:
C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe
-batchmode
-quit
-projectPath
C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work
-executeMethod
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaReferenceCompositeCycle119Batch
-logFile
C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\cycle-119-20260524-155304-validate.log
Successfully changed project path to: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work
C:/Users/maro6/Documents/Unity/Anemora-fast-vs-v24-hd2d-work
Exiting without the bug reporter. Application will terminate with return code 1
[15:53:08] Phase 'validate' FAILED with exit 1
[15:53:08] NoRollback set; preserving worktree after validate failure
```
