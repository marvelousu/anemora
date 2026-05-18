# Stage 4 Time-Window Veil Shader Polish

Date: 2026-05-08

## Summary

Replaced the flat URP Unlit `TimeVolume_SpaceVeil` material with a lightweight URP transparent shader and added repeatable review captures for the before/proposed veil read.

This stays within HD-2D Tier 2 polish. It does not change camera design, scene layout, time-window gameplay logic, palette direction, or final Zone1 lighting mood.

## Changes

- `Assets/Art/Materials/Portal/TimeVolumeVeil.shader`
  - Adds a transparent URP pass for the local time-window veil.
  - Uses view-angle edge tint and a subtle world-space band to make the window boundary read less flat while staying single-pass and texture-free.
  - Keeps `_BaseColor` / `_Color` compatibility with `TimeWindowDiorama` material property blocks.
- `Assets/Art/Materials/Portal/TimeVolume_SpaceVeil.mat`
  - Switched from URP Unlit to `Anemora/Portal/TimeVolumeVeil`.
  - Added reviewable defaults for edge color, edge strength, band scale, band strength, and pulse speed.
- `Assets/Editor/Stage4GraphicsBaselineCapture.cs`
  - Adds a temporary-scene capture path for the veil visual baseline.
  - Menu item: `Anemora/Review/Capture Stage4 Graphics Baseline`
  - Batch entry point: `Anemora.EditorTools.Stage4GraphicsBaselineCapture.CaptureAll`
- `Assets/Tests/EditMode/GraphicsFoundationAssetTests.cs`
  - Adds an EditMode guard that keeps `TimeVolume_SpaceVeil.mat` on `Anemora/Portal/TimeVolumeVeil` with the expected edge / band properties.

## Captures

- `docs/devlog/screenshots/stage4_graphics_baseline_veil_before.png`
  - Synthetic review scene using a flat URP Unlit transparent veil.
- `docs/devlog/screenshots/stage4_graphics_baseline_veil_proposed.png`
  - Same review scene using `TimeVolume_SpaceVeil.mat` and the new shader.
- `docs/devlog/screenshots/stage4_graphics_baseline_veil_review_sheet.png`
  - Side-by-side before/proposed review sheet.

All captures are `1920 x 1080`. The capture scene is temporary and unsaved; no production scene or prefab is written by the automation.

## Verification

Executed:

```powershell
Start-Process -FilePath "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -ArgumentList @(
  "-batchmode",
  "-projectPath", "<worktree>",
  "-executeMethod", "Anemora.EditorTools.Stage4GraphicsBaselineCapture.CaptureAll",
  "-logFile", "<worktree>\stage4_gfx_baseline_capture.log",
  "-quit"
) -Wait -PassThru -WindowStyle Hidden
```

Result:

- Unity exit code: `0`
- Shader import: `TimeVolumeVeil.shader` imported successfully
- Capture outputs: before / proposed / review sheet PNGs created
- PNG dimensions: `1920 x 1080`
- Log scan: no shader compile, C# compile, runtime exception, or assertion failure found
- Expected transient Unity licensing / socket startup messages appeared before successful batchmode quit
- Unity-generated Addressables / ProjectSettings side effects were restored before staging

Targeted EditMode:

```powershell
Start-Process -FilePath "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -ArgumentList @(
  "-batchmode",
  "-projectPath", "<worktree>",
  "-runTests",
  "-testPlatform", "EditMode",
  "-testFilter", "Anemora.Tests.EditMode.GraphicsFoundationAssetTests",
  "-testResults", "<worktree>\stage4_gfx_veil_editmode.xml",
  "-logFile", "<worktree>\stage4_gfx_veil_editmode.log"
) -Wait -PassThru -WindowStyle Hidden
```

Result:

- Unity exit code: `0`
- `GraphicsFoundationAssetTests`: `1/1 passed`
- Log scan: no `error CS`, shader error, unhandled exception, or assertion match

Full suite refresh:

- EditMode: `40/40 passed`
- PlayMode: `33 passed / 34 total`, with one manual capture skipped
- Unity exit code: `0` for both runs
- No `DrawObjectsPass` / `RecordRenderGraph`, shader compile, or C# compile matches in the final full-suite logs
- Full PlayMode still emits the pre-existing TMP Essential Resources missing Editor shutdown error; this is unrelated to the veil shader but remains a graphics/UI foundation cleanup candidate.

## Caveats

- This shader is intentionally lightweight. It does not attempt a full refractive portal, depth-aware distortion, screen-space mask, bloom-heavy edge, or major color-grade shift.
- The current material defaults should be reviewed in captures before any stronger art-direction move is applied to production scenes.
- No Windows standalone build or player-log smoke has been run after this shader polish yet.
- Full PlayMode passes, but the Editor log still contains the existing TMP Essential Resources missing shutdown error.

## Next Graphics Foundation Tasks

- Remove the TMP Essential Resources missing Editor shutdown error.
- Build a second baseline sheet for post-process / volume profile tuning after the veil read is accepted.
