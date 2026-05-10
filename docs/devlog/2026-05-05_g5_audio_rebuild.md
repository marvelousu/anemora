# G5 Audio Rebuild

Date: 2026-05-05

## 1. Reason

The previous G5 automated build from `c17d62f` was generated before the A4 audio implementation reached `origin/main`. That build was usable for automated §A-G / §J / §K checks, but it could not support user manual G5 audio listening in §H.

This pass rebuilt the Windows Standalone player from clean `origin/main` at:

`6809c4b Add Zone1 audio implementation (BGM + SFX + controller)`

Temporary worktree:

`<worktree:Anemora-g5-audio-build>`

## 2. Output

Audio-enabled Windows build:

`<worktree:Anemora-g5-audio-build>\Builds\G5Audio\Anemora_G5_Audio.exe`

Build command shape:

`Unity.exe -batchmode -projectPath <worktree> -buildWindows64Player <output-exe> -quit`

## 3. Verification

| Item | Result | Notes |
| --- | --- | --- |
| PlayMode tests | 23/23 passed | Graphics-enabled batchmode. The prompt expected 25/25, but `origin/main@6809c4b` contains 23 PlayMode `[UnityTest]` methods and no committed audio-specific PlayMode test file. |
| Windows build | Success | Build finished with result success. |
| Player launch | Success | Player window ready in 7.934s at 1280 x 720 windowed. |
| Runtime sample | Success | 30s external process sample completed. |
| Audio scene wiring | Pass | Temporary verifier found active/enabled `Zone1_Audio` with `autoPlayOnStart=True`, 1 music OGG, and 30 Zone1 SFX OGG files. |

## 4. Numeric Comparison

| Metric | Previous G5 build, audioなし `c17d62f` | Audio rebuild, audio入り `6809c4b` | Delta |
| --- | ---: | ---: | ---: |
| Build wall duration | 96.048 s | 108.044 s | +11.996 s |
| BuildReport complete size | 114.9 MB | 117.7 MB | +2.8 MB |
| Disk folder size | 115.081 MiB | 117.853 MiB | +2.772 MiB |
| Player window ready | 5.542 s | 7.934 s | +2.392 s |
| Working set average | 187.983 MiB | 212.008 MiB | +24.025 MiB |
| Working set peak | 189.625 MiB | 217.246 MiB | +27.621 MiB |
| Private memory average | 280.374 MiB | 300.840 MiB | +20.466 MiB |
| CPU average | 1.273% | 1.313% | +0.040 pp |
| CPU peak | 2.202% | 2.077% | -0.125 pp |
| GPU dedicated average | 31.527 MiB | 30.950 MiB | -0.577 MiB |
| GPU dedicated peak | 31.531 MiB | 31.539 MiB | +0.008 MiB |
| GPU shared average | 19.332 MiB | 19.425 MiB | +0.093 MiB |
| GPU shared peak | 19.332 MiB | 19.535 MiB | +0.203 MiB |

FPS note:

- Baseline `2e3569f` remains the latest actual FPS measurement: standalone avg 59.909 FPS, p95 frame 16.683ms at 1920 x 1200.
- This audio rebuild repeated the external 30s process sample but did not remeasure FPS. PresentMon is not installed, and the committed player has no in-build FPS sampler.

## 5. Audio Inclusion

BuildReport confirms audio assets are included in the player. Notable packed assets:

| Asset | BuildReport size |
| --- | ---: |
| `Assets/Audio/Music/Zone1_Ambient.ogg` | 2.3 MB |
| `Assets/Audio/SFX/Zone1/environment/sfx_env_distant_water_01.ogg` | 124.3 KB |
| `Assets/Audio/SFX/Zone1/environment/sfx_env_wind_loop_01.ogg` | 85.3 KB |
| `Assets/Audio/SFX/Zone1/environment/sfx_env_silence_pad_01.ogg` | 62.3 KB |
| `Assets/Scripts/Audio/Zone1AudioController.cs` | included |

Temporary scene verifier result:

| Field | Value |
| --- | --- |
| Controller object | `Zone1_Audio` |
| Active / enabled | `True` / `True` |
| `autoPlayOnStart` | `True` |
| Music source | `Music_Source` |
| Wind ambience source | `Wind_Ambience_Source` |
| Pad ambience source | `Pad_Ambience_Source` |
| One-shot source | `OneShot_Source` |
| Music clip | `Zone1_Ambient` |
| Wind clip | `sfx_env_wind_loop_01` |
| Pad clip | `sfx_env_silence_pad_01` |
| Environment one-shots | 4 assigned |
| Portal clips | `sfx_time_portal_open_01`, `sfx_time_portal_flip_01` |
| NPC/UI sample clips | `sfx_npc_greeting_short_01`, `sfx_ui_button_click_01` |
| Music files | 1 |
| Zone1 SFX files | 30 |

The repo also contains compatibility copies under `Assets/Audio/SFX/{env,footstep,timeframe,npc,ui}/`; the scene/build references the canonical `Assets/Audio/SFX/Zone1/...` set.

## 6. Caveats

| Caveat | Current state |
| --- | --- |
| `PortalStencilFeatureSmokeTest` with `-nographics` | Still expected to fail because its RenderTexture path requires a graphics device. This run used graphics-enabled batchmode, where PlayMode passed 23/23. |
| URP RenderGraph warning | Player log emitted `DrawObjectsPass does not have an implementation of the RecordRenderGraph method` 6996 times in the 30s run. This is the same known custom-pass caveat from `c17d62f`; automated portal/stencil tests still pass. |
| PlayMode count expectation | Prompt expected 25/25, but `6809c4b` source contains 23 PlayMode tests. No audio PlayMode test was committed in this branch. |
| Manual listening | Not automated. User still needs to listen to BGM loop, portal modulation/SFX, NPC/UI SFX, and footstep response from the new audio-enabled build. |

## 7. Matrix Update

Only `docs/G5_ACCEPTANCE_MATRIX.md` §K was updated:

- `K-01`: build output, build duration, size, and player ready time now show previous audioなし and current audio入り values.
- `K-02`: CPU sample updated and FPS caveat retained.
- `K-03`: working set and GPU process memory values now compare audioなし vs audio入り.

No design, scene, audio, script, or settings file was modified by this task.
