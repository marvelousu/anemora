# 2026-05-20 Fast VS HD2D House Exterior Tree Fence Silhouette Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_tree_fence_silhouette_20260520`

This cycle adds a narrow HD-2D silhouette polish pass for the Niro house exterior only. It focuses on the tree base, the west and north fence edges, and a little yard boundary texture. It does not change story, dialogue, font, UI, controls, Time Window behavior, door/map transitions, collider behavior, or trigger behavior. No external, Meshy, or paid assets were used.

## Implementation

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateHouseExteriorTreeFenceSilhouettePolish(...)` and called it from `CreateExterior(...)` after the base tree and fence forms are created.
- Parent review replaced the first worker version of the tree-foot shadow because it read as a black rectangular slab. `Current_HouseExterior_TreeFencePolish_TreeFootShadow` and `Past_HouseExterior_TreeFencePolish_TreeFootDappleShadow` now use a horizontal transparent contact-shadow quad via `CreateHouseExteriorTreeFenceShadowQuad(...)`.
- Added current-side non-colliding silhouette pieces:
  - `Current_HouseExterior_TreeFencePolish_TreeFootShadow`
  - `Current_HouseExterior_TreeFencePolish_TrunkDarkSide`
  - `Current_HouseExterior_TreeFencePolish_CrownLowerShadow`
  - `Current_HouseExterior_TreeFencePolish_CrownLeafChipA`
  - `Current_HouseExterior_TreeFencePolish_CrownLeafChipB`
  - `Current_HouseExterior_TreeFencePolish_WestFenceCapStrip`
  - `Current_HouseExterior_TreeFencePolish_NorthFenceCapStrip`
  - `Current_HouseExterior_TreeFencePolish_BrokenFenceSlatA`
  - `Current_HouseExterior_TreeFencePolish_BrokenFenceSlatB`
- Added past-side non-colliding silhouette pieces:
  - `Past_HouseExterior_TreeFencePolish_TreeFootDappleShadow`
  - `Past_HouseExterior_TreeFencePolish_TrunkWarmSide`
  - `Past_HouseExterior_TreeFencePolish_CrownHighlightChipA`
  - `Past_HouseExterior_TreeFencePolish_CrownHighlightChipB`
  - `Past_HouseExterior_TreeFencePolish_WestFenceCapStrip`
  - `Past_HouseExterior_TreeFencePolish_NorthFenceCapStrip`
  - `Past_HouseExterior_TreeFencePolish_WestFenceFlowerA`
  - `Past_HouseExterior_TreeFencePolish_NorthFenceLeafA`
- Added `ValidateFastVsHd2dFortySixthCycleHouseExteriorTreeFencePolish()` and wired it into `ValidateHouseSliceBatch()`.
- Added `CaptureHd2dFortySixthCycleScreenshotsBatch()` and `CaptureHd2dFortySixthCycleScreenshotsToDirectory(...)`.

## Verification

Worker validation:

- Validation command:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle46_worker_validate_20260520.log`
- Validation result: passed with `Fast VS house slice validation passed.`
- Validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle46_worker_validate_20260520.log`

Worker screenshot capture:

- Capture command:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dFortySixthCycleScreenshotsBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle46_worker_capture_20260520.log`
- Capture result: passed with `Fast VS forty-sixth-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_tree_fence_silhouette_20260520`
- Capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle46_worker_capture_20260520.log`

Parent review and verification:

- Reviewed the first four worker screenshots and rejected the tree base shadow because it looked like a black rectangle.
- Patched `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` so the tree base shadows use the existing transparent contact-shadow texture instead of an opaque shadow cube.
- Parent validation command:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle46_parent_validate_20260520.log`
- Parent validation result: passed with `Fast VS house slice validation passed.`
- Parent capture command:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dFortySixthCycleScreenshotsBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle46_parent_capture_20260520.log`
- Parent capture result: passed and rewrote the four PNGs listed below.
- Parent build command:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle46_parent_build_20260520.log`
- Parent build result: passed with `Build Finished, Result: Success.` and updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`.
- Parent EXE smoke command:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe -batchmode -nographics -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle46_parent_smoke_20260520.log`
- Parent EXE smoke result: ran for 20 seconds, stopped by the verifier, and produced `match_count=0` for `Error|Exception|NullReference|MissingReference|Failed|Crash|Font Atlas Texture|LiberationSans|ScreenSpaceAmbientOcclusion|DrawObjectsPass|RenderGraph`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_tree_fence_silhouette_20260520\01_current_house_exterior_tree_fence_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_tree_fence_silhouette_20260520\02_current_house_exterior_tree_fence_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_tree_fence_silhouette_20260520\03_past_house_exterior_tree_fence_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_tree_fence_silhouette_20260520\04_past_house_exterior_tree_fence_close.png`

## Notes

- The new polish layer stays non-colliding and uses existing materials only.
- The wide overview captures were widened after a first pass because the initial framing read too tightly around the fence and tree.
- No external, Meshy, or paid assets were used.
