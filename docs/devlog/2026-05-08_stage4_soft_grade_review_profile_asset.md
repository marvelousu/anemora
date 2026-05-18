# Stage 4 soft-grade review profile asset

Date: 2026-05-08
Scope: GFX-2 Stage 4 visual baseline profile
Branch: `codex/stage4-graphics-foundation-20260508`

## Summary

The restrained post-process comparison values now exist as a review-only VolumeProfile asset. This makes the soft-grade baseline inspectable and tweakable in Unity without applying it to `Anemora_Main` or changing production lighting.

## Asset

- Path: `Assets/Settings/Stage4SoftGradeReviewVolumeProfile.asset`
- GUID: `4ce103b2c50f93c40b2701579eb9f1bb`
- Components:
  - `ColorAdjustments`
  - `Bloom`
  - `Vignette`

## Values

- `ColorAdjustments.postExposure`: `0.05`
- `ColorAdjustments.contrast`: `7`
- `ColorAdjustments.saturation`: `4`
- `ColorAdjustments.colorFilter`: warm off-white `(1.0, 0.985, 0.95, 1.0)`
- `Bloom.threshold`: `1.16`
- `Bloom.intensity`: `0.10`
- `Bloom.scatter`: `0.44`
- `Vignette.intensity`: `0.07`
- `Vignette.smoothness`: `0.30`

## Automation

- `Stage4GraphicsBaselineCapture.CaptureAll`
  - Creates or updates the review profile before capture.
  - Uses the asset profile for the proposed post-process review shot.
  - Still renders only temporary unsaved scenes for screenshot capture.

- `Stage4GraphicsBaselineCapture.CreateOrUpdateSoftReviewVolumeProfileAsset`
  - Creates or updates only the review profile asset.

## Verification

- Review profile create executeMethod
  - Command: `-executeMethod Anemora.EditorTools.Stage4GraphicsBaselineCapture.CreateOrUpdateSoftReviewVolumeProfileAsset`
  - Result: Unity exit code `0`
  - Checked patterns: `error CS` `0`, shader error `0`, shader warning `0`, exception `0`, assertion `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, TextMesh Pro Essential Resources `0`

- Targeted EditMode
  - Command: `-runTests -testPlatform EditMode -testFilter Anemora.Tests.EditMode.GraphicsFoundationAssetTests`
  - Result: `3/3` passed
  - Guarded: review profile exists, expected restrained values are present, and `Anemora_Main.unity` does not contain the review profile GUID.

- Full EditMode
  - Result: `43/43` passed
  - Checked patterns: `error CS` `0`, shader error `0`, shader warning `0`, exception `0`, assertion `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, TextMesh Pro Essential Resources `0`

- Capture executeMethod
  - Command: `-executeMethod Anemora.EditorTools.Stage4GraphicsBaselineCapture.CaptureAll`
  - Result: Unity exit code `0`
  - Checked patterns: `error CS` `0`, shader error `0`, shader warning `0`, exception `0`, assertion `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, TextMesh Pro Essential Resources `0`
  - Later main-scene review capture work added a screenshot-only CPU soft-grade preview for proposed PNGs, so proposed post-process review hashes are intentionally regenerated after this profile-asset pass.

## User Decision Items

Production application still requires approval. This asset is a review baseline only; it is not assigned to `Anemora_Main`, URP assets, prefabs, or runtime objects.
