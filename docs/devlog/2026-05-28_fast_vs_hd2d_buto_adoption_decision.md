# Phase B-beta Buto Adoption Decision

## Current Decision State

- Buto import status in this workspace: not detected.
- Renderer feature candidate: not detected.
- Volume override/type candidate: not detected.
- Runtime fallback: B-alpha ScreenSpaceLensFlare and Directional Sun lens flare remain active.

## Evidence

- Cycle 176 validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseBBetaButoAdoptionBatch`
- Cycle 176 capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseBBetaButoAdoptionCycle176ScreenshotsBatch`
- Diagnostic report: `docs/devlog/screenshots/fast_vs_hd2d_phase_b_beta_buto_adoption_cycle176_parent_review_20260528_01/parent_review_phase_b_beta_buto_adoption_diagnostics.md`
- Public review set: `docs/review/2026-05-28T18-07/`

## Notes for Tom

- 変更を適用しました: Buto が import 済みかを検出する batch 診断を追加し、未 import の場合は B-alpha fallback を維持する状態にしました。
- 参考画像とのギャップ: Buto 本体が workspace に無いため、B-β の god rays は比較できていません。現状は B-alpha fallback の継続です。
- Tom 判定をお願いします。
- Build exe path: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- 起動時は `Builds/FastVS_HouseSlice/` フォルダごと起動。
