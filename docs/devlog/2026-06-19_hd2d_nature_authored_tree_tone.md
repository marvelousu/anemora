# HD2D nature authored tree tone

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-19

## Summary

This cycle continues the nature-realism pass after the canopy richness cycle. The previous pass improved crown topology, but the nearest house-exterior tree sprite and several distant natural materials still read too bright and graphic in the wide review frames.

The accepted packet is `docs/review/2026-06-19T17-12_nature_authored_tree_tone_r1/`. It keeps the renderer-feature set frozen while adding authored low-poly tree geometry at the house exterior, reducing neon leaf highlights, and increasing the density of distant natural stand meshes.

## Implementation

- Added a deterministic authored natural tree layer at the house exterior behind the existing external tree sprite, using existing low-poly trunk/canopy helpers plus extra leaf cluster and lower shade spray meshes.
- Reduced the house-exterior external tree sprite scale and pushed it slightly deeper so it works as leaf texture behind the authored geometry instead of dominating the frame.
- Retuned current and past external tree sprite tints to darker, more natural greens and olive tones.
- Increased distant natural tree stand, canopy accent, leaf fringe, lobe, and conifer tier counts so the panorama reads as denser natural mass rather than simple distant blobs.
- Retuned distant foreground coppice and natural canopy materials away from saturated greens.
- Reduced global current and past leaf material brightness so all existing authored vegetation sits closer to the new natural tone.
- Kept placement deterministic from authored keys and did not add renderer features, fullscreen passes, random placement, or time-based variation.

## Visual Review

- Review packet: `docs/review/2026-06-19T17-12_nature_authored_tree_tone_r1/`
- Contact sheet: `docs/review/2026-06-19T17-12_nature_authored_tree_tone_r1/00_contact_sheet.png`
- All 13 all-map captures were refreshed from `docs/devlog/screenshots/chapter1_all_maps_cycle05/` and copied into the review packet.
- Representative checks:
  - `01_a1_a2_current.png`: current-side tree colors are darker and less neon while the distant natural ring remains closed.
  - `03_b1_b3_current.png`: plaza/library foreground trees now read as authored tree forms with darker leaf mass and less sprite-like saturation.
  - `04_b1_b3_past.png`: the library-front white haze concern remains absent in the wide review frame; remaining warm leaf flecks are subdued.
  - `06_c1_c3_past.png`: the earlier desk/table artifact concern is not aggravated in the wide route capture.
  - `09_e1_e3_current.png`: farm-side panorama and natural edges retain denser tree silhouettes without reopening map-edge voids.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed.
  - Log: `Logs/nature_authored_tree_tone_validate_r3.log`
  - `Fast VS house slice validation passed.`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
  - Log: `Logs/nature_authored_tree_tone_capture_r3.log`
  - Output copied into `docs/review/2026-06-19T17-12_nature_authored_tree_tone_r1/`.
- EditMode renderer freeze: passed, 36/36.
  - XML: `Logs/nature_authored_tree_tone_editmode_r1.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed.
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed.
  - Log: `Logs/nature_authored_tree_tone_asset_validation_r1.log`
  - `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed.
  - Log: `Logs/nature_authored_tree_tone_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
  - Build timestamp: 2026-06-19 17:29:38 local time.
- Player smoke: 20 seconds, stopped manually after startup.
  - Log: `Logs/nature_authored_tree_tone_player_smoke_r1.log`
  - Case-sensitive failure scan for `Error|Exception|Assert|NullReference|MissingReference|Failed|RenderGraph`: 0 matches.
- R2 review upload: passed.
  - Uploaded 17 files for `wip-hd2d-point15-recovery-20260612/2026-06-19T17-12_nature_authored_tree_tone_r1`.
  - Branch manifest now lists 717 paths.
- `tools/review/validate-devlog-review-sync.ps1`: passed.

Unity batch side effects were reverted after each run. The authored implementation remains scoped to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`; review images are mirrored through the review/R2 flow rather than staged as source.

## Next

- Continue nature realism by replacing remaining sprite-heavy tree reads with species-level modeled silhouettes and stronger trunk/branch structure.
- Continue distant vista quality with richer foreground-to-horizon landform texture density, especially where the distant natural ring still reads as a flat green band.
- Resume bridge traversal and walkable-affordance checks after this visual cycle is committed and propagated.
