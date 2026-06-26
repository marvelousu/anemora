# HD2D nature softgrass canopy

Date: 2026-06-26
Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`

## Summary

- Continued the nature graphics uplift after authored imported-nature replacement.
- Targeted the remaining wide-camera problems in r17: noisy black leaf/grass speckles and still-rounded authored tree crowns.
- Accepted r1 after lowering imported nature leaf/grass dark-pixel density, thinning branch lace geometry, and splitting tree companion crowns into smaller core, top, shoulder, and outer leaf-spray pieces.
- Kept the renderer feature set frozen. No URP renderer feature was added, removed, or reordered.

## Authored Changes

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Added `PixelPattern.SoftGrass` for imported nature leaf/grass materials, reducing dark-pixel frequency and lowering texture tiling.
  - Adjusted imported nature current/past leaf and grass palettes toward lower-contrast natural greens.
  - Reduced global `CreateAuthoredVegetationBranchLaceMesh` branch count and segment thickness to make branch detail read as structure instead of black stipple.
  - Split authored tree companion crowns into smaller crown/core/top/shoulder/spray pieces, using canopy-breakup material on selected outer pieces.
  - Preserved deterministic placement and variation from object/area seeds without `Random`, `Time`, or `DateTime`.

## Review Evidence

- Review packet: `docs/review/2026-06-26T15-28_nature_softgrass_r1/`
- Contact sheet: `docs/review/2026-06-26T15-28_nature_softgrass_r1/00_contact_sheet.png`
- Latest build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Full build path: `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Visual Triage

- r1 accepted direction: compared with authored nature r17, the outdoor maps keep the same composition while leaf/grass surfaces show fewer black dots and authored tree crowns read as more layered.
- Shotdiff versus `docs/review/2026-06-26T13-35_nature_photo_ground_r17/` changed every outdoor map and left the sideview unchanged:
  - `01_a1_a2_current.png`: 1.2151%
  - `02_a1_a2_past.png`: 1.4461%
  - `03_b1_b3_current.png`: 0.9749%
  - `04_b1_b3_past.png`: 1.2071%
  - `05_c1_c3_current.png`: 0.8359%
  - `06_c1_c3_past.png`: 0.8192%
  - `07_d1_d3_current.png`: 0.6861%
  - `08_d1_d3_past.png`: 0.7854%
  - `09_e1_e3_current.png`: 0.7203%
  - `10_e1_e3_past.png`: 0.8294%
  - `11_f1_f6_current.png`: 0.5392%
  - `12_f1_f6_past.png`: 0.5115%
  - `13_scene6_sideview_auto.png`: 0.0000%
- Remaining visual risk: the nature is cleaner than r17 but still authored low-poly HD2D, not fully photoreal. The next larger pass should introduce more species-specific tree silhouettes and better foreground plant clustering instead of only color/texture polish.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed
  - Log: `Logs/nature_softgrass_validate_r1.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed
  - Log: `Logs/nature_softgrass_capture_r1.log`
  - Output: `docs/review/2026-06-26T15-28_nature_softgrass_r1/`
- EditMode renderer freeze: passed
  - Result XML: `Logs/nature_softgrass_editmode_r1.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed
  - Non-passed tests: 0
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed
  - Log: `Logs/nature_softgrass_assetvalidation_r1.log`
  - Result: `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed
  - Log: `Logs/nature_softgrass_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Built player smoke: passed
  - Log: `Logs/nature_softgrass_player_smoke_r1.log`
  - Pass marker: `ANEMORA_HOUSE_SLICE_SMOKE_PASS`

## Notes

- The accepted packet is r1.
- Review images are R2/viewer artifacts and are not staged into git.
- Unity batch side effects must be restored before staging: `link.xml`, generated material/texture assets, `DefaultVolumeProfile.asset`, and raw `docs/devlog/screenshots/chapter1_all_maps_cycle05/` overwrites remain working evidence only.
