# HD2D terrain detail vista

Date: 2026-06-26
Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`

## Summary

- Continued the distant panorama quality pass after specimen canopy detail.
- Rejected r2/r4 because the new terrain-detail meshes were inside the camera bounds but barely moved the review pixels.
- r3 failed the intended distant-ring radius validation after being moved too close; the accepted r6 keeps the radius contract and instead improves the visible RealisticDepth, ScenicRelief, and ProductionDepth layers.
- Added deterministic terrain-detail forest/slope/ridge meshes to every outdoor current/past map.
- Raised the distant vista texture density and material separation so forest, slope, and rock surfaces read as authored distant terrain rather than broad flat fills.
- Kept the renderer feature set frozen. No URP renderer feature was added, removed, or reordered.

## Authored Changes

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Added `CreateDistantPanoramaVistaTerrainDetails` with deterministic ForestMosaic, SlopeEtching, and RidgeIncision layers.
  - Added textured `Ch1Distant_*TerrainDetail*` materials with current/past color separation.
  - Tightened terrain-detail validation for mesh count, textured materials, and wide-camera visibility.
  - Increased the visible distant vista texture density for the existing RealisticDepth, ScenicRelief, ProductionDepth, and base panorama band materials.
  - Preserved deterministic placement from area/time/layer/span seeds without `Random`, `Time`, or `DateTime`.

## Review Evidence

- Review packet: `docs/review/2026-06-26T03-59_terrain_detail_vista_r6/`
- Contact sheet: `docs/review/2026-06-26T03-59_terrain_detail_vista_r6/00_contact_sheet.png`
- Latest build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Full build path: `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Visual Triage

- r2/r4 were rejected: the new terrain-detail meshes passed visibility checks but remained too subtle in real pixels.
- r3 was rejected by validation: the forest layer was moved inside the allowed distant-ring radius and failed with `radius 53.378 is outside the intended ring`.
- r6 accepted direction: keep all distant detail within the 62m+ ring contract, and improve the actually visible RealisticDepth, ScenicRelief, ProductionDepth, and base panorama material layers.
- Shotdiff versus `docs/review/2026-06-25T01-23_specimen_canopy_detail_r2/` at the normal 0.5% review threshold changed 8 individual map PNGs:
  - `05_c1_c3_current.png`: 0.8324%
  - `06_c1_c3_past.png`: 0.8366%
  - `07_d1_d3_current.png`: 1.1201%
  - `08_d1_d3_past.png`: 0.9067%
  - `09_e1_e3_current.png`: 1.3641%
  - `10_e1_e3_past.png`: 0.9698%
  - `11_f1_f6_current.png`: 1.9877%
  - `12_f1_f6_past.png`: 1.7529%
- Visual read: distant surfaces now carry finer forest/rock/terrain texture and stronger material separation, especially in Mia/Aria/Kaia/Ruins wide shots, without adding fog or renderer features.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed
  - Log: `Logs/terrain_detail_validate_r6.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed
  - Log: `Logs/terrain_detail_capture_r6.log`
  - Output: `docs/devlog/screenshots/chapter1_all_maps_cycle05/`
- EditMode renderer freeze: passed
  - Result XML: `Logs/terrain_detail_editmode_r6.xml`
  - Result: 36 total / 36 passed / 0 failed
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed
  - Log: `Logs/terrain_detail_assetvalidation_r6.log`
  - Result: `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed
  - Log: `Logs/terrain_detail_build_r6.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Built player smoke: passed
  - Log: `Logs/terrain_detail_player_smoke_r6.log`
  - Pass marker: `ANEMORA_HOUSE_SLICE_SMOKE_PASS`

## Notes

- The accepted packet is r6. r1/r2/r4 were diagnostic only and should not be uploaded as review targets.
- Review images are R2/viewer artifacts and are not staged into git.
- Unity batch side effects must be restored before staging: `link.xml`, generated material/texture assets, `DefaultVolumeProfile.asset`, and raw `docs/devlog/screenshots/chapter1_all_maps_cycle05/` overwrites remain working evidence only.
