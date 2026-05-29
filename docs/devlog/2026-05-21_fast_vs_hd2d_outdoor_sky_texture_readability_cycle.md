# 2026-05-21 Fast VS HD2D Outdoor Sky Texture Readability Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Generated texture assets:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.asset`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.asset`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_sky_wash_current_central_plaza.asset`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_sky_wash_past_central_plaza.asset`
- Screenshot output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_texture_readability_20260521`

This cycle continues the user's added task for outdoor sky/background work. Cycle88 introduced a stable outdoor camera clear color. Cycle89 makes the existing `OutdoorSkyWash` texture assets visibly contribute a thin cloud/horizon layer without adding new world-space sky panels.

No Meshy/API token, paid asset purchase, external download, or new third-party asset was used.

## Worker Cycle

- Worker: `gpt-5.4-mini` session `019e492a-d336-72f3-ba7f-04f81afff44e`.
- Worker instruction: keep the change narrow, do not add opaque/large panels, do not touch story, Time Window, UI, movement, character, or map transition behavior.
- Worker output: initial procedural sky texture readability changes plus Cycle89 validation and screenshot helpers.

Parent review found two issues and fixed them before accepting the cycle:

- The first readability validation exposed that alpha coverage was effectively zero.
- Root cause: `Mathf.SmoothStep(0, 0.18, value)` had been used as a 0..1 threshold fade, but Unity returns a value between the first two arguments. Parent replaced this with `SmoothFade01(...)`.
- Parent switched the texture write to explicit `Color32[]` + `SetPixels32(...)` to keep alpha handling unambiguous.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CaptureHd2dEightyNinthCycleScreenshotsBatch()` and `CaptureHd2dEightyNinthCycleScreenshotsToDirectory(...)`.
- Added current/past sky readability screenshot helpers for house exterior and central plaza.
- Added `ValidateFastVsHd2dEightyNinthCycleOutdoorSkyTextureReadability()`.
- Added `ValidateHd2dOutdoorSkyWashTextureReadability(...)`.
- Added `TextureAlphaChannelIsEmpty(...)` so stale zero-alpha generated sky assets are recreated.
- Added `SmoothFade01(...)` for true 0..1 edge fades.
- Updated `EnsureHd2dOutdoorSkyWashTexture(...)` to generate a muted broad sky wash, horizon haze, and two thin cloud bands with bounded alpha.

## Validation

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle89_outdoor_sky_texture_parent_validate_20260521_final.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle89_outdoor_sky_texture_parent_capture_20260521.log`
- Result: passed with `Fast VS eighty-ninth-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_texture_readability_20260521`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle89_outdoor_sky_texture_parent_build_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`, `Build Finished, Result: Success.`, and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle89_outdoor_sky_texture_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_texture_readability_20260521\01_current_house_exterior_sky_texture_readability.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_texture_readability_20260521\02_past_house_exterior_sky_texture_readability.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_texture_readability_20260521\03_current_central_plaza_sky_texture_readability.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_texture_readability_20260521\04_past_central_plaza_sky_texture_readability.png`

## Notes

- The output is intentionally subtle. It avoids the earlier failed route where a world-space painted backdrop created a visible rectangular/black band.
- This pass does not change map bounds, player movement, Time Window behavior, story state, dialogue, colliders, or transition pads.
- The plaza library exterior depth task remains represented by `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-21_fast_vs_hd2d_plaza_library_backward_volume_cycle.md`.
