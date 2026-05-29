# 2026-05-20 Fast VS HD2D House Bed Soft Textile Cycle

Purpose: soften the Niro house interior bed textile at close and gameplay distances while keeping the existing bed contract, collision state, and route/story logic stable.

Files changed:
- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
- `docs/devlog/INDEX.md`
- `docs/devlog/2026-05-20_fast_vs_hd2d_house_bed_soft_textile_cycle.md`

Implementation summary:
- Changed the bed top-fold piece to use `bedMaterial` and reduced its slab feel with a thinner Y scale and slightly smaller Z scale.
- Added `CreateHouseBedSoftTextilePolish(...)` and created eight new non-colliding textile detail objects for current and past bed variants.
- Added `ValidateFastVsHd2dFortiethCycleHouseBedSoftTextilePolish()` and wired it into the house slice batch validation path.
- Added `CaptureHd2dFortiethCycleScreenshotsBatch()` and a dedicated capture directory for the new bed textile screenshots.
- Kept all existing bed, pillow, table, book, glow pad, collider, and route objects in place.
- Used deterministic geometry and existing materials only; no external, Meshy, or paid assets were used.

Validation commands:
- Pass: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle40_worker_validate_20260520.log'`
- Pass: `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dFortiethCycleScreenshotsBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle40_worker_capture_20260520.log'`

Screenshot outputs:
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_bed_soft_textile_20260520\01_current_house_bed_soft_textile_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_bed_soft_textile_20260520\02_past_house_bed_soft_textile_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_bed_soft_textile_20260520\03_current_house_bed_soft_textile_gameplay.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_bed_soft_textile_20260520\04_past_house_bed_soft_textile_gameplay.png`

Caveats:
- Unity batch capture initially hit a project-lock conflict because another Unity instance had the project open; the capture succeeded after that instance cleared.
