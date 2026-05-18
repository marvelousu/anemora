# Chapter 1 Playable-First Route Pass - 2026-05-11

## Summary

User priority is now playable-first. This pass moves the Chapter 1 review build away from capture-only progress and toward a route the player can launch, move through, and complete with placeholder gameplay objects.

Implemented in this pass:
- Added concrete route milestones for book reflection, seed bag receive, seed bag delivery, and Scene 4 trace reflection.
- Added objective / prompt / feedback text through `Chapter1PlayableFlowController`.
- Added `Chapter1MilestoneInteractable` for simple E / Space route interactions.
- Added minimal seed bag HUD and receive / deliver notifications.
- Gated Scene 5 chapter close on `progression.chapter1.route_ready_for_close`.
- Added ChapterEndMenu wiring and E-4 save flag compatibility bridge.
- Kept Time Window v3.2 as playable range / interaction flow, with restrained floor/range visual.

## Playable Route

The current Chapter 1-first route is:

1. Spawn in Chapter 1 and move immediately.
2. Follow the objective to the library ruins.
3. Use the Scene 1 book milestone to represent entering the Time Window range, touching the past book, and reflecting the current trace.
4. Receive the seed bag at the Mia route milestone. The seed HUD appears and a notification plays.
5. Deliver the seed bag at the Kaia field route milestone. The seed HUD hides and a notification plays.
6. Use the Scene 4 spice jar milestone to represent entering the Time Window range, touching the past object, and reflecting the current trace.
7. Proceed to Scene 5. The chapter close trigger remains blocked until `progression.chapter1.route_ready_for_close` is set.
8. After route-ready, Scene 5 side-view close runs, autosaves, and shows ChapterEndMenu.

## Route Gate

`ChapterTransitionController.requiredRawFlag` is now:

```text
progression.chapter1.route_ready_for_close
```

That flag is separate from chapter close / save flags. It is written only after the playable route requirements are met:

- `progression.chapter1.scene1.book_reflected`
- `progression.chapter1.seed_bag_delivered`
- `progression.chapter1.scene4.trace_reflected`

If the player reaches Scene 5 before route-ready, the transition does not start and the objective feedback explains that the northern route is not ready yet.

## E-4 Save / Menu Decision

The implementation uses a compatibility bridge:

- Legacy flags are preserved:
  - `chapter1_complete`
  - `chapter1.pebble.kicked`
- E-4 fields are also written:
  - `chapter1_completed`
  - `chapter1_kicked_pebble`
  - `chapter1_book_taken`
  - `chapter1_signboard_revealed`
  - `current_chapter=2`
  - `playtime_chapter1=<seconds>`

`ChapterEndMenu` is intentionally simple but functional for the VS:

- `続ける`: shows `Chapter 2 は次の更新で実装予定です`, then attempts to return to `Title`.
- `メインメニュー`: attempts to load `Title`.
- `終了`: calls `Application.Quit()`.

If `Title` is not registered in the review build, the menu shows a fallback notification instead of soft-locking.

## User-Checkable Build

- EXE: `<temp>\anemora_ch1_playable_first_20260511_0140\Anemora_Chapter1.exe`
- Build log: `<temp>\anemora_ch1_playable_first_build_20260511_0140.log`
- Build result: succeeded, warnings=0, errors=0.

Current font-fix build:

- EXE: `<temp>\anemora_ch1_playable_fontfix_20260511_0200\Anemora_Chapter1.exe`
- Build log: `<temp>\anemora_ch1_playable_fontfix_build_20260511_0200.log`
- Build result: succeeded, warnings=0, errors=0.
- Purpose: fixes missing TMP font asset warnings on ObjectiveText / NotificationText and removes unsupported UI glyphs from the VS objective/menu text.
- Player launch smoke log: `<temp>\anemora_ch1_playable_fontfix_player_launch_smoke_20260511.log`
  - 12-second hidden launch smoke.
  - No ObjectiveText / NotificationText / TMP font / unsupported glyph warnings observed.

Current text-fix build:

- EXE: `<temp>\anemora_ch1_playable_textfix_20260511_0220\Anemora_Chapter1.exe`
- Build log: `<temp>\anemora_ch1_playable_textfix_build_20260511_0220.log`
- Build result: succeeded, warnings=0, errors=0.
- Purpose: fixes the graphics smoke blocker where the objective / prompt text rendered as mojibake in the 0200 font-fix EXE.
- Implementation note: player-facing Chapter 1 route UI strings now come from ASCII-safe `\uXXXX` literals via `Chapter1UiText`, so Unity's Windows C# source decoding cannot corrupt them at compile time.
- Player launch smoke log: `<temp>\anemora_ch1_playable_textfix_player_launch_smoke_20260511_0220.log`
  - 12-second hidden launch smoke.
  - No ObjectiveText / NotificationText / TMP font / unsupported glyph / mojibake-pattern warnings observed.

## Manual Smoke Checklist

1. Launch the EXE.
2. Confirm the player can move immediately.
3. Confirm objective text appears and points toward the library / Time Window book route.
4. Approach `Milestone_S1_TimeWindow_Book` and press E or Space. The current book trace should appear and the objective should advance.
5. Approach `Milestone_S2_Receive_SeedBag` and press E or Space. The seed bag HUD should fade in with `たねのつつみを うけとった`.
6. Approach `Milestone_S4_Deliver_SeedBag` and press E or Space. The seed bag HUD should fade out with `たねのつつみを わたした`.
7. Approach `Milestone_S4_TimeWindow_SpiceJar` and press E or Space. The current trace should appear and the objective should send the player north.
8. Before route-ready, entering Scene 5 should not close the chapter.
9. After route-ready, entering Scene 5 should run the side-view close, save, fade/title, and ChapterEndMenu.
10. Confirm ChapterEndMenu buttons respond and do not trap the player.

## Validation

- Scene setup: `<temp>\anemora_ch1_playable_first_scene_setup2_retry_20260511.log`
- Compile/import smoke: `<temp>\anemora_ch1_playable_first_compile2_20260511.log`
- Runtime validator actual scene: `<temp>\anemora_ch1_playable_first_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`
  - Remaining warnings are accepted S4D / S4G placement deltas.
- Font-fix runtime validator: `<temp>\anemora_ch1_playable_fontfix_runtime_validator2_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`
  - No ObjectiveText / NotificationText font warnings observed in this validator log.
- PlayMode route tests: `<temp>\anemora_ch1_playable_first_flow_playmode_rerun.xml`
  - 3 total / 3 passed / 0 failed
- PlayMode route no-soft-lock guard: `<temp>\anemora_ch1_playable_first_flow_playmode_guard_20260511.xml`
  - 3 total / 3 passed / 0 failed
  - Confirms early Scene 5 block keeps transition idle, does not save, does not show ChapterEndMenu, keeps player control enabled, and leaves fade at 0.
- PlayMode chapter transition tests: `<temp>\anemora_ch1_playable_first_transition_playmode_rerun.xml`
  - 5 total / 5 passed / 0 failed
- PlayMode ChapterEndMenu button guard: `<temp>\anemora_ch1_playable_first_transition_playmode_menu_guard_20260511.xml`
  - 5 total / 5 passed / 0 failed
  - Confirms the scene-wired ChapterEndMenu Continue button shows the Chapter 2 fallback message and Main Menu shows the unregistered-title fallback message in the current review build.
- Font-fix targeted PlayMode: `<temp>\anemora_ch1_playable_fontfix_playmode2_20260511.xml`
  - 8 total / 8 passed / 0 failed
  - No ObjectiveText / NotificationText font warnings or unsupported glyph warnings observed in the final font-fix PlayMode log.
- Font assignment regression guard: `<temp>\anemora_ch1_playable_font_guard_flow_playmode_20260511.xml`
  - 3 total / 3 passed / 0 failed
  - Directly asserts active-scene `ObjectiveText` and `NotificationText` have TMP font assets assigned.
- Font-fix player launch smoke: `<temp>\anemora_ch1_playable_fontfix_player_launch_smoke_20260511.log`
  - No ObjectiveText / NotificationText / TMP font / unsupported glyph warnings observed during launch.
- Text-fix compile/import smoke: `<temp>\anemora_ch1_playable_textfix_compile2_20260511.log`
  - No C# / shader / compiler / exception / batchmode abort errors observed.
- Text-fix scene setup: `<temp>\anemora_ch1_playable_textfix_scene_setup2_retry_20260511.log`
  - Scene setup completed after one transient same-project Unity lock retry.
  - Scene YAML stores route/menu UI text as escaped Unicode and has no known mojibake marker hits.
- Text-fix runtime validator: `<temp>\anemora_ch1_playable_textfix_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`
  - Remaining warnings are accepted S4D / S4G placement deltas.
- Text-fix targeted PlayMode: `<temp>\anemora_ch1_playable_textfix_playmode_20260511.xml`
  - 8 total / 8 passed / 0 failed
  - Confirms the route objective content, font assignment, early S5 gate, route-ready flag, ChapterEndMenu save/menu path, and no-soft-lock guards.
- Text-fix player launch smoke: `<temp>\anemora_ch1_playable_textfix_player_launch_smoke_20260511_0220.log`
  - No ObjectiveText / NotificationText / TMP font / unsupported glyph / known mojibake-pattern warnings observed during launch.
- `git diff --check`: passed.

## Known Gaps

- Route milestones are still placeholder objects / labels, not final production interaction art.
- This is not a full manual playthrough sign-off. It is a playable route pass plus targeted automated smoke coverage.
- Final character sprites and portrait expression mapping remain deferred until user-approved asset import.
- A-4 elder, C-1 optional book / signboard side puzzles, and broad B visual polish are deferred unless they directly improve playable flow.
- Time Window v3.2 is represented through simplified range/interactable milestones in the VS route; final object dressing can be refined later.
- ChapterEndMenu is functional but visually provisional.
- ChapterEndMenu Quit still needs manual smoke in a player build. Automated coverage verifies menu visibility, Continue fallback, Main Menu fallback, save handoff, and no-soft-lock save failure.

## Time Window Trace-Fix Update

Graphics smoke for the 20260511_0220 build confirmed the text mojibake blocker was resolved, then found a new player-facing blocker: the first route Time Window read as a bright bulky ring / portal-like object. The current v3.2 requirement is an enterable range with a thin restrained floor / world-space trace, not a gate/ring/arch silhouette.

Updated implementation:

- `TimeWindowDiorama` hides the prefab frame by default.
- Runtime vertical veil panels are disabled by default.
- The default runtime visual is now a thin low floor/range trace.
- Brush placement preview lift, edge thickness, and alpha were reduced so placement reads as a range mark instead of a ring.
- Playable milestone labels were added for route discoverability while the VS still uses placeholder objects.

Current user-checkable build:

- EXE: `<temp>\anemora_ch1_playable_twtrace_20260511_0238\Anemora_Chapter1.exe`
- Build log: `<temp>\anemora_ch1_playable_twtrace_build_20260511_0238.log`
- Build result: succeeded, warnings=0, errors=0.
- Player launch smoke log: `<temp>\anemora_ch1_playable_twtrace_player_launch_smoke_20260511_0238.log`
  - No ObjectiveText / NotificationText / TMP font / unsupported glyph / known mojibake-pattern warnings observed.

Additional validation:

- Compile/import smoke: `<temp>\anemora_ch1_playable_twtrace_compile2_20260511.log`
- Scene setup: `<temp>\anemora_ch1_playable_twtrace_scene_setup2_retry_20260511.log`
  - Scene setup completed after one transient same-project Unity lock retry.
  - No TMP / Font Asset / unsupported glyph warnings observed in setup.
- Runtime validator: `<temp>\anemora_ch1_playable_twtrace_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`
  - Remaining warnings are accepted S4D / S4G placement deltas.
- Targeted PlayMode: `<temp>\anemora_ch1_playable_twtrace_playmode_20260511.xml`
  - 8 total / 8 passed / 0 failed.
- Demo PlayMode v3.2 visual regression guard: `<temp>\anemora_ch1_twtrace_demo_smoke_playmode_20260511.xml`
  - 2 total / 2 passed / 0 failed.
  - Confirms the runtime Time Window uses five thin floor-trace pieces, does not spawn vertical veil panels by default, and keeps the static prefab frame renderers disabled.
- `git diff --check`: passed.

Obsolete 0238 manual smoke correction notes:

- Seed receive text should read `種の包みを受け取った`.
- Seed delivery text should read `種の包みを渡した`.
- ChapterEndMenu labels should read `続ける`, `メインメニュー`, and `終了`.

Known gap after this update:

- Superseded by the 20260511_0254 static Time Window graphics suppression build below. Do not route the 0238 build as accepted.

## 20260511_0254 Static Time Window Graphics Suppression

Graphics rejected the 20260511_0238 EXE. Text/readability was fixed, but the first route Time Window still read as a raised ring / portal in the actual player camera. The PlayMode guard remains useful, but it only verifies the runtime `TimeWindowDiorama`; it does not prove that static scene dressing is absent from the player-facing camera.

Follow-up implementation:

- `BuildGraphicsIntegrationSlots` no longer spawns `Ch1_TimeWindow_Adjacent_SceneAssembly_A` for the Chapter 1 playable scene.
- `SuppressStaticTimeWindowGraphics` disables renderers under known static Time Window graphics roots if they are introduced by future graphics integration paths.
- Added `PlayableSceneDoesNotExposeStaticTimeWindowRingGraphics` PlayMode guard to scan the loaded Chapter 1 scene for enabled static Time Window graphics renderers.
- Scene YAML no longer contains `Ch1_Graphics_CentralPlaza_Current`, `Ch1_Graphics_CentralPlaza_Past`, `TimeWindow_Adjacent`, `TimeWindowAdjacent`, `TimeWindow_GroundDecalVariants`, `TimeWindowSurfaceHelperKit`, or `ThinSubBand` markers after setup.

Current user-checkable build:

- EXE: `<temp>\anemora_ch1_playable_notwstatic_20260511_0254\Anemora_Chapter1.exe`
- Build log: `<temp>\anemora_ch1_playable_notwstatic_build_20260511_0254.log`
- Build result: succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_notwstatic_player_launch_smoke_20260511_0254.log`
  - No ObjectiveText / NotificationText / TMP font / unsupported glyph / known mojibake-pattern warnings observed.

Validation:

- Compile/import smoke: `<temp>\anemora_ch1_notwstatic_compile_20260511.log`
- Scene setup: `<temp>\anemora_ch1_notwstatic_scene_setup_20260511.log`
  - Scene setup completed.
- Runtime validator: `<temp>\anemora_ch1_notwstatic_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`
  - Remaining warnings are accepted S4D / S4G placement deltas.
- Chapter 1 playable flow PlayMode: `<temp>\anemora_ch1_notwstatic_playable_flow_playmode_20260511.xml`
  - 4 / 4 passed.
  - Includes the new static Time Window graphics suppression guard.
- ChapterTransition PlayMode: `<temp>\anemora_ch1_notwstatic_transition_playmode_20260511.xml`
  - 5 / 5 passed.
- `git diff --check`: passed after restoring Unity-generated Addressables/TMP side effects.

Next:

1. Route the 0254 EXE to graphics for the same narrow playable visual smoke.
2. If Time Window visual passes, continue with manual/playable route smoke rather than broad graphics polish.
3. If it still reads as a portal/ring, suppress additional visible static geometry found in the smoke screenshot and keep the runtime visual as floor/range trace only.

## 20260511_0307 Dynamic Floor Trace Tightening

Graphics smoke for 0254 still saw the blocker, and the screenshot made the visible object identifiable as the `TimeWindow_Diorama` frame / runtime presentation rather than the scene-integrated TimeWindow_Adjacent graphics roots. This pass keeps the static graphics suppression and tightens the dynamic Time Window presentation.

Implementation delta:

- `TimeWindowDiorama`
  - `floorOverlayLift` reduced to 0.012.
  - Thin trace footprint / edge thickness reduced.
  - Edge lift is derived from floor overlay height instead of using the previous 0.045 world-space lift.
  - Veil alpha clamp reduced to 0.045.
  - `TimeWindow_Diorama.prefab` serialized `floorOverlayLift` updated to 0.012.
- `TimeFramePortalController`
  - Brush preview default lift / edge thickness / alpha reduced.
  - Added `NormalizeTimeWindowVisuals` so existing scene serialized values are clamped for local-window mode at runtime.
- `DemoPlayableSmokeTests`
  - Added guards that runtime trace and brush preview stay close to floor height and do not keep old raised values.

Current user-checkable build:

- EXE: `<temp>\anemora_ch1_playable_floortrace_20260511_0307\Anemora_Chapter1.exe`
- Build log: `<temp>\anemora_ch1_playable_floortrace_build_20260511_0307.log`
- Build result: succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_floortrace_player_launch_smoke_20260511_0307.log`
  - No ObjectiveText / NotificationText / TMP font / unsupported glyph / known mojibake-pattern warnings observed.

Validation:

- Scene setup: `<temp>\anemora_ch1_floortrace_scene_setup_20260511.log`
  - Scene setup completed.
  - Scene YAML still has no known static TimeWindow_Adjacent / ThinSubBand / surface helper markers.
- Runtime validator: `<temp>\anemora_ch1_floortrace_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`
  - Remaining warnings are accepted S4D / S4G placement deltas.
- Targeted PlayMode: `<temp>\anemora_ch1_floortrace_targeted_playmode_rerun2_20260511.xml`
  - 11 / 11 passed.
  - Covers Chapter1 playable flow, static Time Window graphics suppression, ChapterTransition, and demo Time Window v3.2 floor-trace height guards.
- `git diff --check`: passed after restoring Unity-generated Addressables/TMP side effects.

Next:

1. Route the 0307 EXE to graphics as the new current narrow playable visual smoke target, superseding 0254.
2. If a ring remains, the next fix should use the screenshot to disable the exact rendered child under `TimeWindow_Diorama` or replace the milestone/book cue.

## 20260511_0316 TimeWindow FrameRoot Off

Graphics rejected the 20260511_0307 EXE: the visible object still looked like the same bright raised oval/ring in the first Time Window / book objective area. The screenshot points at the `TimeWindow_Diorama` prefab frame, so this pass disables that exact family directly.

Implementation delta:

- `TimeWindowDiorama.ApplyFrameVisibility` now disables frame renderers by name (`TimeVolume_FrameRoot`, `TimeVolume_Front_*`, `TimeVolume_Back_*`, `TimeVolume_Left_*`, `TimeVolume_Right_*`) instead of relying only on the serialized `scalableRoot` reference.
- `TimeWindowDiorama` also sets the frame root GameObject inactive when `showPrefabFrame=false`.
- `TimeWindow_Diorama.prefab` now has `TimeVolume_FrameRoot` inactive by default.
- Added `Chapter1QuickTimeWindowKeepsPrefabFrameHidden` PlayMode guard, which opens the Chapter 1 quick Time Window and directly asserts:
  - `TimeVolume_FrameRoot` exists but has 0 enabled renderers.
  - No enabled renderer remains under `TimeVolume_*` frame ancestors.
  - Thin floor trace count / height remains within v3.2 guard limits.

Current user-checkable build:

- EXE: `<temp>\anemora_ch1_playable_frameoff_20260511_0316\Anemora_Chapter1.exe`
- Build log: `<temp>\anemora_ch1_playable_frameoff_build_20260511_0316.log`
- Build result: succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_frameoff_player_launch_smoke_20260511_0316.log`
  - No ObjectiveText / NotificationText / TMP font / unsupported glyph / known mojibake-pattern warnings observed.

Validation:

- Compile/import smoke: `<temp>\anemora_ch1_frameoff_compile_20260511.log`
- Scene setup: `<temp>\anemora_ch1_frameoff_scene_setup_20260511.log`
  - Scene setup completed.
  - Scene YAML still has no known static TimeWindow_Adjacent / ThinSubBand / surface helper markers.
- Runtime validator: `<temp>\anemora_ch1_frameoff_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`
  - Remaining warnings are accepted S4D / S4G placement deltas.
- Targeted PlayMode: `<temp>\anemora_ch1_frameoff_targeted_playmode_20260511.xml`
  - 12 / 12 passed.
  - Covers Chapter1 playable flow, ChapterTransition, demo Time Window v3.2 trace guards, and the new Chapter1 quick-window FrameRoot-off guard.
- `git diff --check`: passed after restoring Unity-generated Addressables/TMP side effects.

Next:

1. Route the 0316 EXE to graphics as the new current narrow playable visual smoke target, superseding 0307.
2. If a ring remains, the next diagnosis should focus on the first route book/milestone cue or a non-TimeWindow_Diorama object in the screenshot.

## 20260511_0333 First Objective Brute-Flat Visual

Graphics rejected the 20260511_0316 EXE: the `TimeWindow_Diorama` frame family was disabled, but the first objective area still had a raised oval/ring-like visual. The renderer dump identified the remaining visual as static S1 library graphics integration `time_window_trace` / `InnerRimHint` meshes, plus the first book marker still sitting above the floor.

Implementation delta:

- `AnemoraChapter1SceneSetup.SuppressStaticTimeWindowGraphics` now disables renderers whose object path or material name contains `time_window_trace`, `InnerRimHint`, or `time_window`.
- `Milestone_S1_TimeWindow_Book` is now a low flat Current-layer marker:
  - position `y=0.045`
  - scale `y=0.025`
  - label `1 BOOK TRACE`
  - shadows disabled
- `TimeWindowDiorama` now generates one dark low-opacity footprint only for the v3.2 visible range trace. It no longer generates four raised edge strips.
- PlayMode guards now assert:
  - no static Time Window graphics renderers remain enabled in the playable Chapter 1 scene
  - first objective marker is flat and Current-layer
  - quick Time Window has one footprint trace, no edge strips, no vertical veil, and no prefab frame renderers
- Renderer dump artifacts:
  - `<temp>\anemora_ch1_first_objective_renderers_base_20260511_033010.txt`
  - `<temp>\anemora_ch1_first_objective_renderers_runtime_20260511_033010.txt`
  - The previous `time_window_trace` / `InnerRimHint` candidates are present but disabled.

Current user-checkable build:

- EXE: `<temp>\anemora_ch1_playable_bruteflat_20260511_0333\Anemora_Chapter1.exe`
- Build log: `<temp>\anemora_ch1_playable_bruteflat_build_20260511_0333.log`
- Build result: succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_bruteflat_player_launch_smoke_20260511_0333.log`
  - No ObjectiveText / NotificationText / TMP font / unsupported glyph / known mojibake-pattern warnings observed.

Validation:

- Scene setup: `<temp>\anemora_ch1_bruteflat_scene_setup2_20260511.log`
  - Scene setup completed.
  - Static `time_window_trace` / `InnerRimHint` renderers are suppressed.
- Runtime validator: `<temp>\anemora_ch1_bruteflat_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`
  - Remaining warnings are accepted S4D / S4G placement deltas.
- Targeted PlayMode:
  - `<temp>\anemora_ch1_bruteflat_flow_playmode2_20260511.xml`: 6 / 6 passed
  - `<temp>\anemora_ch1_bruteflat_demo_playmode_20260511.xml`: 2 / 2 passed
- `git diff --check`: passed after restoring Unity-generated Addressables/TMP side effects.

Next:

1. Route the 0333 EXE to graphics as the new current narrow playable visual smoke target, superseding 0316.
2. If first objective visual passes, continue graphics smoke through blocked S5 feedback, seed HUD notification, route milestone prompts, ChapterEndMenu, and post-route S5 handoff.

## 20260511_0346 Flat Book Cue Replacement

Graphics rejected the 20260511_0333 EXE. The remaining blocker was narrowed to the first-route family book meshes, not the Time Window trace. The enabled `Book_Family_Current_Model` / `Book_Family_Past_Model` meshes were reading as a bright raised oval from the gameplay camera.

Implementation delta:

- `AnemoraChapter1SceneSetup` now flattens the first-route family-book visual after instantiating `Book_Family_Current` and `Book_Family_Past`.
- The original `Book_Family_Current_Model` / `Book_Family_Past_Model` renderers are disabled.
- Temporary playable-first flat cards are added:
  - `Current_FlatBookTrace_PlayableCue`
  - `Past_FlatBookTrace_PlayableCue`
- The book roots, names, reflection target, action id, and milestone semantics are preserved.

Renderer dump evidence:

- `<temp>\anemora_ch1_first_objective_renderers_runtime_20260511_034412.txt`
- The old book model renderers are disabled in base/current/past/runtime-clone contexts.
- Enabled replacement renderers are flat cards, with size around `(0.45, 0.01, 0.35)`.

Current user-checkable build:

- EXE: `<temp>\anemora_ch1_playable_flatbook_20260511_0346\Anemora_Chapter1.exe`
- Build log: `<temp>\anemora_ch1_playable_flatbook_build_20260511_0346.log`
- Build result: succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_flatbook_player_launch_smoke_20260511_0346.log`
  - No ObjectiveText / NotificationText / TMP font / unsupported glyph / known mojibake-pattern / exception / error matches observed.

Validation:

- Scene setup: `<temp>\anemora_ch1_flatbook_scene_setup2_20260511.log`
  - Scene setup completed.
- Runtime validator: `<temp>\anemora_ch1_flatbook_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`
  - Remaining warnings are accepted S4D / S4G placement deltas.
- Targeted PlayMode:
  - `<temp>\anemora_ch1_flatbook_flow_playmode_20260511.xml`: 6 / 6 passed.
  - `<temp>\anemora_ch1_flatbook_flow_playmode2_20260511.xml`: 7 / 7 passed.
  - `<temp>\anemora_ch1_primary_route_playmode_20260511.xml`: 8 / 8 passed.
  - `<temp>\anemora_ch1_chapter_end_menu_playmode_20260511.xml`: 5 / 5 passed.
  - Covers flat first objective, hidden old book mesh, seed HUD receive/deliver notifications, route milestones, Chapter 1 route readiness, early S5 blocked feedback without fade/save/menu, and the primary route from milestones through S5 close to ChapterEndMenu/save flags.
  - ChapterEndMenu scene-wired coverage now asserts continue / main menu / quit button refs and default selection on Continue.
- `git diff --check`: passed after restoring Unity-generated Addressables/TMP side effects.

Next:

1. Route the 0346 EXE to graphics as the new current narrow playable visual smoke target, superseding 0333.
2. If the first objective visual passes, graphics should continue the same narrow smoke through blocked S5 feedback, seed HUD notification, route milestone prompts, ChapterEndMenu, and post-route S5 handoff.

## 20260511_0400 One-Marker-Only First Objective Smoke Build

Graphics rejected the 20260511_0346 EXE. The old family-book meshes were disabled, but the remaining enabled local flat-card cues still read as a bright raised oval/ring from the playable camera. This pass switches the first objective to a single label-only marker and removes local rendered cue cards from the first objective point.

Implementation delta:

- `AnemoraChapter1SceneSetup`
  - `FlattenFirstRouteBookCue` now only disables `Book_Family_Current` / `Book_Family_Past` renderers. It no longer adds `Current_FlatBookTrace_PlayableCue` or `Past_FlatBookTrace_PlayableCue`.
  - `Milestone_S1_TimeWindow_Book` keeps route trigger/state semantics, but its body renderers are disabled.
  - The only intended visible local cue at the first objective point is `Milestone_S1_TimeWindow_Book_Label`.
- `Chapter1PlayableFlowControllerTests`
  - Guards now fail if any local first objective cue renderer remains enabled for `Book_Family_*_Model`, `FlatBookTrace_PlayableCue`, or the marker body renderer.
  - Guards require exactly one enabled `Milestone_S1_TimeWindow_Book_Label` renderer near the first objective.

Renderer dump evidence:

- `<temp>\anemora_ch1_first_objective_renderers_runtime_20260511_035820.txt`
- The dump shows:
  - disabled `Chapter1_PlayableMilestones/Milestone_S1_TimeWindow_Book`
  - disabled `Book_Family_Current_Model` / `Book_Family_Past_Model`
  - no enabled `FlatBookTrace_PlayableCue`
  - enabled `Chapter1_PlayableMilestones/Milestone_S1_TimeWindow_Book_Label`

Current user-checkable build:

- EXE: `<temp>\anemora_ch1_playable_labelonly_20260511_0400\Anemora_Chapter1.exe`
- Build log: `<temp>\anemora_ch1_playable_labelonly_build_20260511_0400.log`
- Build result: succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_labelonly_player_launch_smoke_20260511_0400.log`
  - No ObjectiveText / NotificationText / TMP font / unsupported glyph / known mojibake-pattern / exception / error matches observed.

Validation:

- Scene setup: `<temp>\anemora_ch1_labelonly_scene_setup_20260511.log`
  - Scene setup completed.
- Runtime validator: `<temp>\anemora_ch1_labelonly_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`
  - Remaining warnings are accepted S4D / S4G placement deltas.
- Targeted PlayMode:
  - `<temp>\anemora_ch1_labelonly_flow_playmode_20260511.xml`: 8 / 8 passed.
  - `<temp>\anemora_ch1_chapter_end_menu_playmode_20260511.xml`: 5 / 5 passed.
  - Covers first objective one-marker-only state, seed HUD receive/deliver notifications, route milestones, Chapter 1 route readiness, early S5 blocked feedback without fade/save/menu, and the primary route from milestones through S5 close to ChapterEndMenu/save flags.

Next:

1. Route the 0400 EXE to graphics as the new current narrow playable visual smoke target, superseding 0346.
2. If the same oval/ring remains after this build, the likely culprit is a nearby library/route renderer outside the first objective cue stack; use the renderer dump and graphics screenshot to suppress the local library clutter for playable smoke.

## 20260511_0415 First Objective Library Clutter Suppression Build

Graphics rejected the 0400 label-only build. The explicit first-objective cue stack was already reduced to `Milestone_S1_TimeWindow_Book_Label`, so this pass treats the visible oval/ring as S1/B7 library clutter near the first objective camera area rather than a Time Window or milestone cue.

Implementation delta:

- `AnemoraChapter1SceneSetup`
  - Added a first-objective suppression pass centered at `(0.85, 0, 6.25)`.
  - Suppression radius: 2.25m in X/Z.
  - Disabled enabled renderers near that point when their hierarchy path matches S1 library/B7 detail clutter:
    - `Ch1_Graphics_S1_Library*`
    - `Chapter1_B7_PastLibrary_Dressing*`
    - `Ch1_B7_PastLibrary_DetailMarkers*`
    - `B7_Evidence_*`
    - `LibraryCurrent_*`
    - `LibraryPast_*`
  - Kept `Current_Ground_Library`, `Past_Ground_Library`, and `Milestone_S1_TimeWindow_Book_Label`.
  - Scene setup reported `Suppressed 173 first-objective S1/B7 library renderers for playable smoke.`
- `Chapter1PlayableFlowControllerTests`
  - Added base-scene and runtime-clone guards that fail if enabled S1/B7 clutter renderers remain within the same 2.25m radius.
  - The existing route trigger/action/progression semantics are unchanged.

Renderer dump evidence:

- Base scene: `<temp>\anemora_ch1_first_objective_renderers_base_20260511_041107.txt`
- Runtime clone: `<temp>\anemora_ch1_first_objective_renderers_runtime_20260511_041107.txt`
- Key observations:
  - disabled `LibraryCurrent_FallenBeam_0`
  - disabled `LibraryCurrent_CollapsedShelf_2_Frame`
  - disabled `LibraryPast_OpenBook_0`
  - disabled runtime-clone `B7_Evidence_FamilyBook_Readable`
  - disabled runtime-clone `B7_Evidence_AriaSeat_Readable`
  - disabled runtime-clone `BookshelfMarker_07`
  - enabled local first-objective cue remains only `Milestone_S1_TimeWindow_Book_Label`

Current user-checkable build:

- EXE: `<temp>\anemora_ch1_playable_cluttersupp_20260511_0415\Anemora_Chapter1.exe`
- Build log: `<temp>\anemora_ch1_playable_cluttersupp_build_20260511_0415.log`
- Build result: succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_cluttersupp_player_launch_smoke_20260511_0415.log`
  - No ObjectiveText / NotificationText / TMP font / unsupported glyph / known mojibake-pattern / exception / error matches observed.

Validation:

- Scene setup: `<temp>\anemora_ch1_cluttersupp_scene_setup2_20260511.log`
  - Scene setup completed.
- Runtime validator: `<temp>\anemora_ch1_cluttersupp_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`
  - Remaining warnings are accepted S4D / S4G placement deltas.
- Targeted PlayMode:
  - `<temp>\anemora_ch1_cluttersupp_flow_playmode_20260511.xml`: 8 / 8 passed.
  - Covers first-objective S1/B7 clutter suppression in base scene and runtime clone, plus the existing playable route milestones.

Next:

1. Route the 0415 EXE to graphics as the new current narrow playable visual smoke target, superseding 0400.
2. If the first-objective visual passes, graphics should continue the same narrow smoke through blocked S5 feedback, seed HUD notification, route milestone prompts, ChapterEndMenu, and post-route S5 handoff.

## 20260511_0425 First Objective Camera-Visible-Area Suppression Build

Graphics rejected the 0415 build because S1/B7 clutter just outside the 2.25m cue-centered radius still projected into the lower-left / center-left playable camera area. This pass switches the suppression rule from cue-centered radius to a camera-visible rectangle for the first objective smoke.

Implementation delta:

- `AnemoraChapter1SceneSetup`
  - Suppression now checks whether renderer bounds intersect the first-objective camera smoke rectangle:
    - X: `-2.25 .. 1.85`
    - Z: `4.75 .. 8.95`
  - Suppresses the previous S1/B7 library clutter plus route-level library shell/bookshelf objects:
    - `Ch1_Current_Library_*`
    - `Ch1_Past_Library_*`
    - `Library_Ruin_Model`
    - `Bookshelf_*`
  - Keeps `Current_Ground_Library`, `Past_Ground_Library`, and `Milestone_S1_TimeWindow_Book_Label`.
  - Scene setup reported `Suppressed 279 first-objective S1/B7 library renderers for playable smoke.`
- `Chapter1PlayableFlowControllerTests`
  - Guard now uses the same camera smoke rectangle instead of a 2.25m radius.
  - Renderer dumps now cover a 4.25m radius around the marker to make camera-area spillover visible in logs.

Renderer dump evidence:

- Base scene: `<temp>\anemora_ch1_first_objective_renderers_base_20260511_042353.txt`
- Runtime clone: `<temp>\anemora_ch1_first_objective_renderers_runtime_20260511_042353.txt`
- Key observations:
  - disabled `LibraryCurrent_CollapsedShelf_3_*`
  - disabled route-level `Ch1_Current_Library_EmptyShelf/Bookshelf_Empty_Model/Shelf_*`
  - disabled route-level `Ch1_Past_Library_Bookshelf/Bookshelf_Library_Past_Model`
  - disabled runtime-clone `Ch1_Past_Library_Bookshelf/Bookshelf_Library_Past_Model`
  - enabled first-objective visuals in the smoke area are limited to ground planes, the label marker, and placeholder residents.

Current user-checkable build:

- EXE: `<temp>\anemora_ch1_playable_cameravis_20260511_0425\Anemora_Chapter1.exe`
- Build log: `<temp>\anemora_ch1_playable_cameravis_build_20260511_0425.log`
- Build result: succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_cameravis_player_launch_smoke_20260511_0425.log`
  - No ObjectiveText / NotificationText / TMP font / unsupported glyph / known mojibake-pattern / exception / error matches observed.

Validation:

- Scene setup: `<temp>\anemora_ch1_cameravis_scene_setup3_20260511.log`
  - Scene setup completed.
- Runtime validator: `<temp>\anemora_ch1_cameravis_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`
  - Remaining warnings are accepted S4D / S4G placement deltas.
- Targeted PlayMode:
  - `<temp>\anemora_ch1_cameravis_flow_playmode_20260511.xml`: 8 / 8 passed.

Next:

1. Route the 0425 EXE to graphics as the new current narrow playable visual smoke target, superseding 0415.
2. If the first-objective visual passes, graphics should continue the same narrow smoke through blocked S5 feedback, seed HUD notification, route milestone prompts, ChapterEndMenu, and post-route S5 handoff.

## 20260511_0440 Diagnostic Minimal-Ground First Objective Build

Graphics rejected 0425 because only ground / distant roof / resident-level candidates remained, yet a bright raised oval still appeared in the first route objective view. This pass switches the first objective smoke to a diagnostic minimal view: one neutral matte ground plane plus the first objective label.

Implementation delta:

- `AnemoraChapter1SceneSetup`
  - Expanded the diagnostic first-objective camera band:
    - X: `-3.50 .. 2.60`
    - Z: `3.40 .. 10.25`
  - After milestones are created, disables every enabled renderer intersecting that band except:
    - `Root_Current/Chapter1_Route_Current/Current_Ground_Library`
    - `Chapter1_PlayableMilestones/Milestone_S1_TimeWindow_Book_Label`
  - Replaces `Current_Ground_Library` with a thin neutral matte material:
    - position Y: `-0.01`
    - scale Y: `0.02`
    - material: `Current_Ground_Library_DiagnosticMatteMaterial`
  - This suppresses `Past_Ground_Library`, runtime-clone past ground, residents, S5 distant roof/fog/wall spillover, S1/B7 detail markers, and library arch/roof clutter for this playable-first smoke.
  - Scene setup reported:
    - `Suppressed 295 first-objective S1/B7 library renderers for playable smoke.`
    - `First-objective diagnostic smoke view: kept 2 renderer(s), suppressed 36 camera-visible renderer(s).`
- `Chapter1PlayableFlowControllerTests`
  - First objective guard now treats any renderer in the diagnostic camera band as clutter unless it is the current matte ground or the label.
  - Base and runtime-clone renderer dumps both show exactly two enabled renderers in the diagnostic area.

Renderer dump:

- Base scene: `<temp>\anemora_ch1_first_objective_renderers_base_20260511_043726.txt`
- Runtime clone: `<temp>\anemora_ch1_first_objective_renderers_runtime_20260511_043726.txt`
- Enabled renderers in both dumps:
  - `Chapter1_PlayableMilestones/Milestone_S1_TimeWindow_Book_Label`
  - `Root_Current/Chapter1_Route_Current/Current_Ground_Library`

Current user-checkable build:

- EXE: `<temp>\anemora_ch1_playable_minground_20260511_0440\Anemora_Chapter1.exe`
- Build log: `<temp>\anemora_ch1_playable_minground_build_20260511_0440.log`
- Build result: succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_minground_player_launch_smoke_20260511_0440.log`
  - No ObjectiveText / NotificationText / TMP font / unsupported glyph / known mojibake-pattern / exception / error matches observed.

Validation:

- Scene setup: `<temp>\anemora_ch1_minground_scene_setup2_20260511.log`
- Runtime validator: `<temp>\anemora_ch1_minground_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`
  - Remaining warnings are accepted S4D / S4G placement deltas.
- Targeted PlayMode:
  - `<temp>\anemora_ch1_minground_flow_playmode2_20260511.xml`: 8 / 8 passed.
  - `<temp>\anemora_ch1_cameravis_transition_playmode_20260511.xml`: 5 / 5 passed.

Next:

1. Route the 0440 EXE to graphics as the new current narrow playable visual smoke target, superseding 0425.
2. First check: the first route objective area should now show only the matte ground, objective label, and the moving player, with no bright raised oval/ring/portal-like form.
3. If pass, graphics should continue blocked S5 feedback, seed HUD notification, route milestone prompts, ChapterEndMenu affordance/click behavior, and post-route S5 handoff.

## 20260511_0453 Global Diagnostic Suppression Build

Graphics rejected 0440 because the local world-band dump showed only ground plus label, while the player-facing screenshot still showed a bright raised oval. This pass changes diagnosis from first-objective world-band suppression to global scene-renderer suppression plus an active-camera frustum dump at the exact first-objective smoke moment.

Implementation delta:

- `TimeWindowDiorama`
  - Adds `suppressChapter1FirstObjectiveDiagnosticVisuals`.
  - The Chapter 1 portal prefab now enables this flag.
  - Runtime clone content and veil renderers intersecting the first-objective diagnostic band are suppressed.
- `AnemoraChapter1SceneSetup`
  - First-objective diagnostic smoke now disables every scene `Renderer` except:
    - player-root renderers
    - `Root_Current/Chapter1_Route_Current/Current_Ground_Library`
    - `Chapter1_PlayableMilestones/Milestone_S1_TimeWindow_Book_Label`
  - `Current_Ground_Library` is a thin neutral matte debug ground.
  - Scene setup reported:
    - `Suppressed 295 first-objective S1/B7 library renderers for playable smoke.`
    - `First-objective global diagnostic smoke view: kept 4 renderer(s), suppressed 1612 scene renderer(s).`
- `Chapter1PlayableFlowControllerTests`
  - Writes an active-camera frustum dump including enabled/disabled `Renderer` entries and `CanvasRenderer` / world-space UI entries.
  - First-objective smoke guard still fails if any diagnostic-band renderer remains other than current ground or the label.

Renderer / frustum evidence:

- Base dump: `<temp>\anemora_ch1_first_objective_renderers_base_20260511_045124.txt`
- Runtime clone dump: `<temp>\anemora_ch1_first_objective_renderers_runtime_20260511_045125.txt`
- With-player dump: `<temp>\anemora_ch1_first_objective_renderers_with_player_20260511_045125.txt`
- Active camera frustum dump: `<temp>\anemora_ch1_first_objective_active_camera_frustum_20260511_045125.txt`
- Enabled renderers visible in active camera:
  - `Player/Player_Visual_Past`
  - `Player/Player_Visual_Current`
  - `Root_Current/Chapter1_Route_Current/Current_Ground_Library`
  - `Chapter1_PlayableMilestones/Milestone_S1_TimeWindow_Book_Label`

Current user-checkable build:

- EXE: `<temp>\anemora_ch1_playable_globalsupp_20260511_0453\Anemora_Chapter1.exe`
- Build log: `<temp>\anemora_ch1_playable_globalsupp_build_20260511_0453.log`
- Build result: succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_globalsupp_player_launch_smoke_20260511_0453.log`
  - No ObjectiveText / NotificationText / TMP font / unsupported glyph / known mojibake-pattern / exception / error matches observed.

Validation:

- Scene setup: `<temp>\anemora_ch1_globalsupp_scene_setup_20260511.log`
- Runtime validator: `<temp>\anemora_ch1_globalsupp_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`
  - Remaining warnings are accepted S4D / S4G placement deltas.
- Targeted PlayMode:
  - `<temp>\anemora_ch1_globalsupp_firstobjective_playmode_20260511.xml`: 1 / 1 passed.
  - `<temp>\anemora_ch1_globalsupp_flow_playmode_20260511.xml`: 8 / 8 passed.

Next:

1. Route 0453 to graphics as the current narrow playable smoke target.
2. First check: if the raised oval/ring still appears, it is not coming from ordinary scene renderers or TimeWindow clone content/veil in the active-camera dump; likely suspects become player placeholder/contact rendering, UI/canvas artifact, camera/post-process, or a renderer class not covered by Unity `Renderer`.
3. If the first-objective visual passes, graphics continues blocked S5 feedback, seed HUD notification, route prompts, ChapterEndMenu affordance/click behavior, and post-route handoff smoke.

## 20260511_0510 Near-Normal Fountain-Culprit Suppression Build

Graphics accepted the first-objective visual in `20260511_0453`, but that build globally suppressed too many scene renderers for broader playable smoke. This pass keeps the playable route near-normal again and suppresses only the renderer group that matched the previous raised oval/ring artifact.

Culprit interpretation:

- Comparing the global suppression pass with active-camera/frustum evidence narrowed the ring-like artifact to the current plaza fountain, not the first objective cue stack or Time Window trace.
- The likely culprit is `Root_Current/Chapter1_Route_Current/Ch1_Current_Plaza_Fountain/Plaza_Fountain_Dry_Broken_Model`.
- The object projected into the lower route view and visually matched graphics' repeated description: grey-green, raised, tilted oval/ring-like form.

Implementation delta:

- `AnemoraChapter1SceneSetup`
  - Removes the previous global diagnostic smoke view from the active setup path.
  - Removes the broad first-objective S1/B7 clutter suppression from the active setup path.
  - Suppresses only current plaza fountain renderers whose hierarchy path contains `Ch1_Current_Plaza_Fountain` or `Plaza_Fountain_Dry_Broken`.
  - Scene setup log reports: `Suppressed 1 current plaza fountain renderer(s) for playable-first smoke; previous graphics smoke read this ring-like fountain as the first-objective Time Window.`
- `TimeWindowDiorama`
  - Thin `TimeWindow_ThinFloorTrace_Footprint` generation is restored.
  - Chapter 1 clone content / vertical veil suppression remains limited to the first-objective diagnostic band via the prefab flag, but the gameplay floor trace is visible again.
- `Chapter1PlayableFlowControllerTests`
  - First-objective visual guard now asserts no enabled plaza-fountain renderer remains visible.
  - Active-camera frustum dump remains available for graphics/debug follow-up.

Current user-checkable build:

- EXE: `<temp>\anemora_ch1_playable_fountainoff_20260511_0510\Anemora_Chapter1.exe`
- Build log: `<temp>\anemora_ch1_playable_fountainoff_build_20260511_0510.log`
  - Succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_fountainoff_player_launch_smoke_20260511_0510.log`
  - No ObjectiveText / NotificationText / TMP font / unsupported glyph / known mojibake-pattern / exception / error matches.

Validation:

- Scene setup: `<temp>\anemora_ch1_fountainonly_scene_setup_20260511_0506.log`
- Runtime validator: `<temp>\anemora_ch1_fountainonly_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`
  - Only accepted S4D / S4G placement warnings remain.
- Targeted PlayMode:
  - `<temp>\anemora_ch1_fountainonly_firstobjective_playmode_20260511.xml`: 1 / 1 passed.
  - `<temp>\anemora_ch1_fountainonly_flow_playmode_20260511.xml`: 8 / 8 passed.
  - `<temp>\anemora_ch1_fountainonly_transition_playmode_20260511.xml`: 5 / 5 passed, including scene-wired ChapterEndMenu visibility, default button selection, Continue fallback notification, Main Menu fallback notification, save failure recovery, and side-view camera restore.
- Active-camera frustum dump:
  - `<temp>\anemora_ch1_first_objective_active_camera_frustum_20260511_050859.txt`
  - No enabled `Ch1_Current_Plaza_Fountain` / `Plaza_Fountain_Dry_Broken` renderer appears in the active frustum list.

Graphics routing request:

1. Treat `20260511_0510` as the new current narrow playable smoke target.
2. First verify first-objective no-ring regression in this near-normal build.
3. If pass, continue blocked S5 feedback, seed HUD notification, route milestone prompt after successful interaction, ChapterEndMenu readability/button affordance, and post-route S5 handoff smoke.

## 20260511_0528 Route Input Stability Follow-Up

Graphics verified the `20260511_0510` near-normal build and cleared the first-objective Time Window / book-area visual blocker. The remaining visual/manual smoke gaps are route prompt advancement after a successful interaction, seed HUD notifications, blocked S5 feedback, ChapterEndMenu readability/button affordance, and post-route S5 handoff.

This follow-up keeps the 0510 visual state and makes milestone interaction less brittle for manual/scripted smoke:

- `AnemoraChapter1SceneSetup`
  - Adds `PlayableMilestoneInteractionRange = 2.6f`.
  - Scene milestone interactables now use that range instead of `1.7f`.
  - The current plaza fountain suppression remains active.
- `Chapter1PlayableFlowControllerTests`
  - Asserts every scene milestone interaction range is at least `2.5f`.
  - Places the player near `Milestone_S1_TimeWindow_Book` inside the new range and verifies the book interaction prompt appears in `CurrentObjectiveText`.

Current EXE:

- `<temp>\anemora_ch1_playable_routeinput_20260511_0528\Anemora_Chapter1.exe`

Validation:

- Scene setup: `<temp>\anemora_ch1_routeinput_scene_setup_20260511_0520.log`
  - `Suppressed 1 current plaza fountain renderer(s) for playable-first smoke; previous graphics smoke read this ring-like fountain as the first-objective Time Window.`
- Route PlayMode: `<temp>\anemora_ch1_routeinput_flow_playmode_20260511.xml`
  - 8 / 8 passed.
- Transition PlayMode: `<temp>\anemora_ch1_routeinput_transition_playmode_20260511.xml`
  - 5 / 5 passed.
- Runtime validator: `<temp>\anemora_ch1_routeinput_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`
  - Only accepted S4D / S4G warnings remain.
- Build log: `<temp>\anemora_ch1_playable_routeinput_build_20260511_0528.log`
  - Succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_routeinput_player_launch_smoke_20260511_0528.log`
  - Clear: no ObjectiveText / NotificationText / TMP font / unsupported glyph / mojibake / exception / error matches.

Graphics routing request:

1. Treat `20260511_0528` as the current near-normal playable smoke target, superseding `20260511_0510`.
2. First verify that the first-objective no-ring result is preserved.
3. Then continue manual/narrow smoke for route milestone prompt after successful interaction, seed HUD receive/deliver notification, blocked S5 feedback, ChapterEndMenu readability/button affordance, and post-route S5 handoff.

## 20260511_0545 Time Window Interaction Gate

Graphics completed the `20260511_0528` narrow playable smoke: first-objective no-ring regression passed, objective/control text was readable, and no graphics source issue was found. Broader visual/manual sign-off remains pending because automated graphics input did not capture route advancement.

This follow-up keeps the 0528 visual state and tightens the route semantics toward Time Window v3.2:

- `Chapter1MilestoneInteractable`
  - Adds `requirePlayerInsideTimeWindow`.
  - For Time Window milestones, E / Space now fails with `ときのまどの なかで しらべる` unless an open `TimeWindowDiorama` contains the player.
  - The prompt also switches to that message while the player is near the milestone but outside the active window range.
- `AnemoraChapter1SceneSetup`
  - Marks `Milestone_S1_TimeWindow_Book` and `Milestone_S4_TimeWindow_SpiceJar` as Time Window-gated.
  - Seed receive / deliver milestones remain normal route interactions.
- `Chapter1PlayableFlowControllerTests`
  - Opens a quick local Time Window over the book and spice milestones before asserting route advancement.
  - Verifies the book milestone does not advance outside the active Time Window range.

Current EXE:

- `<temp>\anemora_ch1_playable_timewindowgate_20260511_0545\Anemora_Chapter1.exe`

Validation:

- Scene setup: `<temp>\anemora_ch1_timewindow_gate_scene_setup_20260511.log`
  - Scene setup completed.
  - Fountain suppression log is present.
- Route PlayMode: `<temp>\anemora_ch1_timewindow_gate_flow_playmode_20260511.xml`
  - 8 / 8 passed.
  - Covers Time Window-gated book and spice milestones.
- Transition PlayMode: `<temp>\anemora_ch1_timewindow_gate_transition_playmode_20260511.xml`
  - 5 / 5 passed.
  - The expected save-failure-path `Illegal characters in path` exception appears in the log, but Test Runner exits with code 0.
- Full PlayMode: `<temp>\anemora_ch1_timewindow_gate_full_playmode_20260511.xml`
  - 84 / 84 passed.
  - The same expected save-failure-path exception appears in the log; Test Runner exits with code 0.
- Runtime validator: `<temp>\anemora_ch1_timewindow_gate_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`
  - Only accepted S4D / S4G warnings remain.
- Build log: `<temp>\anemora_ch1_playable_timewindowgate_build_20260511_0545.log`
  - Succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_timewindowgate_player_launch_smoke_20260511_0545.log`
  - Clear: no ObjectiveText / NotificationText / TMP font / unsupported glyph / mojibake / exception / error matches.

Graphics routing request:

1. Treat `20260511_0545` as the current playable smoke target, superseding `20260511_0528`.
2. Verify first-objective no-ring remains cleared.
3. Verify route prompt advancement now requires an active Time Window for book/spice.
4. Continue seed HUD, blocked S5 feedback, ChapterEndMenu readability/button affordance, and post-route S5 handoff smoke.

## 20260511_0552 Active Time Window Floor-Trace-Only Follow-Up

Graphics completed `20260511_0545` narrow playable smoke:

- first-objective no-ring regression before opening Time Window: pass
- active Time Window visual after `F`: blocker
- culprit read: large pale-blue cloned past-geometry panels / vertical slabs, not the v3.2 thin low floor/range trace

Implementation follow-up:

- `TimeWindowDiorama`
  - Added `chapter1FloorTraceOnlyMode`.
  - In `Anemora_Chapter1` only, the local window now renders the low footprint trace without cloning world-aligned past content and without hiding/replacing current-space renderers.
  - Other scenes keep the prior clone/replace behavior.
- `AnemoraChapter1SceneSetup`
  - Enables `chapter1FloorTraceOnlyMode` on the Chapter 1 `TimeWindow_Diorama` prefab during setup.
- `Chapter1PlayableFlowControllerTests`
  - Guards that Chapter 1 quick Time Window has floor-trace-only mode enabled and spawns zero runtime content clones.
  - Keeps the frame/vertical-veil suppression guard.

Current user-checkable EXE:

- `<temp>\anemora_ch1_playable_floortraceonly_20260511_0552\Anemora_Chapter1.exe`

Validation:

- Scene setup: `<temp>\anemora_ch1_floortraceonly_scene_setup_20260511.log`
  - completed, fountain suppression log present
- Route PlayMode: `<temp>\anemora_ch1_floortraceonly_flow_playmode_20260511.xml`
  - 8 / 8 passed
- Demo PlayMode regression: `<temp>\anemora_ch1_floortraceonly_demo_playmode_20260511.xml`
  - 2 / 2 passed
  - confirms non-Chapter1 local Time Window clone/replace behavior remains intact
- Full PlayMode: `<temp>\anemora_ch1_floortraceonly_full_playmode_20260511.xml`
  - 84 / 84 passed
- Runtime validator: `<temp>\anemora_ch1_floortraceonly_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`
  - warnings are accepted S4D / S4G deltas only
- Build log: `<temp>\anemora_ch1_playable_floortraceonly_build_20260511_0552.log`
  - succeeded, warnings=0, errors=0
- Player launch smoke: `<temp>\anemora_ch1_playable_floortraceonly_player_launch_smoke_20260511_0552.log`
  - no ObjectiveText / NotificationText / TMP font / unsupported glyph / mojibake / exception / error matches
- `git diff --check` on touched files: pass
- staged files: none

Next routing:

- Route `20260511_0552` to graphics as current playable smoke target, superseding `20260511_0545`.
- Ask graphics to verify before-open no-ring state, outside-window gate feedback, after-open floor trace, successful inside-window interaction, seed HUD, blocked S5 feedback, ChapterEndMenu, and post-route S5 handoff.

## 20260511_0606 Japanese Time Window Hint Follow-Up

Graphics signed off the `20260511_0552` floor-trace-only Time Window visual for the first-objective smoke. The remaining gaps are manual/targeted route evidence, not graphics source blockers.

This follow-up improves the player-facing Time Window controls shown during that route:

- `Chapter1UiText`
  - Adds Chapter 1 Japanese brush hint text for create / release / close states.
- `AnemoraChapter1SceneSetup`
  - Applies those hints to the Chapter 1 `TimeFramePortalController`.
- `TimeFramePortalController`
  - Resolves the brush-hint font from Japanese-capable OS fonts before falling back to built-in fonts.
- `Chapter1PlayableFlowControllerTests`
  - Guards that Chapter 1 no longer shows `draw time window` / `Right-click` in the brush hint.
  - Confirms the Chapter 1 hint contains `ときのまど` and the close state contains `とじる`.

Current user-checkable EXE:

- `<temp>\anemora_ch1_playable_jphints_20260511_0606\Anemora_Chapter1.exe`

Validation:

- Scene setup: `<temp>\anemora_ch1_jphints_scene_setup_20260511.log`
  - completed, fountain suppression log present
- Route PlayMode: `<temp>\anemora_ch1_jphints_flow_playmode_20260511.xml`
  - 8 / 8 passed
- Demo PlayMode regression: `<temp>\anemora_ch1_jphints_demo_playmode_20260511.xml`
  - 2 / 2 passed
- Runtime validator: `<temp>\anemora_ch1_jphints_runtime_validator_retry_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`
  - warnings are accepted S4D / S4G deltas only
- Build log: `<temp>\anemora_ch1_playable_jphints_build_20260511_0606.log`
  - succeeded, warnings=0, errors=0
- Player launch smoke: `<temp>\anemora_ch1_playable_jphints_player_launch_smoke_20260511_0606.log`
  - no ObjectiveText / NotificationText / TMP font / unsupported glyph / mojibake / exception / error matches

Next:

- Use `20260511_0606` as the current user-checkable playable EXE unless manual smoke finds a regression.
- Remaining evidence targets are outside-window feedback in player view, successful inside-window route prompt advance, seed HUD, blocked S5 feedback, ChapterEndMenu readability/click behavior, and post-route S5 handoff.

## 20260511_0612 Quick-Place Window Includes Player

Graphics and automated smoke repeatedly reached the first-objective view but did not capture route advancement. The likely usability issue was that quick-place `F` opened the Time Window too far in front of the player, so the player could visually open a window while still being outside the gate volume.

Implementation follow-up:

- `TimeFramePortalController`
  - Quick-place and default local-window placement now clamp the center offset so the spawned window includes the player instead of placing the full range ahead of them.
  - This keeps manual `F` usable for the Chapter 1 route while still allowing drag-defined windows.
- `Chapter1PlayableFlowControllerTests`
  - Guards that after quick-place, the spawned `TimeWindowDiorama` contains the player position.

Current user-checkable EXE:

- `<temp>\anemora_ch1_playable_quickinside_20260511_0612\Anemora_Chapter1.exe`

Validation:

- Scene setup: `<temp>\anemora_ch1_quickinside_scene_setup_20260511.log`
  - completed, fountain suppression log present
- Route PlayMode: `<temp>\anemora_ch1_quickinside_flow_playmode_20260511.xml`
  - 8 / 8 passed
- Demo PlayMode regression: `<temp>\anemora_ch1_quickinside_demo_playmode_20260511.xml`
  - 2 / 2 passed
- Runtime validator: `<temp>\anemora_ch1_quickinside_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`
  - warnings are accepted S4D / S4G deltas only
- Build log: `<temp>\anemora_ch1_playable_quickinside_build_20260511_0612.log`
  - succeeded, warnings=0, errors=0
- Player launch smoke: `<temp>\anemora_ch1_playable_quickinside_player_launch_smoke_20260511_0612.log`
  - no ObjectiveText / NotificationText / TMP font / unsupported glyph / mojibake / exception / error matches

Next:

- Use `20260511_0612` as the current user-checkable playable EXE.
- Manual smoke should be able to stand near the first book objective, press `F`, then press `E` without needing an extra reposition just to enter the window volume.

## 20260511_0621 Player-Facing Route Transcript

Added a PlayMode smoke transcript for the current playable route evidence without changing production runtime behavior.

- Test: `Chapter1PlayableFlowControllerTests.PlayerFacingRouteSmokeWritesTranscript`
- Output transcript: `<temp>\anemora_ch1_quickinside_route_transcript_20260511_063055.md`
- Route PlayMode with transcript: `<temp>\anemora_ch1_quickinside_flow_transcript_playmode_20260511_rerun.xml`
  - 9 / 9 passed.
- Transition PlayMode rerun: `<temp>\anemora_ch1_quickinside_transition_playmode_20260511.xml`
  - 5 / 5 passed.
  - Log includes the expected save-failure-path `Illegal characters in path` exception from the no-soft-lock test; Test Runner exited code 0.
- Full PlayMode rerun: `<temp>\anemora_ch1_quickinside_full_playmode_20260511.xml`
  - 85 / 85 passed.
- ChapterEndMenu button guard update: `<temp>\anemora_ch1_quickinside_transition_menu_guard_20260511.xml`
  - 5 / 5 passed.
  - Adds direct checks for Continue / Title / Quit button labels and interactable state.
- Full PlayMode after menu guard: `<temp>\anemora_ch1_quickinside_full_playmode_after_menu_guard_20260511.xml`
  - 85 / 85 passed.

Transcript coverage:

- Initial objective text.
- Early S5 blocked feedback.
- Book interaction blocked outside Time Window.
- Book reflection after Time Window open.
- Seed receive / deliver notification and HUD carrying state.
- Route-ready objective after Scene 4 trace.
- ChapterEndMenu visible, chapter title text, and save flags written.

## 20260511_0645 Prologue Elder Guide

Added the minimal A-4 prologue guidance as playable route support, without opening a new graphics polish loop.

Implementation follow-up:

- `AnemoraChapter1DialogueSetup`
  - Adds `Mob_Resident_B_Prologue_E_ElderGuide.asset`.
  - Lines use the elder's abstract central-plaza guide wording, do not name Reto, and do not add Eluthria references.
  - Includes Niro's inner text after the north-light line: `(...あそこに、誰か)`.
- `AnemoraChapter1SceneSetup`
  - Adds `Chapter1_Prologue_E_ElderGuide` under `Root_Current`.
  - Places a bench, placeholder elder, small north-point cue, and a non-freezing `DialogueProximityTrigger` with 3m radius.
  - Uses `showDialoguePanel=true`, `freezePlayerOnDialogue=false`, `oneShot=true`, and `pastNpcOverheardOnly=false`.
- `Chapter1PlayableFlowControllerTests`
  - Guards that the elder guide is present, enabled, non-freezing, one-shot, current-side, and wired to its DialogueAsset.

Current user-checkable EXE:

- `<temp>\anemora_ch1_playable_elderguide_20260511_0645\Anemora_Chapter1.exe`

Validation:

- Scene setup: `<temp>\anemora_ch1_elderguide_scene_setup_20260511.log`
  - completed successfully.
- Targeted PlayMode: `<temp>\anemora_ch1_elderguide_flow_playmode_20260511.xml`
  - 10 / 10 passed.
- Full PlayMode: `<temp>\anemora_ch1_elderguide_full_playmode_20260511.xml`
  - 86 / 86 passed.
- Runtime validator: `<temp>\anemora_ch1_elderguide_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`.
  - warnings remain accepted S4D / S4G deltas only.
- Build log: `<temp>\anemora_ch1_playable_elderguide_build_20260511_0645.log`
  - succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_elderguide_player_launch_smoke_20260511_0645.log`
  - no ObjectiveText / NotificationText / TMP font / unsupported glyph / mojibake / exception / error matches.

Next:

- Use `20260511_0645` as the current user-checkable playable EXE.
- Continue playable-first with route/menu evidence and only small route-support additions. Defer optional A-4 motion polish and character art.

## 20260511_0657 Seed HUD Playability Guard

Tightened the seed-bag HUD as route feedback rather than a visual-polish loop.

Implementation follow-up:

- `Chapter1SeedBagHudController`
  - Uses a small 24 x 24 bottom-right seed bag icon.
  - Keeps receive/deliver notifications readable and non-blocking.
  - Exposes lightweight state properties for PlayMode route smoke.
- `Chapter1PlayableFlowControllerTests`
  - Guards that the seed HUD icon is bottom-right, 24 x 24, non-interactive, and does not block raycasts.
  - Guards receive/deliver notification text and icon visibility state.

Current user-checkable EXE:

- `<temp>\anemora_ch1_playable_seedhud_20260511_0657\Anemora_Chapter1.exe`

Validation:

- Targeted PlayMode: `<temp>\anemora_ch1_seedhud_flow_playmode_20260511.xml`
  - 11 / 11 passed.
- Full PlayMode: `<temp>\anemora_ch1_seedhud_full_playmode_20260511.xml`
  - 87 / 87 passed.
- Runtime validator: `<temp>\anemora_ch1_seedhud_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`.
- Build log: `<temp>\anemora_ch1_playable_seedhud_build_20260511_0657.log`
  - succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_seedhud_player_launch_smoke_20260511_0657.log`
  - no ObjectiveText / NotificationText / TMP font / unsupported glyph / mojibake / exception / error matches.

Next:

- Use `20260511_0657` as the current user-checkable playable EXE.
- Continue playable-first; route/menu behavior remains the priority, and graphics/runtime should re-enter only on concrete blockers.

## 20260511_0705 Chapter-End ESC Skip

Added a no-soft-lock chapter-end skip path for the playable route close.

Implementation follow-up:

- `ChapterTransitionController`
  - `Esc` during the Chapter 1 close sequence now requests a skip to the chapter-end menu.
  - The skip shortens turn-back/pan/kick/fade/title waits but still runs autosave and reaches `ChapterEndMenu`.
  - Same-scene save failure cleanup continues to clear the skip request.
- `ChapterTransitionControllerPlayModeTests`
  - Guards that a skip request during the close sequence reaches the end menu quickly, keeps the menu interactable, writes the save file, and still writes the pebble action record.

Current user-checkable EXE:

- `<temp>\anemora_ch1_playable_escskip_20260511_0705\Anemora_Chapter1.exe`

Validation:

- Targeted ChapterTransition PlayMode: `<temp>\anemora_ch1_escskip_transition_playmode_20260511.xml`
  - 6 / 6 passed.
- Full PlayMode: `<temp>\anemora_ch1_escskip_full_playmode_20260511.xml`
  - 88 / 88 passed.
- Runtime validator: `<temp>\anemora_ch1_escskip_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`.
- Build log: `<temp>\anemora_ch1_playable_escskip_build_20260511_0705.log`
  - succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_escskip_player_launch_smoke_20260511_0705.log`
  - no ObjectiveText / NotificationText / TMP font / unsupported glyph / mojibake / exception / error matches.

Next:

- Use `20260511_0705` as the current user-checkable playable EXE.
- Manual smoke should check route completion, then press `Esc` during the closing sequence and confirm the chapter-end menu appears with save status.

## 20260511_0711 ChapterEndMenu Keyboard Operation

Added keyboard operation guards for the chapter-end menu so it is not mouse-only.

Implementation follow-up:

- `ChapterTransitionController`
  - While `ChapterEndMenu` is visible, `Tab` / arrow keys move selection across Continue / Title / Quit.
  - `Enter` / keypad Enter / Space activates the selected button.
  - `Esc` on the menu follows the title/main-menu route, with the existing fallback notification when Title is unavailable in the review build.
- `ChapterTransitionControllerPlayModeTests`
  - Guards selection wrap, selected-button activation, and menu `Esc` fallback behavior on the scene-wired Chapter 1 transition.

Current user-checkable EXE:

- `<temp>\anemora_ch1_playable_menukeyboard_20260511_0711\Anemora_Chapter1.exe`

Validation:

- Targeted ChapterTransition PlayMode: `<temp>\anemora_ch1_menukeyboard_transition_playmode_20260511.xml`
  - 6 / 6 passed.
- Full PlayMode: `<temp>\anemora_ch1_menukeyboard_full_playmode_20260511.xml`
  - 88 / 88 passed.
- Runtime validator: `<temp>\anemora_ch1_menukeyboard_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`.
- Build log: `<temp>\anemora_ch1_playable_menukeyboard_build_20260511_0711.log`
  - succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_menukeyboard_player_launch_smoke_20260511_0711.log`
  - no ObjectiveText / NotificationText / TMP font / unsupported glyph / mojibake / exception / error matches.

Next:

- Use `20260511_0711` as the current user-checkable playable EXE.
- Manual smoke should complete the route, reach `ChapterEndMenu`, and verify Tab/arrow selection plus Enter/Space activation.

## 20260511_0719 A-4 Elder Guide Text Guard

Added a lightweight regression guard for the prologue elder guide text without changing production runtime behavior.

Changed:

- `Assets/Tests/EditMode/Chapter1DialogueAssetTests.cs`
  - Includes the A-4 elder guide keys in the Chapter 1 localization coverage set.
  - Guards `Mob_Resident_B_Prologue_E_ElderGuide.asset` turn order: elder lines 1-3, Niro inner text, elder line 4.
  - Guards the Japanese Niro inner text resolves exactly as `(...あそこに、誰か)`.

Validation:

- Targeted EditMode: `<temp>\anemora_ch1_elderguide_dialogue_editmode_20260511.xml`
  - 10 / 10 passed.

Next:

- Current user-checkable EXE remains `20260511_0711`; no rebuild was needed because this pass only adds test coverage.

## 20260511_0733 C-1-beta Dario Signboard Minimal Playable Pass

Added the optional Scene 3 street-corner signboard puzzle as a minimal playable side interaction, without making it part of the required Chapter 1 route.

Changed:

- `AnemoraChapter1DialogueSetup`
  - Adds Dario's fourth Scene 3 market monologue line: `うちの看板、商売の証だ。長く保ってくれよ`.
- `AnemoraChapter1SceneSetup`
  - Adds a current unreadable signboard, a past readable `香料屋ダリオ` signboard, and an inactive current faint-letter trace.
  - Adds `Milestone_S3_DarioSignboard` as a label-only optional marker.
  - Requires Time Window state before the current signboard can be traced.
- `Chapter1PlayableFlowController`
  - Adds `progression.chapter1.signboard_revealed`.
- `Chapter1UiText`
  - Adds signboard prompt/completion/missing-Time-Window feedback strings.
- `Chapter1DialogueAssetTests`
  - Guards the added Dario monologue localization key and asset turn order.
- `Chapter1PlayableFlowControllerTests`
  - Guards the signboard interaction path: blocked outside Time Window, succeeds inside Time Window, writes `progression.chapter1.signboard_revealed`, records `reveal_dario_signboard_001`, activates the faint trace, and bridges to the chapter-end save flag `chapter1_signboard_revealed`.

Current user-checkable EXE:

- `<temp>\anemora_ch1_playable_signboard_20260511_0733\Anemora_Chapter1.exe`

Validation:

- Targeted dialogue EditMode: `<temp>\anemora_ch1_signboard_dialogue_editmode_20260511.xml`
  - 10 / 10 passed.
- Targeted playable-flow PlayMode: `<temp>\anemora_ch1_signboard_flow_playmode_20260511_rerun.xml`
  - 12 / 12 passed.
- Full PlayMode: `<temp>\anemora_ch1_signboard_full_playmode_20260511.xml`
  - 89 / 89 passed.
- Runtime validator: `<temp>\anemora_ch1_signboard_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`.
- Build log: `<temp>\anemora_ch1_playable_signboard_build_20260511_0733.log`
  - succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_signboard_player_launch_smoke_20260511_0733.log`
  - no ObjectiveText / NotificationText / TMP font / unsupported glyph / mojibake / exception / error matches.

Next:

- Treat `20260511_0733` as the current playable-first build.
- Manual smoke should complete the primary route, optionally open the Scene 3 Time Window and trace the signboard, then confirm ChapterEndMenu keyboard operation still works.

## 20260511_0834 Time Window Volume Cue / Scale / LogicalMap Integration

Integrated the graphics Time Window v3.2 volume cue package and the runtime LogicalMap support package into the implementation worktree.

Changed:

- `TimeWindow_Diorama` now instantiates `Ch1_TimeWindowV32_VolumeCue` for Chapter 1 mode when the package prefab is assigned.
  - The active cue is a source-prefab volume/range presentation: floor footprint, broken vertical ticks, thin wisps, and height glints.
  - It does not use cloned past content, broad pale panels/slabs, portal/ring/arch/frame meshes, or the previous floor-only primitive as the primary Chapter 1 cue.
- `AnemoraChapter1SceneSetup` wires the volume cue prefab into `Assets/Prefabs/Portal/TimeWindow_Diorama.prefab`.
- Niro placeholder visual scale was reduced from `0.78` to `0.60` as a visual-only correction.
  - Player root, collider, movement, Time Window membership, milestone interaction ranges, and DialogueProximityTrigger ranges are preserved.
- Runtime `LogicalMap*` support was imported for the next switch-map direction:
  - `LogicalMapSegmentRoot`
  - `LogicalMapSpawnPoint`
  - `LogicalMapSegmentSwitchContext`
  - `LogicalMapSegmentSwitcher`
  - `LogicalMapSegmentSwitchTrigger`
  - `LogicalMapSegmentSwitcherTests`
- Existing `Chapter1AreaSwitchGate` vertical slice remains in place as the current Chapter 1 proof: one Unity scene, fade/spawn/camera handoff, Time Window close-on-switch, and route state retention.

Current user-checkable EXE:

- `<temp>\anemora_ch1_playable_twvolume_logicalmap_20260511_0834\Anemora_Chapter1.exe`

Validation:

- Targeted PlayMode: `<temp>\anemora_ch1_twcue_logicalmap_playmode_20260511.xml`
  - 20 / 20 passed.
  - Includes Time Window volume cue guards, Chapter 1 area switch guards, and `LogicalMapSegmentSwitcherTests` 4 / 4.
- Full PlayMode: `<temp>\anemora_ch1_twcue_logicalmap_full_playmode_20260511.xml`
  - 94 / 94 passed.
- Runtime validator: `<temp>\anemora_ch1_twcue_logicalmap_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`.
  - Warnings remain the accepted S4D/S4G placement deltas only.
- Build log: `<temp>\anemora_ch1_playable_twvolume_logicalmap_build_20260511_0834.log`
  - succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_twvolume_logicalmap_launch_20260511_0834.log`
  - no ObjectiveText / NotificationText / TMP font / unsupported glyph / mojibake / exception / error matches.

Next:

- Send the current EXE to graphics for concrete validation of the corrected Time Window active volume cue and the 0.60 Niro/map scale read.
- Keep runtime support focused on switch-map contract/tests unless validator/test regressions appear.
- The separate implementation-scale-map and implementation-switch-map sessions should avoid reimplementing this same baseline; useful follow-up is review, conflict detection, or targeted refinements on top of this integration.

## 20260511_0858 Time Window Volume Material Copy Fix

Fixed the graphics-reported 0834 magenta Time Window blocker. The implementation worktree had the v3.2 volume cue prefab and manifest, but `Assets/Art/Materials/Zone1/Chapter1TimeWindow/` was empty.

Changed:

- Copied the full `Assets/Art/Materials/Zone1/Chapter1TimeWindow/` package from the graphics worktree into implementation, including `.mat` and `.meta` files.
- Reran Unity import and Chapter 1 scene setup.
- Ran a manifest material guard against `chapter1_time_window_v32_volume_cue_manifest.json`.
  - Result: 7 / 7 listed material paths and `.meta` files found in implementation.

Current user-checkable EXE:

- `<temp>\anemora_ch1_playable_twvolume_materialfix_20260511_0858\Anemora_Chapter1.exe`

Validation:

- Manifest material guard:
  - passed, 7 / 7 materials and metas found.
- Unity import smoke: `<temp>\anemora_ch1_twvolume_material_import_20260511.log`
  - Tundra build success, no script compile errors.
- Scene setup: `<temp>\anemora_ch1_twvolume_material_scene_setup_20260511.log`
  - completed for `Assets/Scenes/Anemora_Chapter1.unity`.
- Targeted PlayMode: `<temp>\anemora_ch1_twvolume_material_playmode_20260511.xml`
  - 21 / 21 passed.
- Runtime validator: `<temp>\anemora_ch1_twvolume_material_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`.
  - Warnings remain accepted S4D/S4G placement deltas only.
- Build log: `<temp>\anemora_ch1_playable_twvolume_materialfix_build_20260511_0858.log`
  - succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_twvolume_materialfix_launch_20260511_0858.log`
  - no ObjectiveText / NotificationText / TMP font / unsupported glyph / mojibake / exception / error / material-missing patterns.

Next:

- Route `20260511_0858` to graphics for Time Window active volume material/color validation.
- Character v12 minimal four-direction set is now user-approved, but not imported yet. Treat Niro replacement as the next scoped implementation task after the Time Window material blocker is cleared.

## 20260511_0915 Niro v12 Scoped Runtime Import

Imported only the user-approved v12 Niro four-direction stills as the playable hero replacement baseline. The broader v13 minimum character pack remains review-only and was not imported.

Changed:

- Added approved Niro v12 stills under `Assets/Art/Sprites/Hero/v12/`.
- Added `AnemoraChapter1CharacterV12Importer` to configure those PNGs as Unity sprites and wire `Hero.prefab`.
- Updated `Assets/Prefabs/Characters/Hero.prefab`:
  - SpriteRenderer default sprite now uses the v12 front still.
  - `HeroAnimatorBinder` idle/walk front, side, and back arrays now reference v12 stills.
  - Left movement continues to use the binder's `flipX` path from the right-facing still.
- Added a Chapter 1 scene-load guard that `Player_Visual_Current` and `Player_Visual_Past` use the v12 Niro front texture while preserving the 0.60 scale.

Current user-checkable EXE:

- `<temp>\anemora_ch1_playable_niro_v12_20260511_0915\Anemora_Chapter1.exe`

Validation:

- v12 importer: `<temp>\anemora_ch1_niro_v12_import_20260511.log`
  - applied approved Niro v12 four-direction stills to `Assets/Prefabs/Characters/Hero.prefab`.
- Targeted PlayMode: `<temp>\anemora_ch1_niro_v12_playmode_20260511.xml`
  - 6 / 6 passed.
  - Covers Chapter 1 scene player visual v12 texture/scale and HeroAnimatorBinder facing behavior.
- Runtime validator: `<temp>\anemora_ch1_niro_v12_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`.
- Build log: `<temp>\anemora_ch1_playable_niro_v12_build_20260511_0915.log`
  - succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_niro_v12_launch_20260511_0915.log`
  - no ObjectiveText / NotificationText / TMP font / unsupported glyph / mojibake / exception / error / material-missing patterns.

Next:

- Route `20260511_0915` to graphics/character for focused Niro v12 scale/contact/silhouette validation.
- Do not import v13 minimum character pack until explicit user acceptance.

## 20260511_0936 Full Area Switch Spine

Shifted main back to active implementation after the user flagged that the chapter still did not feel like a game. The immediate gap was that logical map switching existed only for Prologue/S1/S2, leaving the rest of Chapter 1 effectively as a seamless inspection space.

Changed:

- Extended `Chapter1_AreaSwitchGates` generation so the primary route can switch across the full Chapter 1 spine:
  - `O_Prologue -> S1_Library`
  - `S1_Library <-> S2_MiaHouse`
  - `S2_MiaHouse <-> S3_StreetAria`
  - `S3_StreetAria <-> S4_KaiaField`
  - `S4_KaiaField <-> S5_NorthRuins`
  - `S1_Library -> O_Prologue`
- Kept the one-Unity-scene, logical-map-switch approach: fade, spawn relocation, camera target update, objective/section label update, Time Window close/reset, and route-state retention.
- Added PlayMode coverage for the full switch spine and updated the stale Niro sprite assertion to v12.

Current user-checkable EXE:

- `<temp>\anemora_ch1_playable_fullareas_20260511_0936\Anemora_Chapter1.exe`

Validation:

- Scene setup: `<temp>\anemora_ch1_fullareas_scene_setup_20260511.log`
  - completed and generated the expanded gate set in `Anemora_Chapter1.unity`.
- Targeted PlayMode: `<temp>\anemora_ch1_fullareas_playmode_rerun_20260511.xml`
  - 22 / 22 passed.
- Runtime validator: `<temp>\anemora_ch1_fullareas_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`.
- Build log: `<temp>\anemora_ch1_playable_fullareas_build_20260511_0936.log`
  - succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_fullareas_launch_20260511_0936.log`
  - no ObjectiveText / NotificationText / TMP font / unsupported glyph / mojibake / exception / error / material-missing patterns.

Next:

- Manual smoke should verify walking into area gates through the route, not only test-driven `SwitchNowForTests`.
- Next active implementation pass should improve player-facing gate affordance/objective pacing and then full route completion from S1 Time Window through S5 end menu on this switched-map baseline.

## 20260511_0956 Manual-Smoke Camera / Gate / NPC Prompt Fix

Addressed the user-facing manual smoke blockers reported after the 0936 build: camera too far, game elements not readable enough, NPCs auto-talking, and switch-map gates not presented as playable exits.

Changed:

- Tightened per-section gameplay cameras to graphics-requested closer framing:
  - O_Prologue 2.40
  - S1_Library 2.60
  - S2_MiaHouse 2.60
  - S3_StreetAria 2.70
  - S4_KaiaField 3.30
  - S5_NorthRuins 3.05
- Area switch gates now have:
  - flat low floor cue
  - directional floor cue
  - target-specific Japanese label
  - `E` / `Space` prompt-then-interact switching instead of auto-switch on enter
  - switch completion feedback in the objective overlay
- `DialogueProximityTrigger` text-panel routes now wait for player interaction:
  - `showDialoguePanel=true` sources require E/Space before opening dialogue
  - `freezePlayerOnDialogue=false` remains preserved
  - ambient/audio-only sources remain auto playback with no panel
- Imported/adapted runtime's prompt switch support into `LogicalMapSegmentSwitchTrigger` while keeping production `Chapter1AreaSwitchGate` as the current scene gate component.

Current user-checkable EXE:

- `<temp>\anemora_ch1_playable_smokefix_20260511_0956\Anemora_Chapter1.exe`

Manual smoke route to verify:

1. Start at Niro house. Confirm camera is close enough to read Niro and local route cues.
2. Walk to the visible area gate. Confirm the Japanese destination label is visible and objective overlay shows an `E` prompt.
3. Press `E` or `Space`. Confirm fade, spawn relocation, updated section/objective label, and no stuck fade.
4. At Scene 1, press `F` to open the Time Window, enter the active range, then interact with the book.
5. Continue through S2/S3/S4/S5 gates using the same prompted gate flow.
6. Confirm NPC dialogue does not open only from walking nearby; it should require the prompt/interact path unless it is ambient/audio-only.

Validation:

- Scene setup: `<temp>\anemora_ch1_smokefix_scene_setup_final_20260511.log`
  - completed.
- Targeted PlayMode: `<temp>\anemora_ch1_smokefix_playmode_final_20260511.xml`
  - 31 / 31 passed.
- Runtime validator: `<temp>\anemora_ch1_smokefix_runtime_validator_final_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`.
- Build log: `<temp>\anemora_ch1_playable_smokefix_build_20260511_0956.log`
  - succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_smokefix_launch_20260511_0956.log`
  - no ObjectiveText / NotificationText / TMP font / unsupported glyph / mojibake / exception / error / material-missing patterns.
- Gameplay camera capture: `docs/devlog/screenshots/chapter1_gameplay_camera/20260511_095358`
  - errors=0, warnings=0, captures=6.

Next:

- Route 0956 to graphics for the narrow camera/gate/NPC prompt smoke.
- Main should continue with manual full-route smoke and only add content after this presentation/interaction pass is accepted.

## 20260511_1018 Gate Label / Direction Cue Polish

Follow-up to graphics 0956 review. Camera sizes stay unchanged as the accepted baseline:

- O_Prologue 2.40
- S1_Library 2.60
- S2_MiaHouse 2.60
- S3_StreetAria 2.70
- S4_KaiaField 3.30
- S5_NorthRuins 3.05

Changed:

- Increased all `AreaSwitch_*` destination label scale to `0.15`.
- Added bold TMP outline plus a dark underlay behind world-space gate labels so the Japanese text reads before the gold plate.
- Replaced the simple direction strip with a low flat directional strip plus chevron pieces, aligned toward each target spawn.
- Kept all gate visuals floor-level: no vertical gate, ring, arch, portal, or raised frame.
- Kept NPC text routes as prompt-then-interact and ambient/audio-only routes as automatic.

Current EXE:

- `<temp>\anemora_ch1_playable_gatepolish_20260511_1018\Anemora_Chapter1.exe`

Validation:

- Scene setup: `<temp>\anemora_ch1_gatepolish_scene_setup_20260511.log`
  - completed.
- Targeted PlayMode: `<temp>\anemora_ch1_gatepolish_playmode_fixed_20260511.xml`
  - 31 / 31 passed.
- Runtime validator: `<temp>\anemora_ch1_gatepolish_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`.
- Build log: `<temp>\anemora_ch1_playable_gatepolish_build_20260511_1018.log`
  - succeeded, warnings=0, errors=0.
- Player launch smoke: `<temp>\anemora_ch1_playable_gatepolish_launch_20260511_1018.log`
  - no ObjectiveText / NotificationText / TMP font / unsupported glyph / mojibake / exception / error / material-missing patterns.
- Gameplay camera capture: `docs/devlog/screenshots/chapter1_gameplay_camera/20260511_101443`
  - errors=0, warnings=0, captures=6.

Graphics validation request:

- Check destination labels in the tighter camera views.
- Check low floor chevrons/arrow strips read as route direction, not portal geometry.
- Capture near-gate `E` / `Space` prompt, NPC proximity prompt without auto-opened panel, and before-switch / fade-mid / after-switch objective update.

## 20260511_1048 House Interior -> Exterior Proof Scope

Scope was narrowed per user direction to prove the first map switch only:

- `O_HouseInterior`: Niro starts inside the house with a close local camera.
- `AreaSwitch_HouseInterior_To_HouseExterior`: door/exit prompt-then-interact gate, no auto-switch.
- `O_HouseExterior`: outside house map with house at the back/upper center, spawn near the door, and a visible northeast road cue toward the central plaza.
- `AreaSwitch_HouseExterior_To_CentralPlaza_Unavailable`: visible future route cue only; it has no `Chapter1AreaSwitchGate` component and is not reachable in this proof.

Time Window production work is frozen for the recovery/prototype decision pass. This proof does not change TimeWindow scripts, prefabs, materials, or scene setup.

Current EXE:

- `<temp>\anemora_ch1_playable_houseproof_20260511_1048\Anemora_Chapter1.exe`

Validation:

- EditMode: `<temp>\anemora_ch1_houseproof_editmode_fixed_20260511.xml`
  - `Chapter1SceneStructureTests` 6 / 6 passed.
- Targeted PlayMode: `<temp>\anemora_ch1_houseproof_playmode_fixed_20260511.xml`
  - `Chapter1SceneLoadSmokeTests`, `DialogueProximityTriggerTests`, `LogicalMapSegmentSwitcherTests` 19 / 19 passed.
- Runtime validator: `<temp>\anemora_ch1_houseproof_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`; warnings remain the accepted S4D/S4G placement deltas.
- Build log: `<temp>\anemora_ch1_playable_houseproof_build_20260511_1048.log`
  - succeeded, warnings=0, errors=0.
- Launch smoke: `<temp>\anemora_ch1_playable_houseproof_launch_20260511_1048.log`
  - no ObjectiveText / NotificationText / TMP font / unsupported glyph / mojibake / exception / error / material-missing patterns.
- Gameplay camera capture: `docs/devlog/screenshots/chapter1_gameplay_camera/20260511_104410`
  - errors=0, warnings=0, captures=2.

Manual smoke route:

1. Launch the EXE and confirm Niro starts inside the house.
2. Walk to the door glow / `外へ` cue.
3. Confirm the objective/prompt shows the exit action and that the gate does not auto-switch.
4. Press `E` or `Space`.
5. Confirm fade, outside spawn near the house door, house framing at the back/upper center, and the northeast road cue toward the plaza.

## 20260511_1120 House Exit / Overlap Blocker Fix

Follow-up to user manual smoke that 1048 could not exit the house and house interior/exterior appeared overlapped.

Root / map state:

- `MapRoot_HouseInterior`: active at scene start.
- `MapRoot_HouseExterior`: inactive at scene start.
- `AreaSwitch_HouseInterior_To_HouseExterior`: prompt-then-interact door gate, not auto-switch.
- After successful switch, `MapRoot_HouseInterior` is inactive and `MapRoot_HouseExterior` is active.
- `AreaSwitch_HouseExterior_To_CentralPlaza_Unavailable`: visible future cue only; no `Chapter1AreaSwitchGate`, plaza/library/S2+ route remains unreachable in this proof.

Changes:

- Integrated graphics house minimum map kit:
  - `Assets/Prefabs/Zone1/Chapter1HouseMinimumMap/`
  - `Assets/Art/Materials/Zone1/Chapter1HouseMinimumMap/`
  - `Assets/Editor/AnemoraChapter1HouseMinimumMapKitBuilder.cs`
- Replaced rough interior/exterior proof visuals with `Chapter1_HouseInterior_MinimumMap` and `Chapter1_HouseExterior_MinimumMap`.
- Kept gameplay colliders/triggers separate from the visual prefabs.
- Moved the house exit trigger to the visual doorway and the target spawn to the exterior door threshold.
- Added PlayMode checks for:
  - interior/exterior renderer bounds do not intersect.
  - door trigger covers the visible interior door cue.
  - target spawn is outside near the house door.
  - root activation/deactivation after switch.

Time Window status:

- Frozen. No TimeWindow scripts, prefabs, materials, or scene setup were edited for this fix.

Current EXE:

- `<temp>\anemora_ch1_playable_houseexitfix_20260511_1120\Anemora_Chapter1.exe`

Validation:

- Scene patch: `<temp>\anemora_ch1_houseexit_kit_patch_20260511.log`
  - completed.
- EditMode: `<temp>\anemora_ch1_houseexit_kit_structure_editmode_20260511.xml`
  - `Chapter1SceneStructureTests` 6 / 6 passed.
- PlayMode: `<temp>\anemora_ch1_houseexit_kit_playmode_20260511.xml`
  - `Chapter1SceneLoadSmokeTests` 7 / 7 passed.
- Runtime validator: `<temp>\anemora_ch1_houseexit_kit_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`.
  - Warnings remain accepted S4D/S4G placement deltas only.
- Build log: `<temp>\anemora_ch1_playable_houseexitfix_build_20260511_1120.log`
  - succeeded, warnings=0, errors=0.
- Launch smoke: `<temp>\anemora_ch1_playable_houseexitfix_launch_20260511_1120.log`
  - no ObjectiveText / NotificationText / TMP font / unsupported glyph / mojibake / exception / error / material-missing patterns.
- Gameplay camera capture: `docs/devlog/screenshots/chapter1_gameplay_camera/20260511_112332`
  - errors=0, warnings=0, captures=2.
  - Includes inside start and outside spawn with the exterior root activated for evidence capture only.

Manual smoke:

1. Launch the 1120 EXE.
2. Confirm Niro starts inside the house and only the interior map is visible.
3. Walk to the visible doorway/exit cue and confirm the prompt appears; it should not auto-switch.
4. Press `E` or `Space`.
5. Confirm fade, then outside spawn near the house door.
6. Confirm the outside map is distinct, with house at the back/upper center and northeast road visible but unavailable.

## 20260511_1158 House Exit Prompt / Built-Player Fix

Follow-up to the user rejection of 1120. Built-player smoke showed the player could walk through the interior doorway into the black void before the switch, and the gate relied too much on physics trigger events.

Changes:

- Added an interior `VS_HouseInterior_Blocker_DoorSeal` so the door reads as a switch point, not a walk-through opening.
- Moved/enlarged `AreaSwitch_HouseInterior_To_HouseExterior` to the interior side of the door cue.
- `Chapter1AreaSwitchGate` now polls the player position against the gate bounds each frame, so the prompt works in the built player even if trigger callbacks are missed.
- After switching, the one-way proof gate clears prompt state and rejects follow-up input once its collider is disabled.
- Fade duration is now `0.35s` so the map switch is visible to the player.
- House proof objective text now uses a focused `E/Space: とびらで 外へ 出る` control line.

Time Window status:

- Frozen. No production TimeWindow changes were made in this pass.

Current EXE:

- `<temp>\anemora_ch1_playable_houseexitprompt_20260511_1158\Anemora_Chapter1.exe`

Validation:

- Scene patch: `<temp>\anemora_ch1_houseexit_promptdoor_patch_final_20260511.log`
  - completed; saved `VS_HouseInterior_Blocker_DoorSeal`, `fadeSeconds=0.35`, logging-only gate diagnostics.
- PlayMode: `<temp>\anemora_ch1_houseexit_promptdoor_final_playmode_20260511.xml`
  - `Chapter1SceneLoadSmokeTests` 7 / 7 passed.
- Runtime validator: `<temp>\anemora_ch1_houseexit_promptdoor_runtime_validator_20260511.log`
  - `Info=215, Warning=3, Error=0, PendingWiring=0`.
  - Warnings remain accepted S4D/S4G placement deltas only.
- Build log: `<temp>\anemora_ch1_playable_houseexitprompt_build_20260511_1158.log`
  - succeeded, warnings=0, errors=0.
- Built-player smoke: `<temp>\anemora_ch1_houseexit_realplayer_20260511_1158`
  - screenshot sequence: inside -> prompt/blocked at door -> E -> exterior spawn.
  - player log confirms: prompt visible, input inside gate, switch coroutine start, player frozen, target root activated, player moved, source root disabled, gate collider disabled, movement restored.
  - follow-up `Space` logs as input outside gate and does not switch again.
- Gameplay camera capture: `docs/devlog/screenshots/chapter1_gameplay_camera/20260511_115920`
  - errors=0, warnings=0, captures=2.

Manual smoke route:

1. Launch the 1158 EXE.
2. Move to the door glow with `D+S` or normal movement; Niro should stop at the doorway instead of walking into black space.
3. Confirm the objective/prompt includes `E/Space`.
4. Press `E` or `Space`.
5. Confirm the fade, exterior spawn near the house door, and no re-trigger when pressing `Space` again.
