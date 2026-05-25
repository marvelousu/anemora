# Chapter 1 continuation map content refine cycle38

## Scope

- Branch: `work/chapter1-continuation-map-vs-20260524`
- Target map: `F1-F6` ruins and bridge (`スライド7.PNG`, `スライド14.png`)
- Focus: central river/muddy channel, low valley edges, and bridge-as-only-crossing readability.

## Changes

- Added a ruins bridge helper that reinforces the vertical north/south river or muddy channel without moving route triggers.
- Added low gorge wall cues on both sides of the channel so the center reads as a lower valley.
- Added barren/grass strip cues on the left and right sides of the channel to match the reference's low-plant/荒れた土地 zones.
- Added bridge crossing highlight and underside shadows so the horizontal bridge reads as raised above the low channel.
- Added current-only muddy wet-stain patches to keep the present channel from reading as pale paving.

## Validation

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateChapter1AllMapsBatch`: passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
- Review gallery audit: passed.

## Review Artifacts

- `docs/review/2026-05-26T02-38/reference_slide07_ruins_current.png`
- `docs/review/2026-05-26T02-38/reference_slide14_ruins_past.png`
- `docs/review/2026-05-26T02-38/11_f1_f6_current_cycle38.png`
- `docs/review/2026-05-26T02-38/12_f1_f6_past_cycle38.png`
