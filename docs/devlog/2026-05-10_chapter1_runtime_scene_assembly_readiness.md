# 2026-05-10 Chapter 1 Runtime Scene Assembly Readiness

## Scope

Added validation and template assets so the Chapter 1 implementation session can wire runtime hooks into `Anemora_Chapter1.unity` without touching production scene assembly in this session.

## Added

### Scene Assembly Validator

Editor tool:

- `Assets/Editor/AnemoraChapter1RuntimeSceneValidator.cs`

Menu:

- `Anemora/Runtime/Validate Chapter1 Runtime Hooks`

Batchmode method:

- `AnemoraChapter1RuntimeSceneValidator.ValidateChapter1RuntimeHooksBatch`

The validator checks `Assets/Scenes/Anemora_Chapter1.unity` if it exists. If not, it reports a warning and exits successfully.

Severity levels:

- `Info`: optional or not-yet-assembled context
- `Warning`: missing hook or draft wiring that can be fixed during scene assembly
- `Error`: present hook has invalid serialized data or a runtime contract is broken

### Runtime Hook Prefabs

Created under `Assets/Prefabs/Runtime/Chapter1/`:

- `Chapter1_RuntimeRoot.prefab`
- `Chapter1_StoryAutoTrigger_S4D.prefab`
- `Chapter1_StoryAutoTrigger_S4G.prefab`
- `Chapter1_GameObjectVisibilityReflector_Template.prefab`

The prefabs avoid Dialogue / Save / UI / production scene references. Draft request ids and completed flags are serialized for scene 4 auto-trigger templates.

### Runtime Hardening

- `ProgressFlagSaveData` now has generic `HasRawFlag()` / `SetRawFlag()` helpers.
- `PlayerProgressionRuntime.LoadFromSaveData()` deduplicates lists and preserves unknown / future raw flags.
- `TimeFramePortalController.RequestAutoTrigger()` skips once-only requests when `completedRawFlag` is already set.
- `TimeFrameStoryAutoTrigger` mirrors the completed-flag skip before requesting the controller.
- `PastActionInteractable` defaults to one-shot recording.
- `NiroMonologueController.TryShowStoryAutoTriggerReaction()` is batchmode-safe while preserving the "attempt show" contract.

## Validator Coverage

- `PlayerProgressionRuntime` presence
- `SymbolWheelController` Red selectable / White hidden / Blue locked preview serialized contract
- `TimeFramePortalController` local-window v3.2 serialized fields and Stage 3 manual symbol fallback method
- `BookReflector` current-side object activation or prefab fallback
- `PastBookInteractable.actionId == take_book_001` report
- `TimeFrameStoryAutoTrigger` request id / completed flag / window size / boundary context
- Post-placement S4D/S4G exact draft ids, completed flags, once-only behavior, monologue reaction flag, and expected boundary contexts
- `GameObjectVisibilityReflector` object naming pattern, non-empty entries, and null target check
- Actual `Chapter1_GameObjectVisibilityReflector*` instances satisfy scene 4 trace placement even when the template root is absent.
- `PastActionInteractable.reflectImmediately == false`, `recordOnce == true`, and local-window `requirePastSide == false` Info note.
- `ActionRecordCatalog.touch_spice_jar_001 / Touch / RevealSpiceJarTrace`
- `Niro_BrushReaction.asset` key `dialogue.niro.story_auto.brush_reaction`
- `ChapterTransitionController` root, completion flag, save ids, title UI references, and pebble target reference
- Category summary counts for RuntimeRoot / SymbolWheel / Scene1BookTrace / Scene4AutoTrigger / Scene4TraceReflector / ChapterTransition / PendingWiring / Error
- Layer 9 past NPC interaction exclusion contract

## Verification

- Unity batchmode compile/import smoke: success
- Validator batchmode: success, reports scene missing warning because `Assets/Scenes/Anemora_Chapter1.unity` is not present in this worktree (`Info=0`, `Warning=1`, `Error=0`, `PendingWiring=0`)
- Validator category summary in runtime worktree: `RuntimeRoot=0`, `SymbolWheel=0`, `Scene1BookTrace=0`, `Scene4AutoTrigger=0`, `Scene4TraceReflector=0`, `ChapterTransition=0`, `CP1Scene4Trace=0`, `CP2SightlineReveal=0`, `PendingWiring=0`, `Error=0`
- EditMode tests: `61/61` passed
- PlayMode tests: `50/50` passed
- PlayMode log includes the existing TMP Essential Resources warning after Test Runner reports `Exiting with code 0 (Ok)`.
- `git diff --check`: passed

Logs:

- `Logs/codex_phase_xz_compile_smoke.log`
- `Logs/codex_phase_xz_scene_validator.log`
- `Logs/codex_phase_xz_editmode_results.xml`
- `Logs/codex_phase_xz_playmode_results.xml`

## Scene Assembly Expected Pass Criteria

After `Assets/Scenes/Anemora_Chapter1.unity` is assembled:

- Validator has no `Error` entries.
- Any remaining `Warning` entries are explicitly accepted in scene assembly handover.
- S4D / S4G story auto-trigger objects have real position / size tuning.
- S4D / S4G exact request IDs / completed flags are intact; temporary `Outdoor` boundary context remains warning-level only.
- `BookReflector.reflectedBookObject` or `bookPrefab` is wired.
- `GameObjectVisibilityReflector.visibilityEntries` have no null targets.
- `Past_SpiceJarPlaceholder` records `touch_spice_jar_001` once and reflects on window exit/close.
- `ActionRecordCatalog` contains `touch_spice_jar_001 / Touch / RevealSpiceJarTrace`.
- `NiroMonologueController.storyAutoTriggerDialogue` points to `Niro_BrushReaction.asset`, and the asset references `dialogue.niro.story_auto.brush_reaction`.
- `ChapterTransitionController.completedRawFlag` remains `chapter1_complete`.
- `SymbolWheelController` first-loop serialized state remains Red-only with Blue preview locked.

## Implementation Read-Only Scan

Read-only YAML/meta scan against `<worktree>` found:

- `Anemora_Chapter1.unity` exists.
- `Chapter1_RuntimeRoot` with `PlayerProgressionRuntime` exists.
- `TimeFramePortalSystem` owns `TimeFramePortalController`, `BookReflector`, and `NiroMonologueController`.
- `Past_BookPlaceholder` has `PastBookInteractable.actionId = take_book_001`.
- `BookReflector.reflectedBookObject` and `bookPrefab` are assigned.
- `SymbolWheel` prefab instance exists and is referenced by the portal controller.
- S4D / S4G `TimeFrameStoryAutoTrigger` scene objects are now observed.
- `Chapter1_GameObjectVisibilityReflector_S4F`, `Past_SpiceJarPlaceholder`, and `Ch1_Current_S4F_SpiceTrace_AppearsLater` are now observed.
- `NiroMonologueController.storyAutoTriggerDialogue` is now assigned to `Niro_BrushReaction.asset`.
- `ActionRecordCatalog` contains `touch_spice_jar_001 / Touch / RevealSpiceJarTrace`.
- `Niro_BrushReaction.asset` and shared/en/ja string table rows for `dialogue.niro.story_auto.brush_reaction` are observed.
- `Chapter1_TransitionSequence` exists and `ChapterTransitionController.completedRawFlag` is `chapter1_complete`.

Transfer documents:

- `docs/draft/chapter1_runtime_scene_wiring_contract_2026-05-10.md`
- `docs/draft/chapter1_runtime_transfer_manifest_2026-05-10.md`
- `docs/draft/chapter1_runtime_impl_scene_scan_2026-05-10.md`

## Phase A-D Follow-Up

- Validator now reports `PendingWiring` count in the summary.
- Multiple `PlayerProgressionRuntime` instances are treated as `Error`.
- `NiroMonologueController.storyAutoTriggerDialogue` missing is reported as pending dialogue wiring.
- S4D / S4G `TimeFrameStoryAutoTrigger` missing is reported independently as pending implementation wiring.
- `GameObjectVisibilityReflector` missing is reported as pending implementation wiring.
- Placed story triggers validate `portalController`, `requestId`, `completedRawFlag`, `boundaryContext`, and positive `windowSize`.
- Placed `PastActionInteractable` objects validate `reflectImmediately == false` and source-side `requirePastSide`.
- `BookReflector.reflectedBookObject` reports whether the current-side target starts inactive.
- Scene 4 hook template pack details were added to the wiring contract and transfer manifest.
- EditMode prefab template tests cover runtime root, S4D/S4G trigger drafts, and visibility reflector placeholder entry.

## Phase E-H Follow-Up

- Validator now checks post-placement S4D/S4G contracts:
  - `Chapter1_StoryAutoTrigger_S4D`
  - `chapter1.scene4.s4d.auto_trigger`
  - `progression.chapter1.scene4.s4d_auto_triggered`
  - `Interior`
  - `Chapter1_StoryAutoTrigger_S4G`
  - `chapter1.scene4.s4g.auto_trigger`
  - `progression.chapter1.scene4.s4g_auto_triggered`
  - `Ruin`
- Placed S4D/S4G triggers with wrong request id / completed flag now report `Error`.
- Null `portalController` on placed story triggers remains `Warning` because runtime fallback exists, but implementation should wire the reference explicitly.
- `GameObjectVisibilityReflector` now reports a naming-pattern warning and treats empty placed entries as `Error`.
- Prefab template tests now verify root names, `.meta` / asset GUID presence, non-zero script GUID references, S4D/S4G boundary contexts, exact draft window sizes, scene-independent null references, and visibility placeholder safety.
- Later implementation read-only scans observe scene-placed S4D/S4G triggers and `Chapter1_GameObjectVisibilityReflector_S4F`; runtime template prefabs remain transfer templates rather than scene roots.

## Phase I-N Follow-Up

- Validator accepts implementation-owned `Chapter1_GameObjectVisibilityReflector_S4F` as satisfying scene 4 trace placement even when `Chapter1_GameObjectVisibilityReflector_Template` is absent from the scene.
- S4D/S4G `boundaryContext = Outdoor` is warning-level accepted temporary placement, while request id / completed flag / positive size / `onceOnly` / `showNiroReaction` remain strong checks.
- `PastActionInteractable.requirePastSide = false` is now Info/local-window mode, and `recordOnce = true` is validated.
- `ActionRecordCatalog.touch_spice_jar_001` is validated as `Touch` with `RevealSpiceJarTrace` when the scene uses that action.
- `Niro_BrushReaction.asset` is statically checked for `dialogue.niro.story_auto.brush_reaction`.
- `ChapterTransitionController` validation covers `Chapter1_TransitionSequence`, `chapter1_complete`, title/save references, and pebble target reference.
- EditMode validator tests now cover actual reflector instance without template root, local-window `requirePastSide = false`, catalog validation, and chapter transition contract.
- PlayMode tests now cover spice trace `GameObjectVisibilityReflector` action/effect matching and story auto-trigger completed-flag skip.

## Phase O-S Follow-Up

- Ingested CP-1 Scene 4 trace placement specifics from origin/main handover docs and notes: Kaia field is mixed layout, not a 7-10 tree orchard; CP-1 expects central big nut tree, east small nut tree, crop patches 1-6, `touch_spice_jar_001`, `RevealSpiceJarTrace`, and `Scene4_T4_TraceManifest.asset`.
- Ingested CP-2 Scene 2 [2.F] sightline reveal specifics: no UI hint, no fade transition, `Zone1_Ambient` continues, three east-positioned wind/bird/footstep cues, sun/path Niro monologue cues, static/subtle path light initially hidden.
- Validator now has `CP1Scene4Trace` and `CP2SightlineReveal` categories in `CategorySummary`.
- CP-1 validator checks are reflection-safe: `TraceManifestReflector` and `Chapter1TraceManifest` are inspected by type/name and serialized property names without compile dependency on implementation-only classes.
- CP-1 accepts the legacy `Chapter1_GameObjectVisibilityReflector_S4F` as a non-pending reflector path, but still reports missing `Scene4_T4_TraceManifest.asset` as pending until multi-trace placement lands.
- CP-1 placed reflectors now validate `requiredActionId = touch_spice_jar_001`, `requiredSideEffect = RevealSpiceJarTrace`, non-empty bindings/entries, and no null targets.
- CP-2 validator looks for `Chapter1_Scene2_SightlineReveal` or equivalent controller, profile/dialogue/audio/path-light references, once-only fields, UI-hint/fade dependencies, `Zone1_Ambient` continuity, and initially hidden path-light root.
- CP-2 audio checks can also inspect `Zone1AudioController` for `PlayScene2SightlineReveal(Vector3,float)` and three serialized sightline cue clips when the implementation-side audio extension is present.
- Implementation read-only scan now observes CP-1 manifest/reflector scripts and CP-2 sightline scripts/dialogue/audio assets, while CP-1 manifest asset / scene reflector and CP-2 scene root remain pending static scan items.
- EditMode validator tests were extended for CP-1 manifest pending classification, manifest+target non-pending behavior, and CP-2 sightline controller null-safe / assigned-reference validation.

## Phase U-W Follow-Up

- Read-only implementation scan now observes CP-1 production placement:
  - `Scene4_T4_TraceManifest.asset` with 10 trace entries
  - `Chapter1_Scene4_TraceVisuals`
  - `Chapter1_TraceManifestReflector_S4F`
  - 10 non-null `traceBindings`
  - legacy `Chapter1_GameObjectVisibilityReflector_S4F` still present as compatibility
- CP-1 validator hardening:
  - expected trace ids are checked for EastSmallTree / CentralBigTree / CropPatch01 / CropPatch04 / CropPatch05 / FallenNutPile / SoilDiscoloration / WellWater
  - legacy fallback remains accepted during transfer
  - missing `Scene4_T4_TraceManifest.asset` becomes `Error` when production-complete trace scene objects are present
- Read-only implementation scan now observes CP-2 production placement:
  - `Chapter1_Scene2_SightlineReveal`
  - `Scene2_SightlineRevealProfile.asset`
  - `Chapter1_Scene2_PathLight` initially inactive
  - sun/path Niro monologue assets
  - Zone1AudioController sightline route and audio fields; Phase AA-AD static refresh shows the accessible scene YAML still has one audio controller block with sightline source/clip refs null, so implementation should revalidate current assigned refs after import
- CP-2 validator hardening:
  - profile timing/intensity values are checked with tolerances
  - Zone1AudioController checks all three sightline source refs and clip refs
  - no UI hint / no fade route remains warning-level if serialized fields appear enabled
- S4D/S4G warnings now explicitly state they are implementation placement deltas, not blocker errors, unless story/design finalizes exact contexts/dimensions.
- EditMode validator tests were extended for production-complete CP-1 manifest reflector behavior, CP-1 missing-manifest promotion to error, CP-2 profile tolerance checks, and CP-2 Zone1AudioController source/clip references.

## Phase X-Z Follow-Up

- Transfer collision scan observed same-path implementation files for the runtime validator, validator tests, runtime hook prefabs, transfer docs, implementation `ActionRecordRuntime.cs`, and implementation `TraceManifestReflector.cs`. These are compare-before-copy collisions, not overwrite targets.
- Graphics recapture package `docs/devlog/screenshots/chapter1_scene_integrated/20260510_093539/` is present in the implementation worktree. `capture_manifest.json` reports `errors=0`, `warnings=0`, and `capture_count=18`; runtime does not judge PNG visual quality.
- Post-integration validator expected output is now documented as `Error=0`, `PendingWiring=0`, with warnings limited to the three accepted S4D/S4G placement deltas unless implementation handover explicitly accepts more.
- CP-1 validator now checks manifest active-state contracts for expected trace ids: missing expected ids are errors in production-complete mode, `postActive=false` is an error, `preActive=true` is a warning, and placed trace binding targets are expected to start inactive.
- CP-1 validator reports legacy `Chapter1_GameObjectVisibilityReflector_S4F` coexisting with production `TraceManifestReflector` as compatibility info.
- CP-2 validator now reports the absence of enabled UI hint / fade dependencies as an explicit info entry and scans for `Zone1_Ambient.ogg` when sightline reveal wiring is present.
- Chapter transition lookup remains reflection-safe and now accepts controller type/name matches if the implementation class is unavailable to the runtime worktree.
- EditMode validator tests were extended for CP-1 missing expected trace id, CP-1 active-state/compatibility reporting, CP-2 explicit once-only error, and UI/fade warnings.

## Phase AA-AD Follow-Up

- Transfer manifest now separates runtime import paths into `copy/update recommended`, `compare-before-copy required`, and `do-not-overwrite implementation-owned`.
- `Assets/Scripts/TimeManagement/ActionRecordRuntime.cs` is explicitly do-not-overwrite: implementation owns the multi-reflector dispatch/restoration version required for scene 1 book reflection plus scene 4 trace reflection.
- Post-import expected validator output is restated as `Error=0`, `PendingWiring=0`, and at most three accepted S4D/S4G temporary placement warnings.
- CP-1 validator now treats the design-level canonical trace ids as required while accepting current implementation aliases:
  - canonical: `crop_patch_01`, `crop_patch_04`, `crop_patch_05`, `fallen_nuts_central_big`, `fallen_nuts_east_small`, `well_water_murky`
  - accepted aliases: `crop_patch_01_west`, `crop_patch_04_central`, `crop_patch_05_east`, `fallen_nuts_central`, `fallen_nuts_east`, `well_murky_water`
- Chapter transition validation now also checks `autoWalkTarget`, `kickAnimationClip`, `progressionRuntime`, `chapterTitleJa`, `chapterTitleEn`, and `nextChapterSceneName` when those serialized fields exist.
- #11 audio readiness is documented for low-pass path, `Zone1_Ambient.ogg` continuity, and chapter SFX refs. Actual mix and subjective audio QA remain implementation/playtest owned.
- #15 transition readiness is documented for Niro auto-walk, stone kick, fade/title, save, next chapter hook, and `chapter1_complete` persistence.
- Implementation read-only scan refresh observed #11 script fields and audio assets, but the accessible scene YAML still shows one `Zone1AudioController` serialization with low-pass / chapter SFX / sightline source+clip refs as `{fileID: 0}`. Keep this as a post-import audio validation item; no implementation files were changed.
- Implementation read-only scan confirms `Chapter1_TransitionSequence` still has `chapter1_complete`, auto-walk target, kick clip, pebble, fade/title refs, save ids, and `nextChapterSceneName = Anemora_Chapter2`.
- Runtime validation after AA-AD:
  - compile/import smoke: success (`Logs/codex_phase_aa_ad_compile_smoke.log`)
  - validator batch: success with graceful missing-scene warning only (`Info=0`, `Warning=1`, `Error=0`, `PendingWiring=0`; `Logs/codex_phase_aa_ad_scene_validator.log`)
  - EditMode tests: `62/62` passed (`Logs/codex_phase_aa_ad_editmode_results.xml`)
  - PlayMode tests: `50/50` passed (`Logs/codex_phase_aa_ad_playmode_results.xml`)
  - PlayMode log still includes the existing TMP Essential Resources warning after Test Runner reports `Exiting with code 0 (Ok)`.
  - `git diff --check`: passed after restoring Unity batchmode side effects.

## Phase AE-AH Follow-Up

- Import anticipation docs now include the implementation-side latest reference point:
  - active capture package `docs/devlog/screenshots/chapter1_scene_integrated/20260510_130917/`
  - capture manifest `errors=0`, `warnings=0`, `capture_count=18`, no fallback targets
  - implementation validator `Info=106`, `Warning=3`, `Error=0`, `PendingWiring=0`
  - category summary `CP1Scene4Trace=56`, `CP2SightlineReveal=29`
  - implementation-reported Full EditMode `168 total / 146 passed / 0 failed / 22 skipped`, Full PlayMode `63/63 passed`, build smoke success
- `ActionRecordRuntime.cs` do-not-overwrite warning is repeated in the transfer manifest: implementation owns the multi-reflector dispatch/restoration version.
- Zone1 audio validation policy is clarified:
  - static YAML `{fileID: 0}` observations are advisory handover notes
  - actual opened-scene validator component refs take precedence
  - a static YAML warning should become actionable only when the loaded-scene validator also reports `Zone1AudioController` source/clip refs unassigned
- Runtime validator now emits an Info note on `Zone1AudioController` checks explaining that it uses loaded-scene component references rather than raw YAML.
- Save/progression regression guard was extended:
  - `PlayerProgressionRuntime` now has an EditMode test for blue unlock, S4D/S4G completed flags, `chapter1_complete`, pebble helper flag, future raw flag preservation, and duplicate normalization.
  - `SaveEnvelope` now has an EditMode round-trip test for `progressFlags.rawFlags` carrying blue unlock, S4D/S4G completed, `chapter1_complete`, pebble helper, and future raw flags.
- Runtime validation after AE-AH:
  - compile/import smoke: success (`Logs/codex_phase_ae_ah_compile_smoke.log`)
  - validator batch: success with graceful missing-scene warning only (`Info=0`, `Warning=1`, `Error=0`, `PendingWiring=0`; `Logs/codex_phase_ae_ah_scene_validator.log`)
  - EditMode tests: first invocation exited without XML after test start, second invocation passed `64/64` (`Logs/codex_phase_ae_ah_editmode_results.xml`)
  - PlayMode tests: `50/50` passed (`Logs/codex_phase_ae_ah_playmode_results.xml`)
  - PlayMode log still includes the existing TMP Essential Resources warning after Test Runner reports `Exiting with code 0 (Ok)`.
  - Unity batchmode side effects on Addressables/link.xml and ProjectSettings were restored.

## Phase AM-AP Follow-Up

- Runtime side intentionally restarted from the latest implementation state instead of carrying forward the stale AE-AH baseline.
- Read-only implementation scan now observes the active capture package `docs/devlog/screenshots/chapter1_scene_integrated/20260510_135626/`.
- Capture manifest summary for `20260510_135626`: `errors=0`, `warnings=0`, `planned_viewpoints=9`, `capture_count=18`, fallback target count `0`.
- Latest implementation validator log observed: `<temp>\anemora_ch1_impl_runtime_validator_after_reviewfix2.log`.
- That validator reports `Info=107`, `Warning=3`, `Error=0`, `PendingWiring=0`; category summary includes `CP1Scene4Trace=56`, `CP2SightlineReveal=30`, and `ChapterTransition=2`.
- The three warnings remain only the accepted S4D/S4G temporary placement deltas: S4D `Outdoor`, S4G `Outdoor`, and S4G tuned `windowSize = 4.40 x 3.40`.
- Reviewfix2 implementation tests observed:
  - targeted EditMode `ChapterTransitionControllerTests;ActionRecordCatalogTests`: `5/5` passed
  - targeted PlayMode `ChapterTransitionControllerPlayModeTests`: `3/3` passed
  - Full EditMode: `173 total / 151 passed / 0 failed / 22 skipped`
  - Full PlayMode: `66/66` passed
- Implementation `ActionRecordCatalog.asset` now contains `chapter1_pebble_001 / Push / BF1PebbleKickSeed / futureRecoverySceneId=chapter2_scene4`.
- Implementation scene YAML now serializes `ChapterTransitionController.pebbleRecordId = chapter1_pebble_001`, `pebbleFutureRecoverySceneId = chapter2_scene4`, `nextChapterSceneName = Anemora_Chapter2`, and `loadNextChapterScene = 0`.
- Runtime validator was hardened to check the chapter transition pebble seed catalog entry, pebble record id, future recovery scene id, and `loadNextChapterScene` accepted state without compile-time dependency on implementation-only classes.
- Runtime EditMode validator tests were extended for:
  - `ActionRecordCatalog` chapter pebble seed positive contract
  - missing `futureRecoverySceneId` negative contract
  - `ChapterTransitionController.loadNextChapterScene=false` Info/accepted contract
- Transfer manifest now repeats that implementation-owned `ActionRecordRuntime.cs` and `ChapterTransitionController.cs` must not be overwritten.
- Reviewfix2 build smoke log was not found in the expected temp path during this read-only scan; keep it as pending implementation validation unless implementation supplies a newer build log.
- Runtime AM-AP validation:
  - compile/import smoke: success (`Logs/codex_phase_am_ap_compile_smoke.log`)
  - validator batch: success with runtime scene-missing warning only (`Info=0`, `Warning=1`, `Error=0`, `PendingWiring=0`; `Logs/codex_phase_am_ap_scene_validator.log`)
  - EditMode tests: first run caught one stale assertion string, fixed; rerun passed `66/66` (`Logs/codex_phase_am_ap_editmode_results.xml`)
  - PlayMode tests: `50/50` passed (`Logs/codex_phase_am_ap_playmode_results.xml`)
  - PlayMode log still includes the existing TMP Essential Resources warning after `Exiting with code 0 (Ok)`.
  - Unity batchmode side effects on Addressables/link.xml, link.xml.meta, ProjectSettings.asset, and untracked SceneTemplateSettings.json were restored.
  - `git diff --check`: passed.

## Phase A-D Dialogue / Visual Specifics Readiness

- Fetched origin/main and read the latest handoffs:
  - `docs/draft/chapter1_s1_s2_handover_2026-05-08.md` v1.8 / v1.9 (`838a059`, `fc39fb8`)
  - notes `_handover/anemora-chapter1-a-dialogue-details-2026-05-10.md`
  - notes `_handover/anemora-chapter1-b-visual-specifics-2026-05-10.md`
- Added runtime `DialogueProximityTrigger` for Scene 3 [3.C] overheard market playback.
  - supports `DialogueAsset[]`, `AudioClip[]`, random/sequential playback, cooldown, one-shot, loop-while-inside
  - defaults to no DialogueDisplay panel so overheard dialogue does not freeze the player
  - keeps past NPCs non-reactive through `pastNpcOverheardOnly=true`
- Validator now has new categories:
  - `Scene3Dialogue`
  - `Scene4Dialogue`
  - `BVisualSpecifics`
- Scene 3 [3.C] validator contracts:
  - Dario monologue
  - Dario customer pairs
  - Kairo three-part song
  - Luna calls/laughter
  - market ambient route
- Scene 3 [3.D] / Scene 4 validator contracts now recognize the implementation-observed DialogueAsset names and key families.
- B visual specifics validator coverage:
  - B-7 separates `PastBookInteractable` / `take_book_001` / `BookReflector` runtime flow from full past-library dressing
  - B-8 anchors healthy past field readiness on `Scene4_T4_TraceManifest.asset` / `TraceManifestReflector`
  - B-5 proposes Niro home-past trigger ids and monologue keys without touching blue unlock progression
  - B-3 checks side-view camera and 5s / 10s phase durations reflection-safely when implementation exposes serialized fields
- Implementation read-only scan observations:
  - capture package `20260510_155643` exists with manifest `errors=0`, `warnings=0`, `capture_count=18`
  - Scene 3 / Scene 4 dialogue assets are present
  - `DialogueProximityTrigger` is not yet scene-placed
  - B-5 home-past triggers are not yet scene-placed
  - B-3 side-view fields are not yet visible on the current transition controller serialization
  - `Zone1AudioController` sightline wind/bird/footstep source and clip refs are assigned in scene YAML
- Runtime Phase A-D validation:
  - compile/import smoke: success (`Logs/codex_phase_a_d_compile_smoke.log`)
  - validator batch: success with graceful missing-scene warning only (`Info=0`, `Warning=1`, `Error=0`, `PendingWiring=0`; `Logs/codex_phase_a_d_scene_validator.log`)
  - EditMode tests: first run caught one overly broad test assertion, fixed; rerun passed `68/68` (`Logs/codex_phase_a_d_editmode_results.xml`)
  - PlayMode tests: `52/52` passed (`Logs/codex_phase_a_d_playmode_results.xml`)
  - Unity batchmode side effects on `Assets/AddressableAssetsData/link.xml`, `.meta`, `ProjectSettings.asset`, and generated `SceneTemplateSettings.json` were restored.

## Phase E-H Dialogue Source Compatibility

- Implementation read-only scan now observes placed Scene 3 dialogue source markers under `Chapter1_DialogueSources_Past`.
- Observed source objects:
  - `Chapter1_S3C_Dario_Monologue_Source`
  - `Chapter1_S3C_Dario_CustomerPair1_Source`
  - `Chapter1_S3C_Dario_CustomerPair2_Source`
  - `Chapter1_S3C_Kairo_Song_Source`
  - `Chapter1_S3C_Luna_Calls_Source`
  - `Chapter1_S3D_AriaHouse_Lesson_Source`
- Implementation source markers use a legacy single `dialogueAsset` field, `triggerRadius`, `playOnce`, and disabled base-scene triggers.
- Runtime `DialogueProximityTrigger` now preserves that legacy contract while retaining richer canonical fields (`DialogueAsset[]`, `AudioClip[]`, random/sequential, cooldown, one-shot, loop).
- Runtime `.meta` GUID for `DialogueProximityTrigger.cs` is aligned with implementation's scene-referenced GUID `28b06f9828b3aa647ab0cf80fe0a6be8`.
- `TimeWindowDiorama` now includes `Anemora.Dialogue.DialogueProximityTrigger` in local-window gated interactables so disabled source clones can be enabled only when Niro is inside the window.
- Validator now treats disabled known Scene 3 source markers as Info/accepted and accepts known source object names when `contractId` is absent.
- Tests added/updated for:
  - disabled implementation source markers accepted by validator
  - legacy single `dialogueAsset` / `triggerRadius` compatibility
  - `TimeWindowDiorama` gating disabled dialogue source clones by player-inside state
- Runtime Phase E-H validation:
  - compile/import smoke: success (`Logs/codex_phase_e_h_compile_smoke.log`)
  - validator batch: success with graceful missing-scene warning only (`Info=0`, `Warning=1`, `Error=0`, `PendingWiring=0`; `Logs/codex_phase_e_h_scene_validator.log`)
  - EditMode tests: `69/69` passed (`Logs/codex_phase_e_h_editmode_results.xml`)
  - PlayMode tests: `54/54` passed (`Logs/codex_phase_e_h_playmode_results.xml`)
  - Unity batchmode side effects on `Assets/AddressableAssetsData/link.xml`, `.meta`, `ProjectSettings.asset`, and generated `SceneTemplateSettings.json` were restored.

## Dialogue / B Visual Contract Hardening

- Implementation read-only rescan shows Scene 3 source markers serialize `contractId` as their source object names, for example `Chapter1_S3C_Dario_Monologue_Source`.
- Runtime validator now accepts canonical `chapter1.scene3.*` ids, absent ids carried by known object names, and source-name `contractId` values. Source-name ids are Info compatibility, not Error.
- Validator now checks `DialogueProximityTrigger.cs.meta` GUID `28b06f9828b3aa647ab0cf80fe0a6be8` so implementation source marker script references are protected.
- B visual specifics root coverage was added for:
  - `Chapter1_B5_NiroHouse_CurrentForeshadow`
  - `Chapter1_B5_NiroHouse_PastForeshadow`
  - `Chapter1_B2_RuinHouse_CurrentInterior`
  - `Chapter1_B2_RuinHouse_PastInterior`
  - `Chapter1_Cutscene_S5_SideView`
  - `Chapter1_B7_PastLibrary_Dressing`
  - `Chapter1_B8_PastKaiaField_Dressing`
- Earlier implementation static scan missed `Chapter1_Cutscene_S5_SideView`; latest B visual kit SceneSetup scan now observes all seven roots, including the side-view cutscene root.
- Runtime keeps missing B-root validation as advisory readiness metadata in synthetic/runtime-worktree validation. Visual-only Ch1_B* prefab children remain implementation/graphics-owned and are not runtime blockers unless a future runtime component takes a serialized reference.
- Runtime validation:
  - compile/import smoke: success (`Logs/codex_dialogue_b_visual_followup_compile_smoke.log`)
  - validator batch: first immediate run hit a transient same-project Unity lock after compile smoke; retry succeeded with graceful runtime missing-scene warning only (`Info=0`, `Warning=1`, `Error=0`, `PendingWiring=0`; `Logs/codex_dialogue_b_visual_followup_scene_validator_retry.log`)
  - EditMode tests: `71/71` passed (`Logs/codex_dialogue_b_visual_followup_editmode_results.xml`)
  - PlayMode tests: `54/54` passed (`Logs/codex_dialogue_b_visual_followup_playmode_results.xml`)
  - `git diff --check`: passed
  - Unity batchmode side effects on Addressables/link.xml, ProjectSettings, and generated SceneTemplateSettings were restored.

## B Visual Kit Observed Follow-Up

- Implementation main session reports B visual kit prefabs copied, SceneSetup patched/executed, and all expected B roots instantiated.
- Runtime read-only scan observes:
  - `Chapter1_B5_NiroHouse_CurrentForeshadow` / `Ch1_B5_NiroHouse_CurrentTraceHook`
  - `Chapter1_B5_NiroHouse_PastForeshadow` / `Ch1_B5_NiroHouse_PastFamilyTraceKit`
  - `Chapter1_B2_RuinHouse_CurrentInterior` / `Ch1_B2_EnterableHouse_CurrentInterior`
  - `Chapter1_B2_RuinHouse_PastInterior` / `Ch1_B2_EnterableHouse_PastInterior`
  - `Chapter1_Cutscene_S5_SideView` / `Ch1_B3_SideViewCinematic_Background` / `Ch1_B3_SideViewCinematic_ForegroundAnchors`
  - `Chapter1_B7_PastLibrary_Dressing` / `Ch1_B7_PastLibrary_DetailMarkers`
  - `Chapter1_B8_PastKaiaField_Dressing` / `Ch1_B8_PastKaiaField_DetailMarkers`
- Implementation validator after B kit placement: `Info=107`, `Warning=3`, `Error=0`, `PendingWiring=0`; remaining warnings are accepted S4D/S4G placement deltas.
- Latest scene-integrated capture package `20260510_191543` has manifest `errors=0`, `warnings=0`, `capture_count=18`.
- Dedicated B-3 side-view capture package `20260510_192215` has manifest `errors=0`, `warnings=0`, `capture_count=1`.
- Added EditMode regression guard that missing `Chapter1_Cutscene_S5_SideView` remains an advisory `BVisualSpecifics` warning, not an Error, in runtime-worktree synthetic scenes.

## Phase I-L Runtime Readiness

- Validator now has a separate `Zone1Audio` category for #11 readiness.
- #11 checks are reflection/name safe and use loaded-scene component references:
  - `Zone1_Ambient.ogg` continuity
  - `musicLowPassFilter` and low-pass numeric configuration
  - `timeBrushReactClip`, `ruinDustClip`, `ruinWindClip`, `chapterCloseClip`, `nutsFallClip`
  - CP-2 sightline wind/bird/footstep sources and clips
  - route methods `SetChapterFiveLowPassActive`, `SetChapterFiveLowPassBlend`, `PlayTimeBrushReact`, `PlayRuinDust`, `PlayRuinWind`, `PlayChapterClose`, `PlayNutsFall`, `PlayScene2SightlineReveal`
- #15 checks were tightened reflection-safely:
  - `nextChapterSceneName` must be `Anemora_Chapter2`
  - `PebbleKickedFlag` is checked when exposed and expected to equal `chapter1.pebble.kicked`
  - Chapter transition reports coexistence with `Chapter1_Cutscene_S5_SideView` when present
- `ChapterTransitionController.cs` and `Zone1AudioController.cs` remain implementation-owned and were not edited.
- Runtime validation:
  - compile/import smoke: success (`Logs/codex_phase_i_l_compile_smoke.log`)
  - validator batch: success with runtime scene-missing warning only (`Info=0`, `Warning=1`, `Error=0`, `PendingWiring=0`; `Logs/codex_phase_i_l_scene_validator.log`)
  - EditMode: first run caught a fixture setup issue in the new Zone1Audio test; fixed and reran `73/73` passed (`Logs/codex_phase_i_l_editmode_results_fixed.xml`)
  - PlayMode: `54/54` passed (`Logs/codex_phase_i_l_playmode_results.xml`)
  - `git diff --check`: passed
  - Unity batchmode side effects on Addressables/link.xml, ProjectSettings, and generated SceneTemplateSettings were restored.

## Phase M-P B-3 Side-View Support Follow-Up

- Implementation read-only scan updated the B-3 side-view baseline:
  - `20260510_192215` remains recorded as the graphics-reviewed needs-polish side-view capture.
  - `20260510_193428` is now the current implementation baseline after side-view camera anchor, key/fill lighting, and scene-instance material/color readability overrides. Manifest reports `errors=0`, `warnings=0`, `capture_count=1`.
  - Latest review build path exists at `<temp>\anemora_ch1_review_player_b3_20260510_193428\Anemora_Chapter1.exe`; main reports build success with warnings=0/errors=0.
- Validator now recognizes B-3 support objects by name without implementation-owned class references:
  - `B3_SideView_CameraAnchor`
  - `B3_SideView_Cinematic_KeyLight`
  - `B3_SideView_Twilight_FillLight`
- Missing support objects are warning-level scene-composition follow-up, not runtime flow blockers. Missing `Chapter1_Cutscene_S5_SideView` remains advisory in runtime-worktree synthetic validation and should be present in production implementation after SceneSetup.
- Ownership remains split:
  - visual source quality: graphics-owned
  - scene/capture composition and B-3 support objects: implementation-owned
  - 5s turn-back / 10s pan / monologue / auto-walk / stone kick / fade-title-save runtime flow: `ChapterTransitionController` contract, with implementation source still do-not-overwrite
- Added EditMode regression guard for B-3 support object recognition under `Chapter1_Cutscene_S5_SideView`.
- Runtime validation:
  - compile/import smoke: success (`Logs/codex_phase_m_p_compile_smoke.log`)
  - validator batch: first immediate run hit transient same-project Unity lock; retry succeeded with runtime scene-missing warning only (`Info=0`, `Warning=1`, `Error=0`, `PendingWiring=0`; `Logs/codex_phase_m_p_scene_validator_retry.log`)
  - EditMode tests: `74/74` passed (`Logs/codex_phase_m_p_editmode_results.xml`)
  - PlayMode tests: `54/54` passed (`Logs/codex_phase_m_p_playmode_results.xml`)
  - `git diff --check`: passed
  - notes handover diff check: passed
  - Unity batchmode side effects on Addressables/link.xml, ProjectSettings, and generated SceneTemplateSettings were restored.
