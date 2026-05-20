# 2026-05-20 Fast VS HD2D Outdoor Boundary Nature Cycle

## 目的

- 空や大きな背景板は再導入せず、家外・中央広場の地面端と境界周りの密度を上げる。
- 時の窓、ドア、マップ移動の光、移動導線には触れず、低い草・葉・石・影だけでHD-2Dらしい足元情報を増やす。

## 実装

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` に Cycle36 用の境界自然物生成を追加。
- ニロの家・外に `Current/Past_HouseExterior_BoundaryNature_*` を追加し、現在側は乾いた草・小石、過去側は花・葉の印象に分けた。
- 中央広場に `Current/Past_CentralPlaza_BoundaryNature_*` を追加し、左右の低壁沿いと奥側の樹影を低い装飾として足した。
- `ValidateFastVsHd2dThirtySixthCycleOutdoorBoundaryNatureDetails()` を追加し、必須オブジェクト、親マップ、素材、低い高さ、既存の移動光を検証するようにした。
- `CaptureHd2dThirtySixthCycleScreenshotsBatch()` を追加し、家外と中央広場の現在/過去を4枚保存するようにした。

## 検証

- `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle36_worker_validate_20260520.log'`
- `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dThirtySixthCycleScreenshotsBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle36_worker_capture_20260520.log'`
- worker 側の `ValidateHouseSliceBatch` は成功。
- 親セッション側の `ValidateHouseSliceBatch` は成功。
- 親セッション側の `CaptureHd2dThirtySixthCycleScreenshotsBatch` は成功。
- 親セッション側の `BuildAndValidateBatch` は成功。
- 親セッション側の EXE smoke test は20秒起動で `match_count=0`。

## スクリーンショット

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_boundary_nature_20260520\01_current_house_exterior_boundary_nature.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_boundary_nature_20260520\02_past_house_exterior_boundary_nature.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_boundary_nature_20260520\03_current_central_plaza_boundary_nature.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_outdoor_boundary_nature_20260520\04_past_central_plaza_boundary_nature.png`

## アセット

- 外部アセットは未使用。
- Meshy/API は未使用。
- 有償アセットは未使用。
