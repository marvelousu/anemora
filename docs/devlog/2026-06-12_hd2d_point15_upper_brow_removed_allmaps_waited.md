# 2026-06-12 HD2D point15 upper brow removed all-map waited capture

## Scope

- Recheck the full all-map built-player state after removing only `Current_CentralPlaza_ArchitectureSurfaceDepth_EntranceUpperBrowA`.
- Use a waited player launch so the review has both PNG evidence and an explicit process exit code.
- Keep this as validation only. No additional scene or generator change was made in this slice.

## Built-player evidence

- Player executable:
  - `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Review folder:
  - `docs\review\2026-06-12T23-15_upper_brow_removed_allmaps_waited`
- Player log:
  - `Logs\point15_renderer_upper_brow_removed_allmaps_waited_20260612T231111.log`
- Player result:
  - `PLAYER_EXIT=0`
- Captures:
  - `13` PNGs
  - Total PNG bytes: `8605093`

## Renderer contract

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

## Central plaza lighting sample

```text
ANEMORA_HOUSE_SLICE_LIGHTING_STATE: label=CentralPlaza.current.beforeCapture expectedArea=CentralPlaza activeArea=CentralPlaza sunCurrent=Morning sunTarget=Morning sunTransitioning=True indoorSunSuppression=False main=name=Directional Light,enabled=True,type=Directional,intensity=1.500,color=(1.000,0.840,0.700,1.000),shadows=Soft,shadowStrength=0.800,shadowBias=0.012,shadowNormalBias=0.100 warm=name=FastVS_HD2D_WarmFillLight,enabled=True,type=Point,intensity=0.300,color=(1.000,0.640,0.340,1.000),shadows=None,shadowStrength=1.000,shadowBias=0.050,shadowNormalBias=0.400 cool=name=FastVS_HD2D_CoolRimLight,enabled=True,type=Directional,intensity=0.160,color=(0.460,0.580,0.920,1.000),shadows=None,shadowStrength=1.000,shadowBias=0.050,shadowNormalBias=0.400 ambientMode=Flat ambient=(0.074,0.068,0.058,1.000) ambientMax=0.074 fog=False fogColor=(0.235,0.215,0.178,1.000) fogDensity=0.011 shadowPolicy=renderers=11831,active=1607,enabled=11751,castOn=801,shadowsOnly=46,receive=1459,activeCastOn=64,activeReceive=176
```

## Removal persistence check

The removed current-time object name was absent from this waited all-map player log:

```text
Select-String -Path Logs\point15_renderer_upper_brow_removed_allmaps_waited_20260612T231111.log -Pattern 'ArchitectureSurfaceDepth_EntranceUpperBrowA' | Measure-Object
Count: 0
```

The capture completed:

```text
ANEMORA_HOUSE_SLICE_CAPTURE: end count=13
PLAYER_EXIT=0
```

## Visual review notes

- `03_b1_b3_current.png`: the prior library-front white wash and the door-front plank/column artifact are not visible in this all-map capture.
- `04_b1_b3_past.png`: the library-front mass remains visually coherent; no obvious hole was introduced by omitting the current-time upper brow.
- `13_scene6_sideview_auto.png`: distant sideview fragments remain visible. This is carried forward as a separate long-road / far-outline transparency or visibility-pop diagnostic, not as an upper-brow-removal regression.

## Findings

- The upper-brow removal persists after correct scene regeneration and player rebuild.
- The renderer contract remains the point15 recovery contract:
  - `RenderingMode=2`
  - `DepthPrimingMode=0`
  - `CopyDepthMode=0`
  - `PortalStencilFeatureActive=True`
- The all-map capture did not reveal a new library-front breakage from the one-object removal.
- The remaining visible concern is the distant road / sideview fragment family, which needs a renderer diagnostics pass before any further removal.

## Next action

- Propagate `docs\review\2026-06-12T23-15_upper_brow_removed_allmaps_waited` to anemora-viewer.
- Continue with data-first diagnostics on the long-road / far-outline renderers, including material queue, ZWrite, Cull, bounds, and overlap pairs.
