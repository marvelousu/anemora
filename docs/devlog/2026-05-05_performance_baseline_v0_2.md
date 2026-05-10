# Performance Baseline v0.2 for G5 Reference

Date: 2026-05-05

## 1. Overview

This is the Stage 3 Day 1 audio-loaded performance baseline v0.2 for `docs/G5_ACCEPTANCE_MATRIX.md` section K.

Measured project state:

| Item | Value |
| --- | --- |
| Repository commit measured | `8bd0d01` (`e6e3c61` audio rebuild plus later docs commits) |
| Baseline comparison | v0.1 devlog commit `2e3569f`, measured project commit `4029cc0` |
| Worktree | `<worktree:Anemora-perf-baseline-v0-2>` |
| Unity | 6000.3.14f1 |
| URP | 17.3.0 |
| Scene | `Assets/Scenes/Anemora_Main.unity` |
| Build output | `<worktree:Anemora-perf-baseline-v0-2>\Builds\PerfBaselineV02\AnemoraPerfBaselineV02.exe` |

Measured PC environment:

| Item | Value |
| --- | --- |
| OS | Microsoft Windows 11 Home 10.0.26200 |
| CPU | AMD Ryzen 5 7430U with Radeon Graphics, 6 cores / 12 logical processors |
| RAM visible to Windows | 14,517,496 KiB (13.84 GiB) |
| GPU | AMD Radeon (TM) Graphics |
| GPU memory reported by CIM | 2.00 GiB AdapterRAM |
| GPU memory reported by Unity D3D log | 7088 MB VRAM |
| GPU driver | 31.0.21914.7005 |

Measurement tools:

- Temporary standalone measurement build with an in-player sampler MonoBehaviour.
- Unity `ProfilerRecorder` counters for memory and main loop timing.
- Windows PowerShell `Get-Process` process metrics.
- Windows `GPU Process Memory` performance counters.
- Player log count for the known URP RenderGraph `DrawObjectsPass` warning.

Temporary measurement assets, bootstrap scene, raw logs, and the build output are measurement artifacts only. They are not intended to be committed.

## 2. Windows Standalone Build

Build method:

- Temporary scenes:
  - `Assets/PerfBaselineV02Temp/G5PerfV02Bootstrap.unity`
  - `Assets/Scenes/Anemora_Main.unity`
- Build target: `StandaloneWindows64`.
- Build options: `BuildOptions.None`.
- The bootstrap scene loaded `Anemora_Main` and started the 120s sampler after the main scene was ready.

Build summary:

| Metric | Measured value |
| --- | ---: |
| Build result | Succeeded |
| Build errors | 0 |
| Build warnings | 0 |
| Build duration | 98.253 s (1.638 min) |
| BuildReport total size | 117.669 MiB |
| Disk size of build folder | 117.853 MiB |

Build folder breakdown:

| Path / file | Size |
| --- | ---: |
| `AnemoraPerfBaselineV02_Data/` | 67.623 MiB |
| `UnityPlayer.dll` | 34.657 MiB |
| `MonoBleedingEdge/` | 8.700 MiB |
| `D3D12/` | 4.506 MiB |
| `UnityCrashHandler64.exe` | 1.547 MiB |
| `AnemoraPerfBaselineV02.exe` | 0.637 MiB |
| `Anemora_BurstDebugInformation_DoNotShip/` | 0.185 MiB |

## 3. 120s Runtime Sample

Runtime method:

- Player launched windowed at 1280 x 720.
- No auto-input was applied; this is an idle audio-loaded sample.
- In-player sampler duration: 120.005 s.
- External process observation duration: 131.261 s.
- External process counter rows: 42. `Get-Counter` calls were slower than 1 Hz on this machine, so CPU / GPU averages are based on 42 rows over the full process lifetime.

Player / scene readiness:

| Metric | Measured value | Notes |
| --- | ---: | --- |
| Player exit code | 0 | Sampler ended with `Application.Quit(0)`. |
| Cold start to main scene ready | 1.403 s | Launch to `Anemora_Main` sampler ready marker. |
| `Anemora_Main` load time | 0.871 s | Temporary bootstrap `LoadSceneAsync` to ready marker. |
| Resolution | 1280 x 720 | Windowed player. |
| Quality level | 5 | Current project quality setting. |
| VSync | 1 | VSync-limited player window. |

FPS / frame timing:

| Metric | Measured value | Notes |
| --- | ---: | --- |
| Average FPS | 59.989 | 7199 frames over 120.005 s. |
| Average frame time | 16.768 ms | From `Time.unscaledDeltaTime`. |
| p95 frame time | 16.683 ms | Meets the 60 FPS frame budget. |
| Minimum frame time | 16.650 ms | Stable VSync cadence. |
| Maximum frame time | 750.008 ms | Startup / scene-settle outlier retained in the continuous sample. |
| Main Thread average | 22.462 ms | `ProfilerRecorder`; includes VSync / frame wait, not pure CPU work. |
| PlayerLoop average | 22.453 ms | Same limitation as main thread timing. |

Process / memory / GPU:

| Metric | Measured value |
| --- | ---: |
| CPU average | 1.833% of machine |
| CPU peak | 5.000% of machine |
| Working set average | 277.301 MiB |
| Working set peak | 290.762 MiB |
| Private bytes average | 380.790 MiB |
| Private bytes peak | 393.984 MiB |
| Paged memory peak | 393.984 MiB |
| GPU dedicated average | 50.504 MiB |
| GPU dedicated peak | 52.664 MiB |
| GPU shared average | 29.543 MiB |
| GPU shared peak | 30.586 MiB |
| Total Used Memory average | 141.026 MiB |
| Total Used Memory peak | 141.609 MiB |
| GC Used Memory average | 2.803 MiB |
| GC Reserved Memory average | 3.320 MiB |
| Texture Memory average | 0.000 MiB |
| Mesh Memory average | 0.000 MiB |
| Audio Used Memory average / peak | 0.000 / 0.000 MiB |

Audio runtime state:

| Check | Result |
| --- | --- |
| `Zone1AudioController` | Present |
| Ambient clip | `Zone1_Ambient` |
| Wind clip | `sfx_env_wind_loop_01` |
| Portal clips | `sfx_time_portal_open_01`, `sfx_time_portal_flip_01` |
| Audio sources | 4 total, 3 playing (`Music_Source`, `Wind_Ambience_Source`, `Pad_Ambience_Source`) |
| Unity `Audio Used Memory` counter | 0.000 MiB; the counter did not report the playing compressed OGG streams in this run. |

URP warning count:

| Metric | Count |
| --- | ---: |
| `DrawObjectsPass does not have an implementation of the RecordRenderGraph method` in v0.2 120s player log | 14,402 |
| Previous audio rebuild 30s player log (`e6e3c61`) | 6,996 |
| Increment from previous 30s sample to this 120s sample | +7,406 |

The warning remains the same known URP RenderGraph compatibility caveat from the earlier G5 automated and audio rebuild runs. This baseline did not change URP settings or custom renderer feature code.

## 4. Stress Sample

Stress sample was skipped.

Reason: this v0.2 pass was scoped as a continuous 120s idle audio-loaded sample with no player auto-input. A deterministic portal open / close harness is not present in the repository, and mixing manual input timing into the baseline would make the memory / FPS comparison less reproducible.

No stress GC allocation or Mono heap delta was recorded.

## 5. v0.1 vs v0.2 Comparison

| Metric | v0.1 (`2e3569f`, audio not integrated) | v0.2 (`8bd0d01`, audio loaded 120s) | Delta / reason |
| --- | ---: | ---: | --- |
| BuildReport total size | 114.871 MiB | 117.669 MiB | +2.798 MiB; audio assets and current project state. |
| Build folder disk size | 115.056 MiB | 117.853 MiB | +2.797 MiB; audio assets and current project state. |
| Cold start to ready | 5.030 s | 1.403 s | Not directly comparable; v0.2 used a warm OS cache and a 1280 x 720 measurement run. |
| `Anemora_Main` load time | 0.190 s | 0.871 s | v0.2 includes current audio scene wiring and temporary sampler run context. |
| Average FPS | 59.909 | 59.989 | Both VSync-limited; v0.1 was 1920 x 1200, v0.2 was 1280 x 720. |
| p95 frame time | 16.683 ms | 16.683 ms | Stable 60 FPS frame budget. |
| CPU average / peak | 2.081% / 3.029% | 1.833% / 5.000% | Average remains low; peak is a short process sample spike. |
| Working set average / peak | 217.350 / 225.633 MiB | 277.301 / 290.762 MiB | +59.951 / +65.129 MiB; audio-loaded project state plus temporary sampler build. |
| Private bytes average / peak | Not recorded | 380.790 / 393.984 MiB | New v0.2 metric. |
| Paged memory peak | Not recorded | 393.984 MiB | New v0.2 metric; Windows process paged-memory value. |
| GPU dedicated peak | 78.430 MiB | 52.664 MiB | Lower in v0.2; resolution / graphics API / window state differ. |
| GPU shared peak | 41.598 MiB | 30.586 MiB | Lower in v0.2; same comparability caveat. |
| Total Used Memory average / peak | 104.736 / 105.150 MiB | 141.026 / 141.609 MiB | +36.290 / +36.459 MiB. |
| GC Used Memory average | 3.143 MiB | 2.803 MiB | No idle GC growth observed. |
| Audio Used Memory average / peak | 0.000 / not recorded | 0.000 / 0.000 MiB | Unity counter reported 0 in both runs; audio sources were playing in v0.2. |

Additional comparison with the previous audio rebuild 30s sample (`e6e3c61`):

| Metric | Audio rebuild 30s | v0.2 120s | Delta / reason |
| --- | ---: | ---: | --- |
| Working set average / peak | 212.008 / 217.246 MiB | 277.301 / 290.762 MiB | +65.293 / +73.516 MiB; v0.2 is an instrumented measurement build. |
| CPU average / peak | 1.313% / 2.077% | 1.833% / 5.000% | Higher peak in v0.2; longer and instrumented sample. |
| GPU dedicated average / peak | 30.950 / 31.539 MiB | 50.504 / 52.664 MiB | Higher v0.2 GPU counter values; same 1280 x 720 target, different measurement build. |
| GPU shared average / peak | 19.425 / 19.535 MiB | 29.543 / 30.586 MiB | Higher v0.2 GPU counter values. |
| URP warning count | 6,996 in 30s | 14,402 in 120s | Same warning text; still repeating in player log. |
| FPS | Not remeasured | 59.989 average, 16.683ms p95 | v0.2 closes the FPS measurement gap. |

## 6. G5 Acceptance Matrix Section K Reflection

Use v0.2 as the final Stage 3 automated section K idle baseline:

| K item | v0.2 baseline value | Status |
| --- | --- | --- |
| K-01 Build success / launch | Build succeeded; disk size 117.853 MiB; cold start to ready 1.403 s; main scene load 0.871 s. | Pass |
| K-02 FPS target | Average 59.989 FPS, p95 16.683ms at 1280 x 720, VSync on. | Pass for idle 60 FPS target; startup outlier retained. |
| K-03 Memory / VRAM | Working set peak 290.762 MiB; GPU dedicated peak 52.664 MiB; GPU shared peak 30.586 MiB; Total Used Memory peak 141.609 MiB. | Pass for Stage 3 idle baseline. |

Manual G5 sections should still verify audio listening, UI visibility, 5-8 minute playthrough, and layer-2 hint presentation with this build family.

## 7. Conclusion

Baseline status: full for the requested audio-loaded 120s idle sample, partial for stress testing.

The final Stage 3 section K idle numbers are:

- Average FPS: 59.989.
- p95 frame time: 16.683 ms.
- Build folder size: 117.853 MiB.
- Working set peak: 290.762 MiB.
- GPU dedicated / shared peaks: 52.664 / 30.586 MiB.
- URP `DrawObjectsPass` warning count: 14,402 over the v0.2 player log.

The main follow-up risk is still the repeated URP RenderGraph warning. No idle FPS, build-size, or memory blocker was found in this v0.2 sample.
