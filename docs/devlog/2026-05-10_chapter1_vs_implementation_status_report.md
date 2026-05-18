# Chapter 1 VS Implementation Status Report

Date: 2026-05-10
Worktree: `<worktree>`
Branch: `codex/stage4-chapter1-implementation-20260510`

Note: the requested source note `<notes>\_handover\anemora-chapter1-vs-implementation-status-query-2026-05-09.md` was not present locally at report time. This report follows the 12 requested items pasted into the orchestration session.

## 1. Executive Summary

- Chapter 1 implementation has been started in a new production scene, `Assets/Scenes/Anemora_Chapter1.unity`; `Anemora_Main.unity` remains as the Stage 3 vertical-slice reference.
- The 2026-05-09 story/spec direction is partially implemented, not merely planned: new scene scaffold, graphics aggregate import, Resident_A / Resident_B dialogue migration, portal v3.2 runtime extension, SymbolWheel first-loop behavior, audio extension, trace visualization hooks, and chapter transition controller all exist in the worktree.
- The new east / north-east map direction is represented by imported Chapter 1 map/detail production prefabs and aggregate graphics prefabs. The integrated scene now has capture targets and a both-mode capture package (`20260510_130917`) after refocusing the Kaia field view, polishing the CP-2 path-light, and tightening the Kaia inspection target; graphics review classified it as `needs polish` with no blocker.
- TimeFramePortalSystem v3.2 is implemented as an extension of the existing Stage 3 controller. Stage 3 manual symbol / plane crossing compatibility remains present.
- Dialogue assets for Aria and Reto have been migrated away from the old Stage 3 generic Resident_A / Resident_B assets.
- Scene 1 book trace restoration is wired in the Chapter 1 scene through `PastBookInteractable`, `take_book_001`, and `BookReflector.reflectedBookObject`.
- Scene 4 auto-trigger and multi-object trace systems are now scene-wired for S4D / S4G / S4F, including `Niro_BrushReaction.asset` and `touch_spice_jar_001`.
- Scene 5 chapter transition implementation exists and is scene-wired at a scaffold level; PlayMode smoke now verifies `BeginTransition()` reaches `Complete` and writes the intermediate save. It still needs animation / presentation review in-editor.
- Latest local validation after review-fix hardening: runtime validator runs against `Anemora_Chapter1.unity` with `Info=107, Warning=3, Error=0, PendingWiring=0`; the three warnings are accepted S4D/S4G temporary placement deltas. Capture plan validation for `20260510_135626` exits with `errors=0`, `warnings=3` static-scan graphics-root warnings, while the actual capture manifest is `errors=0`, `warnings=0`, `capture_count=18`. Full EditMode is `172 total / 150 passed / 0 failed / 22 skipped`, full PlayMode is `66/66` passed, Zone1 audio low-pass scene serialization verifies cleanly, and the latest Windows build smoke succeeds after the chapter-transition review fixes.
- This is not yet VS playable sign-off. Build/compile success does not prove subjective playability, flow, readability, or story pacing.

## 2. Twelve-Item Status

### 1. Scene Structure: `Anemora_Main` vs New Scene

Current status: implementation now follows the approved new-scene direction.

`Assets/Scenes/Anemora_Chapter1.unity` has been created and added to `ProjectSettings/EditorBuildSettings.asset`. `Anemora_Main.unity` is intentionally retained as the Stage 3 VS reference and backward compatibility target.

`Assets/Editor/AnemoraChapter1SceneSetup.cs` owns repeatable setup for Chapter 1 scene roots, section trigger markers, camera anchors, `ChapterCameraRigController` wiring, player blocker-collision settings, graphics aggregate placement, runtime root wiring, book reflection wiring, audio setup compatibility, and chapter transition scaffold.

The scene currently contains Chapter 1 root organization including current/past roots, graphics aggregate instances, runtime root, SymbolWheel/progression hook, TimeFramePortalSystem, Zone1 audio, and chapter transition objects.

Remaining work: final section trigger placement review, camera composition tuning, hands-on collision feel review, S4D/S4G timing review, and playable review.

### 2. Map Blockout / Placement: Old Southward Route vs New East / North-East Route

Current status: new map asset integration is started, but final playable map composition is incomplete.

Graphics integration brought in `Chapter1MapProduction`, `Chapter1DetailKitProduction`, and `Chapter1GraphicsIntegration` prefab sets. The implementation scene instantiates `Ch1_GraphicsRoute_Current.prefab` and `Ch1_GraphicsRoute_Past.prefab` as `Chapter1_GraphicsIntegration_Current` and `Chapter1_GraphicsIntegration_Past`.

The available asset package covers Niro house, central plaza, library area, street / corner assets, Aria house, Mia house, Kaia field, ruin foreshadowing area, and detail kit props. This matches the new broader Chapter 1 route direction rather than the older narrow southward path.

Static validation from the graphics session passed against the implementation worktree: 2 aggregate prefabs, 75 map production prefabs, and 18 detail-kit production prefabs are present, with no zero-GUID scripts, renderer missing material slots, or missing nested prefab sources.

Remaining work: scene-level playable placement, foreground occlusion tuning, route readability review, camera framing, and implementation-owned visual polish from the latest graphics review.

### 3. TimeFramePortalSystem: Old Version vs v3.2

Current status: v3.2 extension is implemented in code and partially wired in the Chapter 1 scene.

`TimeFramePortalController` now supports local-window behavior through `useLocalDioramaWindow`, `TimeWindowBoundaryContext`, `TimeFrameAutoTriggerRequest`, and `RequestAutoTrigger(...)`. The existing Stage 3 manual symbol and plane-crossing path remains available when local-window mode is not active.

`PortalCrossingDetector` has an `IsInsideBoundary(...)` path for internal boundary checks. `TimeFrameLayerPolicy` and interaction exclusion work support the rule that past-side NPCs do not treat Niro as an ordinary interactable participant.

`PastActionInteractable` supports touch/take/push-style action recording, and `GameObjectVisibilityReflector` supports multi-object trace visibility for later scene 4 use.

ADR coverage exists in `docs/adr/0010-time-window-mode-v3-2.md`, with updates to ADR-0002 and ADR-0005. Runtime validator now confirms the Stage 3 manual fallback method is still present.

Remaining work: actual S4D/S4G trigger placement, fine boundary tuning per indoor/outdoor/ruin area, and in-editor gameplay verification.

### 4. Character Prefab Placement: Hero v2 / Existing NPC / New NPC

Current status: existing character prefabs are still the active implementation base; final character-generation integration is blocked.

Existing prefabs under `Assets/Prefabs/Characters/` remain the primary scene assets: Hero, Resident_A, and Resident_B. `Anemora_Chapter1.unity` scene setup reuses the current player / NPC infrastructure rather than introducing final new character sprite packages.

Character generation session is currently blocked on PixelLab credits: `/balance` returned `$0.0000`, so missing source PNG generation did not proceed. No final generated source PNG batch has been integrated.

Reto v8 expression / motion carry-forward is not yet integrated as final production sprite wiring. Current dialogue / interaction wiring can proceed with existing or placeholder sprites.

New NPCs implied by the 2026-05-09 story/spec direction are not fully placed as final production prefabs in this implementation worktree yet.

Remaining work: character-generation unblock, final sprite import, animator/prefab update, scene placement, and visual QA with the final character silhouettes.

### 5. Dialogue: Stage 3 Lore-Aware State / 2026-05-09 Replacement

Current status: Resident_A and Resident_B migrations are implemented for the requested Chapter 1 sections.

Old `Resident_A_Greeting.asset` has been removed. New assets exist for `Resident_A_Past_Library_Glimpse.asset` and `Resident_A_Past_AriaHouse_Lesson.asset`, with string keys migrated to `dialogue.scene1.past_aria.*` and `dialogue.scene3.aria_house.*`.

Old `Resident_B_Idle.asset` has been replaced by section-specific Reto assets: `Resident_B_Scene1_B_Initial.asset`, `Resident_B_Scene1_C_LibraryHistory.asset`, `Resident_B_Scene1_D_BrushReaction.asset`, `Resident_B_Scene1_F_BookAppears.asset`, and `Resident_B_Scene1_G_MiaHint.asset`.

StringTable keys have moved to `dialogue.scene1.reto.*`. Related docs and dialogue tests have been updated.

`Anemora_Main.unity` remains compatible where practical: the Resident_B asset rename preserved the original GUID for the initial asset path.

Remaining work: final scene dialogue trigger placement, full S1-S5 dialogue coverage beyond these migrated Resident_A/B points, final story review, and Japanese/English text polish.

### 6. SymbolWheel / UI: Three Symbols vs First-Loop Red Only

Current status: first-loop symbol behavior is implemented.

`SymbolWheelController` now hides the white symbol, keeps the blue symbol preview/disabled during the first loop, and allows red selection. Blue unlock is driven by `PlayerProgressionRuntime` and the raw progression flag `progression.time_symbol.blue_unlocked`.

`SaveEnvelope.progressFlags.rawFlags` now has helper behavior for raw flag persistence. `PlayerProgressionRuntime` loads/exports these flags and notifies UI listeners.

`Assets/UI/Prefabs/SymbolWheel.prefab` has been updated to match the first-loop behavior. Runtime validator checks the SymbolWheel contract where the component is present.

Tutorial UI text for scene 1 [1.D] is not fully production-wired yet. The required four-line instruction flow exists as implementation intent but still needs final UI placement and review.

Remaining work: final tutorial dialogue/UI timing, save/load integration test inside real flow, and second-loop unlock validation.

### 7. Audio: Zone1 Current State / Scene 5 Zone Direction

Current status: Zone1 audio has been extended for Chapter 1 and wired into `Anemora_Chapter1.unity`.

`Zone1AudioController` now supports scene 5 low-pass behavior for `Zone1_Ambient.ogg` and exposes one-shot methods for Chapter 1 SFX: ruin dust, ruin wind, chapter close, time brush reaction, and nuts fall.

New provisional WAV assets exist under `Assets/Audio/SFX/Zone1/chapter1/`: `sfx_env_ruin_dust_01.wav`, `sfx_env_ruin_wind_01.wav`, `sfx_env_chapter_close_01.wav`, `sfx_time_brush_react_01.wav`, and `sfx_nuts_fall_01.wav`.

`Zone1AudioSceneSetup.ConfigureChapter1Scene` has been run against `Anemora_Chapter1.unity`, and scene YAML confirms the Chapter 1 clip references are assigned.

After the low-pass setup refresh, `Music_Source` has a serialized `AudioLowPassFilter` and `Zone1AudioController.musicLowPassFilter` is assigned in the scene. `Zone1AudioSceneSetup.VerifyChapter1Scene` passes, and `Zone1AudioWiringTests` is `4/4`.

Existing Zone1 ambient and Stage 3 SFX infrastructure remain available.

Remaining work: in-editor listening pass, low-pass tuning for scene 5, final replacement if provisional SFX are not acceptable, and capture/playable timing validation.

### 8. Visual Guidance / Direction: Scene 2 [2.F]

Current status: CP-2 wiring is implemented, but not yet final-playable signed off.

`Scene2SightlineRevealController`, `Chapter1SightlineRevealProfile`, `Chapter1_Scene2_SightlineReveal`, and `Chapter1_Scene2_PathLight` are present. The controller preserves the requested no-UI, no-fade, seamless transition style while driving the light transition, east-positioned audio route, path light activation, and two Niro monologue cues.

`Zone1AudioController` has the three sightline cue routes and clips assigned after rerunning `Zone1AudioSceneSetup.ConfigureChapter1Scene`. Runtime validator category output includes `CP2SightlineReveal=29`, including dapple patch count, collider-free, and blockout-plane scale checks.

Graphics review of capture `20260510_130917` keeps CP-2 at `needs polish`, not blocker: central plaza / street corner direction, no-UI route, and no-fade route are acceptable as direction, while the path-light still reads more like broad translucent / blockout geometry than subtle warm environmental guidance.

Remaining work: in-editor listening pass, path-light visual polish, timing review through the actual Scene 2 to Scene 3 route, and final capture-based acceptance.

### 9. Chapter Transition: Scene 5 [5.E]

Current status: first implementation exists and is scene-wired at scaffold level.

`Assets/Scripts/Chapter/ChapterTransitionController.cs` implements a state machine: Niro auto-walk, pebble kick, fadeout, chapter title display, intermediate save, next-chapter handoff, and complete state.

Scene setup creates `Chapter1_TransitionSequence`, `Chapter1_TransitionCanvas`, `Niro_AutoWalk_Target_Chapter1Close`, and a one-off `Chapter1_Pebble_BF1_Seed` instance. It also generates `Assets/Prefabs/Chapter/Chapter1_Pebble.prefab` and `Assets/Animations/Chapter1/Niro_Kick_Pebble.anim`.

The controller writes `chapter1_complete` and `chapter1.pebble.kicked` raw flags into the save envelope and supports a provisional non-loading next chapter mode. After review-fix hardening, same-scene completion restores fade/title UI and the previous player-controller enabled state when `loadNextChapterScene = false`.

`ChapterTransitionControllerTests.cs` verifies the intermediate save envelope flag behavior and the persisted BF1 pebble `ActionRecordEntry`. `ChapterTransitionControllerPlayModeTests.cs` verifies that `BeginTransition()` reaches `Complete`, writes the autosave JSON, records `chapter1_complete` / `chapter1.pebble.kicked`, persists `chapter1_pebble_001` for `chapter2_scene4`, moves the BF1 pebble object, and restores the same-scene UI/player state.

Remaining work: in-editor animation review, title typography review, save service alignment when the production save layer exists, and final story/pacing approval.

### 10. Trace Visualization: Personal-Level Interference

Current status: scene 1 book trace and scene 4 CP-1 multi-object trace support are implemented and scene-wired.

Scene 1 [1.E]/[1.F] uses `PastBookInteractable` with `actionId = take_book_001`, `targetObjectId = Book_Family_Past_001`, and delayed reflection. `BookReflector.reflectedBookObject` points at the inactive current-side book object, so closing/exiting the window can activate `Book_Family_Current` instead of only spawning a prefab.

`BookReflector` now supports both prefab spawn and activation of an existing reflected object. Runtime validator confirms the scene 1 actionId contract and reflector target/fallback are satisfied.

Scene 4 [4.F] now uses `Scene4_T4_TraceManifest.asset`, `Chapter1_Scene4_TraceVisuals`, and `Chapter1_TraceManifestReflector_S4F` to reflect 10 trace bindings for the mixed Kaia field design. The legacy `Chapter1_GameObjectVisibilityReflector_S4F` remains as a compatibility path, while `ActionRecordRuntime` keeps the implementation-owned multi-reflector dispatch/restoration behavior.

After graphics review found capture `20260510_093539` still blocked by Kaia field framing, the Kaia field scene cluster and `Ch1_CaptureTarget_kaia_field` were refocused. Capture `20260510_102837` improved the Kaia-field framing, CP-2 path-light polish produced `20260510_105109`, and a further Kaia inspection target shift produced `docs/devlog/screenshots/chapter1_scene_integrated/20260510_130917/`. Graphics review of `20260510_130917` cleared the CP-1 blocker and classified CP-1 as `needs polish`. Implementation then applied the CP-1 / CP-2 polish pass and generated `docs/devlog/screenshots/chapter1_scene_integrated/20260510_135626/`, with `errors=0`, `warnings=0`, 18 PNGs, and no fallback targets. This latest package is pending graphics visual verdict.

Remaining work: graphics verdict on the `20260510_135626` capture, gameplay verification for scene 4 timing, and in-editor playthrough review.

### 11. Tests / VS Playable Verification

Current status: automated compile and selected validation are passing, but VS playable verification is not complete.

Unity batchmode compile/import passes after runtime validator integration and later validator refresh.

Runtime scene validator runs against `Anemora_Chapter1.unity` and exits successfully. The latest validator result is `Info=107, Warning=3, Error=0, PendingWiring=0`, with `CP1Scene4Trace=56` and `CP2SightlineReveal=30`. The three warnings are the accepted S4D/S4G boundary/window-size deltas.

Capture plan validation after the `20260510_130917` target shift exits with `errors=0`, `warnings=3`, `viewpoints=9`, `sceneExists=True`. The three warnings are static-scan graphics root name warnings; the actual `20260510_130917` capture manifest has `errors=0`, `warnings=0`, `capture_count=18`, and no fallback targets.

Graphics static validation passes against the implementation worktree. `Chapter1MapAssetTests` now pass the previously failing generated-review artifact checks (`71 total / 54 passed / 0 failed / 17 skipped`) after generating the missing Unity/DQ3R review PNGs.

Targeted implementation tests pass for the latest runtime validator import: `Chapter1RuntimeSceneValidatorTests` is `14/14`, `Chapter1SceneCapturePlanTests` is `6/6`, `Chapter1DialogueAssetTests` is `7/7`, and `Zone1AudioWiringTests` is `4/4` after low-pass serialization. After CP-2 path-light contract hardening, CP-2 sightline enter-edge and audio route coverage, camera rig controller wiring, player boundary collision wiring, player axis-slide collision coverage, section trigger enter-edge hardening, chapter transition PlayMode smoke, actual Chapter 1 scene-load smoke coverage, Scene 1 book trace runtime coverage, Scene 4 trace reflection runtime coverage, Scene 4 S4D/S4G auto-trigger runtime coverage, Chapter 1 scene SymbolWheel progression coverage, dialogue key migration hardening, progression raw flag duplicate normalization tests, SaveEnvelope raw flag round-trip tests, and chapter-transition review-fix hardening, full EditMode is `172 total / 150 passed / 0 failed / 22 skipped`, and full PlayMode is `66/66` passed.

Windows build smoke to a repo-external Temp output succeeded. The latest review-fix build is:

- output: `<temp>\anemora_ch1_build_smoke_after_reviewfix\Anemora.exe`
- log: `<temp>\anemora_ch1_impl_build_smoke_after_reviewfix.log`
- result: `Build Finished, Result: Success`
- note: the log contains CodeCoverage package `System.Numerics` resolution noise, but the player build completed and the executable was generated.

Remaining work: graphics review of `20260510_135626`, in-editor playthrough, camera/collision verification, and subjective playable review.

### 12. Active Tasks + Blockers

Active implementation tasks:

- Finish scene assembly for `Anemora_Chapter1.unity`: final section trigger tuning, path blockers, and final scene object references.
- Verify scene 4 S4D/S4G `TimeFrameStoryAutoTrigger` timing and scene 4 `TraceManifestReflector` / compatibility reflector behavior in play.
- Await graphics verdict for capture package `20260510_135626`; the CP-1 / CP-2 polish pass has already been applied and recaptured.
- Continue any implementation-owned visual polish only after the new graphics verdict.
- Run final `git diff --check` and any follow-up tests after graphics/runtime feedback.
- Continue tightening route readability, collision/blocker setup, and in-scene camera composition.

Blockers / dependencies:

- Character final sprite generation is blocked by PixelLab zero balance.
- Final visual/camera polish depends on the graphics review of `20260510_135626` and later hands-on route review.
- Story/session final naming and any core story changes remain Linux/Tom decision territory.
- VS playable completion requires subjective review; automated compile/test pass alone is insufficient.

## 3. Impact on Linux-Side D-1 / D-2 / D-3 Decisions

### D-1

If D-1 concerns scene structure / route direction, implementation has already moved toward the 2026-05-09 direction: new `Anemora_Chapter1.unity`, imported Chapter 1 route graphics, and `Anemora_Main` retained only as Stage 3 reference.

The practical impact is that Linux-side story/spec can assume the old single VS scene is no longer the production target for Chapter 1. However, exact section trigger positions and camera beats are not final, so D-1 should not depend on final coordinates yet.

### D-2

If D-2 concerns time-window v3.2 / interaction rules, the code direction is accepted and implemented as extension A. Existing Stage 3 behavior remains compatible.

Linux-side spec can proceed assuming local-window movement, delayed trace reflection, first-loop red-only symbol behavior, and story auto-trigger hooks are available. S4D/S4G are placed in the scene now, but final beat timing still needs review.

### D-3

If D-3 concerns playable / visual sign-off, implementation is not ready for final approval. Asset/static validation is good, the `20260510_130917` blocker was cleared, and the implementation-owned CP-1 / CP-2 polish recapture `20260510_135626` is available. The graphics verdict on that package and in-editor playthrough remain pending.

Linux-side decisions that require subjective feel, route readability, emotional pacing, or final scene 5 presentation should wait for integrated captures and hands-on review.

## 4. Blockers / Questions for Linux Claude

- The requested query handover file was not present locally. If it contains additional constraints beyond the pasted 12 items, resend or restore it in `notes/_handover/`.
- Confirm whether scene 2 [2.F] visual guidance has a final intended trigger/camera/audio beat, or whether implementation should choose a conservative provisional beat.
- Confirm whether S4D/S4G request IDs and completed flags should remain implementation-draft names or be aligned to story-owned final naming now.
- Confirm whether the chapter title text for [5.E] is final enough to keep in the scene, or should remain provisional until Tom/story review.
- Character final sprite integration is blocked externally by PixelLab credit. Confirm whether placeholder sprites are acceptable for the next integrated capture pass.
- Confirm whether D-1 / D-2 / D-3 have concrete definitions in Linux-side planning; this report inferred their likely impact areas from the pasted request.

## 5. Late Implementation Update

After graphics classified `20260510_130917` as `needs polish` with no blocker, implementation applied a CP-1 / CP-2 visual polish pass and generated:

- `docs/devlog/screenshots/chapter1_scene_integrated/20260510_135626/`
- `capture_manifest.json`: `errors=0`, `warnings=0`, `capture_count=18`, no fallback targets
- backlog seed: `docs/chapter1_graphics_visual_polish_backlog_20260510_135626.md`

Changes:

- CP-1: stronger Current/Past contrast for crop patches 1 / 4 / 5, three Current fallen nut piles vs one Past pre-state pile, east-third soil vs small west stain, murky vs clear well water, and larger / brighter trace post visuals.
- CP-2: reduced runtime path-light dapple patch scale, warm blend, and emission; no UI hint / no fade behavior is unchanged.

Validation:

- runtime validator: `Info=107`, `Warning=3`, `Error=0`, `PendingWiring=0`
- capture plan validation: `errors=0`, `warnings=3`, `viewpoints=9`, `sceneExists=True`
- `Zone1AudioSceneSetup.VerifyChapter1Scene`: success
- Full EditMode: `172 total / 150 passed / 0 failed / 22 skipped`
- Full PlayMode after chapter-transition review-fix hardening: `66/66` passed
  - XML: `<temp>\anemora_ch1_impl_playmode_after_reviewfix.xml`
- Runtime validator after review-fix hardening: `Info=107`, `Warning=3`, `Error=0`, `PendingWiring=0`
  - log: `<temp>\anemora_ch1_impl_runtime_validator_after_reviewfix.log`
- Windows build smoke after review-fix hardening: success
  - output: `<temp>\anemora_ch1_build_smoke_after_reviewfix\Anemora.exe`
  - log: `<temp>\anemora_ch1_impl_build_smoke_after_reviewfix.log`
  - result line: `Build Finished, Result: Success`

## 6. Latest Review-Fix 2 Update

After the previous review-fix hardening, a follow-up code review identified two remaining chapter-transition persistence risks: successful transition did not guarantee the live `ActionRecordRuntime` contained the BF1 pebble action record, and save failure could commit runtime progression before the save file was durable.

Implementation now treats save persistence as the commit point. `ChapterTransitionController` builds the save envelope without mutating runtime state, writes the save file first, then commits `chapter1_complete`, `chapter1.pebble.kicked`, and the live `chapter1_pebble_001` runtime action record. If save fails, fade/title/player state is restored and runtime completion state remains uncommitted.

Latest validation superseding the earlier review-fix counts:

- Targeted EditMode: `ChapterTransitionControllerTests;ActionRecordCatalogTests` `5/5` passed
  - XML: `<temp>\anemora_ch1_impl_reviewfix2_editmode_targeted.xml`
- Targeted PlayMode: `ChapterTransitionControllerPlayModeTests` `4/4` passed
  - XML: `<temp>\anemora_ch1_impl_reviewfix2_transition_playmode_failurecase.xml`
- Full EditMode: `173 total / 151 passed / 0 failed / 22 skipped`
  - XML: `<temp>\anemora_ch1_impl_editmode_after_reviewfix2.xml`
- Full PlayMode: `67/67` passed
  - XML: `<temp>\anemora_ch1_impl_playmode_after_reviewfix2_failurecase.xml`
- Runtime validator: `Info=107`, `Warning=3`, `Error=0`, `PendingWiring=0`
  - log: `<temp>\anemora_ch1_impl_runtime_validator_after_reviewfix2.log`
- Windows build smoke: success
  - output: `<temp>\anemora_ch1_build_smoke_after_reviewfix2\Anemora.exe`
  - log: `<temp>\anemora_ch1_impl_build_smoke_after_reviewfix2.log`

This is still not VS playable sign-off. Graphics verdict for `20260510_135626`, final in-editor timing/camera review, and subjective playthrough remain open.
