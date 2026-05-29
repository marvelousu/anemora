# HD-2D Phase B-alpha Scene Lens Flare Review

Curated public review set for cycle 175.

## Images

- `01_house_interior_sun_cycle_morning.png`
- `02_house_exterior_sun_cycle_morning.png`
- `03_central_plaza_sun_cycle_noon.png`
- `04_library_sun_cycle_evening.png`
- `05_timewindow_aperture.png`

## Diagnostics

- `phase_b_alpha_scene_lens_flare_diagnostics.md`

## Notes for Tom

- 変更を適用しました: Directional Sun に `LensFlareComponentSRP` を追加し、`Assets/Art/LensFlare/LensFlareData_Sun.asset` と `ScreenSpaceLensFlare` Volume override を接続しました。
- 参考画像とのギャップ: URP 17 のローカル package cache に `VolumetricFog` override が露出していないため、B-alpha の god rays はまだ弱く、太陽フレアと screen-space flare の下地確認が主対象です。
- Tom 判定をお願いします。
- Build exe path: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- 起動時は `Builds/FastVS_HouseSlice/` フォルダごと起動。
