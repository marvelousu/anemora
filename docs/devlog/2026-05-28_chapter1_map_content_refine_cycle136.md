# 2026-05-28 Chapter 1 Map Content Refine Cycle 136

Scope:
- F1-F6 past-only right-field and field-stall surrounding dewood cleanup.
- Kept current side, route pads, roads, bridge, and main path unchanged.
- Focused on replacing thin deadwood-like field lines with maintained small planting details.

Changed:
- Added `CreateRuinsCycle136PastRightFieldDewoodDetails` after the right-house finish pass.
- Hid past-only old right-field rails, stake, furrow lines, crop sprig lines, and seed line that read as fallen wood at overview distance.
- Added small grass tucks, soil tucks, crop cushions, planted beds, upright fence posts, short fence caps, and flower dots around the right field.
- Added `ValidateChapter1RuinsCycle136PastRightFieldDewood` to cover hidden deadwood-like objects and new maintained-field details.

Review:
- Review dir: `docs/review/2026-05-28T04-05/`
- Gallery: `Logs/review_gallery_2026-05-28T04-05/index.html`
- Reviewer: No findings. ACCEPT.

Verification:
- `Logs/chapter1_cycle136_validate_r2.log`: passed.
- `Logs/chapter1_cycle136_capture_r2.log`: passed.
- `Logs/chapter1_cycle136_build.log`: passed.
- `Logs/chapter1_cycle136_player_smoke.log`: passed, 30-second smoke with `ErrorLikeMatches=0`.
- `.github/scripts/validate-review-dirs.py`: passed.
- Gallery Playwright check: 4 images, all 1280x720, broken 0, console warnings/errors 0.
- Current-side review hash: baseline/generated `11_f1_f6_current.png` matched exactly.
