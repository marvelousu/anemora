# Bloat Guard Review README Allowlist

Date: 2026-06-14
Branch: `wip/hd2d-point15-recovery-20260612`
Scope: repo hygiene guard follow-up.

## Change

The first review-sync guard commit updated `docs/review/README.md`, which is a
tracked workflow document. CI bloat-guard still rejected it because
`tools/githooks/_bloat_check.sh` treated every path under `docs/review/` as an
ephemeral review image.

Added an exact allowlist for `docs/review/README.md`. Review cycle images and
other files under `docs/review/<cycle>/` remain blocked by the same guard.

## Verification

- `printf 'docs/review/README.md' | SIZE_SRC=worktree bash tools/githooks/_bloat_check.sh`:
  pass.
- `printf 'docs/review/2026-06-14T00-00/example.png' | SIZE_SRC=worktree bash tools/githooks/_bloat_check.sh`:
  blocked as expected.
- Latest push after the hook path fix already had `bloat-guard` and
  `review-sync-guard` green.
- This change keeps the old README documentation commit compatible with future
  PR-range bloat checks that inspect every commit in the range.
