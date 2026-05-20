# 2026-05-20 Fast VS HD2D House Interior Room Depth Cycle

Purpose: reduce the house interior black void and stage-box feel in normal and close review screenshots by adding subtle upper-wall depth framing only. The room still remains open-front, and no ceiling slab or backdrop was added.

Files changed:
- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
- `docs/devlog/INDEX.md`
- `docs/devlog/2026-05-20_fast_vs_hd2d_house_interior_room_depth_cycle.md`

Implementation summary:
- Added `CreateHouseInteriorRoomDepthPolish(Transform root, string prefix, bool past, Materials materials)` and called it from `CreateInterior(...)` after the wall and wainscot pieces but before the bed block.
- Created 12 new non-colliding visual cubes for current and past interiors: upper back-wall shadow, back-wall top cap, left/right wall top caps, and back-left/back-right corner posts.
- Used existing materials only: the HD2D depth shadow material plus existing trim and furniture materials.
- Added `ValidateFastVsHd2dFortyFirstCycleHouseInteriorRoomDepth()` and wired it into `ValidateHouseSliceBatch()`.
- Added `CaptureHd2dFortyFirstCycleScreenshotsBatch()` and `CaptureHd2dFortyFirstCycleScreenshotsToDirectory(...)` with a dedicated screenshot directory.
- Kept the existing wall colliders, bed logic, dialogue/story logic, font logic, and map-transition logic unchanged.
- Used deterministic geometry/material polish only; no external, Meshy, or paid assets were used.

Validation commands:
- Pass: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle41_worker_validate_20260520.log'`
- Pass: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dFortyFirstCycleScreenshotsBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle41_worker_capture_20260520.log'`

Screenshot outputs:
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_interior_room_depth_20260520\01_current_house_room_depth_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_interior_room_depth_20260520\02_past_house_room_depth_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_interior_room_depth_20260520\03_current_house_room_depth_corner_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_interior_room_depth_20260520\04_past_house_room_depth_corner_close.png`

Caveats:
- The first validation pass exposed a material-token mismatch in the new validator; I corrected the checks to match the repo's existing short material names and reran validation successfully.
- The screenshot batch had to wait for Unity to finish opening and refreshing the project before the capture run could complete.
- Parent review adjusted the close-review camera to look from inside the room toward the back-left corner instead of skimming along the wall top cap, then reran validation and screenshot capture.
