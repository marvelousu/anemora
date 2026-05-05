# AUTHORS

> Status: draft v0.2 (2026-05-06). This file reflects the Stage 3 Day 1 near-completion state and remains subject to Stage 4 public-release review.

## Primary Author

Anemora is currently a single-developer project led by Maro.

The repository commit history was checked with `git log --format='%an <%ae>' | Sort-Object -Unique` on 2026-05-05. One unique commit author identity was observed. For this public-facing draft, only the display name is listed:

- Maro

The associated commit email is intentionally not repeated here. External contribution intake is not open through Stage 3 Day 1 and remains a Stage 4 decision; see `CONTRIBUTING.md`.

## Contribution Status

External issue, pull request, and asset contribution intake is not currently open. Stage 4 will decide whether to accept external contributors and, if so, how attribution, review, DCO / CLA, Code of Conduct handling, and contribution licensing work.

Current related docs:

- `CONTRIBUTING.md`
- `NOTICES.md`
- `docs/legal/code_license_options.md`
- `docs/PITCH_PUBLIC.md`

## Public Release and License

The current primary public release path is Steam Early Access.

Anemora's project-owned code and assets remain All Rights Reserved by default unless a third-party license explicitly applies. Stage 4 may re-evaluate the code license before public contribution intake or broader public release. See `NOTICES.md`.

## AI / Production Tool Assistance

Anemora uses AI-assisted and production tools as part of the asset pipeline. These tools are not listed as authors. Per-asset provenance, paid-plan evidence, public-release status, and Steam AI disclosure classification are tracked in `docs/legal/asset_ledger.md`; consumer-facing summaries are in `NOTICES.md`.

Tools referenced in the Stage 3 Day 1 asset pipeline:

| Tool | Current Stage 3 Day 1 use |
|---|---|
| PixelLab (Pixel Apprentice paid) | F1 sprite drafts for Hero, Resident_A, and Resident_B v1; derived finished sprites are tracked separately in the asset ledger. |
| Meshy v6 (paid/API credits) | A3 Zone1 building and environment source asset generation: 14 source items, 3 Blender repair/rebuild passes, and 540 credits consumed. |
| AIVA Pro | BGM comparison workflow. One generated candidate was not final-selected; the accepted Stage 3 BGM uses the Suno source listed below. |
| Suno v5.5 (paid) | Final selected BGM source for `Assets/Audio/Music/Zone1_Ambient.ogg`, derived from `Dustlight Piano B` / `Zone1_Ambient_Suno_DustlightPiano_B.wav`. |
| ElevenLabs SFX v2 (paid) | Zone1 SFX 30 generation and user-approved draft import; final OGG files are under `Assets/Audio/SFX/Zone1/`. |
| Studio One (owned) | BGM/SFX finishing DAW for the audio workflow; detailed final export notes remain tracked per audio asset in `asset_ledger.md`. |

## Acknowledgements

This draft does not add third-party individual names.

Tool makers, third-party fonts, Unity packages, OSS/runtime dependencies, AI-assisted asset provenance, and development-tool notices are summarized or tracked in:

- `NOTICES.md`
- `docs/legal/asset_ledger.md`
- `docs/PITCH_PUBLIC.md`

## Change History

| Version | Date | Notes |
|---|---|---|
| v0.1 | 2026-05-05 | Initial draft. Single primary author listed from commit history; external contribution attribution and acknowledgements remain TBD. |
| v0.2 | 2026-05-06 | Updated for Stage 3 Day 1 near-completion: single-developer status, Steam Early Access path, All Rights Reserved default, detailed AI / production tool usage, and public documentation cross-references. |
