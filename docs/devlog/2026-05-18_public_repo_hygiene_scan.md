# Public Repo Hygiene Scan

Date: 2026-05-18

## Scope

- Project: `<repo>`
- Branch: `work/post-vs-public-20260518`
- Request: scan and sanitize all tracked files, including devlogs, for sensitive information, inappropriate wording, and other-game proper-name references before treating the Fast VS build as public-ready.

## Worker Cycle

- `gpt-5.4-mini` worker `019e3a57-4eb7-7a82-a3da-e68a9d109268` performed an independent read-only scan.
- Worker findings were reviewed locally before edits.

## Changes

- Removed local absolute paths from tracked docs/devlogs and replaced them with repo-relative paths or public-safe placeholders such as `<repo>`, `<character-source>`, `<story-source>`, and `<runtime-source>`.
- Removed the hardcoded local Aria source path from `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`; the scene generator now validates and imports the bundled Aria asset.
- Replaced source JSON `file` fields with repo-relative paths for the CC0 bookshelf texture records.
- Sanitized `SPEC.md` private-planning/career-adjacent risk wording into a public-safe external-schedule note.
- Replaced personal author display text in `AUTHORS.md` with role-based maintainer wording.
- Redacted operational API credential status notes in `docs/legal/asset_ledger.md`; no literal secret values were found.
- Removed the other-game shorthand reference from the old visual-reference rejection list and devlog wording.
- Reworded the bookshelf comparison devlog to avoid a specific other-game style reference.
- Sanitized and included the previously untracked `docs/HD2D_IMPLEMENTATION_PROPOSAL.md` so the working tree has no stray public-facing note with other-game proper names.

## Verification

- Sensitive/path/title scan:
  - No tracked hit remains for local absolute user paths, local username strings, private-memory wording, career/application wording, selected secret patterns, or the targeted other-game references.
  - The only local absolute path hit is `.git`, which is a worktree control file and not tracked public content.
  - `tools/meshy_zone1_buildings.py` still contains the environment variable name `MESHY_API_KEY`; this is an integration placeholder, not a secret value.
  - `docs/devlog/2026-05-05_a4_zone1_audio_implementation.md` still contains a historical `secret scan` row stating that no secret value was found.
- `git diff --check` passed. Git reported line-ending normalization warnings for two source JSON files only.
- Unity batch validation was attempted with `BuildAndValidateBatch`.
  - Script compilation completed successfully.
  - The batch then stopped on the existing Fast VS font validation gate: `Anemora_JP` reported `MisakiGothic` rather than the expected `DotGothic16`.
  - Auto-generated scene/material/addressables noise from that failed validation attempt was reverted from the working tree.
  - Log: `<repo>/Logs/fast_vs_build_validate_20260518_public_hygiene_scan.log`

## Follow-Up

- Before creating public release notes or pushing to a remote, rerun the hygiene scan on tracked text files.
- Regenerate/rebind the Japanese TMP atlas to `DotGothic16` before relying on `BuildAndValidateBatch` as the release gate again.
- Keep real local full paths out of committed docs; use full local paths only in direct user reports when requested.
