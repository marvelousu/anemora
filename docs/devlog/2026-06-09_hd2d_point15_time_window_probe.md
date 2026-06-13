# 2026-06-09 HD2D point15 time window probe

## Context

- Project: Anemora Fast VS House Slice HD-2D recovery.
- Branch: `wip/hd2d-point15-recovery-20260609`.
- Baseline: point15 `e7277f0a`.
- Slice: B time-window direction / back-side behavior diagnosis.
- Acceptance source: built-player only.
- Review folder: `docs/review/2026-06-09T09-08_time_window_probe_bypass_baseline/`.
- Built player: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`.

## Probe Harness

- Added a review-only local movement helper on `TimeWindowPairedSpacePortalController` so the probe can bypass `CharacterController` sliding and directly exercise the existing `EvaluateCrossing()` logic across the portal local Z plane.
- First attempted probe used `CharacterController.Move`; measured local motion slid in X/Y rather than crossing local Z, so it was unsuitable as transfer-trigger evidence.
- The accepted probe uses `mode=bypassLocal` for front transfer, current-side back entry, and other-time return.

## Build Evidence

- Log: `Logs/point15_timewindow_probe_bypass_build_validate_20260609T085933.log`.
- `Fast VS house slice validation passed.`
- `DisplayProgressNotification: Build Successful`
- `Build Finished, Result: Success.`
- `PlayerBuildInfo duration=76693`

## Built-Player Evidence

- Player log: `Logs/point15_timewindow_probe_bypass_player_20260609T090728.log`.
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

Front-side transfer result:

```text
ANEMORA_HOUSE_SLICE_TIME_WINDOW_PROBE: front_after mode=bypassLocal expectedTransfer=True hasPair=True playerInOtherTime=True playerLocal=(19.375,0.720,18.467) portalLocal=(19.872,1.039,18.287) portalSize=(2.410,1.949) outsideRejected=False backRejected=False lastTransition="Entered other-time space at matching local coordinate (19.87, 0.72, 18.47)." apertureLive=True apertureEnabled=1 currentIncludesPlayer=True otherIncludesPlayer=False currentMask=268435457 otherMask=134217728 playerLayer=0 playerVisibleLayer=0 suppressedRenderers=85 wallColliders=5 enabledWallColliders=5 wallSummary="segments=leftSide,rightSide,leftNearGap,rightNearGap,farBackWall, removedNearBackCap=True, colliders=5, centerLocal=(0.000,0.000,4.000), size=(2.850,2.389,8.000), margin=0.220, thickness=0.140, nearGapDepth=0.420, farBackRootZ=2.532, farBackSpaceZ=20.819, farBackFormula=max(minDepth=1.150, portalHeight=1.949*multiplier=1.120+padding=0.280), inward=+localZ"
```

Current-side back entry result:

```text
ANEMORA_HOUSE_SLICE_TIME_WINDOW_PROBE: back_after mode=bypassLocal expectedTransfer=False expectedNoSnap=True measuredBlockPlane=18.667 hasPair=True playerInOtherTime=False playerLocal=(19.375,0.720,18.667) portalLocal=(19.872,1.039,18.287) portalSize=(2.410,1.949) outsideRejected=True backRejected=True lastTransition="Blocked current-side back entry at local (19.87, 0.72, 18.29)." apertureLive=True apertureEnabled=1 currentIncludesPlayer=False otherIncludesPlayer=True currentMask=268435456 otherMask=134217729 playerLayer=0 playerVisibleLayer=0 suppressedRenderers=85 wallColliders=5 enabledWallColliders=0 wallSummary="segments=leftSide,rightSide,leftNearGap,rightNearGap,farBackWall, removedNearBackCap=True, colliders=5, centerLocal=(0.000,0.000,4.000), size=(2.850,2.389,8.000), margin=0.220, thickness=0.140, nearGapDepth=0.420, farBackRootZ=2.532, farBackSpaceZ=20.819, farBackFormula=max(minDepth=1.150, portalHeight=1.949*multiplier=1.120+padding=0.280), inward=+localZ"
```

Other-time return result:

```text
ANEMORA_HOUSE_SLICE_TIME_WINDOW_PROBE: other_return_after mode=bypassLocal expectedReturnCurrent=True hasPair=True playerInOtherTime=False playerLocal=(19.375,0.720,18.107) portalLocal=(19.872,1.039,18.287) portalSize=(2.410,1.949) outsideRejected=False backRejected=False lastTransition="Returned current space at matching local coordinate (19.87, 0.72, 18.11)." apertureLive=True apertureEnabled=1 currentIncludesPlayer=False otherIncludesPlayer=True currentMask=268435456 otherMask=134217729 playerLayer=0 playerVisibleLayer=0 suppressedRenderers=85 wallColliders=5 enabledWallColliders=0 wallSummary="segments=leftSide,rightSide,leftNearGap,rightNearGap,farBackWall, removedNearBackCap=True, colliders=5, centerLocal=(0.000,0.000,4.000), size=(2.850,2.389,8.000), margin=0.220, thickness=0.140, nearGapDepth=0.420, farBackRootZ=2.532, farBackSpaceZ=20.819, farBackFormula=max(minDepth=1.150, portalHeight=1.949*multiplier=1.120+padding=0.280), inward=+localZ"
```

## Interpretation

- Direction gate itself is correct in point15: front-side local Z crossing enters other time, current-side back entry does not transfer, and other-time backward crossing returns to current time.
- The failing point is the current-side back block: the user-requested expectation is no transfer and no snap, but measured result is `outsideRejected=True`, `backRejected=True`, and `playerLocal.z` pinned to `measuredBlockPlane=18.667`.
- Aperture mask evidence also remains relevant for the user's "player visible on the far side" report:
  - current-side/open state: `currentIncludesPlayer=False`, `otherIncludesPlayer=True`
  - after front transfer: `currentIncludesPlayer=True`, `otherIncludesPlayer=False`
  - current-side back state: `currentIncludesPlayer=False`, `otherIncludesPlayer=True`

## Next Action

- Replace current-side back blocking with pass-through/no-transfer behavior instead of snap/block.
- Preserve front-side transfer and other-time return.
- Add a follow-up built-player probe after the fix with the same exact front/back/return assertions.
- Continue to propagate every built-player capture to anemora-viewer.
