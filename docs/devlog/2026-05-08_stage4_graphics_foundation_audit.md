# Stage 4 Graphics Foundation Audit

Date: 2026-05-08

## Summary

This pass records the pre-change graphics foundation state for Stage 4. Scope is URP, renderer features, portal stencil / time-window visual materials, lighting, Volume / post-process readiness, current visual evidence, and verification caveats.

This audit does not change story, map layout, character art, camera direction, or final palette decisions.

## Project Baseline

| Item | Current state |
|---|---|
| Unity Editor | `6000.3.14f1` |
| URP package | `com.unity.render-pipelines.universal@17.3.0` |
| Primary scene | `Assets/Scenes/Anemora_Main.unity` |
| Portal sandbox scene | `Assets/Scenes/Sandbox_E1_Stencil.unity` |
| Pipeline asset | `Assets/Settings/UniversalRenderPipeline.asset` |
| Renderer asset | `Assets/Settings/UniversalRenderPipeline_Renderer.asset` |
| Global settings | `Assets/Settings/UniversalRenderPipelineGlobalSettings.asset` |
| Default Volume profile | `Assets/Settings/DefaultVolumeProfile.asset` |
| Portal flash Volume profile | `Assets/Settings/Portal/PortalFlash_VolumeProfile.asset` |

## URP / Renderer Feature State

- `GraphicsSettings.defaultRenderPipeline` points to `Assets/Settings/UniversalRenderPipeline.asset`.
- All Quality levels point to the same URP pipeline asset. Standalone default quality is `Ultra` (`5`).
- `UniversalRenderPipeline.asset` uses a single renderer data entry: `UniversalRenderPipeline_Renderer.asset`.
- The renderer asset contains one active Renderer Feature:
  - `PortalStencilFeature`
  - `passEvent: 300` (`AfterRenderingOpaques`)
  - `portalMaskLayers: ~0`
  - `insidePortalLayers: ~0`
- `PortalStencilFeature` is already migrated away from URP internal `DrawObjectsPass` to public `RenderObjectsPass`.
- `PortalStencilFeature` enqueues two passes:
  - `Anemora Portal Mask` using LightMode `AnemoraPortalMask`
  - `Anemora Portal Inside` using LightMode `AnemoraPortalInside`
- Stencil contract is ADR-0002 compliant:
  - `StencilBit = 3`
  - `StencilMask = 8`
  - shader `Ref = 8`
  - shader `ReadMask = 8`
  - shader `WriteMask = 8`

## URP Asset Quality Notes

| Setting | Current value | Note |
|---|---:|---|
| HDR | enabled | Appropriate for flash / future lightweight grading. |
| MSAA | disabled (`1`) | Pixel-art friendly; no immediate change. |
| Render Scale | `1` | Baseline performance target preserved. |
| Main Light Shadows | enabled | Matches HD-2D Tier 2. |
| Main Light Shadowmap | `2048` | Reasonable Stage 4 baseline. |
| Additional Lights | per-vertex / per-object path present, no additional shadows | Tier 2 says single-direction-light baseline; avoid expanding lights without review. |
| Shadow Cascades | `1` in pipeline, higher quality tiers define cascades independently | Keep stable until map scale review. |
| Soft Shadows | disabled | Tier 2-friendly and performance conservative. |
| SRP Batcher | enabled | Good baseline. |
| Post-processing support | available but camera-disabled in current scene | See caveat below. |

## Portal Shader / Material Inventory

| Asset | Role | Current visual state |
|---|---|---|
| `Assets/Art/Materials/Portal/PortalMask.shader` | Writes stencil bit 3, no color output | Contains `UniversalForward` and custom `AnemoraPortalMask` passes. |
| `Assets/Art/Materials/Portal/InsideOnly.shader` | Draws only where stencil equals 8 | Contains `UniversalForward` and custom `AnemoraPortalInside` passes with simple main-light shading. |
| `Assets/Art/Materials/Portal/PortalMask.mat` | Portal frame / mask material | Uses `PortalMask.shader`; colorless stencil writer. |
| `Assets/Art/Materials/Portal/InsideOnly.mat` | Sandbox inside-only material | `_BaseColor = (0.16, 0.54, 1, 1)`. |
| `Assets/Art/Materials/Portal/Debug_Current.mat` | Current-side debug / time-window frame material | URP Lit, warm beige `_BaseColor = (0.83, 0.72, 0.52, 1)`. |
| `Assets/Art/Materials/Portal/Debug_Past.mat` | Past-side debug / time-window frame material | URP Lit, blue `_BaseColor = (0.46, 0.64, 0.88, 1)`. |
| `Assets/Art/Materials/Portal/TimeVolume_SpaceVeil.mat` | Runtime local time-window veil material | URP Unlit transparent, `_BaseColor = (0.18, 0.64, 1, 0.16)`. |

`Assets/Shaders/` does not exist in this worktree. Portal shaders currently live beside portal materials under `Assets/Art/Materials/Portal/`.

## Portal Visual Implementation Notes

- `Assets/Prefabs/Portal/Portal_Frame.prefab` uses `PortalMask.mat` and is the stencil-frame path.
- `Assets/Prefabs/Portal/TimeWindow_Diorama.prefab` is the current playable local time-window path. It uses `TimeWindowDiorama` to:
  - clone visible content from `Root_Past`
  - hide overlapping current content
  - add a translucent footprint / rim / side veil
  - apply past-space tint through material property blocks
- In `Anemora_Main`, `TimeFramePortalController.useLocalDioramaWindow` is enabled, so the main playable brush window path currently bypasses atomic stencil crossing and uses `TimeWindowDiorama`.
- `PortalStencilFeature` remains active for the renderer and sandbox tests, and is still part of the ADR-0002 foundation.

## Scene Lighting / Camera / Volume

### `Anemora_Main`

| Item | Current value |
|---|---|
| Main Camera clear color | `(0.11, 0.12, 0.14, 1)` |
| Main Camera FOV | `48` |
| Main Camera near / far | `0.05 / 140` |
| Main Camera culling mask at rest | `1056` (`UI` + Current visual layer) |
| Main Camera URP post-processing | disabled (`m_RenderPostProcessing: 0`) |
| Main Light | Directional, intensity `1.1`, white |
| Main Light shadows | enabled, hard shadows |
| Global Volume on `TimeFramePortalSystem` | present, global, priority `100`, weight `0` |
| PortalFlash profile reference | `PortalFlash_VolumeProfile.asset` |

The Global Volume is driven by `PortalFlashPlayer`, which creates a runtime `ColorAdjustments` profile with `postExposure = 2.5`. Because Main Camera post-processing is currently disabled, the flash Volume is likely not visible in `Anemora_Main` until the camera is updated.

### `Sandbox_E1_Stencil`

| Item | Current value |
|---|---|
| Main Camera clear color | `(0.08, 0.10, 0.13, 1)` |
| Main Camera FOV | `45` |
| Main Camera near / far | `0.1 / 100` |
| Main Camera URP post-processing | disabled (`m_RenderPostProcessing: 0`) |
| Main Light | Directional, intensity `1.4`, white |
| Main Light shadows | enabled, hard shadows |

## Volume Profile State

- `Assets/Settings/DefaultVolumeProfile.asset` is empty (`components: []`).
- `Assets/Settings/Portal/PortalFlash_VolumeProfile.asset` is empty (`components: []`).
- `PortalFlashPlayer` adds `ColorAdjustments` at runtime if missing. This makes the profile asset safe, but also means static review of the asset does not reveal the flash effect.

## Warning / Caveat State

- Last recorded RenderGraph warning cleanup baseline: `docs/devlog/2026-05-06_urp_renderobjects_pass_migration.md`.
  - EditMode `32/32`
  - PlayMode `29/29`
  - Windows build success
  - 30 second player log warning count `0`
- Last recorded Stage 4 performance baseline: `docs/devlog/2026-05-07_stage4_performance_baseline_v0_1.md`.
  - Average FPS `59.884`
  - p95 frame time `16.683 ms`
  - working set peak `325.258 MiB`
  - URP `DrawObjectsPass` warning count `0`
  - checked player-log patterns all `0`
- Current audit has not yet re-run Unity tests or player log smoke in the new graphics-foundation worktree.

## Current Screenshot / Review Artifacts

Existing visual artifacts include:

- `docs/devlog/screenshots/e1_portal_front.png`
- `docs/devlog/screenshots/e1_portal_side.png`
- `docs/devlog/screenshots/e1_portal_back.png`
- `docs/devlog/screenshots/a2_main_current_open.png`
- `docs/devlog/screenshots/a2_main_past_after_cross.png`
- `docs/devlog/screenshots/a2_main_current_after_return.png`
- `docs/devlog/screenshots/stage4_scale_lineup_current_demo.png`
- `docs/devlog/screenshots/stage4_scale_lineup_target_metrics.png`
- `docs/devlog/screenshots/stage4_zone1_antela_blockout_v0_1_review.png`
- `docs/devlog/screenshots/dialogue_tmp_capture/dialogue_tmp_capture_review_sheet.png`

## Immediately Fixable Items

1. Enable post-processing on the `Anemora_Main` Main Camera so `PortalFlashPlayer` can actually drive the flash Volume.
2. Add an automated guard that the main scene camera keeps post-processing enabled while `PortalFlashPlayer` is present.
3. Add a focused `PortalFlashPlayer` PlayMode test for runtime Volume profile creation and weight decay.
4. Add a screenshot capture utility for current/proposed graphics baseline review that does not modify production scene assets.
5. Consider a custom transparent veil shader or material polish for `TimeVolume_SpaceVeil` after screenshot baseline is captured.

## User-Decision Items

These should remain proposal / comparison artifacts until approved:

- Large color grading or palette shift for Current / Past / Future.
- Strong bloom, vignette, chromatic aberration, or heavy contrast changes.
- Camera angle, field of view, orthographic / perspective decision, or composition changes.
- HD-2D Tier 3+ features such as volumetrics, sprite normal maps, multiple-light art direction, or bloom-heavy cinematic treatment.
- Production application of a full Stage 4 visual baseline profile to `Anemora_Main`.

## Proposed First Technical Fix

The smallest graphics-foundation fix is to enable Main Camera post-processing in `Anemora_Main` and cover it with a PlayMode smoke test. This does not change camera composition, palette, lighting direction, map layout, or character art. It only allows the already-wired portal flash Volume to render.
