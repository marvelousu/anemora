# 2026-05-22 Fast VS HD2D Current Interior Surface Readability Cycle 12

## Scope

Cycle 12 improves the current-world house interior surface readability while preserving the Cycle 10 black-crush guard. The accepted implementation does not reapply `SurfaceRampLit` to `current_interior_floor`, `current_interior_wall`, or `current_furniture`.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Intent

The goal is to make the interior less flat without repeating the failed broad ramp rollout. The safe path is texture and plate readability: brighter authored palettes and denser repeat scale for the current interior floor, wall, and furniture plates, while those materials remain on `Universal Render Pipeline/Lit`.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_interior_floor.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_interior_wall.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_furniture.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_current_interior_floor_hd2d_plate.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_current_interior_wall_hd2d_plate.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_current_furniture_hd2d_plate.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_current_interior_surface_readability_cycle12.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\01_current_house_interior_visual_snapshot.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\02_current_house_exterior_visual_snapshot.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\03_current_central_plaza_visual_snapshot.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\04_current_library_visual_snapshot.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\visual_snapshot_metrics_cycle10_20260522.md`

## Parent Review Correction

The worker attempt reintroduced the current-interior materials to `SurfaceRampLit`, but both worker and parent visual snapshot runs failed with `house_interior: local contrast too low (0.0100)`. The screenshot showed the same black-wall failure mode as Cycle 9.

Parent review rejected that implementation. The final direction keeps the current-interior floor, wall, and furniture materials out of `SurfaceRampLit`, keeps the Cycle 10 visual gate intact, and limits this cycle to safer authored texture brightness and repeat-scale changes.

## Implementation Notes

- `SampleCurrentInteriorFloorHd2dPixel`, `SampleCurrentInteriorWallHd2dPixel`, and `SampleCurrentFurnitureHd2dPixel` now use brighter current-world interior palettes.
- Current interior plate tiling is denser:
  - floor: `8 x 6`
  - wall: `6 x 4`
  - furniture: `4 x 4`
- `ShouldUseSurfaceRampShader()` still excludes `current_interior_floor`, `current_interior_wall`, and `current_furniture`.

## Validation Performed

Parent validation:

- `git diff --check -- . ':(exclude)Assets/Scenes/Anemora_FastVS_HouseSlice.unity'`
- Unity batch house validation:
  - `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_current_interior_surface_readability_cycle12_validate_parent_final_20260522.log'`
  - Result: `Fast VS house slice validation passed.`
- Unity batch visual snapshot audit:
  - `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_current_interior_surface_readability_cycle12_capture_parent_final_20260522.log'`
  - Result: `Fast VS HD2D visual snapshot audit passed.`

Visual snapshot metrics after the accepted parent correction:

- `house_interior` average luminance: `0.211`
- `house_interior` local contrast: `0.0212`
- `house_interior` PNG size: `477974` bytes
- Full metrics file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\visual_snapshot_metrics_cycle10_20260522.md`

## Cleanup

- Keep only the source, current-interior material/texture, devlog, and index changes.
- Revert scene YAML churn, ProjectSettings churn, Addressables link churn, unrelated material/meta churn, and failed snapshot churn from the rejected SurfaceRamp attempt.

## Risks

- This is a conservative readability pass, not the final interior lighting solution.
- A future dedicated interior shader should be attempted only after it can pass the visual snapshot gate and avoid the black-wall failure mode.

## Next Steps

- Build a dedicated interior shader only if it can preserve the visible wall/floor/furniture luminance seen in the visual snapshot audit.
- Continue HD-2D improvement through area-specific grading and object-level contact/rim treatments rather than forcing the generic surface ramp onto all interior materials.
