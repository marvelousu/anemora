# 2026-05-20 Fast VS HD2D Lighting Balance Cycle

## Purpose

Reduce hard black shadow crush in the HD-2D house slice while keeping the scene readable and still dimensional.

## Lighting Changes

- `Directional Light` stays the only directional light in the scene.
- `Light.type`: `Directional`
- `Light.shadows`: `Soft`
- `Light.shadowStrength`: `0.52`
- `Light.intensity`: `1.10`
- `RenderSettings.ambientMode`: `Flat`
- `RenderSettings.ambientLight`: `0.29, 0.30, 0.31`

## Validation And Capture

Run:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle37_worker_validate_20260520.log'
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dThirtySeventhCycleScreenshotsBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle37_worker_capture_20260520.log'
```

Result:

- `ValidateHouseSliceBatch` passed.
- `CaptureHd2dThirtySeventhCycleScreenshotsBatch` passed.
- Parent `ValidateHouseSliceBatch` passed.
- Parent `CaptureHd2dThirtySeventhCycleScreenshotsBatch` passed.
- Parent `BuildAndValidateBatch` passed.
- Parent EXE smoke test ran for 20 seconds with `match_count=0`.

## Screenshot Output

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_lighting_balance_20260520\01_current_house_exterior_lighting_balance.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_lighting_balance_20260520\02_current_central_plaza_lighting_balance.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_lighting_balance_20260520\03_current_library_lighting_balance.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_lighting_balance_20260520\04_past_central_plaza_lighting_balance.png`

## Assets

- No external assets used.
- No paid assets used.
