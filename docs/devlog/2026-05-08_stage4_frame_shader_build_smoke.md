# Stage 4 frame shader build smoke

Date: 2026-05-08
Scope: GFX-3 Performance / build smoke refresh
Branch: `codex/stage4-graphics-foundation-20260508`

## Summary

Refreshed the Windows build and player-log smoke after adding the dedicated time-volume frame shader and updating the generated graphics baseline review captures.

This is a build / launch / player-log regression check. It is not a new 120 second FPS / memory performance baseline.

## Build Smoke

- Unity: `6000.3.14f1`
- Output: `Builds/Stage4Smoke/2026-05-08-graphics-foundation-frame-shader/Anemora_Stage4_GraphicsFoundation_FrameShader_Smoke.exe`
- Build folder files: `192`
- Build folder disk size: `131,763,452` bytes / `125.659 MiB`
- Command form: `Unity.exe -batchmode -projectPath <worktree> -buildWindows64Player <output-exe> -logFile stage4_gfx_frame_shader_build_smoke.log -quit`
- Unity exit code: `0`
- Build log marker: `Build Finished, Result: Success.`

Build log caveats:

- `Exception` matches are package reflection lines from `com.unity.testtools.codecoverage` / `ReportGeneratorMerged.dll` resolving `System.Numerics` fields.
- `RenderGraph` / `DrawObjectsPass` matches are build size report asset paths, not runtime warnings.

## Player Smoke

- Ran generated Windows player for 30 seconds.
- Resolution: `1280 x 720`
- Fullscreen: off
- Player log: `stage4_gfx_frame_shader_player_smoke.log`
- The process was stopped after the fixed smoke window, so exit code `-1` is expected and is not treated as the pass/fail signal.

Checked player-log patterns:

| Pattern | Count |
|---|---:|
| `Error` | 0 |
| `Exception` | 0 |
| `Assert` | 0 |
| `DrawObjectsPass` | 0 |
| `RecordRenderGraph` | 0 |
| `RenderGraph` | 0 |
| `NullReference` | 0 |
| `MissingReference` | 0 |
| `Failed` | 0 |
| `TextMesh Pro Essential Resources` | 0 |

## Side Effects

Unity build / player execution touched Addressables, URP settings, `DefaultVolumeProfile`, and `ProjectSettings`. These were generated side effects and were restored before staging.

Build output and local logs remain untracked / ignored artifacts.

## Caveats

- Latest 120 second FPS / memory performance baseline remains `docs/devlog/2026-05-07_stage4_performance_baseline_v0_1.md`.
- The dedicated time-volume frame shader is covered by EditMode asset guards and this build/player smoke, but final in-game visual judgement remains manual.
