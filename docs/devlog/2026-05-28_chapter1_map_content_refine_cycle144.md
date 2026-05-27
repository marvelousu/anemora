# 2026-05-28 Chapter 1 Map Content Refine Cycle 144

## Scope

- F1-F6 ruins map, past side only.
- Targeted the upper-left settlement market/front-yard stalls that still read as loose boards instead of complete past-side shop fronts.
- Kept current-side debris, route pads, roads, bridge, river/gorge, and the main path out of scope.

## Change

- Added `CreateRuinsCycle144PastUpperLeftMarketStallFinishDetails`.
- Added two compact past-side market stalls with stone aprons, counter faces, counter tops, four-post frames, full cloth roofs, bright valances, back bands, cross beams, produce stacks, baskets, small lanterns, a shared sign, and a flower pot.
- Disabled shadow casting and receiving on cycle144 overlays so the pass stays visually localized.
- Added `ValidateChapter1RuinsCycle144PastUpperLeftMarketStallFinish` coverage for the new landmarks and their no-shadow renderer state.

## Review Packet

- Review dir: `docs/review/2026-05-28T07-36/`
- Gallery: `Logs/review_gallery_2026-05-28T07-36/index.html`
- Pixel gate: `current_hash_match=true`; past diff is localized to the upper-left market/front-yard stall band.

## Verification

- Unity validate: passed (`Logs/chapter1_cycle144_validate_r1.log`).
- Unity capture: passed (`Logs/chapter1_cycle144_capture_r1.log`).
- Pixel gate: `past_changed_pixels=2529`, `past_diff_bbox=118,345,270,386`, `past_upper_left_market_region_changed_pixels=2529`, `past_outside_upper_left_market_region_changed_pixels=0`, `current_hash_match=true`.
- Gallery/Playwright: passed; all four review images loaded at 1280x720 with current-page warning/error count 0.
- Reviewer: ACCEPT.
- Review dir validation: passed (`python .github\scripts\validate-review-dirs.py`).
- Build: passed (`Logs/chapter1_cycle144_build.log`).
- Player smoke: passed with no error-like log lines (`Logs/chapter1_cycle144_player_smoke.log`).
