# 2026-05-21 Fast VS HD2D Outdoor Sky Wash Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_wash_20260521`

This cycle adds the requested sky/background task as a restrained HD-2D outdoor sky wash for the house exterior and central plaza maps. It deliberately avoids reintroducing the earlier rough sky/background direction that was reverted on 2026-05-18.

The goal is not a finished painted sky. The goal is to reduce the harsh black void behind the outdoor maps with a low-alpha atmospheric layer that stays behind the map, does not read as a flat blue poster, and does not compete with route glows, the house, or the plaza library facade.

No Meshy/API token, paid asset, downloaded sky texture, dialogue, story flow, Time Window behavior, map transition, route marker, collider, or character asset was changed.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateOutdoorSkyWashTreatment(...)`.
- Added `CreateOutdoorSkyWashQuad(...)`.
- Added `EnsureHd2dOutdoorSkyWashMaterial(...)`.
- Added `EnsureHd2dOutdoorSkyWashTexture(...)`.
- Wired the sky wash into `CreateExterior(...)` and `CreateCentralPlaza(...)` immediately after the existing outdoor void background treatment.
- Added `ValidateFastVsHd2dSeventySecondCycleOutdoorSkyWash()` and `ValidateHd2dOutdoorSkyWashObject(...)`.
- Added `CaptureHd2dSeventySecondCycleScreenshotsBatch()` and `CaptureHd2dSeventySecondCycleScreenshotsToDirectory(...)`.

New visual-only objects:

- `Current_HouseExterior_OutdoorSkyWash_BackPanel`
- `Current_HouseExterior_OutdoorSkyWash_HorizonBand`
- `Past_HouseExterior_OutdoorSkyWash_BackPanel`
- `Past_HouseExterior_OutdoorSkyWash_HorizonBand`
- `Current_CentralPlaza_OutdoorSkyWash_BackPanel`
- `Current_CentralPlaza_OutdoorSkyWash_HorizonBand`
- `Past_CentralPlaza_OutdoorSkyWash_BackPanel`
- `Past_CentralPlaza_OutdoorSkyWash_HorizonBand`

Generated sky-wash materials:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_sky_wash_current_central_plaza.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_sky_wash_past_central_plaza.mat`

Generated sky-wash textures:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_sky_wash_current_central_plaza.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_sky_wash_past_central_plaza.asset`

## Validation

Worker validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle72_outdoor_sky_wash_worker_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Worker screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle72_outdoor_sky_wash_worker_capture_20260521.log`
- Result: passed with `Fast VS seventy-second-cycle screenshots captured`.

Parent image sanity check:

- The four screenshot files exist and are non-trivial PNGs.
- Sampled upper-frame luminance is no longer near-black in the reviewed outdoor shots, while staying muted enough to avoid a poster-like sky block.

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle72_outdoor_sky_wash_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle72_outdoor_sky_wash_parent_capture_20260521.log`
- Result: passed with `Fast VS seventy-second-cycle screenshots captured`.

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle72_outdoor_sky_wash_parent_build_20260521.log`
- Result: passed with `Build Finished, Result: Success.`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle72_outdoor_sky_wash_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

Unity licensing note:

- The logs include the usual licensing noise during batchmode startup, but validation and screenshot capture completed successfully. No Anemora API token was used.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_wash_20260521\01_current_house_exterior_sky_wash_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_wash_20260521\02_past_house_exterior_sky_wash_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_wash_20260521\03_current_central_plaza_sky_wash_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_wash_20260521\04_past_central_plaza_sky_wash_overview.png`

## Residual Risk

- This is intentionally a modest background wash, not final sky art.
- The result should be reviewed in motion later, because a low-alpha background can read differently under live camera movement than in still screenshots.
- If this direction is accepted, a later pass can add carefully authored distant silhouettes or a real painted background. That should remain a separate reviewable cycle because the previous full sky attempt was rejected.
