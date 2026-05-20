# 2026-05-20 Fast VS HD2D House Exterior Tree Crown Silhouette Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_tree_crown_silhouette_20260520`

This cycle tightens only the Niro house exterior tree crown silhouette for the HD-2D Fast VS slice. It keeps the existing colliding trunk/crown base intact and adds non-colliding canopy breakup pieces around the crown edges and front. Story, dialogue, font, UI, controls, Time Window behavior, door/map transitions, collider behavior, and trigger behavior were left alone. No external, Meshy, or paid assets were used.

## Implementation

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateHouseExteriorTreeCrownSilhouetteBreakup(...)` and called it from `CreateExterior(...)` immediately after `CreateHouseExteriorTreeFenceSilhouettePolish(...)`.
- Kept the original colliding crown object `Current_TreePixelCrown` / `Past_TreePixelCrown` unchanged.
- Added current-side non-colliding crown breakup pieces:
  - `Current_HouseExterior_TreeCrownSilhouette_FrontLowerLobeA`
  - `Current_HouseExterior_TreeCrownSilhouette_FrontLowerLobeB`
  - `Current_HouseExterior_TreeCrownSilhouette_LeftEdgeBreakA`
  - `Current_HouseExterior_TreeCrownSilhouette_RightEdgeBreakA`
  - `Current_HouseExterior_TreeCrownSilhouette_OuterLeafChipA`
  - `Current_HouseExterior_TreeCrownSilhouette_OuterLeafChipB`
- Added past-side counterparts with a brighter canopy feel:
  - `Past_HouseExterior_TreeCrownSilhouette_FrontLowerLobeA`
  - `Past_HouseExterior_TreeCrownSilhouette_FrontLowerLobeB`
  - `Past_HouseExterior_TreeCrownSilhouette_LeftEdgeBreakA`
  - `Past_HouseExterior_TreeCrownSilhouette_RightEdgeBreakA`
  - `Past_HouseExterior_TreeCrownSilhouette_OuterLeafChipA`
  - `Past_HouseExterior_TreeCrownSilhouette_OuterLeafChipB`
- Added `ValidateFastVsHd2dFortySeventhCycleHouseExteriorTreeCrownSilhouette()` and wired it into `ValidateHouseSliceBatch()`.
- Added `ValidateHouseExteriorTreeCrownSilhouetteObject(...)` for the new crown breakup pieces.
- Added `CaptureHd2dFortySeventhCycleScreenshotsBatch()` and `CaptureHd2dFortySeventhCycleScreenshotsToDirectory(...)`.
- Parent review removed `Current_HouseExterior_TreeCrownSilhouette_TopNotchShadowA` and `Past_HouseExterior_TreeCrownSilhouette_TopHighlightA` because the first capture made them read as black/yellow block artifacts on the crown face.

## Verification

Worker validation:

- Validation command:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle47_worker_validate_20260520.log`
- Validation result: passed with `Fast VS house slice validation passed.`
- Validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle47_worker_validate_20260520.log`

Worker screenshot capture:

- Capture command:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dFortySeventhCycleScreenshotsBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle47_worker_capture_20260520.log`
- Capture result: passed with `Fast VS forty-seventh-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_tree_crown_silhouette_20260520`
- Capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle47_worker_capture_20260520.log`

Parent review and verification:

- Reviewed worker screenshots and rejected the top notch/highlight pieces as block artifacts.
- Patched `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` to keep only the green leaf-lobe silhouette breakup pieces.
- Parent validation command:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle47_parent_validate_20260520.log`
- Parent validation result: passed with `Fast VS house slice validation passed.`
- Parent capture command:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dFortySeventhCycleScreenshotsBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle47_parent_capture_20260520.log`
- Parent capture result: passed and rewrote the four PNGs listed below.
- Parent build command:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle47_parent_build_20260520.log`
- Parent build result: passed with `Build Finished, Result: Success.` and updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`.
- Parent EXE smoke command:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe -batchmode -nographics -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle47_parent_smoke_20260520.log`
- Parent EXE smoke result: ran for 20 seconds, stopped by the verifier, and produced `match_count=0` for `Error|Exception|NullReference|MissingReference|Failed|Crash|Font Atlas Texture|LiberationSans|ScreenSpaceAmbientOcclusion|DrawObjectsPass|RenderGraph`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_tree_crown_silhouette_20260520\01_current_house_exterior_tree_crown_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_tree_crown_silhouette_20260520\02_current_house_exterior_tree_crown_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_tree_crown_silhouette_20260520\03_past_house_exterior_tree_crown_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_tree_crown_silhouette_20260520\04_past_house_exterior_tree_crown_close.png`

## Notes

- The new canopy pieces stay non-colliding and reuse existing materials only.
- Unity batchmode refreshed nearby scene/material/project files outside the intended write scope; those were left untouched.
- No external, Meshy, or paid assets were used.
