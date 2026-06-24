# HD2D realistic specimen trees

Date: 2026-06-24
Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`

## Summary

- Continued the nature-quality pass after the ScenicRelief distant-vista cycle.
- Added deterministic close/mid specimen tree framing to every outdoor current/past map using the accepted CC0/Textured Nature model path plus authored trunk, canopy, bush, grass, and ground-cover companions.
- Corrected the first visual pass after it produced oversized foreground trunks that cut across the library facade; the accepted r2 packet uses smaller trees and side/rear framing so the nature read improves without blocking the main architecture or route.
- Kept the renderer feature set frozen. No URP renderer feature was added, removed, or reordered.
- Preserved the rejected photo vegetation card path out of the accepted build.

## Authored Changes

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Added `RealisticSpecimenTrees` roots to every outdoor current/past map.
  - Added three deterministic specimen tree clusters per map with imported tree/bush/grass models, authored trunk reinforcement, authored canopy fill, and base planting.
  - Derived placement variation from area/time/index hashes without `Random`, `Time`, or `DateTime`.
  - Added validation for root presence, mesh/material completeness, collider absence, renderer policy, and wide-camera visibility.

## Review Evidence

- Review packet: `docs/review/2026-06-24T23-43_realistic_specimen_trees_r2/`
- Contact sheet: `docs/review/2026-06-24T23-43_realistic_specimen_trees_r2/00_contact_sheet.png`
- Latest build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Full build path: `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Visual Triage

- r1 was rejected: the first capture made imported specimen trees too large and placed a white-barked trunk through the CentralPlaza/library review composition.
- r2 accepted direction: tree scale was reduced, authored canopy fill was tightened, and the third specimen moved from the center to side/rear framing.
- Shotdiff versus `docs/review/2026-06-24T20-52_scenic_relief_backdrop_r1/` at the normal 0.5% review threshold: 2/14 changed.
  - `00_contact_sheet.png`: 0.7937%
  - `08_d1_d3_past.png`: 0.9654%
- Visual-threshold shotdiff at 0.05%: 9/14 changed.
  - B/D/E/F views show the clearest close/mid nature read.
  - A/C stay comparatively stable because existing foreground grass/tree-grove layers and camera framing already dominate those views.
- Visual read: r2 adds recognizable tree mass and trunks without reintroducing a central obstruction. It is a solid close/mid nature step, not the final ceiling for realistic vegetation.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed
  - Log: `Logs/specimen_trees_validate_r3.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed
  - Log: `Logs/specimen_trees_capture_r2.log`
  - Output: `docs/devlog/screenshots/chapter1_all_maps_cycle05/`
- EditMode renderer freeze: passed
  - Result XML: `Logs/specimen_trees_editmode_r1.xml`
  - Result: 36 total / 36 passed / 0 failed
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed
  - Log: `Logs/specimen_trees_assetvalidation_r1.log`
  - Result: `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed
  - Log: `Logs/specimen_trees_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Built player smoke: passed
  - Log: `Logs/specimen_trees_player_smoke_r1.log`
  - Pass marker: `ANEMORA_HOUSE_SLICE_SMOKE_PASS`

## Notes

- The accepted packet is r2. The r1 local packet was used only to diagnose the oversized-trunk composition problem and should not be used for public review.
- Review images are R2/viewer artifacts and are not staged into git. The local r2 packet contains the latest all-map frames plus `00_contact_sheet.png`.
- Unity batch side effects must be restored before staging: `link.xml`, generated material/texture assets, and raw `docs/devlog/screenshots/chapter1_all_maps_cycle05/` overwrites remain working evidence only.
