# 2026-05-27 Chapter1 map content refine cycle109

Scope:
- Chapter 1 E1-E3 Kaia farm current-map orchard panel breakup.
- Keep past-side farm, route pads, roads, yard, and main path stable.
- Treat non-road/non-building sketch marks as approximate area cues, not literal rectangular panels.

Implementation:
- Hid current-side orchard grass/dust rectangles that still read like literal panels.
- Replaced them with smaller open grass chips, dust scuffs, sparse weeds, cut stumps, fallen branches, and bare stems spread across the orchard area.
- Left past-side E1-E3 content untouched.

Verification:
- Unity validate: passed (`Logs/chapter1_cycle109_validate.log`).
- Unity capture: passed (`Logs/chapter1_cycle109_capture.log`).
- Review gallery: passed (`Logs/review_gallery_2026-05-27T07-12/index.html`).
- Playwright gallery check: passed (7 image elements, 5 source images, broken source images 0).
- Reviewer: ACCEPT (Herschel, cycle109).
- Build: passed (`Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`).
- Player smoke: passed (`Logs/chapter1_cycle109_player_smoke.log`, 0 error-like matches).
- Review-dir validation: passed (`python .github/scripts/validate-review-dirs.py`).
