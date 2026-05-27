# 2026-05-28 Chapter 1 Map Content Refine Cycle 135

Scope:
- F1-F6 past-only right-house finish.
- Kept current side, route pads, roads, bridge, and main path unchanged.
- Focused on making the right-side past houses read more finished from the all-map overview.

Changed:
- Added `CreateRuinsCycle135PastRightHouseFinishDetails` after the cycle131 house finish pass.
- Added roof front eaves, roof crest trims, eave shadows, lower wall belts, corner posts, door step stones, porch rails, window sills, and window crossbars to `RightHouse`, `RightPairA`, and `RightPairB`.
- Added `ValidateChapter1RuinsCycle135PastRightHouseFinish` to cover the new right-house trim and finish details.

Review:
- Review dir: `docs/review/2026-05-28T03-44/`
- Gallery: `Logs/review_gallery_2026-05-28T03-44/index.html`
- Reviewer: No findings. ACCEPT.

Verification:
- `Logs/chapter1_cycle135_validate_r2.log`: passed.
- `Logs/chapter1_cycle135_capture_r2.log`: passed.
- `Logs/chapter1_cycle135_build.log`: passed.
- `Logs/chapter1_cycle135_player_smoke.log`: passed, 30-second smoke with `ErrorLikeMatches=0`.
- `.github/scripts/validate-review-dirs.py`: passed.
- Gallery Playwright check: 4 images, all 1280x720, broken 0, console warnings/errors 0.
- Current-side review hash: baseline/generated `11_f1_f6_current.png` matched exactly.
