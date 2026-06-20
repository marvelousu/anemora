# HD2D nature canopy breakup

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-20

## Summary

This cycle follows the forest edge overlap pass and targets the near/mid vegetation that still read as dark clumps in the all-map review contact sheet. The distant panorama now has stronger natural depth, so this pass lifts player-visible tree canopies with deterministic breakup geometry rather than moving placement anchors or retinting the shared `current_leaf`/`leaf` materials.

The accepted packet is `docs/review/2026-06-20T09-40_nature_canopy_breakup_r1/`. It adds brighter authored leaf-face meshes to normal low-poly trees, orchard nut trees, Phase2 vegetation volumes, and Phase2 groves while keeping renderer features frozen, generated vegetation non-colliding, and the authored source change scoped to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`.

## Implementation

- Added deterministic `CanopyBreakupFanA`, `CanopyBreakupFanB`, and `CanopyBreakupSprayA` meshes to `CreateAuthoredLowPolyTree`.
- Added deterministic `CanopyBreakupFanA` and `CanopyBreakupSprayA` meshes to `CreateFarmNutTree`, `CreateChapter1Phase2VegetationVolumeCluster`, and `CreateChapter1Phase2VegetationGrove`.
- Added `Ch1Surface_CurrentNatureCanopyBreakup` and `Ch1Surface_PastNatureCanopyBreakup` generated pixel materials so the new leaf faces can separate from existing darker canopy without retinting crops, grass, or other shared leaf users.
- Added validation coverage via `ValidateChapter1NatureCanopyBreakupForPrefix` for representative outdoor trees, orchard nut trees, Phase2 vegetation volumes, Phase2 groves, and the House Exterior authored vegetation prototype.
- The first Validate attempt exposed that orchard nut trees use a separate generator; the pass was corrected by adding the same canopy breakup treatment to `CreateFarmNutTree`.
- Rejected the first visual capture because it changed 12/13 frames but still read too dark in contact-sheet review. The accepted capture scales the breakup meshes larger and uses an unlit generated material so leaf faces survive wide review distance.
- Kept renderer features, deterministic placement, existing vegetation anchors, and non-colliding feature mesh behavior unchanged.

## Visual Review

- Review packet: `docs/review/2026-06-20T09-40_nature_canopy_breakup_r1/`
- Contact sheet: `docs/review/2026-06-20T09-40_nature_canopy_breakup_r1/00_contact_sheet.png`
- All 13 all-map captures were refreshed from `docs/devlog/screenshots/chapter1_all_maps_cycle05/` and copied into the review packet.
- Shotdiff vs `docs/review/2026-06-20T08-25_forest_edge_overlap_r1/`: accepted r2 visual changed 12/13 frames over the 0.02% review threshold; the side-view stability frame remained unchanged at 0%.
- Strongest movement: `01_a1_a2_current.png` 1.4775%, `02_a1_a2_past.png` 0.8384%, `06_c1_c3_past.png` 0.6364%, `04_b1_b3_past.png` 0.5858%, and `05_c1_c3_current.png` 0.5663%.
- Representative checks:
  - `01_a1_a2_current.png`: House Exterior trees now have visible leaf faces instead of mostly black canopy blobs.
  - `03_b1_b3_current.png` and `04_b1_b3_past.png`: plaza-side trees retain their silhouette but show brighter crown breakup around the building frontage.
  - `09_e1_e3_current.png` and `10_e1_e3_past.png`: orchard rows gain separated leaf patches across the wide farm view.
  - `13_scene6_sideview_auto.png`: side-view traversal/stability frame remains unchanged.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`:
  - r1 log: `Logs/nature_canopy_breakup_validate_r1.log`, failed as expected during implementation because `Current_CentralPlaza_Chapter1_E1_NutTreeA_CanopyBreakupFanA` did not exist yet.
  - r2 log: `Logs/nature_canopy_breakup_validate_r2.log`, passed after adding orchard nut tree coverage.
  - r3 log: `Logs/nature_canopy_breakup_validate_r3.log`, passed after the accepted visual size/material adjustment.
  - `Fast VS house slice validation passed.`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
  - r1 log: `Logs/nature_canopy_breakup_capture_r1.log`, rejected visually as still too subtle/dark.
  - r2 log: `Logs/nature_canopy_breakup_capture_r2.log`, accepted.
  - Output copied into `docs/review/2026-06-20T09-40_nature_canopy_breakup_r1/`.
- Shotdiff triage:
  - r2 output: `Logs/nature_canopy_breakup_shotdiff_r2.json`, rejected after manual review despite 12/13 changed frames because the contact sheet still read too dark.
  - r3 output: `Logs/nature_canopy_breakup_shotdiff_r3.json`, accepted with 12/13 changed and sideview unchanged.
- EditMode renderer freeze: passed, 36/36.
  - XML: `Logs/nature_canopy_breakup_editmode_r1.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed.
  - Note: `-runTests` was executed without `-quit` so Unity emitted the XML test result.
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed.
  - Log: `Logs/nature_canopy_breakup_asset_validation_r1.log`
  - `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed.
  - Log: `Logs/nature_canopy_breakup_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
  - Build timestamp: 2026-06-20 09:36:13 local time.
- Player smoke: 24 seconds, stopped manually after startup.
  - Log: `Logs/nature_canopy_breakup_player_smoke_r1.log`
  - Case-sensitive failure scan for `Error|Exception|Assert|NullReference|MissingReference|Failed|Crash|Fatal`: 0 matches.

Unity batch side effects were reverted before staging. The authored implementation remains scoped to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`; review images are mirrored through the review/R2 flow rather than staged as source.

## Next

- Continue realistic nature uplift by replacing more primitive-looking foreground shrubs/grass blades with richer authored silhouettes.
- Consider a dedicated pass for distant canopy color harmony after the near/mid breakup is stable, so the new leaf faces do not read too separate from the larger panorama.
