# Chapter 1 Orchestration Current State

Date: 2026-05-10
Worktree: `<worktree>`
Branch: `codex/stage4-chapter1-implementation-20260510`

## Implementation State

Chapter 1 implementation is active in `Assets/Scenes/Anemora_Chapter1.unity`.

Implemented and scene-wired areas:

- Resident_A / Resident_B dialogue migration
- Chapter 1 scene scaffold and graphics aggregate placement
- Chapter 1 camera rig section switching controller
- Chapter 1 player boundary collision against route blocker colliders
- TimeFramePortalSystem v3.2 local-window / auto-trigger integration
- SymbolWheel first-loop Red-only contract
- Scene 1 book trace restoration
- Scene 4 S4D / S4G auto-trigger hooks
- Scene 4 CP-1 trace manifest / `TraceManifestReflector`
- Scene 2 CP-2 sightline reveal
- Zone1 Chapter 1 audio extension
- Scene 5 chapter transition scaffold

This is not playable sign-off. Final camera/collision/playthrough and user subjective review remain open.

## Latest Graphics Verdict

Current graphics review target and verdict:

- `docs/devlog/screenshots/chapter1_scene_integrated/20260510_130917/`
- capture mode: `both`
- character state: `placeholder`
- PNG count: `18`
- `capture_manifest.json`: `errors=0`, `warnings=0`, `capture_count=18`
- no fallback camera targets
- backlog seed: `docs/chapter1_graphics_visual_polish_backlog_20260510_130917.md`
- graphics verdict: `needs polish`
- CP-1 Kaia field verdict: `needs polish`
- CP-2 sightline / path-light verdict: `needs polish`
- global verdict: `needs polish`
- blocker: none
- graphics-owned blocker: none
- primary owner for remaining work: implementation scene assembly

This supersedes:

- `20260510_091343`: missing target / fallback capture blocker
- `20260510_093539`: target warnings fixed, but CP-1 Kaia field framing still blocker
- `20260510_102837`: Kaia field refocused, superseded by later CP-2 path-light polish
- `20260510_105109`: CP-2 path-light polish retained, superseded by tighter Kaia-field inspection framing

Implementation changes represented in `20260510_130917`:

- Kaia field refocused around `KaiaFieldSceneCenter = (10.4, 0, -10.8)`
- `Ch1_CaptureTarget_kaia_field` moved to `(11.7, 0.8, -11.35)` for the latest inspection capture
- `kaia_field` capture rotation changed to `(60, -45, 0)` with orthographic size `4.2`
- CP-2 path-light changed from broad cube planes to small collider-free dapple patches
- Graphics review confirms the previous CP-1 framing blocker is resolved. The trace objects are reviewable, but patch 1 / 4 / 5 state contrast, fallen nut pile count, east-third soil discoloration, murky well readability, and the left/top roof-wall weight still need polish.

Current implementation-owned next actions from graphics:

- preserve the `20260510_130917` CP-1 camera target / framing baseline
- polish CP-1 trace readability: patch 1 / 4 / 5, three fallen nut piles, east-third soil, and murky well
- reduce, fade, crop, or soften the left/top roof-wall only if it can be done without losing the field/traces framing
- polish CP-2 path-light so it reads as subtle, static, warm environmental light rather than broad translucent blockout geometry
- do not add UI hint or fade for CP-2

## Runtime State

Latest runtime validator on implementation scene:

- log: `<temp>\anemora_ch1_impl_runtime_validator_after_lowpass_and_progression_import.log`
- summary: `Info=107`, `Warning=3`, `Error=0`, `PendingWiring=0`
- category summary:
  - `RuntimeRoot=1`
  - `SymbolWheel=1`
  - `Scene1BookTrace=3`
  - `Scene4AutoTrigger=8`
  - `Scene4TraceReflector=4`
  - `ChapterTransition=2`
  - `CP1Scene4Trace=56`
  - `CP2SightlineReveal=30`
  - `PendingWiring=0`
  - `Error=0`

Accepted warnings:

- S4D `boundaryContext = Outdoor` vs template `Interior`
- S4G `boundaryContext = Outdoor` vs template `Ruin`
- S4G `windowSize = 4.40 x 3.40` vs draft `4.20 x 3.40`

Runtime hardening added in implementation:

- test-only manifest-path override for synthetic CP-1 missing-manifest tests
- CP-2 path-light validator checks for dapple count, collider-free visuals, and broad-plane regressions
- CP-1 fallen-nut target tokens tightened to avoid generic `East` matches
- scene structure contract checks for section trigger collider/anchor wiring, camera rig controller refs, route boundary colliders, player blocker settings, and 9 graphics capture target positions
- `PrototypePlayerController` has optional blocker-mask movement collision; Stage 3 compatibility remains off by default, and `Anemora_Chapter1.unity` enables it for the Chapter 1 player instance.
- `ChapterSectionTrigger` now fires on enter-edge only; `triggerOnce=false` allows re-entry without restarting camera transitions every frame while the player remains inside.
- `Scene2SightlineRevealController` now also fires on enter-edge only for re-enterable configurations.
- `Chapter1SceneLoadSmokeTests` loads the actual Chapter 1 scene in PlayMode and checks runtime camera rig / section trigger / player blocker contracts, including direct boundary rejection through the player controller and camera movement to each linked section anchor.
- `PlayerProgressionRuntimeTests` now cover Chapter 1 raw progression flags, unknown future flags, blue unlock, and duplicate normalization.
- `SaveEnvelopeRoundTripTests` now cover progress `rawFlags` round-trip and old-save readability without raw flags.
- `Zone1AudioSceneSetup` now serializes `musicLowPassFilter` on `Zone1_Audio`; runtime fallback remains available, but the scene reference is no longer null after the low-pass setup refresh.

Do not overwrite implementation-owned `ActionRecordRuntime.cs`; it owns multi-reflector dispatch/restoration.

## Validation

Latest implementation validation:

- Runtime validator: `Info=107`, `Warning=3`, `Error=0`, `PendingWiring=0`
  - latest log: `<temp>\anemora_ch1_impl_runtime_validator_after_lowpass_and_progression_import.log`
- Capture plan validation after the `20260510_130917` target shift: `errors=0`, `warnings=3`, `viewpoints=9`, `sceneExists=True`
  - log: `<temp>\anemora_ch1_impl_capture_plan_validate_after_kaia_target_130917.log`
  - warnings are static-scan graphics root name warnings; the actual `20260510_130917` capture manifest has `warnings=0` and no fallback targets.
- `Chapter1SceneStructureTests`: `6/6` passed after updating the Kaia capture target contract to `(11.7, 0.8, -11.35)`.
  - XML: `<temp>\anemora_ch1_impl_scene_structure_tests_after_130917.xml`
- `Chapter1SceneCapturePlanTests`: `6/6` passed.
  - XML: `<temp>\anemora_ch1_impl_scene_capture_plan_tests_after_130917.xml`
- `Chapter1RuntimeSceneValidatorTests`: `14/14` passed
  - XML: `<temp>\anemora_ch1_impl_runtime_scene_validator_tests_after_cp1_token_tightening.xml`
- `Zone1AudioSceneSetup.VerifyChapter1Scene`: success after low-pass serialization refresh
  - log: `<temp>\anemora_ch1_impl_zone1_audio_verify_after_lowpass_serialized.log`
- `Zone1AudioWiringTests`: `4/4` passed after low-pass serialization refresh
  - XML: `<temp>\anemora_ch1_impl_zone1_audio_wiring_after_lowpass_serialized.xml`
- Full EditMode: `172 total / 150 passed / 0 failed / 22 skipped`
  - XML: `<temp>\anemora_ch1_impl_editmode_after_runtime_ae_ah_lowpass.xml`
  - `Chapter1DialogueAssetTests`: `7/7` passed
  - `Chapter1SceneStructureTests`: `6/6` passed
  - skips are expected review-capture / pass7-pass8 manifest gates
- Full PlayMode: `63/63` passed
  - XML: `<temp>\anemora_ch1_impl_playmode_after_scene1_book_trace.xml`
  - `Chapter1SceneLoadSmokeTests`: `2/2` passed
  - `ChapterCameraRigControllerTests`: `1/1` passed
  - `PrototypePlayerControllerCollisionTests`: `2/2` passed
  - `ChapterTransitionControllerPlayModeTests`: `1/1` passed
  - `Scene2SightlineRevealControllerTests`: `1/1` passed
  - `Chapter1Scene4TraceRuntimeTests`: `1/1` passed
  - `Chapter1Scene4AutoTriggerRuntimeTests`: `1/1` passed
  - `Chapter1Scene1BookTraceRuntimeTests`: `1/1` passed
  - `SymbolWheelProgressionTests`: `4/4` passed
  - `Zone1AudioWiringTests`: `4/4` passed
- Graphics static validator: pass
  - command: `python tools\verify_chapter1_graphics_integration_static.py --repo <worktree>`
- Windows build smoke after sightline enter-edge hardening: success
  - output: `<temp>\anemora_ch1_build_smoke_after_sightline_reentry\Anemora.exe`
  - log: `<temp>\anemora_ch1_impl_build_smoke_after_sightline_reentry.log`
  - result: `Build Finished, Result: Success`
  - caveat: CodeCoverage package `System.Numerics` resolution noise appears in the log
- `git diff --check`: pass after restoring Unity batchmode side effects

## Active External Prompts

Graphics session prompt:

- `20260510_130917` review is complete: overall / CP-1 / CP-2 are `needs polish`, blocker none
- latest prompt asks graphics to review the implementation-owned polish recapture `20260510_135626`
- graphics should not edit implementation worktree

Runtime session prompt:

- Phase AE-AH was partially imported into implementation: progression/save tests and low-pass validator note were kept, implementation-owned stricter validator behavior was preserved
- next prompt asks runtime to finalize transfer guard / do-not-overwrite lists and run final runtime package validation
- keep implementation-owned runtime scripts read-only, especially `ActionRecordRuntime.cs`, `TraceManifestReflector.cs`, `Zone1AudioController.cs`, and `ChapterTransitionController.cs`

Character session:

- still blocked by PixelLab zero balance
- final sprites are not integrated
- current captures use placeholder character state

## Remaining Implementation Work

- Await graphics verdict for `20260510_135626`; implementation-owned CP-1 trace readability and CP-2 path-light polish has been applied and recaptured.
- Continue any further visual polish only after that verdict.
- Continue playable flow validation in-editor when possible.
- Camera-composition and hands-on collision feel review remain open.
- Character final sprite integration remains external.
- Commit / push / PR / staging remain pending Tom explicit instruction.

## Latest Visual Polish Recapture

Implementation applied the `20260510_130917` graphics verdict follow-up and generated a new capture package:

- `docs/devlog/screenshots/chapter1_scene_integrated/20260510_135626/`
- capture mode: `both`
- character state: `placeholder`
- PNG count: `18`
- `capture_manifest.json`: `errors=0`, `warnings=0`, `capture_count=18`
- no fallback camera targets
- backlog seed: `docs/chapter1_graphics_visual_polish_backlog_20260510_135626.md`

What changed:

- CP-1 Kaia field: patch 1 / 4 / 5 state contrast strengthened, three fallen nut piles added to Current, one pre-state pile added to Past, east-third soil / west stain comparison added, murky / clear well water comparison added, and trace post visuals enlarged / brightened.
- CP-1 capture helper: `kaia_field` orthographic size tightened from `4.2` to `3.95` while preserving the target position.
- CP-2 path-light: dapple patch scale, material warm blend, and emission were reduced to push it away from blockout-plane readability.

Validation after this recapture:

- runtime validator: `Info=107`, `Warning=3`, `Error=0`, `PendingWiring=0`
  - log: `<temp>\anemora_ch1_impl_runtime_validator_after_cp1_cp2_visual_polish.log`
- capture plan validation: `errors=0`, `warnings=3`, `viewpoints=9`, `sceneExists=True`
  - log: `<temp>\anemora_ch1_impl_capture_plan_validate_after_cp1_cp2_visual_polish.log`
- `Zone1AudioSceneSetup.VerifyChapter1Scene`: success
  - log: `<temp>\anemora_ch1_impl_zone1_audio_verify_after_cp1_cp2_visual_polish.log`
- Full EditMode: `172 total / 150 passed / 0 failed / 22 skipped`
  - XML: `<temp>\anemora_ch1_impl_editmode_after_cp1_cp2_visual_polish.xml`
- Targeted `Chapter1SceneLoadSmokeTests` after route occupiable coverage: `3/3` passed
  - XML: `<temp>\anemora_ch1_impl_scene_load_smoke_after_route_occupiable.xml`
- Targeted `ChapterTransitionControllerPlayModeTests`: `2/2` passed after adding trigger-polling scene-wired transition coverage
  - XML: `<temp>\anemora_ch1_impl_chapter_transition_trigger_polling.xml`
- Full PlayMode: `65/65` passed
  - XML: `<temp>\anemora_ch1_impl_playmode_after_transition_polling.xml`
- runtime validator after chapter-transition trigger-polling fallback: `Info=107`, `Warning=3`, `Error=0`, `PendingWiring=0`
  - log: `<temp>\anemora_ch1_impl_runtime_validator_after_transition_polling.log`
- Windows build smoke: success
  - output: `<temp>\anemora_ch1_build_smoke_after_transition_polling\Anemora.exe`
  - log: `<temp>\anemora_ch1_impl_build_smoke_after_transition_polling.log`
  - result line: `Build Finished, Result: Success`

## Latest Review-Fix Validation

Code review of the scene-wired chapter transition found same-scene fallback and BF1 persistence gaps. Implementation fixed them and reran validation.

Runtime fixes:

- `ChapterTransitionController` restores fade/title UI and the previous player controller state when `loadNextChapterScene = false`.
- The intermediate save now includes `chapter1_pebble_001` as a reflected `ActionRecordEntry` targeting `chapter2_scene4`, in addition to the raw flags.
- Trigger polling now uses collider overlap and avoids latching inside-state while required progression flags are missing.
- Route smoke coverage now verifies intended section-center occupancy only.

Validation:

- Targeted `ChapterTransitionControllerTests`: `1/1` passed
  - XML: `<temp>\anemora_ch1_impl_chapter_transition_reviewfix_editmode.xml`
- Targeted `ChapterTransitionControllerPlayModeTests`: `3/3` passed
  - XML: `<temp>\anemora_ch1_impl_chapter_transition_reviewfix_playmode_retry.xml`
- Full EditMode: `172 total / 150 passed / 0 failed / 22 skipped`
  - XML: `<temp>\anemora_ch1_impl_editmode_after_reviewfix.xml`
- Full PlayMode: `66/66` passed
  - XML: `<temp>\anemora_ch1_impl_playmode_after_reviewfix.xml`
- Runtime validator: `Info=107`, `Warning=3`, `Error=0`, `PendingWiring=0`
  - log: `<temp>\anemora_ch1_impl_runtime_validator_after_reviewfix.log`
- Windows build smoke: success
  - output: `<temp>\anemora_ch1_build_smoke_after_reviewfix\Anemora.exe`
  - log: `<temp>\anemora_ch1_impl_build_smoke_after_reviewfix.log`
  - result line: `Build Finished, Result: Success`

## Latest Review-Fix 2 Validation

Follow-up review hardened the chapter transition save semantics beyond the previous review-fix pass:

- BF1 pebble persistence now updates the live `ActionRecordRuntime` after successful save, not only the serialized envelope.
- Completion/progression flags are committed only after the save write succeeds.
- Save failure restores same-scene state and leaves completion flags / pebble action record uncommitted.
- Production `ActionRecordCatalog.asset` has direct test coverage for `chapter1_pebble_001 / Push / BF1PebbleKickSeed / chapter2_scene4`.

Current latest validation:

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
  - result line: `Build Finished, Result: Success`

## Latest Visible Review Artifacts

The earlier `anemora_ch1_build_smoke_after_reviewfix2\Anemora.exe` was a build-smoke output using the normal `EditorBuildSettings` order. Because `Anemora_Main.unity` is still first there, that executable starts from the Stage 3 reference scene and is not suitable for visual Chapter 1 VS review.

For visible review, a separate Chapter 1-first player was generated by temporarily placing `Anemora_Chapter1.unity` first for the build and restoring the project setting afterward:

- Chapter 1 review EXE: `<temp>\anemora_ch1_review_player_chapter1_first_20260510_152530\Anemora_Chapter1.exe`
- build log: `<temp>\anemora_ch1_review_player_chapter1_first_20260510_152530.log`
- result line: `Build Finished, Result: Success`

Fresh scene-integrated capture was generated after reviewfix2:

- capture folder: `docs/devlog/screenshots/chapter1_scene_integrated/20260510_152530/`
- contact sheet: `docs/devlog/screenshots/chapter1_scene_integrated/20260510_152530/contact_sheet_20260510_152530.png`
- manifest: `errors=0`, `warnings=0`, `capture_count=18`, no fallback camera targets

This is the current visible review package. It is not a VS playable sign-off: final character sprites, graphics verdict on the latest capture, hands-on camera/collision/playthrough review, and subjective pacing review remain open.

## Latest Chapter 1 Review Player - 20260511_000102

After the CP-2 path-light graphics pass and B-specific detail capture pass2, a new Chapter 1-first review player was generated:

- Chapter 1 review EXE: `<temp>\anemora_ch1_review_player_cp2_bdetail_20260511_000102\Anemora_Chapter1.exe`
- build log: `<temp>\anemora_ch1_review_build_cp2_bdetail_20260511_000102.log`
- result line: `Chapter1 review build result: Succeeded`
- build summary: `warnings=0`, `errors=0`

This build uses `Assets/Scenes/Anemora_Chapter1.unity` as the only build scene, so it is the current executable to use for Chapter 1 visual/playable spot checks. It still uses placeholder character state.

Latest visual/capture status around this build:

- CP-2 path-light package `20260510_205725`: graphics verdict `pass`
- B-specific detail package `20260511_000102`: routed to graphics for B-5/B-2/B-7/B-8 review
- B-specific contact sheet: `docs/devlog/screenshots/chapter1_b_specific_details/20260511_000102/ch1_b_detail_20260511_000102_contact_sheet.png`

## Current B-Detail / B-3 Evidence - 20260511_001409

Graphics reviewed B-specific detail package `20260511_000102`:

- Overall B-detail: `needs polish`
- B-2 Past: `pass` for local evidence
- B-5 Current/Past, B-2 Current, B-7 Past library, B-8 Past Kaia field: `needs polish`
- blocker: none
- graphics source issue: no
- source pass requested: no

Implementation generated a superseding B-3 enhanced timed evidence package:

- package: `docs/devlog/screenshots/chapter1_side_view_cinematic/20260511_001409`
- manifest: `errors=0`, `warnings=0`, `capture_count=9`
- contact sheet: `ch1_side_view_20260511_001409_b3_enhanced_phase_contact_sheet.png`
- added evidence frames: `monologue_hold`, `pre_kick_contact`, `kick_contact`, `post_kick`, `fade_title_save_handoff`
- character state: `placeholder`
- validation: `Chapter1SceneCapturePlanTests 6/6 passed`

Graphics reviewed `20260511_001409`:

- B-3 enhanced timed visual: `pass`
- scope: placeholder-stage timed closeout evidence
- all required phase frames pass, including monologue hold, kick contact sequence, and fade/title/save handoff
- blocker: none
- graphics source issue: none
- source pass requested: no
- reopen only after final side-view sprite replacement or B-3 camera/framing/lighting/fade-title-source changes

Next main-owned work:

- Polish B-5/B-2 Current/B-7/B-8 detail readability without requesting graphics source changes.

## Latest B-Detail Polish / Review Player - 20260511_002510

Implementation completed a B-detail polish pass after graphics marked `20260511_000102` as needs polish. This supersedes `20260511_000102` for B-5/B-2/B-7/B-8 visual review.

Active B-detail package:

- capture folder: `docs/devlog/screenshots/chapter1_b_specific_details/20260511_002510`
- contact sheet: `docs/devlog/screenshots/chapter1_b_specific_details/20260511_002510/ch1_b_detail_20260511_002510_contact_sheet.png`
- B-7 marker map: `docs/devlog/screenshots/chapter1_b_specific_details/20260511_002510/b7_marker_map_20260511_002510.md`
- manifest: `errors=0`, `warnings=0`, `planned_viewpoints=6`, `capture_count=6`
- character state: `placeholder`

Polish focus:

- B-5 Current empty frame / object trace readability
- B-5 Past object-count contrast
- B-2 Current value separation
- B-7 countable detail evidence markers
- B-8 Past field value separation

Validation:

- runtime validator: `Info=215`, `Warning=3`, `Error=0`, `PendingWiring=0`
  - log: `<temp>\anemora_ch1_impl_runtime_validator_bdetail_polish.log`
- capture helper EditMode: `Chapter1SceneCapturePlanTests 6/6 passed`
  - XML: `<temp>\anemora_ch1_impl_bdetail_polish_capture_helper_editmode.xml`
- B-detail capture: `errors=0`, `warnings=0`, `captures=6`
  - log: `<temp>\anemora_ch1_impl_bdetail_polish2_capture.log`

Latest Chapter 1-first review player:

- Chapter 1 review EXE: `<temp>\anemora_ch1_review_player_bdetail_polish_20260511_002510\Anemora_Chapter1.exe`
- build log: `<temp>\anemora_ch1_review_build_bdetail_polish_20260511_002510.log`
- result line: `Chapter1 review build result: Succeeded`
- build summary: `warnings=0`, `errors=0`

Route `20260511_002510` to graphics for B-detail review. Keep character state caveat: no final character import has been approved.
