# 2026-06-09 HD2D point15 time window visual physics probe

## Context

- Project: Anemora Fast VS House Slice HD-2D recovery.
- Branch: `wip/hd2d-point15-recovery-20260609`.
- Baseline: point15 `e7277f0a`.
- Slice: B time-window visual/physics diagnosis after back-transfer fix.
- Acceptance source: built-player capture only.
- Scope: data only; no time-window fix in this slice.
- Review folder: `docs/review/2026-06-09T12-49_time_window_visual_physics_probe/`.

## Probe

- Added `--anemora-house-slice-time-window-visual-physics-dir`.
- Captures current-side front placement, current-side far/back placement, current far-to-front CharacterController movement, current front-to-far CharacterController transfer, other-time far placement, and other-time far-to-front CharacterController movement.
- Logs portal camera player-layer masks, main camera player-layer visibility, player renderer visibility, current local coordinates, and generated wall collider counts.

## Build Evidence

- Log: `Logs/point15_timewindow_visual_physics_build_validate_20260609T124121.log`.
- `Fast VS house slice validation passed.`
- `DisplayProgressNotification: Build Successful`
- `Build Finished, Result: Success.`
- `PlayerBuildInfo duration=85786`

## Built-Player Evidence

- Player log: `Logs/point15_timewindow_visual_physics_player_20260609T124918.log`.
- `- Loaded All Assemblies, in  3.303 seconds`
- `ANEMORA_HOUSE_SLICE_TIME_WINDOW_VISUAL_PHYSICS: end count=6`

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Captures:

```text
01_current_front_before.png              868898 1280x720
02_current_far_side_before.png           872078 1280x720
03_current_far_to_front_after_cc.png    1024566 1280x720
04_current_front_to_far_after_cc.png    1050295 1280x720
05_other_time_far_before.png            1040268 1280x720
06_other_time_far_to_front_after_cc.png 1028904 1280x720
```

## Key Measurements

Open/current state:

```text
ANEMORA_HOUSE_SLICE_TIME_WINDOW_VISUAL_PHYSICS: open opened=True hasPair=True playerInOtherTime=False playerLocal=(19.900,0.020,15.350) portalLocal=(23.216,1.084,12.513) portalSize=(2.521,2.039) currentIncludesPlayer=False otherIncludesPlayer=True currentMask=268435456 otherMask=134217729 enabledWallColliders=0 playerRendererVisible=True playerViewport=(0.150,0.639,8.040) mainCameraMask=-268435457 mainCameraIncludesPlayerLayer=True
```

Current-side far/back placement:

```text
ANEMORA_HOUSE_SLICE_TIME_WINDOW_VISUAL_PHYSICS: current_far_side_before hasPair=True playerInOtherTime=False playerLocal=(23.216,0.720,13.233) portalLocal=(23.216,1.084,12.513) outsideRejected=False backRejected=False currentIncludesPlayer=False otherIncludesPlayer=True enabledWallColliders=0 playerRendererVisible=True playerViewport=(0.500,0.417,4.075) mainCameraMask=-268435457 mainCameraIncludesPlayerLayer=True
```

Current-side far/back to front movement with CharacterController:

```text
ANEMORA_HOUSE_SLICE_TIME_WINDOW_VISUAL_PHYSICS: current_far_to_front_after_cc expectedProbe=physicalPassCheck hasPair=True playerInOtherTime=False playerLocal=(23.216,0.556,11.983) portalLocal=(23.216,1.084,12.513) outsideRejected=False backRejected=False currentIncludesPlayer=False otherIncludesPlayer=True enabledWallColliders=0 playerRendererVisible=True playerViewport=(0.500,0.214,3.176) mainCameraMask=-268435457 mainCameraIncludesPlayerLayer=True
```

Current-side front to far movement with CharacterController:

```text
ANEMORA_HOUSE_SLICE_TIME_WINDOW_VISUAL_PHYSICS: current_front_to_far_after_cc expectedTransferMaybe hasPair=True playerInOtherTime=True playerLocal=(23.216,0.145,12.693) portalLocal=(23.216,1.084,12.513) outsideRejected=False backRejected=False lastTransition="Entered other-time space at matching local coordinate (23.22, 0.72, 12.69)." currentIncludesPlayer=True otherIncludesPlayer=False enabledWallColliders=5 playerRendererVisible=True mainCameraMask=-268435458 mainCameraIncludesPlayerLayer=False
```

Other-time far/back to front movement with CharacterController:

```text
ANEMORA_HOUSE_SLICE_TIME_WINDOW_VISUAL_PHYSICS: other_time_far_to_front_after_cc expectedReturnOrBlock hasPair=True playerInOtherTime=False playerLocal=(23.216,0.145,12.333) portalLocal=(23.216,1.084,12.513) outsideRejected=False backRejected=False lastTransition="Returned current space at matching local coordinate (23.22, 0.72, 12.33)." currentIncludesPlayer=False otherIncludesPlayer=True enabledWallColliders=0 playerRendererVisible=True mainCameraMask=-268435457 mainCameraIncludesPlayerLayer=True
```

## Findings

- The "player visible on the far side" report is reproduced in built-player data: current-side far/back placement has `playerRendererVisible=True` and `mainCameraIncludesPlayerLayer=True`.
- This visibility is not primarily from the current-to-other aperture texture: in the same state `currentIncludesPlayer=False`, while the real player remains visible to the main camera.
- The "can come to the front without getting caught" report is reproduced: current-side far/back to front movement changes local Z from `13.233` to `11.983`, remains `playerInOtherTime=False`, and has `enabledWallColliders=0`.
- Generated wall colliders are only enabled when `playerInOtherTime=True`; current-side back/far movement has no active wall collider.
- Front-side current-to-other transfer still works with CharacterController movement: after moving forward, `playerInOtherTime=True` and `lastTransition="Entered other-time space..."`.
- Other-time return still works with CharacterController movement: after moving backward, `playerInOtherTime=False` and `lastTransition="Returned current space..."`.
- The current portal frame rendered magenta in `01_current_front_before.png` and `02_current_far_side_before.png`; this is a separate material/reference issue to keep visible in review evidence.

## Viewer

- Propagate target: `work/chapter1-continuation-map-vs-20260524`.
- R2 upload: `uploaded 8 files for chapter1-continuation-map-vs-20260524/2026-06-09T12-49_time_window_visual_physics_probe (bucket TTL 45d); manifest now lists 81 paths`.
- Cloudflare deploy hook: `HookId=629936212 Status=200 Length=124`.
- Hook-only polling stayed stale for 18 polls: `257 cycles / 1209 images`, `hasTarget=False`.
- Git-triggered viewer rebuild commit: `8a0635b chore: refresh review content 2026-06-09 time-window-visual-physics`.
- Public review page after rebuild: `258 cycles / 1215 images`, album `2026-06-09T12-49_time_window_visual_physics_probe` present.
- Public album check: `6 images`; all thumbnails loaded with `complete=true`, `naturalWidth=512`, `naturalHeight=288`.

## Next Action

- Decide the intended physical contract before fixing:
  - If current-side back/far should be allowed to pass through, keep transfer behavior and address only player occlusion/visibility.
  - If current-side back/far should be blocked by the window, add a current-side physical/occlusion barrier instead of relying on the other-time generated wall volume.
- Any B fix must keep front transfer and other-time return measured in this probe.
