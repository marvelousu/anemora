# Chapter 1 Map Content Refine Cycle 13

Branch: `work/chapter1-continuation-map-vs-20260524`

## Scope

- Continue iterative refinement on the published VS-derived continuation branch.
- Keep route trigger centers, transition targets, map centers, and capture cameras unchanged.
- Focus on street-corner plaza readability around D2 and the right-side ruin/grass area.

## Changes

- Added `CreateStreetCornerPlazaReadabilityDetails` and wired it into the Aria street plaza continuation.
- Added low steps around the central stage so it reads as the plaza's platform rather than an isolated box.
- Added right-ruin footprint/threshold/rubble pieces so the ruin keeps a house-like footprint.
- Added lower grass pockets, a right-side tree cluster, and small tufts to better match the reference's grass/tree band without blocking the D1/D3 road.

## Review

- Cycle13 targets the remaining street-corner issue from the visual QA: the plaza should keep an open square while perimeter ruins/stalls and the right ruin read as intentional content.
- Changes avoid moving D1, D2, or D3 and keep the diagonal D3 road shape unchanged.

## Validation

- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`: passed.
- `ValidateChapter1AllMapsBatch`: passed (`Logs/chapter1_cycle13_validate_r1.log`).
- `CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed (`Logs/chapter1_cycle13_capture_r1.log`).

## Review Images

Directory: `docs/review/2026-05-25T18-37`

- `07_d1_d3_current.png`
- `08_d1_d3_past.png`
