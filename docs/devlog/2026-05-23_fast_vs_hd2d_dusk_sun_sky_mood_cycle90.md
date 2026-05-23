# feat(hd2d): add dusk sun sky mood

## Scope

Cycle 90 follows the Cycle89 outdoor backdrop foreground cleanup. Parent review of the Cycle89 captures still showed a flat blue-gray void and dark horizontal bars around the house exterior, while the user proposed a darker overall outdoor mood with a visible sun and a slightly faded camera-like feel. This cycle adds a scene-side dusk/sun overlay so the outdoor maps have an immediate visual direction before another global grade pass, and it persists the shading-foundation profile correction required by validation.

Authored file:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Expected generated side-effect assets:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_dusk_sky_mood_cycle90.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_dusk_sky_mood_cycle90.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_visible_sun_disc_cycle90.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_visible_sun_disc_cycle90.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\DefaultVolumeProfile.asset`

SCOPED_PROMPT_ISSUED cycle=90 authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle90ScreenshotsBatch`

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_dusk_sky_mood_cycle90.mat
  - C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_dusk_sky_mood_cycle90.asset
  - C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_visible_sun_disc_cycle90.mat
  - C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_visible_sun_disc_cycle90.asset
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle90ScreenshotsBatch
notes: Kept the change inside the authored editor file and the allowed generated asset paths only, with no VolumeProfile or unrelated scene files touched.
```

## Implementation Plan

- Add transparent quad-based dusk sky mood overlays for house exterior and central plaza, current and past.
- Add a stronger visible sun disc/halo than the existing Cycle84 sun, positioned where the parent-review captures can see it.
- Keep all new sky/sun objects non-colliding, non-arrival, and shadow-safe.
- Add Cycle90 validation and parent-review captures without changing route logic, Time Window behavior, UI, dialogue, font setup, or global post-processing.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle90ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle90_dusk_sun_mood_parent_review_20260523_01\parent_review_01_current_house_exterior_dusk_sun_mood_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle90_dusk_sun_mood_parent_review_20260523_01\parent_review_02_current_central_plaza_dusk_sun_mood_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle90_dusk_sun_mood_parent_review_20260523_01\parent_review_03_past_house_exterior_dusk_sun_mood_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle90_dusk_sun_mood_parent_review_20260523_01\parent_review_04_past_central_plaza_dusk_sun_mood_overview.png`

## Review Notes

This is not the final camera-grade pass. Parent visual review should check whether the house/plaza outdoor frames now have a readable sun and mood foundation, and whether any black construction-like bars still dominate the shot.

## Parent Retry Note

The first validate attempt exposed a persisted profile drift: `DepthOfField` and `FilmGrain` were enabled in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\DefaultVolumeProfile.asset`. The parent reapplied the shading foundation baseline and includes that profile correction in this cycle so the audit remains stable.
