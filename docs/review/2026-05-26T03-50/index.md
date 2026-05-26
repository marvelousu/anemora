# Stage 7k Plaza Caster Silhouette Trim

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Build: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Launch note: launch the whole `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice` folder, not only the exe.

## Change

Stage7k trims oversized CentralPlaza realtime shadow caster silhouettes without removing realtime shadowing:

- Reduced rewritten caster mesh sizes for eave, canopy, foreground rafter, foreground cross, and door lintel casters.
- Reduced broad cycle140 mesh caster sizes for eave plates and foreground ragged caster plates.
- Kept caster object names, counts, `ShadowsOnly` policy, realtime cookie use, runtime VS-like camera, route pads, and TimeWindow setup intact.

## Images

- [home.png](home.png)
- [plaza_01.png](plaza_01.png)
- [plaza_02_niro_in_shadow.png](plaza_02_niro_in_shadow.png)
- [library.png](library.png)
- [tw_current_aperture.png](tw_current_aperture.png)

## Verification

- `ValidateHd2dStage7PlazaCasterSilhouetteTrimBatch`: exit 0, `Logs/stage7-plaza-caster-trim-validate-single.log`
- `ValidateHouseSliceBatch`: exit 0, `Logs/stage7-plaza-caster-trim-validate-full-gfx.log`
- `CaptureHd2dStage7PlazaCasterSilhouetteTrimReferenceScreenshotsBatch`: exit 0, `Logs/stage7-plaza-caster-trim-capture-gfx.log`
- Build: exit 0, `Logs/stage7-plaza-caster-trim-build-gfx.log`, exe timestamp `2026-05-26 03:48:11`
- Smoke: killed after 20 seconds, error match count 0, `Logs/stage7-plaza-caster-trim-smoke.log`
- `PortalStencilFeature`: active in `Assets/Settings/UniversalRenderPipeline_Renderer.asset`
- Stage7 TiltShift and Outline renderer features: active.
- TimeWindow aperture PNG was viewed and is not black.
- Scene grep before cleanup found Stage7 depth bands, Stage7 shadow lift objects, Stage7 APV current/past volumes, CentralPlaza separate spaces, `TimeWindowPairedSpacePortalController`, and a trimmed cycle140 foreground caster mesh.
- `Assets/Scenes/Anemora_Chapter1.unity` was not present, so no Chapter1 APPLY/INTEGRATOR/REFRESH pipeline was touched.

## Gap Notes

The target standard is still largely insufficient.

- Some plaza shadow silhouettes are smaller, but the right foreground and player-lane shadow language still dominates the composition.
- The plaza still reads as tiled planes under shadow rather than layered HD-2D diorama geometry.
- This trims shadow area but does not add convincing material depth, baked bounce, or hand-painted spatial hierarchy.
- The facade remains flat and repetitive; the reference images have clearer atmospheric depth and surface richness.
- The TimeWindow aperture remains functional, but visually it is still disconnected from the target reference language.

## Status

Proceeding to the next implementation slice. No code or asset commit was made for Stage7k; this review artifact is the only committed output for this checkpoint.
