# feat(hd2d): add phase a sun cycle runtime api

Date: 2026-05-28 JST

## Scope

- Phase A Step 1 from `docs/HD2D_SUN_CYCLE_SPEC.md`.
- Added the SunCycle runtime API:
  - `SunPreset`
  - `SunPresetData`
  - `AnemoraSunCycleDriver`
  - `MapSunAnchor`
- Added editor-only validate/capture entry points for preset asset generation and API diagnostics.
- Scene wiring, existing lighting handoff, Painted Overlay removal, and shader changes remain in later Phase A cycles.

## Area Decision Dependency

- Area management decision was recorded in `docs/devlog/2026-05-28_fast_vs_hd2d_sun_cycle_area_decision.md`.
- FastVS HouseSlice uses same-scene area switching via `FastVsHouseAreaVisibility`, not separate scene files for Interior / Exterior / CentralPlaza / Library.

## Cycle Worker

SCOPED_PROMPT_ISSUED cycle=165 authored_file=Assets/Scripts/FastVS/SunCycle/AnemoraSunCycleDriver.cs validate=Anemora.FastVS.SunCycle.AnemoraSunCycleDriver.ValidateHd2dPhaseASunCycleRuntimeApiBatch capture=Anemora.FastVS.SunCycle.AnemoraSunCycleDriver.CaptureHd2dPhaseASunCycleRuntimeApiCycle165ScreenshotsBatch

## Validation

Final parent runner execution:

- Runner log: `tools/logs/cycle-165-20260528-084534.log`
- Validate log: `Logs/cycle-165-20260528-084534-validate.log`
- Capture log: `Logs/cycle-165-20260528-084534-capture.log`
- Build log: `Logs/cycle-165-20260528-084534-build.log`
- Smoke log: `Logs/cycle-165-20260528-084534-smoke.log`
- Validate result: exit 0.
- Capture result: exit 0.
- Build result: exit 0.
- Smoke result: built exe launched for 24 seconds with `-batchmode -nographics`; scanned `Exception|Shader error|error CS|NullReference|MissingReference|Assertion|Failed`; pattern count 0.

Earlier attempts:

- `cycle-165-20260528-082928`: validate failed on `CreateInstance` missing qualification. The current authored file uses `ScriptableObject.CreateInstance<SunPresetData>()`.
- `cycle-165-20260528-083423`: interrupted run; short Unity log only.
- `cycle-165-20260528-083647`: build failed with Windows paging file exhaustion (`GetLastError: 1455`) during Bee/ApiUpdater. `dotnet build-server shutdown` was run, then the final runner execution above completed.

## Review Artifacts

Local diagnostic output:

- `docs/devlog/screenshots/fast_vs_hd2d_phase_a_sun_cycle_runtime_api_cycle165/parent_review_sun_cycle_preset_strip.png`
- `docs/devlog/screenshots/fast_vs_hd2d_phase_a_sun_cycle_runtime_api_cycle165/sun_cycle_runtime_api_diagnostics.md`

This diagnostic capture is not a Tom visual sign-off gate for Phase A. The Phase A gate remains the later 5-area build screenshots plus TimeWindow aperture check after dynamic sun wiring and overlay removal.

## Build Artifact For Tom

Build exe path for review:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

起動時は `Builds\FastVS_HouseSlice\` をフォルダごと起動対象として扱う。

## Gate Note

変更を適用しました。参考画像とのギャップは、まだ runtime API と preset asset の土台段階のため、参考画像にある god rays / 太陽盤 / 暖寒対比 / エミッシブ表現は未実装です。Tom 判定は Phase A の 5 エリア capture 完了時にお願いします。
