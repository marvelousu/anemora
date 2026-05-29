# 2026-05-23 Fast VS HD-2D Library Shadow Hierarchy Cycle66

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-shading-foundation-20260522`
- Primary file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

## Direction

The user clarified that the HD-2D shadows do not need physically accurate real-world lengths. For this VS build, shadows should read a little exaggerated if that makes the scene feel richer and more legible.

Cycle66 applies that rule to the library interior: floor falloff, rear shelf occlusion, table casts, and the Reto desk/book contact area now have a stronger staged hierarchy.

## Implementation

- Added `CreateLibraryInteriorShadowHierarchyCycle66(...)`.
- Added `ValidateFastVsHd2dCycle66LibraryInteriorShadowHierarchy()`.
- Connected the Cycle66 validator to `ValidateHouseSliceBatch()`.
- Added current-library floor falloff bands:
  - back shelf deep floor occlusion
  - left/right gallery floor falloff
  - Reto desk contact core
  - Reto book contact accent
- Added past-library staged shadows:
  - rear shelf evening band
  - left/right table long casts
  - rear table shared cast
- After screenshot review, softened the small Reto book contact accent from the regular `shadow` material to `hd2d_depth_shadow` so it does not read as a hard black slab.

## Worker Cycle

- Detailed instruction was sent to gpt-5.4-mini worker `019e524b-a220-74f0-9baa-b66de79ebf54`.
- Parent review found the initial exaggerated library bands acceptable, but the Reto book contact accent looked too block-like.
- A refinement instruction was sent to the same worker:
  - reduce the Reto desk contact core X/Z footprint
  - switch the Reto book cast from `materials.Shadow` to `hd2d_depth_shadow`
  - keep all object names stable

## Validation

Unity batch validation passed:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle66_library_shadow_hierarchy_soft_contact_validate_parent_20260523.log`

Screenshot audits passed/captured:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle66_library_shadow_hierarchy_soft_contact_visual_snapshot_parent_20260523.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle66_library_shadow_hierarchy_soft_contact_close_review_parent_20260523.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle66_library_shadow_hierarchy_soft_contact_specific_capture_parent_20260523.log`

Retained evidence:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle66_library_shadow_hierarchy_parent_review_20260523_01`

## Review Notes

- The library now has a stronger shadow hierarchy than the previous flat interior pass.
- The shadow direction intentionally favors readable HD-2D staging over physically exact shadow length.
- The desk geometry still has dark structural blocks in close views. They are not Cycle66 shadow objects, but they should be revisited in a later library furniture polish cycle.
