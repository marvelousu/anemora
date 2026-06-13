# 2026-06-09 HD2D point15 time window back passthrough fix

## Context

- Project: Anemora Fast VS House Slice HD-2D recovery.
- Branch: `wip/hd2d-point15-recovery-20260609`.
- Baseline: point15 `e7277f0a`.
- Slice: B time-window current-side back entry fix.
- Acceptance source: built-player only.
- Review folder: `docs/review/2026-06-09T09-39_time_window_back_passthrough_fix/`.

## Changes

- Removed current-side back-entry snapping from `TimeWindowPairedSpacePortalController.EvaluateCrossing()`.
- Changed `enableBackSideBlocking` default and setup serialization to `false` so scene regeneration does not reintroduce the snap path.
- Replaced editor validation contract from "back-side edge crossing must be rejected" to "current-side back entry must pass through without transfer or rejection."
- Kept front-side transfer and other-time return behavior unchanged.

## Build Evidence

- Log: `Logs/point15_timewindow_back_passthrough_probe_build_validate_20260609T092951.log`.
- `Fast VS house slice validation passed.`
- `DisplayProgressNotification: Build Successful`
- `Build Finished, Result: Success.`
- `PlayerBuildInfo duration=75117`

## Built-Player Evidence

- Player log: `Logs/point15_timewindow_back_passthrough_player_20260609T093727.log`.
- Captures:
  - `01_open_current_front.png`
  - `02_front_after.png`
  - `03_back_before.png`
  - `04_back_after.png`
  - `05_other_return_after.png`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Front-side transfer:

```text
ANEMORA_HOUSE_SLICE_TIME_WINDOW_PROBE: front_after mode=bypassLocal expectedTransfer=True hasPair=True playerInOtherTime=True playerLocal=(19.375,0.720,18.467) portalLocal=(19.872,1.039,18.287) portalSize=(2.410,1.949) outsideRejected=False backRejected=False lastTransition="Entered other-time space at matching local coordinate (19.87, 0.72, 18.47)." apertureLive=True apertureEnabled=1 currentIncludesPlayer=True otherIncludesPlayer=False currentMask=268435457 otherMask=134217728 playerLayer=0 playerVisibleLayer=0 suppressedRenderers=85 wallColliders=5 enabledWallColliders=5 wallSummary="segments=leftSide,rightSide,leftNearGap,rightNearGap,farBackWall, removedNearBackCap=True, colliders=5, centerLocal=(0.000,0.000,4.000), size=(2.850,2.389,8.000), margin=0.220, thickness=0.140, nearGapDepth=0.420, farBackRootZ=2.532, farBackSpaceZ=20.819, farBackFormula=max(minDepth=1.150, portalHeight=1.949*multiplier=1.120+padding=0.280), inward=+localZ"
```

Current-side back entry:

```text
ANEMORA_HOUSE_SLICE_TIME_WINDOW_PROBE: back_after mode=bypassLocal expectedTransfer=False expectedNoSnap=True measuredBlockPlane=18.667 hasPair=True playerInOtherTime=False playerLocal=(19.375,0.720,18.087) portalLocal=(19.872,1.039,18.287) portalSize=(2.410,1.949) outsideRejected=False backRejected=False lastTransition="Review: player placed in current local (19.87, 0.72, 18.91)." apertureLive=True apertureEnabled=1 currentIncludesPlayer=False otherIncludesPlayer=True currentMask=268435456 otherMask=134217729 playerLayer=0 playerVisibleLayer=0 suppressedRenderers=85 wallColliders=5 enabledWallColliders=0 wallSummary="segments=leftSide,rightSide,leftNearGap,rightNearGap,farBackWall, removedNearBackCap=True, colliders=5, centerLocal=(0.000,0.000,4.000), size=(2.850,2.389,8.000), margin=0.220, thickness=0.140, nearGapDepth=0.420, farBackRootZ=2.532, farBackSpaceZ=20.819, farBackFormula=max(minDepth=1.150, portalHeight=1.949*multiplier=1.120+padding=0.280), inward=+localZ"
```

Other-time return:

```text
ANEMORA_HOUSE_SLICE_TIME_WINDOW_PROBE: other_return_after mode=bypassLocal expectedReturnCurrent=True hasPair=True playerInOtherTime=False playerLocal=(19.375,0.720,18.107) portalLocal=(19.872,1.039,18.287) portalSize=(2.410,1.949) outsideRejected=False backRejected=False lastTransition="Returned current space at matching local coordinate (19.87, 0.72, 18.11)." apertureLive=True apertureEnabled=1 currentIncludesPlayer=False otherIncludesPlayer=True currentMask=268435456 otherMask=134217729 playerLayer=0 playerVisibleLayer=0 suppressedRenderers=85 wallColliders=5 enabledWallColliders=0 wallSummary="segments=leftSide,rightSide,leftNearGap,rightNearGap,farBackWall, removedNearBackCap=True, colliders=5, centerLocal=(0.000,0.000,4.000), size=(2.850,2.389,8.000), margin=0.220, thickness=0.140, nearGapDepth=0.420, farBackRootZ=2.532, farBackSpaceZ=20.819, farBackFormula=max(minDepth=1.150, portalHeight=1.949*multiplier=1.120+padding=0.280), inward=+localZ"
```

## Interpretation

- Front-side transfer remains correct: `playerInOtherTime=True`, no rejection.
- Current-side back entry now matches the target transfer contract: `playerInOtherTime=False`, `outsideRejected=False`, `backRejected=False`, and final `playerLocal.z=18.087`, which is past the portal plane `portalLocal.z=18.287`.
- Other-time return remains correct: `playerInOtherTime=False`, no rejection.
- Aperture player-mask values still need a separate visual pass for the user's "player visible on far side" report; this fix is limited to the transfer/snap contract.

## Viewer

- Propagate target: `work/chapter1-continuation-map-vs-20260524`.
- This cycle must be visible in anemora-viewer before the next implementation slice.
