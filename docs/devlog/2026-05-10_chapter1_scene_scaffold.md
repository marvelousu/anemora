# Chapter 1 Scene Scaffold (2026-05-10)

## Summary

graphics foundation orchestrator review の #8 / #9 に向けて、新規 `Assets/Scenes/Anemora_Chapter1.unity` を生成し、既存 Zone1 prefabs を使った Chapter 1 route scaffold、section trigger、camera rig、lighting tone、graphics integration slots を配置した。

## Changed

- Added `Assets/Scenes/Anemora_Chapter1.unity`.
- Added `Anemora_Chapter1.unity` to `ProjectSettings/EditorBuildSettings.asset`.
- Added `Assets/Editor/AnemoraChapter1SceneSetup.cs`.
  - Clones `Anemora_Main` as a Stage 3 reference source, then saves and configures `Anemora_Chapter1`.
  - Removes Stage 3 `DemoZone1_*` roots in the new scene only.
  - Builds `Chapter1_Route_Current` / `Chapter1_Route_Past` under existing `Root_Current` / `Root_Past`.
  - Creates `Chapter1_GraphicsIntegration_Current` / `Chapter1_GraphicsIntegration_Past` slots for graphics-integration prefabs.
- Added lightweight runtime markers:
  - `Assets/Scripts/Chapter/ChapterSectionTrigger.cs`
  - `Assets/Scripts/Chapter/ChapterCameraAnchor.cs`
  - `Assets/Scripts/Chapter/ChapterCameraRigController.cs`
- Added `Assets/Tests/EditMode/Chapter1SceneStructureTests.cs`.
  - Verifies scene roots, section trigger names, trigger collider sizes, camera anchor references, route boundary collider contract, 9 graphics capture target positions, and player movement blocker settings.
- Added `Assets/Tests/PlayMode/Chapter1SceneLoadSmokeTests.cs`.
  - Loads `Anemora_Chapter1` in PlayMode and verifies camera rig, section trigger anchors, player blocker settings, route boundary colliders, and actual `CanOccupy` rejection against every route boundary.
  - Moves the player through all 6 section triggers and verifies each trigger moves the camera to its linked anchor.
- Updated `Assets/Scripts/Player/PrototypePlayerController.cs`.
  - Adds optional movement collision using a blocker layer mask while leaving Stage 3 compatibility off by default.
  - `Anemora_Chapter1.unity` enables this only for the Chapter 1 player instance and blocks the route boundary colliders.
- Updated `Assets/Scripts/Chapter/ChapterSectionTrigger.cs`.
  - `triggerOnce=false` now means re-enterable, not every-frame firing while the player remains inside the trigger.
- Updated `docs/ASSET_STRUCTURE.md`.

## Scene Contents

- Current-side route scaffold includes:
  - Niro house cluster using `House_Player`, `Bed_Player`, `Door_House`, `Table_SmallChair_Wooden`.
  - Plaza cluster using `Plaza_Fountain_Dry_Broken` and `StreetLamp`.
  - Library ruins cluster using `Library_Ruin`, `Bookshelf_Empty`, and inactive `Book_Family_Current`.
  - Placeholder slots for Mia house, street corner / Aria house, Kaia field, north ruins, locked-door visual, and enterable ruin shell.
- Past-side route scaffold includes:
  - Library-side past book setup using `Bookshelf_Library_Past` and `Book_Family_Past`.
  - Placeholder slots for past Aria house interior and Kaia field past state.
- Section triggers:
  - `O_Prologue`
  - `S1_Library`
  - `S2_MiaHouse`
  - `S3_StreetAria`
  - `S4_KaiaField`
  - `S5_NorthRuins`
- Camera:
  - Main camera is orthographic, isometric-positioned, and set to current visual + UI layers.
  - Per-section camera anchor transforms are serialized through `ChapterCameraAnchor`.
  - `ChapterCameraRigController` is wired on `Chapter1_CameraRig`; it subscribes to section trigger entry events and applies the linked camera anchor / orthographic size / tone.
- Lighting:
  - Directional key light at `48, -34, 0`.
  - Twilight ambient / fog tone configured at scene level.

## Verification

- Unity batchmode setup completed:
  - `Anemora Chapter 1 scene setup completed: Assets/Scenes/Anemora_Chapter1.unity`
  - process return code `0`
- Unity batchmode import/compile smoke completed:
  - `Tundra build success`
  - process return code `0`
- Superseded Full EditMode after player boundary collision wiring:
  - `168 total / 146 passed / 0 failed / 22 skipped`
  - `Chapter1SceneStructureTests`: `6/6` passed
  - `Chapter1DialogueAssetTests`: `7/7` passed
  - XML: `<temp>\anemora_ch1_impl_editmode_after_dialogue_key_migration.xml`
  - skips are expected review-capture / pass7-pass8 manifest gates.
- Full PlayMode after player boundary collision, axis-slide coverage, section camera-transition coverage, sightline enter-edge/audio coverage, chapter transition smoke, Scene 1 book trace runtime coverage, Scene 4 trace runtime coverage, Scene 4 auto-trigger runtime coverage, and SymbolWheel scene progression coverage:
  - `63/63` passed
  - `Chapter1SceneLoadSmokeTests`: `2/2` passed
  - `ChapterCameraRigControllerTests`: `1/1` passed
  - `PrototypePlayerControllerCollisionTests`: `2/2` passed
  - `ChapterTransitionControllerPlayModeTests`: `1/1` passed
  - `Scene2SightlineRevealControllerTests`: `1/1` passed
  - `Zone1AudioWiringTests`: `4/4` passed
  - `Chapter1Scene4TraceRuntimeTests`: `1/1` passed
  - `Chapter1Scene4AutoTriggerRuntimeTests`: `1/1` passed
  - `Chapter1Scene1BookTraceRuntimeTests`: `1/1` passed
  - `SymbolWheelProgressionTests`: `4/4` passed
  - XML: `<temp>\anemora_ch1_impl_playmode_after_scene1_book_trace.xml`
- Windows build smoke after sightline enter-edge hardening:
  - output: `<temp>\anemora_ch1_build_smoke_after_sightline_reentry\Anemora.exe`
  - log: `<temp>\anemora_ch1_impl_build_smoke_after_sightline_reentry.log`
  - result: `Build Finished, Result: Success`
  - caveat: CodeCoverage package `System.Numerics` resolution noise appears in the log.
- Runtime validator after the `20260510_130917` Kaia capture target shift:
  - superseded by the later low-pass / progression import validator below
- Runtime validator after low-pass serialization and runtime Phase AE-AH progression/save test import:
  - `Info=107`, `Warning=3`, `Error=0`, `PendingWiring=0`
  - log: `<temp>\anemora_ch1_impl_runtime_validator_after_lowpass_and_progression_import.log`
- Capture plan validation after the same target shift:
  - `errors=0`, `warnings=3`, `viewpoints=9`, `sceneExists=True`
  - log: `<temp>\anemora_ch1_impl_capture_plan_validate_after_kaia_target_130917.log`
  - warnings are static-scan graphics root name warnings; actual capture `20260510_130917` has no warnings or fallback targets.
- Targeted scene structure validation after updating `Ch1_CaptureTarget_kaia_field` to `(11.7, 0.8, -11.35)`:
  - `Chapter1SceneStructureTests`: `6/6` passed
  - XML: `<temp>\anemora_ch1_impl_scene_structure_tests_after_130917.xml`
- Targeted capture helper validation after the `20260510_130917` shift:
  - `Chapter1SceneCapturePlanTests`: `6/6` passed
  - XML: `<temp>\anemora_ch1_impl_scene_capture_plan_tests_after_130917.xml`
- Zone1 audio verification after serializing the scene `musicLowPassFilter` reference:
  - `Zone1AudioSceneSetup.VerifyChapter1Scene`: success
  - log: `<temp>\anemora_ch1_impl_zone1_audio_verify_after_lowpass_serialized.log`
  - `Zone1AudioWiringTests`: `4/4` passed
  - XML: `<temp>\anemora_ch1_impl_zone1_audio_wiring_after_lowpass_serialized.xml`
- Full EditMode after runtime Phase AE-AH progression/save tests and low-pass serialization refresh:
  - `172 total / 150 passed / 0 failed / 22 skipped`
  - XML: `<temp>\anemora_ch1_impl_editmode_after_runtime_ae_ah_lowpass.xml`

## Notes

- `Anemora_Main.unity` remains the Stage 3 reference scene.
- Graphics-integration session owns the production visual assets under `Chapter1MapProduction` / `Chapter1DetailKitProduction`. This scene currently has placeholder slots and optional prefab loading in the setup tool so those assets can be inserted after integration.
- Runtime / Portal Systems session owns #10 / #12 / #13 / #14. This pass only preserves the existing local diorama window wiring and provides scene hook points.

## Latest Graphics Verdict

Graphics review of `docs/devlog/screenshots/chapter1_scene_integrated/20260510_130917/` classified the integrated scene as `needs polish`, with no blocker and no graphics-owned blocker.

Implementation-owned follow-up:

- keep the `20260510_130917` Kaia capture target / framing baseline
- polish CP-1 patch 1 / 4 / 5 state contrast, fallen nut piles, east-third soil, and murky well readability
- reduce or soften left/top roof-wall weight without losing the field/traces framing
- polish CP-2 path-light so it reads as subtle, static, warm environmental light rather than translucent blockout geometry

## Visual Polish Recapture

Applied the latest implementation-owned visual polish and generated:

- `docs/devlog/screenshots/chapter1_scene_integrated/20260510_135626/`
- `capture_manifest.json`: `errors=0`, `warnings=0`, `capture_count=18`, no fallback targets
- backlog seed: `docs/chapter1_graphics_visual_polish_backlog_20260510_135626.md`

Validation:

- runtime validator: `Info=107`, `Warning=3`, `Error=0`, `PendingWiring=0`
- capture plan validation: `errors=0`, `warnings=3`, `viewpoints=9`, `sceneExists=True`
- Full EditMode: `172 total / 150 passed / 0 failed / 22 skipped`
- Targeted `Chapter1SceneLoadSmokeTests`: `3/3` passed after adding section-center occupiable coverage
- Targeted `ChapterTransitionControllerPlayModeTests`: `2/2` passed after adding trigger-polling scene-wired transition coverage
- Full PlayMode: `65/65` passed
  - XML: `<temp>\anemora_ch1_impl_playmode_after_transition_polling.xml`
- Windows build smoke: success
  - output: `<temp>\anemora_ch1_build_smoke_after_transition_polling\Anemora.exe`
  - log: `<temp>\anemora_ch1_impl_build_smoke_after_transition_polling.log`
  - result line: `Build Finished, Result: Success`

Review-fix validation after chapter transition edge-case hardening:

- Chapter transition same-scene completion now restores fade/title visibility and the previous player-controller enabled state when `loadNextChapterScene = false`.
- Intermediate save now persists `chapter1_pebble_001` as a reflected `ActionRecordEntry` for `chapter2_scene4` in addition to `chapter1_complete` / `chapter1.pebble.kicked` raw flags.
- Trigger polling now uses trigger/player collider overlap and avoids latching the inside state when the required progression flag is missing.
- Targeted `ChapterTransitionControllerTests`: `1/1` passed.
  - XML: `<temp>\anemora_ch1_impl_chapter_transition_reviewfix_editmode.xml`
- Targeted `ChapterTransitionControllerPlayModeTests`: `3/3` passed.
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

## Review Fix 2 Validation

Scene scaffold validation now includes the follow-up chapter-transition save hardening:

- live `ActionRecordRuntime` gets `chapter1_pebble_001` after successful chapter transition save
- runtime completion flags are committed only after save write succeeds
- save failure restores same-scene fade/title/player state without committing completion flags or pebble record

Latest validation:

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

## Next

- Send `20260510_135626` to graphics for visual verdict.
- Continue camera composition / playable flow review; current scene structure contract catches route-boundary, section-center occupancy, camera-rig wiring, player blocker settings, and capture-target regressions, but it is not a playable sign-off.
