# Chapter 1 Map Content Refine Cycle 21

Branch: `work/chapter1-continuation-map-vs-20260524`

Scope:
- Refined the left half of the F1-F6 ruins map after Cycle20.
- Focused on F2 lower road and lower house row readability, F1 entry framing, and reducing overly regular road/house edges around F2-F4.

Changes:
- Added road-edge grass breaks and rubble lines around the upper/lower left roads to reduce the straight tiled-band look.
- Added side stubs, broken sills, and roof/trim cues on the left house rows to make the F2/F3/F4 settlement less uniform.
- Added a F2 lower-road block cue plus three lower-house back-wall/roof-lip cues to support the reference layout's lower row.
- Added F1 entry shoulders, threshold, and broken posts to make the toE3 entry read as a deliberate node rather than only an uninterrupted road.

Review:
- Subagent Heisenberg reviewed the left half before changes and flagged F2 lower-road/house-row mismatch, weak F1 entry, and overly regular F3/F4 upper-road edges.
- Subagent Boole reviewed the updated screenshots and reported no blocking issue. Remaining polish: F2 bottom houses still read somewhat connected because their roofs/footprints remain close and partly near the camera bottom edge.

Validation:
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateChapter1AllMapsBatch`
  - First run failed due to missing local `path` material in `CreateRuinsLeftSettlementReadabilityDetails`; fixed and re-ran.
  - Passing log: `Logs/chapter1_cycle21_validate_r2.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`
  - Log: `Logs/chapter1_cycle21_capture_r1.log`
  - Outputs: `docs/devlog/screenshots/chapter1_all_maps_cycle05/11_f1_f6_current.png`, `docs/devlog/screenshots/chapter1_all_maps_cycle05/12_f1_f6_past.png`

Review bundle:
- `docs/review/2026-05-25T20-43/`
