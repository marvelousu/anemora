# 2026-05-10 Chapter 1 Scene 4 Runtime Hooks

## Scope

Chapter 1 implementation scene wiring for scene 4 auto-trigger and trace reflection hooks.

This pass connects the runtime / portal systems delivered by the Runtime session to `Assets/Scenes/Anemora_Chapter1.unity`. It does not complete playable review, camera timing, final animation, or subjective pacing sign-off.

## Implemented

- Added `Assets/ScriptableObjects/Dialogues/Niro_BrushReaction.asset`.
- Added StringTable key `dialogue.niro.story_auto.brush_reaction`.
- Assigned `NiroMonologueController.storyAutoTriggerDialogue` on `TimeFramePortalSystem`.
- Added scene object `Chapter1_StoryAutoTrigger_S4D`.
  - `requestId = chapter1.scene4.s4d.auto_trigger`
  - `completedRawFlag = progression.chapter1.scene4.s4d_auto_triggered`
- Added scene object `Chapter1_StoryAutoTrigger_S4G`.
  - `requestId = chapter1.scene4.s4g.auto_trigger`
  - `completedRawFlag = progression.chapter1.scene4.s4g_auto_triggered`
- Added scene object `Past_SpiceJarPlaceholder` with `PastActionInteractable`.
  - `actionId = touch_spice_jar_001`
  - `targetObjectId = SpiceJar_Past_001`
  - `actionType = Touch`
  - `reflectImmediately = false`
  - `recordOnce = true`
- Added current-side trace object `Ch1_Current_S4F_SpiceTrace_AppearsLater`, initially inactive.
- Added scene object `Chapter1_GameObjectVisibilityReflector_S4F`.
  - `requiredSideEffect = RevealSpiceJarTrace`
  - `requiredActionId = touch_spice_jar_001`
- Added `touch_spice_jar_001` to `ActionRecordCatalog.asset`.
- Added the scene 4 visibility reflector to `ActionRecordRuntime.reflectorBehaviours` alongside `BookReflector`.
- Updated `AnemoraChapter1RuntimeSceneValidator` so implementation-owned visibility reflector instances satisfy the scene 4 wiring contract even when the template root is absent.

## Validation

- Unity compile/import smoke: pass.
  - Log: `<temp>\anemora_ch1_compile_after_validator_warning_patch.log`
- Runtime scene validator: pass.
  - Log: `<temp>\anemora_ch1_runtime_validator_after_scene4_hooks6.log`
  - Summary: `Info=13, Warning=0, Error=0, PendingWiring=0`
- Graphics static validation: pass.
  - Command: `python tools\verify_chapter1_graphics_integration_static.py --repo <worktree>`
- EditMode all-test sweep: partial pass with known graphics capture artifact failures.
  - Result XML: `<temp>\anemora_ch1_impl_editmode_scene4_hooks_abs.xml`
  - Summary: `139 total / 112 passed / 4 failed / 23 skipped`
  - The 4 failures are existing `Chapter1MapAssetTests` capture-file expectations for review PNGs that have not been generated yet.
  - Relevant suites passed: `Chapter1DialogueAssetTests` 7/7, `Chapter1RuntimeHookPrefabTemplateTests` 4/4, `Chapter1SceneStructureTests` 2/2, `ChapterTransitionControllerTests` 1/1.
- `git diff --check`: pass for touched implementation files before doc updates.

Unity import changed `Assets/AddressableAssetsData/link.xml`, its `.meta`, and `Assets/UI/Localization/Fonts/Anemora_EN_DistanceField.mat` during batchmode runs. These were restored as out-of-scope Unity import side effects.

## Remaining Work

- In-editor play verification for S4D / S4G trigger timing and window placement.
- Verify `Past_SpiceJarPlaceholder` interaction feel, range, and visibility with final camera framing.
- Confirm whether request IDs and completed flags remain acceptable draft implementation names.
- Capture review after graphics scene-integrated capture helper is available.
- Final playable sign-off remains user review; validator pass is not a playable completion declaration.
