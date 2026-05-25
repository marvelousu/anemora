# Chapter 1 Map Content Refine Cycle 35

## Scope

- Branch: `work/chapter1-continuation-map-vs-20260524`
- Target: F1-F6 ruins map against reference slide 14.
- Worker: cycle-worker Hilbert edited only `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`; parent adjusted a duplicate F5 door cue before validation.

## Changes

- Added bridge low-cliff cues: mouth lips, abutment shadows, under-bridge shadow, and current-only broken channel pads.
- Added F4 edge remnants to extend the upper-left settlement toward the gorge without treating tree/grass rectangles as borders.
- Added F5 house-pair readability cues: porch/lintel cues, divider, current collapsed roof chunks, and road-edge breaks.
- Added current/past contrast cues for the F4/F5 cluster and low-brush/road break details on the right settlement side.
- After review, widened bridge mouth/shadow cues, strengthened the F4 east edge, moved the F5 house divider to the actual house boundary, renamed the F4 porch cue, and added more scattered F5/F6 field props.

## Verification

- `ValidateChapter1AllMapsBatch`: passed.
- `CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed.
- `BuildAndValidateBatch`: passed.
- 18-second built-player smoke: passed with no error / exception / failed / crash / NullReference matches.
- Review set: `docs/review/2026-05-26T01-34/`

## Notes

- Route trigger centers and transition targets were not moved.
- F5 still needs visual review because the right settlement remains busy and can read as mixed ruin clutter if overworked.
