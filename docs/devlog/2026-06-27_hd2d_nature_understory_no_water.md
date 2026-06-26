# HD2D nature understory no-water retone

Date: 2026-06-27
Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`

## Summary

- Continued the outdoor nature uplift after the species silhouette pass.
- Removed the city "water-like" read by replacing Central Plaza edge voids with dry ground/grass infill and retinting the current-side fog/camera background away from blue-gray.
- Retoned water-named fallback materials and breakup strips to dry stone/earth colors so residual broad surfaces no longer read as water in street/plaza views.
- Kept the renderer feature set frozen. No URP renderer feature was added, removed, or reordered.

## Authored Changes

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Added `CreateChapter1DryMapEdgeInfillForOutdoorMaps` and deterministic dry map-edge infill for outdoor maps.
  - Added Central Plaza dry basin/grass bank covers plus foreground void seals tuned from the wide camera projection.
  - Retoned current outdoor void/fog/camera background, scenic backdrop, and outdoor sky wash colors from blue-gray to olive/earth.
  - Retoned broad-water and waterline-breakup material generation to matte dry stone/soil colors.
  - Preserved deterministic placement and variation without `Random`, `Time`, or `DateTime`.

## Review Evidence

- Accepted review packet: `docs/review/2026-06-27T04-36_nature_understory_no_water_r9/`
- Contact sheet: `docs/review/2026-06-27T04-36_nature_understory_no_water_r9/00_contact_sheet.png`
- Latest build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Full build path: `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Visual Triage

- r1-r8 were diagnostic iterations while isolating the remaining lower-screen blue-gray read.
- The accepted r9 packet removes the Central Plaza current-side right/lower blue patch and leaves the remaining edge fills as green/earth rather than water.
- Mechanical lower-screen blue-like pixel audit on r9:
  - `03_b1_b3_current.png`: 0.0000%
  - `05_c1_c3_current.png`: 0.0010%
  - `01_a1_a2_current.png`: 0.0003%
  - `07_d1_d3_current.png`: 0.0000%
  - `11_f1_f6_current.png`: 0.0000%
- Remaining visual risk: this pass is a no-water/retone cleanup, not a final botanical asset upgrade. The next nature pass should keep pushing foreground plant forms and terrain/nature integration.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed
  - Log: `Logs/nature_understory_no_water_validate_r9.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed
  - Accepted output: `docs/review/2026-06-27T04-36_nature_understory_no_water_r9/`
- EditMode renderer freeze: passed
  - Result XML: `Logs/nature_understory_no_water_editmode_r1.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed
  - Non-passed tests: 0
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed
  - Log: `Logs/nature_understory_no_water_asset_validation_r1.log`
  - Result: `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed
  - Log: `Logs/nature_understory_no_water_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Built player smoke: passed
  - Log: `Logs/nature_understory_no_water_player_smoke_r1.log`
  - Pass marker: `ANEMORA_HOUSE_SLICE_SMOKE_PASS`

## Notes

- Review images are R2/viewer artifacts and are not staged into git.
- The r9 packet is the accepted review target. Earlier r1-r8 packets are diagnostic only.
- Unity batch side effects were restored after validation: `link.xml`, generated material/texture assets, `DefaultVolumeProfile.asset`, `UniversalRenderPipeline.asset`, and raw `docs/devlog/screenshots/chapter1_all_maps_cycle05/` overwrites were not staged.
