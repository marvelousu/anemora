# 2026-05-28 Chapter 1 Map Content Refine Cycle 126

Scope:
- F1-F6 past-only gorge/river readability pass.
- Kept current-side debris, bridge, roads, route pads, and main path unchanged.
- Replaced the raised-looking cycle121 gorge water slabs with lower, narrower cycle126 water ribbons and added side/mouth depth shadows.

Changed:
- Added `CreateChapter1Cycle126RuinsPastDeepGorgeReadDetails` after the cycle121 gorge pass.
- Hid the three cycle121 past gorge water objects while preserving the cycle121 stone walls and floor shadows.
- Added validation for the replacement water/shadow objects and the inactive old water slabs.

Review:
- Review dir: `docs/review/2026-05-28T00-50/`
- Gallery: `Logs/review_gallery_2026-05-28T00-50/index.html`
- Reviewer: No findings. ACCEPT.

Verification:
- `Logs/chapter1_cycle126_validate.log`: passed.
- `Logs/chapter1_cycle126_capture.log`: passed.
- `Logs/chapter1_cycle126_build.log`: passed.
- `Logs/chapter1_cycle126_player_smoke.log`: passed, 30-second smoke with `ErrorLikeMatches=0`.
- `.github/scripts/validate-review-dirs.py`: passed.
- Gallery Playwright check: 4 images, 4 unique, all 1280x720, broken 0, console warnings/errors 0.
- Reviewer confirmed current-side baseline/generated image match and no visible bridge/road/route/main-path damage.
