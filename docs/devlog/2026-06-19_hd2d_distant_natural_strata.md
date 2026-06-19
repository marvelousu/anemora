# HD2D distant natural strata

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-19

## Summary

This cycle continues the distant-vista and nature-realism pass after the authored tree tone cycle. The previous pass darkened the near vegetation and added stronger authored tree mass, but the wide captures still had distant hills that could read as a patterned green band instead of layered natural terrain.

The accepted packet is `docs/review/2026-06-19T18-22_distant_natural_strata_r1/`. It keeps the renderer-feature set frozen while adding more distant ridge relief, denser tree/branch/understory silhouettes, and less gridlike distant landform/canopy texture patterns.

## Implementation

- Increased distant ridge-strata mesh resolution from 9 by 5 to 13 by 6, with more varied slope, terrace cuts, peak height, and front/back depth drift.
- Increased distant conifer spire count and tier depth so the horizon has more species-like vertical mass.
- Increased distant natural trunk and branch-trace counts from 17 to 21 and added deterministic split trunks and twig traces for more authored woodland structure.
- Increased distant understory shrub count from 21 to 27 and added additional deterministic low lobes so the foreground-to-midground natural ring reads less sparse.
- Retuned `DistantLandform` and `DistantCanopy` pixel patterns to reduce checkered repetition and add more contour, rock-shadow, needle-shadow, and broken-canopy variation.
- Kept all placement deterministic from seeds and indices, and did not add renderer features, fullscreen passes, random placement, or time-based variation.

## Visual Review

- Review packet: `docs/review/2026-06-19T18-22_distant_natural_strata_r1/`
- Contact sheet: `docs/review/2026-06-19T18-22_distant_natural_strata_r1/00_contact_sheet.png`
- All 13 all-map captures were refreshed from `docs/devlog/screenshots/chapter1_all_maps_cycle05/` and copied into the review packet.
- Representative checks:
  - `03_b1_b3_current.png`: plaza/library distant terrain now has denser strata, tree breaks, and less flat green-band read.
  - `04_b1_b3_past.png`: the library-front white haze concern remains absent in the wide review frame.
  - `06_c1_c3_past.png`: the earlier desk/table artifact concern is not aggravated in the wide route capture.
  - `09_e1_e3_current.png`: farm-side panorama shows stronger layered landforms, dark forest mass, and richer distant branch texture without reopening edge voids.
  - `11_f1_f6_current.png`: final-route distant ring remains closed with more continuous natural depth.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed.
  - Log: `Logs/distant_natural_strata_validate_r1.log`
  - `Fast VS house slice validation passed.`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
  - Log: `Logs/distant_natural_strata_capture_r1.log`
  - Output copied into `docs/review/2026-06-19T18-22_distant_natural_strata_r1/`.
- EditMode renderer freeze: passed, 36/36.
  - XML: `Logs/distant_natural_strata_editmode_r2.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed.
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed.
  - Log: `Logs/distant_natural_strata_asset_validation_r1.log`
  - `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed.
  - Log: `Logs/distant_natural_strata_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
  - Build timestamp: 2026-06-19 18:36:56 local time.
- Player smoke: 20 seconds, stopped manually after startup.
  - Log: `Logs/distant_natural_strata_player_smoke_r1.log`
  - Case-sensitive failure scan for `Error|Exception|Assert|NullReference|MissingReference|Failed|RenderGraph`: 0 matches.
- R2 review upload: passed.
  - Uploaded 17 files for `wip-hd2d-point15-recovery-20260612/2026-06-19T18-22_distant_natural_strata_r1`.
  - Branch manifest now lists 734 paths.
- `tools/review/validate-devlog-review-sync.ps1`: passed.

Unity batch side effects were reverted after each run. The authored implementation remains scoped to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`; review images are mirrored through the review/R2 flow rather than staged as source.

## Next

- Push the next distant-vista pass toward more realistic natural stratification by adding stronger biome variation, rock/soil transitions, and less uniform treeline spacing.
- Continue replacing sprite-heavy nature reads with modeled tree silhouettes and richer branch/canopy hierarchy.
- Resume bridge traversal and walkable-affordance checks after the current graphics cycle is committed and propagated.
