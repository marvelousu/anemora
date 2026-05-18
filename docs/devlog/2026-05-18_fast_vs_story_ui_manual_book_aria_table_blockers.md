# Fast VS story UI / past book interaction / Aria cue pass

Date: 2026-05-18

## Scope
- Continue the Fast VS V24 route implementation from the previous pass.
- Keep the V24 same-coordinate Time Window behavior and the existing route-map contract.
- Address the latest review items around Reto animation state, dialogue UI, past-book interaction, Aria/past-person cueing, movement pad hitboxes, and library table collision.

## Implementation
- Replaced the story presentation path with `FastVsStoryRuntimeHud`.
  - Runtime HUD is Canvas/TMP based and uses the bundled Japanese TMP font asset.
  - Normal dialogue uses typewriter text.
  - Guide/objective text is visually separate and appears immediately, so it no longer reads like part of the spoken dialogue.
  - Legacy OnGUI story panel is bypassed when the runtime HUD exists.
- Tightened map transition trigger boxes.
  - Door/route trigger volumes are now closer to the visible glow footprint.
  - Validation now checks route trigger size explicitly.
- Changed past-library book progression to manual interaction.
  - Standing near the past-side target book now requires `E / Space / Enter`.
  - Returning to current time does not auto-resume the story; the player must approach Reto and press the interaction key.
- Added past-library story cues.
  - Target book glow and Aria/past-person glow were added.
  - Added `Past_Library_AriaIdleAtTable` using the accepted v46 Aria idle-breath loop source copied into FastVS character assets.
  - Added `FastVsSpriteStripLoopAnimator` for simple 4-frame idle loop playback.
- Added no-step blockers for library reading tables.
  - Invisible tall colliders were added over the table footprints to prevent the player from climbing onto them.
- Adjusted Reto writing-state handling.
  - `SetWritingForReview()` no longer restarts the raise-arms transition when Reto is already in raised writing state or already raising.
  - This keeps normal state anchored on the accepted writing loop instead of repeatedly looking like a raise transition.
- Increased several story pauses.
  - Timewriter reaction pauses, past-book pickup pause, person-notice pause, and Reto book-confirmation pauses were lengthened.

## Source Assets
- Aria source:
  - `C:\Users\maro6\Documents\Unity\Anemora-stage4-hero-v2\docs\review_gallery\imports\stage4_chapter1_character_asset_pack_v46_2026-05-12\selected_64x96_review_only\stateflow_loops_transitions\resident_a_aria\resident_a_aria_normal_loop_breath_v01_4f_64x96_review_only.png`
- FastVS import:
  - `Assets/Art/Characters/FastVS/Aria/resident_a_aria_normal_loop_breath_v01_4f_64x96_review_only.png`

## Verification
- Unity validation:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
  - Result: `Fast VS house slice validation passed.`
- Build:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildHouseSlicePlayer`
  - Switched player build to `BuildOptions.CleanBuildCache` after incremental player build exited `-1`.
  - Result: `Fast VS house slice player built`.
- EXE:
  - `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Review screenshots:
  - `docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/03_library_reto_desk.png`
  - `docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/05_library_past_no_temp_people.png`
  - `docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/06_library_dialogue_tmp_font.png`

## Notes
- The worker cycle was used for asset reconnaissance only on this pass; implementation was completed in the main session to avoid conflicting edits after previous mini workers timed out.
- The capture file name `06_library_dialogue_tmp_font.png` is kept for continuity, but the visible UI is now the runtime TMP HUD rather than the old presenter/OnGUI fallback.
