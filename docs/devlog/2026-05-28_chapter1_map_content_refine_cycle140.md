# 2026-05-28 Chapter 1 Map Content Refine Cycle 140

## Scope

- F1-F6 ruins map, past side only.
- Targeted the central river/gorge that still read as raised instead of deeply cut.
- Kept current-side debris, route pads, roads, bridge, and the main path out of scope.

## Change

- Added `CreateChapter1Cycle140RuinsPastGorgeDeepCutDetails`.
- Added darker abyss cores, vertical drop faces, bright stone rim strips, mouth curtains/sills, bridge under-lip shadow, and very low water pins.
- Disabled shadow casting and shadow receiving on the Cycle140 gorge overlays so the central gorge pass does not spill lighting changes onto nearby houses.
- Added `ValidateChapter1RuinsCycle140PastGorgeDeepCut` coverage for the new landmarks and their no-shadow renderer state.

## Review Packet

- Review dir: `docs/review/2026-05-28T05-57/`
- Gallery: `Logs/review_gallery_2026-05-28T05-57/index.html`
- Pixel gate: `current_hash_match=true`; past side changed only for the generated past review image.

## Verification

- Unity validate: passed after reviewer r1 fix (`Logs/chapter1_cycle140_validate_r2.log`).
- Unity capture: passed after reviewer r1 fix (`Logs/chapter1_cycle140_capture_r2.log`).
- Gallery/Playwright: passed after r2 image refresh; all four review images loaded at 1280x720 with current-page warning/error count 0.
- Pixel gate: `changed_pixels=4858`, `diff_bbox=595,343,669,527`, `current_hash_match=true`, `lower_left_house_region_changed_pixels=0`.
- Review dir validation: passed (`python .github\scripts\validate-review-dirs.py`).
- Reviewer: ACCEPT after r3.
- Build: passed (`Logs/chapter1_cycle140_build.log`).
- Player smoke: passed with no error-like log lines (`Logs/chapter1_cycle140_player_smoke.log`).
