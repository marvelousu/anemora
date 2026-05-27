# 2026-05-28 Chapter 1 Map Content Refine Cycle 133

Scope:
- F1-F6 past-only gorge and river depth read.
- Kept current side, route pads, roads, bridge, and main path unchanged.
- Focused on making the past river channel read as a deep cut instead of a raised blue surface.

Changed:
- Added `CreateChapter1Cycle133RuinsPastGorgeWallDepthDetails` after the cycle129 gorge-depth pass.
- Hid the remaining cycle129 past water threads that still read too high from the overview camera.
- Added lower bottom voids, very small low water glints, stronger inner wall shadows, stone lips, and bridge-mouth deep-drop occlusion.
- Added `ValidateChapter1RuinsCycle133PastGorgeWallDepth` to cover hidden high water threads and new depth cues.

Review:
- Review dir: `docs/review/2026-05-28T03-12/`
- Gallery: `Logs/review_gallery_2026-05-28T03-12/index.html`
- Reviewer: No blocking findings. ACCEPT.

Verification:
- `Logs/chapter1_cycle133_validate.log`: passed.
- `Logs/chapter1_cycle133_capture.log`: passed.
- `Logs/chapter1_cycle133_build.log`: passed.
- `Logs/chapter1_cycle133_player_smoke.log`: passed, 30-second smoke with `ErrorLikeMatches=0`.
- `.github/scripts/validate-review-dirs.py`: passed.
- Gallery Playwright check: 4 images, all 1280x720, broken 0, console warnings/errors 0.
- Current-side review hash: baseline/generated `11_f1_f6_current.png` matched exactly.
