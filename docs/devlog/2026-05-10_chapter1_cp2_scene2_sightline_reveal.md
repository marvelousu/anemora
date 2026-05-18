# 2026-05-10 Chapter 1 CP-2 Scene 2 Sightline Reveal

## Scope

Implemented the CP-2 Scene 2 [2.F] sightline reveal design from the 2026-05-10 story / spec handoff.

The implementation adds a reusable scene trigger, data profile, lighting transition hook, subtle path-light object, positional audio cues, and Niro monologue timing. It keeps the transition to Scene 3 seamless: no UI hint and no fade.

## Implemented

- Added `Assets/Scripts/Chapter/Chapter1SightlineRevealProfile.cs`.
  - Stores light transition values, audio fade timing, path-light delay, and monologue timing.
- Added `Assets/Scripts/Chapter/Scene2SightlineRevealController.cs`.
  - Triggered by a player collider.
  - Runs the 4000K early morning to 5500K morning light transition.
  - Starts 3D east-positioned sightline audio cues through `Zone1AudioController`.
  - Activates `Chapter1_Scene2_PathLight`.
  - Shows the two Niro monologue assets at the configured offsets.
- Added `Assets/ScriptableObjects/Chapter1/Scene2_SightlineRevealProfile.asset`.
- Added dialogue assets:
  - `Assets/ScriptableObjects/Dialogues/Niro_Scene2_Sightline_Sun.asset`
  - `Assets/ScriptableObjects/Dialogues/Niro_Scene2_Sightline_Path.asset`
- Added StringTable keys:
  - `dialogue.niro.scene2.sightline.sun`
  - `dialogue.niro.scene2.sightline.path`
- Added provisional SFX clip aliases under `Assets/Audio/SFX/Zone1/chapter1/`:
  - `zone1_env_wind_breeze.ogg`
  - `zone1_env_bird_chirp.ogg`
  - `zone1_env_footstep_distant.ogg`
- Extended `Assets/Scripts/Audio/Zone1AudioController.cs`.
  - Added sightline cue sources and clips.
  - Added `PlayScene2SightlineReveal(Vector3 worldPosition, float fadeDuration)`.
- Updated `Assets/Editor/Zone1AudioSceneSetup.cs`.
  - Wires the sightline cue sources / clips on `Zone1_Audio`.
  - Wires the scene controller's `audioController` reference.
- Updated `Assets/Scripts/TimeManagement/NiroMonologueController.cs`.
  - Added a generic `TryShowMonologue(ScriptableObject dialogueAsset)` route used by the sightline controller.

## Design Coverage

- Scene 1 early lighting baseline is represented in the Chapter 1 scene setup:
  - approximate 4000K color
  - intensity `0.7`
  - sun angle `30`
- Scene 2 [2.F] target values are stored in `Scene2_SightlineRevealProfile.asset`:
  - approximate 5500K color
  - intensity `1.0`
  - sun angle `60`
  - 8 second transition duration
- Audio:
  - three positional cue clips are assigned.
  - the controller uses subtle fade-in via `Zone1AudioController`.
- Path light:
  - `Chapter1_Scene2_PathLight` starts inactive and is enabled by `Chapter1_Scene2_SightlineReveal`.
  - `Scene2SightlineRevealController` now fires on enter-edge only. If configured as re-enterable, it will not restart reveal work every frame while the player remains inside the volume.
- Narrative timing:
  - sun monologue at `0.5s`
  - path monologue at `3.0s`
- UI / transition:
  - no tutorial UI hint is introduced.
  - no fade is introduced.
  - `Zone1_Ambient.ogg` continuity is preserved.

## Scene Wiring

- Scene object: `Chapter1_Scene2_SightlineReveal`
- Scene object: `Chapter1_Scene2_PathLight`
- `Scene2SightlineRevealController.profile` points to `Scene2_SightlineRevealProfile.asset`.
- `Scene2SightlineRevealController.audioController` points to `Zone1_Audio`.
- `Scene2SightlineRevealController.sunMonologueDialogue` points to `Niro_Scene2_Sightline_Sun.asset`.
- `Scene2SightlineRevealController.pathMonologueDialogue` points to `Niro_Scene2_Sightline_Path.asset`.
- `Zone1AudioController` has sightline wind, bird, and distant footstep sources and clips assigned.

## Validation

- Unity compile/import smoke: pass.
  - Log: `<temp>\anemora_ch1_impl_final_import_smoke.log`
- Runtime scene validator: pass.
  - Log: `<temp>\anemora_ch1_impl_final_runtime_validator.log`
  - Summary: `Info=51, Warning=3, Error=0, PendingWiring=0`
  - Category summary includes `CP2SightlineReveal=15`.
  - No CP-2 sightline audio or object warning remains.
- Capture plan validation: pass.
  - Log: `<temp>\anemora_ch1_impl_final_capture_plan_validate.log`
  - Summary: `errors=0, warnings=12, viewpoints=9, sceneExists=True`
- Actual scene capture generated:
  - `docs/devlog/screenshots/chapter1_scene_integrated/20260510_091343/`
  - 18 PNG files, `capture_manifest.json`, and `capture_report.md`
  - `Capture mode: both`
  - `Character sprite state: placeholder`
- Capture backlog seed generated:
  - `docs/chapter1_graphics_visual_polish_backlog_20260510_091343.md`

## 2026-05-10 Capture Follow-Up

Graphics review of capture `20260510_093539` classified CP-2 as `needs polish`, not blocker. The central plaza / street corner views became reviewable after capture target fixes, and the no-UI / no-fade direction remains intact.

The visual issue from `20260510_093539` was path-light polish: the path-light read as broad translucent planes / blockout geometry rather than a subtle, static, warm environmental cue.

After CP-1 Kaia-field refocusing and CP-2 path-light polish, a new both-mode capture package was generated:

- `docs/devlog/screenshots/chapter1_scene_integrated/20260510_105109/`
- 18 PNG files, `capture_manifest.json`, and `capture_report.md`
- `capture_manifest.json`: `errors=0`, `warnings=0`, `capture_count=18`, no fallback targets
- backlog seed: `docs/chapter1_graphics_visual_polish_backlog_20260510_105109.md`

Path-light implementation changed from 5 broad cube planes to 10 small collider-free dapple patches named `PathLight_Dapple_EastSightline_*`.

After a further Kaia-field capture-target shift, the active graphics-review package is now:

- `docs/devlog/screenshots/chapter1_scene_integrated/20260510_130917/`
- 18 PNG files, `capture_manifest.json`, and `capture_report.md`
- `capture_manifest.json`: `errors=0`, `warnings=0`, `capture_count=18`, no fallback targets
- backlog seed: `docs/chapter1_graphics_visual_polish_backlog_20260510_130917.md`

CP-2 code and path-light visuals are unchanged from the `20260510_105109` polish pass; `20260510_130917` supersedes it only because it contains the latest CP-1 Kaia capture framing.

Graphics review of `20260510_130917` classified CP-2 as `needs polish`, not blocker. Plaza east to street-corner guidance, Current/Past readability, placeholder silhouettes, no-UI direction, and no-fade direction are acceptable as a direction. The remaining issue is implementation-owned visual polish: the path-light still reads too much like broad translucent / blockout geometry instead of subtle, static, warm environmental light.

CP-2 next action:

- keep the no-UI and no-fade route unchanged
- polish the path-light material / shape / value so it blends into the environment as warm dappled light
- do not turn the path-light into a tutorial marker or animated guide line

Latest runtime validation after low-pass serialization and progression/save test import:

- `<temp>\anemora_ch1_impl_runtime_validator_after_lowpass_and_progression_import.log`
- `Info=107`, `Warning=3`, `Error=0`, `PendingWiring=0`
- `CP2SightlineReveal=30`
- the remaining warnings are accepted S4D/S4G temporary placement deltas
- validator now confirms the CP-2 path-light root has 10 dapple patches, no colliders, and no broad blockout-plane scale patches

Capture plan validation after the `20260510_130917` target shift:

- `<temp>\anemora_ch1_impl_capture_plan_validate_after_kaia_target_130917.log`
- `errors=0`, `warnings=3`, `viewpoints=9`, `sceneExists=True`
- warnings are static-scan graphics root name warnings; the actual `20260510_130917` capture manifest has `warnings=0` and no fallback targets.

Full implementation test pass after path-light contract hardening, sightline enter-edge/audio route hardening, scene-load boundary smoke, player axis-slide collision coverage, section trigger enter-edge hardening, chapter transition PlayMode smoke, Scene 1 book trace runtime coverage, Scene 4 trace reflection runtime coverage, Scene 4 auto-trigger runtime coverage, and SymbolWheel scene progression coverage:

- EditMode: `172 total / 150 passed / 0 failed / 22 skipped`
  - XML: `<temp>\anemora_ch1_impl_editmode_after_runtime_ae_ah_lowpass.xml`
  - skips are existing capture/manifest-not-yet-generated gates
- PlayMode: `63/63` passed
  - XML: `<temp>\anemora_ch1_impl_playmode_after_scene1_book_trace.xml`
  - includes `Chapter1SceneLoadSmokeTests`, which loads `Anemora_Chapter1` and verifies camera rig, section trigger anchors, camera movement to each linked anchor, player blocker settings, and boundary rejection
  - includes `PrototypePlayerControllerCollisionTests` axis-slide coverage for diagonal movement blocked on one axis
  - includes `ChapterTransitionControllerPlayModeTests`, which runs `BeginTransition()` to `Complete` and verifies the intermediate save / pebble side effects
  - includes `Scene2SightlineRevealControllerTests`, which verifies a re-enterable sightline trigger does not refire every frame while the player stays inside
  - includes `Zone1AudioWiringTests`, which verifies Chapter 1 sightline 3D sources, cue clips, low-pass filter, and batchmode route behavior
  - includes `Chapter1Scene4TraceRuntimeTests`, which verifies Scene 4 spice action reflection reaches all trace targets
  - includes `Chapter1Scene4AutoTriggerRuntimeTests`, which verifies S4D/S4G story auto-trigger runtime behavior
  - includes `Chapter1Scene1BookTraceRuntimeTests`, which verifies Scene 1 book trace runtime behavior
  - includes `SymbolWheelProgressionTests`, which verifies the scene-wired first-loop symbol contract and blue unlock transition

## Remaining Work

- Further polish the path-light because graphics review of `20260510_130917` still classifies it as blockout-like / `needs polish`.
- In-editor listening pass for the 3D positional cues.
- Playable timing review for the plaza arrival trigger and Scene 3 seamless entry.
- Final lighting polish remains implementation / graphics review dependent.

## 2026-05-10 Visual Polish Recapture

Implementation applied the `20260510_130917` CP-2 graphics verdict follow-up and generated a new review package:

- `docs/devlog/screenshots/chapter1_scene_integrated/20260510_135626/`
- 18 PNG files, `capture_manifest.json`, and `capture_report.md`
- `capture_manifest.json`: `errors=0`, `warnings=0`, `capture_count=18`, no fallback targets
- backlog seed: `docs/chapter1_graphics_visual_polish_backlog_20260510_135626.md`

CP-2 changes in this pass:

- kept no UI hint and no fade behavior unchanged
- reduced dapple patch scale again so the runtime path-light is less broad and less blockout-like when activated
- reduced `Scene2_SightlineRevealProfile.pathEmissionIntensity` from `0.18` to `0.12`
- reduced path-light material warm blend and emission multiplier

Validation after this pass:

- runtime validator: `Info=107`, `Warning=3`, `Error=0`, `PendingWiring=0`
  - log: `<temp>\anemora_ch1_impl_runtime_validator_after_cp1_cp2_visual_polish.log`
- capture plan validation: `errors=0`, `warnings=3`, `viewpoints=9`, `sceneExists=True`
  - log: `<temp>\anemora_ch1_impl_capture_plan_validate_after_cp1_cp2_visual_polish.log`
- `Zone1AudioSceneSetup.VerifyChapter1Scene`: success
  - log: `<temp>\anemora_ch1_impl_zone1_audio_verify_after_cp1_cp2_visual_polish.log`
- Full EditMode: `172 total / 150 passed / 0 failed / 22 skipped`
  - XML: `<temp>\anemora_ch1_impl_editmode_after_cp1_cp2_visual_polish.xml`
- Full PlayMode: `65/65` passed
  - XML: `<temp>\anemora_ch1_impl_playmode_after_transition_polling.xml`
- Windows build smoke: success
  - output: `<temp>\anemora_ch1_build_smoke_after_transition_polling\Anemora.exe`
  - log: `<temp>\anemora_ch1_impl_build_smoke_after_transition_polling.log`

Later review-fix validation supersedes the trigger-polling totals:

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

## 2026-05-10 Path-Light Miniaturization Capture - 20260510_205725

Implementation applied the later CP-2 graphics polish brief that asked for a less broad / less planar path-light while preserving the no-UI and no-fade direction.

Changes in this pass:

- kept the Scene 2 [2.F] no-UI hint and no-fade transition unchanged
- changed `PathLight_Dapple_EastSightline_*` patches from cylinder primitives to flatter cube light cards
- reduced each dapple patch footprint again
- reduced the path-light material warm blend and emission multiplier so the cue reads more like environmental dappled light

New targeted capture package:

- `docs/devlog/screenshots/chapter1_scene_integrated/20260510_205725/`
- `capture_manifest.json`: `errors=0`, `warnings=0`, `capture_count=9`, no fallback targets
- primary review PNGs:
  - `ch1_scene_20260510_205725_current_central_plaza.png`
  - `ch1_scene_20260510_205725_current_street_corner.png`
- capture mode: `current`
- character sprite state: placeholder

Validation after this pass:

- runtime validator: `Info=215`, `Warning=3`, `Error=0`, `PendingWiring=0`
  - log: `<temp>\anemora_ch1_impl_cp2_pathlight_validator.log`
- targeted `Chapter1RuntimeSceneValidatorTests`: `24/24` passed
  - XML: `<temp>\anemora_ch1_impl_cp2_pathlight_editmode_validator_rerun.xml`
- `DialogueProximityTriggerTests`: `7/7` passed after the non-freezing DialogueDisplay route fix
  - XML: `<temp>\anemora_ch1_impl_dialogue_proximity_playmode3.xml`

Graphics review is pending for `20260510_205725`. The intended acceptance question is whether the central plaza / street corner sightline now reads as subtle, static, warm environmental guidance rather than broad translucent debug geometry.

Graphics review result for `20260510_205725`:

- CP-2 path-light overall: `pass`
- central_plaza Current: `pass`
- street_corner Current: `pass`
- blocker: none
- graphics source issue: none
- no source prefab / material / shader / atlas pass requested

Accepted visual finding:

- plaza east to street-corner sightline is readable without UI hint or fade replacement
- path-light reads as subtle, static, warm environmental dapple rather than broad translucent debug / blockout geometry
- placeholder silhouettes and path floor remain readable

CP-2 is now regression-watch only after final lighting, final sprites, camera/exposure, or path-light placement/material changes.
