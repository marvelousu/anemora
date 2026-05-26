# 2026-05-27 Chapter1 map content refine cycle106

Scope:
- Chapter 1 F1-F6 ruins past-map right settlement/right field cleanup.
- Keep current-side debris, F route pads, bridge/roads/buildings, and Time Window behavior stable.
- Treat non-road/non-building sketch marks as approximate area cues, not literal rectangular panels.

Implementation:
- Hid past-side lower-right stall frames, counter lip, rough land rectangles, debris fragments, and dense lower brush that still read like current-side damage.
- Added thin clean lane/grass blend cues plus broad, low furrows and sparse crop lines so the lower-right area reads as an orderly past-side cultivated field rather than scattered debris.
- Left current-side F1-F6 ruins and debris untouched.

Verification:
- Unity validate: passed (`Logs/chapter1_cycle106_validate_r3.log`).
- Unity capture: passed (`Logs/chapter1_cycle106_capture_r2.log`).
- Review gallery: passed (`Logs/review_gallery_2026-05-27T06-27/index.html`).
- Playwright gallery check: passed (7 image elements, 5 source images, broken source images 0).
- Reviewer: ACCEPT.
- Build: passed (`Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`).
- Player smoke: passed (`Logs/chapter1_cycle106_player_smoke.log`, 0 error-like matches).
- Review-dir validation: passed (`python .github/scripts/validate-review-dirs.py`).
