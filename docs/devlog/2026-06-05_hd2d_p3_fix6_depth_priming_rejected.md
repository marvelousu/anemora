# 2026-06-05 HD-2D P3 Fix 6 Depth Priming Rejected

Branch: `wip/snapshot-repair-proof-20260603`

## Summary

Fix6 is rejected on the correct P3 continuous branch. No wrong-line `work/chapter1-continuation-map-vs-20260524` Fix1/Fix2 source diffs were used.

The attempted approach added `DepthOnly` passes to the Time Window portal aperture/inside shaders and re-enabled URP `DepthPrimingMode.Auto`. It passed structural validation, build, R236, smoke, and perf probes, but failed visual acceptance in the real built player: the same blue-gray/fogged geometry loss seen in editor all-map capture reproduced in runtime all-map PNGs.

## Failed Attempt

- `BuildAndValidateBatch`: pass in `Logs/hd2d_p3_fix6_depth_priming_build_validate_pass1_20260605.log`.
  - `Fast VS house slice validation passed.`
  - `Build Finished, Result: Success.`
  - `Fast VS house slice player built`
- Runtime R236: pass in `Logs/hd2d_p3_fix6_depth_priming_runtime_r236_pass1_20260605.log`.
  - `ANEMORA_HOUSE_SLICE_R236_RECHECK_PASS`
- Runtime smoke: pass in `Logs/hd2d_p3_fix6_depth_priming_runtime_smoke_pass1_20260605.log`.
  - `ANEMORA_HOUSE_SLICE_SMOKE_PASS`
- Runtime perf:
  - `Logs/hd2d_p3_fix6_depth_priming_perf_window_pass1_20260605.log`: `avgFps=30.9 activeRenderers=1036 visibleRenderers=578`
  - `Logs/hd2d_p3_fix6_depth_priming_perf_window_pass2_20260605.log`: `avgFps=31.3 activeRenderers=1036 visibleRenderers=577`
  - Both were below the accepted Fix5 sample (`32.5fps`).
- Editor all-map capture: `Logs/hd2d_p3_fix6_depth_priming_allmaps_capture_pass1_20260605.log`
  - Completed, but representative frames showed blue-gray/fogged geometry loss.

To remove ambiguity between editor capture and real runtime, a temporary built-player all-map capture probe was added, built, and run. It produced 13 runtime PNGs in `docs/review/2026-06-05T12-10_hd2d_p3_fix6_runtime_allmaps_probe` and reproduced the failure. Representative failed runtime frames:

- `failed_03_b1_b3_current.png`
- `failed_08_d1_d3_past.png`

## Revert

The unaccepted Fix6 source/shader/runtime-probe changes were removed. The renderer contract is back to:

- `DepthPrimingMode.Disabled`
- `CopyDepthMode.AfterOpaques`

No accepted code change was committed for Fix6.

## Revert Validation

- Rebuilt exe: `C:\Users\maro6\Documents\Unity\Anemora-p3-recovery\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
  - Timestamp: `2026-06-05 12:25:27`
- `BuildAndValidateBatch`: pass in `Logs/hd2d_p3_fix6_rejected_revert_build_validate_pass1_20260605.log`.
  - `Fast VS house slice validation passed.`
  - `Build Finished, Result: Success.`
  - `Fast VS house slice player built`
- Runtime R236: `Logs/hd2d_p3_fix6_rejected_revert_runtime_r236_pass1_20260605.log`
  - `ANEMORA_HOUSE_SLICE_R236_RECHECK_PASS`
  - `activeRenderers=1050 visibleRenderers=578`
- Runtime smoke: `Logs/hd2d_p3_fix6_rejected_revert_runtime_smoke_pass1_20260605.log`
  - `ANEMORA_HOUSE_SLICE_SMOKE_PASS`
- Runtime perf: `Logs/hd2d_p3_fix6_rejected_revert_perf_window_pass1_20260605.log`
  - `ANEMORA_HOUSE_SLICE_PERF: area=CentralPlaza seconds=20.033 frames=623 avgMs=32.16 minMs=16.65 maxMs=449.99 avgFps=31.1 activeRenderers=1036 visibleRenderers=577`
- All-map recapture: `Logs/hd2d_p3_fix6_rejected_revert_allmaps_capture_pass1_20260605.log`
  - 13 PNGs regenerated at `docs/devlog/screenshots/chapter1_all_maps_cycle05`.
  - All 13 PNGs are `1280x720`.
  - Spot review confirmed `03_b1_b3_current.png` and `08_d1_d3_past.png` returned to normal geometry/color instead of the blue-gray Auto failure.

## Propagation

- Review bundle: `docs/review/2026-06-05T12-34_hd2d_p3_fix6_depth_priming_rejected`
  - 13 failed runtime PNGs, 13 reverted/disabled all-map PNGs, logs, and `REPORT.md`.
- R2 upload: `Logs/hd2d_p3_fix6_rejected_r2_upload_20260605.log`
  - `uploaded 41 files for chapter1-continuation-map-vs-20260524/2026-06-05T12-34_hd2d_p3_fix6_depth_priming_rejected`
- Pages deploy hook: `Logs/hd2d_p3_fix6_rejected_pages_deploy_20260605.log`
  - `HTTP_STATUS=200`
  - Deploy id: `bc39f61e-5e1b-4c6a-8643-4183b3c24239`

## Decision

Do not ship the current Fix6 approach. Portal-only `DepthOnly` additions do not make `DepthPrimingMode.Auto` safe for this P3 runtime. The continuous branch remains at accepted Fix5 behavior.
