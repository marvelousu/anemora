# HD2D bridge support validation

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-14

## Investigation

- Followed the bridge traversal concern after the distant-area-landmark cycle. The existing route validation proved the F1-to-midpoint-to-F6 corridor was not blocked, but the bridge crossing still needed a guard that the authored floor support stayed real and colliding.
- A first support sample used physics raycasts against inactive review-area state and failed on bridge lead-in/join points even though the authored `PathOrFloor` bridge pieces existed. That approach was rejected as too dependent on active-area physics state for this editor validation.
- The stable contract is now object-based: the long F1-to-F6 bridge corridor and both bridge road joins must keep enabled colliders, `PathOrFloor` landmarks, expected path materials, and authored scale/height ranges. This complements the existing capsule route-clearance checks.

## Change

- Added `ValidateChapter1BridgeContinuousSupport(prefix)` to `ValidateChapter1BridgeTraversalScaffold`.
- The new guard validates:
  - `Current/Past_CentralPlaza_Chapter1_F1_To_F6_Path`
  - `Current/Past_CentralPlaza_Chapter1_F1_LeftBridgeRoadJoin`
  - `Current/Past_CentralPlaza_Chapter1_F1_RightBridgeRoadJoin`
- Kept the implementation inside `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`; no renderer features, runtime renderer settings, or asset pipeline contracts were changed.

## Verification

- Validate: `Logs/bridge_support_validate_r3.log` passed with return code 0.
- Renderer freeze: `Logs/bridge_support_editmode_r2.xml` passed 36/36 EditMode tests, including `RendererFeatureSet_MatchesFrozenBaseline`.
- Asset validation: `Logs/bridge_support_asset_validation_r1.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/bridge_support_capture_r1.log` produced the Cycle05 all-map Wide set in `docs/review/2026-06-14T21-24_bridge_support_validation/`.
- Shotdiff: `Logs/shotdiff/bridge_support_validation_vs_distant_area_landmarks` compared against `docs/review/2026-06-14T20-26_distant_area_landmarks`. The 13 individual review frames were unchanged at 0.0000-0.0001%; only `00_contact_sheet.png` exceeded budget because the locally regenerated sheet has slightly different text/layout rasterization.
- R2 review upload: `tools/r2/r2-upload-review.ps1 -CycleDir docs/review/2026-06-14T21-24_bridge_support_validation -Branch wip/hd2d-point15-recovery-20260612` uploaded 16 files and updated the branch manifest to 96 paths.
- Viewer propagation: `anemora-viewer` refresh commit `ef12675` triggered the Cloudflare Pages rebuild after the Anemora branch push did not immediately refresh the public page. Local viewer build passed and fetched `89/96` safe files for the wip branch. Public review page then showed `6 cycles / 84 images`, with `2026-06-14T21-24_bridge_support_validation` as the latest 14-image album.
- Public viewer URLs:
  - `https://anemora-viewer.pages.dev/wip-hd2d-point15-recovery-20260612/review/`
  - `https://anemora-viewer.pages.dev/wip-hd2d-point15-recovery-20260612/gallery/docs/review/2026-06-14T21-24_bridge_support_validation/`
  - `https://anemora-viewer.pages.dev/wip-hd2d-point15-recovery-20260612/docs/docs/devlog/2026-06-14_hd2d_bridge_support_validation/`
- Visual review: expected unchanged. This cycle is a bridge traversal validation hardening cycle, not a visible graphics pass.
- Side effects: Unity dirtied generated material/meta, `link.xml`, Volume, and screenshot outputs during validation/capture. Unintended generated asset changes must be reverted before staging; only the authored setup file, devlog/index, and review-cycle metadata are intended for this commit.
