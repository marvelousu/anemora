# Chapter 1 B Visual Specifics Scene Dressing

Date: 2026-05-10
Worktree: `<worktree>`

## Scope

Reflected the Linux story/spec B handoff from `chapter1_s1_s2_handover_2026-05-08.md` v1.9 into the Chapter 1 production scene setup as scene-owned dressing. This pass is not final graphics polish; it establishes stable roots and uses the graphics session's optional `Ch1_B*` prefab kits when they are available.

## Implemented Roots

`AnemoraChapter1SceneSetup` now creates these scene-owned roots. When the copied graphics `Ch1_B*` prefab kits exist, setup instantiates them under the corresponding roots; otherwise it falls back to primitive placeholder dressing.

- `Chapter1_B7_PastLibrary_Dressing`
  - eight bookshelf markers
  - four candle markers and warm local lights
  - Aria reading silhouette
  - two librarian silhouettes
  - family-record book placement marker
- `Chapter1_B8_PastKaiaField_Dressing`
  - ancestor silhouette with hoe marker
  - Dario vendor silhouette
  - spice stall, seven spice bottles, and subtle smoke markers
- `Chapter1_B5_NiroHouse_CurrentForeshadow`
  - current-side empty wall frame
  - single place setting marker
- `Chapter1_B5_NiroHouse_PastForeshadow`
  - past-only family-occupied room dressing
  - two beds, four chairs, two shelves, dish sets, knitting tools, clothes, shoes, pot
  - family painting with unfinished/blurred face markers
- `Chapter1_B2_RuinHouse_CurrentInterior`
  - 4m-class ruined interior floor, cracked wall, collapsed ceiling edge, rotten furniture, no-glass window, moss patch
- `Chapter1_B2_RuinHouse_PastInterior`
  - restored interior floor, wall, fireplace, warm light, orderly table/shelf, two resident silhouettes
- `Chapter1_Cutscene_S5_SideView`
  - inactive cutscene-only visual root
  - instantiates `Ch1_B3_SideViewCinematic_Background` and `Ch1_B3_SideViewCinematic_ForegroundAnchors`
  - contains twilight sky / Antela horizon / ruin foreground / Niro side marker / pebble marker / side-view camera anchor coverage

## Graphics Kit Inputs

The implementation worktree now contains the graphics session's B visual kit prefabs:

- `Assets/Prefabs/Zone1/Chapter1DetailKitProduction/Ch1_B5_NiroHouse_CurrentTraceHook.prefab`
- `Assets/Prefabs/Zone1/Chapter1DetailKitProduction/Ch1_B5_NiroHouse_PastFamilyTraceKit.prefab`
- `Assets/Prefabs/Zone1/Chapter1DetailKitProduction/Ch1_B7_PastLibrary_DetailMarkers.prefab`
- `Assets/Prefabs/Zone1/Chapter1DetailKitProduction/Ch1_B8_PastKaiaField_DetailMarkers.prefab`
- `Assets/Prefabs/Zone1/Chapter1MapProduction/Ch1_B2_EnterableHouse_CurrentInterior.prefab`
- `Assets/Prefabs/Zone1/Chapter1MapProduction/Ch1_B2_EnterableHouse_PastInterior.prefab`
- `Assets/Prefabs/Zone1/Chapter1MapProduction/Ch1_B3_SideViewCinematic_Background.prefab`
- `Assets/Prefabs/Zone1/Chapter1MapProduction/Ch1_B3_SideViewCinematic_ForegroundAnchors.prefab`

## Validation

Scene setup:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath '<worktree>' -executeMethod Anemora.EditorTools.AnemoraChapter1SceneSetup.Apply -logFile '<temp>\anemora_ch1_scene_setup_b_visual_specifics.log'
```

Latest setup result: `Anemora Chapter 1 scene setup completed`.

Log:

```text
<temp>\anemora_ch1_scene_setup_bkit.log
```

Runtime validator:

```text
Info=107, Warning=3, Error=0, PendingWiring=0
```

The three warnings are the accepted S4D/S4G placement deltas.

Latest scene-integrated capture:

```text
docs/devlog/screenshots/chapter1_scene_integrated/20260510_191543
errors=0, warnings=0, captures=18
```

Dedicated side-view cinematic capture:

```text
docs/devlog/screenshots/chapter1_side_view_cinematic/20260510_201202
errors=0, warnings=0, captures=1
```

B-3 timed/phase capture package:

```text
docs/devlog/screenshots/chapter1_side_view_cinematic/20260510_203513
errors=0, warnings=0, captures=5
```

Exported phase frames:

- `turn_back_5s`
- `pan_start_10s`
- `pan_mid_10s`
- `pan_end_10s`
- `stone_kick_handoff`

Latest review build:

```text
<temp>\anemora_ch1_review_player_b3_runtime_flow_20260510_202430\Anemora_Chapter1.exe
Build result: Succeeded, warnings=0, errors=0
```

B-3 runtime flow implementation:

```text
ChapterTransitionController now enables the inactive Chapter1_Cutscene_S5_SideView root/camera for the side-view sequence, waits the 5s turn-back phase, runs the 10s horizontal pan phase, then continues auto-walk -> stone kick -> fade/title/save. Same-scene completion and save failure restore the prior camera/root state.
```

Runtime validator after importing runtime Phase M-P and regenerating the scene:

```text
Info=203, Warning=3, Error=0, PendingWiring=0
```

Runtime validator after B-3 runtime flow:

```text
<temp>\anemora_ch1_impl_runtime_validator_b3_runtime_flow.log
Info=203, Warning=3, Error=0, PendingWiring=0
```

Targeted validator regression:

```text
Chapter1RuntimeSceneValidatorTests + Chapter1SceneCapturePlanTests: 30/30 passed
```

Targeted transition runtime regression:

```text
<temp>\anemora_ch1_impl_b3_runtime_playmode_transition_after_anim_guard.xml
ChapterTransitionControllerPlayModeTests: 5/5 passed
```

Targeted capture helper regression:

```text
<temp>\anemora_ch1_impl_b3_timed_capture_helper_editmode.xml
Chapter1SceneCapturePlanTests: 6/6 passed
```

## Notes

- The B-3 side-view root is inactive in the base scene and is activated only by the chapter transition runtime path. It does not appear in the normal 18 PNG scene-integrated capture, so `chapter1_side_view_cinematic/20260510_201202` remains the current visual review package for B-3.
- `20260510_192215` was the first side-view capture and was reviewed by graphics as needs polish. `20260510_193428` superseded it after implementation-side camera anchor, lighting, and material-readability adjustments. `20260510_200217` superseded both after the improved graphics-source `Chapter1_B3_sideview_*` materials were copied into implementation. `20260510_201202` supersedes it after implementation-side camera/framing pass2: camera target `(23, 1.55, 12.55)`, orthographic size `3.65`, and warmer camera background.
- Graphics reviewed `20260510_201202` as B-3 composition pass. No pass3 still-frame composition pass is required now.
- `20260510_203513` is the first phase-frame package after B-3 runtime flow connection. It is intended for timed cutscene review against graphics' checklist; it exports representative stills for the 5s turn-back, 10s pan, and stone-kick handoff beats.
- Graphics reviewed `20260510_200217` as source-package readiness pass / no graphics-source blocker. B-3 final visual acceptance remains needs polish, now owned by implementation scene/cutscene composition plus final art and character sprite dependency.
- Temporary B-3 scene-instance material/color overrides are suppressed when `Chapter1_B3_sideview_*` source materials are present, so the graphics package materials remain visible.
- The B-5/B-2/B-3/B-7/B-8 roots are scene assembly placements using graphics-package prefab kits. Further B-3 work should focus on side-view camera/framing/lighting/timing unless graphics identifies a new source asset breakage.
- Full side-view camera/runtime phase control is now connected in `ChapterTransitionController`; final visual acceptance still requires playtest/cutscene review with final character sprites.
- Commit, push, PR, and staging were not performed.

## B-Specific Detail Capture Package - 20260510_235343

After graphics identified B-specific detail captures as the next bottleneck, implementation added a dedicated capture helper path:

- execute method: `Anemora.Editor.AnemoraChapter1SceneCaptureHelper.CaptureChapter1BSpecificDetailBatch`
- menu: `Anemora/Assets/Capture Chapter1 B-Specific Detail Review`
- output root: `docs/devlog/screenshots/chapter1_b_specific_details/`
- scene-save policy: the helper opens the production scene, captures, restores active state, and does not save the scene

Generated package:

```text
docs/devlog/screenshots/chapter1_b_specific_details/20260510_235343
errors=0, warnings=0, captures=6
```

Captured evidence:

- `ch1_b_detail_20260510_235343_current_b5_niro_house_current_detail.png`
- `ch1_b_detail_20260510_235343_past_b5_niro_house_past_detail.png`
- `ch1_b_detail_20260510_235343_current_b2_ruin_house_current_detail.png`
- `ch1_b_detail_20260510_235343_past_b2_ruin_house_past_detail.png`
- `ch1_b_detail_20260510_235343_past_b7_past_library_detail.png`
- `ch1_b_detail_20260510_235343_past_b8_past_kaia_field_detail.png`
- `ch1_b_detail_20260510_235343_contact_sheet.png`
- `capture_manifest.json`
- `capture_report.md`

Active-state / fitting notes:

- B-5 Current/Past and B-2 Current/Past are captured with the matching `Root_Current` or `Root_Past` active.
- B-7 and B-8 are Past-only detail captures.
- Character state remains `placeholder`; final Dario / ancestor / Aria / librarian sprite judgment is deferred.
- Capture targets resolved to scene objects; no fallback targets were used.

Validation:

```text
<temp>\anemora_ch1_impl_b_detail_capture.log
Chapter1 scene capture result: errors=0, warnings=0, captures=6

<temp>\anemora_ch1_impl_b_detail_capture_helper_editmode.xml
Chapter1SceneCapturePlanTests: 6/6 passed
```

Graphics review is pending for `20260510_235343`.

## B-Specific Detail Capture Package - 20260511_000102

The first detail package `20260510_235343` was technically valid, but implementation self-review found that B-2 / B-7 could be framed more clearly for graphics closeout. The helper camera plans were tightened and tilted further downward, then a superseding detail package was generated:

```text
docs/devlog/screenshots/chapter1_b_specific_details/20260511_000102
errors=0, warnings=0, captures=6
```

Superseding review package:

- `ch1_b_detail_20260511_000102_current_b5_niro_house_current_detail.png`
- `ch1_b_detail_20260511_000102_past_b5_niro_house_past_detail.png`
- `ch1_b_detail_20260511_000102_current_b2_ruin_house_current_detail.png`
- `ch1_b_detail_20260511_000102_past_b2_ruin_house_past_detail.png`
- `ch1_b_detail_20260511_000102_past_b7_past_library_detail.png`
- `ch1_b_detail_20260511_000102_past_b8_past_kaia_field_detail.png`
- `ch1_b_detail_20260511_000102_contact_sheet.png`
- `capture_manifest.json`
- `capture_report.md`

Framing changes:

- B-5 Current/Past: higher pitch, tighter orthographic size `2.05`
- B-2 Current/Past: higher pitch, tighter orthographic size `2.15`
- B-7 Past library: closer detail target and orthographic size `2.25`
- B-8 Past Kaia field: tighter orthographic size `3.25`

Use `20260511_000102` as the active B-specific detail review package. `20260510_235343` remains an earlier valid machine package, but is superseded for visual review.

## B-3 Enhanced Timed Evidence Package - 20260511_001409

Graphics reviewed the first timed B-3 package `20260510_203513` as capture-generation pass, but final timed visual acceptance still needed explicit evidence for the monologue hold, stone-kick contact sequence, and fade/title/save handoff. Implementation extended the side-view capture helper to export those frames without modifying the production scene or `ChapterTransitionController` runtime behavior.

Updated helper behavior:

- `CaptureChapter1SideViewCinematicBatch` now exports 9 phase frames.
- Added explicit phase IDs:
  - `monologue_hold`
  - `pre_kick_contact`
  - `kick_contact`
  - `post_kick`
  - `fade_title_save_handoff`
- Kick-contact frames apply capture-only offsets to existing B-3 placeholder markers, then restore the scene state.
- Fade/title/save handoff frame temporarily exposes the transition canvas to the capture camera and restores it afterward.
- Character state remains `placeholder`; no character asset import occurred.

Generated package:

```text
docs/devlog/screenshots/chapter1_side_view_cinematic/20260511_001409
errors=0, warnings=0, planned_viewpoints=9, captures=9
```

Review files:

- `capture_manifest.json`
- `capture_report.md`
- `ch1_side_view_20260511_001409_b3_enhanced_phase_contact_sheet.png`
- `ch1_side_view_20260511_001409_b3_turn_back_5s.png`
- `ch1_side_view_20260511_001409_b3_pan_start_10s.png`
- `ch1_side_view_20260511_001409_b3_pan_mid_10s.png`
- `ch1_side_view_20260511_001409_b3_pan_end_10s.png`
- `ch1_side_view_20260511_001409_b3_monologue_hold.png`
- `ch1_side_view_20260511_001409_b3_pre_kick_contact.png`
- `ch1_side_view_20260511_001409_b3_kick_contact.png`
- `ch1_side_view_20260511_001409_b3_post_kick.png`
- `ch1_side_view_20260511_001409_b3_fade_title_save_handoff.png`

Validation:

```text
<temp>\anemora_ch1_impl_b3_enhanced_capture_pass2.log
Chapter1 scene capture result: errors=0, warnings=0, captures=9

<temp>\anemora_ch1_impl_b3_enhanced_capture_helper_editmode_final.xml
Chapter1SceneCapturePlanTests: 6/6 passed
```

This is still visual evidence for graphics review, not final B-3 character acceptance. The final side-facing Niro sprite remains blocked by the character-art approval path.

## Graphics B-3 Enhanced Timed Review - 20260511_001409

Graphics reviewed `20260511_001409` and marked the B-3 enhanced timed visual evidence as `pass` for placeholder-stage timed closeout.

Verdict:

- B-3 enhanced timed visual: `pass`
- scope: placeholder-stage timed closeout evidence
- per-phase review: `turn_back_5s`, pan phases, `monologue_hold`, `pre_kick_contact`, `kick_contact`, `post_kick`, and `fade_title_save_handoff` all pass
- blocker / fail: none
- graphics source issue: none
- source pass requested: no

Graphics artifact:

```text
<worktree>\docs\chapter1_graphics_b3_enhanced_timed_capture_review_20260511_001409.md
```

Reopen B-3 graphics review only after final side-view sprite replacement, B-3 camera/framing/lighting changes, fade/title/save presentation changes, or concrete source prefab/material/shader/atlas breakage.

## Graphics B-Detail Review - 20260511_000102

Graphics reviewed the active B-specific detail package `20260511_000102` and marked it `needs polish` with no blocker and no graphics source issue. `20260510_235343` is now historical only.

Verdict summary:

- Overall B-detail: `needs polish`
- B-5 Current detail: `needs polish`
- B-5 Past detail: `needs polish`
- B-2 Current detail: `needs polish`
- B-2 Past detail: `pass` for local evidence
- B-7 Past-library detail: `needs polish`
- B-8 Past Kaia-field detail: `needs polish`
- blocker: none
- graphics source issue: no

Next implementation polish:

- B-5: improve Current empty-frame / object-trace readability and reduce ambiguity around Past placeholder figures.
- B-2: improve Current value separation; Past can remain the local evidence baseline unless lighting changes.
- B-7: provide marker map or clearer countable detail evidence for 8 shelves, 4 candles, family book, Aria seat, and 2 librarian markers.
- B-8: improve Past field value separation while keeping Dario/stall support subordinate and CP-1 comparisons readable.

## B-Specific Detail Polish Package - 20260511_002510

Implementation applied a local scene-dressing polish pass against the graphics `20260511_000102` checklist. This pass keeps the same B-specific capture contract and remains placeholder-character evidence.

Scene setup changes:

- B-5 Current: added clearer empty wall-frame / object-trace readability overlays.
- B-5 Past: reinforced object-count contrast while preserving the no-visible-people rule for final interpretation.
- B-2 Current: added value-separation support for the ruined interior without replacing the Past baseline.
- B-7 Past library: added explicit count/evidence markers for shelves, candles, family book, Aria seat, and librarian marker locations.
- B-8 Past Kaia field: added value-separation support for healthy crop patches / clear well while keeping Dario/stall support subordinate.

Generated package:

```text
docs/devlog/screenshots/chapter1_b_specific_details/20260511_002510
errors=0, warnings=0, planned_viewpoints=6, captures=6
```

Review files:

- `capture_manifest.json`
- `capture_report.md`
- `b7_marker_map_20260511_002510.md`
- `ch1_b_detail_20260511_002510_contact_sheet.png`
- `ch1_b_detail_20260511_002510_current_b5_niro_house_current_detail.png`
- `ch1_b_detail_20260511_002510_past_b5_niro_house_past_detail.png`
- `ch1_b_detail_20260511_002510_current_b2_ruin_house_current_detail.png`
- `ch1_b_detail_20260511_002510_past_b2_ruin_house_past_detail.png`
- `ch1_b_detail_20260511_002510_past_b7_past_library_detail.png`
- `ch1_b_detail_20260511_002510_past_b8_past_kaia_field_detail.png`

Validation:

```text
<temp>\anemora_ch1_impl_bdetail_polish2_capture.log
Chapter1 scene capture result: errors=0, warnings=0, captures=6

<temp>\anemora_ch1_impl_runtime_validator_bdetail_polish.log
Summary: Info=215, Warning=3, Error=0, PendingWiring=0

<temp>\anemora_ch1_impl_bdetail_polish_capture_helper_editmode.xml
Chapter1SceneCapturePlanTests: 6/6 passed

<temp>\anemora_ch1_review_build_bdetail_polish_20260511_002510.log
Chapter1 review build result: Succeeded, warnings=0, errors=0
```

B-7 marker map:

- `docs/devlog/screenshots/chapter1_b_specific_details/20260511_002510/b7_marker_map_20260511_002510.md`
- covers 8 shelf evidence strips, 4 candle markers, family record book, Aria seat, and 2 librarian placement markers

Latest Chapter 1-first review player:

```text
<temp>\anemora_ch1_review_player_bdetail_polish_20260511_002510\Anemora_Chapter1.exe
```

Use `20260511_002510` as the active B-specific detail review package. `20260511_000102` remains the graphics-reviewed needs-polish baseline.
