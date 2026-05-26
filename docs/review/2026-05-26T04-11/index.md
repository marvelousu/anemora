# Stage 7l Surface Breakup Shader

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Build: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Launch note: launch the whole `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice` folder, not only the exe.

## Change

Stage7l adds a bounded world-space surface breakup multiplier inside `FastVS_SurfaceRampLit.shader`:

- Added coarse/fine hash-based world-space breakup for floor and wall-facing fragments.
- Bounded the grade to subtle values around neutral so it cannot become a new heavy pattern layer.
- Kept realtime shadows, textured shadow tinting, cookies, emission, runtime VS-like camera, route pads, and TimeWindow setup intact.
- Added Stage7l validate and capture editor batch methods.

## Images

- [home.png](home.png)
- [plaza_01.png](plaza_01.png)
- [plaza_02_niro_in_shadow.png](plaza_02_niro_in_shadow.png)
- [library.png](library.png)
- [tw_current_aperture.png](tw_current_aperture.png)

## Verification

- `ValidateHd2dStage7SurfaceBreakupShaderBatch`: exit 0, `Logs/stage7-surface-breakup-shader-validate-single.log`
- `ValidateHouseSliceBatch`: exit 0, `Logs/stage7-surface-breakup-shader-validate-full-gfx.log`
- `CaptureHd2dStage7SurfaceBreakupShaderReferenceScreenshotsBatch`: exit 0, `Logs/stage7-surface-breakup-shader-capture-gfx.log`
- Build: exit 0, `Logs/stage7-surface-breakup-shader-build-gfx.log`, exe timestamp `2026-05-26 04:07:49`
- Build log note: Unity emitted a non-fatal caching client `move_path failed` line, but the build command returned exit 0 and the exe timestamp updated.
- Smoke: killed after 20 seconds, error match count 0, `Logs/stage7-surface-breakup-shader-smoke.log`
- `PortalStencilFeature`: active in `Assets/Settings/UniversalRenderPipeline_Renderer.asset`
- Stage7 TiltShift and Outline renderer features: active.
- TimeWindow aperture PNG was viewed and is not black.
- Scene grep before cleanup found Stage7 depth bands, Stage7 shadow lift objects, Stage7 APV current/past volumes, CentralPlaza separate spaces, and `TimeWindowPairedSpacePortalController`.
- `Assets/Scenes/Anemora_Chapter1.unity` was not present, so no Chapter1 APPLY/INTEGRATOR/REFRESH pipeline was touched.

## Gap Notes

The target standard is still largely insufficient.

- The surface breakup is extremely subtle in the captured plaza views; it does not materially change the read from flat tiled planes.
- Large realtime shadow silhouettes still dominate the plaza composition.
- The wall/floor relationship is still too planar compared with the layered reference diorama look.
- Material richness remains far below the reference images; this shader pass does not replace painted albedo variation, geometry relief, baked bounce, or atmospheric depth.
- The TimeWindow aperture remains functional, but it still does not carry reference-level lighting or composition.

## Status

Proceeding to the next implementation slice. No code or asset commit was made for Stage7l; this review artifact is the only committed output for this checkpoint.
