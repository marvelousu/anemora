# HD2D distant terrain de-repeat

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-19

## Summary

This cycle follows the forest species profile pass. The improved forest silhouettes made the distant slopes more important, and the old 32x32 distant landform/rock textures were still reading as repeated small tiles in wide review frames.

The accepted packet is `docs/review/2026-06-19T23-31_distant_terrain_de_repeat_r1/`. It keeps renderer features frozen while making distant-only pixel textures 64x64, lowering distant landform/rock tiling, and reducing the regular modulo-grid feel in the landform and strata patterns.

## Implementation

- Changed `EnsurePixelTexture` so only distant pixel patterns use 64x64 generated textures; non-distant materials stay 32x32.
- Added `IsDistantPixelPattern` to keep the texture-size change scoped to distant landform/canopy/rock patterns.
- Reworked `PixelPattern.DistantLandform` and `PixelPattern.DistantRockStrata` to use lower-frequency macro variation instead of tight repeated rows.
- Lowered tiling on distant landform, far peak, landform facet, ridge facet, and production-depth rock/terrain materials.
- Kept renderer features, placement determinism, object counts, and time-independent generation unchanged.

## Visual Review

- Review packet: `docs/review/2026-06-19T23-31_distant_terrain_de_repeat_r1/`
- Contact sheet: `docs/review/2026-06-19T23-31_distant_terrain_de_repeat_r1/00_contact_sheet.png`
- All 13 all-map captures were refreshed from `docs/devlog/screenshots/chapter1_all_maps_cycle05/` and copied into the review packet.
- Shotdiff vs `docs/review/2026-06-19T22-22_forest_species_profile_r1/`: all 12 wide current/past frames changed. Strongest movement was `11_f1_f6_current.png` at 2.1483%, `09_e1_e3_current.png` at 1.8416%, and `01_a1_a2_current.png` at 1.5476%.
- Representative checks:
  - `04_b1_b3_past.png`: plaza/library background keeps the white haze regression absent while the hill texture reads less tile-like.
  - `09_e1_e3_current.png`: farm-side far hills carry broader terrain variation behind the improved forest clusters.
  - `11_f1_f6_current.png`: final-route panorama keeps the lake and forest ring readable while the mountain surface repeats less aggressively.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed.
  - Log: `Logs/distant_terrain_texture_de_repeat_validate_r1.log`
  - `Fast VS house slice validation passed.`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
  - Log: `Logs/distant_terrain_texture_de_repeat_capture_r1.log`
  - Output copied into `docs/review/2026-06-19T23-31_distant_terrain_de_repeat_r1/`.
- Shotdiff triage: passed as a visible all-wide-frame change.
  - Output: `Logs/shotdiff/distant_terrain_de_repeat_vs_forest_species_r1/`
- EditMode renderer freeze: passed, 36/36.
  - XML: `Logs/distant_terrain_texture_de_repeat_editmode_r1.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed.
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed.
  - Log: `Logs/distant_terrain_texture_de_repeat_asset_validation_r1.log`
  - `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed.
  - Log: `Logs/distant_terrain_texture_de_repeat_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
  - Build timestamp: 2026-06-19 23:47:43 local time.
- Player smoke: 24 seconds, stopped manually after startup.
  - Log: `Logs/distant_terrain_texture_de_repeat_player_smoke_r1.log`
  - Case-sensitive failure scan for `Error|Exception|Assert|NullReference|MissingReference|Failed|RenderGraph`: 0 matches.
- R2 review upload: passed.
  - Uploaded 17 files.
  - Manifest: `manifests/wip-hd2d-point15-recovery-20260612.json` lists 819 paths.
- `tools/review/validate-devlog-review-sync.ps1`: passed.

Unity batch side effects will be reverted before staging. The authored implementation remains scoped to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`; review images are mirrored through the review/R2 flow rather than staged as source.

## Next

- Add more authored terrain shape variation, because texture de-repeat helps but does not replace actual slope/valley geometry.
- Continue improving area-specific forest and mountain identities across A/B/C/D/E/F.
- Resume bridge traversal and walkable-affordance checks after the distant visual base is stable.
