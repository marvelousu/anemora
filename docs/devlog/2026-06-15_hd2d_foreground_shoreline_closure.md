# HD2D foreground shoreline closure

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-15

## Investigation

- The mid-distance landform closure removed much of the empty panorama moat, but the all-map Wide review still exposed hard near-board edges where the camera sees the playable map from above.
- The issue was structural rather than color polish: the maps needed authored foreground shoreline and edge-break geometry in front of the close play space, while preserving the repaired route and bridge traversal.
- A temporary central cap experiment did not improve the remaining bright legacy surface reads in House Exterior and Central Plaza, so it was discarded. The remaining white legacy overlay/flat-surface reads are now tracked as a separate follow-up instead of hiding them with larger caps.

## Implementation

- Added `ForegroundShorelineClosure` roots to every Chapter 1 outdoor map in both current and past:
  - House Exterior,
  - Central Plaza,
  - Mia House,
  - Aria Street,
  - Kaia Farm,
  - Ruins.
- Each root builds deterministic, collision-free near-edge geometry:
  - `FrontSkirt` low terrain shelves to hide the camera-facing board edge,
  - `SideReturn` shelves to wrap the left/right map edges,
  - `PathTongue` continuations that visually carry roads toward the shoreline,
  - `StoneBreak` low-bank accents to break straight silhouettes,
  - `ReedFold` foreground vegetation folds to soften the transition.
- Added `Ch1Distant_*ForegroundShoreline*` generated materials for terrain, path, stone, and reed folds. These stay separate from renderer features and do not add traversal collision.
- Added all-map validation for root parenting, render layer, mesh density, expected foreground/side/path/stone/reed counts, no colliders, near-edge placement band, material naming, non-shadow renderer policy, landmark markers, and Wide-camera visibility.

## Verification

- Validate: `Logs/foreground_shoreline_closure_validate_r5.log` passed with `Fast VS house slice validation passed.`
- Renderer freeze: `Logs/foreground_shoreline_closure_editmode_r5.xml` passed `36/36`; `RendererFeatureSet_MatchesFrozenBaseline` result is `Passed`.
- Asset validation: `Logs/foreground_shoreline_closure_asset_validation_r5.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/foreground_shoreline_closure_capture_r3.log` produced the accepted 13 all-map Wide review PNGs.
- Review packet: `docs/review/2026-06-15T11-47_foreground_shoreline_closure/` contains the 13 all-map frames, `00_contact_sheet.png`, and `devlog.txt`.
- Shotdiff: `Logs/shotdiff/foreground_shoreline_closure_vs_mid_distance_r3/summary.txt` compared against `docs/review/2026-06-15T10-46_mid_distance_landform_closure`. The contact sheet changed by `6.0621%`. All 12 current/past Wide map frames changed over the 0.5% triage budget: `01_a1_a2_current.png` 3.9465%, `02_a1_a2_past.png` 1.6852%, `03_b1_b3_current.png` 5.5895%, `04_b1_b3_past.png` 1.9858%, `05_c1_c3_current.png` 1.7509%, `06_c1_c3_past.png` 1.7287%, `07_d1_d3_current.png` 2.8891%, `08_d1_d3_past.png` 1.5908%, `09_e1_e3_current.png` 1.5151%, `10_e1_e3_past.png` 1.3824%, `11_f1_f6_current.png` 3.3977%, and `12_f1_f6_past.png` 2.7308%. The side-view frame remained unchanged.

## Visual Review

- Accepted as a foreground shoreline/near-edge closure pass: the Wide review now has authored low terrain, reeds, stones, and path tongues at the camera-facing edges instead of only hard board edges and empty water/void transitions.
- Remaining issue: several current-frame views still show bright legacy white flat-surface or overlay reads near House Exterior and Central Plaza. Larger foreground caps were tested and rejected because they did not address the root cause. The next structural pass should identify and suppress or replace those legacy white surfaces directly, then recapture all Wide frames.

## Next

- Trace the remaining bright legacy foreground/overlay surfaces in House Exterior and Central Plaza and replace them with authored dark shoreline, stone, or terrain materials.
- Continue improving foreground detail density after the white-surface cleanup, especially route approach edges and shoreline seams.
- Keep foreground dressing collision-free unless it is explicitly part of traversal; bridge traversal validation remains the route contract.
