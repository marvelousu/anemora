# Stage 4 Build Smoke + Dispatch State

Date: 2026-05-07

## Summary

This batch records non-review Stage 4 progress after the TMP localization objective coverage milestone. It did not change runtime code, scenes, imported art, or production assets at the time it was written. Resident_A was later approved and imported in `2026-05-07_stage4_resident_a_p1_runtime_import.md`.

## Build Smoke

- Command path: `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe`
- Build target: Windows Standalone
- Output: `Builds/Stage4Smoke/2026-05-07/Anemora_Stage4_Smoke.exe`
- Result: success
- Build log marker: `Build Finished, Result: Success.`

## Player Smoke

- Ran the generated Windows player for 30 seconds at 1280 x 720.
- Player log: `stage4_build_player_smoke.log`
- Checked patterns: `Error`, `Exception`, `Assert`, `DrawObjectsPass`, `RenderGraph`, `NullReference`, `MissingReference`, `Failed`
- Result: no matches.

## Transient Unity Side Effects

Unity build / player execution touched Addressables, URP settings, `DefaultVolumeProfile`, and `ProjectSettings`. These were treated as generated side effects and restored before this note.

## Resident_A Review State

- Candidate A is rejected because the front-facing right eye disappears.
- Candidate B corrected the visible-eye issue and was later user-approved.
- Candidate B was not runtime-ready as generated because the generated image had a flat white background rather than a transparent `4 x 5` frame sheet.
- Follow-up: `2026-05-07_stage4_resident_a_p1_runtime_import.md` records the transparent `32 x 48` cell import and verification.

## TMP Screenshot Harness State

- `DialogueTmpScreenshotCaptureTests.CaptureRepresentativeDialogueTmpScreenshots` is intentionally `[Explicit]`.
- The test skips in batchmode by design.
- Expected output directory when run non-batchmode: `docs/devlog/screenshots/dialogue_tmp_capture/`.
- Capture set: ja-JP / en representative Resident_A and Resident_B dialogue at 1280 x 720, plus selected 1920 x 1080 cases.

## Dispatch

| Workstream | State | Next action | User gate |
|---|---|---|---|
| Resident_A P1 production sheet | Candidate B approved and runtime-imported | Manual in-game review after import | Yes |
| TMP rendered readability | Standalone capture set produced | Review `docs/devlog/screenshots/dialogue_tmp_capture/dialogue_tmp_capture_review_sheet.png` and individual PNGs before UI/font changes | Yes |
| Build / launch stability | Smoke passed | Keep as regression baseline; rerun after import or UI changes | No |
| FPS / memory profiling | Stage 4 v0.1 baseline recorded | Rerun only after major TMP/font/UI, character, or environment import batches | No |
| Dialogue v1 polish | Review sheet exists | Apply only after user approves proposal rows | Yes |
| Audio polish | Objective wiring passed | Listening review / replacement decision later | Yes |
| Zone expansion | Not started | Start after Phase 1 visual/UI baseline stabilizes | Yes |
