# Chapter 1 Continuation Map Refine 4

## Scope

- Continued work on `work/chapter1-continuation-map-vs-20260524`.
- Used the reference images under `C:\Users\maro6\OneDrive\work\projects\anemora_reference\map_chapter_1\map_chapter_1`.
- Kept the start state after the library event via the existing `startAfterLibraryEvent` setup.
- Did not refer to `work/chapter1-continuation-20260520`.

## Changes

- Added a Cycle04 all-map capture batch with wider review framing for the continuation maps.
- Expanded C/D/E/F map route spans so the outer movement points sit near the reference-map edges instead of clustered near the center.
- Repositioned D2/D3 so D2 sits on the plaza-side house edge and D3 sits beyond the right diagonal road.
- Repositioned E2 between Kaia house and its front yard, with E1/E3 spread across the farm field.
- Expanded F from the far-left entry through the bridge to the far-right endpoint, including wider road, valley, settlement, and wasteland support geometry.
- Increased continuation-map invisible boundaries so the wider maps remain traversable.

## Validation

- `ValidateChapter1AllMapsBatch`
- `CaptureChapter1AllMapsCycle04ScreenshotsBatch`
- `BuildAndValidateBatch`
- `python .github\scripts\validate-review-dirs.py`
- MCP Browser PNG decode check for all 13 files in `docs/review/2026-05-25T10-03/`

## Review

- Review directory: `docs/review/2026-05-25T10-03/`
- Raw capture directory: `docs/devlog/screenshots/chapter1_all_maps_cycle04/`
