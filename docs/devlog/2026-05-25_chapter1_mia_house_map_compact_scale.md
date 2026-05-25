# Chapter 1 Mia House Map Compact Scale

Date: 2026-05-25
Branch: work/chapter1-continuation-map-vs-20260524

## Scope

Adjusted only the Chapter 1 C1-C3 Mia house continuation map after review feedback that the map area and Mia house facade were too large.

## Changes

- Reduced the C1-C3 route span so the Mia house map is no longer an over-wide area.
- Rescaled Mia's house facade, roof, volume, door, windows, base, roof lip, and front yard to match the Niro house exterior scale.
- Refit the C1 entrance, lower road, C2 front-yard connection, C3 handoff, trees, plant band, fence, stones, and time-window props around the smaller yard.
- Updated the Chapter 1 all-map screenshot capture camera for the compact Mia map.

## Validation

- `ValidateChapter1AllMapsBatch` passed.
- `CaptureChapter1AllMapsCycle05ScreenshotsBatch` regenerated the review screenshots.
- `BuildAndValidateBatch` passed and built the Windows player.
- Compared the updated C1-C3 screenshot against the map reference slide and Niro house facade scale.

## Outputs

- Screenshots: `docs/devlog/screenshots/chapter1_all_maps_cycle05/`
- Review set: `docs/review/2026-05-25T12-58/`
- Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
