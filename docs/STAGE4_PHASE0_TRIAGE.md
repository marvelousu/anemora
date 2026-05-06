# Stage 4 Phase 0 Triage

Status: v0.5 Stage 4 entry triage (2026-05-06)

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
| Technical caveat | Resolved in Stage 4 Phase 0 | URP `DrawObjectsPass` warning repeated in historical player logs; current player log count is `0` after public `RenderObjectsPass` migration | Keep warning-count regression coverage in portal smoke / performance runs. |

## 2. Immediate Fixes

No immediate Stage 3 fixes are open.

If a new user-visible regression appears, handle it as a focused repair in a temporary worktree and update this document before starting broader Stage 4 production.

## 3. Stage 4 Backlog

| Priority | Workstream | Status | Owner | Next action | Exit check |
|---:|---|---|---|---|---|
| 1 | URP `DrawObjectsPass` / RenderGraph warning cleanup | Resolved in this task | Codex | Replaced internal `DrawObjectsPass` with public `RenderObjectsPass`; added PlayMode warning-count assertion. | EditMode `32/32`, PlayMode `29/29`, Windows build success, 30 second player warning count `0`. |
| 2 | Brush UX polish | Initial runtime hint implemented | Codex + user review | Added a lightweight runtime overlay for create / release / close hints without scene YAML changes. Later UI review can replace it with a localized icon treatment. | A new player can discover `Shift` + left-drag and right-click deletion without developer explanation. |
| 3 | Test-count reconciliation | Resolved as documented runner/source distinction | Codex | Character v2 added three EditMode `[Test]` methods, moving the source-marker count from 31 to 34; the recorded Unity runner count moved from `32/32` to `35/35`, so the historical +1 runner/source delta remains. | Verification docs state the executed runner baseline as EditMode `35/35`, PlayMode `29/29`, with source markers tracked separately as EditMode 34 and PlayMode 29. |
| 4 | Character v2 redraw + Resident review | Resident_A F/F4 review ready | User review + Codex asset work | Hero and Resident_B are now the accepted visual reference. Resident_A v2 remains imported, but runtime review found its pixel feel too strong and its face/head scale too large against the Hero. C3 fix C was demoted because its head/body connection reads detached; G was demoted because the hair is too short; F2 was rejected by user. | Current viable read is F or F4, not F2. F reads more young / anxious witness; F4 reads more composed / slightly older. Before import, re-ground the choice in Resident_A's role: Past-side young resident witness, no prior relationship with Niro, uneasy foreshadowing toward the declining current side. |
| 5 | TMP font / palette readability review | Initial review recorded | User decision + Codex docs/assets | `docs/devlog/2026-05-06_stage4_tmp_palette_readability_review.md` records current prefab/font/palette risks. Next capture JP / EN dialogue screenshots before changing font assets. | Keep / revise decision recorded; any replacement has license and atlas notes. |
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

1. Character art follow-up: review the Resident_A A/B/C sheet first, using Hero and Resident_B as the scale / pixel-granularity reference; handle any later scene-specific Resident_B diagonal orientation only if the runtime scene read needs it.
2. TMP / palette screenshot capture and small UI polish decision.
3. Dialogue and audio polish batches.
4. Verification hardening and Steam EA prep.

The first technical task was the URP warning cleanup. It is now resolved for Phase 0; keep warning-count regression checks when changing portal rendering or performance harness code.

## 6. Revision History

| Version | Date | Change |
|---|---|---|
| v0.9 | 2026-05-07 | Records user rejection of F2 and narrows Resident_A review to F or F4, pending role-framed selection. |
| v0.8 | 2026-05-07 | Records user preference for F over G, adds F2/F3/F4 F-based comparison, and initially identifies F2 as strongest current review candidate. |
| v0.7 | 2026-05-07 | Adds Resident_A F/G/H connected-candidate nearest-neighbor comparison; G is the strongest current review candidate. |
| v0.6 | 2026-05-07 | Demotes C3 fix C after user review: head/body connection reads detached and the preview is too blurry for approval. |
| v0.5 | 2026-05-06 | Records Resident_A A/B/C follow-up review sheet as the next user gate before runtime replacement. Follow-up initially identified C3 fix C as the best base after trying both local headfix and Hero-ratio regeneration. |
| v0.4 | 2026-05-06 | Records post-runtime Resident_A follow-up: reduce over-strong pixel feel and face/head scale, with Hero and Resident_B as the visual reference. |
| v0.3 | 2026-05-06 | Records initial TMP / palette readability review and moves the next action to screenshot capture before UI asset changes. |
| v0.2 | 2026-05-06 | Resolves test-count reconciliation as a documented runner/source distinction after character v2 added three EditMode source markers. |
| v0.1 | 2026-05-06 | Initial Phase 0 triage. Converts Stage 3 closeout facts into immediate fix, Stage 4 backlog, and no-action buckets. |
