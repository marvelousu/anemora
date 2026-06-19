# HD2D distant canopy and landform texture uplift

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-19

## Summary

This cycle continues the environment uplift after the library facade/desk cleanup. The goal was to move the distant panorama and natural graphics away from flat low-poly color slabs and toward authored, production-readable terrain and vegetation.

The accepted packet is `docs/review/2026-06-19T14-36_distant_canopy_landform_r3/`. It keeps the map-edge void closed, adds visible landform grain to the distant ring, separates mountain/landform texture from canopy texture, and adds fine leaf-fringe geometry to the natural tree-stand accents.

## Implementation

- Replaced the remaining flat distant panorama band materials with generated textured band materials for all outdoor maps, current and past.
- Added `DistantLandform` and `DistantCanopy` pixel patterns so mountain/ridge surfaces and green tree/grass masses no longer share the same noisy treatment.
- Retuned distant band UV scale so the large landform faces show authored terrain breakup without the earlier r2 over-noise.
- Converted midground valley, foreground coppice, area landmark/signature, landform facet, ridge facet, treeline fold, production-depth, and mid-distance landform closure materials away from flat fills where they were visibly contributing to the "board" look.
- Added deterministic leaf-fringe quads into the already-visible natural canopy accent mesh, avoiding the earlier separate leaf-veil attempt that failed visibility.
- Muted distant canopy/accent greens so nature reads less neon while preserving a brighter natural highlight in current-time captures.

## Visual Review

- Review packet: `docs/review/2026-06-19T14-36_distant_canopy_landform_r3/`
- Contact sheet: `docs/review/2026-06-19T14-36_distant_canopy_landform_r3/00_contact_sheet.png`
- All 13 all-map captures were refreshed from `docs/devlog/screenshots/chapter1_all_maps_cycle05/`.
- Representative checks:
  - `03_b1_b3_current.png`: distant hill faces and treeline rings now carry visible texture and canopy breakup rather than flat green slabs.
  - `09_e1_e3_current.png`: farm-side panorama now has layered ridge/canopy texture across the full circular vista.
  - `04_b1_b3_past.png` and `06_c1_c3_past.png`: the earlier library-front white haze and desk/table artifact concerns do not reappear in the wide review captures.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed.
  - Log: `Logs/distant_canopy_pattern_validate_r3.log`
  - `Fast VS house slice validation passed.`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
  - Log: `Logs/distant_canopy_pattern_capture_r3.log`
  - Output: `docs/devlog/screenshots/chapter1_all_maps_cycle05/`
- EditMode renderer freeze: passed, 36/36.
  - XML: `Logs/distant_canopy_pattern_editmode_r4.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed.
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed.
  - Log: `Logs/distant_canopy_pattern_asset_validation_r3.log`
  - `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed.
  - Log: `Logs/distant_canopy_pattern_build_r3.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
  - Build timestamp: 2026-06-19 14:53:31 local time.
- Player smoke: 20 seconds, stopped manually after startup.
  - Log: `Logs/distant_canopy_pattern_player_smoke_r3.log`
  - Case-sensitive failure scan for `Error|Exception|Assert|NullReference|MissingReference|Failed|RenderGraph`: 0 matches.
- R2 review upload: passed.
  - Uploaded 17 files for `wip-hd2d-point15-recovery-20260612/2026-06-19T14-36_distant_canopy_landform_r3`.
  - Branch manifest now lists 683 paths.
- `tools/review/validate-devlog-review-sync.ps1`: passed.

Unity batch side effects were reverted after each run. The authored implementation remains scoped to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`; review images are mirrored through the review/R2 flow rather than staged as source.

## Next

- Continue nature realism by replacing the largest near/mid-distance tree crowns with richer generated or authored tree assets where the current procedural meshes still read as coarse.
- Add species-level silhouette variation and less uniform forest edges before moving to broader lighting polish.
