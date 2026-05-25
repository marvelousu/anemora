# 2026-05-25 Chapter 1 Mia House Map Rescale

## Scope
- Refined only the Mia house continuation map after the Cycle05 map review.
- Kept the branch on `work/chapter1-continuation-map-vs-20260524`.

## Changes
- Repositioned C1 to the far-left diagonal road entrance, C2 into Mia's front yard, and C3 at the right road endpoint.
- Rescaled the Mia house map as a full-area layout instead of widening a narrow strip.
- Rebuilt the Mia house ground, main road, diagonal road, house facade, front yard, tree blocks, and lower plant band to match the reference proportions more closely.
- Preserved the existing C3/D1 connection point so the accepted Aria street map layout does not drift.

## Validation
- `ValidateChapter1AllMapsBatch`
- `CaptureChapter1AllMapsCycle05ScreenshotsBatch`
- `BuildAndValidateBatch`
- MCP Browser decode check for 13 review PNGs (`1280x720`, `complete=true`)
- Manual comparison against `map_chapter_1/map_chapter_1/スライド4.PNG`

## Outputs
- Screenshots: `docs/devlog/screenshots/chapter1_all_maps_cycle05/`
- Review packet: `docs/review/2026-05-25T12-26/`
- Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
