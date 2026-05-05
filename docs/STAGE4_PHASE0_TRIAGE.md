# Stage 4 Phase 0 Triage

Status: v0.1 Stage 4 entry triage (2026-05-06)

This document extracts the Stage 4 Phase 0 backlog from the accepted Stage 3 closeout. It is intentionally narrow: it does not reopen Stage 3 completion, choose art direction, or start content expansion.

Source docs:

- `docs/G5_ACCEPTANCE_MATRIX.md`
- `docs/STAGE3_RETROSPECTIVE.md`
- `docs/STAGE4_ROADMAP.md`
- `docs/VS_SCOPE.md`
- `docs/devlog/2026-05-06_stage3_closeout.md`

Source commits:

- `a0bd50b` latest implementation input: demo playable time-window brush repair.
- `aa847af` Stage 3 closeout docs and Stage 4 entry roadmap.

## 1. Gate Result

Stage 3 Vertical Slice is accepted complete. No immediate Stage 3 blocker remains open after the latest demo confirmation.

| Gate | Result | Evidence | Stage 4 effect |
|---|---|---|---|
| New game / demo flow | Accepted | `G5_ACCEPTANCE_MATRIX.md` §L, `2026-05-06_stage3_closeout.md` | Continue from polish and expansion, not Stage 3 repair. |
| Core time-window loop | Accepted | EditMode `32/32`, PlayMode `29/29`, brush preview / generated window match confirmed | Keep core loop stable while adding affordance and tutorialization. |
| Windows build | Accepted | Latest demo build path recorded in closeout devlog | Future build work is regression / release-candidate work. |
| Dialogue / localization | Accepted for Stage 3 | Locale and NPC dialogue tests green | Polish copy and readability in Stage 4. |
| Audio | Accepted for Stage 3 | Assets and wiring tests green; no reported Stage 3 audio blocker | Review mix / loop / replacement candidates in Stage 4. |
| Technical caveat | Accepted with backlog | URP `DrawObjectsPass` warning repeated in player logs | Schedule technical cleanup before public release hardening. |

## 2. Immediate Fixes

No immediate Stage 3 fixes are open.

If a new user-visible regression appears, handle it as a focused repair in a temporary worktree and update this document before starting broader Stage 4 production.

## 3. Stage 4 Backlog

| Priority | Workstream | Status | Owner | Next action | Exit check |
|---:|---|---|---|---|---|
| 1 | URP `DrawObjectsPass` / RenderGraph warning cleanup | Open technical debt | Codex | Prototype a RenderGraph-compatible `PortalStencilFeature` replacement or public `RenderObjectsPass` migration in an isolated worktree. | Player log warning count drops to 0; portal stencil smoke, portal boundary, and Windows build remain green. |
| 2 | Brush UX polish | Open polish | Codex + user review | Add affordance / tutorial hint plan first; code only after the intended on-screen hint is clear. | A new player can discover `Shift` + left-drag and right-click deletion without developer explanation. |
| 3 | Test-count reconciliation | Open verification hygiene | Codex | Identify why Unity Test Runner reports EditMode `32/32` while source marker scan finds 31 markers, or keep a durable explanation in verification docs. | Future verification docs can state one expected count without ambiguity, or clearly preserve the source/runner distinction. |
| 4 | Niro v2 sprite / hat silhouette review | Open art review | User decision + Codex asset work | Review Stage 3 v1 sprites against `STAGE3_REVIEW_AIDS.md`, then decide minor revision vs redraw vs hold. | Revised or retained sprite state is documented in asset ledger and visible in prefab preview. |
| 5 | TMP font / palette readability review | Open visual review | User decision + Codex docs/assets | Compare JP / EN dialogue panels and UI screenshots against current fonts and palette v0. | Keep / revise decision recorded; any replacement has license and atlas notes. |
| 6 | Dialogue v1 polish | Open content polish | Codex draft + user review | Polish Niro / Resident_A / Resident_B text without internal planning vocabulary; keep JP/EN key parity. | StringTable and DialogueAsset remain synchronized; dialogue tests pass. |
| 7 | Audio polish | Open audio review | User listening + Codex asset/docs | Review BGM loop, mix balance, time-window modulation, SFX replacement candidates, and ambience additions. | Keep / replace decisions are recorded per category; audio wiring tests remain green. |
| 8 | Verification gap coverage | Open QA expansion | Codex | Extend stress harness / warning regression / save-load scenario coverage only where it protects Stage 4 work. | `VERIFICATION_SUITE.md` reflects the new automated or manual coverage boundary. |
| 9 | Steam Early Access prep | Later Stage 4 | User decisions + Codex docs | Start after Phase 1 quality baseline is stable. | Store / legal / trailer / press-kit drafts are reviewable without private account data in repo. |

## 4. No Action

| Item | Reason |
|---|---|
| More Stage 3 repair by default | Stage 3 is accepted complete; new fixes require a newly observed regression. |
| Reopening the Stage 3 completion gate | `VS_SCOPE.md` v1.0 and `G5_ACCEPTANCE_MATRIX.md` record the accepted state. |
| Implementing future / blue symbol mechanics immediately | VS intentionally keeps future-side behavior for later Stage 4+ design. |
| Replacing accepted provisional art/audio/fonts without review | Stage 4 should compare before replacing player-facing assets. |
| Public Steam account / tax / payout data in repo | Steam preparation must keep private account details outside tracked docs. |

## 5. Recommended Dispatch Order

1. URP warning cleanup technical spike.
2. Brush UX affordance / tutorial hint plan and small implementation.
3. Test-count reconciliation if it blocks verification communication.
4. Visual review batch: Niro v2, palette, TMP fonts.
5. Dialogue and audio polish batches.
6. Verification hardening and Steam EA prep.

The first technical task should be isolated from art/audio/content work because it touches renderer behavior and has clear regression tests. If the renderer migration proves too large, explicitly defer it with a short decision note and move to brush UX polish.

## 6. Revision History

| Version | Date | Change |
|---|---|---|
| v0.1 | 2026-05-06 | Initial Phase 0 triage. Converts Stage 3 closeout facts into immediate fix, Stage 4 backlog, and no-action buckets. |
