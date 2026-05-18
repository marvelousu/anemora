# Chapter 1 TimeWindow V24 Public Preview Recovery

Date: 2026-05-14
Recovered: 2026-05-19
Source notes:
- `notes/_handover/anemora-timewindow-v24-claude-implementation-procedure-2026-05-14.md`
- `notes/_handover/anemora-timewindow-v24-public-preview-library-procedure-2026-05-14.md`
- `notes/_handover/anemora-timewindow-v24-publicfix-claude-sonnet-procedure-2026-05-14.md`
- `notes/_handover/anemora-chapter1-public-preview-presentation-pass-procedure-2026-05-14.md`

## Recovery note

The repo already had 2026-05-14 records for the Chapter 1 recovery baseline and canon lock,
but the surviving notes show a separate TimeWindow V24 / public-preview implementation chain
on the same date. No root-level devlog for that chain survived in the repo or notes history, so
this file reconstructs the missing day record from the procedure notes.

## Baseline

The accepted-ish baseline was the V24 TimeWindow pass that had fixed coordinate mismatch and
the sparse old-blockout look by copying the active current-side `MapRoot_HouseExterior`
visual stack into the paired past-side root. The next instruction explicitly kept the
coordinate, camera, and generation logic stable and limited the first pass to visual-state
differentiation on the copied past-side renderers.

Recorded baseline artifacts in notes used `<temp>/anemora_ch1_playable_twv24_visualparity_20260514`
and `<temp>/anemora_ch1_twv24_visualparity_smoke_20260514_visible`.

## V24 past visual pass

The first 2026-05-14 pass targeted the HouseExterior TimeWindow so it would read as a V24
window instead of a sparse runtime blockout:

- Keep current-side fixed camera behavior.
- Keep the same-coordinate visual-copy method.
- Alter only past-side renderer/material/color state after the current visual transform copy.
- Preserve entry, return, backside wall, and side-near/far wall checks.
- Keep broad Chapter 1 story/UI/camera work out of scope.

The notes record an expected output build under
`<temp>/anemora_ch1_playable_twv24_pastvisual_20260514` and a built-player smoke `RESULT PASS`.

## Library public-preview expansion

The next pass moved the feature from a HouseExterior-only technical demo toward a public-preview
library event:

- Fix vertical drag / aspect stretching so the past-side view would not look compressed or
  broken when the player dragged a tall window.
- Extend TimeWindow support to Library without broadening it to every map.
- Add a Library first-use event so the Chapter 1 VS had a visible TimeWindow beat rather than
  only a mechanical demo.
- Keep HouseExterior V24 behavior passing.
- Add milestone flags/events for portal committed, other-time entry, and current-time return
  where needed.
- Produce evidence frames for Library prompt, generated aperture, player depth in front,
  entered past, moved inside past, returned current, and event completion.

The note records output under `<temp>/anemora_ch1_playable_twv24_librarypreview_20260514` and
states that the built-player smoke reached `RESULT PASS`, with HouseExterior V24 and Library
V24 passing.

## Public-preview blocker fix

The same date then had a scoped blocker-fix procedure for the public preview candidate. User
feedback identified the build as not public-ready:

- TimeWindow had too little presentation value: weak frame and no generation effect.
- Reto interaction was missing or not firing as expected.
- After talking to Reto, movement could lock.
- Using TimeWindow in Niro's house could swap to old rejected character art.
- Library V24 was passing logs but still lacked presentation, routing, and verification.

The fix procedure prioritized:

- Public-preview allowlist for supported TimeWindow areas.
- No Niro art signature changes from public-preview TimeWindow actions.
- Safe Reto interaction and movement restoration.
- Readable TimeWindow frame / generation-effect presentation.
- Aspect clamp or near-baseline aspect enforcement for public preview stability.

The note records output under `<temp>/anemora_ch1_playable_twv24_publicfix_20260514`,
`RESULT PASS`, touched-file `git diff --check` passing, and a reminder not to commit unless Tom
explicitly asked.

## Presentation pass

The last 2026-05-14 procedure was a presentation pass after M0-M9 smoke/reporting had passed
but the public experience still felt weak. Its purpose was to add safe presentation and
guidance without finalizing story text:

- Make TimeWindow generation less abrupt after dialogue.
- Add or strengthen preview frame and drag-time presentation.
- Add a small commit/generation effect.
- Improve route and walkability readability.
- Add a house-exit / Timewriter-recognition presentation beat.
- Avoid broad Reto story rewrite or final canon text work.

The note records output under `<temp>/anemora_ch1_playable_presentationfix_20260514`,
`RESULT PASS`, touched-file `git diff --check` passing, and a report channel
`[Claude -> orchestration] Chapter1 presentation pass ready`.

## Boundaries

All 2026-05-14 source notes emphasize that these were scoped public-preview passes:

- Do not use graphics/character/runtime worktrees for this implementation chain.
- Do not re-enable old TimeFrame / TimeWindow / Diorama systems.
- Do not import new characters or generic NPCs.
- Do not commit/push unless Tom explicitly asks.
- Keep the work in the Chapter 1 implementation worktree.

This recovery therefore records the work sequence and verification status, not a direct commit
hash from the original day.
