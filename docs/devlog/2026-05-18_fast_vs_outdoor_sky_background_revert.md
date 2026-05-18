# 2026-05-18 Fast VS outdoor sky background revert

## Scope

- Project: `<repo>`
- Scene: `<repo>/Assets/Scenes/Anemora_FastVS_HouseSlice.unity`
- Build: `<repo>/Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`

## User Review Item

- The procedural outdoor sky/background pass was too rough and should be removed for now.

## Worker Cycle

- Plan: remove only the rejected outdoor background pass and avoid touching map layout, story, Time Window, or character logic.
- Worker instruction: gpt-5.4-mini worker `019e3a23-b90a-7e01-9f89-8c0d0b0c29a2` inspected the rollback targets without editing files.
- Worker result: remove `CreateOutdoorBackdrop(...)`, `CreateOutdoorCloudCluster(...)`, their exterior/plaza call sites, the outdoor backdrop validation helpers, the extra screenshot capture, the camera clear-color tweak, and the now-obsolete devlog/memory references.
- Integrator review: the final patch follows those targets and removes the generated outdoor sky/horizon/tree-line/cloud material and texture assets.

## Changes

- `<repo>/Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Removed procedural outdoor backdrop generation from house exterior and central plaza.
  - Removed `CreateOutdoorBackdrop(...)` and `CreateOutdoorCloudCluster(...)`.
  - Removed outdoor backdrop validation helpers and their validation hook.
  - Removed the extra `11_exterior_past_sky_background.png` review screenshot capture.
  - Reverted the camera clear color to the previous dark neutral value.
  - Removed the unused `PixelPattern.Sky` and `PixelPattern.Cloud` cases.
- Removed generated outdoor background assets:
  - `<repo>/Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_cloud_soft.mat`
  - `<repo>/Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_distant_tree_line.mat`
  - `<repo>/Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_outdoor_horizon.mat`
  - `<repo>/Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_outdoor_sky.mat`
  - `<repo>/Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_cloud_soft.mat`
  - `<repo>/Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_distant_tree_line.mat`
  - `<repo>/Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_outdoor_horizon.mat`
  - `<repo>/Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_outdoor_sky.mat`
  - `<repo>/Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_current_cloud_soft.asset`
  - `<repo>/Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_current_distant_tree_line.asset`
  - `<repo>/Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_current_outdoor_horizon.asset`
  - `<repo>/Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_current_outdoor_sky.asset`
  - `<repo>/Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_past_cloud_soft.asset`
  - `<repo>/Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_past_distant_tree_line.asset`
  - `<repo>/Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_past_outdoor_horizon.asset`
  - `<repo>/Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_past_outdoor_sky.asset`

## Verification

- Build and validation passed:
  - `<repo>/Logs/fast_vs_build_validate_20260518_outdoor_sky_background_revert.log`
- Review screenshots regenerated:
  - `<repo>/Logs/fast_vs_capture_review_20260518_outdoor_sky_background_revert.log`
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/02_exterior_niro_shadow.png`
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/07_plaza_library_facade_current.png`
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/08_plaza_library_facade_past.png`
- Windows EXE updated:
  - `<repo>/Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`

## Notes

- This does not attempt a replacement background. It restores the previous no-added-backdrop state so a cleaner background direction can be planned separately.
