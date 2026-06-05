# 2026-06-05 HD-2D P3 Fix 5 SetActive Lightweighting

Branch: `wip/snapshot-repair-proof-20260603`

## Summary

Fix5 was accepted on the correct P3 continuous branch. The wrong-line `work/chapter1-continuation-map-vs-20260524` Fix1/Fix2 source diffs were not used as implementation input.

The milestone turns active-area visibility from "both time roots active for the selected area" into runtime player-time isolation: the selected area keeps only the current root active while Niro is in current time, only the past root active while Niro is in other time, and both roots active while a Time Window portal preview/pair exists.

## Change

- `FastVsHouseAreaVisibility` now applies active area root visibility through current/past keep flags.
- Runtime-only SetActive isolation is exposed for review/probe coverage and ignored outside play mode.
- Portal preview/pair state keeps both time roots active to preserve live aperture rendering.
- Unchanged runtime isolation state returns early, avoiding per-frame reapplication of all 32 area roots.
- `FastVsVisualDirectionGuide` applies the isolation state before camera/culling updates.
- `FastVsHouseRuntimeSmokeProbe` now treats Karla/Aria correctly as past-only AriaInterior NPCs: current-time AriaInterior travel requires the past root to be inactive, then review keep-both verifies those NPC renderers still work.
- R-2/R-3/R-6 recheck coverage was extended to verify inactive-time roots and keep-both portal behavior.

## Validation

- `BuildAndValidateBatch`: pass in `Logs/hd2d_p3_fix5_setactive_build_validate_pass4_20260605.log`.
  - `Fast VS house slice validation passed.`
  - `Build Finished, Result: Success.`
  - `Fast VS house slice player built`
- Latest exe: `C:\Users\maro6\Documents\Unity\Anemora-p3-recovery\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
  - Timestamp: `2026-06-05 10:59:11`
- Runtime recheck: `Logs/hd2d_p3_fix5_setactive_runtime_r236_pass4_20260605.log`
  - `ANEMORA_HOUSE_SLICE_R236_RECHECK_PASS`
  - R-2: CentralPlaza and AriaInterior current roots stayed visible; inactive past roots reported `activeSelf=False activeInHierarchy=False`; keep-both path still made past roots visible.
  - R-3: library facade materials stayed opaque queue `2000`, alpha `1.000`.
  - R-6: `stencilPasses=2`, `wallCollidersOtherTime=5`, `wallCollidersCurrent=0` after return.
- Runtime smoke: `Logs/hd2d_p3_fix5_setactive_runtime_smoke_pass4_20260605.log`
  - `ANEMORA_HOUSE_SLICE_SMOKE_PASS`
  - `[SMOKE] AriaInterior past root inactive during current-time travel: activeSelf=False activeInHierarchy=False`

## Performance

- Immediate pre-Fix5 baseline: `Logs/hd2d_p3_r236_recheck_perf_window_20260605.log`
  - `avgFps=31.4 activeRenderers=1959 visibleRenderers=727`
- Accepted Fix5 run: `Logs/hd2d_p3_fix5_setactive_perf_window_pass5_20260605.log`
  - `ANEMORA_HOUSE_SLICE_PERF: area=CentralPlaza seconds=20.016 frames=650 avgMs=30.79 minMs=16.65 maxMs=416.65 avgFps=32.5 activeRenderers=1036 visibleRenderers=577`
- A/B result: `+1.1fps`, `-923 activeRenderers`, `-150 visibleRenderers` versus the immediate pre-Fix5 R236 baseline.

The first Fix5 perf samples before the no-op guard were around `30.4-30.5fps` despite renderer reduction, so they were not accepted. The accepted run is after guarding unchanged frame state.

## Captures

- All-map capture: `Logs/hd2d_p3_fix5_setactive_allmaps_capture_20260605.log`
  - 13 PNGs at `docs/devlog/screenshots/chapter1_all_maps_cycle05`, all `1280x720`.
- Review bundle: `docs/review/2026-06-05T11-06_hd2d_p3_fix5_setactive_lightweighting`
  - 13 all-map PNGs, 6 logs, and `REPORT.md`.

The capture log completed with `Fast VS chapter 1 all maps screenshots captured` and retained the known URP/Lit shader compiler OOM noise pattern. Spot review checked current plaza, past Aria street, and scene6 sideview as nonblank; existing magenta debug/error materials remain unchanged.

## Propagation

- R2 upload: `Logs/hd2d_p3_fix5_setactive_r2_upload_retry_20260605.log`
  - `uploaded 20 files for chapter1-continuation-map-vs-20260524/2026-06-05T11-06_hd2d_p3_fix5_setactive_lightweighting`
  - The first upload attempt failed one PNG due connectivity; retry succeeded with all files.
- Pages deploy hook: `Logs/hd2d_p3_fix5_setactive_pages_deploy_20260605.log`
  - `HTTP_STATUS=200`
  - Deploy id: `6f11bb57-2b9b-4bfc-ad43-f95d946b0dea`

## Next

Continue to Fix 6 only after treating it as the highest-risk depth-priming/Portal DepthOnly milestone and rerunning the real built-player R-2/R-3/R-6, smoke, perf, and all-map capture/upload gates.
