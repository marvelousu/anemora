# Fast VS Story v4 Review Fix

Date: 2026-05-18

Project:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample`

## User Review Points Addressed

- Story source was likely stale.
- Parent/child material might exist in the latest source.
- Aria dialogue and Reto flow had non-canon or old-version text.
- Book guidance looked like a flat untextured board.
- Past book remained visible after pickup.
- Current Reto desk book placement needed to read as Reto-facing initial desk book, with the returned book separate.
- Dialogue HUD speaker/objective placement and heavy outline needed adjustment.

## Canon Decision

Fast VS now adopts Scene 1 v4 as the working source.

Canonical source files checked:

- `C:\Users\maro6\Documents\Unity\Anemora-stage4-chapter1-impl\docs\devlog\2026-05-09_chapter1_layer1_revision_and_scene3_design.md`
- `C:\Users\maro6\Documents\Unity\Anemora-stage4-chapter1-impl\docs\draft\chapter1_s1_s2_handover_2026-05-08.md`
- `C:\Users\maro6\Documents\Unity\Anemora-stage4-chapter1-runtime\docs\draft\chapter1_graphic_session_handover_2026-05-09.md`

Old/provisional source intentionally superseded:

- `C:\Users\maro6\Documents\Unity\Anemora-stage4-chapter1-impl\docs\devlog\2026-05-12_chapter1_vs_story_canon_inventory.md`

Direct answer:

- Yes, the previous implementation was mixing in the old/provisional v3 basis.
- The latest Scene 1 basis is v4: Niro takes the past book, the returned book appears in the present, and Reto reacts with `...本物だ`.
- The parent/child material is real, but it belongs to Scene 3 Aria/Karla, not Scene 1.
- Niro house "another family" is a separate hidden foreshadowing and should not be folded into the library Reto event.

## Worker Cycle

The requested plan -> gpt-5.4-mini worker -> parent review cycle was followed.

- Story/UI worker: `019e3857-882e-7ee3-bff5-466a22fd5c6f`
- Map/setup worker: `019e3857-c822-7701-95ce-80e02d2db544`

Parent review corrected one worker-side canon slip: `...本物だ` had briefly been inserted into the pre-book timewriter activation beat. It now appears only after the returned book interaction.

## Changed Files

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scripts\FastVS\FastVsStoryFlowController.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scripts\FastVS\FastVsStoryRuntimeHud.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\devlog\2026-05-18_fast_vs_story_canon_v4_source_check.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\devlog\2026-05-18_fast_vs_story_v4_review_fix.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\devlog\INDEX.md`

## Story / UI Changes

- Removed invalid Scene 1 Aria wording that referenced Reto.
- Kept Scene 1 Aria as distant observation only. Exact Scene 1 Aria dialogue was not found in the latest sources, so sparse provisional wording remains:
  - `(...人)`
  - `返却の記録は、あとで机へ戻しておこう。`
  - `(...本を読んでいる)`
  - `(...こちらには気づいていない)`
- Replaced old Mia hint with v4 line:
  - `あなたなら、力になれるかもしれません。`
- Corrected Reto line:
  - `それでも、書いておかないと、いずれ何もかもが...`
- Lengthened pauses around the book reaction beats.
- Changed objective wording from developer-facing `現在の赤い光` to player-facing `赤い光`.
- Door brush event is now:
  - page 0: `?` above Niro + centered framed brush
  - page 1: `ポケットに、何か入っている。`
  - page 2: `(...筆)`
- Dialogue advance hint remains compact `▽`.
- Speaker label and objective/guide text were repositioned inside their panels.
- TMP outline was reduced to lighten the DotGothic16 presentation.

## Map / Prop Changes

- Past target book is now `Past_Library_TargetBook_ForPickup`.
- Past target marker is now `Past_Library_TargetBook_RedCubeMarker`.
- After Niro takes the past book, both the past book prop and red marker are hidden.
- Current returned book is story-controlled separately as `Current_Library_ReturnedBookOnDesk`.
- Initial Reto desk book is separate as `Current_Library_RetoDeskBook_Initial`.
- Past library side shelves were added:
  - `Past_Library_LeftSideBookshelf`
  - `Past_Library_RightSideBookshelf`
- Niro start height was nudged upward to reduce the sunk-feet look at startup.

## Validation

Unity build/validation:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Logs\fast_vs_build_validate_20260518_story_canon_v4_review_fix_rerun2.log`
- Result: pass.

Review screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\devlog\screenshots\fast_vs_story_reto_shadow_20260518`
- Capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Logs\fast_vs_capture_review_20260518_story_canon_v4_review_fix.log`
- Result: pass.

Player smoke:

- EXE: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Logs\fast_vs_player_smoke_20260518_story_canon_v4_review_fix.log`
- Result: 0 matches for error / exception / NullReference / MissingReference / TMP glyph / LiberationSans.

## Remaining Review Notes

- The screenshot pass confirmed the past book and red cube marker are visible, but the dialogue HUD capture did not visibly show a dialogue panel in the captured frame. This should be checked interactively in the next play review.
- Scene 1 Aria still uses sparse provisional text because latest sources specify her role and presence, but not a direct canonical Scene 1 dialogue block.

