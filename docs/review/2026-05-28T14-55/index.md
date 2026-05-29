# Fast VS HD-2D Phase A Gate Review

Date: 2026-05-28

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Build exe:
`Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`

Run note: フォルダごと起動

## Scope

Phase A applies the lightweight dynamic sun-cycle foundation from `docs/HD2D_SUN_CYCLE_SPEC.md`.

This public review set intentionally contains only current project captures. External reference images, side-by-side comparison boards, and diagnostic black-screen captures are not included in `docs/review`.

## Captures

1. [House Interior / Morning](01_current_house_interior_sun_cycle_morning.png)
2. [House Exterior / Morning](02_current_house_exterior_sun_cycle_morning.png)
3. [Central Plaza / Noon](03_current_central_plaza_sun_cycle_noon.png)
4. [Library / Evening](04_current_library_sun_cycle_evening.png)
5. [TimeWindow Aperture](05_timewindow_aperture.png)

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
