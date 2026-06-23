# HD2D portal depth and tree readability

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-22

## Summary

This cycle responds to build-review feedback that the Time Window interior image could render in front of the protagonist, and that outdoor nature still read too much like blockout/unclear foliage. The accepted review build changes the portal aperture overlay to respect scene depth and adds a generated, toned broadleaf tree-card texture for readable outdoor tree silhouettes.

Review packet: `docs/review/2026-06-22T16-45_portal_depth_tree_readability_r1/`.

## Implementation

- Changed `Assets/Art/Materials/Portal/PortalApertureOverlay.shader` from `ZTest Always` to `ZTest LEqual` so nearer geometry, including the player, can occlude the Time Window interior image while preserving the opaque aperture composite.
- Updated `ValidateApertureOpaqueComposite` to reject `ZTest Always` and require the depth-aware aperture contract.
- Added generated current/past readable tree sprite textures:
  - `Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_current_readable_tree_sprite_cc0_toned.asset`
  - `Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_past_readable_tree_sprite_cc0_toned.asset`
- Routed newly added readable tree overlays and the house-exterior tree-crown crop panels through the toned tree texture path.
- Raised the distant natural canopy shadow palette so tree-depth pockets read as deep foliage rather than pure black.
- Kept existing route pads, colliders, map anchors, renderer features, and placement coordinates unchanged.

## Visual Review

- All 13 all-map captures were refreshed in `docs/devlog/screenshots/chapter1_all_maps_cycle05/`.
- The captures were copied to `docs/review/2026-06-22T16-45_portal_depth_tree_readability_r1/`.
- Contact sheet: `docs/review/2026-06-22T16-45_portal_depth_tree_readability_r1/00_contact_sheet.png`.
- Manual review notes:
  - `01_a1_a2_current.png` and `03_b1_b3_current.png` now show clearer authored tree crowns with less fluorescent green.
  - `06_c1_c3_past.png` remains visually stable after the tree texture change.
  - There are still strong black tree-shadow shapes in some outdoor frames; these should be the next nature-quality target rather than hidden by color polish.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Assets/Art/Materials/Portal/PortalApertureOverlay.shader`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed.
  - Final log: `Logs/portal_depth_tree_readability_validate_r7.log`
  - `Fast VS house slice validation passed.`
- EditMode renderer freeze: passed.
  - XML: `Logs/portal_depth_tree_readability_editmode_r3.xml`
  - Result: 36 passed / 0 failed / 0 skipped.
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed.
  - Note: `-runTests` was executed without `-quit` so Unity emitted the XML test result.
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed.
  - Log: `Logs/portal_depth_tree_readability_asset_validation_r2.log`
  - `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
  - Final log: `Logs/portal_depth_tree_readability_capture_r5.log`
  - Output: `docs/devlog/screenshots/chapter1_all_maps_cycle05/`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed.
  - Log: `Logs/portal_depth_tree_readability_build_r1.log`
  - Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
  - Full path: `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
  - Build timestamp: 2026-06-22 17:07:31 local time.
  - Build size: 667648 bytes.
- Player smoke: 24 seconds, stopped after startup.
  - Log: `Logs/portal_depth_tree_readability_player_smoke_r1.log`
  - Failure scan for `error CS|Exception|Assert|NullReference|MissingReference|Failed|Crash|Fatal`: 0 matches.

## Next

- Replace the remaining heavy black foliage/shadow silhouettes with authored tree forms or imported natural assets rather than continuing small color-only polish.
- Add a player/portal close-view capture that directly proves the Time Window interior image is behind the protagonist depth in the built player.
- Continue the nature pass with real tree-model assets or higher-quality generated broadleaf/conifer meshes, then rerun all-map review and built-player capture.
