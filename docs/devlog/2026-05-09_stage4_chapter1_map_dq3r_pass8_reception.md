# Stage 4 Chapter 1 Map DQ3R Pass 8 Reception

Date: 2026-05-09
Recovered: 2026-05-19
Source notes:
- `notes/_handover/_prompts/anemora-graphics-foundation-pass8-commit-staging-2026-05-09.md`
- Prior scaffolding context: `2026-05-09_stage4_chapter1_map_dq3r_pass7_reception.md`

## Recovery note

This devlog was referenced in the Pass 8 staging prompt as an added file, but the root
`docs/devlog/2026-05-09_stage4_chapter1_map_dq3r_pass8_reception.md` file itself was not
present in the recovered Windows worktrees, Linux repo, notes repo history, or local refs.
The content below is reconstructed from the surviving staging prompt and nearby Pass 7
reception devlog.

## Summary

Pass 8 was recorded as fully landed by the staging note at 2026-05-09 23:55. It completed
the next map-authorship batch after Pass 7 and turned the previously prepared Pass 8
reception scaffolding into active integration.

Delivered assets and docs:

- Atlas v5 with 50/50 authored tiles, preserving the atlas v3/v4 UV contract and tile ID
  stability.
- Decal sheet F for persistent FootprintTrail motion decals, anti-paired with particles and
  paired with character motion metadata.
- `Ch1_Next8_WindZonePresetKit.fbx` with 12 wind anchor empties and normalized
  `wind_direction_xz` / `wind_strength_units_per_second` metadata.
- Per-TOD lighting tone matrix document for sky, ambient, key light, and fog tone rows.
- `tools/verify_chapter1_next8_static.py` with 7 static checks.
- Atlas evolution review sheet extended from two panels to a v3/v4/v5 comparison flow.

## Integration behavior

The Unity-side graphics foundation integration picked up the Pass 8 assets without changing
the production scene:

- Atlas path priority became `v5 -> v4 -> v3` for interactive use.
- Pass 8 reception tests that had been graceful skips under Pass 7 flipped to passing once
  the new assets were present.
- `ApplyTimeOfDayRenderSettings` could read the tone matrix rows and override
  `RenderSettings.ambientLight` / `RenderSettings.fogColor` by TOD and beat category when the
  row existed.
- Animated cloth wind direction could resolve from the nearest wind anchor instead of the
  manifest static fallback.
- Character footprint trails could use decal sheet F for walking-character motion cues.
- Importer reservations were expanded for atlas v5 textures and decal sheet F.

## Verification recorded in source note

The staging note records:

- `verify_chapter1_next5_static.py`: 10/10 expected.
- `verify_chapter1_next6_static.py`: 7/7 expected.
- `verify_chapter1_next7_static.py`: 8/8 expected.
- `verify_chapter1_next8_static.py`: 7/7 expected.
- Verifiers 5+6+7+8: 32/32 checks passing.
- Targeted `Chapter1MapAssetTests`: 0 failures, with the remaining skips limited to
  pre-existing interactive-only / material-render capture gates.

The source note contains two count phrasings from different checklist stages:

- Pre-commit expectation: `66 testcase / 63 pass / 3 skip / 0 fail`.
- Commit-message draft summary: `56 testcase / 53 pass / 3 skip / 0 fail`.

Both record the same operative result for this devlog: Pass 8 landed with zero failures and
three remaining known skips.

## Boundaries

- Production scene was not opened or saved.
- TimeWindow remained a thin world-space visual window; the v5 `time_window` tile retained
  the `thin_only` semantic.
- Atlas v3 UV contract, 50 tile IDs, and UV layer name stayed stable across v3 -> v4 -> v5.
- Pass 6 surface shader inputs and Pass 7 cloth shader inputs were preserved.
- Character candidates v4/v10/v11 remained disqualified for production import.

## Missing evidence status

This recovery does not add new screenshot files. The staging note references the atlas
evolution sheet and Pass 8 review screenshots, but only files already found during the earlier
cross-worktree recovery are present in this repo. The missing evidence status remains tracked
from the recovered index notes.
