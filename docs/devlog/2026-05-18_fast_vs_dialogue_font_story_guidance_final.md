# 2026-05-18 Fast VS Dialogue Font / Story Guidance Final Pass

## Scope

This pass addressed the latest review items around story UI readability, Reto dialogue state, current-side guidance for the past observation phase, and DotGothic16 font application.

## Worker Cycle

The requested gpt-5.4-mini cycle was followed.

- gpt-5.4-mini worker `019e382c-8037-7af3-9b95-4fe14b708aa9` handled focused implementation in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scripts\FastVS\FastVsStoryRuntimeHud.cs`, `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scripts\FastVS\FastVsDirectionalSpriteAnimator.cs`, and `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scripts\FastVS\FastVsRetoWritingAnimator.cs`.
- gpt-5.4-mini worker `019e383d-ee86-71a0-bebe-2732bd480225` fixed the compact advance marker and set `TMP_Settings.defaultFontAsset` before runtime TMP text creation.
- gpt-5.4-mini worker `019e3841-cead-7bf2-99ac-6ac64f100ada` added explicit UI glyph and ASCII coverage to the DotGothic16 TMP atlas builder.
- The parent session reviewed the worker reports, regenerated assets, ran Unity validation/build, refreshed screenshots, and ran a player smoke check.

The worker-cycle rule is now recorded in `C:\Users\maro6\shared-context\memory\project_anemora_fast_vs_worker_cycle.md` and indexed from `C:\Users\maro6\shared-context\memory\MEMORY.md`.

## Changes

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scripts\FastVS\FastVsStoryFlowController.cs`
  - Moved `(...人)` out of the book interaction and into the Aria/person observation path.
  - Split current-side Time Window guidance into book and Aria cues.
  - Removed guide-style story lines from the dialogue log path where they were being mistaken for dialogue.
  - Shortened interaction prompts and removed the `VS clear:` style prefix from objective text.
  - Kept the Reto continuation gated on talking to Reto after returning to the present.

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scripts\FastVS\FastVsStoryRuntimeHud.cs`
  - Uses the compact advance marker `▽`.
  - Sets the DotGothic16 TMP font as the TMP default before creating runtime text objects.
  - Keeps dialogue typewriter-based while guide/objective text remains immediate.

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scripts\FastVS\FastVsDirectionalSpriteAnimator.cs`
  - Freezes facing changes while story movement is locked, so dialogue does not allow accidental direction changes.

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scripts\FastVS\FastVsRetoWritingAnimator.cs`
  - Reto now settles into the talk-loop/lowered dialogue idle instead of looping the lower-arms transition.

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
  - Adds a current-side Aria Time Window opening cue.
  - Adds cleaner past-library reading tables and no-step colliders.
  - Adds table legs to the Niro house table.
  - Removes the old central archive-shelf blockout remnants.
  - Adds validation around DotGothic16, Reto talk-loop material, story guidance state, Aria observation ordering, and removed temporary objects.

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Editor\AnemoraTmpJapaneseAtlasBuilder.cs`
  - Uses `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\UI\Localization\Fonts\ThirdParty\DotGothic16-Regular.ttf`.
  - Adds required runtime HUD punctuation including `.`, `▽`, Japanese brackets, punctuation, and printable ASCII `U+0020` to `U+007E`.
  - Rebinds `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\UI\Localization\Fonts\Anemora_JP_DistanceField.mat` to the regenerated atlas.

## Canon Notes

The checked story sources do not contain a locked Scene 1 Aria speech block. `C:\Users\maro6\Documents\Unity\Anemora-stage4-chapter1-impl\docs\devlog\2026-05-09_chapter1_scene1_v3_final.md` describes past Aria as distant, reading, and not noticing Niro, with Niro observation lines like `(...ここに、本が)` and `(...あの子)`. The current Aria/person monologue in the Fast VS build remains a provisional interaction bridge until a canon line set is selected.

## Verification

- DotGothic16 atlas regeneration:
  - Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Logs\fast_vs_build_dotgothic16_font_20260518_ascii_marker_final.log`
  - Result: `Anemora TMP JP Atlas built. requested=6831, missing=55, atlas=4096x4096, format=Alpha8`
  - Confirmed `m_Unicode: 46` and `m_Unicode: 9661` are present in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\UI\Localization\Fonts\Anemora_JP.asset`.

- Unity validation and Windows build:
  - Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Logs\fast_vs_build_validate_20260518_dialogue_font_story_guidance_final2.log`
  - Result: `Fast VS house slice validation passed.`
  - Result: `Build Finished, Result: Success.`
  - EXE: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

- Review screenshots:
  - Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Logs\fast_vs_capture_review_20260518_dialogue_font_story_guidance_final2.log`
  - Output directory: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\devlog\screenshots\fast_vs_story_reto_shadow_20260518`
  - No `The character with Unicode` or `The LiberationSans` warnings remained in the final screenshot log.

- Player smoke:
  - Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Logs\fast_vs_player_smoke_20260518_dialogue_font_story_guidance_final2.log`
  - Ran the Windows player for 18 seconds with `-batchmode -nographics`.
  - No `error`, `exception`, `failed`, `crash`, `NullReference`, `MissingReference`, `The character with Unicode`, or `The LiberationSans` matches.

## Notes

- Unity batch logs still contain a non-fatal licensing background update line in some runs. It did not stop validation, screenshot capture, atlas generation, or build output.
- The final screenshot capture path is still camera-render based. It validates world-space framing and object cleanup, but runtime overlay HUD review should still be checked in the player.
