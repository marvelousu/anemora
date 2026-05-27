# 2026-05-28 Chapter 1 Map Content Refine Cycle 139

Scope:
- F1-F6 Ruins past-only lower-left house finish.
- Kept current side, route pads, roads, bridge, and main paths unchanged.
- Focused on making the past bottom house row read as complete homes instead of thin cleaned-up ruins.

Changed:
- Added `CreateRuinsCycle139PastLowerLeftHouseFinishDetails` after the cycle136 F1-F6 cleanup pass.
- Added front wall faces, fuller roof planes, bold front eaves, ridge trim, lower wall belts, door headers, warm window panes, small flower boxes, window awnings, door stones, front garden strips, and bloom accents to the three past bottom houses.
- Added `ValidateChapter1RuinsCycle139PastLowerLeftHouseFinish` with coverage for all new cycle139 house landmarks.

Review:
- Review dir: `docs/review/2026-05-28T05-14/`
- Gallery: `Logs/review_gallery_2026-05-28T05-14/index.html`
- Reviewer: Initial visual-delta finding fixed in r2. ACCEPT.

Verification:
- `Logs/chapter1_cycle139_validate_r6.log`: passed.
- `Logs/chapter1_cycle139_capture_r6.log`: passed.
- `Logs/chapter1_cycle139_build.log`: passed.
- `Logs/chapter1_cycle139_player_smoke.log`: passed, `ErrorLikeMatches=0`.
- `.github/scripts/validate-review-dirs.py`: passed.
- Gallery Playwright check: 4 images, all 1280x720, broken 0, console warnings/errors 0.
- Current-side review hash: baseline/generated `11_f1_f6_current.png` matched exactly.
- Past-side review hash: baseline/generated `12_f1_f6_past.png` differed as expected.
- Pixel-diff gate: `changed_pixels=4695`, `diff_bbox=0,462,298,499`, `current_hash_match=true`.
