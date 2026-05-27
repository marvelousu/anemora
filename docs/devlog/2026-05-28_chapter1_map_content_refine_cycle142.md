# 2026-05-28 Chapter 1 Map Content Refine Cycle 142

## Scope

- F1-F6 ruins map, past side only.
- Targeted the right-side house row that still read as thin, unfinished, and lower than the surrounding past-side polish.
- Kept current-side debris, route pads, roads, bridge, and the main path out of scope.

## Change

- Added `CreateRuinsCycle142PastRightHouseRowFinishDetails`.
- Added clean wall overlays, fuller roof planes, tile/eave caps, warm door panels, lanterns, shutters, flower boxes, small front garden patches, a compact laundry line, and a small way sign to the three right-side past houses.
- Disabled shadow casting and receiving on cycle142 overlays so the right house pass stays visually localized.
- Added `ValidateChapter1RuinsCycle142PastRightHouseRowFinish` coverage for the new landmarks and their no-shadow renderer state.

## Review Packet

- Review dir: `docs/review/2026-05-28T07-03/`
- Gallery: `Logs/review_gallery_2026-05-28T07-03/index.html`
- Pixel gate: `current_hash_match=true`; past diff is localized to the right house row.

## Verification

- Unity validate: passed (`Logs/chapter1_cycle142_validate_r1.log`).
- Unity capture: passed (`Logs/chapter1_cycle142_capture_r1.log`).
- Pixel gate: `past_changed_pixels=3487`, `past_diff_bbox=768,352,1036,420`, `past_right_house_row_region_changed_pixels=3487`, `past_outside_right_house_row_region_changed_pixels=0`, `current_hash_match=true`.
- Gallery/Playwright: passed; all four review images loaded at 1280x720 with current-page warning/error count 0.
- Reviewer: ACCEPT.
- Review dir validation: passed (`python .github\scripts\validate-review-dirs.py`).
- Build: passed (`Logs/chapter1_cycle142_build.log`).
- Player smoke: passed with no error-like log lines (`Logs/chapter1_cycle142_player_smoke.log`).
