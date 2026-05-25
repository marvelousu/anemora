# Chapter1 Map Content Refine Cycle22

## Scope

- Branch: `work/chapter1-continuation-map-vs-20260524`
- Commit target: F2 lower-row ruin-house separation and ruins review framing.
- Reference: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\map_chapter_1\map_chapter_1\スライド14.png`

## Changes

- Added `CreateRuinsF2LowerHouseSeparationDetails` and wired it after the existing left-settlement readability helper.
- Added narrow path gaps, porch edges, side cracks, roof breaks, rubble pockets, and sparse brush between the F2 lower-row houses so they read as separate ruin houses instead of one connected block.
- Adjusted the cycle05 ruins capture anchor and camera offset so the lower-row houses are visible enough for review.

## Review

- `cycle-worker` implemented the single-file F2 separation helper and avoided generated scene/docs/screenshot edits.
- Parent reviewed the regenerated current/past screenshots against reference slide 14.
- Subagent review reported no blocking issues; remaining nonblocking note is that the left pair can still visually cluster slightly because the roofs remain close.

## Validation

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateChapter1AllMapsBatch` passed.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch` passed.
- Review gallery audit passed for `docs/review/2026-05-25T21-01`.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch` passed.
- Built player smoke log opened without fatal startup errors.

## Review Artifacts

- `docs/review/2026-05-25T21-01/11_f1_f6_current.png`
- `docs/review/2026-05-25T21-01/12_f1_f6_past.png`
- `docs/review/2026-05-25T21-01/reference_slide14.png`
