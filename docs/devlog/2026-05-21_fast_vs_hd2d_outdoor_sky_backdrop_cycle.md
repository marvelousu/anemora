# 2026-05-21 Fast VS HD2D Outdoor Sky Backdrop Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit targets:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsHouseAreaVisibility.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_backdrop_20260521`

This cycle records the user's added task for outdoor background/sky work. The first attempted world-space painted backdrop panel was rejected during parent review because it created a black rectangular band in the plaza upper-library shot. The committed approach is deliberately more conservative: outdoor maps now switch the camera clear color to a muted blue-gray sky tone, while indoor/library maps keep the darker clear color.

No API token, paid asset purchase, Meshy request, or external download was used.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsHouseAreaVisibility.cs`:

- Added serialized clear-color values:
  - Indoor/library: `0.075, 0.078, 0.084, 1`
  - House exterior / central plaza: `0.125, 0.148, 0.170, 1`
- Added `ApplyCameraClearColor()` and wired it into `ApplyVisibility()` so area transitions update the visible background color at runtime and in review captures.

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `ValidateFastVsHd2dEightyEighthCycleOutdoorSkyBackdrop()`.
- Added `ValidateOutdoorSkyClearColorForReview()` to assert that house exterior and central plaza use the sky clear color, while interior returns to the dark indoor clear color.
- Added `CaptureHd2dEightyEighthCycleScreenshotsBatch()` and `CaptureHd2dEightyEighthCycleScreenshotsToDirectory(...)`.
- Kept the plaza screenshots on a high exterior-library angle so the sky/background and backward-volume roofline are both visible for review.

## Review Notes

Worker diagnosis:

- Worker `019e492a-d336-72f3-ba7f-04f81afff44e` inspected the initial failed painted-backdrop attempt.
- Diagnosis: generated backdrop objects existed in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`, but the review camera/frustum and transparent depth stack made them ineffective in the saved PNGs.

Parent review:

- The first parent fix moved the backdrop panels and changed their material behavior, but review screenshots still showed a visible black band above the plaza library.
- Parent session removed that panel route before commit and kept only the stable area clear-color behavior.

## Validation

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle88_outdoor_sky_backdrop_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle88_outdoor_sky_backdrop_parent_capture_20260521.log`
- Result: passed with `Fast VS eighty-eighth-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_backdrop_20260521`
- Hash check confirmed the current house exterior and current central plaza screenshots changed from the prior horizon-depth cleanup captures.

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle88_outdoor_sky_backdrop_parent_build_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`, `Build Finished, Result: Success.`, and `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle88_outdoor_sky_backdrop_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_backdrop_20260521\01_current_house_exterior_sky_backdrop.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_backdrop_20260521\02_past_house_exterior_sky_backdrop.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_backdrop_20260521\03_current_central_plaza_sky_backdrop.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_sky_backdrop_20260521\04_past_central_plaza_sky_backdrop.png`

## Notes

- This pass intentionally does not change map bounds, map transition pads, Time Window behavior, character sprites, story flags, colliders, or UI text.
- The user also asked to keep the plaza library exterior from reading as a flat facade. That work is already represented by the preceding backward-volume cycle in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-21_fast_vs_hd2d_plaza_library_backward_volume_cycle.md`; this cycle keeps that result and adds the outdoor sky/background task to the active HD-2D task stream.
