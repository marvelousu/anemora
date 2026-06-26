# HD2D nature species silhouette

Date: 2026-06-26
Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`

## Summary

- Continued the nature graphics uplift after the SoftGrass/canopy split pass.
- Rejected r1 as a plateau: the new species accents existed but only moved roughly 0.04-0.15% of wide pixels, too subtle for the stated goal.
- Accepted r2 after scaling the deterministic species accents up so tree groves and specimen trees gain clearer conifer, broadleaf, and slender-crown silhouettes.
- Kept the renderer feature set frozen. No URP renderer feature was added, removed, or reordered.

## Authored Changes

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Added `CreateRealisticTreeSpeciesSilhouetteAccents`.
  - Called the species accent pass from both realistic tree grove detail and realistic specimen tree canopy detail.
  - Added deterministic conifer tier, broadleaf crown lobe, and slender upper-trunk/leaf-skirt variants chosen from area/tree seeds.
  - Rejected the first small-scale pass and increased the accent scale for the accepted r2 so the change survives the wide camera.
  - Preserved deterministic placement and variation without `Random`, `Time`, or `DateTime`.

## Review Evidence

- Review packet: `docs/review/2026-06-26T16-43_nature_species_silhouette_r2/`
- Contact sheet: `docs/review/2026-06-26T16-43_nature_species_silhouette_r2/00_contact_sheet.png`
- Latest build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Full build path: `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Visual Triage

- r1 rejected: shotdiff versus `docs/review/2026-06-26T15-28_nature_softgrass_r1/` showed no changed map at the normal review threshold; contact sheet diff was only 0.0415%.
- r2 accepted direction: species accents are still restrained, but foreground and midground crowns gain more readable conifer/broadleaf/slender silhouettes without new black matte artifacts.
- Shotdiff versus `docs/review/2026-06-26T15-28_nature_softgrass_r1/`:
  - `01_a1_a2_current.png`: 0.5317%
  - `02_a1_a2_past.png`: 0.2700%
  - `03_b1_b3_current.png`: 0.4501%
  - `04_b1_b3_past.png`: 0.4051%
  - `05_c1_c3_current.png`: 0.4104%
  - `06_c1_c3_past.png`: 0.2981%
  - `07_d1_d3_current.png`: 0.2326%
  - `08_d1_d3_past.png`: 0.2321%
  - `09_e1_e3_current.png`: 0.2741%
  - `10_e1_e3_past.png`: 0.1616%
  - `11_f1_f6_current.png`: 0.1437%
  - `12_f1_f6_past.png`: 0.1176%
  - `13_scene6_sideview_auto.png`: 0.0000%
- Remaining visual risk: this pass improves silhouette variety but remains an authored low-poly layer. The next substantial nature pass should add better foreground plant clumps and terrain/nature integration rather than only enlarging crown accents.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed
  - Log: `Logs/nature_species_silhouette_validate_r2.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed
  - Log: `Logs/nature_species_silhouette_capture_r2.log`
  - Output: `docs/review/2026-06-26T16-43_nature_species_silhouette_r2/`
- EditMode renderer freeze: passed
  - Result XML: `Logs/nature_species_silhouette_editmode_r2.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed
  - Non-passed tests: 0
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed
  - Log: `Logs/nature_species_silhouette_assetvalidation_r2.log`
  - Result: `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed
  - Log: `Logs/nature_species_silhouette_build_r2.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Built player smoke: passed
  - Log: `Logs/nature_species_silhouette_player_smoke_r2.log`
  - Pass marker: `ANEMORA_HOUSE_SLICE_SMOKE_PASS`

## Notes

- The accepted packet is r2. r1 was diagnostic only and should not be uploaded as a review target.
- Review images are R2/viewer artifacts and are not staged into git.
- Unity batch side effects must be restored before staging: `link.xml`, generated material/texture assets, `DefaultVolumeProfile.asset`, and raw `docs/devlog/screenshots/chapter1_all_maps_cycle05/` overwrites remain working evidence only.
