# 2026-05-28 Chapter 1 Map Content Refine Cycle 143

## Scope

- F1-F6 ruins map, past side only.
- Targeted the upper-left house row that still read as thin and less finished than the cleaner past settlement passes.
- Kept current-side debris, route pads, roads, bridge, river/gorge, and the main path out of scope.

## Change

- Added `CreateRuinsCycle143PastUpperLeftHouseRowFinishDetails`.
- Added clean wall planes, fuller roof planes, bright tile lines, end caps, warm doors, lintels, lanterns, porch slabs, shutters, flower boxes, front garden patches, and a small laundry/planter accent to the three upper-left past houses.
- Disabled shadow casting and receiving on cycle143 overlays so the pass stays visually localized.
- Added `ValidateChapter1RuinsCycle143PastUpperLeftHouseRowFinish` coverage for the new landmarks and their no-shadow renderer state.

## Review Packet

- Review dir: `docs/review/2026-05-28T07-20/`
- Gallery: `Logs/review_gallery_2026-05-28T07-20/index.html`
- Pixel gate: `current_hash_match=true`; past diff is localized to the upper-left house row.

## Verification

- Unity validate: passed (`Logs/chapter1_cycle143_validate_r1.log`).
- Unity capture: passed (`Logs/chapter1_cycle143_capture_r1.log`).
- Pixel gate: `past_changed_pixels=3467`, `past_diff_bbox=111,316,367,370`, `past_upper_left_house_row_region_changed_pixels=3467`, `past_outside_upper_left_house_row_region_changed_pixels=0`, `current_hash_match=true`.
- Gallery/Playwright: passed; all four review images loaded at 1280x720 with current-page warning/error count 0.
- Reviewer: ACCEPT.
- Review dir validation: passed (`python .github\scripts\validate-review-dirs.py`).
- Build: passed (`Logs/chapter1_cycle143_build.log`).
- Player smoke: passed with no error-like log lines (`Logs/chapter1_cycle143_player_smoke.log`).
