# Stage 4 time-volume frame shader polish

Date: 2026-05-08
Scope: GFX-2 Stage 4 visual baseline profile / VFX readability polish
Branch: `codex/stage4-graphics-foundation-20260508`

## Summary

Added a dedicated opaque shader for the local time-window frame materials. The pass keeps the existing Current / Past debug colors but removes the flat URP Lit dependency from the frame bars and adds restrained rim and top-light shaping for clearer HD-2D review silhouettes.

## Changes

- `Assets/Art/Materials/Portal/TimeVolumeFrame.shader`
  - Adds a URP `UniversalForward` opaque pass for time-window frame bars.
  - Uses existing base color with conservative ambient, direct, rim, and top-light controls.
  - Avoids transparency, bloom-heavy emission, stencil changes, or camera / palette changes.

- `Assets/Art/Materials/Portal/Debug_Current.mat`
  - Uses `Anemora/Portal/TimeVolumeFrame`.
  - Keeps `_BaseColor = (0.83, 0.72, 0.52, 1)`.

- `Assets/Art/Materials/Portal/Debug_Past.mat`
  - Uses `Anemora/Portal/TimeVolumeFrame`.
  - Keeps `_BaseColor = (0.46, 0.64, 0.88, 1)`.

- `Stage4GraphicsBaselineCapture`
  - Separates transparent veil panels from opaque review frame bars.
  - Adds twelve frame bars around the review time-volume so the frame material is visible in generated baseline PNGs.

- `GraphicsFoundationAssetTests`
  - Adds `TimeVolumeFrameMaterialsUseDedicatedReadableShader`.
  - Guards shader assignment, base colors, and conservative rim / top-light ranges for both frame materials.

## Capture Artifacts

Updated by `Stage4GraphicsBaselineCapture.CaptureAll`:

- `docs/devlog/screenshots/stage4_graphics_baseline_veil_before.png`
- `docs/devlog/screenshots/stage4_graphics_baseline_veil_proposed.png`
- `docs/devlog/screenshots/stage4_graphics_baseline_veil_review_sheet.png`
- `docs/devlog/screenshots/stage4_graphics_postprocess_current.png`
- `docs/devlog/screenshots/stage4_graphics_postprocess_proposed_soft.png`
- `docs/devlog/screenshots/stage4_graphics_postprocess_review_sheet.png`

Stable SHA-256 hashes:

- `stage4_graphics_baseline_veil_before.png`: `F5D78F4AFBBAAC9BA2D961BB0CCCE304A06C21B0D211E292D1FBB4F5E3725061`
- `stage4_graphics_baseline_veil_proposed.png`: `C9B7DC247B4E056E8C431B855742BDF12B933D9D500B842F78A0AAE1DAE892B7`
- `stage4_graphics_baseline_veil_review_sheet.png`: `D79E60958ECF643160D53F7E60511C1626B8E889F55083D728B8A9C48D188471`
- `stage4_graphics_postprocess_current.png`: `0BECACEACB80F85454C7E8434850199A031EFABBE2C6BBA9F2AE985E8D1C253F`
- `stage4_graphics_postprocess_proposed_soft.png`: `1E77A899726ABF2D76BF039CB04F8F65A56C7371A711773FFA1F62E937DB3801`
- `stage4_graphics_postprocess_review_sheet.png`: `18E15825ACD6AE12D684940DD433117955B5E4261F549AB45DA9099B9AA6B5FA`

## Verification

- Targeted EditMode
  - Command: `-runTests -testPlatform EditMode -testFilter Anemora.Tests.EditMode.GraphicsFoundationAssetTests`
  - Result: `5/5` passed
  - Checked patterns: `error CS` `0`, shader error `0`, shader warning `0`, exception `0`, assertion `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, TextMesh Pro Essential Resources `0`

- CaptureAll executeMethod
  - Command: `-executeMethod Anemora.EditorTools.Stage4GraphicsBaselineCapture.CaptureAll`
  - Result: Unity exit code `0`
  - Checked patterns: `error CS` `0`, shader error `0`, shader warning `0`, exception `0`, assertion `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, TextMesh Pro Essential Resources `0`

- Full EditMode
  - Result: `45/45` passed
  - Source scan: EditMode `44`, PlayMode `34`
  - Checked patterns: `error CS` `0`, shader error `0`, shader warning `0`, exception `0`, assertion `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, TextMesh Pro Essential Resources `0`, YAML `0`

## User Decision Items

None. This is limited to existing time-window frame material readability and generated review scenes. It does not apply a production lighting grade, final palette, camera redesign, or stronger post effect.
