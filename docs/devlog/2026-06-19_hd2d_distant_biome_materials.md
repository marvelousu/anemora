# HD2D distant biome materials

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-19

## Summary

This cycle continues the distant natural strata pass. The previous cycle added more ridge, trunk, twig, conifer, and understory geometry, but several wide frames still read as a green patterned terrain sheet. This pass changes the distant material language so the panorama separates into rock strata, soil/grass, and needle-canopy bands instead of one repeated green landform texture.

The accepted packet is `docs/review/2026-06-19T19-11_distant_biome_materials_r1/`. It keeps the renderer-feature set frozen while adding authored pixel-pattern variants for distant rock strata and needle canopy, then assigns them to far ridges, production-depth rock layers, conifer/treeline folds, and natural canopy materials.

## Implementation

- Added `PixelPattern.DistantRockStrata` for irregular bedding lines, fractures, and scree flecks on far ridge and rock materials.
- Added `PixelPattern.DistantNeedleCanopy` for vertical/diagonal needle mass, deep pockets, and crown tips on conifer and treeline materials.
- Retuned current and past distant band materials so mid treelines use needle canopy and far peaks use rock strata.
- Retuned current near-hill material away from saturated green and toward mixed grass/soil.
- Reassigned far landform facets, far ridge facets, production-depth back-peak layers, natural canopy, canopy accent, treeline fold, and mid-distance coppice materials to the new patterns where appropriate.
- Kept object counts, renderer features, random placement, and time-based variation unchanged.

## Visual Review

- Review packet: `docs/review/2026-06-19T19-11_distant_biome_materials_r1/`
- Contact sheet: `docs/review/2026-06-19T19-11_distant_biome_materials_r1/00_contact_sheet.png`
- All 13 all-map captures were refreshed from `docs/devlog/screenshots/chapter1_all_maps_cycle05/` and copied into the review packet.
- Representative checks:
  - `03_b1_b3_current.png`: current plaza/library distant ring keeps closed map edges while the upper strata read less like a single green sheet.
  - `04_b1_b3_past.png`: the library-front white haze concern remains absent in the wide review frame.
  - `06_c1_c3_past.png`: the earlier desk/table artifact concern is not aggravated in the wide route capture.
  - `09_e1_e3_current.png`: farm-side panorama shows more rock/needle separation across the hills and treeline.
  - `11_f1_f6_current.png`: final-route panorama remains closed and benefits from cooler far-rock materials.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed.
  - Log: `Logs/distant_biome_materials_validate_r1.log`
  - `Fast VS house slice validation passed.`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
  - Log: `Logs/distant_biome_materials_capture_r1.log`
  - Output copied into `docs/review/2026-06-19T19-11_distant_biome_materials_r1/`.
- EditMode renderer freeze: passed, 36/36.
  - XML: `Logs/distant_biome_materials_editmode_r1.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed.
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed.
  - Log: `Logs/distant_biome_materials_asset_validation_r1.log`
  - `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed.
  - Log: `Logs/distant_biome_materials_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
  - Build timestamp: 2026-06-19 19:24:56 local time.
- Player smoke: 20 seconds, stopped manually after startup.
  - Log: `Logs/distant_biome_materials_player_smoke_r1.log`
  - Case-sensitive failure scan for `Error|Exception|Assert|NullReference|MissingReference|Failed|RenderGraph`: 0 matches.
- R2 review upload: passed.
  - Uploaded 17 files for `wip-hd2d-point15-recovery-20260612/2026-06-19T19-11_distant_biome_materials_r1`.
  - Branch manifest now lists 751 paths.
- `tools/review/validate-devlog-review-sync.ps1`: passed.

Unity batch side effects were reverted after each run. The authored implementation remains scoped to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`; review images are mirrored through the review/R2 flow rather than staged as source.

## Next

- Move beyond material separation into stronger 3D skyline composition: varied mountain silhouettes, lower foothill shelves, and tree species clusters per area.
- Continue reducing any remaining patterned terrain-sheet read by adding broader authored terrain planes and more non-uniform treeline spacing.
- Resume bridge traversal and walkable-affordance checks after this graphics cycle is committed and propagated.
