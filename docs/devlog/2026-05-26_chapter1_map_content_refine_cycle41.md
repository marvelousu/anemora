# Chapter 1 map content refine cycle 41

Branch: `work/chapter1-continuation-map-vs-20260524`

Scope:
- Refined C1-C3 Mia house exterior map content.
- Added a small in-file helper to clarify the front yard, C1 diagonal road join, C3 endpoint, and tree/plant zones without moving route triggers.
- Widened the visual front-yard pad around C2 and added cleaner gate/walk/border cues.
- Added lower tree/plant masses so the bottom reference zone reads as vegetation rather than mostly scattered debris.
- Added right-side tree/underbrush density and C3 exit cap/stone lip while keeping reference plant boxes as approximate zones.

Review:
- `docs/review/2026-05-26T03-29/01_c1_c3_current.png`
- `docs/review/2026-05-26T03-29/02_c1_c3_past.png`
- Initial subagent `019e605f-571e-72a0-9c32-67a0e81122ee` flagged front-yard clutter, C1 road kink, and lower plant-strip weakness.
- Follow-up subagent `019e6064-6e7a-76e1-a92b-759dd58e20a7` accepted the updated C1-C3 screenshots for this cycle, with only non-blocking C2/front-yard clutter note.

Validation:
- `git diff --check -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
- Unity `ValidateChapter1AllMapsBatch` passed.
- Unity `CaptureChapter1AllMapsCycle05ScreenshotsBatch` passed.
- Unity `BuildAndValidateBatch` passed.
- Player smoke: fatal match count 0.
