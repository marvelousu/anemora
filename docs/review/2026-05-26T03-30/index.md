# Stage 7j Plaza Receiver Rebalance

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Build: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Launch note: launch the whole `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice` folder, not only the exe.

## Change

Stage7j reduces CentralPlaza realtime shadow darkness without removing realtime shadowing:

- CentralPlaza directional `shadowStrength`: near-black value to `0.92`.
- CentralPlaza realtime receiver `_ShadowReceiveStrength`: `0.86`.
- CentralPlaza receiver `_ShadowTextureStrength`: `0.50` on facade receivers, `0.46` on floor receivers.
- CentralPlaza side/floor shade colors are slightly lifted.
- Exterior and Library runtime values, route pads, map movement, cookie names, VS-like runtime camera, and TimeWindow setup were not targeted.

## Images

- [home.png](home.png)
- [plaza_01.png](plaza_01.png)
- [plaza_02_niro_in_shadow.png](plaza_02_niro_in_shadow.png)
- [library.png](library.png)
- [tw_current_aperture.png](tw_current_aperture.png)

## Verification

- `ValidateHd2dStage7PlazaReceiverRebalanceBatch`: exit 0, `Logs/stage7-plaza-receiver-rebalance-validate-single.log`
- `ValidateHouseSliceBatch`: exit 0, `Logs/stage7-plaza-receiver-rebalance-validate-full-gfx.log`
- `CaptureHd2dStage7PlazaReceiverRebalanceReferenceScreenshotsBatch`: exit 0, `Logs/stage7-plaza-receiver-rebalance-capture-gfx.log`
- Build: exit 0, `Logs/stage7-plaza-receiver-rebalance-build-gfx.log`, exe timestamp `2026-05-26 03:28:03`
- Smoke: killed after 20 seconds, error match count 0, `Logs/stage7-plaza-receiver-rebalance-smoke.log`
- `PortalStencilFeature`: active in `Assets/Settings/UniversalRenderPipeline_Renderer.asset`
- Stage7 TiltShift and Outline renderer features: active.
- TimeWindow aperture PNG was viewed and is not black.
- Scene grep before cleanup found Stage7 depth bands, Stage7 shadow lift objects, Stage7 APV current/past volumes, CentralPlaza separate spaces, and `TimeWindowPairedSpacePortalController`.
- `Assets/Scenes/Anemora_Chapter1.unity` was not present, so no Chapter1 APPLY/INTEGRATOR/REFRESH pipeline was touched.

## Gap Notes

The target standard is still largely insufficient.

- The plaza shadows are less crushed, but the broad silhouettes remain too large and too graphic.
- The facade and floor still read as flat planes with visible tiling; this does not approach the reference's layered material depth.
- The rebalance improves tonal readability more than composition; it does not create Octopath-level spatial hierarchy.
- The close plaza capture still shows heavy shadow shapes dominating the player lane.
- Library and exterior remain largely unchanged in this slice.
- The TimeWindow aperture remains functional, but its visual language is still far from the reference target.

## Status

Proceeding to the next implementation slice. No code or asset commit was made for Stage7j; this review artifact is the only committed output for this checkpoint.
