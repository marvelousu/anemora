# 2026-05-28 Chapter 1 Map Content Refine Cycle 141

## Scope

- F1-F6 ruins map, past side only.
- Targeted the right farm and field stall that still read as sparse or partially ruined.
- Kept current-side debris, route pads, roads, bridge, and the main path out of scope.

## Change

- Added `CreateRuinsCycle141PastRightFarmStallFinishDetails`.
- Completed the past field stall with a stronger canopy outline, rear cloth wall, braces, counter panels, side sign, visible banner, produce baskets, and counter flowers.
- Added tidy past-side crop/flower clumps and harvest baskets in the right field without turning the area into a rectangular green panel.
- Hid the old right-field basket-like debris and adjusted existing stall roof/apron pieces so the stall reads cleaner in the review camera.
- Added `ValidateChapter1RuinsCycle141PastRightFarmStallFinish` coverage, including material checks, adjusted existing stall-piece placement, and no-shadow checks for the new cycle141 overlays.

## Review Packet

- Review dir: `docs/review/2026-05-28T06-28/`
- Gallery: `Logs/review_gallery_2026-05-28T06-28/index.html`
- Pixel gate: `current_hash_match=true`; past diff is localized to the right farm/stall region.

## Verification

- Unity validate: passed after r6 (`Logs/chapter1_cycle141_validate_r6.log`).
- Unity capture: passed after r6 (`Logs/chapter1_cycle141_capture_r6.log`).
- Pixel gate: `past_changed_pixels=3266`, `past_diff_bbox=886,427,1127,499`, `past_right_farm_region_changed_pixels=3266`, `past_outside_right_farm_region_changed_pixels=0`, `current_hash_match=true`.
- Gallery/Playwright: passed; all four review images loaded at 1280x720 with current-page warning/error count 0.
- Reviewer: ACCEPT after side-effect cleanup.
- Review dir validation: passed (`python .github\scripts\validate-review-dirs.py`).
- Build: passed (`Logs/chapter1_cycle141_build.log`).
- Player smoke: passed with no error-like log lines (`Logs/chapter1_cycle141_player_smoke.log`).
