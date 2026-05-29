# Fast VS HD-2D Phase A Gate Review

Date: 2026-05-28

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Build exe:
`Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`

Run note: フォルダごと起動

## Scope

This is the corrected Phase A gate review set for `docs/HD2D_SUN_CYCLE_SPEC.md` §8.

It includes the required five area captures:
Home, HomeOutside, Plaza, Plaza_NiroInShadow, and Library.
The TimeWindow aperture capture is included as an additional Phase A check.

This public review set intentionally contains only current project captures. External reference images, side-by-side comparison boards, and diagnostic black-screen captures are not included in `docs/review`.

## Captures

1. [Home](01_home.png)
2. [HomeOutside](02_home_outside.png)
3. [Plaza](03_plaza.png)
4. [Plaza_NiroInShadow](04_plaza_niro_in_shadow.png)
5. [Library](05_library.png)
6. [TimeWindow Aperture](06_timewindow_aperture.png)

## Verification Notes

- Cycle171 validate/capture/build/smoke completed after Phase A shader lightening.
- Built exe smoke ran for 24 seconds with the configured error-pattern scan and reported zero pattern hits in the smoke log.
- `FastVsRealtimeLightShadowRig.cs` no longer contains the removed Cycle128/Cycle131 painted-overlay tokens.
- The old 0.35s periodic shadow-policy refresh tokens are absent from the realtime rig.
- `FastVS_SurfaceRampLit.shader` uses `Cull Back` and no longer uses the custom `MainLightRealtimeShadow(...)` PCF path.
- The TimeWindow aperture capture was checked for black-frame or obvious missing-render symptoms; final visual judgement remains for Tom.

## Gate

変更を適用しました。

参考画像とのギャップは、Phase A 時点では Buto の volumetric god rays、Tilt Shift、HDR emissive/VFX が未導入であり、target HD-2D quality remains substantially below reference です。

Tom 判定をお願いします。
