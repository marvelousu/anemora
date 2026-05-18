# Stage 4 Story Bible Integration Check

> Date: 2026-05-07
> Scope: documentation / orchestration hygiene
> Inputs: `docs/STORY_BIBLE_v1.md`, `docs/AUTONOMOUS_WORK_GUIDELINE.md`, `docs/STAGE4_ROADMAP.md`, `docs/VERIFICATION_SUITE.md`, `docs/devlog/INDEX.md`
> Result: report only; no story, world, character, or scene decision changed.

## 1. Purpose

Linux-side planning produced two Stage 4 governance inputs and copied them into the Windows worktree:

- `docs/STORY_BIBLE_v1.md`
- `docs/AUTONOMOUS_WORK_GUIDELINE.md`

This pass records how Codex should treat those files while continuing implementation work. It deliberately avoids expanding story detail or changing any locked decision. User-review gates from the previous Stage 4 progress handover remain active.

## 2. Intake Result

`c31f45b` added both files to branch `codex/stage4-hero-v2-20260506`.

The commit message used neutral wording and does not expose spoiler-specific terms. The two documents are internal production references, not public-facing copy.

## 3. Consistency Check

| Area | Current state | Result |
|---|---|---|
| Stage 4 roadmap | Phase 1 already blocks Zone 2-6 expansion until Zone1 scale / first-map baseline is accepted. | Consistent. Keep the existing user-review gate. |
| Character work | Hero and Resident_B are current visual references; Resident_A needs frame / face stability follow-up. | Consistent. Do not finalize new character visuals without user review. |
| Verification docs | `VERIFICATION_SUITE.md` tracks automated coverage and manual gaps. | Consistent. Story-level rules should remain manual/doc-review, not test assertions. |
| Devlog practice | `docs/devlog/INDEX.md` requires every new root devlog to be indexed. | This file is added to the index in the same change. |
| Autonomous work boundary | Guideline allows polish, tests, documentation, technical cleanup, and report-only consistency checks. | This pass fits the allowed report-only documentation scope. |

## 4. Blocked For User Review

These items remain blocked and were not changed:

- Character scale / Resident_A frame-stability approval.
- Zone1 first-map blockout approval.
- Time-window hero spot selection.
- Final NPC naming.
- Zone B/C/D theme decisions.
- Chapter-facing naming and late-game presentation details.
- Large art-style, camera, or release-publication decisions.

## 5. Safe Next Work

Codex can continue without user input on:

- Documentation navigation and cross-link cleanup.
- Verification matrix updates based on actual tests / captures.
- Objective test hardening that does not encode story decisions.
- Technical polish for renderer, localization, save/load, accessibility, or performance.
- Asset ledger / OSS ledger / Steam AI disclosure preparation.

## 6. Verification

- Pathspec intake of `docs/STORY_BIBLE_v1.md` and `docs/AUTONOMOUS_WORK_GUIDELINE.md`: complete in `c31f45b`.
- This integration pass is documentation-only.
- Unity tests were not run for this doc-only change.
- `git diff --check --cached` should be run before commit.

## 7. Follow-Up

When Linux-side planning updates the Story Bible to v1.1 or later, repeat this same pattern:

1. Pull or receive the updated file.
2. Commit only the updated governance file(s) with spoiler-neutral metadata.
3. Add a report-only consistency note if implementation boundaries changed.
4. Keep any content or direction changes behind user review.
