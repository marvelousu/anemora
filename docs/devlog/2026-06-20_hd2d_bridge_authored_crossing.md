# HD2D bridge authored crossing

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-20

## Summary

This cycle closes the current bridge uncertainty on the latest build and improves the F1-F6 bridge read without changing the proven traversal lane. A pre-change built-player reproof already showed current/past bridge traversal still passes on the shallow-water build; this pass then adds non-blocking authored bridge detail so the crossing reads less like a bare box plank and more like an assembled bridge with rails, side stringers, ties, wear strips, abutment caps, posts, braces, and distinct current/past repair-state cues.

The accepted packet is `docs/review/2026-06-20T04-05_bridge_authored_crossing_r1/`. It contains all 13 all-map captures plus six built-player bridge proof frames from the new build.

## Implementation

- Added `CreateRuinsBridgeAuthoredCrossingDetails` in the existing F1-F6 bridge generation path.
- Added non-colliding side stringers, under-beams, abutment caps, cross ties, deck wear strips, side posts, and diagonal side braces.
- Added current-only missing-slat/splinter cues and past-only repair lashings so the two time states stay visually distinct.
- Kept `BridgeOpenWalkLine`, thresholds, midpoint pier, road joins, and `F1_To_F6_Path` traversal colliders unchanged.
- Added `ValidateChapter1BridgeAuthoredCrossingDetails` and `ValidateChapter1BridgeNonBlockingDetail` so the new authored details must exist, remain visible, and stay collider-free.
- Kept renderer features frozen and avoided any procedural placement randomness.

## Visual Review

- Review packet: `docs/review/2026-06-20T04-05_bridge_authored_crossing_r1/`
- Contact sheet: `docs/review/2026-06-20T04-05_bridge_authored_crossing_r1/00_contact_sheet.png`
- All 13 all-map captures were refreshed from `docs/devlog/screenshots/chapter1_all_maps_cycle05/` and copied into the review packet.
- Built-player bridge proof frames were copied into the same packet as `bridge_current_*.png` and `bridge_past_*.png`.
- Shotdiff vs `docs/review/2026-06-20T03-27_waterline_shallow_pool_r1/`: only `11_f1_f6_current.png` and `12_f1_f6_past.png` changed over the 0.05% review threshold. Current moved 0.0997%; past moved 0.1059%; the other 11 frames remained unchanged.
- Representative checks:
  - `11_f1_f6_current.png`: F map gains visible bridge rail/stringer density while preserving the route and waterline work.
  - `bridge_current_02_midspan.png`: player stands on the bridge center with new side posts and diagonal braces visible, no blocking/occlusion of the lane.
  - `bridge_past_02_midspan.png`: past-side bridge keeps the same traversal lane and adds repair lashing cues.
  - `13_scene6_sideview_auto.png`: side-view stability frame remains unchanged.

## Verification

- `git diff --check`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed.
  - Log: `Logs/bridge_authored_crossing_validate_r1.log`
  - `Fast VS house slice validation passed.`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
  - Log: `Logs/bridge_authored_crossing_capture_r1.log`
  - Output copied into `docs/review/2026-06-20T04-05_bridge_authored_crossing_r1/`.
- Shotdiff triage:
  - Summary: `Logs/bridge_authored_crossing_shotdiff_r1.txt`
  - Diff output: `Logs/shotdiff/bridge_authored_crossing_r1/diff/`
  - Accepted because only the intended F map frames changed over threshold.
- EditMode renderer freeze: passed, 36/36.
  - XML: `Logs/bridge_authored_crossing_editmode_r1.xml`
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed.
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed.
  - Log: `Logs/bridge_authored_crossing_asset_validation_r1.log`
  - `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed.
  - Log: `Logs/bridge_authored_crossing_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
  - Build timestamp: 2026-06-20 04:20:02 local time.
- Player smoke: 24 seconds, stopped manually after startup.
  - Log: `Logs/bridge_authored_crossing_player_smoke_r1.log`
  - Case-sensitive failure scan for `Error|Exception|Assert|NullReference|MissingReference|Failed|RenderGraph`: 0 matches.
- Built-player bridge traversal proof: passed on the new build.
  - Log: `Logs/bridge_authored_crossing_bridge_proof_player_r1.log`
  - Frame output: `Logs/bridge_authored_crossing_bridge_proof_frames_r1/`
  - Current final local `(88.79, 0.12, 15.95)`, delta `0.115`.
  - Past final local `(88.79, 0.12, 15.95)`, delta `0.115`.

Unity batch side effects were reverted before staging. The authored implementation remains scoped to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`; review images are mirrored through the review/R2 flow rather than staged as source.

## Next

- Return to the nature/distant-vista strand and keep raising realism where the broad lake and forest silhouettes still read too synthetic.
- Consider one later bridge pass for route-adjacent ground contact shadows and bank vegetation after the natural/water material direction stabilizes.
