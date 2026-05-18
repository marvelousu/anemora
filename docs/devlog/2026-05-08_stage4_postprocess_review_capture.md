# Stage 4 Post-Process Review Capture

Date: 2026-05-08

## Summary

Extended the Stage 4 graphics baseline capture automation with a review-only post-process comparison. The proposed look is intentionally restrained: a small warm color filter, mild contrast / saturation lift, very low bloom, and a soft vignette.

This does not apply any post-process change to `Anemora_Main`, renderer assets, URP assets, prefabs, or runtime materials. A later graphics-foundation pass promoted the same values into a review-only VolumeProfile asset under `Assets/Settings/`; it remains unassigned to production scenes.

## Changes

- `Assets/Editor/Stage4GraphicsBaselineCapture.cs`
  - Adds current/proposed post-process capture outputs.
  - Creates a review-only `VolumeProfile` for the proposed review shot.
  - Enables URP camera post-processing only for the proposed review capture camera.
  - Applies a screenshot-only CPU soft-grade preview after `ReadPixels` so batchmode review PNGs visibly reflect the restrained profile values.
  - Builds a side-by-side post-process review sheet from the two individual PNGs.
  - Uses a copied veil material with `_PulseSpeed = 0` for deterministic review captures; the committed runtime material remains animated.

## Proposed Review Volume

- `ColorAdjustments.postExposure`: `0.04`
- `ColorAdjustments.contrast`: `6`
- `ColorAdjustments.saturation`: `3`
- `ColorAdjustments.colorFilter`: warm off-white
- `Bloom.threshold`: `1.18`
- `Bloom.intensity`: `0.08`
- `Bloom.scatter`: `0.42`
- `Vignette.intensity`: `0.06`
- `Vignette.smoothness`: `0.28`

These are comparison values only. Stronger bloom, stronger vignette, final palette choices, and production Volume application still require user approval.

## Captures

- `docs/devlog/screenshots/stage4_graphics_postprocess_current.png`
- `docs/devlog/screenshots/stage4_graphics_postprocess_proposed_soft.png`
- `docs/devlog/screenshots/stage4_graphics_postprocess_review_sheet.png`

Regenerated deterministic veil review captures:

- `docs/devlog/screenshots/stage4_graphics_baseline_veil_proposed.png`
- `docs/devlog/screenshots/stage4_graphics_baseline_veil_review_sheet.png`

## Verification

Executed:

```powershell
Start-Process -FilePath "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -ArgumentList @(
  "-batchmode",
  "-projectPath", "<worktree>",
  "-executeMethod", "Anemora.EditorTools.Stage4GraphicsBaselineCapture.CaptureAll",
  "-logFile", "<worktree>\stage4_gfx_postprocess_capture.log",
  "-quit"
) -Wait -PassThru -WindowStyle Hidden
```

Result:

- Unity exit code: `0`
- C# compile: no `error CS` match
- Shader compile: no shader error match
- Log scan: no `TextMesh Pro Essential Resources`, `DrawObjectsPass`, `RecordRenderGraph`, unhandled exception, or assertion match
- Post-process PNG outputs created at `1920 x 1080`
- Repeat capture: the regenerated veil proposed / veil review / post-process current / post-process proposed / post-process review PNG hashes stayed unchanged after `_PulseSpeed = 0` was applied to the review-only material copy.
- Later main-scene review capture work added a screenshot-only CPU soft-grade preview, so proposed post-process PNG hashes were regenerated after this original capture pass.

## Caveats

- The proposed review volume is a review asset only and is not assigned to `Anemora_Main`.
- The CPU soft-grade preview is a review artifact for offscreen PNG comparison; it is not a runtime rendering path.
- The sheet is a review artifact, not an approval gate by itself.
- No Windows standalone player-log smoke was run for this review-only capture change.

## Next Graphics Foundation Tasks

- If the soft-grade direction reads well, capture it against a more production-like scene before applying any production Volume.
- Continue warning cleanup and player-log smoke before applying any visual profile to `Anemora_Main`.
