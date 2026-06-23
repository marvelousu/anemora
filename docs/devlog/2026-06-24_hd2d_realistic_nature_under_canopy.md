# HD2D realistic nature under-canopy

Date: 2026-06-24
Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`

## Summary

- Continued the build-review graphics pass focused on two reported issues: Time Window aperture depth order and nature readability.
- Moved the aperture composite plane behind the frame so the player depth can occlude the window interior instead of the interior image drawing in front of the protagonist.
- Removed the accepted-path use of photo vegetation cards around imported tree/cluster companions because the wide review frames still showed dark blotches.
- Added deterministic authored under-canopy foliage fills and ground tufts around imported tree companions, rebuilt toned leaf textures with a dark-pixel alpha key, and made imported leaf/grass materials unlit to reduce black backface artifacts.
- Added a nearfield nature pass for outdoor maps so each current/past map gets clustered shrubs, grass, plant, sapling, moss-rock accents, and authored ground-cover patches at deterministic positions.

## Authored Changes

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Added `CreateChapter1RealisticNearfieldNatureForOutdoorMaps(...)` and deterministic per-area nearfield clusters.
  - Retuned imported tree selection toward broadleaf/birch silhouettes and away from overused conifers.
  - Replaced imported understory companion clutter with authored under-canopy fill meshes plus two grass tufts per imported tree companion.
  - Removed photo vegetation validation requirements from the accepted path after the black-card review failure.
  - Rebuilt toned textured-nature leaf assets in-place and alpha-keyed very dark pixels so black card backgrounds do not survive in the foliage.
  - Adjusted `ValidateApertureOpaqueComposite` to require the aperture plane behind the frame.

## Review Evidence

- Review packet: `docs/review/2026-06-24T02-33_realistic_nature_under_canopy_r1/`
- Contact sheet: `docs/review/2026-06-24T02-33_realistic_nature_under_canopy_r1/00_contact_sheet.png`
- Latest build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Full build path: `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Visual Triage

- Shotdiff versus `docs/review/2026-06-23T22-59_realistic_nature_leaf_lift_r1/` changed 12/13 all-map images above the 0.5% review threshold; `13_scene6_sideview_auto.png` was unchanged.
- Largest changes:
  - `01_a1_a2_current.png`: 23.0697%
  - `05_c1_c3_current.png`: 21.6659%
  - `04_b1_b3_past.png`: 19.3471%
  - `09_e1_e3_current.png`: 19.0020%
  - `03_b1_b3_current.png`: 18.5931%
- Remaining graphics work: the lower-edge foreground nature and some far/background masses still need better authored vegetation assets and material treatment. The next cycle should replace those weaker nature shapes rather than doing another color-only polish pass.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed
  - Log: `Logs/realistic_nature_under_canopy_validate_r1.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed
  - Log: `Logs/realistic_nature_under_canopy_capture_r1.log`
  - Output: `docs/devlog/screenshots/chapter1_all_maps_cycle05/`
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed
  - Log: `Logs/realistic_nature_under_canopy_assetvalidation_r1.log`
  - Result: `[AssetValidation] OK`
- EditMode renderer freeze: passed
  - Result XML: `Logs/realistic_nature_under_canopy_editmode_r1.xml`
  - Result: 36 total / 36 passed / 0 failed
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed
  - Log: `Logs/realistic_nature_under_canopy_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Built player smoke: passed
  - Log: `Logs/realistic_nature_under_canopy_player_smoke_r1.log`
  - Pass marker: `ANEMORA_HOUSE_SLICE_SMOKE_PASS`
  - Failure scan: 0 fail markers after excluding the renderer-contract `error=<none>` field.

## Notes

- Review images are R2/viewer artifacts and are not staged into git. The local packet contains the latest all-map frames plus `00_contact_sheet.png`.
- Unity batch side effects must be restored before staging: `link.xml`, generated material/texture assets, and raw `docs/devlog/screenshots/chapter1_all_maps_cycle05/` overwrites remain working evidence only.
