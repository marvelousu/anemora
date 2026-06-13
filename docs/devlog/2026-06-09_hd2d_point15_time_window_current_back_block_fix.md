# 2026-06-09 HD2D point15 time window current back block fix

## Context

- Project: Anemora Fast VS House Slice HD-2D recovery.
- Branch: `wip/hd2d-point15-recovery-20260609`.
- Baseline: point15 `e7277f0a`.
- Slice: B time-window current-side back/far physical block after built-player visual/physics probe.
- Acceptance source: built-player capture only.
- Review folder: `docs/review/2026-06-09T14-07_time_window_current_back_block_fix/`.
- Prior failed diagnostic folder kept local: `docs/review/2026-06-09T13-48_time_window_current_back_block_fix/`.

## Change

- Added a current-side, back/far physical blocker for the Time Window while the player is in current time.
- Disabled the older `enableBackSideBlocking` snap/rejection path by default; it had been the wrong shape for the reported bug because it could force player placement instead of acting as world collision.
- Kept the other-time generated wall volume enabled only while `playerInOtherTime=True`.
- Added `MovePlayerLocalForReview(...)` so built-player probes can use CharacterController movement instead of transform teleport when measuring the actual crossing behavior.
- Fixed the first attempt's visual regression: invisible collider helpers now create `new GameObject + BoxCollider` directly instead of `GameObject.CreatePrimitive(Cube)` followed by delayed `Destroy(renderer)`. In built-player, delayed renderer destruction produced a one-frame magenta blocker plate in captures `01/02`.

## Build Evidence

- Build/validation log: `Logs/point15_timewindow_invisible_collider_build_validate_20260609T135235.log`.
- `Build Finished, Result: Success.`
- `PlayerBuildInfo duration=219935`
- `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260609\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- `Exiting without the bug reporter. Application will terminate with return code 0`

## Built-Player Evidence

- Player log: `Logs/point15_timewindow_current_back_block_player_20260609T140352.log`.
- `- Loaded All Assemblies, in  4.549 seconds`
- `ANEMORA_HOUSE_SLICE_TIME_WINDOW_VISUAL_PHYSICS: end count=6`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Captures:

```text
01_current_front_before.png             1019765 bytes 1280x720
02_current_far_side_before.png          1021294 bytes 1280x720
03_current_far_to_front_after_cc.png    1021252 bytes 1280x720
04_current_front_to_far_after_cc.png    1047575 bytes 1280x720
05_other_time_far_before.png            1048179 bytes 1280x720
06_other_time_far_to_front_after_cc.png 1025473 bytes 1280x720
```

## Measurements

Current front placement:

```text
ANEMORA_HOUSE_SLICE_TIME_WINDOW_VISUAL_PHYSICS: current_front_before hasPair=True playerInOtherTime=False playerLocal=(23.216,0.720,11.963) portalLocal=(23.216,1.084,12.513) portalSize=(2.521,2.039) outsideRejected=False backRejected=False lastTransition="Review: player placed in current local (23.22, 0.72, 11.96)." apertureLive=True apertureEnabled=1 currentIncludesPlayer=False otherIncludesPlayer=True currentMask=268435456 otherMask=134217729 playerLayer=0 playerVisibleLayer=0 suppressedRenderers=11 currentBackColliders=1 enabledCurrentBackColliders=1 wallColliders=5 enabledWallColliders=0 playerRendererVisible=True mainCameraMask=-268435457 mainCameraIncludesPlayerLayer=True
```

Current back/far placement:

```text
ANEMORA_HOUSE_SLICE_TIME_WINDOW_VISUAL_PHYSICS: current_far_side_before hasPair=True playerInOtherTime=False playerLocal=(23.216,0.720,13.233) portalLocal=(23.216,1.084,12.513) portalSize=(2.521,2.039) outsideRejected=False backRejected=False lastTransition="Review: player placed in current local (23.22, 0.72, 13.23)." apertureLive=True apertureEnabled=1 currentIncludesPlayer=False otherIncludesPlayer=True currentMask=268435456 otherMask=134217729 playerLayer=0 playerVisibleLayer=0 suppressedRenderers=11 currentBackColliders=1 enabledCurrentBackColliders=1 wallColliders=5 enabledWallColliders=0 playerRendererVisible=True mainCameraMask=-268435457 mainCameraIncludesPlayerLayer=True
```

Current back/far to front with CharacterController:

```text
ANEMORA_HOUSE_SLICE_TIME_WINDOW_VISUAL_PHYSICS: current_far_to_front_after_cc expectedProbe=currentBackPhysicalBlock hasPair=True playerInOtherTime=False playerLocal=(23.216,0.556,13.233) portalLocal=(23.216,1.084,12.513) portalSize=(2.521,2.039) outsideRejected=False backRejected=False lastTransition="Review: player placed in current local (23.22, 0.72, 13.23)." apertureLive=True apertureEnabled=1 currentIncludesPlayer=False otherIncludesPlayer=True currentMask=268435456 otherMask=134217729 playerLayer=0 playerVisibleLayer=0 suppressedRenderers=11 currentBackColliders=1 enabledCurrentBackColliders=1 wallColliders=5 enabledWallColliders=0 playerRendererVisible=True mainCameraMask=-268435457 mainCameraIncludesPlayerLayer=True
```

Current front to far with CharacterController:

```text
ANEMORA_HOUSE_SLICE_TIME_WINDOW_VISUAL_PHYSICS: current_front_to_far_after_cc expectedTransferMaybe hasPair=True playerInOtherTime=True playerLocal=(23.216,0.145,12.693) portalLocal=(23.216,1.084,12.513) portalSize=(2.521,2.039) outsideRejected=False backRejected=False lastTransition="Entered other-time space at matching local coordinate (23.22, 0.72, 12.69)." apertureLive=True apertureEnabled=1 currentIncludesPlayer=True otherIncludesPlayer=False currentMask=268435457 otherMask=134217728 playerLayer=0 playerVisibleLayer=0 suppressedRenderers=11 currentBackColliders=1 enabledCurrentBackColliders=0 wallColliders=5 enabledWallColliders=5 playerRendererVisible=True mainCameraMask=-268435458 mainCameraIncludesPlayerLayer=False
```

Other-time far to front return:

```text
ANEMORA_HOUSE_SLICE_TIME_WINDOW_VISUAL_PHYSICS: other_time_far_to_front_after_cc expectedReturnOrBlock hasPair=True playerInOtherTime=False playerLocal=(23.216,0.145,12.333) portalLocal=(23.216,1.084,12.513) portalSize=(2.521,2.039) outsideRejected=False backRejected=False lastTransition="Returned current space at matching local coordinate (23.22, 0.72, 12.33)." apertureLive=True apertureEnabled=1 currentIncludesPlayer=False otherIncludesPlayer=True currentMask=268435456 otherMask=134217729 playerLayer=0 playerVisibleLayer=0 suppressedRenderers=11 currentBackColliders=1 enabledCurrentBackColliders=1 wallColliders=5 enabledWallColliders=0 playerRendererVisible=True mainCameraMask=-268435457 mainCameraIncludesPlayerLayer=True
```

## Magenta Regression Check

The first implementation used primitive cubes for invisible blockers and removed their renderers with `Destroy(renderer)`. Built-player captures showed the delayed renderer removal:

```text
docs/review/2026-06-09T13-48_time_window_current_back_block_fix/01_current_front_before.png magentaPixels=149356 pct=16.206163
docs/review/2026-06-09T14-07_time_window_current_back_block_fix/01_current_front_before.png magentaPixels=0 pct=0.000000
docs/review/2026-06-09T13-48_time_window_current_back_block_fix/02_current_far_side_before.png magentaPixels=149356 pct=16.206163
docs/review/2026-06-09T14-07_time_window_current_back_block_fix/02_current_far_side_before.png magentaPixels=0 pct=0.000000
```

## Result

- Back/far current-side entry no longer reaches the front side through the window: `current_far_to_front_after_cc` remains at `playerLocal.z=13.233`, keeps `playerInOtherTime=False`, and has `currentBackColliders=1 enabledCurrentBackColliders=1`.
- Front current-side crossing still transfers: `current_front_to_far_after_cc` reports `playerInOtherTime=True` and `lastTransition="Entered other-time space at matching local coordinate (23.22, 0.72, 12.69)."`.
- Other-time return still works: `other_time_far_to_front_after_cc` reports `playerInOtherTime=False` and `lastTransition="Returned current space at matching local coordinate (23.22, 0.72, 12.33)."`.
- The magenta blocker plate caused by delayed renderer destruction is gone in the accepted built-player capture.

## Viewer

- Propagate target: `work/chapter1-continuation-map-vs-20260524`.
- R2 upload: `uploaded 8 files for chapter1-continuation-map-vs-20260524/2026-06-09T14-07_time_window_current_back_block_fix (bucket TTL 45d); manifest now lists 89 paths`.
- Git-triggered viewer rebuild commit: `522d716 chore: refresh review content 2026-06-09 time-window-current-back-block`.
- Public review polling: polls 1-21 stayed at `258 cycles · 1215 images`, `hasTarget=False`; poll 22 returned `259 cycles · 1221 images`, `hasTarget=True`, `length=400048`.
- Public review page: `https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/review/`.
- Public review DOM check: title `Review — work/chapter1-continuation-map-vs-20260524`; `has259=True`; `has1221=True`; target album present.
- Public latest thumbnail: `thumbs/chapter1-continuation-map-vs-20260524/docs/review/2026-06-09T14-07_time_window_current_back_block_fix/01_current_front_before.webp`, `complete=True`, `naturalWidth=512`, `naturalHeight=288`.
- Public album page: `https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/gallery/docs/review/2026-06-09T14-07_time_window_current_back_block_fix/`.
- Public album DOM check: `imageCount=6`, `allLoaded=True`; all six images loaded with `naturalWidth=512`, `naturalHeight=288`.
- Public devlog initial DOM check before this final viewer section: title `docs/devlog/2026-06-09_hd2d_point15_time_window_current_back_block_fix.md — Docs`, `textLength=75061`; found `PlayerBuildInfo duration=219935`, `magentaPixels=0 pct=0.000000`, `current_far_to_front_after_cc`, and `currentBackColliders=1 enabledCurrentBackColliders=1`.
- Public recheck after viewer-staleness report, direct HTML with cache-buster: review `status=200`, `length=400048`, `hasTarget=True`, `has259=True`, `has1221=True`, `has258=False`, `has1215=False`.
- Public recheck after viewer-staleness report, album HTML with cache-buster: `status=200`, `length=20446`, `hasTitle=True`, `count01=True`, `count06=True`, `hasDevlog=True`.
- Public recheck after viewer-staleness report, devlog HTML with cache-buster: `status=200`, `length=1014335`, `hasFinal=True`, `hasPending=False`, `has259=True`, `hasCommit522=True`, `hasDuration=True`, `hasMagentaZero=True`.
- Browser-rendered review recheck: title `Review -- work/chapter1-continuation-map-vs-20260524`, `has259=True`, `has1221=True`, target album present, first target thumbnail `complete=True`, `naturalWidth=512`, `naturalHeight=288`.
- Browser-rendered album recheck: title `docs/review/2026-06-09T14-07_time_window_current_back_block_fix -- Gallery`, `imageCount=6`, `allLoaded=True`; each image reports `naturalWidth=512`, `naturalHeight=288`.
- Browser-rendered devlog recheck: title `docs/devlog/2026-06-09_hd2d_point15_time_window_current_back_block_fix.md -- Docs`, `textLength=76587`, `hasFinal=True`, `hasPending=False`, `has259=True`, `has1221=True`, `hasDuration=True`, `hasMagentaZero=True`, `hasCurrentBack=True`.
