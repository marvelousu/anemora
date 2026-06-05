# 2026-06-05 HD-2D P3 R-5 Table Grid Sampler

Branch: `wip/snapshot-repair-proof-20260603`

## Summary

R-5 was accepted on the correct P3 continuous branch. The wrong-line `work/chapter1-continuation-map-vs-20260524` Fix1/Fix2 source diffs were not used as implementation input; the fix was re-derived from current P3 source and current runtime captures.

## Change

- Replaced the visible `lamp` checker material texture with a non-grid `GlowNoise` sampler and reduced the color contrast.
- Replaced `sign_paint` hard `Planks` content with a non-grid `Paper` sampler.
- Removed hard furniture board seams and `%41/%37` grid lines from `SampleFurnitureWoodPlatePixel`.
- Added a stale generated-texture guard so repeat textures are recreated if a local run leaves mipmaps behind.

## Validation

- `ValidateHouseSliceBatch`: pass in `Logs/hd2d_p3_r5_table_grid_validate_pass10_20260605.log`.
- `BuildAndValidateBatch`: pass in `Logs/hd2d_p3_r5_table_grid_build_validate_20260605.log`.
- Latest exe: `C:\Users\maro6\Documents\Unity\Anemora-p3-recovery\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
  - Timestamp: `2026-06-05 08:56:57`
- Runtime perf: `Logs/hd2d_p3_r5_table_grid_perf_window_20260605.log`
  - `ANEMORA_HOUSE_SLICE_PERF: area=CentralPlaza seconds=20.033 frames=646 avgMs=31.01 minMs=16.65 maxMs=466.67 avgFps=32.2 activeRenderers=1959 visibleRenderers=727`
- Runtime smoke: `Logs/hd2d_p3_r5_table_grid_runtime_smoke_20260605.log`
  - `ANEMORA_HOUSE_SLICE_SMOKE_PASS`

## Captures

- All-map capture: `Logs/hd2d_p3_r5_table_grid_allmaps_capture_20260605.log`
  - 13 PNGs at `docs/devlog/screenshots/chapter1_all_maps_cycle05`, all `1280x720`.
- Library close-up capture: `Logs/hd2d_p3_r5_table_grid_library_close_capture_pass10_20260605.log`
  - 3 PNGs at `docs/devlog/screenshots/fast_vs_hd2d_library_reading_tables_20260520`.
- Review bundle: `docs/review/2026-06-05T09-04_hd2d_p3_r5_table_grid_sampler`
  - 13 all-map PNGs, 3 library close-up PNGs, 6 logs, and `REPORT.md`.

Manual review confirmed the previous large yellow checker blocks are gone, the current side table reads as a continuous dark surface with fine texture, and past library tables retain paper/book readability.

## Propagation

- R2 upload: `Logs/hd2d_p3_r5_table_grid_r2_upload_20260605.log`
  - `uploaded 23 files for chapter1-continuation-map-vs-20260524/2026-06-05T09-04_hd2d_p3_r5_table_grid_sampler`
- Pages deploy hook: `Logs/hd2d_p3_r5_table_grid_pages_deploy_20260605.log`
  - `HTTP_STATUS=200`
  - Deploy id: `2bafce94-3f03-4a37-ae1d-b1fbd62d8bf9`

## Next

Continue with R-2/R-3/R-6 recheck on the same P3 branch before moving to high-risk SetActive and depth-priming work.
