# Stage 4 Audio Polish Inventory (2026-05-06)

Scope: report-only Stage 4 dispatch inventory for current `origin/main` / `cab5a59`. No audio assets, scenes, prefabs, ProjectSettings, Addressables, asset ledger rows, generated logs, or builds were modified.

## Repository State

- Branch worker state after `git fetch origin main`: local `HEAD`, `origin/main`, and merge-base all resolve to `cab5a59` (`Import Stage 4 character v2 sprite sets`).
- Runtime audio assets live under `Assets/Audio/`.
- Scene/editor wiring uses canonical Zone1 SFX paths under `Assets/Audio/SFX/Zone1/**`.
- Flat directories under `Assets/Audio/SFX/{env,footstep,timeframe,npc,ui}/` are compatibility copies, not active runtime fallback paths per the prior audio QA audit.

## Current Asset Inventory

| Category | Canonical files | Compatibility copies | Notes |
|---|---:|---:|---|
| BGM | 1 | 0 | `Assets/Audio/Music/Zone1_Ambient.ogg`; 184.84s, 48 kHz stereo per prior QA audit |
| Environment / ambience SFX | 6 | 6 | birds, wind loop, dry leaves, distant water, wood creak, silence pad |
| Footstep SFX | 12 | 12 | wood / stone / grass / sand with walk / run / land variants |
| Time-window SFX | 6 | 6 | wheel open/close, symbol hover/select, portal open/flip |
| NPC SFX | 3 | 3 | greeting, interaction ack, departure |
| UI SFX | 3 | 3 | button click, menu open, menu close |
| Total committed OGG assets | 61 | n/a | 1 BGM + 30 canonical SFX + 30 compatibility SFX |

Canonical SFX paths:

- `Assets/Audio/SFX/Zone1/environment/*.ogg`
- `Assets/Audio/SFX/Zone1/footsteps/*.ogg`
- `Assets/Audio/SFX/Zone1/time_window/*.ogg`
- `Assets/Audio/SFX/Zone1/npc/*.ogg`
- `Assets/Audio/SFX/Zone1/ui/*.ogg`

## Acceptance / Placeholder Status

| Area | Current status | Stage 4 interpretation |
|---|---|---|
| BGM | `Zone1_Ambient.ogg` is committed and wired. Prior audio prompt integration says Suno `Dustlight Piano B` was adopted/exported, while the current ledger row still reads like a draft/pending plan row. | Treat as current accepted runtime draft, but queue a ledger wording cleanup before release documentation. Keep/replace remains a user listening decision. |
| Environment SFX | Ledger rows mark generated under ElevenLabs Creator/API and user-approved for Zone1 draft use; birds and silence pad include explicit user-approved notes. | Accepted for draft runtime use. Stage 4 polish should focus on loop feel, audibility, and whether subtle ambience is desirable. |
| Footsteps | Ledger rows mark generated under ElevenLabs Creator/API and user-approved for Zone1 draft use, with several grass variants explicitly user-approved. | Accepted for draft runtime use. Stage 4 should verify gameplay balance and normalize only after user confirms the character of the sounds. |
| Time-window SFX | Ledger rows mark generated under ElevenLabs Creator/API and user-approved for Zone1 draft use; notes call the time-window tone soft/ethereal and user-approved. | Accepted for draft runtime use. Preserve current tone unless user listening flags harshness, repetition, or insufficient feedback. |
| NPC / UI SFX | Ledger rows mark generated under ElevenLabs Creator/API and user-approved for Zone1 draft use. Prior QA flags these as likely foreground polish targets. | Accepted for draft runtime use, but most likely first replacement/normalization candidates because they repeat and sit in the foreground. |
| Compatibility copies | Prior QA found the 30 flat SFX copies byte-for-byte identical to canonical Zone1 SFX. | Do not remove during audio polish unless a dedicated cleanup task updates references and ledger/notes. |

## License / Ledger Status

- SFX: `docs/legal/asset_ledger.md` contains 30 SFX rows under Zone1 with current canonical paths, ElevenLabs SFX v2 API + ffmpeg-static provenance, ElevenLabs Creator plan evidence, GitHub Public ok after user approval, and Steam `Tier 1 player-consumed`.
- `sfx_env_silence_pad_01` is recorded as ElevenLabs fallback; Stable Audio was not used.
- BGM: the ledger has a `bgm_zone1_v1` row for `Assets/Audio/Music/Zone1_Ambient.ogg`, but the row still says pending verification / draft plan even though the asset is committed and prior devlogs say the Suno source was adopted/exported.
- Prior SFX ledger review already found 30 / 30 SFX path existence and matching IDs. Remaining ledger follow-ups are stable generation/request IDs, measured LUFS/file sizes if desired, and compatibility-copy treatment.

## Technical QA Already Recorded

Prior audio QA (`docs/devlog/2026-05-05_audio_qa_audit.md`) found:

- BGM: -18.0 LUFS, -4.5 dBTP, no hard loop click, but a possible musical breath/restart because the tail is near silent before the loop restarts.
- Primary SFX: all Vorbis, 44.1 kHz, mono.
- Compatibility SFX copies: exact SHA-256 matches to the canonical 30 primary SFX.
- Watch items: 15 SFX have asset-level true peak above -1 dBTP, mitigated by runtime category volumes but still good Stage 4 normalize candidates.
- Likely foreground polish targets: NPC trio, `sfx_time_symbol_hover_01`, and the three UI clips.

## Tests Touching Audio

- `Assets/Tests/PlayMode/Zone1AudioWiringTests.cs`
  - Loads `Anemora_Main`.
  - Asserts `Zone1AudioController` exists with BGM, wind ambience, silence pad, portal, footstep, and environment clips.
  - Asserts `Resident_A_Instance` / dialogue UI have interaction, advance, and close clips.
- `Assets/Tests/EditMode/SaveEnvelopeRoundTripTests.cs`
  - Covers audio settings persistence fields: master, music, SFX, and ambient volume.
- These tests cover wiring and saved settings shape only. They do not verify audible playback, loop quality, mix balance, clipping, or replacement decisions.

## User Review Checkpoints

1. BGM loop: listen in-player for at least one full loop boundary around 3:04-3:05 and decide whether the tail-to-head breath feels intentional or like a restart.
2. BGM with ambience: listen with wind + silence pad enabled; decide whether ambience supports the town mood or masks the piano/string bed.
3. Foreground repetition: repeatedly trigger NPC, UI, and `sfx_time_symbol_hover_01`; mark keep / normalize only / replace.
4. Footsteps in motion: test walk/run/land on wood, stone, grass, and sand; mark which surface variants need volume or character changes.
5. Environment subtlety: decide whether birds and dry leaves should remain distant/subtle or become clearer environmental events.
6. Time-window identity: confirm the soft ethereal tone still fits Stage 4 after visual polish and brush UX changes.

## Recommended Next Audio Task

Run a user-guided listening pass before replacing assets. Use a short checklist with three outcomes per clip group: `keep`, `normalize/mix only`, or `replace candidate`. After that, the first safe implementation task is a docs-backed normalization pass for the 15 high-true-peak SFX, starting with NPC, UI, and `sfx_time_symbol_hover_01`; update the ledger only when actual exported files change.

## Verification

- `rg --files Assets/Audio` for asset inventory.
- `rg` scans across audio docs, ledger, prompts, tests, and runtime controller.
- Unity was not run because this task made documentation-only changes.

## Revision History

| Version | Date | Change |
|---|---|---|
| v0.1 | 2026-05-06 | Initial Stage 4 audio polish inventory and dispatch report |
