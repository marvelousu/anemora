# 2026-05-28 Chapter 1 Map Content Refine Cycle 127

Scope:
- F1-F6 past-only stall and right-side house solidity pass.
- Kept current side, route pads, roads, bridge, gorge/river, and main path unchanged.
- Focused on making the past field stall read as an intact structure and making the right-side houses less flimsy.

Changed:
- Added `CreateRuinsCycle127PastFieldStallAndHouseSolidifyDetails` after the cycle123 F1-F6 past cleanup.
- Added rear stall posts, side canopy bands, counter trim, display produce, and a ground basket.
- Added past exterior wall faces, roof ridges, door jambs, and a warm window inset for the right-side houses.
- Expanded cycle127 validation to cover the new stall/house trim, wall, roof, cloth, produce, and window objects.

Review:
- Review dir: `docs/review/2026-05-28T01-09/`
- Gallery: `Logs/review_gallery_2026-05-28T01-09/index.html`
- Reviewer: No findings. ACCEPT.

Verification:
- `Logs/chapter1_cycle127_validate.log`: passed.
- `Logs/chapter1_cycle127_capture.log`: passed.
- `Logs/chapter1_cycle127_validate_r2.log`: passed after validator coverage expansion.
- `Logs/chapter1_cycle127_build.log`: passed.
- `Logs/chapter1_cycle127_player_smoke.log`: passed, 30-second smoke with `ErrorLikeMatches=0`.
- `.github/scripts/validate-review-dirs.py`: passed.
- Gallery Playwright check: 4 images, 4 unique, all 1280x720, broken 0, console warnings/errors 0.
- Reviewer confirmed current-side baseline/generated image hashes match and no visible route/road/bridge/gorge/main-path damage.
