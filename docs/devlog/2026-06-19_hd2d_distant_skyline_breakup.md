# HD2D distant skyline breakup

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-19

## Summary

This cycle follows the distant biome material pass. The materials now separate rock strata and needle canopy better, but the wide frames still needed stronger landform silhouettes so the horizon reads as terrain instead of a repeated backdrop wall.

The accepted packet is `docs/review/2026-06-19T19-56_distant_skyline_breakup_r1/`. It keeps the renderer-feature set frozen while adding deterministic erosion cuts, shoulders, skyline notches, and extra mesh columns to existing distant relief, ridge facet, and ridge strata meshes.

## Implementation

- Increased distant ridge facet mesh columns from 19 to 23 and added deterministic erosion cuts, spurs, and shelf offsets.
- Increased ridge strata mesh columns from 13 to 17 and added skyline notches/spurs to the top row.
- Increased base distant relief mesh columns from 19 to 23 and added eroded gaps, rock shoulders, skyline lean, and non-flat base lift.
- Kept object counts, renderer features, random placement, and time-based variation unchanged.

## Visual Review

- Review packet: `docs/review/2026-06-19T19-56_distant_skyline_breakup_r1/`
- Contact sheet: `docs/review/2026-06-19T19-56_distant_skyline_breakup_r1/00_contact_sheet.png`
- All 13 all-map captures were refreshed from `docs/devlog/screenshots/chapter1_all_maps_cycle05/` and copied into the review packet.
- Representative checks:
  - `03_b1_b3_current.png`: plaza/library distant ring remains closed with less uniform ridge contour.
  - `04_b1_b3_past.png`: the library-front white haze concern remains absent in the wide review frame.
  - `09_e1_e3_current.png`: farm-side panorama keeps the water/treeline ring and gains more irregular ridge shoulders.
  - `11_f1_f6_current.png`: final-route wide frame shows the clearest skyline breakup, with less wall-like far mountain profile.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed.
  - Log: `Logs/distant_skyline_breakup_validate_r1.log`
  - `Fast VS house slice validation passed.`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
  - Log: `Logs/distant_skyline_breakup_capture_r1.log`
  - Output copied into `docs/review/2026-06-19T19-56_distant_skyline_breakup_r1/`.
- EditMode renderer freeze: passed, 36/36.
  - XML: `Logs/distant_skyline_breakup_editmode_r1.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed.
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed.
  - Log: `Logs/distant_skyline_breakup_asset_validation_r1.log`
  - `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed.
  - Log: `Logs/distant_skyline_breakup_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
  - Build timestamp: 2026-06-19 20:09:58 local time.
- Player smoke: 20 seconds, stopped manually after startup.
  - Log: `Logs/distant_skyline_breakup_player_smoke_r1.log`
  - Case-sensitive failure scan for `Error|Exception|Assert|NullReference|MissingReference|Failed|RenderGraph`: 0 matches.
- R2 review upload: passed.
  - Uploaded 17 files.
  - Manifest: `manifests/wip-hd2d-point15-recovery-20260612.json` lists 768 paths.
- `tools/review/validate-devlog-review-sync.ps1`: passed.

Unity batch side effects were reverted after each run. The authored implementation remains scoped to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`; review images are mirrored through the review/R2 flow rather than staged as source.

## Next

- Make the next pass more structural: area-specific mountain silhouettes and biome clusters rather than only per-mesh erosion.
- Continue reducing terrain-sheet repetition by adding broader foreground-to-midground landform planes where the view still shows flat patterned ground.
- Resume bridge traversal and walkable-affordance checks after this visual cycle is committed and propagated.
