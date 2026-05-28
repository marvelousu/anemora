# feat(hd2d): add phase c alpha emissive vfx fallback

Cycle 177 implements the Phase C-alpha purchase-free slice with stronger HDR-style emission values, two focused point lights, and low-count ParticleSystem fallback emitters because VFX Graph is not present in the local package set.

## Scope

- Increased window and library window emission to Phase C-alpha warm ranges.
- Added four C-alpha fallback emitters: fire spark, firefly, smoke, and water splash.
- Added C-alpha Library warm emissive point light and Plaza water sparkle point light.
- Kept existing director `warmFill` / `coolRim` values unchanged to preserve older validation contracts.
- Recorded VFX Graph package absence in diagnostics instead of failing the Phase.

## Validation Plan

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseCAlphaEmissiveVfxBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseCAlphaEmissiveVfxCycle177ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player with `-batchmode -nographics`, pattern count must be 0.

## Review Notes

- 変更を適用しました: emission 値、C-alpha point lights、ParticleSystem fallback VFX 4 種を追加しました。
- 参考画像とのギャップ: VFX Graph package が無いため、本来指定の `.vfx` asset ではなく既存 ParticleSystem 方式です。Bloom/暖寒対比は追加しましたが、reference_02 の密度にはまだ届いていません。
- Tom 判定をお願いします。
- Build exe path for Tom: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- 起動時は `Builds/FastVS_HouseSlice/` フォルダごと起動。
