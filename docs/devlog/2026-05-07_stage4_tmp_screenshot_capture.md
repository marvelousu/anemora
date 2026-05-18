# Stage 4 TMP Screenshot Capture

Date: 2026-05-07

## Summary

This pass captured representative `DialoguePanel` screenshots from a Windows Standalone player after the Resident_A P1 runtime import. The existing `[Explicit]` PlayMode test remains batchmode-skipped by design, so this capture used a temporary standalone bootstrap build that mirrors the same cases without committing temporary runtime/editor code.

No production assets, scenes, prefabs, localization tables, TMP font assets, or ProjectSettings changes are intended from this pass.

## Method

- Temporary build output: `Builds/Stage4TmpCapture/2026-05-07/Anemora_Stage4_TmpCapture.exe`
- Player output directory: `docs/devlog/screenshots/dialogue_tmp_capture/`
- Capture path: standalone player, `Anemora_Main`, real `NpcInteractable.TryInteract`, locale switch through `LocalizationSettings`, `Canvas.ForceUpdateCanvases`, `TMP_Text.ForceMeshUpdate`, then `Texture2D.ReadPixels`
- Player exit code: `0`
- Player log checked patterns: `Error`, `Exception`, `Assert`, `NullReference`, `MissingReference`, `Failed`
- Checked pattern count: `0`

## Captures

| File | Resolution | Locale | Target |
|---|---:|---|---|
| `dialogue_1280x720_ja-JP_resident_a_line_1.png` | 1280 x 720 | ja-JP | Resident_A line 1 |
| `dialogue_1280x720_ja-JP_resident_b_line_3.png` | 1280 x 720 | ja-JP | Resident_B line 3 |
| `dialogue_1280x720_en_resident_a_line_3.png` | 1280 x 720 | en | Resident_A line 3 |
| `dialogue_1280x720_en_resident_b_line_2.png` | 1280 x 720 | en | Resident_B line 2 |
| `dialogue_1920x1080_ja-JP_resident_a_line_1.png` | 1920 x 1080 | ja-JP | Resident_A line 1 |
| `dialogue_1920x1080_en_resident_b_line_2.png` | 1920 x 1080 | en | Resident_B line 2 |
| `dialogue_tmp_capture_review_sheet.png` | 1280 x 1170 | mixed | Contact sheet for review |

All six direct captures are non-trivial PNG files with the requested dimensions. The contact sheet is for fast review only; inspect the individual PNGs for final readability judgment.

## Initial Readability Notes

- TMP text renders in both ja-JP and en.
- The 1280 x 720 body text is readable but small, especially in the contact sheet and on long English lines.
- The current black translucent panel keeps text visible, but the panel sits at the extreme bottom of the frame and leaves little vertical comfort.
- This is enough evidence to move from "need capture" to "user readability decision pending"; it is not yet a reason to replace fonts or edit `DialoguePanel.prefab` without review.

## Follow-Up

- User review should decide whether the current TMP font size / panel height / contrast is acceptable for Stage 4 baseline.
- If UI polish is requested, change the smallest surface first: dialogue body size / panel padding / panel height, then rerun this capture set and PlayMode dialogue tests.
- Keep font replacement as a later decision unless the screenshots show a clear glyph readability blocker.
