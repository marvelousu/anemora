# Chapter 1 Map Content Refine Cycle 20

Branch: `work/chapter1-continuation-map-vs-20260524`

Scope:
- Refined the right half of the F1-F6 ruins map after Cycle19.
- Focused on F5 two-house readability, F6 exit framing, lower-right low-brush massing, and reducing the impression that the right road is only a broad tiled plaza.

Changes:
- Added `CreateRuinsRightSettlementReadabilityDetails` and wired it after the right-side stall remnants.
- Strengthened F5 as a two-house cluster with readable doors, windows, thresholds, and roof/wall cues.
- Added small grass cuts and rubble details to break the broad tiled apron around F5.
- Added F6 road-end mouth, stone edges, and a small fence hint to frame the toLast exit.
- Added lower-right low-brush patches and clumps while keeping them decorative and non-blocking.

Review:
- Subagent Fermat reviewed the previous right half and flagged weak F5 two-house readability, under-defined F6 exit, a flat lower-right grass area, and old stall remnants competing with the houses.
- Subagent Kierkegaard reviewed the updated screenshots and reported no blocking issue; F5 readability, F6 exit framing, and lower-right brush were acceptable, with only the road/plaza width still slightly broad.

Validation:
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateChapter1AllMapsBatch`
  - Log: `Logs/chapter1_cycle20_validate_r1.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`
  - Log: `Logs/chapter1_cycle20_capture_r1.log`
  - Outputs: `docs/devlog/screenshots/chapter1_all_maps_cycle05/11_f1_f6_current.png`, `docs/devlog/screenshots/chapter1_all_maps_cycle05/12_f1_f6_past.png`

Review bundle:
- `docs/review/2026-05-25T20-31/`
