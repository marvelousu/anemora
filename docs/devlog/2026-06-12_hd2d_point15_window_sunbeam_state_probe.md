# 2026-06-12 HD2D point15 window sunbeam state probe

## Scope

- Continue from the Cycle125 object split measurement.
- Do not delete or tune the library-front sunbeam in this slice.
- Measure the user's report that the library-front sun shaft appears to change when the Time Window is opened.
- Use built-player evidence only.

## Code change

- `Assets/Scripts/FastVS/FastVsHouseRuntimeSmokeProbe.cs`
  - Extended `--anemora-house-slice-window-door-review-dir` state logging with:
    - `sunbeamRenderers`
    - `sunbeamExact`
  - `sunbeamExact` forces object-name logging for:
    - `Current_CentralPlaza_Cycle125_ReferenceDioramaShadow_HighSunbeamColumnA`
    - `Current_CentralPlaza_Cycle125_ReferenceDioramaShadow_CenterChalkSunCatchA`
    - `Current_CentralPlaza_Cycle125_ReferenceDioramaShadow_BackDepthHazeA`
    - `Current_CentralPlaza_Cycle125_ReferenceDioramaShadow_LibraryEaveHardContactA`
    - `Current_CentralPlaza_Cycle126_CloseShadowBarMute_GroundAirUnifierA`

## Build evidence

- Superseded first build log:
  - `Logs\point15_window_sunbeam_state_build_validate_20260612T145309.log`
- Final build log:
  - `Logs\point15_window_sunbeam_exact_build_validate_20260612T150152.log`
- Final build key lines:
  - `DisplayProgressNotification: Build Successful`
  - `Build Finished, Result: Success.`
  - `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Built-player evidence

- Superseded first player log:
  - `Logs\point15_window_sunbeam_state_player_20260612T150021.log`
- Final player log:
  - `Logs\point15_window_sunbeam_exact_player_20260612T150803.log`
- Final review folder:
  - `docs\review\2026-06-12T15-08_window_sunbeam_exact_probe`
- Captures:
  - 20 PNGs
- Main comparison captures:
  - `10_window_closed_before_open.png`
  - `11_window_open.png`
  - `12_window_closed_after_open.png`

## Renderer contract

- Built-player contract line:
  - `ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>`
- Pale-wash suppression remained active:
  - `[Point15Recovery] ApplyRendererShadowPolicy area=CentralPlaza: disabled 14 library-front pale wash renderer(s).`

## Lighting state

- Closed before opening:
  - `ANEMORA_HOUSE_SLICE_LIGHTING_STATE: label=windowDoorReview.closed.ready expectedArea=CentralPlaza activeArea=CentralPlaza sunCurrent=Morning sunTarget=Morning sunTransitioning=False indoorSunSuppression=False main=name=Directional Light,enabled=True,type=Directional,intensity=1.500,color=(1.000,0.840,0.700,1.000),shadows=Soft,shadowStrength=0.800,shadowBias=0.012,shadowNormalBias=0.100 warm=name=FastVS_HD2D_WarmFillLight,enabled=True,type=Point,intensity=0.300,color=(1.000,0.640,0.340,1.000),shadows=None,shadowStrength=1.000,shadowBias=0.050,shadowNormalBias=0.400 cool=name=FastVS_HD2D_CoolRimLight,enabled=True,type=Directional,intensity=0.160,color=(0.460,0.580,0.920,1.000),shadows=None,shadowStrength=1.000,shadowBias=0.050,shadowNormalBias=0.400 ambientMode=Flat ambient=(0.074,0.068,0.058,1.000) ambientMax=0.074 fog=False fogColor=(0.235,0.215,0.178,1.000) fogDensity=0.011 shadowPolicy=renderers=11832,active=1608,enabled=11752,castOn=801,shadowsOnly=46,receive=1459,activeCastOn=64,activeReceive=176`
- Open:
  - `ANEMORA_HOUSE_SLICE_LIGHTING_STATE: label=windowDoorReview.open.ready expectedArea=CentralPlaza activeArea=CentralPlaza sunCurrent=Morning sunTarget=Morning sunTransitioning=False indoorSunSuppression=False main=name=Directional Light,enabled=True,type=Directional,intensity=1.500,color=(1.000,0.840,0.700,1.000),shadows=Soft,shadowStrength=0.800,shadowBias=0.012,shadowNormalBias=0.100 warm=name=FastVS_HD2D_WarmFillLight,enabled=True,type=Point,intensity=0.300,color=(1.000,0.640,0.340,1.000),shadows=None,shadowStrength=1.000,shadowBias=0.050,shadowNormalBias=0.400 cool=name=FastVS_HD2D_CoolRimLight,enabled=True,type=Directional,intensity=0.160,color=(0.460,0.580,0.920,1.000),shadows=None,shadowStrength=1.000,shadowBias=0.050,shadowNormalBias=0.400 ambientMode=Flat ambient=(0.074,0.068,0.058,1.000) ambientMax=0.074 fog=False fogColor=(0.235,0.215,0.178,1.000) fogDensity=0.011 shadowPolicy=renderers=11844,active=1620,enabled=11744,castOn=813,shadowsOnly=46,receive=1471,activeCastOn=76,activeReceive=188`
- Closed after opening:
  - `ANEMORA_HOUSE_SLICE_LIGHTING_STATE: label=windowDoorReview.closedAfter.ready expectedArea=CentralPlaza activeArea=CentralPlaza sunCurrent=Morning sunTarget=Morning sunTransitioning=False indoorSunSuppression=False main=name=Directional Light,enabled=True,type=Directional,intensity=1.500,color=(1.000,0.840,0.700,1.000),shadows=Soft,shadowStrength=0.800,shadowBias=0.012,shadowNormalBias=0.100 warm=name=FastVS_HD2D_WarmFillLight,enabled=True,type=Point,intensity=0.300,color=(1.000,0.640,0.340,1.000),shadows=None,shadowStrength=1.000,shadowBias=0.050,shadowNormalBias=0.400 cool=name=FastVS_HD2D_CoolRimLight,enabled=True,type=Directional,intensity=0.160,color=(0.460,0.580,0.920,1.000),shadows=None,shadowStrength=1.000,shadowBias=0.050,shadowNormalBias=0.400 ambientMode=Flat ambient=(0.074,0.068,0.058,1.000) ambientMax=0.074 fog=False fogColor=(0.235,0.215,0.178,1.000) fogDensity=0.011 shadowPolicy=renderers=11832,active=1608,enabled=11752,castOn=801,shadowsOnly=46,receive=1459,activeCastOn=64,activeReceive=176`

## Sunbeam exact state

| State | HighSunbeam active | HighSunbeam enabled | HighSunbeam visible | BackDepthHaze enabled | GroundAirUnifier enabled |
|---|---:|---:|---:|---:|---:|
| closed.ready | True | True | False | False | False |
| window.closed.beforeOpen | True | True | True | False | False |
| window.open opened=True | True | True | True | False | False |
| window.closed.afterOpen | True | True | True | False | False |

- `HighSunbeamColumnA` material/queue:
  - `Material=FastVS_House_hd2d_plaza_reference_diorama_air_cycle125`
  - `Queue=3124`
  - `Bounds=center=(19.680,1.960,23.352),size=(3.300,3.320,0.000),min=(18.030,0.300,23.352),max=(21.330,3.620,23.352)`
- `CenterChalkSunCatchA` material/queue:
  - `Material=FastVS_House_hd2d_plaza_reference_diorama_sun_cycle125`
  - `Queue=3122`
- `BackDepthHazeA` stayed disabled:
  - `active=True enabled=False visible=False`
- `Current_CentralPlaza_Cycle126_CloseShadowBarMute_GroundAirUnifierA` stayed disabled:
  - `active=True enabled=False visible=False`

## Image deltas

- `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedBefore_vs_open meanAbsRgb=8.314 changedSamplePct=22.142 changed=12754 samples=57600`
- `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedBefore_vs_closedAfter meanAbsRgb=0.001 changedSamplePct=0.007 changed=4 samples=57600`
- `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedDoorBaseline_vs_closedBeforePortal meanAbsRgb=0.465 changedSamplePct=1.844 changed=1062 samples=57600`
- End marker:
  - `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: end count=20`

## Interpretation

- The library-front sunbeam object is not being disabled by opening the Time Window:
  - `HighSunbeamColumnA` remains `active=True enabled=True visible=True` before open, while open, and after closing.
- SunCycle/light/ambient/fog state does not change between closed/open/closedAfter:
  - `sunCurrent=Morning sunTarget=Morning sunTransitioning=False`
  - `main intensity=1.500`
  - `warm intensity=0.300`
  - `cool intensity=0.160`
  - `ambientMax=0.074`
  - `fog=False`
- The large open-window image delta is therefore not a light-state regression.
- The open-window state adds/restores portal-visible renderers:
  - closed: `renderers=11832 active=1608 enabled=11752`
  - open: `renderers=11844 active=1620 enabled=11744`
  - closedAfter: `renderers=11832 active=1608 enabled=11752`
- Visual inspection shows the open portal aperture composites the other-time view over the current library facade/floor. This explains why floor shadow and shaft readability appear to change while the actual sunbeam renderer remains on.

## Next slice

- Do not delete `HighSunbeamColumnA` as a response to the window-open symptom.
- Next fix candidate should target portal aperture compositing/stencil/opacity/culling, not SunCycle or the Cycle125 sunbeam renderer.
- Add a focused portal-aperture material/renderQueue/ZWrite/ZTest/culling probe before changing the portal shader or aperture material.

