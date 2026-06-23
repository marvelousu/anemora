# HD2D realistic nature leaf lift

Date: 2026-06-23
Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`

## Summary

- Continued the nature graphics pass after build-review feedback that the trees/nature still did not read as natural enough.
- Removed the earlier photo branch/canopy-card experiment from the accepted path because it produced dark blotches and weak tree readability in review captures.
- Shifted imported tree selection toward broadleaf/birch silhouettes, added deterministic broadleaf saplings near imported tree companions, added a smaller photo fern/small-plant/clover ground layer, and lifted dark leaf/grass material tones.
- Kept the renderer contract frozen; no URP renderer features were added, removed, or reordered.

## Authored Changes

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Added a deterministic broadleaf sapling companion beside imported tree companions.
  - Added photo vegetation ground-layer cards for tree and cluster companions, then reduced their footprint after black-footprint review.
  - Raised current/past imported nature, canopy breakup, grass silhouette, and `current_leaf` dark tones so wide shots read as foliage instead of black-green masses.
  - Disabled hard shadow casting for authored vegetation meshes.
  - Removed the rejected photo branch/canopy card helpers and texture constants from the accepted path.

## Review Evidence

- Review packet: `docs/review/2026-06-23T22-59_realistic_nature_leaf_lift_r1/`
- Contact sheet: `docs/review/2026-06-23T22-59_realistic_nature_leaf_lift_r1/00_contact_sheet.png`
- Latest build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Full build path: `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Visual Triage

- Shotdiff versus `docs/review/2026-06-23T19-00_imported_nature_scale_r1/` changed 12/13 all-map images above the 0.5% review threshold; `13_scene6_sideview_auto.png` was unchanged.
- Largest changes:
  - `01_a1_a2_current.png`: 8.7188%
  - `05_c1_c3_current.png`: 6.4859%
  - `02_a1_a2_past.png`: 3.7553%
  - `03_b1_b3_current.png`: 3.6189%
  - `06_c1_c3_past.png`: 3.3215%
- Leaf readability pixels increased in the key current wide shots while the rejected branch cards were removed.
- Remaining issue for the next graphics cycle: `01_a1_a2_current.png` still has dark foreground nature silhouettes along the lower edge/lower-right; the foreground-edge silhouette layer should be replaced or retuned rather than polished with more color constants.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed
  - Log: `Logs/realistic_nature_leaf_lift_validate_r1.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed
  - Log: `Logs/realistic_nature_leaf_lift_capture_r1.log`
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed
  - Log: `Logs/realistic_nature_leaf_lift_assetvalidation_r1.log`
  - Result: `[AssetValidation] OK`
- EditMode renderer freeze: passed
  - Result XML: `Logs/realistic_nature_leaf_lift_editmode_r1.xml`
  - Result: 36 total / 36 passed / 0 failed
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed
  - Log: `Logs/realistic_nature_leaf_lift_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Built player smoke: passed
  - Log: `Logs/realistic_nature_leaf_lift_player_smoke_r3.log`
  - Pass marker: `ANEMORA_HOUSE_SLICE_SMOKE_PASS`
  - Failure scan: 0 fail markers after excluding the renderer-contract `error=<none>` field.

## Notes

- Unity batch side effects were restored after each run: `link.xml`, `DefaultVolumeProfile.asset`, generated photo vegetation materials, generated textured nature leaf materials, generated nature surface textures, and the dappled shadow material.
- Generated review screenshots under `docs/devlog/screenshots/chapter1_all_maps_cycle05/` are working evidence and were not staged for the code/docs commit.
