# HD2D tree grove silhouette

Date: 2026-06-26
Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`

## Summary

- Continued the nature graphics uplift after the terrain detail vista pass.
- Rejected r1 because the added branch and leaf detail barely moved the wide review pixels.
- Rejected r2 as the first visible pass because it still left large foreground round-canopy blobs in the outdoor wide shots.
- Accepted r3 after shifting tree selection toward birch/pine silhouettes, reducing the old authored crown volumes, and lowering the oversized imported tree companion caps.
- Added deterministic branch lace, branch fork, canopy breakup, outer leaf spray, and root fern detail to the distant realistic tree grove layer.
- Kept the renderer feature set frozen. No URP renderer feature was added, removed, or reordered.

## Authored Changes

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Added branch/leaf detail constants and validation thresholds for the realistic tree grove layer.
  - Added `CreateRealisticTreeGroveSilhouetteDetail` to layer trunk reinforcement, branch lace, branch forks, canopy breakup fans/sprays, outer leaf sprays, and root ferns over each grove tree.
  - Reduced the old Phase 2 solid canopy/crown volumes so the trees read through textured models, trunks, branches, and leaf sprays instead of large green masses.
  - Shifted imported nature tree selection away from broad round crowns and toward birch/pine silhouettes.
  - Lowered imported tree companion scale caps and under-canopy fill sizes to avoid foreground trees swallowing the map composition.
  - Preserved deterministic placement from object/area seeds without `Random`, `Time`, or `DateTime`.

## Review Evidence

- Review packet: `docs/review/2026-06-26T07-26_tree_grove_silhouette_r3/`
- Contact sheet: `docs/review/2026-06-26T07-26_tree_grove_silhouette_r3/00_contact_sheet.png`
- Latest build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Full build path: `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Visual Triage

- r1 was rejected: validation passed, but shotdiff versus terrain detail vista stayed below the normal 0.5% threshold on every map PNG.
- r2 moved the pixels but still left the house/plaza foreground dominated by round green canopy blobs.
- r3 accepted direction: map trees now present clear trunks, branch forks, birch/pine silhouettes, and layered leaf cards while keeping the distant vista readable.
- Shotdiff versus `docs/review/2026-06-26T03-59_terrain_detail_vista_r6/` at the normal 0.5% review threshold changed all 12 map PNGs:
  - `01_a1_a2_current.png`: 7.8263%
  - `02_a1_a2_past.png`: 11.5776%
  - `03_b1_b3_current.png`: 7.2383%
  - `04_b1_b3_past.png`: 12.2042%
  - `05_c1_c3_current.png`: 4.5499%
  - `06_c1_c3_past.png`: 9.6034%
  - `07_d1_d3_current.png`: 5.3316%
  - `08_d1_d3_past.png`: 7.3634%
  - `09_e1_e3_current.png`: 4.2778%
  - `10_e1_e3_past.png`: 6.3936%
  - `11_f1_f6_current.png`: 4.0173%
  - `12_f1_f6_past.png`: 4.9695%
- Visual read: B/D outdoor shots now read as tree groves rather than primitive vegetation, with visible trunks and branch structure. The style is still authored low-poly HD2D rather than fully photoreal, but the blocking-level tree problem is visibly reduced.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed
  - Log: `Logs/tree_grove_silhouette_validate_r3.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed
  - Log: `Logs/tree_grove_silhouette_capture_r3.log`
  - Output: `docs/devlog/screenshots/chapter1_all_maps_cycle05/`
- EditMode renderer freeze: passed
  - Result XML: `Logs/tree_grove_silhouette_editmode_r3.xml`
  - Result: 36 total / 36 passed / 0 failed
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed
  - Log: `Logs/tree_grove_silhouette_assetvalidation_r3.log`
  - Result: `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed
  - Log: `Logs/tree_grove_silhouette_build_r3.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Built player smoke: passed
  - Log: `Logs/tree_grove_silhouette_player_smoke_r3.log`
  - Pass marker: `ANEMORA_HOUSE_SLICE_SMOKE_PASS`

## Notes

- The accepted packet is r3. r1/r2 were diagnostic only and should not be uploaded as review targets.
- Review images are R2/viewer artifacts and are not staged into git.
- Unity batch side effects must be restored before staging: `link.xml`, generated material/texture assets, `DefaultVolumeProfile.asset`, and raw `docs/devlog/screenshots/chapter1_all_maps_cycle05/` overwrites remain working evidence only.
