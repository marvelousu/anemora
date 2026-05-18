# 2026-05-10 Chapter 1 CP-1 Scene 4 Trace Manifest

## Scope

Implemented the CP-1 Scene 4 [4.F] multi-object trace placement design from the 2026-05-10 story / spec handoff.

This pass keeps the existing Scene 4 spice jar action hook and adds a data-driven trace manifest so the Kaia field visual changes can be reflected as a single effect after the time window closes. It does not claim playable completion or final visual polish.

## Implemented

- Added `Assets/Scripts/Chapter/Chapter1TraceManifest.cs`.
  - Defines `Chapter1TraceManifest`, `TraceEntry`, and `Chapter1TraceCategory`.
  - Stores pre/post active state and color tint per trace object.
- Added `Assets/Scripts/Chapter/Chapter1TraceMarker.cs`.
  - Gives generated trace visual objects stable trace ids.
- Added `Assets/Scripts/TimeManagement/Reflectors/TraceManifestReflector.cs`.
  - Implements `IReflector` and `IReflectedStateRestorer`.
  - Applies the manifest only when the matching `ActionRecord` side effect is observed.
- Updated `Assets/Scripts/TimeManagement/ActionRecordRuntime.cs`.
  - Dispatches one action record to every matching reflector instead of stopping at the first handled reflector.
  - This allows the legacy `GameObjectVisibilityReflector` and the new manifest reflector to coexist for `touch_spice_jar_001`.
- Added `Assets/ScriptableObjects/Chapter1/Scene4_T4_TraceManifest.asset`.
- Updated `Assets/Editor/AnemoraChapter1SceneSetup.cs`.
  - Builds the mixed Kaia field layout: central large nut tree, east small nut tree, and six crop patches.
  - Creates `Chapter1_Scene4_TraceVisuals`.
  - Creates `Chapter1_TraceManifestReflector_S4F`.

## Trace Design Coverage

The manifest wires the CP-1 design as implementation-owned scene objects:

- East small nut tree: withering to dead.
- Central large nut tree: healthy to newly withering.
- Crop patch 1 west: withering to dead.
- Crop patch 4 central and patch 5 east: healthy to newly withering.
- Fallen nut piles: one to three locations.
- Soil discoloration: expands toward the east side of the field.
- Well water: clear to murky.

The map direction follows the handoff: entry from southwest / street corner and exit toward the east / ruin foreshadow direction.

## Scene Wiring

- `Past_SpiceJarPlaceholder` records `touch_spice_jar_001`.
- `touch_spice_jar_001` is present in `ActionRecordCatalog.asset` with:
  - `ActionType.Touch`
  - `SideEffectType.RevealSpiceJarTrace`
- `Chapter1_TraceManifestReflector_S4F` listens for the same action id and side effect.
- The existing `Chapter1_GameObjectVisibilityReflector_S4F` remains wired as a simple compatibility trace hook.

## Validation

- Unity compile/import smoke: pass.
  - Log: `<temp>\anemora_ch1_impl_final_import_smoke.log`
- Runtime scene validator: pass.
  - Log: `<temp>\anemora_ch1_impl_final_runtime_validator.log`
  - Summary: `Info=51, Warning=3, Error=0, PendingWiring=0`
  - Category summary includes `CP1Scene4Trace=15`.
  - The remaining warnings are accepted S4D/S4G placement deltas, not CP-1 trace wiring failures.
- Graphics static validation: pass.
  - Command: `python tools\verify_chapter1_graphics_integration_static.py --repo <worktree>`
- Capture readiness scan: pass.
  - `Chapter1_Scene4_TraceVisuals`, `Chapter1_TraceManifestReflector_S4F`, and `Scene4_T4_TraceManifest.asset` observed.
- Actual scene capture generated:
  - `docs/devlog/screenshots/chapter1_scene_integrated/20260510_091343/`
  - 18 PNG files, `capture_manifest.json`, and `capture_report.md`
  - `Capture mode: both`
  - `Character sprite state: placeholder`

## 2026-05-10 Recapture Follow-Up

Graphics review of capture `20260510_093539` cleared the missing-target blocker but still classified CP-1 as blocker because the `kaia_field` view showed adjacent building/roof geometry instead of the field traces.

Implementation adjusted the CP-1 scene layout and inspection capture target:

- introduced `KaiaFieldSceneCenter = (10.4, 0, -10.8)` in `AnemoraChapter1SceneSetup`
- moved the current/past Kaia field ground, work shed, well, spice jar, S4D/S4G triggers, trace visuals, and manifest positions around that center
- moved `Ch1_CaptureTarget_kaia_field` to the field/traces center
- tuned the capture helper fallback plan for `kaia_field` to a wider inspection view

New actual scene capture after the first Kaia-field refocus and CP-2 path-light polish:

- `docs/devlog/screenshots/chapter1_scene_integrated/20260510_105109/`
- 18 PNG files, `capture_manifest.json`, and `capture_report.md`
- `capture_manifest.json`: `errors=0`, `warnings=0`, `capture_count=18`, no fallback targets
- backlog seed: `docs/chapter1_graphics_visual_polish_backlog_20260510_105109.md`

The `20260510_105109` package supersedes `20260510_102837` because it also contains the CP-2 path-light polish pass.

Implementation then shifted the Kaia capture target again for a tighter trace inspection view:

- `Ch1_CaptureTarget_kaia_field` moved to `(11.7, 0.8, -11.35)`.
- `AnemoraChapter1SceneCaptureHelper` fallback plan for `kaia_field` changed to rotation `(60, -45, 0)` and orthographic size `4.2`.

Current actual scene capture for graphics review:

- `docs/devlog/screenshots/chapter1_scene_integrated/20260510_130917/`
- 18 PNG files, `capture_manifest.json`, and `capture_report.md`
- `capture_manifest.json`: `errors=0`, `warnings=0`, `capture_count=18`, no fallback targets
- backlog seed: `docs/chapter1_graphics_visual_polish_backlog_20260510_130917.md`

The `20260510_130917` package supersedes `20260510_105109` as the active graphics-review target. Graphics review classified CP-1 as `needs polish`, not blocker. The previous Kaia-field framing blocker is resolved: the field/traces are now the subject, and the central big nut tree, east small nut tree, crop patches, well, and trace objects are reviewable in matching Current/Past framing.

Remaining CP-1 visual polish is implementation scene assembly owned:

- preserve the `20260510_130917` camera target / framing baseline
- strengthen patch 1 / 4 / 5 state differences
- make all three fallen nut piles easier to count
- improve east-third soil discoloration readability
- improve murky well readability
- reduce, fade, crop, or soften the left/top roof-wall mass without losing the field/traces framing

Latest runtime validation after low-pass serialization and progression/save test import:

- `<temp>\anemora_ch1_impl_runtime_validator_after_lowpass_and_progression_import.log`
- `Info=107`, `Warning=3`, `Error=0`, `PendingWiring=0`
- `CP1Scene4Trace=56`
- `CP2SightlineReveal=30`
- the remaining warnings are accepted S4D/S4G temporary placement deltas

Capture plan validation after the `20260510_130917` target shift:

- `<temp>\anemora_ch1_impl_capture_plan_validate_after_kaia_target_130917.log`
- `errors=0`, `warnings=3`, `viewpoints=9`, `sceneExists=True`
- warnings are static-scan graphics root name warnings; the actual `20260510_130917` capture manifest has `warnings=0` and no fallback targets.
- `Chapter1SceneStructureTests`: `6/6` passed after updating the expected `kaia_field` capture target to `(11.7, 0.8, -11.35)`.
  - XML: `<temp>\anemora_ch1_impl_scene_structure_tests_after_130917.xml`
- `Chapter1SceneCapturePlanTests`: `6/6` passed.
  - XML: `<temp>\anemora_ch1_impl_scene_capture_plan_tests_after_130917.xml`

Targeted implementation test:

- `Chapter1RuntimeSceneValidatorTests`: `14/14` passed
  - XML: `<temp>\anemora_ch1_impl_runtime_scene_validator_tests_after_manifest_override.xml`
  - The failing synthetic missing-manifest tests were isolated from the real implementation manifest asset by a test-only validator entry point.
- `Chapter1Scene4TraceRuntimeTests`: `1/1` passed
  - XML: `<temp>\anemora_ch1_impl_scene4_trace_runtime.xml`
  - The test loads `Anemora_Chapter1`, dispatches `touch_spice_jar_001`, verifies one reflected record, and confirms all 10 `Chapter1_TraceManifestReflector_S4F` trace targets become active.
- `Chapter1Scene4AutoTriggerRuntimeTests`: `1/1` passed
  - XML: `<temp>\anemora_ch1_impl_scene4_auto_trigger_runtime.xml`
  - The test loads `Anemora_Chapter1`, moves the player into S4D/S4G trigger positions, verifies local windows open, and confirms both completed progression flags are set.
- Full EditMode after runtime Phase AE-AH progression/save tests and Zone1 low-pass serialization refresh: `172 total / 150 passed / 0 failed / 22 skipped`
  - XML: `<temp>\anemora_ch1_impl_editmode_after_runtime_ae_ah_lowpass.xml`
- Full PlayMode after Scene 1 book trace, Scene 4 trace / auto-trigger runtime coverage, and SymbolWheel scene progression coverage: `63/63` passed
  - XML: `<temp>\anemora_ch1_impl_playmode_after_scene1_book_trace.xml`

## Remaining Work

- Implementation-owned visual polish from graphics review of `20260510_130917`.
- In-editor timing / interaction pass for `Past_SpiceJarPlaceholder`.
- Recapture after CP-1 readability polish.
- Playable sign-off remains user review.

## 2026-05-10 Visual Polish Recapture

Implementation applied the `20260510_130917` graphics verdict follow-up and generated a new review package:

- `docs/devlog/screenshots/chapter1_scene_integrated/20260510_135626/`
- 18 PNG files, `capture_manifest.json`, and `capture_report.md`
- `capture_manifest.json`: `errors=0`, `warnings=0`, `capture_count=18`, no fallback targets
- backlog seed: `docs/chapter1_graphics_visual_polish_backlog_20260510_135626.md`

CP-1 changes in this pass:

- preserved the `Ch1_CaptureTarget_kaia_field` position while tightening the helper orthographic size from `4.2` to `3.95`
- added Current/Past Kaia-field state details directly to the scene assembly:
  - Current patch 1 is dead, patch 4 / 5 are newly withering, and the remaining patches stay muted green
  - Past patch 1 is only withering, patch 4 / 5 stay healthy
  - Current has three visible fallen nut piles; Past has one small pre-state pile
  - Current has an east-third soil discoloration overlay; Past has only a small west-side stain
  - Current well water is murky; Past well water is clear
- enlarged / brightened trace-manifest post visuals for patch 1 / 4 / 5, fallen nut piles, soil discoloration, and murky well water
- added a past-side well slot so Current/Past well comparison is visible

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
