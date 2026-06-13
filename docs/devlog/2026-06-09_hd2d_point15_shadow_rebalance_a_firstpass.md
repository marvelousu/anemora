# 2026-06-09 HD2D point15 shadow rebalance A firstpass

## Context

- Project: Anemora Fast VS House Slice HD-2D recovery.
- Branch: `wip/hd2d-point15-recovery-20260609`.
- Baseline: point15 `e7277f0a`.
- Slice: A shadow/lighting first pass.
- Acceptance source: built-player capture only.
- Review folder: `docs/review/2026-06-09T11-13_shadow_rebalance_a_firstpass/`.

## Changes

- Kept `FastVsRealtimeLightShadowRig` out of `mainLight.intensity` and `RenderSettings.ambient*` ownership.
- Added `FastVsHouseAreaVisibility` to notify `AnemoraSunCycleDriver` when review/capture active area changes, so the SunCycle area policy is reapplied before capture.
- Added a SunCycle active-area policy guard in `AnemoraSunCycleDriver.Update()` for area changes that occur outside normal door transitions.
- Lowered outdoor `FastVsHouseLightingDirector` cool rim profiles to `0.16f` for Exterior, CentralPlaza, and Library. Indoor/Mia/Aria values stayed `0.15f`.
- Lowered point15 SunPreset ambient colors:
  - Morning: `(0.54,0.49,0.41)` -> `(0.42,0.38,0.32)`.
  - Evening: `(0.49,0.38,0.32)` -> `(0.42,0.32,0.27)`.
- Updated the Stage 3 lighting validation expected cool-rim values to match the new Round2 A profile.
- Added built-player lighting-state logs to `FastVsHouseRuntimeSmokeProbe` for main/warm/cool light state, ambient/fog, and shadow policy counts.

## Build Evidence

- Log: `Logs/point15_shadow_rebalance_a_firstpass_build_validate_20260609T105912.log`.
- `Fast VS house slice validation passed.`
- `DisplayProgressNotification: Build Successful`
- `Build Finished, Result: Success.`
- `PlayerBuildInfo duration=105294`
- Note: an earlier validation run failed as expected on the old cool-rim validation contract: `Stage 3 exterior cool rim expected 0.250, found 0.160`. The contract was then updated to the new Round2 A values.

## Built-Player Evidence

- First player attempt: `Logs/point15_shadow_rebalance_a_firstpass_player_20260609T111200.log` reached `ANEMORA_HOUSE_SLICE_CAPTURE: begin` only and produced no PNGs after hidden-window launch. It is not acceptance evidence.
- Accepted player run: `Logs/point15_shadow_rebalance_a_firstpass_player_20260609T111301.log`.
- `- Loaded All Assemblies, in  0.285 seconds`
- `ANEMORA_HOUSE_SLICE_CAPTURE: end count=13`
- Captures: 13 PNG files, all `1280x720`.

Renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Key lighting measurements:

```text
ANEMORA_HOUSE_SLICE_LIGHTING_STATE: label=capture.ready expectedArea=CentralPlaza activeArea=CentralPlaza sunCurrent=Morning sunTarget=Morning sunTransitioning=False indoorSunSuppression=False main=name=Directional Light,enabled=True,type=Directional,intensity=1.500,color=(1.000,0.840,0.700,1.000),shadows=Soft,shadowStrength=0.800,shadowBias=0.012,shadowNormalBias=0.100 warm=name=FastVS_HD2D_WarmFillLight,enabled=True,type=Point,intensity=0.300,color=(1.000,0.640,0.340,1.000),shadows=None,shadowStrength=1.000,shadowBias=0.050,shadowNormalBias=0.400 cool=name=FastVS_HD2D_CoolRimLight,enabled=True,type=Directional,intensity=0.160,color=(0.460,0.580,0.920,1.000),shadows=None,shadowStrength=1.000,shadowBias=0.050,shadowNormalBias=0.400 ambientMode=Flat ambient=(0.420,0.380,0.320,1.000) ambientMax=0.420 fog=False fogColor=(0.910,0.770,0.590,1.000) fogDensity=0.011 shadowPolicy=renderers=11836,active=1612,enabled=11769,castOn=801,shadowsOnly=46,receive=1460,activeCastOn=64,activeReceive=177
ANEMORA_HOUSE_SLICE_LIGHTING_STATE: label=Exterior.current.beforeCapture expectedArea=Exterior activeArea=Exterior sunCurrent=Morning sunTarget=Morning sunTransitioning=True indoorSunSuppression=False main=name=Directional Light,enabled=True,type=Directional,intensity=1.500,color=(1.000,0.840,0.700,1.000),shadows=Soft,shadowStrength=0.800,shadowBias=0.012,shadowNormalBias=0.100 warm=name=FastVS_HD2D_WarmFillLight,enabled=True,type=Point,intensity=0.400,color=(1.000,0.720,0.460,1.000),shadows=None,shadowStrength=1.000,shadowBias=0.050,shadowNormalBias=0.400 cool=name=FastVS_HD2D_CoolRimLight,enabled=True,type=Directional,intensity=0.160,color=(0.580,0.720,1.000,1.000),shadows=None,shadowStrength=1.000,shadowBias=0.050,shadowNormalBias=0.400 ambientMode=Flat ambient=(0.152,0.158,0.164,1.000) ambientMax=0.164 fog=True fogColor=(0.226,0.221,0.204,1.000) fogDensity=0.011 shadowPolicy=renderers=11836,active=1217,enabled=11769,castOn=801,shadowsOnly=46,receive=1460,activeCastOn=80,activeReceive=166
ANEMORA_HOUSE_SLICE_LIGHTING_STATE: label=CentralPlaza.current.beforeCapture expectedArea=CentralPlaza activeArea=CentralPlaza sunCurrent=Morning sunTarget=Morning sunTransitioning=True indoorSunSuppression=False main=name=Directional Light,enabled=True,type=Directional,intensity=1.500,color=(1.000,0.840,0.700,1.000),shadows=Soft,shadowStrength=0.800,shadowBias=0.012,shadowNormalBias=0.100 warm=name=FastVS_HD2D_WarmFillLight,enabled=True,type=Point,intensity=0.300,color=(1.000,0.640,0.340,1.000),shadows=None,shadowStrength=1.000,shadowBias=0.050,shadowNormalBias=0.400 cool=name=FastVS_HD2D_CoolRimLight,enabled=True,type=Directional,intensity=0.160,color=(0.460,0.580,0.920,1.000),shadows=None,shadowStrength=1.000,shadowBias=0.050,shadowNormalBias=0.400 ambientMode=Flat ambient=(0.074,0.068,0.058,1.000) ambientMax=0.074 fog=False fogColor=(0.235,0.215,0.178,1.000) fogDensity=0.011 shadowPolicy=renderers=11836,active=1612,enabled=11769,castOn=801,shadowsOnly=46,receive=1460,activeCastOn=64,activeReceive=177
```

Capture sizes:

```text
01_a1_a2_current.png         968278 1280x720
02_a1_a2_past.png           1027949 1280x720
03_b1_b3_current.png        1076115 1280x720
04_b1_b3_past.png           1102279 1280x720
05_c1_c3_current.png         415238 1280x720
06_c1_c3_past.png            442943 1280x720
07_d1_d3_current.png         569930 1280x720
08_d1_d3_past.png            632971 1280x720
09_e1_e3_current.png         675044 1280x720
10_e1_e3_past.png            693233 1280x720
11_f1_f6_current.png         430044 1280x720
12_f1_f6_past.png            465730 1280x720
13_scene6_sideview_auto.png  126430 1280x720
```

## Interpretation

- The core A target was met in the built player: outdoor main light is now `1.500` instead of the prior dark `0.12` capture state.
- Cool rim is reduced to `0.160` for Exterior/CentralPlaza/Library.
- CentralPlaza capture-time ambient is now `ambientMax=0.074` through the HouseLighting profile; the SunPreset Morning ambient profile itself is `ambientMax=0.420`.
- Active shadow policy counts are now logged per area. CentralPlaza reports `activeCastOn=64`, `activeReceive=177`; Exterior reports `activeCastOn=80`, `activeReceive=166`.
- This is a first-pass visual handoff for Tom judgment. No second lighting micro-adjustment is applied until visual review feedback returns.

## Viewer

- Propagate target: `work/chapter1-continuation-map-vs-20260524`.
- R2 upload: `uploaded 16 files for chapter1-continuation-map-vs-20260524/2026-06-09T11-13_shadow_rebalance_a_firstpass (bucket TTL 45d); manifest now lists 63 paths`.
- Cloudflare deploy hook: `HookId=629936212 Status=200 Length=124`.
- Hook-only polling stayed stale for 24 polls: `255 cycles · 1188 images`, `hasTarget=False`.
- Git-triggered viewer rebuild commit: `0bb5189 chore: refresh review content 2026-06-09 shadow-rebalance`.
- Public review page after rebuild: `256 cycles · 1201 images`, album `2026-06-09T11-13_shadow_rebalance_a_firstpass` present.
- Public gallery check: 13 images, all `complete=true`, all `naturalWidth=512`, `naturalHeight=288`.
- Public devlog page check: `hasMain=true`, `hasLighting=true`, `hasViewerSection=true`, `length=73791`.
- Devlog evidence re-upload: `uploaded 16 files for chapter1-continuation-map-vs-20260524/2026-06-09T11-13_shadow_rebalance_a_firstpass (bucket TTL 45d); manifest now lists 63 paths`.
- Git-triggered viewer devlog rebuild commit: `e72f372 chore: refresh review devlog 2026-06-09 shadow-rebalance`.
- Public devlog rendered check after re-hook: `R2 upload=true`, `Hook-only polling=true`, `Git-triggered viewer rebuild commit=true`, `0bb5189=true`, `Public gallery check=true`, `Public devlog page check=true`, `length=74437`.
