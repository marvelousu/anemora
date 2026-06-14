# Branch Frontier and Review Sync Guard

Date: 2026-06-14
Branch: `wip/hd2d-point15-recovery-20260612`
Scope: branch-frontier audit plus devlog/review-image enforcement tooling.

## Branch Finding

This is not a live split between `continuous` and `point15`. There is no remote
`continuous` head; the relevant historical ref is
`origin/work/chapter1-continuation-map-vs-20260524`.

The remembered integration did happen. The evidence is merge commit `5c7c510b`
(`Merge HD-2D foundation into chapter1 continuation (Option B)`), which merged
the HD-2D foundation into the Chapter 1 continuation branch while keeping the
legitimate continuation route work and dropping unauthorized VS-range work.

The current point15 recovery branch and the continuation ref now share
merge-base `e7277f0a`. After that base, the continuation ref has one unique
remote-tip commit (`4ac2108c`, recovery v3 wash-strip base), while point15 has
the current recovery/environment-uplift commits. `docs/STATUS.md` is therefore
the deciding frontier: current implementation proceeds on
`wip/hd2d-point15-recovery-20260612`; `work/chapter1-continuation-*` remains the
Chapter 1 continuation/content-history line unless STATUS is changed.

## Guard Added

- Added `tools/review/validate-devlog-review-sync.ps1`.
- Wired it into `tools/githooks/pre-push`.
- Added `.github/workflows/review-sync-guard.yml` so hook-less pushes and PRs
  still validate tracked devlog discipline.
- Updated R2/viewer docs and workflow wiring so `work/*` and `wip/*` branches
  are treated consistently.
- Updated `tools/r2/r2-upload-review.ps1` to print the manifest URL and viewer
  review URL after upload, plus the required viewer-rebuild reminder.

The guard blocks implementation/workflow changes without a `docs/devlog/*.md`
entry, new devlog files missing from `docs/devlog/INDEX.md`, and recent local
`docs/review/<cycle>/` directories that lack `devlog.txt` or review images.

## Verification

- `git fetch --prune origin`: confirmed the active remote heads are
  `wip/hd2d-point15-recovery-20260612`,
  `wip/snapshot-repair-proof-20260603`, and
  `work/chapter1-continuation-map-vs-20260524`.
- `git rev-list --left-right --count HEAD...origin/work/chapter1-continuation-map-vs-20260524`:
  `25 1`, confirming point15 has the current recovery work and the old
  continuation ref has only one post-base tip commit.
- `git diff --check`: pass.
- `bash -n tools/githooks/pre-push`: pass.
- PowerShell parser checks: pass for `tools/review/validate-devlog-review-sync.ps1`
  and `tools/r2/r2-upload-review.ps1`.
- `tools/review/validate-devlog-review-sync.ps1`: pass in local hook mode.
- `tools/review/validate-devlog-review-sync.ps1 -Ci -BaseRef origin/wip/hd2d-point15-recovery-20260612`:
  pass in CI mode.
