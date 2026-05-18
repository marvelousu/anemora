# 2026-05-18 Fast VS story UI / manual book interaction / Aria cue pass

## Scope

- Replaced the active story display path with a runtime TMP HUD instead of the legacy OnGUI fallback.
- Kept dialogue lines typewriter-based, while guide/objective text appears immediately so it is not mistaken for dialogue.
- Changed the past-library book progression to require player input at the visible cue instead of auto-progressing on proximity.
- Added visible past-library guidance for the target book and the past-side Aria/person cue.
- Added Aria idle-breath sprite placement from the accepted v46 asset source.
- Added invisible no-step blockers around the library reading tables so Niro cannot stand on top of the desks.
- Tightened route-map trigger validation so transition hitboxes stay closer to the visible glow pads.

## Files

- `Assets/Scripts/FastVS/FastVsStoryRuntimeHud.cs`
  - Runtime TMP dialogue / guide / objective HUD.
  - Dialogue uses typewriter reveal.
  - Guide/objective messages are instant and visually separated.
- `Assets/Scripts/FastVS/FastVsStoryFlowController.cs`
  - Manual E / Space / Enter handling for the past book and Reto book-show steps.
  - Runtime HUD review properties.
  - Longer pause beats around the book reveal / question / acceptance beats.
- `Assets/Scripts/FastVS/FastVsSpriteStripLoopAnimator.cs`
  - Small sprite-strip loop driver for past-side Aria idle-breath.
- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Wires `FastVsStoryRuntimeHud`.
  - Copies/validates Aria v46 idle-breath source.
  - Adds past book / Aria guide glows and cue sparks.
  - Adds library table no-step colliders.
  - Validates runtime HUD, Reto writing-loop settle state, manual past-book readiness, and route trigger sizes.

## Asset Source

- Aria idle-breath source:
  `<character-source>/docs/review_gallery/imports/stage4_chapter1_character_asset_pack_v46_2026-05-12/selected_64x96_review_only/stateflow_loops_transitions/resident_a_aria/resident_a_aria_normal_loop_breath_v01_4f_64x96_review_only.png`

The v47/reject/hold sources remain excluded.

## Verification

- Unity batch generation / validation / build:
  `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
  - Passed.
  - Log: `Logs/fast_vs_build_validate_20260518_story_ui_final2.log`
  - Output: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Standalone smoke:
  - Ran the built player for 18 seconds with `-batchmode -nographics`.
  - No `error|exception|failed|crash|NullReference` hits in the smoke output.
  - Log: `Logs/fast_vs_player_smoke_20260518_story_ui_final2.log`
- Review screenshots refreshed:
  `docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/`
  - `03_library_reto_desk.png`
  - `05_library_past_no_temp_people.png`
  - `06_library_dialogue_tmp_font.png`

## Notes

- The final build log contains `System.Numerics.Vector*` resolve messages from Unity Code Coverage / ReportGenerator package scanning. The editor method still completed with return code 0 and the Fast VS validation passed.
- `06_library_dialogue_tmp_font.png` still uses the old filename, but the active presenter is now the runtime TMP HUD path.
