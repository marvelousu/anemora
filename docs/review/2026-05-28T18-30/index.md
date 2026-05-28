# HD-2D Phase C-alpha Emissive VFX Review

Curated public review set for cycle 177.

## Images

- `01_house_interior_sun_cycle_morning.png`
- `02_house_exterior_sun_cycle_morning.png`
- `03_central_plaza_sun_cycle_noon.png`
- `04_library_sun_cycle_evening.png`
- `05_timewindow_aperture.png`

## Diagnostics

- `phase_c_alpha_emissive_vfx_diagnostics.md`

## Notes for Tom

- 変更を適用しました: window/library emission を強化し、C-alpha point lights と ParticleSystem fallback VFX 4 種を追加しました。
- 参考画像とのギャップ: VFX Graph package が無いため `.vfx` asset ではなく fallback です。TimeWindow の発光は強く、reference_02 の自然な暖寒対比と粒子密度にはまだ届いていません。
- Tom 判定をお願いします。
- Build exe path: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- 起動時は `Builds/FastVS_HouseSlice/` フォルダごと起動。
