# Stage 4 Performance Baseline v0.1

Date: 2026-05-07

## Summary

This pass records a Stage 4 post-Resident_A-import Windows Standalone performance baseline. It uses a temporary bootstrap build to run the existing `StressSampleRunner` for a 120 second mixed portal / dialogue stress sample. The temporary bootstrap scripts, generated scene, raw logs, result JSON, CSV, and build output are measurement artifacts only and are not intended for commit.

Measured project state:

| Item | Value |
|---|---:|
| Repository commit measured | `ab7f759` |
| Branch | `codex/stage4-hero-v2-20260506` |
| Unity | 6000.3.14f1 |
| URP | 17.3.0 |
| Scene | `Assets/Scenes/Anemora_Main.unity` |
| Build output | `Builds/Stage4Perf/2026-05-07/Anemora_Stage4_Perf.exe` |

Measured PC environment:

| Item | Value |
|---|---:|
| OS | Microsoft Windows 11 Home 10.0.26200 |
| CPU | AMD Ryzen 5 7430U with Radeon Graphics |
| RAM visible to Windows | 14,865,915,904 bytes |
| GPU | AMD Radeon (TM) Graphics |
| GPU memory reported by CIM | 2.00 GiB AdapterRAM |
| GPU memory reported by Unity D3D log | 7088 MB VRAM |
| GPU driver | 31.0.21914.7005 |

## Build

The build used a temporary bootstrap scene and temporary editor build method under `Assets/PerfStage4Temp/`. The build loaded `Anemora_Main`, created `StressSampleRunner` at runtime, passed an absolute JSON output path, and exited after the sample completed.

| Metric | Measured value |
|---|---:|
| Build result | Success |
| Build log marker | `Build Finished, Result: Success.` |
| BuildReport total size | 131,755,247 bytes / 125.652 MiB |
| Build folder disk size | 131,949,305 bytes / 125.837 MiB |

Unity build side effects were generated in Addressables, URP assets, and ProjectSettings. They were treated as generated artifacts and restored after measurement.

## 120s Runtime Sample

Runtime command parameters:

- Resolution: 1280 x 720
- Fullscreen: off
- Sample duration argument: 120 seconds
- Step delay: 1 second
- External process observation duration: 130.364 seconds
- External sample rows: 63

In-player `StressSampleRunner` result:

| Metric | Measured value |
|---|---:|
| Runner duration | 121.685 s |
| Frame count | 7,287 |
| Average FPS | 59.884 |
| Average frame time | 16.701 ms |
| p95 frame time | 16.683 ms |
| p99 frame time | 16.687 ms |
| Max frame time | 266.657 ms |
| GC used memory start / end / peak | 0.000 / 4.594 / 4.629 MiB |
| Total used memory peak | 151.443 MiB |
| Mono heap peak | 0.000 MiB |
| URP `DrawObjectsPass` warning count | 0 |

Stress action counters:

| Counter | Count |
|---|---:|
| Portal open | 30 |
| Portal close | 30 |
| Portal crossing trigger | 30 |
| Dialogue trigger attempts | 59 |
| Dialogue trigger successes | 1 |

Dialogue success count is expected to be low in this scaffold run because `StressSampleRunner` does not close `DialogueDisplay` after the first successful interaction. This baseline should be treated as a mixed portal stress plus first-dialogue-open sample, not as a repeated-dialogue-throughput benchmark.

External process / GPU sample:

| Metric | Measured value |
|---|---:|
| Player exit code | 0 |
| CPU average / peak | 3.427% / 5.782% of machine |
| Working set average / peak | 313.004 / 325.258 MiB |
| Private bytes average / peak | 415.749 / 435.879 MiB |
| Paged memory peak | 435.879 MiB |
| GPU dedicated average / peak | 59.137 / 61.129 MiB |
| GPU shared average / peak | 32.035 / 32.902 MiB |

Player log checked patterns:

| Pattern | Count |
|---|---:|
| `Error` | 0 |
| `Exception` | 0 |
| `Assert` | 0 |
| `DrawObjectsPass does not have an implementation of the RecordRenderGraph method` | 0 |
| `NullReference` | 0 |
| `MissingReference` | 0 |
| `Failed` | 0 |

## Comparison With Stage 3 v0.2

Stage 3 v0.2 remains the final Stage 3 idle baseline. Stage 4 v0.1 is not a pure idle comparison because it includes runtime portal open / close / crossing attempts and one successful dialogue open.

| Metric | Stage 3 v0.2 | Stage 4 v0.1 | Delta |
|---|---:|---:|---:|
| Build folder disk size | 117.853 MiB | 125.837 MiB | +7.984 MiB |
| Average FPS | 59.989 | 59.884 | -0.105 |
| p95 frame time | 16.683 ms | 16.683 ms | 0.000 ms |
| Working set peak | 290.762 MiB | 325.258 MiB | +34.496 MiB |
| GPU dedicated peak | 52.664 MiB | 61.129 MiB | +8.465 MiB |
| GPU shared peak | 30.586 MiB | 32.902 MiB | +2.316 MiB |
| Total used memory peak | 141.609 MiB | 151.443 MiB | +9.834 MiB |
| URP warning count | 14,402 | 0 | -14,402 |

The FPS target remains satisfied for this 120 second 1280 x 720 sample. The higher memory values are acceptable for Stage 4 v0.1 and should be treated as the new post-character-import / post-URP-warning-fix comparison point.

## Follow-Up

- Do not turn these values into hard automated test thresholds yet; standalone FPS and GPU counters remain machine-dependent.
- Rerun this baseline after TMP/font/UI changes or additional character/environment import batches.
- If repeated dialogue throughput matters later, add an explicit `DialogueDisplay.Close()` or auto-advance hook to the performance harness before claiming repeated dialogue success metrics.
