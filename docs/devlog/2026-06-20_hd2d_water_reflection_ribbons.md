# HD2D water reflection ribbons

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-20

## Summary

This cycle follows the bridge authored-crossing proof and returns to the natural distant-vista strand. The prior wetland and shallow-pool cycles broke up the shoreline, but the wide captures still had broad, still water surfaces that read too flat against the richer distant forests. This pass adds deterministic low-poly water reflection ribbons to the existing waterline breakup layer so lakes and rivers gain authored flow direction, reflected color, and surface shimmer without reintroducing the rejected white haze.

The accepted packet is `docs/review/2026-06-20T05-35_water_reflection_ribbons_r1/`. It keeps renderer features frozen while adding `WaterlineBreakup_ReflectionRibbon` meshes, current/past `Ch1Distant_*WaterlineBreakupReflectionRibbon` materials, validation count coverage, and camera visibility coverage.

## Implementation

- Added `WaterlineBreakupReflectionRibbonCount` and a reflection-ribbon visible minimum so the new water-surface layer is enforced by validation.
- Added `CreateWaterlineBreakupReflectionRibbonMesh`, a low double-sided elongated mesh with deterministic broken glint lanes, small cross-current bend, and irregular edge fade.
- Extended `CreateChapter1PhaseIWaterlineBreakup` to add fourteen deterministic reflection-ribbon meshes per outdoor current/past map.
- Added `EnsureWaterlineBreakupReflectionRibbonMaterial` with current blue-green and past muted ochre-green `Ch1Distant_*WaterlineBreakupReflectionRibbon` textured water materials.
- Extended waterline validation to require reflection-ribbon textures, mesh counts, textured material use, non-colliding renderer policy, and camera visibility.
- Rejected r1 as too subtle, then widened/raised/inner-shifted r2; accepted r3 after strengthening the past water palette enough for the past wide frames while staying away from white haze.
- Kept renderer features, placement determinism, non-colliding geometry, and authored-file scope unchanged.

## Visual Review

- Review packet: `docs/review/2026-06-20T05-35_water_reflection_ribbons_r1/`
- Contact sheet: `docs/review/2026-06-20T05-35_water_reflection_ribbons_r1/00_contact_sheet.png`
- All 13 all-map captures were refreshed from `docs/devlog/screenshots/chapter1_all_maps_cycle05/` and copied into the review packet.
- Shotdiff vs `docs/review/2026-06-20T04-05_bridge_authored_crossing_r1/`: 11/13 frames changed over the 0.02% automated budget. The side-view stability frame remained unchanged; `08_d1_d3_past.png` stayed below the automated budget at 0.0167% but manual review shows the waterline ribbons present, with the larger central lake surface left as the next water-specific target.
- Strongest movement: `01_a1_a2_current.png` 0.8188%, `05_c1_c3_current.png` 0.6208%, `09_e1_e3_current.png` 0.5073%, `06_c1_c3_past.png` 0.2790%.
- Representative checks:
  - `01_a1_a2_current.png`: the broad exterior water edge gains long blue-green reflection lanes instead of a single flat strip.
  - `05_c1_c3_current.png`: mid-distance water now has readable directional surface marks behind the outdoor map.
  - `12_f1_f6_past.png`: past-side water keeps a muted ochre-green palette and avoids the earlier white haze failure mode.
  - `08_d1_d3_past.png`: waterline ribbons are visible, but the central broad lake still needs a separate surface-body pass rather than more shoreline-only polishing.
  - `13_scene6_sideview_auto.png`: side-view traversal/stability frame remains unchanged.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`:
  - r1 log: `Logs/water_reflection_ribbons_validate_r1.log`, passed but the wrapper timed out before capturing exit code.
  - r2 log: `Logs/water_reflection_ribbons_validate_r2.log`, passed with exit code 0.
  - r3 log: `Logs/water_reflection_ribbons_validate_r3.log`, passed with exit code 0.
  - `Fast VS house slice validation passed.`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
  - r3 log: `Logs/water_reflection_ribbons_capture_r3.log`, exit code 0.
  - Output copied into `docs/review/2026-06-20T05-35_water_reflection_ribbons_r1/`.
- Shotdiff triage:
  - r1 output: `Logs/water_reflection_ribbons_shotdiff_r1.json`, rejected because only 4/13 frames changed.
  - r2 output: `Logs/water_reflection_ribbons_shotdiff_r2.json`, improved to 11/13 frames.
  - r3 output: `Logs/water_reflection_ribbons_shotdiff_r3.json`, accepted with 11/13 frames changed and manual review of the remaining D past wide frame.
- EditMode renderer freeze: passed, 36/36.
  - XML: `Logs/water_reflection_ribbons_editmode_r1.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed.
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed.
  - Log: `Logs/water_reflection_ribbons_asset_validation_r1.log`
  - `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed.
  - Log: `Logs/water_reflection_ribbons_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
  - Build timestamp: 2026-06-20 05:41:01 local time.
- Player smoke: 24 seconds, stopped manually after startup.
  - Log: `Logs/water_reflection_ribbons_player_smoke_r1.log`
  - Case-sensitive failure scan for `Error|Exception|Assert|NullReference|MissingReference|Failed|RenderGraph`: 0 matches.

Unity batch side effects were reverted before staging. The authored implementation remains scoped to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`; review images are mirrored through the review/R2 flow rather than staged as source.

## Next

- Add a separate broad lake-surface detail layer for large central water bodies, especially `08_d1_d3_past.png`, instead of continuing to overload shoreline-only breakup.
- Continue natural realism with denser tree silhouette shaping and area-specific far-shore identity once the large-water pass is stable.
