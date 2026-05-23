# feat(hd2d): mask house sky bars

## Scope

Cycle 91 follows the Cycle90 dusk/sun mood pass. Parent review of the Cycle90 current house exterior capture still showed black horizontal construction-like bars and a hard blue-gray void behind the house, so this cycle adds a narrow house-exterior-only mask layer that reads as dusk sky instead of exposed staging geometry.

Authored file:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Expected generated side-effect assets:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_house_sky_bar_mask_cycle91.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_house_sky_bar_mask_cycle91.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\DefaultVolumeProfile.asset`

SCOPED_PROMPT_ISSUED cycle=91 authored_file=`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` validate=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` capture=`Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle91ScreenshotsBatch`

```text
WORKER_RESULT
authored_file: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs
side_effect_files:
  - C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_house_sky_bar_mask_cycle91.mat
  - C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_house_sky_bar_mask_cycle91.mat.meta
  - C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_house_sky_bar_mask_cycle91.asset
  - C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_house_sky_bar_mask_cycle91.asset.meta
validate_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch
capture_method: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle91ScreenshotsBatch
notes: Added the exterior-only Cycle91 sky-bar mask quads plus generated material/texture hooks and batch entry points; I avoided route logic, Time Window behavior, UI/dialogue/fonts, and global volume changes.
```

## Implementation Plan

- Add an exterior-only helper that creates three non-colliding sky-mask quads behind the house roofline.
- Generate a soft-edged dusk texture and transparent unlit material for the mask.
- Validate both current and past mask placement, material token, texture alpha range, and non-shadowing quad mesh state.
- Capture current house overview, current house lower/close review, and past house overview for parent visual inspection.

## Validation Plan

The parent runner will execute:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle91ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player launch/log scan

Expected screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle91_house_exterior_sky_bar_mask_parent_review_20260524_01\parent_review_01_current_house_exterior_sky_bar_mask_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle91_house_exterior_sky_bar_mask_parent_review_20260524_01\parent_review_02_current_house_exterior_sky_bar_mask_lower_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle91_house_exterior_sky_bar_mask_parent_review_20260524_01\parent_review_03_past_house_exterior_sky_bar_mask_overview.png`

## Review Notes

This cycle is a targeted cleanup, not the final sky solution. Parent review should reject the result if the mask reads as a flat card in front of the roof, covers the facade/door/Niro, or makes the house exterior look more artificial than Cycle90.

## Parent Retry Note

The first validate attempt showed the same persisted baseline drift seen during Cycle90: `DepthOfField` and `FilmGrain` were still enabled in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\DefaultVolumeProfile.asset`. The parent disabled those two components and includes the profile correction in this cycle.

The second validate attempt reached the Cycle91 texture audit and found center alpha at `0.573`, above the intended `0.52` ceiling. The parent reduced the generated mask alpha by 18% before retrying.
