# feat(hd2d): lighten surface ramp shadow path

Date: 2026-05-28 JST

## Scope

- Phase A Step 5 from `docs/HD2D_SUN_CYCLE_SPEC.md`.
- `Assets/Art/Shaders/FastVS/FastVS_SurfaceRampLit.shader`: surface pass culling changed from `Cull Off` to `Cull Back`.
- Removed the self-authored 8-tap `MainLightRealtimeShadow(...)` PCF block from the surface ramp shader.
- `Assets/Art/Shaders/FastVS/FastVS_SpriteCardRampUnlit.shader` remains two-sided; this cycle does not change sprite-card culling.

## Cycle Worker

SCOPED_PROMPT_ISSUED cycle=171 authored_file=Assets/Art/Shaders/FastVS/FastVS_SurfaceRampLit.shader validate=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseASurfaceRampShaderLighteningBatch capture=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseASurfaceRampShaderLighteningCycle171ScreenshotsBatch

Worker result was incomplete, so the parent session finished the scoped shader edit and the validation/capture wiring.

## Implementation Notes

- Surface ramp shadows now use URP's standard `TransformWorldToShadowCoord(input.positionWS)` -> `GetMainLight(...)` -> `mainLight.shadowAttenuation` path.
- Cycle171 validation records grep evidence for `Cull Back`, absence of `Cull Off` in the surface shader, absence of `MainLightRealtimeShadow(`, and a single direct shadow coordinate transform.
- The batch screenshot path used a local Unity 6000.3.14f1 / URP 17 D3D11 CoreBlit capture workaround after repeated black PNG captures from `Hidden/Universal/CoreBlit`. This is Editor capture tooling only; runtime shader behavior is the surface ramp culling/shadow path change above.
- Legacy review-cycle validations that previously owned main directional light, ambient/fog, or camera-painted overlays now accept the Phase A ownership split: `AnemoraSunCycleDriver` owns sun/ambient/fog, and `FastVsRealtimeLightShadowRig` owns realtime shadow policy.
- The realtime shadow rig keeps facade/helper slabs shadow-safe, preserves explicit `RealtimeShadowCasterCycle*` objects for dynamic cast shadows, and suppresses legacy camera/sky/haze plates without reintroducing forbidden Cycle128/Cycle131 overlay code.

## Verification Plan

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseASurfaceRampShaderLighteningBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseASurfaceRampShaderLighteningCycle171ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built exe launch for 24 seconds with `-batchmode -nographics`, scanning `Exception|Shader error|error CS|NullReference|MissingReference|Assertion|Failed`.

## Build Artifact For Tom

Build exe path:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

起動時は `Builds\FastVS_HouseSlice\` をフォルダごと起動対象として扱う。
