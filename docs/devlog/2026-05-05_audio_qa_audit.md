# Audio QA Audit: Zone1 BGM and SFX

Date: 2026-05-06

Scope: audio assets introduced by `6809c4b`, audited from current `origin/main`. This pass is report-only: no audio asset was re-encoded, normalized, or moved.

## Method

- Tool: `ffmpeg 6.1.1` from local `ffmpeg-static`.
- LUFS / true peak: `ebur128=peak=true`. FFmpeg reports true peak as dBFS; this report treats it as dBTP-equivalent for QA decisions.
- RMS / sample peak / duration / stream metadata: `astats=metadata=1:reset=0` plus container duration and file-size bitrate.
- BGM loop boundary: PCM `f32le` decode of first/last 250 ms and 20 ms windows.
- LUFS is marked `n/a*` for clips shorter than 0.4 s because the EBU R128 integrated gate is not meaningful for those one-shots.

## Summary

| Metric | Result |
| --- | --- |
| BGM count | 1 |
| Primary SFX count | 30 |
| Compatibility SFX count | 30 |
| Total committed OGG assets | 61 |
| BGM integrated loudness | -18.0 LUFS |
| BGM true peak | -4.5 dBTP |
| BGM RMS | -20.2 dBFS |
| Primary SFX average RMS | -23.6 dBFS |
| Primary SFX RMS range | -67.2 to -17.9 dBFS |
| Primary SFX average true peak | -3.9 dBTP |
| Primary SFX max true peak | 0.0 dBTP |
| Primary SFX average duration | 1.71 s |

## BGM Audit

`Assets/Audio/Music/Zone1_Ambient.ogg` matches the declared loudness target: measured -18.0 LUFS. True peak is -4.5 dBTP, which is below the recommended -1 dBTP ceiling with ample headroom.

Loop boundary is technically click-safe: first/last sample-frame step is -88.3 dBFS, first 20 ms peak is -52.5 dBFS, and last 20 ms peak is -65.3 dBFS. No phase discontinuity or hard sample jump was detected.

The musical loop still deserves in-engine listening. Last 250 ms RMS is -93.6 dBFS while first 250 ms RMS is -18.4 dBFS, a +75.2 dB energy difference after a near-silent tail. This should not click, but it may read as a short breath or phrase restart when the BGM loops in gameplay.

Silence pad is present at `Assets/Audio/SFX/Zone1/environment/sfx_env_silence_pad_01.ogg`, has a compatibility copy, and is wired by `Zone1AudioSceneSetup` into `silencePadClip`.

## Asset Table

| Asset | LUFS | True Peak | RMS | Duration | 判定 | 備考 |
| --- | ---: | ---: | ---: | ---: | --- | --- |
| `Zone1_Ambient.ogg` | -18.0 | -4.5 | -20.2 | 184.84s | OK | 48 kHz stereo; boundary step -88.3 dBFS; musical loop breath to review |
| `sfx_env_birds_01.ogg` | -22.3 | -7.8 | -27.8 | 1.00s | OK | Env one-shot; intentionally distant |
| `sfx_env_distant_water_01.ogg` | -22.3 | -0.1 | -24.0 | 10.00s | Watch | Asset true peak high; playback gain makes it safe but normalize later |
| `sfx_env_dry_leaves_01.ogg` | -25.2 | -0.1 | -30.3 | 2.00s | Watch | Asset true peak high; effective RMS may be very subtle under BGM |
| `sfx_env_silence_pad_01.ogg` | -69.2 | -51.0 | -67.2 | 12.00s | OK | Extremely low by design; silence/air pad |
| `sfx_env_wind_loop_01.ogg` | -23.6 | -5.0 | -19.8 | 12.00s | OK | 44.1 kHz mono ambience loop |
| `sfx_env_wood_creak_01.ogg` | -25.3 | -2.3 | -23.6 | 1.20s | OK | Good one-shot headroom |
| `sfx_footstep_grass_land_01.ogg` | -16.4 | -0.4 | -19.8 | 0.68s | Watch | Asset true peak high; playback footstep gain is -7.5 dB |
| `sfx_footstep_grass_run_01.ogg` | n/a* | -3.1 | -24.4 | 0.38s | OK | Short clip; LUFS gated |
| `sfx_footstep_grass_walk_01.ogg` | -19.5 | -3.5 | -19.3 | 0.48s | OK | Balanced after footstep gain |
| `sfx_footstep_sand_land_01.ogg` | -16.1 | -1.9 | -19.4 | 0.68s | OK | Close to foreground but not clipped |
| `sfx_footstep_sand_run_01.ogg` | n/a* | -1.0 | -18.9 | 0.38s | OK | Short clip; TP close to ceiling |
| `sfx_footstep_sand_walk_01.ogg` | -20.3 | -1.2 | -21.6 | 0.48s | OK | Usable headroom |
| `sfx_footstep_stone_land_01.ogg` | -21.9 | -0.5 | -22.2 | 0.60s | Watch | Asset true peak high |
| `sfx_footstep_stone_run_01.ogg` | n/a* | -4.2 | -17.9 | 0.35s | OK | Loud RMS but playback footstep gain keeps it controlled |
| `sfx_footstep_stone_walk_01.ogg` | -24.0 | -0.3 | -23.4 | 0.45s | Watch | Asset true peak high |
| `sfx_footstep_wood_land_01.ogg` | -25.7 | -0.2 | -26.2 | 0.60s | Watch | Asset true peak high |
| `sfx_footstep_wood_run_01.ogg` | n/a* | -0.4 | -20.0 | 0.35s | Watch | Asset true peak high |
| `sfx_footstep_wood_walk_01.ogg` | -22.9 | 0.0 | -22.7 | 0.45s | Watch | Asset true peak reaches 0.0 dBTP; playback gain prevents clipping |
| `sfx_npc_departure_01.ogg` | -17.6 | -0.4 | -19.1 | 0.80s | Watch | Foreground NPC; asset true peak high |
| `sfx_npc_greeting_short_01.ogg` | -17.5 | -0.9 | -19.3 | 0.48s | Watch | Foreground NPC; asset true peak high |
| `sfx_npc_interaction_ack_01.ogg` | -19.0 | 0.0 | -19.3 | 0.60s | Watch | Foreground NPC; asset true peak reaches 0.0 dBTP |
| `sfx_time_portal_flip_01.ogg` | -24.7 | -5.6 | -26.4 | 1.00s | OK | Good headroom |
| `sfx_time_portal_open_01.ogg` | -19.2 | -5.5 | -18.5 | 1.60s | OK | Forward but clean; time-window volume applies |
| `sfx_time_symbol_hover_01.ogg` | n/a* | -0.5 | -18.6 | 0.20s | Watch | Repeated UI-like cue; asset true peak high |
| `sfx_time_symbol_select_red_01.ogg` | -19.8 | -7.5 | -23.6 | 0.45s | OK | Good headroom |
| `sfx_time_wheel_close_01.ogg` | -28.7 | -2.9 | -25.0 | 0.60s | OK | Soft but audible as secondary cue |
| `sfx_time_wheel_open_01.ogg` | -21.6 | -8.5 | -25.5 | 0.80s | OK | Good headroom |
| `sfx_ui_button_click_01.ogg` | n/a* | -0.2 | -20.8 | 0.20s | Watch | Asset true peak high; UI playback gain is -3.7 dB |
| `sfx_ui_menu_close_01.ogg` | n/a* | 0.0 | -22.0 | 0.25s | Watch | Asset true peak reaches 0.0 dBTP; UI playback gain is -3.7 dB |
| `sfx_ui_menu_open_01.ogg` | n/a* | -0.7 | -20.5 | 0.25s | Watch | Asset true peak high; UI playback gain is -3.7 dB |

## Encoding And Consistency

- BGM: Vorbis, 48 kHz, stereo, measured bitrate about 148.6 kbps.
- Primary SFX: all Vorbis, 44.1 kHz, mono. This is consistent with one-shot and ambience SFX usage.
- Primary SFX bitrate: average 155.1 kbps, with a range from about 65.1 kbps (`sfx_env_silence_pad_01`) to 266.7 kbps (`sfx_ui_button_click_01`).
- The 48 kHz BGM vs 44.1 kHz SFX split is acceptable for Unity playback, but Stage 4 can standardize to 48 kHz if platform QA finds resampling cost/noise.

## Loud / Quiet Candidates

Asset-level true peak above -1 dBTP was found in 15 SFX. These are not immediate playback blockers because category volumes reduce them in `Zone1AudioController` (`footstepVolume=0.42`, `timeWindowVolume=0.70`, `npcVolume=0.70`, `uiVolume=0.65`, ambience one-shots `0.35`). Still, they are the main normalize candidates for Stage 4.

Most likely player-facing loudness watch items:

- `sfx_npc_departure_01.ogg`
- `sfx_npc_greeting_short_01.ogg`
- `sfx_npc_interaction_ack_01.ogg`
- `sfx_time_symbol_hover_01.ogg`
- `sfx_ui_button_click_01.ogg`
- `sfx_ui_menu_close_01.ogg`
- `sfx_ui_menu_open_01.ogg`

Potentially too quiet or too subtle:

- `sfx_env_silence_pad_01.ogg`: intentionally near-silent; OK if it is only an air/psychoacoustic bed.
- `sfx_env_dry_leaves_01.ogg`: effective playback RMS is about -39.4 dBFS after ambience one-shot gain, so it may disappear under BGM + wind.
- `sfx_env_birds_01.ogg`: effective playback RMS is about -36.9 dBFS; acceptable as distant ambience, but not as a gameplay cue.

## Compatibility Copies

Compatibility SFX copies under `Assets/Audio/SFX/{env,footstep,timeframe,npc,ui}/` match the 30 primary `Assets/Audio/SFX/Zone1/**` files byte-for-byte by SHA-256. Total primary SFX bytes and compatibility SFX bytes are both 705,439.

Runtime/editor wiring currently uses the primary Zone1 paths. `Zone1AudioSceneSetup` loads `Assets/Audio/SFX/Zone1/...` directly; there is no runtime fallback lookup from compatibility directories. The compatibility copies are therefore a path-stability fallback for older references and ledger continuity, not an active code fallback path.

## Stage 4 Improvement Proposals

1. Do an in-player BGM loop listening pass. If the near-silent tail feels like a restart, make a dedicated seamless/crossfaded loop edit in Studio One and remeasure.
2. Normalize the 15 high-true-peak SFX to a category target, e.g. -3 dBTP for foreground one-shots and -6 dBTP for ambience/footstep variants.
3. Fine-tune repeated foreground cues first: NPC trio, `sfx_time_symbol_hover_01`, and the three UI clips.
4. Decide whether `sfx_env_dry_leaves_01` and `sfx_env_birds_01` should remain subtle ambience or become clearly audible environmental events.
5. Keep `sfx_env_silence_pad_01` as-is only if its role is intentionally near-silent air; otherwise regenerate or raise it rather than relying on volume.
6. After callsite search in Stage 4, either keep compatibility copies as supported fallback paths or remove them with an `asset_ledger` update.

## Verdict

No blocker was found for G5 manual audio review. BGM loudness matches the declared -18 LUFS target, true peak headroom is safe, all primary SFX are decodable and consistently mono 44.1 kHz, and compatibility copies are exact duplicates. The main Stage 4 work is polish: SFX true-peak normalization and a human loop-listening pass for the BGM tail-to-head transition.
