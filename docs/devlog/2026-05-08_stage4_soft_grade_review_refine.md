# Stage 4 soft-grade review refine

Date: 2026-05-08
Scope: GFX-2 Stage 4 visual baseline profile
Branch: `codex/stage4-graphics-foundation-20260508`

## Summary

Refined the review-only Stage 4 soft-grade profile by a small amount. The change slightly increases review contrast, saturation, bloom softness, and vignette strength while keeping the profile unassigned to `Anemora_Main`.

This remains a review artifact. It does not apply a production grade, final palette, camera change, or stronger in-game post effect.

## Profile Values

- `ColorAdjustments.postExposure`: `0.05`
- `ColorAdjustments.contrast`: `7`
- `ColorAdjustments.saturation`: `4`
- `ColorAdjustments.colorFilter`: warm off-white `(1.0, 0.985, 0.95, 1.0)`
- `Bloom.threshold`: `1.16`
- `Bloom.intensity`: `0.10`
- `Bloom.scatter`: `0.44`
- `Vignette.intensity`: `0.07`
- `Vignette.smoothness`: `0.30`

## Capture Artifacts

Updated by `Stage4GraphicsBaselineCapture.CaptureAll`:

- `docs/devlog/screenshots/stage4_graphics_postprocess_proposed_soft.png`
- `docs/devlog/screenshots/stage4_graphics_postprocess_review_sheet.png`

Updated by `Stage4GraphicsBaselineCapture.CaptureMainSceneSoftGradeReview`:

- `docs/devlog/screenshots/stage4_main_scene_graphics_current.png`
- `docs/devlog/screenshots/stage4_main_scene_graphics_proposed_soft.png`
- `docs/devlog/screenshots/stage4_main_scene_graphics_review_sheet.png`

Stable SHA-256 hashes:

- `stage4_graphics_postprocess_proposed_soft.png`: `96D2B0A7D6575A5269950E807576E89E3D850DA8847999C2BA4E3F4E9C6187C1`
- `stage4_graphics_postprocess_review_sheet.png`: `AD77649BF362E3B9FF44AEEBAE7E126F5BED89B78CC21C5376A5149CAD8CB01A`
- `stage4_main_scene_graphics_current.png`: `EB65BFA7D915FAE7CF50D39CAC9BEB5C221AF9F54ADE0D56E7FD48C7E4F78E4F`
- `stage4_main_scene_graphics_proposed_soft.png`: `C47C61302ACE3B707B302C17DF6F03DAA843EF9F9B2202D47A0877FE52A340A5`
- `stage4_main_scene_graphics_review_sheet.png`: `9FD95150D71CE197D6C01950A760C7D6401FE7186E4395B9ECD0C741C76A986D`

## Verification

- Profile create executeMethod
  - Command: `-executeMethod Anemora.EditorTools.Stage4GraphicsBaselineCapture.CreateOrUpdateSoftReviewVolumeProfileAsset`
  - Result: Unity exit code `0`

- CaptureAll executeMethod
  - Command: `-executeMethod Anemora.EditorTools.Stage4GraphicsBaselineCapture.CaptureAll`
  - Result: Unity exit code `0`

- Main-scene capture executeMethod
  - Command: `-executeMethod Anemora.EditorTools.Stage4GraphicsBaselineCapture.CaptureMainSceneSoftGradeReview`
  - Result: Unity exit code `0`

- Targeted EditMode
  - Command: `-runTests -testPlatform EditMode -testFilter Anemora.Tests.EditMode.GraphicsFoundationAssetTests`
  - Result: `8/8` passed

- Full EditMode
  - Result: `48/48` passed
  - Source scan: EditMode `47`, PlayMode `34`

- Targeted PlayMode
  - Command: `-runTests -testPlatform PlayMode -testFilter Anemora.Tests.PlayMode.MainSceneStartupLogTests`
  - Result: `3/3` passed

Checked log patterns across the profile, capture, full EditMode, and targeted PlayMode runs: `error CS` `0`, shader error `0`, shader warning `0`, exception `0`, assertion `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, TextMesh Pro Essential Resources `0`, YAML `0`, NullReference `0`, MissingReference `0`.

## User Decision Items

- Production use of the soft-grade profile is still approval-gated.
- Stronger bloom, palette separation, or final Current / Past / Future grade decisions remain user-review items.
