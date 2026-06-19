# HD2D realistic nature depth

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-19

## Summary

This cycle follows the distant skyline breakup pass. The previous r2 attempt added leaf flecks and shadow pockets, but shotdiff stayed below 0.05% in every wide frame and the change was too easy to lose at review distance. r3 treats that as a plateau and moves the natural detail from small color/card polish into larger authored geometry.

The accepted packet is `docs/review/2026-06-19T20-53_realistic_nature_depth_r1/`. It keeps the renderer-feature set frozen while adding deterministic canopy-volume meshes, larger leaf-face flecks, stronger canopy shadow pockets, root flares, and interior branch lace to the authored natural tree path.

## Implementation

- Added one deterministic `DistantVista_NaturalCanopyVolume` mesh per panorama segment so distant forest edges read as layered tree crowns rather than a flat green band.
- Moved the natural stand layer within the existing safe panorama ring and increased stand width, height, and depth so the new geometry survives fog, distance, and review resolution.
- Enlarged the leaf fleck and canopy shadow meshes so leaf faces and interior pockets become visible in wide all-map frames.
- Added authored root flares and crown interior branch lace to low-poly tree construction so near natural props have more trunk/crown structure.
- Extended validation to require canopy-volume generation and camera visibility, while preserving deterministic area/index-derived variation.

## Visual Review

- Review packet: `docs/review/2026-06-19T20-53_realistic_nature_depth_r1/`
- Contact sheet: `docs/review/2026-06-19T20-53_realistic_nature_depth_r1/00_contact_sheet.png`
- All 13 all-map captures were refreshed from `docs/devlog/screenshots/chapter1_all_maps_cycle05/` and copied into the review packet after r3.
- Shotdiff vs `docs/review/2026-06-19T19-56_distant_skyline_breakup_r1/`: r3 changed 12/13 wide frames; strongest movement was `10_e1_e3_past.png` at 0.6250%, `11_f1_f6_current.png` at 0.6024%, and `12_f1_f6_past.png` at 0.5311%.
- Representative checks:
  - `07_d1_d3_current.png`: route-side hill and forest layers show visible tree-crown volume behind the water ring.
  - `10_e1_e3_past.png`: farm-side distant slope now has denser natural layering instead of a single patterned wall.
  - `11_f1_f6_current.png`: final-route panorama gains more readable forest depth between the near shore, mid hills, and far mountains.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed.
  - Log: `Logs/distant_realistic_nature_validate_r3.log`
  - `Fast VS house slice validation passed.`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
  - Log: `Logs/distant_realistic_nature_capture_r3.log`
  - Output copied into `docs/review/2026-06-19T20-53_realistic_nature_depth_r1/`.
- Shotdiff triage: passed as a visible r3 change after the r2 plateau.
  - Output: `Logs/shotdiff/realistic_nature_depth_vs_skyline_r3/`
- EditMode renderer freeze: passed, 36/36.
  - XML: `Logs/distant_realistic_nature_editmode_r1.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed.
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed.
  - Log: `Logs/distant_realistic_nature_asset_validation_r1.log`
  - `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed.
  - Log: `Logs/distant_realistic_nature_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
  - Build timestamp: 2026-06-19 21:44:59 local time.
- Player smoke: 24 seconds, stopped manually after startup.
  - Log: `Logs/distant_realistic_nature_player_smoke_r1.log`
  - Case-sensitive failure scan for `Error|Exception|Assert|NullReference|MissingReference|Failed|RenderGraph`: 0 matches.
- R2 review upload: passed.
  - Uploaded 17 files.
  - Manifest: `manifests/wip-hd2d-point15-recovery-20260612.json` lists 785 paths.
- `tools/review/validate-devlog-review-sync.ps1`: passed.

Unity batch side effects will be reverted before staging. The authored implementation remains scoped to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`; review images are mirrored through the review/R2 flow rather than staged as source.

## Next

- Continue moving the distant panorama from generated-looking silhouettes toward authored biome composition: cleaner species grouping, less noisy far-hill texture, and area-specific forest profiles.
- Revisit the foreground tree silhouette so dark branch/card clusters read as intentional branch structure rather than blotches.
- Resume bridge traversal and walkable-affordance checks after this visual cycle is committed and propagated.
