# 2026-05-28 Chapter 1 Map Content Refine Cycle 129

Scope:
- F1-F6 past-only central gorge depth read.
- Kept current side, route pads, roads, bridge, main path, houses, stalls, and farm layout untouched.
- Focused on making the past river read as a deep low gorge rather than a raised water strip.

Changed:
- Added `CreateChapter1Cycle129RuinsPastGorgeDepthCueDetails` after the cycle126 gorge pass.
- Hid older cycle121/cycle126 past-only shallow floor/water/depth cues that visually flattened the gorge.
- Added low bottom shadows, narrow low water threads, bridge under-void shadowing, side drop shades, and bridge-mouth occlusion.
- Added `ValidateChapter1RuinsCycle129PastGorgeDepthCue` to cover hidden legacy cues and the new water/shadow replacements.

Review:
- Review dir: `docs/review/2026-05-28T02-05/`
- Gallery: `Logs/review_gallery_2026-05-28T02-05/index.html`
- Reviewer: No findings. ACCEPT.

Verification:
- `Logs/chapter1_cycle129_validate.log`: passed.
- `Logs/chapter1_cycle129_capture.log`: passed.
- `Logs/chapter1_cycle129_build.log`: passed.
- `Logs/chapter1_cycle129_player_smoke.log`: passed, 30-second smoke with `ErrorLikeMatches=0`.
- `.github/scripts/validate-review-dirs.py`: passed.
- Gallery Playwright check: 4 images, 4 unique, all 1280x720, broken 0, console warnings/errors 0.
- Current-side review hash: baseline/generated `11_f1_f6_current.png` matched exactly.
