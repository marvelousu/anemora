# HD2D distant slope contours

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-20

## Summary

This cycle follows the distant terrain de-repeat pass. The previous pass reduced distant texture repetition, but the far panorama still needed more authored geometry so slopes and valley faces read as terrain rather than textured backdrops.

The accepted packet is `docs/review/2026-06-20T00-59_distant_slope_contours_r1/`. It keeps renderer features frozen while adding a deterministic `DistantVista_SlopeContour` mesh ring with textured Ch1Distant landform materials, validation count coverage, and camera visibility coverage.

## Implementation

- Added `DistantPanoramaVistaSlopeContourMeshCount` and a visible minimum so the new layer is enforced by validation rather than only by convention.
- Added `CreateDistantPanoramaVistaSlopeContours` to place one authored slope-contour mesh per panorama segment.
- Added `CreateDistantPanoramaVistaSlopeContourMesh`, a double-sided low-poly slope grid with deterministic valley cuts, spur lifts, terraces, and cross-slope variation.
- Added `EnsureDistantPanoramaVistaSlopeContourMaterial` with current/past `Ch1Distant_*SlopeContour` textured landform materials.
- Extended distant vista validation to require slope contour counts, textured material use, and visible camera coverage.
- Rejected r1 placement because it only changed 5 pixels in shotdiff; r2 moves the contour ring forward and taller so it reads in all wide frames.
- Kept renderer features, placement determinism, collision-free distant geometry, and authored-file scope unchanged.

## Visual Review

- Review packet: `docs/review/2026-06-20T00-59_distant_slope_contours_r1/`
- Contact sheet: `docs/review/2026-06-20T00-59_distant_slope_contours_r1/00_contact_sheet.png`
- All 13 all-map captures were refreshed from `docs/devlog/screenshots/chapter1_all_maps_cycle05/` and copied into the review packet.
- Shotdiff vs `docs/review/2026-06-19T23-31_distant_terrain_de_repeat_r1/`: all 12 wide current/past frames changed. Strongest movement was `09_e1_e3_current.png` at 4.0834%, `10_e1_e3_past.png` at 3.9320%, `07_d1_d3_current.png` at 3.0008%, and `12_f1_f6_past.png` at 2.7123%.
- Representative checks:
  - `07_d1_d3_current.png`: route-side far terrain now has a visible carved slope layer behind the lake and forest ring.
  - `09_e1_e3_current.png`: farm-side distant hills gain broad valley/ridge faces without covering the playable map.
  - `04_b1_b3_past.png`: plaza/library background keeps the white haze regression from returning while the distant slopes gain more authored contour.
  - `06_c1_c3_past.png`: library-side table artifact regression check remains readable in the packet.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed.
  - r1 log: `Logs/distant_slope_contour_validate_r1.log`
  - r2 log: `Logs/distant_slope_contour_validate_r2.log`
  - `Fast VS house slice validation passed.`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
  - r2 log: `Logs/distant_slope_contour_capture_r2.log`
  - Output copied into `docs/review/2026-06-20T00-59_distant_slope_contours_r1/`.
- Shotdiff triage:
  - r1 output: `Logs/distant_slope_contour_shotdiff_r1/`, rejected as plateau with only 5 changed pixels across wide frames.
  - r2 output: `Logs/distant_slope_contour_shotdiff_r2/`, accepted with all 12 wide frames changed and strongest movement at 4.0834%.
- EditMode renderer freeze: passed, 36/36.
  - XML: `Logs/distant_slope_contour_editmode_r2.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed.
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed.
  - Log: `Logs/distant_slope_contour_asset_validation_r2.log`
  - `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed.
  - Log: `Logs/distant_slope_contour_build_r2.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
  - Build timestamp: 2026-06-20 00:58:04 local time.
- Player smoke: 24 seconds, stopped manually after startup.
  - Log: `Logs/distant_slope_contour_player_smoke_r2.log`
  - Case-sensitive failure scan for `Error|Exception|Assert|NullReference|MissingReference|Failed|RenderGraph`: 0 matches.
- R2 review upload: passed.
  - Uploaded 17 files.
  - Manifest: `manifests/wip-hd2d-point15-recovery-20260612.json` lists 836 paths.
- `tools/review/validate-devlog-review-sync.ps1`: passed.

Unity batch side effects will be reverted before staging. The authored implementation remains scoped to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`; review images are mirrored through the review/R2 flow rather than staged as source.

## Next

- Continue giving each outdoor vista more area-specific terrain identity, especially the farm-side and final-route panoramas.
- Add more midground landform transitions where the lake edge still exposes flat gray gaps.
- Resume bridge traversal and walkable-affordance checks after the distant visual base is stable.
