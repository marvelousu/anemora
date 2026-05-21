# 2026-05-21 Fast VS HD2D Outdoor Horizon Depth Cleanup Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_horizon_depth_cleanup_20260521`

This cycle adds a restrained outdoor horizon cleanup pass for the house exterior and central plaza maps. The previous large sky/background attempt had been rejected as too rough, so this pass intentionally avoids a dominant sky card and only adds thin, non-colliding distant tree/roof silhouette bands to reduce the empty background feeling.

No API token, paid asset purchase, or external download was used.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Wired `CreateOutdoorHorizonDepthCleanupPolish(...)` into both house exterior and central plaza generation.
- Added four house exterior non-colliding horizon objects:
  - `Current_HouseExterior_HorizonDepthCleanup_LowTreeLineA`
  - `Current_HouseExterior_HorizonDepthCleanup_FarRoofStepA`
  - `Past_HouseExterior_HorizonDepthCleanup_LowTreeLineA`
  - `Past_HouseExterior_HorizonDepthCleanup_FarRoofStepA`
- Added four central plaza non-colliding horizon objects:
  - `Current_CentralPlaza_HorizonDepthCleanup_LowTreeLineA`
  - `Current_CentralPlaza_HorizonDepthCleanup_FarRoofBreakA`
  - `Past_CentralPlaza_HorizonDepthCleanup_LowTreeLineA`
  - `Past_CentralPlaza_HorizonDepthCleanup_FarRoofBreakA`
- Added `ValidateFastVsHd2dEightySixthCycleOutdoorHorizonDepthCleanup()`.
- Added `CaptureHd2dEightySixthCycleScreenshotsBatch()` and `CaptureHd2dEightySixthCycleScreenshotsToDirectory(...)`.
- Adjusted the house exterior screenshot review position to the northeast road side so the image is not blocked by the house eave.

## Added Follow-Up Tasks

User requested the following items during this cycle, and they are carried into the next HD-2D task planning:

- Create a more intentional outdoor sky/background treatment instead of leaving the outdoor view as only black void plus tiny silhouettes.
- Make the central plaza library exterior feel less like a flat facade by extending its volume backward within the current plaza map bounds.

## Validation

Worker handoff:

- Worker `019e48ee-30b5-755e-b221-191e13ec8906` was assigned the task, but produced no file diff or Unity logs before shutdown.
- Parent session implemented the cycle directly and completed validation, capture, build, smoke, and repository hygiene.

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle86_outdoor_horizon_depth_cleanup_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture logs:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle86_outdoor_horizon_depth_cleanup_parent_capture_20260521.log`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle86_outdoor_horizon_depth_cleanup_parent_capture_rerun_20260521.log`
- Result: passed with `Fast VS eighty-sixth-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_horizon_depth_cleanup_20260521`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle86_outdoor_horizon_depth_cleanup_parent_build_20260521.log`
- Result: passed with `Fast VS house slice validation passed.` and `Build Finished, Result: Success.`
- Note: Unity emitted unrelated startup/license/import noise, but the batch completed successfully.

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle86_outdoor_horizon_depth_cleanup_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_horizon_depth_cleanup_20260521\01_current_house_exterior_horizon_depth_cleanup.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_horizon_depth_cleanup_20260521\02_past_house_exterior_horizon_depth_cleanup.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_horizon_depth_cleanup_20260521\03_current_central_plaza_horizon_depth_cleanup.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_horizon_depth_cleanup_20260521\04_past_central_plaza_horizon_depth_cleanup.png`

## Notes

- This pass is intentionally subtle and should not be treated as the final sky/background solution.
- All new horizon details are `PropOrFeature` landmarks, non-arrival, non-colliding, and shadow-disabled.
- The central plaza library is still visually too facade-like; the next cycle should prioritize library exterior rear volume and side depth inside the current map bounds.
