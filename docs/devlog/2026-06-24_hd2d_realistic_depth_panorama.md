# HD2D realistic depth panorama

Date: 2026-06-24
Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`

## Summary

- Continued the environment uplift after the all-map tree grove pass, focusing on the remaining weakness that the distant panorama still read too much like a flat outer wall.
- Added deterministic realistic-depth panorama meshes to every outdoor current/past map: near forest, rock shadow, and far woodland layers around the ring.
- Added camera-facing distant back arcs so the upper horizon can catch layered forest/rock silhouettes in wide review captures without changing any URP renderer features.
- Kept the accepted nature path on authored/generated Unity meshes and existing CC0/Textured Nature assets. The previously rejected photo vegetation card path remains out of the build.
- Added validation coverage for realistic-depth mesh counts, back-arc counts, material texture presence, and wide-camera visibility.

## Authored Changes

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Added `CreateDistantPanoramaVistaRealisticDepthBands(...)` for three deterministic low-poly far-environment depth layers.
  - Added `CreateDistantPanoramaVistaRealisticDepthBackArc(...)` for camera-facing far-horizon silhouettes.
  - Added `EnsureDistantPanoramaVistaRealisticDepthMaterial(...)` with per-area material/texture generation.
  - Extended distant panorama validation to require realistic-depth meshes, back arcs, textured materials, and camera-visible coverage.
  - Slightly retuned existing distant-band material tones/tiling so the added layers sit inside the current/past atmosphere instead of popping neon.

## Review Evidence

- Review packet: `docs/review/2026-06-24T17-05_realistic_depth_panorama_r1/`
- Contact sheet: `docs/review/2026-06-24T17-05_realistic_depth_panorama_r1/00_contact_sheet.png`
- Latest build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Full build path: `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Visual Triage

- Shotdiff versus `docs/review/2026-06-24T16-19_realistic_tree_groves_r1/` stayed below the normal 0.5% review threshold: 0/14 changed at that threshold.
- Largest measured deltas:
  - `12_f1_f6_past.png`: 0.4918%
  - `00_contact_sheet.png`: 0.4833%
  - `09_e1_e3_current.png`: 0.4231%
  - `07_d1_d3_current.png`: 0.2335%
  - `11_f1_f6_current.png`: 0.1724%
- Visual read: the change is intentionally small in pixel area because the target sits on the far upper horizon and is partially occluded by the newly larger tree groves. It does add layered forest/rock silhouettes behind the map edge, but this pass should be treated as a structural depth pass rather than the final distant-vista quality ceiling.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed
  - Log: `Logs/realistic_depth_validate_r6.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed
  - Log: `Logs/realistic_depth_capture_r9.log`
  - Output: `docs/devlog/screenshots/chapter1_all_maps_cycle05/`
- EditMode renderer freeze: passed
  - Result XML: `Logs/realistic_depth_editmode_r1.xml`
  - Result: 36 total / 36 passed / 0 failed
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed
  - Log: `Logs/realistic_depth_assetvalidation_r1.log`
  - Result: `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed
  - Log: `Logs/realistic_depth_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Built player smoke: passed
  - Log: `Logs/realistic_depth_player_smoke_r1.log`
  - Pass marker: `ANEMORA_HOUSE_SLICE_SMOKE_PASS`

## Notes

- Review images are R2/viewer artifacts and are not staged into git. The local packet contains the latest all-map frames plus `00_contact_sheet.png`.
- Unity batch side effects must be restored before staging: `link.xml`, generated material/texture assets, and raw `docs/devlog/screenshots/chapter1_all_maps_cycle05/` overwrites remain working evidence only.
- The next panorama-quality pass should target more visible terrain composition rather than subtle color polish if the review still reads as too flat.
