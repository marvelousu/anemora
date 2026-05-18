# Stage 4 Graphics Foundation Build Smoke

Date: 2026-05-08

## Summary

Refreshed the Windows build and player-log smoke after the Stage 4 graphics foundation changes:

- portal flash post-process camera guard
- time-window veil shader
- TMP Settings asset-version guard
- graphics / post-process review capture automation

This does not add a new performance baseline. It is a build / launch / player-log regression check.

## Build Smoke

- Unity: `6000.3.14f1`
- Branch: `codex/stage4-graphics-foundation-20260508`
- Output: `Builds/Stage4Smoke/2026-05-08-graphics-foundation/Anemora_Stage4_GraphicsFoundation_Smoke.exe`
- Command form: `Unity.exe -batchmode -projectPath <worktree> -buildWindows64Player <output-exe> -logFile stage4_gfx_build_smoke.log -quit`
- Unity exit code: `0`
- Build log marker: `Build Finished, Result: Success.`

## Player Smoke

- Ran generated Windows player for 30 seconds.
- Resolution: `1280 x 720`
- Fullscreen: off
- Player log: `stage4_gfx_player_smoke.log`
- The process was stopped after the fixed 30 second smoke window, so the process exit code is not treated as the pass/fail signal.

Checked player-log patterns:

| Pattern | Count |
|---|---:|
| `Error` | 0 |
| `Exception` | 0 |
| `Assert` | 0 |
| `DrawObjectsPass` | 0 |
| `RenderGraph` | 0 |
| `NullReference` | 0 |
| `MissingReference` | 0 |
| `Failed` | 0 |
| `TextMesh Pro Essential Resources` | 0 |

## Side Effects

Unity build / player execution touched Addressables, URP settings, `DefaultVolumeProfile`, and `ProjectSettings`. These were generated side effects and were restored before staging.

Build output and local logs remain untracked / ignored artifacts.

## Caveats

- This is not a 120 second FPS / memory performance baseline. The latest performance baseline remains `docs/devlog/2026-05-07_stage4_performance_baseline_v0_1.md`.
- Build log contains Unity startup / licensing noise and package reflection text; player-log checked patterns are the runtime regression signal here.
