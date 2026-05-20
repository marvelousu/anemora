# 2026-05-20 Fast VS HD2D House Exterior External Tree Sprite Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- External source asset: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\External\OpenGameArt\edomin_tree_sprites_cc0\tree3_0.png`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_external_tree_sprite_20260520`

This cycle replaces the visible Niro house exterior block-tree crown with an alpha PNG tree sprite while preserving the original generated tree trunk/crown colliders as hidden collision shells. Story, dialogue, UI, font, controls, Time Window behavior, door/map transitions, and trigger behavior were left alone.

## Source Asset

- Source page: `https://opengameart.org/content/tree-sprites-0`
- Source file: `https://opengameart.org/sites/default/files/tree3_0.png`
- Source page author credit: `edomin`
- Source page license: `CC0`
- Local source note: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\External\OpenGameArt\edomin_tree_sprites_cc0\README.md`

No Meshy/API-generated or paid assets were used in this cycle.

## Implementation

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `OpenGameArtTreeSpritePath`.
- Added `CreateHouseExteriorExternalTreeSprite(...)`, which creates:
  - `Current_HouseExterior_ExternalTreeSprite_OpenGameArtTree3`
  - `Past_HouseExterior_ExternalTreeSprite_OpenGameArtTree3`
- Added `ExternalSpriteMaterial(...)` for point-filtered alpha sprite materials based on the imported PNG.
- Kept `Current_TreeBillboardLikeTrunk`, `Current_TreePixelCrown`, `Past_TreeBillboardLikeTrunk`, and `Past_TreePixelCrown` as collision shells, but disabled their renderers so the block shapes no longer show.
- Stopped creating the old tree crown silhouette breakup blocks and removed the crown/trunk helper blocks from the tree fence polish path.
- Added `ValidateFastVsHd2dFortyEighthCycleHouseExteriorExternalTreeSprite()` and wired it into `ValidateHouseSliceBatch()`.
- Added `CaptureHd2dFortyEighthCycleScreenshotsBatch()` and `CaptureHd2dFortyEighthCycleScreenshotsToDirectory(...)`.

## Verification

Rejected worker attempts before this parent implementation:

- Cycle48 transparent generated billboard was rejected because it rendered as a black rectangle.
- Cycle48b opaque small-block crown was rejected because it read as a block model rather than HD-2D foliage.

Parent validation:

- Validation command:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle48_external_tree_parent_validate_20260520.log`
- Validation result: passed with `Fast VS house slice validation passed.`
- Validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle48_external_tree_parent_validate_20260520.log`

Parent screenshot capture:

- Capture command:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dFortyEighthCycleScreenshotsBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle48_external_tree_parent_capture_20260520.log`
- Capture result: passed with `Fast VS forty-eighth-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_external_tree_sprite_20260520`
- Capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle48_external_tree_parent_capture_20260520.log`

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_external_tree_sprite_20260520\01_current_house_exterior_external_tree_sprite_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_external_tree_sprite_20260520\02_current_house_exterior_external_tree_sprite_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_external_tree_sprite_20260520\03_past_house_exterior_external_tree_sprite_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_external_tree_sprite_20260520\04_past_house_exterior_external_tree_sprite_close.png`

## Notes

- The tree is now visibly a pixel-art sprite rather than a cube crown.
- The original collision shells remain in place to avoid changing navigation, Time Window occlusion behavior, or arrival landmark bookkeeping.
- The remaining visual mismatch risk is style harmonization: the tree sprite is higher quality and more organic than the surrounding provisional house exterior materials, so future cycles should either improve nearby wall/fence/ground textures or replace more vegetation with matched sprite assets.
