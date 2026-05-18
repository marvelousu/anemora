# Stage 4 Tier 4 graphics performance baseline v0.2

Date: 2026-05-08

## Summary

This pass refreshes the Stage 4 Windows Standalone performance baseline after the first Tier 4 graphics foundation stack:

- URP soft shadows enabled.
- Production soft-grade Volume assigned to `Anemora_Main`.
- Main directional light tuned for visible soft shadows.
- Time-volume frame shader made main-light shadow-aware.

The same 120 second mixed portal / dialogue stress sample shape from v0.1 was used. Temporary bootstrap scripts, generated scene, raw logs, result JSON, CSV, and build output are measurement artifacts only and are not intended for commit.

Measured project state:

| Item | Value |
|---|---:|
| Repository commit measured | `39146cd` |
| Branch | `codex/stage4-graphics-foundation-20260508` |
| Unity | 6000.3.14f1 |
| URP | 17.3.0 |
| Scene | `Assets/Scenes/Anemora_Main.unity` |
| Build output | `Builds/Stage4Perf/2026-05-08-tier4-graphics/Anemora_Stage4_Tier4Graphics_Perf.exe` |

Measured PC environment:

| Item | Value |
|---|---:|
| OS | Microsoft Windows 11 Home 10.0.26200 |
| CPU | AMD Ryzen 5 7430U with Radeon Graphics |
| CPU cores / logical processors | 6 / 12 |
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
| Build report complete size | `125.7 mb` |
| Build folder files | 197 |
| Build folder disk size | 131,976,286 bytes / 125.862 MiB |

Unity build side effects were generated in Addressables, URP assets, and ProjectSettings. They were treated as generated artifacts and restored after measurement.

## 120s Runtime Sample

Runtime command parameters:

- Resolution: 1280 x 720
- Fullscreen: off
- Sample duration argument: 120 seconds
- Step delay: 1 second
- External process observation duration: 127.484 seconds
- External sample rows: 30

In-player `StressSampleRunner` result:

| Metric | Measured value |
|---|---:|
| Runner duration | 121.624 s |
| Frame count | 7,287 |
| Average FPS | 59.914 |
| Average frame time | 16.772 ms |
| p95 frame time | 16.683 ms |
| p99 frame time | 16.688 ms |
| Max frame time | 600.000 ms |
| GC used memory start / end / peak | 0.000 / 4.668 / 4.668 MiB |
| Total used memory peak | 151.474 MiB |
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

Dialogue success count is expected to remain low because `StressSampleRunner` does not close `DialogueDisplay` after the first successful interaction. This baseline is still a mixed portal stress plus first-dialogue-open sample, not a repeated-dialogue-throughput benchmark.

External process / GPU sample:

| Metric | Measured value |
|---|---:|
| Player exit | Completed normally after result JSON write |
| CPU average / peak | 3.548% / 5.033% of machine |
| Working set average / peak | 295.124 / 298.000 MiB |
| Private bytes average / peak | 403.953 / 405.883 MiB |
| Paged memory peak | 405.883 MiB |
| GPU dedicated average / peak | 61.106 / 61.125 MiB |
| GPU shared average / peak | 32.434 / 32.434 MiB |

Player log checked patterns:

| Pattern | Count |
|---|---:|
| `Error` | 0 |
| `Exception` | 0 |
| `Assert` | 0 |
| `DrawObjectsPass does not have an implementation of the RecordRenderGraph method` | 0 |
| `RecordRenderGraph` | 0 |
| `RenderGraph` | 0 |
| `NullReference` | 0 |
| `MissingReference` | 0 |
| `Failed` | 0 |
| `TextMesh Pro Essential Resources` | 0 |

## Comparison With Stage 4 v0.1

Stage 4 v0.1 was measured before the Tier 4 graphics foundation stack. Both runs use the same mixed portal / dialogue stress shape.

| Metric | Stage 4 v0.1 | Stage 4 Tier 4 v0.2 | Delta |
|---|---:|---:|---:|
| Build folder disk size | 125.837 MiB | 125.862 MiB | +0.025 MiB |
| Average FPS | 59.884 | 59.914 | +0.030 |
| p95 frame time | 16.683 ms | 16.683 ms | 0.000 ms |
| Working set peak | 325.258 MiB | 298.000 MiB | -27.258 MiB |
| GPU dedicated peak | 61.129 MiB | 61.125 MiB | -0.004 MiB |
| GPU shared peak | 32.902 MiB | 32.434 MiB | -0.468 MiB |
| Total used memory peak | 151.443 MiB | 151.474 MiB | +0.031 MiB |
| URP warning count | 0 | 0 | 0 |

The FPS target remains satisfied for this 120 second 1280 x 720 sample. The Tier 4 graphics stack did not produce a measurable p95 frame-time regression in this harness, and memory/GPU counters remain within the v0.1 envelope.

## Caveats

- The first attempted external sampler attached to a launcher process instead of the actual player process. It still produced a valid in-player JSON, but no CSV rows. The recorded table above uses the rerun with corrected process attachment.
- The external sampler gathered 30 rows because GPU process memory counters are relatively slow to query on this machine.
- Do not turn these values into hard automated thresholds yet; standalone FPS and GPU counters remain machine-dependent.
