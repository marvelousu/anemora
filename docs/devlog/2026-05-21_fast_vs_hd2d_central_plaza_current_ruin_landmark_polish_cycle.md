# 2026-05-21 Fast VS HD2D Central Plaza Current Ruin Landmark Polish Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_central_plaza_current_ruin_landmark_polish_20260521`

This cycle tightens the current-side central plaza ruin reads around the dry fountain, the notice board, and the front low structure silhouettes. It keeps the past-side fountain presentation intact, and it does not change dialogue, story flow, Time Window behavior, fonts, character behavior, map transitions, input handling, event flags, or collision and route logic.

No external, paid, or downloaded assets were used. The pass relies on existing materials and code-generated low-profile detail cubes only.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateCentralPlazaCurrentRuinLandmarkPolish(...)` plus a small helper for current-only non-arrival landmark cubes.
- Wired the new polish pass into `CreateCentralPlaza(...)` after the fountain, sign, and edge dressing work.
- Added current-side-only ruin polish details around the central plaza fountain basin, the notice board footing, and the front low broken market / structure read:
  - `Current_CentralPlaza_RuinLandmarkPolish_FountainRimChipA`
  - `Current_CentralPlaza_RuinLandmarkPolish_FountainBasinDustA`
  - `Current_CentralPlaza_RuinLandmarkPolish_FountainRimShadowA`
  - `Current_CentralPlaza_RuinLandmarkPolish_NoticeBoardFootShadowA`
  - `Current_CentralPlaza_RuinLandmarkPolish_NoticeBoardAnchorChipA`
  - `Current_CentralPlaza_RuinLandmarkPolish_BrokenMarketEdgeShadowA`
  - `Current_CentralPlaza_RuinLandmarkPolish_BrokenMarketEdgeChipA`
  - `Current_CentralPlaza_RuinLandmarkPolish_FrontStoneWashA`
- Added `ValidateFastVsHd2dFiftyNinthCycleCentralPlazaCurrentRuinLandmarkPolish()` and `ValidateCentralPlazaRuinLandmarkPolishObject(...)`, then wired the validation into `ValidateHouseSliceBatch()`.
- Added `CaptureHd2dFiftyNinthCycleScreenshotsBatch()` and `CaptureHd2dFiftyNinthCycleScreenshotsToDirectory(...)`.
- Kept the past-side central plaza mostly unchanged, using it only as a comparison reference in the screenshot batch.
- Parent review corrected the first pass after screenshot inspection: the current-side dry fountain crack and dry basin crack now use `dust` instead of the former very dark material, and the new ruin polish shadow plates use `dust` / `current_rubble_detail` instead of opaque black `shadow` where they read as black foreign shapes.
- Parent review also reduced `Current_CentralPlaza_RuinLandmarkPolish_BrokenMarketEdgeShadowA` so the front ruin cluster reads as debris/grounding, not a large black slab.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-21_fast_vs_hd2d_central_plaza_current_ruin_landmark_polish_cycle.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_central_plaza_current_ruin_landmark_polish_20260521\`

## Validation

Validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle59_central_plaza_ruin_landmark_worker_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle59_central_plaza_ruin_landmark_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle59_central_plaza_ruin_landmark_parent_validate_r3_20260521.log`
- Result: passed with `Fast VS house slice validation passed.` after parent material correction.

Capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle59_central_plaza_ruin_landmark_worker_capture_20260521.log`
- Result: captured the 4 requested screenshots.
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle59_central_plaza_ruin_landmark_parent_capture_r2_20260521.log`
- Result: captured the 4 screenshots after parent material correction. Parent inspection confirmed the former black slab read in `02_current_central_plaza_fountain_ruin_close.png` was reduced.

Build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle59_central_plaza_ruin_landmark_parent_build_20260521.log`
- Result: build succeeded and produced `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`.

Smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle59_central_plaza_ruin_landmark_parent_smoke_20260521.log`
- Result: 20 second headless player smoke was stopped intentionally after launch; error-pattern match count was `0`.

Unity licensing note:

- Parent validation/capture/build logs still include Unity's `[Licensing::Module] Error: Access token is unavailable; failed to update` warning. It did not fail validation, capture, build, or smoke, and it is not an Anemora API-token requirement.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_central_plaza_current_ruin_landmark_polish_20260521\01_current_central_plaza_ruin_landmark_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_central_plaza_current_ruin_landmark_polish_20260521\02_current_central_plaza_fountain_ruin_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_central_plaza_current_ruin_landmark_polish_20260521\03_current_central_plaza_sign_grounding_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_central_plaza_current_ruin_landmark_polish_20260521\04_past_central_plaza_reference_overview.png`

## External Assets

No external or paid assets were used.

## Residual Risk

- The new current-side ruin polish is intentionally small and material-driven, so it still reads as cube-based dressing rather than a dedicated authored prop set.
- The past-side central plaza remains a comparison reference rather than a freshly reworked target, so the asymmetry between current and past is deliberate.
- The current-side plaza still depends heavily on code-generated tile/debris materials. External or paid assets were not necessary for this cycle, but a later prop/texture pass may benefit from vetted authored ruin-kit textures if the square needs a less procedural look.
