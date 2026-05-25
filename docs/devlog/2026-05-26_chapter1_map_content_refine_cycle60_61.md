# Chapter 1 Map Content Refine Cycle 60-61

## Scope

- Targeted the D1-D3 street corner continuation map after the user asked to keep improving placement, contents, small props, and plants over graphical polish.
- Goal: make the street corner read closer to the reference: four stall blocks under the north fence, a single central stage in the plaza, clearer current/past ruin contrast on the left houses, and a less confusing northeast D3 road.

## Changes

- Added `CreateStreetCornerCycle60PlazaStallHierarchyDetails(...)` and wired it into the D street corner map flow.
- Reinforced the four-stall row with small per-stall rear patches, thresholds, low counters, current rubble/broken posts, and past cloth ties/crates without turning the row into one continuous platform.
- Added thin stage sight lines, plank accents, posts, and current-only stage breakage while keeping the central stage as the dominant shape.
- Changed current-only left D1 ruin base geometry so the two left blocks no longer keep the same full house wall/roof mass as the past map.
- Shortened and simplified the D3 visible exit road, reduced side shoulders, and converted several old D3 path-looking nibbles into grass/dust so the route reads as one diagonal road rather than a branching cluster.
- Kept route trigger constants, route stops, colliders, map centers, capture code, and camera settings unchanged.

## Review

- Initial D-only reviewer found no High issues, but flagged Cycle60 as over-layering existing stalls/stage. The cycle was revised so the additions became small readability accents instead of another full set of structures.
- Second D-only reviewer confirmed the over-layering concern was resolved, then flagged the D3 route as too branch-like and the current left ruins as too house-like. Cycle61 addressed both before commit.
- Remaining watch item: the D3 road can still be refined further if later all-map review wants an even simpler road silhouette, but it is now less cluttered and functionally validated.

## Verification

- Unity `ValidateChapter1AllMapsBatch`: passed (`Logs/chapter1_cycle61b_validate.log`).
- Unity `CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed (`Logs/chapter1_cycle61b_capture.log`).
- Unity `BuildAndValidateBatch`: passed (`Logs/chapter1_cycle61b_build.log`).
- Player smoke: no fatal matches (`Logs/chapter1_cycle61b_player_smoke.log`).

## Review Artifacts

- `docs/review/2026-05-26T08-21/reference_slide05_d_current.png`
- `docs/review/2026-05-26T08-21/reference_slide12_d_past.png`
- `docs/review/2026-05-26T08-21/generated_07_d1_d3_current_cycle61.png`
- `docs/review/2026-05-26T08-21/generated_08_d1_d3_past_cycle61.png`
