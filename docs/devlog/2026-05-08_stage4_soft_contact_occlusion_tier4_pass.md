# Stage 4 soft contact occlusion Tier 4 pass

Date: 2026-05-08
Scope: Tier 4 graphics push / URP renderer feature contact depth
Branch: `codex/stage4-graphics-foundation-20260508`

## Summary

Added a conservative URP Screen Space Ambient Occlusion renderer feature named `Stage4 Soft Contact Occlusion`. The goal is HD-2D Tier 4-style grounding: props, frame bars, lamp bases, and floor contact points gain subtle local depth without changing camera design, palette direction, scene layout, or portal gameplay logic.

The settings intentionally stay restrained:

| Setting | Value |
|---|---:|
| Downsample | `true` |
| Source | `DepthNormals` |
| AO method | `BlueNoise` |
| Samples | `Low` |
| Blur quality | `Medium` |
| Intensity | `1.15` |
| Radius | `0.03` |
| Direct lighting strength | `0.22` |
| Falloff | `120` |

## Changes

- `Assets/Settings/UniversalRenderPipeline_Renderer.asset`
  - Adds `UnityEngine.Rendering.Universal.ScreenSpaceAmbientOcclusion` as a second renderer feature after `PortalStencilFeature`.
  - Keeps `PortalStencilFeature` ordering intact.

- `Assets/Editor/Stage4RendererFeatureSetup.cs`
  - Adds a rerunnable editor setup method:
    - `Anemora/Review/Apply Stage4 Soft Contact Occlusion Renderer Feature`
    - `Anemora.EditorTools.Stage4RendererFeatureSetup.ApplyStage4SoftContactOcclusion`
  - Uses `SerializedObject` because URP's SSAO settings type is internal.

- `Assets/Tests/EditMode/GraphicsFoundationAssetTests.cs`
  - Adds `UniversalRendererUsesStage4SoftContactOcclusion`.
  - Guards feature presence, active state, and conservative serialized SSAO settings.

## Capture Artifacts

Updated by:

- `Stage4GraphicsBaselineCapture.CaptureAll`
- `Stage4GraphicsBaselineCapture.CaptureMainSceneSoftGradeReview`

Stable SHA-256 hashes:

- `stage4_graphics_baseline_veil_before.png`: `4335EAECF9141A666C2A9F9DD362AFCFE496FBD47A025E81D9541072EBBFABF0`
- `stage4_graphics_baseline_veil_proposed.png`: `A3E6A4F3CC441C6AF1A8E166E32E7B21E5FC4B455A0F85FC2328D91E510685A5`
- `stage4_graphics_baseline_veil_review_sheet.png`: `3DD12B6E4D2B20F46DB2CE5814F8858E13653D023C73A9E1E111C7E0083CFC86`
- `stage4_graphics_postprocess_current.png`: `B19A182949A2706A029AE9EF55A588C049A611220B0295F9305FA116BB435944`
- `stage4_graphics_postprocess_proposed_soft.png`: `B7D4BF26C32A872A585AF90BC3D4CB30DBD7F419F205FB08C2CE9A55888CF3CF`
- `stage4_graphics_postprocess_review_sheet.png`: `DFF7DA3FF91B5BEE24FDFF7C4C6232CC5A72F5B2AC8E2930C98923414B4FDD4A`
- `stage4_main_scene_graphics_current.png`: `49CAF175E13E7EDF7A0FBD04AEF0E230176BEFFF08B3E58583B1C3409B02F46E`
- `stage4_main_scene_graphics_proposed_soft.png`: `B8AF2EF54D9F11DF2FFC98F68238047D960D3CF5F886BD43F7F02F1F63A86E91`
- `stage4_main_scene_graphics_review_sheet.png`: `555EC855C1D08155C8E250940A17DDE3841E3C34E588042BE3D72E9709B76AB5`

## Verification

- Apply renderer feature executeMethod
  - Command: `-executeMethod Anemora.EditorTools.Stage4RendererFeatureSetup.ApplyStage4SoftContactOcclusion`
  - Result: Unity exit code `0`

- CaptureAll executeMethod
  - Command: `-executeMethod Anemora.EditorTools.Stage4GraphicsBaselineCapture.CaptureAll`
  - Result: Unity exit code `0`
  - Checked log patterns: shader error `0`, shader warning `0`, exception `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`

- Main scene graphics capture executeMethod
  - Command: `-executeMethod Anemora.EditorTools.Stage4GraphicsBaselineCapture.CaptureMainSceneSoftGradeReview`
  - Result: Unity exit code `0`
  - Checked log patterns: shader error `0`, shader warning `0`, exception `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`

- Targeted EditMode
  - Command: `-runTests -testPlatform EditMode -testFilter Anemora.Tests.EditMode.GraphicsFoundationAssetTests`
  - Result: `15/15` passed

- Full EditMode
  - Result: `55/55` passed

- Targeted PlayMode
  - `MainSceneStartupLogTests`: `3/3` passed
  - `PortalStencilFeatureSmokeTest`: `1/1` passed

- Windows build smoke
  - Output: `Builds/Stage4Smoke/2026-05-08-graphics-foundation-soft-contact-occlusion/Anemora_Stage4_GraphicsFoundation_SoftContactOcclusion_Smoke.exe`
  - Build folder files: `192`
  - Build folder disk size: `132,287,856` bytes / `126.160 MiB`
  - Unity exit code: `0`
  - Build log marker: `Build Finished, Result: Success.`
  - Build included `Hidden/Universal Render Pipeline/ScreenSpaceAmbientOcclusion`

- Player smoke
  - Ran generated Windows player for 30 seconds at `1280 x 720`, fullscreen off.
  - Process exit `-1` is expected because the smoke window stops the player.
  - Checked player-log patterns: Error `0`, Exception `0`, Assert `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, NullReference `0`, MissingReference `0`, Failed `0`, TextMesh Pro Essential Resources `0`, ScreenSpaceAmbientOcclusion `0`.

## Performance Sample

The SSAO pass also refreshed a 120 second Windows Standalone mixed portal/dialogue sample.

| Metric | Measured value |
|---|---:|
| Build output | `Builds/Stage4Perf/2026-05-08-tier4-ssao/Anemora_Stage4_Tier4SSAO_Perf.exe` |
| Build folder files | `197` |
| Build folder disk size | `132,471,204` bytes / `126.334 MiB` |
| Player exit | `0` |
| External sample rows | `26` |
| Runner duration | `121.642 s` |
| Frame count | `7,285` |
| Average FPS | `59.889` |
| Average frame time | `16.698 ms` |
| p95 frame time | `16.683 ms` |
| p99 frame time | `16.688 ms` |
| Max frame time | `250.005 ms` |
| GC used memory start / end / peak | `0.000 / 4.453 / 4.695 MiB` |
| Total used memory peak | `151.570 MiB` |
| URP `DrawObjectsPass` warning count | `0` |
| Portal open / close / crossing | `30 / 30 / 30` |
| Dialogue trigger attempts / successes | `59 / 1` |
| CPU average / peak | `3.908% / 5.841%` of machine |
| Working set average / peak | `319.488 / 334.305 MiB` |
| Private bytes average / peak | `429.391 / 448.465 MiB` |
| GPU dedicated average / peak | `72.022 / 72.023 MiB` |
| GPU shared average / peak | `33.156 / 33.156 MiB` |

Player log checked patterns all remained `0` for Error, Exception, Assert, DrawObjectsPass, RecordRenderGraph, RenderGraph, NullReference, MissingReference, Failed, TextMesh Pro Essential Resources, and ScreenSpaceAmbientOcclusion.

## Comparison With v0.2

| Metric | Tier 4 graphics v0.2 | Soft contact occlusion | Delta |
|---|---:|---:|---:|
| Build folder disk size | `125.862 MiB` | `126.334 MiB` | `+0.472 MiB` |
| Average FPS | `59.914` | `59.889` | `-0.025` |
| p95 frame time | `16.683 ms` | `16.683 ms` | `0.000 ms` |
| Working set peak | `298.000 MiB` | `334.305 MiB` | `+36.305 MiB` |
| GPU dedicated peak | `61.125 MiB` | `72.023 MiB` | `+10.898 MiB` |
| GPU shared peak | `32.434 MiB` | `33.156 MiB` | `+0.722 MiB` |
| Total used memory peak | `151.474 MiB` | `151.570 MiB` | `+0.096 MiB` |
| URP warning count | `0` | `0` | `0` |

The SSAO pass is accepted for now because p95 frame time remains at the 60 FPS budget and player-log warning state stays clean. The memory/GPU increase is expected for a screen-space effect and should be rechecked after larger environment or character batches.

## Tier 4 Notes

- This is the first Tier 4 pass that adds a built-in URP screen-space effect to production renderer data.
- The settings are deliberately low to avoid a dirty, high-contrast AO look.
- If later scene art becomes denser, the first rollback knob is `Intensity`; the second is disabling the feature, not changing portal stencil ordering.
