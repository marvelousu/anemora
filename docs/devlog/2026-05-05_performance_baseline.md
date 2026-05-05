# Performance Baseline for G5 Reference

Date: 2026-05-05

## 1. Overview

Stage 3 Day 1 late-baseline measurement for `docs/G5_ACCEPTANCE_MATRIX.md` §K. The measured project commit was `4029cc0` in a temporary worktree:

`C:\Users\maro6\Documents\Unity\Anemora-perf-baseline`

Only this devlog is intended for commit. Measurement scripts, generated bootstrap scene, build output, and raw result logs were temporary measurement artifacts and are not part of the repository baseline.

| Item | Value |
| --- | --- |
| Unity | 6000.3.14f1 (`d68c3f99a318`) |
| URP | 17.3.0 |
| Scene | `Assets/Scenes/Anemora_Main.unity` |
| OS | Microsoft Windows 11 Home 10.0.26200 |
| CPU | AMD Ryzen 5 7430U with Radeon Graphics, 6 cores / 12 logical processors |
| RAM visible to Windows | 14,517,496 KiB (13.84 GiB) |
| GPU | AMD Radeon (TM) Graphics |
| GPU memory reported by CIM | 2.00 GiB AdapterRAM |
| GPU memory reported by Unity D3D log | 7088 MB VRAM |
| GPU driver | 31.0.21914.7005 |

Measurement tools used:

- Unity PlayMode Test Runner with a temporary sampler MonoBehaviour.
- Unity `ProfilerRecorder` counters.
- Unity `BuildReport`.
- Windows process counters through PowerShell `Get-Process` and `Get-Counter`.
- Windows GPU Process Memory / GPU Engine performance counters.

Important measurement limitation:

- GUI Editor Stats overlay measurement could not be kept alive through command-line `-executeMethod` in this environment. Editor numbers below are PlayMode Test Runner / batchmode measurements, not a visual Game View Stats overlay capture.
- Standalone runtime numbers were measured in a visible Windows player window.

## 2. Editor Measurement

Editor PlayMode measurement method:

- Command-line PlayMode Test Runner.
- Temporary test: 1 test, 301.197984 s duration, passed.
- Sampler duration: 300.004 s.
- Resolution reported by `Screen`: 640 x 480.
- Quality level: 5.

| Metric | Measured value | Notes |
| --- | ---: | --- |
| Average FPS | 1595.702 | Batchmode PlayMode, uncapped and not representative of visual Game View rendering. |
| Minimum FPS | 25.213 | Includes startup / first-frame noise. |
| Maximum FPS | 2985.908 | Batchmode only. |
| Average frame time | 0.627 ms | From `Time.unscaledDeltaTime`. |
| p95 frame time | 0.931 ms | From sampled frame deltas. |
| CPU process average | 18.827% of machine | External process counter. |
| CPU process peak | 24.933% of machine | External process counter. |
| Main Thread average | 0.624 ms | `ProfilerRecorder` counter, converted from ns. |
| PlayerLoop average | 0.516 ms | `ProfilerRecorder` counter, converted from ns. |
| Render Thread | Not measured | Counter was not available in this run. |
| Physics FixedUpdate average | 0.0002 ms | Counter was effectively zero for this scene. |
| GPU frame time | Not measured | `FrameTimingManager` did not return GPU timings in this batchmode run. |

Memory:

| Metric | Measured value |
| --- | ---: |
| Total Used Memory average | 837.986 MiB |
| Total Used Memory peak | 888.438 MiB |
| GC Used Memory average | 602.877 MiB |
| Texture Memory average | 82.637 MiB |
| Mesh Memory average | 0.093 MiB |
| Audio Used Memory average | 1.242 MiB |
| Editor process Working Set average | 1470.126 MiB |
| Editor process Working Set peak | 1569.988 MiB |

Render counters:

| Metric | Measured value | Notes |
| --- | ---: | --- |
| Draw Calls | 0 | Batchmode PlayMode did not provide visual render counters. |
| Batches | 0 | Same limitation. |
| SetPass Calls | 0 | Same limitation. |
| Triangles | 0 | Same limitation. |
| Vertices | 0 | Same limitation. |

## 3. Windows Standalone Build Results

Build method:

- Temporary measurement build scenes:
  - `Assets/PerfBaselineTemp/GeneratedPerfBootstrap.unity`
  - `Assets/Scenes/Anemora_Main.unity`
- Build target: `StandaloneWindows64`.
- Build options: `BuildOptions.None`.
- The bootstrap scene was used only to measure `SceneManager.LoadSceneAsync("Anemora_Main")` load time.

Build summary:

| Metric | Measured value |
| --- | ---: |
| Build result | Succeeded |
| Build errors | 0 |
| Build warnings | 0 |
| Build duration | 70.105 s (1.168 min) |
| BuildReport total size | 114.871 MiB |
| Disk size of build folder | 115.056 MiB |

Build folder breakdown:

| Path / file | Size |
| --- | ---: |
| `AnemoraPerfBaseline_Data/` | 64.825 MiB |
| `UnityPlayer.dll` | 34.657 MiB |
| `MonoBleedingEdge/` | 8.700 MiB |
| `D3D12/` | 4.506 MiB |
| `UnityCrashHandler64.exe` | 1.547 MiB |
| `AnemoraPerfBaseline.exe` | 0.637 MiB |
| `Anemora_BurstDebugInformation_DoNotShip/` | 0.185 MiB |

`AnemoraPerfBaseline_Data/` internal breakdown:

| Path / file | Size |
| --- | ---: |
| `Managed/` | 32.641 MiB |
| `sharedassets1.assets` | 20.647 MiB |
| `Resources/` | 6.027 MiB |
| `globalgamemanagers.assets.resS` | 2.686 MiB |
| `sharedassets1.assets.resS` | 0.846 MiB |
| `sharedassets0.assets.resS` | 0.500 MiB |
| `Plugins/` | 0.426 MiB |

IL2CPP and StreamingAssets:

| Item | Result |
| --- | --- |
| IL2CPP directory | Not present; this build used the current Mono player configuration. |
| `StreamingAssets/` | Not present in the build output. |

Top 20 packed assets by BuildReport:

| Rank | Asset | Size |
| ---: | --- | ---: |
| 1 | `Assets/UI/Localization/Fonts/Anemora_JP_Atlas.asset` | 16.000 MiB |
| 2 | `Assets/UI/Localization/Fonts/Anemora_EN_Atlas.asset` | 4.000 MiB |
| 3 | Built-in Texture2D: Splash Screen Unity Logo | 2.667 MiB |
| 4 | `Assets/Art/Models/Zone1/Textures/Book_Family_Past_0_Image_0.png` | 0.667 MiB |
| 5 | `Assets/UI/Localization/Fonts/Anemora_JP.asset` | 0.543 MiB |
| 6 | `Resources/unity_builtin_extra` | 0.513 MiB |
| 7 | `Packages/com.unity.render-pipelines.core/Runtime/Debugging/Prefabs/Fonts/PerfectDOSVGA437.ttf` | 0.078 MiB |
| 8 | `Packages/com.unity.render-pipelines.core/Runtime/Vrs/Shaders/VrsTexture.compute` | 0.067 MiB |
| 9 | `Packages/com.unity.render-pipelines.universal/Shaders/Utils/CoreBlit.shader` | 0.064 MiB |
| 10 | `Packages/com.unity.render-pipelines.core/Runtime/Debugging/Prefabs/Widgets/DebugUIHandlerRenderingLayerField.prefab` | 0.058 MiB |
| 11 | `Packages/com.unity.render-pipelines.core/Runtime/Debugging/Prefabs/Widgets/DebugUIBitField.prefab` | 0.058 MiB |
| 12 | `Packages/com.unity.render-pipelines.universal/Shaders/Lit.shader` | 0.033 MiB |
| 13 | `Assets/Art/Sprites/Hero/v1/hero_walk_right.png` | 0.026 MiB |
| 14 | `Assets/Art/Sprites/NPC/Resident_B/v1/resident_b_idle.png` | 0.026 MiB |
| 15 | `Assets/Art/Sprites/Hero/v1/hero_walk_back.png` | 0.026 MiB |
| 16 | `Assets/Art/Sprites/NPC/Resident_A/v1/resident_a_walk_front.png` | 0.026 MiB |
| 17 | `Assets/Art/Sprites/NPC/Resident_A/v1/resident_a_idle.png` | 0.026 MiB |
| 18 | `Assets/Art/Sprites/Hero/v1/hero_walk_front.png` | 0.026 MiB |
| 19 | `Assets/Art/Sprites/Hero/v1/hero_idle.png` | 0.026 MiB |
| 20 | `Packages/com.unity.render-pipelines.universal/Shaders/Terrain/WavingGrass.shader` | 0.021 MiB |

Standalone runtime measurement:

- Sampler duration: 300.018 s.
- External process observation duration: 306.139 s.
- Resolution: 1920 x 1200.
- Quality level: 5.

| Metric | Measured value | Notes |
| --- | ---: | --- |
| Cold start to sampler ready | 5.030 s | Launch to `Anemora_Main` sampler ready marker. |
| `Anemora_Main` load time | 0.190 s | Temporary bootstrap `LoadSceneAsync` to main-scene sampler first frame. |
| Average FPS | 59.909 | VSync-limited player window. |
| Minimum FPS | 4.138 | Includes startup / first-frame noise. |
| Maximum FPS | 60.060 | VSync-limited. |
| Average frame time | 16.692 ms | From sampled frame deltas. |
| p95 frame time | 16.683 ms | Stable at 60 FPS budget. |
| CPU process average | 2.081% of machine | External process counter. |
| CPU process peak | 3.029% of machine | External process counter. |
| Main Thread average | 16.664 ms | Includes VSync / frame wait; not pure CPU work. |
| PlayerLoop average | 16.653 ms | Includes VSync / frame wait. |
| CPU frame time | Not measured | `FrameTimingManager` did not return CPU frame timing in the player. |
| GPU frame time | Not measured | `FrameTimingManager` did not return GPU frame timing in the player. |
| Physics time | Not measured | Physics counter was not available in the player run. |

Standalone memory / GPU:

| Metric | Measured value |
| --- | ---: |
| Total Used Memory average | 104.736 MiB |
| Total Used Memory peak | 105.150 MiB |
| GC Used Memory average | 3.143 MiB |
| GC Reserved Memory average | 4.441 MiB |
| Audio Used Memory average | 0.000 MiB |
| Process Working Set average | 217.350 MiB |
| Process Working Set peak | 225.633 MiB |
| GPU dedicated memory average | 78.412 MiB |
| GPU dedicated memory peak | 78.430 MiB |
| GPU shared memory average | 32.551 MiB |
| GPU shared memory peak | 41.598 MiB |
| GPU 3D utilization average | 11.732% |
| GPU 3D utilization peak | 14.417% |

Standalone render counters:

| Metric | Average | Peak / last |
| --- | ---: | ---: |
| Draw Calls | 7.999 | 8 |
| Batches | 7.999 | 8 |
| SetPass Calls | 4.999 | 5 |
| Triangles | 62.743 | 63 |
| Vertices | 122.736 | 123 |

Startup disk I/O probe:

| Metric | Measured value | Notes |
| --- | ---: | --- |
| Probe duration | 20.674 s | Separate high-frequency startup probe. |
| Ready marker in probe | 3.742 s | Warm OS cache after previous build/run. |
| Peak process read rate | 34.437 KiB/s | Windows `\Process(...)\IO Read Bytes/sec`; likely warm-cache limited. |

## 4. Stress Test Results

Stress tests were skipped in this baseline pass.

Reason: this task focused on build/runtime numeric baseline collection. Boundary flip, dialog repeat, and ActionRecord repeat stress need a separate deterministic interaction harness to avoid mixing manual input timing with the baseline metrics.

## 5. Observations / Notes

- Standalone average FPS was 59.909 at 1920 x 1200, quality level 5. This meets the 60 FPS target as a VSync-limited average.
- Standalone minimum FPS was 4.138 because startup / first-frame samples were included. This does not prove a gameplay-frame drop below 30 FPS; it means warm gameplay minimum needs a G5 interaction pass with startup samples excluded.
- Editor PlayMode batchmode produced very high FPS and zero visual render counters. Treat Editor numbers as script/runtime overhead baseline only, not visual rendering baseline.
- GPU dedicated memory peaked at 78.430 MiB, with shared memory peak 41.598 MiB. This is within the measured adapter budget.
- The two TMP atlases dominate packed asset size: Japanese atlas 16.000 MiB plus English atlas 4.000 MiB, matching the expected roughly 20 MiB band.
- Runtime audio memory was 0.000 MiB in the standalone measurement. BGM/SFX integration has not affected this baseline yet.
- Build folder size was 115.056 MiB, well below a 500 MiB Stage 3 reference threshold.
- Largest build-size hot spot is currently font atlas data, not meshes or audio.
- Runtime render counters are extremely low in the measured camera state: 8 draw calls, 63 triangles, 123 vertices. G5 should remeasure after final scene composition and audio integration.
- Existing URP RenderGraph warnings were observed in prior test/build logs for custom render passes; this baseline did not edit URP settings.

## 6. G5_ACCEPTANCE_MATRIX §K Reflection

This devlog records baseline values only. `docs/G5_ACCEPTANCE_MATRIX.md` was not edited in this task.

| G5 §K item | Baseline value | Current status for G5 |
| --- | --- | --- |
| 60 FPS target | Standalone average 59.909 FPS at 1920 x 1200 | Achieved as average / VSync-limited baseline. |
| Minimum 30 FPS | Standalone minimum 4.138 FPS with startup samples included | Not fully assessed; remeasure warm gameplay in G5. |
| VRAM expected range | GPU dedicated peak 78.430 MiB, shared peak 41.598 MiB | OK for current Stage 3 content. |
| Build size expected range | Build folder 115.056 MiB | OK under 500 MiB reference threshold. |
| Cold start | 5.030 s to main-scene sampler ready | Baseline recorded. |
| Scene load | 0.190 s bootstrap to `Anemora_Main` ready | Baseline recorded. |

## 7. Conclusion

Baseline status: partial.

Standalone build and runtime baseline are usable for G5 §K comparison: build succeeds, average FPS is effectively 60, VRAM is low, and build size is small.

The baseline is partial because true GUI Editor Stats overlay values and interaction stress tests were not captured in this pass. G5 should repeat measurement immediately before and after the through-play pass, excluding startup frames for the minimum-FPS check and including final audio/dialog content.
