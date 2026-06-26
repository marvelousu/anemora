# HD2D authored nature rendering

Date: 2026-06-26
Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`

## Summary

- Continued the nature graphics uplift after the tree grove silhouette pass.
- Rejected the photo-card / imported-prefab direction because r13/r14 still showed black matte artifacts and oversized soft green tree blobs in the wide review shots.
- Accepted r17 after replacing the imported nature model path with authored low-poly mesh trees, bushes, grass/plant sprays, and log/moss accents.
- Removed the temporary nature black-renderer probe methods after using them to isolate the artifact path.
- Kept the renderer feature set frozen. No URP renderer feature was added, removed, or reordered.

## Authored Changes

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Routed `CreateImportedNatureModel` through authored replacement helpers for tree/sapling, bush, grass/plant, and log/stump/moss requests.
  - Added authored imported-nature containers and tree/bush/ground-vegetation/log-or-moss model builders with deterministic object/area-derived variation.
  - Replaced photo vegetation ground cards with authored fern, small-plant, and clover mesh objects so the accepted path no longer emits black card mattes.
  - Raised imported nature leaf/grass/wood material tones and reduced branch-lace scale where dark speckling dominated the wide camera.
  - Adjusted validation so authored `ch1_imported_nature_*` materials count for the nature tree model checks.
  - Preserved deterministic placement from object/area seeds without `Random`, `Time`, or `DateTime`.

## Review Evidence

- Review packet: `docs/review/2026-06-26T13-35_nature_photo_ground_r17/`
- Contact sheet: `docs/review/2026-06-26T13-35_nature_photo_ground_r17/contact_sheet.png`
- Map captures: `docs/review/2026-06-26T13-35_nature_photo_ground_r17/chapter1_all_maps_cycle05/`
- Latest build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Full build path: `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Visual Triage

- r14 rejected direction: the black photo-card rectangles were mostly gone, but huge soft green imported tree blobs still dominated outdoor wide shots.
- r15/r16 moved the path toward authored replacement and reduced branch-lace darkness; r17 is the accepted review candidate.
- r17 accepted direction: the r13-style black card mattes and oversized external-prefab blobs are gone from the contact sheet, while the distant panorama, waterline, and foreground nature remain readable.
- Shotdiff versus `docs/review/2026-06-26T07-26_tree_grove_silhouette_r3/` changed the outdoor map PNGs as expected, while the interior sideview stayed unchanged:
  - `01_a1_a2_current.png`: 16.2400%
  - `02_a1_a2_past.png`: 20.9854%
  - `03_b1_b3_current.png`: 13.9789%
  - `04_b1_b3_past.png`: 15.1819%
  - `05_c1_c3_current.png`: 8.8008%
  - `06_c1_c3_past.png`: 9.6883%
  - `07_d1_d3_current.png`: 9.6572%
  - `08_d1_d3_past.png`: 9.7262%
  - `09_e1_e3_current.png`: 8.5754%
  - `10_e1_e3_past.png`: 7.9659%
  - `11_f1_f6_current.png`: 6.7295%
  - `12_f1_f6_past.png`: 6.8505%
  - `13_scene6_sideview_auto.png`: 0.0000%
- Remaining visual risk: some small dark branch/leaf speckles are still visible in wide shots. They are no longer the black matte artifact, but the next nature pass should continue improving species-specific silhouette and leaf cluster readability.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed
  - Log: `Logs/nature_photo_ground_validate_r16b.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed
  - Log: `Logs/nature_photo_ground_capture_r17.log`
  - Output: `docs/review/2026-06-26T13-35_nature_photo_ground_r17/chapter1_all_maps_cycle05/`
- EditMode renderer freeze: passed
  - Result XML: `Logs/nature_photo_ground_editmode_final.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed
  - Non-passed tests: 0
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed
  - Log: `Logs/nature_photo_ground_assetvalidation_final.log`
  - Result: `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed
  - Log: `Logs/nature_photo_ground_build_final.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Built player smoke: passed
  - Log: `Logs/nature_photo_ground_player_smoke_final.log`
  - Pass marker: `ANEMORA_HOUSE_SLICE_SMOKE_PASS`

## Notes

- The accepted packet is r17. r14/r15/r16 were diagnostic only and should not be uploaded as review targets.
- Review images are R2/viewer artifacts and are not staged into git.
- Unity batch side effects must be restored before staging: `link.xml`, generated material/texture assets, `DefaultVolumeProfile.asset`, and raw `docs/devlog/screenshots/chapter1_all_maps_cycle05/` overwrites remain working evidence only.
