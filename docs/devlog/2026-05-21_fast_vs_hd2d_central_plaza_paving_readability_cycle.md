# 2026-05-21 Fast VS HD2D Central Plaza Paving Readability Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_central_plaza_paving_readability_20260521`

This cycle adds a narrow HD-2D paving readability pass to the central plaza floor. The goal is to make the square, library approach, fountain contact area, and notice-board base read with a little more edge definition and micro-detail in screenshots without touching map sizes, spawn points, transitions, Time Window behavior, story/dialogue/UI/fonts, characters, Reto/Aria/book events, map-move glow pads, colliders, or mandatory object names.

No external, API, Meshy, or paid assets were used. The pass uses existing Fast VS materials plus code-generated non-arrival landmark cubes only.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateCentralPlazaPavingReadabilityPolish(...)` and wired it into `CreateCentralPlaza(...)` immediately after `CreateCentralPlazaFloorJointAccents(...)` and before the fountain objects.
- Added 10 new plaza paving readability objects:
  - `Current_CentralPlaza_PavingReadability_LibraryApproachEdgeShadowA`
  - `Current_CentralPlaza_PavingReadability_SquareCornerChipA`
  - `Current_CentralPlaza_PavingReadability_FountainContactDustOrPetalA`
  - `Current_CentralPlaza_PavingReadability_NoticeBoardBaseContactA`
  - `Current_CentralPlaza_PavingReadability_CentralSeamAccentA`
  - `Past_CentralPlaza_PavingReadability_LibraryApproachEdgeShadowA`
  - `Past_CentralPlaza_PavingReadability_SquareCornerChipA`
  - `Past_CentralPlaza_PavingReadability_FountainContactDustOrPetalA`
  - `Past_CentralPlaza_PavingReadability_NoticeBoardBaseContactA`
  - `Past_CentralPlaza_PavingReadability_CentralSeamAccentA`
- Kept the additions flat and thin, non-colliding, tagged `TimeWindowPairedSpaceLandmarkKind.PropOrFeature`, and marked `countsForArrival = false` via `CreateNonArrivalLandmarkCube(...)`.
- Added `ValidateFastVsHd2dSixtyFifthCycleCentralPlazaPavingReadability()` and `ValidateCentralPlazaPavingReadabilityObject(...)`.
- The validation checks each object exists, stays under the correct current/past plaza map root, retains a renderer/material, has no collider, keeps `PropOrFeature`, remains non-arrival, stays thin on Y, and stays near the expected placement.
- Parent review changed the library-approach edge object from the shared shadow material to the current/past stone material because the first capture read as a black bar rather than a paving accent.
- The validation also confirms the critical route glow pads are still present:
  - `Current_HouseExterior_ToPlaza_MapMoveGlowPad`
  - `Past_HouseExterior_ToPlaza_MapMoveGlowPad`
  - `Current_CentralPlaza_ToHouseExterior_MapMoveGlowPad`
  - `Past_CentralPlaza_ToHouseExterior_MapMoveGlowPad`
  - `Current_CentralPlaza_ToLibrary_MapMoveGlowPad`
  - `Past_CentralPlaza_ToLibrary_MapMoveGlowPad`
- Added `CaptureHd2dSixtyFifthCycleScreenshotsBatch()` and `CaptureHd2dSixtyFifthCycleScreenshotsToDirectory(...)`.

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`:

- Bumped the status/version line to `v6.29`.
- Increased root-level markdown coverage by 1.
- Increased dated devlog coverage by 1.
- Increased screenshot evidence coverage by 4.
- Increased the `2026-05-21` date count by 1.
- Added this cycle to the `2026-05-21` table.

## Validation

Worker validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle65_central_plaza_paving_readability_worker_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Worker screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle65_central_plaza_paving_readability_worker_capture_20260521.log`
- Result: passed and wrote the requested screenshots.

Parent review:

- Reviewed the four worker screenshots. The overview frames were usable, but `03_current_plaza_library_approach_paving_close.png` exposed the library-approach edge strip as too dark and bar-like.
- Adjusted only the material choice for `Current_CentralPlaza_PavingReadability_LibraryApproachEdgeShadowA` and `Past_CentralPlaza_PavingReadability_LibraryApproachEdgeShadowA` to use the corresponding stone materials.
- No gameplay, story, Time Window, UI/font, character, map-transition, collider, or map-size behavior was changed during the parent fix.

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle65_central_plaza_paving_readability_parent_capture_r1_20260521.log`
- Result: passed and regenerated the four screenshots listed below.

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle65_central_plaza_paving_readability_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle65_central_plaza_paving_readability_parent_build_20260521.log`
- Result: passed with `Fast VS house slice validation passed.` and rebuilt `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`.

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle65_central_plaza_paving_readability_parent_smoke_20260521.log`
- Result: passed a 20 second batchmode startup smoke run. The process was intentionally stopped after the smoke window.

Unity licensing note:

- The logs still include `[Licensing::Module] Error: Access token is unavailable; failed to update`. That is Unity licensing noise and did not block validation, screenshot capture, build, or smoke. No Anemora API token was used.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_central_plaza_paving_readability_20260521\01_current_plaza_paving_readability_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_central_plaza_paving_readability_20260521\02_past_plaza_paving_readability_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_central_plaza_paving_readability_20260521\03_current_plaza_library_approach_paving_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_central_plaza_paving_readability_20260521\04_past_plaza_fountain_notice_paving_close.png`

## External Assets

No external, API, or paid assets were used.

## Residual Risk

- The paving accents are intentionally subtle, so the gain is incremental rather than a full plaza floor rewrite.
- The close review frames still depend on the existing camera composition. They now show the plaza floor detail more clearly, but a later pass could tune those review angles further if needed.
