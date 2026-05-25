# Chapter 1 map content refine cycle53

Date: 2026-05-26
Branch: work/chapter1-continuation-map-vs-20260524

## Scope

- Targeted the F1-F6 ruins continuation map after the all-map visual review.
- Kept route trigger positions and the VS area unchanged.
- Focused on F2 lower-left settlement framing and route readability, not graphic polish.

## Changes

- Added `CreateRuinsCycle53LowerLeftFrameReadabilityDetails(...)` and wired it into the F ruins side-home flow after the F2 lower-house separation details.
- F2 lower-left:
  - added an in-frame lower road lip, porch pads, and grass/path shoulders around the bottom house row,
  - added current/past-specific small cues so the lower-left road loop remains readable even near the screenshot edge,
  - kept the reference tree/fence/ground zones as approximate placement zones rather than rectangular enclosures.
- Capture:
  - adjusted only the F1-F6 review capture offset slightly wider/farther so F1, F2, bridge, F5, and F6 remain visible together.

## Review

- Subagent all-map review flagged F1-F6 lower-left settlement and F2 route network as clipped/compressed.
- Parent visual review compared:
  - `docs/devlog/screenshots/chapter1_all_maps_cycle05/11_f1_f6_current.png`
  - `docs/devlog/screenshots/chapter1_all_maps_cycle05/12_f1_f6_past.png`
  - F current/past reference diagrams.
- Result: the lower-left house row and F2 lower road read more clearly in frame while keeping the whole F1-F6 route network visible.

## Validation

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
- Unity `ValidateChapter1AllMapsBatch`
  - log: `Logs/chapter1_cycle53_validate.log`
  - result: passed
- Unity `CaptureChapter1AllMapsCycle05ScreenshotsBatch`
  - log: `Logs/chapter1_cycle53_capture.log`
  - result: passed
- Unity `BuildAndValidateBatch`
  - log: `Logs/chapter1_cycle53_build.log`
  - result: passed
  - note: log included nonfatal Bee caching-client warnings, with Unity exit code 0.
- Player smoke
  - log: `Logs/chapter1_cycle53_player_smoke.log`
  - result: no fatal matches

## Review Assets

- `docs/review/2026-05-26T06-35/01_f1_f6_current.png`
- `docs/review/2026-05-26T06-35/02_f1_f6_past.png`
