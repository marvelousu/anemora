# Chapter 1 Scene 5 Chapter Transition

Date: 2026-05-10
Worktree: `<worktree>`

## Summary

Implemented the first scaffold for scene 5 [5.E]: Niro auto-walk, pebble kick, fade out, chapter title display, intermediate save data, and provisional next-chapter handoff.

This is not final playable sign-off. Animation feel, title presentation, camera timing, and save-service integration still require review.

## Runtime

Added `Assets/Scripts/Chapter/ChapterTransitionController.cs`.

State flow:

1. `AutoWalk`
2. `KickStone`
3. `FadeOut`
4. `Title`
5. `Save`
6. `NextChapter`
7. `Complete`

Implemented data points:

- `Chapter1CompleteFlag = "chapter1_complete"`
- `PebbleKickedFlag = "chapter1.pebble.kicked"`
- `pebbleRecordId = "chapter1_pebble_001"`
- `pebbleFutureRecoverySceneId = "chapter2_scene4"`
- provisional save slot: `chapter1_autosave`
- provisional next scene: `Anemora_Chapter2`

## Assets

Generated / added:

- `Assets/Prefabs/Chapter/Chapter1_Pebble.prefab`
- `Assets/Animations/Chapter1/Niro_Kick_Pebble.anim`

The pebble prefab is a one-time data-driven object intended to seed the later BF1 recovery.

## Scene Wiring

`Assets/Editor/AnemoraChapter1SceneSetup.cs` now creates and wires:

- `Chapter1_TransitionSequence`
- `Niro_AutoWalk_Target_Chapter1Close`
- `Chapter1_Pebble_BF1_Seed`
- `Chapter1_TransitionCanvas`
- `Chapter_Title_Group`
- `Chapter_Title_JP`
- `Chapter_Title_EN`

The setup assigns `PlayerProgressionRuntime`, title UI, fade UI, kick clip, pebble object, and player / auto-walk target references.

## Save Behavior

`ChapterTransitionController` builds an intermediate `SaveEnvelope` and writes:

- `chapter1_complete`
- `chapter1.pebble.kicked`
- `chapter1_pebble_001` as a reflected `ActionRecordEntry` with `targetObjectId = "chapter2_scene4"` and `ActionType.Push`

It also includes current player position, scene id, zone id, current-side time-frame state, and current action record save data when available.

The implementation currently writes JSON directly under `Application.persistentDataPath/saves/<slot>.json` because the full production save service is still ADR-level and not implemented.

## Verification

- Added `Assets/Tests/EditMode/ChapterTransitionControllerTests.cs`.
- Test verifies the intermediate save envelope contains `chapter1_complete` and `chapter1.pebble.kicked`.
- Added `Assets/Tests/PlayMode/ChapterTransitionControllerPlayModeTests.cs`.
- PlayMode smoke verifies `BeginTransition()` reaches `Complete`, writes the autosave JSON, records `chapter1_complete` / `chapter1.pebble.kicked`, and moves the BF1 pebble object.
- Unity batchmode compile/import succeeded after the scene setup pass.
- Scene YAML confirms `ChapterTransitionController`, `Chapter1_Pebble_BF1_Seed`, `Chapter1_TransitionCanvas`, and `chapter1_complete` are present.
- Targeted PlayMode: `ChapterTransitionControllerPlayModeTests` `1/1` passed.
  - XML: `<temp>\anemora_ch1_impl_chapter_transition_playmode.xml`
- Full PlayMode after later sightline audio, Scene 1 book trace runtime, Scene 4 trace runtime, Scene 4 auto-trigger runtime coverage, and SymbolWheel scene progression coverage: `63/63` passed.
  - XML: `<temp>\anemora_ch1_impl_playmode_after_scene1_book_trace.xml`
- Added a trigger-polling fallback to `ChapterTransitionController.Update()` so the scene-wired chapter close can fire when the prototype player has a `CapsuleCollider` but no `Rigidbody`. `OnTriggerEnter` remains supported.
- Scene-wired PlayMode now moves the actual Chapter 1 player into `Chapter1_TransitionSequence` and verifies completion through the trigger path, save flags, pebble movement, and title text.
- Targeted EditMode: `ChapterTransitionControllerTests` `1/1` passed after trigger-polling fallback.
  - XML: `<temp>\anemora_ch1_impl_chapter_transition_editmode_after_polling.xml`
- Targeted PlayMode: `ChapterTransitionControllerPlayModeTests` `2/2` passed after trigger-polling coverage.
  - XML: `<temp>\anemora_ch1_impl_chapter_transition_trigger_polling.xml`
- Full PlayMode after route occupancy and scene-wired chapter transition trigger coverage: `65/65` passed.
  - XML: `<temp>\anemora_ch1_impl_playmode_after_transition_polling.xml`
- Runtime validator after trigger-polling fallback: `Info=107`, `Warning=3`, `Error=0`, `PendingWiring=0`.
  - Log: `<temp>\anemora_ch1_impl_runtime_validator_after_transition_polling.log`
- Windows build smoke after trigger-polling fallback: success.
  - output: `<temp>\anemora_ch1_build_smoke_after_transition_polling\Anemora.exe`
  - log: `<temp>\anemora_ch1_impl_build_smoke_after_transition_polling.log`
  - result line: `Build Finished, Result: Success`

## Review Fix Pass

A code review found transition edge cases after the scene-wired trigger pass. The implementation now covers:

- same-scene completion when `loadNextChapterScene = false`: fade/title UI is hidden again and the previous player controller enabled state is restored
- BF1 pebble persistence as both raw flags and an `ActionRecordEntry` (`chapter1_pebble_001`, `chapter2_scene4`, `ActionType.Push`, `reflected = true`)
- trigger polling by trigger/player collider overlap instead of player pivot only
- no inside-state latch when the player is already inside the trigger but a required progression flag is still missing
- route occupancy smoke checks only the intended section centers, avoiding over-specified straight-line center-to-center paths

Review-fix validation:

- Targeted EditMode: `ChapterTransitionControllerTests` `1/1` passed.
  - XML: `<temp>\anemora_ch1_impl_chapter_transition_reviewfix_editmode.xml`
- Targeted PlayMode: `ChapterTransitionControllerPlayModeTests` `3/3` passed.
  - XML: `<temp>\anemora_ch1_impl_chapter_transition_reviewfix_playmode_retry.xml`
- Full EditMode: `172 total / 150 passed / 0 failed / 22 skipped`.
  - XML: `<temp>\anemora_ch1_impl_editmode_after_reviewfix.xml`
- Full PlayMode: `66/66` passed.
  - XML: `<temp>\anemora_ch1_impl_playmode_after_reviewfix.xml`
- Runtime validator: `Info=107`, `Warning=3`, `Error=0`, `PendingWiring=0`.
  - Log: `<temp>\anemora_ch1_impl_runtime_validator_after_reviewfix.log`
  - Remaining warnings are the accepted S4D/S4G temporary placement deltas.
- Windows build smoke: success.
  - output: `<temp>\anemora_ch1_build_smoke_after_reviewfix\Anemora.exe`
  - log: `<temp>\anemora_ch1_impl_build_smoke_after_reviewfix.log`
  - result line: `Build Finished, Result: Success`

## Remaining Work

- In-editor transition timing review.
- Camera / route finalization around scene 5.
- Final chapter title review.
- Final Niro kick animation polish.
- Replace direct JSON write with production save service when implemented.
- Confirm whether `loadNextChapterScene` should remain false until Chapter 2 scene exists.

## Review Fix 2 Validation

Follow-up code review found two remaining transition persistence risks: the BF1 pebble action record was only guaranteed in the saved envelope, not the live `ActionRecordRuntime`, and save failure could leave runtime progression/action state committed before the file write was durable.

The controller now builds the save envelope without mutating runtime progression, writes the temporary save file first, atomically replaces the final save, and only then commits `chapter1_complete`, `chapter1.pebble.kicked`, and the live `chapter1_pebble_001` runtime action record. If save fails, same-scene UI/player state is restored and completion flags / pebble record are not committed.

Additional verification:

- Targeted EditMode: `ChapterTransitionControllerTests;ActionRecordCatalogTests` `5/5` passed.
  - XML: `<temp>\anemora_ch1_impl_reviewfix2_editmode_targeted.xml`
- Targeted PlayMode: `ChapterTransitionControllerPlayModeTests` `4/4` passed, including the save-failure rollback case.
  - XML: `<temp>\anemora_ch1_impl_reviewfix2_transition_playmode_failurecase.xml`
- Full EditMode: `173 total / 151 passed / 0 failed / 22 skipped`.
  - XML: `<temp>\anemora_ch1_impl_editmode_after_reviewfix2.xml`
- Full PlayMode: `67/67` passed.
  - XML: `<temp>\anemora_ch1_impl_playmode_after_reviewfix2_failurecase.xml`
- Runtime validator: `Info=107`, `Warning=3`, `Error=0`, `PendingWiring=0`.
  - Log: `<temp>\anemora_ch1_impl_runtime_validator_after_reviewfix2.log`
  - Remaining warnings are still the accepted S4D/S4G temporary placement deltas.
- Windows build smoke: success.
  - output: `<temp>\anemora_ch1_build_smoke_after_reviewfix2\Anemora.exe`
  - log: `<temp>\anemora_ch1_impl_build_smoke_after_reviewfix2.log`
  - result line: `Build Finished, Result: Success`
