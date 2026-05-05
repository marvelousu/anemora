# asset_ledger SFX consolidation review (2026-05-05)

## Scope

Reviewed the 30 Zone1 SFX rows added under `docs/legal/asset_ledger.md` section 2.4 against `docs/asset_prompts/sfx_zone1.md` v1.0 draft.

This review is format-only. It does not change asset adoption decisions, license decisions, Steam disclosure tiering, or audio implementation choices.

## Checks

| Check | Result |
|---|---|
| SFX row count | 30 entries |
| Column count | 11 columns for every row |
| Expected ID match | 30 / 30 IDs match `sfx_zone1.md` v1.0 |
| Category breakdown | environment 6 / footstep 12 / time-window 6 / NPC 3 / UI 3 |
| Naming pattern | `sfx_<category>_<name>_<index>` for all entries |
| Steam tier | `Tier 1 player-consumed` for all entries |
| Asset path existence | 30 / 30 primary OGG paths exist locally |

## Fixes Applied

- Normalized the Tool column from `ElevenLabs SFX API + ffmpeg-static` to `ElevenLabs SFX v2 API + ffmpeg-static` for generated SFX rows.
- Kept `sfx_env_silence_pad_01` as `ElevenLabs SFX v2 API fallback + ffmpeg-static` because Stable Audio was not used.
- Replaced the generic Input material value `docs/asset_prompts/sfx_zone1.md prompt` with per-entry section references, for example `docs/asset_prompts/sfx_zone1.md section 2.1 prompt`.

## Next Revision Candidates

- If the ElevenLabs manifest exposes stable request IDs or generation IDs, add them to the Notes column or a separate source manifest reference.
- Add final measured LUFS and encoded OGG file sizes if a later audio audit needs tighter traceability.
- Decide whether compatibility copies under `Assets/Audio/SFX/{env,footstep,timeframe,npc,ui}/` should remain Notes-only or receive separate ledger treatment.
- Revisit `sfx_env_silence_pad_01` only if Stage 4 needs a Stable Audio comparison take; current row correctly records that Stable Audio was not used.

## Revision History

| Version | Date | Change |
|---|---|---|
| v0.1 | 2026-05-05 | Initial consolidation review for Zone1 SFX 30 entries |
