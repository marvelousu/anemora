# 2026-05-27 Chapter1 map content refine cycle103

Scope:
- House exterior A1-A2 planting and past-side yard cleanup only.
- Continue treating tree/plant rectangles as approximate area hints, not literal green panels.
- Keep house, porch, roads, A1/A2 route markers, route pads, front road, right continuation ground, and transitions stable.

Implementation:
- Hid the A1-A2 top/left/right/lower plant bands, underbrush panels, right-side verge panels, and several secondary lower canopies.
- Current side: reduced healthy tree density and added sparse dust pockets, dry branches, broken stakes, stones, and tufts spread around the yard.
- Past side: removed old log, stone, and crate clutter from the yard/road edges, then added short clean rails and flower patches.
- Left the current-side debris read intact while preventing the same debris language from carrying into the past version.
- Added explicit validation for the hidden lower canopies so the old tree-band read cannot return silently.

Verification:
- Unity validate: passed (`Logs/chapter1_cycle103_validate_r3.log`).
- Unity capture: passed (`Logs/chapter1_cycle103_capture_r3.log`).
- Review gallery: passed (`Logs/review_gallery_2026-05-27T05-09/index.html`, 4 images indexed).
- Playwright gallery check: passed (5 image elements, 4 unique image sources, broken 0).
- Reviewer: ACCEPT. A1/A2 current no longer reads as literal green plant panels or dense healthy tree bands; past is cleaner; house, porch, roads, markers, route pads, right continuation ground, and transitions remain stable. Low validation note addressed by adding lower-canopy inactive checks.
- Build: passed (`Logs/chapter1_cycle103_build.log`).
- Player smoke: passed (`Logs/chapter1_cycle103_player_smoke.log`, killed after smoke window, error-like matches 0).
- Review-dir validation: passed.
