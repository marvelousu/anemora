# Stage 4 Audio Verification + Dialogue Review Prep

Date: 2026-05-07

## Summary

This batch keeps Stage 4 in Phase 1 quality reinforcement. It does not change in-game dialogue, audio assets, scenes, or runtime behavior.

## Audio Verification

Expanded `Zone1AudioWiringTests`:

- `MainSceneHasZone1AudioControllerWithCoreClips`
  - Now checks clip load state and clip length in addition to non-null references.
- `MainSceneHasZone1AudioSourcesWithSafeRuntimeSettings`
  - New test for Zone1AudioController source existence, loop settings, volume range, spatialBlend range, and distance sanity.
- `MainSceneHasNpcDialogueAudioClips`
  - Keeps NPC / DialogueDisplay clip checks and adds optional AudioSource sanity.

Verification:

- PlayMode: `31 passed / 32 total`.
- The one skipped test is the `[Explicit]` TMP screenshot capture harness.

Manual audio review is still required for:

- BGM loop seam and noise.
- Mix balance.
- Time-window modulation feel.
- NPC interaction / dialogue advance / close timing.
- Ambience one-shot frequency and mood fit.

## Dialogue Review

Created a review-only proposal sheet:

- `docs/devlog/2026-05-07_dialogue_v1_polish_review_sheet.md`

No StringTable or DialogueAsset values have been changed. The sheet keeps all current keys and turn counts so approved rows can be applied as same-key locale value changes.

Open user decisions:

- Whether Resident_A label should become `若い住人` / `Young Resident` instead of `少女` / `Girl`.
- Whether Resident_A should lean further into Past-side everyday life rather than direct building warning.
- Whether Niro and Resident_B should strengthen the subtle "things diminish after being noticed" hint.
- Whether Resident_B JP label should stay `記録者` or become the plainer `記録係`.

## Resident_A P1 Sheet Prep

Production constraints are recorded separately in:

- `docs/devlog/2026-05-07_resident_a_p1_production_sheet_spec.md`

No runtime art import has been performed.
