# 2026-05-27 Chapter 1 Time Window Right-Side Guard Cycle 113

## Scope
- Investigated the report that Time Window placement can stop working on the right side after the street-corner continuation.
- Confirmed the generated scene uses the widened Time Window region (`240 x 58`), which keeps the D/E/F continuation route inside the placement span.
- Expanded the validation guard from only D3/E3/F6 to D1/D3/E1/E3/F1/F5/F6 so entry, mid-route, and exit points all require successful right-side Time Window drag placement.
- Added diagnostic context to failure messages so stale narrow-region scenes report `regionSize`, `playerX`, and `portalX`.

## Verification
- `ValidateChapter1AllMapsBatch`: passed (`Logs/chapter1_cycle113_validate.log`)
- `CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed (`Logs/chapter1_cycle113_capture.log`)
- Gallery regenerated at `Logs/review_gallery_2026-05-27T08-12/index.html`; Playwright image-src check passed with 7 src image elements, 6 unique images, and 0 broken.
- Reviewer subagent Fermat: ACCEPT after Unity side-effect cleanup.
- `BuildAndValidateBatch`: passed (`Logs/chapter1_cycle113_build.log`)
- Player smoke: passed with 0 error-like matches (`Logs/chapter1_cycle113_player_smoke.log`)
- `python .github\scripts\validate-review-dirs.py`: passed
- Review target exe: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Review dir: `docs/review/2026-05-27T08-12`

## Review Notes
- No map screenshot pixels changed; this cycle is a regression guard for Time Window generation reach.
- Explorer subagent Heisenberg confirmed the likely old-build failure mode: a stale narrow `regionSize.x = 78` would clamp right-side placement around local X 37-38, while the current widened region reaches the D/E/F continuation route.
