# 2026-05-28 Chapter 1 Map Content Refine Cycle 128

Scope:
- E1-E3 past-only farm lower-field cleanup.
- Kept current side, route pads, roads, bridge/main paths, and F1-F6 untouched.
- Focused on reducing the past farm's deadwood/debris read by removing old long lower-field rails and stacked field-edge layers.

Changed:
- Added `CreateKaiaFarmCycle128PastFieldEdgeDewoodDetails` after the cycle125 farm cleanup.
- Hid old past-only lower-field fence, rail, row, and edge-layer objects that made the clean past farm read like scattered deadwood.
- Added lower, leaf-colored field-edge trims plus sparse grass/flower details so the past lower field reads as cultivated rather than ruined.
- Added `ValidateChapter1KaiaFarmCycle128PastFieldEdgeDewood` to cover the hidden legacy layers and the new field-edge replacement details.

Review:
- Review dir: `docs/review/2026-05-28T01-44/`
- Gallery: `Logs/review_gallery_2026-05-28T01-44/index.html`
- Reviewer: No findings. ACCEPT.

Verification:
- `Logs/chapter1_cycle128_validate_r4.log`: passed.
- `Logs/chapter1_cycle128_capture_r4.log`: passed.
- `Logs/chapter1_cycle128_build.log`: passed.
- `Logs/chapter1_cycle128_player_smoke.log`: passed, 30-second smoke with `ErrorLikeMatches=0`.
- `.github/scripts/validate-review-dirs.py`: passed.
- Gallery Playwright check: 4 images, 4 unique, all 1280x720, broken 0, console warnings/errors 0.
- Current-side review hash: baseline/generated `09_e1_e3_current.png` matched exactly.
