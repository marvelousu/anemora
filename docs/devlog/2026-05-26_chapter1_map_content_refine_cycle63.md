# Chapter 1 map content refine cycle63

## Scope
- Continued map-content iteration on `work/chapter1-continuation-map-vs-20260524`.
- Targeted the ruins F1-F6 map after comparing `スライド7.PNG` / `スライド14.png` with the generated all-map capture.
- Kept the published VS baseline and ignored `work/chapter1-continuation-20260520`.

## Changes
- Lowered the bridge/gorge visual read so the river/valley feels like a low embankment rather than a deep canyon.
- Narrowed the right-side F5-F6 road/plaza read while preserving the clear route lane.
- Expanded the lower-right rough land / low-plant zone so it reads as a map region, not a small patch.
- Split the lower-left F2 house row into more distinct close-set house ruins with roof seams, porch pads, and side stubs.
- Reinforced the F5 area as two house shells above the road rather than a third collapsed structure in the road.

## Review
- Ran an initial focused ruins visual review before implementation.
- Used `cycle-worker` for the scoped helper implementation in `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`.
- Ran a post-cycle visual review subagent; it flagged the lower-right rough/plant zone as under-scaled, then the zone was expanded before final capture.

## Verification
- `ValidateChapter1AllMapsBatch`: passed (`Logs/chapter1_cycle63c_validate.log`).
- `CaptureChapter1AllMapsCycle05ScreenshotsBatch`: passed (`Logs/chapter1_cycle63c_capture.log`).
- Review images: `docs/review/2026-05-26T09-08/`.
