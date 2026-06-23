# HD2D textured nature and aperture depth

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-23

## Summary

This cycle responds to build-review feedback that the Time Window aperture interior could appear in front of the protagonist and that the outdoor trees/nature were still too primitive to read as authored environment art. The review build keeps the renderer-feature contract frozen, moves the aperture composite onto a depth-aware AlphaTest queue, and replaces the previous blockout-like nature companions with a CC0 textured tree subset plus darker deterministic grass, plant, bush, and ground-cover companions.

Review packet: `docs/review/2026-06-23T07-56_textured_nature_aperture_depth_r1/`.

Latest review build:

`C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Implementation

- Changed `Assets/Art/Materials/Portal/PortalApertureOverlay.shader` to use `Queue` = `AlphaTest` and `ZTest LEqual`, preserving the opaque aperture composite while allowing the player and closer scene geometry to occlude the Time Window interior image.
- Moved the default aperture plane offset slightly behind the frame in `Assets/Scripts/TimeManagement/TimeWindowPairedSpacePortalController.cs`.
- Imported a small CC0 textured tree subset under `Assets/Art/External/CC0/TexturedNaturePack/` with provenance in `SOURCE.md`.
- Added deterministic textured tree selection for current/past maps, favoring readable conifer and birch silhouettes over simple round blobs.
- Added textured nature materials named under `FastVS_House_ch1_textured_nature_*`, with darker current/past leaf palettes and separate bark/leaf material routing.
- Added deterministic 3D undergrowth companions around each tree: grass clusters, plant clusters, bushes, fallen wood, moss rocks, and authored ground-cover patches.
- Toned down the fallback readable tree-card overlay so it supports the model silhouettes instead of dominating them.
- Kept existing map placement coordinates, route pads, colliders, renderer features, and area anchors unchanged.

## Visual Review

- All 13 all-map captures were refreshed in `docs/devlog/screenshots/chapter1_all_maps_cycle05/`.
- Aperture close-view captures were kept in `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260525_stage7s_aperture_frame_blend\`.
- The review packet copies the all-map captures plus the aperture close-view evidence for quick built-player review.
- Manual review notes:
  - `03_b1_b3_current.png` now shows darker, more authored tree forms and undergrowth around the library/plaza edge.
  - `07_d1_d3_current.png` and `09_e1_e3_current.png` show a more readable conifer/birch silhouette mix than the previous primitive foliage.
  - The Time Window aperture close view keeps the protagonist/frame in front of the aperture image after the depth fix.
  - The nature pass is visibly improved, but distant panorama/water remain stylized and should be the next quality target after this review build.

## Verification

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Assets/Scripts/TimeManagement/TimeWindowPairedSpacePortalController.cs Assets/Art/Materials/Portal/PortalApertureOverlay.shader`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: passed.
  - Final log: `Logs/nature_textured_trees_validate_r4.log`
  - `Fast VS house slice validation passed.`
- EditMode renderer freeze: passed.
  - XML: `Logs/nature_textured_trees_editmode_r2b.xml`
  - Result: 36 passed / 0 failed / 0 skipped.
  - `RendererFeatureSet_MatchesFrozenBaseline`: Passed.
  - Note: `-runTests` must be executed without `-quit` for Unity to emit the XML result in this environment.
- `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`: passed.
  - Log: `Logs/nature_textured_trees_assets_r2.log`
  - `[AssetValidation] OK`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
  - Final log: `Logs/nature_textured_trees_capture_r4.log`
  - Output: `docs/devlog/screenshots/chapter1_all_maps_cycle05/`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: passed.
  - Log: `Logs/nature_textured_trees_build_r2.log`
  - Full path: `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
  - Build timestamp: 2026-06-23 07:53:33 local time.
  - Build size: 667648 bytes.
- Player smoke: 20 seconds, stopped after startup.
  - Log: `Logs/nature_textured_trees_player_smoke_r2.log`
  - Filtered failure scan: `SMOKE_REAL_HITS=0`.
  - Renderer contract line still reports `error=<none>` with the frozen four-feature set.

## Next

- Improve the distant panorama and broad water surface with higher-quality authored landform/water materials rather than continuing small color polish.
- Continue replacing the remaining stylized tree-card/foliage shadows with real geometry or higher-quality generated vegetation assets.
- Add another close-view review packet after the next nature pass so the aperture depth and vegetation readability stay checked together.
