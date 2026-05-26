# Chapter 1 Map Content Refine Cycle 66

## Scope

- Branch: `work/chapter1-continuation-map-vs-20260524`
- Target: ruins current/past map.
- Goal: improve non-graphical content and placement readability around house separation, right-side ruin shell shape, low bridge/channel cues, and rough-land/stall props without changing route centers or map scale.

## Changes

- Added `CreateRuinsCycle66HouseCliffAndPropDetails`.
- Wired it after the cycle 63 low-cliff/settlement pass.
- Added left-settlement separation cues:
  - small path-colored gap strips between upper/lower house facades,
  - threshold stones,
  - sparse brush in the inner plaza/road pockets.
- Added right-side ruin shell cues:
  - F5 side walls, roof hint, divider post, and threshold,
  - F6 exit shell wall and side threshold stones.
- Added bridge/low-channel cues:
  - shallow bank/lip strips,
  - sparse brush beside the bridge while keeping the main bridge walkway clear.
- Added right rough-land and old-stall props:
  - stall frame/counter/threshold,
  - rough-land patches and small broken/tidy fragments,
  - low organic brush clumps.

## Validation

- `ValidateChapter1AllMapsBatch`: passed.
- `CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
- Post-review fix:
  - A reviewer flagged the first F6 threshold as overlapping the route pad footprint.
  - Replaced it with upper/lower side stones outside the F6 route pad and main road centerline.
  - Re-ran `ValidateChapter1AllMapsBatch`: passed.
  - Re-ran `CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.

## Review

- Review directory: `docs/review/2026-05-26T09-55`
- Included generated ruins current/past captures and reference slides 7/14.
