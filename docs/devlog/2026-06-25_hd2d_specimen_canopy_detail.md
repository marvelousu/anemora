# HD2D specimen canopy detail

Date: 2026-06-25
Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`

## Summary

- Continued the realistic nature pass after the close/mid specimen tree cycle.
- Added deterministic branch lace, branch forks, canopy-breakup fans/sprays, outer leaf sprays, and root fern details to every outdoor current/past specimen tree.
- Rejected the first r1 review packet as too subtle at the wide review distance; accepted r2 after moving branch and leaf detail toward the canopy edges.
- Kept the renderer feature set frozen. No URP renderer feature was added, removed, or reordered.
- Preserved the rejected photo vegetation card path out of the accepted build.

## Authored Changes

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Added `CreateRealisticSpecimenTreeCanopyDetail` under the existing `RealisticSpecimenTrees` root.
  - Added four branch-detail meshes and eight leaf/detail meshes per specimen tree.
  - Kept all new placement deterministic from area/time/index/seed values without `Random`, `Time`, or `DateTime`.
  - Extended specimen-tree validation so every outdoor current/past map must contain the new branch and leaf-detail counts.

## Review Evidence

- Review packet: `docs/review/2026-06-25T01-23_specimen_canopy_detail_r2/`
- Contact sheet: `docs/review/2026-06-25T01-23_specimen_canopy_detail_r2/00_contact_sheet.png`
- Latest build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Full build path: `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Visual Triage

- r1 was rejected: the initial branch/leaf additions were mostly hidden inside the imported canopies and barely moved the review frames.
- r2 accepted direction: edge branch lace, outer leaf sprays, canopy breakup sprays, and root ferns are pulled toward the canopy silhouette so the wide captures show more natural texture without blocking buildings or routes.
- Shotdiff versus `docs/review/2026-06-24T23-43_realistic_specimen_trees_r2/` at the normal 0.5% review threshold: all individual map PNGs stayed under budget; only `00_contact_sheet.png` changed size.
- Visual-threshold shotdiff at 0.05%: 5 individual map PNGs changed.
  - `04_b1_b3_past.png`: 0.2106%
  - `06_c1_c3_past.png`: 0.0534%
  - `08_d1_d3_past.png`: 0.0678%
  - `10_e1_e3_past.png`: 0.1124%
  - `11_f1_f6_current.png`: 0.0575%
- Visual read: r2 adds more visible branch and leaf articulation to the established specimen-tree frame while preserving the main compositions.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed
  - Log: `Logs/specimen_detail_validate_r2.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed
  - Log: `Logs/specimen_detail_capture_r2.log`
  - Output: `docs/devlog/screenshots/chapter1_all_maps_cycle05/`
- EditMode renderer freeze: passed
  - Result XML: `Logs/specimen_detail_editmode_r1.xml`
  - Result: 36 total / 36 passed / 0 failed
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed
  - Log: `Logs/specimen_detail_assetvalidation_r1.log`
  - Result: `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed
  - Log: `Logs/specimen_detail_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Built player smoke: passed
  - Log: `Logs/specimen_detail_player_smoke_r1.log`
  - Pass marker: `ANEMORA_HOUSE_SLICE_SMOKE_PASS`

## Notes

- The accepted packet is r2. The r1 local packet is diagnostic only because its additions were too hidden at review distance.
- Review images are R2/viewer artifacts and are not staged into git.
- Unity batch side effects must be restored before staging: `link.xml`, generated material/texture assets, `DefaultVolumeProfile.asset`, and raw `docs/devlog/screenshots/chapter1_all_maps_cycle05/` overwrites remain working evidence only.
