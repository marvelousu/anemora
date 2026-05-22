# Fast VS HD2D House Facade Closure / Shadow Cycle 50

Date: 2026-05-22

## Summary

The user pointed out two issues from the latest screenshot: the shadows still looked weak, and the house exterior door area had an unnatural side opening where the interior could be read from outside even though the door was closed. Cycle50 treats the door-side opening as a structural facade bug rather than a lighting problem. The shadow work in this cycle is therefore local and supportive; it is not the final HD-2D shading pass.

## Changed Files

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

## What Changed

- Added a dedicated cycle50 validation gate for the house facade closure and local shadow objects.
- Widened and repositioned the exterior closed-door panel to cover the previous side opening more robustly.
- Reworked the door-side closure pieces from a large dark seam into explicit left/right jamb fills, inner door seams, upper contact shadow, threshold contact shadow, and a right return lip.
- Added current and past right-return facade pieces for the cycle49 door-side closure so current/past map generation and validation stay paired.
- Kept all closure additions non-arrival and non-colliding so the map movement and Time Window current/past coordinate contracts are not affected.

## Validation

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_facade_closure_shadow_cycle50_validate_parent_20260522.log`
  - Result: passed
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_facade_closure_shadow_cycle50_close_review_parent_20260522.log`
  - Result: passed
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_facade_closure_shadow_cycle50_visual_snapshot_parent_20260522.log`
  - Result: passed

## Screenshot Evidence

- Close review source folder: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_close_review_20260520`
- Visual snapshot source folder: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`
- Accepted Cycle50 copy folder: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle50_house_facade_closure_shadow_parent_review_20260522_01`
- Door close-up evidence: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle50_house_facade_closure_shadow_parent_review_20260522_01\01_house_exterior_door_close_after_facade_closure.png`
- Exterior overview evidence: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle50_house_facade_closure_shadow_parent_review_20260522_01\02_current_house_exterior_visual_snapshot.png`

## Self Review

Conditionally OK.

The close-up no longer shows the house interior through the closed door side. The remaining issue is that one right-side vertical area can still read as a deep dark recess rather than a designed return face. The next cycle should avoid adding more black strips and instead make the facade side return read as actual wall/trim geometry with material response.

The shadow read remains a foundation state. Cycle49 and Cycle50 improved the light profile and local contact, but they are still not the product-quality HD-2D shading baseline the user is asking for. The next larger step should strengthen systematic material response and outdoor/background lighting composition, not only add darker overlays.
