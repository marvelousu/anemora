# Stage 4 Verification P0 + Resident_A P1 Note

Date: 2026-05-07

## Summary

Stage 4 remains in Phase 0-1 quality reinforcement. Stage 3 is still accepted complete; this batch does not reopen the Stage 3 gate.

Resident_A art review moved from the F/F4 fallback framing to the P1 direction. The corrected role read is a Past-side ordinary young town resident who helps the living Past side contrast with the declined Current side. Hero v2 and Resident_B v2 remain the scale and pixel-granularity references.

## Added Tests

- `Zone1EnvironmentPrefabTests.Zone1EnvironmentPrefabsHaveValidIntegrity`
  - Covers direct `Assets/Prefabs/Zone1/*.prefab` loadability, missing scripts, renderer presence, finite non-zero bounds, mesh references, and material slots.
- `MainSceneStartupLogTests.MainSceneStartsWithoutErrorsOrExceptions`
  - Loads `Anemora_Main` and asserts that startup settle frames emit no `Error`, `Assert`, or `Exception` logs.
- `DialogueTmpScreenshotCaptureTests.CaptureRepresentativeDialogueTmpScreenshots`
  - `[Explicit]` manual harness for ja-JP / en dialogue screenshots at 1280x720 and selected 1920x1080 cases.

## Verification

- EditMode: `36/36` passed.
- PlayMode: `30 passed / 31 total`; the one skipped test is the `[Explicit]` TMP screenshot capture harness.
- Unity generated transient Addressables / ProjectSettings changes during CLI runs; those were reverted, leaving only the intended test files and docs.

## Resident_A Review Asset

Generated a committed scale review copy:

- `docs/devlog/screenshots/stage4_resident_a_p1_hero_residentb_scale_review.png`

This is not a runtime import. Next art step is a P1-based gameplay-ready Resident_A sheet, reviewed against Hero v2 and Resident_B v2 before replacing the current Resident_A runtime sprites.

## Follow-Up

- Run the explicit TMP capture in non-batchmode before changing dialogue panel or font assets.
- Continue Resident_A P1 production sheet work before runtime import.
- Keep audio listening, dialogue v1 polish, and build/performance checks as the next Stage 4 Phase 1 candidates.
