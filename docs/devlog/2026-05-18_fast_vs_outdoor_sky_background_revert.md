# 2026-05-18 Fast VS outdoor sky background revert

## Scope

- Project: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample`
- Scene: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- Build: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## User Review Item

- The procedural outdoor sky/background pass was too rough and should be removed for now.

## Worker Cycle

- Plan: remove only the rejected outdoor background pass and avoid touching map layout, story, Time Window, or character logic.
- Worker instruction: gpt-5.4-mini worker `019e3a23-b90a-7e01-9f89-8c0d0b0c29a2` inspected the rollback targets without editing files.
- Worker result: remove `CreateOutdoorBackdrop(...)`, `CreateOutdoorCloudCluster(...)`, their exterior/plaza call sites, the outdoor backdrop validation helpers, the extra screenshot capture, the camera clear-color tweak, and the now-obsolete devlog/memory references.
- Integrator review: the final patch follows those targets and removes the generated outdoor sky/horizon/tree-line/cloud material and texture assets.

## Changes

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
  - Removed procedural outdoor backdrop generation from house exterior and central plaza.
  - Removed `CreateOutdoorBackdrop(...)` and `CreateOutdoorCloudCluster(...)`.
  - Removed outdoor backdrop validation helpers and their validation hook.
  - Removed the extra `11_exterior_past_sky_background.png` review screenshot capture.
  - Reverted the camera clear color to the previous dark neutral value.
  - Removed the unused `PixelPattern.Sky` and `PixelPattern.Cloud` cases.
- Removed generated outdoor background assets:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_cloud_soft.mat`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_distant_tree_line.mat`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_outdoor_horizon.mat`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_current_outdoor_sky.mat`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_past_cloud_soft.mat`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_past_distant_tree_line.mat`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_past_outdoor_horizon.mat`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_past_outdoor_sky.mat`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_current_cloud_soft.asset`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_current_distant_tree_line.asset`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_current_outdoor_horizon.asset`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_current_outdoor_sky.asset`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_past_cloud_soft.asset`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_past_distant_tree_line.asset`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_past_outdoor_horizon.asset`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_past_outdoor_sky.asset`

## Verification

- Build and validation passed:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Logs\fast_vs_build_validate_20260518_outdoor_sky_background_revert.log`
- Review screenshots regenerated:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Logs\fast_vs_capture_review_20260518_outdoor_sky_background_revert.log`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\devlog\screenshots\fast_vs_story_reto_shadow_20260518\02_exterior_niro_shadow.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\devlog\screenshots\fast_vs_story_reto_shadow_20260518\07_plaza_library_facade_current.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\devlog\screenshots\fast_vs_story_reto_shadow_20260518\08_plaza_library_facade_past.png`
- Windows EXE updated:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Notes

- This does not attempt a replacement background. It restores the previous no-added-backdrop state so a cleaner background direction can be planned separately.
