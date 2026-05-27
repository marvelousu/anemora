# 2026-05-28 Chapter 1 Map Content Refine Cycle 132

Scope:
- E1-E3 past-only farm and orchard dewood cleanup.
- Kept current side, route pads, roads, bridge, and main paths untouched.
- Focused on reducing the remaining long deadwood/rail read in the past farm lower edge and orchard bands.

Changed:
- Added `CreateKaiaFarmCycle132PastOrchardAndFieldDewoodDetails` after the cycle128 farm cleanup pass.
- Hid past-only long low orchard rails, the lower separator fence hint, and older long lower-field edge/foreground strips that still read as fallen wood.
- Added shorter grass, soil, leaf, and flower accents so the past farm reads as tended and natural instead of debris-strewn.
- Added `ValidateChapter1KaiaFarmCycle132PastOrchardAndFieldDewood` to cover hidden objects and new replacement details.

Review:
- Review dir: `docs/review/2026-05-28T02-53/`
- Gallery: `Logs/review_gallery_2026-05-28T02-53/index.html`
- Reviewer: No findings. ACCEPT.

Verification:
- `Logs/chapter1_cycle132_validate_r2.log`: passed.
- `Logs/chapter1_cycle132_capture_r2.log`: passed.
- `Logs/chapter1_cycle132_build.log`: passed.
- `Logs/chapter1_cycle132_player_smoke.log`: passed, 30-second smoke with `ErrorLikeMatches=0`.
- `.github/scripts/validate-review-dirs.py`: passed.
- Gallery Playwright check: 4 images, 4 unique sources, 3 unique hashes because current baseline/generated matched, all 1280x720, broken 0, console warnings/errors 0.
- Current-side review hash: baseline/generated `09_e1_e3_current.png` matched exactly.
