# 2026-05-22 Fast VS HD2D Area Lighting Profile Cycle 11

## Scope

Cycle 11 adds a repeatable foundation for area-specific HD-2D lighting profiles in the Fast VS house slice. It introduces explicit profile marker objects for the current-world house interior, house exterior, central plaza, and library, plus a dedicated audit so those profiles can be validated in batchmode.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Intent

The goal is to make area lighting reviewable as structured scene data instead of a one-off darkness tweak. Each current-world area now has a named profile object with stable review fields for area identity, target average luminance band, key-light guidance, fill guidance, ambient guidance, and interior/exterior classification.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsHd2dAreaLightingProfile.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_area_lighting_profile_cycle11.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

## Implementation Notes

`AnemoraFastVsHouseSliceSetup.CreateHouseSliceScene()` now creates four current-world profile marker objects:

- `FastVS_HD2D_HouseInteriorLightingProfile`
- `FastVS_HD2D_HouseExteriorLightingProfile`
- `FastVS_HD2D_CentralPlazaLightingProfile`
- `FastVS_HD2D_LibraryLightingProfile`

Each profile is parented under the matching current-area root, placed at the matching area center, and filled with a small `FastVsHd2dAreaLightingProfile` component so the lighting intent can be inspected and audited directly from the scene hierarchy.

The setup validation path now also calls `AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.VerifyAreaLightingProfilesV1()`, which checks the exact object names, parent/root placement, local positions, and review fields for all four profiles.

## Parent Review Correction

The first worker pass stored key-light direction as an ambiguous two-value angle pair and validated only the marker values themselves. Parent review changed this to `keyLightEulerDegrees` so the profile uses the same `Quaternion.Euler(x, y, z)` convention as `FastVsHouseLightingDirector`.

The audit now also applies each area through `FastVsHouseLightingDirector` and checks that the live directional light, warm fill, ambient color, ambient luminance, and main-light rotation match the profile component. This keeps the profile objects from drifting into review-only notes that disagree with the actual rendered lighting.

This cycle does not reapply `SurfaceRampLit` to the current-world house interior floor, wall, or furniture materials. Those remain on the safer material path established after Cycle 10.

## Validation Performed

- `git diff --check -- . ':(exclude)Assets/Scenes/Anemora_FastVS_HouseSlice.unity'`
- Unity batch house validation:
  - `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_area_lighting_profile_cycle11_validate_worker_20260522.log'`
- Unity batch visual snapshot audit:
  - `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_area_lighting_profile_cycle11_capture_worker_20260522.log'`

Parent validation repeated these checks with:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_area_lighting_profile_cycle11_validate_parent_20260522.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_area_lighting_profile_cycle11_capture_parent_20260522.log`

## Cleanup

- Keep the new area lighting profile audit, the new profile component, this devlog, and the index update.
- Revert scene YAML churn, Addressables link churn, ProjectSettings churn, unrelated material/meta churn, and unrelated screenshot changes if they appear in the worktree after batchmode.

## Risks

- The audit is intentionally strict about object names, parent roots, and profile field values, so any deliberate tuning of these profiles will need a corresponding audit update.
- The new profile objects do not yet drive runtime lighting, but the audit now verifies they stay synchronized with the runtime lighting manager values.

## Next Steps

- Tune the four profile records in later cycles as the HD-2D lighting pass expands.
- Use the audit output as the baseline gate for any future area-specific lighting changes.
