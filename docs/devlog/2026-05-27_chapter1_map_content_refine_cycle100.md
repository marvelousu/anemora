# 2026-05-27 Chapter1 map content refine cycle100

Scope:
- Mia house C1-C3 tree / plant areas only.
- Treat user-drawn tree rectangles as approximate area hints, not literal panels.
- Keep C1-C3 roads, route pads, house, front yard, and transition readability stable.

Implementation:
- Hid the old left/right C1 tree block panels on both current and past maps.
- Added smaller rotated open patches and spread tufts so the tree area reads as loose vegetation rather than a rectangular green slab.
- Current side: thinned several dense tree canopies and added dust / stump / dry-branch cues.
- Past side: hid the log/root clutter from the same tree zone and replaced it with clean low rails and a small bloom cue.

Verification:
- Unity validate: passed (`Logs/chapter1_cycle100_validate.log`).
- Unity capture: passed (`Logs/chapter1_cycle100_capture.log`).
- Review gallery: passed (`Logs/review_gallery_2026-05-27T04-12/index.html`, 4 images indexed).
- Playwright gallery check: passed (5 image elements, 4 unique image sources, broken 0).
- Reviewer: ACCEPT. Top-left/top-right C1 tree areas no longer read as dense exact panels; current is sparser/more ruined, past is cleaner, and route pads/roads/house/yard/C3 transition remain stable.
- Build: passed (`Logs/chapter1_cycle100_build.log`).
- Player smoke: passed (`Logs/chapter1_cycle100_player_smoke.log`, killed after smoke window, error-like matches 0).
- Review-dir validation: passed.
