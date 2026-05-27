# 2026-05-28 Chapter 1 Map Content Refine Cycle 131

Scope:
- F1-F6 past-only left-side house row finish.
- Kept current side, route pads, roads, bridge, main path, gorge, stall, and farm layout untouched.
- Focused on making the past-side left settlement houses read less like plain low blockouts.

Changed:
- Added `CreateRuinsCycle131PastLeftHouseFinishDetails` after the cycle130 stall finish pass.
- Added small per-house eave shadows, corner posts, stone door steps, window sills, and window crossbars across the top and bottom left house rows.
- Added `ValidateChapter1RuinsCycle131PastLeftHouseFinish` to cover the new house-row details and material tokens.

Review:
- Review dir: `docs/review/2026-05-28T02-33/`
- Gallery: `Logs/review_gallery_2026-05-28T02-33/index.html`
- Reviewer: No findings. ACCEPT.

Verification:
- `Logs/chapter1_cycle131_validate.log`: passed.
- `Logs/chapter1_cycle131_capture.log`: passed.
- `Logs/chapter1_cycle131_build.log`: passed.
- `Logs/chapter1_cycle131_player_smoke.log`: passed, 30-second smoke with `ErrorLikeMatches=0`.
- `.github/scripts/validate-review-dirs.py`: passed.
- Gallery Playwright check: 4 images, 4 unique, all 1280x720, broken 0, console warnings/errors 0.
- Current-side review hash: baseline/generated `11_f1_f6_current.png` matched exactly.
