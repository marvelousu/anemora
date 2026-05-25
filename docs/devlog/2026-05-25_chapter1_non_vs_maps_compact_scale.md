# Chapter 1 Non-VS Maps Compact Scale

Date: 2026-05-25
Branch: work/chapter1-continuation-map-vs-20260524

## Scope

Compacted the non-public-VS Chapter 1 continuation maps after the Mia house adjustment. The public VS range was intentionally left untouched.

## Changes

- Reduced the D1-D3 Aria street map span, boundary, plaza floor, road bands, stalls, and ruin-side props.
- Reduced the E1-E3 Kaia farm map span, boundary, field footprint, farm lanes, fence references, right grass patches, and front-yard connections.
- Reduced the F1-F6 ruins map span, boundary, settlement ground, bridge/valley footprint, roads, wasteland blocks, and ruin house clusters.
- Tightened the Cycle05 capture framing for D/E/F to match the reduced map footprint.

## Validation

- cycle-worker used for the single authored-file edit scope.
- `ValidateChapter1AllMapsBatch` passed.
- `CaptureChapter1AllMapsCycle05ScreenshotsBatch` regenerated the review screenshots.
- `BuildAndValidateBatch` passed and built the Windows player.
- Manual screenshot check against reference slides 5, 6, and 7.

## Outputs

- Screenshots: `docs/devlog/screenshots/chapter1_all_maps_cycle05/`
- Review set: `docs/review/2026-05-25T13-27/`
- Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
