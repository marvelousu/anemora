# Anemora Trailer Script

> Status: v0 draft (2026-05-05)
> Purpose: Steam Early Access trailer planning source for 30s / 60s / 180s cuts.
> Note: Niro / ニロ、Antela / アンテラは Stage 4 計画時点の provisional name。

## 0. Common Direction

- Public-facing tone: quiet, tactile, restrained, air-first.
- Release card: `Anemora` / `Steam Early Access planned`.
- Rights card if needed: `© 2026 Anemora. All Rights Reserved.`
- BGM source: `Assets/Audio/Music/Zone1_Ambient.ogg`.
- SFX source root: `Assets/Audio/SFX/Zone1/`.
- Text source: `docs/PITCH_PUBLIC.md` v0 and current StringTables.
- Avoid internal design vocabulary in all captions, VO, subtitles, and trailer cards.

### Reusable Audio Palette

| Use | Candidate file |
|---|---|
| Opening air | `Assets/Audio/SFX/Zone1/environment/sfx_env_wind_loop_01.ogg` |
| Town stillness | `Assets/Audio/SFX/Zone1/environment/sfx_env_silence_pad_01.ogg` |
| Small life trace | `Assets/Audio/SFX/Zone1/environment/sfx_env_birds_01.ogg` |
| Dry street detail | `Assets/Audio/SFX/Zone1/environment/sfx_env_dry_leaves_01.ogg` |
| Old building detail | `Assets/Audio/SFX/Zone1/environment/sfx_env_wood_creak_01.ogg` |
| Footsteps | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_stone_walk_01.ogg` |
| Wheel open | `Assets/Audio/SFX/Zone1/time_window/sfx_time_wheel_open_01.ogg` |
| Symbol focus | `Assets/Audio/SFX/Zone1/time_window/sfx_time_symbol_hover_01.ogg` |
| Symbol select | `Assets/Audio/SFX/Zone1/time_window/sfx_time_symbol_select_red_01.ogg` |
| Portal open | `Assets/Audio/SFX/Zone1/time_window/sfx_time_portal_open_01.ogg` |
| Boundary crossing | `Assets/Audio/SFX/Zone1/time_window/sfx_time_portal_flip_01.ogg` |
| NPC notice | `Assets/Audio/SFX/Zone1/npc/sfx_npc_greeting_short_01.ogg` |
| UI click | `Assets/Audio/SFX/Zone1/ui/sfx_ui_button_click_01.ogg` |

### Reusable Niro Text Candidates

These lines are extracted from `NiroMonologueController`-referenced DialogueAssets and `Anemora_Strings` tables. Use as subtitle cards or soft VO, not as final locked script.

| Key | JA | EN |
|---|---|---|
| `dialogue.niro.intro.line_1` | アンテラの朝は、音より先に埃が動く。 | In Antela, dust moves before sound does. |
| `dialogue.niro.intro.line_3` | 返せるものがあるなら、返したい。 | If something can be returned, I want to return it. |
| `dialogue.niro.intro.line_4` | 防げる終わりがあるなら、まだ間に合うと思いたい。 | If an ending can be held back, I want to believe there is still time. |
| `dialogue.niro.past_portal.line_1` | 街が、息をしている。 | The town is breathing. |
| `dialogue.niro.past_portal.line_2` | 知らないはずの温度が、手のひらに残る。 | A warmth I should not know stays in my hand. |

## 1. 30s Trailer

High-density 6-cut version for storefront autoplay, social posts, and short Direct-style recap.

| Time | Cut | Visual / Direction | Audio |
|---|---|---|---|
| 0:00-0:04 | Fading street | Dawn before full light. Niro walks through Antela; stone, dust, faded buildings. Card: `A fading town remembers.` | `Zone1_Ambient.ogg` fade in + `sfx_env_wind_loop_01` low. |
| 0:04-0:08 | Resonance | Niro stops. Dust shifts before the sound. Camera holds on an empty street edge. | Dry leaf one-shot, low silence pad. Optional subtitle: `音より先に埃が動く。` |
| 0:08-0:12 | Brush | Niro raises the brush and draws a square frame in the air. | `sfx_time_wheel_open_01`, BGM slightly opens. |
| 0:12-0:17 | Symbols | Three symbols rise inside the frame. Red symbol receives focus and selection. | `sfx_time_symbol_hover_01` x2, then `sfx_time_symbol_select_red_01`. |
| 0:17-0:23 | Diorama | A small 3D diorama unfolds inside the frame; the same street appears alive with a different time of day. | `sfx_time_portal_open_01`; BGM pad widens. |
| 0:23-0:27 | Encounter | Niro crosses the border. A girl beyond the frame turns and points toward the ruined side. | `sfx_time_portal_flip_01`, then `sfx_npc_greeting_short_01`. |
| 0:27-0:30 | Logo | Hard cut to present street detail, then logo. Card: `Anemora` / `Steam Early Access planned`. | BGM tail, wind only on final frame. |

## 2. 60s Trailer

Extended storefront version. Adds Niro's internal motive, more town atmosphere, and clearer action-result framing.

| Time | Cut | Visual / Direction | Audio |
|---|---|---|---|
| 0:00-0:05 | Title air | Black to dawn. Antela street, no crowd, no combat framing. | `Zone1_Ambient.ogg` fade in, wind loop low. |
| 0:05-0:10 | Niro walk | Niro crosses stone and dry grass edges; footsteps change material. | Stone / grass footsteps, dry leaves. |
| 0:10-0:15 | VO 1 | Close detail: dust, wall crack, window without light. Subtitle: `アンテラの朝は、音より先に埃が動く。` | BGM sparse, silence pad under subtitle. |
| 0:15-0:20 | Object trace | Niro pauses near a familiar object or route marker. Subtitle: `返せるものがあるなら、返したい。` | Small wood creak, no hit accent. |
| 0:20-0:27 | Brush frame | Niro draws the frame. The square catches light but remains restrained. | Wheel open, short UI click if tool equip is shown. |
| 0:27-0:33 | Three symbols | Symbols float, hover, select red. Keep UI readable and unhurried. | Hover x2, red select. |
| 0:33-0:40 | Diorama reveal | Diorama grows inside the frame; camera shifts from flat view to depth. | Portal open, BGM shimmer but no trailer hit. |
| 0:40-0:46 | Cross | Niro steps through. Frame edge passes over the camera as a soft wipe. | Portal flip, footsteps switch to wood or grass. |
| 0:46-0:52 | Past encounter | The girl notices Niro and points toward the library ruin. No explanation card. | NPC greeting, wind drops for one beat. |
| 0:52-0:56 | Return trace | Current side: book or place detail has changed. Subtitle: `街が、息をしている。` | Portal flip tail, distant water if scene supports it. |
| 0:56-1:00 | Logo / CTA | Logo over quiet street. Cards: `Steam Early Access planned` and `All Rights Reserved`. | BGM resolves to soft tail. |

## 3. 180s Trailer

Full hook version. This is closer to a captured playable sequence than a marketing montage.

| Time | Phase | Visual / Direction | Audio |
|---|---|---|---|
| 0:00-0:20 | Antela before dawn | Establish the declining town. Houses are damaged but not empty of memory. Camera is slow; no UI yet. | `Zone1_Ambient.ogg` starts near silence. Wind loop and silence pad sit below BGM. |
| 0:20-0:40 | Niro walking | Niro enters from frame edge. Show scale: small character, large buildings, readable HD-2D depth. | Stone footsteps, dry leaves, one distant bird. |
| 0:40-1:10 | Residual sound | Niro stops at an audio cue. The camera lingers on an empty route, then a faint point of interest. Optional subtitle: `防げる終わりがあるなら、まだ間に合うと思いたい。` | BGM thins. Wood creak and silence pad carry the pause. |
| 1:10-1:30 | Brush | Niro raises the brush, draws a square frame, and the surrounding air reacts. Keep the gesture readable from gameplay camera distance. | Wheel open, subtle UI click, no large impact. |
| 1:30-2:00 | Symbols and diorama | Three symbols appear. Red is selected. A diorama unfolds inside the frame, showing the same place with life and light. | Hover x2, red select, portal open. BGM gains warmth but stays restrained. |
| 2:00-2:30 | Crossing | Niro walks through the frame. The border becomes a visual wipe. After crossing, show new ground texture and living-town details. | Portal flip, footsteps switch surface. Ambient wind lowers; town bed becomes slightly warmer if available. |
| 2:30-2:50 | Girl and pointing | The girl turns toward Niro. She does not explain everything; she points toward the ruined side / library direction. | NPC greeting, short pause, then a quiet acknowledgement cue if dialogue starts. |
| 2:50-3:00 | Return and unease | The frame collapses. Niro returns to the present. A final shot shows the street slightly wrong or newly meaningful. Card: `Anemora`. | Portal close or flip tail. BGM drops to wind and one low pad. |

### 180s Optional Text Cards

| Time | Card |
|---|---|
| 0:18 | `A town that still holds its traces.` |
| 0:58 | `If something can be returned, I want to return it.` |
| 1:42 | `Choose a symbol.` |
| 2:08 | `Step through the frame.` |
| 2:55 | `Notice what changed.` |
| 2:58 | `Steam Early Access planned.` |

## 4. Capture Notes

- Prefer direct gameplay capture over cinematic-only shots.
- Keep UI visible for symbol selection in at least one 30s and one 60s cut.
- Show one clear boundary crossing in every version.
- Show one action-result trace in 60s and 180s; the 30s cut may imply it only through the final present-side shot.
- Avoid lore explanation cards. Use objects, framing, audio cues, and short Niro text instead.
- Keep logo duration short; the trailer should sell the walk, the frame, and the changed place.

## 5. Revision History

| Version | Date | Change |
|---|---|---|
| v0 | 2026-05-05 | Initial 30s / 60s / 180s trailer script draft for Steam Early Access planning |
