# Stage 4 Phase 0 Triage

Date: 2026-05-06

## Summary

Stage 4 started with a docs-only Phase 0 triage pass. The purpose was to avoid turning Stage 3 closeout observations into implicit work and to make the next dispatch order explicit.

The resulting source-of-truth doc is:

- `docs/STAGE4_PHASE0_TRIAGE.md`

## Inputs

- `docs/STAGE4_ROADMAP.md` v1.0
- `docs/STAGE3_RETROSPECTIVE.md` v1.0
- `docs/G5_ACCEPTANCE_MATRIX.md`
- `docs/VS_SCOPE.md` v1.0
- `docs/devlog/2026-05-06_stage3_closeout.md`

## Result

The triage records:

- No immediate Stage 3 fixes are open.
- Stage 3 remains accepted complete for the Vertical Slice.
- The top Stage 4 technical backlog item is the URP `DrawObjectsPass` / RenderGraph warning cleanup.
- Brush UX, test-count reconciliation, Niro v2, TMP / palette review, dialogue polish, audio polish, verification hardening, and Steam EA prep are ordered as Stage 4 backlog.
- No-action items are listed to prevent accidental scope reopening.

## Verification

No runtime code or Unity assets were changed in this pass. Validation was limited to documentation checks:

- `git diff --check`
- pathspec-limited staging planned for docs only

## Next

Start with the URP warning cleanup technical spike in an isolated worktree. If that migration is too large for a safe small change, record the deferral and move to brush UX affordance / tutorialization.
