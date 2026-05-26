# Stage 7i Plaza Shadow Lift

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Build: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Launch note: launch the whole `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice` folder, not only the exe.

## Change

Stage7i adds three local plaza shadow-lift overlay planes in the CentralPlaza current/past setup:

- `Current_FastVS_HD2D_Stage7_CentralPlaza_ShadowLiftForegroundA`
- `Current_FastVS_HD2D_Stage7_CentralPlaza_ShadowLiftMidgroundA`
- `Past_FastVS_HD2D_Stage7_CentralPlaza_ShadowLiftMidgroundA`

The change intentionally reuses existing plaza sun exposure / shadow midtone materials. No new decorative material family was introduced for this slice.

## Images

- [home.png](home.png)
- [library.png](library.png)
- [tw_current_aperture.png](tw_current_aperture.png)

## Verification

- `ValidateHd2dStage7PlazaShadowLiftBatch`: exit 0, `Logs/stage7-plaza-shadow-lift-validate-single.log`
- `ValidateHouseSliceBatch`: exit 0, `Logs/stage7-plaza-shadow-lift-validate-full-gfx.log`
- `CaptureHd2dStage7PlazaShadowLiftReferenceScreenshotsBatch`: exit 0, `Logs/stage7-plaza-shadow-lift-capture-gfx.log`
- Build: exit 0, `Logs/stage7-plaza-shadow-lift-build-gfx.log`, exe timestamp `2026-05-26 03:01:14`
- Smoke: killed after 20 seconds, error match count 0, `Logs/stage7-plaza-shadow-lift-smoke.log`
- `PortalStencilFeature`: active in `Assets/Settings/UniversalRenderPipeline_Renderer.asset`
- TimeWindow aperture PNG was viewed and is not black.
- Scene grep before cleanup found Stage7i shadow-lift objects, Stage7 APV current/past volumes, CentralPlaza separate spaces, and `TimeWindowPairedSpacePortalController`.
- `Assets/Scenes/Anemora_Chapter1.unity` was not present, so no Chapter1 APPLY/INTEGRATOR/REFRESH pipeline was touched.

## Gap Notes

The target standard is still largely insufficient.

- The plaza still reads as a flat wall/floor composition, not Octopath-level layered diorama space.
- The largest black shadow masses remain dominant; this slice only lifts selected local receiver zones.
- Material breakup is still too procedural and tile-like in the plaza.
- The added lift is subtle in wide capture and does not solve the overall lighting hierarchy.
- Library warm light remains a local improvement only; the global night-camp reference quality is still not approached.
- The TimeWindow aperture is functional, but its orange frame and flat portal read remain far from the reference image language.

## Status

Proceeding to the next implementation slice. No code or asset commit was made for Stage7i; this review artifact is the only committed output for this checkpoint.
