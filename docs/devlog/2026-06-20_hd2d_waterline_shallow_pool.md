# HD2D waterline shallow pool

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-20

## Summary

This cycle follows the waterline wetland transition pass. The wetland mats improved shoreline reads, but the water itself still had broad gray surfaces in several wide captures. This pass adds authored low-poly shallow-water patches to the existing waterline breakup layer so lakes and rivers have visible shallow depth and reflected color instead of reading as flat placeholder planes.

The accepted packet is `docs/review/2026-06-20T03-27_waterline_shallow_pool_r1/`. It keeps renderer features frozen while adding deterministic `WaterlineBreakup_ShallowPool` meshes, current/past `Ch1Distant_*WaterlineBreakupShallowPool` materials, validation count coverage, and camera visibility coverage.

## Implementation

- Added `WaterlineBreakupShallowPoolCount` and a shallow-pool visible minimum so the new water surface layer is enforced by validation.
- Added `CreateWaterlineBreakupShallowPoolMesh`, a low double-sided water patch with irregular oval edges, light relief, and deterministic glint ridges.
- Extended `CreateChapter1PhaseIWaterlineBreakup` to add twelve deterministic shallow-pool meshes per outdoor current/past map.
- Added `EnsureWaterlineBreakupShallowPoolMaterial` with current blue-green and past ochre-green `Ch1Distant_*WaterlineBreakupShallowPool` textured water materials.
- Extended waterline validation to require shallow-pool textures, shallow-pool mesh counts, textured material use, and camera visibility.
- Rejected r1 as a visual plateau, then rejected the first r2 placement after validation caught a MiaHouse inner-radius violation; accepted r2b after moving the nearest band back into the validated waterline region.
- Kept renderer features, placement determinism, non-colliding geometry, and authored-file scope unchanged.

## Visual Review

- Review packet: `docs/review/2026-06-20T03-27_waterline_shallow_pool_r1/`
- Contact sheet: `docs/review/2026-06-20T03-27_waterline_shallow_pool_r1/00_contact_sheet.png`
- All 13 all-map captures were refreshed from `docs/devlog/screenshots/chapter1_all_maps_cycle05/` and copied into the review packet.
- Shotdiff vs `docs/review/2026-06-20T02-12_waterline_wetland_transition_r1/`: all 12 wide current/past frames changed, while the side-view stability frame remained unchanged. Strongest movement was `01_a1_a2_current.png` at 1.9141%, `03_b1_b3_current.png` at 1.3213%, `07_d1_d3_current.png` at 1.2203%, and `09_e1_e3_current.png` at 1.0922%.
- Representative checks:
  - `01_a1_a2_current.png`: house exterior water now has visible blue-green shallow patches instead of one continuous gray surface.
  - `03_b1_b3_current.png`: plaza/library background gains shallow water variation without reintroducing the white haze.
  - `10_e1_e3_past.png`: farm-side past water uses muted ochre-green shallow patches that stay within the time-of-day palette.
  - `13_scene6_sideview_auto.png`: side-view traversal/stability frame remains unchanged.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`:
  - r1 log: `Logs/waterline_shallow_pool_validate_r1.log`, passed but visually plateaued.
  - r2 log: `Logs/waterline_shallow_pool_validate_r2.log`, failed because `Current_MiaHouse_WaterlineBreakup_ShallowPool_S01` left the intended river-edge radius band.
  - r2b log: `Logs/waterline_shallow_pool_validate_r2b.log`, passed.
  - `Fast VS house slice validation passed.`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
  - r2b log: `Logs/waterline_shallow_pool_capture_r2b.log`
  - Output copied into `docs/review/2026-06-20T03-27_waterline_shallow_pool_r1/`.
- Shotdiff triage:
  - r1 output: `Logs/waterline_shallow_pool_shotdiff_r1/`, rejected because all individual frames stayed below the 0.05% threshold.
  - r2b output: `Logs/waterline_shallow_pool_shotdiff_r2b/`, accepted with all 12 wide current/past frames changed and strongest movement at 1.9141%.
- EditMode renderer freeze: passed, 36/36.
  - XML: `Logs/waterline_shallow_pool_editmode_r2b.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed.
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed.
  - Log: `Logs/waterline_shallow_pool_asset_validation_r2b.log`
  - `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed.
  - Log: `Logs/waterline_shallow_pool_build_r2b.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
  - Build timestamp: 2026-06-20 03:24:43 local time.
- Player smoke: 24 seconds, stopped manually after startup.
  - Log: `Logs/waterline_shallow_pool_player_smoke_r2b.log`
  - Case-sensitive failure scan for `Error|Exception|Assert|NullReference|MissingReference|Failed|RenderGraph`: 0 matches.

Unity batch side effects will be reverted before staging. The authored implementation remains scoped to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`; review images are mirrored through the review/R2 flow rather than staged as source.

## Next

- Continue tuning water and shore detail where individual maps still share the same broad lake silhouette.
- Add more area-specific midground shoreline identity after the current water-surface breakup stabilizes.
- Resume bridge traversal and walkable-affordance checks after the current natural/water uplift strand is safely checkpointed.
