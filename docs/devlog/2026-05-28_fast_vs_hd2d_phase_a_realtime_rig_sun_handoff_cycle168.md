# feat(hd2d): hand off realtime rig sun control

Date: 2026-05-28 JST

## Scope

- Phase A Step 2 from `docs/HD2D_SUN_CYCLE_SPEC.md`.
- `FastVsRealtimeLightShadowRig` no longer writes Main Directional intensity, shadowStrength, color, rotation, cookie, or cookieSize values.
- Removed the Rig-owned runtime CentralPlaza/Exterior Directional cookie generation helpers that became unused after the handoff.
- Removed Rig fog/ambient writes that competed with `AnemoraSunCycleDriver`; Rig still sets reflection intensity and runtime skybox assignment.
- Kept shadow bias/resolution setup, renderer shadow policy, Painted Overlay code, 0.35s policy refresh, and shader lightening for later Phase A cycles.

## Cycle Worker

SCOPED_PROMPT_ISSUED cycle=168 authored_file=Assets/Scripts/FastVS/FastVsRealtimeLightShadowRig.cs validate=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseARealtimeRigSunHandoffBatch capture=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseARealtimeRigSunHandoffCycle168ScreenshotsBatch

The cycle-worker removed only the main-sun appearance and ambient/fog writes from `ApplyLightAndSky`, leaving Painted Overlay/Cycle128/Cycle131 code and the 0.35s renderer policy loop for later Phase A cycles. The parent removed a duplicate provisional batch entry and kept the final `RealtimeRigSunHandoff` entry points.

## Validation

Final parent runner execution:

- Runner log: `tools/logs/cycle-168-20260528-100246.log`
- Validate log: `Logs/cycle-168-20260528-100246-validate.log`
- Capture log: `Logs/cycle-168-20260528-100246-capture.log`
- Build log: `Logs/cycle-168-20260528-100246-build.log`
- Smoke log: `Logs/cycle-168-20260528-100246-smoke.log`
- Validate result: exit 0.
- Capture result: exit 0.
- Build result: exit 0.
- Smoke result: built exe launched for 24 seconds with `-batchmode -nographics`; scanned `Exception|Shader error|error CS|NullReference|MissingReference|Assertion|Failed`; pattern count 0.

Earlier attempts:

- `cycle-168-20260528-100128`: failed because a provisional batch method name was invoked before the final entry point existed.
- `cycle-168-20260528-100608`: failed before final cleanup while the worktree still contained the provisional state.
- Both failed attempts used `-NoRollback`; the final run above is the authoritative Cycle168 validation result.

## Source Evidence

Exact source grep for `Assets/Scripts/FastVS/FastVsRealtimeLightShadowRig.cs`:

- `mainLight.intensity`: 0
- `mainLight.shadowStrength`: 0
- `mainLight.color`: 0
- `mainLight.transform.rotation`: 0
- `mainLight.cookie`: 0
- `mainLight.cookieSize`: 0
- `mainLight.cookieSize2D`: 0
- `EnsureCentralPlazaSunCookieTexture`: 0
- `EnsureExteriorSunCookieTexture`: 0
- `IsRuntimeDirectionalCookie`: 0
- `RenderSettings.ambientMode`: 0
- `RenderSettings.ambientLight`: 0
- `RenderSettings.fog`: 0

## Review Artifacts

Local diagnostic output:

- `docs/devlog/screenshots/fast_vs_hd2d_phase_a_realtime_rig_sun_handoff_cycle168/parent_review_01_current_house_interior_sun_cycle_morning.png`
- `docs/devlog/screenshots/fast_vs_hd2d_phase_a_realtime_rig_sun_handoff_cycle168/parent_review_02_current_house_exterior_sun_cycle_morning.png`
- `docs/devlog/screenshots/fast_vs_hd2d_phase_a_realtime_rig_sun_handoff_cycle168/parent_review_03_current_central_plaza_sun_cycle_noon.png`
- `docs/devlog/screenshots/fast_vs_hd2d_phase_a_realtime_rig_sun_handoff_cycle168/parent_review_04_current_library_sun_cycle_evening.png`
- `docs/devlog/screenshots/fast_vs_hd2d_phase_a_realtime_rig_sun_handoff_cycle168/realtime_rig_sun_handoff_diagnostics.md`
- `docs/devlog/screenshots/fast_vs_hd2d_phase_a_realtime_rig_sun_handoff_cycle168/sun_cycle_scene_wiring_diagnostics.md`

Public curated review set:

- `docs/review/2026-05-28T10-06/01_house_interior_sun_cycle_morning.png`
- `docs/review/2026-05-28T10-06/02_house_exterior_sun_cycle_morning.png`
- `docs/review/2026-05-28T10-06/03_central_plaza_sun_cycle_noon.png`
- `docs/review/2026-05-28T10-06/04_library_sun_cycle_evening.png`
- `docs/review/2026-05-28T10-06/index.md`
- `docs/review/2026-05-28T10-06/devlog.txt`

This diagnostic capture is not a Tom visual sign-off gate for Phase A. The Phase A gate remains the later 5-area build screenshots plus TimeWindow aperture check after Painted Overlay removal, renderer-policy change, and shader lightening.

## Build Artifact For Tom

Build exe path for review:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

起動時は `Builds\FastVS_HouseSlice\` をフォルダごと起動対象として扱う。

## Gate Note

変更を適用しました。参考画像とのギャップは、まだ Realtime Rig の SunCycle 競合解除段階のため、Painted Overlay が残っていること、0.35s renderer policy が残っていること、god rays / 太陽盤 / 暖寒対比 / エミッシブ表現が未実装である点です。Tom 判定は Phase A の 5 エリア capture 完了時にお願いします。
