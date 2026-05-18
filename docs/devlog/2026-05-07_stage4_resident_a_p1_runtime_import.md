# Stage 4 Resident_A P1 Runtime Import

Date: 2026-05-07

## Summary

Resident_A P1 candidate B was user-approved as the replacement direction for the Past-side ordinary young town resident. This batch converts the approved review image into runtime-ready `32 x 48` cell sprite sheets and replaces only the existing Resident_A v2 PNG contents.

## Runtime Asset Changes

- Updated `Assets/Art/Sprites/NPC/Resident_A/v2/resident_a_idle.png`
- Updated `Assets/Art/Sprites/NPC/Resident_A/v2/resident_a_walk_front.png`
- Updated `Assets/Art/Sprites/NPC/Resident_A/v2/resident_a_walk_back.png`
- Updated `Assets/Art/Sprites/NPC/Resident_A/v2/resident_a_walk_left.png`
- Updated `Assets/Art/Sprites/NPC/Resident_A/v2/resident_a_walk_right.png`

No `.meta`, prefab, AnimatorController, AnimationClip, or scene file was changed. The existing Resident_A v2 sprite GUIDs, slice names, fileIDs, `32 x 48` rects, PPU 32, point filtering, and bottom-center pivots remain intact.

## Processing Notes

- Candidate A was rejected because the front-facing right eye disappeared.
- Candidate B corrected the front-facing eye readability issue and was approved by the user.
- The generated review image had a flat white background, so the import pass removed the generated canvas and neutral grey halo pixels before producing transparent runtime sheets.
- Each frame was component-extracted, nearest-neighbor resized to a max `45 px` height and `22 px` width, and bottom-centered in the existing `32 x 48` cell layout.
- Review comparison: `docs/devlog/screenshots/stage4_resident_a_p1_candidate_b_runtime_sheets_compare.png`

## Verification

- EditMode: `39/39 passed`
- PlayMode: `31 passed / 32 total`; the one skipped test is the existing `[Explicit]` TMP screenshot capture harness.
- Windows build smoke: success
  - Output: `Builds/Stage4Smoke/2026-05-07-resident-a-import/Anemora_Stage4_ResidentAImport_Smoke.exe`
  - Build log marker: `Build Finished, Result: Success.`
- Player smoke: 30 seconds at 1280 x 720
  - Player log: `stage4_build_resident_a_import_player_smoke.log`
  - Checked patterns: `Error`, `Exception`, `Assert`, `DrawObjectsPass`, `RenderGraph`, `NullReference`, `MissingReference`, `Failed`
  - Result: no matches.

## Follow-Up

- Resident_A is no longer blocked on runtime import.
- Manual in-game review should still confirm final feel against Hero v2 and Resident_B v2 in `Anemora_Main`.
- TMP rendered readability, dialogue v1 polish, and audio listening remain separate Stage 4 Phase 1 workstreams. FPS / memory profiling now has a Stage 4 v0.1 baseline in `2026-05-07_stage4_performance_baseline_v0_1.md`; rerun it after major visual/UI changes.
