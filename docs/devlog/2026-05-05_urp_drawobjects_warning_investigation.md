# URP DrawObjectsPass RenderGraph Warning Investigation

Date: 2026-05-05

## 1. Scope

This investigation identifies the root cause of the repeated URP warning:

`The render pass UnityEngine.Rendering.Universal.Internal.DrawObjectsPass does not have an implementation of the RecordRenderGraph method.`

This is report-only. No rendering implementation, URP settings, scene, or project settings were changed.

Measured / inspected state:

| Item | Value |
| --- | --- |
| Investigation worktree | `<worktree:Anemora-urp-warning-investigation>` |
| Project commit inspected | `da6040f` |
| Unity | 6000.3.14f1 |
| URP package | `com.unity.render-pipelines.universal` 17.3.0 |
| Prior warning counts | 6,996 in the 30s audio rebuild player log; 14,402 in the v0.2 120s player log |

## 2. Reproduction Evidence

Existing logs already reproduce the warning deterministically.

| Run | Evidence | Count |
| --- | --- | ---: |
| G5 audio rebuild | `docs/devlog/2026-05-05_g5_audio_rebuild.md` | 6,996 warnings / 30s |
| Performance baseline v0.2 | `<worktree:Anemora-perf-baseline-v0-2>\g5_perf_v02_player_120s.log` | 14,402 warnings / 120s |
| Performance baseline v0.2 frame count | `docs/devlog/2026-05-05_performance_baseline_v0_2.md` | 7,199 frames |

The v0.2 count maps to the portal feature structure:

- `PortalStencilFeature` enqueues exactly 2 passes per camera frame.
- `2 * 7,199 frames = 14,398`.
- The observed count is 14,402, which is 4 extra warnings during scene load / startup before the timed sample.

This strongly indicates a two-pass-per-frame source, not a random log loop.

Deterministic repro command shape:

```powershell
$exe = "<worktree:Anemora-perf-baseline-v0-2>\Builds\PerfBaselineV02\AnemoraPerfBaselineV02.exe"
$log = "C:\Temp\Anemora_urp_warning_repro.log"
Start-Process -FilePath $exe -ArgumentList @("-screen-fullscreen","0","-screen-width","1280","-screen-height","720","-logFile",$log) -Wait
(Select-String -Path $log -Pattern "DrawObjectsPass does not have an implementation of the RecordRenderGraph method").Count
```

For a shorter repro, use the G5 audio player build and stop after 30s, then run the same `Select-String` count.

## 3. Local Code Findings

### 3.1 The only project `DrawObjectsPass` construction is `PortalStencilFeature`

`rg "new DrawObjectsPass|DrawObjectsPass\(" Assets Packages -g "*.cs"` found only:

| Path | Lines | Detail |
| --- | ---: | --- |
| `Assets/Scripts/TimeManagement/Portal/PortalStencilFeature.cs` | 34, 44 | Creates `maskPass` and `insidePass` as `UnityEngine.Rendering.Universal.Internal.DrawObjectsPass`. |

`PortalStencilFeature` imports `UnityEngine.Rendering.Universal.Internal` and creates two internal `DrawObjectsPass` instances:

- `Anemora Portal Mask`, shader tag `AnemoraPortalMask`.
- `Anemora Portal Inside`, shader tag `AnemoraPortalInside`.

`AddRenderPasses` enqueues both every camera frame and updates diagnostics:

- `renderer.EnqueuePass(maskPass)`
- `renderer.EnqueuePass(insidePass)`
- `LastEnqueuedPassCount = 2`

### 3.2 Renderer asset has exactly this custom feature active

`Assets/Settings/UniversalRenderPipeline_Renderer.asset` contains:

| Field | Value |
| --- | --- |
| Renderer feature | `PortalStencilFeature` |
| Active | `m_Active: 1` |
| Event | `passEvent: 300` (`AfterRenderingOpaques`) |
| Portal mask layers | serialized all layers at asset level; runtime changes through `SetLayerMasks()` |
| Inside portal layers | serialized all layers at asset level; runtime changes through `SetLayerMasks()` |

No other project renderer feature constructs `DrawObjectsPass`.

### 3.3 URP 17.3.0 source explains the warning

Inspected package cache:

`<worktree:Anemora-perf-baseline-v0-2>\Library\PackageCache\com.unity.render-pipelines.universal@3b809f23691d`

Relevant findings:

| Package file | Finding |
| --- | --- |
| `Runtime/Passes/DrawObjectsPass.cs` | `DrawObjectsPass` is `public partial class DrawObjectsPass : ScriptableRenderPass`, but its old `Execute(...)` path is inside `#if URP_COMPATIBILITY_MODE`. It does not provide a non-compat `RecordRenderGraph(...)` override in the inspected 17.3.0 package. |
| `Runtime/Passes/ScriptableRenderPass.cs` | The base `RecordRenderGraph(...)` logs the exact warning text when a pass does not override it. |
| `Runtime/ScriptableRenderer.cs` | RenderGraph path calls `pass.RecordRenderGraph(renderGraph, frameData)` for enqueued passes. |
| `Runtime/Passes/RenderObjectsPass.cs` | Public `RenderObjectsPass` has a real `RecordRenderGraph(...)` implementation and supports renderer-list drawing with a `RenderStateBlock`. |
| `Runtime/Settings/RenderGraphSettings.cs` | Without `URP_COMPATIBILITY_MODE`, `enableRenderCompatibilityMode` getter returns `false`; Compatibility Mode is not available through the normal runtime getter. |

Project settings confirm the project is not building with Compatibility Mode:

| File | Value |
| --- | --- |
| `Assets/Settings/UniversalRenderPipelineGlobalSettings.asset` | `m_EnableRenderCompatibilityMode: 0` |
| `ProjectSettings/ProjectSettings.asset` | `scriptingDefineSymbols: {}`; no `URP_COMPATIBILITY_MODE` define |

The older `m_EnableRenderGraph: 0` value still exists in the global settings asset as migration data, but URP 17.3.0 uses `RenderGraphSettings.m_EnableRenderCompatibilityMode`. The inspected runtime getter returns false without the compatibility define.

## 4. Official Unity Documentation Check

Unity's Unity 6 URP documentation matches the package-source reading:

- Compatibility Mode lets a project write a `ScriptableRenderPass` without the render graph API, but Unity states that this non-render-graph path is no longer developed or improved and recommends using RenderGraph for new graphics features.
- RenderGraph custom passes are expected to implement `RecordRenderGraph(...)`; Unity calls that method during render graph configuration to register passes and resources.
- To draw objects from a custom RenderGraph pass, Unity documents the `RendererListHandle` / `CreateRendererList` / `UseRendererList` workflow.
- Unity also documents `AddUnsafePass` as a bridge for some compatibility APIs, with an optimization caveat.

References:

- https://docs.unity.cn/6000.0/Documentation/Manual/urp/compatibility-mode.html
- https://docs.unity.cn/6000.0/Documentation/Manual/urp/render-graph-write-render-pass.html
- https://docs.unity.cn/6000.0/Documentation/Manual/urp/render-graph-draw-objects-in-a-pass.html
- https://docs.unity.cn/6000.0/Documentation/Manual/urp/render-graph-unsafe-pass.html

No exact Unity Issue Tracker item for this specific `DrawObjectsPass` warning was found in the scoped search. The warning is directly explained by the package source and Unity's migration documentation, so the primary finding is an Anemora implementation/API mismatch rather than a confirmed URP 17.3.0 engine bug.

## 5. Root Cause

Root cause:

`PortalStencilFeature` uses the URP internal `UnityEngine.Rendering.Universal.Internal.DrawObjectsPass`, which is a Compatibility Mode / legacy-style pass in URP 17.3.0 and does not implement `RecordRenderGraph(...)` for the default RenderGraph path. Because Anemora does not enable `URP_COMPATIBILITY_MODE`, URP records the frame through RenderGraph and calls the base `ScriptableRenderPass.RecordRenderGraph(...)`, which logs the warning and states that the pass has no effect.

The warning frequency is explained by the feature's structure:

| Feature pass | Warning source |
| --- | --- |
| `maskPass` | one `DrawObjectsPass` warning per frame |
| `insidePass` | one `DrawObjectsPass` warning per frame |

The v0.2 log count is therefore expected: roughly two warnings per rendered frame.

## 6. Hypotheses Ranked

| Rank | Hypothesis | Evidence | Status |
| ---: | --- | --- | --- |
| 1 | `PortalStencilFeature`'s two internal `DrawObjectsPass` instances are the source. | Only project `DrawObjectsPass` construction; renderer asset has the feature active; warning count is two per frame. | Strongly supported. |
| 2 | URP RenderGraph path is active and Compatibility Mode is not active. | `m_EnableRenderCompatibilityMode: 0`; no `URP_COMPATIBILITY_MODE`; package getter returns false without define. | Supported. |
| 3 | A generic URP 17.3.0 bug in built-in passes is causing unrelated `DrawObjectsPass` warnings. | Standard URP `RenderObjectsPass` has `RecordRenderGraph`; warning names `Internal.DrawObjectsPass`, matching the Anemora feature. | Weak / unlikely. |
| 4 | Scene object order, stencil bit selection, or audio integration triggers the warning. | Warning appears before/after scene ready and scales with frames; counts align with two enqueued passes, not scene content. | Not supported. |

## 7. Fix Proposals

No fix was implemented in this task.

Recommended Stage 4 fix order:

1. Replace internal `DrawObjectsPass` usage with a RenderGraph-capable custom pass based on `RendererListHandle`.
   - Use `RecordRenderGraph(...)`.
   - Create renderer lists with `RenderingUtils.CreateRendererListWithRenderStateBlock(...)` or equivalent `RendererListParams`.
   - Set active color and depth attachments through `UniversalResourceData`.
   - Preserve the two shader tags: `AnemoraPortalMask` and `AnemoraPortalInside`.
   - Preserve `SetLayerMasks(...)` as the public runtime API used by portal side switching.
2. Evaluate public `RenderObjectsPass` as a lower-effort replacement.
   - It already implements `RecordRenderGraph(...)`.
   - It supports shader tags, layer mask filtering, and `SetStencilState(...)`.
   - Confirm it can represent both portal passes without relying on `UnityEngine.Rendering.Universal.Internal`.
3. Do not use Compatibility Mode as the Stage 4 public solution.
   - Unity 6 documentation says the non-RenderGraph path is no longer developed or improved.
   - URP 17.3.0 package UI/build validator notes that Compatibility Mode is deprecated / hidden in Unity 6.3 and requires `URP_COMPATIBILITY_MODE` for retention.

Hypothesis test for the fix task:

- Temporarily disable `PortalStencilFeature` in `UniversalRenderPipeline_Renderer.asset`.
- Run the same 30s player repro.
- Expected result: warning count drops to 0, confirming source.
- Then apply the replacement pass and verify:
  - warning count remains 0,
  - `PortalStencilFeatureSmokeTest` passes,
  - `AnemoraMainPortalWiringRoundTripTests` and G4/G5 portal flow pass,
  - visual portal mask / inside rendering still works in Game view.

## 8. Conclusion

The root cause is identified with high confidence: Anemora's `PortalStencilFeature` still depends on URP's internal `DrawObjectsPass`, which is not RenderGraph-compatible in the active URP 17.3.0 configuration. The warning is expected once for each of the two portal stencil passes every rendered frame.

The preferred fix is to migrate the portal stencil feature to a RenderGraph-capable renderer-list pass or to public `RenderObjectsPass`, then verify warnings drop to zero without breaking portal visuals.
