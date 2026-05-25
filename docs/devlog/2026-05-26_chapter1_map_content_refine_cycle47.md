# Chapter 1 Map Content Refine Cycle 47

## Scope
- Branch: `work/chapter1-continuation-map-vs-20260524`
- Target: Kaia farm E1-E3 continuation map.
- Reference: `map_chapter_1` slides 6 and 13.
- Goal: address cycle46 follow-up notes: lower field readability, E2 house/front-yard organization, and current/past farm-state difference through content rather than lighting alone.

## Changes
- Added `CreateKaiaFarmCycle47FieldYardStateDetails` and wired it after the cycle46 Kaia-farm helper.
- Added clearer lower-field blocks, row/edge markers, and crop accents so the south field reads more as a farmed area.
- Kept current rows sparser and more broken while making past rows fuller and more cultivated.
- Added open E2 front-yard floor/path patches and moved the new plant/stone/debris accents toward yard edges to make the yard read as one intentional area.
- Added small E3 exit-side ground and plant/broken-edge details without changing route anchors or camera framing.
- Parent adjusted the current lower-field row material from dust to current-path material after visual capture, so the rows read more as dry soil than white scrape marks.

## Review
- cycle-worker `019e60ac-3004-75d1-a54f-37dab6ea30cf` added the helper and single call-site wiring.
- Parent visual review rejected the first capture's too-white current field rows and corrected the material before final capture.
- Post-change reviewer `019e60b2-1a5f-7352-ada8-62fe57e453fd` found no blocking anomalies. Remaining follow-up candidates are reducing E2 front-yard density and grouping the past lower-field crop rows into slightly clearer bands.

## Validation
- `ValidateChapter1AllMapsBatch`: passed.
- `CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
- `BuildAndValidateBatch`: passed.
- Player smoke: fatal match count 0.
- Review gallery: `docs/review/2026-05-26T04-55/`.
