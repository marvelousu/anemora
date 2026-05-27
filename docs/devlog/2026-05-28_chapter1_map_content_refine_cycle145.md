# 2026-05-28 Chapter 1 Map Content Refine Cycle 145

## Scope

- F1-F6 ruins map, past side only.
- Targeted the lower-left past house row, which still read thinner than the upper-left and right-side houses after the previous passes.
- Kept current-side debris, route pads, roads, bridge, river/gorge, and the main path out of scope.

## Change

- Added `CreateRuinsCycle145PastLowerLeftHouseRowFinishDetails`.
- Added stronger past-side house silhouettes with clean wall planes, roof end caps, bright roof lines, door trim, shutters, awnings, porch slabs, front gardens, baskets, a short fence line, laundry, a shared small sign, and planter details.
- Disabled shadow casting and receiving on cycle145 overlays so the pass stays visually localized.
- Added `ValidateChapter1RuinsCycle145PastLowerLeftHouseRowFinish` coverage for the new landmarks and their no-shadow renderer state.

## Review Packet

- Review dir: `docs/review/2026-05-28T07-56/`
- Gallery: `Logs/review_gallery_2026-05-28T07-56/index.html`
- Pixel gate: `current_hash_match=true`; past diff is localized to the lower-left house row.

## Verification

- Unity validate: passed (`Logs/chapter1_cycle145_validate_r1.log`).
- Unity capture: passed (`Logs/chapter1_cycle145_capture_r1.log`).
- Pixel gate: `past_changed_pixels=2836`, `past_diff_bbox=0,461,299,497`, `past_lower_left_region=0,360,410,560`, `past_outside_lower_left_region_changed_pixels=0`, `current_hash_match=true`.
- Gallery/Playwright: passed; all four review images loaded at 1280x720 with current-page warning/error count 0.
- Reviewer: ACCEPT.
- Review dir validation: passed (`python .github\scripts\validate-review-dirs.py`).
- Build: passed (`Logs/chapter1_cycle145_build.log`).
- Player smoke: passed with no error-like log lines (`Logs/chapter1_cycle145_player_smoke.log`).
